"""Build a pinned, clean Git payload; verify an exact extracted upload root."""
import argparse
import hashlib
import json
from pathlib import Path
import subprocess
import xml.etree.ElementTree as ET
import zipfile
import importlib.util

ROOT = Path(__file__).resolve().parents[1]
PACKAGE = 'sucro.ancientmedievaljapan.environment'
WORKSHOP = '3814638060'
FOLDERS = ('About', 'Defs', 'Languages', 'Patches', 'Textures', 'Sounds')
EXCLUDED = ('Art', 'TestResults', 'Tests', 'Scripts', 'Source', 'DevQuickstarts', '.git', '.github')
LOAD = b'<?xml version="1.0" encoding="utf-8"?>\n<loadFolders><v1.6><li>/</li></v1.6></loadFolders>\n'
_spec = importlib.util.spec_from_file_location('subscriber', ROOT / 'Tests/validate_workshop_payload.py')
subscriber = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(subscriber)


def git(*args):
    return subprocess.check_output(['git', '-C', str(ROOT), *args], text=True, encoding='utf-8').strip()


def digest(data):
    return hashlib.sha256(data).hexdigest()


def inventory(root):
    return {p.relative_to(root).as_posix(): digest(p.read_bytes())
            for p in sorted(root.rglob('*')) if p.is_file()}


def validate(data):
    about = ET.fromstring(data['About/About.xml'])
    if about.findtext('packageId') != PACKAGE or data['About/PublishedFileId.txt'].decode().strip() != WORKSHOP:
        raise ValueError('Wrong publication identity')
    if any(c in about.findtext('name', '') for c in ':：'):
        raise ValueError('Windows/YADA-incompatible display name')
    if any(k.split('/')[0] in EXCLUDED for k in data):
        raise ValueError('Development content in payload')
    if any(not subscriber.subscriber_file(k) for k in data):
        raise ValueError('Subscriber-unnecessary file in payload')
    if data['loadFolders.xml'] != LOAD:
        raise ValueError('Distribution must load only root')
    if not data.get('Assemblies/AncientMedievalJapanEnvironment.dll', b'').startswith(b'MZ'):
        raise ValueError('Production assembly missing')
    plants = ET.fromstring(data['Defs/ThingDefs_Plants/AMJ_WildPlants.xml'])
    haimatsu = next(p for p in plants if p.findtext('defName') == 'AMJ_Shrub_Haimatsu')
    for field, expected in {'harvestedThingDef': 'WoodLog', 'harvestYield': '8', 'harvestTag': 'Wood'}.items():
        if haimatsu.findtext('plant/' + field) != expected:
            raise ValueError('Missing current Haimatsu cutting contract: ' + field)
    for plant in plants:
        for field in ('graphicData/texPath', 'plant/leaflessGraphicPath', 'plant/snowOverlayGraphicPath', 'plant/leaflessSnowOverlayGraphicPath'):
            path = plant.findtext(field)
            if path and not any(k.startswith('Textures/' + path + '/') and k.endswith('.png') for k in data):
                raise ValueError('Missing plant state: ' + path)


def verify(root, manifest_path):
    manifest = json.loads(manifest_path.read_text(encoding='utf-8'))
    actual = inventory(root)
    if actual != manifest['files']:
        missing = sorted(set(manifest['files']) - set(actual))
        extra = sorted(set(actual) - set(manifest['files']))
        changed = sorted(k for k in actual.keys() & manifest['files'].keys() if actual[k] != manifest['files'][k])
        raise ValueError(f'Payload differs: missing={missing}, extra={extra}, changed={changed}')
    validate({k: (root / k).read_bytes() for k in actual})
    print('PASS exact payload:', len(actual), 'files; source', manifest['source_commit'])
    return manifest


