param(
    [Parameter(Mandatory = $true)]
    [string]$RimWorldDir
)

$ErrorActionPreference = "Stop"

$RepoRoot = Split-Path -Parent $PSScriptRoot
$CoreDefs = Join-Path $RimWorldDir "Data\Core\Defs"
$RiverDefsPath = Join-Path $CoreDefs "RiverDefs\RiverDefs.xml"
$WorldGeneratorPath = Join-Path $CoreDefs "WorldGeneration\WorldGenerator.xml"
$OutputDll = Join-Path $RepoRoot "Assemblies\AncientMedievalJapanEnvironment.dll"

function Fail([string]$Message) {
    Write-Host "[ERROR] $Message" -ForegroundColor Red
    exit 1
}

function Pass([string]$Message) {
    Write-Host "[OK] $Message"
}

if (-not (Test-Path $RiverDefsPath)) {
    Fail "RimWorld RiverDefs.xml was not found: $RiverDefsPath"
}

if (-not (Test-Path $WorldGeneratorPath)) {
    Fail "RimWorld WorldGenerator.xml was not found: $WorldGeneratorPath"
}

if (-not (Test-Path $OutputDll)) {
    Fail "Built DLL was not found: $OutputDll"
}

[xml]$riverDefs = Get-Content -LiteralPath $RiverDefsPath
foreach ($defName in @("Creek", "River", "LargeRiver", "HugeRiver")) {
    $node = $riverDefs.SelectSingleNode("/Defs/RiverDef[defName='$defName']")
    if ($null -eq $node) {
        Fail "Required Vanilla RiverDef '$defName' was not found."
    }
}
Pass "Vanilla RiverDefs required by Environment are present"

$river = $riverDefs.SelectSingleNode("/Defs/RiverDef[defName='River']")
$large = $riverDefs.SelectSingleNode("/Defs/RiverDef[defName='LargeRiver']")
$huge = $riverDefs.SelectSingleNode("/Defs/RiverDef[defName='HugeRiver']")

if ($null -eq $river.spawnFlowThreshold -or $null -eq $river.spawnChance) {
    Fail "Vanilla River is missing spawnFlowThreshold/spawnChance."
}
if ($null -eq $large.spawnFlowThreshold -or $null -eq $huge.spawnFlowThreshold) {
    Fail "Vanilla LargeRiver/HugeRiver is missing spawnFlowThreshold."
}
Pass "Vanilla river source structure matches Environment patch assumptions"

[xml]$worldGenerator = Get-Content -LiteralPath $WorldGeneratorPath
$terrainWorker = $worldGenerator.SelectSingleNode("/Defs/WorldGenStepDef[defName='Terrain']/worldGenStep")
if ($null -eq $terrainWorker) {
    Fail "Vanilla Terrain WorldGenStep was not found."
}
if ($terrainWorker.Class -ne "WorldGenStep_Terrain") {
    Fail "Unexpected Vanilla Terrain worker class: $($terrainWorker.Class)"
}
Pass "Vanilla Terrain WorldGenStep source structure matches Environment assumptions"

foreach ($relative in @(
    "About\About.xml",
    "Patches\Rivers.xml"
)) {
    $path = Join-Path $RepoRoot $relative
    if (-not (Test-Path $path)) {
        Fail "Required mod XML was not found: $relative"
    }

    try {
        [xml](Get-Content -LiteralPath $path) | Out-Null
    }
    catch {
        Fail "Invalid XML in $relative : $($_.Exception.Message)"
    }
}
Pass "Environment XML files are well formed"

[xml]$about = Get-Content -LiteralPath (Join-Path $RepoRoot "About\About.xml")
if ($about.ModMetaData.packageId -ne "sucro.ancientmedievaljapan.environment") {
    Fail "Unexpected packageId in About.xml."
}

$harmonyDep = $about.SelectSingleNode("/ModMetaData/modDependencies/li[packageId='brrainz.harmony']")
if ($null -eq $harmonyDep) {
    Fail "Harmony dependency is missing from About.xml."
}
Pass "About.xml package/dependency metadata is correct"

$riverPatchText = Get-Content -LiteralPath (Join-Path $RepoRoot "Patches\Rivers.xml") -Raw
foreach ($expected in @(
    '<spawnFlowThreshold>30000</spawnFlowThreshold>',
    '<spawnChance>0.70</spawnChance>',
    '<spawnFlowThreshold>75000</spawnFlowThreshold>',
    '<spawnChance>0.80</spawnChance>',
    '/Defs/RiverDef[defName="LargeRiver"]/spawnFlowThreshold',
    '/Defs/RiverDef[defName="HugeRiver"]/spawnFlowThreshold'
)) {
    if (-not $riverPatchText.Contains($expected)) {
        Fail "Rivers.xml is missing expected Alpha value/path: $expected"
    }
}
Pass "River Alpha patch values are present"

