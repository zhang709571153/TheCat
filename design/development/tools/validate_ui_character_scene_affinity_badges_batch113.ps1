param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_113_character_scene_affinity_badges_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/ui/character_scene_affinity_badges/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH113_CHARACTER_SCENE_AFFINITY_BADGES_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 113 file: $Relative"
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
    "$batchDirRelative/source/thecat_ui_character_scene_affinity_badges_batch113_chromakey_source_v001.png",
    "$batchDirRelative/source/thecat_ui_character_scene_affinity_badges_batch113_alpha_sheet_v001.png",
    "$batchDirRelative/batch113_semantic_names.txt",
    "$batchDirRelative/thecat_batch_113_character_scene_affinity_badges_2026-06-26_asset_table.csv",
    "$batchDirRelative/thecat_ui_char_scene_aff_batch113_semantic_manifest.csv",
    "$batchDirRelative/thecat_ui_char_scene_aff_batch113_semantic_contact_sheet_v001.png",
    "$batchDirRelative/thecat_ui_char_scene_aff_batch113_96px_64px_readability_board_v001.png",
    "$batchDirRelative/thecat_batch_113_character_scene_affinity_badges_2026-06-26_final_review.csv",
    "$batchDirRelative/thecat_batch_113_character_scene_affinity_badges_2026-06-26_process_note.md",
    "$batchDirRelative/reviews/batch113_generated_candidates_contact_sheet.png",
    "$batchDirRelative/reviews/thecat_batch_113_character_scene_affinity_badges_2026-06-26_agent_review_prompt.md",
    "$batchDirRelative/BATCH113_CHARACTER_SCENE_AFFINITY_BADGES_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH113_CHARACTER_SCENE_AFFINITY_BADGES_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/thecat_batch_113_character_scene_affinity_badges_2026-06-26_candidate_review.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 113 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "saiban_bedroom_guard_badge",
    "nephthys_dream_route_badge",
    "suzune_cat_room_support_badge",
    "kagemaru_shadow_entry_badge",
    "cotton_safe_room_badge",
    "yuheng_reward_shop_badge",
    "locked_role_scene_badge",
    "unknown_role_scene_badge",
    "team_ready_scene_badge"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/batch113_semantic_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 113 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_113_character_scene_affinity_badges_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_ui_char_scene_aff_batch113_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

foreach ($row in $assetRows) {
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*" -or $row.source_truth -notlike "*Batch95 role-scene token context*" -or $row.source_truth -notlike "*Batch96 unlock role-scene token context*" -or $row.source_truth -notlike "*Batch97 unlock skill icon context*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1 plus Batch95/96/97 context"
    }
    if ($row.source_truth -notlike "*IAd live ACL blocked*" -or $row.source_truth -notlike "*no body/face/portrait/animation claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve character source-boundary wording"
    }
    if ($row.risk -ne "safe_symbolic_non_character_ui") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_character_scene_affinity_review") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
}

$manifestIdsByName = @{}
foreach ($row in $manifestRows) {
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in manifest: $($row.semantic_name)"
    }
    $manifestIdsByName[$row.semantic_name] = $row.asset_id
    if ($row.path -notlike "design/development/asset_candidates/ui/character_scene_affinity_badges/*") {
        Add-Failure "Manifest path is not repo-root relative via design/development for $($row.semantic_name): $($row.path)"
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

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_113_character_scene_affinity_badges_2026-06-26_final_review.csv"))
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
        Add-Failure "Final review asset id does not match manifest for $($row.semantic_name)"
    }
    if ($row.unity_gate -notlike "*unity_character_scene_ui_screenshot*" -or $row.unity_gate -notlike "*clean_console*") {
        Add-Failure "Final review row $($row.semantic_name) missing character-scene Unity gate"
    }
    if ($row.ppu -ne "100") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected ppu: $($row.ppu)"
    }
    if ($row.pivot -ne "center") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)"
    }
    if ($row.sorting_layer -ne "UI character scene affinity badge") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none_ui_parent_hit_area") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/thecat_batch_113_character_scene_affinity_badges_2026-06-26_process_note.md" @(
    'built-in Codex imagegen',
    'image2',
    'does not expose a model selector',
    '#ea06d5',
    '948183/1572516',
    '196724/1572516',
    'PASS_WITH_P2',
    'No runtime import was performed',
    'No candidate file was copied into'
)

Assert-Contains $reviewNoteRelative @(
    'PASS_WITH_P2',
    'candidate_complete_pending_unity_review',
    'candidate_keep',
    'candidate_conditional',
    'unknown_role_scene_badge',
    'veil/unknown-scene token',
    'No runtime import was performed',
    'Clean Console'
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH113_CHARACTER_SCENE_AFFINITY_BADGES_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH113_CHARACTER_SCENE_AFFINITY_BADGES_AGENT_REVIEW_2026-06-26.md"
$candidateReview = Resolve-ProjectPath "$batchDirRelative/reviews/thecat_batch_113_character_scene_affinity_badges_2026-06-26_candidate_review.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $candidateReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $candidateHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $candidateReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $candidateHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 113 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 113 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch113*" -or
    $_.Name -like "*character_scene_affinity*" -or
    $_.Name -like "*char_scene_aff*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 113 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 113 character scene affinity badges validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 113 character scene affinity badges validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/thecat_ui_char_scene_aff_batch113_semantic_manifest.csv"
