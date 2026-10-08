"""Fast contract gate; loaded-Def and native menu behavior are tested by Quickstarts."""
from pathlib import Path
import re
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
EXPECTED = {
    "AMJ_WarmTemperateForest": {"AMJ_Tree_Shii", "Plant_TreeMaple", "Plant_TreeBamboo"},
    "AMJ_CoolTemperateForest": {"AMJ_Tree_Beech", "Plant_TreeOak", "Plant_TreeMaple", "Plant_TreeBirch", "Plant_TreePine"},
    "AMJ_SubalpineForest": {"AMJ_Tree_Shirabiso", "Plant_TreeBirch"},
    "AMJ_AlpineZone": set(),
}
PARENTS = {"AMJ_Tree_Shii": "TreeBase", "AMJ_Tree_Beech": "DeciduousTreeBase",
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
        trees = [p.tag for p in wild if p.tag.startswith(("Plant_Tree", "AMJ_Tree_"))]
        if set(trees) != EXPECTED[name] or len(trees) != len(set(trees)):
            errors.append(f"{name}: regional tree set differs from approved contract: {trees}")
        for p in wild:
            if p.tag in EXPECTED[name] and float(p.text) <= 0:
                errors.append(f"{name}/{p.tag}: tree must have positive wild commonality")
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

    def test_approved_wetland_pools_are_atomic_and_balanced(self):
        wetland = ET.parse(ROOT / "Patches/VanillaWetlandVegetation.xml").getroot()
        ops = wetland.findall("./Operation/operations/li")
        self.assertEqual(2, len(ops))
        expected = {
            "TemperateSwamp": (7.30, 3.00, {"Plant_TreeWillow", "Plant_TreeMaple"}),
            "ColdBog": (8.22, 1.80, {"Plant_TreeWillow", "Plant_TreeMaple", "Plant_TreeBirch"}),
        }
        for op, (biome, (total, woody, trees)) in zip(ops, expected.items()):
            self.assertEqual("PatchOperationReplace", op.get("Class"))
            self.assertEqual(
                f'/Defs/BiomeDef[defName="{biome}"]/wildPlants',
                op.findtext("xpath"),
            )
            plants = op.findall("value/wildPlants/*")
            self.assertTrue(plants)
            self.assertEqual(len(plants), len({p.tag for p in plants}))
            weights = {p.tag: float(p.text) for p in plants}
            self.assertFalse(
                {"Plant_Chokevine", "Plant_TreeCypress", "Plant_Astragalus"} & weights.keys()
            )
            self.assertAlmostEqual(total, sum(weights.values()))
            self.assertAlmostEqual(
                woody, sum(w for name, w in weights.items() if name.startswith("Plant_Tree"))
            )
            self.assertEqual(trees, {n for n in weights if n.startswith("Plant_Tree")})

    def test_runtime_expectations_and_native_menu_gate_are_wired(self):
        source = (ROOT / "Tests/Quickstarts/EnvironmentBiomeTerrainQuickstarts.cs").read_text()
        method = source.split("private static string[] ExpectedSowableTrees", 1)[1].split(
            "private static HashSet<string>", 1)[0]
        for name, expected in EXPECTED.items():
            case = method.split(f'case "{name}":', 1)[1].split("case ", 1)[0].split("default:", 1)[0]
            self.assertEqual(expected, set(re.findall(r'"((?:AMJ|Plant)_\w+)"', case)))
        self.assertIn("foreach (BiomePlantRecord record in map.Biome.wildPlants)", source)
        for required in ("AddTreeSowingAssertions(verification, map);",
                         "PlantUtility.ValidPlantTypesForGrowers(",
                         "Command_SetPlantToGrow.IsPlantAvailable(plant, map)",
                         "progress[research] = 0f;", "progress[research] = research.Cost;",
                         "finally", "progress.Clear();", "progress.Add(item.Key, item.Value);"):
            self.assertIn(required, source)


if __name__ == "__main__":
    unittest.main(verbosity=2)
