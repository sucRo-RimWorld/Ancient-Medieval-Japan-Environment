using HarmonyLib;
using Verse;

namespace AncientMedievalJapan.Environment
{
    [HarmonyPatch(typeof(MapGenerator), "GenerateMap")]
    public static class MapTerrainDiagnostics
    {
        public static void Postfix(Map __result)
        {
            if (__result == null || __result.Biome == null)
            {
                return;
            }

            string biome = __result.Biome.defName;
            if (!biome.StartsWith("AMJ_") && biome != "DankPyon_DarkForest")
            {
                return;
            }

            int thin = 0;
            int gravel = 0;
            int soil = 0;
            int rich = 0;
            int other = 0;
            int total = 0;

            foreach (IntVec3 cell in __result.AllCells)
            {
                TerrainDef terrain = __result.terrainGrid.TerrainAt(cell);
                total++;

                switch (terrain.defName)
                {
                    case "AMJ_ThinSoil":
                        thin++;
                        break;
                    case "Gravel":
                        gravel++;
                        break;
                    case "Soil":
                        soil++;
                        break;
                    case "SoilRich":
                        rich++;
                        break;
                    default:
                        other++;
                        break;
                }
            }

            Log.Message(
                "[AMJ Environment] Map terrain summary" +
                " | biome=" + biome +
                " cells=" + total +
                " | ThinSoil=" + Format(thin, total) +
                " Gravel=" + Format(gravel, total) +
                " Soil=" + Format(soil, total) +
                " RichSoil=" + Format(rich, total) +
                " Other=" + Format(other, total));
        }

        private static string Format(int value, int total)
        {
            return value + " (" +
                (total > 0 ? ((float)value / total).ToString("P1") : "0.0%") +
                ")";
        }
    }
}
