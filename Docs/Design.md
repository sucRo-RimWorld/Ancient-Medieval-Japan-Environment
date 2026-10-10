# Ancient & Medieval Japan - Environment — Design

**Target:** RimWorld 1.6  
**Repository:** `sucRo-RimWorld/Ancient-Medieval-Japan-Environment`  
**Role:** standalone sister mod for Japan-oriented climate, terrain, rivers, biomes, vegetation, weather and seasonal scenery.

## 1. Responsibility boundary

Environment is independent from AMJ Core.

**Standalone Japan-environment contract (2026-10-09 author decision):** Environment must provide its ancient-to-medieval Japanese natural environment on the baseline RimWorld/Harmony installation **without requiring other vegetation Mods, Medieval Overhaul or optional DLC**. Its required landscapes and vegetation remain Environment-owned **even if an existing Mod already supplies equivalent species or functions**. VE, non-VE, MO and DLC comparisons inform **optional compatibility only** (duplicate spawns, natural distribution, patch load order and licenses), not removal of Environment species or migration of that scope to another Mod. The shared VE/non-VE implementation-entry audit and formal evidence record still apply, as do accepted-art, historical-description, save and runtime gates. This does not expand Environment into crops, medicinal treatments, processing or harvesting subsystems owned by other AMJ Mods.

Environment owns:
- world-map climate;
- temperature distribution and seasonal range;
- elevation and hilliness distribution;
- coastline shape and coastal-tile frequency;
- world rivers and their size/frequency;
- biome definitions and placement;
- wild vegetation composition;
- regional tree distribution and the ordinary growing-zone tree species available through that distribution;
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

### Save-compatible vegetation development order (approved 2026-10-08)

AMJEでは、作者の既存セーブで繰り返しニューゲームが必要になる変更を減らす。**ワールド／Biomeの配置・地形・海岸・河川など、生成済みワールドやマップへ遡及しない変更を優先して固める**。独自Biomeの追加や全体の再配置は既存ワールドへの完全反映を前提にせず、意図した環境を評価する場合はニューゲームを必要とする変更として扱う。既存マップ上の初期植生は、新規定義／自然発生プールを変更しても即時に再生成されない。

その後に追加できる日本在来の樹木・果樹・採集用植物・伝統薬草および既存植物のリテクスチャは、**AMJE導入済みの既存セーブへ後から適用できる設計を優先する**。

- 新規の野生樹木／果樹は、既存Biomeの `wildPlants` と追加 `ThingDef` を使い、自然発生・成長・実や木の実などの収穫を既存マップでも可能にすることを目標とする。自然発生には時間・気候・地形・密度などの条件があり、即時出現や古い植生の一括置換を保証しない。
- 通常植林できる樹木は上記の **Regional tree sowing contract**（地域内の自然分布、Vanilla樹木栽培研究、`mustBeWildToSow`等）を維持する。全樹木・全果樹を一律に植林可能にする方針ではない。
- 既存Defの画像だけを差し替えるリテクスチャは、DefName・アイテム挙動・保存データ構造を変えずに途中導入可能な形を原則とする。ゲームロジック・保存形式を変える場合は「画像のみ」と見なさず個別監査する。
- **AMJE未導入セーブへの初回導入**と、**AMJE導入済みセーブへの植物追加アップデート**は別の互換性問題である。前者は未検証で、ワールドを再生成しない。AMJEそのものの途中削除を安全と宣言しない。
- この方針は設計目標であり、将来の新規植物がすべてセーブ互換と実証済みという意味ではない。特に自然発生・実際の収穫・植林Jobが成立するかはリリース前の既存セーブ移行E2Eで確認し、未検証項目を明示する。

**回帰ゲート:** 新種・収穫機能の初回実装時に、AMJE旧版で作成したセーブを新版でロードする前後比較を自動化する。ロード済みDef／Biomeプール、既存マップの保持、経時の自然発生、成熟後の果実・木の実の取得、研究前後の植林候補と実際の植林、セーブ・再ロード、所有ERRORゼロを確認する。静的なDef／メニュー判定だけで「既存セーブへ途中導入可能」と判定しない。基本／MO／CCTO／MO+CCTOの互換プロファイルを区別し、ゲーム起動テストは既存の非対話ランナー・ログゲートを使う。テスト仕様・実装状況は `Docs/GoldenPaths/PlantSowingTests.md` に置く。**現時点で移行E2Eは未実装・未通過**。

### Regional tree sowing contract

樹木の地域分布と、それに連動する通常の植林可能樹種はEnvironmentの責務とする。作物栽培の所有とは分け、AMJE樹木もVanillaの樹木栽培研究後に、その樹種が自然分布する地域で植林できるものとする。

`TreeBase` / `DeciduousTreeBase` 由来の `sowTags=Ground`、`sowResearchPrerequisites=TreeSowing`、`mustBeWildToSow=true` を保持する。AMJE管理Biomeでは、自然植生から除外した通常樹木を植林候補にも残さない。Vanilla Def自体の削除や、他Mod所有Biomeでの植林制限は行わない。

| AMJE Biome | 樹木栽培研究後の通常植林候補 |
|---|---|
| `AMJ_WarmTemperateForest` | `AMJ_Tree_Shii`, `Plant_TreeMaple`, `Plant_TreeBamboo` |
| `AMJ_CoolTemperateForest` | `AMJ_Tree_Beech`, `Plant_TreeOak`, `Plant_TreeMaple`, `Plant_TreeBirch`, `Plant_TreePine` |
| `AMJ_SubalpineForest` | `AMJ_Tree_Shirabiso`, `Plant_TreeBirch` |
| `AMJ_AlpineZone` | 通常樹木なし |

`AMJ_Shrub_Haimatsu` は自然低木・伐採資源として残し、植林候補には含めない。自然生成される木本と農業ゾーンから植林可能な通常樹木は、関連するが**同一集合ではない**。RimWorld 1.6ではハイマツが `plant.IsTree` として数えられるため、高山帯の自然木本期待集合は `{AMJ_Shrub_Haimatsu}`、通常植林候補は空集合とする。実行時Quickstartsは両者を別々に検証し、研究のロック／解除とハイマツの植林不可も確認する。候補変更時はBiome、上表、静的契約テスト、ロード後の両期待値を同じ変更で同期する。

回帰手順と静的／実行時検証の区別は [PlantSowingTests](GoldenPaths/PlantSowingTests.md) を参照。実際のメニュー判定はGround適合だけでなく `PlantUtility.ValidPlantTypesForGrowers` と `Command_SetPlantToGrow.IsPlantAvailable` の両段階を使い、ロード後の継承・研究・野生分布条件を検証する。

AMJ Core remains responsible for the minimum Terrain/Defs required for its agriculture to function. Environment may alter the *distribution/context* of land, but does not become a prerequisite for Core agriculture.

Environment remains technically standalone: AMJ Core, CCTO and Medieval Overhaul are not hard dependencies.

However, the **normal AMJ play configuration is expected to coexist with Medieval Overhaul (MO)**. MO compatibility is therefore a first-class design requirement rather than an incidental third-party compatibility case. Environment must not unnecessarily suppress, replace or invalidate MO-owned biomes, plants, terrain or other environmental content when both mods are active.

**Harmony is the sole current technical dependency.** It is used for the post-Vanilla terrain transformation and for temperature-runtime hooks that RimWorld 1.6 does not expose cleanly through Def/XML.

### Public replacement scope

AMJE replaces and reconfigures the generated Vanilla terrain, vegetation, and baseline biome composition for a Japanese-archipelago-inspired environment. This describes the generated result, not wholesale deletion of Vanilla Defs or replacement of the engine pipeline. `EnvironmentTerrainProcessor.Apply` transforms root-surface elevation/coastlines, hilliness, climate and biome selection after Vanilla terrain generation. The four AMJE biome workers take precedence over broad Vanilla workers on eligible land, while wetlands and stronger compatible specialized workers can remain. The current Beta biome mixes still reuse some Vanilla vegetation provisionally alongside AMJE representative plants, but Vanilla vegetation has no default retention right: each reused plant must be re-audited and removed/replaced unless its Japanese distribution, period fit, ecological role, and presentation justify retention. Natural soil distribution is rebalanced without replacing every TerrainDef. Local River / Coast mutators and Vanilla weather/snow/deciduous systems remain the implementation foundation. Public summaries must state both the changed environment and these reuse/coexistence boundaries.

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

- **Medieval Overhaul (MO): standard AMJ coexistence target.** Environment does not require MO to load, but normal AMJ usage assumes both are present. MO-owned environmental content, including specialized natural biomes such as Dark Forest, should remain available where compatible with Environment climate/worldgen. Conflicts should be solved by targeted compatibility rules rather than by making AMJ biomes globally authoritative.
- AMJ Core: optional technical integration. Environment conditions may influence the context in which AMJ crops are chosen, but crop balance remains owned by Core.
- CCTO: optional technical integration. CCTO remains responsible for crop cold-death semantics. Environment does not duplicate or override those crop thresholds; it uses the published CCTO/AMJ values as climate-calibration reference points.
- ReGrowth 2: implementation/reference target, not a dependency.
- other biome/worldgen mods: compatibility should favor explicit targeted patches rather than broad destructive replacement where possible.

