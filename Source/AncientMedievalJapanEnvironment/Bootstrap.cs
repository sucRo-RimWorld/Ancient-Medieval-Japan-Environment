using System;
using System.Reflection;
using System.Linq;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace AncientMedievalJapan.Environment
{
    [StaticConstructorOnStartup]
    public static class Bootstrap
    {
        private const float DailyVariationScale = 3f / 7f;

        static Bootstrap()
        {
            Harmony harmony = new Harmony("sucro.ancientmedievaljapan.environment");
            harmony.PatchAll();

            Log.Message("[AMJ Environment] Assembly loaded; Harmony patches applied.");

            ModContentPack environmentPack = LoadedModManager.RunningModsListForReading
                .FirstOrDefault(mod => mod.PackageIdPlayerFacing == "sucro.ancientmedievaljapan.environment");
            ModMetaData quickstartsByExpectedId =
                ModLister.GetActiveModWithIdentifier("rimworks.quickstarts", true);
            string quickstartsCandidates = string.Join(
                " || ",
                LoadedModManager.RunningModsListForReading
                    .Where(mod =>
                        mod.Name.IndexOf("Quickstart", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        mod.PackageIdPlayerFacing.IndexOf("quickstart", StringComparison.OrdinalIgnoreCase) >= 0)
                    .Select(mod =>
                        mod.Name + " [" + mod.PackageIdPlayerFacing + "]")
                    .ToArray());

            string loadFolders = environmentPack == null
                ? "(Environment ModContentPack not found)"
                : string.Join(" | ", environmentPack.foldersToLoadDescendingOrder.ToArray());

            Log.Message(
                "[AMJ Environment] Dev load-folder diagnostic" +
                " | expectedIdActive=" + (quickstartsByExpectedId != null) +
                " | candidates=" + (quickstartsCandidates.Length == 0 ? "(none)" : quickstartsCandidates) +
                " | folders=" + loadFolders);

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

        private static void ScaleDailyVariationPostfix(PlanetTile ___tile, ref float __result)
        {
            if (___tile.Valid && ___tile.Layer.IsRootSurface)
            {
                __result *= DailyVariationScale;
            }
        }
    }
}
