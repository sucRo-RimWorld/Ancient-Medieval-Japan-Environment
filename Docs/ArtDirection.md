# AMJ Environment — Final Art Direction

## Current accepted production (2026-10-06 JST)

All four AMJE species have accepted normal, immature, UI and snow appearances.
Beech leafless/autumn appearances are also accepted; seasonal mechanics are
verified by bounded native calendar sampling. Docs/PlantVisualCoverage.json is
the current state record, and --require-complete passes with zero pending rows.
Historical trial/review entries below do not override these final approvals.
Haimatsu retains drawSize2.60, visualSizeRange0.45..0.75 and an explicit UI path
for native UI scale1. Existing Vanilla/MO retextures remain post-release work.


## Scope

Environment intentionally deferred final art until the functional/balance Alpha loop was stable.

The final-art pass currently tracks **29 primary art targets / baseline states**, but this is **not an exact final PNG count**. The scope was expanded after reviewing the Vanilla and Medieval Overhaul trees that actually appear alongside AMJE vegetation. For any prerequisite tree Def, every additional loaded graphic state that can appear in normal play is part of the same retexture family and must be covered or explicitly documented as an exception.

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

These are the baseline mature/leafless targets used to track art direction. They intentionally affect the corresponding Vanilla tree Defs wherever they appear while AMJE is active. Creating duplicate AMJ-only tree Defs solely to change graphics would add unnecessary biome/content duplication and compatibility cost.

**State-family completion rule:** before any Vanilla tree Def is marked fully retextured, inspect its loaded RimWorld 1.6 `PlantProperties` and include every non-empty state that can be visible in normal play: base, leafless, immature, polluted, leafless-immature, snow-overlay, leafless-snow-overlay, and immature-snow-overlay as applicable. Do not invent a state that the Def does not use. This rule supersedes treating the numbered list above as the exact number of production PNGs.

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

## AMJ / Medieval Overhaul visual baseline

Environment inherits the AMJ-wide in-game art direction from `Ancient-Medieval-Japan-Project/Docs/ArtStyle.md`. The shared AMJ target is to **look at home beside Medieval Overhaul, not beside RimWorld Vanilla**. Medieval Overhaul is therefore the primary visual calibration reference for AMJ in-game sprites/textures; Vanilla is not the target style.

For AMJE specifically:
- AMJE-owned structural plants must remain compatible with that shared AMJ / Medieval Overhaul-oriented visual language;
- Vanilla vegetation retained by AMJE should be retextured toward the same AMJ baseline when its visual pass is performed, rather than pulling AMJE art back toward Vanilla;
- optional Medieval Overhaul retextures are style-harmonization work inside the same baseline, not a separate visual direction;
- while only part of the retained vegetation has been retextured, a visible contrast against untouched Vanilla art can be a temporary rollout mismatch. Judge whether AMJE art itself is on-style against the accepted AMJ/MO references before deciding to redraw it toward Vanilla.

Environment's tree/plant rules below intentionally allow controlled class differences such as restrained internal gradient variation, but they do not change this project-wide target.

## Plant technical direction

Current plant Defs use `Graphic_Random`.

RimWorld's `Graphic_Collection` loads every texture found inside the configured folder path. A folder containing a single valid texture therefore remains compatible with `Graphic_Random`; additional variants can be added later without changing the Def class.

Alpha art policy:
- begin with **one strong final texture per state** rather than producing many variants immediately;
- use the **adopted ENV-010 atlas style** as the visual baseline for AMJE, Vanilla retextures, and optional MO retextures;
- the adopted style is rounded and simplified rather than botanically literal, with smooth high-resolution edges rather than pixel-art;
- saturation is reduced relative to the first concept pass, while the approved hue relationships remain intact;
- use a restrained but not posterized palette, limited base/shadow/highlight planes with the restrained tree-volume variation defined below, and clear silhouette separation at RimWorld gameplay zoom;
- keep transparent backgrounds;
- no baked terrain, UI border, text, or decorative frame;
- use RimWorld-readable top-down/three-quarter plant silhouettes rather than botanical illustration plates;
- preserve clear empty transparent space around branch/needle edges to avoid a rectangular sprite appearance;
- match existing plant scale through `graphicData.drawSize` / `visualSizeRange` rather than baking excessive canvas padding;
- avoid photographic rendering; target the simplified game-sprite look of the approved AMJE tree references;
- do not bake dynamic snow into plant textures.

