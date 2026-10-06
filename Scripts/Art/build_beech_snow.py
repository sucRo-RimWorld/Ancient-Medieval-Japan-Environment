"""Produce snow-only layers from fixed beech foliage/branch masters."""
import hashlib
import json
import sys
from pathlib import Path

from PIL import Image, ImageDraw

from build_haimatsu_snow import components

ROOT = Path(__file__).resolve().parents[2]
sys.path.insert(0, str(ROOT / 'Tests/Tools'))
import fixed_template


# Reviewed branch ledges, not automatically detected twig-edge fragments.
# x, supporting branch y, width, snow thickness; reviewed against immutable master.
LEAFLESS_LEDGES = (
    (94, 62, 22, 9), (134, 79, 23, 10), (55, 78, 24, 9),
    (157, 91, 24, 9), (207, 102, 24, 10), (43, 116, 25, 10),
    (181, 117, 27, 11), (95, 131, 20, 9), (204, 142, 27, 11),
    (46, 160, 30, 12), (81, 174, 24, 10), (173, 175, 25, 11),
)
SNOW_OUTLINE = (55, 53, 43, 255)
SNOW_BASE = (245, 243, 235, 255)
SNOW_SHADOW = (218, 225, 232, 255)


def smooth_loop(points):
    """Periodic cubic contour; large lobes only, no twig-scale bumps."""
    result = []
    for i in range(len(points)):
        a, b, c, d = [points[j % len(points)] for j in (i-1, i, i+1, i+2)]
        for step in range(10):
            t = step / 10
            result.append(tuple(.5 * (2*b[k] + (-a[k]+c[k])*t +
                (2*a[k]-5*b[k]+4*c[k]-d[k])*t*t +
                (-a[k]+3*b[k]-3*c[k]+d[k])*t*t*t) for k in (0, 1)))
    return result


def build_leafless_clumps(master, anchors=None):
    """Paint twelve supported asymmetric caps using accepted snow color planes.

    The review contract is defined BEFORE drawing. Never derive or enlarge an
    editable mask from generated alpha to make fixed-pixel validation pass.
    Master stays unchanged; this unapproved mask revision cannot become active.
    """
    scale = 4
    high = Image.new('RGBA', (master.width * scale, master.height * scale))
    mask = Image.new('L', master.size)
    contract = ImageDraw.Draw(mask)
    draw = ImageDraw.Draw(high)
    lobes = [(0,.73),(.06,.42),(.20,.33),(.28,.04),(.46,.02),
             (.59,.25),(.74,.23),(.84,.48),(1,.69),(.96,.89),
             (.76,.95),(.58,.86),(.38,.98),(.16,.9)]
    for i, (x, support_y, width, height) in enumerate(LEAFLESS_LEDGES):
        # Independent allowed region around this ledge; includes edge AA.
        contract.rectangle((x-3, support_y-height-3,
                            x+width+3, support_y+4), fill=255)
        local = [(1-u, v) if i % 2 else (u, v) for u,v in lobes]
        contour = smooth_loop([(scale*(x+u*width),
                                scale*(support_y-height+v*height)) for u,v in local])
        draw.polygon(contour, fill=SNOW_BASE)
        # One broad shadow plane on the lower surface, no nested capsule lines.
        shadow = [(x+width*.10,support_y-height*.29),
                  (x+width*.32,support_y-height*.34),
                  (x+width*.50,support_y-height*.19),
                  (x+width*.71,support_y-height*.30),
                  (x+width*.92,support_y-height*.23),
                  (x+width*.95,support_y-height*.10),
                  (x+width*.73,support_y-height*.07),
                  (x+width*.56,support_y-height*.15),
                  (x+width*.38,support_y-height*.04),
                  (x+width*.17,support_y-height*.11)]
        draw.polygon([(a*scale,b*scale) for a,b in shadow], fill=SNOW_SHADOW)
        draw.line(contour+[contour[0]], fill=SNOW_OUTLINE,
                  width=round(1.5*scale), joint='curve')
    layer = high.resize(master.size, Image.Resampling.BOX)
    assert all(not a or m for a,m in zip(layer.getchannel('A').tobytes(), mask.tobytes())), 'Artwork escaped review contract'
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
        mask, layer = build_leafless_clumps(master)
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
            'branch-following lines; sparse outlined snow-cap revision 2 awaiting review'
        )
        mask_meaning = (
            'Twelve predeclared branch-ledge review regions; fixed before painting; '
            'unapproved revision 2, unchanged lower trunk and all outside RGBA'
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
        'template_revision': 'v2-review' if leafless else 'v1',
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
    registry.write_text(json.dumps(spec, indent=2) + '\n', encoding='utf-8')

    preview = Image.alpha_composite(master, layer)
    fixed_template.validate(master, mask, preview)
    preview.save(out / 'exact-composite.png')
    print(name, sum(a > 0 for a in layer.getchannel('A').tobytes()), sha(out / 'snow-overlay.png'),
          'protected RGBA differences=0')


if __name__ == '__main__':
    build(False)
    build(True)
