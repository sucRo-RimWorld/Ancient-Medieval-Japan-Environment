from pathlib import Path
import importlib.util
import tempfile
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]


class UnifiedTestEntrypointTests(unittest.TestCase):
    def test_yoshi_fixture_is_experimental_and_preserves_wetland_probes(self):
        fixture = ROOT / "Tests/Quickstarts/Fixtures/WetlandBehaviorProbeDefs.xml"
        defs = {
            node.findtext("defName"): node
            for node in ET.parse(fixture).getroot().findall("ThingDef")
        }
        self.assertEqual(set(defs), {
            "AMJE_Test_Suge", "AMJE_Test_Mizugoke", "AMJE_Test_Yoshi",
            "AMJE_Test_Hannoki",
            "AMJE_Test_SugeSoilNativeSpawn",
            "AMJE_Test_HannokiSoilNativeSpawn",
            "AMJE_Test_MizugokeSoilNativeSpawn"
        })
        yoshi = defs["AMJE_Test_Yoshi"]
        self.assertEqual(yoshi.get("ParentName"), "PlantBase")
        self.assertEqual(yoshi.findtext("plant/minGrowthTemperature"), "0")
        self.assertEqual(yoshi.findtext("plant/harvestedThingDef"), "Hay")
        self.assertEqual(yoshi.findtext("plant/harvestYield"), "1")
        self.assertEqual(yoshi.findtext("plant/harvestMinGrowth"), "0.80")
        self.assertIsNone(yoshi.find("plant/sowTags"))
        self.assertEqual(yoshi.findtext("ingestible/preferability"),
                         "NeverForNutrition")

    def test_hannoki_fixture_native_wood_and_mo_control(self):
        fixture = ROOT / "Tests/Quickstarts/Fixtures/WetlandBehaviorProbeDefs.xml"
        defs = {
            node.findtext("defName"): node
            for node in ET.parse(fixture).getroot().findall("ThingDef")
        }
        tree = defs["AMJE_Test_Hannoki"]
        self.assertEqual(tree.get("ParentName"), "DeciduousTreeBase")
        self.assertEqual(tree.findtext("plant/minGrowthTemperature"), "0")
        self.assertEqual(tree.findtext("plant/harvestedThingDef"), "WoodLog")
        self.assertEqual(tree.findtext("plant/harvestTag"), "Wood")
        self.assertEqual(tree.findtext("plant/harvestYield"), "1")
        self.assertIsNone(tree.find("plant/sowTags"))
        self.assertEqual(tree.findtext("plant/leaflessGraphicPath"),
                         "Things/Plant/AMJ/Beech_Leafless")

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
            patch = (ROOT / "Tests/Quickstarts/Fixtures" /
                     "WetlandSoilNativeSpawnPatch.xml")
            staged_patch = (manager.fixture_root(game) / "Patches" /
                            "WetlandSoilNativeSpawnPatch.xml")
            self.assertEqual(staged_patch.read_bytes(), patch.read_bytes())
            # No new test case: the existing fixture round-trip covers this
            # disposable patch and the prior native CutPlant probes.

            # Zero-nutrition PlantBase probes must not retain inherited
            # RawBad food preference (duplicate startup ERROR per Def).
            for plant in ET.parse(probe).getroot().findall("ThingDef"):
                self.assertEqual(
                    plant.findtext("ingestible/preferability"),
                    "NeverForNutrition",
                    plant.findtext("defName"),
                )

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