$worldPatchPath = Join-Path $RepoRoot "Patches\WorldGeneration.xml"
if (Test-Path $worldPatchPath) {
    Fail "Obsolete Patches\WorldGeneration.xml is still present. Terrain processing must not reference a custom WorldGenStep type from XML."
}

$terrainSourcePath = Join-Path $RepoRoot "Source\AncientMedievalJapanEnvironment\WorldGenStep_AMJEnvironmentTerrain.cs"
if (-not (Test-Path $terrainSourcePath)) {
    Fail "Environment terrain Harmony source was not found."
}

$terrainSource = Get-Content -LiteralPath $terrainSourcePath -Raw
if (-not $terrainSource.Contains('[HarmonyPatch(typeof(WorldGenStep_Terrain), "GenerateFresh")]')) {
    Fail "Environment terrain source does not patch Vanilla WorldGenStep_Terrain.GenerateFresh."
}
if (-not $terrainSource.Contains("EnvironmentTerrainProcessor.Apply(seed, layer);")) {
    Fail "Environment terrain Harmony postfix does not invoke the terrain processor."
}
Pass "Environment terrain processing uses Harmony postfix with no custom WorldGenStep XML type"

$bootstrapSourcePath = Join-Path $RepoRoot "Source\AncientMedievalJapanEnvironment\Bootstrap.cs"
if (-not (Test-Path $bootstrapSourcePath)) {
    Fail "Environment Bootstrap.cs was not found."
}
$bootstrapSource = Get-Content -LiteralPath $bootstrapSourcePath -Raw
if (-not $bootstrapSource.Contains("DailyVariationScale = 3f / 7f")) {
    Fail "Environment daily random temperature scale is not the calibrated 3/7 value."
}
Pass "Daily random temperature variation uses calibrated ~3 C scale"


$climateSourcePath = Join-Path $RepoRoot "Source\AncientMedievalJapanEnvironment\ClimateCalibrationDiagnostics.cs"
if (-not (Test-Path $climateSourcePath)) {
    Fail "Climate calibration diagnostics source was not found."
}

$climateSource = Get-Content -LiteralPath $climateSourcePath -Raw
foreach ($expected in @(
    '[HarmonyPatch(typeof(Game), "InitNewGame")]',
    'Find.TickManager.gameStartAbsTick == 0',
    'OutdoorTemperatureAt(tile, absTick)',
    '10f, 8f, 5f, 0f, -1f, -4f, -8f',
    '"WarmLowland"',
    '"TemperateLowland"',
    '"CoolLowland"',
    '"Highland"'
)) {
    if (-not $climateSource.Contains($expected)) {
        Fail "Climate calibration diagnostics are missing expected source marker: $expected"
    }
}
Pass "Automatic CCTO climate-calibration diagnostics are present"

$diagnosticsSourcePath = Join-Path $RepoRoot "Source\AncientMedievalJapanEnvironment\WorldGenDiagnostics.cs"
if (-not (Test-Path $diagnosticsSourcePath)) {
    Fail "WorldGenDiagnostics.cs was not found."
}
$diagnosticsSource = Get-Content -LiteralPath $diagnosticsSourcePath -Raw
foreach ($expected in @(
    '"[AMJ Environment] Japan vegetation-band preview"',
    'tile.temperature >= 15f',
    'tile.temperature >= 8f',
    'tile.temperature >= 0f',
    'tile.swampiness >= 0.5f'
)) {
    if (-not $diagnosticsSource.Contains($expected)) {
        Fail "Japan vegetation-band preview is missing expected source marker: $expected"
    }
}
Pass "Japan vegetation-band preview diagnostics are present"

$biomeDefsPath = Join-Path $RepoRoot "Defs\BiomeDefs\AMJ_Biomes.xml"
if (-not (Test-Path $biomeDefsPath)) {
    Fail "AMJ biome Defs were not found."
}

try {
    [xml]$biomeDefs = Get-Content -LiteralPath $biomeDefsPath -Raw
}
catch {
    Fail "AMJ biome Def XML is not well formed: $($_.Exception.Message)"
}

$expectedBiomeDefs = @(
    "AMJ_WarmTemperateForest",
    "AMJ_CoolTemperateForest",
    "AMJ_SubalpineForest",
    "AMJ_AlpineZone"
)
foreach ($defName in $expectedBiomeDefs) {
    if (-not ($biomeDefs.Defs.BiomeDef | Where-Object { $_.defName -eq $defName })) {
        Fail "Missing AMJ BiomeDef: $defName"
    }
}

