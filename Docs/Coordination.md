# AMJ Environment Coordination

This file is the authoritative coordination surface for **Ancient & Medieval Japan: Environment**.

## Working rule

1. Read `AGENTS.md`.
2. Read this file from `main`.
3. Check OPEN / IN PROGRESS items before starting new work.
4. Put durable decisions into `Docs/Design.md` and implementation files.
5. Use this file only for handoff, status, blockers, and cross-repository coordination.

## Source of truth

Primary design source:

`Docs/Design.md`

## Status vocabulary

- **OPEN** — needs work
- **IN PROGRESS** — being investigated or implemented
- **BLOCKED** — waiting on a prerequisite
- **DONE** — completed and reflected in the proper source of truth
- **ARCHIVED** — retained for history only

## Current coordination items

### ENV-001 — Initial Japan-style world generation

**Requested by:** Environment/design  
**Owner:** Environment/worldgen  
**Status:** DONE

Establish the first playable world-generation baseline for a Japan-like environment:
- bounded Japan-oriented temperature range;
- Japan-like elevation and hilliness distribution;
- increased small/medium river frequency;
- no naturally generated Large/Huge rivers;
- more coastline through more indented coasts, bays, peninsulas and islands rather than simply reducing land area.

The authoritative Alpha targets are in `Docs/Design.md`.

River XML and the first C# world-generation prototype are now implemented in source:
- Creek 30,000 / 0.70 and River 75,000 / 0.80;
- Large/Huge natural spawning disabled while their Defs remain available;
- Vanilla Terrain generation is now Harmony-postfixed: Vanilla runs normally first, then Environment applies Japan-oriented elevation/coastline, hilliness, annual-temperature and rainfall transforms and reselects the biome;
- seasonal amplitude uses the Environment 8–16 C curve on the root surface;
- separate daily random variation is reduced to 3/7 of Vanilla on the root surface after CCTO climate calibration;
- Alpha diagnostics automatically log terrain/coast/hilliness and river statistics;
- Harmony is the sole technical dependency;
- `run-tests.bat` builds the DLL and validates the installed RimWorld 1.6 source-Def assumptions.

**Local build finding:** first build attempt failed at `WorldGenStep_AMJEnvironmentTerrain.cs` because `PlanetLayer.GetTileCenter()` exposes RimWorld 1.6 API metadata using `Unity.Mathematics.float3/int3`, while the legacy build script referenced only Assembly-CSharp / UnityEngine.CoreModule / netstandard / Harmony. This is a build-reference issue, not a worldgen logic failure. `build.bat` and the SDK project now reference both `Unity.Mathematics.dll` and `Unity.Collections.dll` from RimWorld's Managed directory. Fix commits: `c122e9c065c1dce49d7c2bc8413eecbea805c5d8`, `3646a352a9b6a78802db4bad61cd2c7f627df73b`.

**Runtime smoke finding:** the first game launch failed while loading the patched `WorldGenerator.xml`: RimWorld could not resolve `AncientMedievalJapan.Environment.WorldGenStep_AMJEnvironmentTerrain` from the XML `Class` attribute. The custom-worker XML replacement has therefore been removed. Terrain processing now uses a Harmony postfix on Vanilla `WorldGenStep_Terrain.GenerateFresh`, preserving the same required ordering (Vanilla terrain first, Environment transform second, later world-gen steps afterward) without custom Def-time type resolution. Fix commits: `41aa8a30e0b2565e169c82383f755e1005b5b16c`, `68653067cae51bc0fb71cd75b7e73bb8b2339dd6`. Design/validator updates: `23088d177cd8d6b0787edbac799b5a4a62c4ea03`, `2df3ca4a99200520dd58354d95ee8bb78b898303`.

The previous build/static PASS predates this integration change and is no longer the current gate result.

**Next action:** pull the latest main and rerun `run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`. If it passes, relaunch RimWorld with Harmony + Environment, enable Dev Mode, and generate the first test world. Use the `[AMJ Environment] Terrain summary` and `[AMJ Environment] River summary` log lines to compare the generated result against the Alpha targets. Climate validation must then use actual temperature samples against the CCTO/AMJ 10/8/5/0 C growth thresholds and -1/-4/-8 C cold-death reference thresholds, not annual mean alone.

