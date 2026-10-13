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
