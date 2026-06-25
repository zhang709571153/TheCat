Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_63_runtime_control_panel_candidates_2026-06-15"
$batchDirRelative = "design/development/asset_candidates/ui/runtime_controls/$batchSlug"
$manifestRelative = "$batchDirRelative/runtime_control_panels_batch63_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchDirRelative/thecat_ui_runtime_control_panels_batch63_review_sheet.png"
$reviewNoteRelative = "$batchDirRelative/runtime_control_panels_batch63_candidate_review.md"
$processNoteRelative = "$batchDirRelative/runtime_control_panels_batch63_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_63_runtime_control_panel_candidates.md"

$expected = @{
    runtime_pause_overlay_panel = @{ Binding = "runtime_control.pause_overlay_panel"; Size = "768x432" }
    runtime_speed_segmented_control = @{ Binding = "runtime_control.speed_segmented_control"; Size = "512x128" }
    runtime_restart_confirm_plate = @{ Binding = "runtime_control.restart_confirm_plate"; Size = "512x256" }
    runtime_keyboard_hint_strip = @{ Binding = "runtime_control.keyboard_hint_strip"; Size = "768x128" }
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

function Get-ImageSize {
    param([string]$Path)
    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        return "$($image.Width)x$($image.Height)"
    } finally {
        $image.Dispose()
    }
}

function Test-TransparentCorners {
    param([string]$Path)
    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $points = @(
            @(0, 0),
            @(($bitmap.Width - 1), 0),
            @(0, ($bitmap.Height - 1)),
            @(($bitmap.Width - 1), ($bitmap.Height - 1))
        )

        foreach ($point in $points) {
            $pixel = $bitmap.GetPixel($point[0], $point[1])
            if ($pixel.A -gt 8) {
                Add-Failure "$Path corner pixel $($point[0]),$($point[1]) should be transparent but alpha is $($pixel.A)"
            }
        }
    } finally {
        $bitmap.Dispose()
    }
}

function Test-Hash {
    param(
        [string]$Path,
        [string]$ExpectedHash,
        [string]$Label
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        Add-Failure "$Label missing at $Path"
        return
    }

    $actual = (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
    if ($actual -ne $ExpectedHash.ToLowerInvariant()) {
        Add-Failure "$Label hash mismatch. Expected $ExpectedHash but found $actual"
    }
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 63 runtime control panel manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 4) {
    Add-Failure "Expected 4 Batch 63 candidate rows but found $($rows.Count)"
}

foreach ($subjectId in $expected.Keys) {
    $matches = @($rows | Where-Object { $_.subject_id -eq $subjectId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected one Batch 63 row for $subjectId but found $($matches.Count)"
        continue
    }

    $row = $matches[0]
    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$subjectId has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.asset_type -ne "runtime_control_panel_candidate") {
        Add-Failure "$subjectId should use runtime_control_panel_candidate asset type but found $($row.asset_type)"
    }

    if ($row.intended_runtime_binding -ne $expected[$subjectId].Binding) {
        Add-Failure "$subjectId has wrong intended runtime binding: $($row.intended_runtime_binding)"
    }

    if ($row.recommendation -ne "candidate_review_only_do_not_import") {
        Add-Failure "$subjectId has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.candidate_path.StartsWith("Assets/", [System.StringComparison]::OrdinalIgnoreCase)) {
        Add-Failure "$subjectId candidate path should not be inside Assets: $($row.candidate_path)"
    }

    $assetPath = Resolve-ProjectPath $row.candidate_path
    if (-not (Test-Path -LiteralPath $assetPath)) {
        Add-Failure "$subjectId candidate asset is missing: $($row.candidate_path)"
    } else {
        $size = Get-ImageSize $assetPath
        if ($size -ne $expected[$subjectId].Size) {
            Add-Failure "$subjectId candidate asset should be $($expected[$subjectId].Size) but is $size"
        }

        if ($row.candidate_size -ne $expected[$subjectId].Size) {
            Add-Failure "$subjectId manifest candidate_size should be $($expected[$subjectId].Size) but is $($row.candidate_size)"
        }

        Test-TransparentCorners $assetPath
        Test-Hash $assetPath $row.candidate_sha256 "$subjectId candidate asset"

        $metaPath = "$assetPath.meta"
        if (Test-Path -LiteralPath $metaPath) {
            Add-Failure "$subjectId candidate must not create a Unity meta file: $metaPath"
        }
    }
}

foreach ($relative in @($reviewSheetRelative, $reviewNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    if (-not (Test-Path -LiteralPath (Resolve-ProjectPath $relative))) {
        Add-Failure "Missing Batch 63 packet file: $relative"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (Test-Path -LiteralPath $reviewSheetPath) {
    $size = Get-ImageSize $reviewSheetPath
    if ($size -ne "1920x1280") {
        Add-Failure "Batch 63 review sheet should be 1920x1280 but is $size"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $text = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in @("Candidate pack complete pending Unity review", "Non-cat UI only", "Do not import into ``Assets``", "Pause overlay", "Speed segmented control", "Keyboard hint strip")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative missing token: $token"
        }
    }
}

$queueCatalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs"
$queueCatalogText = Get-Content -LiteralPath $queueCatalogPath -Raw
foreach ($token in @("RuntimeControlPanelCandidateQueueId", "Runtime Control Panel Candidate Pack", "batch_63_runtime_control_panel_candidates_2026-06-15", "runtime_control_panels_batch63_manifest.csv")) {
    if ($queueCatalogText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "P0AssetProductionQueueCatalog missing token: $token"
    }
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Runtime control panel Batch 63 candidate validation failed with $($failures.Count) issue(s)."
}

Write-Host "Runtime control panel Batch 63 candidate validation passed for $($rows.Count) asset(s)."
