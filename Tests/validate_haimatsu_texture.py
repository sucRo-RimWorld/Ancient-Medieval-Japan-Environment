"""Lock the approved Haimatsu texture and active Graphic_Random path."""
from pathlib import Path
import hashlib
import struct
import xml.etree.ElementTree as ET
import zlib

root = Path(__file__).resolve().parents[1]
plant = next(x for x in ET.parse(root / 'Defs/ThingDefs_Plants/AMJ_WildPlants.xml').getroot() if x.findtext('defName') == 'AMJ_Shrub_Haimatsu')
assert plant.findtext('graphicData/texPath') == 'Things/Plant/AMJ/Haimatsu'
assert plant.findtext('graphicData/graphicClass') == 'Graphic_Random'
b = (root / 'Textures/Things/Plant/AMJ/Haimatsu/Haimatsu_A.png').read_bytes()
assert hashlib.sha256(b).hexdigest() == '44d22e74670bdfe081ef98c8a327700620f3d285e4a86b6dc52625c57daef551'
assert b[:8] == b'\x89PNG\r\n\x1a\n'
o = 8
payload = bytearray()
while o < len(b):
    n = struct.unpack('>I', b[o:o+4])[0]
    kind, data = b[o+4:o+8], b[o+8:o+8+n]
    assert zlib.crc32(kind + data) & 0xffffffff == struct.unpack('>I', b[o+8+n:o+12+n])[0]
    if kind == b'IHDR':
        assert struct.unpack('>IIBBBBB', data) == (256, 256, 8, 6, 0, 0, 0)
    if kind == b'IDAT':
        payload.extend(data)
    o += n + 12
assert o == len(b)
raw = zlib.decompress(payload)
assert len(raw) == 256 * 1025
assert all(raw[i * 1025] <= 4 for i in range(256))
print('[OK] Approved Haimatsu PNG and Def integration')
