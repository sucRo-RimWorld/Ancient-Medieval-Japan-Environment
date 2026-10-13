param([string]$GameRoot='D:\SteamLibrary\steamapps\common\RimWorld')
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'RimWorldRuntime.ps1')
$runtime=Get-RimWorldRuntime $GameRoot
$root=Split-Path -Parent $PSScriptRoot
if(-not $runtime.Modern){throw 'This test build entry is for RimWorld 1.7'}
$production=Join-Path $root 'Assemblies\AncientMedievalJapanEnvironment.dll'
if(-not (Test-Path -LiteralPath $production)){throw 'Install the shared production DLL before 1.7 tests'}
$workshop=[IO.Path]::GetFullPath((Join-Path $GameRoot '..\..\workshop\content\294100'))
$harmony=Find-RimWorldModAssembly (Join-Path $workshop '2009463077') '0Harmony.dll'
$quickstarts=Find-RimWorldModAssembly (Join-Path $workshop '3793646067') 'Quickstarts.dll'
Invoke-RimWorldTestCompile $GameRoot (Join-Path $root 'DevQuickstarts\Assemblies\AncientMedievalJapanEnvironment.Quicktests.dll') @(Join-Path $root 'Tests\Quickstarts\EnvironmentBiomeTerrainQuickstarts.cs') @($harmony,$quickstarts)
Write-Output '[OK] 1.7 Quicktests rebuilt; shared 1.6/1.7 production DLL preserved.'
