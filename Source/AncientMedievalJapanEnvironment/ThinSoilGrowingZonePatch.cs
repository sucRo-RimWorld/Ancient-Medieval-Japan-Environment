using HarmonyLib;
using RimWorld;
using Verse;

namespace AncientMedievalJapan.Environment
{
    // Vanilla's growing-zone designator refuses land below its generic soil
    // cutoff even when the terrain explicitly supports growing. AMJ Thin Soil
    // is intentionally 50%-fertile farmland for poor-soil-compatible crops.
    // Do not change the fertility value or relax placement on other terrains.
    [HarmonyPatch(typeof(Designator_ZoneAdd_Growing),
        nameof(Designator_ZoneAdd_Growing.CanDesignateCell))]
    public static class ThinSoilGrowingZonePatch
    {
        [HarmonyPostfix]
        public static void Postfix(
            Designator_ZoneAdd_Growing __instance,
            IntVec3 c,
            ref AcceptanceReport __result)
        {
            if (__result.Accepted)
            {
                return;
            }

            Map map = __instance.Map;
            if (map == null || !c.InBounds(map))
            {
                return;
            }

            TerrainDef terrain = map.terrainGrid.TerrainAt(c);
            if (terrain == null || terrain.defName != "AMJ_ThinSoil")
            {
                return;
            }

            // Override only the low-fertility designator cutoff. Never
            // override the game's occupancy, fog or roof restrictions.
            if (c.Fogged(map) || c.Roofed(map) ||
                c.GetEdifice(map) != null ||
                map.zoneManager.ZoneAt(c) != null ||
                map.fertilityGrid.FertilityAt(c) < 0.5f)
            {
                return;
            }

            __result = true;
        }
    }
}
