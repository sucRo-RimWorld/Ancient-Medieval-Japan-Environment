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
2. `run-framework-tests.bat --skip-static`, which runs the separate RimTest Redux logic/calculation suite and the four-profile development Pickle loaded-Def matrix on non-visible isolated desktops;
3. `Scripts/Run-EnvironmentIsolatedDesktop.ps1`;
4. the rendered runtime suite on a newly created, non-visible Windows desktop with normal Direct3D rendering;
5. the default Environment Quickstart matrix, including four AMJE climate biomes, two Vanilla wetlands, a natural-world wetland distribution scenario, and River/Coast handoff;
6. optional installed CCTO and Core/Grains integration profiles already owned by `run-runtime-tests.bat`.

The isolated launcher does not switch the active desktop and does not use
`-nographics`. Rendering therefore remains active while automated RimWorld
windows stay off the user's visible desktop. Runtime reports retain the existing
pre-launch/runtime ERROR gate, live-log capture and truncation checks.

`run-static-tests.bat` is the lower-level static-only entry point. `run-framework-tests.bat` is the lower-level RimTest + Pickle development-integration entry point.
`run-runtime-tests.bat` remains a lower-level runtime/debug entry point; when
run directly it may display RimWorld windows. The standard `run-tests.bat`
path invokes it with `--skip-static` from the isolated desktop after the static
gate has already passed, preventing recursion and duplicate static work.

Developer Quickstarts are no longer loaded through the production Mod's
`loadFolders.xml`. The tracked loader is byte-identical to the root-only
subscriber contract. `build.bat` still compiles the developer Quicktests DLL
under `DevQuickstarts`, but `run-runtime-tests.bat` stages that DLL as a
standalone temporary Mod, adds only its packageId to each isolated test
`ModsConfig.xml`, and removes the owned fixture on exit. The fixture manager
refuses to overwrite or delete a same-named directory unless its ownership
marker and packageId match. This keeps test assemblies out of normal AMJE load
semantics and prevents YADA filtering from depending on an executable
`IfModActive` loader entry.

The wiring is regression-locked by
`Tests/test_run_tests_entrypoint.py`. In particular, the test requires both
wetland Quickstarts to remain part of the default runtime suite and protects
the root-only production loader / standalone Quicktests split.

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
| Forest retention and regional sowing | `Tests/test_tree_sowing_contract.py` and the four forest Quickstarts | New nine-scenario run author-reported PASS; individual log evidence not attached |
| Alpine natural woody vs sowable trees | `ExpectedWildTreeLikePlants` versus `ExpectedSowableTrees` | Earlier 69/70 was a superseded historical failure; new nine-scenario run author-reported PASS, no individual log |
| Wetland Phase 5 plants | `Patches/VanillaWetlandVegetation.xml` and two wetland Quickstarts | PR #23 correction included; new nine-scenario run author-reported PASS, no individual log |
| Wetland wildlife, diseases, weather, pack animals and terrain | `Patches/VanillaWetlandEcology.xml`, `Tests/test_wetland_ecology_contract.py` and two wetland Quickstarts | New nine-scenario run author-reported PASS, no individual log; historical PR #24 static CI PASS |
| River/coast handoff | `AMJRiverMapHandoffQuickstart` / `AMJCoastMapHandoffQuickstart` | Historical PASS exists; new nine-scenario run author-reported PASS, no individual log |
| World wetland distribution sanity | `AMJWorldWetlandDistributionQuickstart` (30% natural-world sample; no forced biome) | PR #27 merged as `7264c5e`; author-reported installed-game PASS (2026-10-08), detailed logs/counts not attached |

The earlier warm-forest run returned 73/74 assertions and one pre-launch
PatchOperation ERROR, before the PR #23 corrections. A later Alpine run
returned 69/70, with no captured runtime ERROR; the failed assertion
conflated Haimatsu's naturally woody presence with zero normal
growing-zone sowing options. That assertion is now corrected in main.
Neither historical failure is evidence that the latest main still fails;
neither can be turned into a PASS without rerunning it.

### Wetland descriptions — Japanese approved, author-reported runtime PASS (2026-10-08)

The author approved the revised Japanese `TemperateSwamp` and `ColdBog`
descriptions. The approved text is in `Docs/VanillaWetlandBiomeAudit-ja.md`,
with a corresponding English translation. **As of the original 2026-10-08 test**,
`Patches/VanillaWetlandDescriptions.xml` replaced only two English
`BiomeDef.description` fields and Japanese DefInjected only their descriptions;
at that point Biome labels were unchanged. **The 2026-10-10 naming update**
adds `BiomeDef.label` replacements and Japanese `.label` entries for
temperate wetland / 温帯湿地 and cool wetland / 冷涼湿原. `defName`, BiomeWorker
and gameplay fields remain unchanged. `Tests/test_wetland_ecology_contract.py`
now checks both labels and retained descriptions in both languages, while
each existing wetland Quickstart assertion checks loaded label **and** description.
The 2026-10-08 reported runtime PASS belongs to the **old description-only**
commit; do not apply it to the new names without a fresh native run.

The author separately reported that the newly updated runtime test PASSED
on 2026-10-08, after the description assertions were added to PR #28. This is
**author-reported PASS**, distinct from the older PR #27 nine-scenario result.
No new `.json` or `.log` files were attached: individual assertion counts,
complete capture and zero-owned-ERROR status have not been independently
inspected. The four-profile Vanilla/MO/CCTO/MO+CCTO release matrix is still
open and must not be inferred from the author's report.

### Approved Bamboo description — author-provided runtime-gate PASS (2026-10-08)

