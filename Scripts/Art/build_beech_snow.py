"""Produce snow-only layers from fixed beech foliage/branch masters."""
import hashlib
import json
import math
import sys
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter

from build_haimatsu_snow import components

ROOT = Path(__file__).resolve().parents[2]
sys.path.insert(0, str(ROOT / 'Tests/Tools'))
import fixed_template


def connected_parts(points):
    """Return all 8-connected point groups, including small branch-edge groups."""
    pending = set(points)
    result = []
    neighbours = ((1, 0), (-1, 0), (0, 1), (0, -1),
                  (1, 1), (-1, -1), (1, -1), (-1, 1))
    while pending:
        start = pending.pop()
        stack = [start]
        part = {start}
        while stack:
            x, y = stack.pop()
            for dx, dy in neighbours:
                point = (x + dx, y + dy)
                if point in pending:
                    pending.remove(point)
                    part.add(point)
                    stack.append(point)
        result.append(part)
    return result


def draw_capsule(image, cx, cy, width, height, fill, scale):
    """Draw one gravity-horizontal rounded snow pad on a supersampled layer."""
    x0 = (cx - width / 2) * scale
    y0 = (cy - height / 2) * scale
    x1 = (cx + width / 2) * scale
    y1 = (cy + height / 2) * scale
    ImageDraw.Draw(image).rounded_rectangle(
        (round(x0), round(y0), round(x1), round(y1)),
        radius=max(1, round(height * scale / 2)),
        fill=fill,
    )


def build_leafless_clumps(master, anchors):
    """Turn thin upper-edge anchors into discrete filled snow masses.

    The anchors locate supported branch tops only. They are not themselves the
    snow silhouette: broad horizontal pads rise above them so the result reads
    as accumulated snow rather than a white line traced along each branch.
    """
    scale = 4
    groups = [part for part in connected_parts(anchors) if len(part) >= 5]
    groups.sort(key=lambda part: (
        min(y for x, y in part),
        min(x for x, y in part),
    ))
    high = Image.new('RGBA', (master.width * scale, master.height * scale))

    for part in groups:
        xs = [x for x, y in part]
        ys = [y for x, y in part]
        size = len(part)
        span_x = max(xs) - min(xs) + 1
        span_y = max(ys) - min(ys) + 1
        cx = sum(xs) / size
        cy = sum(ys) / size - 1.1

        width = max(4.4, min(12.5, span_x + 3.2 + math.sqrt(size) * 0.4))
        height = max(3.8, min(6.2, 3.2 + math.sqrt(size) * 0.55))

        if span_y > span_x * 1.15:
            width = max(4.2, min(7.0, 3.8 + math.sqrt(size) * 0.5))
            height = max(3.6, min(5.4, 3.2 + math.sqrt(size) * 0.45))
        if cy < 45:
            width *= 0.9
            height *= 0.9

        draw_capsule(high, cx, cy + 0.7, width, height,
                     (132, 141, 151, 225), scale)
        draw_capsule(high, cx, cy + 0.35, width * 0.96, height * 0.78,
                     (169, 184, 202, 255), scale)
        draw_capsule(high, cx, cy - 0.1, width * 0.92, height * 0.70,
                     (224, 230, 232, 255), scale)
        draw_capsule(high, cx, cy - 0.75, width * 0.78, height * 0.42,
                     (244, 244, 235, 255), scale)

        if size >= 6:
            side = -1 if int(cx + cy) % 2 else 1
            bump_width = min(4.8, max(2.6, height * 0.8))
            bump_height = min(3.8, max(2.2, height * 0.62))
            bump_x = cx + side * width * 0.16
            bump_y = cy - height * 0.34
            draw_capsule(high, bump_x, bump_y, bump_width, bump_height,
                         (239, 241, 237, 255), scale)
            draw_capsule(high, bump_x, bump_y - 0.35,
                         bump_width * 0.75, bump_height * 0.45,
                         (250, 249, 241, 235), scale)

    layer = high.resize(master.size, Image.Resampling.LANCZOS)
    core = layer.getchannel('A').point(lambda value: 255 if value > 8 else 0)
    mask = core.filter(ImageFilter.MaxFilter(3))

    pixels = layer.load()
    allowed = mask.load()
    for y in range(master.height):
        for x in range(master.width):
            r, g, b, a = pixels[x, y]
            if not allowed[x, y]:
                pixels[x, y] = (0, 0, 0, 0)
            elif not a:
                pixels[x, y] = (0, 0, 0, 0)

    return mask, layer


