Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_45_starter_cat_source_lock_audit_pack_2026-06-15"
$manifestRelative = "design/development/asset_candidates/starter_cats/$batchSlug/starter_cat_batch45_source_lock_audit_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "design/development/asset_candidates/starter_cats/$batchSlug/thecat_starter_cat_batch45_source_lock_audit_review_sheet.png"
$reviewNoteRelative = "design/development/asset_candidates/starter_cats/$batchSlug/starter_cat_batch45_source_lock_audit_review.md"
$processNoteRelative = "design/development/asset_candidates/starter_cats/$batchSlug/starter_cat_batch45_source_lock_audit_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_45_starter_cat_source_lock_audit_pack.md"

$expectedCats = @{
    saiban = "04-active-cat-saiban.png"
    nephthys = "05-active-cat-nephthys.png"
    suzune = "06-active-cat-suzune.png"
}

$expectedLocks = @{
    saiban = "saiban_turnaround_colored"
    nephthys = "nephthys_turnaround_colored"
    suzune = "suzune_turnaround_colored"
}

$requiredReviewTokens = @(
    "source-lock audit only",
    "do not import into Unity yet",
    "strictly match the colored three-view turnaround",
    "colored three-view turnarounds as the hard authority",
    "current Unity combat sprite",
    "latest transparent cutout candidate",
    "Formal import remains blocked",
    "Reject any future starter-cat asset that drifts",
    "Reject any human body proportions",
    "active-cat Play Mode screenshot"
)

