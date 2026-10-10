"""Run loaded-Def/integration regression against the development AMJE Environment root with Pickle."""
from __future__ import annotations

import argparse
from datetime import datetime
import json
import os
from pathlib import Path
import shutil
import tempfile
import xml.etree.ElementTree as ET

import EnvironmentFrameworkTestCommon as common

ROOT = common.ROOT
PROFILES = ("Vanilla", "MO", "CCTO", "MO-CCTO")
TEST_ID = "sucro.amje.developmentpickle"
ERROR_ID = "sucro.amje.developmentpickleerrors"
FEATURE = "amje-development.feature"
SCENARIOS = (
    "Development production Mod and assembly are the expected source root",
    "Four authored plant Defs have unique development ownership",
    "Japan-oriented biome plant exclusion is loaded from development source",
    "Replacement native plants retain tuned development commonality",
    "Native wood harvest Defs match the active Vanilla or Medieval Overhaul profile",
)


def next_output() -> Path:
    return Path(tempfile.gettempdir()) / (
        "AMJE-Development-Pickle-" + datetime.now().strftime("%Y%m%d-%H%M%S-%f")
    )


def local_package_present(game: Path, package_id: str) -> bool:
    for about in (game / "Mods").glob("*/About/About.xml"):
        try:
            node = ET.parse(about).getroot().find("packageId")
        except ET.ParseError:
            continue
        if node is not None and (node.text or "").strip().lower() == package_id.lower():
            return True
    return False


