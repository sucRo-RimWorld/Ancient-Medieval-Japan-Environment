# Birch leafy-snow extraction candidate — 2026-11-01

**Status: CANDIDATE, NOT author-accepted, NOT production, NOT game-validated.**
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

**Do not set `snowOverlayGraphicPath` to either candidate until explicit author visual adoption.**
Do not substitute the whole-tree snow images as overlays or mark the `snow` visual-coverage ledger accepted. Only after adoption, copy the accepted snow-only PNG without changing its pixels into the appropriate `Textures/Things/Plant/AMJ/{Shirakaba,Dakekanba}_Snow/…` and create/verify its DDS, then wire the Def and run a bounded native visual acceptance check.
Leafless no-snow policy is unchanged. ColdBog weights, normal/leafless art and CCTO fields are unchanged by this candidate.
