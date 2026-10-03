# Ancient & Medieval Japan: Environment — Design

**Target:** RimWorld 1.6  
**Repository:** `sucRo-RimWorld/Ancient-Medieval-Japan-Environment`  
**Role:** standalone sister mod for Japan-oriented climate, terrain, rivers, biomes, vegetation, weather and seasonal scenery.

## 1. Responsibility boundary

Environment is independent from AMJ Core.

Environment owns:
- world-map climate;
- temperature distribution and seasonal range;
- elevation and hilliness distribution;
- coastline shape and coastal-tile frequency;
- world rivers and their size/frequency;
- biome definitions and placement;
- wild vegetation composition;
- weather/environmental regional differences;
- seasonal scenery where it affects the environment.

Environment does not own:
- cultivated crop Defs;
- crop yield, growDays, fertility sensitivity or sowing skill;
- rice paddies or agricultural production buildings;
- crop processing;
- cooking;
- animals merely for regional flavor;
- unrelated farming automation.

AMJ Core remains responsible for the minimum Terrain/Defs required for its agriculture to function. Environment may alter the *distribution/context* of land, but does not become a prerequisite for Core agriculture.

AMJ Core, CCTO, MO and other AMJ mods are optional compatibility targets, not hard dependencies.

**Harmony is the sole current technical dependency.** It is used for the post-Vanilla terrain transformation and for temperature-runtime hooks that RimWorld 1.6 does not expose cleanly through Def/XML.

## 2. Design goal

The target is not a literal GIS reconstruction of Japan and not one specific historical year.

The world should produce a **pre-Edo Japan-like environmental gameplay profile**:
- temperate to cool, humid climate;
- meaningful hot summers and cold winters;
- little true tropical or polar land;
- limited flat land;
- abundant hills and mountains;
- short, relatively frequent small-to-medium rivers;
- no naturally generated continental-scale rivers;
- long, indented coastlines with bays, peninsulas and islands;
- environmental differences that matter to agriculture, preservation and resource use.

Historical fidelity is subordinate to gameplay. Values are abstractions intended to reproduce the environmental decisions of living in the Japanese archipelago.

## 3. Alpha world-generation targets

The values below are **Alpha baselines**. They are authoritative starting values, not final balance. Runtime world-generation statistics and playtesting may adjust them.

### 3.1 Temperature

#### Annual mean temperature

Target generated land range:

- minimum annual mean: **-8 C**
- maximum annual mean: **20 C**

Typical lowland bands:

- southern warm lowland: **17–20 C**
- central temperate lowland: **12–16 C**
- northern cool lowland: **5–9 C**

High elevations may fall below the lowland bands through elevation cooling, but land annual means remain clamped to the global Alpha range.

#### Elevation cooling

Initial target lapse rate:

- approximately **6.0–6.5 C per 1000 m**

This replaces Vanilla's much broader planet-scale climate behavior with a Japan-oriented range.

#### Seasonal variation target

Seasonal amplitude should remain large enough for both summer heat and winter cold:

- warm south: approximately **±8 C**
- central region: approximately **±11–13 C**
- cool north: approximately **±15–16 C**

Exact curve values are implementation details and may be adjusted after generated-world sampling.

#### Daily temperature variation

RimWorld's actual outdoor temperature includes seasonal shift, a fixed day/night sun-cycle component, and a separate daily random variation. Because CCTO evaluates the plant's actual ambient temperature, a short cold dip can cross a plant's lethal threshold even when the seasonal mean is much warmer.

Alpha policy:

- keep the Vanilla day/night sun-cycle amplitude unchanged initially;
- reduce the separate daily random temperature variation from Vanilla's approximately **±7 C** maximum scale to approximately **±3 C**;
- if playtesting still causes excessive one-night crop deaths, adjust this random component before weakening the crop-specific CCTO thresholds.

This keeps weather variability while avoiding a continental-style random swing overwhelming the Japan-oriented seasonal climate.

### 3.2 Elevation

Target elevation domain:

- ocean floor generation floor: approximately **-300 m**
- land maximum: approximately **3800 m**

Land distribution should be strongly weighted below 1500 m.

Conceptual elevation bands:

| Elevation | Intended role |
|---|---|
| 0–200 m | coastal plain, floodplain, basin lowland |
| 200–600 m | foothills and low uplands |
| 600–1500 m | common mountain land |
| 1500–2500 m | high mountains |
| 2500–3800 m | rare alpine/high mountain tiles |

Values above 3000 m should be uncommon.

