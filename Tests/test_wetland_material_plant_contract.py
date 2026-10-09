"""Pre-implementation, author-approved Suge/Mizugoke XML behavior contract.

These are STATIC shape checks only. The real rimworld 1.6 loaded-Def/UI,
CutPlant and MO drying outcomes still require native runtime tests. Until
PlantDefs exist, explicit fixture tests run and the production case skips.
"""
from __future__ import annotations

from pathlib import Path
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
PLANT_ROOT = ROOT / "Defs" / "ThingDefs_Plants"
SUGE = "AMJ_Plant_Suge"
MIZUGOKE = "AMJ_Plant_Mizugoke"

# Literal only: fixture descriptions and numbers are not approved content.
# Their purpose is to exercise the schema checker and negative mutations.
FIXTURE = """
<Defs>
  <ThingDef ParentName="PlantBase">
    <defName>AMJ_Plant_Suge</defName>
    <selectable>true</selectable>
    <plant>
      <harvestedThingDef>Hay</harvestedThingDef>
      <harvestYield>1</harvestYield>
    </plant>
  </ThingDef>
  <ThingDef ParentName="PlantBase">
    <defName>AMJ_Plant_Mizugoke</defName>
    <selectable>true</selectable>
    <plant />
  </ThingDef>
</Defs>
"""

DISALLOWED_SOW_FIELDS = ("sowTags", "sowResearchPrerequisites", "sowMinSkill")
DISALLOWED_MOSS_HARVEST = (
    "harvestedThingDef", "harvestYield", "harvestTag",
    "harvestMinGrowth", "harvestFailable",
)


def inspect_plant(definition: ET.Element, species: str) -> list[str]:
    """Def-local structural checks; inherited fields need separate loaded audit."""
    problems = []
    if definition.findtext("defName") != species:
        return [f"{species}: wrong defName"]
    if definition.findtext("selectable") != "true":
        problems.append(f"{species}: must be selectable/inspectable")
    if definition.get("ParentName") != "PlantBase":
        # A parent change could inherit yields or sow tags from Tree/Bush.
        # Re-evaluate the inherited loaded state before allowing the change.
        problems.append(f"{species}: parent inheritance not audited")
    plant = definition.find("plant")
    if plant is None:
        return problems + [f"{species}: plant properties missing"]
    for field in DISALLOWED_SOW_FIELDS:
        if plant.find(field) is not None:
            problems.append(f"{species}: wild-only species must not add {field}")
    if species == SUGE:
        if plant.findtext("harvestedThingDef") != "Hay":
            problems.append("Suge: primary output must be Vanilla Hay")
        if plant.find("harvestYield") is None:
            problems.append("Suge: output requires a positive harvestYield")
        else:
            try:
                if float(plant.findtext("harvestYield")) <= 0:
                    problems.append("Suge: output requires a positive harvestYield")
            except (ValueError, TypeError):
                problems.append("Suge: invalid harvestYield")
        if "DankPyon" in ET.tostring(definition, encoding="unicode"):
            problems.append("Suge: no unconditional Medieval Overhaul item reference")
    elif species == MIZUGOKE:
        for field in DISALLOWED_MOSS_HARVEST:
            if plant.find(field) is not None:
                problems.append(f"Mizugoke: no harvest field {field}")
        # Do not install direct resource drops through alternative ThingDef fields.
        for field in ("butcherProducts", "smeltProducts", "mineableYield", "products"):
            if definition.find(field) is not None:
                problems.append(f"Mizugoke: item drop cannot use {field}")
        if definition.findtext("destroyable") == "false":
            problems.append("Mizugoke: cannot make normal removal impossible")
    else:
        problems.append(f"Untracked wetland material species: {species}")
    return problems


def find_current_defs() -> dict[str, ET.Element]:
    found = {}
    for path in sorted(PLANT_ROOT.rglob("*.xml")):
        root = ET.parse(path).getroot()
        for definition in root.findall("ThingDef"):
            name = definition.findtext("defName")
            if name in (SUGE, MIZUGOKE):
                if name in found:
                    raise AssertionError(f"duplicate production PlantDef: {name}")
                found[name] = definition
    return found


class WetlandMaterialPlantContract(unittest.TestCase):
    def fixtures(self):
        root = ET.fromstring(FIXTURE)
        return {t.findtext("defName"): t for t in root.findall("ThingDef")}

    def test_approved_fixture_shapes(self):
        definitions = self.fixtures()
        self.assertEqual([], inspect_plant(definitions[SUGE], SUGE))
        self.assertEqual([], inspect_plant(definitions[MIZUGOKE], MIZUGOKE))

    def test_suge_must_yield_vanilla_hay_not_raw_mo_straw(self):
        definition = self.fixtures()[SUGE]
        definition.find("plant/harvestedThingDef").text = "DankPyon_Straw"
        self.assertIn(
            "Suge: primary output must be Vanilla Hay",
            inspect_plant(definition, SUGE),
        )
        self.assertIn(
            "Suge: no unconditional Medieval Overhaul item reference",
            inspect_plant(definition, SUGE),
        )

    def test_suge_must_not_be_unharvestable(self):
        definition = self.fixtures()[SUGE]
        definition.find("plant").remove(definition.find("plant/harvestedThingDef"))
        self.assertIn(
            "Suge: primary output must be Vanilla Hay",
            inspect_plant(definition, SUGE),
        )

    def test_mizugoke_no_harvest_even_if_yield_zero(self):
        definition = self.fixtures()[MIZUGOKE]
        ET.SubElement(definition.find("plant"), "harvestedThingDef").text = "Hay"
        ET.SubElement(definition.find("plant"), "harvestYield").text = "0"
        self.assertIn(
            "Mizugoke: no harvest field harvestedThingDef",
            inspect_plant(definition, MIZUGOKE),
        )
        self.assertIn(
            "Mizugoke: no harvest field harvestYield",
            inspect_plant(definition, MIZUGOKE),
        )

    def test_mizugoke_remains_selectable_and_cuttable(self):
        definition = self.fixtures()[MIZUGOKE]
        definition.find("selectable").text = "false"
        ET.SubElement(definition, "destroyable").text = "false"
        problems = inspect_plant(definition, MIZUGOKE)
        self.assertIn("AMJ_Plant_Mizugoke: must be selectable/inspectable", problems)
        self.assertIn("Mizugoke: cannot make normal removal impossible", problems)

    def test_mizugoke_does_not_become_sowable(self):
        definition = self.fixtures()[MIZUGOKE]
        ET.SubElement(definition.find("plant"), "sowTags").text = "Ground"
        self.assertIn(
            "AMJ_Plant_Mizugoke: wild-only species must not add sowTags",
            inspect_plant(definition, MIZUGOKE),
        )

    def test_unreviewed_parent_change_is_detected(self):
        definition = self.fixtures()[MIZUGOKE]
        definition.set("ParentName", "BushBase")
        self.assertIn(
            "AMJ_Plant_Mizugoke: parent inheritance not audited",
            inspect_plant(definition, MIZUGOKE),
        )

    def test_real_defs_when_present_but_do_not_fake_implementation(self):
        actual = find_current_defs()
        if not actual:
            self.skipTest(
                "No production Suge/Mizugoke PlantDefs yet: author text/art "
                "and native-runtime acceptance remain pending"
            )
        for name, definition in actual.items():
            with self.subTest(name=name):
                self.assertEqual([], inspect_plant(definition, name))


if __name__ == "__main__":
    unittest.main()
