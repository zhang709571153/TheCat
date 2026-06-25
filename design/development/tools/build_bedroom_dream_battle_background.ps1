param(
    [string]$ProjectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

function Get-HexMd5([string]$Value) {
    $md5 = [System.Security.Cryptography.MD5]::Create()
    try {
        $bytes = [System.Text.Encoding]::UTF8.GetBytes($Value)
        $hash = $md5.ComputeHash($bytes)
        return -join ($hash | ForEach-Object { $_.ToString("x2") })
    }
    finally {
        $md5.Dispose()
    }
}

function Get-HexSha256([string]$Path) {
    $sha = [System.Security.Cryptography.SHA256]::Create()
    try {
        $stream = [System.IO.File]::OpenRead($Path)
        try {
            $hash = $sha.ComputeHash($stream)
            return -join ($hash | ForEach-Object { $_.ToString("x2") })
        }
        finally {
            $stream.Dispose()
        }
    }
    finally {
        $sha.Dispose()
    }
}

$assetId = "thecat_bg_bedroomdream_battle_1920x1080_v001"
$outputRelative = "Assets/TheCat/Art/Scenes/BedroomDream/$assetId.png"
$metaTemplateRelative = "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_bed_sleepglow_sprite_512_v001.png.meta"
$reviewRelative = "design/development/asset_review/p0_bedroom_dream_battle_background_2026-06-14.md"

$sourceFile = Get-ChildItem -LiteralPath (Join-Path $ProjectRoot "design") -Recurse -Filter "bedroom_dream_map_concept.png" |
    Where-Object { $_.FullName -replace "\\", "/" -like "*/assets/levels/lv01_bedroom_dream/concept/bedroom_dream_map_concept.png" } |
    Select-Object -First 1

if ($null -eq $sourceFile) {
    throw "Missing source image: bedroom_dream_map_concept.png under design assets."
}

$sourcePath = $sourceFile.FullName
$sourceRelative = $sourcePath.Substring($ProjectRoot.Length + 1).Replace("\", "/")
$outputPath = Join-Path $ProjectRoot $outputRelative
$metaPath = $outputPath + ".meta"
$metaTemplatePath = Join-Path $ProjectRoot $metaTemplateRelative
$reviewPath = Join-Path $ProjectRoot $reviewRelative

if (!(Test-Path -LiteralPath $metaTemplatePath)) {
    throw "Missing meta template: $metaTemplatePath"
}

New-Item -ItemType Directory -Force -Path (Split-Path -Parent $outputPath) | Out-Null
New-Item -ItemType Directory -Force -Path (Split-Path -Parent $reviewPath) | Out-Null

$source = [System.Drawing.Bitmap]::FromFile($sourcePath)
$target = New-Object System.Drawing.Bitmap 1920, 1080, ([System.Drawing.Imaging.PixelFormat]::Format32bppArgb)

try {
    $graphics = [System.Drawing.Graphics]::FromImage($target)
    try {
        $graphics.CompositingMode = [System.Drawing.Drawing2D.CompositingMode]::SourceCopy
        $graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
        $graphics.Clear([System.Drawing.Color]::FromArgb(255, 22, 18, 38))
        $destination = New-Object System.Drawing.Rectangle 0, 0, 1920, 1080
        $graphics.DrawImage($source, $destination)
    }
    finally {
        $graphics.Dispose()
    }

    $target.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
}
finally {
    $target.Dispose()
    $source.Dispose()
}

$template = Get-Content -LiteralPath $metaTemplatePath -Raw
$guid = Get-HexMd5 ("TheCatUnityGuid:" + $assetId)
$spriteId = Get-HexMd5 ("TheCatSpriteId:" + $assetId)
$meta = $template `
    -replace "(?m)^guid: [0-9a-f]+", ("guid: " + $guid) `
    -replace "(?m)^    spriteID: [0-9a-f]+", ("    spriteID: " + $spriteId) `
    -replace "(?m)^  maxTextureSize: [0-9]+", "  maxTextureSize: 2048" `
    -replace "(?m)^    maxTextureSize: [0-9]+", "    maxTextureSize: 2048"
Set-Content -LiteralPath $metaPath -Value $meta -NoNewline -Encoding ascii

$sourceHash = Get-HexSha256 $sourcePath
$outputHash = Get-HexSha256 $outputPath
$reviewLines = @(
    "# P0 Bedroom Dream Battle Background",
    "",
    ('- Asset id: `' + $assetId + '`'),
    "- Source lock id: ``bedroom_map_concept``",
    ('- Source image: `' + $sourceRelative + '`'),
    ('- Source SHA-256: `' + $sourceHash + '`'),
    ('- Output image: `' + $outputRelative + '`'),
    ('- Output SHA-256: `' + $outputHash + '`'),
    "- Output size: ``1920x1080``",
    "- Unity import: Sprite Single, mipmaps disabled, alpha transparency preserved, ``TheCatP0ImportSettings:v1``.",
    "",
    "## Production Decision",
    "",
    "- Accepted as a source-locked P0 battle-world background derived from the Bedroom Dream map concept.",
    "- This is not cat art and does not change starter-cat formal import state.",
    "- Use in Play Mode screenshot ``07-battle-world-visuals.png`` as the background behind bed, cat, enemies, litter box, and feeder."
)
Set-Content -LiteralPath $reviewPath -Value $reviewLines -Encoding utf8

Write-Output "Generated $outputRelative"
Write-Output "Wrote $metaPath"
Write-Output "Review note: $reviewRelative"
