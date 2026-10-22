# Ancient & Medieval Japan - Environment（中世日本 - 環境）

**RimWorld 1.6 / 1.7 — Beta**

**Workshop release: 0.2.0 · GitHub development version: 0.3.0 (not yet published)**

A standalone environment overhaul that replaces and reconfigures Vanilla terrain, vegetation, and biome composition for a Japanese-archipelago-inspired landscape. It changes world climate, mountains, rivers, coastlines, natural soil distribution, and the placement and vegetation of biomes to create a **pre-Edo Japan-like natural environment**.

AMJ Environment works independently of other AMJ modules. Harmony is its only required mod; Crop Cold Tolerance Overhaul and Medieval Overhaul are optional.

For a stricter realism-focused plant climate model, it can optionally be combined with **[Crop Cold Tolerance Overhaul (CCTO)](https://steamcommunity.com/sharedfiles/filedetails/?id=3812412548)**. AMJ Environment remains fully functional without CCTO.

## Design goal

The target is not a literal GIS reconstruction of Japan and not one specific historical year.

The goal is a gameplay-oriented environmental profile inspired by the Japanese archipelago before the Edo period:

- humid temperate to cool climate;
- hot summers and meaningful cold winters;
- limited flat land and abundant hills/mountains;
- frequent short and medium rivers rather than continental-scale rivers;
- long, irregular coastlines with more bays, peninsulas, and islands;
- vegetation belts that shift from warm-temperate evergreen forest through cool-temperate deciduous forest and subalpine conifers to alpine scrub;
- natural soil quality that becomes poorer and stonier in colder/high-elevation zones;
- weather and seasonal scenery that emerge primarily from RimWorld's existing temperature, snow, and plant systems;
- biome wildlife pools adjusted toward a Japan-like ecological composition using curated Vanilla animals as functional gameplay proxies.

Historical and ecological research is used as a reference, but gameplay clarity takes priority over literal simulation.

## Main features

### Japan-oriented world generation

AMJ Environment recalculates elevation, hilliness, temperature, rainfall, and biome placement while retaining RimWorld's existing world-generation pipeline. The resulting environment replaces the broad Vanilla baseline with a Japan-oriented configuration. Local river and coastal map generation remains compatible with RimWorld's standard systems.

Current Beta targets include:

- land annual mean temperature: approximately **-8°C to 20°C**;
- southern warm lowlands: roughly **17–20°C** annual mean;
- central temperate lowlands: roughly **12–16°C**;
- northern cool lowlands: roughly **5–9°C**;
- elevation cooling: approximately **6.25°C per 1000 m**;
- elevation up to roughly **3800 m**, with most land below 1500 m;
- Hilliness distribution target: **25% Flat / 20% Small Hills / 25% Large Hills / 25% Mountainous / 5% Impassable**;
- annual world-map rainfall broadly around **800–3000**. The current world-generation transform has **no separate biome-specific annual-rainfall targets**; a biome-level precipitation rebalance has not been implemented;
- coastal land frequency around **1.5× the same-seed Vanilla baseline** through more complex coastlines rather than simply flooding land.

### Japan-style rivers

Natural world rivers favor smaller channels:

- Creek: lower spawn threshold, width 4;
- River: lower spawn threshold, width 6;
- Large River / Huge River: retained for compatibility but disabled from normal natural spawning.

AMJ Environment changes the world-level river distribution only. Local river maps continue to use RimWorld's standard river and moving-water terrain systems.

### Four Japan-oriented biome bands

AMJ Environment adds four natural biome bands that take precedence over broad Vanilla biomes on eligible land:

- **Warm-temperate forest** — evergreen broadleaf forest;
- **Cool-temperate forest** — deciduous broadleaf forest;
- **Subalpine forest** — cold evergreen conifer forest;
- **Alpine zone** — sparse vegetation and dwarf-pine scrub above the main forest belt.

These bands are not prefectural or regional borders. They are a gameplay simplification of the broad natural vegetation sequence seen across the Japanese archipelago: warm-temperate evergreen broadleaf forest gives way to cool-temperate deciduous broadleaf forest, then subalpine evergreen conifers, and finally alpine scrub above the treeline as climate becomes colder northward or with elevation.

The temperature thresholds are approximations for RimWorld rather than literal botanical boundaries. New surface worlds exclude the original Vanilla tropical rainforest/swamp, temperate/boreal forest, tundra, arid shrubland, deserts, ice sheet and sea ice from biome selection. The four AMJ bands replace that broad land baseline. Adopted temperate/cool wetlands, ordinary water and stronger specialized biome workers from compatible mods such as Medieval Overhaul still coexist. This correction does not change saved world biomes; fresh four-profile natural-world validation and a corrected Workshop upload remain outstanding.

The design also avoids presenting every map as untouched climax forest. Human activity from the Yayoi period through the medieval period increased pine woodland, grassland, and secondary-forest signatures in many settled regions, but that does **not** justify retaining arbitrary Vanilla vegetation. The first broad retention pass is now largely complete: Vanilla Poplar and Oak were removed from the warm-temperate forest, generic Pine from the subalpine forest, and generic Pine, Birch, Dandelion, and Astragalus from the alpine zone. Their removed woody share was reassigned to Sudajii, Shirabiso, and Haimatsu so the cleanup does not accidentally thin the intended forest structure. Existing wild/cultivated Healroot and MedicineHerbal supply remain. Medicinal plants and remedies belong to a future separate medicine Mod, not Environment.

### Structural Japanese vegetation

Each of the four AMJ biome bands receives **one AMJE-owned representative structural plant** so the climate band is visually readable at a glance:

- **Warm-temperate forest → Sudajii**
- **Cool-temperate forest → Japanese beech**
- **Subalpine forest → Shirabiso**
- **Alpine zone → Haimatsu**

The selection criterion is not "a famous medieval tree for each biome." The plant must represent the natural vegetation structure of that Japanese climate/elevation band and remain appropriate to an environment whose historical presentation extends into ancient and medieval Japan. Haimatsu is intentionally a low alpine shrub rather than a timber tree, preserving the visual and resource difference above the treeline.

#### Sudajii — warm-temperate forest

![Sudajii](Textures/Things/Plant/AMJ/Shii/Shii_A.png)

Sudajii is an evergreen canopy tree representative of the warm-temperate lucidophyll forests of southern Honshu, Shikoku, and Kyushu. Castanopsis-dominated evergreen forest was already established in parts of southwestern Japan in the Jomon period, and archaeological evidence also records the use of Castanopsis fruits. Its timber, bark, and edible seeds have all been used by people.

#### Japanese beech — cool-temperate forest

![Japanese beech](Textures/Things/Plant/AMJ/Beech/Beech_A.png)

Japanese beech is one of the major deciduous canopy trees of Japan's cool-temperate mountain forests. Fagus was part of prehistoric cool-temperate forest communities, and archaeological wooden vessels from the late Heian through medieval period include examples made from Fagus wood. In modern Japan, beech is also used for furniture, interior materials, mushroom logs, and fuel.

#### Shirabiso — subalpine forest

![Shirabiso](Textures/Things/Plant/AMJ/Shirabiso/Shirabiso_A.png)

Shirabiso is a major evergreen conifer of Honshu's subalpine forests. On mountains such as Mt. Ontake, Shirabiso and related subalpine conifers form the forest landscape below the alpine zone. Rather than inventing a specific medieval resource use, AMJE treats this forest as part of the mountain landscape that also became associated with ascetic practice and mountain worship.

#### Haimatsu — alpine zone

![Haimatsu](Textures/Things/Plant/AMJ/Haimatsu/Haimatsu_A.png)

Haimatsu is a creeping evergreen dwarf pine of alpine areas from Hokkaido to the high mountains of central and northern Honshu. It forms dense scrub above the treeline and is one of the clearest visual markers of Japanese alpine vegetation. Its distribution also reflects the history of northern cold-climate flora persisting at high elevation after postglacial warming.

Some generic grasses, mosses, shrubs, secondary trees, and other filler vegetation still reuse Vanilla PlantDefs where the current audit has not rejected them. This reuse is not a permanent retention promise: unsuitable plants are removed or replaced as they are confirmed. The initial vegetation bands remain structurally legible rather than monocultures. Later Japan-native wild-edible plants and regional foraging behavior are Environment-owned future work, described in [Foraging Integration](Docs/ForagingIntegrationDesign-ja.md), not part of the published 0.2.0.

Detailed research rationale and sources for the representative plants are retained in [Docs/Design.md](Docs/Design.md) and [Docs/ArtDirection.md](Docs/ArtDirection.md).

### Natural soil fertility

AMJ Environment adds one new natural terrain:

- **Thin Soil** — fertility **0.50**.

It reuses:

- Gravel — 0.70;
- Soil — 1.00;
- Rich Soil — 1.40.

Poor/stony terrain becomes progressively more common from warm/cool forests toward subalpine and alpine regions.

Player-created agricultural improvements, including Medieval Overhaul's plowed soil, are not rebalanced by Environment.

### Regional weather

The published 0.2.0 version uses RimWorld's existing weather types and climate-dependent rain, fog, thunderstorms and snow; it does not add a parallel weather system.

**Implemented in GitHub's 0.3.0 development source; not yet published to Workshop:**

- **Tsuyu (early-summer rains):** on RimWorld calendar days 16–22 (seven days), the chance weights of ordinary rain and foggy rain increase by a provisional **×1.65** in Environment's supported biomes.
- **Akisame (autumn rains):** on days 30–38, those rain weights increase by a provisional **×1.40**. Both periods also adjust thunderstorm, fog and clear-weather selection weights without forcing rainy weather.
- **Summer and autumn clear spells:** Clear-weather selection receives a provisional **×1.25** bonus on days **23–29** and **39–44**. Day 45 onward (winter quadrum) gets no generic seasonal sunshine bonus.
- **Reduced daylight during rainy/foggy weather:** gameplay sky glow is provisionally multiplied by **×0.80** for Rain/FoggyRain, **×0.70** for RainyThunderstorm and **×0.90** for Fog. Plant light-based growth and solar output are affected through RimWorld's existing light calculations; no direct crop growth penalty is added.

These source changes act on the four AMJ biomes and the two retained wetlands. Weather rendering and annual world-map rainfall remain unchanged. All weights/daylight factors are **tentative**, and real-season crop balance plus optional Dynamic Atmosphere co-loading are not yet verified. The tracked production DLL and published 0.2.0 package do not yet contain this 0.3.0 source behavior.

Sea-of-Japan-side versus Pacific-side winter exposure is not part of Environment's current implementation; geographically differentiated winter climate is a possible WorldMap integration, not an existing Environment feature.

### Seasonal scenery

Seasonal presentation uses Vanilla systems:

- Japanese beech keeps Vanilla fall-color and leafless behavior;
- Sudajii, Shirabiso, and Haimatsu remain evergreen;
- Vanilla SnowGentle / SnowHard and SnowGrid provide winter snow scenery.

Plant scenery still uses RimWorld's native seasonal systems. The separate 0.3.0 seasonal weather-weighting code does not add a custom plant-season renderer.

### Functional wildlife proxies

AMJ Environment adjusts wildlife distribution by biome toward a Japan-like ecological composition but does not add Japan-specific animal Defs.

The custom biomes use a curated set of Vanilla animals as functional gameplay proxies. Clearly unsuitable placeholders such as Raccoon, Elk, Ibex, Arctic Fox, and Lynx were removed from the AMJ biome pools.

These are gameplay proxies, not literal historical-species claims. Japan-specific animals or retextures belong in a separate content feature/mod if added later.

### Vanilla wetlands and retained-tree updates

AMJE retains Vanilla's Temperate Swamp and Cold Bog workers, labels, marsh/mud/shallow-water terrain, movement difficulty and basic densities. The following changes are implemented:

- Replace unsuitable wild-plant pools while preserving total and woody commonality.
- From **0.2.0**, provide four additional natural wetland plants: **Ashi/Yoshi reed**, **Suge sedges**, **Hannoki alder** in both retained wetlands, and **Mizugoke sphagnum** in ColdBog only. They are not sowable. Mature Ashi and Suge yield base **Hay ×2** and **Hay ×1**; Hannoki yields base **WoodLog ×24**; Mizugoke is removable but drops nothing. The previously accepted overall wetland vegetation and tree totals are maintained.
- Rebalance wetland wildlife and exclude unsuitable pack-animal candidates.
- Change wetland disease types, weights and average intervals, removing mechanite diseases and retaining reduced malaria only in warmer wetlands.
- Reweight wetland rain, fog, storms and snow by wetland climate.
- Rewrite both wetland descriptions and the descriptions of six retained Vanilla trees (Bamboo, Maple, Oak, Birch, Pine and Willow) in Japanese and English, without changing those trees' stats.
- Restrict regional tree-sowing choices to the natural distribution; alpine Haimatsu remains wild-only.
- Provide the four representative plants' native cutting wood outputs.

The four new wetland species are part of the **0.2.0** release. Ashi/Suge use approved sprites; Hannoki includes its approved seasonal states; Mizugoke temporarily reuses Vanilla moss art. Existing-tree Vanilla/MO retextures remain deferred.

## CCTO integration

**Crop Cold Tolerance Overhaul is optional.**

Without CCTO, AMJ Environment plants keep an explicit Vanilla-style minimum growth temperature baseline.

When CCTO is present, AMJ Environment conditionally applies CCTO's public cold-tolerance extension to its own structural and wetland plants:

| Plant | AMJE only | AMJE + CCTO |
|---|---|---|
| Sudajii | 0°C minimum growth | 8°C minimum growth; death below -8°C |
| Japanese beech | 0°C minimum growth | 5°C minimum growth; cold dormancy |
| Shirabiso | 0°C minimum growth | 0°C minimum growth; death below -35°C |
| Haimatsu | 0°C minimum growth | 0°C minimum growth; death below -35°C |
| Ashi/Yoshi | 0°C minimum growth | 5°C minimum growth; cold dormancy |
| Suge | 0°C minimum growth | 0°C minimum growth; cold dormancy |
| Hannoki | 0°C minimum growth | 5°C minimum growth; cold dormancy |
| Mizugoke | 0°C minimum growth | 0°C minimum growth; death below -35°C |

This keeps the mods loosely coupled:

- AMJ Environment does not require CCTO;
- CCTO does not contain AMJE-specific balance data;
- AMJE owns the optional compatibility for AMJE-owned plants.

In practical terms:

- **AMJE alone** = complete medieval-Japan-like environment experience;
- **AMJE + CCTO** = the same environment with a stricter, more realism-focused plant cold-response model.

## [Medieval Overhaul](https://steamcommunity.com/sharedfiles/filedetails/?id=2553700067) compatibility

Medieval Overhaul is **not required**, but normal AMJ play is expected to coexist with it.

Compatibility is therefore treated as a first-class target:

- MO specialized natural biomes are not globally suppressed;
- MO Dark Forest retains its own special terrain-patch identity;
- Environment only applies targeted compatibility where needed;
- MO agricultural improvements remain outside Environment's natural-soil balance.

## Dependencies

Required:

- **[Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)**

Not required:

- Crop Cold Tolerance Overhaul;
- Medieval Overhaul.

Optional integrations are applied only when the relevant mod is present.

## Save compatibility

AMJ Environment changes world generation and adds custom BiomeDefs, TerrainDefs, and PlantDefs.

- **Use a new game to evaluate the intended world, biome, river, coastline, and terrain generation.**
- Adding AMJ Environment to an existing save has **not been verified** and will not regenerate the existing world.
- Runtime climate behavior may still be affected after installation because Environment includes temperature hooks.
- **Removing the mod from a save that uses AMJE custom biomes, terrain, or plants is not recommended.**

This mod does **not** use CCTO's "safe to add/remove" statement.

## Languages

- English
- Japanese

Community translators: [source strings, version-specific key changes, and maintenance guidance](Docs/TranslationMaintenance.md).

Independent [Chinese translation add-on (Workshop ID 3823797121)](https://steamcommunity.com/sharedfiles/filedetails/?id=3823797121) is maintained by a third party, not bundled with AMJ Environment; its current compatibility is not independently verified here.

## Current status

**Beta · Workshop 0.2.0 · GitHub development 0.3.0 (unreleased)**

The published 0.2.0 feature set includes world/terrain/river/biome changes, four structural plants, retained-tree descriptions, wetland ecology changes, four additional wetland species and Thin Soil growing-zone support. Existing real-game tests cover important Vanilla, CCTO and Medieval Overhaul scenarios, but do not prove every combination or save-upgrade path.

The GitHub 0.3.0 source adds provisional Tsuyu/Akisame weather selection and gameplay daylight reduction. Original-game bounded checks exist, but seasonal balance, complete compatibility and a matching production DLL/Workshop upload remain outstanding. **The 0.3.0 source and About/Manifest version do not mean 0.3.0 has been released to subscribers.**

## Planned follow-up

The following are **not part of the published 0.2.0**:

- Release and balance review of the source-implemented **0.3.0 Tsuyu/Akisame and weather daylight adjustments**, including compatibility with other weather mods.
- Additional Japan-native vegetation such as Sugi and Susuki, and Environment-owned regional wild-food foraging ([design](Docs/ForagingIntegrationDesign-ja.md)); these are future features, not published gameplay.
- Selected retained Vanilla/Medieval Overhaul tree retextures (deferred).

New games are recommended for changes to world generation; existing worlds are not regenerated by updating. Existing Healroot and medicine supply remain. Medicinal treatments belong outside Environment.

## Research and detailed design

GitHub repository: [Ancient-Medieval-Japan-Environment](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment).

The detailed design source of truth is:

- [Docs/Design.md](Docs/Design.md)

It contains the current world-generation targets, climate calibration, biome/vegetation rationale, soil thresholds, weather model, wildlife-proxy policy, CCTO integration values, and automated validation history.

## AI-assisted development

This mod was developed with AI assistance, primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review are performed by the author.

## License

MIT License.

## Support

[![Ko-fi](https://img.shields.io/badge/Ko--fi-Support%20me-ff5e5b?logo=ko-fi&logoColor=white)](https://ko-fi.com/sucro0629)
