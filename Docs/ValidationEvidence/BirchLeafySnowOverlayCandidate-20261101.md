# Birch leafy-snow extraction candidate — 2026-11-01

**Current status (2026-11-01): author accepted BOTH snow-only candidate images by replying「採用で」to the two actual composite previews. Exact candidate PNG bytes were promoted to `Art/Sources` and production `Textures`, DDS were generated, and both new tree Defs now reference snow-only overlays in PR #310. Native RimWorld visuals and seasonal behavior remain UNVERIFIED.**

The earlier candidate-only qualification below is the historical pre-approval state and does not override this acceptance.
Source of truth for final tree states remains `Docs/Design.md`, `Docs/ArtDirection.md`, and `Docs/PlantVisualCoverage.json`.

## Exact sources and review assets

Both normal sprites below are existing author-approved source derivatives, and the snowy *whole-tree* illustrations are appearance references, not valid RimWorld snow-only overlays. **No accepted source or current production sprite is overwritten.**

| Species | Approved normal reference | Snow illustration reference | Snow-only candidate | Composite review |
|---|---|---|---|---|
| Shirakaba | `Art/Candidates/Shirakaba/Shirakaba_Leafy_256_Review.png` | `Art/Candidates/Shirakaba_Leafy_Snow/Shirakaba_Leafy_Snow_256_Review.png` | `Art/Candidates/BirchLeafySnowOverlay_20261101/Shirakaba_SnowOnly_Candidate.png` | `Art/Candidates/BirchLeafySnowOverlay_20261101/Shirakaba_Composite_Review.png` |
| Dakekanba | `Art/Candidates/Dakekanba/Dakekanba_Leafy_256_Review.png` | `Art/Candidates/Dakekanba_Leafy_Snow/Dakekanba_Leafy_Snow_256_Review.png` | `Art/Candidates/BirchLeafySnowOverlay_20261101/Dakekanba_SnowOnly_Candidate.png` | `Art/Candidates/BirchLeafySnowOverlay_20261101/Dakekanba_Composite_Review.png` |

## Scope and derivation

- The output is an **independent transparent 256×256 RGBA snow-only candidate**, plus a static composite on the exact approved normal 256px bitmap. The originals are untouched.
- Source snow-colored pixels were filtered for proximity to existing real green foliage, with a large-color-difference threshold. Original opaque brown/white wood was excluded; remaining tiny connected components were removed. Dakekanba lower-trunk false positives were explicitly excluded with a crown-only y limit.
- This removes the previously observed unnatural white areas on Shirakaba white bark, Dakekanba branches and lower trunk, while preserving recognizable foliage-top snow masses.
- On source-pixel audit, **zero overlay pixels** overlap original opaque non-foliage bright bark/wood; extracted non-transparent snow pixels: Shirakaba **6,618**, Dakekanba **5,066**. The overlay 256×256 RGBA PNG round-trip decode was checked. Synthetic preview is **not** a real RimWorld screenshot.
- This extraction is a draft visual technique, not an accepted fixed mask or proof that every snow cap is naturally supported. Small shading/edge artifacts and gameplay zoom/lighting are still unreviewed.

## Integration boundary

**Author visual adoption was received on 2026-11-01 (`採用で`). The production snowOverlayGraphicPath is now enabled for both species in PR #310.**
Do not substitute the whole-tree snow images as overlays or mark the `snow` visual-coverage ledger accepted. The exact snow-only PNG blobs are now copied to `Art/Sources` and `Textures/Things/Plant/AMJ/{Shirakaba,Dakekanba}_Snow/{Shirakaba,Dakekanba}_Snow_A.png`, with same-name DDS, and the Def paths are wired. **A bounded native visual acceptance check is still required**; static source adoption and file integrity do not validate loaded-game snow rendering.
Leafless no-snow policy is unchanged. ColdBog weights, normal/leafless art and CCTO fields are unchanged by this candidate.

**2026-11-01後続のRimWorld 1.6ネイティブ確認:** [BirchNative16-20261101.md](BirchNative16-20261101.md) を参照。静的合成での作者採用と、実ゲーム内での強制葉あり／落葉・積雪実装の動作、最終画面の作者受入は別段階。初回撮影のGitHubジョブ結果はログ集計不具合によりFAILであり、原データのUnity ERROR0とは区別する。

## Promotion and provenance after author acceptance

| Species | PNG Git blob (candidate, source master, production) | DDS Git blob | DDS details |
|---|---|---|---|
| Shirakaba | `43c89b797c2a620466ab0b9bd65e5e1013d150ec` | `0fd0db389cacb45118794c07fab384e9a30d8586` | 256×256 BGRA8 uncompressed, mipmaps 9, row order inverted |
| Dakekanba | `a459335df81a748c23bddf034d7fec029f922780` | `654058ff9cc5d76a310c10b6d8c7bd6140052ba2` | 256×256 BGRA8 uncompressed, mipmaps 9, row order inverted |

The DDS transforms are deterministic derivatives of the approved pixel content: each mip is alpha-weighted average, stored flipped in Y to match existing Environment DDS runtime convention. DDS header and dimensions verified at generation; actual RimWorld 1.6/1.7 rendering not yet tested. Source master PNG bytes are NOT the full-tree snowy illustration. The approved normal and adopted leafless tree sprites are unchanged.
