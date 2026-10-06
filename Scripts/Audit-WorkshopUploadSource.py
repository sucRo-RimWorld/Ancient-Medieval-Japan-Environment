"""Read-only same-path hash evidence; never infer uploader from timestamps alone."""
import argparse
import fnmatch
import hashlib
import json
from pathlib import Path


def inventory(root):
    return {p.relative_to(root).as_posix(): hashlib.sha256(p.read_bytes()).hexdigest()
            for p in sorted(root.rglob('*')) if p.is_file() and '.git' not in p.relative_to(root).parts}


def yada_inventory(root, inherited=()):
    # YADA2971543841 Scanner.cs applies inherited basename rules recursively.
    rules = list(inherited)
    if (root / '.rimignore').exists():
        rules += [s.strip() for s in (root / '.rimignore').read_text(encoding='utf-8-sig').splitlines()
                  if s.strip() and not s.strip().startswith('#')]
    found = {}
    for path in root.iterdir():
        if any(fnmatch.fnmatchcase(path.name.lower(), rule.lower()) for rule in rules):
            continue
        if path.is_dir():
            found.update({path.name + '/' + k: v for k, v in yada_inventory(path, rules).items()})
        else:
            found[path.name] = hashlib.sha256(path.read_bytes()).hexdigest()
    return found


def audit(workshop, development, output):
    if output.exists():
        raise ValueError('Use a fresh evidence file')
    actual = inventory(workshop)
    same = {}; missing = []; different = []
    for name, value in actual.items():
        source = development / name
        if not source.is_file():
            missing.append(name)
        else:
            same[name] = hashlib.sha256(source.read_bytes()).hexdigest()
            if same[name] != value:
                different.append(name)
    filtered = yada_inventory(development)
    report = {'workshop_root': str(workshop), 'development_root': str(development),
              'installed_files': len(actual), 'same_path_matches': len(actual) - len(missing) - len(different),
              'missing_in_development': missing, 'different_in_development': different,
              'not_in_current_yada_copy': [k for k in actual if k not in filtered],
              'different_in_current_yada_copy': [k for k, v in actual.items() if k in filtered and filtered[k] != v],
              'workshop_sha256': actual, 'same_path_development_sha256': same,
              'limits': 'Exact bytes identify content provenance, not which uploader implementation ran. Correlate Steam logs and source code; do not attribute an absent staging directory.'}
    output.parent.mkdir(parents=True, exist_ok=True)
    output.write_text(json.dumps(report, indent=2) + '\n', encoding='utf-8')
    print('Compared', len(actual), 'installed files;', report['same_path_matches'], 'same-path matches; evidence:', output)


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--workshop', type=Path, required=True)
    parser.add_argument('--development', type=Path, required=True)
    parser.add_argument('--output', type=Path, required=True)
    args = parser.parse_args()
    audit(args.workshop.resolve(), args.development.resolve(), args.output.resolve())
