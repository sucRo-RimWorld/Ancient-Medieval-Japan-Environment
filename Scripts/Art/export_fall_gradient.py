"""Export an accepted LUT without changing PNG bytes or decoded DDS colors."""
import hashlib
import shutil
from pathlib import Path
from PIL import Image

ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / 'Art/Sources/Other/AMJE_BeechFallGradient.png'
TARGET = ROOT / 'Textures/Other/AMJE_BeechFallGradient.png'


def validate_pair(source, png, dds):
    if source.read_bytes() != png.read_bytes():
        raise ValueError('Production PNG differs from accepted source bytes')
    with Image.open(png) as image:
        rgba = image.convert('RGBA')
        if rgba.size != (256, 256) or rgba.getextrema()[3] != (255, 255):
            raise ValueError('Beech LUT must be opaque 256x256')
        row = rgba.crop((0, 0, 256, 1)).tobytes()
        if rgba.tobytes() != row * 256:
            raise ValueError('Beech LUT must vary horizontally only')
        expected = rgba.tobytes()
    with Image.open(dds) as image:
        if image.size != (256, 256) or image.convert('RGBA').tobytes() != expected:
            raise ValueError('DDS has stale, shifted or differently decoded colors')


def export():
    before = SOURCE.read_bytes()
    TARGET.parent.mkdir(parents=True, exist_ok=True)
    shutil.copyfile(SOURCE, TARGET)
    with Image.open(SOURCE) as image:
        # Standard uncompressed RGBA DDS is supported by RimWorld 1.6's native
        # ModDdsLoader; avoid block-compression changes to the accepted LUT.
        image.convert('RGBA').save(TARGET.with_suffix('.dds'), format='DDS')
    validate_pair(SOURCE, TARGET, TARGET.with_suffix('.dds'))
    assert SOURCE.read_bytes() == before, 'Accepted source was mutated'
    print('[OK] Exact PNG and decoded-RGBA-identical DDS exported; source SHA256=' +
          hashlib.sha256(before).hexdigest())


if __name__ == '__main__':
    export()
