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
RimLogging, Harmony, and optional installed MO/CCTO dependencies:

\`\`\`powershell
$release = "C:\Users\sucRo\AppData\Local\Temp\AMJE-Final-20261008-214525"
$steam = "D:\SteamLibrary\steamapps\workshop\content\294100\3814638060"
$pickleRun = Join-Path $release ("Steam-Pickle-" + (Get-Date -Format "yyyyMMdd-HHmmss"))
py .\Scripts\Run-WorkshopPickle.py --payload $steam --manifest "$release\Manifest.json" --output $pickleRun
if ($LASTEXITCODE -ne 0) { throw "Pickle test failed or was blocked" }
Get-Content "$pickleRun\SteamPickleSummary.json" -Raw -Encoding UTF8
\`\`\`

The test runner stages a temporary test-only Mod under
\`RimWorld/Mods/AMJE.WorkshopPickleAudit\` and a separate early Unity
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
world/river/coast/native-cutting gate. \`steam_release_cleared=false\` in
the Pickle summary is deliberate until all owner publication gates
have passed. An already running game is a preflight blocker, not a
mod/gameplay failure; do not inspect nonexistent report folders.
