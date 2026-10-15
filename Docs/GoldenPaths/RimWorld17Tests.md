# RimWorld 1.6 / 1.7 test entry points

Use `run-tests.bat "PATH_TO_RIMWORLD"`. Version.txt selects the runtime; 1.7 uses
RimWorld.exe, root managed DLLs, UnityStubs and Vulkan. 1.6 retains its original
executable, managed references and production build path.

The shared 1.6 production DLL passed 1.7.4976 rev1043 native runtime tests. Keep
that DLL shared between supported versions. On 1.7, Build-17Tests.ps1 rebuilds
the developer Quicktests only, using an installed dotnet SDK's Roslyn compiler.
It does not replace the shared production DLL with a .NET 10 binary that 1.6
cannot load. Production source changes still require the owning production
build/release gate; this test-tooling port does not establish a 1.7-only product
build workflow.

Python 3 must be available as `python`. Quickstarts, RimLogging, Harmony,
RimTest Redux and its Laboratory dependency, and Pickle remain required for
their existing gates. The standard full entry retains static validation,
rendered Quickstarts, ten RimTest tests and four development Pickle profiles.
An external Mod/framework failure must not be reported as a full PASS.

RimWorldRuntime.ps1 selects reference assemblies and performs hidden desktop
launches. In 1.7 the engine ignores Unity's -logFile override: each run must
copy the native Logs/Player.log after exit and verify its Process pid. A named
mutex and an existing-game check prevent concurrent capture collisions. 1.7
development Pickle consequently requires --workers 1. Native ERROR entries,
missing/wrong-process logs, timeout and nonzero exit all fail; existing report,
capture and source-preservation gates also remain mandatory. Do not weaken
these gates or disable rendering. Pickle uses -pickle-no-http to avoid starting
an unnecessary HttpListener dashboard during automation.

CCTO owns an identical portable copy of the runtime helper; neither Mod takes
a runtime dependency on the other for test tooling. Keep both copies identical.
Its standard run-tests.bat uses the 14-scenario 1.7 entry with the lightweight
29-plant MO balance fixture. Valid fixture graphics and non-edible metadata
are necessary even when only temperature values are being tested; they do not
establish compatibility with actual Medieval Overhaul gameplay.

Initial unchanged-DLL evidence and its test premises are in local
TestResults/Compatibility17-20261013/Result.md. Updated-entry evidence must be
collected separately; old PASS counts cannot certify later tooling changes.
SupportedVersions/loadFolders/README preparation does not update Steam.

## Linux Waylith 1.7 source-pinned loaded-game smoke (2026-10-15)

The shared Project runner now has a separate on-demand **test-only** entry
`/amj environment-nested-17` on private Project Issue #39. It uses the verified
Xvfb → Weston13 GL (X11 backend) nested display and genuine separately owned
**Linux RimWorld 1.7.4977 rev1248**. The exact source was pinned to Environment
`30c6ab64998fbd7dd61e537da138684caf61a9c1`. The original eight C# production
source files were compiled without code edits, using the **actual 1.7**
`UnityStubs.dll` as an explicit compiler reference; test-built
`AncientMedievalJapanEnvironment.dll` SHA-256:
`88c43d21fe8dc0f5b1be840c682f3ff3718ca5a67019dcb2cc49574fe133a238`.
The original game 1.7 `UnityStubs.dll` SHA-256:
`b9e08c6bf72f57fa6f0c09277eadca209a30b157508795072f2dd9f79de655dd`.
No 1.6 DLL, runtime shim or binary substitution was installed into the clone.

**Actual [Project Actions #38748267869](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38748267869), artifact
`amj-environment-native17-38748267869` (ID 11980685032):** original
production assembly bootstrap logged once; all four owned
`AMJ_WarmTemperateForest`, `AMJ_CoolTemperateForest`,
`AMJ_SubalpineForest`, `AMJ_AlpineZone` BiomeDefs and
`AMJ_ThinSoil` TerrainDef resolved in a real 50×50 Quickstarts map.
One exact Pickle 6.7.0 scenario **1/1 PASS**, no failed/skipped/flaky,
real game exit **0**, native ERROR **0**, FMOD ERROR **0**, no timeout;
original full launcher log, separate Pickle summary and compiler logs retained.
Official Harmony3, RimLogging 3.1.4 and Quickstarts 4.1.4 were loaded.
Original owned game and Mod sources remained unchanged.

The earlier [#38747227373](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38747227373)
failed at compilation because the 1.7 managed reference directory lacked
`UnityEngine.CoreModule.dll`; a trial with read-only 1.6 Unity references
[#38747815467](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38747815467)
also failed from conflicting 1.7 type identity. Source-owned test harness
[Project PR #346](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/346)
now selects only actual Waylith `UnityStubs.dll`; do not repeat the
legacy-reference mix or replace the shipping 1.6 DLL with the test-only
.NET 10 output.

**Acceptance boundary:** This proves only this pinned source's real loaded
assembly, five Defs and one synthetic-size map in a **Vanilla** native profile.
It does not verify the shipping shared net472 DLL on Linux 1.7, world-scale
climate/terrain/river/coast proportions, wild vegetation, cutting/harvest,
MO/CCTO profiles, existing saves, UI/art, or an actual Workshop subscriber
package. Keep earlier Windows 1.7 owner runtime evidence separate.
