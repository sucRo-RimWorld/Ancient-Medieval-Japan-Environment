using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace AncientMedievalJapan.Environment
{
    /// <summary>
    /// Ordinary rain and fog tint the Vanilla sky without reducing its gameplay
    /// sunlight. Attenuating each weather worker's glow before Vanilla blends
    /// weather transitions lets plants and solar panels receive the same light
    /// change, without separate crop penalties or visual/cloud replacements.
    /// Provisional factors; calibrate against weather/plant gameplay.
    /// </summary>
    [HarmonyPatch(typeof(WeatherWorker), "CurSkyTarget")]
    public static class RainWeatherDaylightPatch
    {
        internal const float RainGlowFactor = 0.80f;
        internal const float ThunderstormGlowFactor = 0.70f;
        internal const float FogGlowFactor = 0.90f;

        public static void Postfix(Map map, WeatherDef ___def, ref SkyTarget __result)
        {
            if (map == null || ___def == null ||
                !SeasonalRainWeatherPatch.IsJapanEnvironmentBiome(map.Biome))
            {
                return;
            }

            PlanetTile tile = map.Tile;
            if (!tile.Valid || !tile.Layer.IsRootSurface)
            {
                return;
            }

            float factor;
            switch (___def.defName)
            {
                case "Rain":
                case "FoggyRain":
                    factor = RainGlowFactor;
                    break;
                case "RainyThunderstorm":
                    factor = ThunderstormGlowFactor;
                    break;
                case "Fog":
                    factor = FogGlowFactor;
                    break;
                default:
                    return;
            }

            // Glow is already bounded by the engine's celestial day/night cycle.
            // Do not override darkness from game conditions or artificial lamps.
            __result.glow *= factor;
        }
    }
}
