$ErrorActionPreference = "Stop"
$failed = $false

$RepoRoot = Split-Path -Parent $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($RepoRoot) -or -not (Test-Path -LiteralPath $RepoRoot -PathType Container)) {
    Write-Host "[ERROR] Could not resolve repository root from script location: $PSScriptRoot" -ForegroundColor Red
    exit 1
}

Get-ChildItem -LiteralPath $RepoRoot -Filter "*.ps1" -Recurse -File |
    Where-Object { $_.FullName -notmatch "\\TestResults\\" } |
    ForEach-Object {
        $tokens = $null
        $errors = $null

        [System.Management.Automation.Language.Parser]::ParseFile(
            $_.FullName,
            [ref]$tokens,
            [ref]$errors) | Out-Null

        foreach ($parseError in @($errors)) {
            Write-Host (
                "[ERROR] PowerShell parse failed: {0}:{1}:{2}: {3}" -f
                $_.FullName,
                $parseError.Extent.StartLineNumber,
                $parseError.Extent.StartColumnNumber,
                $parseError.Message
            ) -ForegroundColor Red
            $failed = $true
        }
    }

if ($failed) {
    exit 1
}

Write-Host "[OK] PowerShell syntax validation passed"
exit 0
