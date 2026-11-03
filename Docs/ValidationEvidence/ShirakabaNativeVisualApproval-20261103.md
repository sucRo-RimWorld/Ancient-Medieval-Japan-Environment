# Shirakaba autumn + UI icon/InfoCard — author visual acceptance (2026-11-03)

## Scope of explicit author approval

The author replied **「OK」** immediately after the assistant displayed **three distinct actual original-game screenshots** and stated that these appearances had not yet been human-approved: **Shirakaba autumn**, **32/64 px UI icon**, and **actual InfoCard**. This is author approval of **only those displayed Shirakaba appearances**. It does not approve Dakekanba, either species' immature state, continuous seasonal-transition appearance, new numerical/gameplay balance, nor full PR #310 merge/Workshop publication.

## Immutable original native screenshot evidence

Original [Project generic manual native Actions #41435799301](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41435799301), archived native **Artifact 13195990324**, original **RimWorld 1.6.4871 rev600 Vanilla**, exact tested Environment PR #310 owner source `0cf3564a4661408eb1ff5f3ad1fab1fb36ec6e3b`. Its original `manual-native-result.json` explicitly lists all three `visual/captures` as technical PASS with `unity_error_count=0`. The production Birch Def/PNG assets inspected on the later owner branch were unchanged relative to the tested runtime source; separate changes in test-only old-save observer and evidence text do not re-render or authorize revised sprites.

| Actual original screenshot | Framebuffer | Original archived PNG SHA-256 | Verified condition and scope |
| --- | --- | --- | --- |
| `visual/autumn.png` | 1280×720 RGB | `1a5740dd429dc84aa2ba9235de294f7bef1693f81c91d6a70a566c36c2122554` | Mature and 30% growth test specimens; forced full native fall shader color, **not** a naturally elapsed autumn |
| `visual/icon.png` | 1280×720 RGB | `c5b69ac4204a7669ff07e05dd908ed1a5672f8d0b1fa8c3314b2e0808059ff99` | Real `Widgets.DefIcon` rendering at **32 and 64 px**, no synthetic image replacement |
| `visual/infocard.png` | 1280×720 RGB | `e103728b85912874238ed442c79ffe1b2877ac057b8ed8811c3745336af4b808` | Actual original `Dialog_InfoCard(ThingDef)`, including real UI thumbnail, **not** approval of gameplay/stat numbers displayed in the card |

The precise three screenshots shown in chat were compared against native Artifact 13195990324 and their byte-for-byte SHA-256 hashes matched. Their originals remain in that Actions artifact; **no replacement production art or copied fallback image** was introduced by this author decision.

### Native autumn review controls

The original `visual/autumn.unity.log` contained:

`AMJ_PLANT_VISUAL_READY mode=autumn verified=True count=6 localHour=12 season=Fall day=7 weather=Clear paused=True rootSize=23 forcedAutumn=True forcedLeafless=False uiIconValid=True`.

Source-reviewed generic `AMJPlantVisualQuickstart` sets `map.weatherManager.TransitionTo(Clear)`, `lastWeather=Clear`, `curWeatherAge=10000`, `PlantFallColors.FallIntensityOverride=true` and `FallIntensity=1f` in autumn mode, calls the real `PlantFallColors.SetFallShaderGlobals(map)`, pauses `Find.TickManager`, and rejects a noncomplete weather transition (`TransitionLerpFactor < 1`) under the required `verified=True` marker. Frame size 1280×720, camera `rootSize=23`; local-hour=12, season=Fall (day7), snow-free mature test subjects and 30% growth samples. This is an **accepted forced-state color image** under these exact capture conditions, not human approval of continuous seasonal timing.

Separate actual same-tree calendar cold/leafless/recovery original game evidence exists (75/75 Shirakaba source-specific native assertions) but remains scoped as accelerated sampled **mechanics**; no new natural-transition *appearance* acceptance was inferred.

## Ledger and remaining coverage

Update `Docs/PlantVisualCoverage.json` only for **`AMJ_Tree_Shirakaba/autumn`** and **`AMJ_Tree_Shirakaba/icon`**. Each carries this evidence path, the exact source-valid production state fingerprint, date and direct author `OK` statement. The InfoCard belongs to the same icon/UI presentation review; it is not an eighth plant state. `AMJ_Tree_Shirakaba/immature` and `transition` remain **pending**, hence `complete=false`. All Dakekanba pending rows remain unchanged. Previously accepted `normal`, `snow`, `leafless` and tested gameplay are preserved unchanged.

## Source / release scope

No production XML, color palette, tree image PNG/DDS, growth speed, harvest yield, geography, save migration, test harness, or runtime logic changes result from this approval. PR #310 stays open until separate owner gates are satisfied; no Workshop publication authorization is inferred.
