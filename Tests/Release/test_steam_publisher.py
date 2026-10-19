#!/usr/bin/env python3
"""Static publisher regression: a source-only ZIP must never be uploadable."""
import importlib.util
from pathlib import Path
import tempfile
import unittest

ROOT = Path(__file__).resolve().parents[2]
spec = importlib.util.spec_from_file_location("amje_publisher", ROOT / "Tools/SteamPublisher/publisher.py")
publisher = importlib.util.module_from_spec(spec)
spec.loader.exec_module(publisher)


def fixture(root: Path) -> None:
    content = {
        "About/About.xml": ('<ModMetaData><name>Ancient &amp; Medieval Japan - Environment'
                            '（中世日本 - 環境）</name><packageId>sucro.ancientmedievaljapan.environment'
                            '</packageId><modVersion>0.2.0</modVersion><description>Publisher fixture description.</description></ModMetaData>'),
        "About/Manifest.xml": "<Manifest><version>0.2.0</version></Manifest>",
        "About/Changelog.txt": "0.2.0 - Wetland vegetation\n- Four plants\n",
        "About/preview.png": "accepted preview bytes",
        "Defs/Plant.xml": "<Defs/>",
        "LICENSE": "MIT License",
        "loadFolders.xml": publisher.REQUIRED_LOADER,
        "Assemblies/AncientMedievalJapanEnvironment.dll": b"MZtest-binary",
    }
    for name, value in content.items():
        path = root / name
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_bytes(value if isinstance(value, bytes) else value.encode("utf-8"))


class PublisherPayloadTests(unittest.TestCase):
    def test_release_candidate_requires_binary_license_loader_and_preview(self):
        with tempfile.TemporaryDirectory() as temporary:
            root = Path(temporary)
            fixture(root)
            expected = publisher.candidate_files(root)
            self.assertIn(publisher.REQUIRED_DLL, expected)
            self.assertIn(Path("LICENSE"), expected)
            self.assertIn(Path("About/preview.png"), expected)
            for filename, error in [
                ("Assemblies/AncientMedievalJapanEnvironment.dll", "production DLL missing"),
                ("LICENSE", "Subscriber license"),
                ("About/preview.png", "preview path"),
            ]:
                path = root / filename
                data = path.read_bytes()
                path.unlink()
                with self.subTest(filename=filename):
                    with self.assertRaisesRegex(ValueError, error):
                        publisher.candidate_files(root)
                path.write_bytes(data)
            loader = root / "loadFolders.xml"
            loader.write_bytes(b"<loadFolders><v1.6><li>DevQuickstarts</li></v1.6></loadFolders>")
            with self.assertRaisesRegex(ValueError, "only the root"):
                publisher.candidate_files(root)

    def test_lowercase_preview_is_included_without_renaming(self):
        with tempfile.TemporaryDirectory() as temporary:
            root = Path(temporary)
            fixture(root)
            cfg = {"repository": "sucRo-RimWorld/Ancient-Medieval-Japan-Environment",
                   "package_id": "sucro.ancientmedievaljapan.environment",
                   "workshop_id": "3814638060", "visibility_policy": "private_only"}
            zip_path = root / "output/release.zip"
            publisher.build(root, zip_path, cfg, "update_existing")
            import json
            import zipfile
            manifest = json.loads(zip_path.with_suffix(".json").read_text(encoding="utf-8"))
            with zipfile.ZipFile(zip_path) as archive:
                names = archive.namelist()
                self.assertIn("mod/About/preview.png", names)
                self.assertNotIn("mod/About/Preview.png", names)
                self.assertIn("mod/LICENSE", names)
                self.assertIn("mod/Assemblies/AncientMedievalJapanEnvironment.dll", names)
            self.assertIn("Assemblies/AncientMedievalJapanEnvironment.dll", manifest["files"])


if __name__ == "__main__":
    unittest.main()
