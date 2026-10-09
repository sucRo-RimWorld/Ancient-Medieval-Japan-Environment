"""Pickle E2E smoke for the ACTUAL downloaded AMJE Workshop root.

Uses RimWorks Pickle as the test engine, not homemade scenario assertions.
Only tests loaded Defs/source/wood setup; does NOT replace the rendered
Quickstarts world-generation and native cutting-job release contract.
"""
import argparse
import importlib.util
import json
import os
from datetime import datetime
import tempfile
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


def default_steam_root(game):
    return (game.parents[1] / "workshop/content/294100/3814638060").resolve()


# Steam can serialize its own Workshop ID and load-folder XML differently.
# Only these two formatting-only exceptions are allowed for Pickle diagnostics;
# the original candidate SHA256 inventory remains unmodified and authoritative.
METADATA_PATHS = frozenset(("About/PublishedFileId.txt", "loadFolders.xml"))


def _xml_meaning(node):
    """Compare exact effective XML elements, ignoring insignificant whitespace."""
    return (node.tag, tuple(sorted(node.attrib.items())),
            (node.text or "").strip(),
            tuple(_xml_meaning(child) for child in node),
            (node.tail or "").strip())


def verify_downloaded_for_pickle(payload, manifest_path):
    """Check all byte hashes; permit only proven semantic Steam metadata drift.

    This is NOT an exact publication-manifest PASS when the two files differ.
    Never use this exception in Build-WorkshopPayload.py.verify, Quickstarts'
    full release gate, or the final Steam distribution provenance claim.
    """
    manifest = json.loads(manifest_path.read_text(encoding="utf-8"))
    expected = manifest.get("files")
    if (not isinstance(expected, dict) or
            manifest.get("packageId") != payload_tool.PACKAGE or
            manifest.get("workshopId") != payload_tool.WORKSHOP):
        raise ValueError("Invalid publication Manifest identity or file table")
    actual = payload_tool.inventory(payload)
    missing = sorted(expected.keys() - actual.keys())
    extra = sorted(actual.keys() - expected.keys())
    changed = sorted(p for p in expected.keys() & actual.keys()
                     if expected[p] != actual[p])
    if missing or extra or set(changed) - METADATA_PATHS:
        raise ValueError("Workshop content differs from approved candidate: "
                         "missing=" + repr(missing) + ", extra=" + repr(extra) +
                         ", changed=" + repr(changed) +
                         ". Game/test did not start; Steam files untouched.")
    if not changed:
        payload_tool.verify(payload, manifest_path)
        return {"byte_identical": True, "gameplay_files_byte_identical": True,
                "metadata_format_variants": [], "source_commit": manifest["source_commit"]}

    # Require the *candidate* itself to have the published, pinned canonical
    # ID/XML; otherwise this diagnostic cannot authorize even a smoke run.
    canonical_id = (payload_tool.WORKSHOP + "\n").encode("utf-8")
    if (expected.get("About/PublishedFileId.txt") != payload_tool.digest(canonical_id)
            or expected.get("loadFolders.xml") != payload_tool.digest(payload_tool.LOAD)):
        raise ValueError("Candidate manifest metadata is not canonical; abort")

    downloaded = {key: (payload / key).read_bytes() for key in actual}
    try:
        workshop_id = downloaded["About/PublishedFileId.txt"].decode("utf-8-sig").strip()
    except UnicodeError as exc:
        raise ValueError("Steam PublishedFileId encoding is not UTF-8") from exc
    if workshop_id != payload_tool.WORKSHOP:
        raise ValueError("Steam PublishedFileId is " + repr(workshop_id) +
                         ", expected " + payload_tool.WORKSHOP)

    try:
        actual_load = ET.fromstring(downloaded["loadFolders.xml"])
        canonical_load = ET.fromstring(payload_tool.LOAD)
    except ET.ParseError as exc:
        raise ValueError("Steam loadFolders.xml is invalid XML: " + str(exc)) from exc
    if _xml_meaning(actual_load) != _xml_meaning(canonical_load):
        raise ValueError("Steam loadFolders.xml changes active Mod load paths: " +
                         repr(_xml_meaning(actual_load)) +
                         "; expected " + repr(_xml_meaning(canonical_load)))

    # Apply the unmodified production validator to the actual downloaded
    # gameplay bytes, with just the two *verified-equivalent* metadata values
    # normalized in memory; nothing on Steam or in the manifest is rewritten.
    normalized = dict(downloaded)
    normalized["About/PublishedFileId.txt"] = canonical_id
    normalized["loadFolders.xml"] = payload_tool.LOAD
    payload_tool.validate(normalized)
    print("[AMJE] Steam payload: all other " + str(len(actual) - len(changed)) +
          " files match manifest bytes; XML/load identity semantics checked.", flush=True)
    print("[AMJE] Formatting-only metadata variance: " +
          ", ".join(changed), flush=True)
    print("[AMJE] NOT a 32/32 byte-identical distribution result. "
          "Release provenance remains HOLD.", flush=True)
    return {"byte_identical": False, "gameplay_files_byte_identical": True,
            "metadata_format_variants": changed, "source_commit": manifest["source_commit"]}