## 8. Alpha validation

World-generation changes are accepted based on generated-world sampling, not only XML/static checks.

Automated validation is layered by responsibility:
- static/XML/build checks remain the first gate;
- RimTest Redux covers deterministic Environment logic/calculation boundaries in a separate test-only assembly;
- RimWorks Pickle covers loaded-source and loaded-Def integration in Vanilla, MO, CCTO and MO+CCTO development profiles;
- rendering-enabled Quickstarts remain authoritative for generated-world/map, terrain/rendering and native-job regressions;
- the downloaded-Workshop Pickle gate remains separate because publication provenance cannot be established from the development root.

The reusable development procedure is `Docs/GoldenPaths/FrameworkRuntimeTests.md`. Neither RimTest Redux nor Pickle is a production dependency of Environment.

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
| Foxtail millet / Awa | 8 C | -3 C | somewhat more cold-tolerant than rice, but not a winter crop |
| Japanese barnyard millet / Hie | 5 C | -2 C | longer cool-season growth window than Awa/rice, but not especially frost-hardy |
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

The gameplay-validation split is automation-first:

**Automated regression must confirm whenever the condition is mechanically observable:**
- usable target-biome/map generation succeeds and the runtime gate completes without owned ERROR entries;
- accepted Hilliness, river/coast, soil-fertility and vegetation distributions remain inside their locked ranges;
- no natural Large/Huge rivers are produced;
- the representative annual climate samples preserve a warm-lowland -> temperate -> cool -> highland gradient, including increasing hours below crop-relevant 8 C and 0 C thresholds;
- when AMJ Core is installed, the four fixed-biome Quickstarts load the Stage A crop Defs and prove that the generated natural fertility ladder creates real crop-choice differences: Soba/Barley can use Thin Soil, Wheat cannot, and fertility sensitivity produces distinct average growth factors;
- crop/climate integration remains differentiated rather than collapsing to one effective growing window or one universally equivalent soil response.

**Manual playtesting is reserved for conditions that are not adequately reducible to a pass/fail metric:**
- whether Flat land feels frustratingly rare despite meeting the numerical distribution target;
- whether the visible river frequency feels intrusive despite staying within the accepted map/world metrics;
- whether the automated crop-choice differences produce interesting decisions rather than busywork;
- visual coherence, readability and overall seasonal atmosphere.

The rule is therefore: do not use a manual gameplay smoke test merely to reconfirm a condition that can be asserted from generated maps, loaded Defs, annual temperature samples or deterministic runtime output. Manual review begins after those automated gates pass.


### 8.3 Core + Environment gameplay-contract runtime gate

When a local AMJ Core installation is available, `run-runtime-tests.bat` creates an additional isolated **Core + Environment** profile and runs the four fixed-biome Quickstarts with real Core crop Defs loaded alongside Environment.

The integration gate is not a subjective playtest. It automatically checks:
- Core Stage A crop Def availability under the real combined mod stack;
- presence of the Environment natural fertility ladder on each generated map;
- actual Thin Soil coverage creating a sowability gap between low-fertility-tolerant crops and Wheat;
- map-weighted fertility growth factors remaining different across Soba, Kibi, Awa, Hie and Barley;
- Core cold-growth thresholds remaining differentiated;
- representative annual climate diagnostics retaining the accepted warm-to-cold gradient;
- no Environment- or Core-origin runtime ERROR entries.

The log records the generated terrain-cell counts, sowable-cell counts and average fertility factors so later balance changes can be compared without relying on screenshots or memory.

Future automation should extend this same contract to season-length / expected-harvest opportunity and long-run resource throughput when those can be measured robustly from the actual RimWorld APIs. Such metrics should be added only when they model the real game closely enough to avoid replacing one subjective judgment with a misleading synthetic score.

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
- allow third-party naturally generated biomes to remain compatible through normal worker scoring rather than globally suppressing all non-AMJ BiomeDefs;
- treat the four AMJ biomes as **baseline vegetation bands**, not an exclusive world-biome replacement. Specialized biomes from Medieval Overhaul and other compatible mods should be able to replace part of a matching climate band when their own workers score higher.

### 9.3 ReGrowth 2 reference policy

ReGrowth 2 remains an implementation/reference target, not a dependency.

Useful reference patterns:
- biome-specific wild-plant lists;
- rainfall-sensitive weather commonality;
- lightweight biome workers for optional regional sub-biomes;
- visual/seasonal atmosphere added without replacing unrelated core systems.

Do not copy ReGrowth source code, textures, sounds or other restricted assets. Implement AMJ behavior independently and use compatibility patches only when needed.

### 9.4 Tall Grass: Hidden Danger reference policy

Tall Grass: Hidden Danger is a **prior-art/reference candidate** for dynamic vegetation and ground-use feedback. It is not a dependency and should not be copied wholesale.

Reference-worthy patterns:
- vegetation state changing in response to repeated pawn traffic;
- trampled vegetation and emergent footpath-like ground traces;
- recovery/regrowth after traffic pressure is removed;
- using local vegetation change to make settlement surroundings reflect actual human activity.

Scope limits for AMJ Environment:
- prioritize ecological/visual feedback around settlements and traveled ground;
- do **not** treat Tall Grass's combat concealment model, including very high/100% cover-style behavior, as an AMJ target;
- any future implementation must be independently designed and should remain lightweight enough for large colonies and long-running saves;
- avoid duplicating another mod's full terrain or plant system when a small AMJ-owned mechanic or compatibility layer would achieve the intended environmental effect.

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
- return an ordinary score centered near 38, sufficient to establish the AMJ baseline over broad Vanilla workers while intentionally allowing stronger specialized workers from MO or other compatible mods to coexist.

Alpha content policy:
- world-map textures temporarily reuse nearest Vanilla biome textures;
- wild plants use existing Vanilla Defs only;
- wildlife pools are temporary Vanilla-backed functional placeholders so generated maps are not empty, not a historical fauna specification;
- no final textures, new plant graphics, regional weather effects, or Japan-specific wild-plant Defs are part of this pass;
- all placeholder content must be revisited after runtime distribution/map-generation validation.

The next validation gate is runtime, not further spreadsheet-style tuning: generate a world and verify that the four AMJ baseline biomes occupy substantial shares, swamp tiles still resolve to wetland/compatibility biomes, and specialized mod biomes can coexist without preventing the AMJ climate bands from appearing.

First runtime distribution with Medieval Overhaul active:
- AMJ WarmTemperate: **27.0%**;
- AMJ CoolTemperate: **43.7%**;
- AMJ Subalpine: **7.2%**;
- AMJ Alpine: **0.3%**;
- MO Dark Forest: **16.3%**;
- Temperate Swamp: **2.0%**;
- remaining Vanilla tropical/temperate biomes: small residual shares.

This is accepted as a valid coexistence result rather than treated as an AMJ-band failure. MO Dark Forest is a specialized naturally generated biome with its own score rules and is allowed to replace part of the otherwise suitable AMJ cool/subalpine climate space. Exact AMJ biome percentages are therefore mod-list dependent and are not balance targets.


### 9.6 Japan-specific structural wild vegetation

After the biome-placement and natural-soil gates passed, Environment moves from Vanilla-only vegetation placeholders to a deliberately small Japan-specific structural set.

Research basis:
- Ministry of the Environment vegetation classification identifies warm-temperate forests with shii/oak evergreen broadleaf communities such as Sudajii and tabu forests, cool-temperate forests with beech/mizunara communities, subalpine forests with evergreen conifers such as shirabiso/oshirabiso/kometsuga, and alpine vegetation with haimatsu scrub and alpine grassland;
- historical-vegetation research also shows that human activity from the Yayoi period through the medieval period increased pine, grassland and secondary-forest signatures in many settled regions. AMJ should therefore not render every lowland forest as an untouched climax forest.

Sources:
- https://www.biodic.go.jp/reports/3-4/hyo/c090.html
- https://www.biodic.go.jp/reports/2-2/aa029.html
- https://www.ffpri.go.jp/labs/prdb/sirabiso.html
- https://www.biodic.go.jp/reports2/5th/vgt_en/5_vgt_en.pdf
- https://www.jstage.jst.go.jp/article/jaqua/advpub/0/advpub_62.2211/_article/-char/en

Alpha structural set:

| Climate band | AMJ plant | Role |
|---|---|---|
| Warm-temperate | `AMJ_Tree_Shii` / Sudajii | evergreen broadleaf canopy dominant |
| Cool-temperate | `AMJ_Tree_Beech` / Japanese beech | deciduous broadleaf canopy dominant |
| Subalpine | `AMJ_Tree_Shirabiso` / shirabiso fir | evergreen conifer canopy dominant |
| Alpine | `AMJ_Shrub_Haimatsu` / haimatsu dwarf pine | low alpine scrub dominant above treeline |

