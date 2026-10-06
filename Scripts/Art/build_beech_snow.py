"""Produce snow-only layers from fixed beech foliage/branch masters."""
import hashlib,json,sys
from pathlib import Path
from PIL import Image
from build_haimatsu_snow import components
ROOT=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(ROOT/'Tests/Tools'))
import fixed_template

def build(leafless):
    name='Beech_Leafless' if leafless else 'Beech'
    source=ROOT/f'Textures/Things/Plant/AMJ/{name}/{name}_A.png'
    master=Image.open(source).convert('RGBA')
    editable=set()
    for y in range(256):
        for x in range(256):
            r,g,b,a=master.getpixel((x,y))
            if a>245 and (r>g>b and g>70 if leafless else g>=r*.97 and g>b+12 and g>45):editable.add((x,y))
    snow=set()
    if leafless:
        # Only exposed upper wood edges; keep lower trunk and upright branches visible.
        for x,y in editable:
            if y<195 and (x,y-1) not in editable and sum((x+dx,y-1) not in editable for dx in (-2,-1,0,1,2))>=4:
                snow.update((x,yy) for yy in range(y,y+3) if (x,yy) in editable)
    else:
        for pad in components(editable):
            if len(pad)<45:continue
            cols={}
            for x,y in pad:cols.setdefault(x,[]).append(y)
            snow.update((x,y) for x,y in pad if y<=min(cols[x])+.60*(max(cols[x])-min(cols[x])))
    mask=Image.new('L',master.size);layer=Image.new('RGBA',master.size)
    for x,y in snow:
        value=master.getpixel((x,y))[1]
        color=(242,243,236) if value>110 else (216,225,233)
        mask.putpixel((x,y),255);layer.putpixel((x,y),(*color,255))
    out=ROOT/f'Art/Templates/{name}-Snow-v1';out.mkdir(parents=True,exist_ok=True)
    registry=out/'template.json'
    accepted=json.loads(registry.read_text()) if registry.exists() else None
    if accepted and accepted.get('production_status')=='active':
        assert accepted['master']['sha256']==hashlib.sha256(source.read_bytes()).hexdigest()
        assert Image.open(out/'editable-mask.png').tobytes()==mask.tobytes(), 'Accepted mask changed'
        assert Image.open(out/'snow-overlay.png').convert('RGBA').tobytes()==layer.tobytes(), 'Accepted snow changed'
        print(name, 'accepted output unchanged; regeneration skipped')
        return
    (out/'master.png').write_bytes(source.read_bytes());mask.save(out/'editable-mask.png');layer.save(out/'snow-overlay.png')
    sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
    spec={'version':1,'family':f'AMJE-{name}-snow','template_revision':'v1','production_status':'review','size':[256,256],'master':{'path':'master.png','sha256':sha(source)},'editable_mask':{'path':'editable-mask.png','sha256':sha(out/'editable-mask.png')},'approval_basis':'Accepted beech form; author authorized derivatives; snow review pending','mask_meaning':'Exposed upper branch pixels' if leafless else 'Upper 60% of each connected foliage mass; original contour preserved','layer_order':['immutable master','snow-only overlay']}
    (out/'template.json').write_text(json.dumps(spec,indent=2)+'\n')
    preview=Image.alpha_composite(master,layer);fixed_template.validate(master,mask,preview);preview.save(out/'exact-composite.png')
    print(name,len(snow),sha(out/'snow-overlay.png'),'protected RGBA differences=0')
if __name__=='__main__':
    build(False);build(True)
