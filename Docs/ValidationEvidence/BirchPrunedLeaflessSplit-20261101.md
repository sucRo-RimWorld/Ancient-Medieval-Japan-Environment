# Shirakaba and Dakekanba pruned leafless image split — 2026-11-01

## Approval and authoritative source

The user supplied the actual 1774×887 RGBA side-by-side PNG, after several unsuccessful redraws, and explicitly requested 「ではこの画像を分離してそれぞれの落葉画像として」. **The source is the actual uploaded combined PNG; not any regenerated or algorithmically pruned earlier candidate.**

- Raw approved combined upload: `Art/Sources/Things/Plant/AMJ/Birch_Leafless_Pair/Birch_Leafless_Pair_Approved_20261101.png`; original **unmodified bytes**, SHA-256 `19d7479dfb0205c7c9429007971e9f906c9f21efb244c2c694d39aab94a43d11`.
- Exact high-res crops (source art, no generative revision): Shirakaba left PNG 409×830 (`22870eb264843c4cf3b9e494cf414e9857223a64028b5cd69e142f110f272bad`); Dakekanba right PNG 432×829 (`ef14a9c60dee8f34f09dac549f425f6e4fc17fb1ed383c97aa1b56252ee033e2`).
- Derivative 256px RGBA sprites: Shirakaba `da293abfc3cc90bbfa015e19ceb49ba6ddb4a91fe65535ebc9e5e952d8d81485`, Dakekanba `91942dc1d6563d26bad48bd26cce7bb1c73088c18fc3a014310e156acf85da04`. All 4 corners transparent.
- Both old accepted masters and old 256px review candidates are preserved **byte-for-byte** in `Art/Archive/Things/Plant/AMJ/`.

## Placement and comparability

The side-by-side image uses transparent background despite black UI rendering. Alpha threshold >=8 identifies two main connected tree objects; no foreground recomposition or branch-geometry changes were made. Per-species cropping kept an 8px frame of original pixels; uniform Lanczos downscale preserves aspect ratio. The lower trunk root anchor was aligned to the old 256px accepted leafless sprite: median x=130 (Shirakaba original=130) and x=139 (Dakekanba original=138). Alpha>=16 occupied rectangles: Shirakaba [73,13,190,254], Dakekanba [78,12,202,253]. Earlier review sprite rectangles: Shirakaba [53,1,211,256], Dakekanba [56,9,204,254]. The new image is narrower deliberately, exactly matching the user's simplified tree image, not an accidental stretch. Comparisons were made on 256px and reduced 64px representations, including alpha and species-specific bark color.

## Explicit nonacceptance boundaries

- New art has author approval **for these leafless source sprites**, not a game-runtime screenshot pass.
- The new dedicated 2-species ThingDefs, distribution, season switching and full branch-to-leafy continuity have **not** been implemented or tested by the split.
- No snow overlay on bare branches; `LeaflessNoSnow` existing rule persists when species Defs are implemented. Leafy snow candidates and all original accepted leafy assets remain unchanged.
- Future native game tests must verify loaded source size and graphic orientation before any release-stage acceptance. Do not claim runtime PASS from this static image split.
