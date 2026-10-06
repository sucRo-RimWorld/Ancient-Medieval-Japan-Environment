# AMJ Environment Authoritative Art Sources

This directory preserves the exact accepted source bytes for current AMJE art. It is the source archive; production files under `Textures/` remain the shippable game assets.

## Rules

- Preserve accepted source bytes exactly. Do not resize, recolor, recompress, or regenerate them in place.
- Mirror the production `Textures/` path below `Art/Sources/` where practical.
- The current plant masters and snow overlays were recovered from the active accepted `Art/Templates/*` registrations and stored here by reusing the same Git blob bytes.
- `Art/Templates/` remains available for deterministic masks/builders/regression tests. A review-only `exact-composite.png` is not duplicated here when the authoritative inputs are the master plus snow overlay.
- Do not promote a production derivative to source status if its actual accepted source is missing.
- The entire repository-root `Art/` tree is development-only. Root `.rimignore` contains `Art`, so YADA excludes it recursively from Steam Workshop uploads.

## Current accepted source images

- `Things/Plant/AMJ/Shii/Shii_A.png`
- `Things/Plant/AMJ/Shii_Snow/Shii_Snow_A.png`
- `Things/Plant/AMJ/Beech/Beech_A.png`
- `Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png`
- `Things/Plant/AMJ/Beech_Snow/Beech_Snow_A.png`
- `Things/Plant/AMJ/Beech_Leafless_Snow/Beech_Leafless_Snow_A.png`
- `Things/Plant/AMJ/Shirabiso/Shirabiso_A.png`
- `Things/Plant/AMJ/Shirabiso_Snow/Shirabiso_Snow_A.png`
- `Things/Plant/AMJ/Haimatsu/Haimatsu_A.png`
- `Things/Plant/AMJ/Haimatsu_Snow/Haimatsu_Snow_A.png`

These correspond to the currently accepted structural-plant art states in `Docs/PlantVisualCoverage.json` and `Docs/ArtDirection.md`.
