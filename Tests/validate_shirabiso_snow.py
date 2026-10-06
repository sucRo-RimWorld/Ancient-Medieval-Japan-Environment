"""Check fixed master pixels and exact installed Shirabiso snow registration."""
import hashlib
import sys
from pathlib import Path
from PIL import Image, ImageChops
import xml.etree.ElementTree as ET

root=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(root/'Tests/Tools'))
import fixed_template
template=root/'Art/Templates/Shirabiso-Snow-v1/template.json'
master,mask=fixed_template.load_template(template,allow_inactive=True)
original=root/'Textures/Things/Plant/AMJ/Shirabiso/Shirabiso_A.png'
assert hashlib.sha256(original.read_bytes()).hexdigest()=='aaef0a8db34426e028fdebe4a69efd3aadbc17ed98875e1f74c1bc2da4147d3d'
assert Image.open(original).convert('RGBA').tobytes()==master.tobytes()
plant=next(p for p in ET.parse(root/'Defs/ThingDefs_Plants/AMJ_WildPlants.xml').getroot() if p.findtext('defName')=='AMJ_Tree_Shirabiso')
assert plant.findtext('plant/snowOverlayGraphicPath')=='Things/Plant/AMJ/Shirabiso_Snow'
snow=root/'Textures/Things/Plant/AMJ/Shirabiso_Snow/Shirabiso_Snow_A.png'
assert hashlib.sha256(snow.read_bytes()).hexdigest()=='49a98c4b54c6f263c019352aabdbddd7f20cb9aa6b2960e1a23c861e2df31ec6'
layer=Image.open(snow).convert('RGBA')
assert layer.size==(256,256)
assert all(not a or m==255 for a,m in zip(layer.getchannel('A').tobytes(),mask.tobytes()))
composite=Image.alpha_composite(master,layer)
fixed_template.validate(master,mask,composite)
assert composite.tobytes()==Image.open(template.parent/'exact-composite.png').convert('RGBA').tobytes()
shifted=ImageChops.offset(layer,1,0)
assert any(a and not m for a,m in zip(shifted.getchannel('A').tobytes(),mask.tobytes()))
print('[OK] Shirabiso master intact; protected RGBA differences=0; one-pixel shift rejected')
