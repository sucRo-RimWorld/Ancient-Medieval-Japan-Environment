# Environment Development Tools

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

Environment's current `run-tests.bat` is still a build + static-validation gate and does **not** launch RimWorld, so it cannot truthfully report a runtime-log result. The manual runtime smoke and fixed-biome Quickstarts remain separate for now. When Environment gains an automated RimWorld runtime harness, the mod-origin ERROR gate is mandatory from the first version of that harness.

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
- if a requested biome has no normally settleable tile (notably rare Alpine worlds), fall back only for this developer Quicktest to an unoccupied tile of that biome, including Impassable as the last resort; this does not change normal settlement rules;
- log the selected tile as `[AMJ Environment Quicktest] Selected tile`;
- then use the normal map generator, so `[AMJ Environment] Map terrain summary` reports Thin Soil / Gravel / Soil / Rich Soil / Other shares plus the five most common terrain Defs inside `Other`.

Use these fixed-biome Quicktests for ENV-004 terrain-threshold comparison instead of Vanilla/random QuickTest. Keeping the same seed and hilliness preference removes most start-tile noise from cross-biome comparisons.

If Quickstarts is not installed, the normal Environment DLL still builds and the developer Quicktest assembly is skipped.