**First successful world-generation smoke:** PASS for runtime execution, but balance requires another iteration.

Observed summary:
- tiles: 119,904;
- land: 54,546;
- coastal land: 4,167 / **7.6%**;
- annual-mean range: **4.0..20.0 C**;
- transformed elevation range: **-300..2034 m**;
- Hilliness: Flat **27.1%**, Small **36.9%**, Large **22.0%**, Mountainous **10.2%**, Impassable **3.7%**;
- river tiles: 5,181 / **4.3%** of all tiles;
- river edges: 5,000;
- Creek: 2,582; River: 2,418;
- **no LargeRiver/HugeRiver generated**, matching the intended river-size rule.

Interpretation:
- river size filtering is working and the 4.3% river-tile share is suitable for continued testing;
- Small Hills are substantially over target while Mountainous terrain is substantially under target;
- maximum elevation 2034 m is too low to exercise the intended rare >3000 m highlands;
- because elevation is too low, the generated annual-mean minimum only reached 4 C and did not exercise the intended cold/highland range;
- coastline cannot yet be judged against the 1.5x target because the Vanilla same-seed baseline was not logged.

Second-pass implementation:
- Hilliness now uses a spatial ruggedness ranking aimed at the 25/20/25/25/5 Alpha distribution instead of incremental Vanilla-category promotion;
- Large/Mountainous/Impassable tiles receive progressively stronger elevation uplift, with rare high peaks capped at 3800 m;
- diagnostics now log the Vanilla same-seed coastal baseline before Environment modification and report land-only temperature/elevation plus >=1500/2500/3000 m shares.
Implementation commits: `c87fe52cc5451ae8ca3059fc90a4f0b971ec486a`, `a2256bd84cc4dd55a5ceb8944c7d09978f5f575f`.

**Next action:** pull/rebuild, generate another Dev Mode world, and capture all three AMJ Environment diagnostic lines. Compare coastline against the same-seed Vanilla baseline and verify the revised Hilliness/highland/temperature distribution before changing river values.

**Smoke-test environment note:** a later Player.log showed only the legacy two-line diagnostic format (`Terrain summary` + `River summary`) with no startup marker and no `Vanilla terrain baseline`. The observed values were land 63,643, coastal share 5.9%, annual mean -6.3..20.0 C, elevation -300..2875 m, Hilliness 21.5/31.9/29.5/13.0/4.1, and river share 4.6%. Because the log format itself predates the second-pass diagnostics, this run used an older built DLL and must not be used to evaluate the current second-pass terrain balance.

Alpha diagnostics were therefore made independent of Dev Mode and an assembly/Harmony startup marker was added. Commits: `a82f55cb78db53f3ea66acd34ff2e7dd8b529047`, `78ab298a8088f3623db3b333b2f3f5b53a093d3f`.

**Current local verification gate:** pull latest `main`, rebuild with `run-tests.bat`, fully restart RimWorld, then confirm `[AMJ Environment] Assembly loaded; Harmony patches applied.` plus the three current world-generation diagnostic lines before evaluating balance.

**Current second-pass smoke:** runtime and balance targets are aligned on the first valid current-DLL seed.

Observed:
- Vanilla: land 61,209; coastal land 3,084; coastal share **5.0%**;
- Environment: land 61,674; coastal land 4,655; coastal share **7.5%**;
- same-seed coastline multiplier: **1.50x**;
- land count delta: **+0.76%**, indicating the coastline target was achieved primarily by increased coastal complexity rather than by flooding land;
- land annual mean: **-8.0..20.0 C**, avg **11.8 C**;
- land elevation: **0..3366 m**;
- highlands: >=1500 m **7.0%**, >=2500 m **0.4%**, >=3000 m present but rounded to 0.0% in the old diagnostic precision;
- Hilliness: **25.0 / 20.0 / 25.0 / 25.0 / 5.0%**, exactly matching the current Alpha distribution target;
- river tiles: 6,511 / 119,904 = **5.4% of all tiles**; Creek 3,387, River 2,895; no Large/Huge.

