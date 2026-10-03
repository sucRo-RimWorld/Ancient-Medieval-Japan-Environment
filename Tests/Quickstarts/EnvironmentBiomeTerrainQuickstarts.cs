using System;
using RimWorks.Quickstarts;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AncientMedievalJapan.Environment.Quicktests
{
    [StaticConstructorOnStartup]
    public static class QuicktestAssemblyBootstrap
    {
        static QuicktestAssemblyBootstrap()
        {
            Log.Message("[AMJ Environment Quicktest] Developer quicktest assembly loaded.");
        }
    }

    public abstract class BiomeTerrainQuickstartBase : AbstractQuickstart
    {
        protected abstract string TargetBiomeDefName { get; }

        public override TaggedString description
        {
            get
            {
                return "Generates a deterministic 250x250 map in " + TargetBiomeDefName +
                    " for AMJ Environment terrain-distribution validation.";
            }
        }

        public override int mapSize
        {
            get { return 250; }
        }

        public override float planetCoverage
        {
            get { return 0.05f; }
        }

        public override string seed
        {
            get { return "AMJ-Environment-Terrain-Alpha"; }
        }

        public override void PostApplyConfiguration()
        {
            bool forcedNonSettlementTile;
            PlanetTile tile = FindTargetTile(TargetBiomeDefName, out forcedNonSettlementTile);
            if (!tile.Valid)
            {
                throw new InvalidOperationException(
                    "No world tile found for biome " + TargetBiomeDefName + ".");
            }

            Find.GameInitData.startingTile = tile;

            Tile worldTile = Find.WorldGrid[tile];
            Log.Message(
                "[AMJ Environment Quicktest] Selected tile" +
                " | biome=" + TargetBiomeDefName +
                " tile=" + tile +
                " hilliness=" + worldTile.hilliness +
                " forcedNonSettlementTile=" + forcedNonSettlementTile +
                " rainfall=" + worldTile.rainfall.ToString("F0") +
                " annualTemp=" + worldTile.temperature.ToString("F1"));
        }

        private static PlanetTile FindTargetTile(
            string biomeDefName,
            out bool forcedNonSettlementTile)
        {
            forcedNonSettlementTile = false;

            Hilliness[] preferredHilliness =
            {
                Hilliness.Flat,
                Hilliness.SmallHills,
                Hilliness.LargeHills,
                Hilliness.Mountainous
            };

            for (int h = 0; h < preferredHilliness.Length; h++)
            {
                for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
                {
                    SurfaceTile candidate = Find.WorldGrid[i];
                    if (candidate.PrimaryBiome == null ||
                        candidate.PrimaryBiome.defName != biomeDefName ||
                        candidate.hilliness != preferredHilliness[h])
                    {
                        continue;
                    }

                    PlanetTile tile = candidate.tile;
                    if (TileFinder.IsValidTileForNewSettlement(tile))
                    {
                        return tile;
                    }
                }
            }

            Hilliness[] fallbackHilliness =
            {
                Hilliness.Flat,
                Hilliness.SmallHills,
                Hilliness.LargeHills,
                Hilliness.Mountainous,
                Hilliness.Impassable
            };

            for (int h = 0; h < fallbackHilliness.Length; h++)
            {
                for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
                {
                    SurfaceTile candidate = Find.WorldGrid[i];
                    if (candidate.PrimaryBiome == null ||
                        candidate.PrimaryBiome.defName != biomeDefName ||
                        candidate.hilliness != fallbackHilliness[h])
                    {
                        continue;
                    }

                    PlanetTile tile = candidate.tile;
                    if (!Find.WorldObjects.AnyWorldObjectAt(tile))
                    {
                        forcedNonSettlementTile = true;
                        return tile;
                    }
                }
            }

            return PlanetTile.Invalid;
        }
    }

    public sealed class AMJWarmTemperateTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "AMJ_WarmTemperateForest"; }
        }
    }

    public sealed class AMJCoolTemperateTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "AMJ_CoolTemperateForest"; }
        }
    }

    public sealed class AMJSubalpineTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "AMJ_SubalpineForest"; }
        }
    }

    public sealed class AMJAlpineTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "AMJ_AlpineZone"; }
        }
    }

    public sealed class AMJDarkForestTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "DankPyon_DarkForest"; }
        }
    }
}
