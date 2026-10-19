"""Publisher import contract tests; no game launch and no Steam API."""
from __future__ import annotations
import hashlib
import io
import json
from pathlib import Path
import sys
import tempfile
import unittest
from unittest import mock
import zipfile

sys.path.insert(0, str(Path(__file__).resolve().parents[2] / "Tools/SteamPublisher"))
import import_verified_assembly as transfer


def sha(raw):
    return hashlib.sha256(raw).hexdigest()


def fixture(root, original_dll=b"MZ" + b"\0" * 8192):
    preview = b"\x89PNG\r\n\x1a\naccepted"
    (root / "About").mkdir()
    (root / "About/preview.png").write_bytes(preview)
    data = {"About/Preview.png": preview,
            "Assemblies/AncientMedievalJapanEnvironment.dll": original_dll,
            "Defs/Plants.xml": b"<Defs/>"}
    result = io.BytesIO()
    with zipfile.ZipFile(result, "w") as zipped:
        for name, payload in sorted(data.items()):
            zipped.writestr(transfer.CANDIDATE + "/" + name, payload)
    zipped_bytes = result.getvalue()
    config = {
        "schema": 1, "source_commit": "a" * 40,
        "archive_sha256": sha(zipped_bytes),
        "assembly_sha256": sha(original_dll),
        "preview_sha256": sha(preview),
        "workshop_id": "3814638060",
        "package_id": "sucro.ancientmedievaljapan.environment",
    }
    manifest = {
        "source_commit": config["source_commit"],
        "production_assembly_sha256": config["assembly_sha256"],
        "archive_sha256": config["archive_sha256"],
        "approved_preview_sha256": config["preview_sha256"],
        "workshopId": config["workshop_id"],
        "packageId": config["package_id"],
        "files": {name: sha(raw) for name, raw in data.items()},
    }
    evidence = root / "evidence"
    candidate = evidence / "candidate"
    (candidate / transfer.CANDIDATE / "Assemblies").mkdir(parents=True)
    (candidate / "Manifest.json").write_text(json.dumps(manifest))
    (candidate / (transfer.CANDIDATE + ".zip")).write_bytes(zipped_bytes)
    (candidate / transfer.CANDIDATE / transfer.ASSEMBLY).write_bytes(original_dll)
    return evidence, config


class VerifiedAssemblyTests(unittest.TestCase):
    def test_exact_candidate_import_and_modified_gameplay_refusal(self):
        with tempfile.TemporaryDirectory() as td:
            root = Path(td)
            evidence, config = fixture(root)
            with mock.patch.object(transfer.subprocess, "run",
                                   return_value=mock.Mock(returncode=0)):
                self.assertEqual(transfer.verify(evidence, root, config)[:2], b"MZ")
            with mock.patch.object(transfer.subprocess, "run",
                                   side_effect=[mock.Mock(returncode=0),
                                                mock.Mock(returncode=1)]):
                with self.assertRaisesRegex(ValueError, "Gameplay C"):
                    transfer.verify(evidence, root, config)

    def test_changed_dll_and_archive_are_rejected(self):
        with tempfile.TemporaryDirectory() as td:
            root = Path(td)
            evidence, config = fixture(root)
            with mock.patch.object(transfer.subprocess, "run",
                                   return_value=mock.Mock(returncode=0)):
                broken = dict(config, assembly_sha256="0" * 64)
                with self.assertRaisesRegex(ValueError, "identity or build provenance"):
                    transfer.verify(evidence, root, broken)
                nested_zip = evidence / "candidate" / (transfer.CANDIDATE + ".zip")
                nested_zip.write_bytes(nested_zip.read_bytes() + b"tampered")
                with self.assertRaisesRegex(ValueError, "complete subscriber candidate"):
                    transfer.verify(evidence, root, config)


    def test_public_pinned_git_transfer_rejects_wrong_commit_blob_and_dll(self):
        with tempfile.TemporaryDirectory() as td:
            root = Path(td)
            genuine = b"MZ" + b"\\0" * 8192
            original, cfg = fixture(root, genuine)
            evidence = root / "pinned-public-git"
            (evidence / ".git").mkdir(parents=True)
            binary = evidence / transfer.ASSEMBLY
            binary.parent.mkdir(parents=True)
            binary.write_bytes(genuine)
            commit = "c" * 40
            blob = "b" * 40
            cfg.update({
                "transfer_method": "verified_public_git_commit",
                "transfer_repository": "sucRo-RimWorld/Ancient-Medieval-Japan-Environment",
                "transfer_commit": commit,
                "transfer_blob_sha": blob,
                "project_run_id": 39247372678,
                "archive_sha256": "86c433992e21366d026c468b8a7a73142abeb2c44c1f51f74afaafa8977341fe",
            })
            def check(revision, entry, passed=0):
                with mock.patch.object(transfer.subprocess, "check_output",
                                       side_effect=[revision + "\\n", entry + "\\n"]), \\
                     mock.patch.object(transfer.subprocess, "run",
                                       return_value=mock.Mock(returncode=passed)):
                    return transfer.verify_public_pinned_assembly(evidence, root, cfg)
            self.assertEqual(check(commit, blob), genuine)
            with self.assertRaisesRegex(ValueError, "commit mismatch"):
                check("a" * 40, blob)
            with self.assertRaisesRegex(ValueError, "Git blob differs"):
                check(commit, "a" * 40)
            with self.assertRaisesRegex(ValueError, "modified"):
                check(commit, blob, passed=1)
            binary.write_bytes(b"MZ" + b"other content" * 1100)
            with self.assertRaisesRegex(ValueError, "SHA-256 mismatch"):
                check(commit, blob)


if __name__ == "__main__":
    unittest.main()
