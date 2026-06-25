Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$designRootDirectory = Get-ChildItem -LiteralPath (Join-Path $projectRoot "design") -Directory |
    Where-Object {
        Test-Path -LiteralPath (Join-Path $_.FullName "assets\characters\ch01_saiban_swordsaint\turnaround\saiban_turnaround_colored_2026-06-03.png")
    } |
    Select-Object -First 1

if ($null -eq $designRootDirectory) {
    throw "Could not locate the design asset root under design/*/assets/characters."
}

$designAssetRootRelative = "design/$($designRootDirectory.Name)/assets"
$batchRelative = "design/development/asset_candidates/starter_cats/batch_69_turnaround_runtime_comparison_audit_2026-06-15"
$batchPath = Join-Path $projectRoot $batchRelative
New-Item -ItemType Directory -Force -Path $batchPath | Out-Null

$reviewSheetRelative = "$batchRelative/thecat_cat_starter_turnaround_runtime_comparison_batch69_review_sheet.png"
$reviewSheetPath = Join-Path $projectRoot $reviewSheetRelative
$manifestRelative = "$batchRelative/starter_cat_turnaround_runtime_comparison_batch69_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewRelative = "$batchRelative/starter_cat_turnaround_runtime_comparison_batch69_review.md"
$reviewPath = Join-Path $projectRoot $reviewRelative
$processRelative = "$batchRelative/starter_cat_turnaround_runtime_comparison_batch69_process_note.md"
$processPath = Join-Path $projectRoot $processRelative

$cats = @(
    @{
        Id = "saiban"
        DisplayName = "Saiban"
        SourceLock = "saiban_turnaround_colored"
        SourcePath = "$designAssetRootRelative/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png"
        SpritePath = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png"
        ActiveScreenshot = "04-active-cat-saiban.png"
        RequiredTraits = "tabby face; red cape; round sun shield; sword; silver-gold armor"
    },
    @{
        Id = "nephthys"
        DisplayName = "Nephthys"
        SourceLock = "nephthys_turnaround_colored"
        SourcePath = "$designAssetRootRelative/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png"
        SpritePath = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png"
        ActiveScreenshot = "05-active-cat-nephthys.png"
        RequiredTraits = "gold-brown tabby; dark blue hood; crescent; pyramid and obelisk; gold script trim"
    },
    @{
        Id = "suzune"
        DisplayName = "Suzune"
        SourceLock = "suzune_turnaround_colored"
        SourcePath = "$designAssetRootRelative/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png"
        SpritePath = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png"
        ActiveScreenshot = "06-active-cat-suzune.png"
        RequiredTraits = "calico face patches; shrine robe; vermilion bow; bells; wand and talismans"
    }
)

function Resolve-ProjectPath {
    param([string]$RelativePath)
    return Join-Path $projectRoot ($RelativePath -replace "/", [System.IO.Path]::DirectorySeparatorChar)
}

function Get-ImageSizeText {
    param([string]$Path)

    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        return "$($image.Width)x$($image.Height)"
    } finally {
        $image.Dispose()
    }
}

function Get-HashText {
    param([string]$Path)

    return (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
}

function Draw-ContainedImage {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Image]$Image,
        [int]$X,
        [int]$Y,
        [int]$Width,
        [int]$Height
    )

    $scale = [Math]::Min($Width / $Image.Width, $Height / $Image.Height)
    $drawWidth = [int]($Image.Width * $scale)
    $drawHeight = [int]($Image.Height * $scale)
    $drawX = $X + [int](($Width - $drawWidth) / 2)
    $drawY = $Y + [int](($Height - $drawHeight) / 2)
    $Graphics.DrawImage($Image, $drawX, $drawY, $drawWidth, $drawHeight)
}

function Draw-Checkerboard {
    param(
        [System.Drawing.Graphics]$Graphics,
        [int]$X,
        [int]$Y,
        [int]$Width,
        [int]$Height
    )

    $light = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(244, 241, 232))
    $dark = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(218, 212, 203))
    try {
        $tile = 20
        for ($yy = $Y; $yy -lt $Y + $Height; $yy += $tile) {
            for ($xx = $X; $xx -lt $X + $Width; $xx += $tile) {
                $tileX = [int](($xx - $X) / $tile)
                $tileY = [int](($yy - $Y) / $tile)
                $brush = if ((($tileX + $tileY) % 2) -eq 0) { $light } else { $dark }
                $Graphics.FillRectangle($brush, $xx, $yy, $tile, $tile)
            }
        }
    } finally {
        $light.Dispose()
        $dark.Dispose()
    }
}

function Draw-ImagePanel {
    param(
        [System.Drawing.Graphics]$Graphics,
        [string]$Path,
        [int]$X,
        [int]$Y,
        [int]$Width,
        [int]$Height,
        [System.Drawing.Pen]$BorderPen
    )

    Draw-Checkerboard $Graphics $X $Y $Width $Height
    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        Draw-ContainedImage $Graphics $image $X $Y $Width $Height
    } finally {
        $image.Dispose()
    }

    $Graphics.DrawRectangle($BorderPen, $X, $Y, $Width, $Height)
}

