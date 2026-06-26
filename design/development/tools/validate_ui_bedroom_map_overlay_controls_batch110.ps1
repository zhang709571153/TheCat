param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_110_bedroom_map_overlay_controls_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/ui/bedroom_map_overlay_controls/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH110_BEDROOM_MAP_OVERLAY_CONTROLS_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 110 file: $Relative"
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
    "$batchDirRelative/source/thecat_ui_mapovr_batch110_chromakey_source_v001.png",
    "$batchDirRelative/source/thecat_ui_mapovr_batch110_names.txt",
    "$batchDirRelative/alpha/thecat_ui_mapovr_batch110_alpha_sheet_v001.png",
    "$batchDirRelative/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_asset_table.csv",
    "$batchDirRelative/thecat_ui_mapovr_batch110_semantic_manifest.csv",
    "$batchDirRelative/thecat_ui_mapovr_batch110_semantic_contact_sheet_v001.png",
    "$batchDirRelative/thecat_ui_mapovr_batch110_64px_bedroom_map_overlay_readability_board_v001.png",
    "$batchDirRelative/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_final_review.csv",
    "$batchDirRelative/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_process_note.md",
    "$batchDirRelative/reviews/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_agent_review_prompt.md",
    "$batchDirRelative/BATCH110_BEDROOM_MAP_OVERLAY_CONTROLS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH110_BEDROOM_MAP_OVERLAY_CONTROLS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_candidate_review.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 110 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "map_legend_entry_token",
    "map_legend_path_token",
    "map_legend_obstacle_token",
    "map_legend_safe_zone_token",
    "map_legend_hazard_token",
    "map_overlay_pin_selected",
    "map_overlay_pin_available",
    "map_overlay_pin_locked",
    "map_overlay_info_foldout_frame"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/thecat_ui_mapovr_batch110_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 110 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_ui_mapovr_batch110_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

$assetSemantics = @{}
foreach ($row in $assetRows) {
    $assetSemantics[$row.semantic_name] = $true
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1 revision 816"
    }
    if ($row.source_truth -notlike "*Qr1 P0 bedroom dream map boundary*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1 bedroom map boundary"
    }
    if ($row.source_truth -notlike "*no IAd character-body claim*" -or $row.source_truth -notlike "*no HDo/FoW9 map-archive claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve source-boundary wording"
    }
    if ($row.risk -ne "safe_symbolic_non_character_ui_map_overlay") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_bedroom_map_overlay_controls") {
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
    if ($row.path -notlike "design/development/asset_candidates/ui/bedroom_map_overlay_controls/*") {
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
foreach ($sprite in $spriteFiles) {
    if ($sprite.Name.Length -gt 104) {
        Add-Failure "Sprite filename is too long for safe import review: $($sprite.Name)"
    }
}

$variantFiles = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -File)
if ($variantFiles.Count -ne 54) {
    Add-Failure "review variants should contain exactly 54 files, found $($variantFiles.Count)"
}
$infoFoldoutShort = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -File | Where-Object { $_.Name -like "tc_ui_mapovr_info_foldout_batch110_v001*" })
if ($infoFoldoutShort.Count -ne 6) {
    Add-Failure "info foldout short-prefix review variants should contain 6 files, found $($infoFoldoutShort.Count)"
}
$duplicatePreview = Resolve-ProjectPath "$batchDirRelative/superseded/path_length_duplicate_variant/info_foldout_duplicate_alpha_preview_512.png"
if (-not (Test-Path -LiteralPath $duplicatePreview)) {
    Add-Failure "Expected path-length duplicate preview to be preserved in superseded/"
}

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_final_review.csv"))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Final review should contain 9 rows, found $($reviewRows.Count)"
}
$keepCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_keep" }).Count
$conditionalCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_conditional" }).Count
$pendingCount = @($reviewRows | Where-Object { $_.review_decision -eq "pending_review" }).Count
if ($keepCount -ne 7 -or $conditionalCount -ne 2 -or $pendingCount -ne 0) {
    Add-Failure "Expected final review counts keep=7 conditional=2 pending=0, got keep=$keepCount conditional=$conditionalCount pending=$pendingCount"
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
    if ($row.sorting_layer -ne "UI overlay") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "ui_raycast_target_manual_only") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_process_note.md" @(
    "built-in Codex imagegen",
    "Qr1 UI/style truth revision 816",
    "Auto-detected key color",
    "No runtime import was performed",
    "files in candidate package: 0",
    "leak",
    "PASS_WITH_P2",
    "superseded/path_length_duplicate_variant"
)

Assert-Contains $reviewNoteRelative @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "candidate_complete_pending_unity_review",
    "No runtime import was performed",
    "Kepler visual/style review",
    "Ptolemy source-boundary review",
    "Wegener production QA",
    "Entry-vs-available",
    "Clean Console"
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH110_BEDROOM_MAP_OVERLAY_CONTROLS_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH110_BEDROOM_MAP_OVERLAY_CONTROLS_AGENT_REVIEW_2026-06-26.md"
$candidateReview = Resolve-ProjectPath "$batchDirRelative/reviews/thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_candidate_review.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $candidateReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $candidateHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $candidateReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $candidateHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 110 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 110 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch110*" -or
    $_.Name -like "*bedroom_map_overlay_controls*" -or
    $_.Name -like "*thecat_ui_mapovr*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 110 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 110 bedroom map overlay controls validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 110 bedroom map overlay controls validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/thecat_ui_mapovr_batch110_semantic_manifest.csv"
