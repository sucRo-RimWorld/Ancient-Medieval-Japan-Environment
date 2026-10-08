using System;
using System.Collections.Generic;
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
            string[] names = { "AMJ_Tree_Shii", "AMJ_Tree_Beech", "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu" };
            int[] yields = { 42, 40, 30, 8 };
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

            verification.Assert(id + " has an approved EN/JA wetland description",
                delegate
                {
                    return biome != null && biome.description != null &&
                        (string.Equals(biome.description.Trim(),
                            approvedEnglishDescription, StringComparison.Ordinal) ||
                         string.Equals(biome.description.Trim(),
                            approvedJapaneseDescription, StringComparison.Ordinal));
                });

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
        private int landTiles;
        private int unassignedLandTiles;
        private int wetlandCandidates;
        private int temperateSwampTiles;
        private int coldBogTiles;
        private int wetlandOutsideCandidateTiles;
        private bool selectedNaturalWetland;

        public override TaggedString description
        {
            get { return "Audits naturally selected wetland biomes on a larger AMJ world."; }
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

            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                SurfaceTile tile = Find.WorldGrid[i] as SurfaceTile;
                if (tile == null || tile.WaterCovered)
                {
                    continue;
                }

                landTiles++;
                bool candidate = tile.swampiness >= 0.5f;
                if (candidate)
                {
                    wetlandCandidates++;
                }

                string biome = tile.PrimaryBiome == null
                    ? null
                    : tile.PrimaryBiome.defName;
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
                delegate { return unassignedLandTiles == 0; });
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

            return result;
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
                "Plant_Brambles",
                "Plant_Bush",
                "Plant_TreeWillow",
                "Plant_TreeMaple",
                "Plant_Berry",
                "Plant_HealrootWild"
            };
            float[] expected = { 3.2f, 0.8f, 0.2f, 2.0f, 1.0f, 0.05f, 0.05f };
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
                "Plant_Moss",
                "Plant_Bush",
                "Plant_TreeWillow",
                "Plant_TreeBirch",
                "Plant_TreeMaple",
                "Plant_Berry",
                "Plant_HealrootWild"
            };
            float[] expected = { 3.4f, 2.6f, 0.3f, 0.6f, 0.6f, 0.6f, 0.07f, 0.05f };
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
            foreach (Thing thing in map.listerThings.AllThings)
            {
                Plant plant = thing as Plant;
                if (plant != null && plant.def.defName == TargetPlantDefName &&
                    !map.roofGrid.Roofed(plant.Position) &&
                    plant.Growth >= 0.95f && !plant.Dying &&
                    (selected == null || plant.Age < selected.Age)) selected = plant;
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
}
