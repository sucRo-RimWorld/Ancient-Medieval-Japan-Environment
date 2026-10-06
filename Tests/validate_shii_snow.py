"""Check fixed master pixels and exact installed Shii snow registration."""
import hashlib
import sys
from pathlib import Path
from PIL import Image, ImageChops
import xml.etree.ElementTree as ET

root=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(root/'Tests/Tools'))
import fixed_template
template=root/'Art/Templates/Shii-Snow-v1/template.json'
master,mask=fixed_template.load_template(template,allow_inactive=True)
original=root/'Textures/Things/Plant/AMJ/Shii/Shii_A.png'
assert hashlib.sha256(original.read_bytes()).hexdigest()=='c40603c96e357f210ac394bc5c62f89449b37d43793262d692b604488c28d809'
assert Image.open(original).convert('RGBA').tobytes()==master.tobytes()
plant=next(p for p in ET.parse(root/'Defs/ThingDefs_Plants/AMJ_WildPlants.xml').getroot() if p.findtext('defName')=='AMJ_Tree_Shii')
assert plant.findtext('plant/snowOverlayGraphicPath')=='Things/Plant/AMJ/Shii_Snow'
snow=root/'Textures/Things/Plant/AMJ/Shii_Snow/Shii_Snow_A.png'
assert hashlib.sha256(snow.read_bytes()).hexdigest()=='5d9376ced78552f64631c89eda662aef51bd6faff4f1e098238ca0aa3c4b8bb7'
import json
spec=json.loads(template.read_text(encoding='utf-8'))
assert 'crown_anchors' not in spec and len(spec['pad_bboxes'])==6, 'Rejected geometric crown partitions returned'
layer=Image.open(snow).convert('RGBA')
assert layer.size==(256,256)
assert all(not a or m==255 for a,m in zip(layer.getchannel('A').tobytes(),mask.tobytes()))
assert sum(a>0 for a in layer.getchannel('A').tobytes())>=11000, 'Heavy snow coverage regressed'
composite=Image.alpha_composite(master,layer)
fixed_template.validate(master,mask,composite)
assert composite.tobytes()==Image.open(template.parent/'exact-composite.png').convert('RGBA').tobytes()
shifted=ImageChops.offset(layer,1,0)
assert any(a and not m for a,m in zip(shifted.getchannel('A').tobytes(),mask.tobytes()))
print('[OK] Shii master intact; protected RGBA differences=0; one-pixel shift rejected')