Production texture transfer, validation, Def-switch ordering, and focused runtime review must follow `Docs/GoldenPaths/TextureAssetPipeline.md`. Do not invent a per-asset binary-transfer path after the source art is approved.

## Generation entry and new-chat continuity

Before a plant/tree generation call, follow [RetextureGeneration.md](GoldenPaths/RetextureGeneration.md). That procedure reads the current Ancient-Medieval-Japan-Project `Docs/ArtStyle.md`, this document, the current coordination state, and the actual accepted reference images. Do not preserve a separate frozen style prompt in this repository.

## Retexture visual-style rules

Shirabiso snow trial (2026-10-05): accepted master unchanged; its own seven
foliage regions define the snow mask. White/blue-grey snow palette reused from
accepted Haimatsu, upper branch surfaces covered, trunk/sides/outer AA protected.
Current snow overlay SHA-256 b2778ae82c1756e3a48715adb80dc289ce7a96dc38bdb12401f33ee0daca2c56.
Registry Art/Templates/Shirabiso-Snow-v1/template.json remains REVIEW; no snow
visual acceptance inferred. Native snow display remains depth >0.8.

Haimatsu master-registered snow (2026-10-05): previous approximate snow alignment
was rejected by author (`まだずれてる。`). Current review overlay uses a binary
mask registered to the accepted master's own five foliage pads, with generated
snow material colors and the master's broad shade blocks. Current SHA-256:
f77fb8d8db633c5c3b7452927e61bbde152a531fbb351c47f0a721587eef6147.
Registry: Art/Templates/Haimatsu-Snow-v1/template.json (review, not active).
Protected master RGBA differences=0; native snow threshold and drawSize unchanged.
Final in-game appearance remains pending.

Haimatsu snow derivative trial (2026-10-05): author authorized proceeding from
the illustrative combined preview with `良さそうな気はするのでそれで進めて。`.
Separate snow-only overlay installed at Things/Plant/AMJ/Haimatsu_Snow, SHA-256
e17099fe45e6106bf0a3a1bd07c40352b8e8d1ec1ad679923d060b083305bdc0.
Normal master bytes/drawSize remain intact. Native snow overlay routing and
in-game appearance are reviewed separately; illustrative preview is not exact
compositing/pixel-registration evidence.

Snow registration correction (2026-10-05): author reported slightly offset snow
and instructed continuation. Snow-only image edited with the accepted Haimatsu
master as positional reference; cap coverage reduced to upper pad surfaces.
Current overlay SHA-256 e1037f0a6c1c264dd10949e01f604bd50df75966147957482a0587041a3d4f7f.
Actual normalized pixel composite retained at
Art/Candidates/Haimatsu-Snow/Haimatsu_Snow_ExactComposite.png. Master pixels/size
remain unchanged; final in-game snow appearance is still pending acceptance.

Accepted-form derivatives (author rule, 2026-10-05): after a tree's shape is
accepted, generate derived states without renewed pre-generation approval.
Retain its established form/scale/branch identity and shared style as appropriate
to the state. This authorization does not replace final in-game appearance review.

These rules are the canonical visual baseline for AMJE tree/plant retextures. Individual species may add stricter requirements, but they should not redefine the shared style from scratch.

### Shared style

