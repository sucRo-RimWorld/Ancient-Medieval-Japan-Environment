"""Fast contract gate; loaded-Def and native menu behavior are tested by Quickstarts."""
from pathlib import Path
import re
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
EXPECTED = {
    "AMJ_WarmTemperateForest": {"AMJ_Tree_Shii", "AMJ_Tree_Yamazakura", "Plant_TreeMaple", "Plant_TreeBamboo"},
    "AMJ_CoolTemperateForest": {"AMJ_Tree_Beech", "AMJ_Tree_Yamazakura", "Plant_TreeOak", "Plant_TreeMaple", "Plant_TreeBirch", "Plant_TreePine"},
    "AMJ_SubalpineForest": {"AMJ_Tree_Shirabiso", "Plant_TreeBirch"},
    "AMJ_AlpineZone": set(),
}
# Wild woody distribution and ordinary tree sowing are separate contracts.
EXPECTED_WILD_WOODY = {name: set(trees) for name, trees in EXPECTED.items()}
EXPECTED_WILD_WOODY["AMJ_AlpineZone"] = {"AMJ_Shrub_Haimatsu"}
PARENTS = {"AMJ_Tree_Shii": "TreeBase", "AMJ_Tree_Beech": "DeciduousTreeBase",
           "AMJ_Tree_Yamazakura": "DeciduousTreeBase",
           "AMJ_Tree_Shirabiso": "TreeBase", "AMJ_Shrub_Haimatsu": "BushBase"}


def validate(biomes, plants):
    errors = []
    seen = set()
    for biome in biomes.findall("BiomeDef"):
        name = biome.findtext("defName")
        if name not in EXPECTED:
            continue
        seen.add(name)
        wild = biome.find("wildPlants")
        woody = [p.tag for p in wild if p.tag.startswith(("Plant_Tree", "AMJ_Tree_"))
                 or p.tag == "AMJ_Shrub_Haimatsu"]
        if set(woody) != EXPECTED_WILD_WOODY[name] or len(woody) != len(set(woody)):
            errors.append(f"{name}: natural woody plant set differs from approved contract: {woody}")
        for p in wild:
            if p.tag in EXPECTED_WILD_WOODY[name] and float(p.text) <= 0:
                errors.append(f"{name}/{p.tag}: woody plant must have positive wild commonality")
    if seen != set(EXPECTED):
        errors.append("missing approved biome")
    owned = {p.findtext("defName"): p for p in plants.findall("ThingDef")}
    for name, parent in PARENTS.items():
        p = owned.get(name)
        if p is None:
            errors.append(f"{name}: missing owned plant")
            continue
        if p.get("ParentName") != parent:
            errors.append(f"{name}: sowing inheritance changed")
        # Explicit overrides require a deliberate contract update and loaded-Def review.
        for field in ("sowTags", "sowResearchPrerequisites", "mustBeWildToSow"):
            if p.find(f"plant/{field}") is not None:
                errors.append(f"{name}: unreviewed {field} override")
    return errors


def loaded_wetland_baselines(patch_xml):
    """Read approved live Vanilla and Hannoki additions, never the draft JSON."""
    root = ET.fromstring(patch_xml)
    result = {}
    for op in root.findall("./Operation/operations/li"):
        if op.get("Class") != "PatchOperationAdd":
            continue
        xpath = op.findtext("xpath", "")
        for biome in ("TemperateSwamp", "ColdBog"):
            if xpath == f'/Defs/BiomeDef[defName="{biome}"]/wildPlants':
                if biome in result:
                    raise ValueError("duplicate Vanilla wetland baseline")
                value = op.find("value")
                if value is None or not len(value):
                    raise ValueError("missing Vanilla wetland baseline values")
                result[biome] = {node.tag: node.text for node in value}
    if set(result) != {"TemperateSwamp", "ColdBog"}:
        raise ValueError("missing retained Vanilla wetland baselines")
    return result