Scope rules:
- the four AMJE structural plants remain the initial minimum custom set, but generic grass, moss, brambles, bushes, and secondary trees are **not** permanently delegated to Vanilla by default;
- current reuse of Vanilla oak/maple/birch/pine/bamboo and other filler vegetation is a Beta implementation shortcut, not a historical-design justification. Before the post-Beta vegetation/art pass, audit every reused plant for Japanese distribution, pre-Edo period fit, ecological/landscape role, and whether its Def/description/graphic can represent the intended Japanese vegetation without distortion;
- only plants that pass that audit remain in AMJE biome pools. Others are removed, replaced by a more suitable AMJE plant, or left outside AMJE-controlled biome vegetation;
- add no new food, medicine, fiber or other harvested item types in this pass. Gatherable-resource design belongs to the relevant AMJ resource/gathering systems and should be coordinated separately;
- 2026-10-06 wood-yield update: Haimatsu provides a small amount of existing wood (base yield 8), while retaining its low BushBase form. The other base yields are Shii42/Beech40/Shirabiso30. Vanilla yields WoodLog; MO's enabled wood-chain patch substitutes DankPyon_RawWood. This adds no new resource Def. Verify actual cutting outputs with `Docs/GoldenPaths/PlantHarvestTests.md`.
- the three AMJ trees yield ordinary Vanilla wood only;
- haimatsu is a low shrub rather than a normal timber tree, preserving alpine timber scarcity;
- all four new plants use Vanilla graphics as temporary placeholders. Final plant artwork remains deferred until vegetation distribution and gameplay are accepted.

Initial wild-plant commonality:
- Warm-temperate: Shii **2.0**, with Oak 0.4, Poplar 0.15, Maple 0.25 and Bamboo 0.5 retained as secondary components;
- Cool-temperate: Beech **1.8**, with Oak 0.6, Maple 0.8, Birch 0.6 and Pine 0.35 retained;
- Subalpine: Shirabiso **2.6**, Pine 0.9 and Birch 0.5;
- Alpine: Haimatsu **1.3**, with full-size Pine/Birch reduced to 0.02 each.

Automated runtime map validation is the acceptance gate for this Alpha vegetation set. The four fixed-biome Quickstarts run against an isolated profile and verify target-biome identity, target structural-plant generation, dominance over configured secondary tree species, Alpine Haimatsu coverage <=5% of all cells, Alpine full-size Pine+Birch coverage <=1% of all cells, complete live log capture, no Environment-origin live ERROR entries, and **zero Quickstarts pre-launch ERROR entries**.

A later report audit found that an earlier nominal Quickstarts PASS was incomplete: all vegetation assertions passed, but every report contained `preLaunchErrors=2`. The corresponding startup log entries were AMJE-owned XML inheritance errors on Shirabiso and Haimatsu `visualSizeRange`: the inherited parent FloatRange text and child min/max nodes were being merged into one invalid node. The two child ranges now explicitly use `Inherit="False"`, and the runtime runner now fails on any nonzero `preLaunchErrors` in the isolated profile.

A clean rerun after those fixes passed in full. All four standalone biome reports have `passed=true`, `failed=0`, `preLaunchErrors=0`, complete live log capture, and no ERROR-level runtime entries. The current measured structural shares are Warm Shii **3.14%**, Cool Beech **3.22%**, Subalpine Shirabiso **1.29%**, and Alpine Haimatsu **0.68%**; Alpine full-size Pine+Birch remains only **0.03%**. The Alpha structural vegetation composition/commonality values are therefore accepted without retuning. Final plant artwork remains deferred to the later visual-art pass.

#### Optional CCTO integration ownership

Cold-tolerance compatibility for AMJE-owned plants is owned by **AMJE**, not by the CCTO repository.

Product/balance positioning:
- **AMJE standalone** must be a complete and enjoyable "medieval Japan-like environment" mod for players who want the geography, climate, terrain, biomes and vegetation without adopting CCTO's stricter crop/plant cold simulation;
- **AMJE + CCTO** is the higher-realism configuration. CCTO adds species-specific minimum-growth temperatures plus cold death/dormancy behavior, making seasonal and regional plant survival more demanding;
- CCTO is therefore an optional realism layer, not a missing piece required to make AMJE function.

Dependency direction:
- AMJE does **not** require CCTO;
- CCTO does **not** need to know that AMJE exists;
- when CCTO is absent, AMJE plants use ordinary RimWorld plant behavior;
- when CCTO is present, AMJE conditionally patches only its own PlantDefs to use CCTO's public `ColdToleranceExtension`.

This keeps the two mods loosely coupled and allows either mod to be updated or used independently.

Temperature layers:

| Plant | AMJE without CCTO: min growth | AMJE + CCTO: min growth | CCTO cold response |
|---|---:|---:|---|
| Sudajii / `AMJ_Tree_Shii` | 0°C | 8°C | death below -8°C |
| Japanese beech / `AMJ_Tree_Beech` | 0°C | 5°C | cold dormancy |
| Shirabiso / `AMJ_Tree_Shirabiso` | 0°C | 0°C | death below -35°C |
| Haimatsu / `AMJ_Shrub_Haimatsu` | 0°C | 0°C | death below -35°C |

The standalone 0°C values are explicit even though RimWorld's `PlantProperties.minGrowthTemperature` default is also 0°C. This makes the Vanilla-style baseline a deliberate AMJE value rather than an accidental inherited default.

The CCTO values are Alpha gameplay values on CCTO's existing scale, not claims of exact physiological lethal temperatures. Beech follows the same deciduous dormancy model as CCTO Oak/Birch/Maple; Shirabiso follows the CCTO Pine/Great Fir subalpine-conifer model; Haimatsu uses the same strong evergreen frost tolerance without inventing a new lower threshold solely for its alpine label. Sudajii uses a warmer 8°C growth threshold and a conservative -8°C lethal threshold appropriate to the warm-temperate role.

Implementation is isolated in `Patches/Compatibility/CCTO.xml` with `MayRequire="sucro.cropcoldtoleranceoverhaul"`. AMJE's base PlantDefs never contain a CCTO class reference.

Automated validation must cover both configurations:
1. AMJE alone: all four loaded PlantDefs remain at the explicit 0°C baseline and no CCTO dependency is required;
2. AMJE + CCTO: the four loaded PlantDefs expose the table values above and exactly one `ColdToleranceExtension` each.

The normal four-biome runtime gate remains CCTO-free. If CCTO is installed locally, the one-command runtime gate additionally creates a second isolated AMJE+CCTO profile and runs one focused loaded-Def compatibility Quickstart.



## 10. Natural soil fertility

### 10.1 Responsibility and purpose

Environment owns **naturally generated soil quality**. Core/MO own player-created agricultural improvements such as paddies, plowed soil and other cultivated terrain.

The goal is not to make farming uniformly worse. Low-fertility terrain should create meaningful land-use and crop-selection differences:
- fertile lowlands remain valuable;
- ordinary soil remains the common baseline;
- stony/poor ground makes low-fertility-tolerant crops more useful;
- highland regions become progressively harder to cultivate without making them completely sterile.

### 10.2 Existing terrain reuse

Do not duplicate Vanilla/MO terrain roles unnecessarily.

Relevant existing values:
- `SoilRich`: fertility **1.40**;
- `Soil`: fertility **1.00**;
- `Gravel` / stony soil: fertility **0.70**;
- `Sand`: fertility **0.10** and no `GrowSoil` affordance;
- MO `DankPyon_PlowedSoil`: fertility **1.25**, a player-created agricultural improvement and therefore outside Environment's natural-terrain responsibility.

Because Gravel already provides a 0.70 low-fertility step, Alpha adds only **one** additional natural growable terrain rather than creating multiple nearly identical poor-soil Defs.

### 10.3 AMJ Thin Soil

`AMJ_ThinSoil`:
- fertility: **0.50**;
- growable (`GrowSoil`);
- normal natural-ground building affordances retained so fertility is the main gameplay distinction rather than construction prohibition;
- slightly higher path cost than ordinary Soil/Gravel;
- placeholder appearance reuses/tints a Vanilla natural texture; final terrain art is deferred.

Do not add a separate 0.40 "very poor soil" during Alpha. The current useful ladder is:

`Thin Soil 0.50 -> Gravel 0.70 -> Soil 1.00 -> Rich Soil 1.40`.

Add another tier only if playtesting demonstrates a distinct gameplay role.

### 10.4 Accepted Alpha biome distribution thresholds

RimWorld map generation creates a local fertility-noise field and resolves `BiomeDef.terrainsByFertility` against that value. Environment uses this existing XML mechanism; no custom map-generation C# is required for the terrain placement itself.

