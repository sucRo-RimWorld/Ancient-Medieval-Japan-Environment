# Species-aligned snow overlays — proposed design

Stage: accepted base forms may be reused for local snow-only painting and deterministic composition. New ImageGen calls additionally require explicit generation authorization under Core FixedImageTemplates. Per-variant review is recorded in PlantVisualCoverage.json; this plan itself grants no approval.

Keep accepted normal and leafless masters intact. Native PlantProperties provides
snowOverlayGraphicPath, leaflessSnowOverlayGraphicPath and
immatureSnowOverlayGraphicPath; TreeBase/BushBase do not supply these assets.
Do not confuse native ground snow with snow artwork aligned to each plant.

Start with Haimatsu: five coarse asymmetric pads retain their accepted positions,
size and branch identity. Place broad rounded off-white snow caps on upper-facing
surfaces, with restrained blue-grey shadow and a dark edge. Leave foliage sides
and the original branches visible. No flakes, icicles, ground snow or scenery.
Keep normal drawSize 2.60. Snow is a separate transparent aligned overlay, not
paint baked into the normal sprite. Exact compositing/masking must retain the
approved master pixels outside snow coverage; generator similarity is insufficient.

After Haimatsu source and runtime approval, follow the same shared style for
Shii crown tops, Shirabiso branch tiers, and Beech leafy/leafless branch surfaces.
Account for immature snow routing against the actual loaded graphics. Before
claiming any snowy state complete, validate decoded PNGs, alignment, native
overlay routing and appearance on the fixed-noon/Clear paused comparison map.

The base-art shared prompt's no-baked-winter-snow rule continues to apply to
normal sprites. Snow-overlay candidates explicitly replace that output constraint
with isolated snow shapes registered to the unchanged reference silhouette.

## Leafless beech repair and prevention (2026-10-06 JST)

- Keep the accepted master PNG, canvas, trunk position, exposed branch pixels, perspective and scale exact. Read Core ArtStyle and Environment ArtDirection on current main; actually open Sudajii, leafy/leafless beech and the accepted Haimatsu snow composite.
- Select a small number of physically supported upper branch ledges; vary width, thickness and left/right balance. Snow reads as broad irregular upper surfaces with thickness, not strokes along every twig, tiny pellets, detached flakes, or identical pill shapes. Use a strong dark outer edge, one off-white plane and one broad blue-grey underside; minimize internal outlines.
- Define allowed review regions before painting. Never derive the editable mask from the output alpha or expand it just to pass a fixed-pixel gate. Changing an already approved mask requires a new review revision; never silently replace an active contract. The old automatic output-alpha mask and rejected derivative are not valid references for this repair.
- The current `v2-review` contract declares twelve branch ledges in `Scripts/Art/build_beech_snow.py`. Its new mask is unapproved; existing v1 directory is retained for the user-specified PR review path. The master hash is unchanged. No filled exemplar or active status may be added before explicit author acceptance.
- Compare 256px and approximately 64px on the same neutral background: shared style, dark outline weight, few internal planes, identical base form/perspective, supported snow masses, unpainted twig tips and transparency. Base pixel equality is necessary but does not certify natural snow appearance.
- Run `Tests/validate_beech_snow.py` for mask hashes, binary regions, protected RGBA, exact overlay/composite, contiguous mass size/count, outline presence and support. Its rejection fixtures must reject both thin stripes and scattered pellets; these numeric checks remain a structural guard, not a visual approval.
- Keep snow coverage pending, clear the leafless filled exemplar, and mark any prior runtime snow log historical after changing bytes. Leafy snow and unrelated accepted states remain intact. Strict all-state completion must fail until the author reviews the changed native snow state.

### Confirmed failure path

`853d7f5` introduced per-edge capsule synthesis; `62434d2` wrote its output. It used connected wood-edge fragments, generated many small shapes with nested light/shadow rims, and built the mask from the resulting alpha. The regression checked only eroded-area ratio: filled pellets pass that measure. Neither visual outline/mass audit nor approval was enforced by that numerical success. This establishes the cause for the actual PR candidate; an unspecified earlier chat-generated image without a retrievable artifact/prompt cannot be attributed to a model or reference error.

Latest main had already removed the frozen shared prompt in favor of Core ArtStyle, while this PR retained the older generation entry and duplicated style wording. Use the current main entry plus owning style guides; do not restore the frozen prompt during future merges. An older ArtDirection claim that both snow variants were accepted is expressly superseded by the rejection section.