AMJE tree/plant sprites inherit the project-wide invariants in Ancient-Medieval-Japan-Project `Docs/ArtStyle.md`: silhouette-first simplification, strong readable outlines, restrained palette/detail, no photographic or painterly surface treatment, and readability at gameplay scale.

Environment-specific additions/controlled differences are:

- use stronger structural deformation/simplification than botanical-plate realism;
- use a clearly readable near-black or very dark local-color outer outline;
- keep saturation low-to-medium against RimWorld terrain;
- use limited base/shadow/highlight steps; **restrained soft gradient variation is allowed for tree/plant volume**, which is an explicit class difference from the flatter Core crop/item budget;
- use smooth high-resolution edges and transparent backgrounds;
- do not bake in scenery, UI, labels, terrain tiles, turf/grass bases, unrelated ground clutter, or winter snow.

These additions narrow the shared AMJ style for Environment vegetation; they do not replace it.

### Species differentiation

Species must differ through structure, not only through recoloring.

Use a combination of:
- overall silhouette;
- crown width/height;
- crown density;
- branch visibility;
- leaf/needle mass size and arrangement;
- trunk color, thickness, and visible proportion;
- branching rhythm;
- upright versus spreading growth habit.

A recolor of an otherwise identical silhouette is not sufficient for a distinct tree species.

### Trunk and branch treatment

- Use trunk/branch color and shape as a species cue, but keep bark treatment simplified.
- Express bark mainly through **color blocks, value changes, trunk thickness, and branching structure**, not fine etched texture.
- Sparse/open crowns should expose more branch structure; dense evergreen crowns may hide more of it.
- Branch tips must remain thick/simple enough to survive RimWorld zoom reduction.

### Seasonal continuity

For deciduous trees:
- keep the normal leafy source in neutral healthy foliage rather than baking in autumn/red/orange seasonal color when Vanilla seasonal tinting already provides that behavior;
- leafy and leafless states must still read as the **same individual species and approximate tree form**;
- preserve corresponding trunk placement, crown width, overall height, and major branch identity between the two states;
- do not make the leafless state look like a separate species or a differently scaled tree.

Evergreen base textures must likewise avoid baked winter snow; snow/weather remain runtime systems.

### Ground contact

- End the sprite at the trunk/root/branch silhouette on transparency.
- Decorative green circles, turf pads, grass rings, and similar base markers are not part of the AMJE tree style.
- Ground contact should read through the actual plant form rather than an added platform.

### Category baselines

#### Evergreen broadleaf trees

- comparatively dense/heavy crown;
- broad foliage masses;
- stable visual weight;
- avoid evenly spaced round blobs or ornamental symmetry.

Sudajii is the current accepted baseline for this category.

#### Deciduous broadleaf trees

- generally more open than the evergreen-broadleaf baseline;
- allow visible gaps and readable branch structure;
- species identity should remain clear in both leafy and leafless states.

Japanese beech is the current accepted baseline for this category.

#### Tall conifers

- retain a clear vertical/conifer identity;
- use layered, bundled, or irregular needle masses rather than perfect bilateral symmetry;
- avoid an oversized decorative "Christmas tree" silhouette;
- remain wide/readable enough not to collapse into a thin line at gameplay zoom.

Shirabiso should use this category while remaining distinct from Vanilla Pine.

#### Low/dwarf conifers

- read immediately as low and spreading rather than as a scaled-down tall conifer;
- favor horizontal, creeping, wind-shaped, or matted growth;
- keep the silhouette clearly wider than tall when appropriate to the species;
- do not imply a normal timber-tree form when the gameplay/ecological role is shrub-like.

Haimatsu is the current accepted target for this category.

### In-game readability

Final human review is performed at normal gameplay zoom. Prioritize:
- silhouette/species distinction;
- scale relative to neighboring vegetation;
- outline weight;
- palette/saturation against actual terrain;
- branch/foliage readability;
- UI icon readability;
- leafy/leafless continuity where applicable.

