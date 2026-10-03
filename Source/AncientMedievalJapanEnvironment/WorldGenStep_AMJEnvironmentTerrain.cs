using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace AncientMedievalJapan.Environment
{
    public class WorldGenStep_AMJEnvironmentTerrain : WorldGenStep_Terrain
    {
        private const float MinElevation = -300f;
        private const float MaxElevation = 3800f;
        private const float CoastInfluenceElevation = 600f;
        private const float CoastNoiseAmplitude = 260f;

        private static readonly SimpleCurve LowlandTemperatureByLatitude = new SimpleCurve
        {
            new CurvePoint(0f, 20f),
            new CurvePoint(0.2f, 19f),
            new CurvePoint(0.4f, 16f),
            new CurvePoint(0.6f, 13f),
            new CurvePoint(0.8f, 9f),
            new CurvePoint(1f, 5f)
        };

        public override void GenerateFresh(string seed, PlanetLayer layer)
        {
            base.GenerateFresh(seed, layer);

            if (!layer.IsRootSurface)
            {
                return;
            }

            int stableSeed = GenText.StableStringHash(seed);
            ModuleBase coastNoise = new Perlin(
                0.08,
                2.0,
                0.5,
                4,
                Gen.HashCombineInt(stableSeed, 139771),
                QualityMode.High);

            ModuleBase ruggednessNoise = new Perlin(
                0.045,
                2.0,
                0.5,
                5,
                Gen.HashCombineInt(stableSeed, 584903),
                QualityMode.High);

            for (int i = 0; i < layer.TilesCount; i++)
            {
                PlanetTile planetTile = new PlanetTile(i, layer);
                SurfaceTile tile = layer.Tiles[i] as SurfaceTile;
                if (tile == null)
                {
                    continue;
                }

                Vector3 center = layer.GetTileCenter(planetTile);

                tile.elevation = AdjustElevation(tile.elevation, coastNoise.GetValue(center));

                if (tile.elevation <= 0f)
                {
                    tile.hilliness = Hilliness.Flat;
                    tile.swampiness = 0f;
                }
                else
                {
                    tile.hilliness = AdjustHilliness(
                        tile.hilliness,
                        tile.elevation,
                        ruggednessNoise.GetValue(center));

                    if (tile.hilliness == Hilliness.LargeHills ||
                        tile.hilliness == Hilliness.Mountainous ||
                        tile.hilliness == Hilliness.Impassable)
                    {
                        tile.swampiness = 0f;
                    }
                }

                tile.temperature = CalculateAnnualMeanTemperature(
                    layer.LongLatOf(planetTile).y,
                    tile.elevation);

                tile.rainfall = Mathf.Clamp(800f + tile.rainfall * 0.55f, 800f, 3000f);
                tile.PrimaryBiome = SelectBiome(tile, planetTile, layer);
            }
        }

        private static float AdjustElevation(float vanillaElevation, double coastNoiseValue)
        {
            float elevation;

            if (vanillaElevation <= 0f)
            {
                float t = Mathf.InverseLerp(-500f, 0f, vanillaElevation);
                elevation = Mathf.Lerp(MinElevation, 0f, t);
            }
            else
            {
                float t = Mathf.InverseLerp(0f, 5000f, vanillaElevation);
                elevation = Mathf.Lerp(0f, MaxElevation, t);
            }

            float coastWeight = 1f - Mathf.InverseLerp(
                0f,
                CoastInfluenceElevation,
                Mathf.Abs(elevation));

            elevation += (float)coastNoiseValue * CoastNoiseAmplitude * coastWeight;
            return Mathf.Clamp(elevation, MinElevation, MaxElevation);
        }

        private static Hilliness AdjustHilliness(
            Hilliness vanilla,
            float elevation,
            double ruggednessNoiseValue)
        {
            float noise = (float)ruggednessNoiseValue;

            Hilliness result = vanilla;

            if (vanilla == Hilliness.Flat && noise > -0.20f)
            {
                result = noise > 0.35f ? Hilliness.LargeHills : Hilliness.SmallHills;
            }
            else if (vanilla == Hilliness.SmallHills && noise > 0.20f)
            {
                result = Hilliness.LargeHills;
            }
            else if (vanilla == Hilliness.LargeHills && noise > 0.40f)
            {
                result = Hilliness.Mountainous;
            }
            else if (vanilla == Hilliness.Mountainous && noise > 0.82f)
            {
                result = Hilliness.Impassable;
            }

            if (elevation >= 2500f && (int)result < (int)Hilliness.Mountainous)
            {
                result = Hilliness.Mountainous;
            }
            else if (elevation >= 1500f && (int)result < (int)Hilliness.LargeHills)
            {
                result = Hilliness.LargeHills;
            }

            return result;
        }

        private static float CalculateAnnualMeanTemperature(float latitude, float elevation)
        {
            float normalizedLatitude = Mathf.Abs(latitude) / 90f;
            float lowland = LowlandTemperatureByLatitude.Evaluate(normalizedLatitude);
            float elevationCooling = Mathf.Max(0f, elevation) * 0.00625f;

            return Mathf.Clamp(lowland - elevationCooling, -8f, 20f);
        }

        private static BiomeDef SelectBiome(
            Tile tile,
            PlanetTile planetTile,
            PlanetLayer layer)
        {
            List<BiomeDef> biomes = DefDatabase<BiomeDef>.AllDefsListForReading;
            BiomeDef best = null;
            float bestScore = 0f;

            for (int i = 0; i < biomes.Count; i++)
            {
                BiomeDef biome = biomes[i];

                if (!biome.implemented ||
                    !biome.generatesNaturally ||
                    !biome.Worker.CanPlaceOnLayer(biome, layer))
                {
                    continue;
                }

                float score = biome.Worker.GetScore(biome, tile, planetTile);

                if (best == null || score > bestScore)
                {
                    best = biome;
                    bestScore = score;
                }
            }

            return best;
        }
    }
}
