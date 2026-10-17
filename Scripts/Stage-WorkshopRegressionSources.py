"""Stage test-only current-main observers without replacing the Workshop payload.

Compile the emitted C# as a separate observer mod. Use the renamed runner for
harvest regressions, so shipped developer Quickstarts can coexist unchanged.
"""
import argparse
import pathlib
import re
import shutil


PRODUCTION_PACKAGE = 'sucro.ancientmedievaljapan.environment'
CANDIDATE_PACKAGE = PRODUCTION_PACKAGE + '.releasevalidation'


def release_quickstarts(source, package_id=PRODUCTION_PACKAGE):
    if package_id not in (PRODUCTION_PACKAGE, CANDIDATE_PACKAGE):
        raise ValueError('Unsupported production observer package: ' + str(package_id))
    # Unapproved species probes belong to the disposable DEVELOPMENT fixture,
    # not the subscriber payload. Preserve all production wetland assertions.
    for helper in ('AddWetlandBehaviorProbeAssertions', 'AddHannokiBehaviorProbeAssertions'):
        call = '            ' + helper + '(result, Find.CurrentMap);\n'
        if source.count(call) != 2:
            raise ValueError('Review development probe routing before release staging: ' + helper)
        source = source.replace(call, '')
    ownership = '"' + PRODUCTION_PACKAGE + '"'
    # The wetland herb production assertion now checks the same owner as Hannoki.
    # Both literal source checks must be re-targeted together in the release observer.
    if source.count(ownership) != 2:
        raise ValueError('Review production Hannoki and wetland ownership assertions before release staging')
    source = source.replace(ownership, '"' + package_id + '"')
    return source


def stage(output, package_id=PRODUCTION_PACKAGE):
    repository = pathlib.Path(__file__).resolve().parents[1]
    output.mkdir(parents=True, exist_ok=True)
    source = (repository / 'Tests/Quickstarts/EnvironmentBiomeTerrainQuickstarts.cs').read_text(encoding='utf-8-sig')
    source = release_quickstarts(source, package_id)
    source = source.replace('AncientMedievalJapan.Environment.Quicktests', 'AncientMedievalJapan.Environment.WorkshopFinaltests')
    source = source.replace('sucro.ancientmedievaljapan.environment.quicktests', 'sucro.ancientmedievaljapan.environment.workshopfinaltests')
    source = re.sub(r'\bAMJ[A-Za-z0-9_]*Quickstart\b', lambda match: 'Workshop_' + match[0], source)
    (output / 'WorkshopHarvestQuickstarts.cs').write_text(source, encoding='utf-8')
    runner = (repository / 'Scripts/Run-EnvironmentVegetationQuickstarts.ps1').read_text(encoding='utf-8-sig')
    runner = re.sub(r'\bAMJ[A-Za-z0-9_]*Quickstart\b', lambda match: 'Workshop_' + match[0], runner)
    (output / 'Run-WorkshopHarvestQuickstarts.ps1').write_text(runner, encoding='utf-8')
    for name in ['Run-EnvironmentVegetationQuickstarts.ps1', 'Validate-EnvironmentRuntimeLog.ps1', 'RimWorldRuntime.ps1']:
        shutil.copyfile(repository / 'Scripts' / name, output / name)
    for name in ['WorkshopSourceAudit.cs', 'IsolatedDesktopRunner.cs']:
        shutil.copyfile(repository / 'Tests/Release' / name, output / name)
    print(f'Staged observer sources and existing runners: {output}')


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('output', type=pathlib.Path)
    parser.add_argument('--package-id', choices=(PRODUCTION_PACKAGE, CANDIDATE_PACKAGE), default=PRODUCTION_PACKAGE)
    args = parser.parse_args()
    stage(args.output.resolve(), args.package_id)
