param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_119_bedroom_route_socket_markers_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/map/bedroom_route_socket_markers/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH119_BEDROOM_ROUTE_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 119 file: $Relative"
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
    "$batchDirRelative/source/thecat_map_socket_batch119_chromakey_source_v001.png",
    "$batchDirRelative/source/thecat_map_socket_batch119_names.txt",
    "$batchDirRelative/alpha/thecat_map_socket_batch119_alpha_sheet_v001.png",
    "$batchDirRelative/thecat_batch_119_bedroom_route_socket_markers_2026-06-26_asset_table.csv",
    "$batchDirRelative/thecat_map_sock_batch119_semantic_manifest.csv",
    "$batchDirRelative/thecat_map_sock_batch119_semantic_contact_sheet_v001.png",
    "$batchDirRelative/thecat_map_sock_batch119_96px_64px_bedroom_socket_readability_board_v001.png",
    "$batchDirRelative/thecat_batch_119_bedroom_route_socket_markers_2026-06-26_final_review.csv",
    "$batchDirRelative/thecat_batch_119_bedroom_route_socket_markers_2026-06-26_process_note.md",
    "$batchDirRelative/BATCH119_BEDROOM_ROUTE_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH119_BEDROOM_ROUTE_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 119 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "bed_defense_socket_frame",
    "north_entry_socket_marker",
    "east_entry_socket_marker",
    "south_entry_socket_marker",
    "west_entry_socket_marker",
    "safe_rest_socket_marker",
    "hazard_noise_socket_marker",
    "reward_pickup_socket_marker",
    "locked_unknown_socket_marker"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/thecat_map_socket_batch119_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 119 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_119_bedroom_route_socket_markers_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_sock_batch119_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

$assetSemantics = @{}
foreach ($row in $assetRows) {
    $assetSemantics[$row.semantic_name] = $true
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*" -or $row.source_truth -notlike "*local bedroom battle/dream map context*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1 plus bedroom map context"
    }
    if ($row.source_truth -notlike "*no HDo/FoW9 archive claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve HDo/FoW9 source-boundary wording"
    }
    if ($row.risk -ne "safe_symbolic_bedroom_route_socket_map") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_bedroom_route_socket_markers") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
    if ($row.acceptance_gate -notlike "*96px_64px_bedroom_map_readability*" -or $row.acceptance_gate -notlike "*no_recursive_candidate_import*" -or $row.acceptance_gate -notlike "*clean_console*") {
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
    if ($row.path -notlike "design/development/asset_candidates/map/bedroom_route_socket_markers/*") {
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
    if ($sprite.Name.Length -gt 104) {
        Add-Failure "Sprite filename is too long for safe import review: $($sprite.Name)"
    }
}

$activeFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File | Where-Object { $_.FullName -notlike "*\superseded\*" })
$maxActivePathLength = ($activeFiles | ForEach-Object { $_.FullName.Length } | Measure-Object -Maximum).Maximum
if ($maxActivePathLength -gt 240) {
    Add-Failure "Active Batch 119 package path exceeds 240 characters: $maxActivePathLength"
}

$variantImages = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*.png" -Recurse -File)
$variantManifests = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*manifest.csv" -Recurse -File)
if ($variantImages.Count -ne 45 -or $variantManifests.Count -ne 9) {
    Add-Failure "review variants should contain 45 PNGs and 9 manifests, found pngs=$($variantImages.Count) manifests=$($variantManifests.Count)"
}
if ((@($variantImages + $variantManifests) | ForEach-Object { $_.FullName.Length } | Measure-Object -Maximum).Maximum -gt 220) {
    Add-Failure "active review variant paths exceed 220 characters"
}

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_119_bedroom_route_socket_markers_2026-06-26_final_review.csv"))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Final review should contain 9 rows, found $($reviewRows.Count)"
}
$keepCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_keep" }).Count
$conditionalCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_conditional" }).Count
$pendingCount = @($reviewRows | Where-Object { $_.review_decision -eq "pending_review" }).Count
if ($keepCount -ne 5 -or $conditionalCount -ne 4 -or $pendingCount -ne 0) {
    Add-Failure "Expected final review counts keep=5 conditional=4 pending=0, got keep=$keepCount conditional=$conditionalCount pending=$pendingCount"
}

$expectedConditional = @("bed_defense_socket_frame", "safe_rest_socket_marker", "reward_pickup_socket_marker", "locked_unknown_socket_marker")
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
    if ($row.unity_gate -notlike "*bedroom_map_socket_screenshots*" -or $row.unity_gate -notlike "*clean_console*" -or $row.unity_gate -notlike "*no_recursive_candidate_import*") {
        Add-Failure "Final review row $($row.semantic_name) missing bedroom-map Unity gate"
    }
    if ($row.ppu -ne "100") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected ppu: $($row.ppu)"
    }
    if ($row.pivot -ne "bottom-center") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)"
    }
    if ($row.sorting_layer -ne "Map socket overlay") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none_visual_overlay_parent_hit_area") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/thecat_batch_119_bedroom_route_socket_markers_2026-06-26_process_note.md" @(
    'built-in Codex',
    'imagegen',
    'imagegen/image2 intent',
    '#f903f8',
    '853324/1572516',
    '51408/1572516',
    '45 PNGs plus 9 manifests',
    '5 candidate_keep',
    '4 candidate_conditional',
    'No runtime import was performed'
)

Assert-Contains $reviewNoteRelative @(
    'PASS_WITH_P2',
    'candidate_complete_pending_unity_review',
    'candidate_keep',
    'candidate_conditional',
    'bed_defense_socket_frame',
    'safe_rest_socket_marker',
    'reward_pickup_socket_marker',
    'locked_unknown_socket_marker',
    'Batch101',
    'Batch109_117',
    'Batch117',
    'No runtime import was performed',
    'clean Console'
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH119_BEDROOM_ROUTE_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH119_BEDROOM_ROUTE_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md"
$candidateReview = Resolve-ProjectPath "$batchDirRelative/reviews/thecat_batch_119_bedroom_route_socket_markers_2026-06-26_candidate_review.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $candidateReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $candidateHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $candidateReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $candidateHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 119 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 119 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch119*" -or
    $_.Name -like "*bedroom_route_socket*" -or
    $_.Name -like "*thecat_map_sock*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 119 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 119 bedroom route socket markers validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 119 bedroom route socket markers validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/thecat_map_sock_batch119_semantic_manifest.csv"
