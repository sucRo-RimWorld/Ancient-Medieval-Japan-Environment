#!/usr/bin/env python3
"""Offline guard: release branch must not retain owner-hosted Steam jobs.

The only privileged publisher is the main-pinned private Project workflow;
the Environment repository retains owner release sources/build scripts only.
No external API, game launch, credentials or Steam mutation.
"""
import json
from pathlib import Path
from xml.etree import ElementTree as ET

ROOT = Path(__file__).resolve().parents[2]
for forbidden in (
    ".github/workflows/steam-approved.yml",
    ".github/workflows/steam-readiness.yml",
):
    assert not (ROOT / forbidden).exists(), (
        "Owner-specific Steam host job must be retired: " + forbidden
    )

doc = (ROOT / "Docs/GoldenPaths/WorkshopPublication.md").read_text("utf-8")
assert "sucRo-RimWorld/Ancient-Medieval-Japan-Project/.github/workflows/steam-central.yml" in doc
assert "steam-workshop-private" in doc
cfg = json.loads((ROOT / "Publishing/SteamWorkshop.json").read_text("utf-8"))
assert cfg["repository"] == "sucRo-RimWorld/Ancient-Medieval-Japan-Environment"
assert cfg["workshop_id"] == "3814638060"
assert cfg["visibility_policy"] == "preserve_public"
about = ET.parse(ROOT / "About/About.xml").getroot()
assert about.findtext("packageId") == cfg["package_id"]
assert about.findtext("modVersion") == "0.2.0"
pin = json.loads((ROOT / "Publishing/VerifiedAssembly.json").read_text("utf-8"))
assert pin["assembly_sha256"] == "b2ddbf7888322ba9b207c20296dba31175b846c05492902b900ec225da9fcffe"
assert len(pin["transfer_commit"]) == 40
print("PASS owner-only metadata retained; legacy publisher jobs absent; Project central owner HOLD")
