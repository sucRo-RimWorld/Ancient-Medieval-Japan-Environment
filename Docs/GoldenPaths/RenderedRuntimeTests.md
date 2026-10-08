# Rendered runtime tests on an isolated Windows desktop

## Standard AMJE entry point

Use:

```bat
run-tests.bat
```

Optionally pass a RimWorld installation root as the first argument.

```bat
run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"
```

`run-tests.bat` is the canonical AMJE automated gate. It runs:

1. `run-static-tests.bat` for PowerShell syntax, build, installed-source Def validation, texture-copy smoke, and the installed Medieval Overhaul tree-reference audit;
2. `Scripts/Run-EnvironmentIsolatedDesktop.ps1`;
3. the runtime suite on a newly created, non-visible Windows desktop with normal Direct3D rendering;
4. the default Environment Quickstart matrix, including four AMJE climate biomes, two Vanilla wetlands, a natural-world wetland distribution scenario, and River/Coast handoff;
5. optional installed CCTO and Core/Grains integration profiles already owned by `run-runtime-tests.bat`.

The isolated launcher does not switch the active desktop and does not use
`-nographics`. Rendering therefore remains active while automated RimWorld
windows stay off the user's visible desktop. Runtime reports retain the existing
pre-launch/runtime ERROR gate, live-log capture and truncation checks.

`run-static-tests.bat` is the lower-level static-only entry point.
`run-runtime-tests.bat` remains a lower-level runtime/debug entry point; when
run directly it may display RimWorld windows. The standard `run-tests.bat`
path invokes it with `--skip-static` from the isolated desktop after the static
gate has already passed, preventing recursion and duplicate static work.

The wiring is regression-locked by
`Tests/test_run_tests_entrypoint.py`. In particular, the test requires both
wetland Quickstarts to remain part of the default runtime suite.

## Combined AMJ launcher

For a combined Grains + Environment gate, the reusable launcher remains in the
Grains repository:

[Grains IntegratedRuntimeTesting.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/blob/main/Docs/IntegratedRuntimeTesting.md)

Environment no longer depends on that repository merely to obtain a non-visible
runtime path; its own standard `run-tests.bat` now provides the same isolation
property for AMJE-only validation.

## Unified current-development test status (2026-10-08)

This page is the single runtime-testing handoff for Environment's vegetation,
regional tree sowing, and retained-wetland ecology work. It consolidates the
previously separate sowing-test and wetland-development tracks without
reinterpreting a historical PASS as a current-main PASS.

| Workstream | Current source/check | Latest evidence |
|---|---|---|
| Forest retention and regional sowing | `Tests/test_tree_sowing_contract.py` and the four forest Quickstarts | Static CI passed; latest complete runtime matrix still pending |
| Alpine natural woody vs sowable trees | `ExpectedWildTreeLikePlants` versus `ExpectedSowableTrees` | Earlier Alpine 69/70: old assertion failed; correction present in main; no rerun PASS yet |
| Wetland Phase 5 plants | `Patches/VanillaWetlandVegetation.xml` and two wetland Quickstarts | ColdBog Cypress XPath failure fixed by PR #23; current-main runtime rerun pending |
| Wetland wildlife, diseases, weather, pack animals and terrain | `Patches/VanillaWetlandEcology.xml`, `Tests/test_wetland_ecology_contract.py` and two wetland Quickstarts | PR #24 static CI passed; loaded-Def/terrain runtime PASS pending |
| River/coast handoff | `AMJRiverMapHandoffQuickstart` / `AMJCoastMapHandoffQuickstart` | Historical PASS exists; latest-main runtime rerun pending |
| World wetland distribution sanity | `AMJWorldWetlandDistributionQuickstart` (30% natural-world sample; no forced biome) | Regression gate added; installed-game result pending |

The earlier warm-forest run returned 73/74 assertions and one pre-launch
PatchOperation ERROR, before the PR #23 corrections. A later Alpine run
returned 69/70, with no captured runtime ERROR; the failed assertion
conflated Haimatsu's naturally woody presence with zero normal
growing-zone sowing options. That assertion is now corrected in main.
Neither historical failure is evidence that the latest main still fails;
neither can be turned into a PASS without rerunning it.

