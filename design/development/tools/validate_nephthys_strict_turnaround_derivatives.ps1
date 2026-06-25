Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$manifestRelative = "design/development/asset_candidates/starter_cats/batch_32_nephthys_strict_turnaround_derivatives_2026-06-14/nephthys_batch32_strict_turnaround_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative

$expectedTypes = @{
    front_view_reference_512 = "512x512"
    side_view_reference_512 = "512x512"
    back_view_reference_512 = "512x512"
    combat_sprite_reference_512 = "512x512"
    hud_avatar_reference_256 = "256x256"
    skill_icon_motif_reference_128 = "128x128"
    palette_swatch_reference_256 = "256x256"
}

$requiredReviewTokens = @(
    "candidate review only",
    "do not import into Unity yet",
    "formal import remains blocked",
    "colored three-view turnaround",
    "front",
    "side",
    "back",
    "palette",
    "hood",
    "script trim",
    "pyramid",
    "obelisk",
    "crescent",
    "blue gem",
    "ankh",
    "striped tail",
    "outside Assets",
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
    throw "Nephthys strict-turnaround manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne $expectedTypes.Count) {
    Add-Failure "Expected $($expectedTypes.Count) Nephthys candidate rows but found $($rows.Count)"
}

foreach ($type in $expectedTypes.Keys) {
    $matches = @($rows | Where-Object { $_.asset_type -eq $type })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected exactly one $type row but found $($matches.Count)"
    }
}

$reviewNotesSeen = New-Object System.Collections.Generic.HashSet[string]
$reviewSheetsSeen = New-Object System.Collections.Generic.HashSet[string]

foreach ($row in $rows) {
    if ($row.cat_id -ne "nephthys") {
        Add-Failure "Unexpected cat id: $($row.cat_id)"
    }

    if ($row.source_lock_id -ne "nephthys_turnaround_colored") {
        Add-Failure "$($row.asset_type) source lock should be nephthys_turnaround_colored but found $($row.source_lock_id)"
    }

    if ($row.active_screenshot -ne "05-active-cat-nephthys.png") {
        Add-Failure "$($row.asset_type) active screenshot should be 05-active-cat-nephthys.png but found $($row.active_screenshot)"
    }

    if ($row.recommendation -ne "candidate_review_only_pending_playmode_screenshot") {
        Add-Failure "$($row.candidate_path) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.candidate_path -notlike "design/development/asset_candidates/starter_cats/nephthys/batch_32_nephthys_strict_turnaround_derivatives_2026-06-14/*") {
        Add-Failure "$($row.candidate_path) must stay in the Nephthys Batch 32 candidate directory"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if (-not (Test-Path $candidatePath)) {
        Add-Failure "Candidate PNG missing: $($row.candidate_path)"
    } else {
        $actualSize = Get-ImageSize $candidatePath
        if ($actualSize -ne $expectedTypes[$row.asset_type]) {
            Add-Failure "$($row.candidate_path) should be $($expectedTypes[$row.asset_type]) but is $actualSize"
        }

        if ($actualSize -ne $row.candidate_size) {
            Add-Failure "$($row.candidate_path) manifest candidate_size is $($row.candidate_size) but actual is $actualSize"
        }

        Test-Hash $candidatePath $row.candidate_sha256 "candidate $($row.candidate_path)"

        if (Test-Path "$candidatePath.meta") {
            Add-Failure "Candidate PNG must not have a Unity .meta file: $($row.candidate_path).meta"
        }
    }

    if ($row.source_turnaround_path -notlike "design/*/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png") {
        Add-Failure "$($row.asset_type) must pin the Nephthys colored turnaround source"
    }

    $sourcePath = Resolve-ProjectPath $row.source_turnaround_path
    Test-Hash $sourcePath $row.source_turnaround_sha256 "source turnaround $($row.source_lock_id)"

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

    $reviewNotePath = Resolve-ProjectPath $row.review_note
    if (-not (Test-Path $reviewNotePath)) {
        Add-Failure "Review note missing: $($row.review_note)"
    } else {
        $reviewNotesSeen.Add($row.review_note) | Out-Null
    }
}

if ($reviewNotesSeen.Count -ne 1) {
    Add-Failure "Expected one Nephthys review note but found $($reviewNotesSeen.Count)"
}

if ($reviewSheetsSeen.Count -ne 1) {
    Add-Failure "Expected one Nephthys review sheet but found $($reviewSheetsSeen.Count)"
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

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Nephthys strict-turnaround derivative validation failed with $($failures.Count) issue(s)."
}

Write-Host "Nephthys strict-turnaround derivative validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Review notes: $($reviewNotesSeen.Count)"
Write-Host "Review sheets: $($reviewSheetsSeen.Count)"
Write-Host "Formal Unity import remains blocked pending active-cat Play Mode screenshot review."
