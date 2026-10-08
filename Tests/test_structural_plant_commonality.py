"""Regression contract for AMJE structural-plant commonalities.

After the Vanilla retention audit, warm/subalpine/alpine plant weights were
redistributed to preserve woodland cover. The installed-source PowerShell gate
must agree with the production XML instead of checking obsolete literal markers.
"""
from pathlib import Path
import re
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
BIOME_XML = ROOT / "Defs" / "BiomeDefs" / "AMJ_Biomes.xml"
VALIDATOR = ROOT / "Scripts" / "Validate-Environment.ps1"

APPROVED = {
    ("AMJ_WarmTemperateForest", "AMJ_Tree_Shii"): 2.55,
    ("AMJ_CoolTemperateForest", "AMJ_Tree_Beech"): 1.8,
    ("AMJ_SubalpineForest", "AMJ_Tree_Shirabiso"): 3.5,
    ("AMJ_AlpineZone", "AMJ_Shrub_Haimatsu"): 1.34,
}

CONTRACT_ENTRY = re.compile(
    r"@\{\s*Biome\s*=\s*'([^']+)'\s*;\s*"
    r"Plant\s*=\s*'([^']+)'\s*;\s*"
    r"Commonality\s*=\s*(\d+(?:\.\d+)?)\s*\}"
)


class StructuralPlantCommonalityTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.root = ET.parse(BIOME_XML).getroot()
        cls.validator = VALIDATOR.read_text(encoding="utf-8")

    def test_production_xml_matches_approved_balance(self):
        for (biome, plant), expected in APPROVED.items():
            with self.subTest(biome=biome, plant=plant):
                nodes = self.root.findall(
                    f"./BiomeDef[defName='{biome}']/wildPlants/{plant}"
                )
                self.assertEqual(len(nodes), 1)
                self.assertAlmostEqual(float(nodes[0].text), expected, places=6)

    def test_powershell_gate_matches_production_balance(self):
        entries = CONTRACT_ENTRY.findall(self.validator)
        actual = {(biome, plant): float(value) for biome, plant, value in entries}
        self.assertEqual(len(entries), len(APPROVED))
        self.assertEqual(actual, APPROVED)
        self.assertIn('$biomeDefs.SelectNodes($xpath)', self.validator)
        self.assertIn('wildPlants/$($entry.Plant)', self.validator)
        self.assertNotIn(
            'AMJ biome wild-plant composition is missing expected marker:',
            self.validator,
        )


if __name__ == "__main__":
    unittest.main(verbosity=2)
