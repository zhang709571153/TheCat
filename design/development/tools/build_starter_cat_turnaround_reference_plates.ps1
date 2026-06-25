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
$batchRelative = "design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15"
$batchPath = Join-Path $projectRoot $batchRelative
New-Item -ItemType Directory -Force -Path $batchPath | Out-Null

$manifestRelative = "$batchRelative/starter_cat_turnaround_reference_plates_batch70_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchRelative/thecat_cat_starter_turnaround_reference_plates_batch70_review_sheet.png"
$reviewSheetPath = Join-Path $projectRoot $reviewSheetRelative
$reviewRelative = "$batchRelative/starter_cat_turnaround_reference_plates_batch70_review.md"
$reviewPath = Join-Path $projectRoot $reviewRelative
$processRelative = "$batchRelative/starter_cat_turnaround_reference_plates_batch70_process_note.md"
$processPath = Join-Path $projectRoot $processRelative

$cats = @(
    @{
        Id = "saiban"
        DisplayName = "Saiban"
        SourceLock = "saiban_turnaround_colored"
        SourcePath = "$designAssetRootRelative/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png"
        Traits = "tabby face; red cape; sun shield; sword; silver-gold armor"
    },
    @{
        Id = "nephthys"
        DisplayName = "Nephthys"
        SourceLock = "nephthys_turnaround_colored"
        SourcePath = "$designAssetRootRelative/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png"
        Traits = "gold-brown tabby; dark blue hood; crescent; pyramid; gold script trim"
    },
    @{
        Id = "suzune"
        DisplayName = "Suzune"
        SourceLock = "suzune_turnaround_colored"
        SourcePath = "$designAssetRootRelative/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png"
        Traits = "calico face patches; shrine robe; vermilion bow; bells; wand and talismans"
    }
)

$views = @(
    @{ Id = "front"; DisplayName = "Front"; Index = 0 },
    @{ Id = "side"; DisplayName = "Side"; Index = 1 },
    @{ Id = "back"; DisplayName = "Back"; Index = 2 }
)

function Resolve-ProjectPath {
    param([string]$RelativePath)
    return Join-Path $projectRoot ($RelativePath -replace "/", [System.IO.Path]::DirectorySeparatorChar)
}

