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

## Release handoff — first public Beta

- **Current priority:** publish the first public AMJ Environment **Beta** before starting Vanilla / Medieval Overhaul tree retextures.
- **Runtime gate:** Environment runtime testing has already been completed; do not treat the older pending-rerun notes below as a release blocker unless a later code/config change invalidates that result.
- **Workshop presentation:** cover / preview artwork is already complete in a separate workstream but is not stored in this repository yet.
- **Public copy:** README, About.xml, and Japanese/English Workshop source now use Beta release wording. The internal Design document's "Alpha" baselines remain historical/design-stage terminology and are not public release-stage labels.
- **Local publication staging:** the current publication workstream uses `D:\\SteamLibrary\\steamapps\\common\\RimWorld\\Mods\\_AMJ_PublishStaging`; local staging state may be newer than GitHub main and must not be reconstructed from GitHub alone.
- **Post-publication:** resume Environment-owned Vanilla / MO tree retextures only after the first public Beta is published.

## Active tree-art handoff

- **Current stage:** Haimatsu source approved and installed; Alpine in-game review pending.
- **Next target:** finish Haimatsu focused Alpine review, then prepare the next Vanilla tree proposal.
- **Approval state:** Haimatsu design approved with `y`, generated candidate approved with `OK` on 2026-10-05 JST.
- **Mandatory restart entry:** `Docs/GoldenPaths/RetextureGeneration.md`; canonical visual rules in `Docs/ArtDirection.md`; production handling in `Docs/GoldenPaths/TextureAssetPipeline.md`.
- **Reference state:** accepted Sudajii / leafy beech / leafless beech remain unchanged. Shirabiso production path: `Textures/Things/Plant/AMJ/Shirabiso/Shirabiso_A.png`.

## Current coordination items

### DOC-WORKSHOP-002 — Biome rationale and representative-tree section

**Requested by:** author (2026-10-06 JST)  
**Owner:** Documentation/release  
**Status:** IN PROGRESS — README detailed source updated; Japanese Workshop summary drafted; English translation intentionally deferred pending Japanese approval

README now contains the detailed public explanation of why AMJE uses four broad vegetation/climate bands and why each band receives one AMJE-owned structural representative: Warm-temperate / Sudajii, Cool-temperate / Japanese beech, Subalpine / Shirabiso, Alpine / Haimatsu. It also includes the four production plant images and longer ecological/historical summaries.

The Japanese Workshop source is the concise summary of that README content. It explains the same biome/plant rationale, embeds the four production PNGs via raw GitHub URLs, and keeps the plant notes shorter than README/in-game descriptions. Workshop must not become the only location for substantive public information.

Do not translate the new Workshop section into English until the author approves the Japanese wording, per the project-wide Japanese-first publication rule.


### LOC-ENV-001 — Kanji / alias opening pass for current AMJE descriptions

**Requested by:** author (2026-10-06 JST)  
**Owner:** Environment localization  
**Status:** IN PROGRESS — expanded Japanese draft prepared; author approval pending; English intentionally deferred

The four structural-plant Japanese descriptions have now been expanded using the AMJ educational structure: name/aliases → Japanese distribution/ecological context → supported ancient/medieval role or landscape context → modern difference/use where supportable. The current draft also uses literal `\n\n` paragraph breaks between those logical sections for RimWorld readability. Durable research/source rationale is recorded in `Docs/ArtDirection.md`.

The draft deliberately distinguishes direct evidence from landscape context. Sudajii uses Early-Jomon Castanopsis fruit-use evidence; Japanese beech uses Jomon vegetation evidence plus late-Heian/medieval Fagus-genus turned-wood evidence; Shirabiso and Haimatsu do not fabricate specific medieval resource uses, and instead connect their verified Mt. Ontake vegetation roles with independently attested later-medieval mountain worship.

The English Def descriptions remain deliberately unsynchronized until the author approves these expanded Japanese descriptions. Do not translate an intermediate Japanese draft.

Shared durable policy: Core `Docs/HistoricalDescriptionGuidelines.md` commit `9859b6d4a9d6a5a7f4829ae067c5c79318ab57ee`.

### ENV-ART-RULES-011 — shared AMJ art-rule consolidation

**Requested by:** author (2026-10-06 JST; AMJ-wide rule cleanup)  
**Owner:** Environment art / documentation  
**Status:** DONE — follow-up audit and art-rule CI green

Environment art documentation now inherits the Core `Docs/ArtStyle.md` project-wide invariants instead of carrying a second full shared prompt/rule set.

- `AGENTS.md` is a routing layer only for art work.
- `Docs/ArtDirection.md` owns Environment-specific vegetation/terrain/world rules and accepted species baselines. Its restrained soft-gradient allowance is explicitly recorded as a tree/plant class difference from the flatter Core crop/item budget.
- `Docs/GoldenPaths/RetextureGeneration.md` owns preflight/reference loading only; the duplicated frozen generation prompt was removed.
- `Docs/GoldenPaths/TextureAssetPipeline.md` owns installation/runtime validation and only points to the shared fixed-template policy when actually needed.

Source-of-truth commits:
- AGENTS routing: `82497e955bf5aeaa26d6c656724e912ec4045242`;
- generation entry cleanup: `d9450de315c66ac5ace71b032bf5106d82bee08f`;
- ArtDirection inheritance/exception clarification: `c8ef22a14769b3e8f708ae0da908d39040fca469`;
- texture pipeline deduplication: `69d8833629c88d79a91e41e517d7e04c94e31969`.

**Follow-up audit (2026-10-06 JST):** Retexture generation now assembles the current Core+Environment rules instead of using a frozen prompt, does not insert a mandatory pre-generation approval loop, and reviews candidates against the current request plus any already-approved target design. The Golden Path index was synchronized, accepted Sudajii/beech reference blobs are regression-locked, and stale completed visual-review statuses were reconciled. `.github/workflows/art-rule-structure.yml` runs the dedicated documentation/routing guard; run `37442081809` passed.


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
**Status:** DONE

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

**Runtime validation automation:** the four manual vegetation Quicktests have been replaced by a one-command isolated runtime gate. Each fixed-biome Quickstart now implements `Verify()` assertions for biome identity, target structural-plant generation, target dominance over secondary trees, and Alpine safety limits (Haimatsu <=5% of cells; full-size Pine+Birch <=1%). The suite prepares an isolated Core+Harmony+RimLogging+Quickstarts+Environment profile, launches the four biomes sequentially, writes JSON/log output, exits automatically, and fails on Environment-origin ERROR entries.

Implementation:
- Quickstart vegetation assertions: `afd57825808691b48b6ea5d1ac0247954103573d`;
- isolated runtime profile: `65ac0924de6f2edb664b9fc129adffe587a2fa35`;
- mod-origin runtime ERROR gate: `7c251c6301d9373cffb989d2e12abf74e4a3113e`;
- four-run PowerShell runner: `9b59e34892919edfa8711fcddac9e64c21c1a197`;
- one-command batch entry point: `8621ca8e5bf0bc4d828aa98b50409b019fdb07e2`.

**First automated runtime run:** FAIL, and the gate correctly exposed two independent issues before vegetation assertions could run.

1. With the isolated profile intentionally excluding Medieval Overhaul, `Patches/Compatibility/MedievalOverhaul.xml` still attempted its Dark Forest `PatchOperationReplace`. The top-level operation-level `MayRequire` did not skip the patch in this structure. The compatibility patch is now wrapped in `PatchOperationSequence` with `MayRequire="DankPyon.Medieval.Overhaul"` on the child replace operation, so Environment can load cleanly without MO. Fix: `67ae970dd8bb2dc2dd9466594816470d8ed59ad5`.

2. The fixed 5% world had no usable WarmTemperate tile and no worker-positive proxy, so the test stopped with `No usable world tile found for biome test AMJ_WarmTemperateForest`. Fixed-biome developer tests now have a final generic synthetic-climate fallback: select the nearest unoccupied land tile, set only that test tile to the target band's representative annual temperature (17.5 / 11.5 / 4 / -4 C), ensure rainfall >=800 and swampiness <0.5, then assign the requested biome. This is test-only and does not alter normal world generation. Fix: `195cc8dd5ba12ad6f42e468dfd43c8923400cb19`.

Static validator synchronized with both fixes.

**Runtime report audit correction:** the later exported Quickstarts reports showed that the nominal four-biome PASS was a false positive at the harness level. Every standalone report had `passed=true`, `failed=0`, and all vegetation assertions passed, but also `preLaunchErrors=2`. The corresponding startup log errors were AMJE-owned XML inheritance failures:

- Shirabiso: inherited `visualSizeRange` text `1.5~2.0` was merged with child `<min>1.25</min><max>2.7</max>`;
- Haimatsu: inherited `visualSizeRange` text `0.7~1.1` was merged with child `<min>0.45</min><max>0.75</max>`.

The structural distribution measurements themselves were otherwise healthy: Warm Shii 3.21%, Cool Beech 3.21%, Subalpine Shirabiso 1.29%, Alpine Haimatsu 0.69%, with Alpine full-size Pine+Birch only 0.02%. These values do not indicate balance retuning.

Fixes:
- both custom FloatRange overrides now use `Inherit="False"`: `fe6f641d671af41979b36dfc8649d17fb37f4b5e`;
- the runner now fails when `preLaunchErrors > 0`: `aa6b7098d825f0e7de67803d1f4053ad909d7cbe`;
- static regression checks cover both fixes: `5a8d195529318b7802c95a02423246c40cb06c57`.

