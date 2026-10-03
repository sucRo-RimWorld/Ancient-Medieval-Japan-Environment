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

AMJ Core, CCTO, MO and other AMJ mods are optional compatibility targets, not hard dependencies unless a later implementation proves a minimal technical dependency unavoidable.

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
- maximum annual mean: **21 C**

Typical lowland bands:

- southern warm lowland: **19–21 C**
- central temperate lowland: **12–16 C**
- northern cool lowland: **5–8 C**

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
- CCTO: optional integration. CCTO remains responsible for crop cold-death semantics.
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

Gameplay smoke tests should then confirm:
- usable settlement sites still exist;
- Flat land is limited but not frustratingly rare;
- rivers are noticeably more common without dominating maps;
- no natural Large/Huge rivers are produced;
- northern/highland areas remain meaningfully colder;
- southern lowlands remain warm without creating tropical-world conditions.
