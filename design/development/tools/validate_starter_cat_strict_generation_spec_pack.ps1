Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_47_starter_cat_strict_generation_spec_pack_2026-06-15"
$batchRootRelative = "design/development/asset_candidates/starter_cats/$batchSlug"
$manifestRelative = "$batchRootRelative/starter_cat_batch47_strict_generation_spec_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchRootRelative/thecat_starter_cat_batch47_strict_generation_spec_review_sheet.png"
$reviewNoteRelative = "$batchRootRelative/starter_cat_batch47_strict_generation_spec_review.md"
$processNoteRelative = "$batchRootRelative/starter_cat_batch47_strict_generation_spec_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_47_starter_cat_strict_generation_spec_pack.md"

$expectedCats = @{
    saiban = "05-active-cat-saiban.png"
    nephthys = "06-active-cat-nephthys.png"
    suzune = "07-active-cat-suzune.png"
}

$expectedLocks = @{
    saiban = "saiban_turnaround_colored"
    nephthys = "nephthys_turnaround_colored"
    suzune = "suzune_turnaround_colored"
}

$requiredReviewTokens = @(
    "strict generation spec only",
    "do not import into Unity",
    "locked colored three-view turnaround",
    "source palette guard",
    "prompt files",
    "JSON generation specs",
    "Formal import remains blocked",
    "active-cat Play Mode screenshot"
)

$requiredPromptTokens = @(
    "Batch 47",
    "Saiban",
    "Nephthys",
    "Suzune",
    "colored three-view turnaround",
    "Codex-side generation",
    "do not import into Unity",
    "JSON spec",
    "active-cat screenshot"
)

$requiredPerPromptTokens = @(
    "Positive Prompt",
    "Negative Prompt",
    "Hard Source Lock",
    "Must Keep",
    "Reject",
    "non-human",
    "Active screenshot gate"
)

$forbiddenMojibakeToken = "?assets"

$requiredJsonFields = @(
    "batch_slug",
    "cat_id",
    "source_lock_id",
    "source_turnaround_path",
    "palette_hex",
    "must_keep",
    "reject",
    "positive_prompt",
    "negative_prompt",
    "recommendation",
    "unity_validation_required"
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
    throw "Batch 47 starter-cat strict generation spec manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 3) {
    Add-Failure "Expected 3 starter-cat spec rows but found $($rows.Count)"
}

