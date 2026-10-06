"""Reject the observed wrong-root/exclusion/mutation publication failures."""
import importlib.util
import json
from pathlib import Path
import tempfile
import unittest
import subprocess

spec = importlib.util.spec_from_file_location('payload', Path(__file__).resolve().parents[1] / 'Scripts/Build-WorkshopPayload.py')
payload = importlib.util.module_from_spec(spec)
spec.loader.exec_module(payload)


def fixture():
    return {'About/About.xml': b'<ModMetaData><name>AMJ - Environment</name><packageId>sucro.ancientmedievaljapan.environment</packageId></ModMetaData>',
            'About/PublishedFileId.txt': b'3814638060', 'loadFolders.xml': payload.LOAD,
            'Assemblies/AncientMedievalJapanEnvironment.dll': b'MZfixture',
            'Defs/ThingDefs_Plants/AMJ_WildPlants.xml': b'<Defs><ThingDef ParentName="BushBase"><defName>AMJ_Shrub_Haimatsu</defName><plant><harvestedThingDef>WoodLog</harvestedThingDef><harvestYield>8</harvestYield><harvestTag>Wood</harvestTag></plant></ThingDef></Defs>'}


class GateTests(unittest.TestCase):
    def test_old_root_with_fixed_nested_copy_is_rejected(self):
        data = fixture()
        data['Defs/ThingDefs_Plants/AMJ_WildPlants.xml'] = data['Defs/ThingDefs_Plants/AMJ_WildPlants.xml'].replace(b'<harvestYield>8</harvestYield>', b'')
        with self.assertRaisesRegex(ValueError, 'cutting contract'):
            payload.validate(data)
        data['TestResults/SourceSync/Defs/fixed.xml'] = fixture()['Defs/ThingDefs_Plants/AMJ_WildPlants.xml']
        with self.assertRaisesRegex(ValueError, 'Development content'):
            payload.validate(data)

    def test_root_only_and_identity(self):
        for key, value in [('loadFolders.xml', b'<loadFolders/>'), ('About/PublishedFileId.txt', b'999')]:
            data = fixture(); data[key] = value
            with self.assertRaises(ValueError):
                payload.validate(data)

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
