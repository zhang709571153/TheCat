Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_67_bedroom_interaction_affordance_candidates_2026-06-15"
$batchDirRelative = "design/development/asset_candidates/ui/bedroom_interaction_affordances/$batchSlug"
$batchDir = Join-Path $projectRoot ($batchDirRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$manifestRelative = "$batchDirRelative/bedroom_interaction_affordance_batch67_manifest.csv"
$manifestPath = Join-Path $projectRoot ($manifestRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$reviewSheetRelative = "$batchDirRelative/thecat_ui_bedroom_interaction_affordance_batch67_review_sheet.png"
$reviewNoteRelative = "$batchDirRelative/bedroom_interaction_affordance_batch67_candidate_review.md"
$processNoteRelative = "$batchDirRelative/bedroom_interaction_affordance_batch67_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_67_bedroom_interaction_affordance_candidates.md"

$expected = @{
    interaction_bed_ready_ring = @{ Binding = "interaction.bed.ready_ring"; Size = "256x256" }
    interaction_bed_restore_pulse = @{ Binding = "interaction.bed.restore_pulse"; Size = "256x256" }
    interaction_litter_urgent_marker = @{ Binding = "interaction.litter.urgent_marker"; Size = "256x256" }
    interaction_feeder_ready_marker = @{ Binding = "interaction.feeder.ready_marker"; Size = "256x256" }
    interaction_blocked_marker = @{ Binding = "interaction.blocked_marker"; Size = "256x256" }
    interaction_range_ripple = @{ Binding = "interaction.range_ripple"; Size = "512x512" }
}

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message) | Out-Null
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
    throw "Batch 67 bedroom interaction affordance manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 6) {
    Add-Failure "Expected 6 Batch 67 candidate rows but found $($rows.Count)"
}

foreach ($subjectId in $expected.Keys) {
    $matches = @($rows | Where-Object { $_.subject_id -eq $subjectId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected one Batch 67 row for $subjectId but found $($matches.Count)"
        continue
    }

    $row = $matches[0]
    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$subjectId has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.asset_type -ne "bedroom_interaction_affordance_candidate") {
        Add-Failure "$subjectId should use bedroom_interaction_affordance_candidate asset type but found $($row.asset_type)"
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
        Add-Failure "Missing Batch 67 packet file: $relative"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (Test-Path -LiteralPath $reviewSheetPath) {
    $size = Get-ImageSize $reviewSheetPath
    if ($size -ne "1920x1080") {
        Add-Failure "Batch 67 review sheet should be 1920x1080 but is $size"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $text = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in @("Candidate pack complete pending Unity review", "Non-cat UI/VFX only", "Do not import into ``Assets``", "colored three-view turnaround")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative missing token: $token"
        }
    }
}

$processNotePath = Resolve-ProjectPath $processNoteRelative
if (Test-Path -LiteralPath $processNotePath) {
    $text = Get-Content -LiteralPath $processNotePath -Raw
    foreach ($token in @("No Unity ``.meta`` files are created", "No manifest/runtime binding baseline changes", "No cat body", "Formal install remains blocked")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$processNoteRelative missing token: $token"
        }
    }
}

$promptPath = Resolve-ProjectPath $agentPromptRelative
if (Test-Path -LiteralPath $promptPath) {
    $text = Get-Content -LiteralPath $promptPath -Raw
    foreach ($token in @("bed_ready_ring", "interaction_range_ripple", "Do not modify:", "Assets/", "This prompt does not authorize Unity installation")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$agentPromptRelative missing token: $token"
        }
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 67 should not create Unity .meta files."
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Bedroom interaction affordance Batch 67 candidate validation failed with $($failures.Count) issue(s)."
}

Write-Host "Bedroom interaction affordance Batch 67 candidate validation passed for $($rows.Count) asset(s)."