def build(leafless):
    name = 'Beech_Leafless' if leafless else 'Beech'
    source = ROOT / f'Textures/Things/Plant/AMJ/{name}/{name}_A.png'
    master = Image.open(source).convert('RGBA')
    editable = set()
    for y in range(256):
        for x in range(256):
            r, g, b, a = master.getpixel((x, y))
            if a > 245 and (r > g > b and g > 70 if leafless
                            else g >= r * .97 and g > b + 12 and g > 45):
                editable.add((x, y))

    snow = set()
    if leafless:
        for x, y in editable:
            if (y < 195 and (x, y - 1) not in editable and
                    sum((x + dx, y - 1) not in editable
                        for dx in (-2, -1, 0, 1, 2)) >= 4):
                snow.update((x, yy) for yy in range(y, y + 3)
                            if (x, yy) in editable)
        mask, layer = build_leafless_clumps(master, snow)
    else:
        for pad in components(editable):
            if len(pad) < 45:
                continue
            cols = {}
            for x, y in pad:
                cols.setdefault(x, []).append(y)
            snow.update((x, y) for x, y in pad
                        if y <= min(cols[x]) + .60 * (max(cols[x]) - min(cols[x])))

        mask = Image.new('L', master.size)
        layer = Image.new('RGBA', master.size)
        for x, y in snow:
            value = master.getpixel((x, y))[1]
            color = (242, 243, 236) if value > 110 else (216, 225, 233)
            mask.putpixel((x, y), 255)
            layer.putpixel((x, y), (*color, 255))

    out = ROOT / f'Art/Templates/{name}-Snow-v1'
    out.mkdir(parents=True, exist_ok=True)
    registry = out / 'template.json'
    accepted = json.loads(registry.read_text()) if registry.exists() else None
    if accepted and accepted.get('production_status') == 'active':
        assert accepted['master']['sha256'] == hashlib.sha256(source.read_bytes()).hexdigest()
        assert Image.open(out / 'editable-mask.png').tobytes() == mask.tobytes(), 'Accepted mask changed'
        assert Image.open(out / 'snow-overlay.png').convert('RGBA').tobytes() == layer.tobytes(), 'Accepted snow changed'
        print(name, 'accepted output unchanged; regeneration skipped')
        return

    (out / 'master.png').write_bytes(source.read_bytes())
    mask.save(out / 'editable-mask.png')
    layer.save(out / 'snow-overlay.png')
    production = ROOT / f'Textures/Things/Plant/AMJ/{name}_Snow/{name}_Snow_A.png'
    production.parent.mkdir(parents=True, exist_ok=True)
    layer.save(production)
    sha = lambda path: hashlib.sha256(path.read_bytes()).hexdigest()

    if leafless:
        approval_basis = (
            'Rejected by author on 2026-10-06 because snow read as thin '
            'branch-following lines; revised clump-based snow candidate awaiting review'
        )
        mask_meaning = (
            'Rounded review regions expanded from exposed upper wood-edge anchors; '
            'snow is gravity-horizontal area/mass, lower trunk remains protected'
        )
    else:
        approval_basis = (
            'Accepted beech form; author authorized derivatives; snow review pending'
        )
        mask_meaning = (
            'Upper 60% of each connected foliage mass; original contour preserved'
        )

    spec = {
        'version': 1,
        'family': f'AMJE-{name}-snow',
        'template_revision': 'v1',
        'production_status': 'review',
        'size': [256, 256],
        'master': {'path': 'master.png', 'sha256': sha(source)},
        'editable_mask': {
            'path': 'editable-mask.png',
            'sha256': sha(out / 'editable-mask.png'),
        },
        'approval_basis': approval_basis,
        'mask_meaning': mask_meaning,
        'layer_order': ['immutable master', 'snow-only overlay'],
    }
    registry.write_text(json.dumps(spec, indent=2) + '
', encoding='utf-8')

    preview = Image.alpha_composite(master, layer)
    fixed_template.validate(master, mask, preview)
    preview.save(out / 'exact-composite.png')
    print(name, len(snow), sha(out / 'snow-overlay.png'),
          'protected RGBA differences=0')


if __name__ == '__main__':
    build(False)
    build(True)
