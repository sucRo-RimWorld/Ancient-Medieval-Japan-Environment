"""Reject the observed wrong-root/exclusion/mutation publication failures."""
import importlib.util
import json
from pathlib import Path
import tempfile
import unittest

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


if __name__ == '__main__':
    unittest.main()