function Get-HashText {
    param([string]$Path)
    return (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
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

function New-Brush {
    param([int]$R, [int]$G, [int]$B)
    return [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb($R, $G, $B))
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

function Draw-Plate {
    param(
        [System.Drawing.Graphics]$Graphics,
        [string]$Path,
        [int]$X,
        [int]$Y,
        [int]$Width,
        [int]$Height,
        [System.Drawing.Pen]$Pen
    )

    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        Draw-ContainedImage $Graphics $image $X $Y $Width $Height
    } finally {
        $image.Dispose()
    }

    $Graphics.DrawRectangle($Pen, $X, $Y, $Width, $Height)
}

function New-CropRectangle {
    param(
        [System.Drawing.Image]$Image,
        [int]$ViewIndex
    )

    $overlap = [Math]::Max(24, [int]($Image.Width * 0.018))
    $third = [double]$Image.Width / 3.0
    $left = [int][Math]::Floor($third * $ViewIndex)
    $right = if ($ViewIndex -eq 2) { $Image.Width } else { [int][Math]::Ceiling($third * ($ViewIndex + 1)) }
    $x = [Math]::Max(0, $left - $overlap)
    $x2 = [Math]::Min($Image.Width, $right + $overlap)
    return [System.Drawing.Rectangle]::new($x, 0, $x2 - $x, $Image.Height)
}

function Save-ReferencePlate {
    param(
        [string]$SourcePath,
        [System.Drawing.Rectangle]$CropRect,
        [string]$OutputPath
    )

    $source = [System.Drawing.Image]::FromFile($SourcePath)
    $plateSize = 768
    $plate = [System.Drawing.Bitmap]::new($plateSize, $plateSize)
    $graphics = [System.Drawing.Graphics]::FromImage($plate)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $background = New-Brush 250 246 236
    $borderPen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(186, 170, 154), 3)

    try {
        $graphics.FillRectangle($background, 0, 0, $plateSize, $plateSize)
        $scale = [Math]::Min(704 / $CropRect.Width, 704 / $CropRect.Height)
        $drawWidth = [int]($CropRect.Width * $scale)
        $drawHeight = [int]($CropRect.Height * $scale)
        $drawX = [int](($plateSize - $drawWidth) / 2)
        $drawY = [int](($plateSize - $drawHeight) / 2)
        $dest = [System.Drawing.Rectangle]::new($drawX, $drawY, $drawWidth, $drawHeight)
        $graphics.DrawImage($source, $dest, $CropRect, [System.Drawing.GraphicsUnit]::Pixel)
        $graphics.DrawRectangle($borderPen, 8, 8, $plateSize - 16, $plateSize - 16)
        $plate.Save($OutputPath, [System.Drawing.Imaging.ImageFormat]::Png)
    } finally {
        $borderPen.Dispose()
        $background.Dispose()
        $graphics.Dispose()
        $plate.Dispose()
        $source.Dispose()
    }
}

$missing = @()
foreach ($cat in $cats) {
    $fullPath = Resolve-ProjectPath $cat.SourcePath
    if (-not (Test-Path -LiteralPath $fullPath)) {
        $missing += $cat.SourcePath
    }
}

if ($missing.Count -gt 0) {
    throw "Missing starter-cat turnaround source(s): $($missing -join ', ')"
}

$rows = @()
foreach ($cat in $cats) {
    $sourceFull = Resolve-ProjectPath $cat.SourcePath
    $sourceHash = Get-HashText $sourceFull
    $sourceSize = Get-ImageSizeText $sourceFull
    $sourceImage = [System.Drawing.Image]::FromFile($sourceFull)
    try {
        foreach ($view in $views) {
            $cropRect = New-CropRectangle $sourceImage $view.Index
            $outputName = "thecat_cat_$($cat.Id)_turnaround_$($view.Id)_reference_plate_768_batch70_v001.png"
            $outputRelative = "$batchRelative/$outputName"
            $outputFull = Resolve-ProjectPath $outputRelative
            Save-ReferencePlate $sourceFull $cropRect $outputFull
            $rows += [pscustomobject]@{
                cat_id = $cat.Id
                display_name = $cat.DisplayName
                view = $view.Id
                source_lock_id = $cat.SourceLock
                source_turnaround_path = $cat.SourcePath
                source_turnaround_sha256 = $sourceHash
                source_turnaround_size = $sourceSize
                crop_rect = "$($cropRect.X),$($cropRect.Y),$($cropRect.Width),$($cropRect.Height)"
                reference_plate_path = $outputRelative
                reference_plate_sha256 = Get-HashText $outputFull
                reference_plate_size = Get-ImageSizeText $outputFull
                required_traits = $cat.Traits
                import_status = "reference_only_do_not_import_pending_active_cat_playmode_screenshot"
            }
        }
    } finally {
        $sourceImage.Dispose()
    }
}

$rows | Export-Csv -LiteralPath $manifestPath -NoTypeInformation -Encoding UTF8

$sheetWidth = 2100
$sheetHeight = 1750
$margin = 44
$headerHeight = 160
$rowHeight = 480
$rowGap = 36
$plateSizeOnSheet = 360

$sheet = [System.Drawing.Bitmap]::new($sheetWidth, $sheetHeight)
$graphics = [System.Drawing.Graphics]::FromImage($sheet)
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality

$background = New-Brush 29 26 36
$rowBrush = New-Brush 250 246 236
$titleBrush = New-Brush 255 246 214
$textBrush = New-Brush 46 40 45
$mutedBrush = New-Brush 92 82 92
$warningBrush = New-Brush 126 54 65
$borderPen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(94, 78, 112), 3)
$platePen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(181, 168, 154), 2)
$titleFont = [System.Drawing.Font]::new("Arial", 34, [System.Drawing.FontStyle]::Bold)
$subtitleFont = [System.Drawing.Font]::new("Arial", 15, [System.Drawing.FontStyle]::Regular)
$catFont = [System.Drawing.Font]::new("Arial", 22, [System.Drawing.FontStyle]::Bold)
$viewFont = [System.Drawing.Font]::new("Arial", 15, [System.Drawing.FontStyle]::Bold)
$metaFont = [System.Drawing.Font]::new("Arial", 12, [System.Drawing.FontStyle]::Regular)
$monoFont = [System.Drawing.Font]::new("Consolas", 10, [System.Drawing.FontStyle]::Regular)