$expectedWeather = @{
    "AMJ_WarmTemperateForest" = @{
        "Clear" = 16.0
        "Fog" = 1.5
        "Rain" = 3.0
        "DryThunderstorm" = 0.1
        "RainyThunderstorm" = 1.5
        "FoggyRain" = 1.5
        "SnowGentle" = 2.0
        "SnowHard" = 1.0
    }
    "AMJ_CoolTemperateForest" = @{
        "Clear" = 16.0
        "Fog" = 1.5
        "Rain" = 3.0
        "DryThunderstorm" = 0.1
        "RainyThunderstorm" = 1.5
        "FoggyRain" = 1.5
        "SnowGentle" = 4.0
        "SnowHard" = 3.0
    }
    "AMJ_SubalpineForest" = @{
        "Clear" = 16.0
        "Fog" = 1.0
        "Rain" = 2.0
        "DryThunderstorm" = 0.1
        "RainyThunderstorm" = 1.0
        "FoggyRain" = 1.0
        "SnowGentle" = 7.0
        "SnowHard" = 7.0
    }
    "AMJ_AlpineZone" = @{
        "Clear" = 16.0
        "Fog" = 1.0
        "Rain" = 1.0
        "DryThunderstorm" = 0.05
        "RainyThunderstorm" = 0.5
        "FoggyRain" = 0.5
        "SnowGentle" = 12.0
        "SnowHard" = 12.0
    }
}

foreach ($defName in $expectedBiomeDefs) {
    $biome = $biomeDefs.Defs.BiomeDef | Where-Object { $_.defName -eq $defName }
    $weatherNodes = @($biome.baseWeatherCommonalities.ChildNodes | Where-Object { $_.NodeType -eq "Element" })

    if ($weatherNodes.Count -ne 8) {
        Fail "$defName must define exactly 8 baseline weather commonalities."
    }

    foreach ($weatherName in $expectedWeather[$defName].Keys) {
        $node = $biome.baseWeatherCommonalities.SelectSingleNode($weatherName)
        if ($null -eq $node) {
            Fail "$defName is missing weather commonality: $weatherName"
        }

        $actual = [double]::Parse(
            $node.InnerText,
            [System.Globalization.CultureInfo]::InvariantCulture)
        $expected = [double]$expectedWeather[$defName][$weatherName]
        if ([Math]::Abs($actual - $expected) -gt 0.0001) {
            Fail "$defName weather commonality mismatch for $weatherName : expected $expected, got $actual"
        }
    }

    if ([double]$expectedWeather[$defName]["RainyThunderstorm"] -le
        [double]$expectedWeather[$defName]["DryThunderstorm"]) {
        Fail "$defName must prefer rainy thunderstorms over dry thunderstorms."
    }
}
Pass "AMJ regional weather baselines use the accepted humid-Japan Alpha weights"

$biomeWorkerSourcePath = Join-Path $RepoRoot "Source\AncientMedievalJapanEnvironment\JapanBiomeWorkers.cs"
if (-not (Test-Path $biomeWorkerSourcePath)) {
    Fail "JapanBiomeWorkers.cs was not found."
}
$biomeWorkerSource = Get-Content -LiteralPath $biomeWorkerSourcePath -Raw
foreach ($expected in @(
    "BiomeWorker_AMJWarmTemperateForest",
    "BiomeWorker_AMJCoolTemperateForest",
    "BiomeWorker_AMJSubalpineForest",
    "BiomeWorker_AMJAlpineZone",
    "WetlandThreshold = 0.5f",
    "return 38f"
)) {
    if (-not $biomeWorkerSource.Contains($expected)) {
        Fail "Japan biome worker source is missing expected marker: $expected"
    }
}
Pass "AMJ Alpha vegetation-band BiomeDefs and workers are present"

$biomeJaPath = Join-Path $RepoRoot "Languages\Japanese\DefInjected\BiomeDef\AMJ_Biomes.xml"
if (-not (Test-Path $biomeJaPath)) {
    Fail "Japanese localization for AMJ biomes was not found."
}
try {
    [xml]$biomeJa = Get-Content -LiteralPath $biomeJaPath -Raw -Encoding UTF8
}
catch {
    Fail "Japanese AMJ biome localization XML is not well formed: $($_.Exception.Message)"
}
foreach ($defName in $expectedBiomeDefs) {
    if ($null -eq $biomeJa.LanguageData.($defName + ".label")) {
        Fail "Missing Japanese label for AMJ biome: $defName"
    }
    if ($null -eq $biomeJa.LanguageData.($defName + ".description")) {
        Fail "Missing Japanese description for AMJ biome: $defName"
    }
}
Pass "All four AMJ biomes have Japanese labels and descriptions"

$forbiddenWildlife = @(
    "Raccoon",
    "Elk",
    "Ibex",
    "Fox_Arctic",
    "Lynx"
)
foreach ($defName in $expectedBiomeDefs) {
    $biome = $biomeDefs.Defs.BiomeDef | Where-Object { $_.defName -eq $defName }
    foreach ($animalDefName in $forbiddenWildlife) {
        if ($null -ne $biome.wildAnimals.($animalDefName)) {
            Fail "$defName still contains non-Japan Alpha wildlife placeholder: $animalDefName"
        }
    }
}
Pass "AMJ biomes exclude the retired foreign/modern wildlife placeholders"