def run(root: Path, output: Path, game: Path):
    if not (game / "RimWorldWin64.exe").is_file():
        raise ValueError("RimWorld executable not found: " + str(game / "RimWorldWin64.exe"))
    if not (root / "About/About.xml").is_file() or not (root / "Assemblies/AncientMedievalJapanEnvironment.dll").is_file():
        raise ValueError("Environment root must be built before Pickle: " + str(root))
    if output.exists():
        raise ValueError("Choose a fresh --output directory")
    common.ensure_game_closed()
    mods_dir = game / "Mods"
    try:
        root.relative_to(mods_dir.resolve())
    except ValueError as exc:
        raise ValueError("Development Environment root must be under RimWorld/Mods so Pickle loads exact source") from exc

    before = common.snapshot_tree(root)
    user_config = common.user_config_dir()
    originals = {name: common.digest(user_config / name) for name in ("ModsConfig.xml", "Prefs.xml")}
    workshop = game.parents[1] / "workshop/content/294100"
    pickle_dll = common.find_dll(workshop / "3791648678", "RimWorks.Pickle.dll")
    if not (workshop / "3733484696/About/About.xml").is_file():
        raise ValueError("Pickle dependency RimLogging is not installed")

    fixture = mods_dir / "AMJE.DevelopmentPickleAudit"
    early = mods_dir / "AMJE.DevelopmentPickleErrors"
    if fixture.exists() or early.exists():
        raise ValueError("Stale AMJE development Pickle fixture requires inspection; refusing to overwrite")

    output.mkdir(parents=True)
    owned = []
    summary = {"passed": False, "source": "development root", "root": str(root), "profiles": {}}
    try:
        fixture.mkdir()
        owned.append(fixture)
        (fixture / "Pickle/Assemblies").mkdir(parents=True)
        (fixture / "Pickle/Features").mkdir(parents=True)
        common.write_about(
            fixture,
            "AMJE development Pickle audit (temporary)",
            TEST_ID,
            ["rimworks.pickle", "sucro.ancientmedievaljapan.environment"],
        )
        shutil.copy2(ROOT / "Tests/Pickle/Features" / FEATURE, fixture / "Pickle/Features" / FEATURE)

        # RimWorld does not count Pickle/Features or Pickle/Assemblies as
        # ordinary Mod content. A test-only Strings asset prevents the engine
        # from reporting this valid Pickle-only fixture as an empty Mod.
        # Strings assets do not register any gameplay Defs or alter balance.
        (fixture / "Strings").mkdir()
        (fixture / "Strings/AMJEDevelopmentPickleAudit.txt").write_text(
            "AMJE development Pickle fixture load marker (test-only).\n",
            encoding="utf-8",
        )

        early.mkdir()
        owned.append(early)
        (early / "Assemblies").mkdir()
        common.write_about(early, "AMJE development Pickle Unity error observer", ERROR_ID)

        csc, refs, desktop = common.compile_support(game, output, early)
        common.invoke([
            csc, "/nologo", "/target:library",
            "/out:" + str(fixture / "Pickle/Assemblies/AMJE.DevelopmentPickle.Steps.dll"),
            *["/reference:" + str(x) for x in [*refs, pickle_dll]],
            ROOT / "Tests/Pickle/EnvironmentWorkshopSteps.cs",
        ])

        ccto_id = ("sucro.cropcoldtoleranceoverhaul" if
                   local_package_present(game, "sucro.cropcoldtoleranceoverhaul") else
                   "sucro.cropcoldtoleranceoverhaul_steam")
        for profile in PROFILES:
            save_data = output / ("SaveData-" + profile)
            ids = [
                "brrainz.harmony", "ludeon.rimworld", ERROR_ID,
                "rimworks.rimlogging", "rimworks.pickle",
            ]
            if profile.startswith("MO"):
                ids += [
                    "oskarpotocki.vanillafactionsexpanded.core",
                    "syrchalis.processor.framework",
                    "dankpyon.medieval.overhaul",
                ]
            if "CCTO" in profile:
                ids.append(ccto_id)
            ids += ["sucro.ancientmedievaljapan.environment", TEST_ID]
            common.write_isolated_config(user_config, save_data, ids)

            report_dir = output / ("Pickle-" + profile)
            report_dir.mkdir()
            log_path = output / (profile + "-Player.log")
            errors = output / ("Unity-" + profile)
            runner_log = output / (profile + "-Runner.log")
            batch = output / (profile + ".cmd")
            command = (
                '"' + str(game / "RimWorldWin64.exe") + '"'
                ' -savedatafolder="' + str(save_data) + '"'
                ' -logFile "' + str(log_path) + '"'
                ' -pickle-run="' + FEATURE + '" -pickle-mode=fast'
                ' -pickle-report-dir="' + str(report_dir) + '"'
                ' -pickle-no-browser -pickle-run-timeout=4'
            )
            batch.write_text("@echo off\n" + command + "\nexit /b %ERRORLEVEL%\n", encoding="ascii")
            env = os.environ.copy()
            env["AMJE_EXPECTED_PAYLOAD_ROOT"] = str(root)
            env["AMJE_EXPECTED_PROFILE"] = profile
            env["RIMWORLD_AMJE_ERROR_DIRECTORY"] = str(errors)
            common.run_hidden(desktop, batch, env, runner_log)

            report = report_dir / "summary.json"
            if not report.is_file():
                raise ValueError("Pickle summary missing: " + profile)
            result = json.loads(report.read_text(encoding="utf-8-sig"))
            names = [scenario["name"] for scenario in result.get("scenarios", [])]
            if (result.get("exitReason") != "passed" or
                result.get("total") != len(SCENARIOS) or result.get("passed") != len(SCENARIOS) or
                    result.get("failed") != 0 or result.get("skipped") != 0 or
                    set(names) != set(SCENARIOS) or len(names) != len(SCENARIOS)):
                raise ValueError("Pickle scenarios incomplete or failing: " + profile)
            common.validate_runtime_logs(log_path, errors)
            summary["profiles"][profile] = {"passed": True, "scenarios": len(SCENARIOS), "errors": 0}
            (output / "DevelopmentPickleSummary.json").write_text(
                json.dumps(summary, indent=2) + "\n", encoding="utf-8"
            )
            print("PICKLE PASS", profile, len(SCENARIOS), flush=True)

        summary["passed"] = True
        summary["coverage"] = "development loaded-source/Def/wood integration; rendered map and native-job Quickstarts remain separate"
        (output / "DevelopmentPickleSummary.json").write_text(
            json.dumps(summary, indent=2) + "\n", encoding="utf-8"
        )
    finally:
        retired = output / "Retired"
        retired.mkdir(exist_ok=True)
        for source in owned:
            if source.exists():
                shutil.move(str(source), retired / source.name)
        if common.snapshot_tree(root) != before:
            raise ValueError("Environment source root changed during Pickle run")
        if any(common.digest(user_config / name) != original for name, original in originals.items()):
            raise ValueError("Normal RimWorld config changed during Pickle run")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--root", type=Path, default=ROOT)
    parser.add_argument("--output", type=Path)
    parser.add_argument("--game", type=Path, default=Path("D:/SteamLibrary/steamapps/common/RimWorld"))
    args = parser.parse_args()
    try:
        run(args.root.resolve(), (args.output.resolve() if args.output else next_output()), args.game.resolve())
    except Exception as error:
        print("[FAIL] " + type(error).__name__ + ": " + str(error), file=__import__("sys").stderr)
        raise SystemExit(1)
