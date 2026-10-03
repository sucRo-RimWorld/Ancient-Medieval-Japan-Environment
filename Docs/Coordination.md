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

**Next action:** rebuild and generate one representative world. Check the vegetation-band preview share. If the shape is sensible, implement the first AMJ-owned forest BiomeDefs using temporary Vanilla world textures and no final art.
