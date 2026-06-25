Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_75_chinese_ui_scale_evidence_templates_2026-06-20"
$batchDirRelative = "design/development/asset_candidates/ui/chinese_ui_scale_evidence/$batchSlug"
$manifestRelative = "$batchDirRelative/chinese_ui_scale_batch75_manifest.csv"
$captureMatrixRelative = "$batchDirRelative/chinese_ui_scale_batch75_capture_matrix.csv"
$reviewSheetRelative = "$batchDirRelative/thecat_ui_chinese_scale_batch75_review_sheet.png"
$reviewNoteRelative = "$batchDirRelative/chinese_ui_scale_batch75_candidate_review.md"
$processNoteRelative = "$batchDirRelative/chinese_ui_scale_batch75_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_75_chinese_ui_scale_evidence_templates.md"
$buildScriptRelative = "design/development/tools/build_chinese_ui_scale_evidence_templates.py"

$expected = @{
    ui_scale_capture_matrix_sheet = @{ Use = "validation.capture_matrix"; Size = "1920x1080"; Type = "ui_validation_evidence_template" }
    ui_scale_safe_area_overlay = @{ Use = "validation.safe_area_overlay"; Size = "1920x1080"; Type = "ui_validation_safe_area_overlay" }
    ui_scale_surface_note_card = @{ Use = "validation.surface_note_card"; Size = "1280x720"; Type = "ui_validation_note_card" }
    ui_scale_resolution_strip = @{ Use = "validation.resolution_strip"; Size = "1600x320"; Type = "ui_validation_resolution_strip" }
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

$manifestPath = Resolve-ProjectPath $manifestRelative
if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 75 Chinese UI scale evidence manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 4) {
    Add-Failure "Expected 4 Batch 75 template rows but found $($rows.Count)"
}

foreach ($subjectId in $expected.Keys) {
    $matches = @($rows | Where-Object { $_.subject_id -eq $subjectId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected one Batch 75 row for $subjectId but found $($matches.Count)"
        continue
    }

    $row = $matches[0]
    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$subjectId has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.asset_type -ne $expected[$subjectId].Type) {
        Add-Failure "$subjectId has wrong asset type: $($row.asset_type)"
    }

    if ($row.intended_use -ne $expected[$subjectId].Use) {
        Add-Failure "$subjectId has wrong intended use: $($row.intended_use)"
    }

    if ($row.recommendation -ne "validation_template_only_do_not_import") {
        Add-Failure "$subjectId has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.template_path.StartsWith("Assets/", [System.StringComparison]::OrdinalIgnoreCase)) {
        Add-Failure "$subjectId template path should not be inside Assets: $($row.template_path)"
    }

    if ($row.source_references.IndexOf("P0ChineseUiScaleValidationPlan", [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "$subjectId should cite P0ChineseUiScaleValidationPlan in source references"
    }

    $templatePath = Resolve-ProjectPath $row.template_path
    if (-not (Test-Path -LiteralPath $templatePath)) {
        Add-Failure "$subjectId template is missing: $($row.template_path)"
    } else {
        $size = Get-ImageSize $templatePath
        if ($size -ne $expected[$subjectId].Size) {
            Add-Failure "$subjectId template should be $($expected[$subjectId].Size) but is $size"
        }

        if ($row.template_size -ne $expected[$subjectId].Size) {
            Add-Failure "$subjectId manifest template_size should be $($expected[$subjectId].Size) but is $($row.template_size)"
        }

        Test-Hash $templatePath $row.template_sha256 "$subjectId template"

        $metaPath = "$templatePath.meta"
        if (Test-Path -LiteralPath $metaPath) {
            Add-Failure "$subjectId template must not create a Unity meta file: $metaPath"
        }
    }
}

foreach ($relative in @($captureMatrixRelative, $reviewSheetRelative, $reviewNoteRelative, $processNoteRelative, $agentPromptRelative, $buildScriptRelative)) {
    if (-not (Test-Path -LiteralPath (Resolve-ProjectPath $relative))) {
        Add-Failure "Missing Batch 75 packet file: $relative"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (Test-Path -LiteralPath $reviewSheetPath) {
    $size = Get-ImageSize $reviewSheetPath
    if ($size -ne "1920x1080") {
        Add-Failure "Batch 75 review sheet should be 1920x1080 but is $size"
    }
}

$captureMatrixPath = Resolve-ProjectPath $captureMatrixRelative
if (Test-Path -LiteralPath $captureMatrixPath) {
    $matrixRows = @(Import-Csv -LiteralPath $captureMatrixPath -Encoding UTF8)
    if ($matrixRows.Count -ne 20) {
        Add-Failure "Batch 75 capture matrix should contain 20 surface/resolution rows but found $($matrixRows.Count)"
    }

    foreach ($resolution in @("1024x768", "1280x720", "1600x900", "1920x1080")) {
        if (-not @($matrixRows | Where-Object { $_.resolution -eq $resolution })) {
            Add-Failure "Batch 75 capture matrix missing resolution $resolution"
        }
    }

    foreach ($surface in @("main_menu_character_select", "route_map", "battle_hud", "skill_enemy_hud", "result_pause_settings")) {
        if (-not @($matrixRows | Where-Object { $_.surface_id -eq $surface })) {
            Add-Failure "Batch 75 capture matrix missing surface $surface"
        }
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $text = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in @("Validation template only", "Do not import into ``Assets``", "Non-cat UI validation only", "Unity MCP", "1024x768", "Console")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative missing token: $token"
        }
    }
}

$processNotePath = Resolve-ProjectPath $processNoteRelative
if (Test-Path -LiteralPath $processNotePath) {
    $text = Get-Content -LiteralPath $processNotePath -Raw
    foreach ($token in @($batchSlug, "deterministic Pillow generation", "No Unity ``.meta`` files", "Cat consistency impact: none")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$processNoteRelative missing token: $token"
        }
    }
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (Test-Path -LiteralPath $batchDir) {
    $metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta")
    if ($metaFiles.Count -gt 0) {
        Add-Failure "Batch 75 candidate folder must not contain Unity .meta files."
    }
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Chinese UI scale Batch 75 evidence template validation failed with $($failures.Count) issue(s)."
}

Write-Host "Chinese UI scale Batch 75 evidence template validation passed for $($rows.Count) template(s) and 20 capture rows."
