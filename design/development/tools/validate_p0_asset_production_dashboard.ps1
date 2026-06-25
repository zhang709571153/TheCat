Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_46_p0_asset_production_dashboard_2026-06-15"
$batchRootRelative = "design/development/asset_candidates/p0_asset_dashboard/$batchSlug"
$manifestRelative = "$batchRootRelative/p0_asset_batch46_production_dashboard_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchRootRelative/thecat_p0_asset_batch46_production_dashboard_review_sheet.png"
$reviewNoteRelative = "$batchRootRelative/p0_asset_batch46_production_dashboard_review.md"
$processNoteRelative = "$batchRootRelative/p0_asset_batch46_production_dashboard_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_46_p0_asset_production_dashboard.md"

$expectedSubjects = @{
    "cat:saiban" = "04-active-cat-saiban.png"
    "cat:nephthys" = "05-active-cat-nephthys.png"
    "cat:suzune" = "06-active-cat-suzune.png"
    "enemy:black_mud_nightmare" = "07-active-enemy-black-mud.png"
    "enemy:cold_light_shadow" = "08-active-enemy-cold-light.png"
    "enemy:call_tyrant" = "09-active-enemy-call-tyrant.png"
}

$expectedLocks = @{
    "cat:saiban" = "saiban_turnaround_colored"
    "cat:nephthys" = "nephthys_turnaround_colored"
    "cat:suzune" = "suzune_turnaround_colored"
    "enemy:black_mud_nightmare" = "black_mud_concept;black_mud_animation"
    "enemy:cold_light_shadow" = "cold_light_concept;cold_light_animation"
    "enemy:call_tyrant" = "call_tyrant_concept;call_tyrant_animation"
}

$requiredReviewTokens = @(
    "candidate-only production dashboard",
    "do not import into Unity",
    "Codex can generate",
    "Unity remains the install-time and runtime validation gate",
    "active screenshot",
    "Unity validation pending",
    "Call Tyrant still needs a formal boss combat sprite binding decision"
)

$requiredPromptTokens = @(
    "Batch 46",
    "Codex-side asset production",
    "do not import into Unity",
    "source lock",
    "active screenshot",
    "Saiban",
    "Nephthys",
    "Suzune",
    "Black Mud Nightmare",
    "Cold Light Shadow",
    "Call Tyrant"
)

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function Resolve-ProjectPath {
    param([string]$RelativePath)
    if ([string]::IsNullOrWhiteSpace($RelativePath)) {
        return ""
    }

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
    throw "Batch 46 P0 asset production dashboard manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne 6) {
    Add-Failure "Expected 6 P0 dashboard rows but found $($rows.Count)"
}

