"""Static contract for the Environment-specific Pickle/Steam bridge.

Real compilation/Unity execution occurs only on a Windows Steam session.
"""
import importlib.util
from pathlib import Path
import unittest

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
        self.assertNotIn("shutil.copytree(payload", text)
        self.assertNotIn("os.remove(payload", text)


if __name__ == "__main__":
    unittest.main()
