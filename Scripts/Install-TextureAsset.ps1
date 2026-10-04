param(
    [Parameter(Mandatory = $true)]
    [string]$SourcePath,

    [string]$DestinationRelativePath,

    [ValidateRange(1, 16384)]
    [int]$ExpectedWidth = 256,

    [ValidateRange(1, 16384)]
    [int]$ExpectedHeight = 256,

    [switch]$ValidateOnly,

    [switch]$AllowOpaque,

    [switch]$Force
)

$ErrorActionPreference = "Stop"
$RepoRoot = Split-Path -Parent $PSScriptRoot
. (Join-Path $PSScriptRoot 'PngImageData.ps1')

function Fail([string]$Message) {
    Write-Host "[ERROR] $Message" -ForegroundColor Red
    exit 1
}

function Pass([string]$Message) {
    Write-Host "[OK] $Message"
}

function Read-BigEndianUInt32(
    [byte[]]$Bytes,
    [int]$Offset
) {
    return [uint32](
        ([uint64]$Bytes[$Offset] * 16777216) +
        ([uint64]$Bytes[$Offset + 1] * 65536) +
        ([uint64]$Bytes[$Offset + 2] * 256) +
        [uint64]$Bytes[$Offset + 3])
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
                $value = [uint32](
                    ([uint32]($value -shr 1)) -bxor
                    [uint32]3988292384)
            }
            else {
                $value = [uint32]($value -shr 1)
            }
        }

        $crc = $value
    }

    return [uint32]($crc -bxor [uint32]4294967295)
}

function Get-PngAssetInfo(
    [string]$Path,
    [int]$Width,
    [int]$Height,
    [bool]$RequireTransparency
) {
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "PNG file was not found: $Path"
    }

    [byte[]]$bytes = [System.IO.File]::ReadAllBytes($Path)
    [byte[]]$signature = @(137, 80, 78, 71, 13, 10, 26, 10)

    if ($bytes.Length -lt 33) {
        throw "PNG is too small to contain a valid IHDR: $Path"
    }

    for ($i = 0; $i -lt $signature.Length; $i++) {
        if ($bytes[$i] -ne $signature[$i]) {
            throw "PNG signature is invalid: $Path"
        }
    }

    $firstChunk = $true
    $sawIhdr = $false
    $sawIdat = $false
    $sawIend = $false
    $sawTrns = $false
    [int]$actualWidth = 0
    [int]$actualHeight = 0
    [int]$bitDepth = -1
    [int]$colorType = -1
    [int]$offset = 8

    while (($offset + 12) -le $bytes.Length) {
        [uint32]$length = Read-BigEndianUInt32 $bytes $offset
        [int64]$dataEnd64 = [int64]$offset + 8 + [int64]$length
        [int64]$chunkEnd64 = $dataEnd64 + 4

        if ($dataEnd64 -gt [int]::MaxValue -or $chunkEnd64 -gt $bytes.Length) {
            throw "PNG chunk extends beyond end of file: $Path"
        }

        [int]$dataEnd = [int]$dataEnd64
        [int]$chunkEnd = [int]$chunkEnd64
        $chunkType = [System.Text.Encoding]::ASCII.GetString(
            $bytes,
            $offset + 4,
            4)

        if ($firstChunk -and $chunkType -ne "IHDR") {
            throw "PNG first chunk is not IHDR: $Path"
        }
        $firstChunk = $false

        [uint32]$storedCrc = Read-BigEndianUInt32 $bytes $dataEnd
        [uint32]$computedCrc = Get-PngCrc32 $bytes ($offset + 4) ([int]$length + 4)

        if ($storedCrc -ne $computedCrc) {
            throw "PNG chunk CRC is invalid ($chunkType): $Path"
        }

        switch ($chunkType) {
            "IHDR" {
                if ($sawIhdr -or $length -ne 13) {
                    throw "PNG IHDR is duplicated or malformed: $Path"
                }

                $sawIhdr = $true
                $actualWidth = [int](Read-BigEndianUInt32 $bytes ($offset + 8))
                $actualHeight = [int](Read-BigEndianUInt32 $bytes ($offset + 12))
                $bitDepth = [int]$bytes[$offset + 16]
                $colorType = [int]$bytes[$offset + 17]
            }
            "IDAT" {
                $sawIdat = $true
            }
            "tRNS" {
                $sawTrns = $true
            }
            "IEND" {
                if ($length -ne 0 -or $chunkEnd -ne $bytes.Length) {
                    throw "PNG IEND is malformed or trailing bytes are present: $Path"
                }

                $sawIend = $true
            }
        }

        $offset = $chunkEnd
        if ($sawIend) {
            break
        }
    }

    if (-not $sawIhdr -or -not $sawIdat -or -not $sawIend) {
        throw "PNG is missing required IHDR/IDAT/IEND chunks: $Path"
    }

    if ($actualWidth -ne $Width -or $actualHeight -ne $Height) {
        throw "PNG dimensions are $($actualWidth)x$($actualHeight); expected $($Width)x$($Height): $Path"
    }

    $hasAlphaChannel = ($colorType -eq 4 -or $colorType -eq 6)
    $hasTransparencySupport = $hasAlphaChannel -or $sawTrns

    if ($RequireTransparency -and -not $hasTransparencySupport) {
        throw "PNG does not contain an alpha channel or tRNS transparency: $Path"
    }

    [AMJPngImageData]::Validate($bytes)
    $hash = (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash

    return [pscustomobject]@{
        Path = $Path
        Width = $actualWidth
        Height = $actualHeight
        BitDepth = $bitDepth
        ColorType = $colorType
        HasTransparency = $hasTransparencySupport
        Sha256 = $hash
        ByteLength = $bytes.Length
    }
}

