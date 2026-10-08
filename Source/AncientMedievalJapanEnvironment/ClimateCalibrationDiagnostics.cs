using System;
using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace AncientMedievalJapan.Environment
{
    [HarmonyPatch(typeof(Game), "InitNewGame")]
    public static class ClimateCalibrationDiagnostics
    {
        private const int HoursPerYear = 60 * 24;
        private const int TicksPerHour = 2500;

        private static readonly float[] Thresholds =
        {
            10f, 8f, 5f, 0f, -1f, -4f, -8f
        };

        public static void Postfix()
        {
            World world = Find.World;
            if (world == null || world.grid == null ||
                world.tileTemperatures == null ||
                Find.TickManager == null ||
                Find.TickManager.gameStartAbsTick == 0)
            {
                return;
            }

            PlanetLayer layer = world.grid.Surface;
            if (layer == null || layer.TilesCount == 0)
            {
                return;
            }

            LogRepresentative(world, layer, "WarmLowland",
                FindRepresentative(layer, 18.5f, 0f, 300f, 100f));
            LogRepresentative(world, layer, "TemperateLowland",
                FindRepresentative(layer, 14f, 0f, 300f, 100f));
            LogRepresentative(world, layer, "CoolLowland",
                FindRepresentative(layer, 7f, 0f, 300f, 100f));
            LogRepresentative(world, layer, "Highland",
                FindRepresentative(layer, 2f, 1500f, 3200f, 1900f, true));
        }

        private static PlanetTile FindRepresentative(
            PlanetLayer layer,
            float targetAnnualTemperature,
            float minElevation,
            float maxElevation,
            float targetElevation,
            bool allowImpassableFallback = false)
        {
            // Prefer playable representatives in the Northern Hemisphere,
            // then search both hemispheres. For the Highland climate-only
            // probe, fall back to impassable mountain tiles *only* when no
            // playable highland tile exists in the generated small world.
            PlanetTile result = FindRepresentative(
                layer, targetAnnualTemperature, minElevation,
                maxElevation, targetElevation, true, false);

            if (!result.Valid)
            {
                result = FindRepresentative(
                    layer, targetAnnualTemperature, minElevation,
                    maxElevation, targetElevation, false, false);
            }

            if (!result.Valid && allowImpassableFallback)
            {
                result = FindRepresentative(
                    layer, targetAnnualTemperature, minElevation,
                    maxElevation, targetElevation, true, true);

                if (!result.Valid)
                {
                    result = FindRepresentative(
                        layer, targetAnnualTemperature, minElevation,
                        maxElevation, targetElevation, false, true);
                }
            }

            return result;
        }

        private static PlanetTile FindRepresentative(
            PlanetLayer layer,
            float targetAnnualTemperature,
            float minElevation,
            float maxElevation,
            float targetElevation,
            bool northernHemisphereOnly,
            bool allowImpassable)
        {
            PlanetTile best = PlanetTile.Invalid;
            float bestScore = float.MaxValue;

            for (int i = 0; i < layer.TilesCount; i++)
            {
                SurfaceTile tile = layer.Tiles[i] as SurfaceTile;
                if (tile == null || tile.WaterCovered ||
                    (!allowImpassable && tile.hilliness == Hilliness.Impassable))
                {
                    continue;
                }

                PlanetTile planetTile = new PlanetTile(i, layer);
                float latitude = layer.LongLatOf(planetTile).y;

                if (northernHemisphereOnly && latitude < 0f)
                {
                    continue;
                }

                if (tile.elevation < minElevation ||
                    tile.elevation > maxElevation)
                {
                    continue;
                }

                float score =
                    Mathf.Abs(tile.temperature - targetAnnualTemperature) * 8f +
                    Mathf.Abs(tile.elevation - targetElevation) * 0.003f;

                if (score < bestScore)
                {
                    best = planetTile;
                    bestScore = score;
                }
            }

            return best;
        }

        private static void LogRepresentative(
            World world,
            PlanetLayer layer,
            string label,
            PlanetTile tile)
        {
            if (!tile.Valid)
            {
                Log.Warning(
                    "[AMJ Environment] Climate calibration | " + label +
                    " | no representative tile found.");
                return;
            }

            SurfaceTile surfaceTile = layer.Tiles[tile.tileId] as SurfaceTile;
            if (surfaceTile == null)
            {
                return;
            }

            int[] belowHours = new int[Thresholds.Length];
            int[] eventCounts = new int[Thresholds.Length];
            int[] shortEventCounts = new int[Thresholds.Length];
            int[] currentRuns = new int[Thresholds.Length];
            int[] maxRuns = new int[Thresholds.Length];
            bool[] inRun = new bool[Thresholds.Length];

            float minActual = float.MaxValue;
            float maxActual = float.MinValue;

            for (int hour = 0; hour < HoursPerYear; hour++)
            {
                int absTick = hour * TicksPerHour + TicksPerHour / 2;
                float temperature =
                    world.tileTemperatures.OutdoorTemperatureAt(tile, absTick);

                if (temperature < minActual) minActual = temperature;
                if (temperature > maxActual) maxActual = temperature;

                for (int t = 0; t < Thresholds.Length; t++)
                {
                    bool below = temperature < Thresholds[t];

                    if (below)
                    {
                        belowHours[t]++;

                        if (!inRun[t])
                        {
                            inRun[t] = true;
                            eventCounts[t]++;
                            currentRuns[t] = 1;
                        }
                        else
                        {
                            currentRuns[t]++;
                        }
                    }
                    else if (inRun[t])
                    {
                        FinishRun(t, inRun, currentRuns, maxRuns, shortEventCounts);
                    }
                }
            }

            for (int t = 0; t < Thresholds.Length; t++)
            {
                if (inRun[t])
                {
                    FinishRun(t, inRun, currentRuns, maxRuns, shortEventCounts);
                }
            }

            float latitude = layer.LongLatOf(tile).y;
            string biomeName =
                surfaceTile.PrimaryBiome == null
                    ? "null"
                    : surfaceTile.PrimaryBiome.defName;

            Log.Message(
                "[AMJ Environment] Climate calibration" +
                " | " + label +
                " tile=" + tile.tileId +
                " lat=" + latitude.ToString("F1") +
                " annual=" + surfaceTile.temperature.ToString("F1") +
                "C elev=" + surfaceTile.elevation.ToString("F0") + "m" +
                " hill=" + surfaceTile.hilliness +
                " biome=" + biomeName +
                " | actual=" + minActual.ToString("F1") +
                ".." + maxActual.ToString("F1") + "C" +
                " | belowHours" +
                " <10=" + FormatHours(belowHours[0]) +
                " <8=" + FormatHours(belowHours[1]) +
                " <5=" + FormatHours(belowHours[2]) +
                " <0=" + FormatHours(belowHours[3]) +
                " <-1=" + FormatHours(belowHours[4]) +
                " <-4=" + FormatHours(belowHours[5]) +
                " <-8=" + FormatHours(belowHours[6]) +
                " | lethalEvents" +
                " <-1=" + FormatEvents(eventCounts[4], shortEventCounts[4], maxRuns[4]) +
                " <-4=" + FormatEvents(eventCounts[5], shortEventCounts[5], maxRuns[5]) +
                " <-8=" + FormatEvents(eventCounts[6], shortEventCounts[6], maxRuns[6]));
        }

        private static void FinishRun(
            int index,
            bool[] inRun,
            int[] currentRuns,
            int[] maxRuns,
            int[] shortEventCounts)
        {
            if (currentRuns[index] > maxRuns[index])
            {
                maxRuns[index] = currentRuns[index];
            }

            if (currentRuns[index] <= 6)
            {
                shortEventCounts[index]++;
            }

            inRun[index] = false;
            currentRuns[index] = 0;
        }

        private static string FormatHours(int hours)
        {
            return hours + "h/" +
                ((float)hours / HoursPerYear).ToString("P0") +
                "/" + ((float)hours / 24f).ToString("F1") + "d";
        }

        private static string FormatEvents(
            int eventCount,
            int shortEventCount,
            int maxRunHours)
        {
            return eventCount +
                "(short<=6h:" + shortEventCount +
                ",max:" + maxRunHours + "h)";
        }
    }
}
