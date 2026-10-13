"""Reject the observed wrong-root/exclusion/mutation publication failures."""
import importlib.util
import json
from pathlib import Path
import tempfile
import unittest
import subprocess

spec = importlib.util.spec_from_file_location('payload', Path(__file__).resolve().parents[2] / 'Scripts/Build-WorkshopPayload.py')
payload = importlib.util.module_from_spec(spec)
spec.loader.exec_module(payload)


def fixture():
    return {'About/About.xml': b'<ModMetaData><name>AMJ - Environment</name><packageId>sucro.ancientmedievaljapan.environment</packageId><modVersion>0.1.0</modVersion></ModMetaData>',
            'About/Manifest.xml': b'<Manifest><identifier>AncientMedievalJapanEnvironment</identifier><version>0.1.0</version></Manifest>',
            'About/Changelog.txt': b'# fixture\n0.1.0 - fixture changenote\n',
            'About/PublishedFileId.txt': b'3814638060', 'loadFolders.xml': payload.LOAD,
            'Assemblies/AncientMedievalJapanEnvironment.dll': b'MZfixture',
            'Defs/ThingDefs_Plants/AMJ_WildPlants.xml': b'<Defs><ThingDef ParentName="BushBase"><defName>AMJ_Shrub_Haimatsu</defName><plant><harvestedThingDef>WoodLog</harvestedThingDef><harvestYield>8</harvestYield><harvestTag>Wood</harvestTag></plant></ThingDef></Defs>'}