try {
    $sourceFullPath = [System.IO.Path]::GetFullPath($SourcePath)

    if ([System.IO.Path]::GetExtension($sourceFullPath) -ine ".png") {
        Fail "Source asset must be a .png file: $sourceFullPath"
    }

    $sourceInfo = Get-PngAssetInfo `
        $sourceFullPath `
        $ExpectedWidth `
        $ExpectedHeight `
        (-not $AllowOpaque.IsPresent)

    Pass "Source PNG validated: $($sourceInfo.Width)x$($sourceInfo.Height), SHA-256 $($sourceInfo.Sha256)"

    if ($ValidateOnly.IsPresent) {
        exit 0
    }

    if ([string]::IsNullOrWhiteSpace($DestinationRelativePath)) {
        Fail "DestinationRelativePath is required unless -ValidateOnly is used."
    }

    if ([System.IO.Path]::IsPathRooted($DestinationRelativePath)) {
        Fail "DestinationRelativePath must be relative to the repository root."
    }

    $normalizedRelative = $DestinationRelativePath.Replace("/", "\")
    if (
        -not $normalizedRelative.StartsWith("Textures\", [System.StringComparison]::OrdinalIgnoreCase) -and
        -not $normalizedRelative.StartsWith("TestResults\GoldenPath\", [System.StringComparison]::OrdinalIgnoreCase)
    ) {
        Fail "Destination must be under Textures\ or TestResults\GoldenPath\."
    }

    $destinationPath = [System.IO.Path]::GetFullPath(
        (Join-Path $RepoRoot $DestinationRelativePath))
    $repoRootFull = [System.IO.Path]::GetFullPath($RepoRoot)
    $separator = [System.IO.Path]::DirectorySeparatorChar.ToString()
    $repoPrefix = $repoRootFull
    if (-not $repoPrefix.EndsWith($separator)) {
        $repoPrefix += $separator
    }

    if (-not $destinationPath.StartsWith(
        $repoPrefix,
        [System.StringComparison]::OrdinalIgnoreCase))
    {
        Fail "Destination escapes the repository root: $destinationPath"
    }

    if ([System.IO.Path]::GetExtension($destinationPath) -ine ".png") {
        Fail "Destination asset must be a .png file: $destinationPath"
    }

    if ((Test-Path -LiteralPath $destinationPath) -and -not $Force.IsPresent) {
        Fail "Destination already exists. Re-run with -Force only after the replacement image is approved: $destinationPath"
    }

    $destinationDirectory = Split-Path -Parent $destinationPath
    if (-not (Test-Path -LiteralPath $destinationDirectory)) {
        New-Item -ItemType Directory -Path $destinationDirectory -Force | Out-Null
    }

    $tempPath = $destinationPath + ".goldenpath.tmp"
    if (Test-Path -LiteralPath $tempPath) {
        Remove-Item -LiteralPath $tempPath -Force
    }

    try {
        [System.IO.File]::Copy($sourceFullPath, $tempPath, $true)

        $tempInfo = Get-PngAssetInfo `
            $tempPath `
            $ExpectedWidth `
            $ExpectedHeight `
            (-not $AllowOpaque.IsPresent)

        if ($tempInfo.Sha256 -ne $sourceInfo.Sha256) {
            throw "Exact-copy hash mismatch before install: source=$($sourceInfo.Sha256) temp=$($tempInfo.Sha256)"
        }

        Move-Item -LiteralPath $tempPath -Destination $destinationPath -Force

        $destinationInfo = Get-PngAssetInfo `
            $destinationPath `
            $ExpectedWidth `
            $ExpectedHeight `
            (-not $AllowOpaque.IsPresent)

        if ($destinationInfo.Sha256 -ne $sourceInfo.Sha256) {
            throw "Exact-copy hash mismatch after install: source=$($sourceInfo.Sha256) destination=$($destinationInfo.Sha256)"
        }
    }
    finally {
        if (Test-Path -LiteralPath $tempPath) {
            Remove-Item -LiteralPath $tempPath -Force
        }
    }

    Pass "Exact PNG bytes installed: $DestinationRelativePath"
    Pass "Post-copy SHA-256 matches source: $($sourceInfo.Sha256)"
    Write-Host "[NEXT] Update the owning Def/path only after this step succeeds, then run run-tests.bat and the matching texture-debug biome."
}
catch {
    Fail $_.Exception.Message
}
