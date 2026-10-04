using System;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse;

namespace AncientMedievalJapan.Environment
{
    public static class LiveTextureDiagnostics
    {
        [DebugAction(
            "AMJ Environment",
            "Scan current map for bad live textures",
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void ScanCurrentMapForBadLiveTextures()
        {
            Map map = Find.CurrentMap;
            if (map == null)
            {
                Log.Warning("[AMJ Environment LiveTextureAudit] No current map.");
                return;
            }

            int scanned = 0;
            int bad = 0;
            int exceptions = 0;

            foreach (Thing thing in map.listerThings.AllThings)
            {
                if (thing == null || thing.Destroyed)
                {
                    continue;
                }

                Graphic graphic;
                try
                {
                    graphic = thing.Graphic;
                }
                catch (Exception ex)
                {
                    exceptions++;
                    Log.Warning(
                        "[AMJ Environment LiveTextureAudit] GRAPHIC-ERROR" +
                        " def=" + DefNameOf(thing) +
                        " label=" + LabelOf(thing) +
                        " pos=" + thing.Position +
                        " type=" + thing.GetType().FullName +
                        " exception=" + ex.GetType().Name +
                        ": " + ex.Message);
                    continue;
                }

                if (graphic == null)
                {
                    continue;
                }

                scanned++;

                try
                {
                    Material material = graphic.MatAt(thing.Rotation, thing);
                    if (MaterialIsBad(material))
                    {
                        bad++;
                        LogBad(
                            thing,
                            "live",
                            graphic,
                            material);
                    }

                    Plant plant = thing as Plant;
                    if (plant != null && plant.SnowOverlayGraphic != null)
                    {
                        Material snowMaterial =
                            plant.SnowOverlayGraphic.MatSingleFor(plant);
                        if (MaterialIsBad(snowMaterial))
                        {
                            bad++;
                            LogBad(
                                thing,
                                "snowOverlay",
                                plant.SnowOverlayGraphic,
                                snowMaterial);
                        }
                    }
                }
                catch (Exception ex)
                {
                    exceptions++;
                    Log.Warning(
                        "[AMJ Environment LiveTextureAudit] MATERIAL-ERROR" +
                        " def=" + DefNameOf(thing) +
                        " label=" + LabelOf(thing) +
                        " pos=" + thing.Position +
                        " graphic=" + graphic.GetType().FullName +
                        " path=" + (graphic.path ?? "<null>") +
                        " exception=" + ex.GetType().Name +
                        ": " + ex.Message);
                }
            }

            string summary =
                "[AMJ Environment LiveTextureAudit] COMPLETE" +
                " scanned=" + scanned +
                " bad=" + bad +
                " exceptions=" + exceptions;

            Log.Message(summary);
            Messages.Message(
                "AMJ Environment texture scan complete: " +
                bad + " bad texture(s), " +
                exceptions + " exception(s). See Player.log.",
                bad > 0 || exceptions > 0
                    ? MessageTypeDefOf.CautionInput
                    : MessageTypeDefOf.NeutralEvent,
                false);
        }

        private static bool MaterialIsBad(Material material)
        {
            if (material == null || material.mainTexture == null)
            {
                return true;
            }

            Texture texture = material.mainTexture;
            if (texture == BaseContent.BadTex)
            {
                return true;
            }

            string name = texture.name ?? "";
            return
                string.Equals(
                    name,
                    "ERRORTEX",
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(
                    name,
                    "BadTex",
                    StringComparison.OrdinalIgnoreCase);
        }

        private static void LogBad(
            Thing thing,
            string state,
            Graphic graphic,
            Material material)
        {
            string textureName =
                material == null || material.mainTexture == null
                    ? "<null>"
                    : material.mainTexture.name;

            Log.Warning(
                "[AMJ Environment LiveTextureAudit] BAD" +
                " def=" + DefNameOf(thing) +
                " label=" + LabelOf(thing) +
                " state=" + state +
                " pos=" + thing.Position +
                " type=" + thing.GetType().FullName +
                " graphic=" + graphic.GetType().FullName +
                " path=" + (graphic.path ?? "<null>") +
                " texture=" + textureName);
        }

        private static string DefNameOf(Thing thing)
        {
            return thing.def == null ? "<null>" : thing.def.defName;
        }

        private static string LabelOf(Thing thing)
        {
            try
            {
                return thing.LabelNoCount ?? "<null>";
            }
            catch
            {
                return "<label-error>";
            }
        }
    }
}
