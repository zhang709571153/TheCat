Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_57_skill_hud_feedback_candidates_2026-06-15"
$batchDirRelative = "design/development/asset_candidates/ui/skill_hud/$batchSlug"
$manifestRelative = "$batchDirRelative/skill_hud_feedback_batch57_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchDirRelative/thecat_ui_skill_hud_feedback_batch57_review_sheet.png"
$reviewNoteRelative = "$batchDirRelative/skill_hud_feedback_batch57_candidate_review.md"
$processNoteRelative = "$batchDirRelative/skill_hud_feedback_batch57_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_57_skill_hud_feedback_candidates.md"

$expectedSubjects = @(
    "skill_ready_frame",
    "skill_cooldown_overlay",
    "skill_no_target_marker",
    "skill_hunger_cost_chip",
    "auto_target_reticle",
    "interaction_range_ripple"
)

$expectedAssetTypes = @(
    "alpha_candidate_512",
    "checkerboard_review_512",
    "darkfield_review_512",
    "warmfield_review_512",
    "alpha_mask_review_512"
)

$requiredReviewTokens = @(
    "candidate review only",
    "do not import into Unity",
    "non-cat UI/VFX only",
    "HUD-scale readability",
    'No Batch 57 file may be copied into `Assets`'
)

$requiredPromptTokens = @(
    "Batch 57",
    "Skill HUD",
    "non-cat",
    'Do not write into `Assets/TheCat/Art/UI`',
    'Do not create Unity `.meta` files',
    "candidate_review_only_do_not_import"
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

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 57 skill HUD feedback manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 30) {
    Add-Failure "Expected 30 Batch 57 rows but found $($rows.Count)"
}

foreach ($subject in $expectedSubjects) {
    $subjectRows = @($rows | Where-Object { $_.subject_id -eq $subject })
    if ($subjectRows.Count -ne 5) {
        Add-Failure "Expected 5 rows for $subject but found $($subjectRows.Count)"
    }

    foreach ($assetType in $expectedAssetTypes) {
        $matches = @($subjectRows | Where-Object { $_.asset_type -eq $assetType })
        if ($matches.Count -ne 1) {
            Add-Failure "Expected exactly one $assetType row for $subject but found $($matches.Count)"
        }
    }
}

foreach ($row in $rows) {
    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$($row.subject_id) $($row.asset_type) has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.recommendation -ne "candidate_review_only_do_not_import") {
        Add-Failure "$($row.subject_id) $($row.asset_type) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.candidate_path -notlike "$batchDirRelative/*") {
        Add-Failure "$($row.subject_id) $($row.asset_type) candidate path must stay inside Batch 57 directory"
    }

    if ($row.candidate_path -like "Assets/*") {
        Add-Failure "$($row.subject_id) $($row.asset_type) candidate path must not be inside Assets"
    }

    if ($row.reference_asset_paths -like "*Characters*") {
        Add-Failure "$($row.subject_id) $($row.asset_type) must not reference starter-cat body assets"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    Test-Hash $candidatePath $row.candidate_sha256 "$($row.subject_id) $($row.asset_type) candidate"
    if (Test-Path -LiteralPath $candidatePath) {
        $size = Get-ImageSize $candidatePath
        if ($size -ne "512x512") {
            Add-Failure "$($row.subject_id) $($row.asset_type) should be 512x512 but is $size"
        }

        if ($size -ne $row.candidate_size) {
            Add-Failure "$($row.subject_id) $($row.asset_type) size should be $($row.candidate_size) but is $size"
        }

        if (Test-Path -LiteralPath "$candidatePath.meta") {
            Add-Failure "$($row.subject_id) $($row.asset_type) candidate must not have a Unity .meta file"
        }
    }

    Test-Hash (Resolve-ProjectPath $row.generated_alpha_path) $row.generated_alpha_sha256 "$($row.subject_id) generated alpha"

    $referencePaths = @($row.reference_asset_paths -split ";" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
    $referenceHashes = @($row.reference_asset_sha256s -split ";" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
    if ($referencePaths.Count -ne $referenceHashes.Count -or $referencePaths.Count -lt 1) {
        Add-Failure "$($row.subject_id) $($row.asset_type) reference path/hash count is invalid"
    }

    for ($i = 0; $i -lt $referencePaths.Count; $i++) {
        Test-Hash (Resolve-ProjectPath $referencePaths[$i]) $referenceHashes[$i] "$($row.subject_id) reference $i"
    }

    if ($row.review_sheet -ne $reviewSheetRelative) {
        Add-Failure "$($row.subject_id) $($row.asset_type) review sheet should be $reviewSheetRelative but found $($row.review_sheet)"
    }

    if ($row.review_note -ne $reviewNoteRelative) {
        Add-Failure "$($row.subject_id) $($row.asset_type) review note should be $reviewNoteRelative but found $($row.review_note)"
    }

    if ($row.process_note -ne $processNoteRelative) {
        Add-Failure "$($row.subject_id) $($row.asset_type) process note should be $processNoteRelative but found $($row.process_note)"
    }

    if ($row.agent_prompt -ne $agentPromptRelative) {
        Add-Failure "$($row.subject_id) $($row.asset_type) agent prompt should be $agentPromptRelative but found $($row.agent_prompt)"
    }
}

foreach ($subject in $expectedSubjects) {
    $alphaRow = @($rows | Where-Object { $_.subject_id -eq $subject -and $_.asset_type -eq "alpha_candidate_512" })[0]
    if ($null -ne $alphaRow) {
        Test-TransparentCorners (Resolve-ProjectPath $alphaRow.candidate_path)
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (-not (Test-Path -LiteralPath $reviewSheetPath)) {
    Add-Failure "Batch 57 review sheet is missing: $reviewSheetRelative"
} else {
    $sheetSize = Get-ImageSize $reviewSheetPath
    if ($sheetSize -ne "2400x1600") {
        Add-Failure "Batch 57 review sheet should be 2400x1600 but is $sheetSize"
    }

    if (Test-Path -LiteralPath "$reviewSheetPath.meta") {
        Add-Failure "Batch 57 review sheet must not have a Unity .meta file"
    }
}

foreach ($textRelative in @($reviewNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    $textPath = Resolve-ProjectPath $textRelative
    if (-not (Test-Path -LiteralPath $textPath)) {
        Add-Failure "Required Batch 57 text file is missing: $textRelative"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $content = Get-Content -LiteralPath $reviewNotePath -Raw -Encoding UTF8
    foreach ($token in $requiredReviewTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative is missing review token: $token"
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
    Add-Failure "Batch 57 must not create Unity .meta files: $($metaFiles.FullName -join ', ')"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Error $failure
    }

    throw "Batch 57 skill HUD feedback candidate validation failed with $($failures.Count) issue(s)."
}

Write-Host "Batch 57 skill HUD feedback candidate validation passed: $($rows.Count) rows, $($expectedSubjects.Count) subjects, no Unity install."
