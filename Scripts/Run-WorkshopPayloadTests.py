"""Four-profile hidden rendered release gate for an exact candidate or Steam root."""
import argparse
import importlib.util
import json
import os
from pathlib import Path
import re
import shutil
import subprocess
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location('payload', ROOT / 'Scripts/Build-WorkshopPayload.py')
payload_tool = importlib.util.module_from_spec(spec)
spec.loader.exec_module(payload_tool)
PROFILES = ('Vanilla', 'MO', 'CCTO', 'MO-CCTO')
SCENARIOS = ('WarmTemperateTerrain', 'CoolTemperateTerrain', 'SubalpineTerrain', 'AlpineTerrain', 'RiverMapHandoff', 'CoastMapHandoff')

# Exact counts for current Quickstarts. Sixteen tree-sowing checks were
# added in each forest biome; WarmTemperate has six extra tree descriptions.
# CCTO contributes twelve additional assertions in each forest biome.
MAP_ASSERTIONS_BASE = (80, 73, 70, 68, 3, 3)
MAP_ASSERTIONS_CCTO = (92, 85, 82, 80, 3, 3)


def expected_map_assertions(mode):
    if mode not in PROFILES:
        raise ValueError('Unknown release profile: ' + str(mode))
    return MAP_ASSERTIONS_CCTO if 'CCTO' in mode else MAP_ASSERTIONS_BASE



def call(args, **kwargs):
    subprocess.run([str(a) for a in args], check=True, **kwargs)


