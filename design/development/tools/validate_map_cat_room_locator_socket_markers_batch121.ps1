param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_121_cat_room_locator_socket_markers_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/map/cat_room_locator_socket_markers/$batchSlug"
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
        Add-Failure "Missing required Batch 121 file: $Relative"
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

$assetTableRelative = "$batchDirRelative/thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_asset_table.csv"
$manifestRelative = "$batchDirRelative/thecat_map_catroom_socket_batch121_semantic_manifest.csv"
$finalReviewRelative = "$batchDirRelative/thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_final_review.csv"
$processNoteRelative = "$batchDirRelative/thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_process_note.md"

$requiredFiles = @(
    "$batchDirRelative/source/thecat_map_catroom_socket_batch121_source_sheet_v001.png",
    "$batchDirRelative/alpha/thecat_map_catroom_socket_batch121_alpha_sheet_v001.png",
    "$batchDirRelative/batch121_names.txt",
    $assetTableRelative,
    $manifestRelative,
    "$batchDirRelative/thecat_map_catroom_socket_batch121_semantic_contact_sheet_v001.png",
    "$batchDirRelative/thecat_map_catroom_socket_batch121_96px_64px_cat_room_readability_board_v001.png",
    $finalReviewRelative,
    $processNoteRelative,
    "$batchDirRelative/BATCH121_CAT_ROOM_LOCATOR_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_candidate_review.md",
    "$batchDirRelative/reviews/thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_agent_review_prompt.md",
    "$batchDirRelative/reviews/BATCH121_CAT_ROOM_LOCATOR_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md",
    "design/development/asset_review/BATCH121_CAT_ROOM_LOCATOR_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md"
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 121 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "bed_care_locator_socket",
    "feeder_meal_locator_socket",
    "litter_clean_locator_socket",
    "dream_portal_locator_socket",
    "rest_ready_floor_socket",
    "low_hunger_alert_floor_socket",
    "cleanup_needed_alert_floor_socket",
    "return_to_room_floor_socket",
    "locked_interaction_floor_socket"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/batch121_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 121 semantic order"
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
        $row.source_truth -notlike "*Batch91 cat-room elements*" -or
        $row.source_truth -notlike "*Batch92 home-room locator*" -or
        $row.source_truth -notlike "*Batch93 cat-room decals*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite required cat-room context"
    }
    if ($row.source_truth -notlike "*no IAd live approval*" -or $row.source_truth -notlike "*no HDo/FoW9 archive claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve source-boundary wording"
    }
    if ($row.risk -ne "safe_symbolic_map_marker") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_review") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
    if ($row.acceptance_gate -notlike "*cat_room_screenshot*" -or $row.acceptance_gate -notlike "*clean_console*") {
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
    if ($row.path -notlike "design/development/asset_candidates/map/cat_room_locator_socket_markers/*") {
        Add-Failure "Manifest row $($row.asset_id) path is not a workspace-relative Batch 121 path: $($row.path)"
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
        if ($variantRow.source -like "*thecat_map_catroom_socket_*_batch121_candidate_v001.png") {
            Add-Failure "Variant manifest still references superseded long source filename: $($variantManifest.FullName)"
        }
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
if ($keepCount -ne 3 -or $conditionalCount -ne 6 -or $pendingCount -ne 0) {
    Add-Failure "Expected final review counts keep=3 conditional=6 pending=0, got keep=$keepCount conditional=$conditionalCount pending=$pendingCount"
}

$expectedKeep = @("dream_portal_locator_socket", "return_to_room_floor_socket", "locked_interaction_floor_socket")
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
    if ($row.unity_gate -notlike "*cat_room_socket_screenshots*" -or
        $row.unity_gate -notlike "*96px_64px_live_contrast*" -or
        $row.unity_gate -notlike "*no_recursive_candidate_import*" -or
        $row.unity_gate -notlike "*clean_console*") {
        Add-Failure "Final review row $($row.semantic_name) missing Unity cat-room socket gates"
    }
    if ($row.ppu -ne "100") { Add-Failure "Final review row $($row.semantic_name) has unexpected ppu: $($row.ppu)" }
    if ($row.pivot -ne "center") { Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)" }
    if ($row.sorting_layer -ne "MapOverlay") { Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)" }
    if ($row.collider_policy -ne "none") { Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)" }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) { Add-Failure "Final review row $($row.semantic_name) is missing agent_notes" }
}

Assert-Contains $processNoteRelative @(
    "built-in Codex",
    "imagegen",
    "imagegen/image2",
    "#f803f8",
    "903871/1572516",
    "36059/1572516",
    "45 review variant PNGs",
    "candidate_keep",
    "candidate_conditional",
    "PASS_WITH_P2",
    "No runtime import was performed"
)

Assert-Contains "$batchDirRelative/BATCH121_CAT_ROOM_LOCATOR_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md" @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "Marker-vs-prop proof",
    "Explicit human approval"
)

Assert-Contains "design/development/asset_review/BATCH121_CAT_ROOM_LOCATOR_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md" @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "Marker-vs-prop proof",
    "Explicit human approval"
)

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 121 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch121*" -or $_.Name -like "*catroom_socket*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 121 candidate files leaked into Assets: $($assetLeak[0].FullName)"
}

$activeFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File | Where-Object { $_.FullName -notlike "*\superseded\*" })
$maxActivePathLength = ($activeFiles | ForEach-Object { $_.FullName.Length } | Measure-Object -Maximum).Maximum
if ($maxActivePathLength -gt 245) {
    Add-Failure "Active Batch 121 package path exceeds 245 characters: $maxActivePathLength"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Error $failure
    }
    exit 1
}

Write-Output "Batch 121 cat-room locator socket marker validation passed. Rows: $($manifestRows.Count); Manifest: $manifestRelative"
