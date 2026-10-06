"""Stage test-only current-main observers without replacing the Workshop payload.

Compile the emitted C# as a separate observer mod. Use the renamed runner for
harvest regressions, so shipped developer Quickstarts can coexist unchanged.
"""
import argparse
import pathlib
import re
import shutil


def stage(output):
    repository = pathlib.Path(__file__).resolve().parents[1]
    output.mkdir(parents=True, exist_ok=True)
    source = (repository / 'Tests/Quickstarts/EnvironmentBiomeTerrainQuickstarts.cs').read_text(encoding='utf-8-sig')
    source = source.replace('AncientMedievalJapan.Environment.Quicktests', 'AncientMedievalJapan.Environment.WorkshopFinaltests')
    source = source.replace('sucro.ancientmedievaljapan.environment.quicktests', 'sucro.ancientmedievaljapan.environment.workshopfinaltests')
    source = re.sub(r'\bAMJ[A-Za-z0-9_]*Quickstart\b', lambda match: 'Workshop_' + match[0], source)
    (output / 'WorkshopHarvestQuickstarts.cs').write_text(source, encoding='utf-8')
    runner = (repository / 'Scripts/Run-EnvironmentVegetationQuickstarts.ps1').read_text(encoding='utf-8-sig')
    runner = re.sub(r'\bAMJ[A-Za-z0-9_]*Quickstart\b', lambda match: 'Workshop_' + match[0], runner)
    (output / 'Run-WorkshopHarvestQuickstarts.ps1').write_text(runner, encoding='utf-8')
    for name in ['Run-EnvironmentVegetationQuickstarts.ps1', 'Validate-EnvironmentRuntimeLog.ps1']:
        shutil.copyfile(repository / 'Scripts' / name, output / name)
    for name in ['WorkshopSourceAudit.cs', 'IsolatedDesktopRunner.cs']:
        shutil.copyfile(repository / 'Tests/Release' / name, output / name)
    print(f'Staged observer sources and existing runners: {output}')


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('output', type=pathlib.Path)
    stage(parser.parse_args().output.resolve())
