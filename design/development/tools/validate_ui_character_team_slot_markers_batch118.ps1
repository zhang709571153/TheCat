param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_118_character_team_slot_markers_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/ui/character_team_slot_markers/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH118_CHARACTER_TEAM_SLOT_MARKERS_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 118 file: $Relative"
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
    "$batchDirRelative/source/thecat_ui_team_slot_batch118_chromakey_source_v001.png",
    "$batchDirRelative/source/thecat_ui_team_slot_batch118_names.txt",
    "$batchDirRelative/source/thecat_ui_team_slot_front_line_slot_marker_v002_chromakey_source.png",
    "$batchDirRelative/alpha/thecat_ui_team_slot_batch118_alpha_sheet_v001.png",
    "$batchDirRelative/alpha/thecat_ui_team_slot_front_line_slot_marker_v002_alpha.png",
    "$batchDirRelative/thecat_batch_118_character_team_slot_markers_2026-06-26_asset_table.csv",
    "$batchDirRelative/thecat_ui_team_slot_batch118_semantic_manifest.csv",
    "$batchDirRelative/thecat_ui_team_slot_batch118_semantic_contact_sheet_v002.png",
    "$batchDirRelative/thecat_ui_team_slot_batch118_96px_64px_dark_warm_readability_board_v002.png",
    "$batchDirRelative/thecat_batch_118_character_team_slot_markers_2026-06-26_final_review.csv",
    "$batchDirRelative/thecat_batch_118_character_team_slot_markers_2026-06-26_process_note.md",
    "$batchDirRelative/reviews/thecat_batch_118_character_team_slot_markers_2026-06-26_candidate_review.md"
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 118 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "front_line_slot_marker",
    "rear_support_slot_marker",
    "healer_slot_marker",
    "shield_guard_slot_marker",
    "burst_skill_slot_marker",
    "utility_control_slot_marker",
    "synergy_pair_link_marker",
    "reserve_bench_marker",
    "empty_locked_team_slot_marker"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/thecat_ui_team_slot_batch118_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 118 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_118_character_team_slot_markers_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_ui_team_slot_batch118_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