Interpretation:
- coastline, Hilliness, annual-mean range, and rare highland generation all meet the current single-seed Alpha intent;
- river size filtering remains correct;
- this is not yet a final lock because the design requires several seeds and the previous river percentage used all world tiles rather than land tiles.

Diagnostics were expanded before multi-seed validation:
- same-seed coast multiplier and land-count delta are now printed directly;
- >=2500 m and >=3000 m highlands include counts and finer percentages;
- land rainfall and biome distribution are logged;
- river diagnostics now include river-bearing **land-tile share** in addition to all-tile share.
Commit: `81c7729be58ed1b8b2af5cd69a3eb598c74b2cdc`.

**Next action:** rebuild, then generate at least two additional seeds at the same planet coverage and compare the expanded Environment diagnostics. If the terrain/coast/climate distributions remain stable, lock the Alpha world-generation baseline and move to CCTO hourly/daily climate sampling.

**Multi-seed gate result:** PASS. Two additional current-DLL seeds produced coastline multipliers **1.43x** and **1.50x**, land annual means **-7.9..20.0 C** and **-8.0..20.0 C**, exact 25/20/25/25/5 Hilliness, maximum elevations **3800 m** and **3141 m**, rainfall **832..2750** / **832..2917**, and river-bearing land shares **8.8%** / **7.9%**. Across the three current seeds the coastline multiplier averages approximately **1.48x** and land annual-mean averages remain tightly grouped at **11.5–12.0 C**. No Large/Huge rivers were produced.

Exact biome percentages are not locked because the validation mod list includes non-Vanilla biome Defs (for example `DankPyon_DarkForest`), making the histogram mod-list dependent. The terrain/climate envelope itself is accepted.

The current terrain/coast/elevation/rainfall/river Alpha baseline is now locked in `Docs/Design.md`. Further tuning requires a gameplay or compatibility finding rather than single-seed numerical preference.

**Result / references:** initial design `9f5fd58c77b8e9ae5bad00851189d0127a122925`; CCTO calibration `e93da687fcd543f6d3ec94d5398fc604c0559749`; river patch `40d2b6b5615ea26ac6d91ee10f2433e3bd474829` + compatibility hardening `8aee69c23726a08f72b101ccd22a1e2065846364`; terrain prototype `8080b41f144fbacbf31411e11b7853860cd5703b`; climate hooks `16d870ee74a8e259de8233bfc84148ae48cfcf4d` / `b8b84723adf254b3f1bbed7a75cce5223a16222e`; build/static gate `aa87755d6099e89fda36de40acf358fd9bfebb68`, `a1da1cf981d22239f1833765835106804636814c`, `3d5fb2e52058f7d518b56e98620de3ee2af92fc4`; diagnostics `3403742e6c57d89c611cb94f338fff2ff15ef647`.


### ENV-002 — CCTO runtime climate calibration

**Requested by:** Environment/design  
**Owner:** Environment/climate  
**Status:** DONE

Validate the locked world-generation baseline against actual RimWorld 1.6 outdoor temperatures used by CCTO rather than annual mean alone.

Use representative southern warm lowland, central temperate lowland, northern cool lowland, and highland tiles. Sample one in-game year through the public `TileTemperaturesComp.OutdoorTemperatureAt(PlanetTile, absTick)` API so the measurement includes the Environment seasonal-amplitude hook, scaled daily random variation, and Vanilla sun-cycle component.

Collect time below growth thresholds **10 / 8 / 5 / 0 C** and cold-death thresholds **-1 / -4 / -8 C**, plus min/max actual temperature and isolated lethal-cold events.

Automatic climate-calibration diagnostics are implemented in `ClimateCalibrationDiagnostics.cs`. Fresh world finalization selects warm/temperate/cool lowland plus highland representatives and samples `TileTemperaturesComp.OutdoorTemperatureAt` hourly for one 60-day year, including threshold-hours and lethal-event duration statistics. Implementation commit: `9d2e70dde20dfa709fb7bfe5d6ff992bc24df884`.

**First CCTO hourly calibration:** the warm, cool-lowland and highland signals matched intent, but the representative 14 C temperate lowland reached **-8.4 C** and spent **2 h below Barley's -8 C death threshold**. CCTO's cold-death path runs from the plant long-tick check and kills immediately on actual ambient temperature strictly below the threshold, so the brief event is gameplay-significant rather than ignorable. The representative also spent 136 h below -1 C and 70 h below -4 C, which correctly preserves Rice and millet winter risk. Warm lowland had no time below -1 C; cool lowland and highland had 195 h / 338 h below -8 C respectively.