Current Alpha implementation applies additional relief only to Large Hills / Mountainous / Impassable tiles after coastline/land classification, so lowlands remain low while rare highland peaks can approach the upper range.

### 3.3 Hilliness

Initial land-tile distribution target:

| Hilliness | Target share |
|---|---:|
| Flat | 25% |
| Small Hills | 20% |
| Large Hills | 25% |
| Mountainous | 25% |
| Impassable | 5% |

This is a gameplay abstraction of a mountainous archipelago. "Mountainous country" is represented across Small Hills, Large Hills and Mountainous rather than making most settlements Mountainous/Impassable.

The target should be evaluated statistically over multiple generated worlds rather than requiring exact percentages per seed.

Current Alpha implementation uses a spatial ruggedness score (terrain noise plus elevation influence), ranks generated land tiles by that score, and maps the resulting bands toward the 25/20/25/25/5 target. This replaces the first prototype's incremental promotion rules, which overproduced Small Hills and underproduced Mountainous terrain.

### 3.4 Rivers

Japan-oriented river design:

- increase the number of river-bearing tiles;
- favor **Creek** and **River**;
- do not naturally generate **LargeRiver** or **HugeRiver**;
- retain relatively narrow map-scale rivers;
- let high rainfall and mountainous terrain create a dense network rather than one or two continental rivers.

Initial RiverDef Alpha values:

| RiverDef | spawnFlowThreshold | spawnChance | widthOnMap |
|---|---:|---:|---:|
| Creek | **30,000** | **0.70** | 4 |
| River | **75,000** | **0.80** | 6 |
| LargeRiver | **disabled for natural spawn** | — | unchanged for compatibility |
| HugeRiver | **disabled for natural spawn** | — | unchanged for compatibility |

Implementation rule:
- set LargeRiver/HugeRiver natural spawn threshold to a non-spawning value while leaving the Defs present for compatibility;
- naturally spawned rivers are therefore Creek or River only;
- do not delete the larger RiverDefs.

These values must be re-evaluated together with the Environment rainfall model because RimWorld river flow accumulates rainfall.

### 3.5 Coastline and islands

Environment should increase the proportion of **land tiles adjacent to ocean** without merely reducing total land area.

Target form:
- more bays;
- more peninsulas;
- more medium/small islands;
- more irregular coastlines;
- fewer very large uninterrupted continental interiors.

Initial statistical target:

- at the same planet coverage, **coastal land tiles ≈ 1.5× Vanilla baseline**.

This is a comparison target, not an exact per-seed guarantee.

Total land fraction should remain playable. The coastline goal must be achieved primarily by increasing coastal complexity rather than flooding large amounts of land.

## 4. Rainfall baseline

Japan-oriented generation should strongly reduce dry continental interiors.

Initial broad target for most settled land:

- approximately **800–3000 mm/year** equivalent RimWorld rainfall values.

Extremely dry biomes should become uncommon in the natural Environment profile.

Rainfall design must be tuned together with river flow because RimWorld 1.6 river generation accumulates tile rainfall.

Detailed regional rainfall/monsoon/snow-country differences are a later detailed-design section and should not be hard-coded before generated-world sampling.

## 5. Implementation boundary: XML vs C#

### XML/Def/Patch work

Prefer XML where RimWorld exposes stable Def data.

First XML-owned target:
- `RiverDef` spawn thresholds/chances.

BiomeDefs, wild plants and compatible weather/Def changes should also remain XML where possible.

### Minimal C# world-generation layer

RimWorld 1.6 hard-codes major parts of world terrain generation in `WorldGenStep_Terrain`, including:
- elevation noise and range;
- latitude-based average temperature curve;
- elevation temperature reduction;
- hilliness assignment;
- coastline-forming elevation noise.

Therefore the following Environment responsibilities cannot be implemented robustly as pure XML:
- Japan-oriented temperature envelope;
- Japan-oriented elevation distribution;
- target hilliness distribution;
- increased coastline complexity.

Implementation architecture:
- keep the Vanilla `Terrain` `WorldGenStepDef` unchanged;
- Harmony-postfix `WorldGenStep_Terrain.GenerateFresh`;
- after Vanilla terrain generation completes, Environment applies its elevation/coastline, hilliness, annual-temperature and rainfall transforms;
- Environment then re-runs natural biome selection using the adjusted tile values;
- all changes are limited to the root surface layer;
- later Vanilla world-gen steps, including rivers, roads and factions, consume the adjusted terrain normally;
- do **not** reference an Environment custom `WorldGenStep` class from XML. This avoids Def-load-time type-resolution failure and reduces conflict surface with Vanilla Def loading.

