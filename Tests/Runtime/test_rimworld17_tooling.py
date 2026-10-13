import importlib.util
from pathlib import Path
import base64
import tempfile
import unittest

ROOT = Path(__file__).resolve().parents[2]
spec = importlib.util.spec_from_file_location('runtime_common', ROOT/'Scripts/EnvironmentFrameworkTestCommon.py')
common = importlib.util.module_from_spec(spec)
spec.loader.exec_module(common)


class Runtime17Tests(unittest.TestCase):
    def test_executable_and_encoded_arguments_follow_actual_version(self):
        with tempfile.TemporaryDirectory() as tmp:
            game=Path(tmp)/'game with spaces'; game.mkdir()
            version=game/'Version.txt'
            version.write_text('1.7.4976 rev1043')
            self.assertEqual(common.game_exe(game).name, 'RimWorld.exe')
            args=' -savedatafolder="C:/test profile" -pickle-no-http'
            command=common.runtime_command(game,args,game/'report.log')
            encoded=command.split('-ArgumentBase64 ')[1].split(' ')[0]
            self.assertEqual(base64.b64decode(encoded).decode(),args)
            version.write_text('1.6.4871 rev955')
            self.assertEqual(common.game_exe(game).name,'RimWorldWin64.exe')
            version.write_text('1.8.0')
            with self.assertRaises(ValueError):common.game_exe(game)

    def test_harmony_17_and_framework_16_fallback_select_actual_load_folder(self):
        with tempfile.TemporaryDirectory() as tmp:
            root=Path(tmp)
            for v in ('1.6','1.7'):
                d=root/v/'Assemblies';d.mkdir(parents=True)
                (d/'0Harmony.dll').write_bytes(v.encode())
            loader=root/'LoadFolders.xml'
            loader.write_text('<loadFolders><v1.6><li>1.6</li></v1.6><v1.7><li>1.7</li></v1.7></loadFolders>')
            self.assertEqual(common.find_dll(root,'0Harmony.dll','1.7'),root/'1.7/Assemblies/0Harmony.dll')
            (root/'1.7/Assemblies/0Harmony.dll').unlink()
            with self.assertRaises(ValueError):common.find_dll(root,'0Harmony.dll','1.7')
            loader.write_text('<loadFolders><v1.6><li>1.6</li></v1.6></loadFolders>')
            self.assertEqual(common.find_dll(root,'0Harmony.dll','1.7'),root/'1.6/Assemblies/0Harmony.dll')

    def test_native_error_cannot_pass_but_info_error_word_can(self):
        with tempfile.TemporaryDirectory() as tmp:
            root=Path(tmp); logs=root/'errors';logs.mkdir()
            (logs/'Unity-123.log').write_text('[CAPTURE_READY]\n')
            log=root/'Player.log'
            prefix='RimWorld 1.7.4976\n[Vulkan] renderer\n'
            log.write_text(prefix+'2026-10-13 01:02:03.100 INFO [Verse] ERROR count=0\n')
            common.validate_runtime_logs(log,logs)
            log.write_text(prefix+'2026-10-13 01:02:03.100 ERROR [Verse] broken\n')
            with self.assertRaises(ValueError):common.validate_runtime_logs(log,logs)
            log.write_text('RimWorld 1.7.4976\n')
            with self.assertRaises(ValueError):common.validate_runtime_logs(log,logs)


if __name__=='__main__':unittest.main(verbosity=2)