Accepted Alpha thresholds:

| Biome | Thin Soil | Gravel | Soil | Rich Soil |
|---|---:|---:|---:|---:|
| Warm-temperate | <0.30 | 0.30-0.50 | 0.50-0.87 | >=0.87 |
| Cool-temperate | <0.35 | 0.35-0.55 | 0.55-0.87 | >=0.87 |
| Subalpine | <0.50 | 0.50-0.70 | 0.70-0.92 | >=0.92 |
| Alpine | <0.65 | 0.65-0.90 | >=0.90 | none |

These are generation-noise thresholds, not terrain fertility values. They intentionally make lower-quality soil more common as the climate/elevation band becomes harsher.

### 10.5 Medieval Overhaul coexistence

Because MO is the standard AMJ coexistence target, `DankPyon_DarkForest` receives a targeted compatibility patch to use the same natural-soil ladder while remaining entirely MO-owned in biome identity, plants, animals, weather and special content.

Dark Forest initial thresholds:
- Thin Soil <0.40;
- Gravel 0.40-0.60;
- Soil 0.60-0.90;
- Rich Soil >=0.90.

This compatibility patch does **not** replace the MO biome or its terrain patch makers.

### 10.6 Runtime validation

During Alpha, fresh maps in the four AMJ biomes and MO Dark Forest log:

`[AMJ Environment] Map terrain summary`

with shares for Thin Soil / Gravel / Soil / Rich Soil / other terrain.

Acceptance intent:
- Warm/Cool maps retain substantial ordinary Soil and some Rich Soil;
- Thin Soil is visible but not dominant in ordinary lowland forest;
- Subalpine shows a clear shift toward Thin Soil + Gravel;
- Alpine is dominated by poor/stony ground but still has limited growable patches;
- MO Dark Forest continues to generate correctly with its own special terrain patches;
- thresholds are tuned from map-generation results, not by changing terrain fertility values to force a desired percentage.

Fixed-biome Alpha validation results:

| Biome | Thin Soil | Gravel | Soil | Rich Soil | Other |
|---|---:|---:|---:|---:|---:|
| Warm-temperate | 11.3% | 35.5% | 27.4% | 3.2% | 22.5% |
| Cool-temperate | 16.3% | 37.8% | 29.8% | 4.1% | 12.0% |
| Subalpine | 16.9% | 16.5% | 4.8% | 0.8% | 61.0% |
| Alpine | 61.6% | 24.4% | 2.6% | 0.0% | 11.4% |
| MO Dark Forest | 8.9% | 14.0% | 6.4% | 18.1% | 52.6% |

Interpretation:
- among the four fertility-ladder terrains only, Subalpine is approximately **43.3% Thin / 42.3% Gravel / 12.4% Soil / 2.0% Rich**;
- among the four fertility-ladder terrains only, Alpine is approximately **69.6% Thin / 27.5% Gravel / 3.0% Soil / 0% Rich**;
- Subalpine's large Other share is overwhelmingly natural rough rock: `Limestone_Rough` 34.2% of all cells and `Slate_Rough` 26.1%, together accounting for about 99% of its Other category;
- Alpine's Other share is likewise mostly rough rock/natural wall terrain, not unexpected fertile ground;
- MO Dark Forest intentionally retains its own terrain patch makers. Its large `MossyTerrain`, `SoilRich`, rough-rock, mud and shallow-water shares therefore preserve MO biome identity rather than indicate a failed Environment threshold patch.

Result: the Alpha natural-soil thresholds above are accepted without numerical retuning. Further changes require a concrete gameplay or compatibility finding rather than preference from a single terrain percentage.

Existing maps are not retroactively rewritten; the natural-soil pass applies when generating new maps.

## 11. Regional weather baseline

### 11.1 Goal and scope

Environment owns regional weather differences, but Alpha should reuse RimWorld's existing weather system before adding custom weather types or seasonal C#.

Research basis:
- Japan has a humid climate with strong seasonal contrasts; early summer Baiu brings a cloudy/rainy period across most of the country;
- September rainfall is often increased by the autumn rain front and tropical cyclones;
- winter precipitation differs strongly by exposure: the Sea of Japan side and mountains receive frequent/heavy snow, while the Pacific side is often drier and sunnier.

References:
- https://www.data.jma.go.jp/cpd/longfcst/en/tourist.html
- https://www.data.jma.go.jp/cpd/longfcst/en/tourist_baiu.html
- https://www.data.jma.go.jp/cpd/longfcst/en/tourist_japan.html
- https://www.jma.go.jp/jma/kishou/know/kisetsu_riyou/tenkou/Average_Climate_Japan.html

These sources define the climatic shape, not literal RimWorld probabilities. Weather commonalities are gameplay abstractions.

### 11.2 Reuse Vanilla weather mechanics

Do not add AMJ-specific WeatherDefs during Alpha.

Use the existing eight Vanilla entries:
- Clear;
- Fog;
- Rain;
- DryThunderstorm;
- RainyThunderstorm;
- FoggyRain;
- SnowGentle;
- SnowHard.

RimWorld 1.6 already supplies two useful environmental filters:
- weather commonality is multiplied by each WeatherDef's `commonalityRainfallFactor`, so wetter AMJ world tiles naturally receive more rain/fog/snow where the Vanilla WeatherDef defines that curve;
- Rain / RainyThunderstorm / FoggyRain require non-freezing outdoor temperature, while SnowGentle / SnowHard require freezing outdoor temperature. The accepted AMJ seasonal temperature curve therefore converts part of winter precipitation from rain to snow without Environment adding another seasonal selector.

Vanilla does **not** natively apply month/season-specific commonality curves for Baiu, Akisame or typhoon season. Alpha will not add a Harmony patch solely for calendar-specific weather weighting. Add such a layer only if later gameplay shows that the temperature/rainfall-driven baseline fails to create meaningful seasonal weather.

Likewise, Alpha does not attempt a Sea-of-Japan-side versus Pacific-side winter precipitation split. The current four AMJ vegetation bands encode temperature/elevation, not mountain exposure or ocean-facing aspect; fabricating that distinction from the biome name would be misleading. A later coast/orography feature may provide a proper basis if the gameplay value justifies it.

### 11.3 Accepted Alpha base commonalities

| Biome | Clear | Fog | Rain | Dry thunderstorm | Rainy thunderstorm | Foggy rain | Gentle snow | Hard snow |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Warm-temperate | 16 | 1.5 | 3 | **0.1** | 1.5 | 1.5 | 2 | 1 |
| Cool-temperate | 16 | 1.5 | 3 | **0.1** | 1.5 | 1.5 | 4 | 3 |
| Subalpine | 16 | 1 | 2 | **0.1** | 1 | 1 | 7 | 7 |
| Alpine | 16 | 1 | 1 | **0.05** | 0.5 | 0.5 | 12 | 12 |

Interpretation:
- warm/cool forest keeps the strongest liquid-precipitation and fog baseline;
- subalpine/alpine progressively shift commonality toward snow as temperature falls;
- dry thunderstorms remain possible for gameplay variety but are deliberately rare in the humid-Japan baseline;
- rainy thunderstorms are always more common than dry thunderstorms;
- higher AMJ tile rainfall further amplifies precipitation through Vanilla WeatherDef curves, so the table should not be interpreted as final raw percentages.

`RainyThunderstorm` is only a broad severe-rain/storm proxy. Alpha does not claim to simulate a literal typhoon.

### 11.4 Automated validation

The existing four fixed-biome Quickstarts also validate weather.

For each AMJ biome the runtime gate must confirm:
- exactly the eight accepted Vanilla weather entries are loaded in the isolated profile;
- all base commonality values equal the table above;
- rainy-thunderstorm commonality exceeds dry-thunderstorm commonality;
- no pre-launch ERROR and no Environment-origin runtime ERROR occurs.

The Quickstarts log `[AMJ Environment Weather]` with the loaded commonality table for each biome. This is a Def/runtime integrity gate, not a year-long stochastic weather-frequency test.

A statistical simulation should only be added if later weather changes introduce calendar-specific or dynamic weighting that cannot be proven from loaded Def values.

The first post-change automated runtime rerun passed cleanly in all four AMJ biomes: every standalone report returned `passed=true`, `failed=0`, `preLaunchErrors=0`, and `logErrors=0`. Loaded weather commonalities matched the table exactly in WarmTemperate, CoolTemperate, Subalpine, and Alpine. The optional AMJE+CCTO run also passed all **39/39** assertions with the same clean startup/runtime conditions, confirming that CCTO does not alter the weather baseline.

The Alpha regional weather baseline is therefore accepted.

## 11.5 AMJ共通リテクスチャ方針 — Environment所有範囲

AMJでいう**リテクスチャ**は、Environment独自Defの画像制作だけを指さない。**Environmentが自分の責務範囲で使用・再利用するVanilla / Medieval Overhaul等の前提Mod資産についても、AMJE追加資産と並べた際に画風・輪郭・色数・陰影・情報密度・解像感が統一されるよう、必要なテクスチャをEnvironment側から差し替えること**を含む。

