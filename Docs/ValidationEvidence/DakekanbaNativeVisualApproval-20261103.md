# Dakekanba autumn + UI icon — author visual acceptance (2026-11-03)

## Scope of approval and unchanged source

The author replied **「OK」** immediately after the comparison of original-game Shirakaba/Dakekanba **autumn** and **32/64 px UI icon** frames, and the accompanying native evidence/status report. Shirakaba's two states were already accepted and recorded separately in [ShirakabaNativeVisualApproval](ShirakabaNativeVisualApproval-20261103.md). This entry records the **Dakekanba autumn appearance and Dakekanba Widgets.DefIcon at 32/64 px only**.

The response does **not** approve the Dakekanba InfoCard contents or its separate visual presentation, either species' incomplete/immature or transitional appearance, a naturally elapsed full-season screenshot, gameplay coefficients, save compatibility, the entire Environment PR #310 or Steam publication. No production sprite, texture, DDS, Def, C#, or weather-color shader is changed by accepting the displayed result.

## Original real-game provenance

[Project reusable manual native Actions #41621113921](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41621113921), native **Artifact 13224046122**, used original **RimWorld 1.6.4871 rev600 Vanilla**, exact Environment PR #310 source `33499eafbecd7c9dd37d7e90d4e4456dfb053ab2` and `plant-appearance16` case_id `dakekanba`. The original [Shirakaba run #41435799301](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41435799301), native Artifact `13195990324`, supplied the comparison frames. Byte-for-byte hashes were verified against **both actual downloaded Actions artifacts** and the combined review ZIP `AMJ_Birch_VisualReview_BothSpecies_20261103.zip`.

| Original Dakekanba native frame | Format | Exact SHA-256 | Author-review scope |
| --- | --- | --- | --- |
| `visual/autumn.png` | 1280×720 RGB framebuffer | `9861716f2422da7a81f70579b3b4695399c49e5e99260224199b9b288dc560ff` | Original live Dakekanba, full **forced autumn shader** at midday; accepted visible color/silhouette, not normal annual progression |
| `visual/icon.png` | 1280×720 RGB framebuffer | `d446bce430b4df0fe5ad7d9273ea0c3ce62045cfecce03a554e2f049612362d5` | Actual `Widgets.DefIcon` in the 32/64 px UI test window, not a substitute/generated icon |
| `visual/infocard.png` | 1280×720 RGB framebuffer | `ff2493a3867cd8419c8061653ca78c81ff63e96b5bcd8c8a63ca96ffc40131af` | **Technical capture PASS only, NOT part of this explicit author acceptance** |

The actual native run records `matrix_pass=true`, all three screenshot captures technically passed, one `AMJPlantColdQuickstart` **4/4 PASS** with true same-tree cold leafless/recovery labels, process exit0, Unity error_count0. These are independent *mechanical* gates, not additional art approvals.

## Verified autumn review conditions

The original `visual/autumn.unity.log` contains:

`AMJ_PLANT_VISUAL_READY mode=autumn verified=True count=6 localHour=12 season=Fall day=7 weather=Clear paused=True rootSize=23 forcedAutumn=True forcedLeafless=False uiIconValid=True`

The native setup displayed mature and growth 0.30 specimens on snow-free and snow-covered test cells in the same two-row, three-state layout. In this autumn mode, the original `PlantFallColors` native shader intensity was forced to 1; camera rootSize=23, local time noon, weather Clear with transition complete and game paused. The forced maximal autumn-color frame is accepted as shown, **not** the timing, accumulated climate balance or whole-year rendering of natural autumn. The icon frame is an original live-game widget view, not an image-generation output.

## Ledger boundary

Only `Docs/PlantVisualCoverage.json` rows **`AMJ_Tree_Dakekanba/autumn`** and **`AMJ_Tree_Dakekanba/icon`** change from `pending` to `accepted`. Their exact current source fingerprint is frozen for regression checking. The previously accepted normal, snow and leafless states, the already accepted Shirakaba autumn/icon entries, and all production bytes are untouched. Both trees' **`immature` and `transition`** entries remain pending, so neither tree is marked visually complete. Human approval of a UI icon does not establish numerical balance on an InfoCard.

## Implementation and release boundary

This decision is a narrow author-reviewed native-frame acceptance, not a new runtime game test. Do not replace original PNG/DDS with the cropped comparison pixels, do not retune commonality `0.40/0.05`, and do not infer completion of the natural full-year ecology, direct Dakekanba reappearance on the upgraded old save, or full PR #310 merge/Workshop upload.