Fine details that are visible only when the PNG is enlarged are not a reason to increase complexity.

### Prohibited / strongly discouraged directions

Unless a later asset-specific decision explicitly overrides them, avoid:
- botanical-plate realism;
- photographic rendering;
- pixel-art/dithered treatment;
- excessive gradients or many small color steps;
- recolor-only species differentiation;
- decorative ground bases;
- highly symmetrical ornamental tree shapes;
- background/scenery baked into the sprite;
- saturation high enough to detach the plant visually from the map.

### Baseline references

Use the currently accepted Sudajii and Japanese beech work as the first comparison point for future tree retextures:
- Sudajii establishes the evergreen-broadleaf baseline: dense chunky crown, muted palette, thick outline, no ground ring.
- Japanese beech establishes the deciduous-broadleaf baseline: more open/spreading crown, visible branches, pale grey-beige trunk, restrained lighter foliage, matching leafy/leafless identity.

Future tree work should first state its category and the specific structural differences from these accepted baselines before source-art generation begins.

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
- **Accepted in-game on 2026-10-04:** the final 256x256 Sudajii sprite is approved for production after WarmTemperate visual review; scale, silhouette, muted palette, thick outline, transparent base, and removal of the green ground ring are all accepted.

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

Accepted source-art direction:
- visibly more open and horizontally spreading than Sudajii;
- smaller foliage masses with branch structure visible between them;
- smooth pale grey-beige trunk rather than warm orange-brown bark;
- low-saturation yellow-green/olive foliage, lighter than Sudajii;
- thick near-black outline and simplified AMJ crop/Sudajii visual weight remain shared;
- no baked ground/grass base;
- leafy source sprite approved by the author on 2026-10-04 and integrated at `Textures/Things/Plant/AMJ/Beech/Beech_A.png`;
- **Focused in-game review accepted on 2026-10-05:** after the decoded-PNG repair in `1a24e11755c24c2109b7ceebd9628601c62f1f12`, the author reported no issues with the CoolTemperate focused check. The current leafy beech is accepted for production appearance. The report did not separately identify a forced leafless/seasonal-switch check; keep that specific coverage distinct from the confirmed focused appearance review.

### Japanese beech — leafless

Visual role:
- same approximate trunk/crown proportions as the leafy beech;
- branching structure should align closely enough that the seasonal switch does not look like a different species/size;
- no foliage.

Accepted source-art direction:
- preserve the leafy sprite's pale grey-beige trunk identity and overall crown width;
- expose a broad, irregular branching fan rather than a narrow upright silhouette;
- keep branch tips simplified and thick enough to remain readable at game zoom;
- leafless source sprite approved by the author on 2026-10-04 and integrated at `Textures/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png`; in-game seasonal-switch validation remains pending.

### Shirabiso fir

**Source approved 2026-10-05:** author approved the generated standalone evergreen sprite. Compact asymmetric layered conifer crown, muted grey/blue-green needle masses, grey-brown exposed trunk, thick dark outline, transparent background without ground/snow. Production texture: `Textures/Things/Plant/AMJ/Shirabiso/Shirabiso_A.png` (256x256 RGBA). SHA-256: `aaef0a8db34426e028fdebe4a69efd3aadbc17ed98875e1f74c1bc2da4147d3d`. This is a distinct species silhouette; no shared fixed component is declared. **Focused in-game appearance accepted 2026-10-05:** the author replied `OK` after the Subalpine focused-check instruction. Record reported appearance acceptance; this does not supply a new automated runtime log or full-suite result.

Visual role:
- narrow subalpine evergreen conifer;
- layered but compact conical crown;
- colder/darker needle mass than ordinary broadleaf trees;
- visually distinct from Vanilla Pine rather than merely recoloring it.

Avoid:
- oversized Christmas-tree symmetry;
- extremely tall narrow sprite that becomes unreadable at game zoom.

