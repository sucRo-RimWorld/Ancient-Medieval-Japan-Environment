using System;
using RimWorks.Quickstarts;
using RimWorks.Quickstarts.Verification;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AncientMedievalJapan.Environment.Quicktests
{
    // Disposable climate-observation site input. Unlike the ordinary biome
    // Quickstarts, never force any tile biome, rainfall or temperature.
    public sealed class AMJNaturalClimateObservationQuickstart : AbstractQuickstart
    {
        private PlanetTile selected = PlanetTile.Invalid;
        private bool geographicMatch;

        public override TaggedString description
        {
            get { return "Select an unmodified Japan-like climate site for temporary weather observation."; }
        }

        public override string seed
        {
            get { return "AMJ-Environment-NaturalClimate-Observation"; }
        }

        public override float planetCoverage
        {
            get { return 1.0f; }
        }

        public override int mapSize
        {
            get { return 250; }
        }

        public override void PostApplyConfiguration()
        {
            float bestScore = float.MaxValue;
            // Climate representativeness is the primary gate. Geographic
            // representativeness is independently reported, never fabricated.
            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                SurfaceTile surface = Find.WorldGrid[i] as SurfaceTile;
                if (surface == null || surface.WaterCovered ||
                    surface.PrimaryBiome == null ||
                    !SeasonalRainWeatherPatch.IsJapanEnvironmentBiome(surface.PrimaryBiome))
                {
                    continue;
                }

                PlanetTile tile = surface.tile;
                if (!TileFinder.IsValidTileForNewSettlement(tile) ||
                    Find.WorldObjects.AnyWorldObjectAt(tile))
                {
                    continue;
                }

                if (surface.temperature < 12f || surface.temperature > 18f ||
                    surface.rainfall < 1000f || surface.rainfall > 2200f)
                {
                    continue;
                }

                float latitude = Find.WorldGrid.LongLatOf(tile).y;
                bool inGeographicBand = latitude >= 32f && latitude <= 38f;
                // Give a real 32-38 N site priority only among otherwise
                // climate-eligible sites; never enforce the latitude by mutation.
                float score = (inGeographicBand ? 0f : 1000f) +
                    Math.Abs(surface.temperature - 15.8f) * 10f +
                    Math.Abs(surface.rainfall - 1598.2f) / 100f;
                if (score < bestScore)
                {
                    bestScore = score;
                    selected = tile;
                    geographicMatch = inGeographicBand;
                }
            }

            if (!selected.Valid)
            {
                throw new InvalidOperationException(
                    "No natural Japan-like climate site: no biome/temperature/rainfall mutation permitted.");
            }

            Find.GameInitData.startingTile = selected;
            SurfaceTile actual = Find.WorldGrid[selected] as SurfaceTile;
            float actualLatitude = Find.WorldGrid.LongLatOf(selected).y;
            Log.Message(
                "[AMJE_NATURAL_CLIMATE_SITE] tile=" + selected.tileId +
                " lat=" + actualLatitude.ToString("F2") +
                " annualTempC=" + actual.temperature.ToString("F2") +
                " annualRainMm=" + actual.rainfall.ToString("F1") +
                " biome=" + actual.PrimaryBiome.defName +
                " geographic32to38=" + geographicMatch +
                " forcedClimate=False forcedBiome=False");
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification checks = new QuickstartVerification();
            checks.Assert("natural climate site remains loaded", () =>
                selected.Valid && Find.CurrentMap != null &&
                Find.CurrentMap.Tile == selected);
            return checks;
        }
    }
}
