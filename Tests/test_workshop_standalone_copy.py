"""Prevent stale Workshop copy requirements after public-description edits."""
from pathlib import Path
import re
import unittest

ROOT = Path(__file__).resolve().parents[1]

JA_REQUIRED = "必須MOD: Harmony"
EN_REQUIRED = "Required: Harmony"
JA_CORE_OLD = "Ancient & Medieval Japan Coreは不要です。"
EN_CORE_OLD = "Ancient & Medieval Japan Core is not required."


def workshop_text(name):
    source = (ROOT / "Docs" / name).read_text(encoding="utf-8")
    visible = re.sub(r"\[(?:/?url[^\]]*|/?b)\]", "", source)
    return source, visible


class WorkshopStandaloneContractTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.validator = (
            ROOT / "Scripts" / "Validate-Environment.ps1"
        ).read_text(encoding="utf-8")
        cls.ja_raw, cls.ja = workshop_text("SteamWorkshopDescription-ja.txt")
        cls.en_raw, cls.en = workshop_text("SteamWorkshopDescription.txt")

    def test_current_required_and_optional_compatibility_copy(self):
        for name, content, required in (
            ("Japanese", self.ja, (
                "RimWorld 1.6対応、β版。",
                JA_REQUIRED,
                "任意MOD: CCTO",
            )),
            ("English", self.en, (
                "RimWorld 1.6, Beta.",
                EN_REQUIRED,
                "Optional: Crop Cold Tolerance Overhaul (CCTO)",
            )),
        ):
            with self.subTest(language=name):
                for phrase in required:
                    self.assertIn(phrase, content)

    def test_intentionally_removed_core_statement_is_not_required(self):
        self.assertNotIn(JA_CORE_OLD, self.ja)
        self.assertNotIn(EN_CORE_OLD, self.en)
        self.assertNotIn(
            "Japanese Workshop Core independence", self.validator
        )
        self.assertNotIn(
            "English Workshop Core independence", self.validator
        )

    def test_validator_matches_actual_workshop_required_mod_copy(self):
        self.assertIn(
            "@($workshopJaText, '" + JA_REQUIRED +
            "', 'Japanese Workshop required Harmony')",
            self.validator,
        )
        self.assertIn(
            "@($workshopEnText, '" + EN_REQUIRED +
            "', 'English Workshop required Harmony')",
            self.validator,
        )

    def test_readme_and_about_keep_standalone_scope(self):
        readme = (ROOT / "README.md").read_text(encoding="utf-8")
        about = (ROOT / "About" / "About.xml").read_text(encoding="utf-8")
        self.assertIn(
            "Ancient & Medieval Japan Core", readme
        )
        self.assertIn("is not required.", readme)
        self.assertIn(
            "Ancient &amp; Medieval Japan Core is not required.", about
        )

    def test_crlf_workshop_size_limit(self):
        for name, raw in (("Japanese", self.ja_raw), ("English", self.en_raw)):
            with self.subTest(language=name):
                encoded = raw.replace("\r\n", "\n").replace(
                    "\n", "\r\n"
                ).encode("utf-8")
                self.assertLessEqual(len(encoded), 8000)


if __name__ == "__main__":
    unittest.main(verbosity=2)
