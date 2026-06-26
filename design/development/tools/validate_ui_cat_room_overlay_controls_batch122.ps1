param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_122_cat_room_overlay_controls_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/ui/cat_room_overlay_controls/$batchSlug"
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
        Add-Failure "Missing required Batch 122 file: $Relative"
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

$assetTableRelative = "$batchDirRelative/thecat_batch_122_cat_room_overlay_controls_2026-06-26_asset_table.csv"
$manifestRelative = "$batchDirRelative/thecat_ui_catroom_overlay_batch122_semantic_manifest.csv"
$finalReviewRelative = "$batchDirRelative/thecat_batch_122_cat_room_overlay_controls_2026-06-26_final_review.csv"
$processNoteRelative = "$batchDirRelative/thecat_batch_122_cat_room_overlay_controls_2026-06-26_process_note.md"
$candidateReviewRelative = "$batchDirRelative/reviews/thecat_batch_122_cat_room_overlay_controls_2026-06-26_candidate_review.md"
$agentPromptRelative = "$batchDirRelative/reviews/thecat_batch_122_cat_room_overlay_controls_2026-06-26_agent_review_prompt.md"
$agentReviewFile = "BATCH122_CAT_ROOM_OVERLAY_CONTROLS_AGENT_REVIEW_2026-06-26.md"

$requiredFiles = @(
    "$batchDirRelative/source/thecat_ui_catroom_overlay_batch122_source_sheet_v001.png",
    "$batchDirRelative/alpha/thecat_ui_catroom_overlay_batch122_alpha_sheet_v001.png",
    "$batchDirRelative/batch122_names.txt",
    $assetTableRelative,
    $manifestRelative,
    "$batchDirRelative/thecat_ui_catroom_overlay_batch122_semantic_contact_sheet_v001.png",
    "$batchDirRelative/thecat_ui_catroom_overlay_batch122_96px_64px_readability_board_v001.png",
    $finalReviewRelative,
    $processNoteRelative,
    $candidateReviewRelative,
    $agentPromptRelative,
    "$batchDirRelative/$agentReviewFile",
    "$batchDirRelative/reviews/$agentReviewFile",
    "design/development/asset_review/$agentReviewFile"
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 122 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "bed_zone_focus_control",
    "feeder_zone_focus_control",
    "litter_zone_focus_control",
    "dream_gate_focus_control",
    "needs_attention_filter_toggle",
    "resource_need_filter_toggle",
    "interaction_range_overlay_toggle",
    "comfort_zone_overlay_toggle",
    "return_to_default_view_control"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/batch122_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 122 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath $assetTableRelative))
$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath $manifestRelative))
$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath $finalReviewRelative))

if ($assetRows.Count -ne 9) { Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)" }
if ($manifestRows.Count -ne 9) { Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)" }
if ($reviewRows.Count -ne 9) { Add-Failure "Final review should contain 9 rows, found $($reviewRows.Count)" }

$assetIdsByName = @{}
foreach ($row in $assetRows) {
    $assetIdsByName[$row.semantic_name] = $row.asset_id
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*" -or
        $row.source_truth -notlike "*Batch90 cat-room UI*" -or
        $row.source_truth -notlike "*Batch91/92/93 cat-room context*" -or
        $row.source_truth -notlike "*Batch121 locator/socket context*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite required cat-room UI/map context"
    }
    if ($row.source_truth -notlike "*no IAd live approval*" -or $row.source_truth -notlike "*no HDo/FoW9 archive claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve source-boundary wording"
    }
    if ($row.risk -ne "safe") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_review") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
    if ($row.acceptance_gate -notlike "*cat_room_overlay_screenshots*" -or
        $row.acceptance_gate -notlike "*96px_64px_live_contrast*" -or
        $row.acceptance_gate -notlike "*control_vs_marker_proof*" -or
        $row.acceptance_gate -notlike "*no_recursive_candidate_import*" -or
        $row.acceptance_gate -notlike "*clean_console*") {
        Add-Failure "Asset table row $($row.semantic_name) missing required acceptance gates"
    }
}

