"""Make a registered Shirabiso snow layer using accepted snow material colors."""
import hashlib
import json
import sys
from pathlib import Path
from PIL import Image
from build_haimatsu_snow import components, bbox

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / 'Art/Templates/Shirabiso-Snow-v1'
MASTER = ROOT / 'Textures/Things/Plant/AMJ/Shirabiso/Shirabiso_A.png'
MATERIAL = ROOT / 'Art/Templates/Haimatsu-Snow-v1/snow-overlay.png'

def main():
    master = Image.open(MASTER).convert('RGBA')
    material = Image.open(MATERIAL).convert('RGBA')
    foliage = {(x,y) for y in range(256) for x in range(256)
               if (lambda c:c[3]>245 and c[1]>=c[0]-1 and c[1]>c[2]+3 and c[1]>35)(master.getpixel((x,y)))}
    pads = components(foliage)
    assert len(pads) == 7, 'Master foliage segmentation changed'
    colors = [c[:3] for c in material.get_flattened_data() if c[3]>32]
    palette = [min(colors,key=lambda c:sum((a-b)**2 for a,b in zip(c,target)))
               for target in ((169,184,202),(216,224,232),(244,244,235))]
    mask = Image.new('L',master.size)
    layer = Image.new('RGBA',master.size)
    for pad in pads:
        columns = {}
        for x,y in pad:
            columns.setdefault(x,[]).append(y)
        for x,y in pad:
            top,bottom = min(columns[x]),max(columns[x])
            if y > top + .68*(bottom-top):
                continue
            value = sum(master.getpixel((x,y))[:2])/2
            anchors = (60,105,150)
            slot = 0 if value<105 else 1
            fraction = max(0,min(1,(value-anchors[slot])/(anchors[slot+1]-anchors[slot])))
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
        'version':1,'family':'AMJE-Shirabiso-snow','template_revision':'v1','production_status':'review',
        'size':[256,256],'master':{'path':'master.png','sha256':sha(MASTER.read_bytes())},
        'editable_mask':{'path':'editable-mask.png','sha256':sha((OUT/'editable-mask.png').read_bytes())},
        'approval_basis':'Accepted Shirabiso base and accepted-form derivative authorization; snow pending',
        'mask_meaning':'Upper 68% of each master foliage column in seven foliage components; branches/outer AA protected',
        'layer_order':['unchanged Shirabiso master','registered snow-only layer'],
        'material_sha256':sha(MATERIAL.read_bytes()),'pad_bboxes':[bbox(p) for p in pads]}
    registry.write_text(json.dumps(manifest,indent=2)+'\n',encoding='utf-8')
    sys.path.insert(0,str(ROOT/'Tests/Tools'))
    import fixed_template
    base,allowed = fixed_template.load_template(registry,allow_inactive=True)
    preview=Image.alpha_composite(base,layer)
    fixed_template.validate(base,allowed,preview)
    preview.save(OUT/'exact-composite.png')
    print('PASS: seven master-registered foliage regions; protected RGBA differences=0')

if __name__=='__main__':
    main()
