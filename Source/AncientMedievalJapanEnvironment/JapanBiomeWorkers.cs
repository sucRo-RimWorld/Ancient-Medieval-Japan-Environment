using RimWorld;
using RimWorld.Planet;

namespace AncientMedievalJapan.Environment
{
    public static class JapanBiomeScoring
    {
        public const float WetlandThreshold = 0.5f;

        public static bool EligibleLand(Tile tile)
        {
            return tile != null &&
                !tile.WaterCovered &&
                tile.rainfall >= 800f &&
                tile.swampiness < WetlandThreshold;
        }

        public static float BandScore(Tile tile, float centerTemperature)
        {
            // Deliberately high enough to beat the broad Vanilla biome workers
            // inside the AMJ climate envelope, but still ordinary BiomeWorker
            // scoring so a compatibility biome with a stronger score can win.
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
