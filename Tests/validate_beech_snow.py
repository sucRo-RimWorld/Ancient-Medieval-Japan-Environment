"""Verify both native beech snow states and immutable template registration."""
import sys,json,hashlib
from pathlib import Path
from PIL import Image,ImageChops,ImageFilter
import xml.etree.ElementTree as ET
root=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(root/'Tests/Tools'))
import fixed_template
sys.path.insert(0, str(root/'Scripts/Art'))
from build_haimatsu_snow import components
from build_beech_snow import branch_surface, LEAFLESS_BRANCHES


def validate_leafless_massing(layer, base):
    solid = {(x,y) for y in range(layer.height) for x in range(layer.width)
             if layer.getpixel((x,y))[3] >= 128}
    masses = components(solid)
    assert 4 <= len(masses) <= 16, 'Snow must be sparse supported masses, not many pellets'
    for mass in masses:
        xs, ys = zip(*mass)
        width, height = max(xs)-min(xs)+1, max(ys)-min(ys)+1
        assert len(mass) >= 60 and width >= 12 and height >= 6, 'Snow mass too small/thin'
        assert len(mass)/(width*height) >= .4, 'Snow has outline-only or sparse fill'
        outline = sum(max(layer.getpixel(p)[:3]) < 100 for p in mass)
        assert outline >= 20, 'Snow needs a strong dark outer contour'
        bottom={x:max(yy for xx,yy in mass if xx==x) for x,y in mass}
        supported=sum(any(base.getpixel((x,yy))[3]>=128
                          for yy in range(min(y for xx,y in mass if xx==x), min(base.height, y+2)))
                      for x,y in bottom.items())
        assert supported/len(bottom)>=.90, 'Snow must overlap a supporting branch across its width'
    return len(masses), len(solid)


# Known failure classes: an area-ratio-only check accepted tiny filled pellets.
from PIL import ImageDraw
for kind in ('pellets', 'stripes', 'weak-outline', 'floating', 'partial-support'):
    bad = Image.new('RGBA',(256,256))
    draw = ImageDraw.Draw(bad)
    if kind == 'pellets':
        for y in range(30,200,20):
            for x in range(30,220,20):
                draw.ellipse((x,y,x+5,y+5), fill=(245,243,235,255))
    else:
        for x,y in ((30,40),(90,40),(150,40),(30,100),(90,100)):
            draw.rectangle((x,y,x+24,y+(2 if kind == 'stripes' else 9)),
                           fill=(245,243,235,255),
                           outline=(55,53,43,255) if kind != 'weak-outline' else None,
                           width=2)
    fixture_base=Image.new('RGBA',(256,256),
            (70,60,40,255) if kind not in ('floating','partial-support') else (0,0,0,0))
    if kind=='partial-support':
        # Former area-contact test passed a cap resting on only a narrow portion.
        bd=ImageDraw.Draw(fixture_base)
        for x,y in ((30,40),(90,40),(150,40),(30,100),(90,100)):
            bd.rectangle((x,y,x+9,y+12),fill=(70,60,40,255))
    try:
        validate_leafless_massing(bad,fixture_base)
    except AssertionError:
        print('[OK] Rejected leafless snow regression:', kind)
    else:
        raise AssertionError('Snow regression fixture passed: '+kind)

plant=next(p for p in ET.parse(root/'Defs/ThingDefs_Plants/AMJ_WildPlants.xml').getroot() if p.findtext('defName')=='AMJ_Tree_Beech')
for name,field in [('Beech','snowOverlayGraphicPath'),('Beech_Leafless','leaflessSnowOverlayGraphicPath')]:
    template=root/f'Art/Templates/{name}-Snow-v1/template.json'
    base,mask=fixed_template.load_template(template,allow_inactive=True)
    source=root/f'Textures/Things/Plant/AMJ/{name}/{name}_A.png'
    assert Image.open(source).convert('RGBA').tobytes()==base.tobytes()
    assert plant.findtext('plant/'+field)==f'Things/Plant/AMJ/{name}_Snow'
    production=root/f'Textures/Things/Plant/AMJ/{name}_Snow/{name}_Snow_A.png'
    assert production.read_bytes()==(template.parent/'snow-overlay.png').read_bytes()
    layer=Image.open(production).convert('RGBA')
    assert all(not a or m==255 for a,m in zip(layer.getchannel('A').tobytes(),mask.tobytes()))
    preview=Image.alpha_composite(base,layer)
    fixed_template.validate(base,mask,preview)
    assert preview.tobytes()==Image.open(template.parent/'exact-composite.png').convert('RGBA').tobytes()
    assert any(a and not m for a,m in zip(ImageChops.offset(layer,12,0).getchannel('A').tobytes(),mask.tobytes()))
    if name=='Beech_Leafless':
        for corridor in LEAFLESS_BRANCHES:
            for x,y in branch_surface(base,corridor)[2:-2]:
                # Snow must hide the original upper outline and adjacent wood,
                # not merely sit behind/touch the branch silhouette.
                assert min(layer.getpixel((x,y))[3],layer.getpixel((x,y+1))[3])>=245, 'Original upper branch face leaks in front of snow'
        reversed_order=Image.alpha_composite(layer,base)
        assert reversed_order.tobytes()!=preview.tobytes(), 'Snow layer-order regression fixture ineffective'
        print('[OK] Snow hides upper branch face; wood-over-snow order rejected')
        count, area = validate_leafless_massing(layer,base)
        print('[OK] Leafless snow supported outlined masses:', count, 'solid pixels:', area)
        spec=json.loads(template.read_text(encoding='utf-8'))
        ledger=json.loads((root/'Docs/PlantVisualCoverage.json').read_text(encoding='utf-8'))
        state=ledger['plants']['AMJ_Tree_Beech']['states']['snow']
        if state['status']=='pending':
            assert spec['production_status'] in ('review','source_approved') and 'filled_exemplar' not in spec
            if spec['production_status']=='source_approved':
                approved=spec['approved_source']
                assert approved['sha256']==hashlib.sha256((template.parent/approved['path']).read_bytes()).hexdigest()
                assert approved['author_statement'] and approved['date']
                assert state['source_art_status']=='accepted'
                print('[OK] Approved source frozen; native appearance still pending')
            assert state['leafy_variant_status']=='accepted'
            print('[OK] Leafless native review pending; leafy approval retained')
    print('[OK]',name,'exact installed overlay; protected RGBA=0; 12px shifted overlay rejected')
