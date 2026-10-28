# Environment 0.3 seasonal crop-rate impact — actual native 1.6 observation (2026-10-28)

**Status:** SOURCE-PINNED NATIVE MEASUREMENT COMPLETED; **full-season crop/harvest balance NOT accepted**. Evidence in this file does not authorize a gameplay coefficient change, Steam upload or a statement that 0.3 causes no excessive agricultural penalty.

## Test objective and controlled comparison

Answer the author's request to investigate whether the Environment 0.3 rainy-season/weather-daylight changes excessively penalize crop growth over the 60-day seasonal calendar.

- **Product source:** Environment main `dbf420f80a5f707cfd5e33646a41f281c683b984` (0.3.0 *development source*, not Workshop subscriber build).
- **Project native test owner:** [PR #1121](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/1121), merge `a99358ee4a1faa1bf7081d1bb3d9f6ab441ea67a`; [focused preflight correction #1122](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/1122), merge `92ad77b663f51b457e555a42d638b284ec99356b`.
- **Actual native run:** [Project Actions #40631111289](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40631111289), [artifact #12791746154](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40631111289/artifacts/12791746154). Original Linux RimWorld 1.6.4871 rev600, separate staged Vanilla+Environment game, Pickle/Quickstart **2/2 PASS**, zero Unity errors, zero skipped/failed, native process exit 0, no timeout, actual test DLL built zero warnings/errors.
- **Natural site:** original `AMJ_WarmTemperateForest` at +36.66°N, naturally selected Japan-like climate (15.82°C annual mean / 1560.4 mm world-tile annual rainfall). No manual biome/climate forcing.
- **Sample:** three deterministic random seeds (32197 / 32298 / 32399), original `WeatherDecider.StartNextWeather` with the engine-selected weather-event durations across a 60-game-day *calendar*, two-hour midpoint weather/sky/temperature sampling; respectively **44 / 42 / 43 weather events**. These are stepped event samples, **not 60 days of continuous map/world ticks**.
- **Crops:** original loaded `Plant_Potato` and `Plant_Rice`, one unroofed Soil test cell each; sample native `Plant.GrowthRate` at identical weather, temperature and clock states while toggling a disposable late postfix on `WeatherWorker.CurSkyTarget`.
- **Control:** the 0.3 weather-choice windows/durations are **kept active in both treatments**. Only Environment's extra weather sky-glow factor (Rain/FoggyRain ×0.90; thunderstorm ×0.70; Fog ×0.90) is temporarily cancelled in the control. Native measured growth rates are weighted by sampled time. This is **not** an actual 0.2.0-versus-0.3.0, Vanilla-versus-Environment, or crop harvested-yield A/B comparison. The test-only postfix never changes the Environment production files, accepted parameters, installed subscriber package or any player's saves.

## Results: native Plant.GrowthRate, 0.3 actual / same-weather no-added-glow control

| Seed | Weather events | Potato ratio | Rice ratio | Extra light-penalty loss |
|---|---:|---:|---:|---:|
| 0 | 44 | 0.9131 | 0.9131 | 8.69% |
| 1 | 42 | 0.9067 | 0.9067 | 9.33% |
| 2 | 43 | 0.9154 | 0.9154 | 8.46% |
| **Unweighted mean across seeds** | — | **0.9117** | **0.9117** | **8.83%** |

The two crops happen to return identical normalized growth-rate ratios in these sampled conditions; this does **not** imply they have identical growDays, net harvest output, nutrient value, seasonal cold hardiness or labor economics.

### Mean rate ratios by 0.3 northern-season window (equal weight per seed)

| Window | Local game days | Seed 0 | Seed 1 | Seed 2 | Mean ratio | Mean added rate loss |
|---|---|---:|---:|---:|---:|---:|
| Tsuyu | 16–22 | 0.8600 | 0.9025 | 0.8384 | 0.8670 | 13.30% |
| Midsummer clear-weight bonus | 23–29 | 0.8668 | 0.7990 | 0.9067 | 0.8575 | 14.25% |
| Akisame | 30–38 | 0.9342 | 0.9504 | 0.9567 | 0.9471 | 5.29% |
| Autumn clear-weight bonus | 39–44 | 0.8487 | 0.9083 | 0.9181 | 0.8917 | 10.83% |
| Other | remaining 31 days | 0.9453 | 0.9212 | 0.9225 | 0.9297 | 7.03% |

Ratios for rice and potato were identical in the test observations. Seasonal averages above are a descriptive equal-seed average, **not** a day-weighted single yearly aggregate. The annual ratios must be read from the separate first table.

**Interpretation:** Rain/cloud gameplay-light attenuation measurably lowers growth-rate potential on the sampled site, especially around Tsuyu and the following midsummer window. The `Clear ×1.25` candidate-selection bonus in the midsummer/autumn windows is **not a guarantee of dry weather, a daylight deficit offset, or an annual-harvest-neutral rule**. Three deterministic weather paths do not establish whether these results represent typical, rare or excessive agricultural difficulty. A ratio near 0.91 does not establish a 9% harvest loss.

## Outstanding acceptance / do not conflate

- **Not tested:** actual full 60-day map ticks, repeated planting, harvest and pawn Jobs, realized mature crop counts, annual edible output, changes in food stocks, multiple planting dates, broader latitude/elevation/soil, CCTO, MO + Agriculture, mixed Mod weather, solar energy totals, old saves, 1.7, exact Steam-distributed product.
- **Not compared:** 0.2.0/Vanilla's *native seasonal weather event schedule* versus 0.3.0's redistributed weather frequency. This test isolates the **additional sunlight attenuation** under the current 0.3.0 event sequence only. The full 0.3 weather-frequency *plus* glow effect versus 0.2 remains quantitatively unaccepted.
- **No invented acceptance threshold:** the present source provides no independently justified numerical boundary for 'excessive'. The author can assess the impact for intended play; further testing must be tied to a concrete balance decision, rather than expanded to obtain more PASS marks.
- **Independent release-copy FAIL not waived:** the first [run #40630800272](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40630800272) stopped *before* native launch because the current 0.3 `Tests/validate_workshop_description.py` found a combined Workshop description **over 8,000 UTF-8 bytes**. The focused second-run Project-only test harness marked this as `FAILED_EXISTING_SOURCE_8000B_NOT_RELEASE_ELIGIBLE` and ran the **three other original package checks** unmodified so the research-only game candidate could build. All three remaining static tests passed. **0.3 release readiness remains HOLD**, and this successful native observation cannot be used to assert the release-copy requirement passed.

**Gameplay source and numerical configuration were not modified by this measurement.**