目的は前提Mod全体を日本風へ変換することではなく、**Environmentを導入したとき、樹木・植物・地形・自然景観が一つのAMJアートセットとして成立すること**である。

### 11.5.1 所有原則

- **Environmentは、環境景観の責務に属する前提Mod資産の画風統一までを原則として所有する。**
- 対象には、Environmentが景観構成に採用するVanilla / MOの樹木・植物・地形・自然物等が含まれる。
- Coreの作物・食材・加工設備等、Core側の責務に属する前提資産はCoreが所有し、Environmentへ集約しない。
- **同一Def / 同一前提資産のテクスチャを複数AMJ Modが競合して上書きしない。AMJ内で1資産1所有Modを原則とする。**
- 所有先が曖昧な場合は、その資産の**主要なゲーム上・景観上の責務を持つMod**を正本とし、ロード順で勝たせる運用はしない。
- 前提資産をリテクスチャするという理由だけで、Retexture専用Modへ集約しない。複数AMJ Modから横断的に利用され自然な1所有Modを決められない場合、または独立外観パックとして単独導入価値が生じた場合だけ分離を再検討する。

### 11.5.2 実装方式と変更範囲

技術実装・既存リテクスチャMod監査・競合規則は、Projectの共通正本 [`Docs/RetextureImplementationGuidelines.md`](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/RetextureImplementationGuidelines.md) に従う。

Environmentでは特に以下を固定する。

- Environmentが所有するVanilla / MO樹木は、**AMJE固有texPathへ明示的にPatchする方式を標準**とし、元Modと同名のtexture pathを置いてロード順だけで勝たせる方式には依存しない。
- Vanilla樹木のPatchはEnvironment所有のretexture patchへ分離する。MO樹木はMO package IDでguardしたoptional compatibility/retexture patchへ分離する。
- `graphicData/texPath`だけでなく、loaded Defに存在する `leaflessGraphicPath` / `immatureGraphicPath` / `pollutedGraphicPath` / snow overlay系等の**実際に使用される全graphic state**を監査する。ArtDirection上の成木/落葉一覧は最低対象であり、総PNG数の固定値ではない。
- VTE / ReGrowth等が同じDef fieldを明示Patchする場合、AMJE所有対象では最終loaded pathがAMJE pathになることを保証する。同名ファイルだけを置くtexture packは、AMJEが固有pathへ切り替えた後は原則として直接競合しない。
- static retextureにC#やvariation frameworkを追加しない。動的表示が本当に必要な別機能だけを例外とする。

- 純粋なリテクスチャでは、対象Defの成長、分布、収量、耐寒性、カテゴリ、ゲームロジック等を変更しない。必要なPatchはgraphic / texPath等の表示差し替えに限定する。
- ゲームプレイ変更も必要な場合は、リテクスチャに便乗させずEnvironmentの通常仕様として別途設計する。
- Vanilla / MOのDefNameや外部参照は可能な限り維持し、前提Modとの互換性を壊さない。
- **全てのVanilla / MO環境画像を機械的に描き直すことは目的にしない。** AMJE追加資産と並べて明確に浮く、頻繁に表示される、地域景観の統一感へ大きく影響する資産を優先する。

### 11.5.3 時代・地域考証

- リテクスチャも通常のAMJ時代・地域考証対象とする。
- 画風統一だけなら元の植物・地形等の同一性を保つ。形態や種類の見せ方を日本向けに変更する場合は、AMJ対象時代・対象地域に存在する姿として妥当か確認する。
- 「日本らしい」という理由だけで時代外・地域外の意匠や植生へ変更しない。
- 元Defと別種・別物に見えるほどの変更が必要な場合は、単純リテクスチャではなく新Def・互換Patch・分布設計等を含めて責務を再検討する。

### 11.5.4 他Modとの境界と開発順

- **Japan OnlyはEnvironmentのリテクスチャを所有しない。** Japan OnlyはMO由来の西洋要素の除去・非表示化だけを担当する。
- 専用Retexture Modは標準構成では作らない。1資産1所有Modで整理できない横断案件が生じた場合だけ再検討する。
- 外部Modの配布テクスチャを複製・改変再配布するのではなく、Environmentが権利上問題のない独自テクスチャを所有し、Patchから参照する。
- 機能・分布検証が不安定なAlpha段階ではプレースホルダーを許容するが、**Environmentの最終アート工程には、AMJE独自植物だけでなく必要なVanilla / MO前提資産のリテクスチャも含める。**
- 既存のENV-010対象となっているVanilla/MO樹木リテクスチャは、このEnvironment所有原則に基づく正式な作業であり、別Retexture Modへ移管しない。

### 11.5.5 既存樹木の再精査と説明統一

既存Vanilla / Medieval Overhaul樹木については、**現在残しているという事実をそのまま採用理由にしない**。ポストBetaのリテクスチャ工程へ入る前に、まず各樹木をAMJEの植生として残す必要があるかを再監査する。

監査順序は以下とする。

1. AMJEの対象地域・対象時代・4植生帯・景観上の役割に照らし、その既存樹木を残す必要があるか判定する。
2. 不要・不適切・代替済みの樹木は、リテクスチャ対象へ自動的に残さず、分布からの除外・置換・非採用を検討する。
3. 残すと判断した樹木だけをEnvironment所有のリテクスチャ対象とする。
4. 残す既存樹木の**表示名と説明文の両方**を日本列島の古代～中世に対応する植生・樹種群へ整合させる。継承元Vanilla / MOの欧米的・汎用的な呼称を無条件で残さず、日本語名を先に確認し、英語名と説明文に反映する。承認済みの説明本文は名称変更を口実に改稿しない。

AMJE植物説明フォーマットは、現行の独自4植物と同様に、原則として**名称・別名 → 日本での分布／生態 → 古代～中世日本での利用・景観・文化的文脈 → 現代との差異や現代利用（裏付け可能な場合）**の順とする。直接史料が弱い場合は用途を捏造せず、景観・分布文脈に留める。

この説明監査には、文化的・歴史的文脈を欠くVanilla説明の補正も含む。特に竹を「美しくない」とするVanillaの文化的に偏った説明は修正対象とし、竹をAMJEに残すかの植生監査と分離せず同じ工程で扱う。

**2026-10-10作者方針変更：表示名も日本向けに修正する。** 表示名は `ThingDef.label`／日本語 `DefInjected .label`（Biome等は該当Defのlabel）で修正し、識別子 `defName`、収量、収穫品、植林、分布、画像、保存データを改名の都合で変更しない。特定種と断定できないVanilla代理樹木は、現行説明・生態・外観に矛盾しない和名の**樹種群の総称**を使う。固有種への限定（例：Oakをミズナラと断定）は、その地域・外観と承認済み説明の整合を別途確認する。残す樹木以外のVanilla/MO由来の植物・動物・Biomeも、実在物への対応を確認して名称を段階的に監査する。Generic植生は根拠なしに日本固有種へ改称せず、MOの幻想種も無根拠に日本在来種と呼ばない。

したがってポストBetaの既存樹木作業は、**retention audit（残すか） → distribution/ownership decision → Japanese label + approved description → retexture** の順を基本とし、「既存樹木をすべて残してから描き直す」ことは前提にしない。

### 11.5.6 Vanilla自然植物の残存ゲートと樹木量保全

Vanilla自然植物は、既存のBiomeDefに入っていること自体を採用理由にしない。AMJEの各植生帯へ残す場合は、少なくとも以下を満たすこと。

- 日本列島の対象植生帯に対応できる分布・生態上の理由がある。
- 古代～中世というAMJ対象時代に対して明白な時代外来種・未来植物ではない。
- Vanilla Defの名称・外観・季節挙動を、日本側の植物群・景観の代理として扱っても別物へのすり替えにならない。
- AMJE所有の代表植物と役割が重複する場合は、残す側に副次林・林床・遷移帯など明確な役割がある。

GenericなGrass / Moss / Bush / Brambles等は、特定の外来種を主張しない林床・草地フィラーとして残してよい。これに対し、樹木や固有名を持つ草本は個別監査する。

**樹木を削除するときは植生密度と木材供給を別問題として確認する。** `wildPlants` commonalityから木を単純削除すると、総 `plantDensity` が同じでも選択比率が草本へ移り、結果として森林の樹木量・木材量だけが落ち得る。そのため、森林帯で既存樹木を除外する場合は、削除理由と独立した理由がない限り、削除した樹木commonalityをその帯に妥当なAMJE所有樹木または残存樹木へ再配分し、監査前の木本比率を概ね維持する。

例外は高山帯の森林限界で、通常高木を削除した分を別の高木で埋め戻してはならない。この場合はハイマツ等の低木性木本へ必要分だけ再配分し、「木本植生は維持するが通常高木は生やさない」構造を優先する。

