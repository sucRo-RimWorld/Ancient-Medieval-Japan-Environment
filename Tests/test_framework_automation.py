from pathlib import Path
import unittest

ROOT = Path(__file__).resolve().parents[1]


class FrameworkAutomationContractTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.entry = (ROOT / "run-framework-tests.bat").read_text(encoding="utf-8")
        cls.rimtest = (ROOT / "Scripts/Run-EnvironmentRimTest.py").read_text(encoding="utf-8")
        cls.common = (ROOT / "Scripts/EnvironmentFrameworkTestCommon.py").read_text(encoding="utf-8")
        cls.pickle = (ROOT / "Scripts/Run-DevelopmentPickle.py").read_text(encoding="utf-8")
        cls.rimtest_source = (ROOT / "Tests/RimTest/EnvironmentRimTests.cs").read_text(encoding="utf-8")
        cls.feature = (ROOT / "Tests/Pickle/Features/amje-development.feature").read_text(encoding="utf-8")
        cls.steps = (ROOT / "Tests/Pickle/EnvironmentWorkshopSteps.cs").read_text(encoding="utf-8")

    def test_entrypoint_runs_rimtest_then_pickle(self):
        self.assertIn("run-static-tests.bat", self.entry)
        self.assertLess(self.entry.index("Run-EnvironmentRimTest.py"), self.entry.index("Run-DevelopmentPickle.py"))
        self.assertIn("--skip-static", self.entry)

    def test_framework_uses_script_source_root_without_trailing_backslash_argument(self):
        # %~dp0 has a trailing backslash on Windows. Passing it as
        # --root "%ROOT%" can escape the closing quote in Python argv and
        # produce a nonexistent path ending in a literal double quote.
        self.assertNotIn('--root "%ROOT%"', self.entry)
        self.assertEqual(self.entry.count('--game "%RIMWORLD_DIR%"'), 2)
        for source in (self.rimtest, self.pickle):
            self.assertIn("ROOT = common.ROOT", source)
            self.assertIn('parser.add_argument("--root", type=Path, default=ROOT)', source)
            self.assertIn('args.root.resolve()', source)

    def test_rimtest_is_test_only_and_structured(self):
        for marker in (
            "[TestSuite]", "[Test]", "AMJE_RIMTEST_RESULT", "RimTestSummary.json",
            "AMJE.EnvironmentRimTestAudit", "IsolatedDesktopRunner.exe",
            "RIMWORLD_AMJE_ERROR_DIRECTORY", "EXPECTED_TESTS = 10",
        ):
            self.assertIn(marker, self.rimtest_source + self.rimtest + self.common)
        self.assertIn("RimTestRedux.dll", self.rimtest)
        self.assertIn("AncientMedievalJapanEnvironment.dll", self.rimtest)
        self.assertNotIn(
            "RimTestRedux",
            (ROOT / "Source/AncientMedievalJapanEnvironment/AncientMedievalJapanEnvironment.csproj")
            .read_text(encoding="utf-8"),
        )

    def test_pickle_development_matrix_and_bindings(self):
        self.assertIn('PROFILES = ("Vanilla", "MO", "CCTO", "MO-CCTO")', self.pickle)
        self.assertEqual(self.feature.count("  Scenario:"), 5)
        for sentence in (
            "the development Environment Mod and assembly come from the expected source folder",
            "all four Environment plants are uniquely owned by the expected source Mod",
        ):
            self.assertIn("Then " + sentence, self.feature)
            self.assertIn('[Then("' + sentence + '")]', self.steps)
        for marker in (
            "-pickle-run=", "-pickle-mode=fast", "summary.json",
            'result.get("exitReason") != "passed"',
            "DevelopmentPickleSummary.json", "RIMWORLD_AMJE_ERROR_DIRECTORY",
        ):
            self.assertIn(marker, self.pickle)

    def test_both_runners_preserve_source_and_normal_config(self):
        for source in (self.rimtest, self.pickle):
            self.assertIn("snapshot_tree(root)", source)
            self.assertIn('ModsConfig.xml", "Prefs.xml', source)
            self.assertIn("common.validate_runtime_logs", source)


if __name__ == "__main__":
    unittest.main(verbosity=2)