**Calibration change:** keep the seasonal-amplitude curve and Vanilla ±7 C sun-cycle unchanged, but reduce the separate daily random variation from ~±4 C to **~±3 C (3/7 Vanilla)**. This targets the isolated central-lowland Barley failure without weakening northern/highland winter lethality. Implementation commit: `2df9de31cabc3cae5aab52b66f4ae7624bba6529`; validator lock: `2187c482e5d4df676f19ff19a2e2fde4ecb29765`.

**Build/static gate:** PASS after the climate-calibration diagnostics were added. The earlier CS0016 failure was a local DLL file lock from RimWorld still having `AncientMedievalJapanEnvironment.dll` mapped; closing RimWorld and rerunning the gate succeeded. This was not a code compilation failure.

**Final ~±3 C re-run:** PASS. Warm lowland stayed above -1 C; temperate lowland stayed above -8 C while still spending substantial winter time below -1 C and -4 C; cool lowland and highland remained well below -8 C for long periods. The intended Rice/Awa/Hie/Barley regional separation is therefore preserved.

**Result:** ENV-002 complete. Further climate tuning is deferred unless gameplay testing finds a concrete mismatch.


### ENV-003 — Japan vegetation bands and biome structure

**Requested by:** Environment/design  
**Owner:** Environment/biomes  
**Status:** IN PROGRESS

Move from the accepted climate/worldgen baseline into Japan-oriented biome and wild-vegetation structure.

Decision:
- use four coarse natural vegetation bands: WarmTemperate >=15 C, CoolTemperate 8..15 C, Subalpine 0..8 C, Alpine <0 C;
- treat swampiness >=0.5 as a wetland candidate overlay;
- do not repurpose Vanilla TropicalRainforest/TemperateForest/BorealForest/Tundra Defs because their animals, diseases, weather and compatibility semantics would become misleading;
- create AMJ-owned BiomeDefs after validating band shares;
- ReGrowth 2 is reference only; do not copy restricted code/assets.

A preview diagnostic is implemented as `[AMJ Environment] Japan vegetation-band preview`. Commit: `4a2bf50600fbaec0a7cb5fafec02119e594c7fef`.

**Vegetation-band preview:** PASS. First representative world: WarmTemperate **27.5%**, CoolTemperate **51.4%**, Subalpine **19.5%**, Alpine **1.7%**, swampiness>=0.5 **0.7%**. This matches the intended shape; the locked worldgen climate does not need retuning.

**Implemented Alpha biome pass:**
- custom workers for the four accepted climate bands, excluding swampiness>=0.5;
- AMJ-owned BiomeDefs `AMJ_WarmTemperateForest`, `AMJ_CoolTemperateForest`, `AMJ_SubalpineForest`, `AMJ_AlpineZone`;
- temporary Vanilla world textures, existing Vanilla wild plants, and minimal Vanilla wildlife placeholders only;
- no final art yet.
Implementation commits: `c84eaf9549c21c644b3da21eed4d5558c7f8ce4c`, `3e973f5f5b5c4a984e35d1b44fd08de158fbfa5e`. Static validation commit: `14fa045d6d3e8aa794673a92389bde2469cc76ae`.

**Runtime biome distribution:** PASS for coexistence. With Medieval Overhaul active, the first measured world produced AMJ WarmTemperate **27.0%**, CoolTemperate **43.7%**, Subalpine **7.2%**, Alpine **0.3%**, MO Dark Forest **16.3%**, Temperate Swamp **2.0%**, plus small residual Vanilla shares. The lower AMJ Subalpine/Alpine shares are partly explained by specialized MO biome competition rather than a climate-band generation failure.

