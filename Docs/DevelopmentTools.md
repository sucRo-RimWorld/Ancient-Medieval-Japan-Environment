# Environment Development Tools


## 自動テスト優先方針（AMJ共通）

AMJおよび関連Modでは、RimTest Redux・Pickleを積極的に用いた自動テストを優先し、人間による手動テストを最小限にする。

- ロジック・計算・設定検証などはRimTest Redux、ロード後のDef・実際のゲーム内挙動・統合回帰などはPickleを中心に、適した自動テストで確認する。
- 新機能・不具合修正では、再現可能な確認を可能な限り自動化し、リリース前の回帰確認も自動テストへ寄せる。既存のビルド・XML・静的検証は併用する。
- 手動テストは、画像の見た目、UIの読みやすさ、操作感・遊び心地など、人間の目視・操作が必要な項目に限定する。自動で確認済みの数値や挙動を毎回手動で再確認させない。
- 自動化が未整備の項目は、未検証範囲と自動化する対象を明示する。静的検証の成功を実行時テストの成功として扱わない。
- RimWorldを起動する自動テストでは、既存の実行時ERROR検出方針を必ず適用する。シナリオが全件成功しても対象Mod由来のERRORがあれば全体を失敗とする。

## Build

Harmony is required.

Default RimWorld path:

`D:\SteamLibrary\steamapps\common\RimWorld`

Run the combined build/static gate from the repository root:

`run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

For build only:

`build.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The build script:
- locates RimWorld 1.6 `Assembly-CSharp.dll`;
- locates `UnityEngine.CoreModule.dll`, `Unity.Mathematics.dll`, `Unity.Collections.dll`, and `netstandard.dll`;
- locates Harmony from Steam Workshop item 2009463077;
- compiles `Source/AncientMedievalJapanEnvironment/*.cs`;
- writes `Assemblies/AncientMedievalJapanEnvironment.dll`.

## Automated runtime-error policy

The AMJ project-wide test policy is that any automated test which launches RimWorld must capture an isolated runtime log and fail if the repository-owned mod emits an ERROR-level entry. A passing scenario count does not override a mod-origin runtime error.

`run-tests.bat` remains the fast build + static-validation gate and does **not** launch RimWorld.

Environment also has an automated RimWorld runtime gate:

