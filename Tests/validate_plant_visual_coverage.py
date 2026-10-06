"""Validate review evidence; strict closeout refuses pending or stale states."""
import argparse
import copy
import hashlib
import json
from pathlib import Path
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
BASE = {"normal", "immature", "icon", "snow"}
SEASONAL = {"leafless", "autumn", "transition"}


def fingerprint(plant, state=None):
    if state is not None:
        plant = copy.deepcopy(plant)
        # Player-facing localization/prose is nonvisual and must not invalidate
        # an already reviewed sprite/state.
        for field in ("label", "description"):
            node = plant.find(field)
            if node is not None:
                plant.remove(node)
        if state != "icon":
            for field in ("uiIconPath", "uiIconScale", "uiIconOffset", "uiIconColor", "uiIconAngle"):
                node = plant.find(field)
                if node is not None: plant.remove(node)
        if state != "snow":
            for field in ("snowOverlayGraphicPath", "leaflessSnowOverlayGraphicPath", "immatureSnowOverlayGraphicPath"):
                node = plant.find("plant/"+field)
                if node is not None: plant.find("plant").remove(node)
    if state == "leafless":
        params = plant.find("graphicData/shaderParameters")
        if params is not None: plant.find("graphicData").remove(params)
    paths = [plant.findtext("graphicData/texPath")]
    leafless = plant.findtext("plant/leaflessGraphicPath")
    if leafless:
        paths.append(leafless)
    for field in ("snowOverlayGraphicPath", "leaflessSnowOverlayGraphicPath", "immatureSnowOverlayGraphicPath"):
        path = plant.findtext("plant/" + field)
        if path and (ROOT / "Textures" / path).is_dir():
            paths.append(path)
    assets = {}
    for path in paths:
        pngs = sorted((ROOT / "Textures" / path).glob("*.png"))
        assert pngs, f"Missing PNGs: {path}"
        for png in pngs:
            assets[png.relative_to(ROOT).as_posix()] = hashlib.sha256(png.read_bytes()).hexdigest()
    for parameter in plant.findall('graphicData/shaderParameters/*'):
        value = (parameter.text or '').strip()
        if value.startswith('/'):
            png = ROOT / 'Textures' / (value.lstrip('/') + '.png')
            if png.is_file():
                assets[png.relative_to(ROOT).as_posix()] = hashlib.sha256(png.read_bytes()).hexdigest()
    return {"definition": hashlib.sha256(ET.tostring(plant)).hexdigest(), "pngs": assets}