$missing = @()
foreach ($cat in $cats) {
    foreach ($pathKey in @("SourcePath", "SpritePath")) {
        $fullPath = Resolve-ProjectPath $cat[$pathKey]
        if (-not (Test-Path -LiteralPath $fullPath)) {
            $missing += $cat[$pathKey]
        }
    }
}

if ($missing.Count -gt 0) {
    throw "Missing starter-cat audit input(s): $($missing -join ', ')"
}

$rows = @()
foreach ($cat in $cats) {
    $sourceFull = Resolve-ProjectPath $cat.SourcePath
    $spriteFull = Resolve-ProjectPath $cat.SpritePath
    $rows += [pscustomobject]@{
        cat_id = $cat.Id
        display_name = $cat.DisplayName
        source_lock_id = $cat.SourceLock
        source_turnaround_path = $cat.SourcePath
        source_turnaround_sha256 = Get-HashText $sourceFull
        source_turnaround_size = Get-ImageSizeText $sourceFull
        current_sprite_path = $cat.SpritePath
        current_sprite_sha256 = Get-HashText $spriteFull
        current_sprite_size = Get-ImageSizeText $spriteFull
        active_screenshot_target = $cat.ActiveScreenshot
        required_traits = $cat.RequiredTraits
        review_sheet = $reviewSheetRelative
        recommendation = "audit_only_no_import_pending_active_cat_playmode_screenshot"
    }
}

$rows | Export-Csv -LiteralPath $manifestPath -NoTypeInformation -Encoding UTF8

$width = 1800
$height = 1460
$margin = 40
$headerHeight = 150
$rowHeight = 390
$rowGap = 30
$sourceWidth = 980
$spriteWidth = 320
$textWidth = 360
$sourceHeight = 300
$spriteHeight = 300

$bitmap = [System.Drawing.Bitmap]::new($width, $height)
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality

$background = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(28, 26, 36))
$rowBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(247, 243, 234))
$titleBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 246, 210))
$textBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(42, 37, 45))
$mutedBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(92, 82, 92))
$warningBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(126, 54, 65))
$borderPen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(93, 77, 112), 3)
$imageBorderPen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(180, 169, 180), 2)
$titleFont = [System.Drawing.Font]::new("Arial", 34, [System.Drawing.FontStyle]::Bold)
$subtitleFont = [System.Drawing.Font]::new("Arial", 15, [System.Drawing.FontStyle]::Regular)
$catFont = [System.Drawing.Font]::new("Arial", 20, [System.Drawing.FontStyle]::Bold)
$metaFont = [System.Drawing.Font]::new("Arial", 12, [System.Drawing.FontStyle]::Regular)
$monoFont = [System.Drawing.Font]::new("Consolas", 11, [System.Drawing.FontStyle]::Regular)

try {
    $graphics.FillRectangle($background, 0, 0, $width, $height)
    $graphics.DrawString("Batch 69 Starter Cat Turnaround Runtime Comparison", $titleFont, $titleBrush, $margin, 24)
    $graphics.DrawString("Source colored three-view turnarounds vs current Unity combat sprites. Audit only; no import approval.", $subtitleFont, $titleBrush, $margin, 78)
    $graphics.DrawString("Formal cat sprite replacement still requires active-cat Play Mode screenshots.", $subtitleFont, $titleBrush, $margin, 104)

    for ($i = 0; $i -lt $cats.Count; $i++) {
        $cat = $cats[$i]
        $row = $rows[$i]
        $y = $headerHeight + ($i * ($rowHeight + $rowGap))
        $graphics.FillRectangle($rowBrush, $margin, $y, $width - ($margin * 2), $rowHeight)
        $graphics.DrawRectangle($borderPen, $margin, $y, $width - ($margin * 2), $rowHeight)

        $sourceX = $margin + 24
        $sourceY = $y + 58
        $spriteX = $sourceX + $sourceWidth + 28
        $spriteY = $sourceY
        $textX = $spriteX + $spriteWidth + 28
        $textY = $y + 32

        $graphics.DrawString("$($cat.DisplayName) / $($cat.SourceLock)", $catFont, $textBrush, $sourceX, $y + 18)
        $graphics.DrawString("Source colored turnaround", $metaFont, $mutedBrush, $sourceX, $sourceY - 22)
        $graphics.DrawString("Current Unity combat sprite", $metaFont, $mutedBrush, $spriteX, $spriteY - 22)

        Draw-ImagePanel $graphics (Resolve-ProjectPath $cat.SourcePath) $sourceX $sourceY $sourceWidth $sourceHeight $imageBorderPen
        Draw-ImagePanel $graphics (Resolve-ProjectPath $cat.SpritePath) $spriteX $spriteY $spriteWidth $spriteHeight $imageBorderPen

        $graphics.DrawString("Audit status", $catFont, $textBrush, $textX, $textY)
        $graphics.DrawString("No import approval", $metaFont, $warningBrush, $textX, $textY + 36)
        $graphics.DrawString("active screenshot:", $metaFont, $mutedBrush, $textX, $textY + 66)
        $graphics.DrawString($cat.ActiveScreenshot, $monoFont, $textBrush, $textX, $textY + 88)
        $graphics.DrawString("source size: $($row.source_turnaround_size)", $metaFont, $mutedBrush, $textX, $textY + 120)
        $graphics.DrawString("sprite size: $($row.current_sprite_size)", $metaFont, $mutedBrush, $textX, $textY + 144)
        $graphics.DrawString("required traits:", $metaFont, $mutedBrush, $textX, $textY + 176)

        $traitLines = $cat.RequiredTraits -split "; "
        for ($t = 0; $t -lt $traitLines.Count; $t++) {
            $graphics.DrawString("- " + $traitLines[$t], $metaFont, $textBrush, $textX, $textY + 198 + ($t * 22))
        }
    }

    $bitmap.Save($reviewSheetPath, [System.Drawing.Imaging.ImageFormat]::Png)
} finally {
    $graphics.Dispose()
    $bitmap.Dispose()
    $background.Dispose()
    $rowBrush.Dispose()
    $titleBrush.Dispose()
    $textBrush.Dispose()
    $mutedBrush.Dispose()
    $warningBrush.Dispose()
    $borderPen.Dispose()
    $imageBorderPen.Dispose()
    $titleFont.Dispose()
    $subtitleFont.Dispose()
    $catFont.Dispose()
    $metaFont.Dispose()
    $monoFont.Dispose()
}

