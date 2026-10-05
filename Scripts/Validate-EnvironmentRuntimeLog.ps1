param(
    [Parameter(Mandatory = $true)]
    [string]$LogPath,

    [string]$AdditionalModIdPrefixes = ""
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
    Write-Host "[FAIL] Environment-origin runtime ERROR entries were found:" -ForegroundColor Red
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

Write-Host "[OK] No owned AMJ runtime ERROR entries were found." -ForegroundColor Green
exit 0
