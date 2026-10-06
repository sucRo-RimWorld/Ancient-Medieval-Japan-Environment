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

Before a plant/tree generation call, follow [RetextureGeneration.md](GoldenPaths/RetextureGeneration.md). It requires reading the current rules, viewing and attaching accepted source references, proposal-before-generation sequencing, the complete reusable prompt, and candidate review. This applies equally in a new chat and an ongoing chat; remembered text or unviewed filenames do not replace the actual baseline. This file remains the visual source of truth; the generation procedure must carry its shared/category/species rules into the actual request.

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

- Prefer a **strongly simplified/deformed game-sprite treatment** over botanical realism. The target is recognizable species character at RimWorld gameplay zoom, not illustration-plate accuracy.
- Build the sprite from **large readable masses**. Avoid dense micro-detail in individual leaves, bark, twigs, or needles that disappears at normal zoom.
- Use a **clearly readable dark outer outline**, generally near-black or a very dark local hue. Do not thin the outline merely to make the asset look more naturalistic.
- Keep the palette restrained and generally **low-to-medium saturation** so the asset sits naturally on RimWorld's brown/grey terrain and beside Vanilla/MO vegetation.
- Keep color steps limited. As a baseline, think in terms of **base color + shadow + highlight**, with only restrained soft gradient variation where it improves volume. Do not add colors simply to increase detail.
- Avoid very bright lime foliage, highly saturated orange bark, or other accents that make the sprite look pasted on top of the map.
- Use transparent backgrounds. Do not bake in scenery, UI frames, labels, terrain tiles, circular turf bases, decorative grass rings, or unrelated ground clutter.
- Prefer smooth high-resolution edges. Do not intentionally introduce pixel-art/dithered rendering unless a separate approved asset class requires it.
- Do not use photographic or photo-derived surface detail as the final visual language.

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


## Pixel-exact reused components (AMJ shared policy)

Follow the Core source of truth [FixedImageTemplates.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/GoldenPaths/FixedImageTemplates.md). Same style does not mean identical parts: species-specific silhouettes remain distinct. Where a trunk, container, title or other component is reused, register an approved lossless master and binary editable mask with SHA-256 hashes in the owning repository, generate only variable material, and composite deterministically. Final decoded protected RGBA pixel differences must be zero; reference-image editing/visual similarity cannot replace this check. Use Core `Scripts/Art/fixed_template.py` for compositing/validation, then retain Environment's PNG integrity/install gates. Read the master/manifest and actually view the approved reference in every new chat. No shared-pixel guarantee may be claimed before the family template and output pass this gate. Existing proposal approval stages are unchanged.

### Haimatsu normal-zoom refinement — 2026-10-05

Author accepted the in-game size at graphicData.drawSize=2.60 (visualSizeRange remains 0.45..0.75). The current production image is too detailed relative to neighboring Vanilla plants and rocks at that scale. Refinement direction: preserve low, wide, asymmetric creeping-pine structure and muted olive/grey-green palette; consolidate needle foliage into fewer large, smoothly lobed masses, reduce serrated micro-edges and internal shading subdivisions, and simplify visible woody branches. Keep thick dark outlines, transparent background and no ground base. A revised source candidate requires review before production replacement.

Haimatsu further-deformed source accepted in game on 2026-10-05 with `採用で`: five broad foliage pads, coarse rounded lobes, simplified branches and broad limited shading. Production SHA-256 is a20cac361b08b19b0892d2dcdf88bf40257bf186d3661661941e090f19bf18b6 with drawSize=2.60 and visualSizeRange=0.45..0.75. This is the accepted low/dwarf-conifer production reference. Preserve its normal-zoom readability; extra needle/edge/shading micro-detail is not an improvement.

Japanese beech leafless appearance accepted on 2026-10-05: author replied 問題なし after the focused leafless review. The naturally generated mature tree switched to LeaflessNow=True and loaded Beech_Leafless_A without runtime ERROR. This covers leafless appearance, not autumn color or automatic seasonal timing.

### Beech autumn palette trial — 2026-10-05
Author requested beech-like autumn colors. Trial uses golden yellow, yellow-orange and brown through a beech-only fall lookup texture; leafy/leafless source art, pale trunk and open crown remain unchanged. The palette follows observed beech yellow-to-brown phenology: https://tohoku.env.go.jp/blog/2014/01/1863.html . Autumn appearance and post-shader-change normal/leafless confirmation are pending; retain earlier approvals as historical evidence.

