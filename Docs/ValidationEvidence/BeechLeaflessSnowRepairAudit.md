# Leafless beech snow repair audit — 2026-10-06 JST

Status: **UNAPPROVED / REVIEW**. Author rejection supersedes the earlier paired snow approval for the leafless variant. Draft PR #5 only; no merge, release, installed-game update or current runtime/visual acceptance.

## Evidence and cause

Inspected current remote main AGENTS, Coordination, RetextureGeneration and ArtDirection; Core ArtStyle and FixedImageTemplates; Environment TextureAssetPipeline, snow plan and per-state ledger. Opened accepted Sudajii, leafy/leafless beech, leafless template master, current rejected composite and accepted Haimatsu snow composite.

The PR repair `62434d2` came from the deterministic script introduced in `853d7f5`, not a new image-model call. Per-edge connected fragments became numerous small nested capsules. This removed thin stripes but produced pellets, narrow pale rims and multiple highlight bands. An eroded-area test certified filled pixels yet did not reject many tiny masses, weak contour or unsupported snow. The mask was derived from output alpha rather than independently fixed review regions. The PR also retained a frozen generation prompt superseded by current main's Core ArtStyle routing, and historical ArtDirection still said both snow variants were accepted. These are confirmed implementation/gate/document gaps; the particular unspecified earlier chat image cannot be identified from the bounded reference.

## Visual audit

| Criterion | Revised candidate against accepted references |
| --- | --- |
| Shared style | Coarse symbolic masses, low saturation, no texture noise; accepted Haimatsu snow supplies off-white and blue-grey color planes. |
| Outline | Dark outer contour; original base outlines are unchanged outside snow coverage. |
| Internal lines | One broad underside plane; no nested capsules, bark additions or fine decorative strokes. |
| Perspective / form | Same 256px canvas, trunk, branch identity, position and scale. No tree regeneration, rotation or resize. |
| Snow | Twelve asymmetric horizontally supported ledge caps with varied width/thickness; twig tips and large gaps stay exposed. No ground snow, icicles or flakes. |
| Small display | Compared at 256px and approximately 64px on the same neutral background; snow remains discrete masses. This is agent visual inspection, not author approval or native gameplay evidence. |

## Reproducible path and validation

Local painting only; no ImageGen call. Run `Scripts/Art/build_beech_snow.py`, `Tests/validate_beech_snow.py`, and `Tests/validate_plant_visual_coverage.py --self-test`. The review-region geometry is declared before painting, versioned as `v2-review`, and is not approved/active. Original directory name stays `Beech_Leafless-Snow-v1` to keep the requested PR review path. Core `fixed_template.validate` independently reports protected RGBA differences = 0. The accepted master hash stays unchanged.

Structural regression rejects thin stripes, many pellets, weak outline and floating caps; actual candidate has 12 connected solid masses / 2466 solid pixels. Exact overlay-copy/composite, binary mask/hash and displaced-layer checks pass. Windows PNG installer ValidateOnly (256x256, chunk CRC/zlib/decoded pixels) and the all-production decoded-PNG corruption regression pass. Existing base masters, leafy snow and other species' snow composites remain byte-identical to the original PR head.

The state ledger self-tests pass with exactly one pending row: AMJ_Tree_Beech/snow, leafless only. `--require-complete` must fail and its CI may remain blocked until the author reviews this revision. Do not relax the completion gate. Existing historical native logs cannot establish acceptance of these new bytes. No native game test was run for this art-only revision.

## Candidate hashes

- master.png: `24f8664bdedd3ebd0dee58fe627439b3784b5ecc599aaf49d750832f315be112`
- editable-mask.png: `ec1fbe6e4a2f4fb459510ee90dab1740c694d9e61267b701928232e5318de381`
- snow-overlay.png: `811360e723df2ca159dcfebf1d9402331125e2adb895b58d553a1f543ad04f00`
- exact-composite.png: `5d07c19337ffbc1ebc058562db5352cf82a7c33ac8a8ca2739de755749d63413`

## Superseding author feedback: revision 2 rejected

Author: `あまりに厳しい生成結果 / 枝が考慮されてない`. The preceding revision-2 assessment is superseded. Independent horizontal caps were the specific mistake; nearby wood in the interior did not establish branch-aligned contact.

Revision 3 keeps the master exact, traces upper branch pixels inside eight predeclared narrow corridors, and builds the lower snow edge from those pixels. Tests now inspect each column of the bottom edge and reject partial support. All eight shapes have 100% lower-boundary support at zero/one pixel distance; no new generation, installed-game change or human acceptance is claimed.
- Revision 3 editable-mask.png: `339e3704d0ae48365df85e56c308ed995ce8abba158b48fdf729da1bf3902170`
- Revision 3 snow-overlay.png: `6fafc5048ecc8f133a371b35341514909341d59c42fff674a68b390b240d1980`
- Revision 3 exact-composite.png: `bdd10855ce52784ea8a73d81bb0fc8a6beed0ec6abf2f9daa48abd692627b284`
