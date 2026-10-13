from pathlib import Path
import importlib.util
import tempfile
import unittest
import sys

ROOT = Path(__file__).resolve().parents[2]


class FrameworkAutomationContractTests(unittest.TestCase):
    def test_pickle_candidate_identity_rejects_unrelated_or_missing_package(self):
        sys.path.insert(0, str(ROOT / 'Scripts'))
        try:
            spec = importlib.util.spec_from_file_location(
                'pickle_candidate_identity', ROOT / 'Scripts/Run-DevelopmentPickle.py')
            runner = importlib.util.module_from_spec(spec)
            spec.loader.exec_module(runner)
            with tempfile.TemporaryDirectory() as tmp:
                root = Path(tmp)
                (root / 'About').mkdir()
                about = root / 'About/About.xml'
                for identity in ('sucro.ancientmedievaljapan.environment',
                                 'sucro.ancientmedievaljapan.environment.releasevalidation'):
                    about.write_text('<ModMetaData><packageId>' + identity +
                                     '</packageId></ModMetaData>', encoding='utf-8')
                    self.assertEqual(identity, runner.source_package_id(root))
                for content in ('<ModMetaData/>',
                                '<ModMetaData><packageId>unrelated.mod</packageId></ModMetaData>'):
                    about.write_text(content, encoding='utf-8')
                    with self.assertRaisesRegex(ValueError, 'Unsupported Environment source identity'):
                        runner.source_package_id(root)
        finally:
            sys.path.pop(0)

    def test_rimtest_rejects_missing_or_wrong_production_provider(self):
        sys.path.insert(0, str(ROOT / "Scripts"))
        try:
            spec = importlib.util.spec_from_file_location(
                "rimtest_runner", ROOT / "Scripts/Run-EnvironmentRimTest.py")
            runner = importlib.util.module_from_spec(spec)
            spec.loader.exec_module(runner)
            with tempfile.TemporaryDirectory() as tmp:
                root = Path(tmp) / "candidate"
                runner.validate_loaded_source({"productionAssembly": str(
                    root / "Assemblies/AncientMedievalJapanEnvironment.dll")}, root)
                for result in ({}, {"productionAssembly": str(
                        Path(tmp) / "old-source/Assemblies/AncientMedievalJapanEnvironment.dll")}):
                    with self.assertRaises(ValueError):
                        runner.validate_loaded_source(result, root)
        finally:
            sys.path.pop(0)

    def test_parallel_pickle_entrypoint_preserves_worker_and_error_contract(self):
        """Guard the default/opt-in wiring without starting a second game."""
        batch = (ROOT / "run-framework-tests.bat").read_text(encoding="utf-8")
        runner = (ROOT / "Scripts/Run-DevelopmentPickle.py").read_text(encoding="utf-8")
        self.assertIn('if not defined PICKLE_WORKERS set "PICKLE_WORKERS=1"', batch)
        self.assertIn('if not "%PICKLE_WORKERS%"=="1" if not "%PICKLE_WORKERS%"=="2" if not "%PICKLE_WORKERS%"=="4"', batch)
        self.assertIn('--workers %PICKLE_WORKERS%', batch)
        self.assertIn('ThreadPoolExecutor(max_workers=workers)', runner)
        self.assertIn('common.validate_runtime_logs(log_path, errors)', runner)
        self.assertIn('if failures:', runner)
        self.assertIn('raise ValueError(\'Parallel Pickle profile failures:', runner)

    def test_parallel_pickle_invalid_worker_rejected_before_game_launch(self):
        sys.path.insert(0, str(ROOT / "Scripts"))
        try:
            spec = importlib.util.spec_from_file_location(
                "amje_development_pickle", ROOT / "Scripts/Run-DevelopmentPickle.py"
            )
            runner = importlib.util.module_from_spec(spec)
            spec.loader.exec_module(runner)
            with tempfile.TemporaryDirectory() as tmp:
                bogus = Path(tmp) / "does-not-exist"
                for count in (0, 3, 5, -1):
                    with self.subTest(workers=count):
                        with self.assertRaisesRegex(ValueError, "workers must be 1, 2 or 4"):
                            runner.run(bogus, bogus, bogus, count)
        finally:
            sys.path.pop(0)

    def test_17_test_version_gate_and_provider_resolution(self):
        spec = importlib.util.spec_from_file_location(
            "amje_framework_common_17",
            ROOT / "Scripts/EnvironmentFrameworkTestCommon.py",
        )
        common = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(common)
        self.assertEqual(common.parse_test_version("1.6.4871"), "1.6")
        self.assertEqual(common.parse_test_version("1.7.5000"), "1.7")
        for invalid in ("1.5.1", "1.8.0", "1.70.0", ""):
            with self.assertRaisesRegex(ValueError, "Expected RimWorld 1.6 or 1.7"):
                common.parse_test_version(invalid)
        with tempfile.TemporaryDirectory() as tmp:
            root = Path(tmp)
            for folder in ("1.6", "1.7", "Current"):
                dll = root / folder / "Assemblies/RimTestRedux.dll"
                dll.parent.mkdir(parents=True)
                dll.write_bytes(folder.encode("ascii"))
            self.assertEqual(common.find_dll(root, "RimTestRedux.dll", "1.6").read_bytes(), b"1.6")
            self.assertEqual(common.find_dll(root, "RimTestRedux.dll", "1.7").read_bytes(), b"1.7")
            (root / "LoadFolders.xml").write_text(
                "<loadFolders><v1.6><li>1.6</li></v1.6>"
                "<v1.7><li>/</li><li>1.7</li></v1.7></loadFolders>",
                encoding="utf-8",
            )
            self.assertEqual(common.find_dll(root, "RimTestRedux.dll", "1.7").read_bytes(), b"1.7")
            (root / "LoadFolders.xml").write_text(
                "<loadFolders><v1.6><li>1.6</li></v1.6></loadFolders>",
                encoding="utf-8",
            )
            self.assertEqual(common.find_dll(root, "RimTestRedux.dll", "1.7").read_bytes(), b"1.6")
            (root / "1.6/Assemblies/RimTestRedux.dll").unlink()
            with self.assertRaisesRegex(ValueError, "No active RimWorld 1.7"):
                common.find_dll(root, "RimTestRedux.dll", "1.7")

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


if __name__ == "__main__":
    unittest.main(verbosity=2)