### What the standard runner currently proves

`run-tests.bat` runs static/build checks followed by the non-visible,
rendering-enabled runtime suite. The default runtime profile includes
**nine** Quickstarts, in order: WarmTemperate, CoolTemperate, Subalpine,
Alpine, TemperateSwamp, ColdBog, WorldWetlandDistribution, River and Coast. It then runs a focused
CCTO profile when installed and a Grains/Core integration profile when
detected. Each executed scenario requires a successful Quickstart report,
zero captured pre-launch/runtime ERROR, and complete non-truncated logs.

**The current standard runner does not automatically run all four
Vanilla/MO/CCTO/MO+CCTO combinations.** Its `run-runtime-tests.bat`
explicitly disables the isolated MO runtime profile because MO startup has
not reliably reached Quickstarts in the minimal isolated profile. The
four-profile matrix remains a distinct release/integration acceptance gate.
The ninth scenario uses 30% planet coverage and does not force a biome. It
requires natural wetland candidates and biomes, no out-of-candidate wetland
placement or missing land biome, at least 2,000 land tiles, a naturally selected
wetland settlement map, and a provisional gross upper ceiling of 20% wetland
land share. The ceiling is a regression alarm, not a historical target. The
new ninth test is **not yet runtime-verified**; a nine-scenario PASS will
still not establish the final ecological target or approve descriptions.

### Acceptance and follow-up

- Obtain one complete latest-main `run-tests.bat` result with all nine base
  Quickstarts and whichever optional profiles actually execute. Record the
  exact source commit, profile and loaded Def/runtime/error results.
- Run the release four-profile matrix separately when its isolated MO path is
  functioning, without claiming a skip is a PASS.
- Validate world-level wetland frequency after the ecology/vegetation
  changes; the original Vanilla wetland Worker and terrain generation
  continue to be reused.
- Keep `ENV-WETLAND-BIOME-001` and the vegetation Step 1 closeout open until
  runtime/world-share gates and approved bilingual descriptions are complete.

The detailed regional sowing assertions and their safe research/zone probe
are documented in [PlantSowingTests.md](PlantSowingTests.md). Wetland
composition and approval/description requirements are in
`Docs/VanillaWetlandBiomeAudit-ja.md`.

## 2026-10-08 warm-temperate runtime evidence — single scenario

The author uploaded `AMJWarmTemperateTerrainQuickstart(4).log`, generated
under isolated `TestResults/VegetationRuntime/SaveData` using RimWorld
1.6.4871. The loaded mods in this scenario were Harmony, RimWorld, RimLogging,
Quickstarts and AMJE (no CCTO/MO in the active test profile).

- The Quickstarts log records **74 PASS / 0 FAIL**, `Verification PASSED`,
  and `Exiting with code 0`. No `[ERROR]` log tag appears in this file.
- The base-biome wild woody set is
  `{AMJ_Tree_Shii, Plant_TreeMaple, Plant_TreeBamboo}`. The same three
  trees appear in the unlocked growing-zone menu; the locked menu and
  Haimatsu exclusion assertions also PASS.
- Live plant textures: 18,759 scanned, 0 bad states; live non-plant Things:
  7,840 scanned, 0 bad Things; tree texture states: 22 trees audited,
  0 failed states.
- The generated **small** test world logged 3,787 total tiles and 1,262
  land tiles, all naturally mapped to CoolTemperate (994) or Subalpine
  (268). Its `swampiness>=0.5` share was **0**; the Quickstart explicitly
  used a *forced* WarmTemperate climate/biome proxy to test its map behavior.
  This is **not** evidence of acceptable world biome/wetland distribution.
  Audit wetland and warm-biome prevalence over representative worlds/seeds
  before closing the world-share gate; do not infer a generation defect
  from this one small, unsuitable sampling world alone.

