# RimTest Redux + Pickle development automation

This is the owner procedure for repeatable Environment framework tests against the **development Mod root**. It complements, rather than replaces, the existing rendered Quickstarts and the separate downloaded-Workshop Pickle release audit.

## Responsibilities

- **RimTest Redux** owns fast in-game logic/calculation regression checks. The current suite covers climate-band boundaries, wetland eligibility, biome score behavior, annual-temperature calculation, elevation mapping and hill-class uplift.
- **RimWorks Pickle** owns loaded-source / loaded-Def integration. The development suite runs the core plant ownership, biome exclusion/commonality and harvested-wood contracts in `Vanilla`, `MO`, `CCTO` and `MO-CCTO` profiles.
- **Quickstarts** remain authoritative for generated-world/map, terrain/rendering and native-job scenarios that require a full generated state.
- **Workshop Pickle** remains a separate release/provenance check against the actual Steam subscriber root and an approved publication manifest.

RimTest/Pickle test assemblies are built into temporary standalone test Mods under `RimWorld/Mods` and retired into the fresh test-result directory after each run. The production Environment assembly never references either test framework.

## Entry points

Run only the framework layer:

```bat
run-framework-tests.bat
```

The standard full gate is still:

```bat
run-tests.bat
```

The full gate runs static validation first, then the RimTest + Pickle framework layer, then the existing rendered Quickstarts on an isolated Windows desktop.

An alternate RimWorld path may be passed as the first argument. `run-framework-tests.bat <RimWorldDir> --skip-static` is reserved for the already-static-validated full-gate chain.

## Required developer Mods

The framework gate expects these development dependencies to be installed locally:

- Harmony;
- RimTest Redux and ilyvion's Laboratory;
- RimWorks Pickle and RimLogging;
- Medieval Overhaul and its existing dependencies for the MO profiles;
- CCTO for the CCTO profiles.

The Environment repository itself must be a built local Mod under `RimWorld/Mods`. The harness refuses a different source root so a Steam copy cannot silently satisfy development tests.

## Isolation and evidence

Both runners:

1. refuse to start while a normal `RimWorldWin64.exe` is already running;
2. create isolated `-savedatafolder` profiles and do not rewrite the user's normal `ModsConfig.xml` or `Prefs.xml`;
3. run through `Tests/Release/IsolatedDesktopRunner.cs`, preserving normal Direct3D rendering without showing or stealing the user's desktop;
4. capture a dedicated `Player.log` plus the independent Unity error observer;
5. fail if the expected structured result is missing/incomplete, if any expected test/scenario fails/skips, if the runtime log is incomplete, or if the independent error capture records an ERROR;
6. verify that the Environment source tree and normal RimWorld configuration are unchanged after the run.

RimTest emits `RimTestSummary.json`. Pickle emits its normal per-profile `summary.json` files plus `DevelopmentPickleSummary.json`. Result directories are created under the Windows temporary directory by default and are evidence only for the exact source/provider tested.

## Coverage boundary

A green framework run establishes logic/calculation and loaded-Def integration for the tested source and profiles. It does **not** establish generated-map distribution, visual quality, terrain/rendering appearance, native cutting-job behavior, Workshop byte identity, or publication readiness. Those remain with their existing Quickstarts, art, release and Workshop gates.
