# AMJ Environment — current coordination

This live file contains **open owner handoffs and material release/acceptance blockers**, not a chronological log of test runs or superseded instructions. The canonical specification is [Design](Design.md), supported by the affected formal research/validation files and production XML/C#. The original detailed status history is preserved in the [2026-10-28 pre-compaction main snapshot](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/blob/f8d233059681a93186af92fc54d366afa50df15a/Docs/Coordination.md) and Git history; old entries do not authorize changing current values. The Project [SharedRules](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/SharedRules.md) applies.

## Current open handoffs and decisions

### Seasonal vegetation update — five-species scope; implementation OPEN
- **Owner:** [Design — 2026-10-30 seasonal scope](Design.md) and [NativeVegetationStep3Design-ja](NativeVegetationStep3Design-ja.md).
- **Author-confirmed boundary:** the seasonal-appearance update includes only **Yamazakura, Ume, Kuri, Kaki and Susuki** as newly added plants. Vanilla Willow/Pine/Bamboo retextures form a separate workstream. Sugi and Hinoki remain outside this update without deciding their longer-term inclusion. The older Sugi priority-A record does not expand this scoped batch.
- **Open:** production Defs, season-state sprites/behavior, species-specific distribution/balance and appropriate acceptance are not completed by this documentation decision.



### Rainy season, crop sunlight and Environment 0.3 — balance HOLD
- **Owner:** [Design §4.1](Design.md), [seasonal rain research](Research/TsuyuAkisameClimateCalibration-2026-10-21.md) and [2026-10-28 native crop-rate evidence](ValidationEvidence/SeasonalCropRateNative16-20261028.md).
- **Current source:** 0.3.0 development adds separate (a) selected-biome Tsuyu/Akisame weather-selection weights plus post-rain Clear weighting and (b) year-round rainy/foggy-weather sky-glow attenuation. The real 1.6 original-game 60-day *event-stepped* three-seed Potato/Rice `Plant.GrowthRate` comparison isolated **only (b)** under unchanged 0.3 weather schedules: ratios 0.9131 / 0.9067 / 0.9154 (mean 0.9117). **Not** a genuine fully ticked crop harvest or full 0.2-versus-0.3 weather-frequency comparison. Neither season removal nor a gameplay coefficient change is approved by this result.
- **Open:** Author's weather/light decision; actual whole-season crop maturity/harvest, other climates/MO interactions and optional Dynamic Atmosphere co-load remain unaccepted. Do not turn one-off research PASS into a release acceptance. The current full bilingual Workshop description exceeds the 8,000-byte release gate; 0.3 candidate remains **release HOLD** until that separate source issue is corrected.

### Workshop publication — YADA restored; distributed bytes not verified
- **Owner:** [active publication procedure](GoldenPaths/WorkshopPublication.md), [ReleaseCandidate](GoldenPaths/ReleaseCandidate.md), [ReleaseReadiness](ReleaseReadiness.md).
- **Author's latest Environment-specific route (2026-10-20):** RimWorld **YADA + Add Changenote**, not the earlier central/per-owner GitHub Actions SteamCMD upload method. Do not activate the retired Environment publisher; keep existing Workshop ID `3814638060` and visibility, exact clean staging root, and separate author upload approval. The author reported a 0.2.0 YADA upload; **actual downloaded subscriber content and exact DLL/source remain unverified**. Source 0.3.0 has not been verified as distributed.
- **Known external audio:** existing isolated MO/SYR FMOD blank-clip errors remain raw strict ERROR0 **FAIL**; the formal owner release contract alone defines its narrow independent functional classification. No other runtime failure is waived.

### Environment wild food foraging — later feature, implementation DEFERRED
- **Owner:** [Foraging Integration Design](ForagingIntegrationDesign-ja.md) and [Design](Design.md).
- The 12 accepted resources remain planned for a later Environment update, not a separate Wild Food Foraging runtime. Preserve the accepted role of region- and season-limited non-field food/poor-harvest fallback, without year-round grain replacement. Species-specific numerical yield, workload, preparation/storage and spawn densities remain **undecided**; generic acorns/tochi are **on HOLD**, not part of the accepted 12. No new production Def, harvesting system, image or release is authorized by this handoff.

### WorldMap regional geography — separate owner, interface OPEN
- **Owner:** [WorldMap design](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-WorldMap/blob/main/Docs/Design.md) for the fixed Japan tile layout and future intermediate-temperate BiomeDef. Environment owns its four broad forest/altitude BiomeDefs and normal Japanese wild plant ThingDefs. WorldMap development remains active on sea-settlement and mountain/treeline repair; do not create a duplicate intermediate biome or force a fixed-map-specific worker into Environment's random-world generation. No map-coordinate workaround is approved for Environment seasonal-weather selection.

### Additional Japan-native vegetation — implementation/asset acceptance OPEN
- **Owner:** [NativeVegetationStep3Design](NativeVegetationStep3Design-ja.md), [ArtDirection](ArtDirection.md), [Design](Design.md), [PlantVisualCoverage](GoldenPaths/PlantVisualCoverage.md).
- **Sugi (cedar)** is a separate, historically regional natural woodland candidate, not an approval for modern nationwide forestry. **Susuki** is a separate grassland species; accepted Susuki source art must not be relabeled as Ashi. Both require scoped owner need/historical/art/production acceptance and cannot be silently counted as wetland Step3 plants. Reuse accepted masters and protected image bytes.
- Existing Vanilla/MO tree retextures remain **author-deferred**, not an active batch; descriptions and historical naming follow Project's Japanese-author-approval-before-English procedure. The existing tree/plants' accepted Def identities, visuals and wetland graphics must not be overwritten as a side effect of this cleanup.

### Vegetation compatibility, tree sowing and existing-save migration — required when affected
- **Owner:** [Design](Design.md), [PlantSowingTests](GoldenPaths/PlantSowingTests.md), [Vanilla Wetland audit](VanillaWetlandBiomeAudit-ja.md).
- Retain MO's own wetland herbs and other third-party injected wild plants; Environment's wetland patches should remove only their targeted Vanilla entries, not replace whole shared wildPlant collections. Static/native successes in historical source do not establish untested new provider versions.
- Region-specific tree-sowing candidates, Haimatsu no-sowing and natural wild-tree distribution must remain consistent. Existing-save vegetation addition/maturation/harvest/sowing/save-reload E2E is **not universally accepted**; test only the next actually changed plant feature with a relevant old-save scenario. Do not claim ordinary loaded-Def tests prove saved-map migration.

### Current developer/test boundary
- Original 1.6/1.7 native, four-profile and wetland/seasonal observations are linked in [Design](Design.md), [ReleaseReadiness](ReleaseReadiness.md) and [ValidationEvidence](ValidationEvidence/). Preserve distinct source pins, actual error classifications, genuine vs event-stepped sampling and published-subscriber states. No automatic RimWorld launch, redundant full matrix or new monitoring job follows from this coordination cleanup.
