#!/usr/bin/env python3
"""Import a source-pinned genuine 1.6 assembly, never an unverified runner DLL.

The Project Actions artifact contains the original packed candidate and its
external provenance manifest. Only its compiled DLL is imported here; current
About/release copy is assembled by the Environment publisher afterward.
"""
from __future__ import annotations

import argparse
import hashlib
import io
import json
from pathlib import Path
import subprocess
import sys
import zipfile

ROOT = Path(__file__).resolve().parents[2]
CONFIG = ROOT / "Publishing/VerifiedAssembly.json"
ASSEMBLY = "Assemblies/AncientMedievalJapanEnvironment.dll"
CANDIDATE = "AncientMedievalJapanEnvironment"
GAMEPLAY_PATHS = ("Source", "Defs", "Patches", "Languages", "Textures", "loadFolders.xml")


def sha(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def require(condition: bool, message: str) -> None:
    if not condition:
        raise ValueError(message)


def verify(evidence: Path, root: Path, config: dict) -> bytes:
    require(config["schema"] == 1 and config["source_commit"] and
            len(config["source_commit"]) == 40, "Invalid pinned source")
    git_prefix = ["git", "-C", str(root)]
    ancestor = subprocess.run(git_prefix + ["merge-base", "--is-ancestor",
                                          config["source_commit"], "HEAD"],
                              capture_output=True, check=False)
    require(ancestor.returncode == 0, "Pinned verified source is not available as an ancestor")
    changes = subprocess.run(git_prefix + ["diff", "--quiet",
                                          config["source_commit"], "HEAD", "--",
                                          *GAMEPLAY_PATHS], capture_output=True, check=False)
    require(changes.returncode == 0,
            "Gameplay C#/Defs/graphics differ from the tested binary source; new native build required")
    require(not (root / ASSEMBLY).exists(), "Do not overwrite an existing assembly")
    if evidence.is_dir():
        # actions/download-artifact extracts its outer ZIP into this directory.
        manifest = json.loads((evidence / "candidate/Manifest.json").read_text(encoding="utf-8"))
        candidate_bytes = (evidence / ("candidate/" + CANDIDATE + ".zip")).read_bytes()
        original_dll = (evidence / ("candidate/" + CANDIDATE) / ASSEMBLY).read_bytes()
    else:
        # Local regression can inspect the original GitHub artifact ZIP itself.
        with zipfile.ZipFile(evidence) as uploaded:
            require(uploaded.testzip() is None, "Damaged original Actions artifact")
            manifest = json.loads(uploaded.read("candidate/Manifest.json"))
            candidate_bytes = uploaded.read("candidate/" + CANDIDATE + ".zip")
            original_dll = uploaded.read("candidate/" + CANDIDATE + "/" + ASSEMBLY)
    require(manifest["source_commit"] == config["source_commit"] and
            manifest["production_assembly_sha256"] == config["assembly_sha256"] and
            manifest["archive_sha256"] == config["archive_sha256"] and
            manifest["approved_preview_sha256"] == config["preview_sha256"] and
            manifest["workshopId"] == config["workshop_id"] and
            manifest["packageId"] == config["package_id"],
            "Pinned candidate identity or build provenance mismatch")
    require(sha(candidate_bytes) == config["archive_sha256"],
            "Original complete subscriber candidate ZIP differs")
    require(sha(original_dll) == config["assembly_sha256"] and
            manifest["files"][ASSEMBLY] == config["assembly_sha256"] and
            original_dll.startswith(b"MZ") and len(original_dll) >= 8192,
            "Production assembly bytes differ from verified native source")
    with zipfile.ZipFile(io.BytesIO(candidate_bytes)) as candidate:
        require(candidate.testzip() is None, "Damaged original native candidate ZIP")
        expected_names = {CANDIDATE + "/" + name for name in manifest["files"]}
        require(set(candidate.namelist()) == expected_names,
                "Native candidate contains extra/missing subscriber files")
        for name, digest in manifest["files"].items():
            require(sha(candidate.read(CANDIDATE + "/" + name)) == digest,
                    "Native candidate file hash mismatch: " + name)
        require(candidate.read(CANDIDATE + "/" + ASSEMBLY) == original_dll,
                "Native candidate and extracted assembly disagree")
    preview = root / "About/preview.png"
    require(preview.is_file() and sha(preview.read_bytes()) == config["preview_sha256"],
            "Owner-approved Git preview differs from pinned candidate")
    print("VERIFIED_GENUINE_NATIVE_DLL: source=" + config["source_commit"] +
          " sha256=" + config["assembly_sha256"] + " bytes=" + str(len(original_dll)))
    return original_dll


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--evidence", required=True, type=Path)
    parser.add_argument("--checkout", type=Path, default=ROOT)
    parser.add_argument("--install", action="store_true",
                        help="Write only the exact verified production DLL to the CI checkout")
    args = parser.parse_args()
    config = json.loads(CONFIG.read_text(encoding="utf-8"))
    data = verify(args.evidence, args.checkout.resolve(), config)
    if args.install:
        target = args.checkout.resolve() / ASSEMBLY
        target.parent.mkdir(exist_ok=True, parents=True)
        target.write_bytes(data)
        require(sha(target.read_bytes()) == config["assembly_sha256"],
                "Installed binary changed")
        print("VERIFIED_DLL_STAGED; no Steam publication")


if __name__ == "__main__":
    try:
        main()
    except (OSError, ValueError, KeyError, subprocess.SubprocessError,
            zipfile.BadZipFile, json.JSONDecodeError) as exc:
        print("VERIFIED_ASSEMBLY_BLOCKED: " + str(exc), file=sys.stderr)
        sys.exit(2)
