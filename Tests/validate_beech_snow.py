"""Verify AMJE leafy beech snow and intentional snow-free leafless trees.

Author decision 2026-11-01 supersedes legacy leafless snow templates.
Old accepted source/template PNGs remain archived but must never be used by live XML.
"""
import struct
from pathlib import Path
import xml.etree.ElementTree as ET
from PIL import Image,ImageChops
import sys
ROOT=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(ROOT/'Tests/Tools'))
import fixed_template

defs=ET.parse(ROOT/'Defs/ThingDefs_Plants/AMJ_WildPlants.xml').getroot()
NO_SNOW="Things/Plant/AMJ/LeaflessNoSnow"
empty_folder=ROOT/'Textures'/NO_SNOW
png=empty_folder/'LeaflessNoSnow_A.png'
dds=empty_folder/'LeaflessNoSnow_A.dds'
assert png.is_file() and dds.is_file(), 'Transparent leafless fallback guard missing'
with Image.open(png) as im:
    assert im.size==(256,256) and im.mode=='RGBA'
    assert im.getchannel('A').getextrema()==(0,0), 'PNG draws snow on bare branches'
with Image.open(dds) as im:
    assert im.size==(256,256)
    assert im.convert('RGBA').getchannel('A').getextrema()==(0,0), 'DDS draws snow on bare branches'
raw=dds.read_bytes()
assert raw[:4]==b'DDS ' and raw[84:88]==b'DXT5'
assert struct.unpack_from('<I',raw,28)[0]==9, 'DDS mipmaps missing'
assert len(raw)==87536, 'Unexpected DDS size or mip data'
for name in ('Beech','Hannoki'):
    defn=next(p for p in defs if p.findtext('defName')=='AMJ_Tree_'+name)
    assert defn.findtext('plant/leaflessGraphicPath')==f'Things/Plant/AMJ/{name}_Leafless'
    assert defn.findtext('plant/snowOverlayGraphicPath')==f'Things/Plant/AMJ/{name}_Snow', 'Leafy snow changed'
    assert defn.findtext('plant/leaflessSnowOverlayGraphicPath')==NO_SNOW, 'Missing transparent override causes leafy snow fallback'
    assert not any('Leafless_Snow' in (x.text or '') for x in defn.findall('plant/*')), 'Retired leafless snow remains live'
    print('[OK]',name,'keeps original leafless tree, keeps leafy snow, selects transparent leafless overlay')
# Continue guarding the accepted leafy Beech overlay (independent of the retired old leafless template).
template=ROOT/'Art/Templates/Beech-Snow-v1/template.json'
base,mask=fixed_template.load_template(template,allow_inactive=True)
source=ROOT/'Textures/Things/Plant/AMJ/Beech/Beech_A.png'
assert Image.open(source).convert('RGBA').tobytes()==base.tobytes()
production=ROOT/'Textures/Things/Plant/AMJ/Beech_Snow/Beech_Snow_A.png'
assert production.read_bytes()==(template.parent/'snow-overlay.png').read_bytes()
layer=Image.open(production).convert('RGBA')
assert all(not a or m==255 for a,m in zip(layer.getchannel('A').tobytes(),mask.tobytes()))
preview=Image.alpha_composite(base,layer)
fixed_template.validate(base,mask,preview)
assert preview.tobytes()==Image.open(template.parent/'exact-composite.png').convert('RGBA').tobytes()
assert any(a and not m for a,m in zip(ImageChops.offset(layer,12,0).getchannel('A').tobytes(),mask.tobytes()))
print('[OK] Accepted Beech leafy snow overlay unchanged, 12px shifted overlay rejected')
print('[OK] Native rendered bare-branch snow absence still requires a real screenshot')
