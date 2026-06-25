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
$manifestRelative = "$batchRelative/starter_cat_turnaround_reference_plates_batch70_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchRelative/thecat_cat_starter_turnaround_reference_plates_batch70_review_sheet.png"
$reviewSheetPath = Join-Path $projectRoot $reviewSheetRelative
$reviewRelative = "$batchRelative/starter_cat_turnaround_reference_plates_batch70_review.md"
$reviewPath = Join-Path $projectRoot $reviewRelative
$processRelative = "$batchRelative/starter_cat_turnaround_reference_plates_batch70_process_note.md"
$processPath = Join-Path $projectRoot $processRelative

$expectedCats = @("saiban", "nephthys", "suzune")
$expectedViews = @("front", "side", "back")
$expectedLocks = @{
    saiban = "saiban_turnaround_colored"
    nephthys = "nephthys_turnaround_colored"
    suzune = "suzune_turnaround_colored"
}
$expectedSources = @{
    saiban = "$designAssetRootRelative/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png"
    nephthys = "$designAssetRootRelative/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png"
    suzune = "$designAssetRootRelative/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png"
}

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

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

function Test-Contains {
    param(
        [string]$Text,
        [string]$Token,
        [string]$Label
    )

    if ($Text.IndexOf($Token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        Add-Failure "$Label is missing token: $Token"
    }
}

if (-not (Test-Path -LiteralPath $batchPath)) {
    throw "Batch 70 reference plate directory is missing: $batchRelative"
}

foreach ($required in @($manifestPath, $reviewSheetPath, $reviewPath, $processPath)) {
    if (-not (Test-Path -LiteralPath $required)) {
        Add-Failure "Missing Batch 70 artifact: $required"
    }
}

if (Test-Path -LiteralPath $reviewSheetPath) {
    $sheetSize = Get-ImageSizeText $reviewSheetPath
    if ($sheetSize -ne "2100x1750") {
        Add-Failure "Review sheet should be 2100x1750 but is $sheetSize"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchPath -Filter *.meta -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 70 must not contain Unity .meta files: $($metaFiles.Name -join ', ')"
}

if (Test-Path -LiteralPath $manifestPath) {
    $rows = @(Import-Csv -LiteralPath $manifestPath)
    if ($rows.Count -ne 9) {
        Add-Failure "Expected 9 Batch 70 manifest rows but found $($rows.Count)"
    }

    foreach ($cat in $expectedCats) {
        foreach ($view in $expectedViews) {
            $matches = @($rows | Where-Object { $_.cat_id -eq $cat -and $_.view -eq $view })
            if ($matches.Count -ne 1) {
                Add-Failure "Expected one Batch 70 row for $cat/$view but found $($matches.Count)"
                continue
            }

            $row = $matches[0]
            if ($row.source_lock_id -ne $expectedLocks[$cat]) {
                Add-Failure "$cat/$view source lock drifted: $($row.source_lock_id)"
            }

            if ($row.source_turnaround_path -ne $expectedSources[$cat]) {
                Add-Failure "$cat/$view source path drifted: $($row.source_turnaround_path)"
            }

            if ($row.import_status -ne "reference_only_do_not_import_pending_active_cat_playmode_screenshot") {
                Add-Failure "$cat/$view import status is unsafe: $($row.import_status)"
            }

            if ($row.reference_plate_path -notlike "$batchRelative/*.png") {
                Add-Failure "$cat/$view reference plate is outside the Batch 70 directory: $($row.reference_plate_path)"
            }

            if ($row.reference_plate_size -ne "768x768") {
                Add-Failure "$cat/$view reference plate should be 768x768 but manifest says $($row.reference_plate_size)"
            }

            if ($row.crop_rect -notmatch '^\d+,\d+,\d+,\d+$') {
                Add-Failure "$cat/$view crop rect is malformed: $($row.crop_rect)"
            }

            $sourceFull = Resolve-ProjectPath $row.source_turnaround_path
            $plateFull = Resolve-ProjectPath $row.reference_plate_path
            if (-not (Test-Path -LiteralPath $sourceFull)) {
                Add-Failure "$cat/$view source file is missing: $($row.source_turnaround_path)"
            } else {
                $actualSourceHash = Get-HashText $sourceFull
                if ($actualSourceHash -ne $row.source_turnaround_sha256) {
                    Add-Failure "$cat/$view source hash mismatch. Manifest $($row.source_turnaround_sha256), actual $actualSourceHash"
                }
            }

            if (-not (Test-Path -LiteralPath $plateFull)) {
                Add-Failure "$cat/$view reference plate file is missing: $($row.reference_plate_path)"
            } else {
                $actualPlateHash = Get-HashText $plateFull
                if ($actualPlateHash -ne $row.reference_plate_sha256) {
                    Add-Failure "$cat/$view reference plate hash mismatch. Manifest $($row.reference_plate_sha256), actual $actualPlateHash"
                }

                $actualPlateSize = Get-ImageSizeText $plateFull
                if ($actualPlateSize -ne "768x768") {
                    Add-Failure "$cat/$view reference plate should be 768x768 but is $actualPlateSize"
                }
            }
        }
    }
}

if (Test-Path -LiteralPath $reviewPath) {
    $reviewText = Get-Content -LiteralPath $reviewPath -Raw
    foreach ($token in @(
        "reference-only",
        "Do not import into Unity yet",
        "no image generation",
        "deterministic crop",
        "front",
        "side",
        "back",
        "body proportion",
        "face markings",
        "palette",
        "costume",
        "props",
        "civilization motifs",
        "active-cat Play Mode screenshot"
    )) {
        Test-Contains $reviewText $token $reviewRelative
    }

    foreach ($cat in $expectedCats) {
        Test-Contains $reviewText $expectedLocks[$cat] $reviewRelative
        Test-Contains $reviewText $expectedSources[$cat] $reviewRelative
    }
}

if (Test-Path -LiteralPath $processPath) {
    $processText = Get-Content -LiteralPath $processPath -Raw
    foreach ($token in @(
        "No image generation was performed",
        "No source turnarounds",
        "No source turnarounds, Unity runtime sprites",
        "deterministic crop-and-resize",
        "active-cat Play Mode screenshots",
        "Unity-side validation remains pending"
    )) {
        Test-Contains $processText $token $processRelative
    }
}

if ($failures.Count -gt 0) {
    throw "Batch 70 starter-cat turnaround reference plate validation failed:`n$($failures -join "`n")"
}

Write-Output "Starter-cat turnaround reference plates Batch 70 validation passed for 9 plate(s)."
