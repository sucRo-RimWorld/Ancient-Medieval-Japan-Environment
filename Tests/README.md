# Test lifecycle and ownership

| Category | Location | Default execution |
| --- | --- | --- |
| Production | `Tests/Production/` | CI checks approved source, Def, biome and gameplay contracts. |
| Runtime | `Tests/Quickstarts/`, `Tests/Pickle/`, `Tests/RimTest/` | Existing noninteractive RimWorld runners; full runtime ERROR logs required. |
| Release | `Tests/Release/` and existing `Tests/validate_workshop_*.py` | Workshop source/payload and shipping safety. |
| Research | `Tests/Research/` | Explicit opt-in only; excluded from default CI. |

Tests remaining at the root are existing tooling/art validators or contracts pending migration. Categorization does not change the requirements of `run-tests.bat` or skip runtime tests.

Production tests must protect accepted behavior or shipped code, not an unapproved proposal. Research fixtures must have a specific question, owner, evidence destination and retirement condition; once archived, remove the fixture and its references together. Temporary fixtures in `Tests/Quickstarts/Fixtures/` are still needed by active isolated runtime scenarios and must not be deleted merely because they are temporary. Release checks remain independent of research.

When moving test modules, update `ROOT`, CI calls, any import paths and affected documentation together; test the exact PR head before merge. Do not include `Tests/Research/` in ordinary test discovery or CI glob patterns.

## RimWorld 1.7 unstable — development framework tests

The development Pickle/RimTest launchers accept 1.6 and 1.7 test configuration
versions. Their DLL resolver follows the matching provider LoadFolders entry;
when 1.7 has no entry it logs the 1.6-provider fallback as a compatibility
probe. Temporary test-only helper About metadata permits both versions.
The normal ModsConfig/Prefs remain read-only; source, runtime assembly and
public supportedVersions are not changed by this test-harness update.

The launchers read the normal ModsConfig version as test selection, not an
independent game-binary version attestation. Record the actual 1.7 game version
and fresh isolated RimTest/Pickle logs before declaring runtime PASS. Keep
1.6 and 1.7 outcomes separate; no 1.7 release support is implied.
