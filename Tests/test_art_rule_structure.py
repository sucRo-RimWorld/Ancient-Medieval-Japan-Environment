#!/usr/bin/env python3
"""Regression checks for AMJE art-rule inheritance and routing."""
from __future__ import annotations

import hashlib
import json
import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def read(path: str) -> str:
    return (ROOT / path).read_text(encoding="utf-8")


class EnvironmentArtRuleStructureTest(unittest.TestCase):
    def test_agents_is_routing_layer(self):
        text = read("AGENTS.md")
        for required in (
            "Ancient-Medieval-Japan-Project `Docs/ArtStyle.md`",
            "`Docs/ArtDirection.md`",
            "`Docs/GoldenPaths/RetextureGeneration.md`",
            "`Docs/GoldenPaths/TextureAssetPipeline.md`",
        ):
            self.assertIn(required, text)
        for forbidden in (
            "Sudajii",
            "Japanese beech",
            "complete shared prompt",
            "near-black outer outline",
        ):
            self.assertNotIn(forbidden, text)

    def test_accepted_reference_table_matches_current_blobs(self):
        text = read("Docs/GoldenPaths/RetextureGeneration.md")
        paths = (
            "Textures/Things/Plant/AMJ/Shii/Shii_A.png",
            "Textures/Things/Plant/AMJ/Beech/Beech_A.png",
            "Textures/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png",
        )
        for relative in paths:
            data = (ROOT / relative).read_bytes()
            header = f"blob {len(data)}\0".encode("ascii")
            blob_sha = hashlib.sha1(header + data).hexdigest()
            self.assertIn(f"`{relative}` | `{blob_sha}`", text)

    def test_retexture_generation_assembles_current_rules(self):
        text = read("Docs/GoldenPaths/RetextureGeneration.md")
        self.assertIn("Ancient-Medieval-Japan-Project `Docs/ArtStyle.md`", text)
        self.assertIn("Do not maintain a second frozen style prompt", text)
        self.assertIn("do not add a mandatory extra approval round", text.lower())
        for forbidden in (
            "## Complete shared generation prompt",
            "STYLE:",
            "AVOID:",
            "obtain author approval",
            "proposal **before generation**",
        ):
            self.assertNotIn(forbidden, text)

    def test_art_direction_explicitly_inherits_shared_style(self):
        text = read("Docs/ArtDirection.md")
        self.assertIn("inherit the project-wide invariants in Ancient-Medieval-Japan-Project `Docs/ArtStyle.md`", text)
        self.assertIn("restrained soft gradient variation is allowed", text)
        self.assertIn("explicit class difference from the flatter Core crop/item budget", text)

    def test_golden_path_index_does_not_claim_frozen_prompt(self):
        text = read("Docs/GoldenPaths/README.md")
        self.assertIn("current Project+Environment style-rule assembly", text)
        self.assertNotIn("shared prompt", text)
        self.assertNotIn("proposal/approval sequence", text)

    def test_texture_pipeline_points_to_shared_fixed_policy_only(self):
        text = read("Docs/GoldenPaths/TextureAssetPipeline.md")
        self.assertIn("Ancient-Medieval-Japan-Project `Docs/GoldenPaths/FixedImageTemplates.md`", text)
        self.assertNotIn("Same style does not mean identical parts", text)
        self.assertNotIn("zero protected RGBA pixel differences", text)

    def test_current_visual_review_statuses_are_reconciled(self):
        """Check canonical current state, not deleted DONE headings from compacted history."""
        coordination = read("Docs/Coordination.md")
        marker = "### ENV-010 — existing tree retextures deferred; current plant art accepted"
        self.assertIn(marker, coordination)
        section = coordination.split(marker, 1)[1].split("\n### ", 1)[0]
        self.assertIn(
            "**Status:** CURRENT FOUR STRUCTURAL PLANT VISUALS ACCEPTED; "
            "VANILLA/MO RETEXTURES AUTHOR-DEFERRED",
            section,
        )

        art = read("Docs/ArtDirection.md")
        self.assertIn(
            "All four AMJE species have accepted normal, immature, UI and snow appearances.",
            art,
        )
        ledger = json.loads(read("Docs/PlantVisualCoverage.json"))
        plants = ledger["plants"]
        self.assertEqual(
            {
                "AMJ_Tree_Shii", "AMJ_Tree_Beech",
                "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu",
            },
            set(plants),
        )
        for name, plant in plants.items():
            self.assertIs(plant["complete"], True, name)
            for state in ("normal", "immature", "icon", "snow"):
                entry = plant["states"][state]
                self.assertEqual("accepted", entry["status"], f"{name}/{state}")
                self.assertTrue(entry.get("author_statement"), f"{name}/{state}")
                self.assertTrue(entry.get("fingerprint"), f"{name}/{state}")
        beech = plants["AMJ_Tree_Beech"]["states"]
        self.assertEqual("accepted", beech["leafless"]["status"])
        self.assertEqual("accepted", beech["autumn"]["status"])
        self.assertEqual("verified", beech["transition"]["status"])


if __name__ == "__main__":
    unittest.main()