**Final clean rerun:** PASS. All four standalone reports returned `passed=true`, `failed=0`, `preLaunchErrors=0`, `captureLive=true`, and `logTruncated=false`; no report log contained a structured `Level: ERROR` entry. Current target shares were Shii **3.14%**, Beech **3.22%**, Shirabiso **1.29%**, and Haimatsu **0.68%**. Alpine full-size Pine+Birch was **0.03%**, well below the 1% safety limit.

**Result:** ENV-003 is complete for the Alpha structural vegetation stage. The composition/commonality values remain accepted without retuning. Final art remains deferred to the visual-art pass.


### ENV-005 — Optional CCTO integration for AMJE plants

**Requested by:** Environment/design  
**Owner:** Environment/compatibility  
**Status:** DONE

Ownership decision:
- AMJE standalone is intentionally a complete "medieval Japan-like environment" experience without CCTO;
- CCTO is an optional realism layer: combining it with AMJE adds stricter species-specific cold-growth/death/dormancy behavior rather than unlocking basic Environment functionality;
- AMJE owns cold-tolerance compatibility for AMJE-owned plants;
- CCTO remains unaware of AMJE and receives no AMJE-specific balance/code;
- AMJE remains fully usable without CCTO;
- AMJE conditionally uses CCTO's public XML-facing `ColdToleranceExtension` only when CCTO is active.

Two-layer temperature model:
- standalone AMJE explicitly keeps the Vanilla-style `minGrowthTemperature=0 C` baseline on Sudajii, Beech, Shirabiso, and Haimatsu;
- with CCTO active, AMJE's own optional compatibility patch changes the four targets to:
  - Sudajii: min growth 8 C, fixed death -8 C;
  - Beech: min growth 5 C, cold dormancy;
  - Shirabiso: min growth 0 C, fixed death -35 C;
  - Haimatsu: min growth 0 C, fixed death -35 C.

Implementation:
- explicit standalone baseline: `89cf48d65786547b6a7f3fb8dd2027313ef07adf`;
- AMJE-owned optional CCTO patch: `ac99339fc70f290b39ca06b6702a0b4a2e9d1faf`;
- optional load order, not dependency: `f1967a832005bef342aa87999b35bf3fe9dbddbd`;
- runtime loaded-Def assertions: `13772aec27dbccef01889ae59dfa6a66a8b462b1`;
- isolated optional-CCTO profile support: `d69428bb7bfe2ae9952d9b4920fb265abdb030e4`;
- focused compatibility runtime mode: `a7bbae4f6c3bad52fa6c1e63b9ff0ff6b540bed6`;
- one-command optional CCTO sub-gate: `81ba86a3cf7917fb663084ae2f151eae60d7a61e`;
- standalone loaded-Def isolation assertions: `574077b196d26c50922ae9fc56cac641e2400eb8`;
- optional-sub-gate exit-code handling: `43ecf5f52666bb3641fc1ddc581b50450199742e`;
- static validator: `719a91373d4b38a105c3ec7582ca588301b92ccd`;
- design source of truth: `cbf246511026b45ee687c402b8a7d75510623522`;
- product/balance positioning (AMJE standalone complete, CCTO optional realism layer): `5a104a8f09ac084fbd61aebedbfe82b02a5ba57e`;
- development tooling docs: `8ea3386080de63218e23021d2fa4f20cbc65dda3`.

**Verification status:** exported standalone and AMJE+CCTO reports confirm that all loaded-Def CCTO assertions themselves passed, including the 0/5/8 C growth values, dormancy/death behavior, and exactly one CCTO extension per AMJE target. However, the same two AMJE `visualSizeRange` startup XML errors appeared in the CCTO report as `preLaunchErrors=2`. Therefore ENV-005 is **not** accepted yet; the compatibility assertions are good, but the corrected package must rerun with `preLaunchErrors=0`.

The report-audit fixes are shared with ENV-003: `fe6f641d671af41979b36dfc8649d17fb37f4b5e`, `aa6b7098d825f0e7de67803d1f4053ad909d7cbe`, `5a8d195529318b7802c95a02423246c40cb06c57`.

**First post-change local build finding:** the normal Environment DLL built, but the developer Quicktest DLL failed because RimWorld 1.6 exposes `GetActiveModWithIdentifier` on `Verse.ModLister`, not `Verse.LoadedModManager`. The CCTO-active probe now uses `ModLister.GetActiveModWithIdentifier(..., true)`. Fix: `6e2ad338228fc380342e444e81a79eb45bd385a6`; validator lock: `57233f5ef00aef3af82f00503a39c36ba30ae170`.

**Final clean AMJE+CCTO rerun:** PASS. The focused compatibility report returned **29/29 assertions passed**, `preLaunchErrors=0`, complete live log capture, and no structured `Level: ERROR` entry. Loaded Def checks confirmed the intended two-layer values for all four AMJE plants and exactly one CCTO extension per target.

**Result:** ENV-005 is complete. AMJE remains fully functional standalone with its Vanilla-style temperature baseline, while CCTO remains an optional higher-realism layer owned through AMJE's conditional compatibility patch.


### ENV-006 — Regional weather baseline

**Requested by:** Environment/design  
**Owner:** Environment/weather  
**Status:** DONE

Alpha policy:
- reuse the eight Vanilla WeatherDefs rather than introducing AMJ-specific weather types;
- let Vanilla rainfall-factor curves and temperature ranges provide the first layer of regional/seasonal differentiation;
- keep Warm/Cool liquid precipitation and fog stronger than Subalpine/Alpine;
- progressively increase snow commonality toward Subalpine/Alpine;
- make dry thunderstorms rare in the humid-Japan baseline while retaining rainy thunderstorms as the more common severe-storm proxy;
- do not add calendar-specific Baiu/Akisame/typhoon weighting yet;
- do not fake Sea-of-Japan-side vs Pacific-side winter exposure from vegetation-band names alone.

Implementation:
- dry-thunderstorm commonality reduced to 0.1 in Warm/Cool/Subalpine and 0.05 in Alpine: `d695f3cc74ec50effd75c65ae8955c3d6976a603`;
- fixed-biome Quickstarts now assert all eight loaded weather commonalities and rainy > dry thunderstorm: `098e9b55f968b0eb17f4bb98a9cf17d153ec5895`;
- static validator locks the accepted table: `dbf80ab37c56da734646aba770e9d43e48096dca`;
- design source of truth and JMA research basis: `4561fdb8cee7ead6c9cf042adea78bd4f26244fa`.

**Final automated runtime result:** PASS. WarmTemperate, CoolTemperate, Subalpine, and Alpine all returned `passed=true`, `failed=0`, `preLaunchErrors=0`, and `logErrors=0`. Every loaded weather commonality matched the accepted Alpha table and rainy thunderstorms remained more common than dry thunderstorms. The optional AMJE+CCTO run also passed **39/39** assertions with `preLaunchErrors=0` and `logErrors=0`, confirming that the optional realism layer does not alter Environment weather.

The remaining startup WARN messages are generic RimWorld metadata warnings about dependencies lacking `downloadUrl` / `steamWorkshopUrl`; they are not Environment runtime errors and are outside this weather gate.

**Result:** ENV-006 complete. Calendar-specific Baiu/Akisame/typhoon weighting and Sea-of-Japan/Pacific-side winter exposure remain deferred unless a later gameplay finding justifies the added system complexity.


### ENV-007 — Seasonal scenery baseline

**Requested by:** Environment/design  
**Owner:** Environment/seasonal scenery  
**Status:** DONE

Alpha decision:
- do not add a custom seasonal-scene controller;
- reuse Vanilla snow accumulation/rendering from SnowGentle/SnowHard;
- use Japanese beech as the deciduous structural representative through `DeciduousTreeBase`, inherited fall shader behavior, and its leafless graphic;
- keep Sudajii, Shirabiso and Haimatsu evergreen;
- defer final AMJE-specific plant/environment artwork until the final-art pass.

Implementation/test coverage:
- loaded seasonal-scene runtime assertions: `fcca9ce0c3b33fa61ab2b89dce4d9ece685fc784`;
- static validation of deciduous/evergreen profile and test markers: `213aa5a9404e4bb327a4493ed8f8bfc9ddfac525`;
- design source of truth: `b3aa742f9b8c4a4c7479546def9fbc5db81b26d1`.

**Final automated runtime result:** PASS. WarmTemperate and CoolTemperate passed **31/31** assertions, Subalpine and Alpine passed **29/29**, and the focused AMJE+CCTO run passed **43/43**. Every report returned `preLaunchErrors=0`, `logErrors=0`, `captureLive=true`, and `logTruncated=false`. The new seasonal checks confirmed the loaded beech leafless graphic, inherited Vanilla fall-shader behavior, evergreen status of Sudajii/Shirabiso/Haimatsu, and positive SnowGentle/SnowHard snow rates.

One existing Environment warning may appear in the small fixed-world Quickstarts when climate calibration cannot find a Highland representative tile. It is a non-fatal diagnostic from the calibration sampler, not a seasonal-scene failure and not an ERROR-level event.

**Result:** ENV-007 complete. No custom seasonal controller is needed for Alpha. Final texture/visual quality remains deferred to the final-art pass.


### ENV-008 — River/coast world-to-map handoff

**Requested by:** Environment/design  
**Owner:** Environment/world-to-map integration  
**Status:** DONE

Decision:
- AMJE continues to own river/coast distribution at world level only;
- do not add a custom local-map river/coast generator;
- rely on RimWorld 1.6 `WorldGenStep_Mutators` and Vanilla `River` / `Coast` tile mutators;
- validate that Environment-generated river/coastal world tiles still produce actual local River/Ocean terrain;
- keep waterfalls deferred.

