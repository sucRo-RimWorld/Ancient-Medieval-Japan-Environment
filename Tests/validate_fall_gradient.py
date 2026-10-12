"""Catch a stale preferred DDS, changed source, or broken LUT coordinates."""
import argparse
import importlib.util
import tempfile
from pathlib import Path
from PIL import Image

ROOT = Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location('fall_export', ROOT / 'Scripts/Art/export_fall_gradient.py')
export = importlib.util.module_from_spec(spec)
spec.loader.exec_module(export)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--self-test', action='store_true')
    args = parser.parse_args()
    export.validate_pair(export.SOURCE, export.TARGET, export.TARGET.with_suffix('.dds'))
    print('[OK] Accepted LUT/production PNG exact bytes; preferred DDS exact decoded colors')
    if args.self_test:
        with tempfile.TemporaryDirectory(dir=ROOT / 'TestResults') as folder:
            tmp = Path(folder)
            source, png, dds = (tmp / name for name in ('source.png', 'lut.png', 'lut.dds'))
            original = Image.open(export.TARGET).convert('RGBA')
            for kind in ('stale-dds', 'changed-source', 'vertical-drift', 'transparent', 'wrong-size'):
                source.write_bytes(export.SOURCE.read_bytes())
                png.write_bytes(export.TARGET.read_bytes())
                dds.write_bytes(export.TARGET.with_suffix('.dds').read_bytes())
                image = original.copy()
                if kind == 'stale-dds':
                    r, g, b, a = image.getpixel((0, 0))
                    image.putpixel((0, 0), (r ^ 1, g, b, a))
                    image.save(dds, format='DDS')
                elif kind == 'changed-source':
                    source.write_bytes(b'changed-source')
                else:
                    if kind == 'vertical-drift':
                        r, g, b, a = image.getpixel((0, 0))
                        image.putpixel((0, 0), (r ^ 1, g, b, a))
                    elif kind == 'transparent':
                        image.putpixel((0, 0), (0, 0, 0, 0))
                    else:
                        image = image.crop((0, 0, 255, 256))
                    image.save(source)
                    png.write_bytes(source.read_bytes())
                    image.save(dds, format='DDS')
                try:
                    export.validate_pair(source, png, dds)
                except ValueError:
                    continue
                raise AssertionError('Mutation escaped: ' + kind)
        print('[OK] Stale DDS, source mismatch, coordinate drift, alpha and dimension failures rejected')


if __name__ == '__main__':
    main()
