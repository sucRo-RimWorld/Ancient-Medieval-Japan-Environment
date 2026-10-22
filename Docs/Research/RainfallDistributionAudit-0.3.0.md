# Rainfall distribution audit for AMJ Environment 0.3.0 (2026-10-22)

## Current source, not assumed climate values

- Owner source: `Source/AncientMedievalJapanEnvironment/WorldGenStep_AMJEnvironmentTerrain.cs`, `EnvironmentTerrainProcessor.Apply`, root surface, pass 3. **All surface tiles** undergo `tile.rainfall = Mathf.Clamp(800f + tile.rainfall * 0.55f, 800f, 3000f)` after terrain/elevation adjustment and before biome selection. This is a global affine remapping of native tile rainfall, not a measured precipitation model or biome-specific calibration.
- **Implication:** when original rainfall is nonnegative, final rainfall has a hard floor of 800mm and cannot be lower. Examples, strictly formula-derived: input 0→800; 500→1075; 1000→1350; 1500→1625; 2000→1900; 3000→2450; >=4000→3000mm. These are **not observed world frequencies**.
- `JapanBiomeScoring.EligibleLand` additionally requires rainfall >=800mm for the four owned AMJ forests/altitude bands, so this rainfall eligibility condition effectively does not discriminate positive-elevation land after the global clamp. Actual forest bands are primarily assigned by annual temperature: warm >=15°C, cool 8–<15°C, subalpine 0–<8°C, alpine <0°C, subject to other biome workers.
- Existing `WorldGenDiagnostics.LogTerrainSummary` records surface land min/max/mean rainfall and swampiness count, not independent rainfall distributions by warm/cool/subalpine/alpine/TemperateSwamp/ColdBog. Thus it cannot yet establish whether each biome's rainfall envelope is appropriate.
- Existing wetland selection uses Vanilla/compatible biome workers and tile swampiness; mountain hilliness transforms can remove swampiness. An annual rainfall increase alone does not reliably create or maintain wetland sites.
- World river generation is downstream of this terrain pass. River flow/links can respond to rainfall changes; changing the rainfall transform without checking actual river networks risks unintentionally changing river density and map usability.
- The currently accepted Tsuyu/Akisame seasonal weather-selection multiplier and rain/cloud daylight patches **do not alter tile.rainfall**. Their native event-stepped site observation is **not** a measurement of annual precipitation in millimeters or world rainfall distributions.

## Audit conclusion and 0.3.0 handling

1. **Do not add fixed annual precipitation values by BiomeDef.** A biome is selected *after* climate generation and is not a geographical region; setting rainfall based on the result would be circular and flatten latitude/elevation/coast variability.
2. **The uniform global transform is the actual item to evaluate.** Its 800mm floor and compression could exaggerate low-rainfall climate and erase meaningful regional contrasts. Whether that is unacceptable for 0.3.0 remains unverified until generated-world per-biome distributions and river counts are compared.
3. **Do not change production constants 800/0.55/3000 without a source-pinned generated-world dataset** covering multiple representative land/latitude/elevation classes and downstream river distributions. Purely selecting new constants would be an unmeasured gameplay change.
4. For **AMJ WorldMap**, Japan Sea versus Pacific side, local snowfall and coast-specific regional rainfall belong to the fixed geography projection/region model; Environment must retain generic worldwide procedural compatibility.
5. **Scope for 0.3.0:** retain existing global annual rainfall formula and terrain behavior pending evidence. Current 0.3.0 releases can describe changes to the *chance of rainy weather*, but **must not claim rebalanced annual rainfall or river networks**. If an empirical distribution problem is shown before release, address the formula and dependent river effects together in a separate reviewed change.

## Minimum evidence to make a numerical decision

Collect from the **existing original-game native world-generation log**, without a new game/test framework: per-biome count, land annual rainfall min/median/p90/max and distributions by latitude/height; aggregate wetland share and river density/flow measures, alongside the unchanged 0.2.0 baseline where available. Use the same world seed and planet coverage for before/after comparisons and exclude oceans. A single convenient Tokyo-like tile and event-stepped rain hours cannot substitute for this distribution check.

**Audit status:** source-level audit complete; generated-world distributions by biome and comparative river evidence **not yet obtained**; production rainfall parameters **unchanged**, no gameplay acceptance claimed.

## Original 1.6 generated-world evidence (2026-10-22)

