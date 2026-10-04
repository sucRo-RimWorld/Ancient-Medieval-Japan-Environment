using System;
using HarmonyLib;
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
            new Harmony(
                "sucro.ancientmedievaljapan.environment.quicktests")
                .PatchAll(typeof(QuicktestAssemblyBootstrap).Assembly);

            Log.Message(
                "[AMJ Environment Quicktest] Developer quicktest assembly loaded.");
        }
    }

    [HarmonyPatch(typeof(MapDrawLayer), "GetSubMesh")]
    public static class BadRenderMaterialDiagnostics
    {
        public static int BadMaterialUseCount;

        [HarmonyPrefix]
        public static void Prefix(
            MapDrawLayer __instance,
            UnityEngine.Material material)
        {
            if (string.IsNullOrEmpty(
                System.Environment.GetEnvironmentVariable("RIMWORLD_QUICKSTART")))
            {
                return;
            }

            if (!IsBadRenderMaterial(material))
            {
                return;
            }

            BadMaterialUseCount++;

            string layer =
                __instance == null
                    ? "<null>"
                    : __instance.GetType().FullName;
            string materialName =
                material == null
                    ? "<null>"
                    : material.name;
            string textureName = "<no _MainTex>";
            if (material != null && material.HasProperty("_MainTex"))
            {
                textureName =
                    material.mainTexture == null
                        ? "<null>"
                        : material.mainTexture.name;
            }

            int key =
                0x4A4D4500 ^
                (layer == null ? 0 : layer.GetHashCode());

            Log.ErrorOnce(
                "[AMJ Environment BadRenderMaterial]" +
                " layer=" + layer +
                " material=" + materialName +
                " texture=" + textureName,
                key);
        }

        internal static bool IsBadRenderMaterial(
            UnityEngine.Material material)
        {
            if (material == null)
            {
                return false;
            }

            if (material == BaseContent.BadMat)
            {
                return true;
            }

            if (!material.HasProperty("_MainTex"))
            {
                return false;
            }

            if (material.mainTexture == null)
            {
                return false;
            }

            UnityEngine.Texture texture = material.mainTexture;
            return texture == BaseContent.BadTex ||
                string.Equals(
                    texture.name,
                    "ERRORTEX",
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(
                    texture.name,
                    "BadTex",
                    StringComparison.OrdinalIgnoreCase);
        }
    }

    [HarmonyPatch(typeof(Graphic), "TryGetTextureAtlasReplacementInfo")]
    public static class PreAtlasBadTextureDiagnostics
    {
        public static int BadMaterialUseCount;

        [HarmonyPrefix]
        public static void Prefix(
            UnityEngine.Material mat,
            TextureAtlasGroup group)
        {
            if (string.IsNullOrEmpty(
                System.Environment.GetEnvironmentVariable("RIMWORLD_QUICKSTART")))
            {
                return;
            }

            if (!BadRenderMaterialDiagnostics.IsBadRenderMaterial(mat))
            {
                return;
            }

            BadMaterialUseCount++;

            string materialName =
                mat == null
                    ? "<null>"
                    : mat.name;
            string textureName = "<no _MainTex>";

            if (mat != null && mat.HasProperty("_MainTex"))
            {
                textureName =
                    mat.mainTexture == null
                        ? "<null>"
                        : mat.mainTexture.name;
            }

            int key =
                0x4A4D4600 ^
                (materialName == null ? 0 : materialName.GetHashCode()) ^
                (int)group;

            Log.ErrorOnce(
                "[AMJ Environment PreAtlasBadTexture]" +
                " group=" + group +
                " material=" + materialName +
                " texture=" + textureName,
                key);
        }
    }

    [HarmonyPatch(typeof(Graphic), "Draw")]
    public static class RealtimeBadGraphicDiagnostics
    {
        public static int BadGraphicDrawCount;

        [HarmonyPrefix]
        public static void Prefix(
            Graphic __instance,
            Rot4 rot,
            Thing thing)
        {
            if (string.IsNullOrEmpty(
                System.Environment.GetEnvironmentVariable("RIMWORLD_QUICKSTART")) ||
                __instance == null ||
                thing == null)
            {
                return;
            }

            UnityEngine.Material material = null;
            try
            {
                material = __instance.MatAt(rot, thing);
            }
            catch (Exception ex)
            {
                Log.Warning(
                    "[AMJ Environment RealtimeBadGraphic] MATERIAL-ERROR" +
                    " def=" + (thing.def == null ? "<null>" : thing.def.defName) +
                    " type=" + thing.GetType().FullName +
                    " graphic=" + __instance.GetType().FullName +
                    " path=" + (__instance.path ?? "<null>") +
                    " exception=" + ex.GetType().Name +
                    ": " + ex.Message);
                return;
            }

            if (!BadRenderMaterialDiagnostics.IsBadRenderMaterial(material))
            {
                return;
            }

            BadGraphicDrawCount++;

            string textureName = "<no _MainTex>";
            if (material != null && material.HasProperty("_MainTex"))
            {
                textureName =
                    material.mainTexture == null
                        ? "<null>"
                        : material.mainTexture.name;
            }

            int key =
                0x4A4D4700 ^
                (thing.def == null || thing.def.defName == null
                    ? 0
                    : thing.def.defName.GetHashCode());

            Log.ErrorOnce(
                "[AMJ Environment RealtimeBadGraphic]" +
                " def=" + (thing.def == null ? "<null>" : thing.def.defName) +
                " label=" + thing.LabelNoCount +
                " pos=" + thing.Position +
                " type=" + thing.GetType().FullName +
                " graphic=" + __instance.GetType().FullName +
                " path=" + (__instance.path ?? "<null>") +
                " texture=" + textureName,
                key);
        }
    }

    [HarmonyPatch(typeof(Graphic), "DrawFromDef")]
    public static class RealtimeBadGraphicFromDefDiagnostics
    {
        public static int BadGraphicDrawCount;

        [HarmonyPrefix]
        public static void Prefix(
            Graphic __instance,
            Rot4 rot,
            ThingDef thingDef)
        {
            if (string.IsNullOrEmpty(
                System.Environment.GetEnvironmentVariable("RIMWORLD_QUICKSTART")) ||
                __instance == null)
            {
                return;
            }

            UnityEngine.Material material = null;
            try
            {
                material = __instance.MatAt(rot);
            }
            catch (Exception ex)
            {
                Log.Warning(
                    "[AMJ Environment RealtimeBadGraphicFromDef] MATERIAL-ERROR" +
                    " def=" + (thingDef == null ? "<null>" : thingDef.defName) +
                    " graphic=" + __instance.GetType().FullName +
                    " path=" + (__instance.path ?? "<null>") +
                    " exception=" + ex.GetType().Name +
                    ": " + ex.Message);
                return;
            }

            if (!BadRenderMaterialDiagnostics.IsBadRenderMaterial(material))
            {
                return;
            }

            BadGraphicDrawCount++;

            string textureName = "<no _MainTex>";
            if (material != null && material.HasProperty("_MainTex"))
            {
                textureName =
                    material.mainTexture == null
                        ? "<null>"
                        : material.mainTexture.name;
            }

            int key =
                0x4A4D4800 ^
                (thingDef == null || thingDef.defName == null
                    ? 0
                    : thingDef.defName.GetHashCode());

            Log.ErrorOnce(
                "[AMJ Environment RealtimeBadGraphicFromDef]" +
                " def=" + (thingDef == null ? "<null>" : thingDef.defName) +
                " graphic=" + __instance.GetType().FullName +
                " path=" + (__instance.path ?? "<null>") +
                " texture=" + textureName,
                key);
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

            verification.Assert(
                "map render pipeline emitted no BadTex submesh materials",
                delegate
                {
                    return BadRenderMaterialDiagnostics.BadMaterialUseCount == 0;
                });

            verification.Assert(
                "pre-atlas map graphics emitted no BadTex materials",
                delegate
                {
                    return PreAtlasBadTextureDiagnostics.BadMaterialUseCount == 0;
                });

            verification.Assert(
                "realtime Thing graphics emitted no BadTex materials",
                delegate
                {
                    return RealtimeBadGraphicDiagnostics.BadGraphicDrawCount == 0 &&
                        RealtimeBadGraphicFromDefDiagnostics.BadGraphicDrawCount == 0;
                });

            AddWeatherAssertions(verification, map == null ? null : map.Biome);
            AddSeasonalSceneryAssertions(verification);
            AddWildlifeAssertions(verification, map == null ? null : map.Biome);
            AddLivePlantTextureAssertions(verification, map);
            AddLiveThingTextureAssertions(verification, map);
            AddTerrainScatterTextureAssertions(verification, map);

            if (TargetBiomeDefName == "AMJ_WarmTemperateForest")
            {
                AddTreeTextureAuditAssertions(verification);
            }

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
                "Sudajii graphic resolves a non-BadTex texture",
                delegate
                {
                    return shii != null &&
                        shii.graphicData != null &&
                        HasLoadedNonBadTexture(shii.graphicData.Graphic);
                });

            verification.Assert(
                "Japanese beech leafy graphic resolves a non-BadTex texture",
                delegate
                {
                    return beech != null &&
                        beech.graphicData != null &&
                        HasLoadedNonBadTexture(beech.graphicData.Graphic);
                });

            verification.Assert(
                "Japanese beech has a loaded leafless graphic",
                delegate
                {
                    return beech != null &&
                        beech.plant != null &&
                        beech.plant.leaflessGraphic != null;
                });

            verification.Assert(
                "Japanese beech leafless graphic resolves a non-BadTex texture",
                delegate
                {
                    return beech != null &&
                        beech.plant != null &&
                        HasLoadedNonBadTexture(beech.plant.leaflessGraphic);
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
                "Shirabiso graphic resolves a non-BadTex texture",
                delegate
                {
                    return shirabiso != null &&
                        shirabiso.graphicData != null &&
                        HasLoadedNonBadTexture(shirabiso.graphicData.Graphic);
                });

            verification.Assert(
                "Haimatsu graphic resolves a non-BadTex texture",
                delegate
                {
                    return haimatsu != null &&
                        haimatsu.graphicData != null &&
                        HasLoadedNonBadTexture(haimatsu.graphicData.Graphic);
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

        private static void AddLivePlantTextureAssertions(
            QuickstartVerification verification,
            Map map)
        {
            int scannedPlants = 0;
            int badPlants = 0;
            string failures = "";

            if (map != null)
            {
                foreach (Thing thing in map.listerThings.AllThings)
                {
                    Plant plant = thing as Plant;
                    if (plant == null || plant.Destroyed)
                    {
                        continue;
                    }

                    scannedPlants++;

                    Graphic graphic = null;
                    try
                    {
                        graphic = plant.Graphic;
                        UnityEngine.Material material =
                            graphic == null
                                ? null
                                : graphic.MatAt(plant.Rotation, plant);

                        if (!MaterialHasNonBadTexture(material))
                        {
                            badPlants++;
                            AppendLivePlantTextureFailure(
                                plant,
                                "live",
                                graphic,
                                material,
                                ref failures);
                        }

                        Graphic snowGraphic = plant.SnowOverlayGraphic;
                        if (snowGraphic != null)
                        {
                            UnityEngine.Material snowMaterial =
                                snowGraphic.MatSingleFor(plant);

                            if (!MaterialHasNonBadTexture(snowMaterial))
                            {
                                badPlants++;
                                AppendLivePlantTextureFailure(
                                    plant,
                                    "snowOverlay",
                                    snowGraphic,
                                    snowMaterial,
                                    ref failures);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        badPlants++;
                        string failure =
                            plant.def.defName +
                            ":exception=" +
                            ex.GetType().Name +
                            ":" +
                            ex.Message;

                        if (!string.IsNullOrEmpty(failures))
                        {
                            failures += "; ";
                        }
                        failures += failure;

                        Log.Warning(
                            "[AMJ Environment LivePlantTextureAudit] EXCEPTION" +
                            " def=" + plant.def.defName +
                            " label=" + plant.LabelNoCount +
                            " pos=" + plant.Position +
                            " graphic=" +
                                (graphic == null
                                    ? "<null>"
                                    : graphic.GetType().FullName) +
                            " path=" +
                                (graphic == null || graphic.path == null
                                    ? "<null>"
                                    : graphic.path) +
                            " exception=" + ex.GetType().Name +
                            ": " + ex.Message);
                    }
                }
            }

            Log.Message(
                "[AMJ Environment LivePlantTextureAudit] scannedPlants=" +
                scannedPlants +
                " badStates=" +
                badPlants +
                " failures={" +
                failures +
                "}");

            verification.Assert(
                "all live map plant graphics resolve non-BadTex textures",
                delegate
                {
                    return map != null &&
                        scannedPlants > 0 &&
                        badPlants == 0;
                });
        }

        private static void AppendLivePlantTextureFailure(
            Plant plant,
            string state,
            Graphic graphic,
            UnityEngine.Material material,
            ref string failures)
        {
            string path =
                graphic == null || graphic.path == null
                    ? "<null>"
                    : graphic.path;
            string texture =
                material == null || material.mainTexture == null
                    ? "<null>"
                    : material.mainTexture.name;

            if (!string.IsNullOrEmpty(failures))
            {
                failures += "; ";
            }

            failures +=
                plant.def.defName +
                ":" +
                state +
                "=" +
                path +
                " texture=" +
                texture;

            Log.Warning(
                "[AMJ Environment LivePlantTextureAudit] BAD" +
                " def=" + plant.def.defName +
                " label=" + plant.LabelNoCount +
                " state=" + state +
                " pos=" + plant.Position +
                " graphic=" +
                    (graphic == null
                        ? "<null>"
                        : graphic.GetType().FullName) +
                " path=" + path +
                " texture=" + texture);
        }

        private static bool MaterialHasNonBadTexture(
            UnityEngine.Material material)
        {
            if (material == null || material.mainTexture == null)
            {
                return false;
            }

            UnityEngine.Texture texture = material.mainTexture;
            return texture != BaseContent.BadTex &&
                !string.Equals(
                    texture.name,
                    "ERRORTEX",
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    texture.name,
                    "BadTex",
                    StringComparison.OrdinalIgnoreCase);
        }

        private static void AddLiveThingTextureAssertions(
            QuickstartVerification verification,
            Map map)
        {
            int scannedThings = 0;
            int badThings = 0;
            string failures = "";

            if (map != null)
            {
                foreach (Thing thing in map.listerThings.AllThings)
                {
                    if (thing == null ||
                        thing.Destroyed ||
                        thing is Plant ||
                        thing.def == null ||
                        thing.def.graphicData == null)
                    {
                        continue;
                    }

                    scannedThings++;
                    Graphic graphic = null;
                    UnityEngine.Material material = null;

                    try
                    {
                        graphic = thing.Graphic;
                        material =
                            graphic == null
                                ? null
                                : graphic.MatAt(thing.Rotation, thing);

                        if (!MaterialHasNonBadTexture(material))
                        {
                            badThings++;

                            string path =
                                graphic == null || graphic.path == null
                                    ? "<null>"
                                    : graphic.path;
                            string texture =
                                material == null || material.mainTexture == null
                                    ? "<null>"
                                    : material.mainTexture.name;

                            if (!string.IsNullOrEmpty(failures))
                            {
                                failures += "; ";
                            }

                            failures +=
                                thing.def.defName +
                                "=" +
                                path +
                                " texture=" +
                                texture;

                            Log.Warning(
                                "[AMJ Environment LiveThingTextureAudit] BAD" +
                                " def=" + thing.def.defName +
                                " label=" + thing.LabelNoCount +
                                " category=" + thing.def.category +
                                " pos=" + thing.Position +
                                " type=" + thing.GetType().FullName +
                                " graphic=" +
                                    (graphic == null
                                        ? "<null>"
                                        : graphic.GetType().FullName) +
                                " path=" + path +
                                " texture=" + texture);
                        }
                    }
                    catch (Exception ex)
                    {
                        badThings++;

                        if (!string.IsNullOrEmpty(failures))
                        {
                            failures += "; ";
                        }

                        failures +=
                            thing.def.defName +
                            ":exception=" +
                            ex.GetType().Name +
                            ":" +
                            ex.Message;

                        Log.Warning(
                            "[AMJ Environment LiveThingTextureAudit] EXCEPTION" +
                            " def=" + thing.def.defName +
                            " label=" + thing.LabelNoCount +
                            " category=" + thing.def.category +
                            " pos=" + thing.Position +
                            " type=" + thing.GetType().FullName +
                            " graphic=" +
                                (graphic == null
                                    ? "<null>"
                                    : graphic.GetType().FullName) +
                            " path=" +
                                (graphic == null || graphic.path == null
                                    ? "<null>"
                                    : graphic.path) +
                            " exception=" + ex.GetType().Name +
                            ": " + ex.Message);
                    }
                }
            }

            Log.Message(
                "[AMJ Environment LiveThingTextureAudit] scannedThings=" +
                scannedThings +
                " badThings=" +
                badThings +
                " failures={" +
                failures +
                "}");

            verification.Assert(
                "all live non-plant Thing graphics resolve non-BadTex textures",
                delegate
                {
                    return map != null &&
                        scannedThings > 0 &&
                        badThings == 0;
                });
        }

        private static void AddTerrainScatterTextureAssertions(
            QuickstartVerification verification,
            Map map)
        {
            int referencedScatterDefs = 0;
            int badScatterDefs = 0;
            string failures = "";

            System.Collections.Generic.HashSet<string> scatterTypes =
                new System.Collections.Generic.HashSet<string>();

            if (map != null)
            {
                foreach (IntVec3 cell in map.AllCells)
                {
                    TerrainDef terrain = map.terrainGrid.TerrainAt(cell);
                    if (terrain != null &&
                        !string.IsNullOrEmpty(terrain.scatterType))
                    {
                        scatterTypes.Add(terrain.scatterType);
                    }
                }
            }

            foreach (ScatterableDef def in DefDatabase<ScatterableDef>.AllDefsListForReading)
            {
                if (def == null ||
                    string.IsNullOrEmpty(def.scatterType) ||
                    !scatterTypes.Contains(def.scatterType))
                {
                    continue;
                }

                referencedScatterDefs++;

                UnityEngine.Material material = def.mat;
                if (MaterialHasNonBadTexture(material))
                {
                    continue;
                }

                badScatterDefs++;

                string texture =
                    material == null || material.mainTexture == null
                        ? "<null>"
                        : material.mainTexture.name;

                if (!string.IsNullOrEmpty(failures))
                {
                    failures += "; ";
                }

                failures +=
                    def.defName +
                    ":type=" +
                    def.scatterType +
                    " path=" +
                    (def.texturePath ?? "<null>") +
                    " texture=" +
                    texture;

                Log.Warning(
                    "[AMJ Environment TerrainScatterTextureAudit] BAD" +
                    " def=" + def.defName +
                    " scatterType=" + def.scatterType +
                    " path=" + (def.texturePath ?? "<null>") +
                    " texture=" + texture);
            }

            string typeSummary = "";
            foreach (string scatterType in scatterTypes)
            {
                if (!string.IsNullOrEmpty(typeSummary))
                {
                    typeSummary += ",";
                }
                typeSummary += scatterType;
            }

            Log.Message(
                "[AMJ Environment TerrainScatterTextureAudit]" +
                " scatterTypes={" + typeSummary + "}" +
                " referencedDefs=" + referencedScatterDefs +
                " badDefs=" + badScatterDefs +
                " failures={" + failures + "}");

            verification.Assert(
                "all terrain scatter graphics resolve non-BadTex textures",
                delegate
                {
                    return map != null &&
                        badScatterDefs == 0;
                });
        }

        private static void AddTreeTextureAuditAssertions(
            QuickstartVerification verification)
        {
            int auditedTrees = 0;
            int failedStates = 0;
            string failures = "";

            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (def == null ||
                    def.plant == null ||
                    !def.plant.IsTree ||
                    def.graphicData == null)
                {
                    continue;
                }

                auditedTrees++;

                AuditTreeGraphicState(
                    def,
                    "base",
                    def.graphicData.texPath,
                    def.graphicData.Graphic,
                    ref failedStates,
                    ref failures);

                AuditOptionalTreeGraphicState(
                    def,
                    "leafless",
                    "leaflessGraphicPath",
                    def.plant.leaflessGraphic,
                    ref failedStates,
                    ref failures);

                AuditOptionalTreeGraphicState(
                    def,
                    "immature",
                    "immatureGraphicPath",
                    def.plant.immatureGraphic,
                    ref failedStates,
                    ref failures);

                if (ModsConfig.BiotechActive)
                {
                    AuditOptionalTreeGraphicState(
                        def,
                        "polluted",
                        "pollutedGraphicPath",
                        def.plant.pollutedGraphic,
                        ref failedStates,
                        ref failures);
                }

                AuditOptionalTreeGraphicState(
                    def,
                    "leaflessImmature",
                    "leaflessImmatureGraphicPath",
                    def.plant.leaflessImmatureGraphic,
                    ref failedStates,
                    ref failures);

                AuditOptionalTreeGraphicState(
                    def,
                    "snowOverlay",
                    "snowOverlayGraphicPath",
                    def.plant.snowOverlayGraphic,
                    ref failedStates,
                    ref failures);

                AuditOptionalTreeGraphicState(
                    def,
                    "leaflessSnowOverlay",
                    "leaflessSnowOverlayGraphicPath",
                    def.plant.leaflessSnowOverlayGraphic,
                    ref failedStates,
                    ref failures);

                AuditOptionalTreeGraphicState(
                    def,
                    "immatureSnowOverlay",
                    "immatureSnowOverlayGraphicPath",
                    def.plant.immatureSnowOverlayGraphic,
                    ref failedStates,
                    ref failures);
            }

            Log.Message(
                "[AMJ Environment TreeTextureAudit] auditedTrees=" + auditedTrees +
                " failedStates=" + failedStates +
                " failures={" + failures + "}");

            verification.Assert(
                "all loaded tree graphic states resolve non-BadTex textures",
                delegate
                {
                    return auditedTrees > 0 && failedStates == 0;
                });
        }

        private static void AuditOptionalTreeGraphicState(
            ThingDef def,
            string stateName,
            string pathFieldName,
            Graphic graphic,
            ref int failedStates,
            ref string failures)
        {
            string path = GetPlantGraphicPath(def.plant, pathFieldName);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            AuditTreeGraphicState(
                def,
                stateName,
                path,
                graphic,
                ref failedStates,
                ref failures);
        }

        private static string GetPlantGraphicPath(
            PlantProperties plant,
            string fieldName)
        {
            if (plant == null)
            {
                return null;
            }

            System.Reflection.FieldInfo field =
                typeof(PlantProperties).GetField(
                    fieldName,
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);

            return field == null ? null : field.GetValue(plant) as string;
        }

        private static void AuditTreeGraphicState(
            ThingDef def,
            string stateName,
            string path,
            Graphic graphic,
            ref int failedStates,
            ref string failures)
        {
            if (HasLoadedNonBadTexture(graphic))
            {
                return;
            }

            failedStates++;
            if (!string.IsNullOrEmpty(failures))
            {
                failures += "; ";
            }

            failures += def.defName + ":" + stateName + "=" +
                (string.IsNullOrEmpty(path) ? "<empty>" : path);

            Log.Warning(
                "[AMJ Environment TreeTextureAudit] BAD" +
                " def=" + def.defName +
                " state=" + stateName +
                " path=" + (string.IsNullOrEmpty(path) ? "<empty>" : path));
        }

        private static bool HasLoadedNonBadTexture(Graphic graphic)
        {
            if (graphic == null)
            {
                return false;
            }

            try
            {
                Graphic_Collection collection = graphic as Graphic_Collection;
                if (collection != null)
                {
                    System.Reflection.FieldInfo subGraphicsField =
                        typeof(Graphic_Collection).GetField(
                            "subGraphics",
                            System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.NonPublic);

                    Graphic[] subGraphics =
                        subGraphicsField == null
                            ? null
                            : subGraphicsField.GetValue(collection) as Graphic[];

                    if (subGraphics == null || subGraphics.Length == 0)
                    {
                        return false;
                    }

                    for (int i = 0; i < subGraphics.Length; i++)
                    {
                        if (!HasLoadedNonBadTexture(subGraphics[i]))
                        {
                            return false;
                        }
                    }

                    return true;
                }

                UnityEngine.Material material = graphic.MatSingle;
                if (material == null || material.mainTexture == null)
                {
                    return false;
                }

                UnityEngine.Texture texture = material.mainTexture;
                return texture != BaseContent.BadTex &&
                    !string.Equals(
                        texture.name,
                        "ERRORTEX",
                        StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(
                        texture.name,
                        "BadTex",
                        StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Log.Warning(
                    "[AMJ Environment Quicktest] Graphic texture validation failed: " +
                    ex.GetType().Name + ": " + ex.Message);
                return false;
            }
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

    public abstract class WaterHandoffQuickstartBase : AbstractQuickstart
    {
        protected abstract TileMutatorDef TargetMutator { get; }

        protected abstract string TargetTerrainKind { get; }

        public override TaggedString description
        {
            get
            {
                return "Generates a deterministic 250x250 map on an AMJ Environment " +
                    TargetTerrainKind + " world tile and verifies Vanilla world-to-map handoff.";
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
            PlanetTile tile = FindWaterHandoffTile(TargetMutator);
            if (!tile.Valid)
            {
                throw new InvalidOperationException(
                    "No usable world tile found for " +
                    TargetTerrainKind + " handoff test.");
            }

            Find.GameInitData.startingTile = tile;

            Tile worldTile = Find.WorldGrid[tile];
            Log.Message(
                "[AMJ Environment WaterHandoff] selected" +
                " | kind=" + TargetTerrainKind +
                " tile=" + tile +
                " biome=" +
                    (worldTile.PrimaryBiome == null
                        ? "null"
                        : worldTile.PrimaryBiome.defName) +
                " hilliness=" + worldTile.hilliness +
                " mutators=" + MutatorSummary(worldTile));
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification verification = new QuickstartVerification();
            Map map = Find.CurrentMap;

            verification.Assert(
                "current map exists",
                delegate { return map != null; });

            verification.Assert(
                TargetTerrainKind + " mutator survives world-to-map handoff",
                delegate
                {
                    return map != null &&
                        map.TileInfo != null &&
                        map.TileInfo.Mutators != null &&
                        map.TileInfo.Mutators.Contains(TargetMutator);
                });

            int matchingCells = CountTargetWaterCells(map);

            verification.Assert(
                TargetTerrainKind + " terrain generated on local map",
                delegate { return matchingCells > 0; });

            Log.Message(
                "[AMJ Environment WaterHandoff] verified" +
                " | kind=" + TargetTerrainKind +
                " matchingCells=" + matchingCells +
                " mapCells=" +
                    (map == null ? 0 : map.cellIndices.NumGridCells));

            return verification;
        }

        private int CountTargetWaterCells(Map map)
        {
            if (map == null)
            {
                return 0;
            }

            int count = 0;
            foreach (IntVec3 cell in map.AllCells)
            {
                TerrainDef terrain = cell.GetTerrain(map);
                if (terrain == null)
                {
                    continue;
                }

                if (TargetTerrainKind == "river")
                {
                    if (terrain.IsRiver)
                    {
                        count++;
                    }
                }
                else if (TargetTerrainKind == "coast")
                {
                    if (terrain.IsOcean)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static PlanetTile FindWaterHandoffTile(TileMutatorDef mutator)
        {
            PlanetTile fallback = PlanetTile.Invalid;

            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                Tile candidate = Find.WorldGrid[i];
                if (candidate == null ||
                    candidate.WaterCovered ||
                    candidate.Mutators == null ||
                    !candidate.Mutators.Contains(mutator) ||
                    Find.WorldObjects.AnyWorldObjectAt(candidate.tile))
                {
                    continue;
                }

                if (TileFinder.IsValidTileForNewSettlement(candidate.tile))
                {
                    return candidate.tile;
                }

                if (!fallback.Valid &&
                    candidate.hilliness != Hilliness.Impassable)
                {
                    fallback = candidate.tile;
                }
            }

            return fallback;
        }

        private static string MutatorSummary(Tile tile)
        {
            if (tile == null || tile.Mutators == null)
            {
                return "";
            }

            string summary = "";
            for (int i = 0; i < tile.Mutators.Count; i++)
            {
                if (i > 0)
                {
                    summary += ",";
                }
                summary += tile.Mutators[i].defName;
            }
            return summary;
        }
    }

    public sealed class AMJRiverMapHandoffQuickstart : WaterHandoffQuickstartBase
    {
        protected override TileMutatorDef TargetMutator
        {
            get { return TileMutatorDefOf.River; }
        }

        protected override string TargetTerrainKind
        {
            get { return "river"; }
        }
    }

    public sealed class AMJCoastMapHandoffQuickstart : WaterHandoffQuickstartBase
    {
        protected override TileMutatorDef TargetMutator
        {
            get { return TileMutatorDefOf.Coast; }
        }

        protected override string TargetTerrainKind
        {
            get { return "coast"; }
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
