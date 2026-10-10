"""Shared Windows helpers for AMJE RimTest Redux and Pickle development runs."""
from __future__ import annotations

import hashlib
import os
from pathlib import Path
import subprocess
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]


def invoke(args, **kwargs):
    subprocess.run([str(x) for x in args], check=True, **kwargs)


def find_dll(root: Path, name: str) -> Path:
    """Select a RimWorld 1.6-loaded DLL, not an older version in the same Mod.

    Harmony ships 1.4, 1.5 and Current assemblies. Its v1.6 loadFolders
    activates "/" and "Current"; a recursive count is not a validity check.
    """
    loader = next((root / file for file in ("LoadFolders.xml", "loadFolders.xml")
                   if (root / file).is_file()), None)
    if loader is not None:
        try:
            version = ET.parse(loader).getroot().find("v1.6")
        except ET.ParseError as error:
            raise ValueError(f"Invalid {loader}: {error}") from error
        if version is None:
            raise ValueError(f"No RimWorld v1.6 load folders declared in {loader}")

        candidates = []
        for item in version.findall("li"):
            if item.attrib:
                continue  # Conditional folders cannot be assumed active.
            folder = (item.text or "").strip()
            relative = Path(".") if folder == "/" else Path(folder)
            if not folder or relative.is_absolute() or ".." in relative.parts:
                raise ValueError(f"Unsafe RimWorld load folder {folder!r} in {loader}")
            candidate = root / relative / "Assemblies" / name
            if candidate.is_file():
                candidates.append(candidate)
        if candidates:
            return candidates[-1]  # Last loaded folder has precedence.
        raise ValueError(f"No active RimWorld 1.6 {name} in {root} (load folders: "
                         + ", ".join((item.text or "").strip()
                                     for item in version.findall("li")) + ")")

    # Mods with no LoadFolders.xml use one assembly folder (or the 1.6 layout).
    preferred = root / "1.6" / "Assemblies" / name
    if preferred.is_file():
        return preferred
    matches = sorted(root.rglob(name)) if root.is_dir() else []
    if len(matches) != 1:
        raise ValueError(f"Expected one {name} in {root} without LoadFolders.xml; "
                         f"found {len(matches)}: " + ", ".join(map(str, matches)))
    return matches[0]


def ensure_game_closed():
    listing = subprocess.check_output(
        ["tasklist", "/FI", "IMAGENAME eq RimWorldWin64.exe"], text=True
    )
    if "RimWorldWin64.exe" in listing:
        raise ValueError("Close RimWorld normally before automated framework tests")


def user_config_dir() -> Path:
    return (
        Path(os.environ["USERPROFILE"])
        / "AppData/LocalLow/Ludeon Studios/RimWorld by Ludeon Studios/Config"
    )


def digest(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def snapshot_tree(root: Path) -> dict[str, str]:
    ignored = {"TestResults", ".git"}
    result = {}
    for path in sorted(p for p in root.rglob("*") if p.is_file()):
        if any(part in ignored for part in path.relative_to(root).parts):
            continue
        result[path.relative_to(root).as_posix()] = digest(path)
    return result


def write_isolated_config(user_config: Path, save_data: Path, package_ids: list[str]):
    config_dir = save_data / "Config"
    config_dir.mkdir(parents=True)
    config = ET.parse(user_config / "ModsConfig.xml")
    active = config.find("activeMods")
    if active is None:
        raise ValueError("Normal ModsConfig.xml has no activeMods element")
    active.clear()
    for package in package_ids:
        ET.SubElement(active, "li").text = package
    config.write(config_dir / "ModsConfig.xml", encoding="utf-8", xml_declaration=True)

    prefs = ET.parse(user_config / "Prefs.xml")
    for name, value in (("devMode", "True"), ("resetModsConfigOnCrash", "False")):
        node = prefs.find(name)
        if node is None:
            node = ET.SubElement(prefs.getroot(), name)
        node.text = value
    prefs.write(config_dir / "Prefs.xml", encoding="utf-8", xml_declaration=True)
    return config_dir


def compiler(game: Path) -> tuple[Path, list[Path]]:
    csc = Path(os.environ["WINDIR"]) / "Microsoft.NET/Framework64/v4.0.30319/csc.exe"
    if not csc.is_file():
        csc = Path(os.environ["WINDIR"]) / "Microsoft.NET/Framework/v4.0.30319/csc.exe"
    if not csc.is_file():
        raise ValueError(".NET Framework csc.exe was not found")
    managed = game / "RimWorldWin64_Data/Managed"
    refs = [managed / name for name in (
        "Assembly-CSharp.dll", "UnityEngine.CoreModule.dll", "netstandard.dll"
    )]
    missing = [str(x) for x in refs if not x.is_file()]
    if missing:
        raise ValueError("Missing RimWorld managed references: " + ", ".join(missing))
    return csc, refs


def compile_support(game: Path, output: Path, error_mod: Path):
    csc, refs = compiler(game)
    invoke([
        csc, "/nologo", "/target:library",
        "/out:" + str(error_mod / "Assemblies/AMJE.FrameworkErrors.dll"),
        *["/reference:" + str(x) for x in refs],
        ROOT / "Tests/Release/HarvestErrorObserver.cs",
    ])
    desktop = output / "Compiled/IsolatedDesktopRunner.exe"
    desktop.parent.mkdir(parents=True, exist_ok=True)
    invoke([
        csc, "/nologo", "/target:exe", "/out:" + str(desktop),
        ROOT / "Tests/Release/IsolatedDesktopRunner.cs",
    ])
    return csc, refs, desktop


def run_hidden(desktop: Path, batch: Path, env: dict[str, str], runner_log: Path):
    with runner_log.open("w", encoding="utf-8") as log:
        invoke([desktop, batch, ROOT], env=env, stdout=log, stderr=subprocess.STDOUT)


def validate_runtime_logs(log_path: Path, error_dir: Path):
    if not log_path.is_file():
        raise ValueError("RimWorld Player.log was not created: " + str(log_path))
    game_log = log_path.read_text(encoding="utf-8-sig", errors="replace")
    if "Version:  Direct3D 11.0" not in game_log:
        raise ValueError("RimWorld log did not confirm Direct3D 11")
    if "[ERROR]" in game_log or "Level: ERROR" in game_log:
        raise ValueError("RimWorld Player.log contains ERROR")
    captures = list(error_dir.glob("Unity-*.log"))
    if len(captures) != 1:
        raise ValueError("Expected one independent Unity error capture")
    capture = captures[0].read_text(encoding="utf-8-sig", errors="replace")
    if "[CAPTURE_READY]" not in capture or "[ERROR]" in capture:
        raise ValueError("Independent Unity log is incomplete or contains ERROR")


def write_about(root: Path, name: str, package_id: str, load_after: list[str] | None = None):
    (root / "About").mkdir(parents=True)
    load = ""
    if load_after:
        load = "<loadAfter>" + "".join(f"<li>{x}</li>" for x in load_after) + "</loadAfter>"
    (root / "About/About.xml").write_text(
        "<ModMetaData><name>" + name + "</name><packageId>" + package_id +
        "</packageId><supportedVersions><li>1.6</li></supportedVersions>" +
        load + "</ModMetaData>", encoding="utf-8"
    )
