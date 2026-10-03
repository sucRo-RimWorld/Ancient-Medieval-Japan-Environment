using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AncientMedievalJapan.Environment
{
    public static class WorldGenDiagnostics
    {
        public static void LogVanillaTerrainBaseline(PlanetLayer layer)
        {
            if (!layer.IsRootSurface)
            {
                return;
            }

            int land;
            int coastalLand;
            CountLandAndCoast(layer, out land, out coastalLand);

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

            float minTemperature = float.MaxValue;
            float maxTemperature = float.MinValue;
            float sumTemperature = 0f;
            float minLandElevation = float.MaxValue;
            float maxLandElevation = float.MinValue;
            float minOceanElevation = float.MaxValue;

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

            Log.Message(
                "[AMJ Environment] Terrain summary" +
                " | tiles=" + layer.TilesCount +
                " land=" + land +
                " coastalLand=" + coastalLand +
                " coastalShare=" + Percent(coastalLand, land) +
                " | landAnnualMean=" + minTemperature.ToString("F1") +
                ".." + maxTemperature.ToString("F1") +
                " avg=" + averageTemperature.ToString("F1") +
                " | landElevation=" + minLandElevation.ToString("F0") +
                ".." + maxLandElevation.ToString("F0") + "m" +
                " oceanFloorMin=" + minOceanElevation.ToString("F0") + "m" +
                " highland>=1500m=" + Percent(elevation1500, land) +
                " >=2500m=" + Percent(elevation2500, land) +
                " >=3000m=" + Percent(elevation3000, land) +
                " | hilliness Flat=" + Percent(flat, land) +
                " Small=" + Percent(small, land) +
                " Large=" + Percent(large, land) +
                " Mountainous=" + Percent(mountainous, land) +
                " Impassable=" + Percent(impassable, land));
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

            for (int i = 0; i < layer.TilesCount; i++)
            {
                SurfaceTile tile = layer.Tiles[i] as SurfaceTile;
                if (tile == null || tile.potentialRivers == null || tile.potentialRivers.Count == 0)
                {
                    continue;
                }

                riverTiles++;

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
                " (" + ((float)riverTiles / layer.TilesCount).ToString("P1") + ")" +
                " edges=" + riverEdges +
                " | " + breakdown);
        }
    }
}
