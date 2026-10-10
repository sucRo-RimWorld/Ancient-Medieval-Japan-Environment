using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace AncientMedievalJapan.Environment
{
    /// <summary>
    /// Preserve the authored direction of the asymmetric Ashi and Susuki sprites.
    /// Only the plant mesh flip flag is filtered; all seeded random draws and
    /// normal/snow graphic selection continue through the original Plant.Print.
    /// </summary>
    [HarmonyPatch(typeof(Plant), nameof(Plant.Print))]
    internal static class Patch_Plant_Print_KeepSelectedFacing
    {
        private const string AshiDefName = "AMJ_Plant_Yoshi";
        private const string SusukiDefName = "AMJ_Plant_Susuki";

        private static readonly MethodInfo RandomFlipGetter =
            AccessTools.PropertyGetter(typeof(Rand), nameof(Rand.Bool));
        private static readonly MethodInfo FilterMethod =
            AccessTools.Method(typeof(Patch_Plant_Print_KeepSelectedFacing), nameof(FilterFlip));

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> source = instructions.ToList();
            int flipSites = RandomFlipGetter == null
                ? 0
                : source.Count(code => code.Calls(RandomFlipGetter));
            if (flipSites != 1 || FilterMethod == null)
            {
                Log.Error("[AMJ Environment] Plant.Print facing patch incompatible: " +
                    "expected one Rand.Bool flip site, found " + flipSites + ".");
                return source;
            }

            // Keep the original Rand.Bool call so per-plant random state, texture
            // variant selection and other species' visuals do not shift.
            List<CodeInstruction> result = new List<CodeInstruction>(source.Count + 2);
            foreach (CodeInstruction instruction in source)
            {
                result.Add(instruction);
                if (instruction.Calls(RandomFlipGetter))
                {
                    // Stack: original bool, then Plant (__instance).
                    result.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    result.Add(new CodeInstruction(OpCodes.Call, FilterMethod));
                }
            }
            return result;
        }

        private static bool FilterFlip(bool vanillaFlip, Plant plant)
        {
            string defName = plant?.def?.defName;
            if (defName == AshiDefName || defName == SusukiDefName)
            {
                return false;
            }
            return vanillaFlip;
        }
    }
}
