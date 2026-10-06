param([string]$RimWorldDir = 'D:\SteamLibrary\steamapps\common\RimWorld',
      [ValidateSet('Both','Vanilla','MO')][string]$Mode = 'Both')
$ErrorActionPreference = 'Stop'
$modes = if ($Mode -eq 'Both') { @('Vanilla','MO') } else { @($Mode) }
$repoRoot = Split-Path -Parent $PSScriptRoot
$modsRoot = [IO.Path]::GetFullPath((Join-Path $RimWorldDir 'Mods'))
$fixture = Join-Path $modsRoot 'AMJE.HarvestTest'
$observer = Join-Path $modsRoot 'AMJE.HarvestErrorObserver'
$result = Join-Path $repoRoot ('TestResults\Harvest-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
if (Get-Process RimWorldWin64 -ErrorAction SilentlyContinue) { throw 'Close the existing game before harvest tests.' }
if (Test-Path -LiteralPath $fixture) { throw 'Harvest fixture already exists; inspect it before reuse.' }
if (Test-Path -LiteralPath $observer) { throw 'Harvest error observer already exists; inspect it before reuse.' }
New-Item -ItemType Directory -Path $result -Force | Out-Null
try {
    New-Item -ItemType Directory -Path (Join-Path $observer 'About'),(Join-Path $observer 'Assemblies') -Force | Out-Null
    [IO.File]::WriteAllText((Join-Path $observer 'About\About.xml'), '<ModMetaData><name>AMJE harvest error observer</name><author>sucRo</author><packageId>sucro.amje.harvesterrorobserver</packageId><supportedVersions><li>1.6</li></supportedVersions></ModMetaData>')
    $managed = Join-Path $RimWorldDir 'RimWorldWin64_Data\Managed'
    & "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /target:library "/out:$observer\Assemblies\HarvestErrorObserver.dll" "/reference:$managed\Assembly-CSharp.dll" "/reference:$managed\UnityEngine.CoreModule.dll" "/reference:$managed\netstandard.dll" (Join-Path $repoRoot 'Tests\Release\HarvestErrorObserver.cs')
    if ($LASTEXITCODE -ne 0) { throw 'Error observer compilation failed.' }
    New-Item -ItemType Directory -Path $fixture | Out-Null
    foreach ($part in @('About','Assemblies','Defs','Languages','Patches','Textures','DevQuickstarts','loadFolders.xml')) {
        Copy-Item -LiteralPath (Join-Path $repoRoot $part) -Destination $fixture -Recurse
    }
    $aboutPath = Join-Path $fixture 'About\About.xml'
    [xml]$about = Get-Content -LiteralPath $aboutPath -Raw
    $about.ModMetaData.packageId = 'sucro.amje.harvesttest'
    $about.ModMetaData.name = 'AMJE isolated harvest test (not for distribution)'
    $about.Save($aboutPath)
    $payload = @(Get-ChildItem -LiteralPath $fixture -File -Recurse | ForEach-Object {
        @{ path = $_.FullName.Substring($fixture.Length + 1); sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash }
    })
    $payload | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $result 'PayloadManifest.json') -Encoding UTF8
    $commands = @('@echo off', 'set "PSModulePath=%WINDIR%\System32\WindowsPowerShell\v1.0\Modules"')
    foreach ($mode in $modes) {
        $save = Join-Path $result ('SaveData-' + $mode)
        $reports = Join-Path $result ('Reports-' + $mode)
        $prepare = @{ OutputRoot = $save }
        if ($mode -eq 'MO') { $prepare.IncludeMedievalOverhaul = $true }
        & (Join-Path $PSScriptRoot 'Prepare-EnvironmentRuntimeTestSaveData.ps1') @prepare
        if ($LASTEXITCODE -ne 0) { throw 'Profile preparation failed.' }
        $config = Join-Path $save 'Config\ModsConfig.xml'
        [xml]$doc = Get-Content -LiteralPath $config -Raw
        $node = $doc.SelectSingleNode('//activeMods/li[.="sucro.ancientmedievaljapan.environment"]')
        if ($null -eq $node) { throw 'Expected Environment profile entry missing.' }
        $node.InnerText = 'sucro.amje.harvesttest'
        $capture = $doc.CreateElement('li'); $capture.InnerText = 'sucro.amje.harvesterrorobserver'
        $core = $doc.SelectSingleNode('//activeMods/li[.="ludeon.rimworld"]')
        $null = $core.ParentNode.InsertAfter($capture, $core)
        $doc.Save($config)
        $prefsPath = Join-Path $save 'Config\Prefs.xml'
        [xml]$prefs = Get-Content -LiteralPath $prefsPath -Raw
        $reset = $prefs.SelectSingleNode('//resetModsConfigOnCrash')
        if ($null -eq $reset) { $reset = $prefs.CreateElement('resetModsConfigOnCrash'); $null = $prefs.DocumentElement.AppendChild($reset) }
        $reset.InnerText = 'False'
        $prefs.Save($prefsPath)
        $expected = if ($mode -eq 'MO') { 'DankPyon_RawWood' } else { 'WoodLog' }
        $timeout = if ($mode -eq 'MO') { 600 } else { 240 }
        $commands += 'set "RIMWORLD_AMJE_HARVEST_EXPECTED=' + $expected + '"'
        $commands += 'set "RIMWORLD_AMJE_HARVEST_ERROR_LOG=' + (Join-Path $result ($mode + '-Errors.log')) + '"'
        $commands += 'powershell -NoProfile -ExecutionPolicy Bypass -File "' + (Join-Path $PSScriptRoot 'Run-EnvironmentVegetationQuickstarts.ps1') + '" -ExePath "' + (Join-Path $RimWorldDir 'RimWorldWin64.exe') + '" -SaveDataFolder "' + $save + '" -ResultDir "' + $reports + '" -TimeoutSeconds ' + $timeout + ' -TreeTextureAuditOnly >> "' + (Join-Path $result 'Runner.log') + '" 2>&1'
        $commands += 'if errorlevel 1 exit /b 1'
    }
    $commands += 'exit /b 0'
    $batch = Join-Path $result 'Run.cmd'
    [IO.File]::WriteAllLines($batch, $commands, [Text.Encoding]::ASCII)
    $desktopExe = Join-Path $result 'IsolatedDesktopRunner.exe'
    & "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /target:exe "/out:$desktopExe" (Join-Path $repoRoot 'Tests\Release\IsolatedDesktopRunner.cs')
    if ($LASTEXITCODE -ne 0) { throw 'Desktop launcher compilation failed.' }
    & $desktopExe $batch $repoRoot
    if ($LASTEXITCODE -ne 0) { throw "Harvest runtime failed. Evidence: $result" }
    foreach ($mode in $modes) {
        $report = Join-Path $result ('Reports-' + $mode + '\AMJWarmTemperateTerrainQuickstart.json')
        $log = Join-Path $result ('Reports-' + $mode + '\AMJWarmTemperateTerrainQuickstart.log')
        $data = Get-Content -LiteralPath $report -Raw | ConvertFrom-Json
        $errorLog = Join-Path $result ($mode + '-Errors.log')
        if (-not (Test-Path -LiteralPath $errorLog) -or -not (Select-String -LiteralPath $errorLog -SimpleMatch '[CAPTURE_READY]')) { throw 'Independent runtime error capture missing.' }
        if (Select-String -LiteralPath $errorLog -SimpleMatch '[ERROR]') { throw "Runtime ERROR: $errorLog" }
        if (-not $data.passed -or $data.total -ne 9 -or $data.failed -ne 0 -or $data.preLaunchErrors -ne 0 -or -not $data.captureLive -or $data.logTruncated) { throw "Incomplete harvest report: $report" }
        if (Select-String -LiteralPath $log -Pattern '\[ERROR\]|Level:\s*ERROR') { throw "Runtime ERROR: $log" }
        if (@(Select-String -LiteralPath $log -Pattern '\[AMJ Environment Harvest\]').Count -ne 4) { throw "Harvest output evidence missing: $log" }
    }
    foreach ($file in $payload) {
        if ((Get-FileHash -LiteralPath (Join-Path $fixture $file.path) -Algorithm SHA256).Hash -ne $file.sha256) { throw 'Test payload changed during runtime.' }
    }
    Write-Host "[OK] Four native pawn-cutting outputs passed ($($modes -join '+')): $result"
}
finally {
    foreach ($retireTarget in @($fixture, $observer)) {
    if (Test-Path -LiteralPath $retireTarget) {
        $resolved = [IO.Path]::GetFullPath($retireTarget)
        if ($resolved -ne [IO.Path]::GetFullPath($fixture) -and $resolved -ne [IO.Path]::GetFullPath($observer)) { throw 'Unexpected fixture path.' }
        if (-not $resolved.StartsWith($modsRoot + '\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Fixture outside Mods workspace.' }
        $retired = Join-Path $result 'Retired'
        New-Item -ItemType Directory -Path $retired -Force | Out-Null
        Move-Item -LiteralPath $resolved -Destination $retired
    }
    }
}
