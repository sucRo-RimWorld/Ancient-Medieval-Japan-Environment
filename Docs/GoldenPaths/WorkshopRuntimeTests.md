# Actual Workshop payload release gate

Publication provenance and candidate preparation: [WorkshopPublication.md](WorkshopPublication.md).
The map-only PASS does not clear the current native-cutting release contract.

## 2026-10-08 candidate: updated strict Quickstart assertion counts

The author built a 32-file candidate from
`1d46727102f53243099927b59881091d448dba61`. The first Vanilla
WarmTemperate JSON reports 80/80 PASS, zero failures/pre-launch/log errors,
complete capture, no truncation; its hidden desktop runner exited zero. The
outer gate rejected this passing report because it expected the historical
58 assertions, not the current source's 80. This is a test harness
count mismatch; it is **not** a four-profile PASS.

Strict current-source expected map assertion counts:

| Mode | Warm | Cool | Subalpine | Alpine | River | Coast | Map total | Native cutting |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Vanilla / MO | 80 | 73 | 70 | 68 | 3 | 3 | 297 | 9 |
| CCTO / MO+CCTO | 92 | 85 | 82 | 80 | 3 | 3 | 345 | 9 |

Compared with the historical 58/57/54/52 baseline, the current source
runs 16 new tree-sowing checks in each forest biome and six additional
approved EN/JA descriptions in WarmTemperate. CCTO adds 12 temperature
checks in each forest biome. Both the runtime gate and the saved-log
combination gate enforce the same **exact counts**, alongside the original
source/provenance, Direct3D, required mod membership, independent Unity
ERROR, full-log and four-native-cutting gates. The current test suite
locks these values to the corresponding Quickstart assertions.

Only WarmTemperate Vanilla 80/80 has new user-provided runtime evidence;
the other expected totals are statically derived and **await the new
runtime matrix**. Preserve earlier reports and candidate bytes, and use
a fresh result directory for a rerun after the test-only source repair.
No actual Steam Workshop update/download is claimed.

## 2026-10-06/07 JST result

The installed Steam payload is Workshop **3814638060**, packageId
`sucro.ancientmedievaljapan.environment`, root
`D:\SteamLibrary\steamapps\workshop\content\294100\3814638060`.
Steam's `appworkshop_294100.acf` records installed and latest manifest
`8889000939694660295`, size 27998024 bytes, timeupdated 1791295309.
This gate exercised that installed payload without rebuilding or modifying it.

| Configuration | Six map/world/texture/Def checks | Current four-plant native cutting regression |
| --- | --- | --- |
| Core + AMJE | PASS 227/227 | FAIL 7/9; Haimatsu resource and output |
| Core + MO + AMJE | PASS 227/227 | FAIL 7/9; Haimatsu resource and output |
| Core + CCTO + AMJE | PASS 275/275 | FAIL 7/9; Haimatsu resource and output |
| Core + MO + CCTO + AMJE | PASS 275/275 | FAIL 7/9; Haimatsu resource and output |

Here Core means RimWorld's Vanilla Core, not Ancient & Medieval Japan Core.
Harmony, RimLogging, Quickstarts and the test-only source observer were added;
MO profiles also loaded the actual Workshop VEF and SYR Processor dependencies.
CCTO was explicitly selected from Workshop using its `_steam` profile suffix.
DevKit/Rim Control and normal-user gameplay mods were not active.

The six scenarios are WarmTemperate, CoolTemperate, Subalpine, Alpine, River
and Coast. Base assertion counts are 58/57/54/52/3/3; CCTO counts are
70/69/66/64/3/3. All 24 reports have zero failed assertions, zero logErrors and
preLaunchErrors, captureLive=true and logTruncated=false. An independent scan
of full logs found no structured ERROR. Each log proves the single AMJE
ModContentPack and production DLL came from the Workshop path above.
MO's 25 loaded tree graphic families passed; supplementary MO and MO+CCTO
checks verified final DarkForest fertility entries
AMJ_ThinSoil/Gravel/Soil/SoilRich and unique ownership of AMJE's four plants.
All 11 production PNGs passed decoded-image validation.

