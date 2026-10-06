"""Verify both native beech snow states and immutable template registration."""
import sys,json,hashlib
from pathlib import Path
from PIL import Image,ImageChops
import xml.etree.ElementTree as ET
root=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(root/'Tests/Tools'))
import fixed_template
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
    assert any(a and not m for a,m in zip(ImageChops.offset(layer,1,0).getchannel('A').tobytes(),mask.tobytes()))
    print('[OK]',name,'exact installed overlay; protected RGBA=0; shifted overlay rejected')
