# AMJE Retexture Generation — New-Chat Start Procedure

## Scope and authority

Use this procedure for AMJE plant/tree sprites, including AMJE-owned plants and
Vanilla/MO tree retextures. It is the generation entry point, before
[TextureAssetPipeline.md](TextureAssetPipeline.md).

The visual source of truth is [ArtDirection.md](../ArtDirection.md), especially
**Retexture visual-style rules** and the target species section. This procedure
controls how those rules reach the actual image-generation request.

Workshop covers, logos, terrain textures and world-biome images are different
asset classes. Read their owning specifications; never substitute these tree
rules for a cover's shared-left/addon-right layout. If the requested class is
unclear, resolve the class before generating.

## New-chat preflight — required before an image-generation call

1. Read repository `AGENTS.md` and authoritative `main:Docs/Coordination.md`
   first. Read the current main versions, not a remembered chat summary.
2. Read this procedure, `Docs/ArtDirection.md` (shared rules + target species),
   and `Docs/GoldenPaths/TextureAssetPipeline.md`.
3. Resolve the latest OPEN/IN PROGRESS art handoff and distinguish:
   proposed design / author-approved design / generated candidate /
   approved source / installed asset / accepted in-game appearance.
   A request to save rules or to start a new chat is not approval of a pending
   species design.
4. Fetch and actually view the accepted reference PNGs below. Do not merely read
   filenames, rely on an old conversation image, or claim to have inspected
   pixels that were not opened.
5. **Author exception (2026-10-05):** once a tree's shape is accepted, its derivative
   states may be generated without renewed pre-generation approval. Preserve the
   accepted form, scale, branch identity and shared style as appropriate to the
   state. Snow and other state variants still need asset validation and in-game
   appearance review; do not infer final acceptance from generation authorization.
   For a new or changed tree form, reuse the existing approved category/style. Present a concise target-specific
   composition/design proposal **before generation**, then obtain author approval
   for that proposal. If the same proposal already has explicit approval in the
   current handoff, retain it and proceed without asking for approval again.
6. Build the generation request from the complete shared prompt block below,
   plus the approved species differences. Attach the actual viewed reference
   images to the generator. A short handoff does not replace this prompt block.
7. State the target and inherited baseline briefly to the author, then generate.
   Do not restart the style design or produce unrequested alternatives.

When fetching/viewing a required reference is blocked, report that limitation
and resolve the missing reference before generation. Do not silently substitute
an unrelated source. The user must not be used to relay information between
repository workstreams.

## Accepted reference assets

Fetch these paths from the owning repository's current main. The listed blobs
identify the accepted baseline at this procedure's introduction; a later
documented author-approved replacement supersedes them.

| Category / reference | Repository path | Accepted Git blob |
| --- | --- | --- |
| Evergreen broadleaf / Sudajii | `Textures/Things/Plant/AMJ/Shii/Shii_A.png` | `62d795d9717a3ad4d61827bfb470cd7b3cfc7000` |
| Deciduous broadleaf / leafy beech | `Textures/Things/Plant/AMJ/Beech/Beech_A.png` | `7face00d1128333b5fa6e1ab419d3424bc21b6dd` |
| Deciduous structure / leafless beech | `Textures/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png` | `c9fdc9b87e6b559ab2a2ff9788b6d015593b8406` |

For Shirabiso and Haimatsu, Sudajii and leafy beech supply outline, simplification,
palette restraint and shading weight. Their broadleaf silhouettes are **not**
templates for conifer form. Use the tall-conifer or low/dwarf-conifer category
in ArtDirection for structure.

For an image tool accepting local references, pass the actual local paths in
its reference-image argument. If references are conversation images, include
the smallest recent-image set that contains every required reference. Do not
claim that written paths alone attach pixels to a generator.

## Complete shared generation prompt

Retain every constraint in this block in the actual generation request.
Replace bracketed fields only with the approved target-specific design. Do not
drop inherited rules when adding species detail.