$manifestIdsByName = @{}
foreach ($row in $manifestRows) {
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in manifest: $($row.semantic_name)"
    }
    $manifestIdsByName[$row.semantic_name] = $row.asset_id
    if ($assetIdsByName[$row.semantic_name] -ne $row.asset_id) {
        Add-Failure "Asset table id does not match manifest for $($row.semantic_name)"
    }
    if ($row.alpha_extrema -ne "(0, 255)") {
        Add-Failure "Manifest row $($row.asset_id) does not have full alpha extrema"
    }
    if ($row.source -ne "built_in_imagegen_chromakey_magenta_projection_split") {
        Add-Failure "Manifest row $($row.asset_id) has unexpected source: $($row.source)"
    }
    if ($row.path -notlike "design/development/asset_candidates/ui/cat_room_overlay_controls/*") {
        Add-Failure "Manifest row $($row.asset_id) path is not a workspace-relative Batch 122 path: $($row.path)"
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

$variantImages = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*.png" -Recurse -File)
$variantManifests = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*manifest.csv" -Recurse -File)
if ($variantImages.Count -ne 45 -or $variantManifests.Count -ne 9) {
    Add-Failure "review variants should contain 45 PNGs and 9 manifests, found pngs=$($variantImages.Count) manifests=$($variantManifests.Count)"
}

foreach ($variantManifest in $variantManifests) {
    foreach ($variantRow in @(Import-Csv -LiteralPath $variantManifest.FullName)) {
        if (-not (Test-Path -LiteralPath $variantRow.source)) {
            Add-Failure "Variant manifest source path does not exist: $($variantRow.source)"
        }
        if (-not (Test-Path -LiteralPath $variantRow.variant_path)) {
            Add-Failure "Variant manifest output path does not exist: $($variantRow.variant_path)"
        }
        if ((Get-FileHash -Algorithm SHA256 -LiteralPath $variantRow.variant_path).Hash.ToLowerInvariant() -ne $variantRow.sha256.ToLowerInvariant()) {
            Add-Failure "Variant manifest hash mismatch: $($variantRow.variant_path)"
        }
    }
}

$keepCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_keep" }).Count
$conditionalCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_conditional" }).Count
$pendingCount = @($reviewRows | Where-Object { $_.review_decision -eq "pending_review" }).Count
if ($keepCount -ne 4 -or $conditionalCount -ne 5 -or $pendingCount -ne 0) {
    Add-Failure "Expected final review counts keep=4 conditional=5 pending=0, got keep=$keepCount conditional=$conditionalCount pending=$pendingCount"
}

$expectedKeep = @(
    "needs_attention_filter_toggle",
    "interaction_range_overlay_toggle",
    "comfort_zone_overlay_toggle",
    "return_to_default_view_control"
)
foreach ($name in $expectedKeep) {
    $row = $reviewRows | Where-Object { $_.semantic_name -eq $name } | Select-Object -First 1
    if ($null -eq $row -or $row.review_decision -ne "candidate_keep") {
        Add-Failure "Expected $name to be candidate_keep"
    }
}

foreach ($row in $reviewRows) {
    if ($manifestIdsByName[$row.semantic_name] -ne $row.asset_id) {
        Add-Failure "Final review asset id does not match manifest for $($row.semantic_name)"
    }
    if ($row.unity_gate -notlike "*cat_room_overlay_screenshots*" -or
        $row.unity_gate -notlike "*96px_64px_live_contrast*" -or
        $row.unity_gate -notlike "*control_vs_marker_proof*" -or
        $row.unity_gate -notlike "*no_recursive_candidate_import*" -or
        $row.unity_gate -notlike "*clean_console*") {
        Add-Failure "Final review row $($row.semantic_name) missing Unity cat-room overlay-control gates"
    }
    if ($row.ppu -ne "100") { Add-Failure "Final review row $($row.semantic_name) has unexpected ppu: $($row.ppu)" }
    if ($row.pivot -ne "center") { Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)" }
    if ($row.sorting_layer -ne "UI") { Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)" }
    if ($row.collider_policy -ne "none") { Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)" }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) { Add-Failure "Final review row $($row.semantic_name) is missing agent_notes" }
}

Assert-Contains $processNoteRelative @(
    "built-in Codex",
    "imagegen",
    "imagegen/image2",
    "#ed04f3",
    "815987 transparent pixels",
    "61803 partially transparent pixels",
    "45 review variant PNGs",
    "candidate_keep",
    "candidate_conditional",
    "No runtime import was performed"
)

Assert-Contains $candidateReviewRelative @(
    "candidate complete pending Unity review",
    "no character body",
    "96px/64px",
    "candidate_keep",
    "candidate_conditional",
    "Control-vs-marker proof"
)

Assert-Contains "$batchDirRelative/$agentReviewFile" @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "Control-vs-marker proof",
    "Human approval"
)

Assert-Contains "$batchDirRelative/reviews/$agentReviewFile" @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "Control-vs-marker proof",
    "Human approval"
)

Assert-Contains "design/development/asset_review/$agentReviewFile" @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "Control-vs-marker proof",
    "Human approval"
)

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 122 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch122*" -or $_.Name -like "*catroom_overlay*" -or $_.Name -like "*cat_room_overlay_controls*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 122 candidate files leaked into Assets: $($assetLeak[0].FullName)"
}

$activeFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File | Where-Object { $_.FullName -notlike "*\superseded\*" })
$maxActivePathLength = ($activeFiles | ForEach-Object { $_.FullName.Length } | Measure-Object -Maximum).Maximum
if ($maxActivePathLength -gt 245) {
    Add-Failure "Active Batch 122 package path exceeds 245 characters: $maxActivePathLength"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Error $failure
    }
    exit 1
}

Write-Output "Batch 122 cat-room overlay control validation passed. Rows: $($manifestRows.Count); Manifest: $manifestRelative"
