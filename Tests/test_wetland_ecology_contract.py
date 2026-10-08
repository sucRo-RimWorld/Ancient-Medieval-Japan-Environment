"""Static AMJE wetland ecology contract.

Validates the exact set of complete-field replacements in the retained
Vanilla wetland BiomeDefs. The game Quickstarts test the loaded Defs and terrain.
"""
from pathlib import Path
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
PATCH = ROOT / "Patches" / "VanillaWetlandEcology.xml"
QUICKSTART = ROOT / "Tests" / "Quickstarts" / "EnvironmentBiomeTerrainQuickstarts.cs"
DESCRIPTION_PATCH = ROOT / "Patches" / "VanillaWetlandDescriptions.xml"
JAPANESE_DESCRIPTIONS = (
    ROOT / "Languages" / "Japanese" / "DefInjected"
    / "BiomeDef" / "AMJ_Biomes.xml"
)

# The Japanese strings were approved by the author on 2026-10-08.
APPROVED_DESCRIPTIONS = {
    "TemperateSwamp": {
        "ja": "日本列島の温暖で雨の多い低地や河川沿いに広がる湿地。背の高い草やヤナギ類の木立が入り混じり、泥土と浅い水面が広がる。湿った地盤は通行や建築に制約を与える。",
        "en": "A wetland found in the warm, rainy lowlands and along rivers of the Japanese archipelago. Tall grasses and stands of willow trees intermingle among muddy ground and shallow water. The damp ground restricts travel and construction.",
    },
    "ColdBog": {
        "ja": "冷涼な地域に広がる湿原。草本やコケ類に加え、ヤナギやカバノキ類の木立が点在する。水を多く含む地盤はぬかるみやすく、移動や建築が難しい。",
        "en": "A wetland found across cooler regions. Grasses and mosses grow alongside scattered stands of willow and birch trees. The waterlogged ground readily turns muddy, making travel and construction difficult.",
    },
}


WILDLIFE = {
    "TemperateSwamp": {
        "Hare": 0.8, "Squirrel": 0.6, "Rat": 1.0, "Deer": 0.7,
        "WildBoar": 0.8, "Fox_Red": 0.06, "Wolf_Timber": 0.03,
        "Bear_Grizzly": 0.04,
    },
    "ColdBog": {
        "Hare": 0.8, "Snowhare": 0.5, "Squirrel": 0.7, "Rat": 1.0,
        "Deer": 0.5, "WildBoar": 0.5, "Fox_Red": 0.05,
        "Wolf_Timber": 0.05, "Bear_Grizzly": 0.04,
    },
}
WEATHER = {
    "TemperateSwamp": {
        "Clear": 16, "Fog": 2, "Rain": 3,
        "DryThunderstorm": 0.1, "RainyThunderstorm": 1.5,
        "FoggyRain": 1.5, "SnowGentle": 2, "SnowHard": 1,
    },
    "ColdBog": {
        "Clear": 16, "Fog": 2, "Rain": 3,
        "DryThunderstorm": 0.05, "RainyThunderstorm": 1,
        "FoggyRain": 1.5, "SnowGentle": 7, "SnowHard": 7,
    },
}
DISEASE = {
    "TemperateSwamp": {
        "Disease_Flu": 100, "Disease_Plague": 45,
        "Disease_Malaria": 35, "Disease_GutWorms": 65,
        "Disease_MuscleParasites": 40, "Disease_AnimalFlu": 90,
        "Disease_AnimalPlague": 40,
    },
    "ColdBog": {
        "Disease_Flu": 100, "Disease_Plague": 35,
        "Disease_GutWorms": 40, "Disease_MuscleParasites": 30,
        "Disease_AnimalFlu": 100, "Disease_AnimalPlague": 35,
    },
}
DISEASE_MTB = {"TemperateSwamp": 50, "ColdBog": 60}


class WetlandEcologyContractTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        root = ET.parse(PATCH).getroot()
        cls.ops = root.findall("./Operation/operations/li")
        cls.quickstart = QUICKSTART.read_text(encoding="utf-8")

    def test_exact_fields_and_atomic_target_xpaths(self):
        self.assertEqual(len(self.ops), 10)
        targets = set()
        for op in self.ops:
            self.assertEqual(op.get("Class"), "PatchOperationReplace")
            xpath = op.findtext("xpath")
            self.assertIsNotNone(xpath)
            self.assertNotIn(xpath, targets)
            targets.add(xpath)
            value = op.find("value")
            self.assertIsNotNone(value)
            self.assertEqual(len(value), 1)
            self.assertTrue(xpath.endswith("/" + value[0].tag))
        expected_fields = {
            "wildAnimals", "baseWeatherCommonalities", "diseaseMtbDays",
            "diseases", "allowedPackAnimals",
        }
        self.assertEqual(
            targets,
            {f'/Defs/BiomeDef[defName="{biome}"]/{field}'
             for biome in WILDLIFE for field in expected_fields},
        )

    def replacement(self, biome, field):
        xpath = f'/Defs/BiomeDef[defName="{biome}"]/{field}'
        op = next(op for op in self.ops if op.findtext("xpath") == xpath)
        return op.find("value/" + field)

    def test_wildlife_and_pack_animals(self):
        for biome, expected in WILDLIFE.items():
            with self.subTest(biome=biome):
                node = self.replacement(biome, "wildAnimals")
                actual = {x.tag: float(x.text) for x in node}
                self.assertEqual(len(node), len(expected))
                self.assertEqual(actual, expected)
                self.assertEqual(len(self.replacement(biome, "allowedPackAnimals")), 0)
                self.assertTrue(all(x > 0 for x in actual.values()))

    def test_weather_and_disease_balance(self):
        for biome in WILDLIFE:
            with self.subTest(biome=biome):
                weather = self.replacement(biome, "baseWeatherCommonalities")
                self.assertEqual(
                    {x.tag: float(x.text) for x in weather},
                    WEATHER[biome],
                )
                disease = self.replacement(biome, "diseases")
                actual = {
                    x.findtext("diseaseInc"): float(x.findtext("commonality"))
                    for x in disease
                }
                self.assertEqual(len(disease), len(DISEASE[biome]))
                self.assertEqual(actual, DISEASE[biome])
                self.assertNotIn("Disease_FibrousMechanites", actual)
                self.assertNotIn("Disease_SensoryMechanites", actual)
                self.assertEqual(
                    float(self.replacement(biome, "diseaseMtbDays").text),
                    DISEASE_MTB[biome],
                )

    def test_only_approved_wetland_descriptions_are_replaced(self):
        root = ET.parse(DESCRIPTION_PATCH).getroot()
        self.assertEqual(root.tag, "Patch")
        operations = root.findall("./Operation/operations/li")
        self.assertEqual(len(operations), len(APPROVED_DESCRIPTIONS))
        actual = {}
        for op in operations:
            self.assertEqual(op.get("Class"), "PatchOperationReplace")
            xpath = op.findtext("xpath")
            body = op.find("value")
            self.assertIsNotNone(xpath)
            self.assertIsNotNone(body)
            self.assertEqual(len(body), 1)
            self.assertEqual(body[0].tag, "description")
            self.assertNotIn(xpath, actual)
            actual[xpath] = body[0].text
        self.assertEqual(actual, {
            f'/Defs/BiomeDef[defName="{id}"]/description': parts["en"]
            for id, parts in APPROVED_DESCRIPTIONS.items()
        })

    def test_japanese_definjected_contains_exact_approved_text(self):
        root = ET.parse(JAPANESE_DESCRIPTIONS).getroot()
        self.assertEqual(root.tag, "LanguageData")
        for biome, text in APPROVED_DESCRIPTIONS.items():
            with self.subTest(biome=biome):
                key = biome + ".description"
                nodes = root.findall(key)
                self.assertEqual(len(nodes), 1)
                self.assertEqual(nodes[0].text, text["ja"])
                self.assertIsNone(root.find(biome + ".label"))

    def test_loaded_wetland_quickstarts_validate_approved_description(self):
        self.assertIn("has an approved EN/JA wetland description", self.quickstart)
        for parts in APPROVED_DESCRIPTIONS.values():
            self.assertIn(parts["ja"], self.quickstart)
            self.assertIn(parts["en"], self.quickstart)
        self.assertIn("approvedEnglishDescription", self.quickstart)
        self.assertIn("approvedJapaneseDescription", self.quickstart)

    def test_quickstarts_check_loaded_ecology_and_wetland_terrain(self):
        source = self.quickstart
        self.assertIn("protected static void AddWetlandEcologyAssertions", source)
        self.assertIn("AddWetlandEcologyAssertions(result, Find.CurrentMap, false)", source)
        self.assertIn("AddWetlandEcologyAssertions(result, Find.CurrentMap, true)", source)
        for field in ("wildAnimals", "diseases", "allowedPackAnimals",
                      "baseWeatherCommonalities", "diseaseMtbDays",
                      "terrainPatchMakers"):
            self.assertIn(field, source)
        for terrain in ("Mud", "MarshyTerrain", "Marsh", "WaterShallow"):
            self.assertIn('t.defName == "' + terrain + '"', source)


if __name__ == "__main__":
    unittest.main(verbosity=2)