Runtime temperature hooks:
- Harmony overrides the root-surface seasonal shift to an **±8 C → ±16 C** south-to-north curve;
- Harmony scales the separate root-surface daily random variation from the Vanilla ~±7 C scale to **~±3 C**;
- the normal day/night sun-cycle component remains Vanilla for the Alpha baseline.

Use the smallest world-generation code layer that can own these calculations without unrelated runtime systems.

Do not add C# to systems that can be expressed cleanly by Def/Patch XML.

## 6. Waterfalls

True vertical waterfalls are not an Alpha requirement.

RimWorld rivers are fundamentally two-dimensional map features and do not provide a native vertical river-drop model.

A later optional feature may generate a **pseudo-waterfall landform** when conditions such as:
- River present;
- Large Hills or Mountainous;
- suitable rock/terrain layout

are met.

Such a feature may combine map generation, rock walls, water terrain, visual effects and sound. It is explicitly deferred until the core Environment world-generation loop is stable.

## 7. Compatibility principles

- AMJ Core: optional integration. Environment conditions may influence the context in which AMJ crops are chosen, but crop balance remains owned by Core.
- CCTO: optional integration. CCTO remains responsible for crop cold-death semantics. Environment does not duplicate or override those crop thresholds; it uses the published CCTO/AMJ values as climate-calibration reference points.
- ReGrowth 2: implementation/reference target, not a dependency.
- other biome/worldgen mods: compatibility should favor explicit targeted patches rather than broad destructive replacement where possible.

## 8. Alpha validation

World-generation changes are accepted based on generated-world sampling, not only XML/static checks.

For several seeds at common planet coverage values, collect at minimum:
- land annual mean temperature min/max and distribution;
- elevation min/max and distribution;
- Hilliness percentages;
- rainfall distribution;
- Creek/River/Large/Huge counts;
- percentage/count of river-bearing land tiles;
- coastal land-tile count;
- coastal land-tile ratio relative to Vanilla using the same coverage;
- biome distribution.

### 8.1 Alpha world-generation baseline acceptance

The second-pass world-generation baseline is accepted for the current Alpha after three current-DLL seeds at the same common planet coverage.

Observed ranges across the three seeds:
- same-seed coastal-land multiplier vs Vanilla: **1.43x–1.50x** (mean approximately **1.48x**);
- land-count change vs same-seed Vanilla: approximately **-0.6% to +4.4%**;
- land annual-mean temperature: approximately **-8 C to 20 C** on every seed, with whole-land averages **11.5–12.0 C**;
- land maximum elevation: **3141–3800 m**;
- land at or above 1500 m: **7.0–12.1%**;
- land at or above 2500 m: **0.4–1.73%**;
- land at or above 3000 m: rare, from approximately **0.03% to 0.55%** where fine diagnostics were available;
- Hilliness: **25 / 20 / 25 / 25 / 5%** by construction on every current seed;
- land rainfall in the expanded-diagnostic seeds: **832–2917**, with land averages **1223–1416**;
- river-bearing land-tile share in the expanded-diagnostic seeds: **7.9–8.8%**;
- generated natural river sizes: **Creek and River only**; no LargeRiver/HugeRiver.

The coastline target is therefore being met primarily through increased coastal complexity rather than large-scale land loss.

Biome histograms from validation runs are **not** treated as standalone Environment baseline values when additional biome-providing mods are active. The Environment acceptance criterion is the climate/terrain envelope and coherent biome selection; exact biome percentages remain mod-list dependent.

This locks the current terrain/coast/elevation/rainfall/river Alpha baseline. Further changes require a demonstrated gameplay or compatibility problem rather than additional tuning toward one seed.

### 8.2 CCTO / AMJ crop-climate calibration

Environment climate balance must be checked against the authoritative CCTO thresholds used by AMJ rather than against annual mean temperature alone.

Primary calibration crops:

| Crop | Active-growth minimum | Cold-death threshold | Intended climate signal |
|---|---:|---:|---|
| Rice | 10 C | -1 C | warm-season crop; winter growth stops broadly and frost is dangerous |
| Foxtail millet / Awa | 8 C | -4 C | somewhat more cold-tolerant than rice, but not a winter crop |
| Japanese barnyard millet / Hie | 5 C | -4 C | longer cool-season tolerance than Awa/rice |
| Barley | 0 C | -8 C | the main cold-tolerant annual reference |

