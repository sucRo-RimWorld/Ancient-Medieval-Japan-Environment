# AMJ Environment — Final Art Direction

## Scope

Environment intentionally deferred final art until the functional/balance Alpha loop was stable.

The minimum final-art pass now contains **29 image assets**. The scope was expanded after reviewing the Vanilla and Medieval Overhaul trees that actually appear alongside AMJE vegetation.

### AMJE structural plants

1. `AMJ_Tree_Shii` — leafy Sudajii / Shii tree
2. `AMJ_Tree_Beech` — leafy Japanese beech
3. `AMJ_Tree_Beech` — leafless Japanese beech
4. `AMJ_Tree_Shirabiso` — leafy/evergreen Shirabiso fir
5. `AMJ_Shrub_Haimatsu` — evergreen Haimatsu dwarf pine

### Vanilla tree retextures used by AMJE / normal MO coexistence

6. `Plant_TreeOak` — leafy
7. `Plant_TreeOak` — leafless
8. `Plant_TreeMaple` — leafy
9. `Plant_TreeMaple` — leafless
10. `Plant_TreePoplar` — leafy
11. `Plant_TreePoplar` — leafless
12. `Plant_TreeBirch` — leafy
13. `Plant_TreeBirch` — leafless
14. `Plant_TreeWillow` — leafy
15. `Plant_TreeWillow` — leafless
16. `Plant_TreePine` — evergreen
17. `Plant_TreeBamboo` — evergreen

These retextures intentionally affect the corresponding Vanilla tree Defs wherever they appear while AMJE is active. Creating duplicate AMJ-only tree Defs solely to change graphics would add unnecessary biome/content duplication and compatibility cost.

### Medieval Overhaul conditional tree retextures

18. `DankPyon_GreatOak` — leafy
19. `DankPyon_GreatOak` — leafless
20. `DankPyon_GreatIter` — leafy
21. `DankPyon_GreatIter` — leafless
22. `DankPyon_GreatFir` — evergreen
23. `DankPyon_GreatWillow` — leafy
24. `DankPyon_GreatWillow` — leafless

These are **style harmonization only**. Great Oak / Great Iter / Great Fir / Great Willow remain Medieval Overhaul species/content with their original mechanics, scale, transparency class, harvest behavior, and fantasy identity. AMJE should only replace their graphic paths when MO is active.

### Natural terrain

25. `AMJ_ThinSoil` — seamless/tileable thin stony soil

### World-biome textures

26. `AMJ_WarmTemperateForest`
27. `AMJ_CoolTemperateForest`
28. `AMJ_SubalpineForest`
29. `AMJ_AlpineZone`

No new weather, snow, river, coast, or seasonal-effect art is required for the Alpha pass because those systems intentionally reuse Vanilla rendering.

## Plant technical direction

Current plant Defs use `Graphic_Random`.

RimWorld's `Graphic_Collection` loads every texture found inside the configured folder path. A folder containing a single valid texture therefore remains compatible with `Graphic_Random`; additional variants can be added later without changing the Def class.

Alpha art policy:
- begin with **one strong final texture per state** rather than producing many variants immediately;
- use the **adopted ENV-010 atlas style** as the visual baseline for AMJE, Vanilla retextures, and optional MO retextures;
- the adopted style is rounded and simplified rather than botanically literal, with smooth high-resolution edges rather than pixel-art;
- saturation is reduced relative to the first concept pass, while the approved hue relationships remain intact;
- use a restrained but not posterized palette, soft painted shading, and clear silhouette separation at RimWorld gameplay zoom;
- keep transparent backgrounds;
- no baked terrain, UI border, text, or decorative frame;
- use RimWorld-readable top-down/three-quarter plant silhouettes rather than botanical illustration plates;
- preserve clear empty transparent space around branch/needle edges to avoid a rectangular sprite appearance;
- match existing plant scale through `graphicData.drawSize` / `visualSizeRange` rather than baking excessive canvas padding;
- avoid photographic rendering; target a painted game-sprite look compatible with RimWorld's natural assets;
- do not bake dynamic snow into plant textures.

### Sudajii