try {
    $graphics.FillRectangle($background, 0, 0, $sheetWidth, $sheetHeight)
    $graphics.DrawString("Batch 70 Starter Cat Source Turnaround Reference Plates", $titleFont, $titleBrush, $margin, 26)
    $graphics.DrawString("Deterministic crops from locked colored three-view turnarounds. Reference only; do not import into Unity.", $subtitleFont, $titleBrush, $margin, 84)
    $graphics.DrawString("Use these plates as hard visual inputs for future Codex image generation and active-cat screenshot review.", $subtitleFont, $titleBrush, $margin, 112)

    for ($catIndex = 0; $catIndex -lt $cats.Count; $catIndex++) {
        $cat = $cats[$catIndex]
        $y = $headerHeight + ($catIndex * ($rowHeight + $rowGap))
        $graphics.FillRectangle($rowBrush, $margin, $y, $sheetWidth - ($margin * 2), $rowHeight)
        $graphics.DrawRectangle($borderPen, $margin, $y, $sheetWidth - ($margin * 2), $rowHeight)
        $graphics.DrawString("$($cat.DisplayName) / $($cat.SourceLock)", $catFont, $textBrush, $margin + 24, $y + 20)

        for ($viewIndex = 0; $viewIndex -lt $views.Count; $viewIndex++) {
            $view = $views[$viewIndex]
            $row = $rows | Where-Object { $_.cat_id -eq $cat.Id -and $_.view -eq $view.Id } | Select-Object -First 1
            $x = $margin + 28 + ($viewIndex * 420)
            $plateY = $y + 72
            $graphics.DrawString($view.DisplayName, $viewFont, $textBrush, $x, $plateY - 28)
            Draw-Plate $graphics (Resolve-ProjectPath $row.reference_plate_path) $x $plateY $plateSizeOnSheet $plateSizeOnSheet $platePen
            $graphics.DrawString("crop: $($row.crop_rect)", $monoFont, $mutedBrush, $x, $plateY + $plateSizeOnSheet + 10)
        }

        $textX = $margin + 1320
        $textY = $y + 78
        $graphics.DrawString("Reference status", $viewFont, $textBrush, $textX, $textY)
        $graphics.DrawString("No Unity import approval", $metaFont, $warningBrush, $textX, $textY + 30)
        $graphics.DrawString("views: front / side / back", $metaFont, $mutedBrush, $textX, $textY + 58)
        $graphics.DrawString("source size: " + ($rows | Where-Object { $_.cat_id -eq $cat.Id } | Select-Object -First 1).source_turnaround_size, $metaFont, $mutedBrush, $textX, $textY + 84)
        $graphics.DrawString("traits:", $metaFont, $mutedBrush, $textX, $textY + 116)
        $traits = $cat.Traits -split "; "
        for ($i = 0; $i -lt $traits.Count; $i++) {
            $graphics.DrawString("- " + $traits[$i], $metaFont, $textBrush, $textX, $textY + 140 + ($i * 22))
        }
    }

    $sheet.Save($reviewSheetPath, [System.Drawing.Imaging.ImageFormat]::Png)
} finally {
    $monoFont.Dispose()
    $metaFont.Dispose()
    $viewFont.Dispose()
    $catFont.Dispose()
    $subtitleFont.Dispose()
    $titleFont.Dispose()
    $platePen.Dispose()
    $borderPen.Dispose()
    $warningBrush.Dispose()
    $mutedBrush.Dispose()
    $textBrush.Dispose()
    $titleBrush.Dispose()
    $rowBrush.Dispose()
    $background.Dispose()
    $graphics.Dispose()
    $sheet.Dispose()
}