**Evidence limit:** Only the Unity/Quickstarts `.log` was supplied for
this individual scenario. Its exit code and in-log PASS are confirmed, but
the matching `.json` report, outer runner's complete-capture/truncation and
pre-launch counts, and the other seven base scenarios were not provided
in this evidence bundle. The full `run-tests.bat`/wetland Step 1 gate
remains **PENDING**. An historical warm 73/74 failure is superseded for
this individual warm scenario only; it does not establish full-matrix PASS.

## 2026-10-08 post-Highland-fix author-confirmed integration result

The author explicitly confirmed that the **post-fix rerun PASSED**; the
Japanese word `パス` referred to a successful test result, not a request
to skip running it. Accordingly, classify the Highland-corrected
**Grains/Core + MO + AMJE integration as author-reported PASS**. The
prior message describing the rerun as skipped was an interpretation error.

**Evidence levels:** `EnvironmentIsolatedRuntime(2).log` independently
records the earlier eight standalone AMJE Quickstarts (8/8 PASS) and the
focused AMJE+CCTO compatibility scenario (PASS), but also includes the
*pre-fix* Highland validation failure. A new *post-fix* log/JSON was not
included with the author's PASS confirmation. Do not infer per-scenario
counts, capture completeness, exact git revision, or absence of all
pre-launch/runtime errors from the new message alone.

The world wetland-share audit, author-approved wetland descriptions and
separate four-profile release matrix remain outside this PASS assertion.
Historical result narratives below preserve their original context.

## 2026-10-08 current isolated-matrix result (author log)

Evidence: author-supplied `EnvironmentIsolatedRuntime(2).log` from the
rendering-enabled private Windows desktop. The runner verified **all eight
standalone AMJE scenarios**: four forests including Alpine, both
TemperateSwamp and ColdBog, River and Coast. Every scenario reports PASS
and no repository-owned runtime ERROR. The focused **AMJE+CCTO** warm
loaded-Def compatibility scenario also passed with no owned runtime ERROR.

The optional **Grains/Core + MO + AMJE** integration profile initialized and
ran its first warm forest Quickstart, but the **climate gradient log validator**
failed on `Climate calibration line was not found for Highland`. Its
other integration scenarios were not attempted, and the overall runner
correctly returned FAIL. This is not an eight-scenario AMJE failure.

Root cause: `ClimateCalibrationDiagnostics.FindRepresentative` had rejected
every `Hilliness.Impassable` tile. In the tiny test world used by the
Quickstart, the highland sample was not found even though terrain
diagnostics showed 1,500m+ tiles. Climate-sampling diagnostics now prefer
ordinary traversable highland and only fall back to impassable highland
when none is available. **The Highland annual temperature is still measured
via `OutdoorTemperatureAt`; all four gradient values and strict
monotonicity remain mandatory.** The fallback is a *climate probe*, not
evidence that an impassable tile is playable. No gameplay climate, biome
generation or vegetation Def is changed by this correction.

Evidence boundary: the outer log reports each Quickstart's result and
owned-error check, but the individual structured reports/logs for the
optional integration profile are not in this upload. The full eight
standalone scenarios and focused CCTO probe can be recorded as
**runner-confirmed PASS**; the overall suite, Highland gradient
re-verification, Grains/MO integration and world-level wetland distribution
remain **PENDING** after the diagnostic fix.

## Historical verified result

2026-10-05 JST result: Environment build/static, PNG exact-copy, MO static
8-tree/15-reference audit, six base Quickstarts and the optional CCTO run PASS.
Base counts: 58/58, 57/57, 54/54, 52/52, River 3/3, Coast 3/3. CCTO 70/70.
Every report has zero pre-launch errors, complete live capture and no truncation;
runtime ERROR gates PASS. Reports remain in TestResults/VegetationRuntime.

This exercised the current local working tree, including prior uncommitted
plant-review changes. It does not mark pending human plant-state reviews
accepted, claim a full MO runtime run, or claim a combined production
Core+Environment profile. The MO gate remains static as intentionally designed.

Publication preserves newer GitHub-main gameplay-contract tests. The recorded
counts are historical local-snapshot evidence; the newer Core-profile runtime
gates remain pending and are not inferred to pass from those reports.

