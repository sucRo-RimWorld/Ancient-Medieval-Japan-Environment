"""Approved Vanilla tree labels and descriptions: English/Japanese static and loaded contracts."""
from pathlib import Path
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "Docs/VanillaPlantStep2DescriptionReview-ja.md"
LOCALIZATION = ROOT / "Languages/Japanese/DefInjected/ThingDef/AMJ_WildPlants.xml"
PATCH = ROOT / "Patches/VanillaTreeDescriptions.xml"
QUICKSTART = ROOT / "Tests/Quickstarts/EnvironmentBiomeTerrainQuickstarts.cs"

APPROVED = {
    "Plant_TreeBamboo": {"label_en": "Bamboo (take)", "label_ja": "竹", "ja": ["竹（タケ）は、日本の温暖で湿潤な土地に群生する、木のような姿のイネ科植物。古代・中世に存在したマダケやハチクなどの竹類を、ここではまとめて表す。","竹は軽くしなやかで、古くからかご・日用品や建築・農漁業の資材に使われてきた。現在広く見られるモウソウチクは江戸時代の渡来とする記録があり、ここで想定する竹類とは区別する。"], "en": ["Bamboo (take) is a member of the grass family that grows in stands in Japan's warm, humid regions, despite its tree-like appearance. Here it represents bamboo types such as madake and hachiku that existed in ancient and medieval Japan.","Lightweight and flexible, bamboo has long been used for baskets, everyday objects, and materials for building, farming, and fishing. Moso bamboo, common in Japan today, is recorded as having arrived during the Edo period and is distinguished here from these earlier bamboo types."]},
    "Plant_TreeMaple": {"label_en": "Maple (kaede)", "label_ja": "カエデ", "ja": ["楓（カエデ）は、日本の落葉広葉樹林にも生育するカエデ属の樹木をまとめた呼び名。多くは季節によって葉の色を変える。","縄文時代の遺跡周辺にもカエデ属を含む森林が確認されており、古くから日本列島の森林を構成していた。この植物は特定のカエデ一種を再現するものではない。"], "en": ["Maple (kaede) refers to trees of the maple genus found in Japan's deciduous broadleaf forests. Many change leaf color with the seasons.","Forests containing maples have been identified near Jomon-period archaeological sites, showing that these trees have long been part of the Japanese archipelago's forests. This plant does not represent any one maple species."]},
    "Plant_TreeOak": {"label_en": "Oak (nara)", "label_ja": "ナラ", "ja": ["楢（ナラ）は、コナラなど日本に分布する落葉性のナラ類を表す樹木。ブナなどとともに落葉広葉樹林の一部をなす。","日本列島では古くからナラ類を含む森林が広がり、地域ごとに人の火の利用や森林利用の影響を受けてきた。ここでは落葉ナラ類の代理とし、常緑カシ類まで同一視するものではない。"], "en": ["Oak (nara) represents deciduous Japanese oaks such as konara. These trees form part of deciduous broadleaf forests alongside Japanese beech and other species.","Forests containing oaks have existed across the Japanese archipelago since ancient times, and regional patterns of fire use and forest management have influenced them. Here this plant represents deciduous oaks, not evergreen kashi oaks."]},
    "Plant_TreeBirch": {"label_en": "Birch (kaba)", "label_ja": "カバノキ", "ja": ["樺（カバ。カバノキ類）は、冷涼な山地にも分布する落葉高木の仲間。亜高山帯にはダケカンバなどが、針葉樹林に接する斜面にも見られる。","この植物は冷温帯のカバノキ類と、亜高山帯のダケカンバに相当する樹木をまとめて表す。実際の種類は地域や標高によって異なる。"], "en": ["Birch (kaba, Japanese birches) refers to deciduous tall trees also found in cool mountain regions. In the subalpine zone, species such as dakekanba grow on slopes adjoining conifer forests.","This plant represents birches of cool-temperate forests as well as trees corresponding to dakekanba in the subalpine zone. The actual species vary by region and elevation."]},
    "Plant_TreePine": {"label_en": "Pine (matsu)", "label_ja": "マツ", "ja": ["松（マツ）は、日本の山地や痩せた土地にも育つ針葉樹の仲間。ここではアカマツなどのマツ類を代表する樹木として扱う。","古代から中世にかけて、地域によっては伐採や燃料利用に伴ってマツ類が増加したことが花粉分析からうかがえる。全国の森林が一律にマツ林だったわけではない。"], "en": ["Pine (matsu) is a type of conifer that also grows in Japan's mountains and on nutrient-poor soils. Here it represents pines such as akamatsu (Japanese red pine).","Pollen analysis suggests that, from ancient through medieval times, pines increased in some regions as trees were felled and wood was gathered for fuel. This does not mean that pine forests covered the Japanese archipelago uniformly."]},
    "Plant_TreeWillow": {"label_en": "Willow (yanagi)", "label_ja": "ヤナギ", "ja": ["柳（ヤナギ）は、川沿いや湿った地面に育つヤナギ類を表す樹木。日本各地の川原では、氾濫や流水の影響を受ける場所にヤナギ林が見られる。","河川沿いの湿地林は、集落が接する低地景観を理解するうえでも重要な自然環境である。この植物は特定のヤナギ一種を表すものではない。"], "en": ["Willow (yanagi) represents willows that grow along rivers and on wet ground. Along Japanese riverbanks, willow woodlands occur where flooding and running water shape the landscape.","Riverside wetland woodlands are also an important natural feature of the lowland landscapes adjoining settlements. This plant does not represent any single willow species."]},
}


class ApprovedVanillaTreeDescriptionsTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.source = SOURCE.read_text(encoding="utf-8")
        cls.quickstart = QUICKSTART.read_text(encoding="utf-8")

    def test_approved_japanese_and_translated_paragraphs_in_documentation(self):
        self.assertIn("6樹木すべて承認済み", self.source)
        for name, parts in APPROVED.items():
            with self.subTest(tree=name):
                section = self.source.split("### " + name + " —", 1)[1].split(
                    "\n### ", 1)[0].split("\n## ", 1)[0]
                self.assertIn("日本語：作者承認済み", section)
                for paragraph in parts["ja"] + parts["en"]:
                    self.assertIn("> " + paragraph, section)
                self.assertEqual(section.count("\n>\n"), 2)

    def test_patch_replaces_only_six_english_labels_and_descriptions(self):
        root = ET.parse(PATCH).getroot()
        self.assertEqual(root.tag, "Patch")
        operations = root.findall("Operation")
        self.assertEqual(len(operations), 2 * len(APPROVED))
        actual = {}
        for op in operations:
            self.assertEqual(op.get("Class"), "PatchOperationReplace")
            xpath = op.findtext("xpath")
            self.assertNotIn(xpath, actual)
            value = op.find("value")
            self.assertIsNotNone(value)
            self.assertEqual([node.tag for node in value], [xpath.rsplit('/', 1)[-1]])
            actual[xpath] = value.findtext(xpath.rsplit("/", 1)[-1])
        expected = {}
        for name, parts in APPROVED.items():
            xpath = '/Defs/ThingDef[defName="' + name + '"]/'
            expected[xpath + "description"] = r"\n\n".join(parts["en"])
            expected[xpath + "label"] = parts["label_en"]
        self.assertEqual(actual, expected)

    def test_japanese_uses_approved_descriptions_and_japanese_tree_labels(self):
        root = ET.parse(LOCALIZATION).getroot()
        self.assertEqual(root.tag, "LanguageData")
        for name, parts in APPROVED.items():
            with self.subTest(tree=name):
                self.assertEqual(len(root.findall(name + ".description")), 1)
                self.assertEqual(
                    root.findtext(name + ".description"),
                    r"\n\n".join(parts["ja"]),
                )
                self.assertEqual(len(root.findall(name + ".label")), 1)
                self.assertEqual(root.findtext(name + ".label"), parts["label_ja"])
        # Existing structural plants use three paragraphs; approved Vanilla
        # trees use two. Both follow the historical-description guidelines.
        for name in ("AMJ_Tree_Shii", "AMJ_Tree_Beech",
                     "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu"):
            self.assertEqual(
                root.findtext(name + ".description").count(r"\n\n"), 2
            )
        for name in APPROVED:
            self.assertEqual(
                root.findtext(name + ".description").count(r"\n\n"), 1
            )

    def test_loaded_tree_quickstart_asserts_exact_en_ja_paragraphs(self):
        warm = self.quickstart.split(
            "public sealed class AMJWarmTemperateTerrainQuickstart", 1
        )[1].split("public sealed class AMJTemperateSwampVegetationQuickstart", 1)[0]
        self.assertIn(
            'result.Assert("Plant_TreeBamboo has approved EN/JA description"',
            warm,
        )
        self.assertIn('DefDatabase<ThingDef>.GetNamedSilentFail("Plant_TreeBamboo")', warm)
        self.assertIn("AssertApprovedVanillaTreeDescription(result", warm)
        self.assertIn('defName + " has approved EN/JA description"', warm)
        for name, parts in APPROVED.items():
            with self.subTest(tree=name):
                self.assertIn(name, warm)
                for paragraph in parts["ja"] + parts["en"]:
                    self.assertIn(paragraph, warm)
        self.assertIn('string.Equals(actual, approvedEnglish', warm)
        self.assertIn('string.Equals(actual, approvedJapanese', warm)
        self.assertIn('string.Equals(tree.label, approvedEnglishLabel', warm)
        self.assertIn('string.Equals(tree.label, approvedJapaneseLabel', warm)
        self.assertIn('string.Equals(bamboo.label, "Bamboo (take)"', warm)
        for parts in APPROVED.values():
            self.assertIn(parts["label_en"], warm)
            self.assertIn(parts["label_ja"], warm)


if __name__ == "__main__":
    unittest.main(verbosity=2)