$reviewLines = @(
    "# Batch 70 Starter Cat Source Turnaround Reference Plates",
    "",
    "Generated: 2026-06-15",
    "",
    "Purpose: produce deterministic front, side, and back reference plates directly from the locked colored three-view starter-cat turnarounds. These plates are hard visual inputs for future Codex image generation, local cutout work, and active-cat Play Mode screenshot review.",
    "",
    "Status: reference-only. Do not import into Unity yet.",
    "",
    "- Review sheet: ``$reviewSheetRelative``",
    "- Review sheet SHA-256: ``$(Get-HashText $reviewSheetPath)``",
    "- Manifest: ``$manifestRelative``",
    "- Process note: ``$processRelative``",
    "- Generated by: ``design/development/tools/build_starter_cat_turnaround_reference_plates.ps1``",
    "",
    "## Reference Rules",
    "",
    "- Source-derived only: no image generation, repainting, inpainting, recoloring, retouching, or costume simplification was performed.",
    "- Each plate is a deterministic crop and resize from the matching colored three-view turnaround.",
    "- Plates are reference inputs, not final transparent runtime sprites.",
    "- Reject any future candidate that changes body proportion, face markings, palette, costume, props, civilization motifs, or cat-first anatomy from these plates.",
    "- Formal Unity import remains blocked until active-cat Play Mode screenshot comparison passes.",
    "",
    "## Rows"
)

foreach ($row in $rows) {
    $reviewLines += ""
    $reviewLines += "### $($row.display_name) / $($row.view)"
    $reviewLines += ""
    $reviewLines += "- Source lock: ``$($row.source_lock_id)``"
    $reviewLines += "- Source turnaround: ``$($row.source_turnaround_path)``"
    $reviewLines += "- Source SHA-256: ``$($row.source_turnaround_sha256)``"
    $reviewLines += "- Crop rect: ``$($row.crop_rect)``"
    $reviewLines += "- Reference plate: ``$($row.reference_plate_path)``"
    $reviewLines += "- Reference plate SHA-256: ``$($row.reference_plate_sha256)``"
    $reviewLines += "- Import status: ``$($row.import_status)``"
}

Set-Content -LiteralPath $reviewPath -Encoding UTF8 -Value $reviewLines

$processLines = @(
    "# Batch 70 Process Note",
    "",
    "No image generation was performed.",
    "",
    "The build script located the real design root by discovering the Saiban colored turnaround sentinel under ``design/*/assets``. This avoids hard-coded encoded directory names.",
    "",
    "No source turnarounds, Unity runtime sprites, Unity ``.meta`` files, source-lock hashes, runtime bindings, or formal starter-cat import decisions were modified.",
    "",
    "The reference plates are deterministic crop-and-resize derivatives from the locked colored three-view turnarounds. They are intended for future Codex image generation prompts, source-lock review, and active-cat Play Mode screenshot comparison.",
    "",
    "Unity-side validation remains pending: capture active-cat Play Mode screenshots for Saiban, Nephthys, and Suzune, compare them against the Batch 70 plates and Batch 69 runtime comparison sheet, inspect SpriteRenderer/HUD references, and confirm Console has no errors before any starter-cat sprite replacement."
)

Set-Content -LiteralPath $processPath -Encoding UTF8 -Value $processLines

Write-Output "Built Batch 70 starter-cat source turnaround reference plates: $batchRelative"