Implementation/test coverage:
- dedicated River and Coast handoff Quickstarts: `b6189c8c5b0e172020dd8cdc225e6415d755f180`;
- standalone runtime runner now includes both handoff tests: `71fd964f8b194ce3ee3feee94fdc1311998f7636`;
- static validator covers both tests/runner entries: `6680f276ab1cf5dd16026a4e775fde0ad3278d32`;
- design source of truth: `2816449f2c90d27b8f117d47e2e991b1d13ba232`.

**Final local runtime result:** PASS, reported by the author after running the combined Environment runtime gate with the new River and Coast handoff Quickstarts enabled. The run therefore satisfies the current gate as implemented, including the River/Ocean terrain assertions and the existing pre-launch/runtime ERROR checks. Detailed assertion counts and water-cell counts were not supplied for this run and are not recorded as fixed evidence.

**Result:** ENV-008 complete. AMJE retains world-level ownership only; Vanilla River/Coast mutators remain the accepted local-map generation path. Waterfalls remain deferred.


### ENV-009 — Functional Vanilla wildlife proxy baseline

**Requested by:** Environment/design audit  
**Owner:** Environment/biomes  
**Status:** DONE

Responsibility:
- Environment does not add Japan-specific animal Defs merely for regional flavor;
- AMJ-owned BiomeDefs still require functional wildlife pools, so Alpha uses curated Vanilla PawnKindDefs as gameplay proxies;
- proxy names are not literal historical-species claims.

Implemented cleanup:
- removed Raccoon, Elk, Ibex, Fox_Arctic and Lynx from AMJ biome pools;
- replaced the Alpine Elk/Ibex/ArcticFox/Lynx placeholder mix with Deer/Fox_Red while retaining existing generic hare/wolf proxies;
- runtime assertions verify retired placeholders have zero commonality and accepted proxies remain positive through the public `BiomeDef.CommonalityOfAnimal` API.

Implementation/test history:
- wildlife pool cleanup: `bc949740fad30df8d7cf68169c3c87318de09139`;
- original runtime proxy assertions: `9cccbc3a80eee3e3ebc7a9550fde01d3093ee11a`;
- public-API runtime fix: `9d8d4c5ce95753e0e4d0c145dffb9d374aab30c6`;
- original static/localization coverage: `1d5ad32d47c8da48af9e8ecae8a9c3a37b9c85aa`;
- static validation now locks the full accepted positive proxy sets as well as the retired list: `bef56f78096fff07eb8c6a80bf53feba48e480b4`;
- design source of truth and research/proxy interpretation: `6a12906195ba153efc98c67c17f7ddd5fa125894`.

**Runtime status:** PASS by inheritance from the combined Environment runtime gate reported by the author during ENV-008 verification. The wildlife assertions were already present in the Quicktest source before the River/Coast handoff tests were added, so that successful combined run exercised them. No separate manual wildlife smoke is required.

**Result:** the Alpha wildlife layer is a functional Vanilla proxy baseline only. Future Japan-specific animals, textures or animal-resource loops require a separate owning feature/mod and should not silently expand Environment's scope.


### ENV-010 — Final Environment art pass

**Requested by:** Environment/release readiness  
**Owner:** Environment/art  
**Status:** IN PROGRESS

All functional Alpha Environment gates are now complete, so the project can enter the previously deferred final-art stage.

Expanded minimum final-art scope after Vanilla/MO audit:
- 5 AMJE plant-state images: Sudajii, leafy Japanese beech, leafless Japanese beech, Shirabiso, Haimatsu;
- 12 Vanilla tree-state retextures used by AMJE / normal MO coexistence: Oak, Maple, Poplar, Birch, Willow (leafy + leafless), Pine, Bamboo;
- 7 optional Medieval Overhaul tree-state retextures: Great Oak, Great Iter, Great Willow (leafy + leafless), Great Fir;
- 1 seamless Thin Soil texture;
- 4 world-biome textures: WarmTemperate, CoolTemperate, Subalpine, Alpine.

Total minimum: **29 image assets**.

The retexture scope is visual only. Vanilla Def mechanics are unchanged; MO Def mechanics/content identity are unchanged and MO graphic-path patches apply only when MO is active.

Art policy:
- start with one final texture per plant/state; the existing `Graphic_Random` implementation can load a one-texture folder and accept more variants later;
- preserve Vanilla seasonal systems rather than baking snow/autumn state into evergreen/base textures;
- only switch Def paths after the corresponding binary asset actually exists;
- final visual review is intentionally manual for silhouette, scale, tiling, color/contrast, and world-map distinction;
- automated validation must continue to catch missing-texture/runtime ERRORs after each asset is integrated.

Art direction source of truth: `Docs/ArtDirection.md`; initial direction `6c310e7efb1fb0ae2a03e87cda7d803bffd398a8`, expanded Vanilla/MO retexture scope `d973f30d70235c6060e68fcd8aa574964454c6c5`.

**Current state:** the ENV-010 atlas direction is the accepted style baseline. The first production asset, Sudajii, has now been extracted/reworked from that accepted direction into an AMJE-owned transparent PNG and integrated at `Textures/Things/Plant/AMJ/Shii/Shii_A.png`.

Integration commits:
- binary Sudajii asset: `94898bcd1dff5023a21b559fab01b990b2f0e832`;
- `AMJ_Tree_Shii` switched from the Vanilla TreeOak placeholder folder to `Things/Plant/AMJ/Shii`: `fad9938eb10e8cff2e8d8381c9622a7b505c5a6b`;
- static missing-texture / placeholder-path regression check: `74883e61a5761baca1dd6b751019390171e97e08`.

**Verification status:** the normal build/static gate passes after the final Sudajii asset switch and validator fixes. The latest isolated runtime reports generated AMJE vegetation successfully and did not show a missing-texture error, but the runtime suite previously verified only that plant Defs/instances existed, not that their resolved material textures were non-BadTex.

Static-gate follow-up commits:
- UTF-8 BOM fix for Windows PowerShell 5.1 validator parsing: `b4d873dfe2a36b5f73a3d82870352d2a57a7e19f`;
- wildlife XML commonality parse fix: `d8c523b31431388467f52c6b1728dc8b67f09489`.

BadTex runtime coverage added:
- plant graphic assertions now resolve the live `Graphic.MatSingle.mainTexture` for Sudajii, Japanese beech leafy/leafless, Shirabiso and Haimatsu and require a non-null, non-`BaseContent.BadTex`/ERRORTEX texture: `37c89ea3f862d5d27dba5b8fb4d9eaa681fe6f0c`;
- static validator locks those runtime assertions in place: `3056060f3188d0f17930c5bcb4bf81dc72104fd0`.

Follow-up after the BadTex gate passed locally: the user's normal-play `Player.log` did not emit any standard missing-texture/BadTex diagnostic for the visible question-mark tree, so the audit scope is being widened beyond AMJE-owned plants.

Tree-audit extension:
- all loaded `ThingDef` trees now have their base graphic plus configured leafless/immature/polluted/snow-overlay states resolved and checked for null/BadTex/ERRORTEX; `Graphic_Collection` variants are inspected individually: `bbcf4ae47066d828fa59dc0b1f84de84798823f2`;
- isolated runtime profiles can now include Medieval Overhaul with its required Vanilla Expanded Framework and Processor Framework dependencies: `f302316f7f14ecd3a73d9343d68d0e83a1216cfa`;
- focused tree-texture audit runner mode: `4e2c06b2071fd6d3e237ffed5bb3dfd56ee4369c`;
- `run-runtime-tests.bat` automatically runs the optional MO profile when MO and both required framework Workshop mods are installed: `7985593955576535ea4b9eb251facb1a057ddccc`, with CCTO failure preservation fix `8a16eb847904458b45835bb8919de9ca393c0326`;
- static validator locks the expanded audit harness in place: `11c69e2cec9c873d508e7d1d903f14d37aef86da`.

Expanded audit first-run result: the AMJE-only profile failed on `Plant_TreePine:polluted=Things/Plant/TreePine_Polluted`. This was a test-harness false positive, not evidence of the visible question-mark tree: RimWorld only loads `pollutedGraphic` when Biotech is active, while the isolated AMJE-only profile does not activate Biotech. The audit now checks the polluted tree state only when `ModsConfig.BiotechActive` is true: `16f5dbb583ed7ec9a98b90687d3508df61677a15`; the static validator locks this condition in place: `01d35282578bebf2bed52b5d859cc2e6a62d51d7`.

**Verification status:** previous AMJE-only/CCTO BadTex gates passed locally; the corrected all-tree audit proceeds into the optional Medieval Overhaul profile, but the first focused MO run exceeded the original 180-second outer timeout before producing a report. This is currently treated as a harness-time-budget issue rather than a tree-texture failure because no `BAD def=...` result was produced.

MO runtime-harness follow-up:
- the runner now prints a 15-second `[WAIT]` heartbeat while RimWorld is still alive, so slow startup is distinguishable from a frozen console: `1bbb382ce4702e82871b10bdcc2da1bf39ed099b`;
- the focused Medieval Overhaul profile timeout is extended from 180 to 360 seconds: `e22e76922e9de519ce621e5e7e8447c0b573e950`;
- static validation locks both the MO-specific 360-second timeout and heartbeat marker in place: `31e291906c3a421b0b65443d2e832c129da9bb9b`.

