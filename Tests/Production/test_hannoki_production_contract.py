"""Production Hannoki contracts: XML source links only, never a native-game PASS.

2026-10-13 author-approved values: 20 growth days, 24 WoodLog,
800 harvest work, Beauty 2, wetland weights 0.70/0.45,
unchanged full/woody baseline totals; CCTO 5 C and dormancy.
"""
from __future__ import annotations

from pathlib import Path
import hashlib
import json
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
DEF_NAME = "AMJ_Tree_Hannoki"
PLANTS = ROOT / "Defs/ThingDefs_Plants/AMJ_WildPlants.xml"
WETLANDS = ROOT / "Patches/VanillaWetlandVegetation.xml"
CCTO = ROOT / "Patches/Compatibility/CCTO.xml"
JA = ROOT / "Languages/Japanese/DefInjected/ThingDef/AMJ_WildPlants.xml"
LEDGER = ROOT / "Docs/PlantVisualCoverage.json"
SNOW_HASHES = {
    "Hannoki/Hannoki_A.png": "6594360cca125386b5ff1033b7e376a23fc602be120e6c096b18244d6fedced1",
    "Hannoki_Snow/Hannoki_Snow_A.png": "ecc2c0412e030eb19f3f45ab016aaed94a3b1e7b37f66d6db8375f573651df16",
    "Hannoki_Leafless/Hannoki_Leafless_A.png": "dbdd69bc98f21d294c2a083b12225de55392eefc48c03164b103b82176155003",
    "LeaflessNoSnow/LeaflessNoSnow_A.png": "70a213096a8a3aa52d8b05ee5ff5f8d14305a01d51f9305d2555880a4a2b458c",
}
WOODY = {
    "TemperateSwamp": {"Plant_TreeWillow": 1.30, "Plant_TreeMaple": 1.00, DEF_NAME: 0.70},
    "ColdBog": {"Plant_TreeWillow": 0.45, "Plant_TreeBirch": 0.45,
                "Plant_TreeMaple": 0.45, DEF_NAME: 0.45},
}
BASE_TOTAL = {"TemperateSwamp": 7.30, "ColdBog": 8.22}
WOODY_TOTAL = {"TemperateSwamp": 3.00, "ColdBog": 1.80}


class HannokiProductionContract(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.root = ET.parse(PLANTS).getroot()
        defs = cls.root.findall(f"./ThingDef[defName='{DEF_NAME}']")
        if len(defs) != 1:
            raise AssertionError("Hannoki must have exactly one production Def")
        cls.tree = defs[0]
        cls.patch = ET.parse(WETLANDS).getroot()
        cls.ops = cls.patch.findall("./Operation/operations/li")

    def test_no_new_unapproved_sow_or_image_behavior(self):
        t = self.tree
        self.assertEqual(t.get("ParentName"), "DeciduousTreeBase")
        self.assertEqual(t.findtext("graphicData/texPath"), "Things/Plant/AMJ/Hannoki")
        self.assertEqual(t.findtext("graphicData/graphicClass"), "Graphic_Random")
        self.assertIsNone(t.find("plant/immatureGraphicPath"))
        for field in ("sowTags", "sowResearchPrerequisites", "mustBeWildToSow"):
            self.assertIsNone(t.find("plant/" + field), field)
        for prop, expected in (("growDays", "20"), ("minGrowthTemperature", "0"),
                               ("harvestedThingDef", "WoodLog"), ("harvestTag", "Wood"),
                               ("harvestWork", "800"), ("harvestYield", "24")):
            self.assertEqual(t.findtext("plant/" + prop), expected)
        self.assertEqual(t.findtext("statBases/Beauty"), "2")

    def test_exact_accepted_seasonal_pngs_and_paths(self):
        paths = dict(leaflessGraphicPath="Hannoki_Leafless",
                     snowOverlayGraphicPath="Hannoki_Snow",
                     leaflessSnowOverlayGraphicPath="LeaflessNoSnow")
        for prop, stem in paths.items():
            self.assertEqual(self.tree.findtext("plant/" + prop),
                             "Things/Plant/AMJ/" + stem)
        for rel, expected in SNOW_HASHES.items():
            png = ROOT / "Textures/Things/Plant/AMJ" / rel
            self.assertTrue(png.is_file(), str(png))
            self.assertEqual(hashlib.sha256(png.read_bytes()).hexdigest(), expected)

    def test_wetland_weights_transfer_not_add_biomass(self):
        self.assertEqual(len(self.ops), 4)
        for n, biome in enumerate(WOODY):
            remove, add = self.ops[2*n:2*n+2]
            self.assertEqual(remove.get("Class"), "PatchOperationConditional")
            self.assertEqual(add.get("Class"), "PatchOperationAdd")
            self.assertEqual(add.findtext("xpath"),
                             f'/Defs/BiomeDef[defName="{biome}"]/wildPlants')
            rows = add.findall("value/*")
            self.assertEqual(len(rows), len({x.tag for x in rows}))
            actual = {x.tag: float(x.text) for x in rows}
            expected = WOODY[biome]
            self.assertEqual({x: actual[x] for x in expected}, expected)
            self.assertAlmostEqual(sum(actual.values()), BASE_TOTAL[biome])
            self.assertAlmostEqual(
                sum(v for k, v in actual.items()
                    if k.startswith(("Plant_Tree", "AMJ_Tree_"))), WOODY_TOTAL[biome])
            self.assertGreater(actual["Plant_HealrootWild"], 0)

    def test_native_temperature_and_optional_ccto(self):
        ops = ET.parse(CCTO).getroot().findall("./Operation/operations/li")
        ours = [op for op in ops if DEF_NAME in (op.findtext("xpath") or "")]
        self.assertEqual(len(ours), 2)
        by_kind = {op.get("Class"): op for op in ours}
        self.assertEqual(set(by_kind), {"PatchOperationReplace", "PatchOperationAddModExtension"})
        for op in ours:
            self.assertEqual(op.get("MayRequire"), "sucro.cropcoldtoleranceoverhaul")
        self.assertEqual(by_kind["PatchOperationReplace"].findtext("value/minGrowthTemperature"), "5")
        exts = by_kind["PatchOperationAddModExtension"].findall("value/li")
        self.assertEqual(len(exts), 1)
        self.assertEqual(exts[0].get("Class"), "CropColdToleranceOverhaul.ColdToleranceExtension")
        self.assertEqual(exts[0].findtext("coldDormancy"), "true")

    def test_approved_display_text_and_review_not_yet_closed(self):
        ja = ET.parse(JA).getroot()
        self.assertEqual(ja.findtext(DEF_NAME + ".label"), "ハンノキ")
        self.assertEqual(len(ja.findall(DEF_NAME + ".description")), 1)
        description = ja.findtext(DEF_NAME + ".description")
        self.assertTrue(description.startswith("榛の木（ハンノキ）は"))
        self.assertIn("縄文時代の関東地方の谷にはハンノキ属", description)
        self.assertEqual(description.count(chr(92) + "n" + chr(92) + "n"), 1)
        self.assertEqual(self.tree.findtext("description").count(chr(92) + "n" + chr(92) + "n"), 1)
        data = json.loads(LEDGER.read_text(encoding="utf-8"))["plants"][DEF_NAME]
        self.assertIs(data["complete"], False)
        self.assertEqual(set(data["states"]),
                         {"normal", "immature", "icon", "snow", "leafless", "autumn", "transition"})
        self.assertTrue(any(x["status"] == "pending" for x in data["states"].values()))


if __name__ == "__main__":
    unittest.main()
