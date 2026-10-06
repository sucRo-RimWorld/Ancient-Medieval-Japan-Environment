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


# Each selected corridor follows ONE visible branch, not an arbitrary snow bbox.
# x0, x1, estimated upper-surface y0/y1, snow depth. All columns must have support.
LEAFLESS_BRANCHES = (
    (92, 113, 56, 68, 10), (51, 73, 77, 81, 10),
    (164, 174, 88, 83, 8), (44, 59, 115, 113, 10),
    (185, 208, 113, 111, 12), (83, 108, 136, 149, 12),
    (204, 218, 146, 140, 10), (60, 79, 158, 161, 12),
)
SNOW_OUTLINE = (32, 29, 22, 255)
SNOW_BASE = (245, 243, 235, 255)
SNOW_SHADOW = (218, 225, 232, 255)


def branch_surface(master, branch):
    """Trace actual opaque upper branch pixels inside a declared narrow corridor."""
    x0,x1,y0,y1,depth = branch
    surface=[]
    for x in range(x0,x1+1):
        target=y0+(y1-y0)*(x-x0)/(x1-x0)
        rows=[y for y in range(round(target)-3,round(target)+4)
              if master.getpixel((x,y))[3]>=245]
        assert rows, f'Branch corridor lacks support at {x}'
        y=min(rows,key=lambda v:abs(v-target))
        # Follow the top of this local branch run, not a different twig above it.
        while y>round(target)-4 and master.getpixel((x,y-1))[3]>=128:
            y-=1
        surface.append((x,y))
    return surface


def build_leafless_clumps(master, anchors=None):
    """Use branch-shaped contact boundaries and tapered snow depth above them."""
    import math
    scale=4
    high=Image.new('RGBA',(master.width*scale,master.height*scale))
    mask=Image.new('L',master.size)
    draw=ImageDraw.Draw(high)
    contract=ImageDraw.Draw(mask)
    for index,branch in enumerate(LEAFLESS_BRANCHES):
        x0,x1,y0,y1,depth=branch
        # Contract is independent of painted alpha and never widened after a leak.
        contract.rectangle((x0-2,min(y0,y1)-depth-7,
                            x1+2,max(y0,y1)+5),fill=255)
        surface=branch_surface(master,branch)
        # Opaque snow lip covers the original upper outline/wood face.
        # Ending at the silhouette edge incorrectly leaves wood in foreground.
        bottom=[(x,y+4) for x,y in surface]
        top=[]
        for x,y in bottom:
            t=(x-x0)/(x1-x0)
            # Thickness tapers into the branch at both ends, with unequal lobes.
            h=1+depth*(math.sin(math.pi*t)**.7)*(.85+.15*math.sin(t*5+index))
            near=[v for xx,v in bottom if abs(xx-x)<=3]
            top.append((x,sum(near)/len(near)-h-4))
        polygon=top+list(reversed(bottom))
        draw.polygon([(round(x*scale),round(y*scale)) for x,y in polygon],fill=SNOW_BASE)
        shadow=[(x,y-3.5) for x,y in bottom]+list(reversed(bottom))
        draw.polygon([(round(x*scale),round(y*scale)) for x,y in shadow],fill=SNOW_SHADOW)
        draw.line([(round(x*scale),round(y*scale)) for x,y in polygon+[polygon[0]]],
                  fill=SNOW_OUTLINE,width=round(1.5*scale),joint='curve')
    layer=high.resize(master.size,Image.Resampling.BOX)
    assert all(not a or m for a,m in zip(layer.getchannel('A').tobytes(),mask.tobytes())), 'Artwork escaped review contract'
    return mask,layer


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
            'branch-following lines; front-occluding snow revision 4 awaiting review'
        )
        mask_meaning = (
            'Eight narrow branch corridors traced against original opaque upper surfaces; '
            'unapproved revision 4, unchanged lower trunk and all outside RGBA'
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
        'template_revision': 'v4-review' if leafless else 'v1',
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