2026-10-08監査後の残存方針:
- 暖温帯: Maple / Bambooを残し、主構造はSudajiiへ集約。Vanilla Poplar / Oakは除外。
- 冷温帯: Oak / Maple / Birch / Pineを副次樹木として残す。Oakはナラ類、Birchはカバノキ類、Pineはアカマツ系二次林を表す汎用代理として扱い、後続の説明・外観監査対象とする。
- 亜高山帯: Birchをダケカンバ系の副次高木として残し、汎用Pineは除外してShirabisoへcommonalityを移す。
- 高山帯: 通常高木Pine / Birchと汎用Dandelion / Astragalusを除外し、Haimatsu・Grass・Mossへ再配分する。
- Berryは特定種を断定しない採集用低木、Grass / TallGrass / Brambles / Moss / Bushはgenericな林床・草地要素として当面残す。

Vanilla湿地Biomeも同じ残存ゲートの対象とする。AMJEの4植生帯は `swampiness < 0.5` の陸地を基準にし、湿潤地では `TemperateSwamp` / `ColdBog` が共存できるため、そこに残る `wildPlants` を監査しないとVanilla植生が湿地経由で再流入する。

2026-10-08湿地監査:
- `TemperateSwamp`: Chokevine 0.80をBrambles 0.80へ、Cypress 1.00を削除してWillow 1.00→2.00へ移す。総commonality **7.30**、木本commonality **3.00**を維持する。
- `ColdBog`: Chokevine 3.00をTallGrass +1.00 / Moss +2.00へ、Astragalus 0.10をMossへ、Cypress 0.60をBirch 0.60へ置換する。総commonality **8.22**、木本commonality **1.80**を維持する。
- Willowは日本の湿地・河畔に対応可能な汎用ヤナギ類代理、Mapleは在来カエデ類代理として残す。ColdBogのBirchは冷温帯～亜高山帯で既に採用しているカバノキ類代理を流用する。
- Vanilla Cypressは湿地性のVanilla樹木であり、日本のヒノキを表す代理としては扱わない。
- `Plant_HealrootWild` は湿地を含む既存供給を維持する。薬系分離により撤去予定はない。

**2026-10-08作者確定：** Environmentでは野生・栽培ヒールルートと既存薬草供給を維持する。日本向け薬用植物と葛根採取・生薬・加工薬は独立薬系Modへ移管し、EnvironmentでのHealroot置換・最終撤去計画は撤回する。正本：[Project薬系企画](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedicinalPlantsAndRemedies.md)。

### 11.5.7 植生整備ロードマップ

Environmentの植生整備は、以下の順序を正本とする。

1. **不要なVanilla樹木・植生と、そこから再流入する不適切なBiome bundleを整理する。** 古代～中世日本の植生として残す根拠が弱いものを除外し、必要に応じて木本・総植生commonalityを妥当な日本側植物へ再配分する。AMJE-owned 4植生帯の主要植物監査は2026-10-08時点で完了したが、`TemperateSwamp` / `ColdBog` は植物だけでなく野生動物・病気・天候・説明を含むBiome全体の採用監査を完了してからStep 1を閉じる。
2. **残すVanilla/MO樹木・植物の日本向け表示名と説明文を監査してからリテクスチャする。** 日本語名・地域／樹種対応・Historical Description Guidelinesを確認し、英語ラベルも日本向けの意味に揃える。承認済み本文は維持し、未承認の本文変更は公表しない。実物に一致する範囲で `retention → Japanese label + description → retexture` とする。
3. **不足している日本の自然植生を追加する。** 湿地・森林・草地の自然景観を対象とし、薬用植物・薬材・加工薬は薬系Mod、果実・木の実の採集ゲームプレイは独立[Wild Food Foraging](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Wild-Food-Foraging/blob/main/Docs/Design.md)が所有する。**2026-10-10作者決定：Wild Food ForagingはEnvironment（`sucro.ancientmedievaljapan.environment`）を必須依存とする。** スダジイ（`AMJ_Tree_Shii`）などEnvironmentが提供する日本在来植生が前提となり、山野採集側に代替樹木・代替Biomeを重複定義しない。**依存方向はForaging→Environmentのみ**で、Environmentは採集Modを必須依存化せず単独で成立する。スダジイの分布は地域生態の現行ルールを維持し、全Biomeへの強制出現を意味しない。木の実の採集機能・収量・追加種・旧セーブ互換は未承認／未検証。
4. **既存の医療供給を維持する。** Wild Healrootの最終撤去工程は撤回。薬系Modの導入をEnvironmentの成立条件にしない。

植生・アート整備は既存医療供給を維持したまま進める。

**2026-10-10作者確定のStep 3実装順：** 湿地4種（葦／アシ、菅／スゲ、榛の木／ハンノキ、水苔／ミズゴケ）の**実装と検証の完了を先行条件**とし、その**後に**優先度Aで記録復旧した**杉（スギ）と薄・芒（ススキ）の追加**へ進む。スギ／ススキ間の着手順は未指定で、湿地工程へ割り込ませない。ここでの「湿地4種の完了」は、4種の本番PlantDef・承認済み説明／画像が実際に導入され、意図した湿地で自然発生し、収穫・除去・非栽培等の確定仕様と既存Biome・4構成（Vanilla／MO／CCTO／MO+CCTO）の必要な回帰が通過した状態をいう。**資料・仮Defの作成、CIのみのPASS、候補画像の存在をもって完了とはしない。** Steamの公開・配信確認は別ゲートとし、開始条件に勝手に追加しない。

**スギの実装範囲：** まず古代・中世以前から存在する**日本在来の天然スギ林の分布**を森林植生として扱う。**中世後期の地域的な植林・木材利用**は歴史的な利用として説明・今後の任意の植林設計で区別し、人工林を自然林の分布に混ぜない。**近現代の全国的な人工スギ林の広がりは再現しない。** 本決定はスギの実装方針を確定するが、植林研究／メニューの解放条件、木材収量、commonality、追加Recipeは承認しない。


### 11.5.8 Vanilla湿地Biomeの採用監査

2026-10-08の再監査では、`TemperateSwamp` / `ColdBog` について「湿地だから残す」だけでは不十分と判定した。

**結論:** 湿地環境自体は日本列島の自然環境として必要であり、両Vanilla BiomeDefの名称・基本Worker・mud / marsh / shallow water等の地形生成はgenericな湿地基盤として再利用してよい。互換性上の利点も大きいため、現段階では新しいAMJE湿地BiomeDefへ分離しない。

ただし、AMJEが両Biomeを採用する以上、`wildPlants` だけをPatchした状態を最終形とはしない。以下はEnvironmentの監査対象とする。

- **野生動物:** Vanilla湿地にはRaccoon / Ibex / Muffalo / Alpaca / Megasloth / Rhinoceros / Arctic Fox / Polar Bear / Lynx等、AMJEのJapan-oriented wildlife proxy policyと矛盾する候補が多数残る。湿地でも4基準Biomeと同じproxy原則へ揃える。
- **病気:** VanillaのFlu / Plague / Malaria / parasites / Mechanites等のbundleを、そのまま日本の湿地性疾患として採用しない。AMJの時代・Incident方針と整合させて再監査する。
- **天候:** Vanilla湿地のDryThunderstorm 1および暖冷湿地共通のSnow 4/4は、AMJEの湿潤気候・温度帯別weather baselineと不整合なので補正対象とする。
- **説明:** 「diseaseにchoked」「trees and vines」といったVanilla汎用過酷Biome説明を残さず、日本の湿原・沼沢地の生態と景観へ書き換える。
- **植生:** Phase 5のChokevine / Cypress / Astragalus除外とcommonality再配分は維持する。不足するヨシ・スゲ・ハンノキ・ミズゴケ等はStep 3で追加候補とする。
- **地形:** MarshyTerrain / Mud / WaterShallow / MossyTerrain / Marsh等による湿地構造は再利用候補として維持し、Step 1では新規地形追加を要求しない。

日本の湿地は低地のヨシ・スゲ湿原、ハンノキ林、ミズゴケ高層湿原等を含み、温暖側から冷涼側まで広く存在するため、TemperateSwamp / ColdBogというgenericな二類型自体には残存理由がある。正本の詳細監査は `Docs/VanillaWetlandBiomeAudit-ja.md` とする。

**Step 1の完了条件:** 両湿地のwildAnimals / diseases / weather / descriptionをAMJE方針へ揃え、loaded Defテストで不適切な植物・動物の再流入がないこと、湿地地形生成が維持されること、world上の湿地比率が不意に消失・急増していないことを確認する。これらの回帰ゲートについて2026-10-08に作者からPASS報告を受領したため、**基本Step 1を作者報告ベースで完了**とし、既存植物の残存判定・日本語説明監査へ進める。ただし実機ログによる独立監査や実際の湿地比率の較正、別構成のMO/CCTO統合検証は未完了と区別する。画像リテクスチャの保留は解除しない。

