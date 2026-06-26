param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_115_skill_targeting_telegraphs_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/vfx/skill_targeting_telegraphs/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH115_SKILL_TARGETING_TELEGRAPHS_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 115 file: $Relative"
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
    "$batchDirRelative/source/tc_vfx_tgt_batch115_chromakey_source_v001.png",
    "$batchDirRelative/source/tc_vfx_tgt_batch115_names.txt",
    "$batchDirRelative/alpha/tc_vfx_tgt_batch115_alpha_sheet_v001.png",
    "$batchDirRelative/tc_vfx_tgt_batch_115_skill_targeting_telegraphs_2026-06-26_asset_table.csv",
    "$batchDirRelative/tgt_batch115_semantic_manifest.csv",
    "$batchDirRelative/tgt_batch115_semantic_contact_sheet_v001.png",
    "$batchDirRelative/tgt_batch115_128px_64px_skill_targeting_readability_board_v001.png",
    "$batchDirRelative/tc_vfx_tgt_batch_115_skill_targeting_telegraphs_2026-06-26_final_review.csv",
    "$batchDirRelative/tc_vfx_tgt_batch_115_skill_targeting_telegraphs_2026-06-26_process_note.md",
    "$batchDirRelative/BATCH115_SKILL_TARGETING_TELEGRAPHS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH115_SKILL_TARGETING_TELEGRAPHS_AGENT_REVIEW_2026-06-26.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 115 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "skill_target_valid_ring",
    "skill_target_invalid_cross",
    "skill_aoe_circle_field",
    "skill_aoe_cone_field",
    "skill_line_target_strip",
    "skill_chain_link_path",
    "skill_shield_zone_floor",
    "skill_heal_zone_floor",
    "skill_summon_slot_rune"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/tc_vfx_tgt_batch115_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 115 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_vfx_tgt_batch_115_skill_targeting_telegraphs_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tgt_batch115_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

$assetNames = @{}
foreach ($row in $assetRows) {
    $assetNames[$row.semantic_name] = $true
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1 revision 816"
    }
    if ($row.risk -ne "safe_symbolic_non_character_skill_telegraph") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_static_skill_targeting_telegraphs") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
}

$manifestIdsByName = @{}
foreach ($row in $manifestRows) {
    $manifestIdsByName[$row.semantic_name] = $row.asset_id
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in manifest: $($row.semantic_name)"
    }
    if (-not $assetNames.ContainsKey($row.semantic_name)) {
        Add-Failure "Manifest semantic_name missing from asset table: $($row.semantic_name)"
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

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_vfx_tgt_batch_115_skill_targeting_telegraphs_2026-06-26_final_review.csv"))
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
    if ($manifestIdsByName[$row.semantic_name] -ne $row.asset_id) {
        Add-Failure "Final review asset id mismatch for $($row.semantic_name)"
    }
    if ($row.sorting_layer -ne "WorldVFX/Telegraph") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ($row.unity_gate -notlike "*skill_cast_battle_screenshot*") {
        Add-Failure "Final review row $($row.semantic_name) missing skill-cast screenshot gate"
    }
}

Assert-Contains "$batchDirRelative/tc_vfx_tgt_batch_115_skill_targeting_telegraphs_2026-06-26_process_note.md" @(
    "built-in Codex",
    "imagegen",
    "Qr1 UI/style truth revision 816",
    "does not expose a model selector",
    "No runtime import was performed",
    "No character body, face, portrait",
    "Final review CSV: 5",
    "4 candidate_conditional"
)

Assert-Contains $reviewNoteRelative @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "Hypatia",
    "Curie",
    "Beauvoir",
    "candidate_complete_pending_unity_review",
    "Do not import Batch115"
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH115_SKILL_TARGETING_TELEGRAPHS_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH115_SKILL_TARGETING_TELEGRAPHS_AGENT_REVIEW_2026-06-26.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 115 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 115 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*tgt_batch115*" -or
    $_.Name -like "*batch115*" -or
    $_.FullName -like "*skill_targeting_telegraphs*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 115 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 115 skill targeting telegraph validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 115 skill targeting telegraph validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/tgt_batch115_semantic_manifest.csv"