$expectedWildlife = @{
    "AMJ_WarmTemperateForest" = @(
        "Hare", "Squirrel", "Rat", "Deer", "WildBoar",
        "Fox_Red", "Wolf_Timber", "Bear_Grizzly"
    )
    "AMJ_CoolTemperateForest" = @(
        "Hare", "Squirrel", "Rat", "Deer", "WildBoar",
        "Fox_Red", "Wolf_Timber", "Bear_Grizzly"
    )
    "AMJ_SubalpineForest" = @(
        "Hare", "Snowhare", "Deer", "WildBoar",
        "Fox_Red", "Wolf_Timber", "Bear_Grizzly"
    )
    "AMJ_AlpineZone" = @(
        "Hare", "Snowhare", "Deer", "Fox_Red", "Wolf_Timber"
    )
}

foreach ($defName in $expectedBiomeDefs) {
    $biome = $biomeDefs.Defs.BiomeDef | Where-Object { $_.defName -eq $defName }
    foreach ($animalDefName in $expectedWildlife[$defName]) {
        $node = $biome.wildAnimals.($animalDefName)
        if ($null -eq $node) {
            Fail "$defName is missing accepted Vanilla wildlife proxy: $animalDefName"
        }

        $commonality = [double]::Parse(
            $node.InnerText,
            [System.Globalization.CultureInfo]::InvariantCulture)
        if ($commonality -le 0) {
            Fail "$defName wildlife proxy must have positive commonality: $animalDefName"
        }
    }
}
Pass "AMJ biomes keep the accepted Vanilla wildlife proxy pools"



$wildPlantsPath = Join-Path $RepoRoot "Defs\ThingDefs_Plants\AMJ_WildPlants.xml"
if (-not (Test-Path $wildPlantsPath)) {
    Fail "AMJ Japan-specific wild plant Defs were not found."
}
try {
    [xml]$wildPlants = Get-Content -LiteralPath $wildPlantsPath -Raw
}
catch {
    Fail "AMJ wild plant XML is not well formed: $($_.Exception.Message)"
}
$expectedWildPlants = @(
    "AMJ_Tree_Shii",
    "AMJ_Tree_Beech",
    "AMJ_Tree_Shirabiso",
    "AMJ_Shrub_Haimatsu"
)
foreach ($defName in $expectedWildPlants) {
    if (-not ($wildPlants.Defs.ThingDef | Where-Object { $_.defName -eq $defName })) {
        Fail "Missing AMJ wild PlantDef: $defName"
    }
}
Pass "Four Japan-specific structural wild PlantDefs are present"

foreach ($defName in $expectedWildPlants) {
    $plantDef = $wildPlants.Defs.ThingDef | Where-Object { $_.defName -eq $defName }
    if ($null -eq $plantDef.plant.minGrowthTemperature) {
        Fail "AMJ wild PlantDef does not explicitly declare Vanilla baseline minGrowthTemperature: $defName"
    }
    if ([double]$plantDef.plant.minGrowthTemperature -ne 0) {
        Fail "AMJ wild PlantDef Vanilla baseline minGrowthTemperature must be 0 C: $defName"
    }
}
Pass "AMJ wild plants explicitly preserve the Vanilla-style 0 C growth baseline"

$wildPlantRaw = Get-Content -LiteralPath $wildPlantsPath -Raw
foreach ($expected in @(
    '<visualSizeRange Inherit="False">',
    '<min>1.25</min>',
    '<max>2.7</max>',
    '<min>0.45</min>',
    '<max>0.75</max>'
)) {
    if (-not $wildPlantRaw.Contains($expected)) {
        Fail "AMJ wild plant inherited visual-size override is missing expected marker: $expected"
    }
}
Pass "Custom Shirabiso/Haimatsu visual-size ranges replace inherited FloatRange nodes cleanly"

foreach ($expected in @(
    '<ThingDef ParentName="DeciduousTreeBase">',
    '<defName>AMJ_Tree_Beech</defName>',
    '<leaflessGraphicPath>Things/Plant/TreeMaple_Leafless</leaflessGraphicPath>',
    '<ThingDef ParentName="TreeBase">',
    '<defName>AMJ_Tree_Shii</defName>',
    '<defName>AMJ_Tree_Shirabiso</defName>',
    '<ThingDef ParentName="BushBase">',
    '<defName>AMJ_Shrub_Haimatsu</defName>'
)) {
    if (-not $wildPlantRaw.Contains($expected)) {
        Fail "AMJ seasonal-scenery plant profile is missing expected marker: $expected"
    }
}
Pass "AMJ seasonal plant profiles preserve one deciduous representative and three evergreen structural plants"