class TreeSowingContractTests(unittest.TestCase):
    def setUp(self):
        self.biomes = ET.parse(ROOT / "Defs/BiomeDefs/AMJ_Biomes.xml").getroot()
        self.plants = ET.parse(ROOT / "Defs/ThingDefs_Plants/AMJ_WildPlants.xml").getroot()

    def test_repository_contract(self):
        self.assertEqual([], validate(self.biomes, self.plants))

    def test_removed_tree_reintroduced(self):
        ET.SubElement(self.biomes.find("BiomeDef/wildPlants"), "Plant_TreePoplar").text = "0.5"
        self.assertTrue(validate(self.biomes, self.plants))

    def test_owned_tree_unavailable(self):
        self.biomes.find("BiomeDef/wildPlants/AMJ_Tree_Shii").text = "0"
        self.assertTrue(validate(self.biomes, self.plants))

    def test_regional_gate_bypass(self):
        ET.SubElement(self.plants.find("ThingDef/plant"), "mustBeWildToSow").text = "false"
        self.assertTrue(validate(self.biomes, self.plants))

    def test_haimatsu_accidentally_sowable(self):
        self.plants.findall("ThingDef")[-1].set("ParentName", "TreeBase")
        self.assertTrue(validate(self.biomes, self.plants))

    def test_alpine_haimatsu_must_be_naturally_present(self):
        alpine = next(b for b in self.biomes.findall("BiomeDef")
                      if b.findtext("defName") == "AMJ_AlpineZone")
        alpine.find("wildPlants").remove(alpine.find("wildPlants/AMJ_Shrub_Haimatsu"))
        self.assertTrue(validate(self.biomes, self.plants))

    def test_alpine_haimatsu_must_have_positive_commonality(self):
        alpine = next(b for b in self.biomes.findall("BiomeDef")
                      if b.findtext("defName") == "AMJ_AlpineZone")
        alpine.find("wildPlants/AMJ_Shrub_Haimatsu").text = "0"
        self.assertTrue(validate(self.biomes, self.plants))

    def test_scoped_wetland_baselines_preserve_vanilla_balance(self):
        source = (ROOT / "Patches/VanillaWetlandVegetation.xml").read_text()
        root = ET.fromstring(source)
        ops = root.findall("./Operation/operations/li")
        self.assertEqual(4, len(ops))
        expected = {
            "TemperateSwamp": (7.30, 3.00, {"Plant_TreeWillow", "Plant_TreeMaple"},
                               {"Plant_Chokevine", "Plant_TreeCypress"}),
            "ColdBog": (8.22, 1.80, {"Plant_TreeWillow", "Plant_TreeMaple",
                                      "Plant_TreeBirch"},
                        {"Plant_Chokevine", "Plant_TreeCypress", "Plant_Astragalus"}),
        }
        baselines = loaded_wetland_baselines(source)
        for i, (biome, (total, woody, trees, excluded)) in enumerate(expected.items()):
            remove, add = ops[2*i:2*i+2]
            path = f'/Defs/BiomeDef[defName="{biome}"]/wildPlants'
            self.assertEqual(remove.get("Class"), "PatchOperationConditional")
            self.assertEqual(remove.find("match").get("Class"), "PatchOperationRemove")
            self.assertEqual(remove.findtext("match/xpath"), remove.findtext("xpath"))
            self.assertTrue(remove.findtext("xpath").startswith(path + "/*["))
            self.assertEqual(add.get("Class"), "PatchOperationAdd")
            self.assertEqual(add.findtext("xpath"), path)
            self.assertEqual({x.tag: x.text for x in add.findall("value/*")}, baselines[biome])
            # The removal is limited to exact Vanilla entries; an absent
            # Cypress cannot abort a patch and MO's herbs are never selected.
            targeted = set(re.findall(r"self::(\w+)", remove.findtext("xpath")))
            self.assertEqual(targeted, (set(baselines[biome]) - {"AMJ_Tree_Hannoki"}) | excluded)
            self.assertFalse(any(name.startswith("DankPyon_") for name in targeted))
            weights = {k: float(v) for k, v in baselines[biome].items()}
            self.assertEqual(set(), excluded.intersection(weights))
            self.assertEqual(trees | {"AMJ_Tree_Hannoki"}, {k for k in weights if k.startswith(("Plant_Tree", "AMJ_Tree_"))})
            self.assertAlmostEqual(total, sum(weights.values()))
            self.assertAlmostEqual(woody, sum(v for k, v in weights.items()
                                               if k.startswith(("Plant_Tree", "AMJ_Tree_"))))
            self.assertGreater(weights["Plant_HealrootWild"], 0)
            self.assertNotIn("Plant_Reeds", weights)
            self.assertNotIn("Plant_Bulrush", weights)

    def test_scoped_patch_keeps_mo_herbs_before_or_after_environment(self):
        source = (ROOT / "Patches/VanillaWetlandVegetation.xml").read_text()
        ops = ET.fromstring(source).findall("./Operation/operations/li")
        baseline = loaded_wetland_baselines(source)
        herbs = dict.fromkeys((
            "DankPyon_Plant_MindwortWild", "DankPyon_Plant_PoppyWild",
            "DankPyon_Plant_FleawortWild", "DankPyon_Plant_FlyAgaricWild"), "0.05")
        for i, (biome, approved) in enumerate(baseline.items()):
            targets = set(re.findall(r"self::(\w+)", ops[2*i].findtext("xpath")))
            for mo_before in (True, False):
                # ColdBog/Cypress is deliberately missing: a real earlier
                # prelaunch PatchOperation failure must not recur.
                loaded = {**approved, "Plant_Chokevine": "0.8",
                          "UnrelatedMod_Plant": "0.123"}
                if mo_before:
                    loaded.update(herbs)
                for name in targets:
                    loaded.pop(name, None)
                loaded.update(approved)
                if not mo_before:
                    loaded.update(herbs)
                with self.subTest(biome=biome, mo_before=mo_before):
                    self.assertTrue(all(loaded[k] == v for k, v in herbs.items()))
                    self.assertEqual(loaded["UnrelatedMod_Plant"], "0.123")
                    self.assertTrue(all(loaded[k] == v for k, v in approved.items()))
                    self.assertNotIn("Plant_Chokevine", loaded)

    def test_dlc_free_wetland_baseline_preserves_herbal_medicine(self):
        """Odyssey-only vegetation cannot become a mandatory base-game Def reference."""
        for biome in self.biomes.findall("BiomeDef"):
            wild = biome.find("wildPlants")
            self.assertIsNotNone(wild)
            self.assertGreater(float(wild.findtext("Plant_HealrootWild", "0")), 0)
            self.assertIsNone(wild.find("Plant_Reeds"))
            self.assertIsNone(wild.find("Plant_Bulrush"))
        about = ET.parse(ROOT / "About/About.xml").getroot()
        dependencies = {p.text.lower() for p in about.findall("./modDependencies/li/packageId") if p.text}
        self.assertNotIn("ludeon.rimworld.odyssey", dependencies)


if __name__ == "__main__":
    unittest.main(verbosity=2)
