Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_64_secondary_enemy_warning_candidates_2026-06-15"
$batchDirRelative = "design/development/asset_candidates/enemies/secondary_warning_vfx/$batchSlug"
$manifestRelative = "$batchDirRelative/secondary_enemy_warning_batch64_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchDirRelative/thecat_vfx_secondary_enemy_warnings_batch64_review_sheet.png"
$reviewNoteRelative = "$batchDirRelative/secondary_enemy_warning_batch64_candidate_review.md"
$processNoteRelative = "$batchDirRelative/secondary_enemy_warning_batch64_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_64_secondary_enemy_warning_candidates.md"

$expected = @{
    dream_rail_train_track_warning = "warning.dream_rail_track"
    red_eye_alarm_shockring_warning = "warning.red_eye_alarm_shock_ring"
    unread_red_dot_swarm_warning = "warning.unread_red_dot_swarm_attach"
    falling_dream_teddy_slam_warning = "warning.falling_dream_teddy_slam"
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
    throw "Batch 64 secondary enemy warning manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 4) {
    Add-Failure "Expected 4 Batch 64 candidate rows but found $($rows.Count)"
}

foreach ($subjectId in $expected.Keys) {
    $matches = @($rows | Where-Object { $_.subject_id -eq $subjectId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected one Batch 64 row for $subjectId but found $($matches.Count)"
        continue
    }

    $row = $matches[0]
    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$subjectId has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.asset_type -ne "secondary_enemy_warning_vfx_candidate") {
        Add-Failure "$subjectId should use secondary_enemy_warning_vfx_candidate asset type but found $($row.asset_type)"
    }

    if ($row.intended_runtime_binding -ne $expected[$subjectId]) {
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
        if ($size -ne "256x256") {
            Add-Failure "$subjectId candidate asset should be 256x256 but is $size"
        }

        if ($row.candidate_size -ne "256x256") {
            Add-Failure "$subjectId manifest candidate_size should be 256x256 but is $($row.candidate_size)"
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
        Add-Failure "Missing Batch 64 packet file: $relative"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (Test-Path -LiteralPath $reviewSheetPath) {
    $size = Get-ImageSize $reviewSheetPath
    if ($size -ne "1560x860") {
        Add-Failure "Batch 64 review sheet should be 1560x860 but is $size"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $text = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in @("Candidate pack complete pending Unity review", "Non-cat warning VFX only", "Batch 10 remains the installed P0 warning VFX baseline", "Do not import into ``Assets``")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative missing token: $token"
        }
    }
}

$queueCatalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetProductionQueueCatalog.cs"
$queueCatalogText = Get-Content -LiteralPath $queueCatalogPath -Raw
foreach ($token in @("SecondaryEnemyWarningCandidateQueueId", "Secondary Enemy Warning Candidate Pack", "batch_64_secondary_enemy_warning_candidates_2026-06-15", "secondary_enemy_warning_batch64_manifest.csv")) {
    if ($queueCatalogText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "P0AssetProductionQueueCatalog missing token: $token"
    }
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Secondary enemy warning Batch 64 candidate validation failed with $($failures.Count) issue(s)."
}

Write-Host "Secondary enemy warning Batch 64 candidate validation passed for $($rows.Count) asset(s)."