Visual role:
- dense warm-temperate evergreen broadleaf canopy;
- broad rounded crown;
- dark glossy foliage;
- visually heavier/darker than Japanese beech;
- trunk/branch structure should remain readable beneath the crown.

Accepted ENV-010 production treatment:
- use the strongest simplified/deformed **C-direction** from the in-game comparison;
- foliage masses are broad and chunky, with fewer internal clusters than the earlier Sudajii pass;
- use a clearly thicker, near-black outer outline so the tree reads at the same visual weight as the current AMJ crop sprites;
- remove the small green ground/grass ring entirely; the sprite must end at the trunk/root silhouette on transparent background;
- keep the hue relationships from the accepted comparison, but lower foliage saturation by roughly one visual step so the tree sits naturally against RimWorld's brown/grey terrain and existing muted vegetation;
- especially suppress the brightest yellow-green highlights and the orange saturation of the trunk while preserving value contrast;
- do not reduce contrast by thinning the outline or flattening all shading; simplification comes from larger shape masses and fewer color steps.

Avoid:
- European oak silhouette;
- tropical palm appearance;
- highly symmetric ornamental tree shape;
- decorative turf, grass clumps, or a circular ground base baked into the sprite;
- high-saturation lime highlights that make the tree appear pasted on top of the terrain.

### Japanese beech — leafy

Visual role:
- broad cool-temperate deciduous canopy;
- lighter/open crown than Sudajii;
- smooth pale-grey trunk impression;
- compatible with Vanilla fall-color shader behavior.

The base art should use neutral healthy foliage rather than permanently orange/red autumn colors, because seasonal color is supplied dynamically.

### Japanese beech — leafless

Visual role:
- same approximate trunk/crown proportions as the leafy beech;
- branching structure should align closely enough that the seasonal switch does not look like a different species/size;
- no foliage.

### Shirabiso fir

Visual role:
- narrow subalpine evergreen conifer;
- layered but compact conical crown;
- colder/darker needle mass than ordinary broadleaf trees;
- visually distinct from Vanilla Pine rather than merely recoloring it.

Avoid:
- oversized Christmas-tree symmetry;
- extremely tall narrow sprite that becomes unreadable at game zoom.

### Haimatsu dwarf pine

Visual role:
- low, spreading, wind-shaped alpine shrub;
- clearly wider than tall;
- creeping/matted pine structure rather than a miniature upright tree;
- no wood-harvest visual implication.

This is the most important silhouette distinction for keeping the alpine zone above the treeline.

## Retextured plant description policy

This section is the ENV-010 application of the AMJ-wide historical description policy in Ancient-Medieval-Japan-Core `Docs/HistoricalDescriptionGuidelines.md`.

Every plant whose in-game artwork is replaced by AMJE must also receive an AMJE-authored description so the visual pass does not retain text that conflicts with AMJE's tone, environmental role, or ancient/medieval Japanese historical context. This is a minimum requirement for the art scope; the shared AMJ policy is broader and also applies to Vanilla/MO items, plants, and animals that AMJ explicitly adopts, patches, selects, or localizes.

This applies to:
- AMJE-owned structural plants;
- Vanilla tree Defs retextured by AMJE;
- Medieval Overhaul tree Defs conditionally retextured by AMJE when MO is active.

Text changes are presentation/localization only. They must not imply mechanical changes that AMJE does not actually make, and MO-owned plants must retain their original gameplay/fantasy identity even when their wording is rewritten for consistency.

Authoring workflow:
1. write the Japanese description first;
2. have the author review and approve the Japanese wording;
3. only after Japanese approval, translate that approved text into English;
4. keep the English version semantically aligned with the approved Japanese source rather than independently rewriting it.

Labels should remain unchanged unless there is a separate concrete naming issue. The current requirement is to replace unsuitable or tonally inconsistent descriptions, not to rename every retextured plant.

## Thin Soil technical direction

`AMJ_ThinSoil` currently reuses/tints Vanilla Gravel.

Final texture should:
- tile seamlessly;
- read as shallow brown-grey soil with visible small stones;
- remain visually distinguishable from Vanilla Gravel and ordinary Soil;
- avoid looking like constructed flooring;
- remain low-contrast enough that pawns/plants/items stay readable on top;
- work with the existing `FadeRough` edge behavior.

