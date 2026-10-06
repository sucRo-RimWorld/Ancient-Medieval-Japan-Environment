#!/usr/bin/env python3
"""Regression checks for AMJE art-rule inheritance and routing."""
from __future__ import annotations

import unittest
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def read(path: str) -> str:
    return (ROOT / path).read_text(encoding="utf-8")


class EnvironmentArtRuleStructureTest(unittest.TestCase):
    def test_agents_is_routing_layer(self):
        text = read("AGENTS.md")
        for required in (
            "Core `Docs/ArtStyle.md`",
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

    def test_retexture_generation_assembles_current_rules(self):
        text = read("Docs/GoldenPaths/RetextureGeneration.md")
        self.assertIn("Core `Docs/ArtStyle.md`", text)
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
        self.assertIn("inherit the project-wide invariants in Core `Docs/ArtStyle.md`", text)
        self.assertIn("restrained soft gradient variation is allowed", text)
        self.assertIn("explicit class difference from the flatter Core crop/item budget", text)

    def test_golden_path_index_does_not_claim_frozen_prompt(self):
        text = read("Docs/GoldenPaths/README.md")
        self.assertIn("current Core+Environment style-rule assembly", text)
        self.assertNotIn("shared prompt", text)
        self.assertNotIn("proposal/approval sequence", text)

    def test_texture_pipeline_points_to_shared_fixed_policy_only(self):
        text = read("Docs/GoldenPaths/TextureAssetPipeline.md")
        self.assertIn("Core `Docs/GoldenPaths/FixedImageTemplates.md`", text)
        self.assertNotIn("Same style does not mean identical parts", text)
        self.assertNotIn("zero protected RGBA pixel differences", text)

    def test_superseded_visual_review_statuses_are_reconciled(self):
        text = read("Docs/Coordination.md")
        expected_done = (
            "ENV-010 Sudajii final visual direction",
            "ENV-010 leafy-beech payload recovery and decoded-PNG regression gate (2026-10-05 JST)",
            "ENV-010 Shirabiso source approval and integration (2026-10-05 JST)",
        )
        for heading in expected_done:
            marker = f"### {heading}"
            start = text.find(marker)
            self.assertGreaterEqual(start, 0, heading)
            end = text.find("\n### ", start + len(marker))
            section = text[start:] if end < 0 else text[start:end]
            status_marker = "**Status:** "
            status_start = section.find(status_marker)
            self.assertGreaterEqual(status_start, 0, heading)
            status = section[status_start + len(status_marker):].splitlines()[0]
            self.assertTrue(status.startswith("DONE"), f"{heading}: {status}")


if __name__ == "__main__":
    unittest.main()
