# Wetland four-plant 1.6 calendar and actual save/reload — PASS (2026-10-19)

**Native evidence:** [Project Actions #39194825692](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39194825692), artifact `amje-season-save16-39194825692` (ID `12160958435`). The original [#39187347933](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39187347933) and duplicate [#39187347981](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39187347981) remain genuine FAILs from the incorrect test-only entry-root loader and are not relabeled.

- Environment clean source: `03c743376a34ef9256acfed147da74adf5e75c51` (test-only native reload scene fix; no changes to production PlantDefs, approved art or harvest values).
- Exact 60-file packaged candidate ZIP SHA256: `0f2e717de24e93c742d789c97281a96af857cb27a8a0f02422693b3a79d3f3ed`.
- Production DLL SHA256: `1d258a4f0e9e52d268bdb60a6b71a7d39b892acd4ee1549fafa546dfbee255df`.
- Genuine Linux RimWorld 1.6.4871 rev600; **Vanilla ColdBog**; `AMJWetlandSeasonSaveQuickstart`, 26/26 checks PASS; first process exit 0, no prelaunch/runtime Unity ERROR, no truncation.
- Actual calendar sampling: outdoor temperature **-11.80 to 30.94 °C**, `graphics=True`, `winterDeaths=0`, plant/terrain/graphics assertions PASS. This samples calendar hours rather than simulating 60 consecutive days of ticks.
- Genuine isolated saved game `AMJE_020_Wetland_SeasonSave.rws`: **12,594,839 bytes**, SHA256 `ae8f4de8931b32366ec6ec32cfdc4b597d3385968c6a5401d6b8be84665b8d28`.
- A **separate real RimWorld process** loaded that file using the normal entry → Play scene transition. Original Unity log explicitly shows `AMJE_020_RELOAD_SCENE_QUEUED scene=Play` and `AMJE_020_RELOAD_RESULT passed=True loadedColdBog=True species=Yoshi,Suge,Mizugoke,Hannoki`; reload exit 0, no timeout, raw severity errors 0.
- Expected behavior/limits: all four exact production ThingDef identities survive the serialize/reload cycle on ColdBog; this does **not** prove a full 60-day simulation, old saves, 1.7 save/reload, all provider combinations, a four-profile byte-identical final upload, or actual Steam subscriber delivery.

**Release status:** Scoped seasonal/sample and new-save reload requirement PASS. 0.2.0 release still HOLD for remaining final candidate/publication checks and independent publisher/subscriber evidence. Existing MO/SYR blank-clip FMOD exception is unchanged; raw strict ERROR0 failures are not waived.
