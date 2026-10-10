param(
    [Parameter(Mandatory = $true)]
    [string]$RimWorldDir
)

$ErrorActionPreference = "Stop"

$RepoRoot = Split-Path -Parent $PSScriptRoot
. (Join-Path $PSScriptRoot 'PngImageData.ps1')
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

function Get-PngCrc32(
    [byte[]]$Bytes,
    [int]$Offset,
    [int]$Count
) {
    [uint32]$crc = 4294967295

    for ($i = $Offset; $i -lt ($Offset + $Count); $i++) {
        [uint32]$value = [uint32]($crc -bxor [uint32]$Bytes[$i])

        for ($bit = 0; $bit -lt 8; $bit++) {
            if (($value -band 1) -ne 0) {
                $value = [uint32](([uint32]($value -shr 1)) -bxor [uint32]3988292384)
            }
            else {
                $value = [uint32]($value -shr 1)
            }
        }

        $crc = $value
    }

    return [uint32]($crc -bxor [uint32]4294967295)
}

function Test-PngStructure([string]$Path) {
    [byte[]]$bytes = [System.IO.File]::ReadAllBytes($Path)
    [byte[]]$signature = @(137, 80, 78, 71, 13, 10, 26, 10)

    if ($bytes.Length -lt 20) {
        return $false
    }

    for ($i = 0; $i -lt $signature.Length; $i++) {
        if ($bytes[$i] -ne $signature[$i]) {
            return $false
        }
    }

    [int64]$offset = 8
    $firstChunk = $true
    $sawIend = $false

    while (($offset + 12) -le $bytes.Length) {
        [uint64]$length =
            ([uint64]$bytes[$offset] * 16777216) +
            ([uint64]$bytes[$offset + 1] * 65536) +
            ([uint64]$bytes[$offset + 2] * 256) +
            [uint64]$bytes[$offset + 3]

        [int64]$dataEnd = $offset + 8 + [int64]$length
        [int64]$chunkEnd = $dataEnd + 4
        if ($chunkEnd -gt $bytes.Length) {
            return $false
        }

        $chunkType =
            [System.Text.Encoding]::ASCII.GetString(
                $bytes,
                [int]$offset + 4,
                4)

        if ($firstChunk -and $chunkType -ne "IHDR") {
            return $false
        }
        $firstChunk = $false

        [uint32]$storedCrc =
            ([uint32]$bytes[$dataEnd] * 16777216) +
            ([uint32]$bytes[$dataEnd + 1] * 65536) +
            ([uint32]$bytes[$dataEnd + 2] * 256) +
            [uint32]$bytes[$dataEnd + 3]

        [uint32]$computedCrc = Get-PngCrc32 $bytes ([int]$offset + 4) ([int]$length + 4)
        if ($storedCrc -ne $computedCrc) {
            return $false
        }

        if ($chunkType -eq "IEND") {
            if ($length -ne 0 -or $chunkEnd -ne $bytes.Length) {
                return $false
            }
            $sawIend = $true
            break
        }

        $offset = $chunkEnd
    }

    if (-not $sawIend) { return $false }
    try { [AMJPngImageData]::Validate($bytes) }
    catch { return $false }
    return $true
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

[xml]$riverDefs = Get-Content -LiteralPath $RiverDefsPath -Raw -Encoding UTF8
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

[xml]$worldGenerator = Get-Content -LiteralPath $WorldGeneratorPath -Raw -Encoding UTF8
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
        [xml](Get-Content -LiteralPath $path -Raw -Encoding UTF8) | Out-Null
    }
    catch {
        Fail "Invalid XML in $relative : $($_.Exception.Message)"
    }
}
Pass "Environment XML files are well formed"

