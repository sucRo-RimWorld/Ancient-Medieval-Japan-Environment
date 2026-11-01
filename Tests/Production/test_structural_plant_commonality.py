"""Regression contract for AMJE structural-plant commonalities.

After the Vanilla retention audit, warm/subalpine/alpine plant weights were
redistributed to preserve woodland cover. Validate the production XML values.
"""
from pathlib import Path
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
BIOME_XML = ROOT / "Defs" / "BiomeDefs" / "AMJ_Biomes.xml"

APPROVED = {
    ("AMJ_WarmTemperateForest", "AMJ_Tree_Shii"): 2.55,
    ("AMJ_CoolTemperateForest", "AMJ_Tree_Beech"): 1.8,
    ("AMJ_SubalpineForest", "AMJ_Tree_Shirabiso"): 3.5,
    ("AMJ_CoolTemperateForest", "AMJ_Tree_Shirakaba"): 0.6,
    ("AMJ_SubalpineForest", "AMJ_Tree_Dakekanba"): 0.5,
    ("AMJ_AlpineZone", "AMJ_Shrub_Haimatsu"): 1.34,
}

class StructuralPlantCommonalityTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.root = ET.parse(BIOME_XML).getroot()

    def test_production_xml_matches_approved_balance(self):
        for (biome, plant), expected in APPROVED.items():
            with self.subTest(biome=biome, plant=plant):
                nodes = self.root.findall(
                    f"./BiomeDef[defName='{biome}']/wildPlants/{plant}"
                )
                self.assertEqual(len(nodes), 1)
                self.assertAlmostEqual(float(nodes[0].text), expected, places=6)


if __name__ == "__main__":
    unittest.main(verbosity=2)