class GateTests(unittest.TestCase):

    def test_reaudit_cannot_promote_incomplete_native_profile(self):
        spec = importlib.util.spec_from_file_location(
            'combining', payload.ROOT / 'Scripts/Combine-WorkshopPayloadResults.py')
        combining = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(combining)
        with tempfile.TemporaryDirectory() as tmp:
            result = Path(tmp)
            maps = result / 'Reports-MO'
            maps.mkdir()
            for name in combining.gate.SCENARIOS:
                (maps / ('Workshop_AMJ' + name + 'Quickstart.json')).write_text('{}')
            self.assertEqual(set(), combining.completed_report_profiles(result))
            harvest = result / 'Harvest-MO'
            harvest.mkdir()
            (harvest / 'Workshop_AMJWarmTemperateTerrainQuickstart.json').write_text('{}')
            self.assertEqual({'MO'}, combining.completed_report_profiles(result))
            (maps / 'Workshop_AMJColdBogVegetationQuickstart.json').unlink()
            self.assertEqual(set(), combining.completed_report_profiles(result))

    def test_release_observer_excludes_unapproved_development_probes(self):
        spec = importlib.util.spec_from_file_location(
            'staging', payload.ROOT / 'Scripts/Stage-WorkshopRegressionSources.py')
        staging = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(staging)
        source = (payload.ROOT / 'Tests/Quickstarts/EnvironmentBiomeTerrainQuickstarts.cs').read_text(encoding='utf-8-sig')
        release = staging.release_quickstarts(source)
        # Ordinary developer Quickstarts keep their probes. The release copy
        # keeps all production assertions, including natural biome exclusion.
        for helper in ('AddWetlandBehaviorProbeAssertions', 'AddHannokiBehaviorProbeAssertions'):
            call = helper + '(result, Find.CurrentMap);'
            self.assertEqual(2, source.count(call))
            self.assertNotIn(call, release)
            with self.assertRaises(ValueError):
                staging.release_quickstarts(source.replace(call, '', 1))
        self.assertEqual(source.count('result.Assert('), release.count('result.Assert('))
        self.assertIn('natural surface has zero ', release)
        self.assertIn('remains available for natural generation', release)
        self.assertEqual(2, release.count('AddHannokiProductionAssertions(result, Find.CurrentMap,'))
        candidate = staging.release_quickstarts(source, staging.CANDIDATE_PACKAGE)
        self.assertIn('"' + staging.CANDIDATE_PACKAGE + '"', candidate)
        self.assertNotIn('"' + staging.PRODUCTION_PACKAGE + '"', candidate)
        with self.assertRaises(ValueError):
            staging.release_quickstarts(source, 'unrelated.package')
        with tempfile.TemporaryDirectory() as tmp:
            output = Path(tmp)
            staging.stage(output, staging.CANDIDATE_PACKAGE)
            self.assertEqual((payload.ROOT / 'Scripts/RimWorldRuntime.ps1').read_bytes(),
                             (output / 'RimWorldRuntime.ps1').read_bytes())
            self.assertIn('"' + staging.CANDIDATE_PACKAGE + '"',
                          (output / 'WorkshopHarvestQuickstarts.cs').read_text(encoding='utf-8'))

    def test_reject_incorrect_release_reports(self):
        spec = importlib.util.spec_from_file_location(
            'gate', payload.ROOT / 'Scripts/Run-WorkshopPayloadTests.py')
        gate = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(gate)
        self.assertEqual((85, 78, 75, 74, 59, 62, 24, 3, 3),
                         gate.expected_map_assertions('MO'))
        self.assertEqual((97, 90, 87, 86, 59, 62, 24, 3, 3),
                         gate.expected_map_assertions('MO-CCTO'))
        self.assertEqual(17, gate.CUTTING_ASSERTIONS)
        name = 'Workshop_AMJAlpineTerrainQuickstart'
        valid = {'quickstart': name, 'total': 70, 'passed': True, 'failed': 0,
                 'timedOut': False, 'preLaunchErrors': 0, 'logErrors': 0,
                 'logTruncated': False, 'captureLive': True, 'errors': [],
                 'results': [{'passed': True} for _ in range(70)]}
        gate.validate_report(valid, 70, name)
        for key, value in (('quickstart', 'wrong'), ('total', 68),
                           ('passed', False), ('failed', 1), ('timedOut', True),
                           ('preLaunchErrors', 1), ('logErrors', 1),
                           ('logTruncated', True), ('captureLive', False),
                           ('errors', ['error'])):
            changed = dict(valid)
            changed[key] = value
            with self.subTest(key=key), self.assertRaises(ValueError):
                gate.validate_report(changed, 70, name)
        changed = dict(valid)
        changed['results'] = valid['results'][:-1]
        with self.assertRaises(ValueError):
            gate.validate_report(changed, 70, name)

        # An old natural-world PASS omitted biome replacement entirely.
        # Reject it in every profile, and reject a failed exclusion even if
        # the report's top-level passed flag incorrectly remains true.
        world_name = 'Workshop_AMJWorldWetlandDistributionQuickstart'
        for profile in gate.PROFILES:
            count = gate.expected_map_assertions(profile)[gate.SCENARIOS.index('WorldWetlandDistribution')]
            world = dict(valid, quickstart=world_name, total=count,
                         results=[{'passed': True} for _ in range(count)])
            gate.validate_report(world, count, world_name)
            old = dict(world, total=7, results=world['results'][:7])
            with self.subTest(profile=profile), self.assertRaises(ValueError):
                gate.validate_report(old, count, world_name)
            world['results'][-1] = {'passed': False}
            with self.subTest(profile=profile), self.assertRaises(ValueError):
                gate.validate_report(world, count, world_name)
        changed['results'] = valid['results'][:-1] + [{'passed': False}]
        with self.assertRaises(ValueError):
            gate.validate_report(changed, 70, name)

    def test_old_root_with_fixed_nested_copy_is_rejected(self):
        data = fixture()
        data['Defs/ThingDefs_Plants/AMJ_WildPlants.xml'] = data['Defs/ThingDefs_Plants/AMJ_WildPlants.xml'].replace(b'<harvestYield>8</harvestYield>', b'')
        with self.assertRaisesRegex(ValueError, 'cutting contract'):
            payload.validate(data)
        data['TestResults/SourceSync/Defs/fixed.xml'] = fixture()['Defs/ThingDefs_Plants/AMJ_WildPlants.xml']
        with self.assertRaisesRegex(ValueError, 'Development content'):
            payload.validate(data)

    def test_tracked_loadfolders_matches_distribution_contract(self):
        self.assertEqual((payload.ROOT / 'loadFolders.xml').read_bytes(), payload.LOAD)
    def test_root_only_and_identity(self):
        for key, value in [('loadFolders.xml', b'<loadFolders/>'), ('About/PublishedFileId.txt', b'999')]:
            data = fixture(); data[key] = value
            with self.assertRaises(ValueError):
                payload.validate(data)

    def test_changenote_metadata_contract(self):
        data = fixture()
        payload.validate(data)

        mismatch = fixture()
        mismatch['About/Manifest.xml'] = b'<Manifest><version>0.1.1</version></Manifest>'
        with self.assertRaisesRegex(ValueError, 'modVersion'):
            payload.validate(mismatch)

        missing_entry = fixture()
        missing_entry['About/Changelog.txt'] = b'0.0.9 - old\n'
        with self.assertRaisesRegex(ValueError, 'Changelog'):
            payload.validate(missing_entry)

    def test_missing_extra_and_mutated_files(self):
        with tempfile.TemporaryDirectory() as temp:
            root = Path(temp) / 'root'; root.mkdir()
            for name, content in fixture().items():
                path = root / name; path.parent.mkdir(parents=True, exist_ok=True); path.write_bytes(content)
            manifest = Path(temp) / 'manifest.json'
            manifest.write_text(json.dumps({'files': payload.inventory(root), 'source_commit': 'fixture'}))
            payload.verify(root, manifest)
            extra = root / 'stale.xml'; extra.write_bytes(b'stale')
            with self.assertRaisesRegex(ValueError, 'extra='):
                payload.verify(root, manifest)
            extra.unlink()
            target = root / 'About/PublishedFileId.txt'; target.write_bytes(b'999')
            with self.assertRaisesRegex(ValueError, 'changed='):
                payload.verify(root, manifest)
            target.unlink()
            with self.assertRaisesRegex(ValueError, 'missing='):
                payload.verify(root, manifest)

    def test_local_workshop_id_is_safe_but_other_untracked_runtime_content_is_not(self):
        with tempfile.TemporaryDirectory() as temp:
            root = Path(temp)
            about = root / 'About'
            about.mkdir()
            published = about / 'PublishedFileId.txt'
            published.write_text(payload.WORKSHOP + '\n', encoding='utf-8')
            payload.audit_untracked_runtime_sources(root, set())

            published.write_text('999\n', encoding='utf-8')
            with self.assertRaisesRegex(ValueError, 'Wrong local Workshop ID'):
                payload.audit_untracked_runtime_sources(root, set())

            published.write_text(payload.WORKSHOP + '\n', encoding='utf-8')
            unexpected = about / 'unexpected.xml'
            unexpected.write_text('<ModMetaData/>', encoding='utf-8')
            with self.assertRaisesRegex(ValueError, 'Untracked runtime content'):
                payload.audit_untracked_runtime_sources(root, set())
            unexpected.unlink()

            defs = root / 'Defs'
            defs.mkdir()
            stale = defs / 'UntrackedPlant.xml'
            stale.write_text('<Defs/>', encoding='utf-8')
            with self.assertRaisesRegex(ValueError, 'Untracked runtime content'):
                payload.audit_untracked_runtime_sources(root, set())
            payload.audit_untracked_runtime_sources(root, {'Defs/UntrackedPlant.xml'})

    def test_yada_defensive_exclusions(self):
        rules = set((payload.ROOT / '.rimignore').read_text().splitlines())
        self.assertTrue(set(payload.EXCLUDED) <= rules)

    def test_dirty_and_wrong_head_are_rejected_before_build(self):
        original = payload.ROOT
        with tempfile.TemporaryDirectory() as temp:
            payload.ROOT = Path(temp)
            def git(*args):
                return subprocess.check_output(['git', '-C', temp, *args], text=True).strip()
            try:
                git('init', '-q')
                file = Path(temp) / 'input.xml'; file.write_text('approved')
                git('add', 'input.xml')
                git('-c', 'user.name=Regression', '-c', 'user.email=regression@example.invalid', 'commit', '-qm', 'fixture')
                sha = git('rev-parse', 'HEAD')
                with self.assertRaisesRegex(ValueError, 'pinned HEAD'):
                    payload.build(Path(temp) / 'output', '0' * 40, Path(temp), file)
                file.write_text('uncommitted change')
                with self.assertRaisesRegex(ValueError, 'tracked modifications'):
                    payload.build(Path(temp) / 'output', sha, Path(temp), file)
                self.assertFalse((Path(temp) / 'output').exists())
                file.write_text('approved')
                stale = Path(temp) / 'Source/AncientMedievalJapanEnvironment/Stale.cs'
                stale.parent.mkdir(parents=True); stale.write_text('untracked C#')
                with self.assertRaisesRegex(ValueError, 'Untracked production C#'):
                    payload.build(Path(temp) / 'output', sha, Path(temp), file)
            finally:
                payload.ROOT = original


if __name__ == '__main__':
    unittest.main()
