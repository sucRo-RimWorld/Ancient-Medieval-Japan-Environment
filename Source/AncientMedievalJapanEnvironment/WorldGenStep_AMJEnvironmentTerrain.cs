using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace AncientMedievalJapan.Environment
{
    [HarmonyPatch(typeof(WorldGenStep_Terrain), "GenerateFresh")]
    public static class Patch_WorldGenStep_Terrain_GenerateFresh
    {
        public static void Postfix(string seed, PlanetLayer layer)
        {
            EnvironmentTerrainProcessor.Apply(seed, layer);
        }
    }

    public static class EnvironmentTerrainProcessor
    {
        // Restrict only this root-surface generation pass. Keep Vanilla Defs
        // intact for saves; water and third-party workers remain eligible.
        public static bool IsBiomeCandidateAllowed(string defName)
        {
            switch (defName)
            {
                case "TropicalRainforest":
                case "TemperateForest":
                case "BorealForest":
                case "Tundra":
                case "TropicalSwamp":
                case "TemperateSwamp":
                case "ColdBog":
                case "AridShrubland":
                case "Desert":
                case "ExtremeDesert":
                case "IceSheet":
                case "SeaIce":
                    return false;
                default:
                    return true;
            }
        }

        private sealed class RuggedTile
        {
            public SurfaceTile tile;
            public float score;
        }

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

        public static void Apply(string seed, PlanetLayer layer)
        {
            if (!layer.IsRootSurface)
            {
                return;
            }

            WorldGenDiagnostics.LogVanillaTerrainBaseline(layer);

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

            List<RuggedTile> ruggedLand = new List<RuggedTile>();

            // Pass 1: reshape elevation/coastline and collect a continuous
            // ruggedness score for every resulting land tile.
            for (int i = 0; i < layer.TilesCount; i++)
            {
                PlanetTile planetTile = new PlanetTile(i, layer);
                SurfaceTile tile = layer.Tiles[i] as SurfaceTile;
                if (tile == null)
                {
                    continue;
                }

                Vector3 center = layer.GetTileCenter(planetTile);
                float ruggedness = (float)ruggednessNoise.GetValue(center);

                tile.elevation = AdjustBaseElevation(
                    tile.elevation,
                    coastNoise.GetValue(center));

                if (tile.elevation <= 0f)
                {
                    tile.hilliness = Hilliness.Flat;
                    tile.swampiness = 0f;
                    continue;
                }

                // Elevation contributes to the rank but noise remains the
                // dominant term so mountain belts stay spatially coherent.
                float elevationFactor = Mathf.Clamp01(tile.elevation / 2000f);
                ruggedLand.Add(new RuggedTile
                {
                    tile = tile,
                    score = ruggedness + elevationFactor * 0.55f
                });
            }

            // Pass 2: rank land by ruggedness. This deliberately targets the
            // Alpha 25/20/25/25/5 gameplay distribution rather than relying
            // on Vanilla's planet-scale Hilliness thresholds.
            ruggedLand.Sort(delegate(RuggedTile a, RuggedTile b)
            {
                return a.score.CompareTo(b.score);
            });

            int landCount = ruggedLand.Count;
            int flatEnd = Mathf.RoundToInt(landCount * 0.25f);
            int smallEnd = flatEnd + Mathf.RoundToInt(landCount * 0.20f);
            int largeEnd = smallEnd + Mathf.RoundToInt(landCount * 0.25f);
            int mountainousEnd = largeEnd + Mathf.RoundToInt(landCount * 0.25f);

            for (int i = 0; i < landCount; i++)
            {
                RuggedTile sample = ruggedLand[i];
                Hilliness hilliness;

                if (i < flatEnd)
                {
                    hilliness = Hilliness.Flat;
                }
                else if (i < smallEnd)
                {
                    hilliness = Hilliness.SmallHills;
                }
                else if (i < largeEnd)
                {
                    hilliness = Hilliness.LargeHills;
                }
                else if (i < mountainousEnd)
                {
                    hilliness = Hilliness.Mountainous;
                }
                else
                {
                    hilliness = Hilliness.Impassable;
                }

                sample.tile.hilliness = hilliness;
                sample.tile.elevation = ApplyMountainUplift(
                    sample.tile.elevation,
                    hilliness,
                    sample.score);

                if (hilliness == Hilliness.LargeHills ||
                    hilliness == Hilliness.Mountainous ||
                    hilliness == Hilliness.Impassable)
                {
                    sample.tile.swampiness = 0f;
                }
            }

            // Pass 3: temperature/rainfall/biome must use the final adjusted
            // elevation and Hilliness.
            for (int i = 0; i < layer.TilesCount; i++)
            {
                PlanetTile planetTile = new PlanetTile(i, layer);
                SurfaceTile tile = layer.Tiles[i] as SurfaceTile;
                if (tile == null)
                {
                    continue;
                }

                tile.temperature = CalculateAnnualMeanTemperature(
                    layer.LongLatOf(planetTile).y,
                    tile.elevation);

                tile.rainfall = Mathf.Clamp(800f + tile.rainfall * 0.55f, 800f, 3000f);
                tile.PrimaryBiome = SelectBiome(tile, planetTile, layer);
            }

            EnsureAlpineSettlementAccess(layer);
            WorldGenDiagnostics.LogTerrainSummary(layer);
        }

        public const float MaxAlpineImpassableFraction = 0.25f;

        private static void EnsureAlpineSettlementAccess(PlanetLayer layer)
        {
            int alpineCount = 0;
            List<SurfaceTile> impassableAlpine = new List<SurfaceTile>();
            for (int i = 0; i < layer.TilesCount; i++)
            {
                SurfaceTile tile = layer.Tiles[i] as SurfaceTile;
                if (tile == null || tile.PrimaryBiome == null ||
                    tile.PrimaryBiome.defName != "AMJ_AlpineZone") continue;
                alpineCount++;
                if (tile.hilliness == Hilliness.Impassable) impassableAlpine.Add(tile);
            }
            // Preserve the highest peaks; change access, not elevation/climate.
            impassableAlpine.Sort(delegate(SurfaceTile a, SurfaceTile b)
            {
                return a.elevation.CompareTo(b.elevation);
            });
            int limit = Mathf.FloorToInt(alpineCount * MaxAlpineImpassableFraction);
            int converted = Mathf.Max(0, impassableAlpine.Count - limit);
            for (int i = 0; i < converted; i++) impassableAlpine[i].hilliness = Hilliness.Mountainous;
            Log.Message("[AMJ Environment] Alpine access | total=" + alpineCount +
                " impassableBefore=" + impassableAlpine.Count +
                " impassableAfter=" + (impassableAlpine.Count - converted) +
                " convertedToMountainous=" + converted);
        }

        private static float AdjustBaseElevation(
            float vanillaElevation,
            double coastNoiseValue)
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

        private static float ApplyMountainUplift(
            float elevation,
            Hilliness hilliness,
            float ruggednessScore)
        {
            float uplift = 0f;

            switch (hilliness)
            {
                case Hilliness.LargeHills:
                    uplift = 250f;
                    break;
                case Hilliness.Mountainous:
                    uplift = 750f;
                    break;
                case Hilliness.Impassable:
                    uplift = 1400f;
                    break;
            }

            // Within the same class, more rugged tiles receive a little more
            // relief. This creates rare high peaks without lifting all land.
            uplift += Mathf.Max(0f, ruggednessScore - 0.45f) * 450f;

            return Mathf.Clamp(elevation + uplift, 0f, MaxElevation);
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
                    !IsBiomeCandidateAllowed(biome.defName) ||
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
