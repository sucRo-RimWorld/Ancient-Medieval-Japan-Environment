using System;
using System.Reflection;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace AncientMedievalJapan.Environment
{
    [StaticConstructorOnStartup]
    public static class Bootstrap
    {
        private const float DailyVariationScale = 4f / 7f;

        static Bootstrap()
        {
            Harmony harmony = new Harmony("sucro.ancientmedievaljapan.environment");
            harmony.PatchAll();

            Type cachedType = typeof(TileTemperaturesComp).GetNestedType(
                "CachedTileTemperatureData",
                BindingFlags.NonPublic);

            MethodInfo target = cachedType == null
                ? null
                : cachedType.GetMethod(
                    "OffsetFromDailyRandomVariation",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MethodInfo postfix = typeof(Bootstrap).GetMethod(
                "ScaleDailyVariationPostfix",
                BindingFlags.Static | BindingFlags.NonPublic);

            if (target != null && postfix != null)
            {
                harmony.Patch(target, null, new HarmonyMethod(postfix));
            }
            else
            {
                Log.Warning(
                    "[AMJ Environment] Could not locate RimWorld daily temperature variation method. " +
                    "World generation will continue, but daily temperature variation will remain Vanilla.");
            }
        }

        private static void ScaleDailyVariationPostfix(ref float __result)
        {
            __result *= DailyVariationScale;
        }
    }
}
