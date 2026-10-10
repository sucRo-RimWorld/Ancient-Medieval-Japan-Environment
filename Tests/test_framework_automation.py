from pathlib import Path
import importlib.util
import tempfile
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

    def test_rimtest_bridge_compiles_with_bundled_net_framework_csc(self):
        # Framework64/v4.0.30319/csc.exe is C# 5-era: nameof (C# 6)
        # aborts before the non-visible RimTest desktop is ever launched.
        self.assertIn(
            'postfix: new HarmonyMethod(typeof(RimTestAutomationBridge), "AfterResultsLogged")',
            self.rimtest_source,
        )
        self.assertIn("public static void AfterResultsLogged()", self.rimtest_source)
        self.assertNotIn("nameof(", self.rimtest_source)
        self.assertIn("Microsoft.NET/Framework64/v4.0.30319/csc.exe", self.common)

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

        # Pickle-only test folders are invisible to RimWorld's normal
        # ModContentPack.AnyNonTranslationContentLoaded guard. The fixture
        # must include a harmless text asset, not a dummy gameplay Def.
        self.assertIn('(fixture / "Strings").mkdir()', self.pickle)
        self.assertIn(
            '(fixture / "Strings/AMJEDevelopmentPickleAudit.txt").write_text(',
            self.pickle,
        )
        self.assertIn('AMJE development Pickle fixture load marker (test-only)', self.pickle)
        self.assertLess(
            self.pickle.index('(fixture / "Strings/AMJEDevelopmentPickleAudit.txt").write_text('),
            self.pickle.index('common.run_hidden(desktop, batch, env, runner_log)'),
        )

    def test_harmony_rimworld_16_loader_selects_current_not_old_dlls(self):
        spec = importlib.util.spec_from_file_location(
            "amje_framework_common",
            ROOT / "Scripts/EnvironmentFrameworkTestCommon.py",
        )
        common = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(common)
        with tempfile.TemporaryDirectory() as tmp:
            root = Path(tmp)
            for folder in ("1.4", "1.5", "Current"):
                dll = root / folder / "Assemblies/0Harmony.dll"
                dll.parent.mkdir(parents=True)
                dll.write_bytes(folder.encode("ascii"))
            (root / "LoadFolders.xml").write_text(
                "<loadFolders>"
                "<v1.4><li>/</li><li>1.4</li></v1.4>"
                "<v1.5><li>/</li><li>1.5</li></v1.5>"
                "<v1.6><li>/</li><li>Current</li></v1.6>"
                "</loadFolders>",
                encoding="utf-8",
            )
            selected = common.find_dll(root, "0Harmony.dll")
            self.assertEqual(selected, root / "Current/Assemblies/0Harmony.dll")
            self.assertEqual(selected.read_bytes(), b"Current")
            # A missing active DLL must fail rather than fall back to 1.5/1.4.
            selected.unlink()
            with self.assertRaisesRegex(ValueError, "No active RimWorld 1.6"):
                common.find_dll(root, "0Harmony.dll")
            self.assertIn('common.find_dll(workshop / "2009463077", "0Harmony.dll")',
                          self.rimtest)

    def test_both_runners_preserve_source_and_normal_config(self):
        for source in (self.rimtest, self.pickle):
            self.assertIn("snapshot_tree(root)", source)
            self.assertIn('ModsConfig.xml", "Prefs.xml', source)
            self.assertIn("common.validate_runtime_logs", source)


if __name__ == "__main__":
    unittest.main(verbosity=2)