def run(payload, manifest, output, game, steam, profiles=PROFILES):
    if 'RimWorldWin64.exe' in subprocess.check_output(['tasklist', '/FI', 'IMAGENAME eq RimWorldWin64.exe'], text=True):
        raise ValueError('An existing game must close before release tests')
    if output.exists():
        raise ValueError('Use a fresh result directory')
    if steam and payload != (game.parents[1] / 'workshop/content/294100/3814638060').resolve():
        raise ValueError('--steam requires the actual installed Workshop3814638060 root')
    if not steam and manifest is None:
        raise ValueError('Candidate testing requires its exact publication manifest')
    if manifest:
        payload_tool.verify(payload, manifest)
    before = payload_tool.inventory(payload)
    normal = Path(os.environ['USERPROFILE']) / 'AppData/LocalLow/Ludeon Studios/RimWorld by Ludeon Studios/Config'
    normal_before = {name: payload_tool.digest((normal / name).read_bytes()) for name in ('ModsConfig.xml', 'Prefs.xml')}
    mods = game / 'Mods'
    observer = mods / 'AMJE.PayloadGateObserver'
    error_observer = mods / 'AMJE.PayloadGateEarlyErrors'
    fixture = mods / 'AMJE.PayloadGateCandidate'
    if observer.exists() or fixture.exists() or error_observer.exists():
        raise ValueError('Existing release fixtures require inspection')
    output.mkdir(parents=True)
    (output / 'Before.json').write_text(json.dumps({'payload': before, 'normal': normal_before}, indent=2))
    selected = payload
    owned = []
    try:
        if not steam:
            shutil.copytree(payload, fixture); owned.append(fixture); selected = fixture
            about = ET.parse(fixture / 'About/About.xml')
            about.find('packageId').text = 'sucro.ancientmedievaljapan.environment.releasevalidation'
            about.find('name').text = 'AMJE payload gate candidate (temporary)'
            about.write(fixture / 'About/About.xml', encoding='utf-8', xml_declaration=True)
        selected_before = payload_tool.inventory(selected)
        observer.mkdir(); owned.append(observer)
        (observer / 'About').mkdir(); (observer / 'Assemblies').mkdir()
        (observer / 'About/About.xml').write_text('<ModMetaData><name>AMJE payload gate observer</name><author>AMJ testing</author><packageId>sucro.amje.payloadgateobserver</packageId><supportedVersions><li>1.6</li></supportedVersions></ModMetaData>')
        generated = output / 'Generated'
        call(['python', ROOT / 'Scripts/Stage-WorkshopRegressionSources.py', generated])
        managed = game / 'RimWorldWin64_Data/Managed'
        workshop = game.parents[1] / 'workshop/content/294100'
        csc = Path(os.environ['WINDIR']) / 'Microsoft.NET/Framework64/v4.0.30319/csc.exe'
        references = [managed / x for x in ('Assembly-CSharp.dll', 'UnityEngine.CoreModule.dll', 'UnityEngine.IMGUIModule.dll', 'Unity.Mathematics.dll', 'Unity.Collections.dll', 'netstandard.dll')]
        references += [next((workshop / mod).rglob(dll)) for mod, dll in [('2009463077', '0Harmony.dll'), ('3793646067', 'Quickstarts.dll')]]
        call([csc, '/nologo', '/target:library', '/out:' + str(observer / 'Assemblies/ReleaseGate.dll'),
              *['/reference:' + str(p) for p in references], generated / 'WorkshopHarvestQuickstarts.cs', ROOT / 'Tests/Release/WorkshopSourceAudit.cs'])
        error_observer.mkdir(); owned.append(error_observer)
        (error_observer / 'About').mkdir(); (error_observer / 'Assemblies').mkdir()
        (error_observer / 'About/About.xml').write_text('<ModMetaData><name>AMJE early Unity errors (temporary)</name><author>AMJ testing</author><packageId>sucro.amje.payloadgateerrors</packageId><supportedVersions><li>1.6</li></supportedVersions></ModMetaData>')
        call([csc, '/nologo', '/target:library', '/out:' + str(error_observer / 'Assemblies/EarlyErrors.dll'),
              *['/reference:' + str(managed / p) for p in ('Assembly-CSharp.dll', 'UnityEngine.CoreModule.dll', 'netstandard.dll')], ROOT / 'Tests/Release/HarvestErrorObserver.cs'])
        desktop = output / 'IsolatedDesktopRunner.exe'
        call([csc, '/nologo', '/target:exe', '/out:' + str(desktop), ROOT / 'Tests/Release/IsolatedDesktopRunner.cs'])
        summary = {'source': 'Steam downloaded root' if steam else 'main candidate; only fixture About identity changed', 'root': str(payload), 'profiles': {}}
        for mode in profiles:
            config_dir = output / ('SaveData-' + mode) / 'Config'; config_dir.mkdir(parents=True)
            config = ET.parse(normal / 'ModsConfig.xml'); active = config.find('activeMods'); active.clear()
            ids = ['brrainz.harmony', 'ludeon.rimworld', 'sucro.amje.payloadgateerrors', 'rimworks.rimlogging', 'rimworks.quickstarts']
            if mode.startswith('MO'):
                ids += ['oskarpotocki.vanillafactionsexpanded.core', 'syrchalis.processor.framework', 'dankpyon.medieval.overhaul']
            if 'CCTO' in mode:
                ids += ['sucro.cropcoldtoleranceoverhaul_steam']
            ids += ['sucro.ancientmedievaljapan.environment_steam' if steam else 'sucro.ancientmedievaljapan.environment.releasevalidation']
            ids += ['sucro.amje.payloadgateobserver']
            for value in ids: ET.SubElement(active, 'li').text = value
            config.write(config_dir / 'ModsConfig.xml', encoding='utf-8', xml_declaration=True)
            prefs = ET.parse(normal / 'Prefs.xml')
            for name, value in [('devMode', 'True'), ('resetModsConfigOnCrash', 'False')]:
                node = prefs.find(name)
                if node is None: node = ET.SubElement(prefs.getroot(), name)
                node.text = value
            prefs.write(config_dir / 'Prefs.xml', encoding='utf-8', xml_declaration=True)
            report_dir = output / ('Reports-' + mode)
            harvest_dir = output / ('Harvest-' + mode)
            errors = output / ('Errors-' + mode)
            env = os.environ.copy()
            env = {k: v for k, v in env.items() if k.lower() != 'psmodulepath'}
            env['PSMODULEPATH'] = str(Path(os.environ['WINDIR']) / 'System32/WindowsPowerShell/v1.0/Modules')
            env['AMJE_EXPECTED_PAYLOAD_ROOT'] = str(selected)
            env['RIMWORLD_AMJE_ERROR_DIRECTORY'] = str(errors)
            env.pop('RIMWORLD_AMJE_HARVEST_EXPECTED', None)
            env.pop('AMJE_TRANSLATION_OUTPUT', None)
            ps = Path(os.environ['WINDIR']) / 'System32/WindowsPowerShell/v1.0/powershell.exe'
            common = f'-ExePath "{game / "RimWorldWin64.exe"}" -SaveDataFolder "{config_dir.parent}" -TimeoutSeconds 420'
            command = f'"{ps}" -NoProfile -ExecutionPolicy Bypass -File "{generated / "Run-WorkshopHarvestQuickstarts.ps1"}" {common}'
            batch = output / (mode + '.cmd')
            batch.write_text('@echo off\n' + command + f' -ResultDir "{report_dir}"\nif errorlevel 1 exit /b 1\n'
                             + 'set "RIMWORLD_AMJE_HARVEST_EXPECTED=' + ('DankPyon_RawWood' if mode.startswith('MO') else 'WoodLog') + '"\n'
                             + command + f' -ResultDir "{harvest_dir}" -TreeTextureAuditOnly\nif errorlevel 1 exit /b 1\nexit /b 0\n', encoding='ascii')
            with (output / (mode + '-Runner.log')).open('w') as log:
                call([desktop, batch, ROOT], env=env, stdout=log, stderr=subprocess.STDOUT)
            counts = expected_map_assertions(mode)
            totals = []
            for directory, scenarios, expected in [(report_dir, SCENARIOS, counts), (harvest_dir, SCENARIOS[:1], [9])]:
                for scenario, count in zip(scenarios, expected):
                    name = 'Workshop_AMJ' + scenario + 'Quickstart'
                    data = json.loads((directory / (name + '.json')).read_text(encoding='utf-8-sig'))
                    if not data['passed'] or data['total'] != count or data['failed'] or data['preLaunchErrors'] or data.get('logErrors', 0) or not data['captureLive'] or data['logTruncated']:
                        raise ValueError('Invalid report: ' + name)
                    log = (directory / (name + '.log')).read_text(encoding='utf-8-sig')
                    if ('[AMJE WorkshopSourceAudit] PASS sourceRoot=' + str(selected) not in log
                            or 'Version:  Direct3D 11.0' not in log or '-nographics' in log
                            or re.search(r'\[ERROR\]|Level:\s*ERROR', log)):
                        raise ValueError('Source/ERROR gate: ' + name)
                    found = re.findall(r'\[AMJE WorkshopSourceAudit\] MOD package=(\S+) root=', log)
                    if sorted(i.lower().removesuffix('_steam') for i in found) != sorted(i.removesuffix('_steam') for i in ids):
                        raise ValueError('Unexpected active profile: ' + str(found))
                    if directory == harvest_dir and log.count('[AMJ Environment Harvest]') != 4:
                        raise ValueError('Four native cutting outputs missing')
                    totals.append(count)
            captures = list(errors.glob('Unity-*.log'))
            if len(captures) != 7 or any('[CAPTURE_READY]' not in p.read_text(encoding='utf-8-sig') or '[ERROR]' in p.read_text(encoding='utf-8-sig') for p in captures):
                raise ValueError('Independent Unity ERROR capture incomplete')
            summary['profiles'][mode] = {'map_assertions': sum(totals[:-1]), 'cutting_assertions': totals[-1], 'runtime_errors': 0, 'reports': 7}
            (output / 'Summary.json').write_text(json.dumps(summary, indent=2))
            print('PASS', mode, summary['profiles'][mode], flush=True)
        if payload_tool.inventory(selected) != selected_before:
            raise ValueError('Test fixture changed')
    finally:
        retired = output / 'Retired'; retired.mkdir(exist_ok=True)
        for target in owned:
            if target.parent != mods or target.name not in ('AMJE.PayloadGateObserver', 'AMJE.PayloadGateCandidate', 'AMJE.PayloadGateEarlyErrors'):
                raise ValueError('Unsafe retirement target')
            shutil.move(str(target), retired / target.name)
        if before != payload_tool.inventory(payload) or any(payload_tool.digest((normal / name).read_bytes()) != value for name, value in normal_before.items()):
            raise ValueError('Original payload or normal configuration changed')
        (output / 'Preservation.json').write_text(json.dumps({'payload_unchanged': True, 'normal_config_unchanged': True}))
    summary['passed'] = True
    summary['full_four_profile_gate'] = set(profiles) == set(PROFILES)
    (output / 'Summary.json').write_text(json.dumps(summary, indent=2))


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--payload', type=Path, required=True)
    parser.add_argument('--manifest', type=Path)
    parser.add_argument('--output', type=Path, required=True)
    parser.add_argument('--game', type=Path, default=Path('D:/SteamLibrary/steamapps/common/RimWorld'))
    parser.add_argument('--steam', action='store_true')
    parser.add_argument('--profiles', nargs='+', choices=PROFILES, default=list(PROFILES), help='Focused rerun; fewer than four profiles cannot alone clear the release gate')
    args = parser.parse_args()
    run(args.payload.resolve(), args.manifest.resolve() if args.manifest else None, args.output.resolve(), args.game.resolve(), args.steam, args.profiles)
