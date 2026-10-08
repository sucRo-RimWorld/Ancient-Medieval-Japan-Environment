"""Pickle E2E smoke for the ACTUAL downloaded AMJE Workshop root.

Uses RimWorks Pickle as the test engine, not homemade scenario assertions.
Only tests loaded Defs/source/wood setup; does NOT replace the rendered
Quickstarts world-generation and native cutting-job release contract.
"""
import argparse
import importlib.util
import json
import os
from pathlib import Path
import shutil
import subprocess
import sys
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location("payload", ROOT / "Scripts/Build-WorkshopPayload.py")
payload_tool = importlib.util.module_from_spec(spec)
spec.loader.exec_module(payload_tool)

PROFILES = ("Vanilla", "MO", "CCTO", "MO-CCTO")
TEST_ID = "sucro.amje.workshoppickle"
ERROR_ID = "sucro.amje.pickleearlyerrors"
FEATURE = "amje-workshop.feature"
SCENARIOS = (
    "Downloaded production Mod and assembly are the actual selected Steam root",
    "Four authored plant Defs have unique Workshop ownership",
    "Japan-oriented biome plant exclusion is loaded",
    "Replacement native plants retain their tuned commonality",
    "Native wood harvest Defs match Vanilla or Medieval Overhaul",
)


def find_dll(root, name):
    preferred = root / "1.6" / "Assemblies" / name
    if preferred.is_file():
        return preferred
    matches = sorted(root.rglob(name))
    if len(matches) != 1:
        raise ValueError("Expected one " + name + " in " + str(root) +
                         "; found " + str(len(matches)))
    return matches[0]


def invoke(args, **kwargs):
    subprocess.run([str(x) for x in args], check=True, **kwargs)