foreach ($subjectId in $expectedSubjects.Keys) {
    $matches = @($rows | Where-Object { $_.subject_id -eq $subjectId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected exactly one Batch 46 row for $subjectId but found $($matches.Count)"
    }
}

foreach ($row in $rows) {
    if (-not $expectedSubjects.ContainsKey($row.subject_id)) {
        Add-Failure "Unexpected subject id: $($row.subject_id)"
        continue
    }

    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$($row.subject_id) has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.source_lock_ids -ne $expectedLocks[$row.subject_id]) {
        Add-Failure "$($row.subject_id) source locks should be $($expectedLocks[$row.subject_id]) but found $($row.source_lock_ids)"
    }

    if ($row.active_screenshot -ne $expectedSubjects[$row.subject_id]) {
        Add-Failure "$($row.subject_id) active screenshot should be $($expectedSubjects[$row.subject_id]) but found $($row.active_screenshot)"
    }

    if ($row.recommendation -ne "dashboard_only_unity_validation_pending") {
        Add-Failure "$($row.subject_id) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.source_reference_paths -match "\?assets") {
        Add-Failure "$($row.subject_id) source path appears mojibake-corrupted: $($row.source_reference_paths)"
    }

    if ($row.current_runtime_asset_path -notlike "Assets/TheCat/Art/*") {
        Add-Failure "$($row.subject_id) current runtime asset must stay under Assets/TheCat/Art: $($row.current_runtime_asset_path)"
    }

    if ($row.unity_install_target_path -notlike "Assets/TheCat/Art/*") {
        Add-Failure "$($row.subject_id) install target must stay under Assets/TheCat/Art: $($row.unity_install_target_path)"
    }

    if ($row.latest_candidate_preview_path -notlike "design/development/asset_candidates/*") {
        Add-Failure "$($row.subject_id) latest candidate must stay under design/development/asset_candidates: $($row.latest_candidate_preview_path)"
    }

    if ($row.review_sheet -ne $reviewSheetRelative) {
        Add-Failure "$($row.subject_id) review sheet should be $reviewSheetRelative but found $($row.review_sheet)"
    }

    if ($row.review_note -ne $reviewNoteRelative) {
        Add-Failure "$($row.subject_id) review note should be $reviewNoteRelative but found $($row.review_note)"
    }

    if ($row.process_note -ne $processNoteRelative) {
        Add-Failure "$($row.subject_id) process note should be $processNoteRelative but found $($row.process_note)"
    }

    if ($row.agent_prompt -ne $agentPromptRelative) {
        Add-Failure "$($row.subject_id) agent prompt should be $agentPromptRelative but found $($row.agent_prompt)"
    }

    $sourcePaths = @($row.source_reference_paths -split ";")
    $sourceHashes = @($row.source_reference_sha256s -split ";")
    if ($sourcePaths.Count -ne $sourceHashes.Count) {
        Add-Failure "$($row.subject_id) source path/hash count mismatch"
    } else {
        for ($i = 0; $i -lt $sourcePaths.Count; $i++) {
            Test-Hash (Resolve-ProjectPath $sourcePaths[$i]) $sourceHashes[$i] "$($row.subject_id) source reference $i"
        }
    }

    Test-Hash (Resolve-ProjectPath $row.current_runtime_asset_path) $row.current_runtime_asset_sha256 "$($row.subject_id) current runtime asset"
    Test-Hash (Resolve-ProjectPath $row.latest_candidate_preview_path) $row.latest_candidate_preview_sha256 "$($row.subject_id) latest candidate preview"

    if (-not (Test-Path -LiteralPath (Resolve-ProjectPath $row.latest_candidate_manifest))) {
        Add-Failure "$($row.subject_id) latest candidate manifest is missing: $($row.latest_candidate_manifest)"
    }

    if ($row.unity_validation_gate.IndexOf("screenshot", [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        Add-Failure "$($row.subject_id) Unity validation gate must mention screenshot review"
    }

    if ($row.next_action.IndexOf("Unity", [System.StringComparison]::OrdinalIgnoreCase) -lt 0 -and $row.next_action.IndexOf("screenshot", [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        Add-Failure "$($row.subject_id) next action should mention Unity or screenshot validation"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (-not (Test-Path -LiteralPath $reviewSheetPath)) {
    Add-Failure "Batch 46 review sheet is missing: $reviewSheetRelative"
} else {
    $sheetSize = Get-ImageSize $reviewSheetPath
    if ($sheetSize -ne "3200x1800") {
        Add-Failure "Batch 46 review sheet should be 3200x1800 but is $sheetSize"
    }

    if (Test-Path -LiteralPath "$reviewSheetPath.meta") {
        Add-Failure "Batch 46 review sheet must not have a Unity .meta file"
    }
}

foreach ($textRelative in @($reviewNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    $textPath = Resolve-ProjectPath $textRelative
    if (-not (Test-Path -LiteralPath $textPath)) {
        Add-Failure "Required Batch 46 text file is missing: $textRelative"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $content = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in $requiredReviewTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative is missing review token: $token"
        }
    }
}

$promptPath = Resolve-ProjectPath $agentPromptRelative
if (Test-Path -LiteralPath $promptPath) {
    $content = Get-Content -LiteralPath $promptPath -Raw
    foreach ($token in $requiredPromptTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$agentPromptRelative is missing prompt token: $token"
        }
    }
}

$batchRoot = Resolve-ProjectPath $batchRootRelative
$metaFiles = @(Get-ChildItem -Path $batchRoot -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 46 must not create Unity .meta files: $($metaFiles.FullName -join ', ')"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "P0 asset Batch 46 production dashboard validation failed with $($failures.Count) issue(s)."
}

Write-Host "P0 asset Batch 46 production dashboard validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Review sheet: 1"
Write-Host "Review note: 1"
Write-Host "Process note: 1"
Write-Host "Agent prompt: 1"
Write-Host "Unity install remains blocked pending active screenshot and editor validation."