Second MO runtime attempt also exceeded the 360-second outer timeout. The previous MO timeout log stopped during very early mod loading, before Quickstarts/AMJE initialization, so repeatedly extending the timeout is not a useful default gate. The MO tree-link check is therefore moved to a deterministic static audit of the installed MO 1.6 Defs and texture files.

MO timeout pivot:
- new `Scripts/Validate-MedievalOverhaulTreeTextures.ps1` scans installed MO 1.6 `TreeBase` / `DeciduousTreeBase` Defs and checks base plus configured leafless/immature/polluted/snow-state texture paths against MO texture roots: `37cfa2cafe8dc88db597bd549d0ba9f7c7457363`, with PowerShell variable-delimiting fix `2fc70cee687356afcb585d81b8092727b7804936`;
- the static MO audit now runs from `run-tests.bat`: `aeefd28ded6fab2d26dd8d85b016183860764d30`;
- the hanging isolated MO runtime profile is removed from the default runtime gate; AMJE and CCTO runtime checks remain unchanged: `a7fe02a2e739874b0274dd14952dad06a931d954`;
- static validation locks the new audit path in place: `6d2fb435ee1cd2deefb5e5b8d97ffd038ae6630c`.

An offline audit of the supplied MO 1.6 source archive found all 15 tree graphic references from the MO tree Defs resolving to PNG assets, including the four Dark Forest great trees and the four fruit-tree base/immature states. This reduces the likelihood that the visible question-mark tree is a simple missing file in MO's own current tree Defs.

The revised static gate and normal runtime gate now both pass locally after removing the hanging MO minimal-runtime profile. This confirms AMJE-owned plant graphics, the normal AMJE/CCTO runtime path, and installed MO tree source texture references all pass the current automated coverage.

To minimize manual diagnosis of the still-visible question-mark object, a Dev Mode live-map diagnostic has been added. It scans the actual current map things under the user's normal mod list, resolves each live `thing.Graphic` material (plus plant snow overlays), and logs the exact `defName`, label, map position, graphic class/path and texture name for null/BadTex/ERRORTEX cases: `f3d26cc9142c16f857a26aa3d71deb9484b16686`. Static validation locks this diagnostic in place: `d2ebf5437121b2da8b564973e00675d4e16856b6`.

The user clarified that the red question-mark textures were visible during the automated Quickstarts themselves, including both the first WarmTemperate run and the final Alpine run. Video review shows many question-mark tiles/objects across the map rather than one isolated tree. Since the existing live-plant audit still passed, the diagnosis has been widened beyond plants.

Quickstart live-object audit extension:
- every fixed-biome Quickstart now scans live non-plant map Things with standard `graphicData`, resolves the actual `thing.Graphic.MatAt(...)`, and logs exact `defName`, label, category, position, runtime type, graphic class/path and texture for null/BadTex/ERRORTEX cases: `4fc048be9ad3cc8afa62609505d000df2f7bc970`;
- static validation locks the new `[AMJ Environment LiveThingTextureAudit] BAD` assertion path in place: `56bb08d616a2da200758bf64c8e86d656a53be26`.

The visible question marks still appear in WarmTemperate while both live-plant and live-non-plant Thing audits pass. Video review shows repeated question-mark decals distributed over open ground rather than attached to ordinary Things. RimWorld's `SectionLayer_TerrainScatter` renders `ScatterableDef` materials directly from each terrain's `scatterType`, outside `map.listerThings`; its generated scatter points use a five-cell minimum spacing, which matches the observed repeated ground pattern. AMJ Thin Soil currently uses `scatterType=Rocky`, so terrain scatter is now the leading candidate.

Terrain-scatter diagnostic extension:
- each fixed-biome Quickstart now collects the scatter types actually present on map terrain, checks every referenced loaded `ScatterableDef.mat.mainTexture` for null/BadTex/ERRORTEX, and logs exact defName/scatterType/texturePath on failure: `6de0ed9adcde6823d6f71d6e34d09744b695c035`;
- static validation locks the new `[AMJ Environment TerrainScatterTextureAudit] BAD` path in place: `b6b6d3db2e18270d99200175639c9852e725962a`.

The user reran WarmTemperate with visible red question marks still present, yet the plant, non-plant Thing, and terrain-scatter audits all passed. This rules out their source materials at the data-object level, but not the actual material handed to RimWorld's map section renderer.

Render-pipeline diagnostic extension:
- the developer Quicktest assembly now Harmony-patches `MapDrawLayer.GetSubMesh(Material)`, the common path used by static map section layers (terrain, terrain scatter, map-mesh Things, etc.), and emits `[AMJ Environment BadRenderMaterial]` as an ERROR whenever the actual render material is `BaseContent.BadMat` or uses `BaseContent.BadTex`/ERRORTEX: `a58053f875131a90f4f859723db4ce251015c0df`;
- Quicktest compilation now explicitly references Harmony: `b93f50402e91c4554fa70e506ced15181d2a2046`;
- first local build after this change failed because `Environment.GetEnvironmentVariable` was resolved against the enclosing `AncientMedievalJapan.Environment` namespace instead of `System.Environment`; fixed by fully qualifying the runtime API: `77968bd4d566530713f359a6a00ee341b3e308f1`;
- static validation locks the render-material interceptor, Harmony reference, and fully-qualified environment lookup in place: `8f924aa69154b1e0fde3b479ebfc75ca9ee87466`, `9b970172077dd784963251220fcedb510edb9249`.

This is intentionally lower-level than the previous Def/Thing/scatter checks. The first run with the interceptor did not find a bad submesh material; instead the diagnostic itself caused Unity log flooding by reading `Material.mainTexture` on built-in shader materials that legitimately do not expose `_MainTex` (for example SunShadowFade, EdgeShadow, masks, lighting overlays, and water-depth materials). Quickstarts therefore reported all 54 assertions passing while the overall report failed with 10,000 captured log errors.

Harness correction:
- render-material probing now first checks `material.HasProperty("_MainTex")`; materials without that shader property are skipped instead of invoking Unity's error-producing getter: `adff36a11f26f27937a0a7cde1b121363ab4d2e7`;
- static validation locks the safe `_MainTex` guard in place: `d80c81249e0e2df76cfd7ddcab49dfd1674369d1`.

The corrected submesh-material interceptor now runs cleanly: WarmTemperate still visibly shows red question-mark placeholders, but the full Environment runtime gate passes, including AMJE+CCTO. This means the question marks are not being exposed as BadTex on the final static `MapDrawLayer.GetSubMesh` materials. A likely remaining gap is atlas substitution: static sprite paths can pass a BadTex source material into `Graphic.TryGetTextureAtlasReplacementInfo` and then hand a valid atlas material to the section layer, masking the original BadTex from the previous interceptor. Realtime Thing/Mote-style draws also bypass map submesh generation.

Deeper render-path tracing:
- Quicktests now intercept `Graphic.TryGetTextureAtlasReplacementInfo` before atlas substitution and fail/log `[AMJ Environment PreAtlasBadTexture]` if the source material is BadTex;
- Quicktests also intercept `Graphic.Draw` and `Graphic.DrawFromDef` to catch realtime Thing/Mote-style BadTex draws with defName, label, position, runtime type and graphic path;
- implementation: `7ccd9b38c5c69580a961c672e086b3abb41ed4e5`;
- static validation locks all three new trace markers/assertions in place: `cf29a5d60da56539a9a76f197b0d4d7a726811a5`.

Latest WarmTemperate rerun triggered the new realtime tracer, but the three hits were all intentional pawn shadow graphics: Human, YorkshireTerrier, and Hare were reported as `Verse.Graphic_Shadow` with `BadTexture`. This is a harness false positive. Vanilla `Graphic_Shadow` inherits the base `Graphic.MatAt/MatSingle`, which return `BaseContent.BadMat`, but `Graphic_Shadow.DrawWorker` never renders that material; it renders `MatBases.SunShadowFade` directly. The visible question marks therefore remain unexplained by these three errors.

Harness correction:
- realtime tracing now skips `Graphic_Shadow` before probing `MatAt`, and the assertion explicitly refers to non-shadow BadTex materials: `42ee30a048316ecd00dd5f70b59aa1aa892b380c`;
- static validation locks the Graphic_Shadow false-positive guard in place: `b7692029f3751ac7e1a3c72a5210877fc3508f3c`.

The user confirmed the visible red question marks still appear after excluding the intentional `Graphic_Shadow` sentinel, while the runtime gate passes. The informational line about Medieval Overhaul is expected: this Quickstart profile does not load MO at runtime, and MO tree-path coverage remains static-only. It is not evidence that the visible placeholders come from MO.

Video/frame review shows many roughly pawn/cell-sized red-question-mark placeholders distributed across the generated map. The prior generic `Graphic.Draw` tracer does not cover RimWorld 1.6 pawn bodies: pawn rendering uses `PawnRenderNodeWorker.GetFinalizedMaterial` and the render-tree pipeline directly. This is now the next targeted gap rather than immediately blaming Flecks.

Pawn render-node diagnostic:
- Quicktests now Harmony-postfix `PawnRenderNodeWorker.GetFinalizedMaterial` and emit `[AMJ Environment PawnRenderBadMaterial]` with pawn def/kind, label, position, node class, primary graphic path, material/texture, and apparel/hediff/gene context if an actual pawn render node resolves BadTex;
- implementation: `ddbf164838e166098f188ec9c337a51bbfdc81fa`;
- static validation locks the new pawn-render trace and assertion in place: `b8946fc2408f88ac5c006df791a4b1e593ba659c`.

