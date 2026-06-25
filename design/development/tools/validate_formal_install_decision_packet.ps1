Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_56_formal_install_decision_packet_2026-06-15"
$batchDirRelative = "design/development/asset_candidates/formal_install_decisions/$batchSlug"
$decisionCsvRelative = "$batchDirRelative/formal_install_decision_batch56.csv"
$decisionNoteRelative = "$batchDirRelative/formal_install_decision_batch56_review.md"
$processNoteRelative = "$batchDirRelative/formal_install_decision_batch56_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_56_formal_install_decision_packet.md"

$expectedGroups = @(
    "starter_cat_saiban",
    "starter_cat_nephthys",
    "starter_cat_suzune",
    "enemy_black_mud_nightmare",
    "enemy_cold_light_shadow",
    "enemy_call_tyrant",
    "bedroom_interactables",
    "starter_skill_vfx"
)
$requiredNoteTokens = @(
    "blocked pending Unity evidence",
    "No candidate is approved for install",
    "Batch 54",
    "Batch 55",
    "Unity MCP",
    'No files were copied into `Assets`',
    'No Unity `.meta` files were created'
)
$requiredPromptTokens = @(
    "Batch 56",
    "formal Unity install decision packet",
    "Batch 54",
    "Batch 55",
    "Do not install anything",
    "Unity MCP"
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

$decisionCsvPath = Resolve-ProjectPath $decisionCsvRelative
if (-not (Test-Path -LiteralPath $decisionCsvPath)) {
    throw "Batch 56 decision CSV is missing: $decisionCsvRelative"
}

$rows = @(Import-Csv -LiteralPath $decisionCsvPath -Encoding UTF8)
if ($rows.Count -ne 8) {
    Add-Failure "Expected 8 Batch 56 decision rows but found $($rows.Count)"
}

foreach ($group in $expectedGroups) {
    $matches = @($rows | Where-Object { $_.subject_group -eq $group })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected exactly one decision row for $group but found $($matches.Count)"
    }
}

foreach ($row in $rows) {
    if ($row.decision -ne "blocked_pending_unity_evidence") {
        Add-Failure "$($row.subject_group) decision must stay blocked until Unity evidence exists"
    }

    if ($row.install_allowed -ne "false") {
        Add-Failure "$($row.subject_group) install_allowed must be false"
    }

    if ($row.unity_import_root -notlike "Assets/TheCat/Art*") {
        Add-Failure "$($row.subject_group) has unexpected Unity import root: $($row.unity_import_root)"
    }

    if ($row.required_unity_evidence.IndexOf("Unity Console", [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        Add-Failure "$($row.subject_group) required evidence must mention Unity Console"
    }

    if ($row.blocking_reason.IndexOf("missing", [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        Add-Failure "$($row.subject_group) blocker should explicitly mention missing evidence"
    }

    Test-Hash (Resolve-ProjectPath $row.candidate_manifest_path) $row.candidate_manifest_sha256 "$($row.subject_group) manifest"
    Test-Hash (Resolve-ProjectPath $row.candidate_review_path) $row.candidate_review_sha256 "$($row.subject_group) review"
    Test-Hash (Resolve-ProjectPath $row.candidate_process_path) $row.candidate_process_sha256 "$($row.subject_group) process"
}

foreach ($textRelative in @($decisionNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    $textPath = Resolve-ProjectPath $textRelative
    if (-not (Test-Path -LiteralPath $textPath)) {
        Add-Failure "Required Batch 56 text file is missing: $textRelative"
    }
}

$decisionNotePath = Resolve-ProjectPath $decisionNoteRelative
if (Test-Path -LiteralPath $decisionNotePath) {
    $content = Get-Content -LiteralPath $decisionNotePath -Raw -Encoding UTF8
    foreach ($token in $requiredNoteTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$decisionNoteRelative is missing note token: $token"
        }
    }
}

$promptPath = Resolve-ProjectPath $agentPromptRelative
if (Test-Path -LiteralPath $promptPath) {
    $content = Get-Content -LiteralPath $promptPath -Raw -Encoding UTF8
    foreach ($token in $requiredPromptTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$agentPromptRelative is missing prompt token: $token"
        }
    }
}

$metaFiles = @(Get-ChildItem -Path (Resolve-ProjectPath $batchDirRelative) -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 56 decision packet must not create Unity .meta files: $($metaFiles.FullName -join ', ')"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Formal install decision Batch 56 validation failed with $($failures.Count) issue(s)."
}

Write-Host "Formal install decision Batch 56 validation passed: $($rows.Count) blocked rows, no install approval, and no Unity asset writes."
