param([string]$RimWorldDir = 'D:\SteamLibrary\steamapps\common\RimWorld')
$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$modsRoot = [IO.Path]::GetFullPath((Join-Path $RimWorldDir 'Mods'))
$fixture = Join-Path $modsRoot 'AMJE.HarvestTest'
$result = Join-Path $repoRoot ('TestResults\Harvest-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
if (Get-Process RimWorldWin64 -ErrorAction SilentlyContinue) { throw 'Close the existing game before harvest tests.' }
if (Test-Path -LiteralPath $fixture) { throw 'Harvest fixture already exists; inspect it before reuse.' }
New-Item -ItemType Directory -Path $result -Force | Out-Null
try {
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
    foreach ($mode in @('Vanilla','MO')) {
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
        $doc.Save($config)
        $expected = if ($mode -eq 'MO') { 'DankPyon_RawWood' } else { 'WoodLog' }
        $timeout = if ($mode -eq 'MO') { 600 } else { 240 }
        $commands += 'set "RIMWORLD_AMJE_HARVEST_EXPECTED=' + $expected + '"'
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
    foreach ($mode in @('Vanilla','MO')) {
        $report = Join-Path $result ('Reports-' + $mode + '\AMJWarmTemperateTerrainQuickstart.json')
        $log = Join-Path $result ('Reports-' + $mode + '\AMJWarmTemperateTerrainQuickstart.log')
        $data = Get-Content -LiteralPath $report -Raw | ConvertFrom-Json
        if (-not $data.passed -or $data.total -ne 9 -or $data.failed -ne 0 -or $data.preLaunchErrors -ne 0 -or -not $data.captureLive -or $data.logTruncated) { throw "Incomplete harvest report: $report" }
        if (Select-String -LiteralPath $log -Pattern '\[ERROR\]|Level:\s*ERROR') { throw "Runtime ERROR: $log" }
        if (@(Select-String -LiteralPath $log -Pattern '\[AMJ Environment Harvest\]').Count -ne 4) { throw "Harvest output evidence missing: $log" }
    }
    foreach ($file in $payload) {
        if ((Get-FileHash -LiteralPath (Join-Path $fixture $file.path) -Algorithm SHA256).Hash -ne $file.sha256) { throw 'Test payload changed during runtime.' }
    }
    Write-Host "[OK] Four native pawn-cutting outputs passed in Vanilla and actual MO: $result"
}
finally {
    if (Test-Path -LiteralPath $fixture) {
        $resolved = [IO.Path]::GetFullPath($fixture)
        if ($resolved -ne [IO.Path]::GetFullPath((Join-Path $modsRoot 'AMJE.HarvestTest'))) { throw 'Unexpected fixture path.' }
        $retired = Join-Path $result 'Retired'
        New-Item -ItemType Directory -Path $retired -Force | Out-Null
        Move-Item -LiteralPath $resolved -Destination $retired
    }
}
