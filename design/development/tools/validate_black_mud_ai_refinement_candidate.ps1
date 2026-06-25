Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_39_black_mud_ai_refinement_candidate_2026-06-15"
$candidateRoot = "design/development/asset_candidates/enemies/black_mud_nightmare/$batchSlug"
$manifestRelative = "design/development/asset_candidates/enemies/$batchSlug/black_mud_batch39_ai_refinement_manifest.csv"
$manifestPath = Join-Path $projectRoot ($manifestRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)

$expectedTypes = @{
    ai_refinement_raw_codex = $null
    ai_refinement_combat_1024 = "1024x1024"
    ai_refinement_combat_512_preview = "512x512"
}

$requiredReviewTokens = @(
    "candidate review only",
    "do not import into Unity yet",
    "Formal Unity import remains blocked",
    "Black Mud Nightmare",
    "source concept",
    "source animation",
    "Batch 38 source reference",
    "black sludge body",
    "red eyes",
    "soft-mud monster silhouette",
    "crawling pressure",
    "bed-contact threat",
    "Rejection Rules",
    "07-active-enemy-black-mud.png"
)

$requiredPromptTokens = @(
    "low crawling black-violet sludge mass",
    "glossy soft mud body",
    "squat dome silhouette",
    "two bright hostile red eyes",
    "sleepy face imprint",
    "bed-contact threat",
    "Avoid"
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
    throw "Black Mud Batch 39 AI refinement manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne $expectedTypes.Count) {
    Add-Failure "Expected $($expectedTypes.Count) Black Mud AI refinement rows but found $($rows.Count)"
}

foreach ($type in $expectedTypes.Keys) {
    $matches = @($rows | Where-Object { $_.asset_type -eq $type })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected exactly one $type row but found $($matches.Count)"
    }
}

$reviewNotesSeen = New-Object System.Collections.Generic.HashSet[string]
$reviewSheetsSeen = New-Object System.Collections.Generic.HashSet[string]
$promptRecordsSeen = New-Object System.Collections.Generic.HashSet[string]

