#!/usr/bin/env python3
"""Read-only verification and local staging of the exact owner native DLL.

Accept the real Project Actions #39247372678 artifact, never a substitute.
This script runs no RimWorld process and does not perform Steam publication.
"""
from __future__ import annotations
import argparse
import hashlib
import io
import json
from pathlib import Path
import subprocess
import sys
from zipfile import ZipFile

SOURCE = "fe6861ac490bbd43ec73dce8efc1c2a5e5140479"
ARTIFACT_SHA256 = "5822dc2fe137343a22df103e6dd3173902a2d0b937f46e98210618f3834afc2d"
ARCHIVE_SHA256 = "86c433992e21366d026c468b8a7a73142abeb2c44c1f51f74afaafa8977341fe"
DLL_SHA256 = "b2ddbf7888322ba9b207c20296dba31175b846c05492902b900ec225da9fcffe"
PREVIEW_SHA256 = "25b61be8c04f866603e99d56270d177f15159aaf6f0fa7e4e24504821947e6ef"
DLL_REL = "Assemblies/AncientMedievalJapanEnvironment.dll"
SOURCE_FIELDS = ("Source", "Defs", "Patches", "Languages", "Textures", "loadFolders.xml")


def digest(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def require(ok: bool, reason: str) -> None:
    if not ok:
        raise ValueError(reason)


def run_git(root: Path, *args: str) -> None:
    subprocess.run(["git", "-C", str(root), *args], check=True,
                   stdout=subprocess.DEVNULL)


def verify(artifact: Path, root: Path) -> bytes:
    require(root.joinpath(".git").exists(), "Checkout Git worktree required")
    run_git(root, "merge-base", "--is-ancestor", SOURCE, "HEAD")
    run_git(root, "diff", "--quiet", SOURCE, "HEAD", "--", *SOURCE_FIELDS)
    raw = artifact.read_bytes()
    require(digest(raw) == ARTIFACT_SHA256, "Actions artifact ZIP hash mismatch")
    with ZipFile(io.BytesIO(raw)) as outer:
        require(outer.testzip() is None, "Actions artifact CRC failure")
        manifest = json.loads(outer.read("candidate/Manifest.json"))
        require(manifest["source_commit"] == SOURCE and
                manifest["archive_sha256"] == ARCHIVE_SHA256 and
                manifest["production_assembly_sha256"] == DLL_SHA256 and
                manifest["approved_preview_sha256"] == PREVIEW_SHA256 and
                manifest["packageId"] == "sucro.ancientmedievaljapan.environment" and
                manifest["workshopId"] == "3814638060",
                "Native source provenance differs")
        original = outer.read("candidate/AncientMedievalJapanEnvironment.zip")
        require(digest(original) == ARCHIVE_SHA256, "Original candidate ZIP hash differs")
        data = outer.read("candidate/AncientMedievalJapanEnvironment/" + DLL_REL)
        require(digest(data) == DLL_SHA256 and len(data) >= 8192 and data[:2] == b"MZ",
                "Genuine .NET DLL hash or header mismatch")
        require(manifest["files"][DLL_REL] == DLL_SHA256,
                "Native manifest DLL hash differs")
        with ZipFile(io.BytesIO(original)) as inner:
            require(inner.testzip() is None, "Original candidate CRC failure")
            require(inner.read("AncientMedievalJapanEnvironment/" + DLL_REL) == data,
                    "Original source ZIP DLL does not match extracted DLL")
    preview = root.joinpath("About/preview.png")
    require(preview.is_file() and digest(preview.read_bytes()) == PREVIEW_SHA256,
            "Accepted preview differs")
    return data


def main() -> None:
    p = argparse.ArgumentParser()
    p.add_argument("--artifact", required=True, type=Path)
    p.add_argument("--checkout", default=Path(__file__).resolve().parents[2], type=Path)
    p.add_argument("--install", action="store_true")
    a = p.parse_args()
    root = a.checkout.resolve()
    data = verify(a.artifact, root)
    if a.install:
        target = root / DLL_REL
        if target.exists():
            require(digest(target.read_bytes()) == DLL_SHA256,
                    "Refusing to overwrite a different DLL")
        else:
            target.parent.mkdir(parents=True, exist_ok=True)
            target.write_bytes(data)
        require(digest(target.read_bytes()) == DLL_SHA256, "Installed DLL differs")
    print(f"TESTBRANCH_NATIVE_DLL_VERIFIED; sha256={DLL_SHA256}; installed={a.install}")


if __name__ == "__main__":
    try:
        main()
    except (OSError, ValueError, KeyError, RuntimeError, subprocess.CalledProcessError,
            json.JSONDecodeError) as e:
        print("TESTBRANCH_NATIVE_DLL_BLOCKED: " + str(e), file=sys.stderr)
        sys.exit(2)
