from pathlib import Path
import importlib.util
import tempfile
import unittest

ROOT = Path(__file__).resolve().parents[1]


class FrameworkAutomationContractTests(unittest.TestCase):
    def test_harmony_rimworld_16_loader_selects_current_not_old_dlls(self):
        spec = importlib.util.spec_from_file_location(
            "amje_framework_common",
            ROOT / "Scripts/EnvironmentFrameworkTestCommon.py",
        )
        common = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(common)
        with tempfile.TemporaryDirectory() as tmp:
            root = Path(tmp)
            for folder in ("1.4", "1.5", "Current"):
                dll = root / folder / "Assemblies/0Harmony.dll"
                dll.parent.mkdir(parents=True)
                dll.write_bytes(folder.encode("ascii"))
            (root / "LoadFolders.xml").write_text(
                "<loadFolders>"
                "<v1.4><li>/</li><li>1.4</li></v1.4>"
                "<v1.5><li>/</li><li>1.5</li></v1.5>"
                "<v1.6><li>/</li><li>Current</li></v1.6>"
                "</loadFolders>",
                encoding="utf-8",
            )
            selected = common.find_dll(root, "0Harmony.dll")
            self.assertEqual(selected, root / "Current/Assemblies/0Harmony.dll")
            self.assertEqual(selected.read_bytes(), b"Current")
            # A missing active DLL must fail rather than fall back to 1.5/1.4.
            selected.unlink()
            with self.assertRaisesRegex(ValueError, "No active RimWorld 1.6"):
                common.find_dll(root, "0Harmony.dll")


if __name__ == "__main__":
    unittest.main(verbosity=2)