foreach ($row in $rows) {
    if ($row.enemy_id -ne "black_mud_nightmare") {
        Add-Failure "Unexpected enemy id: $($row.enemy_id)"
    }

    if ($row.source_lock_ids -ne "black_mud_concept;black_mud_animation") {
        Add-Failure "$($row.asset_type) source locks should be black_mud_concept;black_mud_animation but found $($row.source_lock_ids)"
    }

    if ($row.active_screenshot -ne "07-active-enemy-black-mud.png") {
        Add-Failure "$($row.asset_type) active screenshot should be 07-active-enemy-black-mud.png but found $($row.active_screenshot)"
    }

    if ($row.recommendation -ne "candidate_review_only_pending_playmode_screenshot") {
        Add-Failure "$($row.candidate_path) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.candidate_path -notlike "design/development/asset_candidates/enemies/black_mud_nightmare/$batchSlug/*") {
        Add-Failure "$($row.candidate_path) must stay in the Black Mud Batch 39 candidate directory"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if (-not (Test-Path -LiteralPath $candidatePath)) {
        Add-Failure "Candidate PNG missing: $($row.candidate_path)"
    } else {
        $actualSize = Get-ImageSize $candidatePath
        $expectedSize = $expectedTypes[$row.asset_type]
        if ($null -ne $expectedSize -and $actualSize -ne $expectedSize) {
            Add-Failure "$($row.candidate_path) should be $expectedSize but is $actualSize"
        }

        if ($actualSize -ne $row.candidate_size) {
            Add-Failure "$($row.candidate_path) manifest candidate_size is $($row.candidate_size) but actual is $actualSize"
        }

        Test-Hash $candidatePath $row.candidate_sha256 "candidate $($row.candidate_path)"

        if (Test-Path -LiteralPath "$candidatePath.meta") {
            Add-Failure "Candidate PNG must not have a Unity .meta file: $($row.candidate_path).meta"
        }
    }

    if ($row.concept_source_path -notlike "design/*/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png") {
        Add-Failure "$($row.asset_type) must pin the Black Mud concept source"
    }

    if ($row.animation_source_path -notlike "design/*/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png") {
        Add-Failure "$($row.asset_type) must pin the Black Mud animation source"
    }

    Test-Hash (Resolve-ProjectPath $row.concept_source_path) $row.concept_source_sha256 "source concept"
    Test-Hash (Resolve-ProjectPath $row.animation_source_path) $row.animation_source_sha256 "source animation"

    if (-not (Test-Path -LiteralPath $row.generated_source_path)) {
        Add-Failure "Generated source path missing: $($row.generated_source_path)"
    } else {
        Test-Hash $row.generated_source_path $row.generated_source_sha256 "built-in generated source"
    }

    foreach ($referenceField in @("batch38_review_sheet", "batch38_combat_reference")) {
        $referencePath = Resolve-ProjectPath $row.$referenceField
        if (-not (Test-Path -LiteralPath $referencePath)) {
            Add-Failure "$referenceField missing: $($row.$referenceField)"
        }
    }

    $reviewSheetPath = Resolve-ProjectPath $row.review_sheet
    if (-not (Test-Path -LiteralPath $reviewSheetPath)) {
        Add-Failure "Review sheet missing: $($row.review_sheet)"
    } else {
        $reviewSheetsSeen.Add($row.review_sheet) | Out-Null
        $sheetSize = Get-ImageSize $reviewSheetPath
        if ($sheetSize -ne "1600x900") {
            Add-Failure "$($row.review_sheet) should be 1600x900 but is $sheetSize"
        }
        if (Test-Path -LiteralPath "$reviewSheetPath.meta") {
            Add-Failure "Review sheet must not have a Unity .meta file: $($row.review_sheet).meta"
        }
    }

    $reviewNotePath = Resolve-ProjectPath $row.review_note
    if (-not (Test-Path -LiteralPath $reviewNotePath)) {
        Add-Failure "Review note missing: $($row.review_note)"
    } else {
        $reviewNotesSeen.Add($row.review_note) | Out-Null
    }

    $promptPath = Resolve-ProjectPath $row.prompt_record
    if (-not (Test-Path -LiteralPath $promptPath)) {
        Add-Failure "Prompt record missing: $($row.prompt_record)"
    } else {
        $promptRecordsSeen.Add($row.prompt_record) | Out-Null
    }
}

if ($reviewNotesSeen.Count -ne 1) {
    Add-Failure "Expected one Black Mud AI review note but found $($reviewNotesSeen.Count)"
}

if ($reviewSheetsSeen.Count -ne 1) {
    Add-Failure "Expected one Black Mud AI review sheet but found $($reviewSheetsSeen.Count)"
}

if ($promptRecordsSeen.Count -ne 1) {
    Add-Failure "Expected one Black Mud AI prompt record but found $($promptRecordsSeen.Count)"
}

foreach ($noteRelative in $reviewNotesSeen) {
    $notePath = Resolve-ProjectPath $noteRelative
    $content = Get-Content -LiteralPath $notePath -Raw -Encoding UTF8
    foreach ($token in $requiredReviewTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$noteRelative is missing review token: $token"
        }
    }
}

foreach ($promptRelative in $promptRecordsSeen) {
    $promptPath = Resolve-ProjectPath $promptRelative
    $content = Get-Content -LiteralPath $promptPath -Raw -Encoding UTF8
    foreach ($token in $requiredPromptTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$promptRelative is missing prompt token: $token"
        }
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath $candidateRoot) -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 39 candidate folder must not contain Unity .meta files."
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Black Mud AI refinement validation failed with $($failures.Count) issue(s)."
}

Write-Host "Black Mud AI refinement candidate validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Review notes: $($reviewNotesSeen.Count)"
Write-Host "Review sheets: $($reviewSheetsSeen.Count)"
Write-Host "Prompt records: $($promptRecordsSeen.Count)"
Write-Host "Formal Unity import remains blocked pending active-enemy Play Mode screenshot review."
