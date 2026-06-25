Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_48_saiban_ai_generation_pilot_2026-06-15"
$catDirRelative = "design/development/asset_candidates/starter_cats/saiban/$batchSlug"
$batchRootRelative = "design/development/asset_candidates/starter_cats/$batchSlug"
$manifestRelative = "$batchRootRelative/saiban_batch48_ai_generation_pilot_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$catDirRelative/thecat_cat_saiban_batch48_ai_generation_pilot_review_sheet.png"
$reviewNoteRelative = "$catDirRelative/saiban_batch48_ai_generation_pilot_review.md"
$processNoteRelative = "$catDirRelative/saiban_batch48_ai_generation_pilot_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_48_saiban_ai_generation_pilot.md"

$expectedAssetTypes = @(
    "chromakey_source_1024",
    "alpha_candidate_1024",
    "alpha_preview_512",
    "checkerboard_review_512",
    "darkfield_review_512",
    "warmfield_review_512",
    "alpha_mask_review_512"
)

$requiredReviewTokens = @(
    "candidate review only",
    "do not import into Unity",
    "Batch 47 strict starter-cat generation specs",
    "locked colored turnaround reference",
    "helmet and armor are more ornate",
    "active-cat Play Mode screenshot",
    "No Unity"
)

$requiredPromptTokens = @(
    "Batch 48",
    "Saiban",
    "Batch 47",
    "three-view",
    "built-in image_gen",
    "chroma-key",
    "do not import into Unity",
    "active-cat screenshot"
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
    throw "Batch 48 Saiban AI generation pilot manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 7) {
    Add-Failure "Expected 7 Saiban Batch 48 rows but found $($rows.Count)"
}

foreach ($assetType in $expectedAssetTypes) {
    $matches = @($rows | Where-Object { $_.asset_type -eq $assetType })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected exactly one row for $assetType but found $($matches.Count)"
    }
}

foreach ($row in $rows) {
    if ($row.cat_id -ne "saiban") {
        Add-Failure "Unexpected cat id: $($row.cat_id)"
    }

    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$($row.asset_type) has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.source_lock_id -ne "saiban_turnaround_colored") {
        Add-Failure "$($row.asset_type) has wrong source lock: $($row.source_lock_id)"
    }

    if ($row.active_screenshot -ne "04-active-cat-saiban.png") {
        Add-Failure "$($row.asset_type) active screenshot should be 04-active-cat-saiban.png but found $($row.active_screenshot)"
    }

    if ($row.recommendation -ne "candidate_review_only_do_not_import") {
        Add-Failure "$($row.asset_type) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.candidate_path -notlike "$catDirRelative/*") {
        Add-Failure "$($row.asset_type) candidate path must stay in the Batch 48 Saiban directory"
    }

    if ($row.source_turnaround_path -match "\?assets") {
        Add-Failure "$($row.asset_type) source path appears mojibake-corrupted: $($row.source_turnaround_path)"
    }

    if ($row.batch47_json_path -notlike "design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/*.json") {
        Add-Failure "$($row.asset_type) must reference the Saiban Batch 47 JSON spec"
    }

    if ($row.batch47_prompt_path -notlike "design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/*.md") {
        Add-Failure "$($row.asset_type) must reference the Saiban Batch 47 prompt"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    Test-Hash $candidatePath $row.candidate_sha256 "$($row.asset_type) candidate"
    if (Test-Path -LiteralPath $candidatePath) {
        $size = Get-ImageSize $candidatePath
        if ($size -ne $row.candidate_size) {
            Add-Failure "$($row.asset_type) size should be $($row.candidate_size) but is $size"
        }

        if (Test-Path -LiteralPath "$candidatePath.meta") {
            Add-Failure "$($row.asset_type) candidate must not have a Unity .meta file"
        }
    }

    Test-Hash (Resolve-ProjectPath $row.source_turnaround_path) $row.source_turnaround_sha256 "$($row.asset_type) source turnaround"
    Test-Hash (Resolve-ProjectPath $row.batch47_json_path) $row.batch47_json_sha256 "$($row.asset_type) Batch 47 JSON"
    Test-Hash (Resolve-ProjectPath $row.batch47_prompt_path) $row.batch47_prompt_sha256 "$($row.asset_type) Batch 47 prompt"
    Test-Hash (Resolve-ProjectPath $row.batch47_card_path) $row.batch47_card_sha256 "$($row.asset_type) Batch 47 card"
    Test-Hash (Resolve-ProjectPath $row.unity_sprite_path) $row.unity_sprite_sha256 "$($row.asset_type) current Unity sprite"
    Test-Hash (Resolve-ProjectPath $row.generated_source_path) $row.generated_source_sha256 "$($row.asset_type) generated source"
    Test-Hash (Resolve-ProjectPath $row.alpha_candidate_path) $row.alpha_candidate_sha256 "$($row.asset_type) alpha candidate"

    if ($row.review_sheet -ne $reviewSheetRelative) {
        Add-Failure "$($row.asset_type) review sheet should be $reviewSheetRelative but found $($row.review_sheet)"
    }

    if ($row.review_note -ne $reviewNoteRelative) {
        Add-Failure "$($row.asset_type) review note should be $reviewNoteRelative but found $($row.review_note)"
    }

    if ($row.process_note -ne $processNoteRelative) {
        Add-Failure "$($row.asset_type) process note should be $processNoteRelative but found $($row.process_note)"
    }

    if ($row.agent_prompt -ne $agentPromptRelative) {
        Add-Failure "$($row.asset_type) agent prompt should be $agentPromptRelative but found $($row.agent_prompt)"
    }
}

$alphaRow = @($rows | Where-Object { $_.asset_type -eq "alpha_candidate_1024" })[0]
if ($null -ne $alphaRow) {
    Test-TransparentCorners (Resolve-ProjectPath $alphaRow.candidate_path)
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (-not (Test-Path -LiteralPath $reviewSheetPath)) {
    Add-Failure "Batch 48 review sheet is missing: $reviewSheetRelative"
} else {
    $sheetSize = Get-ImageSize $reviewSheetPath
    if ($sheetSize -ne "2000x1300") {
        Add-Failure "Batch 48 review sheet should be 2000x1300 but is $sheetSize"
    }

    if (Test-Path -LiteralPath "$reviewSheetPath.meta") {
        Add-Failure "Batch 48 review sheet must not have a Unity .meta file"
    }
}

foreach ($textRelative in @($reviewNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    $textPath = Resolve-ProjectPath $textRelative
    if (-not (Test-Path -LiteralPath $textPath)) {
        Add-Failure "Required Batch 48 text file is missing: $textRelative"
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

$metaFiles = @(Get-ChildItem -Path (Resolve-ProjectPath $catDirRelative) -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 48 must not create Unity .meta files: $($metaFiles.FullName -join ', ')"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Saiban Batch 48 AI generation pilot validation failed with $($failures.Count) issue(s)."
}

Write-Host "Saiban Batch 48 AI generation pilot validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Review sheet: 1"
Write-Host "Review note: 1"
Write-Host "Process note: 1"
Write-Host "Agent prompt: 1"
Write-Host "Formal Unity import remains blocked pending active-cat Play Mode screenshot review."
