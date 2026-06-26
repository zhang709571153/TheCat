param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_107_scene_selection_tokens_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/ui/scene_selection_tokens/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH107_SCENE_SELECTION_TOKENS_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 107 file: $Relative"
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
    "$batchDirRelative/source/tc_ui_scene_batch107_chromakey_source_v001.png",
    "$batchDirRelative/source/tc_ui_scene_batch107_names.txt",
    "$batchDirRelative/alpha/tc_ui_scene_batch107_alpha_sheet_v001.png",
    "$batchDirRelative/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_asset_table.csv",
    "$batchDirRelative/tc_ui_scene_batch107_semantic_manifest.csv",
    "$batchDirRelative/tc_ui_scene_batch107_semantic_contact_sheet_v001.png",
    "$batchDirRelative/tc_ui_scene_batch107_64px_scene_selection_readability_board_v001.png",
    "$batchDirRelative/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_final_review.csv",
    "$batchDirRelative/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_process_note.md",
    "$batchDirRelative/BATCH107_SCENE_SELECTION_TOKENS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH107_SCENE_SELECTION_TOKENS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_candidate_review.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 107 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "bedroom_scene_card_token",
    "cat_room_scene_card_token",
    "dream_route_scene_card_token",
    "battle_scene_card_token",
    "reward_scene_card_token",
    "shop_event_scene_card_token",
    "settings_scene_card_token",
    "locked_scene_card_token",
    "unknown_scene_card_token"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/tc_ui_scene_batch107_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 107 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_ui_scene_batch107_semantic_manifest.csv"))
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
    if ($row.source_truth -notlike "*no IAd character-body claim*" -or $row.source_truth -notlike "*no HDo map-archive claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve source-boundary wording"
    }
    if ($row.risk -ne "safe_symbolic_non_character") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_scene_selection_ui_tokens") {
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

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_final_review.csv"))
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
    if ($row.sorting_layer -ne "UI canvas scene token slot") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none_ui_click_area_owned_by_parent") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
}

Assert-Contains "$batchDirRelative/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_process_note.md" @(
    "built-in Codex",
    "imagegen",
    "Qr1 UI/style truth revision 816",
    "No runtime import was performed",
    "No character body, portrait, paw, animation frame",
    "No HDo/FoW9 map archive coverage is claimed",
    "candidate_keep=5",
    "candidate_conditional=4"
)

Assert-Contains $reviewNoteRelative @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "candidate_complete_pending_unity_review",
    "No runtime import was performed",
    "Production QA",
    "source-lock",
    "unknown",
    "cat-room/shop/reward"
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH107_SCENE_SELECTION_TOKENS_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH107_SCENE_SELECTION_TOKENS_AGENT_REVIEW_2026-06-26.md"
$candidateReview = Resolve-ProjectPath "$batchDirRelative/reviews/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_candidate_review.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $candidateReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $candidateHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $candidateReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $candidateHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 107 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 107 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch107*" -or
    $_.Name -like "*scene_selection_tokens*" -or
    $_.Name -like "*tc_ui_scene*batch107*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 107 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 107 scene-selection token validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 107 scene-selection token validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/tc_ui_scene_batch107_semantic_manifest.csv"
