# Downloaded Steam Environment Pickle E2E smoke

AMJE uses **RimWorks Pickle** (Workshop 3791648678) with test-only step
assembly and a Gherkin feature for loaded-source and biome/plant Def
verification. This is a separate runtime tool from CCTO's Pickle suite,
modeled on its already deployed feature/step/report conventions.
RimTest Redux remains appropriate for pure code-level isolated logic,
but does not establish which actual subscribed Steam content RimWorld loads.

The source of truth is the installed Workshop ID 3814638060 root.
Do not rebuild or write into it. Preflight with the candidate's external
Manifest.json. Close the ordinary game normally before automated runs.

Run from the Environment repository on Windows, with Steam, Pickle,
RimLogging, Harmony and installed MO/CCTO dependencies. Close RimWorld
normally before the automated test. The standard author-facing entry
point is **one command**:

```bat
test-run.bat
```

The batch automatically locates installed Workshop 3814638060 and
searches `%TEMP%\AMJE-Final-*\Manifest.json` for an **exact byte-identical**
matching candidate. It chooses a fresh
`%TEMP%\AMJE-Steam-Pickle-...` output, runs four Pickle profiles and
shows PASS/FAIL with the latest runner log and any scenario summary
automatically if needed. Mismatching/no manifest blocks the run;
neither Steam files nor normal RimWorld config are modified.
The batch calls Python internally, but the author need only run
`test-run.bat`, not assemble `py` arguments or find reports.

The existing `run-tests.bat` remains the separate development
static and rendered-runtime test entry. If RimWorld is installed in
a non-default directory, pass that game directory as the single batch
argument. Developer-only Python flags remain optional.

The test runner stages a temporary test-only Mod under
`RimWorld/Mods/AMJE.WorkshopPickleAudit` and a separate early Unity
error-capture observer, compiles Pickle Steps from source, and uses the
existing isolated desktop launcher. It does not write to the Steam
Workshop installation or the normal ModsConfig/Prefs. Fixtures are
retired into the fresh result folder. It runs all four profiles:
Vanilla, real MO, CCTO and MO+CCTO. Pickle must generate five passing
scenarios per profile with zero failures/skips and no Unity ERROR.

Pickle scenarios check: exactly one Environment Mod and assembly loaded
from subscribed Workshop root; all four AMJE plant Defs owned by that
root; exclusions of Poplar/Oak/Pine and alpine plants; retained wetland
exclusions; representatives' balancing commonality; four wood harvest
Def yields and MO/Vanilla resource type. These are **loaded Def checks**,
not new world-generation, tree growth, graphics or actual native cutting
jobs.

Pickle does not replace the existing full 40-report Quickstarts rendered
world/river/coast/native-cutting gate. `steam_release_cleared=false` in
the Pickle summary is deliberate until all owner publication gates
have passed. An already running game is a preflight blocker, not a
mod/gameplay failure; do not inspect nonexistent report folders.

## Downloaded metadata serialization (2026-10-08)

Steam's installed root may differ from the exact 32-file candidate in
`About/PublishedFileId.txt` or `loadFolders.xml` only. The batch runs an
automated **fail-closed semantic audit** before Pickle:

- Files added, missing or changed outside these **two** metadata paths block all tests.
- The installed Workshop ID must be exactly `3814638060` after UTF-8 BOM/whitespace handling.
- The XML must parse and have the same exact effective `loadFolders/v1.6/li=/` tree, attributes and text as the candidate. Different load paths, versions or extra entries block tests.
- The original manifest must retain the canonical SHA256 for both metadata files, and the full production validator is rerun against the downloaded gameplay bytes, with only the approved metadata values normalized **in memory**.
- The source candidate `Build-WorkshopPayload.py verify` retains strict 32/32 byte equality. The Pickle report records `manifest_audit.byte_identical=false` and the named metadata differences when equivalent formatting is accepted. **This does not clear Steam release provenance.** The full rendered map and native-cutting gates remain separate.

The normal user entry remains **`test-run.bat`**. It automatically prints the precise failed path or passes to Pickle; do not ask the author to run Python or compare XML manually.
