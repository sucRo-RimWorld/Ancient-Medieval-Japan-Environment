"""Register generated snow material to the immutable master's five foliage pads."""
import hashlib
import json
from pathlib import Path
from PIL import Image

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / 'Art/Templates/Haimatsu-Snow-v1'
MASTER = ROOT / 'Textures/Things/Plant/AMJ/Haimatsu/Haimatsu_A.png'
MATERIAL = ROOT / 'Art/Candidates/Haimatsu-Snow/Haimatsu_SnowOverlay_Corrected256.png'

def components(points):
    pending = set(points)
    result = []
    while pending:
        stack = [pending.pop()]
        part = set(stack)
        while stack:
            x, y = stack.pop()
            for dx, dy in ((1,0),(-1,0),(0,1),(0,-1),(1,1),(-1,-1),(1,-1),(-1,1)):
                p = (x+dx, y+dy)
                if p in pending:
                    pending.remove(p); part.add(p); stack.append(p)
        if len(part) > 20:
            result.append(part)
    return sorted(result, key=len, reverse=True)

def bbox(part):
    return min(x for x,y in part), min(y for x,y in part), max(x for x,y in part), max(y for x,y in part)

def order(part):
    x0,y0,x1,y1 = bbox(part)
    return (y0, x0)

def main():
    master = Image.open(MASTER).convert('RGBA')
    material = Image.open(MATERIAL).convert('RGBA')
    assert master.size == material.size == (256,256)
    foliage = {(x,y) for y in range(256) for x in range(256)
               if (lambda c: c[3] > 245 and c[1] >= c[0]-1 and c[1] > c[2]+5 and c[1] > 35)(master.getpixel((x,y)))}
    pads = components(foliage)[:5]
    caps = components({(x,y) for y in range(256) for x in range(256) if material.getpixel((x,y))[3] > 32})[:5]
    assert len(pads) == len(caps) == 5
    pads.sort(key=order); caps.sort(key=order)
    layer = Image.new('RGBA', master.size)
    mask = Image.new('L', master.size)
    registration = []
    snow_colors = [material.getpixel(p)[:3] for cap in caps for p in cap
                   if min(material.getpixel(p)[:3]) > 95]
    palette = [min(snow_colors,key=lambda c:sum((a-b)**2 for a,b in zip(c,target)))
               for target in ((169,184,202),(216,224,232),(244,244,235))]
    for pad, cap in zip(pads, caps):
        x0,y0,x1,y1 = bbox(pad)
        sx0,sy0,sx1,sy1 = bbox(cap)
        for x,y in pad:
            column = [py for px,py in pad if px == x]
            top, bottom = min(column), max(column)
            if y > top + .58*(bottom-top):
                continue
            # Reuse generated material colors; shade follows the master's own
            # broad light/shadow blocks, not an independently warped cap contour.
            value = sum(master.getpixel((x,y))[:2]) / 2
            anchors = (60,105,158)
            slot = 0 if value < anchors[1] else 1
            fraction = max(0,min(1,(value-anchors[slot])/(anchors[slot+1]-anchors[slot])))
            color = tuple(round(a+(b-a)*fraction) for a,b in zip(palette[slot],palette[slot+1]))
            layer.putpixel((x,y), (*color,255)); mask.putpixel((x,y),255)
        registration.append({'master_pad_bbox':bbox(pad),'generated_material_bbox':bbox(cap)})
    OUT.mkdir(parents=True,exist_ok=True)
    (OUT/'master.png').write_bytes(MASTER.read_bytes())
    layer.save(OUT/'snow-overlay.png'); mask.save(OUT/'editable-mask.png')
    sha = lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
    manifest = {'version':1,'family':'AMJE-Haimatsu-snow','template_revision':'v1',
                'production_status':'review','size':[256,256],
                'master':{'path':'master.png','sha256':sha(OUT/'master.png')},
                'editable_mask':{'path':'editable-mask.png','sha256':sha(OUT/'editable-mask.png')},
                'approval_basis':'Accepted Haimatsu base; derivative authorization. Snow appearance pending.',
                'mask_meaning':'Upper 58% of each master foliage column, foliage alpha >245. All branches/outer antialias protected.',
                'layer_order':['unchanged master','registered snow-only overlay'],
                'material_sha256':sha(MATERIAL),'registration':registration}
    registry = OUT/'template.json'
    if registry.exists():
        accepted = json.loads(registry.read_text(encoding='utf-8'))
        if accepted.get('production_status') == 'active':
            assert manifest['master'] == accepted['master'] and manifest['editable_mask'] == accepted['editable_mask'], 'Accepted master/mask cannot be redefined'
            manifest = accepted
    (OUT/'template.json').write_text(json.dumps(manifest,indent=2)+'\n',encoding='utf-8')
    # Review compose uses the exact Core implementation; inactive remains pending.
    import sys
    sys.path.insert(0,str(ROOT/'Tests/Tools'))
    import fixed_template
    base, allowed = fixed_template.load_template(OUT/'template.json',allow_inactive=True)
    preview = Image.alpha_composite(base,layer)
    fixed_template.validate(base,allowed,preview)
    preview.save(OUT/'exact-composite.png')
    assert all(not a or m for a,m in zip(layer.getchannel('A').tobytes(),mask.tobytes()))
    print('PASS: five master-registered pads; protected RGBA differences=0; no snow outside mask')

if __name__ == '__main__':
    main()
