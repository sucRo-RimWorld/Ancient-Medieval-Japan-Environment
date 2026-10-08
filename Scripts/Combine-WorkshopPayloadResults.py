"""Recheck full logs from completed profiles of one unchanged release payload."""
import argparse
import importlib.util
import json
from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location('gate', ROOT / 'Scripts/Run-WorkshopPayloadTests.py')
gate = importlib.util.module_from_spec(spec); spec.loader.exec_module(gate)


def combine(payload, manifest_path, results, output, game, steam):
    if output.exists(): raise ValueError('Use a fresh combined report')
    manifest = gate.payload_tool.verify(payload, manifest_path)
    actual = gate.payload_tool.inventory(payload)
    selected = payload if steam else game / 'Mods/AMJE.PayloadGateCandidate'
    dependencies = {'brrainz.harmony': '2009463077', 'rimworks.rimlogging': '3733484696', 'rimworks.quickstarts': '3793646067'}
    profiles = {}
    for result in results:
        summary = json.loads((result / 'Summary.json').read_text())
        before = json.loads((result / 'Before.json').read_text())
        preserved = json.loads((result / 'Preservation.json').read_text())
        if summary['root'] != str(payload) or before['payload'] != actual or not all(preserved.values()):
            raise ValueError('Mixed payload or unpreserved run: ' + str(result))
        if not steam:
            tested = gate.payload_tool.inventory(result / 'Retired/AMJE.PayloadGateCandidate')
            if set(tested) != set(actual) or any(tested[k] != actual[k] for k in actual if k != 'About/About.xml'):
                raise ValueError('Retired fixture runtime bytes differ from publication payload')
        for mode in summary['profiles']:
            if mode not in gate.PROFILES: raise ValueError('Unknown profile')
            counts = gate.expected_map_assertions(mode)
            required = dict(dependencies)
            if mode.startswith('MO'):
                required.update({'dankpyon.medieval.overhaul': '3219596926', 'oskarpotocki.vanillafactionsexpanded.core': '2023507013', 'syrchalis.processor.framework': '3210544395'})
            if 'CCTO' in mode: required['sucro.cropcoldtoleranceoverhaul'] = '3812412548'
            expected_ids = set(required) | {'ludeon.rimworld', 'sucro.amje.payloadgateerrors', 'sucro.amje.payloadgateobserver',
                                         'sucro.ancientmedievaljapan.environment' if steam else 'sucro.ancientmedievaljapan.environment.releasevalidation'}
            evidence = []
            for folder, scenarios, totals in [('Reports-' + mode, gate.SCENARIOS, counts), ('Harvest-' + mode, gate.SCENARIOS[:1], [9])]:
                for scenario, total in zip(scenarios, totals):
                    name = 'Workshop_AMJ' + scenario + 'Quickstart'
                    path = result / folder / (name + '.json')
                    data = json.loads(path.read_text(encoding='utf-8-sig'))
                    if not data['passed'] or data['total'] != total or data['failed'] or data['preLaunchErrors'] or data.get('logErrors', 0) or not data['captureLive'] or data['logTruncated']:
                        raise ValueError('Incomplete report: ' + str(path))
                    log = path.with_suffix('.log').read_text(encoding='utf-8-sig')
                    if '[AMJE WorkshopSourceAudit] PASS sourceRoot=' + str(selected) not in log or 'Version:  Direct3D 11.0' not in log or '-nographics' in log or re.search(r'\[ERROR\]|Level:\s*ERROR', log):
                        raise ValueError('Source/render/error failure: ' + str(path))
                    found = re.findall(r'\[AMJE WorkshopSourceAudit\] MOD package=(\S+) root=', log)
                    if len(found) != len(expected_ids) or {i.lower().removesuffix('_steam') for i in found} != expected_ids:
                        raise ValueError('Unexpected active profile')
                    for package, identifier in required.items():
                        expected_root = game.parents[1] / 'workshop/content/294100' / identifier
                        rows = [line.lower() for line in log.splitlines() if '[AMJE WorkshopSourceAudit] MOD package=' in line]
                        if not any('package=' + package in line and ' root=' + str(expected_root).lower() + ' ' in line for line in rows):
                            raise ValueError('Dependency is not the real Workshop root: ' + package)
                    if folder.startswith('Harvest') and log.count('[AMJ Environment Harvest]') != 4:
                        raise ValueError('Native output evidence missing')
                    evidence.append(str(path))
            errors = list((result / ('Errors-' + mode)).glob('Unity-*.log'))
            if len(errors) != 7 or any('[CAPTURE_READY]' not in p.read_text() or '[ERROR]' in p.read_text() for p in errors):
                raise ValueError('Independent error evidence incomplete')
            profiles[mode] = {'map_assertions': sum(counts), 'native_cutting_assertions': 9, 'runtime_errors': 0, 'reports': evidence}
    if set(profiles) != set(gate.PROFILES):
        raise ValueError('Four fully completed profiles required')
    report = {'passed': True, 'source': 'actual downloaded Workshop' if steam else 'candidate with About-only fixture identity',
              'source_commit': manifest['source_commit'], 'archive_sha256': manifest['archive_sha256'],
              'payload_root': str(payload), 'payload_files': len(actual), 'profiles': profiles,
              'steam_release_cleared': steam, 'failed_attempts': 'Preserved separately; excluded only when their profiles did not complete the full gate'}
    output.parent.mkdir(parents=True, exist_ok=True)
    output.write_text(json.dumps(report, indent=2) + '\n')
    print('PASS four-profile source/render/cutting/error/preservation gate:', output)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--payload', type=Path, required=True)
    parser.add_argument('--manifest', type=Path, required=True)
    parser.add_argument('--results', nargs='+', type=Path, required=True)
    parser.add_argument('--output', type=Path, required=True)
    parser.add_argument('--game', type=Path, default=Path('D:/SteamLibrary/steamapps/common/RimWorld'))
    parser.add_argument('--steam', action='store_true')
    args = parser.parse_args()
    combine(args.payload.resolve(), args.manifest.resolve(), [p.resolve() for p in args.results], args.output.resolve(), args.game.resolve(), args.steam)
