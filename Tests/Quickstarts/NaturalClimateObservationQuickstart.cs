using System;
using HarmonyLib;
using RimWorks.Quickstarts;
using RimWorks.Quickstarts.Verification;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AncientMedievalJapan.Environment.Quicktests
{
    // Disposable climate-observation site input. Unlike the ordinary biome
    // Quickstarts, never force any tile biome, rainfall or temperature.
    public sealed class AMJNaturalClimateObservationQuickstart : AbstractQuickstart
    {
        private PlanetTile selected = PlanetTile.Invalid;
        private bool geographicMatch;

        public override TaggedString description
        {
            get { return "Select an unmodified Japan-like climate site for temporary weather observation."; }
        }

        public override string seed
        {
            get { return "AMJ-Environment-NaturalClimate-Observation"; }
        }

        public override float planetCoverage
        {
            get { return 1.0f; }
        }

        public override int mapSize
        {
            get { return 250; }
        }

        public override void PostApplyConfiguration()
        {
            float bestScore = float.MaxValue;
            // Climate representativeness is the primary gate. Geographic
            // representativeness is independently reported, never fabricated.
            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                SurfaceTile surface = Find.WorldGrid[i] as SurfaceTile;
                if (surface == null || surface.WaterCovered ||
                    surface.PrimaryBiome == null ||
                    !IsClimateBiome(surface.PrimaryBiome))
                {
                    continue;
                }

                PlanetTile tile = surface.tile;
                if (!TileFinder.IsValidTileForNewSettlement(tile) ||
                    Find.WorldObjects.AnyWorldObjectAt(tile))
                {
                    continue;
                }

                if (surface.temperature < 12f || surface.temperature > 18f ||
                    surface.rainfall < 1000f || surface.rainfall > 2200f)
                {
                    continue;
                }

                float latitude = Find.WorldGrid.LongLatOf(tile).y;
                // Seasonal window comparison is calibrated for northern Japan.
                if (latitude <= 0f) continue;
                bool inGeographicBand = latitude >= 32f && latitude <= 38f;
                // Give a real 32-38 N site priority only among otherwise
                // climate-eligible sites; never enforce the latitude by mutation.
                float score = (inGeographicBand ? 0f : 1000f) +
                    Math.Abs(surface.temperature - 15.8f) * 10f +
                    Math.Abs(surface.rainfall - 1598.2f) / 100f;
                if (score < bestScore)
                {
                    bestScore = score;
                    selected = tile;
                    geographicMatch = inGeographicBand;
                }
            }

            if (!selected.Valid)
            {
                throw new InvalidOperationException(
                    "No natural Japan-like climate site: no biome/temperature/rainfall mutation permitted.");
            }

            Find.GameInitData.startingTile = selected;
            SurfaceTile actual = Find.WorldGrid[selected] as SurfaceTile;
            float actualLatitude = Find.WorldGrid.LongLatOf(selected).y;
            Log.Message(
                "[AMJE_NATURAL_CLIMATE_SITE] tile=" + selected +
                " lat=" + actualLatitude.ToString("F2") +
                " annualTempC=" + actual.temperature.ToString("F2") +
                " annualRainMm=" + actual.rainfall.ToString("F1") +
                " biome=" + actual.PrimaryBiome.defName +
                " geographic32to38=" + geographicMatch +
                " forcedClimate=False forcedBiome=False");
        }

        private static bool IsClimateBiome(BiomeDef biome)
        {
            switch (biome.defName)
            {
                case "AMJ_WarmTemperateForest":
                case "AMJ_CoolTemperateForest":
                case "AMJ_SubalpineForest":
                case "AMJ_AlpineZone":
                case "TemperateSwamp":
                case "ColdBog":
                    return true;
                default:
                    return false;
            }
        }

        private bool weatherObserved;

        public override void PostLoaded()
        {
            base.PostLoaded();
            if (Find.CurrentMap == null || !selected.Valid ||
                Find.CurrentMap.Tile != selected) return;
            weatherObserved = ObserveWeather(Find.CurrentMap);
        }

        // Event-duration sampling, not 60 days of world ticks or crop growth.
        private static bool ObserveWeather(Map map)
        {
            var duration = AccessTools.Field(typeof(WeatherDecider), "curWeatherDuration");
            var cooldown = AccessTools.Field(typeof(WeatherDecider), "ticksWhenRainAllowedAgain");
            var advance = AccessTools.Method(typeof(WeatherDecider), "StartNextWeather");
            if (duration == null || cooldown == null || advance == null) return false;

            int tick = Find.TickManager.TicksGame;
            int oldDuration = (int)duration.GetValue(map.weatherDecider);
            int oldCooldown = (int)cooldown.GetValue(map.weatherDecider);
            WeatherDef oldCurrent = map.weatherManager.curWeather;
            WeatherDef oldPrevious = map.weatherManager.lastWeather;
            int oldAge = map.weatherManager.curWeatherAge;
            double[] wet = new double[3];
            double[] span = new double[3];
            double[] realGlow = new double[3];
            double[] clearGlow = new double[3];
            WeatherDef clear = DefDatabase<WeatherDef>.GetNamedSilentFail("Clear");
            if (clear == null) return false;
            int events = 0;
            int elapsed = 0;
            const int limit = GenDate.DaysPerYear * GenDate.TicksPerDay;
            const int step = 2 * GenDate.TicksPerHour;
            float longitude = Find.WorldGrid.LongLatOf(map.Tile).x;
            Rand.PushState(32197);
            try
            {
                while (elapsed < limit && events < 512)
                {
                    Find.TickManager.DebugSetTicksGame(tick + elapsed);
                    Find.World.tileTemperatures.WorldComponentTick();
                    object cached = AccessTools.Method(
                        typeof(TileTemperaturesComp), "RetrieveCachedData")
                        .Invoke(Find.World.tileTemperatures, new object[] { map.Tile });
                    AccessTools.Method(cached.GetType(), "CheckCache").Invoke(cached, null);
                    map.mapTemperature.TemperatureUpdate();
                    advance.Invoke(map.weatherDecider, null);
                    events++;
                    int length = (int)duration.GetValue(map.weatherDecider);
                    if (length <= 0) return false;
                    int used = Math.Min(length, limit - elapsed);
                    for (int offset = 0; offset < used; offset += step)
                    {
                        int count = Math.Min(step, used - offset);
                        Find.TickManager.DebugSetTicksGame(tick + elapsed + offset + count / 2);
                        map.weatherManager.curWeatherAge = offset + count / 2;
                        map.skyManager.SkyManagerUpdate();
                        int day = GenDate.DayOfYear(Find.TickManager.TicksAbs, longitude);
                        int band = day >= 15 && day <= 23 ? 0 :
                            day >= 30 && day <= 38 ? 1 : 2;
                        span[band] += count;
                        if (map.weatherManager.RainRate > 0.1f) wet[band] += count;
                        float baselineGlow = clear.Worker.CurSkyTarget(map).glow;
                        if (baselineGlow > 0.05f)
                        {
                            realGlow[band] += map.skyManager.CurSkyGlow * count;
                            clearGlow[band] += baselineGlow * count;
                        }
                    }
                    elapsed += used;
                    map.weatherManager.curWeatherAge = length + 1;
                }
                Log.Message("AMJE_NATURAL_RAIN_OBSERVATION" +
                    " method=event_duration_not_full_ticks" +
                    " events=" + events +
                    " tsuyuWetHours=" + (wet[0] / GenDate.TicksPerHour).ToString("F1") +
                    " akisameWetHours=" + (wet[1] / GenDate.TicksPerHour).ToString("F1") +
                    " otherWetHours=" + (wet[2] / GenDate.TicksPerHour).ToString("F1") +
                    " tsuyuTotalHours=" + (span[0] / GenDate.TicksPerHour).ToString("F1") +
                    " akisameTotalHours=" + (span[1] / GenDate.TicksPerHour).ToString("F1") +
                    " otherTotalHours=" + (span[2] / GenDate.TicksPerHour).ToString("F1") +
                    " tsuyuDaylightRatio=" + (realGlow[0] / Math.Max(1, clearGlow[0])).ToString("F3") +
                    " akisameDaylightRatio=" + (realGlow[1] / Math.Max(1, clearGlow[1])).ToString("F3") +
                    " otherDaylightRatio=" + (realGlow[2] / Math.Max(1, clearGlow[2])).ToString("F3"));
                return elapsed == limit && events < 512 &&
                    span[0] > 0 && span[1] > 0 && span[2] > 0;
            }
            finally
            {
                Rand.PopState();
                Find.TickManager.DebugSetTicksGame(tick);
                duration.SetValue(map.weatherDecider, oldDuration);
                cooldown.SetValue(map.weatherDecider, oldCooldown);
                map.weatherManager.curWeather = oldCurrent;
                map.weatherManager.lastWeather = oldPrevious;
                map.weatherManager.curWeatherAge = oldAge;
                Find.World.tileTemperatures.WorldComponentTick();
                map.mapTemperature.TemperatureUpdate();
                map.skyManager.SkyManagerUpdate();
            }
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification checks = new QuickstartVerification();
            checks.Assert("natural climate site remains loaded", () =>
                selected.Valid && Find.CurrentMap != null &&
                Find.CurrentMap.Tile == selected);
            checks.Assert("native weather observation completed", () => weatherObserved);
            return checks;
        }
    }
}