**Compatibility decision:** the four AMJ biomes are baseline vegetation bands, not exclusive replacements. **MO is a standard AMJ coexistence target**, not merely another optional biome mod: Environment stays technically loadable without MO, but normal AMJ play is expected to include MO. MO-owned specialized natural biomes such as Dark Forest should therefore remain available through normal BiomeWorker competition where their climate rules fit. A brief attempted score increase that would have forced AMJ ownership over MO Dark Forest was reverted. Coexistence implementation commit: `3e5dedf94743baaea7248f0073882cde0a7ddf9f`; validator restore: `7ff5ad1d637f8e5b9e4f6af790466359b1331065`.

**Natural-soil prerequisite:** complete. ENV-004 is locked for Alpha, so ENV-003 has moved into Japan-specific wild vegetation.

**Japan-specific structural vegetation implementation:** added a deliberately small four-PlantDef set using temporary Vanilla graphics:
- `AMJ_Tree_Shii` — warm-temperate evergreen broadleaf dominant;
- `AMJ_Tree_Beech` — cool-temperate deciduous broadleaf dominant;
- `AMJ_Tree_Shirabiso` — subalpine evergreen conifer dominant;
- `AMJ_Shrub_Haimatsu` — alpine dwarf-pine scrub dominant.

Existing Vanilla oak/maple/birch/pine/bamboo remain at lower commonality as secondary/placeholder components rather than being removed. No new food/medicine/fiber item types were added; the trees yield ordinary wood, while haimatsu is non-timber low scrub. Final graphics remain deferred.

Implementation commits:
- PlantDefs: `00619f365ffbd4e14d88f3956a5af19cbe1ed60f`;
- Japanese localization: `f71899c7afdf3bd89456f175f0a1e5f651076652`;
- biome commonality: `858009e67afd6607652559cdf698d72bf1875fc3`;
- static validator: `582478b9a6f13ab4fc71e546cdaf600bad6c997c`;
- design source of truth: `258fd93af61570d9ae0082ef494993969deabf3d`.

**Local static-gate finding:** the first vegetation validation run failed before XML semantics because Windows PowerShell 5.1 decoded the BOM-less Japanese localization file with the local ANSI code page. The repository file itself is valid UTF-8, but the validator's plain `Get-Content` produced mojibake and consumed bytes around closing tags, making the decoded text appear malformed. The validator now reads the Japanese localization explicitly with `-Encoding UTF8`. Fix commit: `f400a230f366f96d46387630aa844d4d9ae9e090`.

**Local build/static rerun:** PASS after the UTF-8 validator fix. The Japan-specific vegetation PlantDefs, biome wiring, Japanese localization, normal Environment DLL, developer Quicktest DLL, and static validation gate all completed successfully.

**Next action:** use the four fixed-biome Quicktests (WarmTemperate, CoolTemperate, Subalpine, Alpine) to verify runtime generation without Environment-origin errors and check the intended structural progression: Shii -> Beech -> Shirabiso -> Haimatsu. Pay particular attention to haimatsu movement blockage and to limited timber availability in Subalpine/Alpine. Final plant images remain deferred until this runtime gate passes.


### ENV-004 — Low-fertility natural terrain

**Requested by:** Environment/design  
**Owner:** Environment/terrain  
**Status:** DONE

Add meaningful naturally poor growable ground after the initial biome structure, without duplicating Vanilla Gravel or MO agricultural improvements.

Implemented Alpha pass:
- one new `AMJ_ThinSoil` TerrainDef at fertility **0.50**;
- reuse Vanilla `Gravel` at **0.70**, `Soil` at **1.00**, and `SoilRich` at **1.40** rather than adding a redundant second poor-soil tier;
- biome-specific `terrainsByFertility` thresholds make poor/stony ground progressively more common from warm/cool forest toward subalpine/alpine;
- targeted MO Dark Forest compatibility preserves the MO biome while applying the same natural-soil ladder;
- player-created MO `DankPyon_PlowedSoil` remains untouched;
- fresh-map diagnostics report terrain shares automatically.

Implementation: `8f0324b1476ce9f7ffd082f74114d8bd6bda19ee` (TerrainDef), `dc9dec1e5c5936261fc825b0dba05a818d1c878b` (AMJ biome distribution), `0869bd7d9c4f3fdabef3b811050ccf9be1a9ee07` (MO compatibility), `0e88f29467b3237bc160733eb799b164c710e654` (map diagnostics).

