"""Make a registered Shii snow layer using accepted snow material colors."""
import hashlib
import json
import sys
from pathlib import Path
from PIL import Image
from build_haimatsu_snow import components, bbox

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / 'Art/Templates/Shii-Snow-v1'
MASTER = ROOT / 'Textures/Things/Plant/AMJ/Shii/Shii_A.png'
MATERIAL = ROOT / 'Art/Templates/Haimatsu-Snow-v1/snow-overlay.png'

def main():
    master = Image.open(MASTER).convert('RGBA')
    material = Image.open(MATERIAL).convert('RGBA')
    foliage = {(x,y) for y in range(256) for x in range(256)
               if (lambda c:c[3]>245 and c[1]>=c[0]*.90 and c[1]>c[2]+3 and c[1]>35)(master.getpixel((x,y)))}
    # Trace the master's actual upper-lit foliage masses. Geometric crown
    # partitions caused artificial snow cutoffs and are intentionally removed.
    upper = {p for p in foliage if sum(master.getpixel(p)[:2])/2 >= 85}
    pads = [p for p in components(upper) if len(p)>=100]
    assert len(pads)==5, 'Master upper-lit foliage segmentation changed'
    # Trace each column of the left leaf mass, keeping its original scalloped edge.
    left_cap = set()
    for x in range(15, 67):
        column = sorted(y for px,y in foliage if px==x and 136<=y<=180)
        if column:
            top = column[0]
            left_cap.update((x,y) for y in column if y<=top+10)
    pads.append(left_cap)
    colors = [c[:3] for c in material.get_flattened_data() if c[3]>32]
    palette = [min(colors,key=lambda c:sum((a-b)**2 for a,b in zip(c,target)))
               for target in ((169,184,202),(216,224,232),(244,244,235))]
    mask = Image.new('L',master.size)
    layer = Image.new('RGBA',master.size)
    for pad in pads:
        for x,y in pad:
            value = sum(master.getpixel((x,y))[:2])/2
            if (x,y) in left_cap:
                value = max(value, 100)
            shade_anchors = (75,85,120)
            slot = 0 if value<85 else 1
            fraction = max(0,min(1,(value-shade_anchors[slot])/(shade_anchors[slot+1]-shade_anchors[slot])))
            color = tuple(round(a+(b-a)*fraction) for a,b in zip(palette[slot],palette[slot+1]))
            layer.putpixel((x,y),(*color,255)); mask.putpixel((x,y),255)
    OUT.mkdir(parents=True,exist_ok=True)
    sha = lambda b:hashlib.sha256(b).hexdigest()
    registry = OUT/'template.json'
    accepted = json.loads(registry.read_text(encoding='utf-8')) if registry.exists() else None
    if accepted and accepted.get('production_status')=='active':
        assert accepted['master']['sha256']==sha(MASTER.read_bytes())
        assert Image.open(OUT/'editable-mask.png').tobytes()==mask.tobytes(), 'Active mask is immutable'
    (OUT/'master.png').write_bytes(MASTER.read_bytes())
    mask.save(OUT/'editable-mask.png');layer.save(OUT/'snow-overlay.png')
    manifest = accepted if accepted and accepted.get('production_status')=='active' else {
        'version':1,'family':'AMJE-Shii-snow','template_revision':'v1','production_status':'review',
        'size':[256,256],'master':{'path':'master.png','sha256':sha(MASTER.read_bytes())},
        'editable_mask':{'path':'editable-mask.png','sha256':sha((OUT/'editable-mask.png').read_bytes())},
        'approval_basis':'Accepted Shii base and accepted-form derivative authorization; snow pending',
        'mask_meaning':'Five connected upper/middle-lit foliage masses at threshold 85 from master pixels; left lateral foliage cap follows master upper contour; branches/outer AA protected',
        'layer_order':['unchanged Shii master','registered snow-only layer'],
        'material_sha256':sha(MATERIAL.read_bytes()),'pad_bboxes':[bbox(p) for p in pads]}
    registry.write_text(json.dumps(manifest,indent=2)+'\n',encoding='utf-8')
    sys.path.insert(0,str(ROOT/'Tests/Tools'))
    import fixed_template
    base,allowed = fixed_template.load_template(registry,allow_inactive=True)
    preview=Image.alpha_composite(base,layer)
    fixed_template.validate(base,allowed,preview)
    preview.save(OUT/'exact-composite.png')
    print('PASS: five traced heavy-snow foliage masses; protected RGBA differences=0')

if __name__=='__main__':
    main()
