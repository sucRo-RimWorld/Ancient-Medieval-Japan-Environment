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
            // AMJ vegetation bands are authoritative for ordinary non-wetland
            // land inside the Environment climate envelope. This must beat
            // broad Vanilla workers and MO's common Dark Forest score (40)
            // without globally disabling third-party BiomeDefs.
            return 50f - System.Math.Abs(tile.temperature - centerTemperature) * 0.25f;
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