### Haimatsu dwarf pine

**Current production accepted in game 2026-10-05:** author accepted the further-deformed five-mass Haimatsu with `採用で`. Production: `Textures/Things/Plant/AMJ/Haimatsu/Haimatsu_A.png` (256x256 RGBA), SHA-256 `a20cac361b08b19b0892d2dcdf88bf40257bf186d3661661941e090f19bf18b6`, drawSize=2.60. Thick dark outlines, coarse rounded foliage pads, simplified woody branches and restrained shading; transparent/no ground/snow. Focused Alpine appearance is accepted; dedicated winter/snow appearance review is not claimed. No shared fixed component is declared.

Visual role:
- low, spreading, wind-shaped alpine shrub;
- clearly wider than tall;
- creeping/matted pine structure rather than a miniature upright tree;
- no wood-harvest visual implication.

This is the most important silhouette distinction for keeping the alpine zone above the treeline.

## Retextured plant description policy

This section is the ENV-010 application of the AMJ-wide historical description policy in Ancient-Medieval-Japan-Project `Docs/HistoricalDescriptionGuidelines.md`.

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

Japanese opening-name rule (shared AMJ policy): when an established kanji form exists, put it at the start of the description; include recognized aliases / alternate names and common alternate written forms there as well. Do not invent kanji or weakly sourced names. Current structural-plant name evidence: Forestry and Forest Products Research Institute identifies ブナ as 山毛欅 with aliases シロブナ / ホンブナ; Forestry Agency material records 橅 / 椈 as alternate writings, スダジイ aliases イタジイ / ナガジイ, シラビソ alias シラベ, and ハイマツ as 這松. Dictionary references record シラビソ as 白檜曽. English text is translated from the approved Japanese opening rather than independently normalized.

Historical-description research basis for the current four-plant Japanese draft:
- **Sudajii:** FFPRI identifies it as a warm-temperate evergreen canopy tree, records the aliases イタジイ / ナガジイ, and lists timber, bark-tannin/dye and edible-seed uses. Vegetation-history research places Castanopsis / evergreen broadleaf forest in western Japan by the Early Jomon; the Nara National Research Institute cultural-property database records Early-Jomon use of Castanopsis fruits at the Ireibaru site in Okinawa.
- **Japanese beech:** FFPRI identifies ブナ as 山毛欅, aliases シロブナ / ホンブナ, and a principal cool-temperate deciduous tree. Vegetation-history research records Fagus in Jomon cool-temperate forests. A Nagano archaeological report records Fagus-genus wood among selectively used turned wooden vessels / lacquerware from the late Heian to medieval period. Forestry Agency material documents modern interior/furniture, mushroom-log and fuel uses.
- **Shirabiso:** University of Tokyo and FFPRI sources identify 白檜曽 / シラビソ（シラベ） as a major subalpine evergreen conifer. Current Forestry Agency / Environment Ministry descriptions of Mt. Ontake record Shirabiso-dominated subalpine forest, while scholarship on Ontake worship places organized medieval worship / ascent in the later medieval period. The draft therefore describes the forest as part of the mountain-religion landscape without claiming a specific timber use.
- **Haimatsu:** Forestry Agency and Ministry of the Environment sources identify 這松 as a representative alpine dwarf pine above the treeline. Botanical-history research treats its Japanese alpine distribution as a northern cold-climate lineage left at high elevation after postglacial warming. Current Mt. Ontake vegetation includes Haimatsu above the treeline; the historical sentence separately notes the mountain's later-medieval worship history rather than claiming direct medieval use of Haimatsu.

