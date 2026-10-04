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

            AddWeatherAssertions(verification, map == null ? null : map.Biome);
            AddSeasonalSceneryAssertions(verification);
            AddWildlifeAssertions(verification, map == null ? null : map.Biome);

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

        private static void AddWildlifeAssertions(
            QuickstartVerification verification,
            BiomeDef biome)
        {
            string[] forbidden =
            {
                "Raccoon",
                "Elk",
                "Ibex",
                "Fox_Arctic",
                "Lynx"
            };

            for (int i = 0; i < forbidden.Length; i++)
            {
                string animalDefName = forbidden[i];
                verification.Assert(
                    (biome == null ? "null" : biome.defName) +
                        " excludes wildlife placeholder " + animalDefName,
                    delegate
                    {
                        return GetAnimalCommonality(
                            biome,
                            animalDefName) <= 0f;
                    });
            }

            string[] required =
                ExpectedWildlifeForBiome(
                    biome == null ? null : biome.defName);

            for (int i = 0; i < required.Length; i++)
            {
                string animalDefName = required[i];
                verification.Assert(
                    (biome == null ? "null" : biome.defName) +
                        " keeps wildlife proxy " + animalDefName,
                    delegate
                    {
                        return GetAnimalCommonality(
                            biome,
                            animalDefName) > 0f;
                    });
            }
        }

        private static string[] ExpectedWildlifeForBiome(
            string biomeDefName)
        {
            if (biomeDefName == "AMJ_WarmTemperateForest" ||
                biomeDefName == "AMJ_CoolTemperateForest")
            {
                return new string[]
                {
                    "Hare",
                    "Squirrel",
                    "Rat",
                    "Deer",
                    "WildBoar",
                    "Fox_Red",
                    "Wolf_Timber",
                    "Bear_Grizzly"
                };
            }

            if (biomeDefName == "AMJ_SubalpineForest")
            {
                return new string[]
                {
                    "Hare",
                    "Snowhare",
                    "Deer",
                    "WildBoar",
                    "Fox_Red",
                    "Wolf_Timber",
                    "Bear_Grizzly"
                };
            }

            if (biomeDefName == "AMJ_AlpineZone")
            {
                return new string[]
                {
                    "Hare",
                    "Snowhare",
                    "Deer",
                    "Fox_Red",
                    "Wolf_Timber"
                };
            }

            return new string[0];
        }

        private static float GetAnimalCommonality(
            BiomeDef biome,
            string animalDefName)
        {
            if (biome == null)
            {
                return -1f;
            }

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    animalDefName);

            if (pawnKind == null)
            {
                return -1f;
            }

            return biome.CommonalityOfAnimal(pawnKind);
        }

        private static void AddSeasonalSceneryAssertions(
            QuickstartVerification verification)
        {
            ThingDef beech =
                DefDatabase<ThingDef>.GetNamedSilentFail("AMJ_Tree_Beech");
            ThingDef shii =
                DefDatabase<ThingDef>.GetNamedSilentFail("AMJ_Tree_Shii");
            ThingDef shirabiso =
                DefDatabase<ThingDef>.GetNamedSilentFail("AMJ_Tree_Shirabiso");
            ThingDef haimatsu =
                DefDatabase<ThingDef>.GetNamedSilentFail("AMJ_Shrub_Haimatsu");

            verification.Assert(
                "Japanese beech has a loaded leafless graphic",
                delegate
                {
                    return beech != null &&
                        beech.plant != null &&
                        beech.plant.leaflessGraphic != null;
                });

            verification.Assert(
                "Japanese beech keeps Vanilla fall shader behavior",
                delegate
                {
                    return beech != null &&
                        HasShaderParameter(
                            beech.graphicData,
                            "_FallBehaviorEnabled");
                });

            verification.Assert(
                "warm/subalpine/alpine structural evergreens stay non-leafless",
                delegate
                {
                    return shii != null &&
                        shirabiso != null &&
                        haimatsu != null &&
                        shii.plant.leaflessGraphic == null &&
                        shirabiso.plant.leaflessGraphic == null &&
                        haimatsu.plant.leaflessGraphic == null;
                });

            WeatherDef gentle =
                DefDatabase<WeatherDef>.GetNamedSilentFail("SnowGentle");
            WeatherDef hard =
                DefDatabase<WeatherDef>.GetNamedSilentFail("SnowHard");

            verification.Assert(
                "Vanilla snow weather remains available for seasonal scenery",
                delegate
                {
                    return gentle != null &&
                        hard != null &&
                        gentle.snowRate > 0f &&
                        hard.snowRate > 0f;
                });
        }

        private static bool HasShaderParameter(
            GraphicData graphicData,
            string parameterName)
        {
            if (graphicData == null ||
                graphicData.shaderParameters == null)
            {
                return false;
            }

            System.Reflection.FieldInfo nameField =
                typeof(ShaderParameter).GetField(
                    "name",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);

            if (nameField == null)
            {
                return false;
            }

            for (int i = 0; i < graphicData.shaderParameters.Count; i++)
            {
                ShaderParameter parameter =
                    graphicData.shaderParameters[i];
                if (parameter != null &&
                    (string)nameField.GetValue(parameter) == parameterName)
                {
                    return true;
                }
            }

            return false;
        }

        private static void AddWeatherAssertions(
            QuickstartVerification verification,
            BiomeDef biome)
        {
            verification.Assert(
                "AMJ biome has exactly eight baseline weather entries",
                delegate
                {
                    return biome != null &&
                        biome.baseWeatherCommonalities != null &&
                        biome.baseWeatherCommonalities.Count == 8;
                });

            string[] weatherDefNames =
            {
                "Clear",
                "Fog",
                "Rain",
                "DryThunderstorm",
                "RainyThunderstorm",
                "FoggyRain",
                "SnowGentle",
                "SnowHard"
            };

            for (int i = 0; i < weatherDefNames.Length; i++)
            {
                string weatherDefName = weatherDefNames[i];
                float expected = ExpectedWeatherCommonality(
                    biome == null ? null : biome.defName,
                    weatherDefName);
                float actual = GetWeatherCommonality(
                    biome,
                    weatherDefName);

                verification.Assert(
                    (biome == null ? "null" : biome.defName) +
                        " weather " + weatherDefName +
                        " commonality=" + expected.ToString("0.##"),
                    delegate
                    {
                        return System.Math.Abs(actual - expected) < 0.001f;
                    });
            }

            float dry = GetWeatherCommonality(biome, "DryThunderstorm");
            float rainy = GetWeatherCommonality(biome, "RainyThunderstorm");

            verification.Assert(
                "rainy thunderstorm is more common than dry thunderstorm",
                delegate { return rainy > dry; });

            Log.Message(
                "[AMJ Environment Weather] biome=" +
                (biome == null ? "null" : biome.defName) +
                " Clear=" + GetWeatherCommonality(biome, "Clear").ToString("0.##") +
                " Fog=" + GetWeatherCommonality(biome, "Fog").ToString("0.##") +
                " Rain=" + GetWeatherCommonality(biome, "Rain").ToString("0.##") +
                " DryThunderstorm=" + dry.ToString("0.##") +
                " RainyThunderstorm=" + rainy.ToString("0.##") +
                " FoggyRain=" + GetWeatherCommonality(biome, "FoggyRain").ToString("0.##") +
                " SnowGentle=" + GetWeatherCommonality(biome, "SnowGentle").ToString("0.##") +
                " SnowHard=" + GetWeatherCommonality(biome, "SnowHard").ToString("0.##"));
        }

        private static float ExpectedWeatherCommonality(
            string biomeDefName,
            string weatherDefName)
        {
            if (weatherDefName == "Clear")
            {
                return 16f;
            }

            if (biomeDefName == "AMJ_WarmTemperateForest")
            {
                if (weatherDefName == "Fog") return 1.5f;
                if (weatherDefName == "Rain") return 3f;
                if (weatherDefName == "DryThunderstorm") return 0.1f;
                if (weatherDefName == "RainyThunderstorm") return 1.5f;
                if (weatherDefName == "FoggyRain") return 1.5f;
                if (weatherDefName == "SnowGentle") return 2f;
                if (weatherDefName == "SnowHard") return 1f;
            }
            else if (biomeDefName == "AMJ_CoolTemperateForest")
            {
                if (weatherDefName == "Fog") return 1.5f;
                if (weatherDefName == "Rain") return 3f;
                if (weatherDefName == "DryThunderstorm") return 0.1f;
                if (weatherDefName == "RainyThunderstorm") return 1.5f;
                if (weatherDefName == "FoggyRain") return 1.5f;
                if (weatherDefName == "SnowGentle") return 4f;
                if (weatherDefName == "SnowHard") return 3f;
            }
            else if (biomeDefName == "AMJ_SubalpineForest")
            {
                if (weatherDefName == "Fog") return 1f;
                if (weatherDefName == "Rain") return 2f;
                if (weatherDefName == "DryThunderstorm") return 0.1f;
                if (weatherDefName == "RainyThunderstorm") return 1f;
                if (weatherDefName == "FoggyRain") return 1f;
                if (weatherDefName == "SnowGentle") return 7f;
                if (weatherDefName == "SnowHard") return 7f;
            }
            else if (biomeDefName == "AMJ_AlpineZone")
            {
                if (weatherDefName == "Fog") return 1f;
                if (weatherDefName == "Rain") return 1f;
                if (weatherDefName == "DryThunderstorm") return 0.05f;
                if (weatherDefName == "RainyThunderstorm") return 0.5f;
                if (weatherDefName == "FoggyRain") return 0.5f;
                if (weatherDefName == "SnowGentle") return 12f;
                if (weatherDefName == "SnowHard") return 12f;
            }

            return -999f;
        }

        private static float GetWeatherCommonality(
            BiomeDef biome,
            string weatherDefName)
        {
            if (biome == null || biome.baseWeatherCommonalities == null)
            {
                return -999f;
            }

            for (int i = 0; i < biome.baseWeatherCommonalities.Count; i++)
            {
                WeatherCommonalityRecord record =
                    biome.baseWeatherCommonalities[i];
                if (record != null &&
                    record.weather != null &&
                    record.weather.defName == weatherDefName)
                {
                    return record.commonality;
                }
            }

            return -999f;
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
