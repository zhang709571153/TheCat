param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_111_scene_preview_backplates_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/ui/scene_preview_backplates/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH111_SCENE_PREVIEW_BACKPLATES_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 111 file: $Relative"
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
    "$batchDirRelative/source/tc_ui_scene_prev_batch111_chromakey_source_v002.png",
    "$batchDirRelative/source/tc_ui_scene_prev_batch111_names.txt",
    "$batchDirRelative/alpha/tc_ui_scene_prev_batch111_alpha_sheet_v002.png",
    "$batchDirRelative/superseded/rejected_source/tc_ui_scene_prev_batch111_rejected_source_v001_paw_markers.png",
    "$batchDirRelative/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_asset_table.csv",
    "$batchDirRelative/tc_ui_scene_prev_batch111_semantic_manifest.csv",
    "$batchDirRelative/tc_ui_scene_prev_batch111_semantic_contact_sheet_v001.png",
    "$batchDirRelative/tc_ui_scene_prev_batch111_192x108_scene_preview_readability_board_v001.png",
    "$batchDirRelative/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_final_review.csv",
    "$batchDirRelative/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_process_note.md",
    "$batchDirRelative/reviews/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_agent_review_prompt.md",
    "$batchDirRelative/BATCH111_SCENE_PREVIEW_BACKPLATES_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH111_SCENE_PREVIEW_BACKPLATES_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_candidate_review.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 111 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "bedroom_preview_backplate",
    "cat_room_preview_backplate",
    "dream_route_preview_backplate",
    "battle_preview_backplate",
    "reward_preview_backplate",
    "shop_event_preview_backplate",
    "settings_preview_backplate",
    "locked_preview_backplate",
    "unknown_preview_backplate"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/tc_ui_scene_prev_batch111_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 111 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_ui_scene_prev_batch111_semantic_manifest.csv"))
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
    if ($row.source_truth -notlike "*Batch107 scene-selection token context*" -or $row.source_truth -notlike "*Batch110 bedroom map overlay context*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite local scene-selection/backplate context"
    }
    if ($row.source_truth -notlike "*no IAd character-body claim*" -or $row.source_truth -notlike "*no HDo/FoW9 map-archive claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve source-boundary wording"
    }
    if ($row.risk -ne "safe_symbolic_non_character_scene_preview_ui") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_scene_preview_backplates") {
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
    if ($row.path -notlike "design/development/asset_candidates/ui/scene_preview_backplates/*") {
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

$variantFiles = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -File)
if ($variantFiles.Count -ne 54) {
    Add-Failure "review variants should contain exactly 54 files, found $($variantFiles.Count)"
}
$variantPngs = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*.png" -File)
if ($variantPngs.Count -ne 45) {
    Add-Failure "review variants should contain exactly 45 PNGs, found $($variantPngs.Count)"
}
$variantManifests = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*_review_variants_manifest.csv" -File)
if ($variantManifests.Count -ne 9) {
    Add-Failure "review variants should contain exactly 9 variant manifests, found $($variantManifests.Count)"
}
foreach ($variantManifest in $variantManifests) {
    $variantRows = @(Import-Csv -LiteralPath $variantManifest.FullName)
    if ($variantRows.Count -ne 5) {
        Add-Failure "Variant manifest $($variantManifest.Name) should contain 5 rows, found $($variantRows.Count)"
    }
    foreach ($row in $variantRows) {
        foreach ($field in @("source", "variant_path")) {
            $value = $row.$field
            if ([System.IO.Path]::IsPathRooted($value) -or $value -like "*:\*") {
                Add-Failure "Variant manifest $($variantManifest.Name) uses absolute $field path: $value"
            }
            if ($value -like "*\*") {
                Add-Failure "Variant manifest $($variantManifest.Name) uses backslashes in $field path: $value"
            }
            if ($value -notlike "design/development/asset_candidates/ui/scene_preview_backplates/*") {
                Add-Failure "Variant manifest $($variantManifest.Name) has unexpected $field path root: $value"
            }
        }
        $variantPath = Resolve-ProjectPath $row.variant_path
        if (-not (Test-Path -LiteralPath $variantPath)) {
            Add-Failure "Missing variant path from manifest $($variantManifest.Name): $($row.variant_path)"
            continue
        }
        $hash = (Get-FileHash -Algorithm SHA256 -LiteralPath $variantPath).Hash.ToLowerInvariant()
        if ($hash -ne $row.sha256.ToLowerInvariant()) {
            Add-Failure "Variant hash mismatch for $($row.variant_path)"
        }
    }
}

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_final_review.csv"))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Final review should contain 9 rows, found $($reviewRows.Count)"
}
$keepCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_keep" }).Count
$conditionalCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_conditional" }).Count
$pendingCount = @($reviewRows | Where-Object { $_.review_decision -eq "pending_review" }).Count
if ($keepCount -ne 9 -or $conditionalCount -ne 0 -or $pendingCount -ne 0) {
    Add-Failure "Expected final review counts keep=9 conditional=0 pending=0, got keep=$keepCount conditional=$conditionalCount pending=$pendingCount"
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
    if ($row.sorting_layer -ne "UI scene card backplate") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none_ui_click_area_owned_by_parent") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_process_note.md" @(
    'built-in Codex imagegen',
    'v001 rejected',
    'paw markers',
    'v002 accepted',
    '#ef03e7',
    '326018/1572864',
    '17242/1572864',
    'No runtime import was performed',
    'PASS_WITH_P2',
    '0 .meta',
    '0 runtime Assets'
)

Assert-Contains $reviewNoteRelative @(
    'PASS_WITH_P2',
    'candidate_keep',
    'candidate_complete_pending_unity_review',
    'Noether visual/style review',
    'Bernoulli source-lock review',
    'Faraday production QA',
    'v001 rejected',
    'v002 accepted',
    'No runtime import was performed',
    'Clean Console'
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH111_SCENE_PREVIEW_BACKPLATES_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH111_SCENE_PREVIEW_BACKPLATES_AGENT_REVIEW_2026-06-26.md"
$candidateReview = Resolve-ProjectPath "$batchDirRelative/reviews/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_candidate_review.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $candidateReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $candidateHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $candidateReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $candidateHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 111 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 111 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch111*" -or
    $_.Name -like "*scene_preview_backplates*" -or
    $_.Name -like "*tc_ui_scene_prev*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 111 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 111 scene preview backplates validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 111 scene preview backplates validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/tc_ui_scene_prev_batch111_semantic_manifest.csv"
