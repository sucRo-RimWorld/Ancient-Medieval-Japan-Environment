$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
. (Join-Path $root 'Scripts/PngImageData.ps1')

$files = @(Get-ChildItem (Join-Path $root 'Textures') -Recurse -Filter '*.png')
if ($files.Count -eq 0) { throw 'No production PNGs were found' }
foreach ($file in $files) {
    [AMJPngImageData]::Validate([IO.File]::ReadAllBytes($file.FullName))
    Write-Host "[OK] Decoded PNG payload: $($file.Name)"
}

# CRC-valid PNG with a wrong zlib checksum: the previous structural gate passed it.
# Generate from a healthy production file so the fixture remains small and deterministic.
[byte[]]$bytes = [IO.File]::ReadAllBytes($files[0].FullName)
$offset = 8
while ($offset + 12 -le $bytes.Length) {
    $length = [int](([uint32]$bytes[$offset] * 16777216) +
        ([uint32]$bytes[$offset + 1] * 65536) +
        ([uint32]$bytes[$offset + 2] * 256) + $bytes[$offset + 3])
    $type = [Text.Encoding]::ASCII.GetString($bytes, $offset + 4, 4)
    if ($type -eq 'IDAT') {
        # Last byte of this repository's single-IDAT zlib stream is Adler-32.
        $bytes[$offset + 7 + $length] = $bytes[$offset + 7 + $length] -bxor 1
        [uint32]$crc = 4294967295
        for ($i = $offset + 4; $i -lt $offset + 8 + $length; $i++) {
            $crc = [uint32]($crc -bxor $bytes[$i])
            for ($bit = 0; $bit -lt 8; $bit++) {
                if (($crc -band 1) -ne 0) {
                    $crc = [uint32](($crc -shr 1) -bxor [uint32]3988292384)
                } else { $crc = [uint32]($crc -shr 1) }
            }
        }
        $crc = [uint32]($crc -bxor [uint32]4294967295)
        for ($i = 0; $i -lt 4; $i++) {
            $bytes[$offset + 8 + $length + $i] = [byte](($crc -shr (24 - 8 * $i)) -band 255)
        }
        break
    }
    $offset += $length + 12
}
$rejected = $false
try { [AMJPngImageData]::Validate($bytes) }
catch {
    if ($_.Exception.ToString() -notmatch 'Adler-32 is invalid') { throw }
    $rejected = $true
}
if (-not $rejected) { throw 'CRC-valid but checksum-corrupt PNG was accepted' }
Write-Host '[OK] CRC-valid / zlib-corrupt regression fixture rejected'
