using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AncientMedievalJapan.Environment
{
    public static class WorldGenDiagnostics
    {
        private static int vanillaLand;
        private static int vanillaCoastalLand;
        private static float vanillaCoastalShare;
        public static void LogVanillaTerrainBaseline(PlanetLayer layer)
        {
            if (!layer.IsRootSurface)
            {
                return;
            }

            int land;
            int coastalLand;
            CountLandAndCoast(layer, out land, out coastalLand);

            vanillaLand = land;
            vanillaCoastalLand = coastalLand;
            vanillaCoastalShare = land > 0 ? (float)coastalLand / land : 0f;

            Log.Message(
                "[AMJ Environment] Vanilla terrain baseline" +
                " | land=" + land +
                " coastalLand=" + coastalLand +
                " coastalShare=" + Percent(coastalLand, land));
        }

        public static void LogTerrainSummary(PlanetLayer layer)
        {
            if (!layer.IsRootSurface)
            {
                return;
            }

            int land = 0;
            int coastalLand = 0;
            int flat = 0;
            int small = 0;
            int large = 0;
            int mountainous = 0;
            int impassable = 0;
            int elevation1500 = 0;
            int elevation2500 = 0;
            int elevation3000 = 0;
            int warmTemperate = 0;
            int coolTemperate = 0;
            int subalpine = 0;
            int alpine = 0;
            int wetlandCandidates = 0;

            float minTemperature = float.MaxValue;
            float maxTemperature = float.MinValue;
            float sumTemperature = 0f;
            float minLandElevation = float.MaxValue;
            float maxLandElevation = float.MinValue;
            float minOceanElevation = float.MaxValue;
            float minRainfall = float.MaxValue;
            float maxRainfall = float.MinValue;
            float sumRainfall = 0f;

            Dictionary<string, int> biomeCounts = new Dictionary<string, int>();
            List<PlanetTile> neighbors = new List<PlanetTile>();

            for (int i = 0; i < layer.TilesCount; i++)
            {
                Tile tile = layer.Tiles[i];

                if (tile.WaterCovered)
                {
                    if (tile.elevation < minOceanElevation)
                    {
                        minOceanElevation = tile.elevation;
                    }

                    continue;
                }

                land++;

                if (tile.temperature >= 15f)
                {
                    warmTemperate++;
                }
                else if (tile.temperature >= 8f)
                {
                    coolTemperate++;
                }
                else if (tile.temperature >= 0f)
                {
                    subalpine++;
                }
                else
                {
                    alpine++;
                }

                if (tile.swampiness >= 0.5f)
                {
                    wetlandCandidates++;
                }

                if (tile.rainfall < minRainfall) minRainfall = tile.rainfall;
                if (tile.rainfall > maxRainfall) maxRainfall = tile.rainfall;
                sumRainfall += tile.rainfall;

                string biomeName = tile.PrimaryBiome == null ? "null" : tile.PrimaryBiome.defName;
                int biomeCount;
                biomeCounts.TryGetValue(biomeName, out biomeCount);
                biomeCounts[biomeName] = biomeCount + 1;

                if (tile.temperature < minTemperature) minTemperature = tile.temperature;
                if (tile.temperature > maxTemperature) maxTemperature = tile.temperature;
                sumTemperature += tile.temperature;

                if (tile.elevation < minLandElevation) minLandElevation = tile.elevation;
                if (tile.elevation > maxLandElevation) maxLandElevation = tile.elevation;

                if (tile.elevation >= 1500f) elevation1500++;
                if (tile.elevation >= 2500f) elevation2500++;
                if (tile.elevation >= 3000f) elevation3000++;

                switch (tile.hilliness)
                {
                    case Hilliness.Flat: flat++; break;
                    case Hilliness.SmallHills: small++; break;
                    case Hilliness.LargeHills: large++; break;
                    case Hilliness.Mountainous: mountainous++; break;
                    case Hilliness.Impassable: impassable++; break;
                }

                neighbors.Clear();
                PlanetTile planetTile = new PlanetTile(i, layer);
                Find.WorldGrid.GetTileNeighbors(planetTile, neighbors);

                for (int n = 0; n < neighbors.Count; n++)
                {
                    Tile neighbor = layer.Tiles[neighbors[n].tileId];
                    if (neighbor.WaterCovered)
                    {
                        coastalLand++;
                        break;
                    }
                }
            }

            float averageTemperature = land > 0
                ? sumTemperature / land
                : 0f;
            float averageRainfall = land > 0
                ? sumRainfall / land
                : 0f;
            float coastalShare = land > 0 ? (float)coastalLand / land : 0f;
            float coastVsVanilla = vanillaCoastalShare > 0f
                ? coastalShare / vanillaCoastalShare
                : 0f;
            float landDelta = vanillaLand > 0
                ? ((float)land - vanillaLand) / vanillaLand
                : 0f;

            List<KeyValuePair<string, int>> biomeEntries =
                new List<KeyValuePair<string, int>>(biomeCounts);
            biomeEntries.Sort(delegate(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
            {
                return string.CompareOrdinal(a.Key, b.Key);
            });

            string biomeBreakdown = "";
            for (int i = 0; i < biomeEntries.Count; i++)
            {
                if (biomeBreakdown.Length > 0)
                {
                    biomeBreakdown += ", ";
                }

                biomeBreakdown += biomeEntries[i].Key + "=" +
                    biomeEntries[i].Value + " (" +
                    Percent(biomeEntries[i].Value, land) + ")";
            }

            Log.Message(
                "[AMJ Environment] Terrain summary" +
                " | tiles=" + layer.TilesCount +
                " land=" + land +
                " coastalLand=" + coastalLand +
                " coastalShare=" + Percent(coastalLand, land) +
                " coastVsVanilla=" + coastVsVanilla.ToString("F2") + "x" +
                " landDelta=" + landDelta.ToString("P1") +
                " | landAnnualMean=" + minTemperature.ToString("F1") +
                ".." + maxTemperature.ToString("F1") +
                " avg=" + averageTemperature.ToString("F1") +
                " | landElevation=" + minLandElevation.ToString("F0") +
                ".." + maxLandElevation.ToString("F0") + "m" +
                " oceanFloorMin=" + minOceanElevation.ToString("F0") + "m" +
                " highland>=1500m=" + elevation1500 + " (" + Percent(elevation1500, land) + ")" +
                " >=2500m=" + elevation2500 + " (" + PercentFine(elevation2500, land) + ")" +
                " >=3000m=" + elevation3000 + " (" + PercentFine(elevation3000, land) + ")" +
                " | hilliness Flat=" + Percent(flat, land) +
                " Small=" + Percent(small, land) +
                " Large=" + Percent(large, land) +
                " Mountainous=" + Percent(mountainous, land) +
                " Impassable=" + Percent(impassable, land));

            Log.Message(
                "[AMJ Environment] Climate/biome summary" +
                " | landRainfall=" + minRainfall.ToString("F0") +
                ".." + maxRainfall.ToString("F0") +
                " avg=" + averageRainfall.ToString("F0") +
                " | " + biomeBreakdown);

            Log.Message(
                "[AMJ Environment] Japan vegetation-band preview" +
                " | WarmTemperate>=15C=" + warmTemperate +
                " (" + Percent(warmTemperate, land) + ")" +
                " CoolTemperate=8..15C=" + coolTemperate +
                " (" + Percent(coolTemperate, land) + ")" +
                " Subalpine=0..8C=" + subalpine +
                " (" + Percent(subalpine, land) + ")" +
                " Alpine<0C=" + alpine +
                " (" + Percent(alpine, land) + ")" +
                " | swampiness>=0.5=" + wetlandCandidates +
                " (" + Percent(wetlandCandidates, land) + ")");
        }

        private static void CountLandAndCoast(
            PlanetLayer layer,
            out int land,
            out int coastalLand)
        {
            land = 0;
            coastalLand = 0;
            List<PlanetTile> neighbors = new List<PlanetTile>();

            for (int i = 0; i < layer.TilesCount; i++)
            {
                Tile tile = layer.Tiles[i];
                if (tile.WaterCovered)
                {
                    continue;
                }

                land++;
                neighbors.Clear();

                PlanetTile planetTile = new PlanetTile(i, layer);
                Find.WorldGrid.GetTileNeighbors(planetTile, neighbors);

                for (int n = 0; n < neighbors.Count; n++)
                {
                    Tile neighbor = layer.Tiles[neighbors[n].tileId];
                    if (neighbor.WaterCovered)
                    {
                        coastalLand++;
                        break;
                    }
                }
            }
        }

        private static string Percent(int value, int total)
        {
            if (total <= 0)
            {
                return "0.0%";
            }

            return ((float)value / total).ToString("P1");
        }

        private static string PercentFine(int value, int total)
        {
            if (total <= 0)
            {
                return "0.00%";
            }

            return ((float)value / total).ToString("P2");
        }
    }

    [HarmonyPatch(typeof(WorldGenStep_Rivers), "GenerateFresh")]
    public static class Patch_Rivers_GenerateFresh_Diagnostics
    {
        public static void Postfix(PlanetLayer layer)
        {
            if (!layer.IsRootSurface)
            {
                return;
            }

            Dictionary<string, int> counts = new Dictionary<string, int>();
            int riverEdges = 0;
            int riverTiles = 0;
            int riverLandTiles = 0;
            int landTiles = 0;

            for (int i = 0; i < layer.TilesCount; i++)
            {
                SurfaceTile tile = layer.Tiles[i] as SurfaceTile;
                if (tile == null)
                {
                    continue;
                }

                if (!tile.WaterCovered)
                {
                    landTiles++;
                }

                if (tile.potentialRivers == null || tile.potentialRivers.Count == 0)
                {
                    continue;
                }

                riverTiles++;
                if (!tile.WaterCovered)
                {
                    riverLandTiles++;
                }

                for (int r = 0; r < tile.potentialRivers.Count; r++)
                {
                    SurfaceTile.RiverLink link = tile.potentialRivers[r];
                    if (i >= link.neighbor.tileId)
                    {
                        continue;
                    }

                    riverEdges++;
                    string defName = link.river == null ? "null" : link.river.defName;

                    int current;
                    counts.TryGetValue(defName, out current);
                    counts[defName] = current + 1;
                }
            }

            string breakdown = "";
            foreach (KeyValuePair<string, int> pair in counts)
            {
                if (breakdown.Length > 0)
                {
                    breakdown += ", ";
                }

                breakdown += pair.Key + "=" + pair.Value;
            }

            Log.Message(
                "[AMJ Environment] River summary" +
                " | riverTiles=" + riverTiles +
                " allTileShare=" + ((float)riverTiles / layer.TilesCount).ToString("P1") +
                " riverLandTiles=" + riverLandTiles +
                " landShare=" + (landTiles > 0 ? ((float)riverLandTiles / landTiles).ToString("P1") : "0.0%") +
                " edges=" + riverEdges +
                " | " + breakdown);
        }
    }
}