`run-runtime-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The runtime gate:
- runs the normal build/static gate first;
- creates an isolated save-data profile under `TestResults/VegetationRuntime/SaveData` and does not modify the user's normal `ModsConfig.xml` or `Prefs.xml`;
- enables only Core, Harmony, RimLogging, Quickstarts, and Environment in that isolated profile;
- launches WarmTemperate, CoolTemperate, Subalpine, and Alpine fixed-biome Quickstarts sequentially from the command line;
- writes one Quickstarts JSON report and one runtime log per biome;
- exits RimWorld automatically after each verification run;
- fails if a Quickstart assertion fails, if live log capture is incomplete, if a run times out, if the isolated runtime log contains an Environment-origin ERROR, **or if Quickstarts reports any pre-launch ERROR entry**. Pre-launch errors are treated as fatal regardless of channel because they occur before reliable live attribution and the profile is intentionally isolated.

The current vegetation/weather assertions verify:
- the generated map uses the requested AMJ biome;
- the band-specific structural plant actually generates;
- Shii / Beech / Shirabiso / Haimatsu each exceed the configured secondary tree species in their target biome;
- Haimatsu occupies at most 5% of all map cells;
- full-size Pine + Birch together occupy at most 1% of Alpine cells;
- each AMJ biome loads exactly the accepted eight Vanilla weather entries with the locked Alpha commonality values;
- rainy thunderstorms remain more common than dry thunderstorms in every AMJ biome;
- Japanese beech keeps its loaded leafless graphic and inherited Vanilla fall-shader behavior;
- Sudajii, Shirabiso and Haimatsu remain non-leafless evergreen structural plants;
- Vanilla gentle/hard snow weather remains available with positive snow rates;
- a river-bearing Environment world tile retains the Vanilla River mutator and generates River-tagged local terrain;
- a coastal Environment world tile retains the Vanilla Coast mutator and generates Ocean-tagged local terrain;
- each AMJ biome excludes Raccoon/Elk/Ibex/Fox_Arctic/Lynx and retains its accepted positive Vanilla wildlife proxy set through loaded BiomeDef commonality lookup.

The runtime suite also writes `[AMJ Environment Vegetation]` count/share summaries so balance can be reviewed without manual log counting.

If CCTO is installed either as the local development mod (`RimWorld/Mods/CropColdToleranceOverhaul`) or Workshop item `3812412548`, the same `run-runtime-tests.bat` command automatically adds a second isolated profile containing CCTO + AMJE. A focused WarmTemperate Quickstart then verifies the loaded Def values for all four AMJE plants:
- Sudajii: 8 C / fixed death -8 C;
- Beech: 5 C / cold dormancy;
- Shirabiso: 0 C / fixed death -35 C;
- Haimatsu: 0 C / fixed death -35 C;
- exactly one CCTO extension on each target.

If CCTO is not installed, this optional compatibility sub-gate is reported as skipped; the standalone Environment runtime gate remains valid because CCTO is not a dependency.


Manual testing is still reserved for genuinely visual or experiential checks, especially final artwork appearance. Placeholder-era vegetation presence, dominance, alpine blockage safety, and runtime errors are automated and should not be rechecked manually by default.

## First runtime smoke

The static validator also confirms that the obsolete custom Terrain worker XML patch is absent and that terrain processing is wired through a Harmony postfix.

After a successful build, start RimWorld with:
- Harmony;
- Ancient & Medieval Japan: Environment;
- no other world-generation/biome overhaul for the first smoke.

During the current Alpha, Environment always writes compact world-generation diagnostics:

- `[AMJ Environment] Vanilla terrain baseline` — Vanilla land count and coastal-land share before Environment transforms;
- `[AMJ Environment] Terrain summary` — Environment land/coastal share, same-seed coast multiplier vs Vanilla, land-count delta, land annual-mean temperature range, land elevation/highland shares, ocean-floor minimum, and Hilliness percentages;
- `[AMJ Environment] Climate/biome summary` — land rainfall min/max/average and land biome counts/shares;
- `[AMJ Environment] River summary` — river-bearing tile counts, all-tile share, **land-tile share**, unique river edges, and RiverDef counts.

The following numeric/runtime checks are targets for Pickle automation; use RimTest Redux for isolated transform/selection logic where suitable. Until the runtime harness exists, these remain explicitly unautomated checks. Once automated, reserve manual checks for coastline appearance and gameplay usability rather than repeating numeric/log checks.

Create several worlds and check:

1. startup/world generation completes with no AMJ Environment error;
2. Large/Huge rivers do not naturally appear;
3. Creek/River tiles are visibly more common than Vanilla;
4. land annual mean temperatures stay within -8 C to 20 C;
5. highland temperatures fall with elevation;
6. Flat terrain is reduced while usable settlement sites remain common enough;
7. coastlines are visibly more indented rather than merely losing land area;
8. the world still contains coherent natural biomes after Environment adjustment.

## Climate/CCTO validation

New-game initialization emits four `[AMJ Environment] Climate calibration` lines for warm lowland, temperate lowland, cool lowland and highland representatives. The diagnostic runs from `Game.InitNewGame` after `gameStartAbsTick` has been initialized; it must not sample `TileTemperaturesComp` from `World.FinalizeInit`, because Quickstarts generates the world before that absolute game-start tick exists. Each line samples RimWorld's actual outdoor-temperature API once per in-game hour for a full 60-day year (1440 samples).

Reference thresholds:
- growth: 10 / 8 / 5 / 0 C;
- cold death: -1 / -4 / -8 C.

Each calibration line includes actual yearly min/max, hours/days below every reference threshold, and lethal-cold event counts/durations.

The first balance goal is qualitative:
- southern lowlands: long warm season; barley normally safe;
- central lowlands: rice/millets stop in winter; barley generally safer;
- northern/highlands: normal annual crops cannot safely overwinter outdoors.

Do not retune CCTO crop thresholds to compensate for Environment world-generation errors. Adjust Environment climate first.

Current calibrated runtime target:
- seasonal amplitude: ±8 C south to ±16 C north;
- Vanilla sun-cycle: unchanged at ±7 C;
- separate daily random variation: **~±3 C** (3/7 of Vanilla).

The previous ~±4 C pass allowed a representative 14 C central lowland to dip below Barley's -8 C death threshold for 2 hours, so the random component was reduced before changing any CCTO crop threshold.


## Biome/vegetation validation

Current Alpha adds four AMJ-owned vegetation-band biomes with temporary Vanilla-backed art/content:
- `AMJ_WarmTemperateForest`;
- `AMJ_CoolTemperateForest`;
- `AMJ_SubalpineForest`;
- `AMJ_AlpineZone`.

After a successful build, generate a fresh world and inspect:
- `[AMJ Environment] Japan vegetation-band preview`;
- `[AMJ Environment] Climate/biome summary`.

Expected first-pass behavior:
- CoolTemperate should remain the largest band;
- WarmTemperate should be common;
- Subalpine should be meaningful but smaller;
- Alpine should be rare;
- swampiness >=0.5 remains outside the AMJ forest workers so existing wetland biomes can still generate;
- the climate/worldgen baseline should not be retuned merely to force exact biome shares.

At this stage, Vanilla world textures, existing Vanilla plants, and a minimal Vanilla wildlife pool are placeholders. Do not begin final image work until biome placement and map generation are stable.


## Fixed-biome Quicktests

When the RimWorks Quickstarts mod (Workshop **3793646067**) is installed and active, `build.bat` also compiles a developer-only assembly:

`DevQuickstarts/Assemblies/AncientMedievalJapanEnvironment.Quicktests.dll`

`loadFolders.xml` loads that assembly only while `rimworks.quickstarts` is active, so Quickstarts is **not** a normal Environment dependency.

The Quicktest picker exposes five deterministic terrain-validation starts:

- `AMJWarmTemperateTerrainQuickstart`
- `AMJCoolTemperateTerrainQuickstart`
- `AMJSubalpineTerrainQuickstart`
- `AMJAlpineTerrainQuickstart`
- `AMJDarkForestTerrainQuickstart`

All five:
- use world seed `AMJ-Environment-Terrain-Alpha`;
- use 5% planet coverage;
- generate a normal **250x250** map;
- select the requested biome after world generation and before map generation;
- prefer Flat, then Small Hills, Large Hills, and Mountainous valid settlement tiles in that order;
- if the requested biome exists but has no normally settleable tile, fall back only for this developer Quicktest to an unoccupied tile of that biome, including Impassable as the last resort;
- if another biome wins every tile that the requested biome worker considers eligible (possible for rare Alpine under MO coexistence), select a deterministic worker-eligible proxy tile and replace only that world tile's `PrimaryBiome` for the developer Quicktest before map generation; Alpine has a final coldest-land proxy fallback so the terrain-threshold test remains runnable even when the fixed 5% world contains zero natural Alpine tiles;
- forced proxy use is logged as `forcedBiome=True` with `originalBiome` and `targetBiomeScore`; it validates map terrain generation for that BiomeDef, **not** natural biome frequency or normal settlement availability; normal Environment world generation and settlement rules are unchanged;
- log the selected tile as `[AMJ Environment Quicktest] Selected tile`;
- then use the normal map generator, so `[AMJ Environment] Map terrain summary` reports Thin Soil / Gravel / Soil / Rich Soil / Other shares plus the five most common terrain Defs inside `Other`.

Use these fixed-biome Quicktests for ENV-004 terrain-threshold comparison instead of Vanilla/random QuickTest. Keeping the same seed and hilliness preference removes most start-tile noise from cross-biome comparisons.

If Quickstarts is not installed, the normal Environment DLL still builds and the developer Quicktest assembly is skipped.
