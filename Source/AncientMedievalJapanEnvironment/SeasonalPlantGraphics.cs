using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace AncientMedievalJapan.Environment
{
    /// <summary>
    /// An optional visual window for AMJE's forthcoming native seasonal plants.
    /// Days are 1..15 within a quadrum, with an exclusive end boundary.
    /// The winter-to-spring wrap is supported. This never changes Plant growth,
    /// harvesting, CCTO dormancy or the Def identity stored in a save.
    /// </summary>
    public sealed class SeasonalPlantGraphicWindow
    {
        public Season startSeason = Season.Spring;
        public int startDay = 1;
        public Season endSeason = Season.Summer;
        public int endDay = 1;
        public bool allowWhileLeafless;
        public GraphicData graphicData;
    }

    /// <summary>
    /// Opt-in per ThingDef, rather than a global replacement of all plant graphics.
    /// Keep absent until the species' normal/seasonal art is accepted and installed.
    /// Windows are evaluated in XML order; the first eligible window is used.
    /// </summary>
    public sealed class SeasonalPlantAppearanceExtension : DefModExtension
    {
        public List<SeasonalPlantGraphicWindow> windows;
    }

    internal static class SeasonalPlantGraphics
    {
        // A weak table avoids serialization and retaining removed plants indefinitely.
        private sealed class LastAppearance
        {
            internal int windowIndex = int.MinValue;
        }

        private static readonly ConditionalWeakTable<Plant, LastAppearance> LastShown =
            new ConditionalWeakTable<Plant, LastAppearance>();

        internal static int ActiveWindowIndex(Plant plant, SeasonalPlantAppearanceExtension extension)
        {
            if (plant == null || plant.Map == null || plant.LifeStage != PlantLifeStage.Mature ||
                extension == null || extension.windows == null)
            {
                return -1;
            }

            int ordinal = LocalSeasonOrdinal(plant.Map);
            if (ordinal < 0)
            {
                return -1;
            }

            for (int i = 0; i < extension.windows.Count; i++)
            {
                SeasonalPlantGraphicWindow window = extension.windows[i];
                if (window == null || window.graphicData == null ||
                    (!window.allowWhileLeafless && plant.LeaflessNow))
                {
                    continue;
                }

                int start = WindowOrdinal(window.startSeason, window.startDay);
                int end = WindowOrdinal(window.endSeason, window.endDay);
                if (start < 0 || end < 0 || start == end)
                {
                    continue;
                }

                bool matches = start < end
                    ? ordinal >= start && ordinal < end
                    : ordinal >= start || ordinal < end;

                if (matches)
                {
                    return i;
                }
            }

            return -1;
        }

        private static int WindowOrdinal(Season season, int day)
        {
            if (day < 1 || day > 15)
            {
                return -1;
            }

            switch (season)
            {
                case Season.Spring: return day - 1;
                case Season.Summer: return 15 + day - 1;
                case Season.Fall: return 30 + day - 1;
                case Season.Winter: return 45 + day - 1;
                default: return -1;
            }
        }

        private static int LocalSeasonOrdinal(Map map)
        {
            if (map == null)
            {
                return -1;
            }

            Season season = GenDate.Season(
                Find.TickManager.TicksAbs, Find.WorldGrid.LongLatOf(map.Tile));

            // Vanilla can report permanent seasons on maps near the equator.
            // Still present the normal 60-day appearance calendar there, without
            // changing the map's permanent growing season or weather.
            if (season == Season.PermanentSummer || season == Season.PermanentWinter)
            {
                return GenLocalDate.DayOfYear(map) % 60;
            }

            return WindowOrdinal(season, GenLocalDate.DayOfQuadrum(map) + 1);
        }

        internal static void RefreshAfterLongTick(Plant plant)
        {
            if (plant == null || !plant.Spawned || plant.Map == null)
            {
                return;
            }

            SeasonalPlantAppearanceExtension extension =
                plant.def.GetModExtension<SeasonalPlantAppearanceExtension>();
            if (extension == null || extension.windows == null || extension.windows.Count == 0)
            {
                return;
            }

            int active = ActiveWindowIndex(plant, extension);
            LastAppearance state = LastShown.GetOrCreateValue(plant);
            if (state.windowIndex == int.MinValue)
            {
                state.windowIndex = active;
                return;
            }

            if (state.windowIndex != active)
            {
                state.windowIndex = active;
                plant.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlagDefOf.Things);
            }
        }
    }

    // Only AMJE plants with the extension can override the engine's Graphic.
    // Outside a configured interval the original leafless/snow/fall rendering wins.
    [HarmonyPatch(typeof(Plant), nameof(Plant.Graphic), MethodType.Getter)]
    internal static class Patch_Plant_Graphic_SeasonalAppearance
    {
        [HarmonyPostfix]
        private static void Postfix(Plant __instance, ref Graphic __result)
        {
            if (__instance == null || __instance.Map == null)
            {
                return;
            }

            SeasonalPlantAppearanceExtension extension =
                __instance.def.GetModExtension<SeasonalPlantAppearanceExtension>();
            if (extension == null || extension.windows == null)
            {
                return;
            }

            int index = SeasonalPlantGraphics.ActiveWindowIndex(__instance, extension);
            if (index >= 0)
            {
                __result = extension.windows[index].graphicData.Graphic;
            }
        }
    }

    [HarmonyPatch(typeof(Plant), nameof(Plant.TickLong))]
    internal static class Patch_Plant_TickLong_SeasonalAppearance
    {
        [HarmonyPostfix]
        private static void Postfix(Plant __instance)
        {
            SeasonalPlantGraphics.RefreshAfterLongTick(__instance);
        }
    }
}
