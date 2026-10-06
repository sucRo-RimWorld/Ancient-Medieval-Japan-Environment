using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using Verse;
using RimWorld;
[StaticConstructorOnStartup]
public static class WorkshopSourceAudit {
 static WorkshopSourceAudit(){LongEventHandler.ExecuteWhenFinished(Run);}
 static void Run(){try{
  var mods=LoadedModManager.RunningModsListForReading;
  foreach(var m in mods) Log.Message("[AMJE WorkshopSourceAudit] MOD package="+m.PackageIdPlayerFacing+" root="+m.RootDir);
  var packs=mods.Where(m=>m.PackageIdPlayerFacing.StartsWith("sucro.ancientmedievaljapan.environment",StringComparison.OrdinalIgnoreCase)).ToArray();
  string expected=@"D:\SteamLibrary\steamapps\workshop\content\294100\3814638060";
  if(packs.Length!=1 || !String.Equals(Path.GetFullPath(packs[0].RootDir).TrimEnd('\\'),expected,StringComparison.OrdinalIgnoreCase)) throw new Exception("Workshop source mismatch");
  var assembly=AppDomain.CurrentDomain.GetAssemblies().Single(a=>a.GetName().Name=="AncientMedievalJapanEnvironment");
  if(!assembly.Location.StartsWith(expected+"\\",StringComparison.OrdinalIgnoreCase)) throw new Exception("Production assembly source mismatch: "+assembly.Location);
  foreach(string name in new[]{"AMJ_Tree_Shii","AMJ_Tree_Beech","AMJ_Tree_Shirabiso","AMJ_Shrub_Haimatsu"}){
   var defs=DefDatabase<ThingDef>.AllDefsListForReading.Where(d=>d.defName==name).ToArray();
   if(defs.Length!=1 || !String.Equals(defs[0].modContentPack.RootDir.TrimEnd('\\'),expected,StringComparison.OrdinalIgnoreCase)) throw new Exception("Def ownership/duplication: "+name);
  }
  if(mods.Any(m=>m.PackageIdPlayerFacing.Equals("DankPyon.Medieval.Overhaul",StringComparison.OrdinalIgnoreCase))){
   var biome=DefDatabase<BiomeDef>.GetNamed("DankPyon_DarkForest");
   string[] names={"AMJ_ThinSoil","Gravel","Soil","SoilRich"};
   float[] mins={-999f,.4f,.6f,.9f}, maxs={.4f,.6f,.9f,999f};
   if(biome.terrainsByFertility.Count!=4) throw new Exception("MO DarkForest soil count");
   for(int i=0;i<4;i++){var t=biome.terrainsByFertility[i];if(t.terrain.defName!=names[i] || Math.Abs(t.min-mins[i])>.001 || Math.Abs(t.max-maxs[i])>.001) throw new Exception("MO DarkForest soil patch mismatch "+i);}
   Log.Message("[AMJE WorkshopSourceAudit] MO DarkForest soil patch PASS AMJ_ThinSoil/Gravel/Soil/SoilRich");
  }
  string output=System.Environment.GetEnvironmentVariable("AMJE_TRANSLATION_OUTPUT");
  if(!String.IsNullOrEmpty(output)){
   var builder=new StringBuilder();
   var type=typeof(Log).Assembly.GetType("Verse.LanguageReportGenerator");
   foreach(string method in new[]{"AppendGeneralLoadErrors","AppendDefInjectionsLoadErros","AppendRenamedDefInjections","AppendArgumentCountMismatches","AppendUnnecessaryKeyedTranslations","AppendObsoleteBackstoryTranslations","AppendTKeySystemErrors"}) type.GetMethod(method,BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Static).Invoke(null,new object[]{builder});
   File.WriteAllText(output,builder.ToString(),Encoding.UTF8);
   Log.Message("[AMJE WorkshopSourceAudit] Translation diagnostics saved="+output);
  }
  Log.Message("[AMJE WorkshopSourceAudit] PASS workshopId=3814638060 root="+packs[0].RootDir+" assembly="+assembly.Location);
 }catch(Exception e){Log.Error("[AMJE WorkshopSourceAudit] ERROR "+e);}}
}