def build(output, expected_commit, game, preview):
    head = git('rev-parse', 'HEAD')
    if head != expected_commit or git('diff', 'HEAD', '--name-only'):
        raise ValueError('Requires exact pinned HEAD and no tracked modifications')
    if output.exists():
        raise ValueError('Output exists; use a fresh directory')
    tracked = git('ls-files').splitlines()
    compiled_sources = {p.relative_to(ROOT).as_posix() for p in (ROOT / 'Source/AncientMedievalJapanEnvironment').rglob('*.cs')}
    if compiled_sources - set(tracked):
        raise ValueError('Untracked production C# source: ' + str(sorted(compiled_sources - set(tracked))))
    rules = subscriber.patterns((ROOT / '.rimignore').read_text(encoding='utf-8-sig'))
    kept, leaks, lost = subscriber.audit(tracked, rules)
    if leaks or lost:
        raise ValueError('Shared subscriber policy failure: ' + str((leaks, lost)))
    # Runtime files must be tracked; stale untracked PNG/XML cannot enter a release.
    for folder in FOLDERS:
        disk = {p.relative_to(ROOT).as_posix() for p in (ROOT / folder).rglob('*') if p.is_file()}
        if disk - set(tracked):
            raise ValueError('Untracked runtime content: ' + str(sorted(disk - set(tracked))))
    # Build here, never accept an existing developer DLL as provenance.
    subprocess.run(['cmd.exe', '/d', '/c', str(ROOT / 'build.bat'), str(game)], cwd=ROOT, check=True)
    if git('rev-parse', 'HEAD') != head or git('diff', 'HEAD', '--name-only'):
        raise ValueError('Build modified tracked inputs')
    data = {k: (ROOT / k).read_bytes() for k in kept if not k.startswith('Assemblies/') and k != 'loadFolders.xml'}
    data['Assemblies/AncientMedievalJapanEnvironment.dll'] = (ROOT / 'Assemblies/AncientMedievalJapanEnvironment.dll').read_bytes()
    data['About/PublishedFileId.txt'] = (WORKSHOP + '\n').encode()
    preview_bytes = preview.read_bytes()
    if not preview_bytes.startswith(b'\x89PNG\r\n\x1a\n'):
        raise ValueError('Approved preview must be PNG')
    data['About/Preview.png'] = preview_bytes
    data['loadFolders.xml'] = LOAD
    # Root .rimignore is policy/provenance input, never a subscriber file.
    validate(data)
    output.mkdir(parents=True)
    payload = output / 'AncientMedievalJapanEnvironment'
    for name, content in data.items():
        path = payload / name
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_bytes(content)
    archive = output / 'AncientMedievalJapanEnvironment.zip'
    with zipfile.ZipFile(archive, 'w', zipfile.ZIP_DEFLATED) as z:
        for name, content in sorted(data.items()):
            z.writestr('AncientMedievalJapanEnvironment/' + name, content)
    manifest = {'schema': 1, 'source_commit': head, 'source_tree': git('rev-parse', 'HEAD^{tree}'),
                'packageId': PACKAGE, 'workshopId': WORKSHOP, 'archive_sha256': digest(archive.read_bytes()),
                'subscriber_filter_sha256': digest((ROOT / '.rimignore').read_bytes()),
                'files': inventory(payload), 'built_from': 'clean pinned Git checkout; build.bat executed by builder',
                'approved_preview_sha256': digest(preview_bytes),
                'runtime_validation': 'pending; build/static evidence only', 'steam_publication': 'author-manual; not uploaded'}
    manifest_path = output / 'Manifest.json'
    manifest_path.write_text(json.dumps(manifest, indent=2) + '\n', encoding='utf-8')
    with zipfile.ZipFile(archive) as z:
        if z.testzip() is not None or len(z.namelist()) != len(data):
            raise ValueError('Invalid archive')
        for name, content in data.items():
            if z.read('AncientMedievalJapanEnvironment/' + name) != content:
                raise ValueError('Archive mismatch')
    verify(payload, manifest_path)
    subprocess.run(['python', str(ROOT / 'Tests/validate_workshop_payload.py'), '--payload', str(payload),
                    '--expected-assembly', 'AncientMedievalJapanEnvironment.dll'], check=True)
    print('Candidate:', payload)
    print('Archive SHA256:', manifest['archive_sha256'])


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    sub = parser.add_subparsers(dest='action', required=True)
    make = sub.add_parser('build')
    make.add_argument('--output', type=Path, required=True)
    make.add_argument('--expected-commit', required=True)
    make.add_argument('--preview', type=Path, required=True, help='Existing author-approved Workshop PNG; recorded separately from Git source')
    make.add_argument('--game', type=Path, default=Path('D:/SteamLibrary/steamapps/common/RimWorld'))
    check = sub.add_parser('verify')
    check.add_argument('root', type=Path)
    check.add_argument('manifest', type=Path)
    args = parser.parse_args()
    if args.action == 'build':
        build(args.output.resolve(), args.expected_commit, args.game.resolve(), args.preview.resolve())
    else:
        verify(args.root.resolve(), args.manifest.resolve())
