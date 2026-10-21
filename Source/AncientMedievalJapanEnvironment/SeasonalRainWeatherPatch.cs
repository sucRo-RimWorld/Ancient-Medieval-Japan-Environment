using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AncientMedievalJapan.Environment
{
    /// <summary>
    /// Changes only the relative chance of existing weather when the game next
    /// chooses weather. Never mutates the shared BiomeDef/weather definitions or
    /// the annual tile rainfall used by world and river generation.
    /// All factors and day windows below are provisional pending game sampling.
    /// </summary>
    [HarmonyPatch(typeof(WeatherDecider), "CurrentWeatherCommonality")]
    public static class SeasonalRainWeatherPatch
    {
        // 60-day RimWorld calendar: roughly June to mid-July and Sep to Oct.
        // Not a claim about every Japanese region or medieval observations.
        internal const int TsuyuStart = 15;
        internal const int TsuyuEnd = 23;
        internal const int AkisameStart = 30;
        internal const int AkisameEnd = 38;

        public static void Postfix(Map ___map, WeatherDef weather, ref float __result)
        {
            // Respect Vanilla zero weights: disallowed temperatures, fire/rain
            // bans, tutorial restrictions and unavailable biome weather.
            if (__result <= 0f || ___map == null || weather == null ||
                Find.TickManager == null || Find.WorldGrid == null)
            {
                return;
            }

            PlanetTile tile = ___map.Tile;
            if (!tile.Valid || !tile.Layer.IsRootSurface ||
                !IsJapanEnvironmentBiome(___map.Biome))
            {
                return;
            }

            float latitude = Find.WorldGrid.LongLatOf(tile).y;
            float longitude = Find.WorldGrid.LongLatOf(tile).x;
            int day = GenDate.DayOfYear(Find.TickManager.TicksAbs, longitude);

            // Respect the seasonal phase of real Southern Hemisphere maps.
            // WorldMap's virtual-latitude projection is outside this owner.
            if (latitude < 0f)
            {
                day = (day + 30) % GenDate.DaysPerYear;
            }

            bool tsuyu = day >= TsuyuStart && day <= TsuyuEnd;
            bool akisame = day >= AkisameStart && day <= AkisameEnd;
            if (!tsuyu && !akisame)
            {
                return;
            }

            switch (weather.defName)
            {
                case "Rain":
                case "FoggyRain":
                    __result *= tsuyu ? 1.65f : 1.40f;
                    // Encourage wet sequences but leave actual duration, 
                    // repeatable flags and all rain prohibitions to Vanilla.
                    if (___map.weatherManager != null &&
                        ___map.weatherManager.curWeather != null &&
                        ___map.weatherManager.curWeather.rainRate > 0.1f)
                    {
                        __result *= 1.15f;
                    }
                    break;
                case "RainyThunderstorm":
                    __result *= tsuyu ? 1.08f : 1.12f;
                    break;
                case "Fog":
                    __result *= 1.15f;
                    break;
                case "Clear":
                    __result *= tsuyu ? 0.75f : 0.85f;
                    break;
            }
        }

        internal static bool IsJapanEnvironmentBiome(BiomeDef biome)
        {
            if (biome == null)
            {
                return false;
            }

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
    }
}
