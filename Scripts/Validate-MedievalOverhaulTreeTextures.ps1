param(
    [Parameter(Mandatory = $true)]
    [string]$RimWorldDir
)

$ErrorActionPreference = "Stop"

function Fail([string]$Message) {
    Write-Host "[ERROR] $Message" -ForegroundColor Red
    exit 2
}

$steamApps = Resolve-Path (Join-Path $RimWorldDir "..\..")
$moRoot = Join-Path $steamApps "workshop\content\294100\3219596926"

if (-not (Test-Path -LiteralPath (Join-Path $moRoot "About\About.xml"))) {
    Write-Host "[INFO] Medieval Overhaul is not installed in the expected Workshop path; skipping static MO tree-texture audit."
    exit 0
}

$defsRoot = Join-Path $moRoot "1.6\Defs"
if (-not (Test-Path -LiteralPath $defsRoot)) {
    Fail "Medieval Overhaul 1.6 Defs folder was not found: $defsRoot"
}

$textureRoots = @(
    (Join-Path $moRoot "Textures"),
    (Join-Path $moRoot "1.6\Textures")
) | Where-Object { Test-Path -LiteralPath $_ }

if ($textureRoots.Count -eq 0) {
    Fail "No Medieval Overhaul texture roots were found under: $moRoot"
}

function Test-TexturePath([string]$TexPath) {
    if ([string]::IsNullOrWhiteSpace($TexPath)) {
        return $false
    }

    $relative = $TexPath.Replace("/", "\")
    foreach ($root in $textureRoots) {
        $exact = Join-Path $root ($relative + ".png")
        if (Test-Path -LiteralPath $exact) {
            return $true
        }

        $dir = Join-Path $root $relative
        if (Test-Path -LiteralPath $dir -PathType Container) {
            $png = Get-ChildItem -LiteralPath $dir -Filter "*.png" -File -ErrorAction SilentlyContinue |
                Select-Object -First 1
            if ($null -ne $png) {
                return $true
            }
        }

        $parent = Split-Path -Parent $dir
        $leaf = Split-Path -Leaf $dir
        if (Test-Path -LiteralPath $parent -PathType Container) {
            $prefixed = Get-ChildItem -LiteralPath $parent -Filter ($leaf + "*.png") -File -ErrorAction SilentlyContinue |
                Select-Object -First 1
            if ($null -ne $prefixed) {
                return $true
            }
        }
    }

    return $false
}

$stateTags = @(
    @{ Name = "base"; XPath = "graphicData/texPath" },
    @{ Name = "leafless"; XPath = "plant/leaflessGraphicPath" },
    @{ Name = "immature"; XPath = "plant/immatureGraphicPath" },
    @{ Name = "polluted"; XPath = "plant/pollutedGraphicPath" },
    @{ Name = "leaflessImmature"; XPath = "plant/leaflessImmatureGraphicPath" },
    @{ Name = "snowOverlay"; XPath = "plant/snowOverlayGraphicPath" },
    @{ Name = "leaflessSnowOverlay"; XPath = "plant/leaflessSnowOverlayGraphicPath" },
    @{ Name = "immatureSnowOverlay"; XPath = "plant/immatureSnowOverlayGraphicPath" }
)

$treeDefs = 0
$references = 0
$missing = New-Object System.Collections.Generic.List[string]

Get-ChildItem -LiteralPath $defsRoot -Recurse -Filter "*.xml" -File | ForEach-Object {
    try {
        [xml]$doc = Get-Content -LiteralPath $_.FullName -Raw
    }
    catch {
        Fail "Failed to parse Medieval Overhaul XML: $($_.FullName) :: $($_.Exception.Message)"
    }

    $nodes = $doc.SelectNodes("//ThingDef[@ParentName='TreeBase' or @ParentName='DeciduousTreeBase']")
    foreach ($node in @($nodes)) {
        $treeDefs++
        $defNameNode = $node.SelectSingleNode("defName")
        $defName = if ($null -eq $defNameNode) { "<unnamed>" } else { $defNameNode.InnerText.Trim() }

        foreach ($state in $stateTags) {
            $pathNode = $node.SelectSingleNode($state.XPath)
            if ($null -eq $pathNode) {
                continue
            }

            $texPath = $pathNode.InnerText.Trim()
            if ([string]::IsNullOrWhiteSpace($texPath)) {
                continue
            }

            $references++
            if (-not (Test-TexturePath $texPath)) {
                $entry = "${defName}:$($state.Name)=$texPath"
                $missing.Add($entry)
                Write-Host "[FAIL] Missing MO tree texture: $entry" -ForegroundColor Red
            }
        }
    }
}

if ($treeDefs -le 0) {
    Fail "No Medieval Overhaul 1.6 tree ThingDefs were found under: $defsRoot"
}

if ($references -le 0) {
    Fail "No Medieval Overhaul tree graphic references were found."
}

if ($missing.Count -gt 0) {
    Fail "Medieval Overhaul tree-texture audit found $($missing.Count) missing reference(s)."
}

Write-Host "[OK] Medieval Overhaul tree texture references resolve."
Write-Host "     Tree defs: $treeDefs"
Write-Host "     Graphic references: $references"
exit 0
