param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_109_bedroom_entry_path_props_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/map/bedroom_entry_path_props/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH109_BEDROOM_ENTRY_PATH_PROPS_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 109 file: $Relative"
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
    "$batchDirRelative/source/thecat_map_path_batch109_chromakey_source_v001.png",
    "$batchDirRelative/source/thecat_map_path_batch109_names.txt",
    "$batchDirRelative/alpha/thecat_map_path_batch109_alpha_sheet_v001.png",
    "$batchDirRelative/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_asset_table.csv",
    "$batchDirRelative/thecat_map_path_batch109_semantic_manifest.csv",
    "$batchDirRelative/thecat_map_path_batch109_semantic_contact_sheet_v001.png",
    "$batchDirRelative/thecat_map_path_batch109_64px_bedroom_entry_path_readability_board_v001.png",
    "$batchDirRelative/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_final_review.csv",
    "$batchDirRelative/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_process_note.md",
    "$batchDirRelative/reviews/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_agent_review_prompt.md",
    "$batchDirRelative/reviews/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_candidate_review.md",
    "$batchDirRelative/BATCH109_BEDROOM_ENTRY_PATH_PROPS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH109_BEDROOM_ENTRY_PATH_PROPS_AGENT_REVIEW_2026-06-26.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 109 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "north_doorway_dream_arch_prop",
    "side_window_moonbeam_marker",
    "blanket_curtain_entry_prop",
    "floor_path_ribbon_curve",
    "sock_trail_path_marker",
    "alarm_clock_hazard_prop",
    "nightlight_safe_glow_marker",
    "toy_train_path_blocker",
    "wall_crack_entry_marker"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/thecat_map_path_batch109_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 109 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_path_batch109_semantic_manifest.csv"))
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
    if ($row.risk -ne "safe_symbolic_non_character_map_entry_path") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_bedroom_entry_path_props") {
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

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_final_review.csv"))
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
    if ($row.pivot -ne "bottom-center") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_process_note.md" @(
    "built-in Codex imagegen",
    "Qr1 UI/style truth revision 816",
    "No runtime import was performed",
    "No Unity `.meta` files",
    "candidate_keep",
    "candidate_conditional",
    "source_lock"
)

Assert-Contains $reviewNoteRelative @(
    "PASS_WITH_P2",
    "candidate_keep",
    "candidate_conditional",
    "candidate_complete_pending_unity_review",
    "No runtime import was performed",
    "Production QA",
    "Source-Lock",
    "Animation remains paused",
    "wall_crack"
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH109_BEDROOM_ENTRY_PATH_PROPS_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH109_BEDROOM_ENTRY_PATH_PROPS_AGENT_REVIEW_2026-06-26.md"
$candidateReview = Resolve-ProjectPath "$batchDirRelative/reviews/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_candidate_review.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $candidateReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $candidateHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $candidateReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $candidateHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 109 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 109 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch109*" -or
    $_.Name -like "*bedroom_entry_path_props*" -or
    $_.Name -like "*thecat_map_path*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 109 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 109 bedroom entry/path props validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 109 bedroom entry/path props validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/thecat_map_path_batch109_semantic_manifest.csv"