$shiiTexturePath = Join-Path $RepoRoot "Textures\Things\Plant\AMJ\Shii\Shii_A.png"
if (-not (Test-Path -LiteralPath $shiiTexturePath)) {
    Fail "Final Sudajii texture is missing: Textures/Things/Plant/AMJ/Shii/Shii_A.png"
}
if (-not $wildPlantRaw.Contains('<texPath>Things/Plant/AMJ/Shii</texPath>')) {
    Fail "AMJ_Tree_Shii does not point to the final AMJE Sudajii texture folder."
}
if ($wildPlantRaw.Contains('<defName>AMJ_Tree_Shii</defName>') -and
    $wildPlantRaw.Contains('<texPath>Things/Plant/TreeOak</texPath>')) {
    Fail "AMJ_Tree_Shii still references the Vanilla TreeOak placeholder."
}
Pass "Final Sudajii texture exists and AMJ_Tree_Shii uses the AMJE-owned path"




$cctoPatchPath = Join-Path $RepoRoot "Patches\Compatibility\CCTO.xml"
if (-not (Test-Path -LiteralPath $cctoPatchPath)) {
    Fail "Optional CCTO compatibility patch was not found."
}
try {
    [xml]$cctoPatchXml = Get-Content -LiteralPath $cctoPatchPath -Raw
}
catch {
    Fail "Optional CCTO compatibility XML is not well formed: $($_.Exception.Message)"
}
$cctoPatchRaw = Get-Content -LiteralPath $cctoPatchPath -Raw
foreach ($expected in @(
    'PatchOperationSequence',
    'MayRequire="sucro.cropcoldtoleranceoverhaul"',
    'AMJ_Tree_Shii',
    '<minGrowthTemperature>8</minGrowthTemperature>',
    '<coldDeathTemperature>-8</coldDeathTemperature>',
    'AMJ_Tree_Beech',
    '<minGrowthTemperature>5</minGrowthTemperature>',
    '<coldDormancy>true</coldDormancy>',
    'AMJ_Tree_Shirabiso',
    '<coldDeathTemperature>-35</coldDeathTemperature>',
    'AMJ_Shrub_Haimatsu',
    'CropColdToleranceOverhaul.ColdToleranceExtension'
)) {
    if (-not $cctoPatchRaw.Contains($expected)) {
        Fail "Optional CCTO compatibility patch is missing expected marker: $expected"
    }
}
Pass "AMJE owns a conditional CCTO compatibility layer for its four wild plants"


$biomeDefsRawForPlants = Get-Content -LiteralPath $biomeDefsPath -Raw
foreach ($expected in @(
    '<AMJ_Tree_Shii>2.0</AMJ_Tree_Shii>',
    '<AMJ_Tree_Beech>1.8</AMJ_Tree_Beech>',
    '<AMJ_Tree_Shirabiso>2.6</AMJ_Tree_Shirabiso>',
    '<AMJ_Shrub_Haimatsu>1.3</AMJ_Shrub_Haimatsu>'
)) {
    if (-not $biomeDefsRawForPlants.Contains($expected)) {
        Fail "AMJ biome wild-plant composition is missing expected marker: $expected"
    }
}
Pass "Japan-specific structural plants are wired into their target biomes"

$wildPlantJaPath = Join-Path $RepoRoot "Languages\Japanese\DefInjected\ThingDef\AMJ_WildPlants.xml"
if (-not (Test-Path $wildPlantJaPath)) {
    Fail "Japanese localization for AMJ wild plants was not found."
}
try {
    [xml]$wildPlantJa = Get-Content -LiteralPath $wildPlantJaPath -Raw -Encoding UTF8
}
catch {
    Fail "Japanese wild-plant localization XML is not well formed: $($_.Exception.Message)"
}
foreach ($defName in $expectedWildPlants) {
    if ($null -eq $wildPlantJa.LanguageData.($defName + ".label")) {
        Fail "Missing Japanese label for AMJ wild plant: $defName"
    }
}
Pass "Japan-specific wild plants have Japanese labels"


$naturalTerrainPath = Join-Path $RepoRoot "Defs\TerrainDefs\AMJ_NaturalTerrains.xml"
if (-not (Test-Path $naturalTerrainPath)) {
    Fail "AMJ natural terrain Defs were not found."
}
try {
    [xml]$naturalTerrains = Get-Content -LiteralPath $naturalTerrainPath -Raw
}
catch {
    Fail "AMJ natural terrain XML is not well formed: $($_.Exception.Message)"
}
$thinSoil = $naturalTerrains.Defs.TerrainDef | Where-Object { $_.defName -eq "AMJ_ThinSoil" }
if (-not $thinSoil) {
    Fail "Missing AMJ_ThinSoil TerrainDef."
}
if ([double]$thinSoil.fertility -ne 0.50) {
    Fail "AMJ_ThinSoil fertility must be 0.50."
}
Pass "AMJ thin-soil natural terrain is present at fertility 0.50"

