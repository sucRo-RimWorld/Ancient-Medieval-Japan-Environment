"""Static contract for the Environment-specific Pickle/Steam bridge.

Real compilation/Unity execution occurs only on a Windows Steam session.
"""
import importlib.util
import json
from pathlib import Path
import unittest
from unittest import mock
import os
import tempfile

ROOT = Path(__file__).resolve().parents[1]


class WorkshopPickleTests(unittest.TestCase):
    def test_manifest_discovery_fails_closed_and_selects_verified_bytes(self):
        path = ROOT / "Scripts/Run-WorkshopPickle.py"
        spec = importlib.util.spec_from_file_location("pickle_runner", path)
        runner = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(runner)
        with tempfile.TemporaryDirectory() as temp:
            root = Path(temp)
            earlier = root / "AMJE-Final-20261008-210000"
            latest = root / "AMJE-Final-20261008-220000"
            earlier.mkdir()
            latest.mkdir()
            for directory in (earlier, latest):
                (directory / "Manifest.json").write_text("{}", encoding="utf-8")
            old_manifest = earlier / "Manifest.json"
            newest = latest / "Manifest.json"
            os.utime(old_manifest, (1000000000, 1000000000))
            os.utime(newest, (2000000000, 2000000000))
            def verify(p, manifest):
                if manifest != old_manifest:
                    raise ValueError("Wrong candidate")
                return {"files": {}}
            with mock.patch.object(runner, "verify_downloaded_for_pickle",
                                   side_effect=verify) as checker:
                result = runner.find_verified_manifest(root / "Downloaded", root)
            self.assertEqual(result, old_manifest)
            self.assertEqual(checker.call_count, 2)
            with mock.patch.object(runner, "verify_downloaded_for_pickle",
                                   side_effect=ValueError("Wrong bytes")):
                with self.assertRaisesRegex(ValueError, "No verified Manifest"):
                    runner.find_verified_manifest(root / "Downloaded", root)

    def test_steam_metadata_semantics_preserve_strict_manifest_and_gameplay_hashes(self):
        spec = importlib.util.spec_from_file_location(
            "pickle_runner", ROOT / "Scripts/Run-WorkshopPickle.py")
        runner = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(runner)
        with tempfile.TemporaryDirectory() as temp:
            root = Path(temp)
            steam = root / "steam"
            steam.mkdir()
            about = steam / "About"
            about.mkdir()
            files = {
                "About/PublishedFileId.txt": (runner.payload_tool.WORKSHOP + "\n").encode(),
                "loadFolders.xml": runner.payload_tool.LOAD,
                "Defs/test.xml": b"<Defs><ThingDef/></Defs>",
            }
            for name, data in files.items():
                p = steam / name
                p.parent.mkdir(parents=True, exist_ok=True)
                p.write_bytes(data)
            manifest = root / "Manifest.json"
            manifest.write_text(json.dumps({
                "source_commit": "fixture", "packageId": runner.payload_tool.PACKAGE,
                "workshopId": runner.payload_tool.WORKSHOP,
                "files": {n: runner.payload_tool.digest(data)
                          for n, data in files.items()},
            }), encoding="utf-8")
            with mock.patch.object(runner.payload_tool, "validate") as validate:
                same = runner.verify_downloaded_for_pickle(steam, manifest)
                self.assertTrue(same["byte_identical"])
                (steam / "About/PublishedFileId.txt").write_bytes(
                    runner.payload_tool.WORKSHOP.encode())
                (steam / "loadFolders.xml").write_text(
                    '<loadFolders>\n <v1.6> <li>/</li> </v1.6>\n</loadFolders>\n',
                    encoding="utf-8")
                formatted = runner.verify_downloaded_for_pickle(steam, manifest)
                self.assertFalse(formatted["byte_identical"])
                self.assertTrue(formatted["gameplay_files_byte_identical"])
                self.assertEqual(set(formatted["metadata_format_variants"]),
                                 set(runner.METADATA_PATHS))
                self.assertTrue(validate.called)
                for invalid in (
                    '<loadFolders><v1.5><li>/</li></v1.5></loadFolders>',
                    '<loadFolders><v1.6><li>Other</li></v1.6></loadFolders>',
                    '<loadFolders><v1.6><li>/</li><li>Other</li></v1.6></loadFolders>',
                ):
                    (steam / "loadFolders.xml").write_text(invalid, encoding="utf-8")
                    with self.subTest(xml=invalid), self.assertRaisesRegex(
                            ValueError, "changes active Mod load paths"):
                        runner.verify_downloaded_for_pickle(steam, manifest)
                (steam / "loadFolders.xml").write_text(
                    '<loadFolders><v1.6><li>/</li></v1.6></loadFolders>',
                    encoding="utf-8")
                (steam / "About/PublishedFileId.txt").write_text("999",
                    encoding="utf-8")
                with self.assertRaisesRegex(ValueError, "PublishedFileId"):
                    runner.verify_downloaded_for_pickle(steam, manifest)
                (steam / "About/PublishedFileId.txt").write_text(
                    runner.payload_tool.WORKSHOP, encoding="utf-8")
                (steam / "Defs/test.xml").write_text("<Defs/>", encoding="utf-8")
                with self.assertRaisesRegex(ValueError, "Workshop content differs"):
                    runner.verify_downloaded_for_pickle(steam, manifest)


    def test_preflight_failure_does_not_assume_result_folder(self):
        spec = importlib.util.spec_from_file_location(
            "pickle_runner", ROOT / "Scripts/Run-WorkshopPickle.py")
        runner = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(runner)
        with tempfile.TemporaryDirectory() as temp:
            with mock.patch("sys.stderr") as stderr:
                runner.show_failure(ValueError("RimWorld is open"),
                                    Path(temp) / "missing")
                self.assertTrue(stderr.write.called)


if __name__ == "__main__":
    unittest.main()
