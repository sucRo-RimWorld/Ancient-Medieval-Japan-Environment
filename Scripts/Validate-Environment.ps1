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
    "Patches\Rivers.xml",
    "Patches\WorldGeneration.xml"
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

$worldPatchText = Get-Content -LiteralPath (Join-Path $RepoRoot "Patches\WorldGeneration.xml") -Raw
if (-not $worldPatchText.Contains("AncientMedievalJapan.Environment.WorldGenStep_AMJEnvironmentTerrain")) {
    Fail "WorldGeneration.xml does not select the Environment Terrain worker."
}
Pass "Environment Terrain worker patch is present"

Write-Host ""
Write-Host "[OK] AMJ Environment static validation passed"
exit 0
