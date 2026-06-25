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
$manifestRelative = "$batchRelative/starter_cat_turnaround_runtime_comparison_batch69_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchRelative/thecat_cat_starter_turnaround_runtime_comparison_batch69_review_sheet.png"
$reviewSheetPath = Join-Path $projectRoot $reviewSheetRelative
$reviewRelative = "$batchRelative/starter_cat_turnaround_runtime_comparison_batch69_review.md"
$reviewPath = Join-Path $projectRoot $reviewRelative
$processRelative = "$batchRelative/starter_cat_turnaround_runtime_comparison_batch69_process_note.md"
$processPath = Join-Path $projectRoot $processRelative

$expectedCats = @("saiban", "nephthys", "suzune")
$expectedSourceLocks = @{
    saiban = "saiban_turnaround_colored"
    nephthys = "nephthys_turnaround_colored"
    suzune = "suzune_turnaround_colored"
}
$expectedSourcePaths = @{
    saiban = "$designAssetRootRelative/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png"
    nephthys = "$designAssetRootRelative/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png"
    suzune = "$designAssetRootRelative/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png"
}
$expectedSpritePaths = @{
    saiban = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png"
    nephthys = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png"
    suzune = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png"
}
$expectedScreenshots = @{
    saiban = "04-active-cat-saiban.png"
    nephthys = "05-active-cat-nephthys.png"
    suzune = "06-active-cat-suzune.png"
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
    throw "Batch 69 audit directory is missing: $batchRelative"
}

foreach ($required in @($manifestPath, $reviewSheetPath, $reviewPath, $processPath)) {
    if (-not (Test-Path -LiteralPath $required)) {
        Add-Failure "Missing Batch 69 audit artifact: $required"
    }
}

if (Test-Path -LiteralPath $reviewSheetPath) {
    $sheetSize = Get-ImageSizeText $reviewSheetPath
    if ($sheetSize -ne "1800x1460") {
        Add-Failure "Review sheet should be 1800x1460 but is $sheetSize"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchPath -Filter *.meta -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 69 audit directory must not contain Unity .meta files: $($metaFiles.Name -join ', ')"
}

if (Test-Path -LiteralPath $manifestPath) {
    $rows = @(Import-Csv -LiteralPath $manifestPath)
    if ($rows.Count -ne 3) {
        Add-Failure "Expected 3 Batch 69 manifest rows but found $($rows.Count)"
    }

    foreach ($cat in $expectedCats) {
        $matches = @($rows | Where-Object { $_.cat_id -eq $cat })
        if ($matches.Count -ne 1) {
            Add-Failure "Expected one Batch 69 row for $cat but found $($matches.Count)"
            continue
        }

        $row = $matches[0]
        if ($row.source_lock_id -ne $expectedSourceLocks[$cat]) {
            Add-Failure "$cat source lock should be $($expectedSourceLocks[$cat]) but found $($row.source_lock_id)"
        }

        if ($row.source_turnaround_path -ne $expectedSourcePaths[$cat]) {
            Add-Failure "$cat source path drifted: $($row.source_turnaround_path)"
        }

        if ($row.current_sprite_path -ne $expectedSpritePaths[$cat]) {
            Add-Failure "$cat current sprite path drifted: $($row.current_sprite_path)"
        }

        if ($row.active_screenshot_target -ne $expectedScreenshots[$cat]) {
            Add-Failure "$cat active screenshot target drifted: $($row.active_screenshot_target)"
        }

        if ($row.review_sheet -ne $reviewSheetRelative) {
            Add-Failure "$cat review sheet should be $reviewSheetRelative but found $($row.review_sheet)"
        }

        if ($row.recommendation -ne "audit_only_no_import_pending_active_cat_playmode_screenshot") {
            Add-Failure "$cat recommendation is unsafe: $($row.recommendation)"
        }

        $sourcePath = Resolve-ProjectPath $row.source_turnaround_path
        $spritePath = Resolve-ProjectPath $row.current_sprite_path
        if (-not (Test-Path -LiteralPath $sourcePath)) {
            Add-Failure "$cat source image is missing: $($row.source_turnaround_path)"
        } else {
            $actualHash = Get-HashText $sourcePath
            if ($actualHash -ne $row.source_turnaround_sha256) {
                Add-Failure "$cat source hash mismatch. Manifest $($row.source_turnaround_sha256), actual $actualHash"
            }
        }

        if (-not (Test-Path -LiteralPath $spritePath)) {
            Add-Failure "$cat current sprite is missing: $($row.current_sprite_path)"
        } else {
            $actualHash = Get-HashText $spritePath
            if ($actualHash -ne $row.current_sprite_sha256) {
                Add-Failure "$cat current sprite hash mismatch. Manifest $($row.current_sprite_sha256), actual $actualHash"
            }
        }
    }
}

if (Test-Path -LiteralPath $reviewPath) {
    $reviewText = Get-Content -LiteralPath $reviewPath -Raw
    foreach ($token in @(
        "audit-only",
        "Do not import into Unity yet",
        "active-cat Play Mode screenshot",
        "colored three-view turnaround",
        "body proportion",
        "face markings",
        "palette",
        "costume",
        "props",
        "civilization motifs"
    )) {
        Test-Contains $reviewText $token $reviewRelative
    }

    foreach ($cat in $expectedCats) {
        Test-Contains $reviewText $expectedSourceLocks[$cat] $reviewRelative
        Test-Contains $reviewText $expectedSourcePaths[$cat] $reviewRelative
        Test-Contains $reviewText $expectedSpritePaths[$cat] $reviewRelative
        Test-Contains $reviewText $expectedScreenshots[$cat] $reviewRelative
    }
}

if (Test-Path -LiteralPath $processPath) {
    $processText = Get-Content -LiteralPath $processPath -Raw
    foreach ($token in @(
        "No image generation was performed",
        "No source turnarounds",
        "No image generation",
        "active-cat Play Mode screenshots",
        "Unity-side validation remains pending"
    )) {
        Test-Contains $processText $token $processRelative
    }
}

if ($failures.Count -gt 0) {
    throw "Batch 69 starter-cat turnaround runtime comparison audit validation failed:`n$($failures -join "`n")"
}

Write-Output "Starter-cat turnaround runtime comparison Batch 69 audit validation passed for 3 cat(s)."