[Project native Actions #39762263750](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39762263750), [artifact #12418233930](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39762263750/artifacts/12418233930), Environment exact source `5dd9c8d2b16210aff188899a57079d26120cfcf1`, original RimWorld **1.6.4871 rev600**, Vanilla, `AMJWorldWetlandDistributionQuickstart`: **24/24 native assertions PASS, Unity ERROR0, exit0**, single generated world (119,904 surface tiles, 62,655 land). Values below are observed `tile.rainfall` after existing annual remap, **not native rainy-hour counts**, and do not establish a Japan-specific geographic placement.

| Biome | Land tiles | Minimum mm | Median mm | p90 mm | Maximum mm | Mean mm |
|---|---:|---:|---:|---:|---:|---:|
| AMJ_WarmTemperateForest | 15,820 | 832.3 | 1846.5 | 2328.7 | 2837.0 | 1809.3 |
| AMJ_CoolTemperateForest | 32,586 | 832.3 | 1176.8 | 1969.4 | 2826.2 | 1328.0 |
| AMJ_SubalpineForest | 12,336 | 832.3 | 1005.7 | 1432.0 | 2450.2 | 1087.8 |
| AMJ_AlpineZone | 1,010 | 846.5 | 1003.9 | 1346.5 | 1524.1 | 1046.2 |
| TemperateSwamp | 903 | 1273.6 | 1825.1 | 2047.5 | 2170.4 | 1816.0 |

Entire land min/max **832–2837mm**, average **1405mm**. Wetland output includes **TemperateSwamp 903 (1.4%)**, with no ColdBog tiles observed in this seed. River diagnostics: **5703 river-bearing land tiles (9.1%)**, **6031 river edges**, `Creek=3056`, `River=2975`; water-inclusive river tile count 6167. Tallies are produced on the **same exact generated world**, not a Vanilla baseline comparison. Existing warm forest share 25.2%, cool forest 52.0%, subalpine 19.7%, alpine 1.6%.

**Interpretation:** The present transform yields substantially less rainfall in the higher/cooler biome classes for this seed. This is a valid distribution finding but **not proof the highland rainfall is historically wrong**: no regional elevation/site and no precipitation snow/rain partition analysis. High cool-forest share affects the aggregate climate. The earlier source-level concern about compression and a nonfunctional `>=800` threshold remains. No before/after river control or independent-seed variation is available; a numerical formula replacement is **not accepted from this dataset alone**.

**0.3.0 disposition:** Baseline **measured**, weather frequency and rain/cloud gameplay-daylight unchanged; annual rainfall transform remains `clamp(800 + 0.55 × original, 800, 3000)`. Assess a targeted distribution change only together with source-matched river outcomes and generated-site relevance. No additional production modification, Steam publication, or reported multi-seed acceptance.


## Inverse-transform diagnosis from the same world (2026-10-22)

The measured land minimum (832.3mm) and maximum (2837.0mm) are **strictly inside** the final clamp bounds (800,3000). Accordingly, on these sampled land tiles neither clamp bound is active; the pre-AMJ source values can be reconstructed exactly up to logged rounding by `original=(final-800)/0.55`. Because this is a positive affine transform, **rank ordering, median and nearest-rank p90 also transform identically**; it cannot itself create the warm/cool/highland rank separation. Note these inferred source rainfall numbers are engine input values, *not historical Japanese station observations*.

| Biome | Approximate source rainfall median mm | Final observed median mm |
|---|---:|---:|
| AMJ_WarmTemperateForest | 1902.7 | 1846.5 |
| AMJ_CoolTemperateForest | 685.1 | 1176.8 |
| AMJ_SubalpineForest | 374.0 | 1005.7 |
| AMJ_AlpineZone | 370.7 | 1003.9 |
| TemperateSwamp | 1863.8 | 1825.1 |

The remap **increases** the values of already dry source sites, whereas it modestly **reduces** the values of naturally rainy source sites (>1777.8mm). On this generated world the low-elevation versus highland rainfall ordering was already present in input climate; it is not evidence that the 800mm clamp alone created a dry-highland pattern. A simple global offset/slope adjustment would continue to preserve spatial ranks (where unclipped); it cannot by itself add windward mountain precipitation or distinguish Sea of Japan/Pacific sides.

**0.3.0 scope decision:** Keep the current `800 + 0.55 × original` transform and the existing river generation unchanged. Evidence supports noting a model limitation, not approving a new arbitrary universal coefficient or biome-specific fixed rain. Geography-dependent orographic precipitation is a separate model change and requires world-projection/geographic inputs and same-seed river acceptance; it is **not silently part of 0.3.0**. If revisited, compare altitude/latitude/coast strata and precipitation representation including snow, with the WorldMap owner responsible for regional Japan-side mapping. No extra native run merely to recompute the invertible transform.