$assetSemantics = @{}
foreach ($row in $assetRows) {
    $assetSemantics[$row.semantic_name] = $true
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*" -or $row.source_truth -notlike "*IAd local source-lock boundary*" -or $row.source_truth -notlike "*Batch95/113/116 symbolic character UI context*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1, IAd local boundary, and Batch95/113/116 context"
    }
    if ($row.source_truth -notlike "*symbolic-only no character body art*" -or $row.source_truth -notlike "*no portrait/framesheet/identity replacement claim*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve symbolic-only character boundary"
    }
    if ($row.risk -ne "safe_symbolic_non_character_team_slot_ui") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_character_team_slot_markers") {
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
    if ($row.path -notlike "design/development/asset_candidates/ui/character_team_slot_markers/*") {
        Add-Failure "Manifest row $($row.asset_id) path is not workspace-relative Batch118 path: $($row.path)"
    }
    if ($row.semantic_name -eq "front_line_slot_marker") {
        if ($row.path -notlike "*candidate_v002.png") {
            Add-Failure "front_line_slot_marker must point to active candidate_v002 path"
        }
        if ($row.status -notlike "*v002*") {
            Add-Failure "front_line_slot_marker status should record v002 replacement"
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

$activeFrontV1 = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/semantic_sprites") -Filter "*front_line_slot_marker*candidate_v001.png" -File)
if ($activeFrontV1.Count -gt 0) {
    Add-Failure "front_line_slot_marker v001 must not remain in active semantic_sprites"
}
Assert-Exists "$batchDirRelative/superseded/front_line_v001/thecat_ui_team_slot_front_line_slot_marker_batch118_candidate_v001.png"

$variantImages = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*.png" -File)
$variantManifests = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/reviews/variants") -Filter "*manifest.csv" -File)
if ($variantImages.Count -ne 45 -or $variantManifests.Count -ne 9) {
    Add-Failure "review variants should contain 45 PNGs and 9 manifests, found pngs=$($variantImages.Count) manifests=$($variantManifests.Count)"
}

$maxVariantPathLength = 0
foreach ($file in @($variantImages + $variantManifests)) {
    if ($file.FullName.Length -gt $maxVariantPathLength) {
        $maxVariantPathLength = $file.FullName.Length
    }
}
if ($maxVariantPathLength -gt 220) {
    Add-Failure "Active review variant paths should stay below 221 characters after short-prefix rebuild, found max=$maxVariantPathLength"
}
Assert-Exists "$batchDirRelative/superseded/long_path_variants_v001/archive_map.csv"

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_118_character_team_slot_markers_2026-06-26_final_review.csv"))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Final review should contain 9 rows, found $($reviewRows.Count)"
}
$keepCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_keep" }).Count
$conditionalCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_conditional" }).Count
$pendingCount = @($reviewRows | Where-Object { $_.review_decision -eq "pending_review" }).Count
if ($keepCount -ne 3 -or $conditionalCount -ne 6 -or $pendingCount -ne 0) {
    Add-Failure "Expected final review counts keep=3 conditional=6 pending=0, got keep=$keepCount conditional=$conditionalCount pending=$pendingCount"
}

$expectedConditional = @(
    "front_line_slot_marker",
    "rear_support_slot_marker",
    "shield_guard_slot_marker",
    "synergy_pair_link_marker",
    "reserve_bench_marker",
    "empty_locked_team_slot_marker"
)
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
    if ($row.unity_gate -notlike "*character_team_ui_screenshots*" -or $row.unity_gate -notlike "*recttransform_binding*" -or $row.unity_gate -notlike "*no_recursive_candidate_import*" -or $row.unity_gate -notlike "*human_approval*" -or $row.unity_gate -notlike "*clean_console*") {
        Add-Failure "Final review row $($row.semantic_name) missing required Unity gate tokens"
    }
    if ($row.ppu -ne "100") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected ppu: $($row.ppu)"
    }
    if ($row.pivot -ne "center") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)"
    }
    if ($row.sorting_layer -ne "UI character team slot marker") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none_ui_parent_hit_area") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ([string]::IsNullOrWhiteSpace($row.target_runtime_path)) {
        Add-Failure "Final review row $($row.semantic_name) is missing target_runtime_path"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/thecat_batch_118_character_team_slot_markers_2026-06-26_process_note.md" @(
    "built-in Codex",
    "imagegen",
    "does not expose a model selector",
    "#ff00ff",
    "1045830/1572516",
    "17332/1572516",
    "front_line_slot_marker",
    "v001",
    "1064736/1572516",
    "5089/1572516",
    "candidate_v002",
    "5ef00deb5b006d56b2b924906a340ffccf37c1250449f1881df6e8fb070ced79",
    "Max active variant path length after short-prefix rebuild",
    "201",
    "3 candidate_keep",
    "6 candidate_conditional",
    "No runtime import was performed"
)

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 118 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch118*" -or
    $_.Name -like "*character_team_slot_markers*" -or
    $_.Name -like "*thecat_ui_team_slot*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 118 must not write runtime Assets files before approval"
}

if (Test-Path -LiteralPath (Resolve-ProjectPath $reviewNoteRelative)) {
    Assert-Contains $reviewNoteRelative @(
        "PASS_WITH_P2",
        "candidate_complete_pending_unity_review",
        "3 candidate_keep",
        "6 candidate_conditional",
        "front_line_slot_marker",
        "candidate_v002",
        "no_recursive_candidate_import",
        "human approval",
        "No runtime import was performed"
    )
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 118 character team slot markers validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 118 character team slot markers validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/thecat_ui_team_slot_batch118_semantic_manifest.csv"
