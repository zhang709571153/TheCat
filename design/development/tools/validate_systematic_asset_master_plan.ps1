$ErrorActionPreference = "Stop"

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchDirRelative = "design/development/asset_candidates/p0_asset_dashboard/batch_66_systematic_asset_master_plan_2026-06-15"
$batchDir = Join-Path $projectRoot $batchDirRelative
$matrixRelative = "$batchDirRelative/p0_asset_batch66_master_gap_matrix.csv"
$blueprintRelative = "$batchDirRelative/p0_asset_batch66_master_blueprint.md"
$processRelative = "$batchDirRelative/p0_asset_batch66_process_note.md"
$promptRelative = "design/development/agent_prompts/p0_asset_batch_67_bedroom_interaction_affordance_candidates.md"

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure([string]$message) {
    $failures.Add($message) | Out-Null
}

function Require-File([string]$relativePath) {
    $path = Join-Path $projectRoot $relativePath
    if (-not (Test-Path -LiteralPath $path)) {
        Add-Failure "Missing file: $relativePath"
    }

    return $path
}

$matrixPath = Require-File $matrixRelative
$blueprintPath = Require-File $blueprintRelative
$processPath = Require-File $processRelative
$promptPath = Require-File $promptRelative

if (Test-Path -LiteralPath $matrixPath) {
    $rows = Import-Csv -LiteralPath $matrixPath
    if ($rows.Count -ne 15) {
        Add-Failure "Expected 15 master gap rows but found $($rows.Count)."
    }

    $requiredLanes = @(
        "starter_cat_body",
        "core_enemy_body",
        "bedroom_interactables",
        "bedroom_interaction_feedback",
        "starter_skill_vfx",
        "runtime_controls",
        "secondary_enemy_warning",
        "route_map_readability",
        "formal_install_decisions"
    )

    foreach ($lane in $requiredLanes) {
        $matches = @($rows | Where-Object { $_.lane_id -eq $lane })
        if ($matches.Count -ne 1) {
            Add-Failure "Expected exactly one matrix row for lane '$lane' but found $($matches.Count)."
        }
    }

    $nextLane = @($rows | Where-Object { $_.lane_id -eq "bedroom_interaction_feedback" })
    if ($nextLane.Count -eq 1) {
        if ($nextLane[0].current_state -ne "candidate_complete_pending_unity_review") {
            Add-Failure "Bedroom interaction feedback lane is not marked as candidate-complete pending Unity review."
        }

        if ($nextLane[0].next_batch_policy -ne "review_only_until_runtime_scale_passes") {
            Add-Failure "Bedroom interaction feedback lane does not preserve review-only Unity-gated policy."
        }

        if ($nextLane[0].current_evidence.IndexOf("Batch 67", [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "Bedroom interaction feedback lane does not mention Batch 67 evidence."
        }
    }

    $catLane = @($rows | Where-Object { $_.lane_id -eq "starter_cat_body" })
    if ($catLane.Count -eq 1 -and $catLane[0].cat_consistency_rule -ne "must_match_locked_colored_three_view_turnarounds") {
        Add-Failure "Starter cat body lane does not enforce locked colored three-view turnaround matching."
    }
}

$texts = @{}
foreach ($relative in @($blueprintRelative, $processRelative, $promptRelative)) {
    $path = Join-Path $projectRoot $relative
    if (Test-Path -LiteralPath $path) {
        $texts[$relative] = Get-Content -LiteralPath $path -Raw
    }
}

if ($texts.ContainsKey($blueprintRelative)) {
    foreach ($token in @("Batch 67", "bedroom interaction affordance", "Unity remains", "colored three-view turnaround", "Do not import into Unity")) {
        if ($texts[$blueprintRelative].IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "Blueprint is missing required token: $token"
        }
    }
}

if ($texts.ContainsKey($processRelative)) {
    foreach ($token in @('No Unity asset', 'No `.meta` file', 'No starter-cat', 'Batch 67')) {
        if ($texts[$processRelative].IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "Process note is missing required token: $token"
        }
    }
}

if ($texts.ContainsKey($promptRelative)) {
    foreach ($token in @('Do not modify:', 'Assets/', 'No `.meta` files', 'bed_ready_ring', 'interaction_range_ripple', 'This prompt does not authorize Unity installation')) {
        if ($texts[$promptRelative].IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "Batch 67 prompt is missing required token: $token"
        }
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 66 should not create Unity .meta files."
}

if ($failures.Count -gt 0) {
    $failures | ForEach-Object { Write-Error $_ }
    throw "Systematic asset master plan validation failed with $($failures.Count) issue(s)."
}

Write-Host "Systematic asset master plan validation passed."
