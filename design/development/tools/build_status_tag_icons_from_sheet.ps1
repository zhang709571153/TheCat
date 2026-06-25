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

$sheetPath = Join-Path $ProjectRoot "Assets/TheCat/Art/_GeneratedReferences/thecat_style_status_icons_5x64_v001.png"
$iconDir = Join-Path $ProjectRoot "Assets/TheCat/Art/UI/Icons"
$metaTemplatePath = Join-Path $iconDir "thecat_ui_core_sleep_icon_64_v001.png.meta"
$reviewDir = Join-Path $ProjectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_status_tag_icon_split_2026-06-14.md"

$icons = @(
    @{ AssetId = "thecat_ui_status_sleep_stable_64_v001"; SourceCell = 0; Subject = "sleep_stable"; Display = "Sleep Stable" },
    @{ AssetId = "thecat_ui_status_slow_64_v001"; SourceCell = 1; Subject = "slow"; Display = "Slow" },
    @{ AssetId = "thecat_ui_status_knockback_64_v001"; SourceCell = 2; Subject = "knockback"; Display = "Knockback" },
    @{ AssetId = "thecat_ui_status_mark_64_v001"; SourceCell = 3; Subject = "mark"; Display = "Mark" },
    @{ AssetId = "thecat_ui_status_shield_64_v001"; SourceCell = 4; Subject = "shield"; Display = "Shield" }
)

if (!(Test-Path -LiteralPath $sheetPath)) {
    throw "Missing source sheet: $sheetPath"
}

if (!(Test-Path -LiteralPath $metaTemplatePath)) {
    throw "Missing meta template: $metaTemplatePath"
}

New-Item -ItemType Directory -Force -Path $iconDir | Out-Null
New-Item -ItemType Directory -Force -Path $reviewDir | Out-Null

$template = Get-Content -LiteralPath $metaTemplatePath -Raw
$sheet = [System.Drawing.Bitmap]::FromFile($sheetPath)

try {
    if ($sheet.Width -ne 320 -or $sheet.Height -ne 64) {
        throw "Expected status sheet 320x64, found $($sheet.Width)x$($sheet.Height): $sheetPath"
    }

    $reviewLines = New-Object System.Collections.Generic.List[string]
    $reviewLines.Add("# P0 Status Tag Icon Split")
    $reviewLines.Add("")
    $reviewLines.Add('- Source sheet: `Assets/TheCat/Art/_GeneratedReferences/thecat_style_status_icons_5x64_v001.png`')
    $reviewLines.Add('- Output directory: `Assets/TheCat/Art/UI/Icons`')
    $reviewLines.Add("- Cell order: Sleep Stable, Slow, Knockback, Mark, Shield")
    $reviewLines.Add("")
    $reviewLines.Add("| subject | display | source cell | output | md5 |")
    $reviewLines.Add("| --- | --- | --- | --- | --- |")

    foreach ($icon in $icons) {
        $assetId = [string]($icon["AssetId"])
        $cell = [int]($icon["SourceCell"])
        $outputPath = Join-Path $iconDir ($assetId + ".png")
        $metaPath = $outputPath + ".meta"
        $rect = New-Object -TypeName System.Drawing.Rectangle -ArgumentList (($cell * 64), 0, 64, 64)
        $crop = $sheet.Clone($rect, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)

        try {
            $crop.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
        }
        finally {
            $crop.Dispose()
        }

        $guid = Get-HexMd5 ("TheCatUnityGuid:" + $assetId)
        $spriteId = Get-HexMd5 ("TheCatSpriteId:" + $assetId)
        $meta = $template `
            -replace "(?m)^guid: [0-9a-f]+", ("guid: " + $guid) `
            -replace "(?m)^    spriteID: [0-9a-f]+", ("    spriteID: " + $spriteId)
        Set-Content -LiteralPath $metaPath -Value $meta -NoNewline -Encoding ascii

        $hash = Get-HexMd5 (([System.IO.File]::ReadAllBytes($outputPath)) -join ",")
        $relativeOutput = "Assets/TheCat/Art/UI/Icons/" + $assetId + ".png"
        $reviewLines.Add("| $($icon["Subject"]) | $($icon["Display"]) | $cell | ``$relativeOutput`` | ``$hash`` |")
    }

    Set-Content -LiteralPath $reviewNotePath -Value $reviewLines -Encoding utf8
    Write-Output "Generated $($icons.Count) status tag icons from $sheetPath"
    Write-Output "Review note: $reviewNotePath"
}
finally {
    $sheet.Dispose()
}