### Color review lighting baseline — author rule (2026-10-05)
Palette comparisons require fixed local 12:00, Clear weather after transition, a paused map and matching zoom/season/shader intensity. Record actual verified conditions following PlantVisualCoverage. Do not compensate source colors for uncontrolled sunset/night/weather lighting. Current beech autumn palette is pending controlled review; no further palette change is authorized by a lighting observation alone.

### Beech controlled autumn acceptance — 2026-10-05

Author accepted the golden-yellow/yellow-orange/brown beech palette with `では採用で。` after the local-noon Clear screenshot. Current beech-specific fall destination is `/Other/AMJE_BeechFallGradient`, SHA-256 `ea1f01ec5c6472c96b189ec7823dbfa10adfcc9f7cd3ef88e7b7e714d7a68c93`. Leafy/leafless source sprites and normal trunk/crown structure are unchanged. Accepted comparison conditions: verified localHour=12.000, Clear, weather transition=1, paused=True, camera rootSize=24, native fallIntensity=1, growth=1, ticksAbs=330000. Runtime evidence is `TestResults/BeechNoonClearReview/CoolTemperate.log`; author screenshot is `C:/Users/sucRo/AppData/Local/Temp/codex-clipboard-074fe799-01bd-45d3-934a-7b6b94d8ac35.png`. This accepts autumn appearance only; automatic seasonal transition and other pending rows remain unverified.
# Growth-stage acceptance (2026-10-05)

Author accepted the four-species 10%/50%/100% growth comparison with `問題なさそうに見える。` at local noon/Clear/paused. All four immature-state ledger rows are accepted against their current production fingerprints. Evidence: TestResults/PlantGrowthReview/Growth.log. Snow, icon and outstanding beech state reviews remain separate.

### Haimatsu master-registered snow accepted (2026-10-05 JST)
Author accepted the native snow screenshot with `問題なさそう`. Current snow overlay f77fb8d8db633c5c3b7452927e61bbde152a531fbb351c47f0a721587eef6147 retained. The snow ledger row is accepted against current fingerprint. Fixed-template v1 is ACTIVE with accepted exact composite recorded as filled exemplar; normal/immature pending rows are not inferred accepted from the snow screenshot. No further visual changes.

Shirabiso heavier snow trial: upper registered foliage-column coverage increased to 68% by author request; protected master pixels unchanged. Current overlay SHA-256 49a98c4b54c6f263c019352aabdbddd7f20cb9aa6b2960e1a23c861e2df31ec6. Final native appearance pending.

### Shirabiso heavier snow accepted (2026-10-05 JST)
Author accepted with `これ採用で`. Retain upper-column 68% coverage and overlay SHA-256 49a98c4b54c6f263c019352aabdbddd7f20cb9aa6b2960e1a23c861e2df31ec6. Shirabiso snow template v1 ACTIVE with accepted filled exemplar; master/branches/sides/outer-edge pixels unchanged. Other pending states are separate.

### Shii snow trial (2026-10-06)
Separate white/blue-grey snow caps reuse the accepted snow palette, registered to five crown anchors and the original master's foliage pixels. Trunk, branch identity, outline/scale remain fixed. Art/Templates/Shii-Snow-v1 is REVIEW, not active. Current snow overlay SHA-256 3b75aa90715df29ce4dcdc05b324e3ff8d280dd4aff332af55a4579f5ed5ed13. Final native appearance pending author review.

Existing Vanilla/MO tree retextures are deferred to post-release updates by author decision. Initial-release art work remains the four current AMJE plants and their required states.

Shii snow-shape correction (2026-10-06): previous geometric cap layout rejected. Current mask traces the master's seven upper-lit foliage masses directly; no artificial crown partition remains. Current snow overlay SHA-256 d2d7baaa71c3c14d48e4070d79c187eb8ffc636a336e4957abc6f13833da9616. Native appearance acceptance pending; master untouched.

### Shii snow accepted as compromise (2026-10-06 JST)
Author: `やや怪しいがこれで妥協`. Current overlay 5d9376ced78552f64631c89eda662aef51bd6faff4f1e098238ca0aa3c4b8bb7 retained; slight left foliage snow-shape concern remains. Snow row accepted only; template ACTIVE with exact filled exemplar. Native noon/Clear/paused/zoom18 verified, owned ERROR gate passed. Normal/immature/icon rows remain separate. Next image assets: beech leafy and leafless snow; existing retextures deferred.

Beech snow candidates (2026-10-06): leafy upper-foliage caps; leafless thin snow on exposed upper wood edges. Immutable normal/leafless masters preserved. Both variants pending native appearance review; fixed templates REVIEW.

