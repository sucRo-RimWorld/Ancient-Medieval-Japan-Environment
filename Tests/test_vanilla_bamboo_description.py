"""Approved Vanilla Bamboo description; Japanese first, English aligned."""
from pathlib import Path
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
JA_PARTS = ("竹（タケ）は、日本の温暖で湿潤な土地に群生する、木のような姿のイネ科植物。古代・中世に存在したマダケやハチクなどの竹類を、ここではまとめて表す。", "竹は軽くしなやかで、古くからかご・日用品や建築・農漁業の資材に使われてきた。現在広く見られるモウソウチクは江戸時代の渡来とする記録があり、ここで想定する竹類とは区別する。")
EN_PARTS = ("Bamboo (take) is a member of the grass family that grows in stands in Japan's warm, humid regions, despite its tree-like appearance. Here it represents bamboo types such as madake and hachiku that existed in ancient and medieval Japan.", "Lightweight and flexible, bamboo has long been used for baskets, everyday objects, and materials for building, farming, and fishing. Moso bamboo, common in Japan today, is recorded as having arrived during the Edo period and is distinguished here from these earlier bamboo types.")
SOURCE = ROOT / "Docs/VanillaPlantStep2DescriptionReview-ja.md"
LOCALIZATION = ROOT / "Languages/Japanese/DefInjected/ThingDef/AMJ_WildPlants.xml"
PATCH = ROOT / "Patches/VanillaTreeDescriptions.xml"
QUICKSTART = ROOT / "Tests/Quickstarts/EnvironmentBiomeTerrainQuickstarts.cs"


class ApprovedBambooDescriptionTest(unittest.TestCase):
    def test_approved_japanese_and_corresponding_translation_in_source(self):
        source = SOURCE.read_text(encoding="utf-8")
        block = source.split("### Plant_TreeBamboo", 1)[1].split(
            "### Plant_TreeMaple", 1
        )[0]
        for paragraph in JA_PARTS + EN_PARTS:
            self.assertIn(paragraph, block)
        self.assertIn("日本語：作者承認済み", block)
        self.assertIn("ほか5樹木", source)

    def test_description_patch_is_english_only_and_atomic(self):
        root = ET.parse(PATCH).getroot()
        self.assertEqual(root.tag, "Patch")
        operations = root.findall("Operation")
        self.assertEqual(len(operations), 1)
        op = operations[0]
        self.assertEqual(op.get("Class"), "PatchOperationReplace")
        self.assertEqual(
            op.findtext("xpath"),
            '/Defs/ThingDef[defName="Plant_TreeBamboo"]/description',
        )
        value = op.find("value")
        self.assertIsNotNone(value)
        self.assertEqual([node.tag for node in value], ["description"])
        self.assertEqual(value.findtext("description"), r"\n\n".join(EN_PARTS))

    def test_approved_japanese_localized_and_other_five_unmodified(self):
        root = ET.parse(LOCALIZATION).getroot()
        self.assertEqual(root.tag, "LanguageData")
        self.assertEqual(
            root.findtext("Plant_TreeBamboo.description"),
            r"\n\n".join(JA_PARTS),
        )
        self.assertIsNone(root.find("Plant_TreeBamboo.label"))
        for name in ("Plant_TreeMaple", "Plant_TreeOak", "Plant_TreeBirch",
                     "Plant_TreePine", "Plant_TreeWillow"):
            self.assertIsNone(root.find(name + ".description"))

    def test_warm_quickstart_loads_bamboo_description(self):
        source = QUICKSTART.read_text(encoding="utf-8")
        warm = source.split(
            "public sealed class AMJWarmTemperateTerrainQuickstart", 1
        )[1].split("public sealed class AMJTemperateSwampVegetationQuickstart", 1)[0]
        self.assertIn(
            'result.Assert("Plant_TreeBamboo has approved EN/JA description"',
            warm,
        )
        for paragraph in JA_PARTS + EN_PARTS:
            self.assertIn(paragraph, warm)
        self.assertIn('DefDatabase<ThingDef>.GetNamedSilentFail("Plant_TreeBamboo")', warm)
        self.assertIn("string.Equals(actual, approvedEnglish", warm)
        self.assertIn("string.Equals(actual, approvedJapanese", warm)


if __name__ == "__main__":
    unittest.main(verbosity=2)