The user noted that the placeholder count is visually too high to make ordinary pawns a strong explanation, and that the first WarmTemperate Quickstart reproduces the issue every run. For iterative diagnosis, repeatedly launching the full six-scenario runtime suite is unnecessary; keep the full suite as the final regression gate, but use a focused WarmTemperate-only interactive runner during root-cause investigation.

Focused interactive texture-debug runner:
- new `run-texture-debug.bat` builds/static-validates, prepares the same isolated Dev Mode profile, launches only `AMJWarmTemperateTerrainQuickstart`, and intentionally omits Quickstarts verify/report flags so RimWorld stays open until the user closes it manually;
- this lets the user select/inspect a visible red-question-mark object directly while all current quicktest Harmony diagnostics remain active through `RIMWORLD_QUICKSTART`;
- implementation: `cebf28d972452009098532225d30641719b53efd`;
- static validation ensures the helper stays WarmTemperate-only and does not accidentally add auto-exit `-quickstartreport` / `-quickstartverify` flags: `977beab5ea5e1ff07a9f5c7ed3cc0d27c237096f`.

The full `run-runtime-tests.bat` remains unchanged and should still be used once the defect is fixed to prove all biome + river/coast + optional CCTO runtime coverage.

The focused interactive run resolved the object identity: the user selected one of the visible red question marks and the inspect panel identified it as **Sudajii / `AMJ_Tree_Shii`**. The large number of placeholders is consistent with WarmTemperate's high Sudajii wild-plant commonality; the pawn-render hypothesis is no longer relevant to this defect.

Binary inspection then found the direct cause in `Textures/Things/Plant/AMJ/Shii/Shii_A.png`: the PNG had valid IHDR and PLTE chunks but a malformed transition after the indexed-color transparency (`tRNS`) chunk. Three stray bytes occurred before the valid IDAT chunk, so standard PNG chunk traversal failed and RimWorld's texture loader fell back to the red `BadTexture` placeholder. The image's IDAT payload itself was intact.

Fix and regression coverage:
- repaired the existing accepted Sudajii PNG without replacing the artwork; the malformed bytes were removed, the `tRNS` CRC was corrected, and the repaired file now parses as IHDR/PLTE/tRNS/IDAT/IEND with valid boundaries: `0421396e6184fdc12933d07db0bf88990b43ab9d`;
- Quicktest BadTex detection now also recognizes the runtime texture name `BadTexture`, and Sudajii's resolved UI icon is explicitly asserted non-BadTex: `1eacad99783d5e0de5e0a0cd84183053e2c4b85e`, follow-up collection-path guard `69013cbaa5cb3c01fe7ded3167953fb3c3e7439b`;
- static validation now checks PNG chunk structure, first for Sudajii and then recursively for all AMJE PNG assets so future generated art cannot silently ship a malformed image: `2f0a868fe438b8f54fde3476b48923af887a1867`, `d615543c312b429e37c8a3fb33c0f53538ea0a7d`.

**Next action:** pull/build and rerun `run-texture-debug.bat`. Confirm that Sudajii now renders normally both on the map and in its inspect/UI icon. Once confirmed, run the normal runtime gate once as the regression check, then continue ENV-010 with the remaining planned tree retextures and Japanese-first description review.


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


**Core integration audit (2026-10-04):** AMJ Core has reconciled its older duplicate terrain/Hilliness ownership text with Environment. Core now treats fertility **0.50** as the shared low-fertility integration point: Soba/Kibi/Awa/Hie/Barley remain sowable, MO Wheat at fertilityMin 0.70 does not, and the intended growth-factor order is Soba > Kibi > Awa > Hie > Barley. Core does **not** request a 0.40 Environment terrain; Soba's 0.40 fertilityMin remains a crop property/compatibility floor. No Environment implementation change is requested. Core design reconciliation: `c841b66547f51af79743a72d3db20d38b7345816`; Core regression coverage: `201bbe90aa92a0d2d55ac9d1d16c92bdf6f28c11`.

### TEST-001 — AMJ-wide runtime ERROR gate policy

**Requested by:** project-wide automated-test policy  
**Owner:** testing/tooling  
**Status:** DONE

AMJ automated tests that launch RimWorld must capture an isolated runtime log and fail on ERROR-level entries attributed to the repository-owned mod, even when scenario counts otherwise pass.

Environment's `run-tests.bat` remains a build + static-validation gate only. A separate `run-runtime-tests.bat` now launches four fixed-biome vegetation Quickstarts against an isolated test profile, writes separate runtime logs/reports, and fails on Environment-origin ERROR entries in addition to Quickstart assertion failures. The user's normal mod list and preferences are not modified.

Policy commits: `2a86387ebf1bfe5d3de3fbf09de93800cace0e74`, `7148ff5df9cdc2078e55c4233bf4b60e1e112c70`.


### DOC-001 — Shared public-description format and save compatibility

**Requested by:** author / public-description policy (2026-10-04 JST)  
**Owner:** Documentation/release  
**Status:** DONE (public source files prepared; Steam page not yet published)

All AMJ-related mod descriptions must include save compatibility. CCTO is the evolving format baseline. Durable shared policy: [Docs/ModDescriptionGuidelines.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/ModDescriptionGuidelines.md). Addition/removal safety must reflect each mod's actual implementation; custom content and world-generation mods do not inherit CCTO's safe-removal claim.

About.xml now states the development build's save-compatibility limits; AGENTS.md points to the shared policy for future README/Workshop preparation.

Public source preparation is now complete:
- detailed README source: `fa998e2e0f8a690f6b0add09146d3247dd3366f4`;
- Japanese Workshop source written first: `c2686e93378b14aceb0270c758e1bebb9ec9325a`;
- English translation synchronized from the Japanese source: `105c181b69c1ce402a6467b5e9478d2a057a2db0`;
- Workshop presentation/authoring policy: `a3c951739bcbf5624009740c6b1c0df7014ce334`;
- About.xml Alpha positioning and save-compatibility wording: `72f93c6d93b2a9b9fb8e47a95ff54688b3e5f14e`;
- public-description static validation: `06f7aa605d6d4519fa2ae643fa18055847fbe9a0`, marker correction `274b9cac3f4544fab1b2dc49c5a7ec45134a8679`.

Japanese Workshop source is **3692 UTF-8 bytes** and English is **3616 bytes**, both below the 8,000-byte policy limit.

**Result:** repository-side public copy is prepared. This does **not** claim that the Steam Workshop page itself has been created or updated. Final screenshots/art should be completed before public release presentation is treated as finished.


### TEST-POLICY-002 — RimTest Redux / Pickle automation-first policy

**Requested by:** author (2026-10-04 JST)  
**Owner:** Testing/tooling  
**Status:** DONE (policy documentation)

The shared project policy now prioritizes RimTest Redux / Pickle automated testing and minimizes human manual tests. Durable instructions are in `AGENTS.md` and `Docs/DevelopmentTools.md`. Reproducible logic, loaded Defs, runtime behavior and release regressions should be automated; manual testing is reserved for appearance, readability and play/interaction feel. Build/static checks remain complementary, and runtime suites retain the mandatory mod-origin ERROR gate.

This documentation update does not claim new runtime coverage or a new test PASS. Existing implementation/test history remains unchanged. Environment's runtime harness is still absent; runtime/numeric checks listed in DevelopmentTools are explicitly identified as automation targets.

**Next action:** apply this policy to subsequent feature, fix and release work; record unautomated coverage explicitly and move reproducible checks into the automated gate.

**Result / references:** AGENTS policy commit `32d067ab2057ba032ccb21bdc371f3b2a6770d74`; development workflow commit `8a6106bc4aab214d3da77f75207c76d6e5bdb816`.

### ENV-010 text/localization rule — retextured plants

**Requested by:** author (2026-10-04 JST)  
**Owner:** art/localization  
**Status:** IN PROGRESS

All plants whose artwork is replaced by AMJE now also require an AMJE-authored description. This includes AMJE-owned structural plants, Vanilla tree Defs retextured by AMJE, and MO tree Defs conditionally retextured by AMJE. Labels remain unchanged unless a separate naming issue is identified; this requirement is specifically about replacing unsuitable/inconsistent descriptions.

Authoring flow is fixed: draft Japanese first, author reviews/approves the Japanese wording, then translate only the approved Japanese text into English. English should remain semantically aligned with the approved Japanese source rather than becoming an independent rewrite.

Canonical policy is recorded in `Docs/ArtDirection.md`: `4881b88a2efcbbc560429026329b2e1a34ee4a0c`.

### ENV-010 historical description scope expansion

**Requested by:** author (2026-10-04 JST)  
**Owner:** art/localization  
**Status:** IN PROGRESS

The prior ENV-010 rule requiring rewritten descriptions for AMJE-retextured plants is now explicitly part of the broader AMJ historical-description policy. AMJE must audit inherited Vanilla/MO descriptions for AMJ-facing items/plants/animals it uses, patches, retextures, selects, or localizes, not only the specific tree sprites being replaced.

Descriptions should contain supported historical facts and meaningful modern differences where supportable, with Japanese drafted and author-approved before English translation. Retexture work still carries a mandatory description review as a minimum art-pass requirement.

**Result / references:** shared policy in Core `Docs/HistoricalDescriptionGuidelines.md` commit `ca17b37eb3cca5266d1f62a2d73f527a503d76e5`; AMJE AGENTS `0e1b74173eca79dde09dffa2287fc5f72a583c27`; ArtDirection alignment `3b8b7c753b79951b315c2ffe31822416a31595c8`.

### ENV-010 Sudajii final visual direction

**Requested by:** author (2026-10-04 JST)  
**Owner:** art/localization  
**Status:** DONE — accepted production appearance recorded in ArtDirection

