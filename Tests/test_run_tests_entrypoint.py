from pathlib import Path
import importlib.util
import tempfile
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]


class UnifiedTestEntrypointTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.run_tests = (ROOT / "run-tests.bat").read_text(encoding="utf-8")
        cls.run_static = (ROOT / "run-static-tests.bat").read_text(encoding="utf-8")
        cls.run_runtime = (ROOT / "run-runtime-tests.bat").read_text(encoding="utf-8")
        cls.run_framework = (ROOT / "run-framework-tests.bat").read_text(encoding="utf-8")
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
        cls.world_quickstarts = (
            ROOT / "Tests" / "Quickstarts" / "EnvironmentBiomeTerrainQuickstarts.cs"
        ).read_text(encoding="utf-8")
        cls.runtime_log_validator = (
            ROOT / "Scripts" / "Validate-EnvironmentRuntimeLog.ps1"
        ).read_text(encoding="utf-8")
        cls.load_folders = (ROOT / "loadFolders.xml").read_text(encoding="utf-8")
        cls.quicktest_manager = (
            ROOT / "Scripts" / "Manage-EnvironmentQuicktestFixture.py"
        ).read_text(encoding="utf-8")

    def test_run_tests_is_the_full_standard_entrypoint(self):
        self.assertIn('run-static-tests.bat', self.run_tests)
        self.assertIn('run-framework-tests.bat', self.run_tests)
        self.assertIn('Run-EnvironmentIsolatedDesktop.ps1', self.run_tests)
        self.assertNotIn('run-runtime-tests.bat', self.run_tests)
        self.assertIn('Run-EnvironmentRimTest.py', self.run_framework)
        self.assertIn('Run-DevelopmentPickle.py', self.run_framework)
        self.assertIn('--skip-static', self.run_framework)
        # The established hidden rendered Quickstarts must run even if a
        # later RimTest/Pickle development harness gate fails. Neither gate
        # may be silently skipped or treated as optional in the full command.
        self.assertLess(
            self.run_tests.index('run-static-tests.bat'),
            self.run_tests.index('Run-EnvironmentIsolatedDesktop.ps1'),
        )
        self.assertLess(
            self.run_tests.index('Run-EnvironmentIsolatedDesktop.ps1'),
            self.run_tests.index('run-framework-tests.bat'),
        )
        self.assertLess(
            self.run_tests.index('run-framework-tests.bat'),
            self.run_tests.index('[OK] AMJ Environment full static + framework + isolated runtime validation passed'),
        )
        self.assertEqual(self.run_tests.count('if errorlevel 1 exit /b 1'), 3)

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

    def test_xml_validation_loads_whole_utf8_documents(self):
        # Without -Raw PowerShell may coerce Get-Content's line array into an
        # invalid XmlDocument even when About.xml is well formed.
        for statement in (
            "[xml]$riverDefs = Get-Content -LiteralPath $RiverDefsPath -Raw -Encoding UTF8",
            "[xml]$worldGenerator = Get-Content -LiteralPath $WorldGeneratorPath -Raw -Encoding UTF8",
            "[xml](Get-Content -LiteralPath $path -Raw -Encoding UTF8) | Out-Null",
            '[xml]$about = Get-Content -LiteralPath (Join-Path $RepoRoot "About\\About.xml") -Raw -Encoding UTF8',
        ):
            self.assertIn(statement, self.validator)

    def test_isolated_launcher_keeps_rendering_on_private_desktop(self):
        self.assertIn('CreateDesktop', self.isolated)
        self.assertIn("startup.desktop = 'WinSta0\\'", self.isolated)
        self.assertIn('run-runtime-tests.bat', self.isolated)
        self.assertIn('--skip-static', self.isolated)
        self.assertNotIn('-nographics', self.isolated)
        self.assertNotIn('SwitchDesktop', self.isolated)
        self.assertIn('Manage-EnvironmentQuicktestFixture.py', self.isolated)
        self.assertIn('cleanup --game $RimWorldRoot', self.isolated)

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

    def test_mo_core_integration_reuses_wetland_native_spawn_quickstarts(self):
        # The separately launched, proven MO+Grains/Core profile needs both
        # existing wetland map scenarios, not only the four forest/river/coast
        # scenarios. Read the real Quickstart map; never force MO herb spawns.
        core = self.quickstarts.split("elseif ($CoreIntegrationOnly) {", 1)[1].split(
            "else {", 1
        )[0]
        for name in (
            "AMJTemperateSwampVegetationQuickstart",
            "AMJColdBogVegetationQuickstart",
        ):
            self.assertIn('"' + name + '"', core)
        wetland = self.world_quickstarts.split(
            "protected static void AddWetlandEcologyAssertions(", 1
        )[1].split("private static void AddWildlifeAssertions(", 1)[0]
        self.assertIn("map.listerThings.ThingsOfDef(herb).Count", wetland)
        self.assertIn("[AMJ Environment MO Wetland Native Spawn]", wetland)
        self.assertIn('naturalHerbs > 0', wetland)
        self.assertIn('if (medievalOverhaulActive)', wetland)
        self.assertNotIn("GenSpawn.Spawn(", wetland)
        self.assertNotIn("GenSpawn.TrySpawn", wetland)

    def test_unforced_natural_world_wetlands_gate(self):
        scenario = "AMJWorldWetlandDistributionQuickstart"
        self.assertIn(f'"{scenario}"', self.quickstarts)
        marker = f"class {scenario} : AbstractQuickstart"
        self.assertIn(marker, self.world_quickstarts)
        body = self.world_quickstarts.split(marker, 1)[1].split(
            "public sealed class AMJRiverMapHandoffQuickstart", 1
        )[0]
        for requirement in (
            "get { return 0.30f; }",
            "wetlandCandidates > 0",
            "wetlands > 0",
            "wetlandOutsideCandidateTiles == 0",
            "wetlands <= landTiles * 0.20f",
            "selectedNaturalWetland",
        ):
            self.assertIn(requirement, body)
        self.assertIn("preferredWetland.Valid || (!wetland && fallback.Valid)", body)
        self.assertIn("candidateShare=", body)
        self.assertIn("wetlandOfCandidates=", body)
        self.assertIn('$scenarioTimeout = [Math]::Max($TimeoutSeconds, 420)', self.quickstarts)
        self.assertIn('while ($elapsedSeconds -lt $scenarioTimeout)', self.quickstarts)
        self.assertNotIn("tile.PrimaryBiome = ", body)
        self.assertNotIn("tile.swampiness = ", body)

    def test_static_gate_still_contains_build_and_source_validation(self):
        for required in (
            'Validate-PowerShellSyntax.ps1',
            'build.bat',
            'Validate-Environment.ps1',
            'Validate-MedievalOverhaulTreeTextures.ps1',
        ):
            self.assertIn(required, self.run_static)


    def test_production_loadfolders_are_root_only(self):
        self.assertEqual(
            self.load_folders,
            '<?xml version="1.0" encoding="utf-8"?>\n'
            '<loadFolders><v1.6><li>/</li></v1.6></loadFolders>\n',
        )
        self.assertNotIn("DevQuickstarts", self.load_folders)
        self.assertNotIn("IfModActive", self.load_folders)
        # The PowerShell validator must enforce the same root-only production
        # contract. An old Quickstarts gating check once blocked run-tests.bat.
        for marker in (
            "[xml]$loadFoldersXml = Get-Content -LiteralPath $loadFoldersPath -Raw -Encoding UTF8",
            "$loadFolderVersions.Count -ne 1",
            "$loadFolderEntries.Count -ne 1",
            "$loadFolderEntries[0].Attributes.Count -ne 0",
            "$loadFolderEntries[0].InnerText -ne '/'",
            "developer Quickstarts are staged as a separate test Mod",
        ):
            self.assertIn(marker, self.validator)
        self.assertNotIn("loadFolders.xml is missing expected Quickstarts gating marker",
                         self.validator)
        self.assertNotIn("Developer Quickstarts are gated behind rimworks.quickstarts",
                         self.validator)

    def test_runtime_quicktests_are_staged_outside_production_load_path(self):
        for marker in (
            "Manage-EnvironmentQuicktestFixture.py",
            'stage --game "%RIMWORLD_DIR%" --dll "%QUICKTEST_DLL%"',
            'activate --game "%RIMWORLD_DIR%"',
            'cleanup --game "%RIMWORLD_DIR%"',
        ):
            self.assertIn(marker, self.run_runtime)
        self.assertEqual(self.run_runtime.count('activate --game "%RIMWORLD_DIR%"'), 3)
        self.assertIn("sucro.ancientmedievaljapan.environment.quicktests", self.quicktest_manager)
        self.assertIn('--defs "%WETLAND_PROBE_DEFS%"', self.run_runtime)
        self.assertIn("Quicktests fixture path already exists; refusing to overwrite", self.quicktest_manager)
        self.assertIn("Refusing to manage unowned Quicktests fixture", self.quicktest_manager)

    def test_quicktest_fixture_manager_stage_activate_cleanup(self):
        spec = importlib.util.spec_from_file_location(
            "quicktest_fixture",
            ROOT / "Scripts" / "Manage-EnvironmentQuicktestFixture.py",
        )
        manager = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(manager)

        with tempfile.TemporaryDirectory() as temp:
            temp = Path(temp)
            game = temp / "RimWorld"
            (game / "Mods").mkdir(parents=True)
            dll = temp / manager.DLL_NAME
            dll.write_bytes(b"MZfixture")
            probe = ROOT / "Tests/Quickstarts/Fixtures/WetlandBehaviorProbeDefs.xml"
            manager.stage(game, dll, probe)
            staged = manager.fixture_root(game) / "Defs/WetlandBehaviorProbeDefs.xml"
            self.assertEqual(staged.read_bytes(), probe.read_bytes())

            with self.assertRaisesRegex(ValueError, "refusing to overwrite"):
                manager.stage(game, dll, probe)

            config = temp / "ModsConfig.xml"
            config.write_text(
                '<?xml version="1.0" encoding="utf-8"?>'
                '<ModsConfigData><activeMods>'
                '<li>brrainz.harmony</li><li>ludeon.rimworld</li>'
                '<li>rimworks.quickstarts</li>'
                '<li>sucro.ancientmedievaljapan.environment</li>'
                '</activeMods></ModsConfigData>',
                encoding="utf-8",
            )
            manager.activate(game, config)
            active = ET.parse(config).getroot().find("activeMods")
            ids = [(node.text or "").strip().lower() for node in active]
            self.assertEqual(
                ids[-2:],
                [
                    manager.ENVIRONMENT_PACKAGE,
                    manager.PACKAGE,
                ],
            )
            self.assertEqual(ids.count(manager.PACKAGE), 1)

            manager.activate(game, config)
            active = ET.parse(config).getroot().find("activeMods")
            ids = [(node.text or "").strip().lower() for node in active]
            self.assertEqual(ids.count(manager.PACKAGE), 1)

            manager.cleanup(game)
            self.assertFalse(manager.fixture_root(game).exists())



if __name__ == "__main__":
    unittest.main(verbosity=2)
