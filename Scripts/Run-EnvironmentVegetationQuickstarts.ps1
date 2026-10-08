param(
    [Parameter(Mandatory = $true)]
    [string]$ExePath,

    [Parameter(Mandatory = $true)]
    [string]$SaveDataFolder,

    [Parameter(Mandatory = $true)]
    [string]$ResultDir,

    [int]$TimeoutSeconds = 180,

    [switch]$CctoCompatibilityOnly,

    [switch]$TreeTextureAuditOnly,

    [switch]$CoreIntegrationOnly
)

$ErrorActionPreference = "Stop"

function Fail([string]$Message, [int]$Code = 2) {
    Write-Host "[ERROR] $Message" -ForegroundColor Red
    exit $Code
}

if (-not (Test-Path -LiteralPath $ExePath)) {
    Fail "RimWorld executable was not found: $ExePath"
}

New-Item -ItemType Directory -Force -Path $ResultDir | Out-Null

$modeCount = @(
    $CctoCompatibilityOnly,
    $TreeTextureAuditOnly,
    $CoreIntegrationOnly
) | Where-Object { $_ } | Measure-Object | Select-Object -ExpandProperty Count

if ($modeCount -gt 1) {
    Fail "Only one focused runtime mode may be selected at a time."
}

if ($CctoCompatibilityOnly -or $TreeTextureAuditOnly) {
    $scenarios = @(
        "AMJWarmTemperateTerrainQuickstart"
    )
}
elseif ($CoreIntegrationOnly) {
    $scenarios = @(
        "AMJWarmTemperateTerrainQuickstart",
        "AMJCoolTemperateTerrainQuickstart",
        "AMJSubalpineTerrainQuickstart",
        "AMJAlpineTerrainQuickstart",
        "AMJRiverMapHandoffQuickstart",
        "AMJCoastMapHandoffQuickstart"
    )
}
else {
    $scenarios = @(
        "AMJWarmTemperateTerrainQuickstart",
        "AMJCoolTemperateTerrainQuickstart",
        "AMJSubalpineTerrainQuickstart",
        "AMJAlpineTerrainQuickstart",
        "AMJTemperateSwampVegetationQuickstart",
        "AMJColdBogVegetationQuickstart",
        "AMJWorldWetlandDistributionQuickstart",
        "AMJRiverMapHandoffQuickstart",
        "AMJCoastMapHandoffQuickstart"
    )
}

$validator = Join-Path $PSScriptRoot "Validate-EnvironmentRuntimeLog.ps1"
if (-not (Test-Path -LiteralPath $validator)) {
    Fail "Runtime log validator was not found: $validator"
}

foreach ($name in $scenarios) {
    $report = Join-Path $ResultDir ($name + ".json")
    $log = Join-Path $ResultDir ($name + ".log")

    Remove-Item -LiteralPath $report -Force -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $log -Force -ErrorAction SilentlyContinue

    Write-Host ""
    Write-Host "============================================================"
    Write-Host "Running $name"
    Write-Host "============================================================"

    $arguments = @(
        '-savedatafolder="' + $SaveDataFolder + '"',
        '-logFile "' + $log + '"',
        '-quickstart="' + $name + '"',
        '-quickstartreport="' + $report + '"',
        '-quickstarttimeout=' + [Math]::Max(30, $TimeoutSeconds - 30)
    ) -join ' '

    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = $ExePath
    $psi.Arguments = $arguments
    $psi.WorkingDirectory = Split-Path -Parent $ExePath
    $psi.UseShellExecute = $false
    $psi.EnvironmentVariables["RIMWORLD_QUICKSTART"] = $name

    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = $psi

    try {
        if (-not $process.Start()) {
            Fail "Failed to start RimWorld for $name"
        }

        $elapsedSeconds = 0
        $pollSeconds = 15
        $finished = $false

        while ($elapsedSeconds -lt $TimeoutSeconds) {
            $remaining = $TimeoutSeconds - $elapsedSeconds
            $waitSeconds = [Math]::Min($pollSeconds, $remaining)

            if ($process.WaitForExit($waitSeconds * 1000)) {
                $finished = $true
                break
            }

            $elapsedSeconds += $waitSeconds
            Write-Host (
                "[WAIT] " + $name +
                " is still running (" + $elapsedSeconds +
                "s / " + $TimeoutSeconds + "s)."
            )
        }

        if (-not $finished) {
            Write-Host "[ERROR] $name exceeded the outer timeout." -ForegroundColor Red
            try {
                $process.Kill()
                $process.WaitForExit()
            }
            catch {
                Write-Host "[WARN] Failed to terminate RimWorld cleanly: $($_.Exception.Message)" -ForegroundColor Yellow
            }
            exit 124
        }

        if ($process.ExitCode -ne 0) {
            Fail "$name exited with code $($process.ExitCode). See $log" 1
        }
    }
    finally {
        $process.Dispose()
    }

    if (-not (Test-Path -LiteralPath $report)) {
        Fail "$name did not write a Quickstarts report: $report"
    }

    try {
        $result = Get-Content -LiteralPath $report -Raw -Encoding UTF8 | ConvertFrom-Json
    }
    catch {
        Fail "Failed to parse Quickstarts report for $name : $($_.Exception.Message)"
    }

    if (-not $result.passed -or [int]$result.failed -ne 0 -or [int]$result.total -le 0) {
        Fail "$name verification failed. See $report" 1
    }

    if ([int]$result.preLaunchErrors -gt 0) {
        Fail "$name recorded $($result.preLaunchErrors) pre-launch ERROR entries. See $report and $log" 1
    }

    if (-not $result.captureLive -or $result.logTruncated) {
        Fail "$name could not prove complete live log capture." 1
    }

    if ($CoreIntegrationOnly) {
        & powershell -NoProfile -ExecutionPolicy Bypass -File $validator `
            -LogPath $log `
            -AdditionalModIdPrefixes "sucro.ancientmedievaljapan.core" `
            -RequireClimateGradient
    }
    else {
        & powershell -NoProfile -ExecutionPolicy Bypass -File $validator -LogPath $log
    }
    if ($LASTEXITCODE -ne 0) {
        exit 1
    }

    Write-Host "[OK] $name passed."
}

Write-Host ""
if ($CctoCompatibilityOnly) {
    Write-Host "[OK] AMJE + CCTO loaded-Def compatibility Quickstart passed." -ForegroundColor Green
}
elseif ($TreeTextureAuditOnly) {
    Write-Host "[OK] Loaded tree graphic-state texture audit passed." -ForegroundColor Green
}
elseif ($CoreIntegrationOnly) {
    Write-Host "[OK] AMJ Core + Environment gameplay-contract Quickstarts passed." -ForegroundColor Green
}
else {
    Write-Host "[OK] Environment vegetation, natural-world wetlands, and river/coast Quickstarts passed." -ForegroundColor Green
}
exit 0