Key references:
- FFPRI Sudajii: https://www.ffpri.go.jp/kys/business/jumokuen/jumoku/zukan/sudajii.html
- FFPRI distribution maps: https://www.ffpri.go.jp/labs/prdb/sudazii.html , https://www.ffpri.go.jp/labs/prdb/buna.html , https://www.ffpri.go.jp/labs/prdb/sirabiso.html
- FFPRI beech name: https://www.ffpri.go.jp/fsm/business/jumokuen/06_ha/buna.html
- Early-Jomon Castanopsis use (Nabunken): https://heritagemap.nabunken.go.jp/statistic/64887-%E4%BC%8A%E7%A4%BC%E5%8E%9F%E9%81%BA%E8%B7%A1.html
- Jomon vegetation / wood use: https://doi.org/10.4116/jaqua.36.329
- Medieval turned-wood evidence (Nagano archaeological report): https://sitereports.nabunken.go.jp/files/attach/9/9472/7427_1_%E5%B1%8B%E4%BB%A3%E9%81%BA%E8%B7%A1%E7%BE%A4.pdf
- Forestry Agency beech modern use: https://www.rinya.maff.go.jp/kanto/joetu/invitation/invitation/shinetutrail.html
- Mt. Ontake vegetation: https://www.rinya.maff.go.jp/chubu/policy/business/conservation/hogorin/2-20.html
- Later-medieval Ontake worship: https://doi.org/10.57492/sangakushugen.42.0_5
- Haimatsu alpine ecology: https://www.rinya.maff.go.jp/tohoku/syo/huzisato/zukan/haimatu.html
- Alpine-flora history / Haimatsu: https://doi.org/10.18942/bunrui.KJ00004872189

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

For any additional loaded state discovered during integration, use the same AMJE-owned family with an explicit state suffix, e.g. `TreeOak_Immature`, `TreeWillow_Polluted`, `TreePine_Immature`, rather than falling back to the original/broad-pack art. The exact folders are created only for states that the loaded target Def actually uses.

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

Vanilla path replacements should live in an AMJE-owned retexture patch and point to AMJE-owned unique texPaths. Medieval Overhaul path replacements must be isolated in an optional compatibility/retexture patch guarded by MO's package ID. No MO source asset is copied or edited; AMJE supplies its own replacement textures and changes only the loaded graphic paths when both mods are active. Same-name texture shadowing is not the canonical AMJE mechanism. If ReGrowth, VTE, or another active Mod explicitly patches the same loaded Def field, AMJE's ownership contract requires the final loaded path for an AMJE-owned target to resolve to the AMJE path; add only the narrow load-order/compatibility rule needed for that real conflict.

## Acceptance

Automated/static checks can verify:
- expected paths exist in Def XML;
- every owned non-empty loaded graphic state resolves to the expected AMJE path;
- no placeholder Vanilla/MO or broad-pack path remains on an owned state once that state is converted;
- optional MO patches are absent/harmless when MO is inactive;
- runtime loads without missing-texture ERRORs or BadTex.

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

## Fixed reused components

When a future Environment asset intentionally reuses a visible component pixel-exactly, follow Ancient-Medieval-Japan-Project `Docs/GoldenPaths/FixedImageTemplates.md`. Same-style but structurally distinct species do not use fixed-pixel templates.

### Revision 5 native appearance accepted; leafless snow complete (2026-10-06 JST)

Author: `ブナOKなのでPRマージして`. This explicitly closes the separate native-appearance review for the already frozen revision-5 leafless-beech snow source. Keep exact composite SHA-256 `24a10f6a45ed200170a5dabf49cabd2bacecfa3bfaaa1679300e64204b933126`, installed snow overlay SHA-256 `3cc4da672499e8bb7f96847623793050ab136b766cd91713a7921eda66dc496b`, and leafless master SHA-256 `24f8664bdedd3ebd0dee58fe627439b3784b5ecc599aaf49d750832f315be112`. Template v5 is ACTIVE with the approved filled exemplar. The Beech snow ledger row is accepted for both leafy and leafless variants; all current AMJE structural-plant visual states are complete. This approval authorizes PR #5 merge but does not fabricate a new runtime-test PASS.
