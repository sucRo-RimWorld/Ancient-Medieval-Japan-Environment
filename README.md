# Ancient & Medieval Japan: Environment

**RimWorld 1.6 — Alpha**

A standalone environment overhaul that reshapes RimWorld toward a **pre-Edo Japan-like climate and landscape**, with Japan-oriented world generation, biomes, vegetation, weather, seasonal scenery, rivers, coastlines, and natural soil distribution.

AMJ Environment is designed to work on its own for players who mainly want a medieval-Japan-like natural setting. **Ancient & Medieval Japan Core is not required.**

For a stricter realism-focused plant climate model, it can optionally be combined with **Crop Cold Tolerance Overhaul (CCTO)**. AMJ Environment remains fully functional without CCTO.

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
- weather and seasonal scenery that emerge primarily from RimWorld's existing temperature, snow, and plant systems.

Historical and ecological research is used as a reference, but gameplay clarity takes priority over literal simulation.

## Main features

### Japan-oriented world generation

AMJ Environment modifies the root-surface world after Vanilla terrain generation, preserving compatibility with later world-generation systems.

Current Alpha targets include:

- land annual mean temperature: approximately **-8°C to 20°C**;
- southern warm lowlands: roughly **17–20°C** annual mean;
- central temperate lowlands: roughly **12–16°C**;
- northern cool lowlands: roughly **5–9°C**;
- elevation cooling: approximately **6.25°C per 1000 m**;
- elevation up to roughly **3800 m**, with most land below 1500 m;
- Hilliness distribution target: **25% Flat / 20% Small Hills / 25% Large Hills / 25% Mountainous / 5% Impassable**;
- rainfall broadly around **800–3000**;
- coastal land frequency around **1.5× the same-seed Vanilla baseline** through more complex coastlines rather than simply flooding land.

### Japan-style rivers

Natural world rivers favor smaller channels:

- Creek: lower spawn threshold, width 4;
- River: lower spawn threshold, width 6;
- Large River / Huge River: retained for compatibility but disabled from normal natural spawning.

AMJ Environment changes the world-level river distribution only. Local river maps continue to use RimWorld's standard River tile mutator and moving-water terrain system.

### Four Japan-oriented biome bands

AMJ Environment adds four natural biome bands:

- **Warm-temperate forest** — evergreen broadleaf forest;
- **Cool-temperate forest** — deciduous broadleaf forest;
- **Subalpine forest** — cold evergreen conifer forest;
- **Alpine zone** — sparse vegetation and dwarf-pine scrub above the main forest belt.

These AMJ biomes are intentionally not globally authoritative. Specialized biomes from compatible mods such as Medieval Overhaul can still win where appropriate.

### Structural Japanese vegetation

A deliberately small set of Japan-specific structural plants is included:

- **Sudajii / Shii tree** — warm-temperate evergreen canopy;
- **Japanese beech** — cool-temperate deciduous canopy;
- **Shirabiso fir** — subalpine evergreen conifer;
- **Haimatsu dwarf pine** — alpine scrub.

Generic grasses, mosses, shrubs, secondary trees, and other filler vegetation continue to reuse Vanilla PlantDefs. The goal is to make the vegetation bands structurally legible without turning Environment into a large plant-content mod.

The current plant graphics are still placeholders using Vanilla assets. Final plant artwork is deferred to the final visual pass.

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

AMJ Environment reuses RimWorld's existing weather types rather than adding a parallel weather system.

Warm/cool forests favor more liquid precipitation and fog, while subalpine/alpine regions shift progressively toward snow. Dry thunderstorms are deliberately rare relative to rainy thunderstorms.

Calendar-specific Baiu, Akisame, and typhoon-season weighting is not part of Alpha. Sea-of-Japan-side versus Pacific-side winter exposure is also deferred until there is a proper geographic basis for it.

### Seasonal scenery

Seasonal presentation uses Vanilla systems:

- Japanese beech keeps Vanilla fall-color and leafless behavior;
- Sudajii, Shirabiso, and Haimatsu remain evergreen;
- Vanilla SnowGentle / SnowHard and SnowGrid provide winter snow scenery.

No custom seasonal controller is added in Alpha.

### Functional wildlife proxies

AMJ Environment does not add Japan-specific animal Defs.

The custom biomes use a curated set of Vanilla animals as functional gameplay proxies. Clearly unsuitable placeholders such as Raccoon, Elk, Ibex, Arctic Fox, and Lynx were removed from the AMJ biome pools.

These are gameplay proxies, not literal historical-species claims. Japan-specific animals or retextures belong in a separate content feature/mod if added later.

## CCTO integration

**Crop Cold Tolerance Overhaul is optional.**

Without CCTO, AMJ Environment plants keep an explicit Vanilla-style minimum growth temperature baseline.

When CCTO is present, AMJ Environment conditionally applies CCTO's public cold-tolerance extension to its own four structural plants:

| Plant | AMJE only | AMJE + CCTO |
|---|---|---|
| Sudajii | 0°C minimum growth | 8°C minimum growth; death below -8°C |
| Japanese beech | 0°C minimum growth | 5°C minimum growth; cold dormancy |
| Shirabiso | 0°C minimum growth | 0°C minimum growth; death below -35°C |
| Haimatsu | 0°C minimum growth | 0°C minimum growth; death below -35°C |

This keeps the mods loosely coupled:

- AMJ Environment does not require CCTO;
- CCTO does not contain AMJE-specific balance data;
- AMJE owns the optional compatibility for AMJE-owned plants.

In practical terms:

- **AMJE alone** = complete medieval-Japan-like environment experience;
- **AMJE + CCTO** = the same environment with a stricter, more realism-focused plant cold-response model.

## Medieval Overhaul compatibility

Medieval Overhaul is **not required**, but normal AMJ play is expected to coexist with it.

Compatibility is therefore treated as a first-class target:

- MO specialized natural biomes are not globally suppressed;
- MO Dark Forest retains its own special terrain-patch identity;
- Environment only applies targeted compatibility where needed;
- MO agricultural improvements remain outside Environment's natural-soil balance.

## Dependencies

Required:

- **Harmony**

Not required:

- Ancient & Medieval Japan Core;
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

## Current status

**Alpha**

The core environment systems are implemented and automated runtime gates cover world/climate assumptions, biome vegetation, natural terrain, weather, seasonal-state wiring, optional CCTO integration, and river/coast world-to-map handoff.

Final custom visual assets are still pending, and broader real-play balance/compatibility feedback may change Alpha values.

## Research and detailed design

The detailed design source of truth is:

- [Docs/Design.md](Docs/Design.md)

It contains the current world-generation targets, climate calibration, biome/vegetation rationale, soil thresholds, weather model, wildlife-proxy policy, CCTO integration values, and automated validation history.

## AI-assisted development

This mod was developed with AI assistance, primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review are performed by the author.

## License

MIT License.

## Support

[![Ko-fi](https://img.shields.io/badge/Ko--fi-Support%20me-ff5e5b?logo=ko-fi&logoColor=white)](https://ko-fi.com/sucro0629)