These values are owned by CCTO/AMJ, not by Environment.

Generated-climate acceptance intent:

- **southern warm lowlands:** rice and other warm crops have a long season; winter nights may stop growth and occasionally threaten frost-sensitive crops, while barley is normally safe;
- **central temperate lowlands:** rice and millets stop growing for winter and can be killed by frost if left standing; barley normally remains the safer cold-season annual, though the colder inland edge may threaten it;
- **northern cool lowlands / highlands:** ordinary annual crops cannot remain safely exposed through winter; even barley can face lethal cold, while dormancy-capable perennial crops gain a clear role.

Validation must sample actual hourly/daily outdoor temperatures, not only the tile annual mean.

Current Alpha diagnostic implementation:
- after fresh world creation, select four non-impassable representative root-surface tiles: warm lowland, temperate lowland, cool lowland and highland;
- prefer Northern Hemisphere tiles for consistent seasonal orientation, with all-surface fallback if a representative cannot be found there;
- lowland representatives are selected below 300 m near annual means 18.5 C / 14 C / 7 C;
- the highland representative is selected between 1500–3200 m near 2 C annual mean;
- sample the public RimWorld `TileTemperaturesComp.OutdoorTemperatureAt(PlanetTile, absTick)` API once per in-game hour across all 60 days of one year (1440 samples per representative);
- because this uses the game API after Harmony initialization, samples include the Environment seasonal-amplitude hook, reduced daily random variation, and Vanilla sun-cycle component rather than a separate approximation.

For each representative, collect:
- actual yearly min/max;
- hours and equivalent days below 10 C, 8 C, 5 C and 0 C;
- hours/days below -1 C, -4 C and -8 C;
- event count, count of short events (<=6 h), and maximum continuous duration for each lethal threshold.

If a region's gameplay does not match the intended signal above, adjust Environment climate variation before changing the established CCTO crop thresholds.

First hourly calibration result with the ~±4 C random scale:
- warm lowland (annual 18.5 C): actual -0.6..37.6 C; no time below the Rice -1 C death threshold;
- temperate lowland (annual 14.0 C): actual -8.4..35.9 C; 136 h below -1 C, 70 h below -4 C, and **2 h below -8 C**;
- cool lowland (annual 7.0 C): actual -17.1..32.7 C; 195 h below -8 C;
- highland (annual 2.0 C, 1643 m): actual -21.1..25.4 C; 338 h below -8 C.

CCTO checks cold-death through the plant's normal long-tick path and kills immediately when actual ambient temperature is strictly below the configured threshold. Therefore the temperate representative's 2 h below -8 C is sufficient to threaten Barley even though the event is brief. That conflicts with the intended signal that Barley should normally remain the safer central-lowland annual.

Calibration decision:
- keep the current seasonal-amplitude curve and Vanilla ±7 C sun-cycle component;
- reduce only the separate daily random component from ~±4 C to **~±3 C** (3/7 of Vanilla);
- the re-run passed, so the ~±3 C random scale is accepted as the Alpha CCTO climate baseline.

This is the smallest change that removes a one-night random-spike failure mode in central lowlands while preserving clearly lethal Barley conditions in cool lowlands and highlands.

Gameplay smoke tests should then confirm:
- usable settlement sites still exist;
- Flat land is limited but not frustratingly rare;
- rivers are noticeably more common without dominating maps;
- no natural Large/Huge rivers are produced;
- northern/highland areas remain meaningfully colder;
- southern lowlands remain warm without creating tropical-world conditions;
- crop-season behavior follows the CCTO calibration intent rather than being dominated by isolated random temperature spikes.


## 9. Biome and vegetation architecture

### 9.1 Natural vegetation model

The Environment biome model should follow Japan's broad natural forest zonation rather than Vanilla's continental labels.

Research basis:
- Ministry of the Environment, "Vegetation of Japan": https://www.biodic.go.jp/reports/2-2/aa029.html
- the broad natural sequence is evergreen broad-leaved forest -> deciduous broad-leaved forest -> evergreen coniferous forest from warmer to colder conditions, with a similar vertical transition as elevation increases;
- the same source identifies warm-temperate evergreen broadleaf forest, cool-temperate deciduous broadleaf forest, and subarctic/subalpine evergreen conifer forest as the major broad zones.