The Sudajii in-game comparison was reviewed against actual AMJE terrain and the current AMJ crop-art baseline. The author selected the strongest simplified/deformed **C-direction** and then approved a lower-saturation refinement for production.

Locked production direction:
- remove the baked green ground/grass ring entirely;
- use a thicker near-black outer outline;
- simplify foliage into larger/chunkier masses with fewer internal color clusters;
- keep the approved hue relationships but reduce foliage saturation, especially the brightest yellow-green highlights;
- reduce trunk orange saturation while preserving value contrast;
- keep transparent background and no decorative ground base;
- target the same simplified visual weight as the current AMJ crop sprites rather than the earlier more detailed Sudajii pass.

Canonical art-direction update: `0815e0257deaa4b39fd41fe5231426a5215f5306`.

The standalone Sudajii sprite was then generated in the locked C-direction and approved by the author. The first GitHub binary write for that final sprite was malformed and was correctly rejected by the new static PNG-structure gate with `Final Sudajii PNG is structurally invalid`; that failed write must not be treated as a valid production asset.

The exact validated 256x256 source PNG was then transferred without manual base64 transcription and now replaces `Textures/Things/Plant/AMJ/Shii/Shii_A.png`. GitHub blob SHA is `62d795d9717a3ad4d61827bfb470cd7b3cfc7000`; corrective commit: `81a2c60af98553610c87d6606b0eba71d05b2a6a`. This file is the approved C-direction asset: transparent background, no ground/grass ring, muted olive/forest foliage, restrained trunk saturation, and the accepted simplified silhouette.

The author completed the focused WarmTemperate visual check and accepted the final Sudajii sprite in game. The rendered tree now sits naturally against the terrain, the green ground ring is gone, and the muted C-style silhouette/outline treatment is approved. Canonical art-direction acceptance: `ee61dba6614e1344a96284b669f8983cee16af99`.

The author confirmed the normal Environment runtime gate passes with the accepted Sudajii asset in place. Sudajii is therefore complete for ENV-010 visual implementation and regression coverage.

The author approved the revised Japanese beech pair after rejecting the first pass as too similar to Sudajii. The accepted pair has a visibly open, horizontally spreading crown, exposed branch structure, pale grey-beige bark, lower-saturation lighter foliage, and a matching broad leafless branching silhouette.

Implementation:
- leafy sprite: `Textures/Things/Plant/AMJ/Beech/Beech_A.png`;
- leafless sprite: `Textures/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png`;
- `AMJ_Tree_Beech` now points to those AMJE-owned paths instead of Vanilla `TreeMaple` placeholders;
- integration commit: `8d0eeabf808ec4cbe37a78745675e9aa39561466`;
- static validation now locks both files, both AMJE paths, PNG structure, and absence of the former TreeMaple placeholders: `af0fb7608c3fa1d0f3f3ec29877b9ba3637a66bd`;
- canonical art-direction record: `b21471b8e1bb7de1d328c9fb5055c12993463863`.

The first binary transfer of the approved beech sprites was malformed and was correctly rejected by the AMJE PNG-structure gate (`Beech_A.png`). The Def/path integration itself was correct; only the PNG payloads needed replacement.

Corrective binary transfer:
- leafy source was re-read directly from the validated 256x256 PNG and committed without manual reconstruction; current Git blob SHA `f6866dc5faa5ad4293f8bd1e72cc43b6668265f9`;
- leafless source was transferred the same way; current Git blob SHA `c9fdc9b87e6b559ab2a2ff9788b6d015593b8406`;
- corrective commit: `b7027fc930d77a9b079ace5b5f9d663ade2f236c`.

The author reran `run-tests.bat` after the corrected binary transfer and confirmed it passes. Static PNG structure, Def path, and placeholder-regression validation are therefore clean for both Japanese beech states.

The focused check exposed two separate issues:

1. The debug runner was still opening WarmTemperate, where Japanese beech does not naturally populate. The author had to spawn a beech manually. The focused runner now targets `AMJCoolTemperateTerrainQuickstart`, where `AMJ_Tree_Beech` is the intended natural structural tree: `da47b7b9e911bb2f38136b7ba884c99d54997be9`.
2. The manually spawned leafy beech rendered as the red question mark. Binary inspection showed that the PNG's chunk boundaries were valid, so the old static structure gate passed, but the `IDAT` CRC was wrong. The leafless PNG CRCs were already valid. The leafy PNG was repaired in place by recalculating only the stored IDAT CRC; current leafy blob SHA `c0928ef133866cefb006dade3240238412190d39`, corrective commit `1900bf107143aa025ed9713c408b8847425f8712`.

The static PNG validator now checks CRCs for every PNG chunk in addition to signature/chunk boundaries, preventing this class of false PASS: `25ccb50389f3b4dd6497cd78ad84849bfe0388cd`.

A follow-up static-validation failure exposed another recurring maintenance class: the validator still hardcoded the old WarmTemperate focused-debug target even though the runner had correctly moved to CoolTemperate. This was a stale self-test, not a new texture or Quickstart defect.

The focused-debug validator has now been made target-agnostic. Instead of requiring WarmTemperate and forbidding every other biome, it now:
- accepts any of the four known AMJE terrain Quickstarts;
- requires exactly one `RIMWORLD_QUICKSTART` assignment and exactly one `-quickstart=` invocation;
- requires those two targets to match;
- continues to forbid report/verify flags so the focused window remains open for manual inspection.

This removes the need to rewrite the static validator every time the focused texture-debug biome changes. Commit: `a7f7cbb7db10170cd9cdd5803abdc0b64ee60362`.

A parser failure then exposed that commit `a7f7cbb7db10170cd9cdd5803abdc0b64ee60362` had accidentally truncated the replacement block inside `Validate-Environment.ps1`: the regex string for `RIMWORLD_QUICKSTART` was left unterminated and the remainder of the intended validation block was missing. The many later errors involving method calls and ampersands were cascading parser errors from that single unterminated string.

The complete target-agnostic texture-debug validation block has now been restored: `9d80bfdbf0c7fe68ec32c50b90473f40663fc8b5`.

Because this class of error has recurred during PowerShell edits, repository-level prevention was added as well: `.github/workflows/powershell-syntax.yml` parses every `.ps1` with PowerShell's own AST parser on pushes and pull requests that touch PowerShell files. This is intended to catch malformed scripts on GitHub before the author discovers them during a local run. CI commit: `e3cf52307b9933dad57aab1640923c927de8dc23`.

A second parser failure (`UnexpectedToken ')'` near line 1163) showed that `Validate-Environment.ps1` still contained a duplicated trailing validator block after its intended `exit 0`. The extra tail began with a stray `)` and repeated earlier texture-debug/runtime validation content. This was the remaining source of the parser failure.

The validator has now been structurally repaired so it has a single terminal `exit 0` and no duplicated tail. Prevention was strengthened at two levels:
- local: new `Scripts/Validate-PowerShellSyntax.ps1` parses every repository `.ps1` via PowerShell AST, and `run-tests.bat` executes this preflight before build/static validation;
- repository process: `AGENTS.md` now requires any future `.ps1` edit to be staged on a temporary branch and merged only after the PowerShell syntax PR check passes. Large direct string-replacement edits to `Validate-Environment.ps1` are explicitly discouraged, with edited-range/end-of-file re-read required before merge.

This repair was deliberately staged on PR #1 rather than written directly to main. The `PowerShell syntax validation` workflow completed successfully on the PR head, and the PR was then squash-merged to main. Merge commit: `5d0a6a3167275d7323da76b1d4b45c692f168d66`.

The first local run of the new syntax preflight exposed a Windows command-line quoting defect rather than a parser defect: `run-tests.bat` passed `%~dp0` as `-RepoRoot "%~dp0"`; because `%~dp0` ends with a backslash, PowerShell received a path with a stray trailing quote and `Get-ChildItem` failed with `Path contains invalid characters`.

The prevention design has been simplified so this argument cannot fail:
- `Scripts/Validate-PowerShellSyntax.ps1` no longer accepts `RepoRoot`; it derives the repository root only from its own `$PSScriptRoot`;
- `run-tests.bat` invokes the preflight with no path argument;
- the GitHub workflow now executes that exact repository preflight script instead of maintaining a second inline parser implementation.

This was staged through PR #2. The actual repository preflight completed successfully in GitHub Actions before merge. Squash merge commit: `4bc33424ed008706aea6c616b7b7956e781004a7`.

The author reran `run-tests.bat` after the self-rooting preflight fix and confirmed it passes. The PowerShell syntax guard, build, AMJE static validation, and MO static tree-texture audit are therefore clean in the current main state.

**Next action:** resume the focused Japanese beech visual check with `run-texture-debug.bat`. The runner currently targets CoolTemperate so `AMJ_Tree_Beech` should appear naturally. Inspect the leafy sprite first; if accepted, check the leafless seasonal state next.

### ENV-010 Golden Path — production texture pipeline

**Requested by:** author / Environment art  
**Owner:** Environment/art + tooling  
**Status:** DONE

The successful Sudajii/Japanese-beech lessons are now captured as a reusable production-texture Golden Path. The Environment repository already contains the concrete implementation in `Docs/GoldenPaths/TextureAssetPipeline.md` and `Scripts/Install-TextureAsset.ps1`, with exact-byte SHA-256 copy verification, PNG signature/chunk/CRC/dimension/transparency validation, Def-switch ordering, repository static gates, and correct-biome focused runtime review. `run-texture-debug.bat` now accepts a biome selector instead of requiring a code edit for each target. The implementation and CI smoke coverage were merged in `8a2307e45bf669a154a1236297695be3f2a03cc2`; the corresponding GitHub PowerShell/Golden-Path workflow run completed successfully.