$thinSoilRaw = Get-Content -LiteralPath (Join-Path $RepoRoot "Defs\TerrainDefs\AMJ_NaturalTerrains.xml") -Raw
foreach ($expected in @(
    '<TerrainDef ParentName="NaturalTerrainBase">',
    '<categoryType>Soil</categoryType>',
    '<li>Soil</li>',
    '<generatedFilth>Filth_Dirt</generatedFilth>'
)) {
    if (-not $thinSoilRaw.Contains($expected)) {
        Fail "AMJ_ThinSoil is missing Vanilla-compatible natural-soil marker: $expected"
    }
}
Pass "AMJ thin soil inherits Vanilla natural-terrain filth acceptance"


$biomeDefsRaw = Get-Content -LiteralPath $biomeDefsPath -Raw
foreach ($expected in @(
    "<terrain>AMJ_ThinSoil</terrain>",
    "<terrain>Gravel</terrain>",
    "<max>0.30</max>",
    "<max>0.35</max>",
    "<max>0.50</max>",
    "<max>0.65</max>"
)) {
    if (-not $biomeDefsRaw.Contains($expected)) {
        Fail "AMJ biome soil distribution is missing expected marker: $expected"
    }
}
Pass "AMJ biome low-fertility terrain thresholds are present"

$moTerrainPatchPath = Join-Path $RepoRoot "Patches\Compatibility\MedievalOverhaul.xml"
if (-not (Test-Path $moTerrainPatchPath)) {
    Fail "Medieval Overhaul terrain compatibility patch was not found."
}
$moTerrainPatch = Get-Content -LiteralPath $moTerrainPatchPath -Raw
foreach ($expected in @(
    'MayRequire="DankPyon.Medieval.Overhaul"',
    'PatchOperationSequence',
    '<li Class="PatchOperationReplace" MayRequire="DankPyon.Medieval.Overhaul">',
    'DankPyon_DarkForest',
    'AMJ_ThinSoil',
    '<max>0.40</max>',
    '<max>0.60</max>',
    '<max>0.90</max>'
)) {
    if (-not $moTerrainPatch.Contains($expected)) {
        Fail "MO natural-soil compatibility patch is missing expected marker: $expected"
    }
}
Pass "MO Dark Forest natural-soil compatibility is present"

$mapTerrainDiagnosticsPath = Join-Path $RepoRoot "Source\AncientMedievalJapanEnvironment\MapTerrainDiagnostics.cs"
if (-not (Test-Path $mapTerrainDiagnosticsPath)) {
    Fail "MapTerrainDiagnostics.cs was not found."
}
$mapTerrainDiagnostics = Get-Content -LiteralPath $mapTerrainDiagnosticsPath -Raw
foreach ($expected in @(
    '[HarmonyPatch(typeof(MapGenerator), "GenerateMap")]',
    '"[AMJ Environment] Map terrain summary"',
    '"AMJ_ThinSoil"',
    '"DankPyon_DarkForest"',
    '" | OtherTop="',
    'Dictionary<string, int> otherTerrains'
)) {
    if (-not $mapTerrainDiagnostics.Contains($expected)) {
        Fail "Map terrain diagnostics are missing expected marker: $expected"
    }
}
Pass "Map terrain-share diagnostics are present"

$loadFoldersPath = Join-Path $RepoRoot "loadFolders.xml"
if (-not (Test-Path $loadFoldersPath)) {
    Fail "loadFolders.xml was not found."
}
$loadFoldersRaw = Get-Content -LiteralPath $loadFoldersPath -Raw
foreach ($expected in @(
    '<li>/</li>',
    '<li IfModActive="rimworks.quickstarts">DevQuickstarts</li>'
)) {
    if (-not $loadFoldersRaw.Contains($expected)) {
        Fail "loadFolders.xml is missing expected Quickstarts gating marker: $expected"
    }
}
Pass "Developer Quickstarts are gated behind rimworks.quickstarts"

$bootstrapPath = Join-Path $RepoRoot "Source\AncientMedievalJapanEnvironment\Bootstrap.cs"
$bootstrapSource = Get-Content -LiteralPath $bootstrapPath -Raw
foreach ($expected in @(
    '[AMJ Environment] Dev load-folder diagnostic',
    'GetActiveModWithIdentifier("rimworks.quickstarts", true)',
    'foldersToLoadDescendingOrder'
)) {
    if (-not $bootstrapSource.Contains($expected)) {
        Fail "Bootstrap is missing expected dev load-folder diagnostic marker: $expected"
    }
}
Pass "Runtime load-folder diagnostic is present"


$aboutRaw = Get-Content -LiteralPath (Join-Path $RepoRoot "About\About.xml") -Raw
if (-not $aboutRaw.Contains('<li>rimworks.quickstarts</li>')) {
    Fail "About.xml must load after Quickstarts when that mod is active."
}
if (-not $aboutRaw.Contains('<li>sucro.cropcoldtoleranceoverhaul</li>')) {
    Fail "About.xml must load after CCTO when that optional compatibility target is active."
}
Pass "Environment declares optional Quickstarts/CCTO load order"

