#!/usr/bin/env python3
"""Ensure Environment's only paste-ready Workshop text is bilingual."""
from pathlib import Path
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
TITLE = "Ancient & Medieval Japan - Environment（中世日本 - 環境）"
BODY = ROOT / "Docs/SteamWorkshopDescription.txt"
JA_SOURCE = ROOT / "Docs/SteamWorkshopDescription-ja.txt"


def require(ok, message):
    if not ok:
        raise SystemExit("FAIL: " + message)


body = BODY.read_text(encoding="utf-8")
ja_source = JA_SOURCE.read_text(encoding="utf-8").strip()
about = ET.parse(ROOT / "About/About.xml").getroot()
require(about.findtext("name") == TITLE, "About name differs from Workshop title")
require(body.startswith("[h1]" + TITLE + "[/h1]"), "Workshop title differs from About name")
require(body.count(TITLE) == 1, "Workshop title must appear once")
ja_marker = "[h2]日本語 / Japanese[/h2]"
gallery_marker = "[h2]Representative Plants / 各環境帯の代表植物[/h2]"
require(body.count(ja_marker) == 1 and body.count(gallery_marker) == 1,
        "missing or repeated language/gallery section")
require(body.count("[hr][/hr]") == 1 and "\n\n[hr][/hr]\n\n" + ja_marker in body,
        "English/Japanese boundary requires one Steam horizontal rule")
ja_pos = body.index(ja_marker)
gallery_pos = body.index(gallery_marker)
require(ja_pos > 0 and gallery_pos > ja_pos, "expected English, Japanese, shared gallery order")
require(body[ja_pos:gallery_pos].strip() == ja_source,
        "combined Japanese section differs from maintained Japanese source")
require("[h2]Features[/h2]" in body[:ja_pos] and
        "[h2]主な特徴[/h2]" in body[ja_pos:gallery_pos],
        "one language is missing its feature section")
for term in ("[h2]Save compatibility[/h2]", "[h2]セーブ互換性[/h2]",
             "Harmony", "Crop Cold Tolerance Overhaul", "Medieval Overhaul"):
    require(term in body, "required public-installation detail missing: " + term)
require(len(body.encode("utf-8")) <= 8000, "combined description exceeds 8000 UTF-8 bytes")
require(len(body.replace("\n", "\r\n").encode("utf-8")) <= 8000,
        "CRLF copy exceeds 8000 UTF-8 bytes")

images = re.findall(r"\[img\](.*?)\[/img\]", body)
require(len(images) == 4 and len(set(images)) == 4,
        "exactly four unique representative-plant images are required")
for plant in ("Shii/Shii_A.png", "Beech/Beech_A.png",
              "Shirabiso/Shirabiso_A.png", "Haimatsu/Haimatsu_A.png"):
    require(sum(path.endswith(plant) for path in images) == 1,
            "missing or repeated image: " + plant)
for label in ("Warm-temperate — Sudajii / 暖温帯林 — スダジイ",
              "Cool-temperate — Japanese beech / 冷温帯林 — ブナ",
              "Subalpine — Shirabiso / 亜高山帯林 — シラビソ",
              "Alpine — Haimatsu / 高山帯 — ハイマツ"):
    require(body.count(label) == 1, "missing/repeated bilingual caption: " + label)

for tag in ("h1", "h2", "b", "list", "img", "url"):
    opening = re.findall(r"\[" + tag + r"(?:=[^\]]+)?\]", body)
    closing = re.findall(r"\[/" + tag + r"\]", body)
    require(len(opening) == len(closing), "unbalanced BBCode tag: " + tag)
require(not any(term in body for term in
                ("MIT License", "AI assistance", "AIの支援", "Ko-fi",
                 "img.shields.io", "ライセンス")),
        "Workshop description contains license, AI or donation disclosure")
print("PASS: one bilingual title, English→Japanese→shared gallery, 4 unique images,")
print("      matching Japanese source, dependencies/save warnings, size and BBCode")
