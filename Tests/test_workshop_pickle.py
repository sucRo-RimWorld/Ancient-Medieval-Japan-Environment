"""Static contract for the Environment-specific Pickle/Steam bridge.

Real compilation/Unity execution occurs only on a Windows Steam session.
"""
import importlib.util
from pathlib import Path
import unittest
from unittest import mock
import os
import tempfile

ROOT = Path(__file__).resolve().parents[1]


class WorkshopPickleTests(unittest.TestCase):
    def test_feature_step_binding_and_diagnostic_contract(self):
        steps = (ROOT / "Tests/Pickle/EnvironmentWorkshopSteps.cs").read_text()
        features = (ROOT / "Tests/Pickle/Features/amje-workshop.feature").read_text()
        for sentence in (
            "the installed Environment Mod and assembly come from the exact Steam folder",
            "all four Environment plants are uniquely owned by the downloaded Mod",
            "rejected Vanilla vegetation is absent from the selected Japanese biomes",
            "the replacement structural plants preserve their balanced commonalities",
            "the harvested wood Def contracts remain correct with or without Medieval Overhaul",
        ):
            self.assertIn('Then ' + sentence, features)
            self.assertIn('[Then("' + sentence + '")]', steps)
        self.assertEqual(features.count("  Scenario:"), 5)
        self.assertIn("AMJE_EXPECTED_PAYLOAD_ROOT", steps)
        self.assertIn("AMJE_EXPECTED_PROFILE", steps)
        self.assertIn("AMJE_EXPECTED_PROFILE", (ROOT / "Scripts/Run-WorkshopPickle.py").read_text())
        self.assertIn("DankPyon_RawWood", steps)
        self.assertIn("WoodLog", steps)
        self.assertIn('Plant_TreePoplar', steps)
        self.assertIn('Plant_TreePine', steps)
        self.assertIn('TemperateSwamp', steps)
        self.assertIn('ColdBog', steps)

    def test_one_command_batch_entry(self):
        launcher = ROOT / "test-run.bat"
        contents = launcher.read_text(encoding="utf-8")
        for expected in ("Scripts\\Run-WorkshopPickle.py", "where py",
                         "--game", 'if not "%RESULT%"=="0"'):
            self.assertIn(expected, contents)
        for forbidden in ("--manifest", "--output", "rmdir /S", "taskkill"):
            self.assertNotIn(forbidden, contents)

    def test_manifest_discovery_fails_closed_and_selects_verified_bytes(self):
        path = ROOT / "Scripts/Run-WorkshopPickle.py"
        spec = importlib.util.spec_from_file_location("pickle_runner", path)
        runner = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(runner)
        with tempfile.TemporaryDirectory() as temp:
            root = Path(temp)
            earlier = root / "AMJE-Final-20261008-210000"
            latest = root / "AMJE-Final-20261008-220000"
            earlier.mkdir()
            latest.mkdir()
            for directory in (earlier, latest):
                (directory / "Manifest.json").write_text("{}", encoding="utf-8")
            old_manifest = earlier / "Manifest.json"
            newest = latest / "Manifest.json"
            os.utime(old_manifest, (1000000000, 1000000000))
            os.utime(newest, (2000000000, 2000000000))
            def verify(p, manifest):
                if manifest != old_manifest:
                    raise ValueError("Wrong candidate")
                return {"files": {}}
            with mock.patch.object(runner.payload_tool, "verify",
                                   side_effect=verify) as checker:
                result = runner.find_verified_manifest(root / "Downloaded", root)
            self.assertEqual(result, old_manifest)
            self.assertEqual(checker.call_count, 2)
            with mock.patch.object(runner.payload_tool, "verify",
                                   side_effect=ValueError("Wrong bytes")):
                with self.assertRaisesRegex(ValueError, "No verified Manifest"):
                    runner.find_verified_manifest(root / "Downloaded", root)

    def test_preflight_failure_does_not_assume_result_folder(self):
        spec = importlib.util.spec_from_file_location(
            "pickle_runner", ROOT / "Scripts/Run-WorkshopPickle.py")
        runner = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(runner)
        with tempfile.TemporaryDirectory() as temp:
            with mock.patch("sys.stderr") as stderr:
                runner.show_failure(ValueError("RimWorld is open"),
                                    Path(temp) / "missing")
                self.assertTrue(stderr.write.called)

    def test_pickle_runner_preserves_download_and_release_gate(self):
        path = ROOT / "Scripts/Run-WorkshopPickle.py"
        spec = importlib.util.spec_from_file_location("pickle_runner", path)
        module = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(module)
        self.assertEqual(len(module.SCENARIOS), 5)
        self.assertEqual(len(module.PROFILES), 4)
        self.assertEqual(module.FEATURE, "amje-workshop.feature")
        text = path.read_text()
        for required in (
            "payload_tool.verify(payload, manifest)",
            "payload_tool.inventory(payload) != inventory",
            "normal_config_unchanged",
            "steam_release_cleared",
            "IsolatedDesktopRunner.cs",
            "RimWorks.Pickle.dll",
            "HarvestErrorObserver.cs",
            "-pickle-run=",
            "-pickle-mode=fast",
            "-pickle-no-browser",
            "summary.json",
        ):
            self.assertIn(required, text)
        self.assertIn('"steam_release_cleared"] = False', text)
        self.assertIn("find_verified_manifest(payload)", text)
        self.assertIn("next_result_directory()", text)
        self.assertIn("show_failure(error, output)", text)
        self.assertNotIn("shutil.copytree(payload", text)
        self.assertNotIn("os.remove(payload", text)


if __name__ == "__main__":
    unittest.main()