The AMJ-wide Golden Path closeout rule is additionally referenced from Environment `AGENTS.md` in `6c6d6a2aae3a80c3650f30049982fe8a36f01ea7`, pointing to Core `Docs/DevelopmentGoldenPathGuidelines.md`.

**Next action:** resume Japanese beech visual validation using the repository Golden Path rather than ad-hoc binary transfer/debug-runner edits.
### ENV-010 visual-style baseline — retexture rules

**Requested by:** author / Environment art  
**Owner:** Environment/art  
**Status:** DONE

The shared visual language for AMJE tree/plant retextures is now canonical in `Docs/ArtDirection.md` under **Retexture visual-style rules**. The rule set locks strong simplification/deformation, thick dark outlines, restrained low-to-medium saturation, limited color steps, transparent/no-ground-base sprites, structural rather than recolor-only species differentiation, deciduous leafy/leafless continuity, and normal-game-zoom readability.

Category baselines are also fixed for evergreen broadleaf, deciduous broadleaf, tall conifer, and low/dwarf conifer work. Accepted Sudajii and Japanese beech art are the current reference baselines for evergreen-broadleaf and deciduous-broadleaf retextures. Future species should declare their category and structural differences from those baselines before source-art generation.

Canonical art-direction commit: `447b55e082b5e08e0c083e37f5f81dc92e458651`. Agent enforcement/reference commit: `a970a4bb12df937a7f08a9240e00b30248e63c2d`.

Production binary transfer/integration remains governed separately by `Docs/GoldenPaths/TextureAssetPipeline.md`.

**Next action:** continue Japanese beech visual validation, then apply the fixed visual rules + existing texture Golden Path to Shirabiso, Haimatsu, and subsequent Vanilla/MO tree retextures.


### ENV-010 leafy-beech payload recovery and decoded-PNG regression gate (2026-10-05 JST)

**Requested by:** continuation of Environment texture work  
**Owner:** Environment/art + tooling  
**Status:** DONE — binary recovery and focused appearance review completed; later acceptance entry is authoritative

The focused preflight found that the current leafy `Beech_A.png` blob `c0928ef133866cefb006dade3240238412190d39` passed chunk CRC validation but failed zlib decompression and normal Pillow image decoding. The earlier CRC-only correction therefore did not resolve the image payload corruption; prior static PASS must not be treated as proof that this image was decodable.

Recovery used the pre-CRC-rewrite payload's original stored IDAT CRC and its zlib Adler-32 as independent checks. Exactly one two-byte correction satisfied both checks and restored the expected 65,792 decoded scanline bytes for a 256x256, 8-bit indexed PNG. Offsets relative to the original file: 11783 (26 -> 30) and 12611 (188 -> 124). The recovered image also passed all PNG chunk CRCs, legal scanline filters, Pillow loading, and visual comparison with the accepted leafy source. The approved original pair was resolved from `libfile_d663b627e21c81919d33a20cc804444f`; no art was regenerated.

Current leafy production blob: `7face00d1128333b5fa6e1ab419d3424bc21b6dd`. SHA-256: `88274eaac76fc6fb8165c5d155a3008b0384004dbe65b38f2cce6badf6c1e072`. Sudajii and leafless beech also passed independent chunk/decoded-image checks; they were not changed.

Reusable prevention is implemented in `Scripts/PngImageData.ps1`, called by both the exact-copy installer and the static PNG gate. It validates zlib/DEFLATE, Adler-32, decoded scanline sizes/filters, and Adam7 pass sizes without a Python dependency. `Tests/Test-PngImageData.ps1` validates every production PNG and rejects a CRC-valid / zlib-corrupt fixture. The workflow now runs explicit Ubuntu PowerShell 7 and Windows PowerShell 5.1 jobs and triggers for production PNG changes. The transient `matrix.shell` workflow mistake (run 37218721890) was fixed on the PR before merging; PR run 37218817889 passed both jobs, including syntax, production PNG decoding, the corruption regression and exact-copy smoke.

**Result / references:** PR #4 squash-merged after the successful checks; implementation/PNG/Golden Path commit `1a24e11755c24c2109b7ceebd9628601c62f1f12`. Durable procedure is in `Docs/GoldenPaths/TextureAssetPipeline.md`.

**Next action:** pull main and run `run-texture-debug.bat "D:\\SteamLibrary\\steamapps\\common\\RimWorld" CoolTemperate` for leafy/leafless beech appearance and seasonal continuity. This environment did not contain RimWorld, so no new runtime/build or in-game visual PASS is claimed. After author acceptance, lock the ArtDirection visual result and continue to Shirabiso, then Haimatsu using the existing Golden Path.


### ENV-010 Japanese beech focused appearance accepted (2026-10-05 JST)

**Requested by:** author confirmation after focused texture debug  
**Owner:** Environment/art  
**Status:** DONE (reported focused appearance review)

After the repaired leafy-beech PNG and decoded-image regression gate were merged in `1a24e11755c24c2109b7ceebd9628601c62f1f12`, the author replied `問題なし` to the CoolTemperate focused-debug check. Record this as acceptance of the current in-game beech appearance. Canonical acceptance is reflected in `Docs/ArtDirection.md`.

This confirmation does not supply a new automated runtime report or separately describe forced leafless/seasonal-switch coverage. Preserve that distinction; do not fabricate a runtime suite PASS from the visual confirmation. The accepted assets and existing exact-byte/decoded-image regression guards remain unchanged.

**Next action:** prepare the Shirabiso source-art proposal using the tall-conifer category and accepted Sudajii/beech style baselines. Proposed structure: a compact upright conifer with broad, irregular layered needle masses, limited muted grey/blue-green color steps, subtle gradients, a visible short grey-brown trunk, thick dark outline, transparent background and no ground base/snow. Present the proposal before generation; this is not yet an author-approved new asset or a Def-path change. Haimatsu follows Shirabiso.


### ENV-010 new-chat generation continuity (2026-10-05 JST)

**Requested by:** author, before Shirabiso generation  
**Owner:** Environment/art + documentation  
**Status:** DONE (repository instructions / reusable generation entry)

The generation entry is now `Docs/GoldenPaths/RetextureGeneration.md`, required from AGENTS and linked from ArtDirection, the Golden Path index and TextureAssetPipeline. It carries current-source reading, actual reference viewing/attachment, precise work-stage recovery, current Core+Environment style-rule assembly, and candidate review into every new/resumed tree-art task. Visual specifications remain owned by ArtDirection; current progress remains only on main Coordination.

No image was generated and no production asset or Def was changed for this request. Shirabiso's previously presented design remains awaiting author approval. The top Active tree-art handoff is the current restart position. Existing beech/Sudajii visual results and PNG regression gates are preserved.

### ART-TEMPLATE-001 — AMJ shared pixel-exact components

**Status:** DONE (policy binding); family registration required before derivatives

Core policy/tooling is published in commit `7ce9af2ce4b3cf3bce1efda89ca1b199ab4efc36`: `Docs/GoldenPaths/FixedImageTemplates.md` and `Scripts/Art/fixed_template.py`. These Environment instructions bind to that shared policy. Reused visible components require hashed masters/masks and zero protected RGBA pixel differences. Distinct species keep species-specific structure. No asset/Def was changed; no generation or runtime test was performed. Shirabiso approval remains pending. Register a template before making a derivative that declares shared fixed parts.

### ENV-010 Shirabiso source approval and integration (2026-10-05 JST)

**Status:** DONE — source/integration and later focused appearance acceptance completed

The author approved the standalone generated Shirabiso. The approved source was resized once to 256x256 RGBA using Lanczos; the resulting PNG passed chunk CRC, zlib decompression, decoded byte count, Pillow loading, dimension/alpha and exact-copy checks before the Def path was changed. Production SHA-256: `aaef0a8db34426e028fdebe4a69efd3aadbc17ed98875e1f74c1bc2da4147d3d`. Existing Sudajii/beech files are unchanged. The source/design acceptance is canonical in ArtDirection.

This environment lacks PowerShell and RimWorld: no local PowerShell installer, build, full repository static suite or runtime PASS is claimed. GitHub CI runs the existing PowerShell PNG/payload gates and texture installer smoke. A dedicated Python regression locks the Shirabiso Def/path, production hash, dimensions and transparency. Existing Japanese/English descriptions were reviewed; no text change is introduced in this art-only integration, and the broader historical-description audit remains open.

**Next action:** pull main, run `run-tests.bat`, then `run-texture-debug.bat "D:\SteamLibrary\steamapps\common\RimWorld" Subalpine`. Review natural Shirabiso at normal zoom; record appearance acceptance separately from source approval. Then prepare Haimatsu proposal.

### ENV-010 Shirabiso focused appearance accepted (2026-10-05 JST)

**Status:** DONE (reported visual review)

After integration `e4ca2108f22aaacbead783240c59aab5759f8290` and the Subalpine focused-check instruction, the author replied `OK`. Record reported focused appearance acceptance in ArtDirection. No new runtime report or explicit full automated-suite PASS was supplied.

**Next proposal (not approved):** Haimatsu as a low, wind-shaped creeping pine shrub, clearly wider than tall, several asymmetric spreading needle masses, muted grey/blue-green palette, short partly hidden grey-brown woody branches, thick dark outlines, transparent/no ground/snow. Preserve accepted shared style; no upright miniature-tree silhouette. Present this before generation and await approval.

### ENV-010 Haimatsu source approval and integration (2026-10-05 JST)

