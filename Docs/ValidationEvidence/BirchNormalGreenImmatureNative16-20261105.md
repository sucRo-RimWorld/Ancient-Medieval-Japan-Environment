# Birch normal-green immature appearance — real RimWorld 1.6 native screenshot evidence (2026-11-05)

## Purpose and actual result

Both separate real original-game 1.6 Vanilla generic `plant-appearance16` executions against **exact Environment owner source `315c5db277c31de2dc783a68a651850caa5522fd`** have completed with **preflight/build/native PASS**, original Unity ERROR0, full original 1280×720 framebuffer captures under spring/normal green condition. The complete archived original images and Unity/launcher logs remain in original Project GitHub Actions native artifacts.

| Original-game 1.6.4871 rev600 execution | Case | Original native artifact | `visual/normal.png` SHA-256 | `visual/normal.unity.log` SHA-256 |
| --- | --- | --- | --- | --- |
| [Project Actions #41987600388](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41987600388) | `shirakaba` | [13396488538](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41987600388/artifacts/13396488538) | `bf9aabd284e998ec4b38ee61f1f74bed942a9f5721285d51e5d221fafcc47f77` | `fca59d1f4fd346d6c6c2d08b62a0442ff48aadf1a46b7a57e236822a89864cb0` |
| [Project Actions #41988280255](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41988280255) | `dakekanba` | [13396174100](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41988280255/artifacts/13396174100) | `33791e222f81a18b9d73927ed1939672f6c47abcc174bdf31abf166f6dca9018` | `0d8789ae28310d64dd5bb14b6a1d7e928b3d8b3c0f1564a549e77578aeaf6a9a` |

**Native `mode=normal` assertions, separately PASS for each species:** 

- `AMJ_PLANT_VISUAL_READY mode=normal verified=True count=6 localHour=12 season=Spring day=10 weather=Clear paused=True rootSize=23 forcedAutumn=False forcedLeafless=False uiIconValid=True`.
- Original scene creates six real RimWorld `Plant` objects through the existing original-game `ThingMaker.MakeThing` + `GenSpawn.Spawn` visual fixture to inspect **actual rendering**; it is **not** evidence of spontaneous wild regrowth or an uninterrupted growing season.
- Same row scene `row=0 col=0 pos=(46, 0, 72) growth=1.00 leafless=False snow=0.0` and `row=0 col=2 pos=(64, 0, 72) growth=0.30 leafless=False snow=0.0`, both `valid=True`, native paths `Things/Plant/AMJ/Shirakaba` and `Things/Plant/AMJ/Dakekanba` respectively, no BadTex.
- Original Project shared `AMJPlantVisualQuickstart` sets full fall-color override to **intensity `0f` in `normal` mode** after arranging native Spring date, Clear and local noon. This is a controlled normal-green color render; the prior author-reviewed 30%-size view had **intensity 1** full autumn and cannot substitute.
- Both jobs' existing additional modes and cold Quickstart are PASS but already-approved separate appearance states are not re-accepted and game-era full yearly cycle is not established. The original `manual-native-result.json` truthfully records `author_visual_acceptance=NOT_RUN`.

The visual comparison presented in chat was **exactly four 130×130 pixel crops** from the two original `normal.png` files: mature x=505 and immature x=774, y=212..342, each enlarged via **nearest-neighbor 4×**, arranged as two species rows by two growth columns. No species generation, repaint, color adjustment or new graphics are introduced. The final locally presented composite was `Birch_NormalGreen_30pct_RealGame_20261105.png`, 1276×1281, SHA-256 `5050b5d69afc419fa168d747b5dc5a3d3f098d5e1e037aef713d41573f4a8911`. Its original frame bytes are the artifact files listed above; the local composite itself is **not** asserted to have been uploaded to GitHub.

## Acceptance boundary

The earlier user `ｙ` accepted **both species' immature shape and relative size** from an actual 30%-growth **full-autumn** comparison. The current `normal` screenshots are now successfully **captured and presented** for the distinct ordinary green immature color review; **no subsequent user approval of those green screenshots has yet been recorded**. Therefore retain each `Docs/PlantVisualCoverage.json` `immature.status=pending`, `partial_acceptance.geometry.status=accepted`, `partial_acceptance.normal_green_leaf_color.status=pending`, `complete=false` and Environment PR #310 merge / Workshop publication HOLD. Existing accepted images, Def XML, growth/wood balance, current shared test engine, and loaded historical saves were not changed to produce these images. The new capture was accomplished solely by adding `normal` to owner case allowed modes in `Tests/PlantNativeAcceptance.json` at the pinned exact owner source.
