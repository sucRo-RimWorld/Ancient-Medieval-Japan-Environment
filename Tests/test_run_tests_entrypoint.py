from pathlib import Path
import unittest

ROOT = Path(__file__).resolve().parents[1]


class UnifiedTestEntrypointTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.run_tests = (ROOT / "run-tests.bat").read_text(encoding="utf-8")
        cls.run_static = (ROOT / "run-static-tests.bat").read_text(encoding="utf-8")
        cls.run_runtime = (ROOT / "run-runtime-tests.bat").read_text(encoding="utf-8")
        cls.isolated = (
            ROOT / "Scripts" / "Run-EnvironmentIsolatedDesktop.ps1"
        ).read_text(encoding="utf-8")
        cls.quickstarts = (
            ROOT / "Scripts" / "Run-EnvironmentVegetationQuickstarts.ps1"
        ).read_text(encoding="utf-8")
        cls.validator = (
            ROOT / "Scripts" / "Validate-Environment.ps1"
        ).read_text(encoding="utf-8")
        cls.climate_diagnostics = (
            ROOT / "Source" / "AncientMedievalJapanEnvironment"
            / "ClimateCalibrationDiagnostics.cs"
        ).read_text(encoding="utf-8")
        cls.runtime_log_validator = (
            ROOT / "Scripts" / "Validate-EnvironmentRuntimeLog.ps1"
        ).read_text(encoding="utf-8")

    def test_run_tests_is_the_full_standard_entrypoint(self):
        self.assertIn('run-static-tests.bat', self.run_tests)
        self.assertIn('Run-EnvironmentIsolatedDesktop.ps1', self.run_tests)
        self.assertNotIn('run-runtime-tests.bat', self.run_tests)

    def test_runtime_runner_does_not_recurse_into_run_tests(self):
        self.assertNotIn('call "%ROOT%run-tests.bat"', self.run_runtime)
        self.assertIn('run-static-tests.bat', self.run_runtime)
        self.assertIn('--skip-static', self.run_runtime)
        self.assertIn('Ancient-Medieval-Japan-Grains', self.run_runtime)

    def test_validator_checks_actual_static_gate_ownership(self):
        self.assertIn('$runStaticTestsSource', self.validator)
        self.assertIn('run-static-tests.bat is missing expected static-audit marker', self.validator)
        self.assertIn('Validate-MedievalOverhaulTreeTextures.ps1', self.run_static)
        self.assertIn('run-static-tests.bat', self.run_tests)
        self.assertIn('Run-EnvironmentIsolatedDesktop.ps1', self.run_tests)
        self.assertIn('run-static-tests.bat', self.validator)
        self.assertNotIn('run-tests.bat is missing expected MO static-audit marker', self.validator)
        self.assertIn("if ($runtimeBatch.Contains('call \"%ROOT%run-tests.bat\"'))", self.validator)

    def test_isolated_launcher_keeps_rendering_on_private_desktop(self):
        self.assertIn('CreateDesktop', self.isolated)
        self.assertIn("startup.desktop = 'WinSta0\\'", self.isolated)
        self.assertIn('run-runtime-tests.bat', self.isolated)
        self.assertIn('--skip-static', self.isolated)
        self.assertNotIn('-nographics', self.isolated)
        self.assertNotIn('SwitchDesktop', self.isolated)

    def test_highland_climate_sampling_keeps_full_gradient_gate(self):
        climate = self.climate_diagnostics
        self.assertIn(
            "FindRepresentative(layer, 2f, 1500f, 3200f, 1900f, true)",
            climate,
        )
        self.assertEqual(
            climate.count("allowImpassableFallback = false"), 1
        )
        self.assertIn("if (!result.Valid && allowImpassableFallback)", climate)
        self.assertIn("!allowImpassable && tile.hilliness == Hilliness.Impassable", climate)
        self.assertIn("targetElevation, true, false", climate)
        self.assertIn("targetElevation, false, false", climate)
        self.assertIn("targetElevation, true, true", climate)
        self.assertIn("targetElevation, false, true", climate)
        self.assertIn("if ($RequireClimateGradient)", self.runtime_log_validator)
        self.assertIn(
            '@("WarmLowland", "TemperateLowland", "CoolLowland", "Highland")',
            self.runtime_log_validator,
        )

    def test_standard_runtime_suite_includes_both_wetlands(self):
        self.assertIn('"AMJTemperateSwampVegetationQuickstart"', self.quickstarts)
        self.assertIn('"AMJColdBogVegetationQuickstart"', self.quickstarts)
        self.assertIn('"AMJRiverMapHandoffQuickstart"', self.quickstarts)
        self.assertIn('"AMJCoastMapHandoffQuickstart"', self.quickstarts)

    def test_static_gate_still_contains_build_and_source_validation(self):
        for required in (
            'Validate-PowerShellSyntax.ps1',
            'build.bat',
            'Validate-Environment.ps1',
            'Validate-MedievalOverhaulTreeTextures.ps1',
        ):
            self.assertIn(required, self.run_static)


if __name__ == "__main__":
    unittest.main(verbosity=2)