**2026-10-08暫定実装:** `Patches/VanillaWetlandEcology.xml` が両Vanilla湿地の `wildAnimals` / `baseWeatherCommonalities` / `diseaseMtbDays` / `diseases` / `allowedPackAnimals` をAMJE方針に沿って補正する。具体的な全Def・relative commonalityは `Docs/VanillaWetlandBiomeAudit-ja.md` の「湿地生態bundle実装」を正本とする。Vanillaの湿地地形とBiomeWorker、Phase 5植物構成はそのまま保持する。数値は史実の再現値ではなく実機確認待ちのゲームバランス案。説明文の日本語本文は2026-10-08に作者承認済み。英語版と日本語DefInjectedの置換を実装し、旧VanillaラベルとBiomeWorkerは維持する。ロード済み説明文の新たな回帰アサーションについて、2026-10-08に作者から実機テストPASSの報告を受領した。実機ログ・JSONは未添付のため、個別アサーション数、完全キャプチャ、起動前・実行時ERRORゼロを独立再監査したとまでは扱わない。これのみで全構成の統合試験完了や詳細な湿地出現率の確定を宣言しない。確定本文と英訳は `Docs/VanillaWetlandBiomeAudit-ja.md` を正本とする。

**自然分布回帰（2026-10-08）:** 湿地Biomeを強制できる既存Quickstartとは独立して、coverage 0.30の決定的seedの自然生成世界を読み取り専用で計数するテストを追加する。両湿地合算の存在、湿潤度0.5以上の候補の存在、候補外湿地・Biome未割当ゼロ、自然湿地マップへの入植、湿地の陸地比率20%以下を初期の粗い異常検出条件とする。20%は歴史的な適正比率ではなく、精密な目標比率は実測後に再評価する。作者から新しいテストのPASS報告を受けているが、実機ログ・JSONは未受領のため詳細なアサーション数、湿地比率、ERROR検証結果の独立監査は未完了。先行8/8の証拠とは区別する。詳細は `Docs/VanillaWetlandBiomeAudit-ja.md`。

### 11.5.9 Vanilla残存植物 Step 2監査

現行の6Biomeに含まれるVanilla樹木6種とgeneric植物7種の採用状態を再点検し、日本語説明と歴史資料を`Docs/VanillaPlantStep2DescriptionReview-ja.md` に記録した。竹は江戸期のモウソウチクへ一律に同定しない。マツ二次林の歴史的な存在を全自然林への分布根拠にしない。**6樹木すべての日本語本文を2026-10-08に作者承認済み**。既存のAMJE代表樹木4種は3段落構成、今回のVanilla樹木6種は2段落構成だが、いずれも名前・分布・歴史的な利用または景観・現代との差異（裏付け可能な場合）の共通順序を保持する。Birch/Willowは裏付けのない中世利用を加えず景観文脈に留める。改行はXMLリテラル `\\n\\n` に統一する。

竹の英語 `ThingDef.description` Patch・日本語DefInjected・ロード済み検証に加え、楓・楢・樺・松・柳の英訳／説明限定Patch／日本語DefInjected／Quickstartロード後アサーションをPR #30で実装・マージ済み（`bfb27420428dc75a060a317b3c22bdd4c5856fa8`）。**この2026-10-08のPR #30時点では** Vanillaのラベル、Biome・自然分布、植林、ゲームプレイ、画像は変更していなかった。**追加5樹木を含む最新版 `run-tests.bat` は2026-10-08に作者から通過報告あり**。これは作者報告ランタイムPASSであり、最新実行の個別JSON・完全ログ・所有ERRORゼロの独立監査はまだ行っていない。過去の竹テストPASSはそれ自体の旧版証拠として保持する。独立したVanilla/MO/CCTO/MO+CCTOの4構成統合試験は未完了であり、既存樹木のリテクスチャは引き続き保留する。

### 11.5.10 Step 3 不足する日本の自然植生

**2026-10-09一次採取の3構成設計：** `Docs/Research/WetlandPlantMaterialsProfiles-ja.md` にVanilla単体／Medieval Overhaul併用／将来の他AMJ連携の素材責務を記録。**作者確定：スゲはVanilla `Hay`、MOでは既存`Hay→DankPyon_Straw`へ統合し、スゲ専用原料を作らない。将来の菅笠・蓑・敷物も汎用干し草／藁でよい。ミズゴケは選択と情報表示・刈り取り除去を許可、収穫指示・素材ドロップ・栽培は不可。** ヨシのHay採用、ハンノキの`WoodLog`収量、スゲを含む`2/1/24`の収量は未承認。MOの任意WoodChain、他AMJの製品・Recipeは未実装。本番Defは変更していない。

**2026-10-10本文承認：** 湿地4種（アシ／ヨシ、スゲ、ハンノキ、ミズゴケ）の日本語説明本文は、アシ優先とミズゴケ泥炭の修正文を含む最終候補を作者が「OK」と回答して**4件とも承認済み**。正式本文は`Docs/NativeVegetationStep3Design-ja.md` §4、意味を揃えた英訳は同§4.1。**文章承認のみ**で、表示ラベル、画像、ゲーム内本番Def/DefInjected、配分・収穫数量、自然発生・ゲーム内テスト・Workshopは未完了。

**2026-10-09作者決定：** ヨシ・スゲ・ハンノキ・ミズゴケの4種は、他Modに同名・類似の植物がある場合でも、**Environment自身の実装対象**とする。Environment一つで中世日本の自然環境を成立させるのが目的であり、重複監査は実装省略判定には使わない。既存Modは任意互換（同時導入時の二重出現、植物分布、Patch競合、ロード後Def）の調査対象のみとする。形式的な実装開始判定記録、日本語説明・画像の作者承認、実機テストは維持する。

**森林候補の復元（2026-10-10）：** **杉（スギ、*Cryptomeria japonica*）もススキと同じ追加植物の優先度A**。2026-10-07の「冷温帯中心・一部暖温帯、林業景観」の候補が正式設計とPlantDefに未記録だったため、`Docs/NativeVegetationStep3Design-ja.md` §3.5に復元。現代人工林と古代～中世の自然植生を同一視せず、独立実装必要性監査、史実本文・画像承認、植生帯分布・植林・採取物・MO互換の検証を別途行う。**スギは未実装・ゲームデータへの反映なし**。

**草地候補の復元（2026-10-10）：** **薄・芒（ススキ、*Miscanthus sinensis*）はEnvironmentの優先度Aの追加候補**。2026-10-07の検討で暖温帯・冷温帯の草地・二次草原向けに位置づけていたが、正式文書と本番Defへは移っていなかった。湿地4種と区別し、独立したVE／非VEの既存Mod・必要性監査、日本語本文・画像の作者承認、自然発生比率・収穫素材の設計を後続で行う。**ススキは未実装・数値と資材未承認**。正本は`Docs/NativeVegetationStep3Design-ja.md` §3.4。

湿地候補・説明草案・承認ゲートは `Docs/NativeVegetationStep3Design-ja.md` を正本とする。2026-10-08最新作者指示で薬系を分離したため、ヨモギ・葛の追加／葛根採取はEnvironment対象外。研究と承認済み説明は[Project薬系企画](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedicinalPlantsAndRemedies.md)へ移管した。Environmentは既存Healrootを維持し、旧Step 4撤去計画を実行しない。新PlantDef・画像参照は承認ゲートを通す。

**DLC非所持プロファイルと既存植物の重複（2026-10-09）：** Odyssey専用のReeds/Bulrushがヨシ等の水辺景観と部分的に重なることは公開資料で確認したが、作者の環境にはOdyssey DLCがなく、その実Def・水域配置・パッチ競合をロード済み実機では検証していない。**EnvironmentはOdysseyを必須依存にせず、DLCなしの湿地植生を成立させる。** Odyssey互換は独立した任意機能として保留し、配布可能・互換PASSと主張しない。完全置換方式の`Patches/VanillaWetlandVegetation.xml`とOdysseyの水域植物との競合は未検証として扱う。実装前のVE／非VE現行Mod調査、必要性の決定記録、日本語説明・画像承認ゲートは引き続き有効。詳細は`Docs/NativeVegetationStep3Design-ja.md`、既存Mod実XMLの重複監査は`Docs/Research/WetlandPlantExistingModAudit-ja.md`。

## 12. Seasonal scenery baseline

### 12.1 Alpha principle

Environment should not add a parallel seasonal-scenery system when RimWorld already provides the required visual state changes.

The Alpha seasonal-scene model is therefore:
- use Environment's accepted seasonal temperature curve and weather baseline;
- let Vanilla snow weather and SnowGrid provide winter ground/plant snow presentation;
- use one deciduous AMJE structural tree (Japanese beech) for autumn/leafless seasonal contrast;
- keep Sudajii, Shirabiso and Haimatsu evergreen so the four vegetation bands do not all change identically;
- defer final AMJE-owned plant textures, including required Vanilla/MO retextures, until the final-art pass while mechanics/distribution are still being stabilized; this is scheduling deferral only and does not transfer texture ownership to another Mod.

