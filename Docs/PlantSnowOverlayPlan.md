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

### Revision 2 rejected for branch mismatch — revision 3 pending

The author rejected the twelve-cap repair because it did not consider actual branches. Proximity to any wood pixel was insufficient: a horizontal snow bbox can overlap a branch while its lower edge still floats or crosses the branch direction. Revision 2 is invalid as a production or style reference.

Revision `v3-review` uses eight narrow selected branch corridors on the unchanged master. For every column, trace the original opaque upper surface and use it as the snow contact boundary; upper snow thickness tapers along that boundary rather than imposing an independent horizontal cap. Keep branches below visible. Validate contact along at least 90% of the entire lower boundary with no more than one-pixel AA tolerance, not a fraction of the snow interior. The new partial-support regression rejects the earlier insufficient criterion. Current candidate has full lower-boundary contact on all eight shapes; that proves geometry only, not author visual acceptance. All statuses remain pending/review.

### Revision 4 — snow foreground occlusion, still unapproved

Author reported that revision 3 looked as though snow was a lower layer. Binary composition was already master -> snow; the visual error was stopping the cap at the wood silhouette, leaving the original upper outline/wood face foreground-visible. Revision 4 retains the same eight branch corridors and unchanged mask/master, but extends an opaque four-pixel snow lip over the original upper branch face. A broad blue-grey front plane and darker contour make the cap thickness visible. The original upper outline and its adjacent wood are covered, not redrawn above snow.

Physical branch support and final visible snow rim are distinct: a front lip may overhang the support edge. Do not force that rim to end at the original silhouette or restore the original wood contour in front. Validate supporting branch overlap across cap width and require opaque snow over the original upper surface/adjacent face. Exact master -> snow composition and a wood-over-snow negative fixture lock order separately. Revision `v4-review` remains REVIEW with no accepted filled exemplar or native visual acceptance.

### Revision 5 — wider whole-tree accumulation, still REVIEW

Author requested more snow overall while retaining the limited accumulation appropriate to branches. Extend the eight existing branch corridors with seven upper/outer/lower ledges, for fifteen supporting corridors and fourteen connected snow masses. Keep foreground lips and original-branch occlusion; the finest outer-left twig uses a smaller lip. Do not fill the canopy, coat all twig tips or paint the vertical trunk.

Solid snow coverage increases from 2108 to 3628 pixels (about 1.72x), distributed across the full crown rather than thickening only existing patches. Original master and accepted other variants stay unchanged. Additional allowed corridors are declared before painting in an unapproved `v5-review` contract; no active/approved mask is modified and no mask is widened from output leaks. Support, opaque upper-wood-face occlusion, mass/outline, layer-order and PNG gates remain required. No source/native visual acceptance is claimed.

### Revision 5 source accepted as compromise (2026-10-06 JST)

Author: `これで妥協する`, referring to the presented revision-5 source composite. Record source-art acceptance as a compromise, not a newly completed native-game review. Freeze current image bytes, master/mask and source composition. Template is `v5` / `source_approved`; approved source hash and statement are registered. Keep production activation/native snow coverage distinct; leafless snow ledger remains pending for the native appearance of these exact bytes. No further art adjustment is authorized by this acceptance. See `ValidationEvidence/BeechLeaflessSnowSourceAcceptance.md`.
