using System;
using UnityEngine;
using Verse;
using RimWorld;
[StaticConstructorOnStartup]
public static class AMJEReleaseAudit
{
 static AMJEReleaseAudit(){LongEventHandler.ExecuteWhenFinished(Run);}
 static void Check(bool ok,string message){if(!ok) throw new Exception(message);}
 static bool Valid(Graphic g){return g!=null && g.MatSingle.mainTexture!=null && g.MatSingle.mainTexture!=BaseContent.BadTex;}
 static void Run(){try{
  ModContentPack pack=null;
  foreach(var m in LoadedModManager.RunningModsListForReading){
   if(m.PackageIdPlayerFacing=="sucro.ancientmedievaljapan.environment")pack=m;
   Check(m.PackageIdPlayerFacing!="rimworks.quickstarts","Development quickstarts active");
  }
  Check(pack!=null && pack.RootDir.EndsWith("AMJE.ReleaseCandidateCheck"),"Wrong source pack selected");
  foreach(var folder in pack.foldersToLoadDescendingOrder)Check(!folder.Contains("DevQuickstarts"),"Development load folder selected");
  foreach(string name in new[]{"AMJ_Tree_Shii","AMJ_Tree_Beech","AMJ_Tree_Shirabiso","AMJ_Shrub_Haimatsu"}){
   ThingDef def=DefDatabase<ThingDef>.GetNamed(name);
   Check(Valid(def.graphicData.Graphic),name+" normal BadTex");
   Check(def.uiIcon!=null && def.uiIcon!=BaseContent.BadTex,name+" UI BadTex");
   Check(Valid(def.plant.snowOverlayGraphic),name+" snow BadTex");
   if(name=="AMJ_Tree_Beech"){
    Check(Valid(def.plant.leaflessGraphic),"Beech leafless BadTex");
    Check(Valid(def.plant.leaflessSnowOverlayGraphic),"Beech leafless snow BadTex");
   }
   if(name=="AMJ_Shrub_Haimatsu"){
    Check(Math.Abs(GenUI.IconDrawScale(def)-1f)<.001f,"Haimatsu UI scale");
    Check(Math.Abs(def.graphicData.drawSize.x-2.60f)<.001f,"Haimatsu map scale");
   }
   Log.Message("[AMJ Environment] ReleaseAudit valid plant="+name+" icon="+def.uiIcon.name+" uiScale="+GenUI.IconDrawScale(def));
  }
  Log.Message("[AMJ Environment] ReleaseAudit PASS exact candidate pack="+pack.RootDir+" fourPlants=4 leaflessSnow=True quickstarts=False");
 }catch(Exception e){Log.Error("[AMJ Environment] ReleaseAudit ERROR "+e);}}
}