No new Harmony patch, GameCondition, WeatherDef or seasonal calendar controller is added for this stage.

### 12.2 Deciduous / evergreen structure

`AMJ_Tree_Beech` inherits `DeciduousTreeBase`. This preserves Vanilla's fall-color shader parameters and uses a leafless graphic when the plant enters Vanilla's leafless state.

The other three structural plants intentionally remain non-deciduous:
- `AMJ_Tree_Shii` — evergreen warm-temperate broadleaf;
- `AMJ_Tree_Shirabiso` — evergreen subalpine conifer;
- `AMJ_Shrub_Haimatsu` — evergreen alpine dwarf pine.

This creates the intended broad visual progression without inventing AMJE-specific seasonal logic.

### 12.3 Snow

AMJE uses Vanilla `SnowGentle` and `SnowHard`. Both have positive snow rates and feed Vanilla's existing snow accumulation/rendering system.

Because the accepted AMJE weather table already shifts colder bands toward more snow commonality, and the accepted climate model produces winter freezing conditions, snow scenery is treated as an emergent result of the existing temperature + weather systems rather than a separate Environment feature.

### 12.4 Validation

The fixed-biome runtime gate verifies loaded seasonal-scene prerequisites:
- Japanese beech has a loaded leafless graphic;
- Japanese beech retains the inherited `_FallBehaviorEnabled` shader parameter;
- Sudajii, Shirabiso and Haimatsu do not acquire a leafless graphic;
- Vanilla SnowGentle and SnowHard remain loaded with positive snow rates;
- no pre-launch or Environment-origin runtime ERROR occurs.

These checks validate integration/state wiring, not artistic quality. Final texture appearance remains a manual visual check at the final-art stage.

The first automated runtime rerun after adding these checks passed cleanly:
- WarmTemperate: **31/31** assertions;
- CoolTemperate: **31/31**;
- Subalpine: **29/29**;
- Alpine: **29/29**;
- AMJE+CCTO focused run: **43/43**.

All reports had `preLaunchErrors=0`, `logErrors=0`, complete live capture, and no truncated logs. The seasonal-scenery baseline is therefore accepted for Alpha without adding a custom seasonal controller.

## 13. World-to-map river and coast handoff

### 13.1 Ownership

Environment owns the **world-level** frequency and shape context of rivers/coasts:
- Creek / River spawn thresholds and chances;
- no natural Large/Huge rivers;
- increased coastal-tile frequency and coastline complexity.

Environment does **not** implement a second local-map river or coast generator.

RimWorld 1.6 already converts world information into map features through `WorldGenStep_Mutators`:
- a coastal world tile receives the Vanilla `Coast` tile mutator;
- a river-bearing world tile receives the Vanilla `River` mutator in the Core-only path;
- those workers generate ocean water, beach terrain, moving river water and the associated local water data.

AMJE therefore keeps this boundary:
1. alter world distribution;
2. preserve the Vanilla world-to-map handoff;
3. add custom local-water generation only if a later gameplay feature genuinely requires behavior Vanilla cannot express.

This is intentionally compatible with other systems that also understand Vanilla Coast/River mutators.

### 13.2 Alpha validation

Two deterministic developer Quickstarts validate the handoff:
- `AMJRiverMapHandoffQuickstart`;
- `AMJCoastMapHandoffQuickstart`.

Each test:
- selects an unoccupied land world tile carrying the corresponding Vanilla mutator;
- generates a normal 250x250 local map;
- verifies the mutator remains present on `map.TileInfo`;
- verifies at least one local terrain cell is tagged `River` or `Ocean`, respectively;
- remains under the standard isolated-runtime ERROR gate.

These tests do not lock an exact local water-cell percentage. Local map geometry is still Vanilla behavior; AMJE only needs to guarantee that its modified world rivers/coasts continue to reach that behavior.

The first local rerun of the combined Environment runtime gate after adding the River/Coast handoff Quickstarts was reported **PASS** by the author. This confirms the Alpha handoff path is accepted: AMJE world-level river/coast changes continue to reach Vanilla local-map generation without requiring a custom local-water generator. Exact assertion counts and water-cell counts were not recorded from that run, so they are intentionally not treated as design constants.

### 13.3 Waterfalls remain deferred

The existence of working Vanilla local rivers does not change the Waterfall decision in Section 6.

A true or pseudo-waterfall would require additional local-map logic beyond the standard River mutator and is not needed for the Alpha environment loop. It remains deferred until there is a concrete gameplay/visual reason to add it.

## 14. Functional wildlife proxy baseline

### 14.1 Responsibility boundary

Environment does not become a Japan-animal content mod.

The four AMJ-owned BiomeDefs still need functional `wildAnimals` pools so generated maps are not empty and ordinary RimWorld wildlife systems continue to work. Alpha therefore uses a **small curated set of Vanilla PawnKindDefs as gameplay proxies**, not as claims that the literal RimWorld species shown on screen are historically exact Japanese taxa.

Japan-specific animal Defs, retextures, subspecies, hunting products or animal-production loops belong in a dedicated content/resource feature or another owning mod if they later become worthwhile.

### 14.2 Research basis and proxy rule

Environment Ministry distribution material identifies native/widely distributed Japanese mammal groups including sika deer, wild boar, foxes, bears and hares. Raccoon, by contrast, is treated by Japan's invasive-species policy as an introduced invasive animal.

References:
- https://www.env.go.jp/press/files/jp/6252.html
- https://www.biodic.go.jp/kiso/atlas/pdf/3.mammals.pdf
- https://www.env.go.jp/nature/intro/2outline/iaslist.html

Alpha interpretation:
- `Deer` is a functional proxy for Japanese deer / sika-deer ecology;
- `WildBoar` is a functional proxy for Japanese wild boar;
- `Fox_Red` is a functional proxy for native Japanese fox populations;
- `Bear_Grizzly` is only a large-bear gameplay proxy and must not be read as claiming grizzly bears were literally distributed across medieval Japan;
- `Wolf_Timber` is a large-canid gameplay proxy for the historical Japanese-wolf niche, not a species-level reconstruction;
- `Hare` / `Snowhare`, `Squirrel` and `Rat` remain generic small-mammal functional proxies.

Do not add Vanilla `Monkey` merely to represent Japanese macaques: RimWorld's Monkey description/graphic semantics represent a generic curly-tailed tropical monkey and are visually misleading as a macaque stand-in. A future Japan-specific macaque requires its own appropriate Def/art or a compatible external animal mod.

### 14.3 Removed placeholders

The following earlier Vanilla placeholders are excluded from all AMJ biome pools:
- `Raccoon` — modern introduced/invasive species in Japan;
- `Elk` — use `Deer` as the gameplay proxy instead;
- `Ibex` — not an appropriate Japan-alpine proxy;
- `Fox_Arctic` — use `Fox_Red` as the Japan-like proxy;
- `Lynx` — no broad Japanese medieval counterpart suitable for these generic biome pools.

### 14.4 Accepted Alpha pools

| Biome | Vanilla wildlife proxies |
|---|---|
| Warm-temperate | Hare, Squirrel, Rat, Deer, WildBoar, Fox_Red, Wolf_Timber, Bear_Grizzly |
| Cool-temperate | Hare, Squirrel, Rat, Deer, WildBoar, Fox_Red, Wolf_Timber, Bear_Grizzly |
| Subalpine | Hare, Snowhare, Deer, WildBoar, Fox_Red, Wolf_Timber, Bear_Grizzly |
| Alpine | Hare, Snowhare, Deer, Fox_Red, Wolf_Timber |

The exact commonality numbers remain ordinary Alpha gameplay values in `AMJ_Biomes.xml`. They are not historical population-density estimates.

### 14.5 Automated validation

Static validation requires:
- none of the five retired placeholders appears in any AMJ biome;
- every accepted proxy for each biome is present with positive commonality.

The fixed-biome runtime Quickstarts repeat the same check through the loaded `BiomeDef.CommonalityOfAnimal` API. This catches Patch/Def interactions that static XML inspection cannot.

The combined Environment runtime gate executed after these assertions were introduced and was later reported PASS by the author during the ENV-008 verification cycle. Because the same Quickstart source already contained the wildlife assertions at that time, the Alpha proxy set is accepted without a separate manual wildlife smoke test.


### First-release artwork scope (author decision, 2026-10-05)
Finish AMJE's four existing custom plants and their required state graphics/display checks before release. Retextures of existing Vanilla/MO trees are deferred to post-release updates and are not first-release blockers. Verify icons, remaining state display and the actual distribution configuration before release. A visually disruptive existing-tree mismatch may be reconsidered narrowly with evidence; this does not authorize broad retexture work now.
