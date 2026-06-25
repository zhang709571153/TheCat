Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$manifestRelative = "design/development/asset_candidates/starter_cats/batch_05_source_locked_derivatives_2026-06-14/starter_cat_batch05_candidate_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative

$expectedCats = @("saiban", "nephthys", "suzune")
$expectedTypes = @{
    combat_sprite_refinement_512 = "512x512"
    front_animation_keyframe_512 = "512x512"
    hud_avatar_256 = "256x256"
    skill_icon_motif_128 = "128x128"
}
$expectedScreenshots = @{
    saiban = "04-active-cat-saiban.png"
    nephthys = "05-active-cat-nephthys.png"
    suzune = "06-active-cat-suzune.png"
}
$expectedSourceLocks = @{
    saiban = "saiban_turnaround_colored"
    nephthys = "nephthys_turnaround_colored"
    suzune = "suzune_turnaround_colored"
}
$catReviewTokens = @{
    saiban = @("shield", "sword", "tabby", "cape", "helm")
    nephthys = @("hood", "pyramid", "obelisk", "gold and blue", "dream-script")
    suzune = @("calico", "shrine", "bell", "wand", "vermilion")
}
$commonReviewTokens = @(
    "do not import into Unity yet",
    "colored turnaround",
    "front",
    "side",
    "back",
    "palette",
    "Prohibited drift",
    "Rejection Rules"
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

function Test-PathOutsideAssets {
    param([string]$RelativePath)
    return $RelativePath -like "design/development/asset_candidates/starter_cats/*"
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
    throw "Starter cat candidate manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne 12) {
    Add-Failure "Expected 12 starter cat candidate rows but found $($rows.Count)"
}

$seenKeys = New-Object System.Collections.Generic.HashSet[string]
$reviewNotesSeen = New-Object System.Collections.Generic.HashSet[string]
$reviewSheetsSeen = New-Object System.Collections.Generic.HashSet[string]

foreach ($cat in $expectedCats) {
    foreach ($type in $expectedTypes.Keys) {
        $matches = @($rows | Where-Object { $_.cat_id -eq $cat -and $_.asset_type -eq $type })
        if ($matches.Count -ne 1) {
            Add-Failure "Expected exactly one $cat $type row but found $($matches.Count)"
        }
    }
}

foreach ($row in $rows) {
    $key = "$($row.cat_id)|$($row.asset_type)|$($row.candidate_path)"
    if (-not $seenKeys.Add($key)) {
        Add-Failure "Duplicate starter cat candidate row: $key"
    }

    if (-not $expectedCats.Contains($row.cat_id)) {
        Add-Failure "Unexpected starter cat id: $($row.cat_id)"
    }

    if (-not $expectedTypes.ContainsKey($row.asset_type)) {
        Add-Failure "Unexpected starter cat candidate type: $($row.asset_type)"
    }

    if ($expectedSourceLocks.ContainsKey($row.cat_id) -and $row.source_lock_id -ne $expectedSourceLocks[$row.cat_id]) {
        Add-Failure "$($row.cat_id) source lock should be $($expectedSourceLocks[$row.cat_id]) but found $($row.source_lock_id)"
    }

    if ($expectedScreenshots.ContainsKey($row.cat_id) -and $row.active_screenshot -ne $expectedScreenshots[$row.cat_id]) {
        Add-Failure "$($row.cat_id) active screenshot should be $($expectedScreenshots[$row.cat_id]) but found $($row.active_screenshot)"
    }

    if ($row.recommendation -ne "candidate_review_only_pending_playmode_screenshot") {
        Add-Failure "$($row.candidate_path) has unsafe recommendation: $($row.recommendation)"
    }

    if (-not (Test-PathOutsideAssets $row.candidate_path)) {
        Add-Failure "$($row.candidate_path) must stay under design/development/asset_candidates/starter_cats"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if (-not (Test-Path $candidatePath)) {
        Add-Failure "Candidate PNG missing: $($row.candidate_path)"
    } else {
        $expectedSize = $expectedTypes[$row.asset_type]
        $actualSize = Get-ImageSize $candidatePath
        if ($actualSize -ne $expectedSize) {
            Add-Failure "$($row.candidate_path) should be $expectedSize but is $actualSize"
        }

        Test-Hash $candidatePath $row.candidate_sha256 "candidate $($row.candidate_path)"

        $metaPath = "$candidatePath.meta"
        if (Test-Path $metaPath) {
            Add-Failure "Candidate PNG must not have a Unity .meta file: $($row.candidate_path).meta"
        }
    }

    $sourcePath = Resolve-ProjectPath $row.source_turnaround_path
    Test-Hash $sourcePath $row.source_turnaround_sha256 "source turnaround $($row.source_lock_id)"

    $spritePath = Resolve-ProjectPath $row.current_sprite_path
    Test-Hash $spritePath $row.current_sprite_sha256 "locked Unity sprite $($row.current_sprite_path)"

    if (-not (Test-PathOutsideAssets $row.review_sheet)) {
        Add-Failure "$($row.review_sheet) must stay under design/development/asset_candidates/starter_cats"
    }

    $reviewSheetPath = Resolve-ProjectPath $row.review_sheet
    if (-not (Test-Path $reviewSheetPath)) {
        Add-Failure "Review sheet missing: $($row.review_sheet)"
    } else {
        $reviewSheetsSeen.Add($row.review_sheet) | Out-Null
        $sheetSize = Get-ImageSize $reviewSheetPath
        if ($sheetSize -ne "1600x900") {
            Add-Failure "$($row.review_sheet) should be 1600x900 but is $sheetSize"
        }

        if (Test-Path "$reviewSheetPath.meta") {
            Add-Failure "Review sheet must not have a Unity .meta file: $($row.review_sheet).meta"
        }
    }

    if (-not (Test-PathOutsideAssets $row.review_note)) {
        Add-Failure "$($row.review_note) must stay under design/development/asset_candidates/starter_cats"
    }

    $reviewNotePath = Resolve-ProjectPath $row.review_note
    if (-not (Test-Path $reviewNotePath)) {
        Add-Failure "Review note missing: $($row.review_note)"
    } else {
        $reviewNotesSeen.Add($row.review_note) | Out-Null
    }
}

foreach ($noteRelative in $reviewNotesSeen) {
    $notePath = Resolve-ProjectPath $noteRelative
    $content = Get-Content -LiteralPath $notePath -Raw

    foreach ($token in $commonReviewTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$noteRelative is missing review token: $token"
        }
    }

    $catId = ($rows | Where-Object { $_.review_note -eq $noteRelative } | Select-Object -First 1).cat_id
    foreach ($token in $catReviewTokens[$catId]) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$noteRelative is missing $catId trait token: $token"
        }
    }
}

if ($reviewNotesSeen.Count -ne 3) {
    Add-Failure "Expected 3 per-cat review notes but found $($reviewNotesSeen.Count)"
}

if ($reviewSheetsSeen.Count -ne 3) {
    Add-Failure "Expected 3 per-cat review sheets but found $($reviewSheetsSeen.Count)"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Starter cat candidate pack validation failed with $($failures.Count) issue(s)."
}

Write-Host "Starter cat candidate pack validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Review notes: $($reviewNotesSeen.Count)"
Write-Host "Review sheets: $($reviewSheetsSeen.Count)"
Write-Host "Formal Unity import remains blocked pending active-cat Play Mode screenshot review."