**Build finding:** the first ENV-004 local build failed because the legacy .NET Framework compiler used by `build.bat` does not support the C# `nameof` expression in `MapTerrainDiagnostics.cs`. This is a compiler-syntax compatibility issue, not a map-terrain logic failure. The Harmony attribute now uses the equivalent string literal `"GenerateMap"`, and the static validator was updated to match. Fix commits: `81852f75fc9b98c8215843a8760bc4eaa3fdbb68`, `6ef7af956751965479497b1a32c4e8d007b40ad5`.

**Fixed-biome test tooling:** the previous random QuickTest is unsuitable for cross-biome terrain comparison because its landing biome is random. Environment now ships five developer-only RimWorks Quickstarts (WarmTemperate, CoolTemperate, Subalpine, Alpine, MO Dark Forest) behind `IfModActive="rimworks.quickstarts"`. They share a fixed world seed, generate 250x250 maps, and prefer Flat settlement tiles before hillier fallbacks so terrain-threshold comparisons are less noisy. Source: `498cfced568ea9b55947baa273029339dab92b05`; conditional load folder: `4c58adcda39fa76ca4cad8c414b9961bf76127a5`; conditional build support: `f7aeff8d72282edf38bd5f4c2ba4fc850da1fc44`; optional load order: `3f5da75fdf2f97935d4248a3a3dff04f96ae6950`; validator: `a65965ee9ff2d9bdc68535695fae3f8764140aa0`.

**Load-diagnosis note:** the developer Quicktest DLL can build successfully yet still fail to appear in the Quickstarts picker if RimWorld never loads the conditional `DevQuickstarts` folder. The Quicktest assembly now emits `[AMJ Environment Quicktest] Developer quicktest assembly loaded.` from a static constructor so runtime loading can be verified directly. Diagnostic commit: `7316c9a7e8ee3b4a3c209e5da8ce8e467763dbb8`.

**Runtime diagnosis after missing Quicktest load marker:** the conditional XML syntax matches RimWorld 1.6 source, so do not change it blindly. First runtime result was `quickstartsActive=False` and Environment loaded only its root folder, even though the user is using Workshop item 3793646067. This points to either a package-id mismatch between the installed Workshop build and the expected `rimworks.quickstarts`, or the Workshop item not being present in RimWorld's active-mod list. The startup diagnostic now also enumerates any active mod whose display name or package ID contains `Quickstart`, with its player-facing package ID. Implementation: `c40d9c72f93a41b67efde0b25904ca412afe294e`.

**Quickstarts diagnosis resolved:** Workshop item 3793646067 had not actually been enabled; the visible button was RimWorld's Vanilla Dev QuickTest button. Once Quickstarts is active, the fixed-biome test assembly can be used as designed.

**Runtime terrain samples:** WarmTemperate reported Thin/Gravel/Soil/Rich/Other = 11.3/35.5/27.4/3.2/22.5%. CoolTemperate reported 16.3/37.8/29.8/4.1/12.0%. Subalpine reported 16.9/16.5/4.8/0.8/61.0%. Among only the four fertility-ladder terrains, Subalpine is approximately 43.3% Thin, 42.3% Gravel, 12.4% Soil, 2.0% Rich, so the intended strong fertility degradation is present; the high Other share still needs interpretation before threshold tuning.

**Def error found during fixed-biome runs:** `AMJ_ThinSoil makes terrain filth and also accepts it.` Cause: AMJ_ThinSoil declared `generatedFilth=Filth_Dirt` without inheriting Vanilla `NaturalTerrainBase`, leaving `filthAcceptanceMask` at TerrainDef's default `Any`. Fix: inherit `NaturalTerrainBase` and align with Vanilla soil semantics using `categoryType=Soil` and the `Soil` tag. Implementation: `7646617cec9272d1589c308fdd65abda4c3e7c07`; validator: `f626fbc21dee496be824a36bf3005a0a438307bc`.

**Fixed-biome runtime follow-up:** Dark Forest reported Thin/Gravel/Soil/Rich/Other = 8.9/14.0/6.4/18.1/52.6%. Its high Rich and Other shares differ sharply from the AMJ forest biomes and should be interpreted with MO's own terrain patch makers before changing Environment thresholds.