def run(payload, manifest, output, game):
    expected = (game.parents[1] / "workshop/content/294100/3814638060").resolve()
    if payload != expected:
        raise ValueError("Use --payload with the actual installed Steam Workshop root")
    if output.exists():
        raise ValueError("Choose a fresh --output directory")
    if "RimWorldWin64.exe" in subprocess.check_output(
            ["tasklist", "/FI", "IMAGENAME eq RimWorldWin64.exe"], text=True):
        raise ValueError("Close RimWorld normally before Pickle automation")
    payload_tool.verify(payload, manifest)
    inventory = payload_tool.inventory(payload)

    mods_dir = game / "Mods"
    fixture = mods_dir / "AMJE.WorkshopPickleAudit"
    early = mods_dir / "AMJE.PickleEarlyErrors"
    if fixture.exists() or early.exists():
        raise ValueError("Stale Pickle audit fixture requires inspection; do not overwrite")
    user_config = (Path(os.environ["USERPROFILE"]) /
                   "AppData/LocalLow/Ludeon Studios/RimWorld by Ludeon Studios/Config")
    originals = {name: payload_tool.digest((user_config / name).read_bytes())
                 for name in ("ModsConfig.xml", "Prefs.xml")}

    output.mkdir(parents=True)
    compiled = output / "Compiled"
    compiled.mkdir()
    owned = []
    completed = False
    try:
        fixture.mkdir()
        owned.append(fixture)
        (fixture / "About").mkdir()
        (fixture / "Pickle/Assemblies").mkdir(parents=True)
        (fixture / "Pickle/Features").mkdir()
        (fixture / "About/About.xml").write_text(
            '<ModMetaData><name>AMJE Workshop Pickle audit (temporary)</name>'
            '<packageId>' + TEST_ID + '</packageId>'
            '<supportedVersions><li>1.6</li></supportedVersions>'
            '<loadAfter><li>rimworks.pickle</li>'
            '<li>sucro.ancientmedievaljapan.environment</li></loadAfter>'
            '</ModMetaData>', encoding="utf-8")
        shutil.copy2(ROOT / "Tests/Pickle/Features" / FEATURE,
                     fixture / "Pickle/Features" / FEATURE)
        early.mkdir()
        owned.append(early)
        (early / "About").mkdir()
        (early / "Assemblies").mkdir()
        (early / "About/About.xml").write_text(
            '<ModMetaData><name>AMJE Pickle Unity error observer</name>'
            '<packageId>' + ERROR_ID + '</packageId>'
            '<supportedVersions><li>1.6</li></supportedVersions>'
            '</ModMetaData>', encoding="utf-8")

        csc = (Path(os.environ["WINDIR"]) /
               "Microsoft.NET/Framework64/v4.0.30319/csc.exe")
        managed = game / "RimWorldWin64_Data/Managed"
        workshop = game.parents[1] / "workshop/content/294100"
        pickle_dll = find_dll(workshop / "3791648678", "RimWorks.Pickle.dll")
        refs = [managed / name for name in (
            "Assembly-CSharp.dll", "UnityEngine.CoreModule.dll", "netstandard.dll")]
        invoke([csc, "/nologo", "/target:library",
                "/out:" + str(fixture / "Pickle/Assemblies/AMJE.WorkshopPickle.Steps.dll"),
                *["/reference:" + str(x) for x in [*refs, pickle_dll]],
                ROOT / "Tests/Pickle/EnvironmentWorkshopSteps.cs"])
        invoke([csc, "/nologo", "/target:library",
                "/out:" + str(early / "Assemblies/AMJE.PickleErrors.dll"),
                *["/reference:" + str(x) for x in refs],
                ROOT / "Tests/Release/HarvestErrorObserver.cs"])
        desktop = compiled / "IsolatedDesktopRunner.exe"
        invoke([csc, "/nologo", "/target:exe", "/out:" + str(desktop),
                ROOT / "Tests/Release/IsolatedDesktopRunner.cs"])

        summary = {"passed": False, "source": "actual Steam installed root",
                   "root": str(payload), "profiles": {}}
        for profile in PROFILES:
            config_dir = output / ("SaveData-" + profile) / "Config"
            config_dir.mkdir(parents=True)
            config = ET.parse(user_config / "ModsConfig.xml")
            active = config.find("activeMods")
            active.clear()
            ids = ["brrainz.harmony", "ludeon.rimworld", ERROR_ID,
                   "rimworks.rimlogging", "rimworks.pickle"]
            if profile.startswith("MO"):
                ids += ["oskarpotocki.vanillafactionsexpanded.core",
                        "syrchalis.processor.framework", "dankpyon.medieval.overhaul"]
            if "CCTO" in profile:
                ids.append("sucro.cropcoldtoleranceoverhaul_steam")
            ids += ["sucro.ancientmedievaljapan.environment_steam", TEST_ID]
            for package in ids:
                ET.SubElement(active, "li").text = package
            config.write(config_dir / "ModsConfig.xml", encoding="utf-8",
                         xml_declaration=True)
            prefs = ET.parse(user_config / "Prefs.xml")
            for name, value in (("devMode", "True"),
                                ("resetModsConfigOnCrash", "False")):
                node = prefs.find(name)
                if node is None:
                    node = ET.SubElement(prefs.getroot(), name)
                node.text = value
            prefs.write(config_dir / "Prefs.xml", encoding="utf-8",
                        xml_declaration=True)

            report_dir = output / ("Pickle-" + profile)
            report_dir.mkdir()
            log_path = output / (profile + "-Player.log")
            errors = output / ("Unity-" + profile)
            runner_log = output / (profile + "-Runner.log")
            batch = output / (profile + ".cmd")
            cmd = ('"' + str(game / "RimWorldWin64.exe") + '"'
                   ' -savedatafolder="' + str(config_dir.parent) + '"'
                   ' -logFile "' + str(log_path) + '"'
                   ' -pickle-run="' + FEATURE + '" -pickle-mode=fast'
                   ' -pickle-report-dir="' + str(report_dir) + '"'
                   ' -pickle-no-browser -pickle-run-timeout=4')
            batch.write_text("@echo off\n" + cmd + "\nexit /b %ERRORLEVEL%\n",
                             encoding="ascii")
            env = os.environ.copy()
            env["AMJE_EXPECTED_PAYLOAD_ROOT"] = str(payload)
            env["AMJE_EXPECTED_PROFILE"] = profile
            env["RIMWORLD_AMJE_ERROR_DIRECTORY"] = str(errors)
            with runner_log.open("w", encoding="utf-8") as log:
                invoke([desktop, batch, ROOT], env=env, stdout=log,
                       stderr=subprocess.STDOUT)
            report = report_dir / "summary.json"
            result = json.loads(report.read_text(encoding="utf-8-sig"))
            names = [s["name"] for s in result["scenarios"]]
            if (result["total"] != len(SCENARIOS) or
                    result["passed"] != len(SCENARIOS) or
                    result["failed"] != 0 or result["skipped"] != 0 or
                    set(names) != set(SCENARIOS) or len(names) != len(SCENARIOS)):
                raise ValueError("Pickle scenarios incomplete or failing: " + profile)
            game_log = log_path.read_text(encoding="utf-8-sig")
            if ("Version:  Direct3D 11.0" not in game_log or
                    "[ERROR]" in game_log or "Level: ERROR" in game_log):
                raise ValueError("Pickle game log shows ERROR or missing Direct3D: " + profile)
            captures = list(errors.glob("Unity-*.log"))
            if (len(captures) != 1 or any(
                    "[CAPTURE_READY]" not in x.read_text(encoding="utf-8-sig") or
                    "[ERROR]" in x.read_text(encoding="utf-8-sig")
                    for x in captures)):
                raise ValueError("Independent Unity log incomplete/error: " + profile)
            summary["profiles"][profile] = {
                "passed": True, "scenarios": len(SCENARIOS), "errors": 0}
            (output / "SteamPickleSummary.json").write_text(
                json.dumps(summary, indent=2) + "\n", encoding="utf-8")
            print("PICKLE PASS", profile, len(SCENARIOS), flush=True)
        completed = True
    finally:
        retired = output / "Retired"
        retired.mkdir(exist_ok=True)
        for source in owned:
            if source.parent != mods_dir or source.name not in (
                    "AMJE.WorkshopPickleAudit", "AMJE.PickleEarlyErrors"):
                raise ValueError("Refusing to retire unrelated Mod directory")
            shutil.move(str(source), retired / source.name)
        if payload_tool.inventory(payload) != inventory:
            raise ValueError("Downloaded Workshop root changed during Pickle run")
        if any(payload_tool.digest((user_config / name).read_bytes()) != original[name]
               for name in originals):
            raise ValueError("Normal RimWorld config changed during Pickle run")
        (output / "Preservation.json").write_text(
            json.dumps({"steam_payload_unchanged": True,
                        "normal_config_unchanged": True}), encoding="utf-8")
    if completed:
        summary["passed"] = True
        summary["steam_release_cleared"] = False
        summary["coverage"] = "Pickle source/loaded Def/wood checks only; world maps and native cutting jobs are separate"
        (output / "SteamPickleSummary.json").write_text(
            json.dumps(summary, indent=2) + "\n", encoding="utf-8")
        print("PICKLE PASS downloaded root loaded-Def smoke:", output)


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--payload", type=Path, required=True)
    parser.add_argument("--manifest", type=Path, required=True)
    parser.add_argument("--output", type=Path, required=True)
    parser.add_argument("--game", type=Path, default=Path(
        "D:/SteamLibrary/steamapps/common/RimWorld"))
    args = parser.parse_args()
    try:
        run(args.payload.resolve(), args.manifest.resolve(),
            args.output.resolve(), args.game.resolve())
    except Exception as error:
        print("PICKLE BLOCKED/FAIL: " + str(error), file=sys.stderr)
        sys.exit(1)