### Beech leafy and leafless snow accepted (2026-10-06 JST)
Author: `OK` after paired native snow comparison. Both leafy and leafless snow accepted; corresponding templates ACTIVE with exact filled exemplars. Leafy overlay 221fdf06941864a7a366819042fe325b15ad587d5af9e3bb914a672b2f3d5f51; leafless overlay 3b6e3dabcf402483920ac406d54fa37aef9e9cea06a47bf0c15e9e66a230141f. Native state-specific paths loaded, noon/Clear/paused/zoom18 verified, owned ERROR gate passed. Snow row alone accepted; other states remain separate. All four current species now have accepted snow art. Existing retextures deferred; UI icons and pending rechecks remain.

Haimatsu UI scale fix (2026-10-06): explicit uiIconPath uses unchanged Haimatsu_A. Native UI scale becomes1; map drawSize2.60 and visualSizeRange0.45..0.75 retained. This corrects info-card overflow without changing accepted map appearance; native visual review pending.

### Haimatsu info icon accepted (2026-10-06 JST)
Author: `ハイマツOK` after actual native info-card review. Icon row accepted against current fingerprint. Explicit uiIconPath retained; live UI scale1, map drawSize2.60 and visual range0.45..0.75 verified. Image bytes unchanged; owned ERROR gate passed. Other pending states remain separate. Owned info-review game closed and fixture archived after acceptance.

### Remaining UI icons accepted (2026-10-06 JST)
Author: `OK` after native small/large icon panel. Shii/Beech/Shirabiso icon rows accepted; all four species UI icons now accepted. Native texture validity/noon/Clear/paused confirmed, owned ERROR gate passed. Map normal/immature rows remain separate; no completion claim.

### Normal/growth acceptance and duplicate-review prevention (2026-10-06 JST)
Author: `これ何回も見てるけどOK`. All four normal and immature rows accepted against current growth-only review. Repeated reviews were caused by whole-Def fingerprint invalidation after unrelated snow/UI additions. Fingerprints now ignore UI-only fields outside icon states and snow paths outside snow states, retaining size/art and other fields. Prior approval is restored only when exact former full fingerprint can be reconstructed, not from an assumed visual equivalence.

### Duplicate leafless review corrected; visual coverage closeout (2026-10-06 JST)
Prior explicit leafless approval `問題なし` confirmed in 2026-10-05 record and ledger prior_acceptance; original leafless PNG exactly identical. Approval restored, not new human acceptance. Overbroad fall-shader invalidation caused redundant review. Native state/material revalidated in BeechLeaflessFinal, fixed noon/Clear. Seasonal transition separately VERIFIED by bounded native calendar sampling; no human visual PASS or full-world-year simulation claimed. Existing Vanilla/MO retextures remain post-release scope.

### Leafless beech snow rejection supersedes earlier acceptance (2026-10-06 JST)

The author's rejection of `Art/Templates/Beech_Leafless-Snow-v1/exact-composite.png` supersedes the earlier paired `OK` **for the leafless snow variant only**. The earlier leafy snow approval and accepted base/leafless master remain intact. The first repair at `62434d2` is also unapproved: automatically placing a small capsule at each upper-edge component made scattered pellets with weak rims, rather than coarse supported snow masses.

Current replacement is a local, deterministically painted twelve-cap candidate, with dark outer contours and off-white / blue-grey color planes matched to the accepted Haimatsu snow. Cap widths and thicknesses vary; exposed branches, tree position, scale and perspective remain those of the unchanged leafless master. Sparse internal divisions replace layered capsule highlights. Template revision `v2-review` stays `production_status=review`, has no approved filled exemplar, and uses a separately declared unapproved ledge-region contract. It is not an active mask revision. Production snow bytes in this draft PR are for review only; no installed-game or human acceptance is claimed. See `PlantSnowOverlayPlan.md` for preflight, mask and audit rules.

### Revision 2 rejected for branch mismatch — revision 3 pending

The author rejected the twelve-cap repair because it did not consider actual branches. Proximity to any wood pixel was insufficient: a horizontal snow bbox can overlap a branch while its lower edge still floats or crosses the branch direction. Revision 2 is invalid as a production or style reference.

Revision `v3-review` uses eight narrow selected branch corridors on the unchanged master. For every column, trace the original opaque upper surface and use it as the snow contact boundary; upper snow thickness tapers along that boundary rather than imposing an independent horizontal cap. Keep branches below visible. Validate contact along at least 90% of the entire lower boundary with no more than one-pixel AA tolerance, not a fraction of the snow interior. The new partial-support regression rejects the earlier insufficient criterion. Current candidate has full lower-boundary contact on all eight shapes; that proves geometry only, not author visual acceptance. All statuses remain pending/review.
