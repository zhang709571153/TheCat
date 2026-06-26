param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_105_combat_feedback_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/vfx/combat_feedback/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH105_COMBAT_FEEDBACK_TOKENS_AGENT_REVIEW_2026-06-26.md"
$failures = New-Object System.Collections.Generic.List[string]

function Resolve-ProjectPath {
    param([string]$Relative)
    return Join-Path $projectRoot ($Relative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
}

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message) | Out-Null
}

function Assert-Exists {
    param([string]$Relative)
    if (-not (Test-Path -LiteralPath (Resolve-ProjectPath $Relative))) {
        Add-Failure "Missing required Batch 105 file: $Relative"
    }
}

function Assert-Contains {
    param([string]$Relative, [string[]]$Tokens)
    $path = Resolve-ProjectPath $Relative
    if (-not (Test-Path -LiteralPath $path)) {
        Add-Failure "Missing required text file: $Relative"
        return
    }
    $text = Get-Content -Raw -LiteralPath $path
    foreach ($token in $Tokens) {
        if ($text -notlike "*$token*") {
            Add-Failure "$Relative missing token: $token"
        }
    }
}

$requiredFiles = @(
    "$batchDirRelative/source/tc_vfx_cfb_batch105_chromakey_source_v001.png",
    "$batchDirRelative/source/tc_vfx_cfb_batch105_names.txt",
    "$batchDirRelative/alpha/tc_vfx_cfb_batch105_alpha_sheet_v001.png",
    "$batchDirRelative/tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_asset_table.csv",
    "$batchDirRelative/tc_vfx_cfb_batch105_semantic_manifest.csv",
    "$batchDirRelative/tc_vfx_cfb_batch105_semantic_contact_sheet_v001.png",
    "$batchDirRelative/tc_vfx_cfb_batch105_64px_combat_feedback_readability_board_v001.png",
    "$batchDirRelative/tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_final_review.csv",
    "$batchDirRelative/tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_process_note.md",
    "$batchDirRelative/BATCH105_COMBAT_FEEDBACK_TOKENS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH105_COMBAT_FEEDBACK_TOKENS_AGENT_REVIEW_2026-06-26.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 105 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "normal_hit_spark_token",
    "shield_flash_token",
    "slow_freeze_token",
    "aftershock_ring_token",
    "bed_hit_crack_token",
    "monster_death_puff_token",
    "damage_number_chip_token",
    "heal_plus_spark_token",
    "debuff_burst_token"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/tc_vfx_cfb_batch105_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 105 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_vfx_cfb_batch105_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

$assetIdsByName = @{}
foreach ($row in $assetRows) {
    $assetIdsByName[$row.semantic_name] = $row.asset_id
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1 revision 816"
    }
    if ($row.risk -ne "safe_symbolic_non_character") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_static_vfx_tokens") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
}

foreach ($row in $manifestRows) {
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in manifest: $($row.semantic_name)"
    }
    if ($assetIdsByName[$row.semantic_name] -ne $row.asset_id) {
        Add-Failure "Asset id mismatch for $($row.semantic_name): manifest=$($row.asset_id) asset_table=$($assetIdsByName[$row.semantic_name])"
    }
    if ($row.alpha_extrema -ne "(0, 255)") {
        Add-Failure "Manifest row $($row.asset_id) does not have full alpha extrema"
    }
    $spritePath = Resolve-ProjectPath $row.path
    if (-not (Test-Path -LiteralPath $spritePath)) {
        Add-Failure "Missing sprite path from manifest: $($row.path)"
        continue
    }
    $hash = (Get-FileHash -Algorithm SHA256 -LiteralPath $spritePath).Hash.ToLowerInvariant()
    if ($hash -ne $row.sha256.ToLowerInvariant()) {
        Add-Failure "Hash mismatch for $($row.path)"
    }
}

$spriteFiles = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/semantic_sprites") -Filter "*.png" -File)
if ($spriteFiles.Count -ne 9) {
    Add-Failure "semantic_sprites should contain exactly 9 current PNGs, found $($spriteFiles.Count)"
}
foreach ($sprite in $spriteFiles) {
    if ($sprite.Name.Length -gt 96) {
        Add-Failure "Sprite filename is too long for safe import review: $($sprite.Name)"
    }
}

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_final_review.csv"))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Final review should contain 9 rows, found $($reviewRows.Count)"
}
$keepCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_keep" }).Count
$conditionalCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_conditional" }).Count
$pendingCount = @($reviewRows | Where-Object { $_.review_decision -eq "pending_review" }).Count
if ($keepCount -ne 5 -or $conditionalCount -ne 4 -or $pendingCount -ne 0) {
    Add-Failure "Expected final review counts keep=5 conditional=4 pending=0, got keep=$keepCount conditional=$conditionalCount pending=$pendingCount"
}
foreach ($row in $reviewRows) {
    if ($assetIdsByName[$row.semantic_name] -ne $row.asset_id) {
        Add-Failure "Final review asset id mismatch for $($row.semantic_name)"
    }
    if ($row.sorting_layer -ne "UI/VFX overlay") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
}

Assert-Contains "$batchDirRelative/tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_process_note.md" @(
    "built-in Codex",
    "imagegen",
    "Qr1 UI/style truth revision 816",
    "No runtime import was performed",
    "No character body, portrait, animation frame, framesheet",
    "damage_number_chip_token",
    "bed_hit_crack_token"
)

Assert-Contains $reviewNoteRelative @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "Production QA",
    "P1 closed",
    "candidate_complete_pending_unity_review",
    "No runtime import was performed"
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH105_COMBAT_FEEDBACK_TOKENS_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH105_COMBAT_FEEDBACK_TOKENS_AGENT_REVIEW_2026-06-26.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 105 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 105 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*tc_vfx_cfb*" -or
    $_.Name -like "*batch105*" -or
    $_.FullName -like "*combat_feedback*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 105 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 105 combat feedback token validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 105 combat feedback token validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/tc_vfx_cfb_batch105_semantic_manifest.csv"