[xml]$about = Get-Content -LiteralPath (Join-Path $RepoRoot "About\About.xml") -Raw -Encoding UTF8
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
        $node = $biome.wildAnimals.SelectSingleNode($animalDefName)
        if ($null -eq $node) {
            Fail "$defName is missing accepted Vanilla wildlife proxy: $animalDefName"
        }

        $commonality = [double]::Parse(
            $node.InnerText.Trim(),
            [System.Globalization.NumberStyles]::Float,
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
    '<leaflessGraphicPath>Things/Plant/AMJ/Beech_Leafless</leaflessGraphicPath>',
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
if (-not (Test-PngStructure $shiiTexturePath)) {
    Fail "Final Sudajii PNG is structurally invalid: Textures/Things/Plant/AMJ/Shii/Shii_A.png"
}

$textureRoot = Join-Path $RepoRoot "Textures"
if (Test-Path -LiteralPath $textureRoot) {
    $pngFiles =
        Get-ChildItem -LiteralPath $textureRoot -Filter "*.png" -Recurse -File

    foreach ($pngFile in $pngFiles) {
        if (-not (Test-PngStructure $pngFile.FullName)) {
            $relativePng =
                $pngFile.FullName.Substring($RepoRoot.Length).TrimStart("\")
            Fail "AMJE PNG is structurally invalid: $relativePng"
        }
    }

    Pass "AMJE PNG assets have valid chunk structure"
}
if (-not $wildPlantRaw.Contains('<texPath>Things/Plant/AMJ/Shii</texPath>')) {
    Fail "AMJ_Tree_Shii does not point to the final AMJE Sudajii texture folder."
}
if ($wildPlantRaw.Contains('<defName>AMJ_Tree_Shii</defName>') -and
    $wildPlantRaw.Contains('<texPath>Things/Plant/TreeOak</texPath>')) {
    Fail "AMJ_Tree_Shii still references the Vanilla TreeOak placeholder."
}
Pass "Final Sudajii texture exists and AMJ_Tree_Shii uses the AMJE-owned path"

$beechLeafyPath = Join-Path $RepoRoot "Textures\Things\Plant\AMJ\Beech\Beech_A.png"
$beechLeaflessPath = Join-Path $RepoRoot "Textures\Things\Plant\AMJ\Beech_Leafless\Beech_Leafless_A.png"
foreach ($beechTexture in @(
    @{ Path = $beechLeafyPath; Label = "leafy" },
    @{ Path = $beechLeaflessPath; Label = "leafless" }
)) {
    if (-not (Test-Path -LiteralPath $beechTexture.Path)) {
        Fail "Final Japanese beech $($beechTexture.Label) texture is missing: $($beechTexture.Path)"
    }
    if (-not (Test-PngStructure $beechTexture.Path)) {
        Fail "Final Japanese beech $($beechTexture.Label) PNG is structurally invalid: $($beechTexture.Path)"
    }
}
if (-not $wildPlantRaw.Contains('<texPath>Things/Plant/AMJ/Beech</texPath>')) {
    Fail "AMJ_Tree_Beech does not point to the final AMJE leafy beech texture folder."
}
if (-not $wildPlantRaw.Contains('<leaflessGraphicPath>Things/Plant/AMJ/Beech_Leafless</leaflessGraphicPath>')) {
    Fail "AMJ_Tree_Beech does not point to the final AMJE leafless beech texture folder."
}
if ($wildPlantRaw.Contains('<texPath>Things/Plant/TreeMaple</texPath>') -or
    $wildPlantRaw.Contains('<leaflessGraphicPath>Things/Plant/TreeMaple_Leafless</leaflessGraphicPath>')) {
    Fail "AMJ_Tree_Beech still references Vanilla TreeMaple placeholder graphics."
}
Pass "Final Japanese beech leafy/leafless textures exist and AMJ_Tree_Beech uses AMJE-owned paths"





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


# Pin accepted post-retention-audit commonalities to their owning biomes,
# rather than searching for old literal XML fragments anywhere in the file.
$structuralPlantCommonality = @(
    @{ Biome = 'AMJ_WarmTemperateForest'; Plant = 'AMJ_Tree_Shii'; Commonality = 2.55 },
    @{ Biome = 'AMJ_CoolTemperateForest'; Plant = 'AMJ_Tree_Beech'; Commonality = 1.8 },
    @{ Biome = 'AMJ_SubalpineForest'; Plant = 'AMJ_Tree_Shirabiso'; Commonality = 3.5 },
    @{ Biome = 'AMJ_AlpineZone'; Plant = 'AMJ_Shrub_Haimatsu'; Commonality = 1.34 }
)
foreach ($entry in $structuralPlantCommonality) {
    $xpath = "/Defs/BiomeDef[defName='$($entry.Biome)']/wildPlants/$($entry.Plant)"
    $nodes = @($biomeDefs.SelectNodes($xpath))
    if ($nodes.Count -ne 1) {
        Fail "AMJ biome $($entry.Biome) must contain exactly one $($entry.Plant) wild-plant entry (found $($nodes.Count))."
    }
    $actual = [double]::Parse(
        $nodes[0].InnerText,
        [System.Globalization.CultureInfo]::InvariantCulture
    )
    if ([Math]::Abs($actual - [double]$entry.Commonality) -gt 0.000001) {
        Fail "AMJ biome $($entry.Biome) plant $($entry.Plant) commonality mismatch: expected $($entry.Commonality), actual $actual."
    }
}
Pass "Japan-specific structural plants match the approved biome-specific commonalities"

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
try {
    [xml]$loadFoldersXml = Get-Content -LiteralPath $loadFoldersPath -Raw -Encoding UTF8
}
catch {
    Fail "loadFolders.xml is not well formed: $($_.Exception.Message)"
}
$loadFolderVersions = @($loadFoldersXml.SelectNodes('/loadFolders/*'))
$loadFolderEntries = @($loadFoldersXml.SelectNodes('/loadFolders/v1.6/*'))
if ($loadFoldersXml.DocumentElement.Name -ne 'loadFolders' -or
    $loadFoldersXml.DocumentElement.Attributes.Count -ne 0 -or
    $loadFolderVersions.Count -ne 1 -or
    $loadFolderVersions[0].Name -ne 'v1.6' -or
    $loadFolderVersions[0].Attributes.Count -ne 0 -or
    $loadFolderEntries.Count -ne 1 -or
    $loadFolderEntries[0].Name -ne 'li' -or
    $loadFolderEntries[0].Attributes.Count -ne 0 -or
    $loadFolderEntries[0].InnerText -ne '/') {
    Fail "loadFolders.xml must load only v1.6 /; developer Quickstarts are staged as a separate test Mod."
}
Pass "Production loadFolders.xml is root-only; developer Quickstarts use a standalone test fixture"

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

$buildSource = Get-Content -LiteralPath (Join-Path $RepoRoot "build.bat") -Raw
foreach ($expected in @(
    '/reference:"%HARMONY_DLL%"',
    'AncientMedievalJapanEnvironment.Quicktests.dll'
)) {
    if (-not $buildSource.Contains($expected)) {
        Fail "build.bat is missing expected Quicktest Harmony marker: $expected"
    }
}

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
    'Sudajii graphic resolves a non-BadTex texture',
    'Sudajii UI icon resolves a non-BadTex texture',
    '"BadTexture"',
    'Japanese beech leafy graphic resolves a non-BadTex texture',
    'Japanese beech leafless graphic resolves a non-BadTex texture',
    'Shirabiso graphic resolves a non-BadTex texture',
    'Haimatsu graphic resolves a non-BadTex texture',
    'AddTreeTextureAuditAssertions',
    'AddLiveThingTextureAssertions',
    '[AMJ Environment LiveThingTextureAudit] BAD',
    'all live non-plant Thing graphics resolve non-BadTex textures',
    'AddTerrainScatterTextureAssertions',
    '[AMJ Environment TerrainScatterTextureAudit] BAD',
    'all terrain scatter graphics resolve non-BadTex textures',
    'BadRenderMaterialDiagnostics',
    '[AMJ Environment BadRenderMaterial]',
    'System.Environment.GetEnvironmentVariable',
    'material.HasProperty("_MainTex")',
    'MapDrawLayer',
    'GetSubMesh',
    'map render pipeline emitted no BadTex submesh materials',
    'PreAtlasBadTextureDiagnostics',
    '[AMJ Environment PreAtlasBadTexture]',
    'TryGetTextureAtlasReplacementInfo',
    'pre-atlas map graphics emitted no BadTex materials',
    'RealtimeBadGraphicDiagnostics',
    '[AMJ Environment RealtimeBadGraphic]',
    'RealtimeBadGraphicFromDefDiagnostics',
    '[AMJ Environment RealtimeBadGraphicFromDef]',
    'Graphic_Shadow',
    'realtime Thing graphics emitted no non-shadow BadTex materials',
    'PawnRenderBadMaterialDiagnostics',
    '[AMJ Environment PawnRenderBadMaterial]',
    'PawnRenderNodeWorker',
    'GetFinalizedMaterial',
    'pawn render nodes emitted no BadTex materials',
    'all loaded tree graphic states resolve non-BadTex textures',
    '[AMJ Environment TreeTextureAudit]',
    'leaflessSnowOverlayGraphicPath',
    'immatureSnowOverlayGraphicPath',
    'ModsConfig.BiotechActive',
    'Graphic_Collection',
    'HasLoadedNonBadTexture',
    'BaseContent.BadTex',
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
Pass "Deterministic Environment Quickstarts and tree-texture audit hooks are present"

foreach ($relative in @(
    "run-runtime-tests.bat",
    "run-texture-debug.bat",
    "Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1",
    "Scripts\Run-EnvironmentVegetationQuickstarts.ps1",
    "Scripts\Validate-EnvironmentRuntimeLog.ps1",
    "Scripts\Validate-MedievalOverhaulTreeTextures.ps1"
)) {
    $path = Join-Path $RepoRoot $relative
    if (-not (Test-Path -LiteralPath $path)) {
        Fail "Environment runtime-test harness file is missing: $relative"
    }
}

$runtimeBatch = Get-Content -LiteralPath (Join-Path $RepoRoot "run-runtime-tests.bat") -Raw
foreach ($expected in @(
    'run-static-tests.bat',
    '--skip-static',
    'Run-EnvironmentVegetationQuickstarts.ps1',
    'TestResults\VegetationRuntime',
    'CCTO_INSTALLED',
    'SaveData-CCTO',
    'Reports-CCTO',
    '-IncludeCCTO',
    '-CctoCompatibilityOnly',
    'Medieval Overhaul tree-path validation is handled by the static gate'
)) {
    if (-not $runtimeBatch.Contains($expected)) {
        Fail "run-runtime-tests.bat is missing expected marker: $expected"
    }
}

$textureDebugBatch = Get-Content -LiteralPath (Join-Path $RepoRoot "run-texture-debug.bat") -Raw
foreach ($expected in @(
    'TestResults\TextureDebug',
    'TEXTURE_DEBUG_TARGET',
    'RIMWORLD_QUICKSTART=%TEXTURE_DEBUG_TARGET%',
    '-quickstart=%TEXTURE_DEBUG_TARGET%',
    'if not defined BIOME set "BIOME=CoolTemperate"',
    'AMJWarmTemperateTerrainQuickstart',
    'AMJCoolTemperateTerrainQuickstart',
    'AMJSubalpineTerrainQuickstart',
    'AMJAlpineTerrainQuickstart',
    'RimWorld will stay open until you close it manually',
    'Prepare-EnvironmentRuntimeTestSaveData.ps1'
)) {
    if (-not $textureDebugBatch.Contains($expected)) {
        Fail "run-texture-debug.bat is missing expected Golden Path marker: $expected"
    }
}

foreach ($forbidden in @(
    '-quickstartreport',
    '-quickstartverify'
)) {
    if ($textureDebugBatch.Contains($forbidden)) {
        Fail "run-texture-debug.bat must remain focused/non-auto-exit; forbidden marker found: $forbidden"
    }
}

Pass "Focused texture-debug runner supports all four AMJE Golden Path biome targets"

# The public entry point delegates to run-static-tests.bat; the MO audit
# must be present in that static gate, not in the parent entry point.
$runTestsSource = Get-Content -LiteralPath (Join-Path $RepoRoot "run-tests.bat") -Raw
foreach ($expected in @(
    'run-static-tests.bat',
    'Run-EnvironmentIsolatedDesktop.ps1'
)) {
    if (-not $runTestsSource.Contains($expected)) {
        Fail "run-tests.bat is missing expected test-entrypoint marker: $expected"
    }
}

$runStaticTestsSource = Get-Content -LiteralPath (Join-Path $RepoRoot "run-static-tests.bat") -Raw
foreach ($expected in @(
    'Validate-PowerShellSyntax.ps1',
    'build.bat',
    'Validate-Environment.ps1',
    'Validate-MedievalOverhaulTreeTextures.ps1',
    'Auditing installed Medieval Overhaul tree texture references'
)) {
    if (-not $runStaticTestsSource.Contains($expected)) {
        Fail "run-static-tests.bat is missing expected static-audit marker: $expected"
    }
}

if ($runtimeBatch.Contains('call "%ROOT%run-tests.bat"')) {
    Fail "run-runtime-tests.bat must not call run-tests.bat recursively."
}

$moTreeAudit = Get-Content -LiteralPath (Join-Path $RepoRoot "Scripts\Validate-MedievalOverhaulTreeTextures.ps1") -Raw
foreach ($expected in @(
    '3219596926',
    'TreeBase',
    'DeciduousTreeBase',
    'leaflessGraphicPath',
    'immatureGraphicPath',
    'Test-TexturePath',
    'Medieval Overhaul tree texture references resolve'
)) {
    if (-not $moTreeAudit.Contains($expected)) {
        Fail "MO tree-texture static audit is missing expected marker: $expected"
    }
}

$runtimeProfileBuilder = Get-Content -LiteralPath (Join-Path $RepoRoot "Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1") -Raw
foreach ($expected in @(
    'IncludeMedievalOverhaul',
    'OskarPotocki.VanillaFactionsExpanded.Core',
    'syrchalis.processor.framework',
    'DankPyon.Medieval.Overhaul'
)) {
    if (-not $runtimeProfileBuilder.Contains($expected)) {
        Fail "Environment runtime profile builder is missing expected marker: $expected"
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
    'TreeTextureAuditOnly',
    '[WAIT]',
    'preLaunchErrors'
)) {
    if (-not $runtimeRunner.Contains($expected)) {
        Fail "Vegetation runtime runner is missing expected marker: $expected"
    }
}

$liveTextureDiagnosticPath = Join-Path $RepoRoot "Source\AncientMedievalJapanEnvironment\LiveTextureDiagnostics.cs"
if (-not (Test-Path -LiteralPath $liveTextureDiagnosticPath)) {
    Fail "Live texture diagnostic source is missing."
}
$liveTextureDiagnostic = Get-Content -LiteralPath $liveTextureDiagnosticPath -Raw
foreach ($expected in @(
    'Scan current map for bad live textures',
    '[AMJ Environment LiveTextureAudit] BAD',
    'thing.Graphic',
    'MatAt(thing.Rotation, thing)',
    'SnowOverlayGraphic',
    'BaseContent.BadTex'
)) {
    if (-not $liveTextureDiagnostic.Contains($expected)) {
        Fail "Live texture diagnostic is missing expected marker: $expected"
    }
}

$runtimeLogValidator = Get-Content -LiteralPath (Join-Path $RepoRoot "Scripts\Validate-EnvironmentRuntimeLog.ps1") -Raw
foreach ($expected in @(
    'sucro.ancientmedievaljapan.environment',
    'Level:\s*ERROR',
    'Owned AMJ runtime ERROR'
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
# Public copy links named mods; validate the visible text, not link delimiters.
$readmeRaw = [regex]::Replace($readmeRaw, '\[([^\]]+)\]\([^\)]+\)', '$1').Replace('**', '')
foreach ($expected in @(
    'RimWorld 1.6',
    'Beta',
    'Ancient & Medieval Japan Core is not required',
    'Crop Cold Tolerance Overhaul is optional',
    'Use a new game',
    'Removing the mod from a save'
)) {
    if (-not $readmeRaw.Contains($expected)) {
        Fail "README is missing required public-positioning marker: $expected"
    }
}

# Workshop intentionally omits the redundant Core-not-required claim.
# Validate required Harmony and optional CCTO as written in the current copy.
$workshopJaPath = Join-Path $RepoRoot "Docs\SteamWorkshopDescription-ja.txt"
$workshopEnPath = Join-Path $RepoRoot "Docs\SteamWorkshopDescription.txt"
$workshopJaRaw = Get-Content -LiteralPath $workshopJaPath -Raw -Encoding UTF8
$workshopEnRaw = Get-Content -LiteralPath $workshopEnPath -Raw -Encoding UTF8
$workshopJaText = [regex]::Replace($workshopJaRaw, '\[(?:/?url[^\]]*|/?b)\]', '')
$workshopEnText = [regex]::Replace($workshopEnRaw, '\[(?:/?url[^\]]*|/?b)\]', '')

if ([System.Text.Encoding]::UTF8.GetByteCount($workshopJaRaw) -gt 8000) {
    Fail "Japanese Workshop description exceeds 8,000 UTF-8 bytes."
}
if ([System.Text.Encoding]::UTF8.GetByteCount($workshopEnRaw) -gt 8000) {
    Fail "English Workshop description exceeds 8,000 UTF-8 bytes."
}

foreach ($pair in @(
    @($workshopJaText, 'RimWorld 1.6対応、β版。', 'Japanese Workshop Beta stage'),
    @($workshopJaText, '必須MOD: Harmony', 'Japanese Workshop required Harmony'),
    @($workshopJaText, '任意MOD: CCTO', 'Japanese Workshop CCTO optionality'),
    @($workshopEnText, 'RimWorld 1.6, Beta.', 'English Workshop Beta stage'),
    @($workshopEnText, 'Required: Harmony', 'English Workshop required Harmony'),
    @($workshopEnText, 'Optional: Crop Cold Tolerance Overhaul (CCTO)', 'English Workshop CCTO optionality')
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
    '<description>Beta.',
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
