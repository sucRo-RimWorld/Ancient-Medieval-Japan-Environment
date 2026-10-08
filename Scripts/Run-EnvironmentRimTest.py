"""Run AMJE Environment logic/calculation tests through RimTest Redux on an isolated desktop."""
from __future__ import annotations

import argparse
from datetime import datetime
import json
import os
from pathlib import Path
import shutil
import tempfile

import EnvironmentFrameworkTestCommon as common

ROOT = common.ROOT
TEST_ID = "sucro.amje.environment.rimtests"
ERROR_ID = "sucro.amje.rimtesterrors"
EXPECTED_TESTS = 10


def next_output() -> Path:
    return Path(tempfile.gettempdir()) / (
        "AMJE-RimTest-" + datetime.now().strftime("%Y%m%d-%H%M%S-%f")
    )


def run(root: Path, output: Path, game: Path):
    if not (game / "RimWorldWin64.exe").is_file():
        raise ValueError("RimWorld executable not found: " + str(game / "RimWorldWin64.exe"))
    if not (root / "About/About.xml").is_file() or not (root / "Assemblies/AncientMedievalJapanEnvironment.dll").is_file():
        raise ValueError("Environment root must be built before RimTest: " + str(root))
    if output.exists():
        raise ValueError("Choose a fresh --output directory")
    common.ensure_game_closed()

    mods_dir = game / "Mods"
    try:
        root.relative_to(mods_dir.resolve())
    except ValueError as exc:
        raise ValueError("Development Environment root must be under RimWorld/Mods so RimTest loads exact source") from exc

    workshop = game.parents[1] / "workshop/content/294100"
    rimtest_dll = common.find_dll(workshop / "3762405308", "RimTestRedux.dll")
    harmony_dll = common.find_dll(workshop / "2009463077", "0Harmony.dll")
    if not (workshop / "3296362231/About/About.xml").is_file():
        raise ValueError("RimTest Redux dependency ilyvion's Laboratory is not installed")

    before = common.snapshot_tree(root)
    user_config = common.user_config_dir()
    originals = {name: common.digest(user_config / name) for name in ("ModsConfig.xml", "Prefs.xml")}

    fixture = mods_dir / "AMJE.EnvironmentRimTestAudit"
    early = mods_dir / "AMJE.RimTestErrors"
    if fixture.exists() or early.exists():
        raise ValueError("Stale AMJE RimTest fixture requires inspection; refusing to overwrite")

    output.mkdir(parents=True)
    owned = []
    try:
        fixture.mkdir()
        owned.append(fixture)
        (fixture / "Assemblies").mkdir()
        common.write_about(
            fixture,
            "AMJE Environment RimTest audit (temporary)",
            TEST_ID,
            ["ilyvion.rimtestredux", "sucro.ancientmedievaljapan.environment"],
        )
        early.mkdir()
        owned.append(early)
        (early / "Assemblies").mkdir()
        common.write_about(early, "AMJE RimTest Unity error observer", ERROR_ID)

        csc, refs, desktop = common.compile_support(game, output, early)
        invoke_refs = refs + [
            harmony_dll,
            rimtest_dll,
            root / "Assemblies/AncientMedievalJapanEnvironment.dll",
        ]
        common.invoke([
            csc, "/nologo", "/target:library",
            "/out:" + str(fixture / "Assemblies/AMJE.Environment.RimTests.dll"),
            *["/reference:" + str(x) for x in invoke_refs],
            ROOT / "Tests/RimTest/EnvironmentRimTests.cs",
        ])

        save_data = output / "SaveData"
        common.write_isolated_config(user_config, save_data, [
            "brrainz.harmony",
            "ludeon.rimworld",
            "ilyvion.laboratory",
            "ilyvion.rimtestredux",
            ERROR_ID,
            "sucro.ancientmedievaljapan.environment",
            TEST_ID,
        ])

        result_path = output / "RimTestSummary.json"
        log_path = output / "RimTest-Player.log"
        errors = output / "Unity-RimTest"
        runner_log = output / "RimTest-Runner.log"
        batch = output / "RimTest.cmd"
        command = (
            '"' + str(game / "RimWorldWin64.exe") + '"'
            ' -savedatafolder="' + str(save_data) + '"'
            ' -logFile "' + str(log_path) + '"'
        )
        batch.write_text("@echo off\n" + command + "\nexit /b %ERRORLEVEL%\n", encoding="ascii")
        env = os.environ.copy()
        env["AMJE_RIMTEST_RESULT"] = str(result_path)
        env["RIMWORLD_AMJE_ERROR_DIRECTORY"] = str(errors)
        common.run_hidden(desktop, batch, env, runner_log)

        if not result_path.is_file():
            raise ValueError("RimTest result bridge did not create RimTestSummary.json")
        result = json.loads(result_path.read_text(encoding="utf-8-sig"))
        if (result.get("passed") is not True or result.get("total") != EXPECTED_TESTS or
                result.get("pass") != EXPECTED_TESTS or result.get("failed") != 0 or
                result.get("skipped") != 0 or result.get("unknown") != 0):
            raise ValueError("RimTest suite incomplete or failing: " + json.dumps(result, ensure_ascii=False))
        common.validate_runtime_logs(log_path, errors)
        print(f"RIMTEST PASS {EXPECTED_TESTS}/{EXPECTED_TESTS}", flush=True)
    finally:
        retired = output / "Retired"
        retired.mkdir(exist_ok=True)
        for source in owned:
            if source.exists():
                shutil.move(str(source), retired / source.name)
        if common.snapshot_tree(root) != before:
            raise ValueError("Environment source root changed during RimTest run")
        if any(common.digest(user_config / name) != original for name, original in originals.items()):
            raise ValueError("Normal RimWorld config changed during RimTest run")


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
