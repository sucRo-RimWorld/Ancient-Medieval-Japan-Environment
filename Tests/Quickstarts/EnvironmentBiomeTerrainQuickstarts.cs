using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorks.Quickstarts;
using RimWorks.Quickstarts.Verification;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

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
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(
                    texture.name,
                    "BadTexture",
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

            // Graphic_Shadow intentionally inherits Graphic.MatAt/MatSingle,
            // which return BaseContent.BadMat. Its DrawWorker does not use
            // that material; it renders with MatBases.SunShadowFade instead.
            // Probing MatAt here would therefore report every pawn shadow as
            // a false BadTex hit.
            if (__instance is Graphic_Shadow)
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

    [HarmonyPatch(typeof(PawnRenderNodeWorker), "GetFinalizedMaterial")]
    public static class PawnRenderBadMaterialDiagnostics
    {
        public static int BadMaterialUseCount;

        [HarmonyPostfix]
        public static void Postfix(
            PawnRenderNode node,
            PawnDrawParms parms,
            UnityEngine.Material __result)
        {
            if (string.IsNullOrEmpty(
                System.Environment.GetEnvironmentVariable("RIMWORLD_QUICKSTART")) ||
                parms.pawn == null ||
                node == null ||
                !BadRenderMaterialDiagnostics.IsBadRenderMaterial(__result))
            {
                return;
            }

            BadMaterialUseCount++;

            Pawn pawn = parms.pawn;
            Graphic primaryGraphic = node.PrimaryGraphic;
            string materialName =
                __result == null
                    ? "<null>"
                    : __result.name;
            string textureName = "<no _MainTex>";

            if (__result != null && __result.HasProperty("_MainTex"))
            {
                textureName =
                    __result.mainTexture == null
                        ? "<null>"
                        : __result.mainTexture.name;
            }

            string nodeContext = "";
            if (node.apparel != null && node.apparel.def != null)
            {
                nodeContext = " apparel=" + node.apparel.def.defName;
            }
            else if (node.hediff != null && node.hediff.def != null)
            {
                nodeContext = " hediff=" + node.hediff.def.defName;
            }
            else if (node.gene != null && node.gene.def != null)
            {
                nodeContext = " gene=" + node.gene.def.defName;
            }

            int key =
                0x4A4D4900 ^
                (pawn.def == null || pawn.def.defName == null
                    ? 0
                    : pawn.def.defName.GetHashCode()) ^
                node.GetType().FullName.GetHashCode();

            Log.ErrorOnce(
                "[AMJ Environment PawnRenderBadMaterial]" +
                " def=" + (pawn.def == null ? "<null>" : pawn.def.defName) +
                " kind=" + (pawn.kindDef == null ? "<null>" : pawn.kindDef.defName) +
                " label=" + pawn.LabelNoCount +
                " pos=" + pawn.Position +
                " node=" + node.GetType().FullName +
                " graphic=" +
                    (primaryGraphic == null
                        ? "<null>"
                        : primaryGraphic.GetType().FullName) +
                " path=" +
                    (primaryGraphic == null || primaryGraphic.path == null
                        ? "<null>"
                        : primaryGraphic.path) +
                " material=" + materialName +
                " texture=" + textureName +
                nodeContext,
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

        protected virtual string[] ExcludedPlantDefNames
        {
            get { return new string[0]; }
        }

        protected virtual bool ValidateAmjBiomeContracts
        {
            get { return true; }
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

            string harvestExpected = System.Environment.GetEnvironmentVariable("RIMWORLD_AMJE_HARVEST_EXPECTED");
            if (!string.IsNullOrEmpty(harvestExpected))
            {
                AddHarvestOutputAssertions(verification, map, harvestExpected);
                return verification;
            }

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
                "realtime Thing graphics emitted no non-shadow BadTex materials",
                delegate
                {
                    return RealtimeBadGraphicDiagnostics.BadGraphicDrawCount == 0 &&
                        RealtimeBadGraphicFromDefDiagnostics.BadGraphicDrawCount == 0;
                });

            verification.Assert(
                "pawn render nodes emitted no BadTex materials",
                delegate
                {
                    return PawnRenderBadMaterialDiagnostics.BadMaterialUseCount == 0;
                });

            if (ValidateAmjBiomeContracts)
            {
                AddWeatherAssertions(verification, map == null ? null : map.Biome);
                AddWildlifeAssertions(verification, map == null ? null : map.Biome);
                AddTreeSowingAssertions(verification, map);
                AddMoForestHerbAssertions(verification, map);
            }

            AddSeasonalSceneryAssertions(verification);
            AddLivePlantTextureAssertions(verification, map);
            AddLiveThingTextureAssertions(verification, map);
            AddTerrainScatterTextureAssertions(verification, map);

            if (ValidateAmjBiomeContracts && CoreIsActive())
            {
                AddCoreAgricultureIntegrationAssertions(verification, map);
            }

            if (TargetBiomeDefName == "AMJ_WarmTemperateForest")
            {
                AddTreeTextureAuditAssertions(verification);
            }

            string[] excludedPlants = ExcludedPlantDefNames;
            for (int i = 0; i < excludedPlants.Length; i++)
            {
                string excludedDefName = excludedPlants[i];
                verification.Assert(
                    excludedDefName + " excluded plant is absent from target biome",
                    delegate { return CountThings(map, excludedDefName) == 0; });
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

            if (ValidateAmjBiomeContracts)
            {
                if (CctoIsActive())
                {
                    AddCctoCompatibilityAssertions(verification);
                }
                else
                {
                    AddStandaloneTemperatureAssertions(verification);
                }
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

        // Natural woody vegetation differs from grow-zone sowability for
        // Alpine Haimatsu (plant.IsTree in RimWorld 1.6, but not sowable).
        private static string[] ExpectedWildTreeLikePlants(string biome)
        {
            if (biome == "AMJ_AlpineZone")
                return new[] { "AMJ_Shrub_Haimatsu" };
            return ExpectedSowableTrees(biome);
        }

        private static string[] ExpectedSowableTrees(string biome)
        {
            switch (biome)
            {
                case "AMJ_WarmTemperateForest":
                    return new[] { "AMJ_Tree_Shii", "Plant_TreeMaple", "Plant_TreeBamboo" };
                case "AMJ_CoolTemperateForest":
                    return new[] { "AMJ_Tree_Beech", "Plant_TreeOak", "Plant_TreeMaple", "Plant_TreeBirch", "Plant_TreePine" };
                case "AMJ_SubalpineForest":
                    return new[] { "AMJ_Tree_Shirabiso", "Plant_TreeBirch" };
                case "AMJ_AlpineZone":
                    return new string[0];
                default:
                    throw new InvalidOperationException("No tree-sowing contract for " + biome);
            }
        }

        private static HashSet<string> AvailableGrowingZoneTrees(Zone_Growing zone, Map map)
        {
            HashSet<string> result = new HashSet<string>();
            // Use both native stages of Command_SetPlantToGrow.ProcessInput.
            foreach (ThingDef plant in PlantUtility.ValidPlantTypesForGrowers(
                new List<IPlantToGrowSettable> { zone }))
            {
                if (plant.plant.IsTree && Command_SetPlantToGrow.IsPlantAvailable(plant, map))
                {
                    result.Add(plant.defName);
                }
            }
            return result;
        }

        private static void AddTreeSowingAssertions(QuickstartVerification verification, Map map)
        {
            verification.Assert("tree-sowing regression setup and cleanup succeed", delegate
            {
                if (map == null || map.Biome == null)
                    throw new InvalidOperationException("Tree sowing requires a live map.");
                ResearchProjectDef research = DefDatabase<ResearchProjectDef>.GetNamed("TreeSowing");
                if (research.Cost <= 0f)
                    throw new InvalidOperationException("TreeSowing must have a positive cost.");
                Dictionary<ResearchProjectDef, float> progress =
                    (Dictionary<ResearchProjectDef, float>)AccessTools.Field(
                        typeof(ResearchManager), "progress").GetValue(Find.ResearchManager);
                Dictionary<ResearchProjectDef, float> saved =
                    new Dictionary<ResearchProjectDef, float>(progress);
                try
                {
                    // Unregistered test zone: no zone-grid, home-area or selection changes.
                    Zone_Growing zone = new Zone_Growing();
                    zone.zoneManager = map.zoneManager;
                    foreach (IntVec3 cell in map.AllCells)
                    {
                        if (!cell.IsPolluted(map))
                        {
                            zone.cells.Add(cell);
                            break;
                        }
                    }
                    if (zone.cells.Count == 0)
                        throw new InvalidOperationException("No unpolluted test cell.");

                    string[] ordinaryTrees = { "AMJ_Tree_Shii", "AMJ_Tree_Beech",
                        "AMJ_Tree_Shirabiso", "Plant_TreeOak", "Plant_TreeMaple",
                        "Plant_TreeBirch", "Plant_TreePine", "Plant_TreeBamboo", "Plant_TreePoplar" };
                    foreach (string name in ordinaryTrees)
                    {
                        ThingDef tree = DefDatabase<ThingDef>.GetNamed(name);
                        verification.Assert(name + " keeps regional Ground/TreeSowing contract", delegate
                        {
                            return tree.plant != null && tree.plant.IsTree &&
                                tree.plant.sowTags.Contains("Ground") && tree.plant.mustBeWildToSow &&
                                tree.plant.sowResearchPrerequisites != null &&
                                tree.plant.sowResearchPrerequisites.Contains(research);
                        });
                    }

                    HashSet<string> expectedWild = new HashSet<string>(
                        ExpectedWildTreeLikePlants(map.Biome.defName));
                    HashSet<string> expectedSowable = new HashSet<string>(
                        ExpectedSowableTrees(map.Biome.defName));
                    HashSet<string> regionalWildTrees = new HashSet<string>();
                    // AllWildPlants is map-wide in RimWorld 1.6. It can include
                    // plants from additional map biomes / tile mutators (coast,
                    // river, etc.), so it is not the base biome's distribution.
                    foreach (BiomePlantRecord record in map.Biome.wildPlants)
                        if (record.plant != null && record.plant.plant != null &&
                            record.plant.plant.IsTree && record.commonality > 0f)
                            regionalWildTrees.Add(record.plant.defName);
                    verification.Assert("base biome wild woody set equals approved natural vegetation set",
                        delegate { return regionalWildTrees.SetEquals(expectedWild); });
                    Log.Message("[AMJ Environment TreeSowing] biome=" + map.Biome.defName +
                        " baseBiomeWildWoody={" + string.Join(",", regionalWildTrees) + "}" +
                        " expectedWild={" + string.Join(",", expectedWild) + "}" +
                        " expectedSowable={" + string.Join(",", expectedSowable) + "}");

                    progress[research] = 0f;
                    verification.Assert("TreeSowing is unfinished in locked-state test",
                        delegate { return !research.IsFinished; });
                    HashSet<string> locked = AvailableGrowingZoneTrees(zone, map);
                    verification.Assert("no ordinary tree selectable before TreeSowing",
                        delegate { return locked.Count == 0; });

                    progress[research] = research.Cost;
                    verification.Assert("TreeSowing is finished in unlocked-state test",
                        delegate { return research.IsFinished; });
                    HashSet<string> available = AvailableGrowingZoneTrees(zone, map);
                    verification.Assert("growing-zone tree options equal approved regional set",
                        delegate { return available.SetEquals(expectedSowable); });
                    ThingDef haimatsu = DefDatabase<ThingDef>.GetNamed("AMJ_Shrub_Haimatsu");
                    verification.Assert("Haimatsu remains outside growing-zone sowing options", delegate
                    {
                        return !PlantUtility.CanSowOnGrower(haimatsu, zone);
                    });
                    Log.Message("[AMJ Environment TreeSowing] biome=" + map.Biome.defName +
                        " expectedSowable={" + string.Join(",", expectedSowable) + "}" +
                        " available={" + string.Join(",", available) + "}");
                }
                finally
                {
                    // IsFinished can insert missing progress entries; restore the whole dictionary.
                    progress.Clear();
                    foreach (KeyValuePair<ResearchProjectDef, float> item in saved)
                        progress.Add(item.Key, item.Value);
                }
                return true;
            });
        }

        private static void AddHarvestOutputAssertions(QuickstartVerification verification, Map map, string expected)
        {
            string[] names = { "AMJ_Tree_Shii", "AMJ_Tree_Beech", "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu", "AMJ_Tree_Hannoki" };
            int[] yields = { 42, 40, 30, 8, 24 };
            float originalYieldFactor = Find.Storyteller.difficulty.cropYieldFactor;
            Find.Storyteller.difficulty.cropYieldFactor = 1f;
            try
            {
            bool mo = ModLister.GetActiveModWithIdentifier("dankpyon.medieval.overhaul", true) != null;
            verification.Assert("harvest profile matches expected resource", delegate {
                return map != null && ((expected == "WoodLog" && !mo) || (expected == "DankPyon_RawWood" && mo));
            });
            for (int index = 0; index < names.Length; index++)
            {
                string name = names[index];
                ThingDef def = DefDatabase<ThingDef>.GetNamed(name);
                int expectedYield = yields[index];
                verification.Assert(name + " loaded harvest resource and base yield", delegate {
                    return def.plant.harvestedThingDef != null && def.plant.harvestedThingDef.defName == expected &&
                        def.plant.harvestYield == expectedYield && def.plant.harvestTag == "Wood";
                });
                IntVec3 cell = map.Center + new IntVec3(index * 4 - 8, 0, 0);
                foreach (IntVec3 area in GenRadial.RadialCellsAround(cell, 2f, true))
                {
                    if (!area.InBounds(map)) continue;
                    foreach (Thing thing in area.GetThingList(map).ToArray())
                        if (!(thing is Pawn)) thing.Destroy(DestroyMode.Vanish);
                    map.terrainGrid.SetTerrain(area, TerrainDefOf.Soil);
                }
                Plant plant = (Plant)GenSpawn.Spawn(def, cell, map);
                plant.Growth = 1f;
                plant.HitPoints = plant.MaxHitPoints;
                Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, Faction.OfPlayer);
                pawn.skills.GetSkill(SkillDefOf.Plants).Level = 0;
                GenSpawn.Spawn(pawn, cell + IntVec3.North, map);
                map.designationManager.AddDesignation(new Designation(plant, DesignationDefOf.CutPlant));
                pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.CutPlant, plant), JobCondition.InterruptForced);
                int ticks = 0;
                while (!plant.Destroyed && ticks++ < 15000)
                {
                    pawn.pather.PatherTick();
                    if (pawn.jobs.curDriver == null) break;
                    pawn.jobs.curDriver.DriverTick();
                    if (pawn.jobs.curDriver != null) pawn.jobs.curDriver.DriverTickInterval(1);
                }
                int count = 0;
                bool wrongResource = false;
                foreach (IntVec3 area in GenRadial.RadialCellsAround(cell, 2f, true))
                {
                    if (!area.InBounds(map)) continue;
                    foreach (Thing thing in area.GetThingList(map))
                    {
                        if (thing.def.defName == expected) count += thing.stackCount;
                        if ((thing.def.defName == "WoodLog" || thing.def.defName == "DankPyon_RawWood") && thing.def.defName != expected)
                            wrongResource = true;
                    }
                }
                bool destroyed = plant.Destroyed;
                int output = count;
                bool wrong = wrongResource;
                verification.Assert(name + " native pawn cutting produces only " + expected, delegate {
                    return destroyed && output == expectedYield && !wrong;
                });
                Log.Message("[AMJ Environment Harvest] plant=" + name + " resource=" + expected + " output=" + count +
                    " destroyed=" + destroyed + " wrongResource=" + wrongResource + " targetedPawnTicks=" + ticks +
                    " currentJob=" + (pawn.CurJob == null ? "null" : pawn.CurJob.def.defName));
                pawn.Destroy(DestroyMode.Vanish);
            }
            }
            finally { Find.Storyteller.difficulty.cropYieldFactor = originalYieldFactor; }
        }

        private static void AddMoForestHerbAssertions(
            QuickstartVerification verification, Map map)
        {
            if (ModLister.GetActiveModWithIdentifier(
                    "dankpyon.medieval.overhaul", true) == null)
                return;

            string biome = map == null || map.Biome == null
                ? "" : map.Biome.defName;
            float expected = 0f;
            switch (biome)
            {
                case "AMJ_WarmTemperateForest":
                case "AMJ_CoolTemperateForest":
                case "AMJ_AlpineZone":
                    expected = 0.05f;
                    break;
                case "AMJ_SubalpineForest":
                    expected = 0.16f;
                    break;
                default:
                    return;
            }

            // MO owns the ThingDefs and its five Vanilla biome values.
            // These are equivalent weights on AMJE's replacement habitats.
            string[] herbs = {
                "DankPyon_Plant_MindwortWild",
                "DankPyon_Plant_PoppyWild",
                "DankPyon_Plant_FleawortWild",
                "DankPyon_Plant_FlyAgaricWild"
            };
            int naturallySpawned = 0;
            string observations = "";
            foreach (string name in herbs)
            {
                ThingDef herb = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                float targetWeight = expected;
                verification.Assert(biome + " preserves MO herbal wild commonality " + name,
                    delegate
                    {
                        return map != null && herb != null &&
                            System.Math.Abs(map.Biome.CommonalityOfPlant(herb) -
                                targetWeight) < 0.001f;
                    });
                int found = herb == null || map == null
                    ? 0 : map.listerThings.ThingsOfDef(herb).Count;
                naturallySpawned += found;
                observations += " " + name + "=" + found;
            }
            Log.Message("[AMJ Environment MO Forest Native Spawn] biome=" +
                biome + " total=" + naturallySpawned + observations);

            // Alpine terrain and severe snow can suppress current live plants
            // even though its loaded Tundra-equivalent pool stays registered.
            if (biome != "AMJ_AlpineZone")
            {
                int observed = naturallySpawned;
                verification.Assert(biome + " naturally spawns MO medicinal vegetation",
                    delegate { return map != null && observed > 0; });
            }
        }

        // These defs exist only in the temporary Quicktests Mod. This checks
        // RimWorld's REAL loaded inheritance and native CutPlant job, without
        // promoting unapproved wetland yields or provisional graphics.
        protected static void AddWetlandBehaviorProbeAssertions(
            QuickstartVerification verification, Map map)
        {
            string[] names = { "AMJE_Test_Suge", "AMJE_Test_Mizugoke", "AMJE_Test_Yoshi" };
            float previousYieldFactor = Find.Storyteller.difficulty.cropYieldFactor;
            Find.Storyteller.difficulty.cropYieldFactor = 1f;

            try
            {
                for (int index = 0; index < names.Length; index++)
                {
                    string name = names[index];
                    bool hayProbe = index != 1;
                    bool yoshi = index == 2;
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                    verification.Assert(name + " belongs to disposable Quicktests fixture", delegate
                    {
                        return def != null && def.plant != null &&
                            def.modContentPack != null &&
                            def.modContentPack.PackageIdPlayerFacing ==
                                "sucro.ancientmedievaljapan.environment.quicktests";
                    });

                    verification.Assert(name + " loaded parent/interaction contract", delegate
                    {
                        return def != null && def.plant != null &&
                            def.selectable &&
                            System.Math.Abs(def.plant.minGrowthTemperature) < 0.001f &&
                            (hayProbe
                                ? def.plant.harvestedThingDef != null &&
                                  def.plant.harvestedThingDef.defName == "Hay" &&
                                  def.plant.harvestTag == "Standard" &&
                                  System.Math.Abs(def.plant.harvestYield - 1f) < 0.001f
                                : def.plant.harvestedThingDef == null) &&
                            (!yoshi || def.plant.sowTags == null ||
                                def.plant.sowTags.Count == 0);
                    });

                    if (map == null || def == null || def.plant == null)
                        continue;

                    // Use prepared, disposable soil cells. The plant is NOT
                    // added to the biome pool and no natural-spawn PASS is claimed.
                    IntVec3 cell = map.Center + new IntVec3(index * 7 + 12, 0, 12);
                    foreach (IntVec3 area in GenRadial.RadialCellsAround(cell, 2f, true))
                    {
                        if (!area.InBounds(map))
                            continue;
                        foreach (Thing thing in area.GetThingList(map).ToArray())
                            if (!(thing is Pawn))
                                thing.Destroy(DestroyMode.Vanish);
                        map.terrainGrid.SetTerrain(area, TerrainDefOf.Soil);
                    }

                    Plant plant = (Plant)GenSpawn.Spawn(def, cell, map);
                    if (yoshi)
                    {
                        // Probe the native harvest-eligibility threshold, not
                        // an approved harvest product or a Harvest-designated job.
                        plant.Growth = 0.40f;
                        bool immatureHarvestable = plant.HarvestableNow;
                        plant.Growth = 1f;
                        bool matureHarvestable = plant.HarvestableNow;
                        verification.Assert(name + " maturity gates native harvest eligibility",
                            delegate { return !immatureHarvestable && matureHarvestable; });
                        Log.Message("[AMJ Environment Reed Maturity Probe] " +
                            "biome=" + map.Biome.defName +
                            " immatureHarvestable=" + immatureHarvestable +
                            " matureHarvestable=" + matureHarvestable);
                    }
                    plant.Growth = 1f;
                    plant.HitPoints = plant.MaxHitPoints;
                    Pawn pawn = PawnGenerator.GeneratePawn(
                        PawnKindDefOf.Colonist, Faction.OfPlayer);
                    pawn.skills.GetSkill(SkillDefOf.Plants).Level = 0;
                    GenSpawn.Spawn(pawn, cell + IntVec3.North, map);
                    map.designationManager.AddDesignation(
                        new Designation(plant, DesignationDefOf.CutPlant));
                    pawn.jobs.StartJob(
                        JobMaker.MakeJob(JobDefOf.CutPlant, plant),
                        JobCondition.InterruptForced);

                    int ticks = 0;
                    while (!plant.Destroyed && ticks++ < 15000)
                    {
                        pawn.pather.PatherTick();
                        if (pawn.jobs.curDriver == null)
                            break;
                        pawn.jobs.curDriver.DriverTick();
                        if (pawn.jobs.curDriver != null)
                            pawn.jobs.curDriver.DriverTickInterval(1);
                    }

                    int hay = 0;
                    bool unexpectedItem = false;
                    foreach (IntVec3 area in GenRadial.RadialCellsAround(cell, 2f, true))
                    {
                        if (!area.InBounds(map))
                            continue;
                        foreach (Thing thing in area.GetThingList(map))
                        {
                            if (thing.def.category != ThingCategory.Item)
                                continue;
                            if (thing.def.defName == "Hay")
                                hay += thing.stackCount;
                            else
                                unexpectedItem = true;
                        }
                    }

                    bool removed = plant.Destroyed;
                    int observedHay = hay;
                    bool wrongDrop = unexpectedItem;
                    verification.Assert(
                        name + " native CutPlant removes and drops expected items",
                        delegate
                        {
                            return removed && !wrongDrop &&
                                observedHay == (hayProbe ? 1 : 0);
                        });
                    Log.Message("[AMJ Environment Wetland Native Cut Probe] " +
                        "biome=" + map.Biome.defName +
                        " def=" + name + " destroyed=" + removed +
                        " Hay=" + observedHay +
                        " otherItem=" + wrongDrop + " ticks=" + ticks);
                    pawn.Destroy(DestroyMode.Vanish);
                }
            }
            finally
            {
                Find.Storyteller.difficulty.cropYieldFactor = previousYieldFactor;
            }
        }

        // Runs on the existing wetland maps, with a test-only tree from the
        // temporary Quicktests Mod. No production Hannoki/harvest balancing.
        protected static void AddHannokiBehaviorProbeAssertions(
            QuickstartVerification verification, Map map)
        {
            const string name = "AMJE_Test_Hannoki";
            ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
            ThingDef reference = DefDatabase<ThingDef>.GetNamedSilentFail("AMJ_Tree_Shii");
            bool mo = ModLister.GetActiveModWithIdentifier(
                "dankpyon.medieval.overhaul", true) != null;
            string expectedResource = reference != null && reference.plant != null &&
                reference.plant.harvestedThingDef != null
                ? reference.plant.harvestedThingDef.defName : null;
            bool recognizedResource = expectedResource == "WoodLog" ||
                (mo && expectedResource == "DankPyon_RawWood");

            verification.Assert(name + " is owned by temporary Quicktests fixture",
                delegate
                {
                    return def != null && def.plant != null &&
                        def.modContentPack != null &&
                        def.modContentPack.PackageIdPlayerFacing ==
                            "sucro.ancientmedievaljapan.environment.quicktests";
                });
            verification.Assert(name + " inherits a native deciduous tree wood contract",
                delegate
                {
                    return def != null && def.plant != null &&
                        def.plant.IsTree && def.selectable &&
                        System.Math.Abs(def.plant.minGrowthTemperature) < 0.001f &&
                        def.plant.harvestedThingDef != null &&
                        def.plant.harvestTag == "Wood" &&
                        System.Math.Abs(def.plant.harvestYield - 1f) < 0.001f &&
                        recognizedResource &&
                        def.plant.harvestedThingDef.defName == expectedResource &&
                        (!mo || expectedResource == "WoodLog" ||
                            DefDatabase<ThingDef>.GetNamedSilentFail("DankPyon_RawWood") != null);
                });

            if (map == null || def == null || def.plant == null || !recognizedResource)
                return;

            // Existing Shii indicates the actual loaded wood-chain state.
            // This run tests only that state; it does not toggle MO settings.
            string tags = def.plant.sowTags == null
                ? "(none)" : string.Join(",", def.plant.sowTags.ToArray());
            Log.Message("[AMJ Environment Hannoki Native Contract] biome=" +
                map.Biome.defName + " MO=" + mo + " woodReference=" +
                expectedResource + " sowTags=" + tags +
                " mustBeWildToSow=" + def.plant.mustBeWildToSow);

            IntVec3 cell = map.Center + new IntVec3(33, 0, 12);
            foreach (IntVec3 area in GenRadial.RadialCellsAround(cell, 2f, true))
            {
                if (!area.InBounds(map))
                    continue;
                foreach (Thing thing in area.GetThingList(map).ToArray())
                    if (!(thing is Pawn))
                        thing.Destroy(DestroyMode.Vanish);
                map.terrainGrid.SetTerrain(area, TerrainDefOf.Soil);
            }

            float oldYieldFactor = Find.Storyteller.difficulty.cropYieldFactor;
            Find.Storyteller.difficulty.cropYieldFactor = 1f;
            try
            {
                Plant plant = (Plant)GenSpawn.Spawn(def, cell, map);
                plant.Growth = 1f;
                plant.HitPoints = plant.MaxHitPoints;
                Pawn pawn = PawnGenerator.GeneratePawn(
                    PawnKindDefOf.Colonist, Faction.OfPlayer);
                pawn.skills.GetSkill(SkillDefOf.Plants).Level = 0;
                GenSpawn.Spawn(pawn, cell + IntVec3.North, map);
                map.designationManager.AddDesignation(
                    new Designation(plant, DesignationDefOf.CutPlant));
                pawn.jobs.StartJob(
                    JobMaker.MakeJob(JobDefOf.CutPlant, plant),
                    JobCondition.InterruptForced);

                int ticks = 0;
                while (!plant.Destroyed && ticks++ < 15000)
                {
                    pawn.pather.PatherTick();
                    if (pawn.jobs.curDriver == null)
                        break;
                    pawn.jobs.curDriver.DriverTick();
                    if (pawn.jobs.curDriver != null)
                        pawn.jobs.curDriver.DriverTickInterval(1);
                }

                int output = 0;
                bool unexpected = false;
                foreach (IntVec3 area in GenRadial.RadialCellsAround(cell, 2f, true))
                {
                    if (!area.InBounds(map))
                        continue;
                    foreach (Thing thing in area.GetThingList(map))
                    {
                        if (thing.def.category != ThingCategory.Item)
                            continue;
                        if (thing.def.defName == expectedResource)
                            output += thing.stackCount;
                        else
                            unexpected = true;
                    }
                }
                bool destroyed = plant.Destroyed;
                int observedOutput = output;
                bool wrongItem = unexpected;
                verification.Assert(name + " native CutPlant drops reference wood only",
                    delegate
                    {
                        return destroyed && observedOutput == 1 && !wrongItem;
                    });
                Log.Message("[AMJ Environment Hannoki Native Cut Probe] biome=" +
                    map.Biome.defName + " def=" + name + " resource=" +
                    expectedResource + " count=" + observedOutput +
                    " destroyed=" + destroyed + " otherItem=" + wrongItem +
                    " ticks=" + ticks);
                pawn.Destroy(DestroyMode.Vanish);
            }
            finally
            {
                Find.Storyteller.difficulty.cropYieldFactor = oldYieldFactor;
            }
        }

        // Live loaded-Def, natural density and actual work: no disposable Defs used.
        protected static void AddWetlandProductionHerbAssertions(
            QuickstartVerification v, Map map, bool cold)
        {
            string[] names = cold
                ? new[] {"AMJ_Plant_Yoshi","AMJ_Plant_Suge","AMJ_Plant_Mizugoke"}
                : new[] {"AMJ_Plant_Yoshi","AMJ_Plant_Suge"};
            float[] expected = cold ? new[] {0.60f,1.10f,1.20f}
                                    : new[] {1.00f,0.80f};
            float originalYield = Find.Storyteller.difficulty.cropYieldFactor;
            Find.Storyteller.difficulty.cropYieldFactor = 1f;
            try
            {
                for (int i = 0; i < names.Length; i++)
                {
                    string name = names[i];
                    float approvedWeight = expected[i];
                    bool moss = name == "AMJ_Plant_Mizugoke";
                    bool reed = name == "AMJ_Plant_Yoshi";
                    int expectedBaseYield = reed ? 2 : 1;
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                    int natural = map == null || def == null ? 0 :
                        map.listerThings.ThingsOfDef(def).Count;
                    int shallow = 0;
                    if (map != null && def != null)
                        foreach (Thing plant in map.listerThings.ThingsOfDef(def))
                        {
                            TerrainDef t = map.terrainGrid.TerrainAt(plant.Position);
                            if (t != null && t.defName == "WaterShallow") shallow++;
                        }
                    v.Assert(name + " production Def and approved harvest data",
                        delegate { return def != null && def.plant != null &&
                            def.modContentPack != null &&
                            def.modContentPack.PackageIdPlayerFacing ==
                                "sucro.ancientmedievaljapan.environment" &&
                            def.selectable &&
                            (def.plant.sowTags == null || def.plant.sowTags.Count == 0) &&
                            Math.Abs(def.plant.minGrowthTemperature -
                                (CctoIsActive() && reed ? 5f : 0f)) < 0.001f &&
                            (moss ? def.plant.harvestedThingDef == null :
                                def.plant.harvestedThingDef != null &&
                                def.plant.harvestedThingDef.defName == "Hay" &&
                                def.plant.harvestTag == "Standard" &&
                                Math.Abs(def.plant.harvestYield - expectedBaseYield) < 0.001f); });
                    v.Assert(name + " loaded biome weight",
                        delegate { return map != null && def != null &&
                            Math.Abs(map.Biome.CommonalityOfPlant(def) - approvedWeight) < 0.001f; });
                    v.Assert(name + " naturally generated",
                        delegate { return natural > 0; });
                    v.Assert(name + " texture loaded without BadTex",
                        delegate { return def != null && def.graphicData != null &&
                            HasLoadedNonBadTexture(def.graphicData.Graphic); });
                    Log.Message("[AMJ Wetland Production Spawn] biome=" +
                        (map == null || map.Biome == null ? "none" : map.Biome.defName) +
                        " def=" + name + " natural=" + natural + " shallow=" +
                        shallow + " other=" + (natural-shallow) + " weight=" + approvedWeight);
                    if (map == null || def == null || def.plant == null) continue;
                    IntVec3 cell = map.Center + new IntVec3(35+i*8,0,-18);
                    foreach (IntVec3 area in GenRadial.RadialCellsAround(cell,2f,true))
                    {
                        if (!area.InBounds(map)) continue;
                        foreach (Thing thing in area.GetThingList(map).ToArray())
                            if (!(thing is Pawn)) thing.Destroy(DestroyMode.Vanish);
                        map.terrainGrid.SetTerrain(area,TerrainDefOf.Soil);
                    }
                    // Keep Vanilla's normal, failable harvest behavior in
                    // shipping Defs. A single real cutting of a wild herb can
                    // fail by chance even at high skill; bounded independent
                    // real cuts distinguish stochastic loss from broken output.
                    // No harvested item is fabricated by the test observer.
                    int observedHay=0;
                    bool invalidItem=false;
                    bool removed=false;
                    int totalTicks=0, attempts=0;
                    int limit=moss ? 1 : 3;
                    for (int attempt=0;attempt<limit;attempt++)
                    {
                        attempts++;
                        Plant specimen=(Plant)GenSpawn.Spawn(def,cell,map);
                        specimen.Growth=1f;
                        specimen.HitPoints=specimen.MaxHitPoints;
                        Pawn pawn=PawnGenerator.GeneratePawn(
                            PawnKindDefOf.Colonist,Faction.OfPlayer);
                        pawn.skills.GetSkill(SkillDefOf.Plants).Level=20;
                        GenSpawn.Spawn(pawn,cell+IntVec3.North,map);
                        map.designationManager.AddDesignation(
                            new Designation(specimen,DesignationDefOf.CutPlant));
                        pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.CutPlant,specimen),
                            JobCondition.InterruptForced);
                        int ticks=0;
                        while (!specimen.Destroyed && ticks++<15000)
                        {
                            pawn.pather.PatherTick();
                            if (pawn.jobs.curDriver==null) break;
                            pawn.jobs.curDriver.DriverTick();
                            if (pawn.jobs.curDriver!=null)
                                pawn.jobs.curDriver.DriverTickInterval(1);
                        }
                        totalTicks+=ticks;
                        removed=specimen.Destroyed;
                        int thisHay=0;
                        bool unexpected=false;
                        foreach (IntVec3 area in GenRadial.RadialCellsAround(cell,2f,true))
                        {
                            if (!area.InBounds(map)) continue;
                            foreach (Thing thing in area.GetThingList(map))
                            {
                                if (thing.def.category!=ThingCategory.Item) continue;
                                if (thing.def.defName=="Hay") thisHay+=thing.stackCount;
                                else unexpected=true;
                            }
                        }
                        observedHay=thisHay;
                        invalidItem=unexpected;
                        Log.Message("[AMJ Wetland Production Cut Attempt] def="+name+
                            " index="+(attempt+1)+" removed="+removed+
                            " Hay="+thisHay+" other="+unexpected+" ticks="+ticks);
                        pawn.Destroy(DestroyMode.Vanish);
                        // Never suppress real job timeout, wrong-item drops,
                        // or a missing/nonremovable moss specimen.
                        if (moss || observedHay>0 || !removed || invalidItem) break;
                    }
                    v.Assert(name + " native CutPlant output type and removal",
                        delegate { return removed && !invalidItem &&
                            (moss ? observedHay==0 : observedHay>0); });
                    Log.Message("[AMJ Wetland Production Cut] def="+name+
                        " removed="+removed+" Hay="+observedHay+
                        " other="+invalidItem+" attempts="+attempts+
                        " totalTicks="+totalTicks);
                }
            }
            finally {Find.Storyteller.difficulty.cropYieldFactor=originalYield;}
        }

        // Production AMJ_Tree_Hannoki, not the disposable AMJE_Test_Hannoki.
        // Checks the actual loaded wood-chain choice, loaded seasonal graphics
        // and a colonist's native CutPlant drop at the approved yield of 24.
        // Natural plant count is recorded BEFORE the manual cutting specimen.
        protected static void AddHannokiProductionAssertions(
            QuickstartVerification verification, Map map, float expectedCommonality)
        {
            const string name = "AMJ_Tree_Hannoki";
            ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
            ThingDef shii = DefDatabase<ThingDef>.GetNamedSilentFail("AMJ_Tree_Shii");
            bool ccto = CctoIsActive();
            string expectedWood = shii != null && shii.plant != null &&
                shii.plant.harvestedThingDef != null
                ? shii.plant.harvestedThingDef.defName : null;
            bool knownWood = expectedWood == "WoodLog" ||
                expectedWood == "DankPyon_RawWood";
            int natural = map == null || def == null
                ? 0 : map.listerThings.ThingsOfDef(def).Count;
            verification.Assert("Hannoki loaded production Def and approved native tree values",
                delegate {
                    return def != null && def.plant != null && map != null &&
                        def.modContentPack != null &&
                        def.modContentPack.PackageIdPlayerFacing ==
                            "sucro.ancientmedievaljapan.environment" &&
                        def.plant.IsTree && def.selectable &&
                        Math.Abs(def.plant.growDays - 20f) < 0.001f &&
                        Math.Abs(def.plant.harvestYield - 24f) < 0.001f &&
                        Math.Abs(def.plant.harvestWork - 800f) < 0.001f &&
                        def.plant.harvestTag == "Wood" &&
                        Math.Abs(def.plant.minGrowthTemperature - (ccto ? 5f : 0f)) < 0.001f &&
                        knownWood && def.plant.harvestedThingDef != null &&
                        def.plant.harvestedThingDef.defName == expectedWood;
                });
            verification.Assert("Hannoki loaded wetland pool has approved weight",
                delegate { return map != null && def != null &&
                    Math.Abs(map.Biome.CommonalityOfPlant(def) - expectedCommonality) < 0.001f; });
            verification.Assert("Hannoki naturally spawns in this actual wetland map",
                delegate { return natural > 0; });
            verification.Assert("Hannoki normal and leafless graphics are loaded (not BadTex)",
                delegate { return def != null && def.graphicData != null &&
                    def.plant != null && HasLoadedNonBadTexture(def.graphicData.Graphic) &&
                    HasLoadedNonBadTexture(def.plant.leaflessGraphic); });
            verification.Assert("Hannoki leafy and leafless snow overlays are loaded (not BadTex)",
                delegate { return def != null && def.plant != null &&
                    HasLoadedNonBadTexture(def.plant.snowOverlayGraphic) &&
                    HasLoadedNonBadTexture(def.plant.leaflessSnowOverlayGraphic); });

            int extensions = 0;
            bool dormancy = false;
            if (def != null && def.modExtensions != null)
                foreach (DefModExtension extension in def.modExtensions)
                    if (extension.GetType().FullName ==
                        "CropColdToleranceOverhaul.ColdToleranceExtension")
                    {
                        extensions++;
                        System.Reflection.FieldInfo field =
                            AccessTools.Field(extension.GetType(), "coldDormancy");
                        dormancy = field != null &&
                            field.GetValue(extension) is bool &&
                            (bool)field.GetValue(extension);
                    }
            int extCount = extensions;
            bool extDormancy = dormancy;
            verification.Assert("Hannoki optional CCTO extension matches active profile",
                delegate { return ccto ? extCount == 1 && extDormancy : extCount == 0; });

            Log.Message("[AMJ Environment Hannoki Production Spawn] biome=" +
                (map == null || map.Biome == null ? "<none>" : map.Biome.defName) +
                " naturallyGenerated=" + natural + " weight=" + expectedCommonality +
                " wood=" + expectedWood + " CCTO=" + ccto +
                " coldExtensions=" + extCount + " dormancy=" + extDormancy);
            if (map == null || def == null || def.plant == null || !knownWood)
                return;

            // Isolated test specimen: never include it in the natural count.
            IntVec3 cell = map.Center + new IntVec3(41, 0, 14);
            foreach (IntVec3 area in GenRadial.RadialCellsAround(cell, 2f, true))
            {
                if (!area.InBounds(map)) continue;
                foreach (Thing thing in area.GetThingList(map).ToArray())
                    if (!(thing is Pawn)) thing.Destroy(DestroyMode.Vanish);
                map.terrainGrid.SetTerrain(area, TerrainDefOf.Soil);
            }
            float previousYield = Find.Storyteller.difficulty.cropYieldFactor;
            Find.Storyteller.difficulty.cropYieldFactor = 1f;
            try
            {
                Plant plant = (Plant)GenSpawn.Spawn(def, cell, map);
                plant.Growth = 1f;
                plant.HitPoints = plant.MaxHitPoints;
                Pawn pawn = PawnGenerator.GeneratePawn(
                    PawnKindDefOf.Colonist, Faction.OfPlayer);
                pawn.skills.GetSkill(SkillDefOf.Plants).Level = 0;
                GenSpawn.Spawn(pawn, cell + IntVec3.North, map);
                map.designationManager.AddDesignation(
                    new Designation(plant, DesignationDefOf.CutPlant));
                pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.CutPlant, plant),
                    JobCondition.InterruptForced);
                int ticks = 0;
                while (!plant.Destroyed && ticks++ < 15000)
                {
                    pawn.pather.PatherTick();
                    if (pawn.jobs.curDriver == null) break;
                    pawn.jobs.curDriver.DriverTick();
                    if (pawn.jobs.curDriver != null)
                        pawn.jobs.curDriver.DriverTickInterval(1);
                }
                int output = 0;
                bool wrongResource = false;
                foreach (IntVec3 area in GenRadial.RadialCellsAround(cell, 2f, true))
                {
                    if (!area.InBounds(map)) continue;
                    foreach (Thing thing in area.GetThingList(map))
                    {
                        if (thing.def.category != ThingCategory.Item) continue;
                        if (thing.def.defName == expectedWood)
                            output += thing.stackCount;
                        else
                            wrongResource = true;
                    }
                }
                bool removed = plant.Destroyed;
                int observed = output;
                bool wrong = wrongResource;
                verification.Assert("Hannoki production native CutPlant yields exactly 24 loaded wood",
                    delegate { return removed && observed == 24 && !wrong; });
                Log.Message("[AMJ Environment Hannoki Production Cut] biome=" +
                    map.Biome.defName + " expected=" + expectedWood +
                    " output=" + observed + " destroyed=" + removed +
                    " wrongItem=" + wrong + " ticks=" + ticks);
                pawn.Destroy(DestroyMode.Vanish);
            }
            finally { Find.Storyteller.difficulty.cropYieldFactor = previousYield; }
        }

        protected static void AddWetlandEcologyAssertions(
            QuickstartVerification verification, Map map, bool cold)
        {
            BiomeDef biome = map == null ? null : map.Biome;
            string id = cold ? "ColdBog" : "TemperateSwamp";
            string[] animals = cold
                ? new[] { "Hare", "Snowhare", "Squirrel", "Rat", "Deer",
                    "WildBoar", "Fox_Red", "Wolf_Timber", "Bear_Grizzly" }
                : new[] { "Hare", "Squirrel", "Rat", "Deer", "WildBoar",
                    "Fox_Red", "Wolf_Timber", "Bear_Grizzly" };
            string[] diseases = cold
                ? new[] { "Disease_Flu", "Disease_Plague",
                    "Disease_GutWorms", "Disease_MuscleParasites",
                    "Disease_AnimalFlu", "Disease_AnimalPlague" }
                : new[] { "Disease_Flu", "Disease_Plague", "Disease_Malaria",
                    "Disease_GutWorms", "Disease_MuscleParasites",
                    "Disease_AnimalFlu", "Disease_AnimalPlague" };

            string approvedEnglishDescription = cold
                ? "A wetland found across cooler regions. Grasses and mosses grow alongside scattered stands of willow and birch trees. The waterlogged ground readily turns muddy, making travel and construction difficult."
                : "A wetland found in the warm, rainy lowlands and along rivers of the Japanese archipelago. Tall grasses and stands of willow trees intermingle among muddy ground and shallow water. The damp ground restricts travel and construction.";
            string approvedJapaneseDescription = cold
                ? "冷涼な地域に広がる湿原。草本やコケ類に加え、ヤナギやカバノキ類の木立が点在する。水を多く含む地盤はぬかるみやすく、移動や建築が難しい。"
                : "日本列島の温暖で雨の多い低地や河川沿いに広がる湿地。背の高い草やヤナギ類の木立が入り混じり、泥土と浅い水面が広がる。湿った地盤は通行や建築に制約を与える。";

            string approvedEnglishLabel = cold
                ? "cool wetland" : "temperate wetland";
            string approvedJapaneseLabel = cold
                ? "冷涼湿原" : "温帯湿地";
            verification.Assert(id + " has an approved EN/JA wetland label and description",
                delegate
                {
                    return biome != null && biome.label != null &&
                        biome.description != null &&
                        (string.Equals(biome.label.Trim(),
                            approvedEnglishLabel, StringComparison.Ordinal) ||
                         string.Equals(biome.label.Trim(),
                            approvedJapaneseLabel, StringComparison.Ordinal)) &&
                        (string.Equals(biome.description.Trim(),
                            approvedEnglishDescription, StringComparison.Ordinal) ||
                         string.Equals(biome.description.Trim(),
                            approvedJapaneseDescription, StringComparison.Ordinal));
                });

            // The Environment wetland patch must preserve any MO additions,
            // independent of whether MO's PatchOperationAdd runs before/after it.
            bool medievalOverhaulActive = false;
            foreach (ModContentPack mod in LoadedModManager.RunningModsListForReading)
            {
                if (mod.PackageIdPlayerFacing.StartsWith(
                    "dankpyon.medieval.overhaul", StringComparison.OrdinalIgnoreCase))
                {
                    medievalOverhaulActive = true;
                    break;
                }
            }
            if (medievalOverhaulActive)
            {
                string[] moHerbs = new[] {
                    "DankPyon_Plant_MindwortWild", "DankPyon_Plant_PoppyWild",
                    "DankPyon_Plant_FleawortWild", "DankPyon_Plant_FlyAgaricWild" };
                verification.Assert(id + " preserves MO wild medicinal plants", delegate
                {
                    if (biome == null)
                        return false;
                    foreach (string name in moHerbs)
                    {
                        ThingDef herb = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                        if (herb == null ||
                            System.Math.Abs(biome.CommonalityOfPlant(herb) - 0.05f) > 0.001f)
                            return false;
                    }
                    return true;
                });

                // Count only already spawned plants on the generated map.
                // Do not artificially spawn MO species or change their weights.
                int naturalHerbs = 0;
                string observations = "";
                foreach (string name in moHerbs)
                {
                    ThingDef herb = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                    int count = (map == null || herb == null)
                        ? 0 : map.listerThings.ThingsOfDef(herb).Count;
                    naturalHerbs += count;
                    observations += " " + name + "=" + count;
                }
                Log.Message("[AMJ Environment MO Wetland Native Spawn] biome=" + id +
                    " total=" + naturalHerbs + observations);
                verification.Assert(id + " naturally spawned MO medicinal vegetation",
                    delegate { return map != null && naturalHerbs > 0; });
            }

            var animalField = AccessTools.Field(typeof(BiomeDef), "wildAnimals");
            var diseaseField = AccessTools.Field(typeof(BiomeDef), "diseases");
            var packField = AccessTools.Field(typeof(BiomeDef), "allowedPackAnimals");

            var loadedAnimals = biome == null || animalField == null
                ? null : animalField.GetValue(biome) as List<BiomeAnimalRecord>;
            var loadedDiseases = biome == null || diseaseField == null
                ? null : diseaseField.GetValue(biome) as List<BiomeDiseaseRecord>;
            var loadedPack = biome == null || packField == null
                ? null : packField.GetValue(biome) as List<ThingDef>;

            verification.Assert(id + " has exactly the approved wildlife proxies", delegate
            {
                if (loadedAnimals == null || loadedAnimals.Count != animals.Length)
                    return false;
                HashSet<string> expected = new HashSet<string>(animals);
                foreach (BiomeAnimalRecord record in loadedAnimals)
                    if (record == null || record.animal == null ||
                        record.commonality <= 0f ||
                        !expected.Remove(record.animal.defName))
                        return false;
                return expected.Count == 0;
            });

            verification.Assert(id + " has no foreign wild pack animals", delegate
            {
                return loadedPack != null && loadedPack.Count == 0;
            });

            verification.Assert(id + " has exactly the approved disease group", delegate
            {
                if (loadedDiseases == null || loadedDiseases.Count != diseases.Length)
                    return false;
                HashSet<string> expected = new HashSet<string>(diseases);
                foreach (BiomeDiseaseRecord record in loadedDiseases)
                    if (record == null || record.diseaseInc == null ||
                        record.commonality <= 0f ||
                        !expected.Remove(record.diseaseInc.defName))
                        return false;
                return expected.Count == 0;
            });

            verification.Assert(id + " has regionally adjusted disease frequency",
                delegate { return biome != null &&
                    System.Math.Abs(biome.diseaseMtbDays - (cold ? 60f : 50f)) < 0.001f; });

            string[] weatherNames = { "Clear", "Fog", "Rain", "DryThunderstorm",
                "RainyThunderstorm", "FoggyRain", "SnowGentle", "SnowHard" };
            float[] expectedWeather = cold
                ? new[] { 16f, 2f, 3f, 0.05f, 1f, 1.5f, 7f, 7f }
                : new[] { 16f, 2f, 3f, 0.1f, 1.5f, 1.5f, 2f, 1f };

            verification.Assert(id + " has exactly eight climate weather entries",
                delegate { return biome != null &&
                    biome.baseWeatherCommonalities != null &&
                    biome.baseWeatherCommonalities.Count == weatherNames.Length; });
            for (int i = 0; i < weatherNames.Length; i++)
            {
                string name = weatherNames[i];
                float expected = expectedWeather[i];
                verification.Assert(id + " weather " + name + "=" + expected, delegate
                {
                    return System.Math.Abs(
                        GetWeatherCommonality(biome, name) - expected) < 0.001f;
                });
            }

            verification.Assert(id + " retains Vanilla wetland terrain patch makers",
                delegate { return biome != null &&
                    biome.terrainPatchMakers != null &&
                    biome.terrainPatchMakers.Count >= (cold ? 3 : 2); });
            verification.Assert(id + " generates wetland terrain on the map",
                delegate
                {
                    if (map == null)
                        return false;
                    foreach (IntVec3 cell in map.AllCells)
                    {
                        TerrainDef t = map.terrainGrid.TerrainAt(cell);
                        if (t != null && (t.defName == "Mud" ||
                            t.defName == "MarshyTerrain" ||
                            t.defName == "Marsh" ||
                            t.defName == "WaterShallow"))
                            return true;
                    }
                    return false;
                });

            Log.Message("[AMJ Environment Wetland] biome=" + id +
                " wildlife=" + (loadedAnimals == null ? -1 : loadedAnimals.Count) +
                " diseases=" + (loadedDiseases == null ? -1 : loadedDiseases.Count) +
                " packAnimals=" + (loadedPack == null ? -1 : loadedPack.Count) +
                " diseaseMtbDays=" + (biome == null ? -1f : biome.diseaseMtbDays));
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
                "Sudajii UI icon resolves a non-BadTex texture",
                delegate
                {
                    return shii != null &&
                        shii.uiIcon != null &&
                        shii.uiIcon != BaseContent.BadTex &&
                        !string.Equals(
                            shii.uiIcon.name,
                            "ERRORTEX",
                            StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(
                            shii.uiIcon.name,
                            "BadTex",
                            StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(
                            shii.uiIcon.name,
                            "BadTexture",
                            StringComparison.OrdinalIgnoreCase);
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
                    StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(
                    texture.name,
                    "BadTexture",
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
                        StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(
                        texture.name,
                        "BadTexture",
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

        private static bool CoreIsActive()
        {
            return ModLister.GetActiveModWithIdentifier(
                "sucro.ancientmedievaljapan.core",
                true) != null;
        }

        private static void AddCoreAgricultureIntegrationAssertions(
            QuickstartVerification verification,
            Map map)
        {
            string[] cropDefNames =
            {
                "AMJC_Plant_Buckwheat_Soba",
                "AMJC_Plant_ProsoMillet_Kibi",
                "AMJC_Plant_FoxtailMillet_Awa",
                "AMJC_Plant_BarnyardMillet_Hie",
                "AMJC_Plant_Barley",
                "DankPyon_Plant_Wheat"
            };

            ThingDef[] crops = new ThingDef[cropDefNames.Length];
            bool allLoaded = true;
            for (int i = 0; i < cropDefNames.Length; i++)
            {
                crops[i] = DefDatabase<ThingDef>.GetNamedSilentFail(
                    cropDefNames[i]);
                if (crops[i] == null || crops[i].plant == null)
                {
                    allLoaded = false;
                }
            }

            verification.Assert(
                "Core Stage A crop Defs are loaded in the Environment integration profile",
                delegate { return allLoaded; });

            int thin = 0;
            int gravel = 0;
            int soil = 0;
            int rich = 0;

            if (map != null)
            {
                foreach (IntVec3 cell in map.AllCells)
                {
                    TerrainDef terrain = map.terrainGrid.TerrainAt(cell);
                    if (terrain == null)
                    {
                        continue;
                    }

                    if (terrain.defName == "AMJ_ThinSoil")
                    {
                        thin++;
                    }
                    else if (terrain.defName == "Gravel")
                    {
                        gravel++;
                    }
                    else if (terrain.defName == "Soil")
                    {
                        soil++;
                    }
                    else if (terrain.defName == "SoilRich")
                    {
                        rich++;
                    }
                }
            }

            int ladderCells = thin + gravel + soil + rich;

            verification.Assert(
                "Environment natural fertility ladder exists on the generated map",
                delegate { return map != null && ladderCells > 0; });

            verification.Assert(
                "Environment Thin Soil creates a real low-fertility farming tier",
                delegate { return thin > 0; });

            if (!allLoaded || ladderCells <= 0)
            {
                return;
            }

            ThingDef soba = crops[0];
            ThingDef kibi = crops[1];
            ThingDef awa = crops[2];
            ThingDef hie = crops[3];
            ThingDef barley = crops[4];
            ThingDef wheat = crops[5];

            int sobaSuitable = SuitableNaturalLadderCells(
                soba, thin, gravel, soil, rich);
            int barleySuitable = SuitableNaturalLadderCells(
                barley, thin, gravel, soil, rich);
            int wheatSuitable = SuitableNaturalLadderCells(
                wheat, thin, gravel, soil, rich);

            verification.Assert(
                "Soba remains plantable on every Environment natural farming tier",
                delegate { return sobaSuitable == ladderCells; });

            verification.Assert(
                "Barley remains plantable on every Environment natural farming tier",
                delegate { return barleySuitable == ladderCells; });

            verification.Assert(
                "Wheat loses exactly the Thin Soil part of the Environment farming ladder",
                delegate
                {
                    return wheatSuitable < sobaSuitable
                        && sobaSuitable - wheatSuitable == thin;
                });

            float sobaFactor = AverageNaturalLadderFertilityFactor(
                soba, thin, gravel, soil, rich);
            float kibiFactor = AverageNaturalLadderFertilityFactor(
                kibi, thin, gravel, soil, rich);
            float awaFactor = AverageNaturalLadderFertilityFactor(
                awa, thin, gravel, soil, rich);
            float hieFactor = AverageNaturalLadderFertilityFactor(
                hie, thin, gravel, soil, rich);
            float barleyFactor = AverageNaturalLadderFertilityFactor(
                barley, thin, gravel, soil, rich);

            float maxFactor = System.Math.Max(
                sobaFactor,
                System.Math.Max(
                    kibiFactor,
                    System.Math.Max(
                        awaFactor,
                        System.Math.Max(hieFactor, barleyFactor))));
            float minFactor = System.Math.Min(
                sobaFactor,
                System.Math.Min(
                    kibiFactor,
                    System.Math.Min(
                        awaFactor,
                        System.Math.Min(hieFactor, barleyFactor))));

            verification.Assert(
                "Environment soil distribution produces differentiated Stage A crop growth factors",
                delegate
                {
                    return !float.IsNaN(minFactor)
                        && !float.IsNaN(maxFactor)
                        && maxFactor - minFactor > 0.01f;
                });

            verification.Assert(
                "Soba has the strongest average poor-soil growth factor among pre-wheat Stage A crops",
                delegate
                {
                    return sobaFactor > kibiFactor
                        && kibiFactor > awaFactor
                        && awaFactor > hieFactor
                        && hieFactor > barleyFactor;
                });

            verification.Assert(
                "Core cold-growth thresholds remain differentiated under Environment",
                delegate
                {
                    return barley.plant.minGrowthTemperature
                            < soba.plant.minGrowthTemperature
                        && System.Math.Abs(
                            soba.plant.minGrowthTemperature
                            - hie.plant.minGrowthTemperature) < 0.001f
                        && soba.plant.minGrowthTemperature
                            < awa.plant.minGrowthTemperature
                        && System.Math.Abs(
                            awa.plant.minGrowthTemperature
                            - kibi.plant.minGrowthTemperature) < 0.001f;
                });

            Log.Message(
                "[AMJ Core+Environment GameplayContract]" +
                " biome=" +
                    (map == null || map.Biome == null
                        ? "null"
                        : map.Biome.defName) +
                " ladderCells=" + ladderCells +
                " thin=" + thin +
                " gravel=" + gravel +
                " soil=" + soil +
                " rich=" + rich +
                " suitable{soba=" + sobaSuitable +
                ",barley=" + barleySuitable +
                ",wheat=" + wheatSuitable + "}" +
                " fertilityFactor{soba=" + sobaFactor.ToString("F3") +
                ",kibi=" + kibiFactor.ToString("F3") +
                ",awa=" + awaFactor.ToString("F3") +
                ",hie=" + hieFactor.ToString("F3") +
                ",barley=" + barleyFactor.ToString("F3") + "}");
        }

        private static int SuitableNaturalLadderCells(
            ThingDef crop,
            int thin,
            int gravel,
            int soil,
            int rich)
        {
            if (crop == null || crop.plant == null)
            {
                return 0;
            }

            float min = crop.plant.fertilityMin;
            int result = 0;
            if (min <= 0.50f + 0.0001f) result += thin;
            if (min <= 0.70f + 0.0001f) result += gravel;
            if (min <= 1.00f + 0.0001f) result += soil;
            if (min <= 1.40f + 0.0001f) result += rich;
            return result;
        }

        private static float AverageNaturalLadderFertilityFactor(
            ThingDef crop,
            int thin,
            int gravel,
            int soil,
            int rich)
        {
            if (crop == null || crop.plant == null)
            {
                return float.NaN;
            }

            float total = 0f;
            int count = 0;

            AddFertilityTier(crop, 0.50f, thin, ref total, ref count);
            AddFertilityTier(crop, 0.70f, gravel, ref total, ref count);
            AddFertilityTier(crop, 1.00f, soil, ref total, ref count);
            AddFertilityTier(crop, 1.40f, rich, ref total, ref count);

            return count <= 0 ? float.NaN : total / count;
        }

        private static void AddFertilityTier(
            ThingDef crop,
            float fertility,
            int cells,
            ref float total,
            ref int count)
        {
            if (cells <= 0 ||
                crop.plant.fertilityMin > fertility + 0.0001f)
            {
                return;
            }

            float sensitivity = crop.plant.fertilitySensitivity;
            float factor = fertility * sensitivity + (1f - sensitivity);
            total += factor * cells;
            count += cells;
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


    // World distribution checks must never force a biome onto the sample tiles.
    public sealed class AMJWorldWetlandDistributionQuickstart : AbstractQuickstart
    {
        // Independent acceptance inventory: do not derive this from the
        // production exclusion predicate, which could itself regress.
        private static readonly string[] ReplacedVanillaBiomes =
        {
            "TropicalRainforest", "TropicalSwamp", "TemperateForest", "BorealForest",
            "Tundra", "AridShrubland", "Desert", "ExtremeDesert", "IceSheet", "SeaIce"
        };
        private static readonly string[] AmjBands =
        {
            "AMJ_WarmTemperateForest", "AMJ_CoolTemperateForest",
            "AMJ_SubalpineForest", "AMJ_AlpineZone"
        };
        private readonly Dictionary<string, int> biomeCounts = new Dictionary<string, int>();
        private int surfaceTiles;
        private int unassignedSurfaceTiles;
        private int landTiles;
        private int unassignedLandTiles;
        private int wetlandCandidates;
        private int temperateSwampTiles;
        private int coldBogTiles;
        private int wetlandOutsideCandidateTiles;
        private bool selectedNaturalWetland;

        public override TaggedString description
        {
            get { return "Audits biome replacement, retained wetlands and compatible biomes on an unforced AMJ world."; }
        }

        public override int mapSize
        {
            get { return 250; }
        }

        public override float planetCoverage
        {
            get { return 0.30f; }
        }

        public override string seed
        {
            get { return "AMJ-Environment-Terrain-Alpha"; }
        }

        public override void PostApplyConfiguration()
        {
            PlanetTile preferredWetland = PlanetTile.Invalid;
            PlanetTile fallback = PlanetTile.Invalid;

            biomeCounts.Clear();
            surfaceTiles = unassignedSurfaceTiles = landTiles = unassignedLandTiles = 0;
            wetlandCandidates = temperateSwampTiles = coldBogTiles = wetlandOutsideCandidateTiles = 0;
            selectedNaturalWetland = false;

            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                SurfaceTile tile = Find.WorldGrid[i] as SurfaceTile;
                if (tile == null)
                {
                    continue;
                }

                surfaceTiles++;
                string biome = tile.PrimaryBiome == null ? null : tile.PrimaryBiome.defName;
                if (biome == null)
                {
                    unassignedSurfaceTiles++;
                }
                else
                {
                    biomeCounts[biome] = CountBiome(biome) + 1;
                }
                // Include water-covered SeaIce in the exclusion audit, then
                // retain the existing land/wetland distribution measurements.
                if (tile.WaterCovered)
                {
                    continue;
                }

                landTiles++;
                bool candidate = tile.swampiness >= 0.5f;
                if (candidate)
                {
                    wetlandCandidates++;
                }

                if (biome == null)
                {
                    unassignedLandTiles++;
                }

                bool wetland = biome == "TemperateSwamp" || biome == "ColdBog";
                if (biome == "TemperateSwamp")
                {
                    temperateSwampTiles++;
                }
                else if (biome == "ColdBog")
                {
                    coldBogTiles++;
                }

                if (wetland && !candidate)
                {
                    wetlandOutsideCandidateTiles++;
                }

                // Count every tile, but skip redundant settlement queries.
                if (preferredWetland.Valid || (!wetland && fallback.Valid))
                {
                    continue;
                }
                if (!TileFinder.IsValidTileForNewSettlement(tile.tile))
                {
                    continue;
                }

                if (!fallback.Valid)
                {
                    fallback = tile.tile;
                }
                if (wetland && !preferredWetland.Valid)
                {
                    preferredWetland = tile.tile;
                }
            }

            PlanetTile selectedTile = preferredWetland.Valid
                ? preferredWetland
                : fallback;
            if (!selectedTile.Valid)
            {
                throw new InvalidOperationException(
                    "No natural settlement tile found for world wetland audit.");
            }

            selectedNaturalWetland = preferredWetland.Valid;
            Find.GameInitData.startingTile = selectedTile;

            List<string> counts = new List<string>();
            foreach (string name in ReplacedVanillaBiomes)
                counts.Add(name + "=" + CountBiome(name));
            foreach (string name in AmjBands)
                counts.Add(name + "=" + CountBiome(name));
            counts.Add("DankPyon_DarkForest=" + CountBiome("DankPyon_DarkForest"));
            Log.Message("[AMJ Environment NaturalBiomeAudit] | surface=" + surfaceTiles +
                " unassigned=" + unassignedSurfaceTiles + " | " + string.Join(" ", counts.ToArray()));

            int wetlands = temperateSwampTiles + coldBogTiles;
            Log.Message(
                "[AMJ Environment WorldWetlandDistribution]" +
                " | seed=" + seed +
                " planetCoverage=" + planetCoverage.ToString("F2") +
                " land=" + landTiles +
                " wetlandCandidates=" + wetlandCandidates +
                " candidateShare=" + (landTiles > 0
                    ? ((float)wetlandCandidates / landTiles).ToString("P2")
                    : "N/A") +
                " TemperateSwamp=" + temperateSwampTiles +
                " ColdBog=" + coldBogTiles +
                " wetlandShare=" + (landTiles > 0
                    ? ((float)wetlands / landTiles).ToString("P2")
                    : "N/A") +
                " wetlandOfCandidates=" + (wetlandCandidates > 0
                    ? ((float)wetlands / wetlandCandidates).ToString("P2")
                    : "N/A") +
                " unassigned=" + unassignedLandTiles +
                " outsideCandidates=" + wetlandOutsideCandidateTiles +
                " selectedNaturalWetland=" + selectedNaturalWetland);
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification result = new QuickstartVerification();
            int wetlands = temperateSwampTiles + coldBogTiles;
            Map map = Find.CurrentMap;

            result.Assert("large world has at least 2,000 land tiles",
                delegate { return landTiles >= 2000; });
            result.Assert("all natural land tiles have biomes",
                delegate { return unassignedLandTiles == 0 && unassignedSurfaceTiles == 0; });
            result.Assert("natural world has wetland candidates",
                delegate { return wetlandCandidates > 0; });
            result.Assert("natural world has actual wetland biomes",
                delegate { return wetlands > 0; });
            result.Assert("natural wetland biomes stay inside candidate tiles",
                delegate { return wetlandOutsideCandidateTiles == 0; });
            result.Assert("wetland share below provisional 20 percent ceiling",
                delegate { return landTiles > 0 && wetlands <= landTiles * 0.20f; });
            result.Assert("local map uses an unforced natural wetland",
                delegate
                {
                    return selectedNaturalWetland &&
                        map != null &&
                        map.Biome != null &&
                        (map.Biome.defName == "TemperateSwamp" ||
                         map.Biome.defName == "ColdBog");
                });

            foreach (string biome in ReplacedVanillaBiomes)
            {
                string name = biome;
                result.Assert("natural surface has zero " + name + " tiles",
                    delegate { return surfaceTiles > 0 && CountBiome(name) == 0; });
            }
            foreach (string biome in AmjBands)
            {
                string name = biome;
                result.Assert("natural world retains " + name,
                    delegate { return CountBiome(name) > 0; });
            }
            // Availability is required; occurrence of both wetland types on
            // this one seed is not. Native worker scores decide the subtype.
            foreach (string wetland in new[] { "TemperateSwamp", "ColdBog" })
            {
                string name = wetland;
                result.Assert(name + " remains available for natural generation",
                    delegate
                    {
                        BiomeDef def = DefDatabase<BiomeDef>.GetNamedSilentFail(name);
                        return def != null && def.implemented && def.generatesNaturally &&
                            def.Worker.CanPlaceOnLayer(def, Find.WorldGrid.Surface);
                    });
            }
            result.Assert("MO Dark Forest coexists when MO is loaded",
                delegate
                {
                    return ModLister.GetActiveModWithIdentifier("DankPyon.Medieval.Overhaul", true) == null ||
                        CountBiome("DankPyon_DarkForest") > 0;
                });

            return result;
        }

        private int CountBiome(string name)
        {
            int count;
            return biomeCounts.TryGetValue(name, out count) ? count : 0;
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


    // One-off Japan-oriented warm-forest rain-duration/sky observation.
    // Intentionally separate from the accepted vegetation/terrain Quickstart.
    public sealed class AMJWarmRainObservationQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "AMJ_WarmTemperateForest"; }
        }
        protected override string TargetPlantDefName
        {
            get { return "AMJ_Tree_Shii"; }
        }
        private bool observationCompleted;

        // The accepted WarmTemperate terrain Quickstart may select a
        // forced-climate biome above 40°N. This *one-time observation* instead
        // anchors the weather test near Kanto's latitude and annual climate.
        // It never modifies production worldgen or the real installed game.
        private bool japanLatitudeProxyChosen;
        public override void PostApplyConfiguration()
        {
            base.PostApplyConfiguration();
            // Isolated test-only Tokyo normal proxy: grid geometry is selected
            // first; Vanilla/AMJE procedural tile climate is NOT assumed
            // identical to that station's annual normal.
            PlanetTile best = PlanetTile.Invalid;
            float bestScore = float.MaxValue;
            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                SurfaceTile tile = Find.WorldGrid[i] as SurfaceTile;
                if (tile == null || tile.WaterCovered ||
                    tile.hilliness == Hilliness.Impassable ||
                    Find.WorldObjects.AnyWorldObjectAt(tile.tile))
                    continue;
                var coords = Find.WorldGrid.LongLatOf(tile.tile);
                if (coords.y < 32f || coords.y > 38f)
                    continue;
                float score = Math.Abs(coords.y - 35.7f) * 10f;
                if (tile.hilliness == Hilliness.Flat)
                    score -= 2f;
                if (TileFinder.IsValidTileForNewSettlement(tile.tile))
                    score -= 1f;
                if (score < bestScore)
                {
                    bestScore = score;
                    best = tile.tile;
                }
            }
            if (!best.Valid)
            {
                Log.Warning("AMJE_WARM_RAIN_PROXY no non-ocean 32–38N tile on generated 5%-coverage world");
                return;
            }
            var selected = Find.WorldGrid[best] as SurfaceTile;
            string previousBiome = selected.PrimaryBiome == null ?
                "null" : selected.PrimaryBiome.defName;
            float originalRainfall = selected.rainfall;
            float originalTemperature = selected.temperature;
            selected.PrimaryBiome = DefDatabase<BiomeDef>.GetNamed("AMJ_WarmTemperateForest");
            selected.rainfall = 1598.2f;
            selected.temperature = 15.8f;
            if (selected.swampiness >= 0.5f)
                selected.swampiness = 0f;
            Find.GameInitData.startingTile = best;
            japanLatitudeProxyChosen = true;
            var ll = Find.WorldGrid.LongLatOf(best);
            Log.Message("AMJE_WARM_RAIN_PROXY" +
                " lat=" + ll.y.ToString("F2") +
                " lon=" + ll.x.ToString("F2") +
                " rainfall=" + selected.rainfall.ToString("F1") +
                " annualTemp=" + selected.temperature.ToString("F1") +
                " originalRainfall=" + originalRainfall.ToString("F0") +
                " originalTemp=" + originalTemperature.ToString("F1") +
                " originalBiome=" + previousBiome +
                " biomeForcedForIsolatedObservation=True" +
                " annualClimateForcedForIsolatedObservation=True" +
                " station=Tokyo_JMA_1991_2020" +
                " targetLatitude=35.7 rainfallReference=1598.2 tempReference=15.8");
        }
        // This is a bounded NATIVE EVENT-DURATION-STEPPED model, NOT 60 days of
        // actual map ticks. Real 1.6 WeatherDecider chooses weather and samples
        // its live durationRange at every transition; we advance the game clock
        // to the next native weather event boundary and sample live sky glow.
        // Production weather/game-condition ticks, rainfall volume and plant
        // growth over time are deliberately NOT emulated or claimed.
        private static bool SampleNativeElapsedWeatherEvents(Map map)
        {
            if (map == null || map.weatherDecider == null ||
                map.weatherManager == null || Find.TickManager == null)
                return false;
            var durationField = AccessTools.Field(
                typeof(WeatherDecider), "curWeatherDuration");
            var rainBanField = AccessTools.Field(
                typeof(WeatherDecider), "ticksWhenRainAllowedAgain");
            var nextWeather = AccessTools.Method(
                typeof(WeatherDecider), "StartNextWeather");
            WeatherDef clear = DefDatabase<WeatherDef>.GetNamedSilentFail("Clear");
            if (durationField == null || rainBanField == null ||
                nextWeather == null || clear == null)
            {
                Log.Warning("AMJE_WARM_RAIN_ELAPSED unavailable native WeatherDecider reflection");
                return false;
            }

            int initialTicks = Find.TickManager.TicksGame;
            int originalDuration = (int)durationField.GetValue(map.weatherDecider);
            int originalRainBan = (int)rainBanField.GetValue(map.weatherDecider);
            WeatherDef current = map.weatherManager.curWeather;
            WeatherDef previous = map.weatherManager.lastWeather;
            int weatherAge = map.weatherManager.curWeatherAge;
            var longLat = Find.WorldGrid.LongLatOf(map.Tile);
            const int totalTicks = GenDate.DaysPerYear * GenDate.TicksPerDay;
            const int sampleTickStep = GenDate.TicksPerHour * 2;
            bool completed = true;
            try
            {
                for (int seedIndex = 0; seedIndex < 3; seedIndex++)
                {
                    map.weatherManager.curWeather = current;
                    map.weatherManager.lastWeather = previous;
                    map.weatherManager.curWeatherAge = weatherAge;
                    durationField.SetValue(map.weatherDecider, originalDuration);
                    rainBanField.SetValue(map.weatherDecider, originalRainBan);
                    int elapsed = 0;
                    int events = 0;
                    int maxWetRun = 0;
                    int consecutiveWet = 0;
                    double[] wet = new double[3];
                    double[] whole = new double[3];
                    double[] actualSun = new double[3];
                    double[] referenceSun = new double[3];
                    double[] wetByDay = new double[GenDate.DaysPerYear];
                    Rand.PushState(32197 + seedIndex * 7741);
                    try
                    {
                        while (elapsed < totalTicks && events < 512)
                        {
                            Find.TickManager.DebugSetTicksGame(initialTicks + elapsed);
                            // Reuse the established native calendar temperature
                            // refresh; no map-wide or weather game ticks run.
                            Find.World.tileTemperatures.WorldComponentTick();
                            object cached = AccessTools.Method(
                                typeof(TileTemperaturesComp), "RetrieveCachedData")
                                .Invoke(Find.World.tileTemperatures,
                                    new object[] { map.Tile });
                            AccessTools.Method(cached.GetType(), "CheckCache")
                                .Invoke(cached, null);
                            map.mapTemperature.TemperatureUpdate();
                            nextWeather.Invoke(map.weatherDecider, null);
                            events++;
                            int duration = (int)durationField.GetValue(
                                map.weatherDecider);
                            if (duration <= 0)
                            {
                                completed = false;
                                break;
                            }
                            int remaining = Math.Min(duration, totalTicks - elapsed);
                            for (int offset = 0; offset < remaining;
                                 offset += sampleTickStep)
                            {
                                int span = Math.Min(sampleTickStep,
                                    remaining - offset);
                                int midpoint = offset + span / 2;
                                Find.TickManager.DebugSetTicksGame(
                                    initialTicks + elapsed + midpoint);
                                map.weatherManager.curWeatherAge = midpoint;
                                map.skyManager.SkyManagerUpdate();
                                int localDay = GenDate.DayOfYear(
                                    Find.TickManager.TicksAbs, longLat.x);
                                int band = localDay >= 15 && localDay <= 23 ? 0 :
                                    localDay >= 30 && localDay <= 38 ? 1 : 2;
                                bool raining = map.weatherManager.RainRate > 0.1f;
                                whole[band] += span;
                                if (raining)
                                {
                                    wet[band] += span;
                                    wetByDay[localDay] += span;
                                    consecutiveWet += span;
                                    maxWetRun = Math.Max(maxWetRun, consecutiveWet);
                                }
                                else
                                {
                                    consecutiveWet = 0;
                                }
                                float clearGlow = clear.Worker.CurSkyTarget(map).glow;
                                if (clearGlow > 0.05f)
                                {
                                    actualSun[band] += map.skyManager.CurSkyGlow * span;
                                    referenceSun[band] += clearGlow * span;
                                }
                            }
                            elapsed += remaining;
                            map.weatherManager.curWeatherAge = duration + 1;
                        }
                    }
                    finally
                    {
                        Rand.PopState();
                    }
                    completed &= elapsed == totalTicks &&
                        events > 2 && events < 512 &&
                        whole[0] > 0 && whole[1] > 0 && whole[2] > 0 &&
                        referenceSun[0] > 0 && referenceSun[1] > 0;
                    int[] rainDays = new int[3];
                    for (int day = 0; day < GenDate.DaysPerYear; day++)
                    {
                        int band = day >= 15 && day <= 23 ? 0 :
                            day >= 30 && day <= 38 ? 1 : 2;
                        // >=6 hours of weather rain is a game-only comparison
                        // threshold, NOT JMA >=1mm precipitation per day.
                        if (wetByDay[day] >= 6 * GenDate.TicksPerHour)
                            rainDays[band]++;
                    }
                    Log.Message("AMJE_WARM_RAIN_ELAPSED" +
                        " method=native_event_duration_stepped_not_full_map_ticks" +
                        " seed=" + seedIndex + " biome=" + map.Biome.defName +
                        " lat=" + longLat.y.ToString("F1") +
                        " events=" + events +
                        " elapsedDays=" + (elapsed / (float)GenDate.TicksPerDay).ToString("F2") +
                        " tsuyuWetHours=" + (wet[0] / GenDate.TicksPerHour).ToString("F1") +
                        " tsuyuWetDays6h=" + rainDays[0] + "/9" +
                        " tsuyuDaylightRatio=" + (actualSun[0] / Math.Max(1, referenceSun[0])).ToString("F3") +
                        " akisameWetHours=" + (wet[1] / GenDate.TicksPerHour).ToString("F1") +
                        " akisameWetDays6h=" + rainDays[1] + "/9" +
                        " akisameDaylightRatio=" + (actualSun[1] / Math.Max(1, referenceSun[1])).ToString("F3") +
                        " otherWetHours=" + (wet[2] / GenDate.TicksPerHour).ToString("F1") +
                        " otherWetDays6h=" + rainDays[2] + "/42" +
                        " otherDaylightRatio=" + (actualSun[2] / Math.Max(1, referenceSun[2])).ToString("F3") +
                        " longestWetHoursApprox=" +
                        (maxWetRun / (float)GenDate.TicksPerHour).ToString("F1"));
                    Log.Message("AMJE_WARM_RAIN_ELAPSED_DAYS seed=" + seedIndex +
                        " wetHoursByLocalDay=" +
                        string.Join(",", wetByDay.Select(v =>
                            (v / GenDate.TicksPerHour).ToString("F1")).ToArray()));
                }
            }
            finally
            {
                Find.TickManager.DebugSetTicksGame(initialTicks);
                map.weatherManager.curWeather = current;
                map.weatherManager.lastWeather = previous;
                map.weatherManager.curWeatherAge = weatherAge;
                durationField.SetValue(map.weatherDecider, originalDuration);
                rainBanField.SetValue(map.weatherDecider, originalRainBan);
                Find.World.tileTemperatures.WorldComponentTick();
                map.mapTemperature.TemperatureUpdate();
                map.skyManager.SkyManagerUpdate();
            }
            return completed;
        }


        public override void PostLoaded()
        {
            base.PostLoaded();
            Map map = Find.CurrentMap;
            if (map == null) return;
            var coordinate = Find.WorldGrid.LongLatOf(map.Tile);
            var tile = map.TileInfo;
            Log.Message("AMJE_WARM_RAIN_TARGET" +
                " biome=" + map.Biome.defName +
                " lat=" + coordinate.y.ToString("F2") +
                " lon=" + coordinate.x.ToString("F2") +
                " annualRainfallMm=" + tile.rainfall.ToString("F0") +
                " annualTempC=" + tile.temperature.ToString("F1") +
                " KantoLatitudeProxy=" + (coordinate.y >= 30f && coordinate.y <= 40f) +
                " weatherSampling=native_event_duration_stepped_not_full_map_ticks");
            observationCompleted = japanLatitudeProxyChosen &&
                map.Biome.defName == "AMJ_WarmTemperateForest" &&
                SampleNativeElapsedWeatherEvents(map);
        }
        public override QuickstartVerification Verify()
        {
            QuickstartVerification result = new QuickstartVerification();
            result.Assert("warm temperate weather duration observation completed",
                delegate { return observationCompleted; });
            return result;
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
                    "Plant_TreeMaple",
                    "Plant_TreeBamboo"
                };
            }
        }

        protected override string[] ExcludedPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_TreePoplar",
                    "Plant_TreeOak"
                };
            }
        }
        private static void AssertApprovedVanillaTreeDescription(
            QuickstartVerification verification, string defName,
            string approvedEnglishLabel, string approvedJapaneseLabel,
            string approvedEnglish, string approvedJapanese)
        {
            verification.Assert(defName + " has approved EN/JA description", delegate
            {
                ThingDef tree = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
                if (tree == null || string.IsNullOrEmpty(tree.description))
                    return false;
                bool labelMatches =
                    string.Equals(tree.label, approvedEnglishLabel, StringComparison.Ordinal) ||
                    string.Equals(tree.label, approvedJapaneseLabel, StringComparison.Ordinal);
                if (!labelMatches)
                    return false;
                string actual = tree.description.Replace("\\n", "\n")
                    .Replace("\r\n", "\n").Trim();
                return string.Equals(actual, approvedEnglish, StringComparison.Ordinal) ||
                    string.Equals(actual, approvedJapanese, StringComparison.Ordinal);
            });
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification result = base.Verify();
            result.Assert("Plant_TreeBamboo has approved EN/JA description", delegate
            {
                ThingDef bamboo = DefDatabase<ThingDef>.GetNamedSilentFail("Plant_TreeBamboo");
                if (bamboo == null || string.IsNullOrEmpty(bamboo.description))
                    return false;
                bool labelMatches =
                    string.Equals(bamboo.label, "Bamboo (take)", StringComparison.Ordinal) ||
                    string.Equals(bamboo.label, "竹", StringComparison.Ordinal);
                if (!labelMatches)
                    return false;
                string actual = bamboo.description.Replace("\\n", "\n")
                    .Replace("\r\n", "\n").Trim();
                string approvedEnglish = "Bamboo (take) is a member of the grass family that grows in stands in Japan's warm, humid regions, despite its tree-like appearance. Here it represents bamboo types such as madake and hachiku that existed in ancient and medieval Japan." + "\n\n" + "Lightweight and flexible, bamboo has long been used for baskets, everyday objects, and materials for building, farming, and fishing. Moso bamboo, common in Japan today, is recorded as having arrived during the Edo period and is distinguished here from these earlier bamboo types.";
                string approvedJapanese = "竹（タケ）は、日本の温暖で湿潤な土地に群生する、木のような姿のイネ科植物。古代・中世に存在したマダケやハチクなどの竹類を、ここではまとめて表す。" + "\n\n" + "竹は軽くしなやかで、古くからかご・日用品や建築・農漁業の資材に使われてきた。現在広く見られるモウソウチクは江戸時代の渡来とする記録があり、ここで想定する竹類とは区別する。";
                return string.Equals(actual, approvedEnglish, StringComparison.Ordinal) ||
                    string.Equals(actual, approvedJapanese, StringComparison.Ordinal);
            });
            AssertApprovedVanillaTreeDescription(result, "Plant_TreeMaple", "Maple (kaede)", "カエデ",
                "Maple (kaede) refers to trees of the maple genus found in Japan's deciduous broadleaf forests. Many change leaf color with the seasons." + "\n\n" + "Forests containing maples have been identified near Jomon-period archaeological sites, showing that these trees have long been part of the Japanese archipelago's forests. This plant does not represent any one maple species.",
                "楓（カエデ）は、日本の落葉広葉樹林にも生育するカエデ属の樹木をまとめた呼び名。多くは季節によって葉の色を変える。" + "\n\n" + "縄文時代の遺跡周辺にもカエデ属を含む森林が確認されており、古くから日本列島の森林を構成していた。この植物は特定のカエデ一種を再現するものではない。");
            AssertApprovedVanillaTreeDescription(result, "Plant_TreeOak", "Oak (nara)", "ナラ",
                "Oak (nara) represents deciduous Japanese oaks such as konara. These trees form part of deciduous broadleaf forests alongside Japanese beech and other species." + "\n\n" + "Forests containing oaks have existed across the Japanese archipelago since ancient times, and regional patterns of fire use and forest management have influenced them. Here this plant represents deciduous oaks, not evergreen kashi oaks.",
                "楢（ナラ）は、コナラなど日本に分布する落葉性のナラ類を表す樹木。ブナなどとともに落葉広葉樹林の一部をなす。" + "\n\n" + "日本列島では古くからナラ類を含む森林が広がり、地域ごとに人の火の利用や森林利用の影響を受けてきた。ここでは落葉ナラ類の代理とし、常緑カシ類まで同一視するものではない。");
            AssertApprovedVanillaTreeDescription(result, "Plant_TreeBirch", "Birch (kaba)", "カバノキ",
                "Birch (kaba, Japanese birches) refers to deciduous tall trees also found in cool mountain regions. In the subalpine zone, species such as dakekanba grow on slopes adjoining conifer forests." + "\n\n" + "This plant represents birches of cool-temperate forests as well as trees corresponding to dakekanba in the subalpine zone. The actual species vary by region and elevation.",
                "樺（カバ。カバノキ類）は、冷涼な山地にも分布する落葉高木の仲間。亜高山帯にはダケカンバなどが、針葉樹林に接する斜面にも見られる。" + "\n\n" + "この植物は冷温帯のカバノキ類と、亜高山帯のダケカンバに相当する樹木をまとめて表す。実際の種類は地域や標高によって異なる。");
            AssertApprovedVanillaTreeDescription(result, "Plant_TreePine", "Pine (matsu)", "マツ",
                "Pine (matsu) is a type of conifer that also grows in Japan's mountains and on nutrient-poor soils. Here it represents pines such as akamatsu (Japanese red pine)." + "\n\n" + "Pollen analysis suggests that, from ancient through medieval times, pines increased in some regions as trees were felled and wood was gathered for fuel. This does not mean that pine forests covered the Japanese archipelago uniformly.",
                "松（マツ）は、日本の山地や痩せた土地にも育つ針葉樹の仲間。ここではアカマツなどのマツ類を代表する樹木として扱う。" + "\n\n" + "古代から中世にかけて、地域によっては伐採や燃料利用に伴ってマツ類が増加したことが花粉分析からうかがえる。全国の森林が一律にマツ林だったわけではない。");
            AssertApprovedVanillaTreeDescription(result, "Plant_TreeWillow", "Willow (yanagi)", "ヤナギ",
                "Willow (yanagi) represents willows that grow along rivers and on wet ground. Along Japanese riverbanks, willow woodlands occur where flooding and running water shape the landscape." + "\n\n" + "Riverside wetland woodlands are also an important natural feature of the lowland landscapes adjoining settlements. This plant does not represent any single willow species.",
                "柳（ヤナギ）は、川沿いや湿った地面に育つヤナギ類を表す樹木。日本各地の川原では、氾濫や流水の影響を受ける場所にヤナギ林が見られる。" + "\n\n" + "河川沿いの湿地林は、集落が接する低地景観を理解するうえでも重要な自然環境である。この植物は特定のヤナギ一種を表すものではない。");
            return result;
        }
    }

    public sealed class AMJTemperateSwampVegetationQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "TemperateSwamp"; }
        }

        protected override string TargetPlantDefName
        {
            get { return "Plant_TallGrass"; }
        }

        protected override string[] SecondaryPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_TreeWillow",
                    "Plant_TreeMaple",
                    "Plant_Brambles"
                };
            }
        }

        protected override string[] ExcludedPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_Chokevine",
                    "Plant_TreeCypress"
                };
            }
        }

        protected override bool ValidateAmjBiomeContracts
        {
            get { return false; }
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification result = base.Verify();
            BiomeDef biome = Find.CurrentMap == null ? null : Find.CurrentMap.Biome;
            string[] names =
            {
                "Plant_TallGrass",
                "AMJ_Plant_Yoshi",
                "AMJ_Plant_Suge",
                "Plant_Brambles",
                "Plant_Bush",
                "Plant_TreeWillow",
                "Plant_TreeMaple",
                "Plant_Berry",
                "Plant_HealrootWild"
            };
            float[] expected = { 1.40f, 1.00f, 0.80f, 0.8f, 0.2f, 1.30f, 1.0f, 0.05f, 0.05f };
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                float value = expected[i];
                result.Assert("TemperateSwamp " + name + " commonality=" + value, delegate
                {
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                    return biome != null && def != null &&
                        System.Math.Abs(biome.CommonalityOfPlant(def) - value) < 0.001f;
                });
            }
            AddWetlandEcologyAssertions(result, Find.CurrentMap, false);
            AddWetlandBehaviorProbeAssertions(result, Find.CurrentMap);
            AddHannokiBehaviorProbeAssertions(result, Find.CurrentMap);
            AddHannokiProductionAssertions(result, Find.CurrentMap, 0.70f);
            AddWetlandProductionHerbAssertions(result, Find.CurrentMap, false);
            return result;
        }
    }

    public sealed class AMJColdBogVegetationQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "ColdBog"; }
        }

        protected override string TargetPlantDefName
        {
            get { return "Plant_TallGrass"; }
        }

        protected override string[] SecondaryPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_Moss",
                    "Plant_TreeWillow",
                    "Plant_TreeBirch",
                    "Plant_TreeMaple"
                };
            }
        }

        protected override string[] ExcludedPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_Chokevine",
                    "Plant_TreeCypress",
                    "Plant_Astragalus"
                };
            }
        }

        protected override bool ValidateAmjBiomeContracts
        {
            get { return false; }
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification result = base.Verify();
            BiomeDef biome = Find.CurrentMap == null ? null : Find.CurrentMap.Biome;
            string[] names =
            {
                "Plant_TallGrass",
                "AMJ_Plant_Yoshi",
                "AMJ_Plant_Suge",
                "Plant_Moss",
                "AMJ_Plant_Mizugoke",
                "Plant_Bush",
                "Plant_TreeWillow",
                "Plant_TreeBirch",
                "Plant_TreeMaple",
                "Plant_Berry",
                "Plant_HealrootWild"
            };
            float[] expected = { 1.70f, 0.60f, 1.10f, 1.40f, 1.20f, 0.3f, 0.45f, 0.45f, 0.45f, 0.07f, 0.05f };
            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                float value = expected[i];
                result.Assert("ColdBog " + name + " commonality=" + value, delegate
                {
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                    return biome != null && def != null &&
                        System.Math.Abs(biome.CommonalityOfPlant(def) - value) < 0.001f;
                });
            }
            AddWetlandEcologyAssertions(result, Find.CurrentMap, true);
            AddWetlandBehaviorProbeAssertions(result, Find.CurrentMap);
            AddHannokiBehaviorProbeAssertions(result, Find.CurrentMap);
            AddHannokiProductionAssertions(result, Find.CurrentMap, 0.45f);
            AddWetlandProductionHerbAssertions(result, Find.CurrentMap, true);
            return result;
        }
    }

    public sealed class AMJPlantGrowthReviewQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName { get { return "AMJ_CoolTemperateForest"; } }
        protected override string TargetPlantDefName { get { return "AMJ_Tree_Beech"; } }

        public override void PostLoaded()
        {
            base.PostLoaded();
            Map map = Find.CurrentMap;
            IntVec3 center = map.Center;
            string[] names = { "AMJ_Tree_Shii", "AMJ_Tree_Beech", "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu" };
            bool snowReview = System.Environment.GetEnvironmentVariable("RIMWORLD_AMJE_SNOW_REVIEW") == "1";
            bool beechSnowReview = snowReview && System.Environment.GetEnvironmentVariable("RIMWORLD_AMJE_BEECH_SNOW_REVIEW") == "1";
            if (beechSnowReview) names = new string[] { "AMJ_Tree_Beech", "AMJ_Tree_Beech", "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu" };
            float[] growths = snowReview ? new float[] { 1f, 1f, 1f } : new float[] { 0.1f, 0.5f, 1f };
            foreach (IntVec3 cell in CellRect.CenteredOn(center, 12))
            {
                if (!cell.InBounds(map)) continue;
                map.roofGrid.SetRoof(cell, null);
                foreach (Thing thing in cell.GetThingList(map).ToArray())
                    if (thing is Plant || thing.def.category == ThingCategory.Building) thing.Destroy(DestroyMode.Vanish);
                map.terrainGrid.SetTerrain(cell, TerrainDefOf.Soil);
            }
            for (int row = 0; row < names.Length; row++)
                for (int column = 0; column < growths.Length; column++)
                {
                    Plant plant = (Plant)ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed(names[row]));
                    plant.Growth = growths[column];
                    IntVec3 position = center + new IntVec3((column - 1) * 5, 0, 8 - row * 5);
                    GenSpawn.Spawn(plant, position, map);
                    if (beechSnowReview && row == 1) plant.MakeLeafless(Plant.LeaflessCause.Cold, false);
                    float snow = snowReview ? column * 0.5f : 0f;
                    foreach (IntVec3 snowCell in CellRect.CenteredOn(position, 1))
                        map.snowGrid.SetDepth(snowCell, snow);
                    UnityEngine.Material material = plant.Graphic.MatAt(plant.Rotation, plant);
                    bool valid = material != null && material.mainTexture != null && material.mainTexture != BaseContent.BadTex;
                    if (snowReview && (names[row] == "AMJ_Shrub_Haimatsu" || names[row] == "AMJ_Tree_Shirabiso" || names[row] == "AMJ_Tree_Shii" || names[row] == "AMJ_Tree_Beech"))
                    {
                        Graphic overlay = plant.SnowOverlayGraphic;
                        UnityEngine.Material snowMaterial = overlay == null ? null : overlay.MatSingleFor(plant);
                        bool snowValid = snowMaterial != null && snowMaterial.mainTexture != null &&
                            snowMaterial.mainTexture != BaseContent.BadTex;
                        Log.Message("[AMJ Environment SnowReview] plant=" + names[row] + " growth=" + plant.Growth +
                            " position=" + position + " leafless=" + plant.LeaflessNow + " snowDepth=" + map.snowGrid.GetDepth(position) +
                            " snowTexture=" + (snowValid ? snowMaterial.mainTexture.name : "<bad>") + " valid=" + snowValid);
                        if (!snowValid) Log.Error("[AMJ Environment SnowReview] " + names[row] + " snow overlay missing.");
                    }
                    Log.Message("[AMJ Environment GrowthReview] plant=" + names[row] + " growth=" + plant.Growth +
                        " position=" + position + " leafless=" + plant.LeaflessNow + " snowDepth=" + map.snowGrid.GetDepth(position) +
                        " texture=" + (valid ? material.mainTexture.name : "<bad>") + " valid=" + valid);
                    if (!valid) Log.Error("[AMJ Environment GrowthReview] Missing growth-state material.");
                }
            float longitude = Find.WorldGrid.LongLatOf(map.Tile).x;
            long local = Find.TickManager.TicksAbs + GenDate.LocalTicksOffsetFromLongitude(longitude);
            int delta = (int)(GenDate.TicksPerDay / 2 - ((local % GenDate.TicksPerDay) + GenDate.TicksPerDay) % GenDate.TicksPerDay);
            Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + delta);
            WeatherDef clear = DefDatabase<WeatherDef>.GetNamed("Clear");
            map.weatherManager.TransitionTo(clear);
            map.weatherManager.lastWeather = clear;
            map.weatherManager.curWeatherAge = 10000;
            AccessTools.Field(typeof(PlantFallColors), "FallIntensityOverride").SetValue(null, true);
            AccessTools.Field(typeof(PlantFallColors), "FallIntensity").SetValue(null, 0f);
            PlantFallColors.SetFallShaderGlobals(map);
            Find.TickManager.Pause();
            CameraJumper.TryJump(center, map);
            AccessTools.Field(typeof(CameraDriver), "rootSize").SetValue(Find.CameraDriver, 18f);
            map.skyManager.SkyManagerUpdate();
            float hour = GenDate.HourFloat(Find.TickManager.TicksAbs, longitude);
            bool baseline = System.Math.Abs(hour - 12f) < 0.001f && map.weatherManager.curWeather == clear &&
                map.weatherManager.TransitionLerpFactor >= 1f && Find.TickManager.Paused;
            Log.Message("[AMJ Environment GrowthReview] baseline localHour=" + hour + " weather=" +
                map.weatherManager.curWeather.defName + " transition=" + map.weatherManager.TransitionLerpFactor +
                " paused=" + Find.TickManager.Paused + " zoom=18 fallIntensity=0 verified=" + baseline);
            if (!baseline) Log.Error("[AMJ Environment GrowthReview] Review baseline failed.");
            if (System.Environment.GetEnvironmentVariable("RIMWORLD_AMJE_HAIMATSU_INFO_REVIEW") == "1")
            {
                ThingDef haimatsu = DefDatabase<ThingDef>.GetNamed("AMJ_Shrub_Haimatsu");
                float scale = GenUI.IconDrawScale(haimatsu);
                bool valid = haimatsu.uiIcon != null && haimatsu.uiIcon != BaseContent.BadTex;
                Log.Message("[AMJ Environment InfoReview] icon=" + (valid ? haimatsu.uiIcon.name : "<bad>") +
                    " scale=" + scale + " mapDrawSize=" + haimatsu.graphicData.drawSize +
                    " visualRange=" + haimatsu.plant.visualSizeRange + " valid=" + valid);
                if (!valid || Math.Abs(scale - 1f) > 0.001f || Math.Abs(haimatsu.graphicData.drawSize.x - 2.60f) > 0.001f ||
                    Math.Abs(haimatsu.plant.visualSizeRange.min - 0.45f) > 0.001f || Math.Abs(haimatsu.plant.visualSizeRange.max - 0.75f) > 0.001f)
                    Log.Error("[AMJ Environment InfoReview] Haimatsu UI/map scale regression.");
                var specimens = map.listerThings.ThingsOfDef(haimatsu);
                Thing actual = specimens.Count > 0 ? specimens[specimens.Count - 1] : null;
                Find.WindowStack.Add(actual != null ? new Dialog_InfoCard(actual) : new Dialog_InfoCard(haimatsu));
            }

            if (System.Environment.GetEnvironmentVariable("RIMWORLD_AMJE_ICON_REVIEW") == "1")
                Find.WindowStack.Add(new AMJPlantIconReviewWindow());
        }
    }


    public sealed class AMJPlantIconReviewWindow : Window
    {
        private readonly ThingDef[] plants;
        public override UnityEngine.Vector2 InitialSize { get { return new UnityEngine.Vector2(570f, 265f); } }
        public AMJPlantIconReviewWindow()
        {
            doCloseX = true;
            doCloseButton = true;
            forcePause = true;
            absorbInputAroundWindow = false;
            string[] names = { "AMJ_Tree_Shii", "AMJ_Tree_Beech", "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu" };
            plants = new ThingDef[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                plants[i] = DefDatabase<ThingDef>.GetNamed(names[i]);
                bool valid = plants[i].uiIcon != null && plants[i].uiIcon != BaseContent.BadTex;
                Log.Message("[AMJ Environment IconReview] plant=" + names[i] + " icon=" +
                    (valid ? plants[i].uiIcon.name : "<bad>") + " valid=" + valid);
                if (!valid) Log.Error("[AMJ Environment IconReview] Missing icon " + names[i]);
            }
        }
        public override void DoWindowContents(UnityEngine.Rect inRect)
        {
            Widgets.Label(new UnityEngine.Rect(0f, 0f, 510f, 28f), "植物アイコン確認（小・大）");
            for (int i = 0; i < plants.Length; i++)
            {
                float x = i * 132f;
                Widgets.Label(new UnityEngine.Rect(x, 32f, 130f, 28f), plants[i].LabelCap);
                Widgets.DefIcon(new UnityEngine.Rect(x + 10f, 85f, 32f, 32f), plants[i]);
                Widgets.DefIcon(new UnityEngine.Rect(x + 54f, 68f, 64f, 64f), plants[i]);
            }
        }
    }

    public sealed class AMJBeechSeasonalSampleQuickstart : BiomeTerrainQuickstartBase
    {
        private bool sawLeafless;
        private bool sawRecovery;
        private bool validTextures = true;
        private float minFall = 1f;
        private float maxFall;
        protected override string TargetBiomeDefName { get { return "AMJ_CoolTemperateForest"; } }
        protected override string TargetPlantDefName { get { return "AMJ_Tree_Beech"; } }

        public override void PostApplyConfiguration()
        {
            base.PostApplyConfiguration();
            SurfaceTile coldest = null;
            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                SurfaceTile tile = Find.WorldGrid[i] as SurfaceTile;
                if (tile == null || tile.PrimaryBiome.defName != TargetBiomeDefName ||
                    !TileFinder.IsValidTileForNewSettlement(tile.tile)) continue;
                if (coldest == null || tile.temperature < coldest.temperature) coldest = tile;
            }
            if (coldest != null) Find.GameInitData.startingTile = coldest.tile;
            Log.Message("[AMJ Environment SeasonSample] natural coldest biome tile=" +
                Find.GameInitData.startingTile + " annualTemp=" +
                Find.WorldGrid[Find.GameInitData.startingTile].temperature);
        }

        public override void PostLoaded()
        {
            base.PostLoaded();
            Map map = Find.CurrentMap;
            Plant selected = null;
            float selectedThreshold = float.MinValue;
            System.Reflection.PropertyInfo leaflessThresholdProperty =
                AccessTools.Property(typeof(Plant), "LeaflessTemperatureThresh");
            foreach (Thing thing in map.listerThings.AllThings)
            {
                Plant plant = thing as Plant;
                if (plant != null && plant.def.defName == TargetPlantDefName &&
                    !map.roofGrid.Roofed(plant.Position) &&
                    plant.Growth >= 0.95f && !plant.Dying)
                {
                    // Individual native thresholds span -18..-10 C. Pick the
                    // most cold-sensitive existing tree for observable state
                    // coverage; do not change its threshold, climate or seed.
                    float candidateThreshold = (float)leaflessThresholdProperty.GetValue(plant, null);
                    if (selected == null || candidateThreshold > selectedThreshold ||
                        (candidateThreshold == selectedThreshold && plant.Age < selected.Age))
                    {
                        selected = plant;
                        selectedThreshold = candidateThreshold;
                    }
                }
            }
            if (selected == null) { Log.Error("[AMJ Environment SeasonSample] No outdoor beech."); return; }
            AccessTools.Field(typeof(PlantFallColors), "FallIntensityOverride").SetValue(null, false);
            UnityEngine.Vector2 longLat = Find.WorldGrid.LongLatOf(map.Tile);
            int start = Find.TickManager.TicksGame;
            bool previous = selected.LeaflessNow;
            float threshold = (float)AccessTools.Property(typeof(Plant), "LeaflessTemperatureThresh").GetValue(selected, null);
            float minTemperature = float.MaxValue;
            Log.Message("[AMJ Environment SeasonSample] start thingID=" + selected.thingIDNumber +
                " pos=" + selected.Position + " latitude=" + longLat.y + " leaflessThreshold=" + threshold +
                " method=hourly-calendar-sampling-plus-vanilla-TickLong; not full-world tick simulation");
            for (int step = 0; step <= 1440; step++)
            {
                Find.TickManager.DebugSetTicksGame(start + step * GenDate.TicksPerHour);
                Find.World.tileTemperatures.WorldComponentTick();
                object temperatureData = AccessTools.Method(typeof(TileTemperaturesComp),
                    "RetrieveCachedData").Invoke(Find.World.tileTemperatures, new object[] { map.Tile });
                AccessTools.Method(temperatureData.GetType(), "CheckCache").Invoke(temperatureData, null);
                map.mapTemperature.TemperatureUpdate();
                Room room = selected.GetRoom();
                if (room != null)
                {
                    object tracker = AccessTools.Field(typeof(Room), "tempTracker").GetValue(room);
                    AccessTools.Method(tracker.GetType(), "EqualizeTemperature").Invoke(tracker, null);
                }
                map.skyManager.SkyManagerUpdate();
                selected.TickLong();
                minTemperature = System.Math.Min(minTemperature, selected.AmbientTemperature);
                if (selected.Destroyed) { Log.Error("[AMJ Environment SeasonSample] Tracked tree destroyed."); break; }
                int day = GenDate.DayOfYear(Find.TickManager.TicksAbs, longLat.x);
                float factor = PlantFallColors.GetFallColorFactor(longLat.y, day);
                minFall = System.Math.Min(minFall, factor);
                maxFall = System.Math.Max(maxFall, factor);
                bool leafless = selected.LeaflessNow;
                if (leafless) sawLeafless = true;
                if (sawLeafless && previous && !leafless) sawRecovery = true;
                UnityEngine.Material material = selected.Graphic.MatAt(selected.Rotation, selected);
                bool valid = material != null && material.mainTexture != null && material.mainTexture != BaseContent.BadTex;
                validTextures &= valid;
                if (step % 24 == 0 || leafless != previous)
                    Log.Message("[AMJ Environment SeasonSample] step=" + step + " day=" + day +
                        " ticksAbs=" + Find.TickManager.TicksAbs + " temp=" + selected.AmbientTemperature.ToString("F2") +
                        " outdoor=" + map.mapTemperature.OutdoorTemp.ToString("F2") +
                        " fallFactor=" + factor.ToString("F3") + " leafless=" + leafless +
                        " texture=" + (valid ? material.mainTexture.name : "<bad>"));
                previous = leafless;
            }
            Log.Message("[AMJ Environment SeasonSample] result minFall=" + minFall + " maxFall=" + maxFall +
                " leafless=" + sawLeafless + " recovered=" + sawRecovery + " validTextures=" + validTextures +
                " minimumTemperature=" + minTemperature + " leaflessThreshold=" + threshold);
            // Restore the scheduler's timeline after accelerated calendar sampling.
            Find.TickManager.DebugSetTicksGame(start);
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification result = base.Verify();
            result.Assert("natural calendar fall factor changes without override", delegate { return minFall < 0.1f && maxFall > 0.9f; });
            result.Assert("same beech naturally becomes leafless", delegate { return sawLeafless; });
            result.Assert("same beech naturally recovers leaves", delegate { return sawRecovery; });
            result.Assert("sampled live state textures are valid", delegate { return validTextures; });
            return result;
        }
    }

    public sealed class AMJCoolTemperateTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        public override void PostLoaded()
        {
            base.PostLoaded();
            string mode = System.Environment.GetEnvironmentVariable(
                "RIMWORLD_AMJE_BEECH_REVIEW");
            if (mode != "leafless" && mode != "autumn") return;
            Map map = Find.CurrentMap;
            if (map == null) return;
            Plant selected = null;
            int count = 0;
            foreach (Thing thing in map.listerThings.AllThings)
            {
                Plant plant = thing as Plant;
                if (plant == null || plant.Destroyed ||
                    plant.def.defName != "AMJ_Tree_Beech") continue;
                count++;
                if (selected == null || plant.Growth > selected.Growth)
                    selected = plant;
            }
            if (selected == null)
            {
                Log.Error("[AMJ Environment BeechReview] No naturally generated beech found.");
                return;
            }
            bool before = selected.LeaflessNow;
            if (mode == "leafless")
                selected.MakeLeafless(Plant.LeaflessCause.Cold, false);
            else
            {
                System.Reflection.FieldInfo enabled = AccessTools.Field(
                    typeof(PlantFallColors), "FallIntensityOverride");
                System.Reflection.FieldInfo intensity = AccessTools.Field(
                    typeof(PlantFallColors), "FallIntensity");
                if (enabled == null || intensity == null)
                {
                    Log.Error("[AMJ Environment BeechReview] Native fall controls unavailable.");
                    return;
                }
                enabled.SetValue(null, true);
                intensity.SetValue(null, 1f);
                PlantFallColors.SetFallShaderGlobals(map);
            }
            CameraJumper.TryJumpAndSelect(selected);
            if (System.Environment.GetEnvironmentVariable("RIMWORLD_AMJE_COLOR_BASELINE") == "1")
            {
                float longitude = Find.WorldGrid.LongLatOf(map.Tile).x;
                long localTicks = Find.TickManager.TicksAbs +
                    GenDate.LocalTicksOffsetFromLongitude(longitude);
                long withinDay = ((localTicks % GenDate.TicksPerDay) +
                    GenDate.TicksPerDay) % GenDate.TicksPerDay;
                int delta = (int)(GenDate.TicksPerDay / 2 - withinDay);
                Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + delta);
                WeatherDef clear = DefDatabase<WeatherDef>.GetNamed("Clear");
                map.weatherManager.TransitionTo(clear);
                map.weatherManager.lastWeather = clear;
                map.weatherManager.curWeatherAge = 10000;
                Find.TickManager.Pause();
                AccessTools.Field(typeof(CameraDriver), "rootSize").SetValue(
                    Find.CameraDriver, 24f);
                map.skyManager.SkyManagerUpdate();
                float actualHour = GenDate.HourFloat(Find.TickManager.TicksAbs, longitude);
                float transition = map.weatherManager.TransitionLerpFactor;
                bool baselineValid = System.Math.Abs(actualHour - 12f) < 0.001f &&
                    map.weatherManager.curWeather == clear &&
                    map.weatherManager.lastWeather == clear &&
                    transition >= 1f && Find.TickManager.Paused;
                Log.Message("[AMJ Environment ColorBaseline] localHour=" +
                    actualHour.ToString("F3") + " weather=" + map.weatherManager.curWeather.defName +
                    " transition=" + transition + " paused=" + Find.TickManager.Paused +
                    " ticksAbs=" + Find.TickManager.TicksAbs + " longitude=" + longitude +
                    " zoom=24 fallIntensity=" + (mode == "autumn" ? "1" : "natural") +
                    " growth=" + selected.Growth + " verified=" + baselineValid);
                if (!baselineValid)
                    Log.Error("[AMJ Environment ColorBaseline] Fixed review conditions failed.");
            }
            UnityEngine.Material material = selected.Graphic.MatAt(selected.Rotation, selected);
            bool valid = material != null && material.mainTexture != null &&
                material.mainTexture != BaseContent.BadTex;
            Log.Message("[AMJ Environment BeechReview] mode=forced-" + mode + " count=" +
                count + " selected=" + selected.Position + " growth=" + selected.Growth +
                " beforeLeafless=" + before + " afterLeafless=" + selected.LeaflessNow +
                " texture=" + (material == null || material.mainTexture == null ?
                    "<null>" : material.mainTexture.name) + " valid=" + valid);
            if (selected.LeaflessNow != (mode == "leafless") || !valid)
                Log.Error("[AMJ Environment BeechReview] Review state/texture check failed.");
            Messages.Message("Beech " + mode + " appearance review: selected " +
                selected.Position + ". Seasonal transition remains a separate check.",
                MessageTypeDefOf.NeutralEvent, false);
        }

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
                    "Plant_TreeBirch"
                };
            }
        }

        protected override string[] ExcludedPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_TreePine"
                };
            }
        }
    }

    public sealed class AMJAlpineTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        public override void PostLoaded()
        {
            base.PostLoaded();
            if (System.Environment.GetEnvironmentVariable(
                "RIMWORLD_AMJE_TEXTURE_REVIEW") != "1")
            {
                return;
            }

            Map map = Find.CurrentMap;
            if (map == null) return;
            Plant selected = null;
            int count = 0;
            foreach (Thing thing in map.listerThings.AllThings)
            {
                Plant plant = thing as Plant;
                if (plant == null || plant.Destroyed ||
                    plant.def.defName != "AMJ_Shrub_Haimatsu") continue;
                count++;
                if (selected == null || plant.Growth > selected.Growth)
                    selected = plant;
            }
            if (selected == null)
            {
                Log.Error("[AMJ Environment TextureReview] No naturally generated Haimatsu found.");
                return;
            }
            CameraJumper.TryJumpAndSelect(selected);
            Log.Message("[AMJ Environment TextureReview] Haimatsu count=" + count +
                " selected=" + selected.Position + " growth=" + selected.Growth);
            Messages.Message("Haimatsu: " + count + " naturally generated; selected " +
                selected.Position, MessageTypeDefOf.NeutralEvent, false);
        }

        protected override string TargetBiomeDefName
        {
            get { return "AMJ_AlpineZone"; }
        }

        protected override string TargetPlantDefName
        {
            get { return "AMJ_Shrub_Haimatsu"; }
        }

        protected override float MaxTargetCellFraction
        {
            get { return 0.05f; }
        }

        protected override string[] ExcludedPlantDefNames
        {
            get
            {
                return new string[]
                {
                    "Plant_Dandelion",
                    "Plant_Astragalus",
                    "Plant_TreePine",
                    "Plant_TreeBirch"
                };
            }
        }
    }

    public sealed class AMJDarkForestTerrainQuickstart : BiomeTerrainQuickstartBase
    {
        protected override string TargetBiomeDefName
        {
            get { return "DankPyon_DarkForest"; }
        }
    }

    // 0.2.0 test-only: one real ColdBog map, accelerated calendar and a
    // genuine RimWorld save. Reload is a separate process, not inferred from
    // the presence of a saved .rws file.
    public sealed class AMJWetlandSeasonSaveQuickstart : BiomeTerrainQuickstartBase
    {
        public const string SaveName = "AMJE_020_Wetland_SeasonSave";
        protected override string TargetBiomeDefName { get { return "ColdBog"; } }
        protected override string TargetPlantDefName { get { return "Plant_Moss"; } }
        protected override bool ValidateAmjBiomeContracts { get { return false; } }
        private bool speciesPlaced;
        private bool seasonalRange;
        private bool saved;
        private bool validGraphics = true;
        private bool weatherWindowWeightsCorrect;
        private bool weatherLightCorrect;
        private bool plantAndSolarRainResponseCorrect;

        // Probe actual Vanilla consumer paths, not a second copy of the
        // daylight-factor formula. Restore map weather/time and destroy the
        // temporary crop/panel before the existing season-save Quickstart.
        private static bool SamplePlantAndSolarRainResponse(Map map)
        {
            ThingDef cropDef = DefDatabase<ThingDef>.GetNamedSilentFail("Plant_Potato");
            ThingDef panelDef = DefDatabase<ThingDef>.GetNamedSilentFail("SolarGenerator");
            WeatherDef clear = DefDatabase<WeatherDef>.GetNamedSilentFail("Clear");
            WeatherDef rain = DefDatabase<WeatherDef>.GetNamedSilentFail("Rain");
            var outputProp = AccessTools.Property(typeof(CompPowerPlantSolar),
                "DesiredPowerOutput");
            if (cropDef == null || panelDef == null || clear == null ||
                rain == null || outputProp == null || map == null)
                return false;

            int originalTicks = Find.TickManager.TicksGame;
            WeatherDef originalWeather = map.weatherManager.curWeather;
            WeatherDef originalPrevious = map.weatherManager.lastWeather;
            Plant crop = null;
            ThingWithComps panel = null;
            try
            {
                IntVec3 cropCell = map.Center + new IntVec3(10, 0, 10);
                IntVec3 panelCell = map.Center + new IntVec3(-10, 0, 10);
                if (!cropCell.InBounds(map) || !panelCell.InBounds(map) ||
                    map.roofGrid.Roofed(cropCell) || map.roofGrid.Roofed(panelCell))
                    return false;

                crop = GenSpawn.Spawn(ThingMaker.MakeThing(cropDef), cropCell,
                    map, WipeMode.Vanish) as Plant;
                panel = GenSpawn.Spawn(ThingMaker.MakeThing(panelDef), panelCell,
                    map, WipeMode.Vanish) as ThingWithComps;
                CompPowerPlantSolar solar = panel == null ? null :
                    panel.TryGetComp<CompPowerPlantSolar>();
                if (crop == null || solar == null)
                    return false;

                for (int hour = 0; hour < 24; hour++)
                {
                    Find.TickManager.DebugSetTicksGame(
                        originalTicks + hour * GenDate.TicksPerHour);
                    if (clear.Worker.CurSkyTarget(map).glow < 0.8f)
                        continue;

                    map.weatherManager.curWeather = clear;
                    map.weatherManager.lastWeather = clear;
                    map.skyManager.SkyManagerUpdate();
                    float clearGlow = map.skyManager.CurSkyGlow;
                    float clearGrowth = crop.GrowthRateFactor_Light;
                    float clearSolar = (float)outputProp.GetValue(solar, null);

                    map.weatherManager.curWeather = rain;
                    map.weatherManager.lastWeather = rain;
                    map.skyManager.SkyManagerUpdate();
                    float rainGlow = map.skyManager.CurSkyGlow;
                    float rainGrowth = crop.GrowthRateFactor_Light;
                    float rainSolar = (float)outputProp.GetValue(solar, null);

                    bool success = clearGlow >= 0.8f && clearGrowth > 0.01f &&
                        clearSolar > 0f && rainGlow < clearGlow &&
                        rainGrowth < clearGrowth && rainSolar < clearSolar &&
                        Math.Abs(rainGlow / clearGlow - 0.8f) < 0.015f &&
                        Math.Abs(rainSolar / clearSolar - 0.8f) < 0.015f;
                    Log.Message("AMJE_RAIN_CONSUMERS native=True" +
                        " crop=" + crop.def.defName +
                        " minGlow=" + crop.def.plant.growMinGlow.ToString("F3") +
                        " optGlow=" + crop.def.plant.growOptimalGlow.ToString("F3") +
                        " clearGlow=" + clearGlow.ToString("F3") +
                        " rainGlow=" + rainGlow.ToString("F3") +
                        " clearGrowthFactor=" + clearGrowth.ToString("F3") +
                        " rainGrowthFactor=" + rainGrowth.ToString("F3") +
                        " clearSolarW=" + clearSolar.ToString("F1") +
                        " rainSolarW=" + rainSolar.ToString("F1") +
                        " passed=" + success);
                    return success;
                }
                return false;
            }
            finally
            {
                Find.TickManager.DebugSetTicksGame(originalTicks);
                map.weatherManager.curWeather = originalWeather;
                map.weatherManager.lastWeather = originalPrevious;
                map.skyManager.SkyManagerUpdate();
                if (panel != null && panel.Spawned)
                    panel.Destroy(DestroyMode.Vanish);
                if (crop != null && crop.Spawned)
                    crop.Destroy(DestroyMode.Vanish);
            }
        }

        private static bool SampleRainLight(Map map)
        {
            WeatherDef clear = DefDatabase<WeatherDef>.GetNamedSilentFail("Clear");
            WeatherDef rain = DefDatabase<WeatherDef>.GetNamedSilentFail("Rain");
            WeatherDef rainFog = DefDatabase<WeatherDef>.GetNamedSilentFail("FoggyRain");
            WeatherDef thunder = DefDatabase<WeatherDef>.GetNamedSilentFail("RainyThunderstorm");
            WeatherDef fog = DefDatabase<WeatherDef>.GetNamedSilentFail("Fog");
            var targetMethod = AccessTools.Method(typeof(WeatherWorker), "CurSkyTarget");
            if (clear == null || rain == null || rainFog == null ||
                thunder == null || fog == null || targetMethod == null)
                return false;
            var patchInfo = Harmony.GetPatchInfo(targetMethod);
            bool patched = patchInfo != null && patchInfo.Postfixes.Any(
                item => item.PatchMethod != null &&
                    item.PatchMethod.DeclaringType == typeof(RainWeatherDaylightPatch));

            int initial = Find.TickManager.TicksGame;
            bool checkedDaylight = false;
            bool ratiosOk = patched;
            try
            {
                for (int h = 0; h < 24; h++)
                {
                    Find.TickManager.DebugSetTicksGame(initial + h * GenDate.TicksPerHour);
                    float clearGlow = clear.Worker.CurSkyTarget(map).glow;
                    if (clearGlow <= 0.7f) continue;
                    checkedDaylight = true;
                    WeatherDef[] defs = { rain, rainFog, thunder, fog };
                    float[] expected = {
                        0.80f,
                        0.80f,
                        0.70f,
                        0.90f
                    };
                    for (int i = 0; i < defs.Length; i++)
                    {
                        float actual = defs[i].Worker.CurSkyTarget(map).glow;
                        ratiosOk &= Math.Abs(actual / clearGlow - expected[i]) < 0.001f;
                        Log.Message("AMJE_RAIN_LIGHT native=True weather=" +
                            defs[i].defName + " clearGlow=" + clearGlow.ToString("F3") +
                            " rainGlow=" + actual.ToString("F3") +
                            " expectedFactor=" + expected[i].ToString("F2"));
                    }
                    break;
                }
            }
            finally
            {
                Find.TickManager.DebugSetTicksGame(initial);
            }
            return checkedDaylight && ratiosOk;
        }

        // One loaded-native selector probe, not an actual 60-day run or a
        // meteorological calibration. Keep random state and absolute ticks
        // unchanged after probing, before the existing saved-game acceptance.
        private static bool SampleWeatherWindows(Map map)
        {
            WeatherDef rain = DefDatabase<WeatherDef>.GetNamedSilentFail("Rain");
            WeatherDef clear = DefDatabase<WeatherDef>.GetNamedSilentFail("Clear");
            var currentWeight = AccessTools.Method(typeof(WeatherDecider),
                "CurrentWeatherCommonality");
            var choose = AccessTools.Method(typeof(WeatherDecider),
                "ChooseNextWeather");
            if (map == null || rain == null || clear == null ||
                map.weatherDecider == null || currentWeight == null || choose == null)
                return false;

            var patchInfo = Harmony.GetPatchInfo(currentWeight);
            bool patched = patchInfo != null && patchInfo.Postfixes.Any(
                item => item.PatchMethod != null &&
                    item.PatchMethod.DeclaringType == typeof(SeasonalRainWeatherPatch));
            var longLat = Find.WorldGrid.LongLatOf(map.Tile);
            int initial = Find.TickManager.TicksGame;
            int originalDay = GenDate.DayOfYear(Find.TickManager.TicksAbs, longLat.x);
            int[] days = { 12, 18, 26, 33, 45 };
            float[] rainFactors = { 1f, 1.65f, 1f, 1.40f, 1f };
            float[] clearFactors = { 1f, 0.75f, 1f, 0.85f, 1f };
            bool exactWeights = patched;
            try
            {
                for (int i = 0; i < days.Length; i++)
                {
                    // Match the production hook's phase adjustment on real
                    // Southern Hemisphere tiles without editing native seasons.
                    int target = longLat.y < 0f ?
                        (days[i] + 30) % GenDate.DaysPerYear : days[i];
                    int delta = (target - originalDay +
                        GenDate.DaysPerYear) % GenDate.DaysPerYear;
                    Find.TickManager.DebugSetTicksGame(
                        initial + delta * GenDate.TicksPerDay);
                    int observed = GenDate.DayOfYear(
                        Find.TickManager.TicksAbs, longLat.x);
                    float testRain = 10f, testClear = 10f;
                    SeasonalRainWeatherPatch.Postfix(map, rain, ref testRain);
                    SeasonalRainWeatherPatch.Postfix(map, clear, ref testClear);
                    exactWeights &= observed == target &&
                        Math.Abs(testRain - 10f * rainFactors[i]) < 0.01f &&
                        Math.Abs(testClear - 10f * clearFactors[i]) < 0.01f;

                    // Invoke the actual Harmony-patched Vanilla weighted
                    // chooser on the loaded game, with fixed random seed. This
                    // samples selection *tendency*, not weather-day duration.
                    int rainDraws = 0;
                    int draws = 240;
                    float actualRainWeight = (float)currentWeight.Invoke(
                        map.weatherDecider, new object[] { rain });
                    Rand.PushState(16781);
                    try
                    {
                        for (int j = 0; j < draws; j++)
                        {
                            WeatherDef result = (WeatherDef)choose.Invoke(
                                map.weatherDecider, null);
                            if (result != null &&
                                (result.defName == "Rain" ||
                                 result.defName == "FoggyRain" ||
                                 result.defName == "RainyThunderstorm"))
                                rainDraws++;
                        }
                    }
                    finally
                    {
                        Rand.PopState();
                    }
                    Log.Message("AMJE_WEATHER_WINDOW native=True biome=" +
                        map.Biome.defName + " latitude=" + longLat.y.ToString("F1") +
                        " targetDay=" + days[i] +
                        " effectiveDay=" + observed +
                        " rainWeight=" + actualRainWeight.ToString("F3") +
                        " wetSelections=" + rainDraws + "/" + draws +
                        " directWeights=" + (exactWeights ? "PASS" : "FAIL") +
                        " harmonyPostfix=" + patched);
                }
            }
            finally
            {
                Find.TickManager.DebugSetTicksGame(initial);
            }
            return exactWeights;
        }

        public override void PostLoaded()
        {
            base.PostLoaded();
            Map map = Find.CurrentMap;
            if (map == null) return;
            weatherWindowWeightsCorrect = SampleWeatherWindows(map);
            weatherLightCorrect = SampleRainLight(map);
            plantAndSolarRainResponseCorrect = SamplePlantAndSolarRainResponse(map);
            string[] names = { "AMJ_Plant_Yoshi", "AMJ_Plant_Suge",
                               "AMJ_Plant_Mizugoke", "AMJ_Tree_Hannoki" };
            var plants = new List<Plant>();
            for (int i = 0; i < names.Length; i++)
            {
                ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(names[i]);
                if (def == null) break;
                IntVec3 cell = map.Center + new IntVec3((i-2)*8, 0, -8);
                foreach (IntVec3 area in GenRadial.RadialCellsAround(cell, 3f, true))
                {
                    if (!area.InBounds(map)) continue;
                    foreach (Thing thing in area.GetThingList(map).ToArray())
                        if (!(thing is Pawn)) thing.Destroy(DestroyMode.Vanish);
                    map.terrainGrid.SetTerrain(area, TerrainDefOf.Soil);
                }
                Plant plant = ThingMaker.MakeThing(def) as Plant;
                if (plant == null) break;
                plant.Growth = 1f;
                GenSpawn.Spawn(plant, cell, map, WipeMode.Vanish);
                plants.Add(plant);
            }
            speciesPlaced = plants.Count == names.Length;
            if (!speciesPlaced) return;
            int initialTicks = Find.TickManager.TicksGame;
            float minTemp = float.MaxValue, maxTemp = float.MinValue;
            try
            {
                for (int hour = 0; hour <= 1440; hour += 6)
                {
                    Find.TickManager.DebugSetTicksGame(initialTicks + hour * GenDate.TicksPerHour);
                    Find.World.tileTemperatures.WorldComponentTick();
                    object cached = AccessTools.Method(typeof(TileTemperaturesComp),
                        "RetrieveCachedData").Invoke(Find.World.tileTemperatures, new object[] { map.Tile });
                    AccessTools.Method(cached.GetType(), "CheckCache").Invoke(cached, null);
                    map.mapTemperature.TemperatureUpdate();
                    map.skyManager.SkyManagerUpdate();
                    float temp = map.mapTemperature.OutdoorTemp;
                    minTemp = Math.Min(minTemp, temp);
                    maxTemp = Math.Max(maxTemp, temp);
                    foreach (Plant plant in plants)
                    {
                        if (plant.Destroyed) continue;
                        plant.TickLong();
                        UnityEngine.Material material =
                            plant.Graphic.MatAt(plant.Rotation, plant);
                        validGraphics &= material != null &&
                            material.mainTexture != null &&
                            material.mainTexture != BaseContent.BadTex;
                    }
                }
                seasonalRange = maxTemp - minTemp > 8f;
            }
            finally
            {
                Find.TickManager.DebugSetTicksGame(initialTicks);
            }
            Log.Message("AMJE_020_SEASON observed=True minC=" + minTemp.ToString("F2") +
                " maxC=" + maxTemp.ToString("F2") +
                " graphics=" + validGraphics +
                " outcome=calendar_sample_not_full_60_day_tick_simulation");
            // Natural low-temperature death is a valid seasonal outcome.
            // Restore any such specimens to full-grown state for an independent
            // save/reload identity test; never change the production PlantDefs.
            int winterDeaths = 0;
            for (int i = 0; i < plants.Count; i++)
            {
                if (!plants[i].Destroyed) continue;
                winterDeaths++;
                IntVec3 cell = map.Center + new IntVec3((i-2)*8, 0, -8);
                Plant replacement = ThingMaker.MakeThing(plants[i].def) as Plant;
                replacement.Growth = 1f;
                GenSpawn.Spawn(replacement, cell, map, WipeMode.Vanish);
            }
            Log.Message("AMJE_020_SEASON winterDeaths=" + winterDeaths);
            // Genuine game serialization; the independent reload process must
            // observe the corresponding exact species after Scribe completes.
            GameDataSaveLoader.SaveGame(SaveName);
            var saves = typeof(GenFilePaths).GetMethod("FilePathForSavedGame",
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);
            string name = saves == null ? null :
                saves.Invoke(null, new object[] { SaveName }) as string;
            saved = !string.IsNullOrEmpty(name) && System.IO.File.Exists(name);
            Log.Message("AMJE_020_SAVED complete=" + saved + " name=" + SaveName);
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification result = base.Verify();
            result.Assert("four production wetland plant instances placed", delegate
                { return speciesPlaced; });
            result.Assert("real cold bog calendar changes outdoor temperature", delegate
                { return seasonalRange; });
            result.Assert("real seasonal native plant graphics not BadTex", delegate
                { return validGraphics; });
            result.Assert("native game save exists on isolated disk", delegate
                { return saved; });
            result.Assert("native weather chooser Tsuyu/Akisame weight windows and Harmony hook", delegate
                { return weatherWindowWeightsCorrect; });
            result.Assert("native rain sky sunlight attenuation and Harmony patch", delegate
                { return weatherLightCorrect; });
            result.Assert("native potato light-growth factor and solar desired power fall in rain", delegate
                { return plantAndSolarRainResponseCorrect; });
            return result;
        }
    }

    // Loading a save during PlayDataLoader's startup completion callback runs
    // before the entry-root's world audio manager is ready. Begin loading only
    // after the real entry UI has rendered, like the ordinary Load Game action.
    [HarmonyPatch(typeof(Root_Entry), "Update")]
    public static class AMJWetlandSaveReloadBoot
    {
        private static int entryFrames;
        private static bool invoked;

        [HarmonyPostfix]
        public static void AfterEntryUpdate()
        {
            if (invoked ||
                System.Environment.GetEnvironmentVariable("AMJ_WETLAND_020_RELOAD") != "1")
                return;
            if (++entryFrames < 180) return;
            invoked = true;
            Log.Message("AMJE_020_RELOAD_START name=" +
                AMJWetlandSeasonSaveQuickstart.SaveName +
                " entryFrames=" + entryFrames);
            // Follow RimWorld's normal saved-game entry -> Play scene path.
            // Loading synchronously inside Root_Entry.Update leaves MapUI owned
            // by the wrong scene; a manually constructed Root_Play is also invalid.
            // Root_Play.Start performs the actual SavedGameLoaderNow load.
            LongEventHandler.QueueLongEvent(delegate
            {
                Current.Game = new Game
                {
                    InitData = new GameInitData
                    {
                        gameToLoad = AMJWetlandSeasonSaveQuickstart.SaveName
                    }
                };
            }, "Play", "LoadingLongEvent", true, null);
            Log.Message("AMJE_020_RELOAD_SCENE_QUEUED scene=Play");
        }
    }

    [HarmonyPatch(typeof(Game), "LoadGame")]
    public static class AMJWetlandSaveReloadObserver
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (System.Environment.GetEnvironmentVariable("AMJ_WETLAND_020_RELOAD") != "1")
                return;
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                Map map = Find.CurrentMap;
                string[] names = { "AMJ_Plant_Yoshi", "AMJ_Plant_Suge",
                                   "AMJ_Plant_Mizugoke", "AMJ_Tree_Hannoki" };
                bool ok = map != null;
                foreach (string name in names)
                {
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                    ok &= def != null && map != null &&
                        map.listerThings.ThingsOfDef(def).Count > 0;
                }
                Log.Message("AMJE_020_RELOAD_RESULT passed=" + ok +
                    " loadedColdBog=" + (map != null &&
                        map.Biome.defName == "ColdBog") +
                    " species=Yoshi,Suge,Mizugoke,Hannoki");
                if (!ok || map.Biome.defName != "ColdBog")
                    Log.Error("AMJE_020_RELOAD_FAILED native save/load invalid");
                UnityEngine.Application.Quit();
            });
        }
    }
}
