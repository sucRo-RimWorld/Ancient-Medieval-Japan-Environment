# Birch immature geometry acceptance — autumn-colored original RimWorld comparison (2026-11-03)

## Author decision and exact scope

The author answered **「ｙ」** (approval/continue) immediately after seeing the real-game **Shirakaba/Dakekanba mature 100% versus immature 30% side-by-side** image and being asked specifically **「この未熟木の形状・大きさで採用してよいか」**. This author decision accepts the **shape, proportions and rendered size at native Growth=0.30 for both species** as shown; it is not an approval of the green-leaf immature color, all weather/season variants, the entire plant-state appearance or any gameplay balance figure. Do not overinterpret it as `immature=accepted` / `complete=true` while the ordinary green-leaf color has not been separately reviewed.

The exact user-visible comparison image was `Birch_Immature_Comparison_RealGame_20261103.png` (990×990 pixels), SHA-256 `99a5f057b37b3b40f978cf86daf4b87451bdfc695df2ebe467d04a09ff837ee8`, made by **cropping existing original native game screenshots and resizing with nearest-neighbor 4×**; no synthetic tree, image-generation repainting, altered tree sprite or color restoration was applied. The comparison displayed *each* species' Growth 1.00 and 0.30 on the same nominal camera/terrain context, under an **autumn shader forced to intensity 1** (not ordinary spring/green).

## Actual original-game source evidence

| Original species | Original real-game native artifact | Real original screenshot SHA-256 | Native immature assertion |
| --- | --- | --- | --- |
| `AMJ_Tree_Shirakaba` | [Project Actions #41435799301](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41435799301), Artifact `13195990324`, `visual/autumn.png` and `visual/autumn.unity.log` | `1a5740dd429dc84aa2ba9235de294f7bef1693f81c91d6a70a566c36c2122554` | `mode=autumn row=0 col=2 ... growth=0.30 leafless=False snow=0.0 ... path=Things/Plant/AMJ/Shirakaba ... valid=True` |
| `AMJ_Tree_Dakekanba` | [Project Actions #41621113921](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41621113921), Artifact `13224046122`, `visual/autumn.png` and `visual/autumn.unity.log` | `9861716f2422da7a81f70579b3b4695399c49e5e99260224199b9b288dc560ff` | `mode=autumn row=0 col=2 ... growth=0.30 leafless=False snow=0.0 ... path=Things/Plant/AMJ/Dakekanba ... valid=True` |

For each original game, a matching mature `row=0 col=0` tree was reported at `growth=1.00`, same actual loaded sprite path, no snow, no leaflessness, `valid=True`. Six actual trees were rendered per scene. The native ready marker was `verified=True count=6 localHour=12 season=Fall day=7 weather=Clear paused=True rootSize=23 forcedAutumn=True forcedLeafless=False uiIconValid=True` with strict Unity ERROR0 in each actual execution. RimWorld **1.6.4871 rev600 Vanilla**; source-pinned owner test inputs correspond to Environment PR #310. The current accepted production `Defs/ThingDefs_Plants/AMJ_WildPlants.xml` and PNG masters remain unchanged; per-species normal-state fingerprint in `Docs/PlantVisualCoverage.json` is retained as the scope's image-source fingerprint.

## Ledger contract and remaining acceptance

- `AMJ_Tree_Shirakaba/immature`: **`status=pending`**, with nested `partial_acceptance.geometry.status=accepted`, date, explicit author statement, specimen Growth=0.30, matching mature reference 1.00, original real-game source URL/SHA and accepted reviewed production fingerprint.
- `AMJ_Tree_Dakekanba/immature`: **the identical limited geometry/size acceptance**, recorded independently against its actual native capture.
- `partial_acceptance.normal_green_leaf_color.status=pending`: neither reviewed image was normal green. The already-approved mature **normal** artwork color must not be treated as a separate author approval of the scaled immature sprite at native growth 0.30. Existing fall-color approval itself remains accepted independently and is not reopened.
- Preserve `complete=false` for both species while green immature appearance is pending. `transition=verified` continues to mean genuine sampled original seasonal mechanics only, not approval of each intermediate rendered frame.
- No new art asset, normal/snow/leafless texture, image dimensions, `ThingDef`, C#, growth rates, ecology coefficients, save format, game test, Steam publication or merge authorization follows from this decision.

**2026-11-05 evidence follow-up:** Both exact-source original real RimWorld 1.6 *normal-green* Growth 0.30 native frames have now actually been captured (Shirakaba Project Actions #41987600388, Dakekanba #41988280255), and the source/binary SHA-256, tested conditions, comparison-crop scope and actual original Unity log assertions are recorded in [BirchNormalGreenImmatureNative16-20261105.md](BirchNormalGreenImmatureNative16-20261105.md). **The green image was presented for visual author review, but its author approval is still pending; do not convert either immature row to accepted or complete from technical screenshot PASS.** The earlier geometry-only decision remains complete, with no need for further test runs just to retrieve this same evidence.