**Status:** IN PROGRESS (source approved / installed; runtime appearance pending)

The approved generated source was resized once with Lanczos to 256x256 RGBA. Chunk CRC, zlib decompression, decoded scanline/filter checks, Pillow decoding, dimensions, transparency, exact-copy equality and Def-path regression passed. Production SHA-256: `44d22e74670bdfe081ef98c8a327700620f3d285e4a86b6dc52625c57daef551`. Canonical approval is in ArtDirection. Existing assets, plant mechanics and descriptions are unchanged. The reusable TextureAssetPipeline remains the production procedure; a Haimatsu regression joins the existing CI gates.

Execution is in a cloud Linux workspace despite the author using the desktop app. PowerShell and RimWorld are unavailable here; no local build/runtime/visual PASS is claimed.

**Next action:** verify CI, then run `run-tests.bat` and `run-texture-debug.bat "D:\SteamLibrary\steamapps\common\RimWorld" Alpine` in a Windows local execution task. Author review at normal zoom is still required.

### POLICY-001 — AMJ各Modのリテクスチャ所有方針

**Requested by:** author (2026-10-05 JST)  
**Owner:** Environment art / cross-mod ownership  
**Status:** DONE — durable design updated

AMJ共通方針に合わせ、Environmentは自分の景観責務に属するVanilla / MO等の前提資産まで画風統一を所有することを正式化した。

- ENV-010のVanilla/MO樹木リテクスチャはEnvironment本体の正式責務であり、別Retexture Modへ移管しない。
- 同一前提資産を他AMJ Modと競合上書きしない。1資産1所有Modを原則とする。
- Alpha中のplaceholder / final-art deferは作業順の都合であり、所有責務の移管を意味しない。
- Japan OnlyはEnvironmentのリテクスチャを担当しない。
- 横断資産で自然なownerを決められない場合だけ、将来の共通Retexture Modを再検討する。

Durable design source: `Docs/Design.md §11.5 AMJ共通リテクスチャ方針 — Environment所有範囲`, commit `5c836d3907577197396a465dbd88a982f601890b`.

**Next action:** ENV-010は既存Golden Path / ArtDirectionをそのまま使い、Environment-owned Vanilla/MO tree retexturesを継続する。

### TEST-002 — Core + Environment gameplay-contract automation

**Requested by:** author (2026-10-05 JST)  
**Owner:** Environment runtime integration / Core agriculture contract  
**Status:** DONE — runtime/integration gate completed; no release rerun required unless later changes invalidate it

Core + Environmentの独自ゲームプレイ差を、固定バイオームQuickstart上で機械評価する統合ゲートを追加した。

自動評価:
- 4バイオーム実マップのThin Soil / Gravel / Soil / Rich Soilセル数;
- Core Stage A作物のロード確認;
- Soba / BarleyがThin Soilを利用でき、Wheatは利用できないこと;
- 実マップ土壌分布でSoba/Kibi/Awa/Hie/Barleyの平均肥沃度成長倍率が分化すること;
- Coreの成長最低温度差が維持されること;
- Environment年間気候診断の <8C / <0C 時間がWarmLowland→Temperate→Cool→Highlandで寒冷化すること;
- Environment / Core由来のruntime ERRORゼロ。

`run-runtime-tests.bat` はローカルCoreを検出した場合、MO依存を含む隔離Core+Environmentプロファイルを自動生成し4固定バイオームを追加実行する。

PowerShell syntax workflowは関連スクリプト変更までGreen。Quicktest C#コンパイルと実RimWorld統合実行はこのクラウド環境では未実行。

設計精査中、Environmentの旧気候表に残っていたAwa -4C / Hie -4Cを、Core正本のAwa -3C / Hie -2Cへ同期した。

Durable design source: `Docs/Design.md §8.3 Core + Environment gameplay-contract runtime gate`, commits `14b09c8c05e4ee060446f4ab7d42db9ba94fca2f`, `6a3420b71b6a7fc60b16cbdd4bed73f759a33d2e`.

**Result:** author confirmed the Environment runtime/integration test work is complete. Preserve the existing automated gate for future regressions; do not rerun it solely because publication work resumed.

### TEST-POLICY-003 — Non-interactive runtime tests

**Requested by:** author (2026-10-05 JST)  
**Owner:** Testing/tooling  
**Status:** DONE — project-wide policy recorded; runtime harness migration pending

AMJ共通方針として、人間の目視を必要としないQuickstarts / integration / runtime ERROR / map・気候・土壌テストは可視RimWorldウィンドウを出さず実行する。

BadTex、Texture Atlas、Graphic.Draw等の描画検証ではrenderingを無効化せず、仮想/オフスクリーン/非表示表示先で実描画経路を維持する。可視実行は最終texture review等、人間判断が必要な場合だけに限定する。

Shared durable source: Core `Docs/DevelopmentGoldenPathGuidelines.md`, commit `a81c0389fb1834a098a462291b3f4814abcffef3`. Repository instruction: `AGENTS.md`, commit `6e33a8c4789b3d22e1b8d98901e0427d06fe636b`.

**Next action:** desktop Work/local Windows toolingで`run-runtime-tests.bat`系の非対話実行経路を実装する。rendering-dependent BadTex coverageを失わず、通常自動ゲートがデスクトップへ表示されないことを確認する。visual/debug runnerは別に保持する。

### TEST-RENDERED-PUBLISH — Isolated rendered runtime tooling (2026-10-05 JST)

**Owner:** Testing/tooling
**Status:** DONE (tooling publication); latest-main runtime regression pending

The earlier local snapshot passed Core 7/7, Environment six base Quickstarts
(58/57/54/52/3/3 assertions) and CCTO 70/70, with eight clean runtime logs
and combined exit 0. Game windows were enumerated on an independent non-visible
WinSta0 desktop while Direct3D rendering stayed enabled. The first hidden run
exposed worker-thread texture/map assertions; Core now posts those two steps
through PickleDriver and checks Unity main-thread execution. New Village Python
text reads explicitly use UTF-8. The saved launcher rerun passed after a separate
Haimatsu-review game released its DLL lock. No unrelated process was terminated.

Only this task's tooling/docs are published. Original working folders and other
local art/Def/review changes are preserved. These changes were transplanted onto
latest GitHub main, retaining its newer Core eight-scenario suite and Environment
Core-profile tests. Those newer runtime gates were not executed in the recorded
run and remain pending. No production art/Def/config change is included.

**Next action:** run the saved isolated desktop launcher against updated local
repositories to exercise the newer suites; human visual acceptance stays separate.

**Procedure:** Core Docs/IntegratedRuntimeTesting.md and
Scripts/IntegratedRuntimeDesktop/Run-AMJ-IsolatedDesktop.ps1.

### WORK-001 — Retexture implementation precedent audit

**Requested by:** author (2026-10-05 JST)  
**Owner:** ENV-010 art compatibility / implementation  
**Status:** DONE — audit reflected in Environment design and Golden Path

The earlier handoff bundled runtime testing that the author has confirmed is already finished. No runtime rerun belongs to this item.

The cross-project prior-art audit covered VTE / VTE Variations, ReGrowth 2, current MO 1.6.2.2, Clean Textures, Van's retextures, Misc. Training Medieval Retexture, Primitive/Adaptive Primitive Storage, Better Looking Plants, and another current 1.6 plant/mushroom replacement pack.

Environment decision:
- Vanilla/MO tree retextures point to AMJE-owned unique texPaths through explicit patches;
- optional MO path replacements remain guarded compatibility work;
- a tree Def is complete only after all loaded visible graphic states are audited, not merely its mature/leafless baseline;
- same-path broad packs normally cease to conflict once AMJE points the Def to a unique path;
- ReGrowth/VTE-style explicit-path competitors require final-loaded-path ownership checks where they touch the same state;
- no C# or variation framework for ordinary static tree retextures.

Durable sources:
- shared Core `Docs/RetextureImplementationGuidelines.md` — `e61a6379886d0f8a5ada8edb75a94943572f23ea`;
- Environment `Docs/Design.md` — `132544092a7552d86f85547c627c757e0412249e`;
- Environment `Docs/ArtDirection.md` — `b42781ffbab01a70cce52349d858647c3aa18fd7`;
- Environment texture Golden Path — `1656615b6cb47176d8a0cf3dd4fb07e54739e845`.

**Next action:** continue ENV-010 art in its existing priority order. Before integrating each first Vanilla/MO target, enumerate the target's current loaded graphic states and create the matching AMJE target/state regression entries.

### DEV-TOOLS-001 — DevKit / Rim Control を開発専用補助ツールとして採用

**Requested by:** author (2026-10-06 JST)  
**Owner:** Testing/tooling  
**Status:** DONE (policy documentation)

AMJ共通の対話型開発補助ツールとして、DevKit — Better Dev Mode Menu (Workshop `3814373104`) と Rim Control (Workshop `3774299554`) を採用した。

DevKitはDef検索・スポーン・Debug Action等を用いた目視確認の準備短縮、Rim Controlはゲーム中の数値・visual・placement等の一時変更によるプロトタイピングに使用する。どちらも自動テストの代替や出荷依存にはしない。

Rim Controlで得た採用値はXML / C# / Def / 正式設計書へ正本化し、上書きを無効にしてから静的検証・RimTest Redux・Pickle・runtime gateで再検証する。正式テスト、自動テスト、リリースゲート、通常プロファイルの正式確認では両ツールを無効化する。

**Result / references:** durable rule in `Docs/DevelopmentTools.md`. Documentation-only change; no new runtime PASS is claimed.

