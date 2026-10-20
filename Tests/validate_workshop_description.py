#!/usr/bin/env python3
"""Ensure Environment's only paste-ready Workshop text is bilingual."""
from pathlib import Path
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
TITLE = "Ancient & Medieval Japan - Environment（中世日本 - 環境）"
BODY = ROOT / "Docs/SteamWorkshopDescription.txt"


def require(ok, message):
    if not ok:
        raise SystemExit("FAIL: " + message)


body = BODY.read_text(encoding="utf-8")
about = ET.parse(ROOT / "About/About.xml").getroot()
require(about.findtext("name") == TITLE, "About name differs from Workshop title")
require(body.startswith("[h1]" + TITLE + "[/h1]"), "Workshop title differs from About name")
require(body.count(TITLE) == 1, "Workshop title must appear once")
ja_marker = "[h2]日本語 / Japanese[/h2]"
gallery_marker = "[h2]Representative Plants / 各環境帯の代表植物[/h2]"
require(body.count(ja_marker) == 1 and body.count(gallery_marker) == 1,
        "missing or repeated language/gallery section")
separator = "\n\n[hr][/hr]\n\n"
require(body.count("[hr][/hr]") == 2 and
        body.count(separator + ja_marker) == 1 and
        body.count(separator + gallery_marker) == 1,
        "English and Japanese text and trailing image gallery require two distinct horizontal rules")
ja_pos = body.index(ja_marker)
gallery_separator_pos = body.index(separator + gallery_marker)
gallery_pos = body.index(gallery_marker)
require(0 < ja_pos < gallery_separator_pos < gallery_pos,
        "expected English, Japanese, then separate shared gallery")
require(bool(body[ja_pos:gallery_separator_pos].strip()),
        "Japanese section must not be empty")
require("[img]" not in body[:gallery_pos],
        "shared images must appear once after the second horizontal rule")
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
version = (about.findtext("modVersion") or "").strip()
require(bool(re.fullmatch(r"\d+\.\d+\.\d+", version)),
        "About version is not a semantic release number")
wetland_release = tuple(map(int, version.split("."))) >= (0, 2, 0)
expected_plants = ("Shii/Shii_A.png", "Beech/Beech_A.png",
                   "Shirabiso/Shirabiso_A.png", "Haimatsu/Haimatsu_A.png")
if wetland_release:
    expected_plants += ("Yoshi/Yoshi_A.png", "Suge/Suge_A.png",
                        "Hannoki/Hannoki_A.png")
require(len(images) == len(expected_plants) and len(set(images)) == len(expected_plants),
        "representative plant gallery must contain the approved unique image set")
for plant in expected_plants:
    require(sum(path.endswith(plant) for path in images) == 1,
            "missing or repeated image: " + plant)
if wetland_release:
    require(not any("Mizugoke" in path or "Moss" in path for path in images),
            "temporary Vanilla-moss reuse does not authorize a sphagnum gallery image")
    require("[b]Cool bog — Sphagnum (mizugoke) / 冷涼湿原 — 水苔（ミズゴケ）[/b]"
            in body and "Its graphics temporarily reuse Vanilla moss artwork."
            in body and "画像は暫定的にバニラの苔と同じものを使用しています。" in body,
            "missing separately approved text-only sphagnum and temporary-art caption")
    require(not any(phrase in body for phrase in
                    ("湿地植物の追加（未実装）", "Additional wetland plants（未実装）")),
            "implemented wetland plants are still listed as unreleased candidates")
    for name in ("葦", "菅", "榛の木", "水苔", "Yoshi", "Suge", "Hannoki", "mizugoke"):
        require(name.casefold() in body.casefold(),
                "missing wetland-plant wording: " + name)
for tag in ("h1", "h2", "b", "list", "img", "url"):
    opening = re.findall(r"\[" + tag + r"(?:=[^\]]+)?\]", body)
    closing = re.findall(r"\[/" + tag + r"\]", body)
    require(len(opening) == len(closing), "unbalanced BBCode tag: " + tag)
require(not any(term in body for term in
                ("MIT License", "AI assistance", "AIの支援", "Ko-fi",
                 "img.shields.io", "ライセンス")),
        "Workshop description contains license, AI or donation disclosure")

print(f"PASS: one bilingual title, English→Japanese→shared gallery, {len(images)} unique images,")
print("      Japanese body, dependencies/save warnings, size and BBCode")
