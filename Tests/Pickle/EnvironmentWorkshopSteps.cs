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
            string profile = Environment.GetEnvironmentVariable("AMJE_EXPECTED_PROFILE");
            bool requiresMO = profile == "MO" || profile == "MO-CCTO";
            bool requiresCCTO = profile == "CCTO" || profile == "MO-CCTO";
            bool loadedMO = LoadedModManager.RunningModsListForReading.Any(m =>
                m.PackageIdPlayerFacing.StartsWith("dankpyon.medieval.overhaul",
                    StringComparison.OrdinalIgnoreCase));
            bool loadedCCTO = LoadedModManager.RunningModsListForReading.Any(m =>
                m.PackageIdPlayerFacing.StartsWith("sucro.cropcoldtoleranceoverhaul",
                    StringComparison.OrdinalIgnoreCase));
            ctx.Assert(profile == "Vanilla" || profile == "MO" ||
                       profile == "CCTO" || profile == "MO-CCTO",
                       "Unknown expected Pickle profile");
            ctx.Assert(loadedMO == requiresMO, "Incorrect MO active state in " + profile);
            ctx.Assert(loadedCCTO == requiresCCTO, "Incorrect CCTO active state in " + profile);
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

        [Then("the development Environment Mod and assembly come from the expected source folder")]
        public void VerifyDevelopmentOrigin(PickleContext ctx)
        {
            VerifyOrigin(ctx);
        }

        [Then("all four Environment plants are uniquely owned by the downloaded Mod")]
        public void VerifyPlantOwnership(PickleContext ctx)
        {
            string root = Environment.GetEnvironmentVariable("AMJE_EXPECTED_PAYLOAD_ROOT");
            foreach (string name in new[] { "AMJ_Tree_Shii", "AMJ_Tree_Beech",
                                            "AMJ_Tree_Shirabiso", "AMJ_Shrub_Haimatsu" })
                ctx.Assert(LoadedPlantOwnedBySteam(name, root), name + " is absent, duplicated or has wrong Mod owner");
        }

        [Then("all four Environment plants are uniquely owned by the expected source Mod")]
        public void VerifyDevelopmentPlantOwnership(PickleContext ctx)
        {
            VerifyPlantOwnership(ctx);
            VerifyImplementedWetlandColdSpecs(ctx);
        }

        // Add to the existing four-profile Pickle scenario; no extra scenario or
        // CCTO assembly reference is required. Each wetland species enters this
        // gate when its approved production PlantDef actually exists.
        // Absence does NOT establish wetland implementation/runtime acceptance.
        private static void VerifyImplementedWetlandColdSpecs(PickleContext ctx)
        {
            bool ccto = LoadedModManager.RunningModsListForReading.Any(m =>
                m.PackageIdPlayerFacing.StartsWith("sucro.cropcoldtoleranceoverhaul",
                    StringComparison.OrdinalIgnoreCase));
            string root = Environment.GetEnvironmentVariable("AMJE_EXPECTED_PAYLOAD_ROOT");
            string[] names = {
                "AMJ_Plant_Yoshi", "AMJ_Plant_Suge",
                "AMJ_Tree_Hannoki", "AMJ_Plant_Mizugoke"
            };
            float[] cctoGrowth = { 5f, 0f, 5f, 0f };
            bool[] dormancy = { true, true, true, false };
            float[] coldDeath = { float.NaN, float.NaN, float.NaN, -35f };

            for (int i = 0; i < names.Length; i++)
            {
                ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(names[i]);
                if (def == null)
                    continue; // Not implemented yet; not a passing wetland-species test.

                ctx.Assert(LoadedPlantOwnedBySteam(names[i], root),
                    "Wetland PlantDef absent, duplicated or not owned by Environment: " + names[i]);
                ctx.Assert(def.plant != null, "Wetland PlantDef has no plant properties: " + names[i]);
                if (def.plant == null)
                    continue;

                float expectedGrowth = ccto ? cctoGrowth[i] : 0f;
                ctx.Assert(Math.Abs(def.plant.minGrowthTemperature - expectedGrowth) < 0.001f,
                    "Wetland minimum growth mismatch: " + names[i]);

                DefModExtension[] extensions = def.modExtensions == null
                    ? new DefModExtension[0]
                    : def.modExtensions.Where(ext => ext != null
                        && ext.GetType().FullName ==
                           "CropColdToleranceOverhaul.ColdToleranceExtension").ToArray();

                ctx.Assert(extensions.Length == (ccto ? 1 : 0),
                    "Wetland CCTO extension count mismatch: " + names[i]);
                if (!ccto || extensions.Length != 1)
                    continue;

                FieldInfo dormantField = extensions[0].GetType().GetField("coldDormancy");
                FieldInfo deathField = extensions[0].GetType().GetField("coldDeathTemperature");
                ctx.Assert(dormantField != null
                    && (bool)dormantField.GetValue(extensions[0]) == dormancy[i],
                    "Wetland CCTO dormancy mismatch: " + names[i]);
                bool deathMatches = false;
                if (deathField != null)
                {
                    float loadedDeath = (float)deathField.GetValue(extensions[0]);
                    deathMatches = float.IsNaN(coldDeath[i])
                        ? float.IsNaN(loadedDeath)
                        : Math.Abs(loadedDeath - coldDeath[i]) < 0.001f;
                }
                ctx.Assert(deathMatches,
                    "Wetland CCTO death threshold mismatch: " + names[i]);
            }
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

            // MO itself adds all four medicinal wild plants to both Vanilla
            // wetlands. Environment may not remove or reweight their entries.
            // These are loaded-game assertions, not a synthetic XML fixture.
            bool mo = LoadedModManager.RunningModsListForReading.Any(m =>
                m.PackageIdPlayerFacing.StartsWith("dankpyon.medieval.overhaul",
                                                    StringComparison.OrdinalIgnoreCase));
            if (mo)
            {
                // Retained Vanilla MO pools must stay untouched. AMJE's
                // new climate BiomeDefs need corresponding optional entries
                // because they replace most of the source Vanilla world tiles.
                foreach (string[] source in new[] {
                    new[] { "TemperateForest", "0.05" },
                    new[] { "BorealForest", "0.16" },
                    new[] { "Tundra", "0.05" }
                })
                foreach (string herb in new[] {
                    "DankPyon_Plant_MindwortWild", "DankPyon_Plant_PoppyWild",
                    "DankPyon_Plant_FleawortWild", "DankPyon_Plant_FlyAgaricWild"
                })
                    ctx.Assert(CommonalityIs(source[0], herb,
                        float.Parse(source[1], System.Globalization.CultureInfo.InvariantCulture)),
                        "MO original Vanilla herbal pool was modified: "
                        + source[0] + "/" + herb);

                foreach (string[] target in new[] {
                    new[] { "AMJ_WarmTemperateForest", "0.05" },
                    new[] { "AMJ_CoolTemperateForest", "0.05" },
                    new[] { "AMJ_SubalpineForest", "0.16" },
                    new[] { "AMJ_AlpineZone", "0.05" }
                })
                foreach (string herb in new[] {
                    "DankPyon_Plant_MindwortWild", "DankPyon_Plant_PoppyWild",
                    "DankPyon_Plant_FleawortWild", "DankPyon_Plant_FlyAgaricWild"
                })
                    ctx.Assert(CommonalityIs(target[0], herb,
                        float.Parse(target[1], System.Globalization.CultureInfo.InvariantCulture)),
                        "MO medicinal wild plant missing from replacement AMJE biome: "
                        + target[0] + "/" + herb);

                foreach (string biome in new[] { "TemperateSwamp", "ColdBog" })
                foreach (string herb in new[] {
                    "DankPyon_Plant_MindwortWild", "DankPyon_Plant_PoppyWild",
                    "DankPyon_Plant_FleawortWild", "DankPyon_Plant_FlyAgaricWild"
                })
                    ctx.Assert(CommonalityIs(biome, herb, 0.05f),
                        "MO medicinal plant missing/reweighted in loaded wetland: "
                        + biome + "/" + herb);
            }
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
