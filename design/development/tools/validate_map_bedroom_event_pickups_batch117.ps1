param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_117_bedroom_event_pickups_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/map/bedroom_event_pickups/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH117_BEDROOM_EVENT_PICKUPS_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 117 file: $Relative"
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
    "$batchDirRelative/source/tc_map_pickup_batch117_chromakey_source_v001.png",
    "$batchDirRelative/source/tc_map_pickup_batch117_names.txt",
    "$batchDirRelative/source/tc_map_pickup_story_page_clue_pickup_v002_chromakey_source.png",
    "$batchDirRelative/alpha/tc_map_pickup_batch117_alpha_sheet_v001.png",
    "$batchDirRelative/alpha/tc_map_pickup_story_page_clue_pickup_v002_alpha.png",
    "$batchDirRelative/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_asset_table.csv",
    "$batchDirRelative/tc_map_pickup_batch117_semantic_manifest.csv",
    "$batchDirRelative/tc_map_pickup_batch117_semantic_contact_sheet_v001.png",
    "$batchDirRelative/tc_map_pickup_batch117_semantic_contact_sheet_v002.png",
    "$batchDirRelative/tc_map_pickup_batch117_96px_64px_bedroom_event_pickup_readability_board_v001.png",
    "$batchDirRelative/tc_map_pickup_batch117_96px_64px_bedroom_event_pickup_readability_board_v002.png",
    "$batchDirRelative/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_final_review.csv",
    "$batchDirRelative/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_process_note.md",
    "$batchDirRelative/BATCH117_BEDROOM_EVENT_PICKUPS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH117_BEDROOM_EVENT_PICKUPS_AGENT_REVIEW_2026-06-26.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 117 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "fish_treat_pickup",
    "dream_thread_spool_pickup",
    "memory_shard_cluster_pickup",
    "moon_ticket_pass_pickup",
    "blessing_lantern_pickup",
    "story_page_clue_pickup",
    "star_button_key_pickup",
    "soft_light_orb_pickup",
    "reroll_dice_charm_pickup"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/tc_map_pickup_batch117_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 117 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_map_pickup_batch117_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

$assetSemantics = @{}
foreach ($row in $assetRows) {
    $assetSemantics[$row.semantic_name] = $true
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*" -or $row.source_truth -notlike "*Batch101/108/109 bedroom-map context*" -or $row.source_truth -notlike "*Batch104 reward-token context*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1 plus bedroom/reward context"
    }
    if ($row.source_truth -notlike "*no IAd character-body claim*" -or $row.source_truth -notlike "*no HDo/FoW9 map-archive claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve source-boundary wording"
    }
    if ($row.risk -ne "safe_symbolic_non_character_bedroom_map_pickup") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_bedroom_event_pickups") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
}

$manifestIdsByName = @{}
foreach ($row in $manifestRows) {
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in manifest: $($row.semantic_name)"
    }
    $manifestIdsByName[$row.semantic_name] = $row.asset_id
    if (-not $assetSemantics.ContainsKey($row.semantic_name)) {
        Add-Failure "Manifest semantic_name has no asset-table semantic match: $($row.semantic_name)"
    }
    if ($row.alpha_extrema -ne "(0, 255)") {
        Add-Failure "Manifest row $($row.asset_id) does not have full alpha extrema"
    }
    if ($row.path -notlike "design/development/asset_candidates/map/bedroom_event_pickups/*") {
        Add-Failure "Manifest row $($row.asset_id) path is not workspace-relative design path: $($row.path)"
    }
    if ($row.semantic_name -eq "story_page_clue_pickup") {
        if ($row.path -notlike "*candidate_v002.png") {
            Add-Failure "story_page_clue_pickup must point to active candidate_v002 path"
        }
        if ($row.status -notlike "*v002*") {
            Add-Failure "story_page_clue_pickup status should record v002 rework"
        }
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
    if ($sprite.Name.Length -gt 104) {
        Add-Failure "Sprite filename is too long for safe import review: $($sprite.Name)"
    }
}