The first Alpine Quicktest attempt exposed two developer-tooling errors rather than terrain-balance failures:
- climate calibration still ran from `World.FinalizeInit`, where RimWorks Quickstarts has generated the world but `gameStartAbsTick` is not yet initialized; `TileTemperaturesComp` therefore logged the TicksAbs error;
- the Alpine selector required a normally valid settlement tile, but the generated rare Alpine band had none.

Fixes:
- climate calibration now runs from `Game.InitNewGame` after the absolute start tick exists: `535338a7186c937d0c465f8b40b1719af9c2e827`;
- fixed-biome Quicktests retain normal settlement preference but may use an unoccupied non-settleable target-biome tile, including Impassable only as the final developer-only fallback: `09eb629ab0bdd84bb72856dcf6d7c17e00223f83`;
- map terrain diagnostics now log the five largest Defs making up `Other`: `d14bfb197bad14a8ae4350a58b02e16521e9fa0f`.

**Second Alpine retry finding:** the fixed 5% seed contained **zero natural `AMJ_AlpineZone` tiles**, so even the non-settlement fallback could not select one. This is compatible with normal biome competition: a target worker can be eligible while another biome (notably MO Dark Forest in overlapping cold/wet climates) wins the final `PrimaryBiome`.

The fixed-biome Quicktest now distinguishes natural-world placement from terrain-threshold testing. It first prefers an exact normal settlement tile, then any exact unoccupied target-biome tile. If the target biome is absent, it selects a tile on which the target biome worker scores positively and temporarily changes only that tile's `PrimaryBiome` for the developer test. Alpine additionally falls back to the coldest suitable land tile if the 5% world has no worker-positive Alpine proxy. The selected tile logs `forcedBiome`, `originalBiome`, and `targetBiomeScore`. This does not modify normal worldgen or settlement behavior. Implementation commits: `819445d5fb2e26e52f932017ec4153bf30c47a7e`, syntax follow-up `0d4e6e4b9f591d2338ace576d3376ae8442fcdd2`.

The separate startup error `No textures found at path Things/Item/Resource/PlantFoodRaw/RawLentils` is not emitted by Environment and is independent of the Alpine selector failure; do not treat it as an Environment terrain-balance result.

**Local build finding after synthetic-biome fallback:** the normal Environment DLL built, but the developer Quicktest DLL failed with CS0266 because `Find.WorldGrid[PlanetTile]` exposes the base `Tile` type while the fallback assigned it directly to `SurfaceTile`. All Quicktest world-grid reads that require `SurfaceTile` now use an explicit `as SurfaceTile` cast with null handling. Fix commit: `4dda399fa08ee5ceb28d826bc048f16bebd018dd`.

**Local build/static rerun:** PASS after `4dda399fa08ee5ceb28d826bc048f16bebd018dd`; both the normal Environment DLL and the developer fixed-biome Quicktest DLL compiled, and the static validation gate completed successfully.

**Alpine fixed-biome terrain result:** map generation succeeded. Terrain share was ThinSoil **61.6%**, Gravel **24.4%**, Soil **2.6%**, RichSoil **0.0%**, Other **11.4%**. Within only the four fertility-ladder terrains, this is approximately Thin **69.6%**, Gravel **27.5%**, Soil **3.0%**, Rich **0%**, so the Alpine threshold design is producing the intended severe fertility degradation. `OtherTop` was dominated by rough rock/natural wall terrain: `DankPyon_NaturalWall_Clay_Rough` 8.5% of all cells and `Slate_Rough` 2.0%; together they account for about 92% of Alpine's Other category. The remaining listed Other entries were individually <=0.2%. No fertility-threshold change is indicated by this Alpine sample.

**Dark Forest expanded terrain result:** ThinSoil **8.9%**, Gravel **14.0%**, Soil **6.4%**, RichSoil **18.1%**, Other **52.6%**. `OtherTop` is `MossyTerrain` **23.5%**, `Marble_Rough` **19.8%**, `Sandstone_Rough` **6.8%**, `Mud` **1.2%**, and `WaterShallow` **0.8%**. The supplied MO 1.6 Dark Forest Def confirms that MO itself has a terrain patch maker with MossyTerrain at 0.00-0.32, SoilRich at 0.32-0.80, Mud at 0.80-0.93, shallow water at 0.93-1.06, and deep water above that. Environment's compatibility patch replaces only `terrainsByFertility` and intentionally leaves this MO patch maker untouched. Therefore the large RichSoil/MossyTerrain shares are expected MO biome identity, not a failure of Environment's soil thresholds. No Dark Forest threshold change is indicated by this result.