def find_verified_manifest(payload, search_root=None):
    """Select only a candidate manifest that verifies the real Steam bytes."""
    parent = Path(search_root) if search_root is not None else Path(tempfile.gettempdir())
    manifests = sorted(parent.glob("AMJE-Final-*/Manifest.json"),
                       key=lambda p: (p.stat().st_mtime, str(p)), reverse=True)
    rejected = []
    for candidate in manifests:
        try:
            verify_downloaded_for_pickle(payload, candidate)
        except (OSError, ValueError, KeyError, TypeError) as error:
            rejected.append(candidate.parent.name + ": " + str(error))
            continue
        return candidate
    if not manifests:
        raise ValueError("No AMJE-Final-*/Manifest.json found under " + str(parent)
                         + "; Steam files have not been modified.")
    raise ValueError("No verified Manifest.json matches the downloaded Workshop bytes; "
                     "Steam files have not been modified. Checked: " +
                     "; ".join(rejected[:5]))


def next_result_directory():
    return (Path(tempfile.gettempdir()) /
            ("AMJE-Steam-Pickle-" + datetime.now().strftime("%Y%m%d-%H%M%S-%f")))


def show_failure(error, output):
    print("[FAIL] " + type(error).__name__ + ": " + str(error), file=sys.stderr)
    if output is None or not output.is_dir():
        print("[AMJE] Preflight stopped before starting RimWorld; no runtime logs "
              "exist for this attempt.", file=sys.stderr)
        return
    print("[AMJE] Partial automated test reports: " + str(output), file=sys.stderr)
    runners = sorted(output.glob("*-Runner.log"),
                     key=lambda item: item.stat().st_mtime, reverse=True)
    if runners:
        latest = runners[0]
        print("[AMJE] Last runner " + latest.name + ":", file=sys.stderr)
        for line in latest.read_text(encoding="utf-8-sig", errors="replace").splitlines()[-30:]:
            print(line, file=sys.stderr)
    for path in sorted(output.glob("Pickle-*/summary.json")):
        try:
            result = json.loads(path.read_text(encoding="utf-8-sig"))
            print("[AMJE] " + path.parent.name + " total=" +
                  str(result.get("total")) + " passed=" + str(result.get("passed")) +
                  " failed=" + str(result.get("failed")) +
                  " skipped=" + str(result.get("skipped")), file=sys.stderr)
        except (OSError, ValueError):
            print("[AMJE] Incomplete Pickle summary: " + str(path), file=sys.stderr)


def run(payload, manifest, output, game):
    expected = (game.parents[1] / "workshop/content/294100/3814638060").resolve()
    if payload != expected:
        raise ValueError("Use --payload with the actual installed Steam Workshop root")
    if output.exists():
        raise ValueError("Choose a fresh --output directory")
    if "RimWorldWin64.exe" in subprocess.check_output(
            ["tasklist", "/FI", "IMAGENAME eq RimWorldWin64.exe"], text=True):
        raise ValueError("Close RimWorld normally before Pickle automation")
    audit = verify_downloaded_for_pickle(payload, manifest)
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
                   "root": str(payload), "profiles": {},
                   "manifest_audit": audit, "steam_release_cleared": False}
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
            if (result.get("exitReason") != "passed" or
                    result["total"] != len(SCENARIOS) or
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
    parser.add_argument("--payload", type=Path, help="Override detected Steam root")
    parser.add_argument("--manifest", type=Path, help="Override matching Manifest.json")
    parser.add_argument("--output", type=Path, help="Override automatic fresh result path")
    parser.add_argument("--game", type=Path, default=Path(
        "D:/SteamLibrary/steamapps/common/RimWorld"))
    args = parser.parse_args()
    game = args.game.resolve()
    payload = args.payload.resolve() if args.payload else default_steam_root(game)
    output = args.output.resolve() if args.output else next_result_directory()
    try:
        manifest = (args.manifest.resolve() if args.manifest
                    else find_verified_manifest(payload))
        print("[AMJE] Steam root: " + str(payload), flush=True)
        print("[AMJE] Matched manifest: " + str(manifest), flush=True)
        print("[AMJE] Automatic Pickle results: " + str(output), flush=True)
        run(payload, manifest, output, game)
    except Exception as error:
        show_failure(error, output)
        sys.exit(1)
