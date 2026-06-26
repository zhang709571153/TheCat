param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_112_scene_preview_accent_badges_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/ui/scene_preview_accent_badges/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH112_SCENE_PREVIEW_ACCENT_BADGES_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 112 file: $Relative"
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
    "$batchDirRelative/source/tc_ui_scene_accent_batch112_chromakey_source_v001.png",
    "$batchDirRelative/tc_ui_scene_accent_batch112_names.txt",
    "$batchDirRelative/alpha/tc_ui_scene_accent_batch112_alpha_sheet_v001.png",
    "$batchDirRelative/alpha/tc_ui_scene_accent_batch112_alpha_sheet_v002_no_despill.png",
    "$batchDirRelative/superseded/rejected_alpha/tc_ui_scene_accent_batch112_alpha_sheet_v001_despill_damage.png",
    "$batchDirRelative/superseded/rejected_alpha/tc_ui_scene_accent_batch112_semantic_contact_sheet_v001_despill_damage.png",
    "$batchDirRelative/tc_batch_112_scene_preview_accent_badges_2026-06-26_asset_table.csv",
    "$batchDirRelative/tc_ui_scene_accent_batch112_semantic_manifest.csv",
    "$batchDirRelative/tc_ui_scene_accent_batch112_semantic_contact_sheet_v001.png",
    "$batchDirRelative/tc_ui_scene_accent_batch112_96px_64px_scene_preview_readability_board_v001.png",
    "$batchDirRelative/tc_batch_112_scene_preview_accent_badges_2026-06-26_final_review.csv",
    "$batchDirRelative/tc_batch_112_scene_preview_accent_badges_2026-06-26_process_note.md",
    "$batchDirRelative/reviews/tc_batch_112_scene_preview_accent_badges_2026-06-26_agent_review_prompt.md",
    "$batchDirRelative/BATCH112_SCENE_PREVIEW_ACCENT_BADGES_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH112_SCENE_PREVIEW_ACCENT_BADGES_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/tc_batch_112_scene_preview_accent_badges_2026-06-26_candidate_review.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 112 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "bedroom_accent_badge",
    "cat_room_accent_badge",
    "dream_route_accent_badge",
    "battle_accent_badge",
    "reward_accent_badge",
    "shop_event_accent_badge",
    "settings_accent_badge",
    "locked_accent_badge",
    "unknown_accent_badge"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/tc_ui_scene_accent_batch112_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 112 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_batch_112_scene_preview_accent_badges_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_ui_scene_accent_batch112_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

$assetIdsByName = @{}
foreach ($row in $assetRows) {
    $assetIdsByName[$row.semantic_name] = $row.asset_id
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*" -or $row.source_truth -notlike "*Batch107 scene-selection token context*" -or $row.source_truth -notlike "*Batch111 scene-preview backplate context*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1, Batch107, and Batch111 source context"
    }
    if ($row.source_truth -notlike "*no IAd character-body claim*" -or $row.source_truth -notlike "*no HDo/FoW9 map-archive claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve source-boundary wording"
    }
    if ($row.risk -ne "safe_symbolic_non_character_scene_preview_ui") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_scene_preview_overlay_review") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
}

$manifestIdsByName = @{}
foreach ($row in $manifestRows) {
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in manifest: $($row.semantic_name)"
    }
    $manifestIdsByName[$row.semantic_name] = $row.asset_id
    if ($assetIdsByName[$row.semantic_name] -ne $row.asset_id) {
        Add-Failure "Asset table asset_id does not match manifest for $($row.semantic_name)"
    }
    if ($row.path -notlike "design/development/asset_candidates/ui/scene_preview_accent_badges/*") {
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

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_batch_112_scene_preview_accent_badges_2026-06-26_final_review.csv"))
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
    if ($row.ppu -ne "100") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected ppu: $($row.ppu)"
    }
    if ($row.pivot -ne "center") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)"
    }
    if ($row.sorting_layer -ne "UI scene preview accent overlay") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none_ui_parent_hit_area") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/tc_batch_112_scene_preview_accent_badges_2026-06-26_process_note.md" @(
    'built-in Codex imagegen',
    'v001 alpha',
    '--despill',
    'rejected',
    'v002 alpha',
    'no-despill',
    '#ef05d7',
    '830940/1572516',
    '387370/1572516',
    'PASS_WITH_P2',
    'No runtime import was performed',
    'No candidate file was copied into Assets/'
)

Assert-Contains $reviewNoteRelative @(
    'PASS_WITH_P2',
    'candidate_complete_pending_unity_review',
    'no remaining P1 visual/style blocker',
    'Production QA reviewer',
    'Target-size readability reviewer',
    'candidate_keep',
    'candidate_conditional',
    'No runtime import was performed',
    'Clean Console'
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH112_SCENE_PREVIEW_ACCENT_BADGES_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH112_SCENE_PREVIEW_ACCENT_BADGES_AGENT_REVIEW_2026-06-26.md"
$candidateReview = Resolve-ProjectPath "$batchDirRelative/reviews/tc_batch_112_scene_preview_accent_badges_2026-06-26_candidate_review.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $candidateReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $candidateHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $candidateReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $candidateHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 112 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 112 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch112*" -or
    $_.Name -like "*scene_preview_accent*" -or
    $_.Name -like "*tc_ui_scene_accent*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 112 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 112 scene preview accent badges validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 112 scene preview accent badges validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/tc_ui_scene_accent_batch112_semantic_manifest.csv"