The cutting regression deliberately uses the **current main test code** in a
separate observer namespace while the production Defs/DLL remain the unchanged
Workshop payload. In all four configurations, Shii/Beech/Shirabiso output
42/40/30 of WoodLog (standalone) or DankPyon_RawWood (MO); Haimatsu is destroyed
but yields zero. Its two assertions fail against the accepted base yield of8.
These are assertion failures with zero runtime exceptions, not startup crashes.

## Cause and release decision

The installed Haimatsu Def inherits BushBase and lacks harvestedThingDef,
harvestYield and harvestTag. Current main already contains the explicit
WoodLog/base8/wood-tag fix in `d38e28544693f8ac2d095143b64a37de68ab887a`.
The optional MO resource conversion is owned by MO, not a new AMJE override.
No further production-code/XML repair was needed in this investigation.

The existing current-main `Run-PlantHarvestTests.ps1 -Mode Both` was rebuilt
and rerun on the isolated desktop: Vanilla9/9 and actual MO9/9, zero captured
Unity ERROR and zero structured ERROR, complete reports, four real cutting
outputs per profile. Result directory: TestResults/Harvest-20261007-000033.
This confirms the existing repair, but is explicitly not a Workshop PASS.

The Workshop payload also retains Alpha metadata and older plant descriptions.
Its production Defs are therefore not identical to current main. A passing
map suite must not be reported as a passing current-main release contract.

**Release gate: HOLD** pending the author's manual Workshop update and a rerun
against the newly downloaded actual payload. Runtime coexistence is healthy,
but the published payload fails the existing four-species cutting regression.
Do not overwrite the installed Workshop folder to simulate a release or claim
that testing a main-derived fixture validates the published Workshop version.

Warnings: Japanese's 36 Def-injection load errors are inherited Vanilla keys
(PsychicAmplifier, PawnRenderTree, quest/stat/apparel/mechanoid/thought paths),
not AMJ keys. The diagnostic report also identifies one inherited
FactionGreetingWarm argument mismatch. Installed-but-inactive mods produce
metadata dependency-URL/display-name warnings during discovery. The tiny
Quickstart world has no representative Highland climate tile, producing one
AMJE warning per scenario; this does not establish annual highland climate
coverage. These warnings are retained, not suppressed.

## Repeatable procedure

1. Read main AGENTS/Coordination and the actual production/compatibility XML.
   Read `PlantHarvestTests.md` as well as the old rendered map gate; shipped
   developer assemblies can be older than current regression contracts.
2. Record About/packageId, Workshop ID, installed/latest ACF manifest and a
   SHA-256 manifest of every installed Workshop file. Hash normal ModsConfig
   and Prefs before/after. Results and save profiles go outside the payload.
3. Generate four isolated profiles with the existing
   `Prepare-EnvironmentRuntimeTestSaveData.ps1` flags IncludeMedievalOverhaul
   and IncludeCCTO as appropriate. Normalize IDs to lowercase. Select AMJE
   with `sucro.ancientmedievaljapan.environment_steam` and CCTO with
   `sucro.cropcoldtoleranceoverhaul_steam`; the suffix alone is not proof.
4. Compile `Tests/Release/WorkshopSourceAudit.cs` into a separate temporary
   observer mod (`sucro.amje.workshopfinalobserver`), add it to the profiles,
   and require its PASS marker in every runtime log. It rejects a local
   production DLL, duplicate/wrongly owned AMJE plant Defs and incorrect MO
   DarkForest soil entries. Set AMJE_TRANSLATION_OUTPUT to an isolated file
   to obtain precise inherited translation diagnostics without changing data.
5. Run all six existing vegetation/water Quickstarts for each profile using
   `Run-EnvironmentVegetationQuickstarts.ps1`, timeout420. Do not call
   run-runtime-tests.bat as Workshop proof: it builds local code and skips MO.
