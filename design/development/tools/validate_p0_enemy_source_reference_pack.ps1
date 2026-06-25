Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_38_p0_enemy_source_reference_pack_2026-06-15"
$manifestRelative = "design/development/asset_candidates/enemies/$batchSlug/p0_enemy_batch38_source_reference_manifest.csv"
$manifestPath = Join-Path $projectRoot ($manifestRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$candidateRoot = "design/development/asset_candidates/enemies/p0_core/$batchSlug"

$expectedEnemies = @{
    black_mud_nightmare = @{
        ConceptLock = "black_mud_concept"
        AnimationLock = "black_mud_animation"
        Screenshot = "07-active-enemy-black-mud.png"
    }
    cold_light_shadow = @{
        ConceptLock = "cold_light_concept"
        AnimationLock = "cold_light_animation"
        Screenshot = "08-active-enemy-cold-light.png"
    }
    call_tyrant = @{
        ConceptLock = "call_tyrant_concept"
        AnimationLock = "call_tyrant_animation"
        Screenshot = "09-active-enemy-call-tyrant.png"
    }
}

$expectedTypes = @{
    concept_reference_512 = "512x512"
    animation_reference_512 = "512x512"
    combat_sprite_reference_512 = "512x512"
    warning_motif_reference_256 = "256x256"
    palette_swatch_reference_256 = "256x256"
}

$requiredReviewTokens = @(
    'candidate review only',
    'do not import into Unity yet',
    'Formal Unity import remains blocked',
    'Candidate files stay outside `Assets`',
    'No Unity `.meta` files',
    'No runtime enemy sprite',
    'Black Mud Nightmare',
    'Cold Light Shadow',
    'Call Tyrant',
    'source concept',
    'source animation',
    'Required active screenshot'
)

$expectedDesignRootBytes = "ZGVzaWduL+aipuWig+aUr+mFjeiAheaguOW/g+eOqeazlS9hc3NldHMvZW5lbWllcw=="
$expectedDesignRoot = [System.Text.Encoding]::UTF8.GetString([Convert]::FromBase64String($expectedDesignRootBytes))
$designEnemyPathPattern = 'design/[^`\s)]*/assets/enemies'

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
    throw "P0 enemy Batch 38 manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
$expectedRowCount = $expectedEnemies.Count * $expectedTypes.Count
if ($rows.Count -ne $expectedRowCount) {
    Add-Failure "Expected $expectedRowCount P0 enemy candidate rows but found $($rows.Count)"
}

foreach ($enemyId in $expectedEnemies.Keys) {
    $enemyRows = @($rows | Where-Object { $_.enemy_id -eq $enemyId })
    if ($enemyRows.Count -ne $expectedTypes.Count) {
        Add-Failure "Expected $($expectedTypes.Count) rows for $enemyId but found $($enemyRows.Count)"
    }

    foreach ($type in $expectedTypes.Keys) {
        $matches = @($enemyRows | Where-Object { $_.asset_type -eq $type })
        if ($matches.Count -ne 1) {
            Add-Failure "Expected exactly one $enemyId $type row but found $($matches.Count)"
        }
    }
}

$reviewNotesSeen = New-Object System.Collections.Generic.HashSet[string]
$reviewSheetsSeen = New-Object System.Collections.Generic.HashSet[string]

foreach ($row in $rows) {
    if (-not $expectedEnemies.ContainsKey($row.enemy_id)) {
        Add-Failure "Unexpected enemy id: $($row.enemy_id)"
        continue
    }

    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$($row.candidate_path) has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.candidate_path -notlike "$candidateRoot/*") {
        Add-Failure "$($row.candidate_path) must stay under $candidateRoot"
    }

    if ($row.candidate_path -like "Assets/*") {
        Add-Failure "$($row.candidate_path) must not be under Assets"
    }

    if ($row.recommendation -ne "candidate_review_only_pending_unity_import_gate") {
        Add-Failure "$($row.candidate_path) has unsafe recommendation: $($row.recommendation)"
    }

    $enemy = $expectedEnemies[$row.enemy_id]
    if ($row.active_screenshot -ne $enemy.Screenshot) {
        Add-Failure "$($row.enemy_id) active screenshot should be $($enemy.Screenshot) but found $($row.active_screenshot)"
    }

    if ($row.asset_type -eq "concept_reference_512" -or $row.asset_type -eq "combat_sprite_reference_512") {
        if ($row.source_lock_ids -ne $enemy.ConceptLock) {
            Add-Failure "$($row.enemy_id) $($row.asset_type) should use concept lock $($enemy.ConceptLock) but found $($row.source_lock_ids)"
        }
    } elseif ($row.asset_type -eq "animation_reference_512" -or $row.asset_type -eq "warning_motif_reference_256") {
        if ($row.source_lock_ids -ne $enemy.AnimationLock) {
            Add-Failure "$($row.enemy_id) $($row.asset_type) should use animation lock $($enemy.AnimationLock) but found $($row.source_lock_ids)"
        }
    } elseif ($row.asset_type -eq "palette_swatch_reference_256") {
        if ($row.source_lock_ids -notlike "*$($enemy.ConceptLock)*" -or $row.source_lock_ids -notlike "*$($enemy.AnimationLock)*") {
            Add-Failure "$($row.enemy_id) palette should reference both concept and animation locks but found $($row.source_lock_ids)"
        }
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if (-not (Test-Path -LiteralPath $candidatePath)) {
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

        if (Test-Path -LiteralPath "$candidatePath.meta") {
            Add-Failure "Candidate PNG must not have a Unity .meta file: $($row.candidate_path).meta"
        }
    }

    $conceptPath = Resolve-ProjectPath $row.concept_source_path
    Test-Hash $conceptPath $row.concept_source_sha256 "concept source $($row.enemy_id)"
    $animationPath = Resolve-ProjectPath $row.animation_source_path
    Test-Hash $animationPath $row.animation_source_sha256 "animation source $($row.enemy_id)"

    $reviewSheetPath = Resolve-ProjectPath $row.review_sheet
    if (-not (Test-Path -LiteralPath $reviewSheetPath)) {
        Add-Failure "Review sheet missing: $($row.review_sheet)"
    } else {
        $reviewSheetsSeen.Add($row.review_sheet) | Out-Null
        $sheetSize = Get-ImageSize $reviewSheetPath
        if ($sheetSize -ne "2400x1350") {
            Add-Failure "$($row.review_sheet) should be 2400x1350 but is $sheetSize"
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
}

if ($reviewNotesSeen.Count -ne 1) {
    Add-Failure "Expected one P0 enemy review note but found $($reviewNotesSeen.Count)"
}

if ($reviewSheetsSeen.Count -ne 1) {
    Add-Failure "Expected one P0 enemy review sheet but found $($reviewSheetsSeen.Count)"
}

foreach ($noteRelative in $reviewNotesSeen) {
    $notePath = Resolve-ProjectPath $noteRelative
    $content = Get-Content -LiteralPath $notePath -Raw -Encoding UTF8
    foreach ($token in $requiredReviewTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$noteRelative is missing review token: $token"
        }
    }

    $sourcePathMatches = [System.Text.RegularExpressions.Regex]::Matches($content, $designEnemyPathPattern)
    foreach ($match in $sourcePathMatches) {
        if (-not $match.Value.StartsWith($expectedDesignRoot, [System.StringComparison]::Ordinal)) {
            Add-Failure "$noteRelative contains noncanonical design enemy source path: $($match.Value)"
        }
    }
}

foreach ($row in $rows) {
    foreach ($field in @($row.concept_source_path, $row.animation_source_path)) {
        if (-not $field.StartsWith($expectedDesignRoot, [System.StringComparison]::Ordinal)) {
            Add-Failure "$($row.enemy_id) manifest source path is not under the canonical design enemy root: $field"
        }
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath $candidateRoot) -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 38 candidate folder must not contain Unity .meta files."
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "P0 enemy Batch 38 source-reference validation failed with $($failures.Count) issue(s)."
}

Write-Host "P0 enemy Batch 38 source-reference validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Enemies: $($expectedEnemies.Count)"
Write-Host "Review notes: $($reviewNotesSeen.Count)"
Write-Host "Review sheets: $($reviewSheetsSeen.Count)"
Write-Host "Formal Unity import remains blocked pending active-enemy Play Mode screenshot review."
