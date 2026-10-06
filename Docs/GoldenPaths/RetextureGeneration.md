# AMJE Retexture Generation — New-Chat Start Procedure

## Scope and authority

Use this procedure for AMJE plant/tree sprites, including AMJE-owned plants and
Vanilla/MO tree retextures. It is the generation entry point, before
[TextureAssetPipeline.md](TextureAssetPipeline.md).

The shared visual source of truth is Core `Docs/ArtStyle.md`; Environment-specific tree/plant rules and accepted species baselines are owned by [ArtDirection.md](../ArtDirection.md). This procedure owns only the generation preflight and handoff, not a second copy of either style guide.

Workshop covers, logos, terrain textures and world-biome images are different
asset classes. Read their owning specifications; never substitute these tree
rules for a cover's shared-left/addon-right layout. If the requested class is
unclear, resolve the class before generating.

## New-chat preflight — required before an image-generation call

1. Read repository `AGENTS.md` and authoritative `main:Docs/Coordination.md`
   first. Read the current main versions, not a remembered chat summary.
2. Read Core `Docs/ArtStyle.md`, this procedure, `Docs/ArtDirection.md` (Environment/category/species additions), and `Docs/GoldenPaths/TextureAssetPipeline.md`.
3. Resolve the latest OPEN/IN PROGRESS art handoff and distinguish:
   proposed design / author-approved design / generated candidate /
   approved source / installed asset / accepted in-game appearance.
   A request to save rules or to start a new chat is not approval of a pending
   species design.
4. Fetch and actually view the accepted reference PNGs below. Do not merely read
   filenames, rely on an old conversation image, or claim to have inspected
   pixels that were not opened.
5. Reuse the existing approved category/style and any already-approved target design. If no target-specific proposal is already approved, derive the narrowest reasonable composition from the current request and the owning style rules; do not add a mandatory extra approval round unless the user explicitly asks for proposal/review-first work.
6. Build the generation request from the current Core `ArtStyle.md` invariants plus the applicable Environment/category/species rules in `ArtDirection.md`. Attach the actual viewed reference images. Do not maintain a second frozen style prompt in this procedure.
7. Generate one candidate for the requested target. Do not restart the style design or produce unrequested alternatives.

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

## Generation request assembly

The generation request must carry the **current** applicable rules from:
- Core `Docs/ArtStyle.md` for AMJ-wide invariants;
- `Docs/ArtDirection.md` for Environment tree/plant class rules and the target species;
- the actual accepted reference images opened during preflight.

Do not copy a long frozen prompt into this file. That previously created a second style source that could drift from the project-wide rules. The request should contain only the shared invariants plus the approved target-specific silhouette/structure differences needed for the current asset.

If the target-specific request would contradict Core ArtStyle or ArtDirection, stop and resolve the specification first instead of compensating with ad-hoc prompt text.

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

For any explicitly reused visible component, follow the Core `Docs/GoldenPaths/FixedImageTemplates.md`; ordinary same-style tree sprites do not require fixed-pixel templates.