$quicktestSourcePath = Join-Path $RepoRoot "Tests\Quickstarts\EnvironmentBiomeTerrainQuickstarts.cs"
if (-not (Test-Path $quicktestSourcePath)) {
    Fail "Fixed-biome Quickstart source was not found."
}
$quicktestSource = Get-Content -LiteralPath $quicktestSourcePath -Raw
foreach ($expected in @(
    'AMJWarmTemperateTerrainQuickstart',
    'AMJCoolTemperateTerrainQuickstart',
    'AMJSubalpineTerrainQuickstart',
    'AMJAlpineTerrainQuickstart',
    'AMJDarkForestTerrainQuickstart',
    'AMJRiverMapHandoffQuickstart',
    'AMJCoastMapHandoffQuickstart',
    'TileMutatorDefOf.River',
    'TileMutatorDefOf.Coast',
    '"[AMJ Environment WaterHandoff] verified"',
    'terrain.IsRiver',
    'terrain.IsOcean',
    'Find.GameInitData.startingTile = tile;',
    'Hilliness.Flat',
    'Hilliness.Impassable',
    'forcedNonSettlementTile',
    'forcedBiome',
    'targetBiome.Worker.GetScore',
    'proxy.PrimaryBiome = targetBiome;',
    'FindClosestClimateProxy',
    'TargetRepresentativeTemperature',
    'forcedClimate',
    'proxy.temperature = forcedTemperature;',
    'Find.WorldObjects.AnyWorldObjectAt(tile)',
    'AMJ-Environment-Terrain-Alpha',
    'QuickstartVerification Verify()',
    'AMJ_Tree_Shii',
    'AMJ_Tree_Beech',
    'AMJ_Tree_Shirabiso',
    'AMJ_Shrub_Haimatsu',
    'MaxTargetCellFraction',
    '"[AMJ Environment Vegetation] biome="',
    'AddWeatherAssertions',
    'ExpectedWeatherCommonality',
    '"[AMJ Environment Weather] biome="',
    'AddSeasonalSceneryAssertions',
    'AddWildlifeAssertions',
    'ExpectedWildlifeForBiome',
    'excludes wildlife placeholder',
    'keeps wildlife proxy',
    'Japanese beech has a loaded leafless graphic',
    '_FallBehaviorEnabled',
    'Vanilla snow weather remains available for seasonal scenery',
    'CctoIsActive()',
    'ModLister.GetActiveModWithIdentifier',
    'AddCctoCompatibilityAssertions',
    'CropColdToleranceOverhaul.ColdToleranceExtension',
    'AMJ_Tree_Shii',
    'AMJ_Tree_Beech',
    'AMJ_Tree_Shirabiso',
    'AMJ_Shrub_Haimatsu'
)) {
    if (-not $quicktestSource.Contains($expected)) {
        Fail "Fixed-biome Quickstart source is missing expected marker: $expected"
    }
}
Pass "Five deterministic fixed-biome Quickstarts are present"

foreach ($relative in @(
    "run-runtime-tests.bat",
    "Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1",
    "Scripts\Run-EnvironmentVegetationQuickstarts.ps1",
    "Scripts\Validate-EnvironmentRuntimeLog.ps1"
)) {
    $path = Join-Path $RepoRoot $relative
    if (-not (Test-Path -LiteralPath $path)) {
        Fail "Environment runtime-test harness file is missing: $relative"
    }
}

$runtimeBatch = Get-Content -LiteralPath (Join-Path $RepoRoot "run-runtime-tests.bat") -Raw
foreach ($expected in @(
    'run-tests.bat',
    'Run-EnvironmentVegetationQuickstarts.ps1',
    'TestResults\VegetationRuntime',
    'CCTO_INSTALLED',
    'SaveData-CCTO',
    'Reports-CCTO',
    '-IncludeCCTO',
    '-CctoCompatibilityOnly'
)) {
    if (-not $runtimeBatch.Contains($expected)) {
        Fail "run-runtime-tests.bat is missing expected marker: $expected"
    }
}

$runtimeRunner = Get-Content -LiteralPath (Join-Path $RepoRoot "Scripts\Run-EnvironmentVegetationQuickstarts.ps1") -Raw
foreach ($expected in @(
    'AMJWarmTemperateTerrainQuickstart',
    'AMJCoolTemperateTerrainQuickstart',
    'AMJSubalpineTerrainQuickstart',
    'AMJAlpineTerrainQuickstart',
    'AMJRiverMapHandoffQuickstart',
    'AMJCoastMapHandoffQuickstart',
    'quickstartreport',
    'Validate-EnvironmentRuntimeLog.ps1',
    'CctoCompatibilityOnly',
    'preLaunchErrors'
)) {
    if (-not $runtimeRunner.Contains($expected)) {
        Fail "Vegetation runtime runner is missing expected marker: $expected"
    }
}

