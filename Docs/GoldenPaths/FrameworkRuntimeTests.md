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

## Upcoming four-species wetland/CCTO regression (approved design; not yet implemented)

The proposed CCTO temperature/dormancy values are owned by Environment `Docs/NativeVegetationStep3Design-ja.md` §3.3.1. Do not stage unapproved PlantDefs, art, or CCTO patch entries merely to make a test pass. Once the actual four wetland PlantDefs are available, run the four existing Pickle profiles (Vanilla/MO/CCTO/MO-CCTO) and the required native winter/harvest/save Quickstarts. Assert exactly one CCTO extension per AMJE wetland plant only in CCTO-enabled profiles; no extension in CCTO-free profiles. Verify the specified 0°C/5°C growth minima, 3 dormancy responses, Mizugoke −35°C death threshold, cold-to-warm recovery and real Harvest/CutPlant/drop outcomes. Distinguish loaded-Def success from generated-map, rendered snow/leafless, winter survival and old-save success.

**No game window on the user's desktop:** use `run-tests.bat` as the default full test entry, or `run-framework-tests.bat` for the isolated RimTest/Pickle subset. The standard launchers already use non-visible Windows desktops and preserve actual graphics; do not pass `-nographics`. The lower-level `run-runtime-tests.bat` can display a window if invoked directly and therefore is **not** the entrance for these unattended acceptance tests. Do not launch a second game while another RimWorld instance is running. No new Windows runtime PASS is implied by this documentation.

## Coverage boundary

A green framework run establishes logic/calculation and loaded-Def integration for the tested source and profiles. It does **not** establish generated-map distribution, visual quality, terrain/rendering appearance, native cutting-job behavior, Workshop byte identity, or publication readiness. Those remain with their existing Quickstarts, art, release and Workshop gates.