foreach ($catId in $expectedCats.Keys) {
    $matches = @($rows | Where-Object { $_.cat_id -eq $catId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected exactly one Batch 47 row for $catId but found $($matches.Count)"
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

    if ($row.asset_type -ne "strict_generation_spec_card_1100x900") {
        Add-Failure "$($row.cat_id) has wrong asset type: $($row.asset_type)"
    }

    if ($row.recommendation -ne "strict_generation_spec_only_do_not_import") {
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
        Add-Failure "$($row.cat_id) spec card path must stay in its Batch 47 cat directory"
    }

    if ($row.spec_json_path -notlike "design/development/asset_candidates/starter_cats/$($row.cat_id)/$batchSlug/*.json") {
        Add-Failure "$($row.cat_id) JSON spec path must stay in its Batch 47 cat directory"
    }

    if ($row.generation_prompt_path -notlike "design/development/asset_candidates/starter_cats/$($row.cat_id)/$batchSlug/*.md") {
        Add-Failure "$($row.cat_id) generation prompt path must stay in its Batch 47 cat directory"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if (-not (Test-Path -LiteralPath $candidatePath)) {
        Add-Failure "$($row.cat_id) spec card is missing: $($row.candidate_path)"
    } else {
        $size = Get-ImageSize $candidatePath
        if ($size -ne "1100x900") {
            Add-Failure "$($row.cat_id) spec card should be 1100x900 but is $size"
        }

        if ($row.candidate_size -ne "1100x900") {
            Add-Failure "$($row.cat_id) manifest candidate_size should be 1100x900 but is $($row.candidate_size)"
        }

        Test-Hash $candidatePath $row.candidate_sha256 "$($row.cat_id) spec card"
        if (Test-Path -LiteralPath "$candidatePath.meta") {
            Add-Failure "$($row.cat_id) spec card must not have a Unity .meta file"
        }
    }

    $jsonPath = Resolve-ProjectPath $row.spec_json_path
    Test-Hash $jsonPath $row.spec_json_sha256 "$($row.cat_id) JSON spec"
    if (Test-Path -LiteralPath $jsonPath) {
        $jsonText = Get-Content -LiteralPath $jsonPath -Raw -Encoding UTF8
        $json = $jsonText | ConvertFrom-Json
        foreach ($field in $requiredJsonFields) {
            if (-not ($json.PSObject.Properties.Name -contains $field)) {
                Add-Failure "$($row.spec_json_path) is missing JSON field: $field"
            }
        }

        if ($json.recommendation -ne "strict_generation_spec_only_do_not_import") {
            Add-Failure "$($row.spec_json_path) has unsafe recommendation: $($json.recommendation)"
        }

        if ($json.positive_prompt.IndexOf("non-human", [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$($row.spec_json_path) positive prompt must include non-human body rule"
        }

        if ($jsonText.IndexOf($forbiddenMojibakeToken, [System.StringComparison]::OrdinalIgnoreCase) -ge 0) {
            Add-Failure "$($row.spec_json_path) must not contain mojibake ?assets paths"
        }
    }

    $promptPath = Resolve-ProjectPath $row.generation_prompt_path
    Test-Hash $promptPath $row.generation_prompt_sha256 "$($row.cat_id) generation prompt"
    if (Test-Path -LiteralPath $promptPath) {
        $promptText = Get-Content -LiteralPath $promptPath -Raw -Encoding UTF8
        foreach ($token in $requiredPerPromptTokens) {
            if ($promptText.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
                Add-Failure "$($row.generation_prompt_path) is missing prompt token: $token"
            }
        }

        if ($promptText.IndexOf($forbiddenMojibakeToken, [System.StringComparison]::OrdinalIgnoreCase) -ge 0) {
            Add-Failure "$($row.generation_prompt_path) must not contain mojibake ?assets paths"
        }
    }

    Test-Hash (Resolve-ProjectPath $row.source_turnaround_path) $row.source_turnaround_sha256 "$($row.cat_id) source turnaround"
    Test-Hash (Resolve-ProjectPath $row.unity_sprite_path) $row.unity_sprite_sha256 "$($row.cat_id) current Unity sprite"
    Test-Hash (Resolve-ProjectPath $row.latest_cutout_preview_path) $row.latest_cutout_preview_sha256 "$($row.cat_id) latest cutout preview"

    $palette = @($row.palette_hex -split ";")
    if ($palette.Count -lt 5) {
        Add-Failure "$($row.cat_id) palette guard should have at least 5 colors but found $($palette.Count)"
    }

    foreach ($color in $palette) {
        if ($color -notmatch "^#[0-9a-fA-F]{6}$") {
            Add-Failure "$($row.cat_id) palette color is not hex: $color"
        }
    }

    $bbox = @($row.visible_bbox_source -split ",")
    if ($bbox.Count -ne 4) {
        Add-Failure "$($row.cat_id) visible bbox should have 4 values but found $($bbox.Count)"
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
if (-not (Test-Path -LiteralPath $reviewSheetPath)) {
    Add-Failure "Batch 47 review sheet is missing: $reviewSheetRelative"
} else {
    $sheetSize = Get-ImageSize $reviewSheetPath
    if ($sheetSize -ne "3600x1120") {
        Add-Failure "Batch 47 review sheet should be 3600x1120 but is $sheetSize"
    }

    if (Test-Path -LiteralPath "$reviewSheetPath.meta") {
        Add-Failure "Batch 47 review sheet must not have a Unity .meta file"
    }
}

foreach ($textRelative in @($reviewNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    $textPath = Resolve-ProjectPath $textRelative
    if (-not (Test-Path -LiteralPath $textPath)) {
        Add-Failure "Required Batch 47 text file is missing: $textRelative"
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

$batchRoot = Resolve-ProjectPath $batchRootRelative
$metaFiles = @(Get-ChildItem -Path $batchRoot -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 47 must not create Unity .meta files: $($metaFiles.FullName -join ', ')"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Starter-cat Batch 47 strict generation spec validation failed with $($failures.Count) issue(s)."
}

Write-Host "Starter-cat Batch 47 strict generation spec validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Review sheet: 1"
Write-Host "Review note: 1"
Write-Host "Process note: 1"
Write-Host "Agent prompt: 1"
Write-Host "Formal Unity import remains blocked pending generated candidate review and active-cat Play Mode screenshot review."
