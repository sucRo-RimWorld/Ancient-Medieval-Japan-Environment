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
