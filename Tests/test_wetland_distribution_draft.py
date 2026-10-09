"""Check proposed, unapproved wild wetland plant weights without modifying live Defs."""
from copy import deepcopy
from decimal import Decimal
from pathlib import Path
import json
import unittest
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
DRAFT = ROOT / "Docs/Research/WetlandPlantDistributionDraft.json"
PATCH = ROOT / "Patches/VanillaWetlandVegetation.xml"
WOODY_PREFIXES = ("Plant_Tree", "AMJ_Tree_")
ADDITIONS = {
    "TemperateSwamp": {"AMJ_Plant_Yoshi", "AMJ_Plant_Suge", "AMJ_Tree_Hannoki"},
    "ColdBog": {"AMJ_Plant_Yoshi", "AMJ_Plant_Suge", "AMJ_Tree_Hannoki", "AMJ_Plant_Mizugoke"},
}


def loaded_baselines(patch_xml):
    root = ET.fromstring(patch_xml)
    result = {}
    for op in root.findall("./Operation/operations/li"):
        xpath = op.findtext("xpath", "")
        for biome in ADDITIONS:
            if xpath == f'/Defs/BiomeDef[defName="{biome}"]/wildPlants':
                if op.get("Class") != "PatchOperationReplace" or biome in result:
                    raise ValueError("unexpected wetland operation")
                node = op.find("value/wildPlants")
                if node is None:
                    raise ValueError("missing wildPlants")
                result[biome] = {p.tag: p.text for p in node}
    return result


def total(pool, woody=False):
    return sum((Decimal(value) for name, value in pool.items()
                if not woody or name.startswith(WOODY_PREFIXES)), Decimal("0"))


def verify(draft, actual):
    failures = []
    if draft.get("status") != "proposal_not_approved":
        failures.append("proposal approval status has changed unexpectedly")
    if draft.get("source_patch") != "Patches/VanillaWetlandVegetation.xml":
        failures.append("incorrect live source reference")
    old, new = draft.get("baselines", {}), draft.get("proposed", {})
    if old != actual or set(new) != set(ADDITIONS):
        failures.append("baseline patch drift or missing biome")
        return failures
    for biome, additions in ADDITIONS.items():
        before, after = actual[biome], new[biome]
        if set(after) != (set(before) | additions):
            failures.append(f"{biome}: changes not restricted to proposed draft species")
        for key, amount in before.items():
            if (key in ("Plant_HealrootWild", "Plant_Berry", "Plant_Bush", "Plant_Brambles")
                    and Decimal(after.get(key, "0")) != Decimal(amount)):
                failures.append(f"{biome}: required existing species changed {key}")
        if Decimal(after.get("Plant_HealrootWild", "0")) <= 0:
            failures.append(f"{biome}: herbal medicine supply absent")
        for name, value in after.items():
            if Decimal(value) <= 0:
                failures.append(f"{biome}: nonpositive weight {name}")
        if total(before) != total(after):
            failures.append(f"{biome}: overall commonality changed")
        if total(before, woody=True) != total(after, woody=True):
            failures.append(f"{biome}: tree commonality changed")
        if Decimal(before["Plant_TallGrass"]) <= Decimal(after["Plant_TallGrass"]):
            failures.append(f"{biome}: TallGrass proxy was not reduced")
    if Decimal(old["ColdBog"]["Plant_Moss"]) <= Decimal(new["ColdBog"]["Plant_Moss"]):
        failures.append("ColdBog: Moss proxy was not reduced")
    return failures


class WildWetlandDistributionDraftTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.draft = json.loads(DRAFT.read_text(encoding="utf-8"))
        cls.actual = loaded_baselines(PATCH.read_text(encoding="utf-8"))

    def test_proposal_preserves_actual_baseline_and_both_weight_totals(self):
        self.assertEqual([], verify(self.draft, self.actual))
        self.assertEqual(Decimal("7.30"), total(self.draft["proposed"]["TemperateSwamp"]))
        self.assertEqual(Decimal("8.22"), total(self.draft["proposed"]["ColdBog"]))

    def test_unapproved_weight_increase_is_rejected(self):
        changed = deepcopy(self.draft)
        changed["proposed"]["TemperateSwamp"]["AMJ_Plant_Yoshi"] = "1.01"
        self.assertIn("TemperateSwamp: overall commonality changed", verify(changed, self.actual))

    def test_equal_total_cannot_hide_tree_ratio_change(self):
        changed = deepcopy(self.draft)
        changed["proposed"]["ColdBog"]["AMJ_Tree_Hannoki"] = "0.35"
        changed["proposed"]["ColdBog"]["AMJ_Plant_Suge"] = "1.20"
        self.assertIn("ColdBog: tree commonality changed", verify(changed, self.actual))

    def test_existing_wetland_source_drift_is_rejected(self):
        changed = deepcopy(self.actual)
        changed["ColdBog"]["Plant_TallGrass"] = "3.3"
        self.assertIn("baseline patch drift or missing biome", verify(self.draft, changed))


if __name__ == "__main__":
    unittest.main()
