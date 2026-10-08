using System;
using System.IO;
using System.Linq;
using System.Reflection;
using RimWorks.Pickle;
using Verse;
using RimWorld;

// Test-only assembly loaded through Pickle/Assemblies, never shipped to users.
// These steps inspect the real subscribed Environment Defs, not a copied fixture.
namespace AMJE.WorkshopPickle
{
    [PickleSteps]
    public sealed class SteamEnvironmentSteps
    {
        private const string Package = "sucro.ancientmedievaljapan.environment";

        private static ModContentPack FindSteamMod()
        {
            ModContentPack[] matches = LoadedModManager.RunningModsListForReading
                .Where(m => m.PackageIdPlayerFacing.Equals(Package, StringComparison.OrdinalIgnoreCase)
                    || m.PackageIdPlayerFacing.Equals(Package + "_steam", StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (matches.Length != 1)
                throw new InvalidOperationException("Expected exactly one Steam Environment Mod, found " + matches.Length);
            return matches[0];
        }

        private static bool SameRoot(string actual, string expected)
        {
            if (String.IsNullOrEmpty(actual) || String.IsNullOrEmpty(expected))
                return false;
            return String.Equals(Path.GetFullPath(actual).TrimEnd('\\', '/'),
                                 Path.GetFullPath(expected).TrimEnd('\\', '/'),
                                 StringComparison.OrdinalIgnoreCase);
        }

        private static bool LoadedPlantOwnedBySteam(string name, string root)
        {
            ThingDef[] defs = DefDatabase<ThingDef>.AllDefsListForReading
                .Where(d => d.defName == name).ToArray();
            return defs.Length == 1 && defs[0].plant != null
                && defs[0].modContentPack != null
                && SameRoot(defs[0].modContentPack.RootDir, root);
        }

        private static bool CommonalityIs(string biomeName, string plantName, float expected)
        {
            BiomeDef biome = DefDatabase<BiomeDef>.GetNamedSilentFail(biomeName);
            ThingDef plant = DefDatabase<ThingDef>.GetNamedSilentFail(plantName);
            return biome != null && plant != null
                && Math.Abs(biome.CommonalityOfPlant(plant) - expected) < 0.001f;
        }

        [Then("the installed Environment Mod and assembly come from the exact Steam folder")]
        public void VerifyOrigin(PickleContext ctx)
        {
            string expected = Environment.GetEnvironmentVariable("AMJE_EXPECTED_PAYLOAD_ROOT");
            ctx.Assert(!String.IsNullOrEmpty(expected), "Missing pinned Steam root environment variable");
            ModContentPack mod = FindSteamMod();
            ctx.Assert(SameRoot(mod.RootDir, expected), "Loaded Environment root is not the downloaded Workshop root");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name == "AncientMedievalJapanEnvironment").ToArray();
            ctx.Assert(assemblies.Length == 1, "Expected exactly one production Environment assembly");
            if (assemblies.Length == 1)
                ctx.Assert(assemblies[0].Location.StartsWith(
                    Path.GetFullPath(expected).TrimEnd('\\', '/') + Path.DirectorySeparatorChar,
                    StringComparison.OrdinalIgnoreCase), "Production DLL is not loaded from subscribed Steam root");
        }

        [Then("all four Environment plants are uniquely owned by the downloaded Mod")]
        public void VerifyPlantOwnership(PickleContext ctx)
        {
            string root = Environment.GetEnvironmentVariable("AMJE_EXPECTED_PAYLOAD_ROOT");
            foreach (string name in new[] { "AMJ_Tree_Shii", "AMJ_Tree_Beech",
                                            "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu" })
                ctx.Assert(LoadedPlantOwnedBySteam(name, root), name + " is absent, duplicated or has wrong Mod owner");
        }

        [Then("rejected Vanilla vegetation is absent from the selected Japanese biomes")]
        public void VerifyVanillaRemoval(PickleContext ctx)
        {
            foreach (string[] row in new[] {
                new[] { "AMJ_WarmTemperateForest", "Plant_TreePoplar" },
                new[] { "AMJ_WarmTemperateForest", "Plant_TreeOak" },
                new[] { "AMJ_SubalpineForest", "Plant_TreePine" },
                new[] { "AMJ_AlpineZone", "Plant_TreePine" },
                new[] { "AMJ_AlpineZone", "Plant_TreeBirch" },
                new[] { "AMJ_AlpineZone", "Plant_Dandelion" },
                new[] { "AMJ_AlpineZone", "Plant_Astragalus" },
                new[] { "TemperateSwamp", "Plant_Chokevine" },
                new[] { "TemperateSwamp", "Plant_TreeCypress" },
                new[] { "ColdBog", "Plant_Chokevine" },
                new[] { "ColdBog", "Plant_TreeCypress" },
                new[] { "ColdBog", "Plant_Astragalus" }
            })
                ctx.Assert(CommonalityIs(row[0], row[1], 0f),
                           "Unexpected vegetation in " + row[0] + ": " + row[1]);
        }

        [Then("the replacement structural plants preserve their balanced commonalities")]
        public void VerifyRepresentativePlantWeights(PickleContext ctx)
        {
            string[][] rows = {
                new[] { "AMJ_WarmTemperateForest", "AMJ_Tree_Shii", "2.55" },
                new[] { "AMJ_CoolTemperateForest", "AMJ_Tree_Beech", "1.8" },
                new[] { "AMJ_SubalpineForest", "AMJ_Tree_Shirabiso", "3.5" },
                new[] { "AMJ_AlpineZone", "AMJ_Shrub_Haimatsu", "1.34" }
            };
            foreach (string[] row in rows)
                ctx.Assert(CommonalityIs(row[0], row[1],
                    float.Parse(row[2], System.Globalization.CultureInfo.InvariantCulture)),
                    "Representative plant weight mismatch: " + row[0] + "/" + row[1]);
        }

        [Then("the harvested wood Def contracts remain correct with or without Medieval Overhaul")]
        public void VerifyLoadedWoodDefs(PickleContext ctx)
        {
            bool mo = LoadedModManager.RunningModsListForReading.Any(m =>
                m.PackageIdPlayerFacing.StartsWith("dankpyon.medieval.overhaul",
                                                   StringComparison.OrdinalIgnoreCase));
            string expectedWood = mo ? "DankPyon_RawWood" : "WoodLog";
            string[] names = { "AMJ_Tree_Shii", "AMJ_Tree_Beech",
                               "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu" };
            float[] yields = { 42f, 40f, 30f, 8f };
            for (int i = 0; i < names.Length; i++)
            {
                ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(names[i]);
                ctx.Assert(def != null && def.plant != null,
                           "Missing production plant Def " + names[i]);
                if (def == null || def.plant == null) continue;
                ctx.Assert(Math.Abs(def.plant.harvestYield - yields[i]) < 0.001f,
                           "Incorrect harvest yield: " + names[i]);
                ctx.Assert(def.plant.harvestedThingDef != null &&
                           def.plant.harvestedThingDef.defName == expectedWood,
                           "Incorrect loaded wood resource: " + names[i]);
            }
        }
    }
}
