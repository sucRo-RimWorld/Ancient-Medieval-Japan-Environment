using System;
using RimWorks.Quickstarts;
using RimWorks.Quickstarts.Verification;
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

        protected virtual string TargetPlantDefName
        {
            get { return null; }
        }

        protected virtual string[] SecondaryPlantDefNames
        {
            get { return new string[0]; }
        }

        protected virtual float MaxTargetCellFraction
        {
            get { return 1f; }
        }

        protected virtual string[] LimitedTimberPlantDefNames
        {
            get { return new string[0]; }
        }

        protected virtual float MaxLimitedTimberCellFraction
        {
            get { return 1f; }
        }

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
            bool forcedBiome;
            string originalBiome;
            float targetBiomeScore;
            PlanetTile tile = FindTargetTile(
                TargetBiomeDefName,
                out forcedNonSettlementTile,
                out forcedBiome,
                out originalBiome,
                out targetBiomeScore);
            if (!tile.Valid)
            {
                throw new InvalidOperationException(
                    "No usable world tile found for biome test " + TargetBiomeDefName + ".");
            }

            Find.GameInitData.startingTile = tile;

            Tile worldTile = Find.WorldGrid[tile];
            Log.Message(
                "[AMJ Environment Quicktest] Selected tile" +
                " | biome=" + TargetBiomeDefName +
                " tile=" + tile +
                " hilliness=" + worldTile.hilliness +
                " forcedNonSettlementTile=" + forcedNonSettlementTile +
                " forcedBiome=" + forcedBiome +
                " originalBiome=" + originalBiome +
                " targetBiomeScore=" + targetBiomeScore.ToString("F2") +
                " rainfall=" + worldTile.rainfall.ToString("F0") +
                " annualTemp=" + worldTile.temperature.ToString("F1"));
        }

        public override QuickstartVerification Verify()
        {
            if (string.IsNullOrEmpty(TargetPlantDefName))
            {
                return null;
            }

            QuickstartVerification verification = new QuickstartVerification();
            Map map = Find.CurrentMap;

            verification.Assert(
                "current map exists",
                delegate { return map != null; });

            verification.Assert(
                "map biome matches " + TargetBiomeDefName,
                delegate
                {
                    return map != null &&
                        map.Biome != null &&
                        map.Biome.defName == TargetBiomeDefName;
                });

            int targetCount = CountThings(map, TargetPlantDefName);
            int cellCount = map == null ? 0 : map.cellIndices.NumGridCells;
            float targetFraction =
                cellCount <= 0 ? 0f : (float)targetCount / (float)cellCount;

            verification.Assert(
                TargetPlantDefName + " generated",
                delegate { return targetCount > 0; });

            verification.Assert(
                TargetPlantDefName + " stays below safety cell fraction",
                delegate { return targetFraction <= MaxTargetCellFraction; });

            string secondarySummary = "";
            string[] secondary = SecondaryPlantDefNames;
            for (int i = 0; i < secondary.Length; i++)
            {
                string secondaryDefName = secondary[i];
                int secondaryCount = CountThings(map, secondaryDefName);

                if (i > 0)
                {
                    secondarySummary += ", ";
                }
                secondarySummary += secondaryDefName + "=" + secondaryCount;

                verification.Assert(
                    TargetPlantDefName + " exceeds " + secondaryDefName,
                    delegate
                    {
                        return targetCount > CountThings(map, secondaryDefName);
                    });
            }

            int limitedTimberCount = 0;
            string[] limitedTimber = LimitedTimberPlantDefNames;
            for (int i = 0; i < limitedTimber.Length; i++)
            {
                limitedTimberCount += CountThings(map, limitedTimber[i]);
            }

            float limitedTimberFraction =
                cellCount <= 0 ? 0f : (float)limitedTimberCount / (float)cellCount;

            verification.Assert(
                "limited timber stays below safety cell fraction",
                delegate
                {
                    return limitedTimberFraction <= MaxLimitedTimberCellFraction;
                });

            if (CctoIsActive())
            {
                AddCctoCompatibilityAssertions(verification);
            }
            else
            {
                AddStandaloneTemperatureAssertions(verification);
            }

            Log.Message(
                "[AMJ Environment Vegetation] biome=" + TargetBiomeDefName +
                " target=" + TargetPlantDefName +
                " targetCount=" + targetCount +
                " targetCellShare=" + (targetFraction * 100f).ToString("F2") + "%" +
                " secondary={" + secondarySummary + "}" +
                " limitedTimberCount=" + limitedTimberCount +
                " limitedTimberCellShare=" +
                    (limitedTimberFraction * 100f).ToString("F2") + "%" +
                " cctoActive=" + CctoIsActive());

            return verification;
        }

        private static bool CctoIsActive()
        {
            return ModLister.GetActiveModWithIdentifier(
                "sucro.cropcoldtoleranceoverhaul",
                true) != null;
        }

        private static void AddStandaloneTemperatureAssertions(
            QuickstartVerification verification)
        {
            string[] defNames =
            {
                "AMJ_Tree_Shii",
                "AMJ_Tree_Beech",
                "AMJ_Tree_Shirabiso",
                "AMJ_Shrub_Haimatsu"
            };

            for (int i = 0; i < defNames.Length; i++)
            {
                string defName = defNames[i];
                ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);

                verification.Assert(
                    defName + " standalone min growth temperature is 0 C",
                    delegate
                    {
                        return def != null &&
                            System.Math.Abs(def.plant.minGrowthTemperature) < 0.001f;
                    });

                verification.Assert(
                    defName + " standalone has no CCTO extension",
                    delegate
                    {
                        return CountCctoExtensions(def) == 0;
                    });
            }
        }

        private static void AddCctoCompatibilityAssertions(
            QuickstartVerification verification)
        {
            AddCctoPlantAssertions(
                verification,
                "AMJ_Tree_Shii",
                8f,
                false,
                -8f);

            AddCctoPlantAssertions(
                verification,
                "AMJ_Tree_Beech",
                5f,
                true,
                float.NaN);

            AddCctoPlantAssertions(
                verification,
                "AMJ_Tree_Shirabiso",
                0f,
                false,
                -35f);

            AddCctoPlantAssertions(
                verification,
                "AMJ_Shrub_Haimatsu",
                0f,
                false,
                -35f);
        }

        private static void AddCctoPlantAssertions(
            QuickstartVerification verification,
            string defName,
            float expectedMinGrowth,
            bool expectedDormancy,
            float expectedDeath)
        {
            ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);

            verification.Assert(
                defName + " exists for CCTO compatibility",
                delegate { return def != null; });

            verification.Assert(
                defName + " CCTO min growth temperature",
                delegate
                {
                    return def != null &&
                        System.Math.Abs(
                            def.plant.minGrowthTemperature -
                            expectedMinGrowth) < 0.001f;
                });

            DefModExtension extension = FindCctoExtension(def);

            verification.Assert(
                defName + " has exactly one CCTO extension",
                delegate
                {
                    return def != null &&
                        CountCctoExtensions(def) == 1;
                });

            verification.Assert(
                defName + " CCTO dormancy flag",
                delegate
                {
                    if (extension == null)
                    {
                        return false;
                    }

                    System.Reflection.FieldInfo field =
                        extension.GetType().GetField("coldDormancy");
                    return field != null &&
                        (bool)field.GetValue(extension) == expectedDormancy;
                });

            verification.Assert(
                defName + " CCTO cold death threshold",
                delegate
                {
                    if (extension == null)
                    {
                        return false;
                    }

                    System.Reflection.FieldInfo field =
                        extension.GetType().GetField("coldDeathTemperature");
                    if (field == null)
                    {
                        return false;
                    }

                    float actual = (float)field.GetValue(extension);
                    if (float.IsNaN(expectedDeath))
                    {
                        return float.IsNaN(actual);
                    }

                    return System.Math.Abs(actual - expectedDeath) < 0.001f;
                });
        }

        private static DefModExtension FindCctoExtension(ThingDef def)
        {
            if (def == null || def.modExtensions == null)
            {
                return null;
            }

            for (int i = 0; i < def.modExtensions.Count; i++)
            {
                DefModExtension extension = def.modExtensions[i];
                if (extension != null &&
                    extension.GetType().FullName ==
                        "CropColdToleranceOverhaul.ColdToleranceExtension")
                {
                    return extension;
                }
            }

            return null;
        }

        private static int CountCctoExtensions(ThingDef def)
        {
            if (def == null || def.modExtensions == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < def.modExtensions.Count; i++)
            {
                DefModExtension extension = def.modExtensions[i];
                if (extension != null &&
                    extension.GetType().FullName ==
                        "CropColdToleranceOverhaul.ColdToleranceExtension")
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountThings(Map map, string defName)
        {
            if (map == null || string.IsNullOrEmpty(defName))
            {
                return 0;
            }

            ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (def == null)
            {
                return 0;
            }

            return map.listerThings.ThingsOfDef(def).Count;
        }

        private static PlanetTile FindTargetTile(
            string biomeDefName,
            out bool forcedNonSettlementTile,
            out bool forcedBiome,
            out string originalBiome,
            out float targetBiomeScore)
        {
            forcedNonSettlementTile = false;
            forcedBiome = false;
            originalBiome = biomeDefName;
            targetBiomeScore = 0f;

            Hilliness[] preferredHilliness =
            {
                Hilliness.Flat,
                Hilliness.SmallHills,
                Hilliness.LargeHills,
                Hilliness.Mountainous
            };

            PlanetTile exactTile = FindExactBiomeTile(
                biomeDefName,
                preferredHilliness,
                true);
            if (exactTile.Valid)
            {
                return exactTile;
            }

            Hilliness[] fallbackHilliness =
            {
                Hilliness.Flat,
                Hilliness.SmallHills,
                Hilliness.LargeHills,
                Hilliness.Mountainous,
                Hilliness.Impassable
            };

            exactTile = FindExactBiomeTile(
                biomeDefName,
                fallbackHilliness,
                false);
            if (exactTile.Valid)
            {
                forcedNonSettlementTile =
                    !TileFinder.IsValidTileForNewSettlement(exactTile);
                return exactTile;
            }

            BiomeDef targetBiome =
                DefDatabase<BiomeDef>.GetNamedSilentFail(biomeDefName);
            if (targetBiome == null)
            {
                return PlanetTile.Invalid;
            }

            PlanetTile proxyTile = FindWorkerEligibleProxy(
                targetBiome,
                fallbackHilliness,
                out targetBiomeScore);

            bool forcedClimate = false;
            float forcedTemperature = 0f;

            if (!proxyTile.Valid)
            {
                proxyTile = FindClosestClimateProxy(
                    biomeDefName,
                    fallbackHilliness,
                    out forcedTemperature);
                forcedClimate = proxyTile.Valid;
            }

            if (!proxyTile.Valid)
            {
                return PlanetTile.Invalid;
            }

            SurfaceTile proxy = Find.WorldGrid[proxyTile] as SurfaceTile;
            if (proxy == null)
            {
                return PlanetTile.Invalid;
            }

            originalBiome =
                proxy.PrimaryBiome == null
                    ? "null"
                    : proxy.PrimaryBiome.defName;

            if (forcedClimate)
            {
                proxy.temperature = forcedTemperature;
                if (proxy.rainfall < 800f)
                {
                    proxy.rainfall = 800f;
                }
                if (proxy.swampiness >= 0.5f)
                {
                    proxy.swampiness = 0f;
                }

                targetBiomeScore = targetBiome.Worker.GetScore(
                    targetBiome,
                    proxy,
                    proxy.tile);
            }

            proxy.PrimaryBiome = targetBiome;
            forcedBiome = true;
            forcedNonSettlementTile =
                !TileFinder.IsValidTileForNewSettlement(proxyTile);

            Log.Message(
                "[AMJ Environment Quicktest] Proxy fallback" +
                " | biome=" + biomeDefName +
                " forcedClimate=" + forcedClimate +
                " temperature=" + proxy.temperature.ToString("F1") +
                " rainfall=" + proxy.rainfall.ToString("F0") +
                " swampiness=" + proxy.swampiness.ToString("F2"));

            return proxyTile;
        }

        private static PlanetTile FindExactBiomeTile(
            string biomeDefName,
            Hilliness[] preferredHilliness,
            bool requireValidSettlement)
        {
            for (int h = 0; h < preferredHilliness.Length; h++)
            {
                for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
                {
                    SurfaceTile candidate = Find.WorldGrid[i] as SurfaceTile;
                    if (candidate == null)
                    {
                        continue;
                    }
                    if (candidate.PrimaryBiome == null ||
                        candidate.PrimaryBiome.defName != biomeDefName ||
                        candidate.hilliness != preferredHilliness[h])
                    {
                        continue;
                    }

                    PlanetTile tile = candidate.tile;
                    if (requireValidSettlement)
                    {
                        if (TileFinder.IsValidTileForNewSettlement(tile))
                        {
                            return tile;
                        }
                    }
                    else if (!Find.WorldObjects.AnyWorldObjectAt(tile))
                    {
                        return tile;
                    }
                }
            }

            return PlanetTile.Invalid;
        }

        private static PlanetTile FindWorkerEligibleProxy(
            BiomeDef targetBiome,
            Hilliness[] preferredHilliness,
            out float bestScore)
        {
            bestScore = 0f;

            for (int h = 0; h < preferredHilliness.Length; h++)
            {
                PlanetTile bestTile = PlanetTile.Invalid;
                float bestForHilliness = 0f;

                for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
                {
                    SurfaceTile candidate = Find.WorldGrid[i] as SurfaceTile;
                    if (candidate == null)
                    {
                        continue;
                    }
                    if (candidate.WaterCovered ||
                        candidate.hilliness != preferredHilliness[h] ||
                        Find.WorldObjects.AnyWorldObjectAt(candidate.tile))
                    {
                        continue;
                    }

                    float score = targetBiome.Worker.GetScore(
                        targetBiome,
                        candidate,
                        candidate.tile);
                    if (score > bestForHilliness)
                    {
                        bestForHilliness = score;
                        bestTile = candidate.tile;
                    }
                }

                if (bestTile.Valid)
                {
                    bestScore = bestForHilliness;
                    return bestTile;
                }
            }

            return PlanetTile.Invalid;
        }

        private static PlanetTile FindClosestClimateProxy(
            string biomeDefName,
            Hilliness[] preferredHilliness,
            out float forcedTemperature)
        {
            forcedTemperature = TargetRepresentativeTemperature(biomeDefName);

            for (int h = 0; h < preferredHilliness.Length; h++)
            {
                PlanetTile bestTile = PlanetTile.Invalid;
                float bestDistance = float.MaxValue;

                for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
                {
                    SurfaceTile candidate = Find.WorldGrid[i] as SurfaceTile;
                    if (candidate == null ||
                        candidate.WaterCovered ||
                        candidate.hilliness != preferredHilliness[h] ||
                        Find.WorldObjects.AnyWorldObjectAt(candidate.tile))
                    {
                        continue;
                    }

                    float distance =
                        System.Math.Abs(candidate.temperature - forcedTemperature);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestTile = candidate.tile;
                    }
                }

                if (bestTile.Valid)
                {
                    return bestTile;
                }
            }

            return PlanetTile.Invalid;
        }

        private static float TargetRepresentativeTemperature(string biomeDefName)
        {
            if (biomeDefName == "AMJ_WarmTemperateForest")
            {
                return 17.5f;
            }
            if (biomeDefName == "AMJ_CoolTemperateForest")
            {
                return 11.5f;
            }
            if (biomeDefName == "AMJ_SubalpineForest")
            {
                return 4f;
            }
            if (biomeDefName == "AMJ_AlpineZone")
            {
                return -4f;
            }

            return 10f;
        }

    }

    public sealed class AMJWarmTemperateTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "AMJ_WarmTemperateForest"; }
        }

        protected override string TargetPlantDefName
        {
            get { return "AMJ_Tree_Shii"; }
        }

        protected override string[] SecondaryPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_TreeOak",
                    "Plant_TreePoplar",
                    "Plant_TreeMaple",
                    "Plant_TreeBamboo"
                };
            }
        }
    }

    public sealed class AMJCoolTemperateTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "AMJ_CoolTemperateForest"; }
        }

        protected override string TargetPlantDefName
        {
            get { return "AMJ_Tree_Beech"; }
        }

        protected override string[] SecondaryPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_TreeOak",
                    "Plant_TreeMaple",
                    "Plant_TreeBirch",
                    "Plant_TreePine"
                };
            }
        }
    }

    public sealed class AMJSubalpineTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "AMJ_SubalpineForest"; }
        }

        protected override string TargetPlantDefName
        {
            get { return "AMJ_Tree_Shirabiso"; }
        }

        protected override string[] SecondaryPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_TreePine",
                    "Plant_TreeBirch"
                };
            }
        }
    }

    public sealed class AMJAlpineTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "AMJ_AlpineZone"; }
        }

        protected override string TargetPlantDefName
        {
            get { return "AMJ_Shrub_Haimatsu"; }
        }

        protected override string[] SecondaryPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_TreePine",
                    "Plant_TreeBirch"
                };
            }
        }

        protected override float MaxTargetCellFraction
        {
            get { return 0.05f; }
        }

        protected override string[] LimitedTimberPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_TreePine",
                    "Plant_TreeBirch"
                };
            }
        }

        protected override float MaxLimitedTimberCellFraction
        {
            get { return 0.01f; }
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
