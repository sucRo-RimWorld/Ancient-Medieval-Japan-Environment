# Tsuyu / Akisame: period-scale climate calibration and crop-light risk (2026-10-21)

Owner: Ancient-Medieval-Japan-Environment. **Research and numerical decision gate, not author-approved final multiplier values.** This document supplements [Design 4.1](../Design.md#41-provisional-tsuyuakisame-seasonal-weather-selection-2026-10-21), without modifying already implemented runtime behavior.

## Scope and decision

1. Preserve currently implemented seasonal weather identity and rain/weather light factors **temporarily** while aggregate meteorological and game-crop outcomes are unknown. Do **not** silently replace 0.80 by 0.90 merely because a single daylight sample showed a 45.5% drop in the potato light-growth **component**.
2. Derive target rain **day frequency / wet-spell lengths** from Japanese station data and compare to game **elapsed weather intervals**, not 240 independent weather chooser draws at a frozen state. Do not treat the game map's annual rainfall (mm) as a measured result of its local weather events.
3. Derive target **period-mean global irradiance** and separately target **sunshine duration**, both relative to a climate/season reference. Irradiance (direct + diffuse) is **not equivalent** to sunshine-hours threshold, nor is either identical to an instantaneous `SkyTarget.glow` factor.
4. Keep ordinary rain and thunderstorm daylight attenuation one owned path using `WeatherWorker.CurSkyTarget`; no separate direct plant growth patch, solar patch, or Dynamic Atmosphere cloud shader/overlay replacement. Check the live crop/solar nonlinear outputs after final climate tuning.
5. Region/latitude is a **model ownership constraint**. Environment's four Japan-oriented biome Defs and the retained Vanilla `TemperateSwamp` / `ColdBog` currently share one calendar; recorded ColdBog test latitude ~50.8° is a technical load/behavior check, **not** a Tokyo/Japan rainfall calibration point. WorldMap's projection or further district climate is not implemented here.

## Meteorological evidence — Japan Meteorological Agency normal values, 1991–2020

**JMA Tokyo station (47662), 10-day `nml_sfc_10d` 30-year normals, 1991–2020:** [official table](https://www.data.jma.go.jp/stats/etrn/view/nml_sfc_10d.php?block_no=47662&prec_no=44). The table's 全天日射量 values are **MJ/m² per day**, sunshine duration are **hours per 10-day period**; the 10-day windows include all natural weather, not only Rain and FoggyRain.

| Tokyo normal 10-day period | Precipitation (mm over period) | Sunshine (hours in period) | Global solar irradiation (MJ/m²/day) | Context |
|---|---:|---:|---:|---|
| May 21–31 | 58.9 | 65.5 | 17.8 | Pre-rainy baseline comparison; actually 11 days |
| June 11–20 | 67.5 | 38.6 | 14.3 | Core Tsuyu |
| June 21–30 | 57.1 | 33.0 | 13.2 | Core Tsuyu |
| July 21–31 | 43.3 | 59.6 | 16.3 | Later summer comparison; actually 11 days |
| September 11–20 | 75.1 | 42.5 | 12.0 | Akisame-related autumn period |

Note: published values for 旬 can include 11 days in the final part of 31-day months; compare irradiation (per-day) safely across uneven period lengths, but **do not divide sunshine hours as if all periods had identical duration**.

- June 11–20 global irradiation vs late-May value: **14.3/17.8 ≈ 0.803**; June 21–30 **13.2/17.8 ≈ 0.742**. These ratios are *whole-period station observations*, not recommended individual-rain `SkyTarget.glow` factors. Natural astronomical/seasonal changes, weather coverage and location all differ.
- For comparison, JMA [daily June precipitation and sunshine normals](https://ds.data.jma.go.jp/stats/etrn/view/nml_sfc_d.php?block_no=47662&day=&month=6&prec_no=44&view=a1s&year=) indicate `>=1.0 mm` rain-day fraction about **0.42–0.43** around June 16–25. The same [July daily table](https://www.data.jma.go.jp/stats/etrn/view/nml_sfc_d.php?block_no=47662&day=&month=7&prec_no=44&view=a1&year=) has **0.23–0.27** around July 26–31. These are *probabilities of a calendar day exceeding 1 mm*, not fraction of time a weather Def is selected or hours actually raining.
- Autumn [September daily normals](https://www.data.jma.go.jp/stats/etrn/view/nml_sfc_d.php?block_no=47662&day=&month=9&prec_no=44&view=a1&year=) show about **0.36–0.39** fraction of days at `>=1mm` around September 11–30.
- [JMA Kanto/Koshin seasonal normal](https://www.data.jma.go.jp/cpd/baiu/kako_baiu09) is roughly **June 7 to July 19**. [JMA national FAQ](https://www.jma.go.jp/jma/kishou/know/faq/faq33.html) explicitly describes unsettled/slower transitions, regional movement of the rain front, occasional dry breaks, and exceptional years with unclear rainy-period boundaries. Using only this one Tokyo normal to assert every Japanese biome's weather is inaccurate.
- All values describe **modern observations**. They do not establish quantitative medieval Japanese daily rainfall or solar radiation. They are the most reproducible *environmental reference proxy* until an appropriate historical station or paleoclimate source is justified.

## Verified real-game 1.6 sunlight consumer, with sensitivity envelope

The actual **single loaded daytime ColdBog** [Project run #39576155780](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39576155780) measured native Clear `SkyManager.CurSkyGlow=0.911`, potato `GrowthRateFactor_Light=0.818`, desired solar output `1548.5 W`; when Rain, `glow=0.729`, potato `0.446`, desired solar output `1238.8 W`. Full 29/29 original-game check, ERROR0, genuine save/reload PASS. This confirms crop factor drop **45.5%** at that point, not final growth or harvest. `Plant_Potato` `growMinGlow=0.510`, `growOptimalGlow=1.000`. Vanilla light factor is effectively `clamp((glow−min)/(optimal−min),0,1)` for that plant; real values below are a **static algebraic sensitivity study**, not independent 1.6/1.7 native tests.

| Hypothetical rain glow multiplier | Sky glow at the *same* Clear 0.911 | Predicted potato light factor | Decrease from clear potato light factor |
|---:|---:|---:|---:|
| 1.00 | 0.911 | 0.818 | 0.0% |
| 0.95 | 0.865 | 0.725 | 11.4% |
| 0.90 | 0.820 | 0.632 | 22.7% |
| 0.85 | 0.774 | 0.539 | 34.1% |
| 0.80 | 0.729 | 0.447 | 45.4% |
| 0.70 | 0.638 | 0.261 | 68.2% |

These rows must not be interpreted as final recommendations. The first two decimal values in actual native evidence use the fully loaded map; the sensitivity table assumes light at an unroofed same cell equals calculated scaled `CurSkyGlow`. Light response can change with lamp, roof, altitude/season, plant Def-specific `growMinGlow` and `growOptimalGlow`, and weather transitions.

**Whole-period confounding:** Hypothetically, if only 40% of the period's daylight hours were at `Rain` and every other hour behaved like `Clear`, a 0.80 rainy-hour factor lowers **linear mean sky glow only about 8%**, not 20%, even though light-growth factor during the selected rainy hours drops about 45.5% for the sampled potato. This is illustrative arithmetic and **not** an assertion that real-game rainy hours are 40%. The observed JMA whole-period irradiation drop around Tsuyu can neither verify nor reject the current per-weather 0.80 on its own.

## Rain-duration / spell risk and baseline engine constraints

- Current `SeasonalRainWeatherPatch.cs`: days 15–23 rain candidate weights x1.65, autumn days 30–38 x1.40, wet-to-wet different rain types x1.15; Clear x0.75 or x0.85. Vanilla `CurrentWeatherCommonality` retains temperature/rain prohibition, and uses `rainfall` in its base weights. These multipliers are **weather-event selection weights**, not a guaranteed frequency of rain in elapsed hours or 1 mm daily precipitation.
- Reference old decompile [WeatherDecider](https://github.com/josh-m/RW-Decompile/blob/master/RimWorld/WeatherDecider.cs) picks weather and then samples each WeatherDef `durationRange`; non-repeatable same weather cannot be selected immediately again. Reference old [WeatherDef](https://github.com/josh-m/RW-Decompile/blob/master/Verse/WeatherDef.cs) has default range `16000–160000 ticks` (at 60,000 ticks/day, roughly 0.27–2.67 game-days). **The current installed 1.6/1.7 WeatherDef duration fields are NOT verified here, so do not use that legacy range as a measured 1.6 value.**
- The 1.6 ColdBog native 240 fixed-seed `ChooseNextWeather` draws at each of 5 dates show wet-choice counts 33,77,46,62,46. Draws held a weather state and seed fixed, did **not** advance `weatherManager.curWeatherAge` naturally or sample `durationRange`; no native streak-length evidence exists. In a window only nine game-days long, actual selected events may number far fewer than 240. The current x1.15 bonus for a wet predecessor does not force or assure a multi-day uninterrupted rain; even a same-name nonrepeatability check can restrict it.
- **Evaluate actual weather transitions and gameplay daylight as a coupled system only when balance changes are committed.** At least one representative Japan-forest biome under normal unlocked rain, and explicit weather elapsed `start/end, defName, rainRate, real-duration-in-game-hours` across full 60-game-day samples and distinct fixed seeds (random-weather variation). Report wet hours, rain-days meeting a defined threshold, wet spells, uninterrupted sunny breaks, total daylight-weighted `CurSkyGlow`, native potato crop-light factor, and separate solar demand. Compare target measures in **the same units** (JMA day threshold is not the RimWorld rainRate value).
- Before author acceptance of any tuned multiplier/season window: (a) no new WeatherDefs/effects needed by default; (b) no permanent new background profiler or gameplay tick loop; (c) no added native tests solely to grow PASS count; (d) native 1.6/1.7 after actual production changes only, with optional Dynamic Atmosphere real co-load if installed. Do not use separate WorldMap coordinates to override ordinary Environment weather.

## Recommended resolution

**HOLD the provisional per-rain 0.80/0.70/0.90 and seasonal 1.65/1.40/1.15 numbers as provisional, NOT approved final.** They are known to function as implemented; evidence is insufficient to choose a scientifically more realistic alternative number or spell controller. The next implementation slice should produce the missing elapsed-time temporal weather/daylight sample **as a disposable native observation**, after which prefer adjusting the existing source multipliers/short window edges, not inventing rain-front global game conditions, humidity state, or forced infinite rain. Keep the installed subscriber DLL/Steam Workshop unchanged until separately authorized.