def validate(data, plants):
    errors, pending = [], []
    if set(data["plants"]) != set(plants):
        errors.append("Ledger plant inventory differs from implemented AMJE plants")
    for name, plant in plants.items():
        row = data["plants"].get(name, {})
        states = row.get("states", {})
        if set(states) != BASE | SEASONAL:
            errors.append(f"{name}: missing/extra state rows")
        deciduous = bool(plant.findtext("plant/leaflessGraphicPath"))
        for state in BASE | SEASONAL:
            entry = states.get(state, {})
            status = entry.get("status")
            na = state in SEASONAL and not deciduous
            if na:
                if status != "na" or entry.get("reason") != "evergreen":
                    errors.append(f"{name}/{state}: requires explicit evergreen N/A")
            elif status == "pending":
                pending.append(f"{name}/{state}")
            elif status == "verified" and state == "transition":
                if entry.get("verification_kind") != "automated_native_calendar_sampling":
                    errors.append(f"{name}/{state}: unsupported automated verification")
                evidence = ROOT / entry.get("evidence", "")
                if not evidence.is_file() or entry.get("fingerprint") != fingerprint(plant, state):
                    errors.append(f"{name}/{state}: missing or stale automated evidence")
                if not entry.get("scope") or not entry.get("date"):
                    errors.append(f"{name}/{state}: missing verification scope/date")
            elif status == "accepted":
                if state == "autumn" or entry.get("review_kind") == "color":
                    controls = entry.get("color_conditions", {})
                    expected = {"local_hour": 12, "weather": "Clear", "paused": True,
                                "weather_transition_complete": True, "verified": True}
                    if any(controls.get(k) != v for k, v in expected.items()):
                        errors.append(f"{name}/{state}: unverified time/weather color baseline")
                    for key in ("season", "shader_intensity", "zoom", "evidence"):
                        if key not in controls or controls[key] in (None, ""):
                            errors.append(f"{name}/{state}: missing color condition {key}")
                for key in ("date", "author_statement", "evidence"):
                    if not entry.get(key):
                        errors.append(f"{name}/{state}: missing {key}")
                evidence = entry.get("evidence", "").split("#")[0]
                if not evidence or not (ROOT / evidence).is_file():
                    errors.append(f"{name}/{state}: evidence file missing")
                if entry.get("fingerprint") != fingerprint(plant, state):
                    errors.append(f"{name}/{state}: reviewed production changed")
            else:
                errors.append(f"{name}/{state}: invalid status or unsupported N/A")
        incomplete = any(x.startswith(name + "/") for x in pending)
        if row.get("complete") is not (not incomplete):
            errors.append(f"{name}: complete flag contradicts pending rows")
    return errors, sorted(pending)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--require-complete", action="store_true")
    parser.add_argument("--self-test", action="store_true")
    args = parser.parse_args()
    plants = {p.findtext("defName"): p for p in ET.parse(ROOT / "Defs/ThingDefs_Plants/AMJ_WildPlants.xml").getroot()}
    data = json.loads((ROOT / "Docs/PlantVisualCoverage.json").read_text(encoding="utf-8"))
    errors, pending = validate(data, plants)
    if args.self_test:
        name = next(iter(plants))
        for mutation in ("missing", "false_complete", "stale", "unsupported_na"):
            broken = copy.deepcopy(data)
            row = broken["plants"][name]
            if mutation == "missing": del row["states"]["snow"]
            if mutation == "false_complete":
                row["complete"] = True
                row["states"]["snow"] = {"status": "pending"}
            if mutation == "stale": row["states"]["normal"]["fingerprint"] = {}
            if mutation == "unsupported_na": row["states"]["snow"] = {"status": "na", "reason": "evergreen"}
            assert validate(broken, plants)[0], f"Regression escaped: {mutation}"
        print("[OK] Missing states, false completion, stale art and unsupported N/A rejected")
        original = plants[name]
        ui_changed = copy.deepcopy(original)
        ET.SubElement(ui_changed, 'uiIconScale').text = '0.5'
        assert fingerprint(original, 'normal') == fingerprint(ui_changed, 'normal')
        assert fingerprint(original, 'icon') != fingerprint(ui_changed, 'icon')
        snow_changed = copy.deepcopy(original)
        snow_changed.find('plant/snowOverlayGraphicPath').text = 'missing-test-overlay'
        assert fingerprint(original, 'normal') == fingerprint(snow_changed, 'normal')
        assert fingerprint(original, 'snow') != fingerprint(snow_changed, 'snow')
        text_changed = copy.deepcopy(original)
        text_changed.find('label').text = (text_changed.findtext('label') or '') + ' test'
        text_changed.find('description').text = (text_changed.findtext('description') or '') + ' test'
        assert fingerprint(original, 'normal') == fingerprint(text_changed, 'normal')
        assert fingerprint(original, 'icon') == fingerprint(text_changed, 'icon')
        assert fingerprint(original, 'snow') == fingerprint(text_changed, 'snow')
        size_changed = copy.deepcopy(original)
        ET.SubElement(size_changed.find('graphicData'), 'drawSize').text = '9'
        assert fingerprint(original, 'normal') != fingerprint(size_changed, 'normal')
        print('[OK] Text/UI/snow scoping preserves unrelated approvals; icon/snow/size changes invalidate affected states')

        deciduous = next(n for n,p in plants.items() if p.findtext('plant/leaflessGraphicPath'))
        broken = copy.deepcopy(data)
        entry = broken['plants'][deciduous]['states']['autumn']
        entry.pop('color_conditions', None)
        entry.update(status='accepted', date='2026-10-05', author_statement='test',
                     evidence='Docs/ArtDirection.md', fingerprint=fingerprint(plants[deciduous], "autumn"))
        assert any('color baseline' in e for e in validate(broken, plants)[0])
        entry['color_conditions'] = dict(local_hour=12, weather='Clear', paused=True,
            weather_transition_complete=True, verified=True, season='test',
            shader_intensity=1, zoom='test', evidence='Docs/ArtDirection.md')
        assert not any('color baseline' in e or 'color condition' in e for e in validate(broken, plants)[0])
        entry['color_conditions']['weather'] = 'Rain'
        assert any('color baseline' in e for e in validate(broken, plants)[0])
        print('[OK] Unrecorded or non-Clear color approval rejected; recorded baseline accepted')
    for error in errors: print("[FAIL]", error)
    for item in pending: print("[PENDING]", item)
    if errors or (args.require_complete and pending):
        raise SystemExit(1)
    print(f"[OK] Ledger consistent; {len(pending)} visual reviews still pending")


if __name__ == "__main__":
    main()
