"""Guard links between Environment-owned wetland PlantDefs and live biome pools.

This checks source-level references only. It does not validate biome fertility,
native spawning, loaded Def inheritance, visual assets or actual harvest/Cut Jobs.
The existing weight draft remains author-unapproved and independent of this gate.
"""
from pathlib import Path
import re
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
DEF_ROOT = ROOT / "Defs" / "ThingDefs_Plants"
PATCH = ROOT / "Patches" / "VanillaWetlandVegetation.xml"

WETLAND_SPECIES = {
    "AMJ_Plant_Yoshi": {"TemperateSwamp", "ColdBog"},
    "AMJ_Plant_Suge": {"TemperateSwamp", "ColdBog"},
    "AMJ_Tree_Hannoki": {"TemperateSwamp", "ColdBog"},
    "AMJ_Plant_Mizugoke": {"ColdBog"},
}
BIOMES = ("TemperateSwamp", "ColdBog")
XPATH = re.compile(r'^/Defs/BiomeDef\[defName=["\x27](TemperateSwamp|ColdBog)["\x27]\]/wildPlants$')


def read_owned_plant_defs(xml_sources):
    """Return defName occurrence counts for files supplied as XML text."""
    counts = {}
    for xml in xml_sources:
        tree = ET.fromstring(xml)
        for node in tree.findall("ThingDef"):
            name = node.findtext("defName")
            if name in WETLAND_SPECIES:
                counts[name] = counts.get(name, 0) + 1
    return counts


def read_wetland_references(patch_xml):
    """Read each current whole-pool replacement, not author-unapproved draft."""
    root = ET.fromstring(patch_xml)
    references = {biome: set() for biome in BIOMES}
    seen = set()
    for op in root.findall("./Operation/operations/li"):
        xpath = op.findtext("xpath", "")
        match = XPATH.fullmatch(xpath)
        if not match:
            continue
        biome = match.group(1)
        if biome in seen:
            raise ValueError(f"duplicate wildPlants replacement: {biome}")
        seen.add(biome)
        if op.get("Class") != "PatchOperationReplace":
            raise ValueError(f"unexpected patch operation for {biome}")
        plants = op.find("value/wildPlants")
        if plants is None:
            raise ValueError(f"missing replacement pool: {biome}")
        references[biome] = {
            node.tag for node in plants if node.tag in WETLAND_SPECIES
        }
    if seen != set(BIOMES):
        raise ValueError("expected both Vanilla wetland pool replacements")
    return references


def issues(references, definitions):
    """Check only source linkage and restricted candidate wetland placement."""
    problems = []
    for name, count in definitions.items():
        if name in WETLAND_SPECIES and count != 1:
            problems.append(f"duplicate production PlantDef: {name}")
    active = set()
    for biome in BIOMES:
        for name in references.get(biome, set()):
            if name not in WETLAND_SPECIES:
                continue
            active.add(name)
            if biome not in WETLAND_SPECIES[name]:
                problems.append(f"{name}: not approved for {biome} in current plan")
            if definitions.get(name, 0) != 1:
                problems.append(f"{biome}: missing unique production PlantDef {name}")
    for name in WETLAND_SPECIES:
        if definitions.get(name, 0) and name not in active:
            problems.append(f"{name}: production Def has no owned wetland pool reference")
    return problems


class WetlandProductionReferenceContract(unittest.TestCase):
    def test_valid_staged_fixture_with_one_species(self):
        refs = {"TemperateSwamp": {"AMJ_Plant_Yoshi"}, "ColdBog": set()}
        self.assertEqual([], issues(refs, {"AMJ_Plant_Yoshi": 1}))

    def test_missing_production_def_is_detected(self):
        refs = {"TemperateSwamp": {"AMJ_Plant_Yoshi"}, "ColdBog": set()}
        self.assertIn(
            "TemperateSwamp: missing unique production PlantDef AMJ_Plant_Yoshi",
            issues(refs, {}),
        )

    def test_orphan_def_without_spawn_reference_is_detected(self):
        self.assertIn(
            "AMJ_Plant_Suge: production Def has no owned wetland pool reference",
            issues({"TemperateSwamp": set(), "ColdBog": set()}, {"AMJ_Plant_Suge": 1}),
        )

    def test_mizugoke_cannot_be_added_to_temperate_swamp_by_default(self):
        result = issues(
            {"TemperateSwamp": {"AMJ_Plant_Mizugoke"}, "ColdBog": set()},
            {"AMJ_Plant_Mizugoke": 1},
        )
        self.assertIn(
            "AMJ_Plant_Mizugoke: not approved for TemperateSwamp in current plan",
            result,
        )

    def test_duplicated_production_species_detected(self):
        xml = "<Defs><ThingDef><defName>AMJ_Tree_Hannoki</defName></ThingDef></Defs>"
        self.assertEqual(
            {"AMJ_Tree_Hannoki": 2}, read_owned_plant_defs([xml, xml])
        )
        self.assertIn(
            "duplicate production PlantDef: AMJ_Tree_Hannoki",
            issues({"TemperateSwamp": {"AMJ_Tree_Hannoki"}, "ColdBog": set()},
                   {"AMJ_Tree_Hannoki": 2}),
        )

    def test_real_patch_is_parsed_but_no_unapproved_weights_promoted(self):
        refs = read_wetland_references(PATCH.read_text(encoding="utf-8"))
        self.assertEqual(set(BIOMES), set(refs))
        self.assertTrue(all(names <= set(WETLAND_SPECIES) for names in refs.values()))

    def test_production_references_when_species_are_added(self):
        xml_sources = [
            path.read_text(encoding="utf-8")
            for path in sorted(DEF_ROOT.rglob("*.xml"))
        ]
        defs = read_owned_plant_defs(xml_sources)
        refs = read_wetland_references(PATCH.read_text(encoding="utf-8"))
        if not defs and not any(refs.values()):
            self.skipTest("No new wetland PlantDefs or live pool refs: production pending")
        self.assertEqual([], issues(refs, defs))


if __name__ == "__main__":
    unittest.main()