The final fertility remains 0.50; art must communicate poor natural ground without looking completely barren.

## World-biome texture direction

World textures should distinguish the four AMJ bands at globe zoom without becoming saturated map icons.

### Warm-temperate forest
- deep evergreen green;
- slightly humid/dense visual impression;
- warmer green than Subalpine.

### Cool-temperate forest
- medium natural green;
- lighter/more neutral than Warm-temperate;
- must be visibly distinguishable from Warm-temperate because both currently reuse Vanilla TemperateForest.

### Subalpine forest
- dark cool blue-green / conifer impression;
- denser/cooler than Cool-temperate.

### Alpine zone
- muted grey-green/brown;
- sparse vegetation impression;
- should read as high-elevation land, not snow/ice biome.

Do not bake winter snow into world-biome textures; temperature/weather already control seasonal map behavior and the world texture represents biome identity rather than current-day weather.

## Proposed paths

AMJE plant folders:

- `Textures/Things/Plant/AMJ/Shii/`
- `Textures/Things/Plant/AMJ/Beech/`
- `Textures/Things/Plant/AMJ/Beech_Leafless/`
- `Textures/Things/Plant/AMJ/Shirabiso/`
- `Textures/Things/Plant/AMJ/Haimatsu/`

Vanilla retexture folders:
- `Textures/Things/Plant/AMJ/Retexture/TreeOak/`
- `Textures/Things/Plant/AMJ/Retexture/TreeOak_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/TreeMaple/`
- `Textures/Things/Plant/AMJ/Retexture/TreeMaple_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/TreePoplar/`
- `Textures/Things/Plant/AMJ/Retexture/TreePoplar_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/TreeBirch/`
- `Textures/Things/Plant/AMJ/Retexture/TreeBirch_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/TreeWillow/`
- `Textures/Things/Plant/AMJ/Retexture/TreeWillow_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/TreePine/`
- `Textures/Things/Plant/AMJ/Retexture/TreeBamboo/`

Medieval Overhaul conditional retexture folders:
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatOak/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatOak_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatIter/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatIter_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatFir/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatWillow/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatWillow_Leafless/`

Terrain:

- `Textures/Terrain/Surfaces/AMJ_ThinSoil.png`

World:

- `Textures/World/Biomes/AMJ_WarmTemperateForest.png`
- `Textures/World/Biomes/AMJ_CoolTemperateForest.png`
- `Textures/World/Biomes/AMJ_SubalpineForest.png`
- `Textures/World/Biomes/AMJ_AlpineZone.png`

The corresponding Def paths should only be switched after each actual asset exists, so the repository never points to missing textures.

Vanilla path replacements should live in an AMJE-owned retexture patch. Medieval Overhaul path replacements must be isolated in an optional compatibility patch guarded by MO's package ID. No MO source asset is copied or edited; AMJE supplies its own replacement textures and changes only the loaded graphic paths when both mods are active.

## Acceptance

Automated/static checks can verify:
- expected paths exist in Def XML;
- no placeholder Vanilla texture path remains once a target is converted;
- runtime loads without missing-texture ERRORs.

Human visual review is still required for:
- scale;
- silhouette readability;
- tiling seams;
- color/contrast;
- whether Warm/Cool/Subalpine/Alpine are distinguishable on the world map.

This is one of the few project stages where manual visual review is intentionally required.

## Adopted style baseline

The author accepted the final ENV-010 atlas direction after iterative comparison.

The accepted characteristics are:
- smoother/high-resolution rendering than the rejected pixel-like pass;
- stronger simplification/deformation than the initial more naturalistic concept;
- lower saturation than the initial concept, but without shifting the established warm/cool hue relationships;
- rounded, readable foliage masses and simplified branch/trunk shapes;
- enough internal shading to remain game-art rather than flat iconography;
- consistent treatment across AMJE, Vanilla retextures, Medieval Overhaul retextures, low vegetation, and world/terrain examples.

The atlas itself is a **style/reference board**, not a shippable sprite sheet. Production assets are generated or painted individually from this baseline so that transparency, crop, scale, and RimWorld path structure can be controlled per Def/state.

