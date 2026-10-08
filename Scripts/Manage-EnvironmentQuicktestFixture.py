"""Manage the standalone developer Quickstarts fixture used by AMJE runtime tests."""

import argparse
import shutil
from pathlib import Path
import xml.etree.ElementTree as ET

PACKAGE = "sucro.ancientmedievaljapan.environment.quicktests"
ENVIRONMENT_PACKAGE = "sucro.ancientmedievaljapan.environment"
QUICKSTARTS_PACKAGE = "rimworks.quickstarts"
FOLDER = "AMJE.EnvironmentQuicktests"
DLL_NAME = "AncientMedievalJapanEnvironment.Quicktests.dll"
MARKER_NAME = ".amje-environment-quicktests"
MARKER_TEXT = "AMJE Environment Quicktests temporary fixture\n"
ABOUT_XML = f"""<?xml version="1.0" encoding="utf-8"?>
<ModMetaData>
  <name>AMJE Environment Quicktests (temporary)</name>
  <packageId>{PACKAGE}</packageId>
  <supportedVersions>
    <li>1.6</li>
  </supportedVersions>
  <loadAfter>
    <li>{QUICKSTARTS_PACKAGE}</li>
    <li>{ENVIRONMENT_PACKAGE}</li>
  </loadAfter>
  <description>Temporary automated-test fixture. Do not enable in normal play.</description>
</ModMetaData>
"""


def fixture_root(game):
    return game / "Mods" / FOLDER


def _validate_owned_fixture(root):
    marker = root / MARKER_NAME
    about = root / "About" / "About.xml"
    dll = root / "Assemblies" / DLL_NAME
    if not marker.is_file() or marker.read_text(encoding="utf-8") != MARKER_TEXT:
        raise ValueError("Refusing to manage unowned Quicktests fixture: " + str(root))
    if not about.is_file():
        raise ValueError("Quicktests fixture About.xml missing: " + str(about))
    metadata = ET.parse(about).getroot()
    if (metadata.findtext("packageId") or "").strip().lower() != PACKAGE:
        raise ValueError("Quicktests fixture packageId changed: " + str(about))
    if not dll.is_file():
        raise ValueError("Quicktests fixture DLL missing: " + str(dll))


def stage(game, dll):
    root = fixture_root(game)
    if root.exists():
        raise ValueError("Quicktests fixture path already exists; refusing to overwrite: " + str(root))
    if not dll.is_file():
        raise ValueError("Quicktests DLL was not found: " + str(dll))
    data = dll.read_bytes()
    if not data.startswith(b"MZ"):
        raise ValueError("Quicktests DLL is not a PE assembly: " + str(dll))
    try:
        (root / "About").mkdir(parents=True)
        (root / "Assemblies").mkdir()
        (root / MARKER_NAME).write_text(MARKER_TEXT, encoding="utf-8")
        (root / "About" / "About.xml").write_text(ABOUT_XML, encoding="utf-8")
        (root / "Assemblies" / DLL_NAME).write_bytes(data)
        _validate_owned_fixture(root)
    except Exception:
        if root.exists():
            shutil.rmtree(root)
        raise
    print("STAGED AMJE Quicktests fixture:", root)


def activate(game, config):
    root = fixture_root(game)
    _validate_owned_fixture(root)
    if not config.is_file():
        raise ValueError("Isolated ModsConfig.xml was not found: " + str(config))
    tree = ET.parse(config)
    active = tree.getroot().find("activeMods")
    if active is None:
        raise ValueError("ModsConfig.xml has no activeMods: " + str(config))

    children = list(active)
    ids = [((node.text or "").strip().lower()) for node in children]
    for required in (QUICKSTARTS_PACKAGE, ENVIRONMENT_PACKAGE):
        if required not in ids:
            raise ValueError("Isolated ModsConfig.xml is missing required Mod: " + required)

    for node in list(active):
        if (node.text or "").strip().lower() == PACKAGE:
            active.remove(node)

    children = list(active)
    ids = [((node.text or "").strip().lower()) for node in children]
    environment_index = ids.index(ENVIRONMENT_PACKAGE)
    node = ET.Element("li")
    node.text = PACKAGE
    active.insert(environment_index + 1, node)
    tree.write(config, encoding="utf-8", xml_declaration=True)
    print("ACTIVATED AMJE Quicktests fixture in:", config)


def cleanup(game):
    root = fixture_root(game)
    if not root.exists():
        print("AMJE Quicktests fixture already absent:", root)
        return
    _validate_owned_fixture(root)
    shutil.rmtree(root)
    print("REMOVED AMJE Quicktests fixture:", root)


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    sub = parser.add_subparsers(dest="action", required=True)

    make = sub.add_parser("stage")
    make.add_argument("--game", type=Path, required=True)
    make.add_argument("--dll", type=Path, required=True)

    use = sub.add_parser("activate")
    use.add_argument("--game", type=Path, required=True)
    use.add_argument("--config", type=Path, required=True)

    remove = sub.add_parser("cleanup")
    remove.add_argument("--game", type=Path, required=True)

    args = parser.parse_args()
    if args.action == "stage":
        stage(args.game.resolve(), args.dll.resolve())
    elif args.action == "activate":
        activate(args.game.resolve(), args.config.resolve())
    else:
        cleanup(args.game.resolve())


if __name__ == "__main__":
    main()
