Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_75_chinese_ui_scale_evidence_templates_2026-06-20"
$batchDirRelative = "design/development/asset_candidates/ui/chinese_ui_scale_evidence/$batchSlug"
$mockupDirRelative = "$batchDirRelative/local_scale_mockups_v001"
$mockupPngDirRelative = "$mockupDirRelative/mockups"
$manifestRelative = "$mockupDirRelative/chinese_ui_scale_batch75_local_mockup_manifest.csv"
$contactSheetRelative = "$mockupDirRelative/thecat_ui_chinese_scale_batch75_local_mockup_contact_sheet.png"
$reviewNoteRelative = "$mockupDirRelative/chinese_ui_scale_batch75_local_mockup_review.md"

$surfaces = @(
    "main_menu_character_select",
    "route_map",
    "battle_hud",
    "skill_enemy_hud",
    "result_pause_settings"
)
$resolutions = @("1024x768", "1280x720", "1600x900", "1920x1080")
$resolutionIds = @("compact_4_3", "baseline_16_9", "desktop_16_9", "wide_1080p")

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function Resolve-ProjectPath {
    param([string]$RelativePath)
    return Join-Path $projectRoot ($RelativePath -replace "/", [System.IO.Path]::DirectorySeparatorChar)
}

function Get-ImageSize {
    param([string]$Path)
    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        return "$($image.Width)x$($image.Height)"
    }
    finally {
        $image.Dispose()
    }
}

function Test-Hash {
    param(
        [string]$Path,
        [string]$ExpectedHash,
        [string]$Label
    )

    $actual = (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
    if ($actual -ne $ExpectedHash.ToLowerInvariant()) {
        Add-Failure "$Label hash mismatch. Expected $ExpectedHash but found $actual"
    }
}

$mockupDir = Resolve-ProjectPath $mockupDirRelative
$mockupPngDir = Resolve-ProjectPath $mockupPngDirRelative
$manifestPath = Resolve-ProjectPath $manifestRelative
$contactSheetPath = Resolve-ProjectPath $contactSheetRelative
$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative

foreach ($path in @($mockupDir, $mockupPngDir, $manifestPath, $contactSheetPath, $reviewNotePath)) {
    if (-not (Test-Path -LiteralPath $path)) {
        Add-Failure "Missing Batch 75 local mockup evidence file or directory: $path"
    }
}

if ($failures.Count -eq 0) {
    $mockupRootResolved = (Resolve-Path -LiteralPath $mockupDir).Path
    $mockupRootPrefix = $mockupRootResolved.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar

    $rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
    if ($rows.Count -ne 20) {
        Add-Failure "Expected 20 Batch 75 local mockup rows, found $($rows.Count)"
    }

    $seen = @{}
    $manifestFileNames = @{}
    foreach ($row in $rows) {
        if ($surfaces -notcontains $row.surface_id) {
            Add-Failure "Unexpected Batch 75 local mockup surface: $($row.surface_id)"
        }
        if ($resolutionIds -notcontains $row.resolution_id) {
            Add-Failure "Unexpected Batch 75 local mockup resolution_id: $($row.resolution_id)"
        }
        if ($resolutions -notcontains $row.resolution) {
            Add-Failure "Unexpected Batch 75 local mockup resolution: $($row.resolution)"
        }
        if ($row.status -ne "local_preflight_not_unity_screenshot") {
            Add-Failure "Unexpected Batch 75 local mockup status: $($row.status)"
        }
        if ($row.notes -notlike "*non_cat_ui_scale_mockup*") {
            Add-Failure "Batch 75 local mockup notes should identify non-cat UI scale mockup: $($row.notes)"
        }
        if ($row.path -like "Assets/*" -or $row.path -like "Assets\*") {
            Add-Failure "Batch 75 local mockup path must not point into Assets: $($row.path)"
            continue
        }
        $absolute = Resolve-ProjectPath $row.path
        if (-not (Test-Path -LiteralPath $absolute)) {
            Add-Failure "Batch 75 local mockup PNG missing: $($row.path)"
            continue
        }
        $resolved = (Resolve-Path -LiteralPath $absolute).Path
        if (-not ($resolved.Equals($mockupRootResolved, [System.StringComparison]::OrdinalIgnoreCase) -or $resolved.StartsWith($mockupRootPrefix, [System.StringComparison]::OrdinalIgnoreCase))) {
            Add-Failure "Batch 75 local mockup path must stay under exact local_scale_mockups_v001 root: $($row.path)"
        }
        if (-not $resolved.StartsWith((Resolve-Path -LiteralPath $mockupPngDir).Path.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
            Add-Failure "Batch 75 manifest PNG path must stay under mockups/ directory: $($row.path)"
        }
        $manifestFileNames[[System.IO.Path]::GetFileName($resolved)] = $true
        $size = Get-ImageSize $absolute
        if ($size -ne $row.resolution) {
            Add-Failure "$($row.path) should be $($row.resolution) but is $size"
        }
        Test-Hash $absolute $row.sha256 $row.path

        $key = "$($row.surface_id)|$($row.resolution)"
        if ($seen.ContainsKey($key)) {
            Add-Failure "Duplicate Batch 75 local mockup row: $key"
        }
        $seen[$key] = $true
    }

    foreach ($surface in $surfaces) {
        foreach ($resolution in $resolutions) {
            $key = "$surface|$resolution"
            if (-not $seen.ContainsKey($key)) {
                Add-Failure "Missing Batch 75 local mockup row: $key"
            }
        }
    }

    $pngFiles = @(Get-ChildItem -LiteralPath $mockupPngDir -Filter "*.png")
    if ($pngFiles.Count -ne 20) {
        Add-Failure "Expected 20 Batch 75 local mockup PNGs, found $($pngFiles.Count)"
    }
    foreach ($file in $pngFiles) {
        if (-not $manifestFileNames.ContainsKey($file.Name)) {
            Add-Failure "Mockup PNG exists but is not listed in manifest: $($file.Name)"
        }
    }
    foreach ($fileName in $manifestFileNames.Keys) {
        if (-not (Test-Path -LiteralPath (Join-Path $mockupPngDir $fileName))) {
            Add-Failure "Manifest references PNG missing from mockups/ directory: $fileName"
        }
    }

    if (Test-Path -LiteralPath $contactSheetPath) {
        $contactSize = Get-ImageSize $contactSheetPath
        if ($contactSize -ne "1920x1600") {
            Add-Failure "Batch 75 local mockup contact sheet should be 1920x1600 but is $contactSize"
        }
    }

    if (Test-Path -LiteralPath $reviewNotePath) {
        $text = Get-Content -LiteralPath $reviewNotePath -Raw
        foreach ($token in @("local_preflight_not_unity_screenshot", "20 mockups", "do not replace Unity screenshots", "cooldown ``99``", "Console")) {
            if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
                Add-Failure "$reviewNoteRelative missing token: $token"
            }
        }
    }

    $metaFiles = @(Get-ChildItem -LiteralPath $mockupDir -Recurse -Filter "*.meta")
    if ($metaFiles.Count -ne 0) {
        Add-Failure "Batch 75 local mockup evidence must not contain Unity .meta files. Found $($metaFiles.Count)"
    }
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }
    throw "Batch 75 local Chinese UI scale mockup validation failed with $($failures.Count) issue(s)."
}

Write-Host "Batch 75 local Chinese UI scale mockup validation passed for 20 mockups."