$reviewSheetHash = Get-HashText $reviewSheetPath
$reviewLines = @(
    "# Batch 69 Starter Cat Turnaround Runtime Comparison Audit",
    "",
    "Generated: 2026-06-15",
    "",
    "Purpose: show the real colored three-view turnaround source beside the current Unity combat sprite for Saiban, Nephthys, and Suzune before any further starter-cat image generation or import decision.",
    "",
    "Status: audit-only. Do not import into Unity yet.",
    "",
    "- Review sheet: ``$reviewSheetRelative``",
    "- Review sheet SHA-256: ``$reviewSheetHash``",
    "- Manifest: ``$manifestRelative``",
    "- Process note: ``$processRelative``",
    "- Generated by: ``design/development/tools/build_starter_cat_turnaround_runtime_comparison_audit.ps1``",
    "",
    "## Audit Rows",
    ""
)

foreach ($row in $rows) {
    $reviewLines += "### $($row.display_name)"
    $reviewLines += ""
    $reviewLines += "- Source lock: ``$($row.source_lock_id)``"
    $reviewLines += "- Source turnaround: ``$($row.source_turnaround_path)``"
    $reviewLines += "- Source SHA-256: ``$($row.source_turnaround_sha256)``"
    $reviewLines += "- Current Unity sprite: ``$($row.current_sprite_path)``"
    $reviewLines += "- Current sprite SHA-256: ``$($row.current_sprite_sha256)``"
    $reviewLines += "- Active-cat screenshot target: ``$($row.active_screenshot_target)``"
    $reviewLines += "- Required traits to compare: $($row.required_traits)"
    $reviewLines += "- Decision: audit-only hold. Formal Unity import remains blocked until active-cat Play Mode screenshot comparison passes."
    $reviewLines += ""
}

$reviewLines += "## Human Review Instructions"
$reviewLines += ""
$reviewLines += "- Compare body proportion, face markings, palette, costume, props, and civilization motifs against the colored three-view turnaround."
$reviewLines += "- Reject current or future runtime cat art if it drifts toward generic cute-cat, generated-lineup, or human-proportion styling."
$reviewLines += "- Use this packet as preflight evidence before capturing active-cat Play Mode screenshots."
$reviewLines += "- This packet does not approve cat image generation and does not approve Unity import."

Set-Content -LiteralPath $reviewPath -Value $reviewLines -Encoding UTF8

$processLines = @(
    "# Batch 69 Starter Cat Turnaround Runtime Comparison Process Note",
    "",
    "Generated: 2026-06-15",
    "",
    "Inputs:",
    "",
    "- three colored turnaround source PNGs from ``$designAssetRootRelative/characters``",
    "- three current Unity combat sprites from ``Assets/TheCat/Art/Characters/Sprites``",
    "",
    "Outputs:",
    "",
    "- ``$reviewSheetRelative``",
    "- ``$manifestRelative``",
    "- ``$reviewRelative``",
    "",
    "No image generation was performed. No source turnarounds, current Unity sprites, Unity ``.meta`` files, source hashes, runtime bindings, or formal starter-cat import decisions were modified.",
    "",
    "Unity-side validation remains pending: capture active-cat Play Mode screenshots ``04-active-cat-saiban.png``, ``05-active-cat-nephthys.png``, and ``06-active-cat-suzune.png`` and compare them against this audit packet and the original colored three-view turnarounds."
)

Set-Content -LiteralPath $processPath -Value $processLines -Encoding UTF8

Write-Output "Wrote $reviewSheetRelative"
Write-Output "Wrote $manifestRelative"
Write-Output "Wrote $reviewRelative"
Write-Output "Wrote $processRelative"
