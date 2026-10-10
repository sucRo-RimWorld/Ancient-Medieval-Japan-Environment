"""Source contract for the selective Plant.Print mirror filter.

This is a static guard only. It cannot establish Harmony IL compatibility,
actual atlas UV orientation, or rendered normal/snow appearance in RimWorld.
"""
from pathlib import Path
import re
import unittest

SOURCE = Path(__file__).resolve().parents[1] / "Source" / "AncientMedievalJapanEnvironment" / "PlantFacingPatches.cs"


def validate_patch(source):
    problems = []
    if not re.search(r'\[HarmonyPatch\(typeof\(Plant\),\s*nameof\(Plant\.Print\)\)\]', source):
        problems.append("Patch target must be Plant.Print")
    if not re.search(r'const string AshiDefName\s*=\s*"AMJ_Plant_Yoshi"', source):
        problems.append("Ashi name missing")
    if not re.search(r'const string SusukiDefName\s*=\s*"AMJ_Plant_Susuki"', source):
        problems.append("Susuki name missing")
    if "source.Count(code => code.Calls(RandomFlipGetter))" not in source or "flipSites != 1" not in source:
        problems.append("Must reject missing/ambiguous Rand.Bool locations")
    if "result.Add(instruction);" not in source or "instruction.Calls(RandomFlipGetter)" not in source:
        problems.append("Must preserve original Rand.Bool evaluation")
    if not re.search(r'OpCodes\.Ldarg_0.*?OpCodes\.Call,\s*FilterMethod', source, re.S):
        problems.append("Must pass current Plant to bool filter")
    if not re.search(r'defName == AshiDefName\s*\|\|\s*defName == SusukiDefName', source):
        problems.append("Filter must be scoped to the two intended species")
    if not re.search(r'if\s*\(defName == AshiDefName.*?\)\s*\{\s*return false;\s*\}\s*return vanillaFlip;', source, re.S):
        problems.append("All other species must keep the original flip")
    return problems


class PlantDirectionContractTests(unittest.TestCase):
    def test_production_source_contract(self):
        self.assertEqual([], validate_patch(SOURCE.read_text(encoding="utf-8")))

    def test_wrong_species_scope_rejected(self):
        source = SOURCE.read_text(encoding="utf-8").replace(
            '"AMJ_Plant_Yoshi"', '"Plant_TallGrass"'
        )
        self.assertIn("Ashi name missing", validate_patch(source))

    def test_random_call_removal_rejected(self):
        source = SOURCE.read_text(encoding="utf-8").replace(
            "result.Add(instruction);", "// removed original call"
        )
        self.assertIn("Must preserve original Rand.Bool evaluation", validate_patch(source))

    def test_unrelated_species_mirroring_rejected(self):
        source = SOURCE.read_text(encoding="utf-8").replace(
            "return vanillaFlip;", "return false;"
        )
        self.assertIn("All other species must keep the original flip", validate_patch(source))


if __name__ == "__main__":
    unittest.main()