```text
Create one standalone [SPECIES / STATE] RimWorld plant sprite.
The attached approved AMJE Sudajii and Japanese beech sprites are the visual
style references. Match their visual weight, simplification, outline thickness,
restrained saturation and shading treatment. Preserve the AMJE shared style;
apply the approved species structure below rather than copying a broadleaf
silhouette onto a conifer.

STYLE:
Strongly simplified/deformed game sprite built from large readable shape
masses. Thick, clearly readable near-black or very dark local-color outlines.
Low-to-medium saturation. For each material, use base color + shadow +
highlight, with only a subtle gradient where useful. Smooth high-resolution
edges. Readable at normal RimWorld gameplay zoom. Simplified trunk/branch color
blocks and structure; no fine etched bark, individual leaf/needle micro-detail
or dense twigs.

SPECIES STRUCTURE:
[APPROVED CATEGORY, SILHOUETTE, WIDTH/HEIGHT, BRANCH/FOLIAGE ARRANGEMENT,
TRUNK VISIBILITY AND PALETTE DIFFERENCES]
Species identity must come from structure as well as color. Avoid ornamental
bilateral symmetry. For a deciduous pair, preserve trunk position, scale and
major branch identity between leafy and leafless states.

COMPOSITION AND OUTPUT:
One isolated sprite with a transparent background, appropriate three-quarter
RimWorld plant view, complete unclipped silhouette and clear space around the
edges. No text, labels, UI, comparison sheet, frame or decorative ground base.
No scenery, grass ring, turf platform, baked ground shadow or baked winter snow.
Use neutral healthy foliage for deciduous base art; seasonal tint is applied
in game. Production target is a validated 256x256 transparent PNG, following
the existing texture pipeline after source approval.

AVOID:
Botanical-plate realism, photographic surfaces, pixel art, dithering, dense
micro-detail, many color steps, excessive gradients, thin outlines, bright
lime foliage, saturated orange bark, recolor-only species differentiation,
perfect Christmas-tree symmetry and backgrounds baked into the sprite.
```

Do not ask the generator to invent an alternative style or reinterpret an
accepted reference. Species-specific requirements may narrow the shared rules;
a conflicting style change requires an explicit author decision and canonical
ArtDirection update first.

## Candidate review and production handoff

Before presenting a generated candidate, compare it against the approved
proposal and the viewed references:

- category/silhouette and species distinction;
- strong simplification, large masses and thick outline;
- restrained palette, limited color steps and subtle shading;
- transparent background, complete edges and no ground/scenery/snow;
- deciduous state continuity when applicable.

If a constraint failed, name the discrepancy and correct it using the same
baseline. Do not label a conflicting candidate as ready for integration.
Source-art approval and in-game appearance acceptance are separate stages.

After source approval, use `TextureAssetPipeline.md` for the exact production
PNG bytes, structure/CRC/decoded-image validation, hash-preserving install,
Def-switch ordering and the matching natural-biome runtime check. Do not
rebuild base64 by hand, regenerate merely to transfer an image, or repair
only a stored CRC to hide corrupt image data.

Record durable accepted design/appearance in `Docs/ArtDirection.md`.
Record current stage, remaining coverage and next action only in
`main:Docs/Coordination.md`. Before leaving the task or moving to another chat,
ensure those records describe the current state and identify the actual
reference/output paths; do not depend on chat-only approvals or attachments.

## Portable restart instruction

A new chat can start with:

> AMJ Environmentの樹木リテクスチャを継続してください。
> mainのAGENTS.mdとDocs/Coordination.mdを最初に確認し、
> Docs/GoldenPaths/RetextureGeneration.mdの開始手順から再開してください。

Repository:
`sucRo-RimWorld/Ancient-Medieval-Japan-Environment`.

## Pixel-exact reused components (AMJ shared policy)

Follow the Core source of truth [FixedImageTemplates.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/GoldenPaths/FixedImageTemplates.md). Same style does not mean identical parts: species-specific silhouettes remain distinct. Where a trunk, container, title or other component is reused, register an approved lossless master and binary editable mask with SHA-256 hashes in the owning repository, generate only variable material, and composite deterministically. Final decoded protected RGBA pixel differences must be zero; reference-image editing/visual similarity cannot replace this check. Use Core `Scripts/Art/fixed_template.py` for compositing/validation, then retain Environment's PNG integrity/install gates. Read the master/manifest and actually view the approved reference in every new chat. No shared-pixel guarantee may be claimed before the family template and output pass this gate. Existing proposal approval stages are unchanged.
