param(
    [Parameter(Mandatory = $true)]
    [string]$LogPath,

    [string]$AdditionalModIdPrefixes = "",

    [switch]$RequireClimateGradient
)

$ErrorActionPreference = "Stop"

function Fail([string]$Message) {
    Write-Host "[FAIL] $Message" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path -LiteralPath $LogPath)) {
    Fail "Runtime log was not produced: $LogPath"
}

$text = Get-Content -LiteralPath $LogPath -Raw -Encoding UTF8
$prefixes = New-Object System.Collections.Generic.List[string]
$prefixes.Add("sucro.ancientmedievaljapan.environment")
foreach ($value in ($AdditionalModIdPrefixes -split ';')) {
    $trimmed = $value.Trim()
    if (-not [string]::IsNullOrWhiteSpace($trimmed)) {
        $prefixes.Add($trimmed)
    }
}
$errors = New-Object System.Collections.Generic.List[string]

$blocks = [regex]::Matches(
    $text,
    '(?ms)^Timestamp:\s*.*?(?=^Timestamp:\s*|\z)'
)

foreach ($match in $blocks) {
    $block = $match.Value
    if ($block -notmatch '(?m)^Level:\s*ERROR\s*$') {
        continue
    }

    $idMatch = [regex]::Match($block, '(?mi)^mod_id:\s*([^\r\n]+)')
    $channelMatch = [regex]::Match($block, '(?mi)^Channel:\s*Mod\.([^\r\n]+)')
    $owned = $false

    foreach ($prefix in $prefixes) {
        if ($idMatch.Success -and
            $idMatch.Groups[1].Value.Trim().StartsWith(
                $prefix,
                [System.StringComparison]::OrdinalIgnoreCase)) {
            $owned = $true
            break
        }

        if ($channelMatch.Success -and
            $channelMatch.Groups[1].Value.Trim().StartsWith(
                $prefix,
                [System.StringComparison]::OrdinalIgnoreCase)) {
            $owned = $true
            break
        }
    }

    if ($owned) {
        $errors.Add($block.Trim())
    }
}

# Fallback for plain Unity/Verse logging if structured RimLogging metadata is absent.
foreach ($match in [regex]::Matches(
    $text,
    '(?mi)^.*\[AMJ Environment\].*$')) {
    $line = $match.Value.Trim()
    if ($line -match '(?i)error|exception|failed') {
        if (-not $errors.Contains($line)) {
            $errors.Add($line)
        }
    }
}

if ($errors.Count -gt 0) {
    Write-Host "[FAIL] Owned AMJ runtime ERROR entries were found:" -ForegroundColor Red
    $limit = [Math]::Min($errors.Count, 5)
    for ($i = 0; $i -lt $limit; $i++) {
        Write-Host ""
        Write-Host $errors[$i]
    }
    if ($errors.Count -gt $limit) {
        Write-Host ""
        Write-Host "... plus $($errors.Count - $limit) more Environment runtime ERROR entries."
    }
    exit 1
}

if ($RequireClimateGradient) {
    $labels = @("WarmLowland", "TemperateLowland", "CoolLowland", "Highland")
    $below8 = @{}
    $below0 = @{}

    foreach ($label in $labels) {
        $pattern =
            '\[AMJ Environment\] Climate calibration \| ' +
            [regex]::Escape($label) +
            '.*?\| belowHours.*?<8=(\d+)h.*?<0=(\d+)h'
        $match = [regex]::Match(
            $text,
            $pattern,
            [System.Text.RegularExpressions.RegexOptions]::Singleline)

        if (-not $match.Success) {
            Fail "Climate calibration line was not found for $label."
        }

        $below8[$label] = [int]$match.Groups[1].Value
        $below0[$label] = [int]$match.Groups[2].Value
    }

    for ($i = 1; $i -lt $labels.Count; $i++) {
        $warmer = $labels[$i - 1]
        $colder = $labels[$i]

        if ($below8[$warmer] -gt $below8[$colder]) {
            Fail "Climate <8C hours are not monotonic: $warmer=$($below8[$warmer]), $colder=$($below8[$colder])."
        }

        if ($below0[$warmer] -gt $below0[$colder]) {
            Fail "Climate <0C hours are not monotonic: $warmer=$($below0[$warmer]), $colder=$($below0[$colder])."
        }
    }

    if ($below8["WarmLowland"] -ge $below8["Highland"]) {
        Fail "Climate gradient collapsed for <8C hours: WarmLowland=$($below8["WarmLowland"]), Highland=$($below8["Highland"])."
    }

    if ($below0["WarmLowland"] -ge $below0["Highland"]) {
        Fail "Climate gradient collapsed for <0C hours: WarmLowland=$($below0["WarmLowland"]), Highland=$($below0["Highland"])."
    }

    Write-Host (
        "[OK] Climate gameplay gradient preserved: " +
        "<8C Warm/Temperate/Cool/Highland=" +
        "$($below8["WarmLowland"])/$($below8["TemperateLowland"])/" +
        "$($below8["CoolLowland"])/$($below8["Highland"]); " +
        "<0C=" +
        "$($below0["WarmLowland"])/$($below0["TemperateLowland"])/" +
        "$($below0["CoolLowland"])/$($below0["Highland"])."
    ) -ForegroundColor Green
}

Write-Host "[OK] No owned AMJ runtime ERROR entries were found." -ForegroundColor Green
exit 0
