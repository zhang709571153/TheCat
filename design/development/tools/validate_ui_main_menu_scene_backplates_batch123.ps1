param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_123_main_menu_scene_backplates_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/ui/main_menu_scene_backplates/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH123_MAIN_MENU_SCENE_BACKPLATES_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 123 file: $Relative"
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
    "$batchDirRelative/source/thecat_ui_main_menu_scene_backplates_batch123_source_sheet_v001.png",
    "$batchDirRelative/alpha/thecat_ui_main_menu_scene_backplates_batch123_alpha_sheet_v001.png",
    "$batchDirRelative/thecat_ui_main_menu_scene_backplates_batch123_names.txt",
    "$batchDirRelative/thecat_batch_123_main_menu_scene_backplates_2026-06-26_asset_table.csv",
    "$batchDirRelative/tc_ui_menu_backplate_batch123_semantic_manifest.csv",
    "$batchDirRelative/tc_ui_menu_backplate_batch123_semantic_contact_sheet_v001.png",
    "$batchDirRelative/tc_ui_menu_backplate_batch123_192x108_96x64_readability_board_v001.png",
    "$batchDirRelative/thecat_batch_123_main_menu_scene_backplates_2026-06-26_process_note.md",
    "$batchDirRelative/thecat_batch_123_main_menu_scene_backplates_2026-06-26_final_review.csv",
    "$batchDirRelative/thecat_batch_123_main_menu_scene_backplates_2026-06-26_candidate_review.md",
    "$batchDirRelative/reviews/thecat_batch_123_main_menu_scene_backplates_2026-06-26_candidate_review.md",
    "$batchDirRelative/reviews/thecat_batch_123_main_menu_scene_backplates_2026-06-26_agent_review_prompt.md",
    "$batchDirRelative/BATCH123_MAIN_MENU_SCENE_BACKPLATES_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH123_MAIN_MENU_SCENE_BACKPLATES_AGENT_REVIEW_2026-06-26.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 123 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "title_crest_backplate",
    "start_continue_pedestal",
    "dream_window_backplate",
    "cat_room_doorway_frame",
    "route_map_scroll_frame",
    "settings_corner_panel",
    "notice_ribbon_backplate",
    "locked_file_seal",
    "version_footer_plaque"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/thecat_ui_main_menu_scene_backplates_batch123_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 123 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_123_main_menu_scene_backplates_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_ui_menu_backplate_batch123_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

$assetSemantics = @{}
foreach ($row in $assetRows) {
    $assetSemantics[$row.semantic_name] = $true
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*" -or $row.source_truth -notlike "*ui_main_menu and Batch106 context*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1 plus main-menu context"
    }
    if ($row.source_truth -notlike "*no IAd character-body claim*" -or $row.source_truth -notlike "*no HDo/FoW9 archive claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve source-boundary wording"
    }
    if ($row.risk -ne "safe_symbolic_main_menu_ui") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_main_menu_scene_backplates") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
    if ($row.acceptance_gate -notlike "*contact_sheet_review*" -or $row.acceptance_gate -notlike "*production_qa*" -or $row.acceptance_gate -notlike "*engine_screenshot*" -or $row.acceptance_gate -notlike "*clean_console*") {
        Add-Failure "Asset table row $($row.semantic_name) missing required gates"
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
    if ($row.path -notlike "design/development/asset_candidates/ui/main_menu_scene_backplates/*") {
        Add-Failure "Manifest row $($row.asset_id) path is not workspace-relative design path: $($row.path)"
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
    if ($sprite.Name.Length -gt 72) {
        Add-Failure "Sprite filename is too long for safe import review: $($sprite.Name)"
    }
}

$variantImages = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*.png" -Recurse -File)
$variantManifests = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*manifest.csv" -Recurse -File)
if ($variantImages.Count -ne 45 -or $variantManifests.Count -ne 9) {
    Add-Failure "review variants should contain 45 PNGs and 9 manifests, found pngs=$($variantImages.Count) manifests=$($variantManifests.Count)"
}
if ((@($variantImages + $variantManifests) | ForEach-Object { $_.FullName.Length } | Measure-Object -Maximum).Maximum -gt 245) {
    Add-Failure "active review variant paths exceed 245 characters"
}

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_123_main_menu_scene_backplates_2026-06-26_final_review.csv"))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Final review should contain 9 rows, found $($reviewRows.Count)"
}
$pendingCount = @($reviewRows | Where-Object { $_.review_decision -eq "pending_review" }).Count
$rejectCount = @($reviewRows | Where-Object { $_.review_decision -like "reject*" }).Count
if ($pendingCount -ne 0 -or $rejectCount -ne 0) {
    Add-Failure "Final review must have no pending or rejected rows, got pending=$pendingCount reject=$rejectCount"
}
foreach ($row in $reviewRows) {
    if ($manifestIdsByName[$row.semantic_name] -ne $row.asset_id) {
        Add-Failure "Final review asset id does not match manifest for $($row.semantic_name)"
    }
    if ($row.unity_gate -notlike "*main_menu_screenshots*" -or $row.unity_gate -notlike "*192x108_96x64_live_readability*" -or $row.unity_gate -notlike "*no_recursive_candidate_import*" -or $row.unity_gate -notlike "*clean_console*") {
        Add-Failure "Final review row $($row.semantic_name) missing Unity main-menu gate"
    }
    if ($row.ppu -ne "100") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected ppu: $($row.ppu)"
    }
    if ($row.pivot -ne "center") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)"
    }
    if ($row.sorting_layer -ne "UI") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/thecat_batch_123_main_menu_scene_backplates_2026-06-26_process_note.md" @(
    'built-in Codex',
    'imagegen',
    'imagegen/image2 constraint',
    '#f802f7',
    '967488/1572516',
    '23519/1572516',
    '45 PNGs plus 9',
    'Generic candidate-pack validator: passed',
    'No runtime import was performed'
)

Assert-Contains $reviewNoteRelative @(
    'PASS_WITH_P2',
    'candidate_complete_pending_unity_review',
    'candidate_keep',
    'candidate_conditional',
    'main_menu_screenshots',
    'clean Console',
    'No runtime import was performed'
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH123_MAIN_MENU_SCENE_BACKPLATES_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH123_MAIN_MENU_SCENE_BACKPLATES_AGENT_REVIEW_2026-06-26.md"
$candidateReview = Resolve-ProjectPath "$batchDirRelative/thecat_batch_123_main_menu_scene_backplates_2026-06-26_candidate_review.md"
$reviewMirror = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $candidateReview) -and (Test-Path -LiteralPath $reviewMirror)) {
    $hashes = @(
        (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash,
        (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash,
        (Get-FileHash -Algorithm SHA256 -LiteralPath $candidateReview).Hash,
        (Get-FileHash -Algorithm SHA256 -LiteralPath $reviewMirror).Hash
    ) | Select-Object -Unique
    if ($hashes.Count -ne 1) {
        Add-Failure "Batch 123 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 123 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch123*" -or
    $_.Name -like "*main_menu_scene_backplate*" -or
    $_.Name -like "*tc_ui_menu_backplate*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 123 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 123 main-menu scene backplate validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 123 main-menu scene backplate validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/tc_ui_menu_backplate_batch123_semantic_manifest.csv"