The Japanese description for Vanilla `Plant_TreeBamboo` is approved and
maintained verbatim in `Docs/VanillaPlantStep2DescriptionReview-ja.md`.
`Patches/VanillaTreeDescriptions.xml` replaces only that PlantDef's
English description, and Japanese ThingDef DefInjected provides the approved
Japanese prose. Both preserve the two-paragraph boundary (`\\n\\n`);
the Vanilla label and all plant gameplay fields remain unchanged. The
warm-temperate Quickstart checks the loaded description against the
approved EN/JA text after normalizing literal and actual newlines.
`Tests/test_vanilla_bamboo_description.py` guards the source text, patch
scope and test wiring.

The author supplied the tail of `TestResults/EnvironmentIsolatedRuntime.log`
after running the updated test path. The excerpt confirms:
- `[OK] AMJCoastMapHandoffQuickstart passed.`
- `[OK] No owned AMJ runtime ERROR entries were found.`
- `[OK] AMJ Core + Environment gameplay-contract Quickstarts passed.`
- `[OK] Environment runtime gate passed.`
- The run emitted separate base, CCTO, and Grains/Core report paths.

**Evidence boundary:** this is the *provided tail*, not the complete
nine-scenario JSON/log set or a separately captured executable revision.
The overall runtime-gate PASS is accepted as author-supplied evidence for this
post-Bamboo test run; per-assertion Bamboo details and the exact source SHA
are not independently audited. The previous PR #27/#28 PASS remains distinct.
Medieval Overhaul isolated runtime stays disabled by the standard harness;
the full four-profile release matrix is still OPEN. The five additional Vanilla
texts were subsequently approved on 2026-10-08 and implemented with new
loaded-text assertions, but this historical Bamboo runtime result does not
cover those new assertions.

**Progress-visibility limitation:** `Run-EnvironmentIsolatedDesktop.ps1`
redirects the inner `run-runtime-tests.bat` output to
`TestResults/EnvironmentIsolatedRuntime.log`. Its console only prints
`[WINDOWS]` when a new desktop-window process list is seen, not once per
scenario. A stationary `[WINDOWS]` line is therefore not evidence of a hang.
Use `Get-Content .\\TestResults\\EnvironmentIsolatedRuntime.log -Tail 30 -Wait`
to follow the existing redirected progress. Adding safe real-time progress
mirroring without duplicate or destructive logging is a separate harness
improvement, not part of the Bamboo description implementation.

### Approved retained Vanilla trees — latest gate author-reported PASS (2026-10-08)

The author approved the original Japanese drafts for Maple/Oak/Birch/Pine/Willow
after comparing their format with the four AMJE-owned structural plant descriptions
and the previously accepted Bamboo. All six Vanilla tree descriptions use two
paragraphs; the existing AMJE structural trees use three. Both are permitted by
the shared Historical Description Guidelines: name/aliases, Japan ecology and
ancient/medieval role or landscape, and modern comparison only where justified.
Birch/Willow do not invent otherwise unsupported medieval resource uses.

`Docs/VanillaPlantStep2DescriptionReview-ja.md` preserves the approved Japanese
paragraphs and their corresponding new English translations.
`Patches/VanillaTreeDescriptions.xml` replaces only six English
`ThingDef.description` fields; `AMJ_WildPlants.xml` DefInjected has all six
approved Japanese descriptions. `Tests/test_vanilla_bamboo_description.py`
now protects all six approved texts, description-only patch scope, paragraph
format and loaded-test wiring. The warm-temperate Quickstart checks the loaded
English *or* Japanese text of each tree via the DefDatabase; no planting or
world-generation behavior was changed.

**Acceptance boundary:** PR #30 passed all three GitHub static jobs and was
squash merged as `bfb27420428dc75a060a317b3c22bdd4c5856fa8`. On
2026-10-08 the author reported that the updated installed-game `run-tests.bat`
**passed** with the five new loaded-description assertions present. Record this
as **author-reported latest runtime PASS**, not independently inspected output:
no fresh per-scenario JSON/full runner log or owned ERROR-zero capture was
attached. Do not infer precise assertion counts or independent error audit.
The earlier Bamboo runtime-gate PASS remains distinct historical evidence.
Vanilla/MO/CCTO/MO+CCTO four-profile release testing remains separate and OPEN.

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
new ninth test was reported **PASS** by the author on 2026-10-08. The actual
per-scenario `.json`/`.log` was not attached, so counts, precise wetland shares,
complete capture, and zero-owned-ERROR evidence have not been independently
inspected. Even a complete PASS would not establish a final ecological target
or approve bilingual wetland descriptions.

The natural-world scenario additionally logs candidate share and the
fraction of wetland candidates actually assigned a retained wetland. The
complete land scan skips redundant settlement-suitability checks once
starting-tile choices are resolved. Only this 30%-coverage scenario receives
a 420-second timeout; the earlier eight retain their existing timeouts. The
outer isolated-desktop limit remains 1800 seconds. These are safeguards,
not measured runtime outcomes.

### Acceptance and follow-up

- Preserve the author's reported PASS for the new nine-scenario test. Do not
  infer exact counts or independent log certification; if structured logs are
  provided later, record the actual source revision, optional profiles,
  capture integrity, owned ERROR status, and wetland shares.
- Run the release four-profile matrix separately when its isolated MO path is
  functioning, without claiming a skip is a PASS.
- Validate world-level wetland frequency after the ecology/vegetation
  changes; the original Vanilla wetland Worker and terrain generation
  continue to be reused.
- The wetland Step 1 baseline (wildlife, diseases, weather, vegetation,
  terrain, natural distribution and approved bilingual descriptions) is
  accepted **on author-reported runtime PASS**, not independently audited logs.
  Retained-plant selection and Japanese description review can proceed;
  existing-tree retexture remains deferred. Actual wetland share calibration
  and the separate four-profile release matrix remain open.

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

