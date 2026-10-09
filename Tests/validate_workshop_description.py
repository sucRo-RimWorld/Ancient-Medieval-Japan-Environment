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
require(body[ja_pos:gallery_separator_pos].strip() == ja_source,
        "combined Japanese section differs from maintained Japanese source")
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

# Public change-list coverage must not omit implemented features or mislabel candidates.
two_game = (ROOT / "Docs/2GameDescription-ja.txt").read_text(encoding="utf-8")
english = body[:ja_pos]
japanese = body[ja_pos:gallery_separator_pos]
keywords = (
    ("temperature", "気温・降水量"), ("rivers", "河川を増やし"),
    ("coastal", "湾・半島"), ("four biomes", "4バイオーム"),
    ("four plants", "4植物"), ("Vanilla vegetation", "バニラ植生"),
    ("tree-sowing", "植林候補"), ("Thin Soil", "痩せた土壌"),
    ("stony/poor", "礫地"), ("rain, fog", "雨・霧"),
    ("wildlife", "野生動物の出現構成"), ("wild-plant pools", "温帯湿地・冷涼湿原"),
    ("pack animals", "荷役動物"), ("disease types", "病気の種類"),
    ("wetland rain", "湿地の雨"), ("movement difficulty", "移動困難度"),
    ("six retained Vanilla trees", "バニラ樹木6種"), ("native wood yield", "伐採時の木材"),
)
for en_term, ja_term in keywords:
    require(any(line.startswith("[*]") and en_term.lower() in line.lower()
                for line in english.splitlines()), "Workshop English missing category: "+en_term)
    require(any(line.startswith("[*]") and ja_term in line
                for line in japanese.splitlines()), "Workshop Japanese missing category: "+ja_term)
    require(any(line.startswith("・") and ja_term in line
                for line in two_game.splitlines()), "2game missing category: "+ja_term)
def pending_lines(text, heading, end):
    require(heading in text, "missing future section")
    block = text.split(heading, 1)[1].split(end, 1)[0]
    return [line for line in block.splitlines() if line.startswith("[*]") or line.startswith("・")]
for text, start, end in (
    (english, "[h2]Future candidates[/h2]", "Vanilla world generation"),
    (japanese, "[h2]追加候補・今後の予定[/h2]", "既存の世界生成"),
    (two_game, "▼ 今後の予定", "詳しい仕様"),
):
    future = pending_lines(text, start, end)
    require(len(future) == 5 and all(line.endswith("（未実装）") for line in future),
            "every future candidate must end in （未実装）")
for obsolete in ("final Wild Healroot cleanup", "野生Healrootを最後に"):
    require(obsolete not in body and obsolete not in two_game and
            obsolete not in (ROOT / "README.md").read_text(encoding="utf-8"),
            "withdrawn Healroot-removal plan still present")

print("PASS: one bilingual title, English→Japanese→shared gallery, 4 unique images,")
print("      matching Japanese source, dependencies/save warnings, size and BBCode")