For Alpha gameplay, use four coarse climate bands:
- **Warm-temperate forest:** annual mean >= **15 C**;
- **Cool-temperate forest:** annual mean >= **8 C** and < 15 C;
- **Subalpine forest:** annual mean >= **0 C** and < 8 C;
- **Alpine zone:** annual mean < **0 C**.

These thresholds are gameplay approximations, not literal botanical boundaries. The existing Environment temperature/elevation model already makes the same sequence occur both northward and upward.

Wetlands are treated as a moisture/topography overlay on those climate bands rather than as a separate latitude zone. Initial diagnostic candidate threshold: **swampiness >= 0.5**.

### 9.2 Biome Def strategy

Do **not** repurpose Vanilla `TropicalRainforest`, `TemperateForest`, `BorealForest` or `Tundra` into AMJ-specific biomes.

Reason:
- those Defs also own animal pools, diseases, terrain generation, weather, forage behavior and compatibility targets;
- changing their meaning would make unrelated patches and content inherit the wrong semantic assumptions;
- for example, scoring Vanilla `TropicalRainforest` as a Japanese warm-temperate forest would still retain tropical disease/animal/content assumptions unless most of the Def were replaced.

Instead:
- add AMJ-owned BiomeDefs for the accepted vegetation zones;
- use temporary Vanilla world textures during Alpha where needed; final biome artwork remains deferred until Environment mechanics and distribution are stable;
- start each biome from the nearest Vanilla gameplay profile, then replace vegetation/weather/terrain details deliberately rather than by hidden inheritance;
- keep exact animal additions outside Environment unless required for biome functionality;
- allow third-party naturally generated biomes to remain compatible through normal worker scoring rather than globally suppressing all non-AMJ BiomeDefs.

### 9.3 ReGrowth 2 reference policy

ReGrowth 2 remains an implementation/reference target, not a dependency.

Useful reference patterns:
- biome-specific wild-plant lists;
- rainfall-sensitive weather commonality;
- lightweight biome workers for optional regional sub-biomes;
- visual/seasonal atmosphere added without replacing unrelated core systems.

Do not copy ReGrowth source code, textures, sounds or other restricted assets. Implement AMJ behavior independently and use compatibility patches only when needed.

### 9.4 Validation before BiomeDef implementation

Before creating the four AMJ BiomeDefs, log the distribution of the proposed climate bands over generated land:
- WarmTemperate >=15 C;
- CoolTemperate 8..15 C;
- Subalpine 0..8 C;
- Alpine <0 C;
- swampiness >=0.5 candidates.

The target is not equal shares. Expected shape:
- cool-temperate should be the largest or one of the largest settled bands;
- warm-temperate should be common but concentrated toward warmer lowlands;
- subalpine should be meaningful but clearly smaller;
- alpine should be rare;
- wetland candidates should remain a minority.

If this shape is stable, proceed to AMJ BiomeDefs and vegetation composition. Do not tune the locked world-generation climate merely to force biome percentages.

Observed preview on the first validation seed:
- WarmTemperate: **27.5%**;
- CoolTemperate: **51.4%**;
- Subalpine: **19.5%**;
- Alpine: **1.7%**;
- swampiness >=0.5 candidates: **0.7%**.

This matches the intended shape closely enough that no climate/worldgen retuning is required.

### 9.5 Alpha BiomeDef implementation

The first AMJ-owned biome pass is implemented with four BiomeDefs:
- `AMJ_WarmTemperateForest`;
- `AMJ_CoolTemperateForest`;
- `AMJ_SubalpineForest`;
- `AMJ_AlpineZone`.

Biome workers:
- use the accepted annual-mean temperature bands;
- require rainfall >=800;
- exclude swampiness >=0.5 so existing wetland biomes continue to handle the initial 0.7% wetland minority;
- return an ordinary score centered near 38, sufficient to supersede broad Vanilla biome workers inside the AMJ climate bands without globally disabling third-party natural biomes.

Alpha content policy:
- world-map textures temporarily reuse nearest Vanilla biome textures;
- wild plants use existing Vanilla Defs only;
- wildlife pools are temporary Vanilla-backed functional placeholders so generated maps are not empty, not a historical fauna specification;
- no final textures, new plant graphics, regional weather effects, or Japan-specific wild-plant Defs are part of this pass;
- all placeholder content must be revisited after runtime distribution/map-generation validation.

The next validation gate is runtime, not further spreadsheet-style tuning: generate a world and verify that actual biome counts are dominated by the four AMJ BiomeDefs while swamp tiles still resolve to appropriate wetland/compatibility biomes.
