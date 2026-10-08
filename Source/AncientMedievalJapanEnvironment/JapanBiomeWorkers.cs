using RimWorld;
using RimWorld.Planet;

namespace AncientMedievalJapan.Environment
{
    public static class JapanBiomeScoring
    {
        public static bool EligibleLand(Tile tile)
        {
            return tile != null &&
                !tile.WaterCovered &&
                tile.rainfall >= 800f;
        }

        public static float BandScore(Tile tile, float centerTemperature)
        {
            // Strong enough to establish the AMJ baseline vegetation bands
            // over ordinary workers, but intentionally not authoritative:
            // specialized biomes from MO or other compatible mods may still
            // win part of the same climate band through normal worker scoring.
            return 38f - System.Math.Abs(tile.temperature - centerTemperature) * 0.25f;
        }
    }

    public class BiomeWorker_AMJWarmTemperateForest : BiomeWorker
    {
        public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
        {
            if (!JapanBiomeScoring.EligibleLand(tile) || tile.temperature < 15f)
            {
                return 0f;
            }

            return JapanBiomeScoring.BandScore(tile, 17.5f);
        }
    }

    public class BiomeWorker_AMJCoolTemperateForest : BiomeWorker
    {
        public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
        {
            if (!JapanBiomeScoring.EligibleLand(tile) ||
                tile.temperature < 8f ||
                tile.temperature >= 15f)
            {
                return 0f;
            }

            return JapanBiomeScoring.BandScore(tile, 11.5f);
        }
    }

    public class BiomeWorker_AMJSubalpineForest : BiomeWorker
    {
        public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
        {
            if (!JapanBiomeScoring.EligibleLand(tile) ||
                tile.temperature < 0f ||
                tile.temperature >= 8f)
            {
                return 0f;
            }

            return JapanBiomeScoring.BandScore(tile, 4f);
        }
    }

    public class BiomeWorker_AMJAlpineZone : BiomeWorker
    {
        public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
        {
            if (!JapanBiomeScoring.EligibleLand(tile) || tile.temperature >= 0f)
            {
                return 0f;
            }

            return JapanBiomeScoring.BandScore(tile, -4f);
        }
    }
}
