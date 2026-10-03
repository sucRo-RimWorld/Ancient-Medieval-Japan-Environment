using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace AncientMedievalJapan.Environment
{
    [HarmonyPatch(typeof(GenTemperature), "SeasonalShiftAmplitudeAt")]
    public static class Patch_SeasonalShiftAmplitudeAt
    {
        public static void Postfix(PlanetTile tile, ref float __result)
        {
            if (!tile.Valid || !tile.Layer.IsRootSurface)
            {
                return;
            }

            float distance = Find.WorldGrid.DistanceFromEquatorNormalized(tile);
            float amplitude = Mathf.Lerp(8f, 16f, distance);
            float latitude = Find.WorldGrid.LongLatOf(tile).y;

            __result = latitude >= 0f ? amplitude : -amplitude;
        }
    }
}
