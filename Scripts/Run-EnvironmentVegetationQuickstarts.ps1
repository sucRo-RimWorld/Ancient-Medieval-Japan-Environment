param(
    [Parameter(Mandatory = $true)]
    [string]$ExePath,

    [Parameter(Mandatory = $true)]
    [string]$SaveDataFolder,

    [Parameter(Mandatory = $true)]
    [string]$ResultDir,

    [int]$TimeoutSeconds = 180,

    [switch]$CctoCompatibilityOnly
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

if ($CctoCompatibilityOnly) {
    $scenarios = @(
        "AMJWarmTemperateTerrainQuickstart"
    )
}
else {
    $scenarios = @(
        "AMJWarmTemperateTerrainQuickstart",
        "AMJCoolTemperateTerrainQuickstart",
        "AMJSubalpineTerrainQuickstart",
        "AMJAlpineTerrainQuickstart"
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

        if (-not $process.WaitForExit($TimeoutSeconds * 1000)) {
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

    if (-not $result.captureLive -or $result.logTruncated) {
        Fail "$name could not prove complete live log capture." 1
    }

    & powershell -NoProfile -ExecutionPolicy Bypass -File $validator -LogPath $log
    if ($LASTEXITCODE -ne 0) {
        exit 1
    }

    Write-Host "[OK] $name passed."
}

Write-Host ""
if ($CctoCompatibilityOnly) {
    Write-Host "[OK] AMJE + CCTO loaded-Def compatibility Quickstart passed." -ForegroundColor Green
}
else {
    Write-Host "[OK] All four Environment vegetation runtime Quickstarts passed." -ForegroundColor Green
}
exit 0