$variantImages = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*.png" -File)
$variantManifests = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*manifest.csv" -File)
if ($variantImages.Count -ne 45 -or $variantManifests.Count -ne 9) {
    Add-Failure "review variants should contain 45 PNGs and 9 manifests, found pngs=$($variantImages.Count) manifests=$($variantManifests.Count)"
}

$activeStoryV1 = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/semantic_sprites") -Filter "*story_page_clue_pickup*candidate_v001.png" -File)
if ($activeStoryV1.Count -gt 0) {
    Add-Failure "story_page_clue_pickup v001 must not remain in active semantic_sprites"
}

$storyV2Variants = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*story_page_clue_pickup*candidate_v002*.png" -File)
if ($storyV2Variants.Count -ne 5) {
    Add-Failure "story_page_clue_pickup v002 should have 5 active review variant PNGs, found $($storyV2Variants.Count)"
}

Assert-Exists "$batchDirRelative/superseded/story_page_v001/tc_map_pickup_story_page_clue_pickup_batch117_candidate_v001.png"

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_final_review.csv"))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Final review should contain 9 rows, found $($reviewRows.Count)"
}
$keepCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_keep" }).Count
$conditionalCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_conditional" }).Count
$pendingCount = @($reviewRows | Where-Object { $_.review_decision -eq "pending_review" }).Count
if ($keepCount -ne 5 -or $conditionalCount -ne 4 -or $pendingCount -ne 0) {
    Add-Failure "Expected final review counts keep=5 conditional=4 pending=0, got keep=$keepCount conditional=$conditionalCount pending=$pendingCount"
}

$expectedConditional = @("dream_thread_spool_pickup", "memory_shard_cluster_pickup", "soft_light_orb_pickup", "reroll_dice_charm_pickup")
foreach ($name in $expectedConditional) {
    $row = $reviewRows | Where-Object { $_.semantic_name -eq $name } | Select-Object -First 1
    if ($null -eq $row -or $row.review_decision -ne "candidate_conditional") {
        Add-Failure "Expected $name to be candidate_conditional"
    }
}

foreach ($row in $reviewRows) {
    if ($manifestIdsByName[$row.semantic_name] -ne $row.asset_id) {
        Add-Failure "Final review asset id does not match manifest for $($row.semantic_name)"
    }
    if ($row.unity_gate -notlike "*bedroom_map_screenshots*" -or $row.unity_gate -notlike "*clean_console*") {
        Add-Failure "Final review row $($row.semantic_name) missing bedroom-map Unity gate"
    }
    if ($row.ppu -ne "100") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected ppu: $($row.ppu)"
    }
    if ($row.pivot -ne "bottom-center") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)"
    }
    if ($row.sorting_layer -ne "Map pickup overlay") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "scene_owned_trigger_or_none") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_process_note.md" @(
    "built-in Codex imagegen",
    "imagegen skill",
    "does not expose a model selector",
    "#fb02f9",
    "1197158/1572516",
    "72149/1572516",
    "story_page_clue_pickup",
    "candidate_v002",
    "5812be2ee628e8b6530579237fbcc1271dde7dd89d28c09fffd94166f5185781",
    "8305fc8be2ea48dd2df1006fbfa23f22ede78fd527a8d045b955d7975dd72d54",
    "PASS_WITH_P2",
    "5 candidate_keep",
    "4 candidate_conditional",
    "No runtime import was performed"
)

Assert-Contains $reviewNoteRelative @(
    "PASS_WITH_P2",
    "candidate_complete_pending_unity_review",
    "candidate_keep",
    "candidate_conditional",
    "dream_thread_spool_pickup",
    "memory_shard_cluster_pickup",
    "soft_light_orb_pickup",
    "reroll_dice_charm_pickup",
    "Batch101/108/109/104",
    "No runtime import was performed",
    "clean Console"
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH117_BEDROOM_EVENT_PICKUPS_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH117_BEDROOM_EVENT_PICKUPS_AGENT_REVIEW_2026-06-26.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 117 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 117 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch117*" -or
    $_.Name -like "*batch_117*" -or
    $_.Name -like "*bedroom_event_pickups*" -or
    $_.Name -like "*tc_map_pickup*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 117 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 117 bedroom event pickups validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 117 bedroom event pickups validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/tc_map_pickup_batch117_semantic_manifest.csv"
