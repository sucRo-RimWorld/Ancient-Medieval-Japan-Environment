"""Guard the separate snow asset and unchanged accepted base master."""
from pathlib import Path
import hashlib
import xml.etree.ElementTree as ET
from PIL import Image

root = Path(__file__).resolve().parents[1]
plant = next(p for p in ET.parse(root / 'Defs/ThingDefs_Plants/AMJ_WildPlants.xml').getroot()
             if p.findtext('defName') == 'AMJ_Shrub_Haimatsu')
assert plant.findtext('plant/snowOverlayGraphicPath') == 'Things/Plant/AMJ/Haimatsu_Snow'
assert float(plant.findtext('graphicData/drawSize')) == 2.60
base = root / 'Textures/Things/Plant/AMJ/Haimatsu/Haimatsu_A.png'
snow = root / 'Textures/Things/Plant/AMJ/Haimatsu_Snow/Haimatsu_Snow_A.png'
assert hashlib.sha256(base.read_bytes()).hexdigest() == 'a20cac361b08b19b0892d2dcdf88bf40257bf186d3661661941e090f19bf18b6'
assert hashlib.sha256(snow.read_bytes()).hexdigest() == 'f77fb8d8db633c5c3b7452927e61bbde152a531fbb351c47f0a721587eef6147'
with Image.open(snow) as image:
    image.load()
    assert image.mode == 'RGBA' and image.size == (256, 256)
    assert image.getchannel('A').getextrema() == (0, 255)
    with Image.open(base) as master:
        base_alpha = master.convert('RGBA').getchannel('A').tobytes()
    snow_alpha = image.getchannel('A').tobytes()
    snow_pixels = sum(a > 32 for a in snow_alpha)
    outside = sum(s > 32 and b <= 32 for b, s in zip(base_alpha, snow_alpha))
    overlap = sum(s > 32 and b > 32 for b, s in zip(base_alpha, snow_alpha))
    coverage = overlap / sum(b > 32 for b in base_alpha)
    assert outside == 0, 'Snow is misregistered outside the master silhouette'
    assert .30 < coverage < .65, 'Snow caps should retain visible foliage sides/branches'
    import sys
    sys.path.insert(0, str(root / 'Tests/Tools'))
    import fixed_template
    template = root / 'Art/Templates/Haimatsu-Snow-v1/template.json'
    master, mask = fixed_template.load_template(template, allow_inactive=True)
    mask_bytes = mask.tobytes()
    assert all(not a or m == 255 for a,m in zip(image.getchannel('A').tobytes(),mask_bytes)), 'Snow escapes registered pad mask'
    actual = Image.alpha_composite(master, image)
    fixed_template.validate(master, mask, actual)
    with Image.open(template.parent / 'exact-composite.png') as recorded:
        assert recorded.convert('RGBA').tobytes() == actual.tobytes(), 'Recorded preview differs from installed composite'
    if '--self-test' in sys.argv:
        from PIL import ImageChops
        shifted = ImageChops.offset(image, 1, 0)
        assert any(a and m == 0 for a,m in zip(shifted.getchannel('A').tobytes(),mask_bytes)), 'One-pixel misregistration was not detected'
        old = root / 'Art/Candidates/Haimatsu-Snow/Haimatsu_SnowOverlay_Corrected256.png'
        with Image.open(old) as previous:
            assert any(a and m == 0 for a,m in zip(previous.convert('RGBA').getchannel('A').tobytes(),mask_bytes)), 'Rejected approximate overlay passed registration gate'
        print('[OK] One-pixel shift and rejected approximate overlay fail registered-mask gate')
print('[OK] Separate transparent Haimatsu snow asset; accepted base bytes and size intact')
