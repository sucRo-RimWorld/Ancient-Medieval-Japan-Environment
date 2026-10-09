"""Static regression for the scoped AMJE wetland implementation-entry decision.

Project's exact validator remains the shared formal policy gate. This local
regression catches source-evidence drift and accidental scope reversal.
"""
from pathlib import Path
import hashlib
import json
import unittest

ROOT = Path(__file__).resolve().parents[1]
RECORD = ROOT / "Docs/Research/WetlandPlantImplementationDecision.json"
EXPECTED_ECOSYSTEMS = {"vanilla_mo", "amj", "ve", "non_ve"}
EXPECTED_PLANTS = {
    "AMJ_Plant_Yoshi", "AMJ_Plant_Suge", "AMJ_Tree_Hannoki",
    "AMJ_Plant_Mizugoke",
}
FIELDS = {
    "function", "historical_fit", "dependencies", "retention",
    "progression_balance", "reuse", "maintenance_license",
}


def errors(record, evidence_by_path):
    findings = []
    if record.get("schema_version") != 1 or record.get("status") != "ready":
        findings.append("unsupported or unready implementation decision")
    if record.get("owner") != "sucRo-RimWorld/Ancient-Medieval-Japan-Environment":
        findings.append("wrong owning repository")
    if record.get("unresolved") != []:
        findings.append("unresolved implementation necessity")
    search = record.get("search", {})
    if not EXPECTED_ECOSYSTEMS.issubset(set(search.get("ecosystems", []))):
        findings.append("missing VE/non-VE/Vanilla+MO/AMJ audit")
    evidence = record.get("evidence", [])
    ids = set()
    for item in evidence:
        path = item.get("path")
        item_id = item.get("id")
        if not item_id or item_id in ids:
            findings.append("duplicate or empty evidence identifier")
        ids.add(item_id)
        data = evidence_by_path.get(path)
        if data is None or hashlib.sha256(data).hexdigest() != item.get("sha256"):
            findings.append("missing or mutated evidence bytes")
    candidates = record.get("candidates", [])
    if not EXPECTED_ECOSYSTEMS.issubset({c.get("ecosystem") for c in candidates}):
        findings.append("uncovered ecosystem candidate")
    for c in candidates:
        if c.get("outcome") not in {"independent", "optional_compatibility"}:
            findings.append("existing Mod cannot cancel a self-contained AMJE species")
        if not c.get("evidence") or any(r not in ids for r in c.get("evidence", [])):
            findings.append("unreferenced candidate evidence")
        comparisons = c.get("comparison", {})
        if any(not isinstance(comparisons.get(k), str) or not comparisons[k].strip()
               for k in FIELDS):
            findings.append("incomplete substantive comparison")
    decision = record.get("decision", {})
    if decision.get("kind") != "independent" or not decision.get("independent_gap"):
        findings.append("standalone plant responsibility was lost")
    if decision.get("specification") != "Docs/NativeVegetationStep3Design-ja.md":
        findings.append("decision points outside the owning specification")
    return findings


class WetlandEntryDecisionTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.record = json.loads(RECORD.read_text(encoding="utf-8"))
        cls.source = {
            item["path"]: (ROOT / item["path"]).read_bytes()
            for item in cls.record["evidence"]
        }

    def test_signed_source_and_scoped_independent_decision(self):
        self.assertEqual([], errors(self.record, self.source))
        text = (ROOT / "Docs/NativeVegetationStep3Design-ja.md").read_text(encoding="utf-8")
        for name in EXPECTED_PLANTS:
            self.assertIn(name, text)
        self.assertIn("Environment自身で実装する", text)

    def test_evidence_tampering_is_rejected(self):
        changed = dict(self.source)
        path = self.record["evidence"][0]["path"]
        changed[path] += b"\n"
        self.assertIn("missing or mutated evidence bytes", errors(self.record, changed))

    def test_ambiguous_external_reuse_is_rejected(self):
        changed = json.loads(json.dumps(self.record))
        changed["decision"]["kind"] = "use_as_is"
        self.assertIn("standalone plant responsibility was lost", errors(changed, self.source))

    def test_missing_non_ve_coverage_is_rejected(self):
        changed = json.loads(json.dumps(self.record))
        changed["search"]["ecosystems"].remove("non_ve")
        self.assertIn("missing VE/non-VE/Vanilla+MO/AMJ audit", errors(changed, self.source))


if __name__ == "__main__":
    unittest.main()