$requiredPromptTokens = @(
    "Batch 45",
    "Saiban",
    "Nephthys",
    "Suzune",
    "colored three-view turnaround",
    "do not import into Unity",
    'candidate files stay outside `Assets`',
    "active-cat Play Mode screenshot"
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

    if (-not (Test-Path $Path)) {
        Add-Failure "$Label missing at $Path"
        return
    }

    $actual = (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
    if ($actual -ne $ExpectedHash.ToLowerInvariant()) {
        Add-Failure "$Label hash mismatch. Expected $ExpectedHash but found $actual"
    }
}

if (-not (Test-Path $manifestPath)) {
    throw "Batch 45 starter-cat source-lock audit manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne 3) {
    Add-Failure "Expected 3 starter-cat audit rows but found $($rows.Count)"
}

foreach ($catId in $expectedCats.Keys) {
    $matches = @($rows | Where-Object { $_.cat_id -eq $catId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected exactly one Batch 45 row for $catId but found $($matches.Count)"
    }
}

foreach ($row in $rows) {
    if (-not $expectedCats.ContainsKey($row.cat_id)) {
        Add-Failure "Unexpected cat id: $($row.cat_id)"
        continue
    }

    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$($row.cat_id) has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.asset_type -ne "source_lock_lineage_card_1000x640") {
        Add-Failure "$($row.cat_id) has wrong asset type: $($row.asset_type)"
    }

    if ($row.recommendation -ne "source_lock_audit_only_do_not_import") {
        Add-Failure "$($row.cat_id) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.source_lock_id -ne $expectedLocks[$row.cat_id]) {
        Add-Failure "$($row.cat_id) source lock should be $($expectedLocks[$row.cat_id]) but found $($row.source_lock_id)"
    }

    if ($row.active_screenshot -ne $expectedCats[$row.cat_id]) {
        Add-Failure "$($row.cat_id) active screenshot should be $($expectedCats[$row.cat_id]) but found $($row.active_screenshot)"
    }

    if ($row.source_turnaround_path -match "\?assets") {
        Add-Failure "$($row.cat_id) source path appears mojibake-corrupted: $($row.source_turnaround_path)"
    }

    if ($row.candidate_path -notlike "design/development/asset_candidates/starter_cats/$($row.cat_id)/$batchSlug/*") {
        Add-Failure "$($row.cat_id) candidate path must stay in its Batch 45 cat directory"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if (-not (Test-Path $candidatePath)) {
        Add-Failure "$($row.cat_id) audit card is missing: $($row.candidate_path)"
    } else {
        $size = Get-ImageSize $candidatePath
        if ($size -ne "1000x640") {
            Add-Failure "$($row.cat_id) audit card should be 1000x640 but is $size"
        }

        if ($row.candidate_size -ne "1000x640") {
            Add-Failure "$($row.cat_id) manifest candidate_size should be 1000x640 but is $($row.candidate_size)"
        }

        Test-Hash $candidatePath $row.candidate_sha256 "$($row.cat_id) audit card"
        if (Test-Path "$candidatePath.meta") {
            Add-Failure "$($row.cat_id) audit card must not have a Unity .meta file"
        }
    }

    $sourcePath = Resolve-ProjectPath $row.source_turnaround_path
    Test-Hash $sourcePath $row.source_turnaround_sha256 "$($row.cat_id) source turnaround"

    $spritePath = Resolve-ProjectPath $row.unity_sprite_path
    Test-Hash $spritePath $row.unity_sprite_sha256 "$($row.cat_id) current Unity sprite"

    $cutoutPath = Resolve-ProjectPath $row.latest_cutout_preview_path
    Test-Hash $cutoutPath $row.latest_cutout_preview_sha256 "$($row.cat_id) latest cutout preview"

    if (-not (Test-Path (Resolve-ProjectPath $row.latest_cutout_manifest))) {
        Add-Failure "$($row.cat_id) latest cutout manifest is missing: $($row.latest_cutout_manifest)"
    }

    if ($row.review_sheet -ne $reviewSheetRelative) {
        Add-Failure "$($row.cat_id) review sheet should be $reviewSheetRelative but found $($row.review_sheet)"
    }

    if ($row.review_note -ne $reviewNoteRelative) {
        Add-Failure "$($row.cat_id) review note should be $reviewNoteRelative but found $($row.review_note)"
    }

    if ($row.process_note -ne $processNoteRelative) {
        Add-Failure "$($row.cat_id) process note should be $processNoteRelative but found $($row.process_note)"
    }

    if ($row.agent_prompt -ne $agentPromptRelative) {
        Add-Failure "$($row.cat_id) agent prompt should be $agentPromptRelative but found $($row.agent_prompt)"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (-not (Test-Path $reviewSheetPath)) {
    Add-Failure "Batch 45 review sheet is missing: $reviewSheetRelative"
} else {
    $sheetSize = Get-ImageSize $reviewSheetPath
    if ($sheetSize -ne "3200x900") {
        Add-Failure "Batch 45 review sheet should be 3200x900 but is $sheetSize"
    }

    if (Test-Path "$reviewSheetPath.meta") {
        Add-Failure "Batch 45 review sheet must not have a Unity .meta file"
    }
}

foreach ($textRelative in @($reviewNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    $textPath = Resolve-ProjectPath $textRelative
    if (-not (Test-Path $textPath)) {
        Add-Failure "Required Batch 45 text file is missing: $textRelative"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path $reviewNotePath) {
    $content = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in $requiredReviewTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative is missing review token: $token"
        }
    }
}

$promptPath = Resolve-ProjectPath $agentPromptRelative
if (Test-Path $promptPath) {
    $content = Get-Content -LiteralPath $promptPath -Raw
    foreach ($token in $requiredPromptTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$agentPromptRelative is missing prompt token: $token"
        }
    }
}

$batchRoot = Resolve-ProjectPath "design/development/asset_candidates/starter_cats/$batchSlug"
$metaFiles = @(Get-ChildItem -Path $batchRoot -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 45 must not create Unity .meta files: $($metaFiles.FullName -join ', ')"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Starter-cat Batch 45 source-lock audit validation failed with $($failures.Count) issue(s)."
}

Write-Host "Starter-cat Batch 45 source-lock audit validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Review sheet: 1"
Write-Host "Review note: 1"
Write-Host "Process note: 1"
Write-Host "Agent prompt: 1"
Write-Host "Formal Unity import remains blocked pending active-cat Play Mode screenshot review."