**Final Subalpine expanded result:** ThinSoil **16.9%**, Gravel **16.5%**, Soil **4.8%**, RichSoil **0.8%**, Other **61.0%**. `OtherTop` is `Limestone_Rough` **34.2%**, `Slate_Rough` **26.1%**, `PackedDirt` **0.4%**, `DankPyon_Floor_Versailles_Sandstone` **0.2%**, and `AncientTile` **0.1%**. Limestone + Slate alone account for **60.3% of all cells** and about **98.9% of the Other category**, so the large Other share is overwhelmingly natural rough rock rather than a failure of the fertility ladder.

**Result:** ENV-004 accepted and locked for Alpha with no threshold retuning. Warm/Cool retain substantial ordinary soil; Subalpine shifts strongly toward Thin+Gravel among growable fertility terrains; Alpine is overwhelmingly poor/stony; and MO Dark Forest retains its own special terrain-patch identity. The accepted values and runtime evidence are now recorded in `Docs/Design.md`. Future changes require a concrete gameplay or compatibility finding.

### TEST-001 — AMJ-wide runtime ERROR gate policy

**Requested by:** project-wide automated-test policy  
**Owner:** testing/tooling  
**Status:** DONE (policy) / runtime harness not yet present

AMJ automated tests that launch RimWorld must capture an isolated runtime log and fail on ERROR-level entries attributed to the repository-owned mod, even when scenario counts otherwise pass.

Environment's current `run-tests.bat` is a build + static-validation gate only and does not launch RimWorld, so it does not fabricate a runtime-log result. The requirement is now fixed in `AGENTS.md` and `Docs/DevelopmentTools.md`: when an automated RimWorld runtime harness is added to Environment, the mod-origin ERROR gate is mandatory from the first version.

Policy commits: `2a86387ebf1bfe5d3de3fbf09de93800cace0e74`, `7148ff5df9cdc2078e55c4233bf4b60e1e112c70`.


### DOC-001 — Shared public-description format and save compatibility

**Requested by:** author / public-description policy (2026-10-04 JST)  
**Owner:** Documentation/release  
**Status:** DONE (repository documentation)

All AMJ-related mod descriptions must include save compatibility. CCTO is the evolving format baseline. Durable shared policy: [Docs/ModDescriptionGuidelines.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/ModDescriptionGuidelines.md). Addition/removal safety must reflect each mod's actual implementation; custom content and world-generation mods do not inherit CCTO's safe-removal claim.

About.xml now states the development build's save-compatibility limits; AGENTS.md points to the shared policy for future README/Workshop preparation.

**Next action:** Use the shared CCTO-based format when preparing the public description; verify save addition/removal before making stronger claims.


### TEST-POLICY-002 — RimTest Redux / Pickle automation-first policy

**Requested by:** author (2026-10-04 JST)  
**Owner:** Testing/tooling  
**Status:** DONE (policy documentation)

The shared project policy now prioritizes RimTest Redux / Pickle automated testing and minimizes human manual tests. Durable instructions are in `AGENTS.md` and `Docs/DevelopmentTools.md`. Reproducible logic, loaded Defs, runtime behavior and release regressions should be automated; manual testing is reserved for appearance, readability and play/interaction feel. Build/static checks remain complementary, and runtime suites retain the mandatory mod-origin ERROR gate.

This documentation update does not claim new runtime coverage or a new test PASS. Existing implementation/test history remains unchanged. Environment's runtime harness is still absent; runtime/numeric checks listed in DevelopmentTools are explicitly identified as automation targets.

**Next action:** apply this policy to subsequent feature, fix and release work; record unautomated coverage explicitly and move reproducible checks into the automated gate.

**Result / references:** AGENTS policy commit `32d067ab2057ba032ccb21bdc371f3b2a6770d74`; development workflow commit `8a6106bc4aab214d3da77f75207c76d6e5bdb816`.