$runtimeLogValidator = Get-Content -LiteralPath (Join-Path $RepoRoot "Scripts\Validate-EnvironmentRuntimeLog.ps1") -Raw
foreach ($expected in @(
    'sucro.ancientmedievaljapan.environment',
    'Level:\s*ERROR',
    'Environment-origin runtime ERROR'
)) {
    if (-not $runtimeLogValidator.Contains($expected)) {
        Fail "Environment runtime ERROR gate is missing expected marker: $expected"
    }
}
Pass "Automated four-biome vegetation runtime harness and mod-origin ERROR gate are present"


$steamapps = [System.IO.Path]::GetFullPath((Join-Path $RimWorldDir "..\.."))
$quickstartsRoot = Join-Path $steamapps "workshop\content\294100\3793646067"
if (Test-Path $quickstartsRoot) {
    $quickstartsDll = Get-ChildItem -Path $quickstartsRoot -Filter "Quickstarts.dll" -Recurse -File -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($null -ne $quickstartsDll) {
        $quicktestDll = Join-Path $RepoRoot "DevQuickstarts\Assemblies\AncientMedievalJapanEnvironment.Quicktests.dll"
        if (-not (Test-Path $quicktestDll)) {
            Fail "Quickstarts is installed but the Environment fixed-biome Quicktest DLL was not built."
        }
        Pass "Fixed-biome Quicktest DLL was built against installed Quickstarts"
    }
}





$publicDocs = @(
    "README.md",
    "Docs\WorkshopDescription.md",
    "Docs\SteamWorkshopDescription-ja.txt",
    "Docs\SteamWorkshopDescription.txt"
)

foreach ($relative in $publicDocs) {
    $path = Join-Path $RepoRoot $relative
    if (-not (Test-Path -LiteralPath $path)) {
        Fail "Public description source is missing: $relative"
    }
}

$readmeRaw = Get-Content -LiteralPath (Join-Path $RepoRoot "README.md") -Raw -Encoding UTF8
foreach ($expected in @(
    'RimWorld 1.6',
    'Alpha',
    'Ancient & Medieval Japan Core is not required',
    'Crop Cold Tolerance Overhaul is optional',
    'Use a new game',
    'Removing the mod from a save'
)) {
    if (-not $readmeRaw.Contains($expected)) {
        Fail "README is missing required public-positioning marker: $expected"
    }
}

$workshopJaPath = Join-Path $RepoRoot "Docs\SteamWorkshopDescription-ja.txt"
$workshopEnPath = Join-Path $RepoRoot "Docs\SteamWorkshopDescription.txt"
$workshopJaRaw = Get-Content -LiteralPath $workshopJaPath -Raw -Encoding UTF8
$workshopEnRaw = Get-Content -LiteralPath $workshopEnPath -Raw -Encoding UTF8

if ([System.Text.Encoding]::UTF8.GetByteCount($workshopJaRaw) -gt 8000) {
    Fail "Japanese Workshop description exceeds 8,000 UTF-8 bytes."
}
if ([System.Text.Encoding]::UTF8.GetByteCount($workshopEnRaw) -gt 8000) {
    Fail "English Workshop description exceeds 8,000 UTF-8 bytes."
}

foreach ($pair in @(
    @($workshopJaRaw, '現在はAlphaです。', 'Japanese Workshop Alpha stage'),
    @($workshopJaRaw, 'Ancient & Medieval Japan Coreは不要です。', 'Japanese Workshop Core independence'),
    @($workshopJaRaw, 'Crop Cold Tolerance Overhaul（CCTO）は任意です。', 'Japanese Workshop CCTO optionality'),
    @($workshopEnRaw, 'Currently Alpha.', 'English Workshop Alpha stage'),
    @($workshopEnRaw, 'Ancient & Medieval Japan Core is not required.', 'English Workshop Core independence'),
    @($workshopEnRaw, 'Crop Cold Tolerance Overhaul (CCTO) is optional.', 'English Workshop CCTO optionality')
)) {
    if (-not $pair[0].Contains($pair[1])) {
        Fail "$($pair[2]) marker is missing."
    }
}

$aboutPublicRaw = Get-Content -LiteralPath (Join-Path $RepoRoot "About\About.xml") -Raw -Encoding UTF8
if ($aboutPublicRaw.Contains('Development build')) {
    Fail "About.xml still describes AMJE as a Development build."
}
foreach ($expected in @(
    '<description>Alpha.',
    'Ancient &amp; Medieval Japan Core is not required.',
    'Crop Cold Tolerance Overhaul (CCTO) is optional',
    'A new game is recommended',
    'Removing this mod from saves'
)) {
    if (-not $aboutPublicRaw.Contains($expected)) {
        Fail "About.xml is missing required public-positioning marker: $expected"
    }
}
Pass "README / Workshop / About public-positioning sources are present and size-safe"

Write-Host ""
Write-Host "[OK] AMJ Environment static validation passed"
exit 0
