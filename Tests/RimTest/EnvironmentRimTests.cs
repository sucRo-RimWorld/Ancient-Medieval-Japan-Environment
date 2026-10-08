using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using AncientMedievalJapan.Environment;
using HarmonyLib;
using RimTestRedux;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace AMJE.Environment.RimTests
{
    internal static class TestTile
    {
        public static SurfaceTile Land(float temperature, float rainfall = 1200f, float swampiness = 0f)
        {
            SurfaceTile tile = (SurfaceTile)FormatterServices.GetUninitializedObject(typeof(SurfaceTile));
            tile.temperature = temperature;
            tile.rainfall = rainfall;
            tile.swampiness = swampiness;
            tile.elevation = 100f;
            tile.PrimaryBiome = DefDatabase<BiomeDef>.GetNamedSilentFail("TemperateForest");
            if (tile.PrimaryBiome == null)
                throw new AssertionException("Vanilla TemperateForest BiomeDef is not loaded");
            return tile;
        }

        public static float Score(BiomeWorker worker, float temperature, float rainfall = 1200f, float swampiness = 0f)
        {
            return worker.GetScore(null, Land(temperature, rainfall, swampiness), default(PlanetTile));
        }
    }

    [TestSuite]
    public static class BiomeScoringTests
    {
        [Test]
        public static void WetlandThresholdRemainsHalf()
        {
            Assert.That(Math.Abs(JapanBiomeScoring.WetlandThreshold - 0.5f) < 0.0001f).Is.True();
        }

        [Test]
        public static void BandScorePeaksAtCenter()
        {
            SurfaceTile centered = TestTile.Land(11.5f);
            SurfaceTile offset = TestTile.Land(15.5f);
            Assert.That(JapanBiomeScoring.BandScore(centered, 11.5f) >
                        JapanBiomeScoring.BandScore(offset, 11.5f)).Is.True();
            Assert.That(Math.Abs(JapanBiomeScoring.BandScore(centered, 11.5f) - 38f) < 0.0001f).Is.True();
        }

        [Test]
        public static void EligibilityRejectsDryAndWetLand()
        {
            Assert.That(JapanBiomeScoring.EligibleLand(TestTile.Land(10f, 1200f, 0.49f))).Is.True();
            Assert.That(JapanBiomeScoring.EligibleLand(TestTile.Land(10f, 799f, 0f))).Is.False();
            Assert.That(JapanBiomeScoring.EligibleLand(TestTile.Land(10f, 1200f, 0.5f))).Is.False();
        }

        [Test]
        public static void WarmTemperateBoundaryIsFifteenDegrees()
        {
            var worker = new BiomeWorker_AMJWarmTemperateForest();
            Assert.That(TestTile.Score(worker, 15f) > 0f).Is.True();
            Assert.That(TestTile.Score(worker, 14.99f) == 0f).Is.True();
        }

        [Test]
        public static void CoolTemperateBandIsEightToBelowFifteen()
        {
            var worker = new BiomeWorker_AMJCoolTemperateForest();
            Assert.That(TestTile.Score(worker, 8f) > 0f).Is.True();
            Assert.That(TestTile.Score(worker, 14.99f) > 0f).Is.True();
            Assert.That(TestTile.Score(worker, 7.99f) == 0f).Is.True();
            Assert.That(TestTile.Score(worker, 15f) == 0f).Is.True();
        }

        [Test]
        public static void SubalpineBandIsZeroToBelowEight()
        {
            var worker = new BiomeWorker_AMJSubalpineForest();
            Assert.That(TestTile.Score(worker, 0f) > 0f).Is.True();
            Assert.That(TestTile.Score(worker, 7.99f) > 0f).Is.True();
            Assert.That(TestTile.Score(worker, -0.01f) == 0f).Is.True();
            Assert.That(TestTile.Score(worker, 8f) == 0f).Is.True();
        }

        [Test]
        public static void AlpineBandIsBelowZero()
        {
            var worker = new BiomeWorker_AMJAlpineZone();
            Assert.That(TestTile.Score(worker, -0.01f) > 0f).Is.True();
            Assert.That(TestTile.Score(worker, 0f) == 0f).Is.True();
        }
    }

    [TestSuite]
    public static class TerrainCalculationTests
    {
        private static object Invoke(string name, params object[] args)
        {
            MethodInfo method = typeof(EnvironmentTerrainProcessor).GetMethod(
                name, BindingFlags.NonPublic | BindingFlags.Static);
            if (method == null)
                throw new AssertionException("Missing EnvironmentTerrainProcessor method: " + name);
            return method.Invoke(null, args);
        }

        [Test]
        public static void AnnualMeanTemperatureUsesLatitudeElevationAndClamps()
        {
            float equator = (float)Invoke("CalculateAnnualMeanTemperature", 0f, 0f);
            float pole = (float)Invoke("CalculateAnnualMeanTemperature", 90f, 0f);
            float mountain = (float)Invoke("CalculateAnnualMeanTemperature", 0f, 1000f);
            float high = (float)Invoke("CalculateAnnualMeanTemperature", 90f, 3800f);
            Assert.That(Math.Abs(equator - 20f) < 0.001f).Is.True();
            Assert.That(Math.Abs(pole - 5f) < 0.001f).Is.True();
            Assert.That(Math.Abs(mountain - 13.75f) < 0.001f).Is.True();
            Assert.That(Math.Abs(high - (-8f)) < 0.001f).Is.True();
        }

        [Test]
        public static void BaseElevationMappingRetainsConfiguredSeaAndPeakBounds()
        {
            float sea = (float)Invoke("AdjustBaseElevation", -500f, 0d);
            float coast = (float)Invoke("AdjustBaseElevation", 0f, 0d);
            float peak = (float)Invoke("AdjustBaseElevation", 5000f, 0d);
            Assert.That(Math.Abs(sea - (-300f)) < 0.001f).Is.True();
            Assert.That(Math.Abs(coast) < 0.001f).Is.True();
            Assert.That(Math.Abs(peak - 3800f) < 0.001f).Is.True();
        }

        [Test]
        public static void MountainUpliftOrdersRuggedClasses()
        {
            float large = (float)Invoke("ApplyMountainUplift", 500f, Hilliness.LargeHills, 0f);
            float mountain = (float)Invoke("ApplyMountainUplift", 500f, Hilliness.Mountainous, 0f);
            float impassable = (float)Invoke("ApplyMountainUplift", 500f, Hilliness.Impassable, 0f);
            Assert.That(large < mountain).Is.True();
            Assert.That(mountain < impassable).Is.True();
        }
    }

    [StaticConstructorOnStartup]
    public static class RimTestAutomationBridge
    {
        private const string AssemblyName = "AMJE.Environment.RimTests";

        static RimTestAutomationBridge()
        {
            try
            {
                Type viewer = AccessTools.TypeByName("RimTestRedux.Testing.Viewer");
                MethodInfo target = viewer == null ? null : AccessTools.Method(viewer, "LogTestsResults");
                if (target == null)
                    throw new InvalidOperationException("RimTest Redux Viewer.LogTestsResults was not found");
                new Harmony("sucro.amje.environment.rimtest.bridge").Patch(
                    target,
                    postfix: new HarmonyMethod(typeof(RimTestAutomationBridge), nameof(AfterResultsLogged)));
            }
            catch (Exception error)
            {
                WriteFatal(error);
                Application.Quit();
            }
        }

        public static void AfterResultsLogged()
        {
            string path = System.Environment.GetEnvironmentVariable("AMJE_RIMTEST_RESULT");
            if (String.IsNullOrEmpty(path))
                return;
            try
            {
                Assembly testAssembly = typeof(BiomeScoringTests).Assembly;
                Type assemblyLink = AccessTools.TypeByName("RimTestRedux.Testing.Assembly2TestSuiteLink");
                Type testLink = AccessTools.TypeByName("RimTestRedux.Testing.TestSuite2TestLink");
                Type explorer = AccessTools.TypeByName("RimTestRedux.Testing.TestExplorer");
                MethodInfo getSuites = AccessTools.Method(assemblyLink, "GetTestSuites");
                MethodInfo getTests = AccessTools.Method(testLink, "GetTests");
                MethodInfo getStatus = AccessTools.Method(explorer, "GetTestStatus");
                MethodInfo getError = AccessTools.Method(explorer, "GetTestError");
                if (getSuites == null || getTests == null || getStatus == null || getError == null)
                    throw new InvalidOperationException("RimTest Redux result API changed");

                var rows = new List<string>();
                int passed = 0, failed = 0, skipped = 0, unknown = 0;
                IEnumerable suites = (IEnumerable)getSuites.Invoke(null, new object[] { testAssembly });
                foreach (Type suite in suites.Cast<Type>().OrderBy(x => x.FullName))
                {
                    IEnumerable tests = (IEnumerable)getTests.Invoke(null, new object[] { suite });
                    foreach (MethodInfo test in tests.Cast<MethodInfo>().OrderBy(x => x.Name))
                    {
                        object statusObject = getStatus.Invoke(null, new object[] { test });
                        string status = statusObject == null ? "UNKNOWN" : statusObject.ToString();
                        Exception error = getError.Invoke(null, new object[] { test }) as Exception;
                        if (status == "PASS") passed++;
                        else if (status == "ERROR") failed++;
                        else if (status == "SKIP") skipped++;
                        else unknown++;
                        rows.Add("{\"suite\":\"" + Json(suite.Name) + "\",\"test\":\"" +
                                 Json(test.Name) + "\",\"status\":\"" + Json(status) +
                                 "\",\"error\":\"" + Json(error == null ? "" : error.ToString()) + "\"}");
                    }
                }
                int total = passed + failed + skipped + unknown;
                bool ok = total == 10 && failed == 0 && skipped == 0 && unknown == 0;
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path,
                    "{\n  \"assembly\": \"" + AssemblyName + "\",\n" +
                    "  \"passed\": " + (ok ? "true" : "false") + ",\n" +
                    "  \"total\": " + total + ",\n" +
                    "  \"pass\": " + passed + ",\n" +
                    "  \"failed\": " + failed + ",\n" +
                    "  \"skipped\": " + skipped + ",\n" +
                    "  \"unknown\": " + unknown + ",\n" +
                    "  \"tests\": [" + String.Join(",", rows.ToArray()) + "]\n}\n");
            }
            catch (Exception error)
            {
                WriteFatal(error);
            }
            finally
            {
                Application.Quit();
            }
        }

        private static void WriteFatal(Exception error)
        {
            string path = System.Environment.GetEnvironmentVariable("AMJE_RIMTEST_RESULT");
            if (String.IsNullOrEmpty(path))
                return;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, "{\"assembly\":\"" + AssemblyName +
                    "\",\"passed\":false,\"fatal\":\"" + Json(error.ToString()) + "\"}\n");
            }
            catch { }
        }

        private static string Json(string value)
        {
            if (value == null) return "";
            var b = new StringBuilder();
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\\': b.Append("\\\\"); break;
                    case '"': b.Append("\\\""); break;
                    case '\r': b.Append("\\r"); break;
                    case '\n': b.Append("\\n"); break;
                    case '\t': b.Append("\\t"); break;
                    default:
                        if (c < 32) b.Append("\\u" + ((int)c).ToString("x4"));
                        else b.Append(c);
                        break;
                }
            }
            return b.ToString();
        }
    }
}