6. Stage the newer observer sources with:
   `python Scripts/Stage-WorkshopRegressionSources.py TestResults/WorkshopObserver`.
   Compile WorkshopHarvestQuickstarts.cs into the observer mod, referencing
   the same installed game/Unity/Harmony/Quickstarts assemblies as build.bat.
   The namespace, Harmony ID and Quickstart names are separated automatically;
   no production Def, patch, DLL or shipped developer assembly is replaced.
7. Run the emitted Run-WorkshopHarvestQuickstarts.ps1 with TreeTextureAuditOnly
   for each profile. Set RIMWORLD_AMJE_HARVEST_EXPECTED to WoodLog without MO
   or DankPyon_RawWood with MO. Its renamed WarmTemperate scenario invokes
   the current native-cutting assertions through the existing Verify path.
   Preserve failed reports; do not stop after a passing shipped older suite.
8. Launch the batch on an independent Windows desktop using the existing
   `Tests/Release/IsolatedDesktopRunner.cs` compiled executable. Steam must run
   in the same normal user session. Keep Direct3D enabled, never switch the
   desktop and do not use -nographics. Set PSModulePath to Windows PowerShell's
   system modules only in this test process tree. Maintain an outer timeout.
9. Require exact profile membership and source/DLL markers, complete reports,
   all assertions passing, zero pre-launch/global/mod-origin ERROR entries,
   and unchanged full payload/config manifests. A batch that continues after
   individual failures is only an evidence collector, not a PASS gate.
10. Remove only the temporary observer/fixture roots created by this run,
    retaining source, reports, runtime logs, manifests and failed assertions.

## Evidence location

The full run is retained locally under
`C:\Users\sucRo\.codex\.chatgpt-projects\g-p-6abf6039d3d08191b279b8154951a150\WorkshopFinalTest`:
VerifiedSummary.json, Reports-*, Harvest-*, Supplemental-*, Translation-*,
WorkshopBefore.json, appworkshop-before.acf, PngValidation.log and runner sources.
All 1112 original Workshop files and the normal ModsConfig/Prefs were unchanged
after all runs. During closeout Steam downloaded an additional About/preview.png;
installed/latest manifest became 8630945342812668549, size28329880,
timeupdated1791296780. No existing file hash changed, including all runtime
Defs/patches/DLLs/textures. The runtime result and Haimatsu defect are unchanged.
This is an observed external Steam metadata/media update, not an agent upload.
These reports are local runtime evidence, not a cloud CI game execution.

Long-running colony play, autonomous cutting/hauling and full-year highland
balance are not claimed by this short map/native-job gate.


## 2026-10-07 JST downloaded manifest 7945743700437607405

Actual installed Workshop root, 26 files, tested without rebuilding or editing
production bytes. Hidden Direct3D four-profile runtime PASS: Vanilla and MO
227/227 map assertions each, CCTO and MO+CCTO 275/275 each; native cutting 9/9
in every profile, including Haimatsu. All 28 completed reports have zero ERROR,
complete live capture and real Workshop source/DLL markers; seven independent
Unity captures per profile also pass. Payload and normal ModsConfig/Prefs hashes
were preserved, and temporary observers were retired.

Evidence: TestResults/WorkshopDownloaded-20261007-Run1 and Run2;
Run2/VerifiedSummary.json combines the completed profiles. Run1 CCTO failed
before world generation with Quickstarts LogCapture.CountErrors collection
modification exception. Its failed log is retained; a fresh CCTO rerun passed.
This is a test-harness failure, not evidence of an AMJE compatibility defect.

The downloaded payload is NOT the exact approved candidate manifest: production
DLL, loadFolders.xml and PublishedFileId.txt bytes differ; preview filename case
also differs. Runtime XML/textures match the candidate. loadFolders retains a
conditional DevQuickstarts entry despite that folder being absent. Runtime PASS
therefore does not clear exact-candidate provenance/packaging approval. No Steam
upload, production repair, long-term play or full-year climate coverage claimed.

Repeat the existing Run-WorkshopPayloadTests.py --steam workflow; use --profiles
for a fresh focused retry and recheck all completed profile logs, independent
captures and Before/Preservation manifests. Do not discard failed-attempt logs
or substitute candidate execution for downloaded-payload runtime evidence.
