param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_116_character_roster_status_chips_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/ui/character_roster_status_chips/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH116_CHARACTER_ROSTER_STATUS_CHIPS_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 116 file: $Relative"
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
    "$batchDirRelative/source/tc_ui_roster_batch116_chromakey_source_v001.png",
    "$batchDirRelative/source/tc_ui_roster_batch116_names.txt",
    "$batchDirRelative/alpha/tc_ui_roster_batch116_alpha_sheet_v001.png",
    "$batchDirRelative/thecat_batch_116_character_roster_status_chips_2026-06-26_asset_table.csv",
    "$batchDirRelative/tc_ui_roster_batch116_semantic_manifest.csv",
    "$batchDirRelative/tc_ui_roster_batch116_semantic_contact_sheet_v001.png",
    "$batchDirRelative/tc_ui_roster_batch116_96px_64px_character_roster_readability_board_v001.png",
    "$batchDirRelative/thecat_batch_116_character_roster_status_chips_2026-06-26_final_review.csv",
    "$batchDirRelative/thecat_batch_116_character_roster_status_chips_2026-06-26_process_note.md",
    "$batchDirRelative/BATCH116_CHARACTER_ROSTER_STATUS_CHIPS_AGENT_REVIEW_2026-06-26.md",
    "$batchDirRelative/reviews/BATCH116_CHARACTER_ROSTER_STATUS_CHIPS_AGENT_REVIEW_2026-06-26.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 116 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "character_roster_ready_chip",
    "character_roster_locked_chip",
    "character_roster_new_chip",
    "character_roster_source_locked_chip",
    "character_roster_selected_ribbon",
    "character_roster_role_slot_chip",
    "character_roster_scene_affinity_chip",
    "character_roster_team_slot_chip",
    "character_roster_review_needed_chip"
)

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/tc_ui_roster_batch116_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 116 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_116_character_roster_status_chips_2026-06-26_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Asset table should contain 9 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/tc_ui_roster_batch116_semantic_manifest.csv"))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Manifest should contain 9 rows, found $($manifestRows.Count)"
}

$assetNames = @{}
foreach ($row in $assetRows) {
    $assetNames[$row.semantic_name] = $true
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in asset table: $($row.semantic_name)"
    }
    if ($row.source_truth -notlike "*Qr1 UI/style truth revision 816*" -or $row.source_truth -notlike "*Batch88/95/96/113/114 context*") {
        Add-Failure "Asset table row $($row.semantic_name) does not cite Qr1 plus Batch88/95/96/113/114 context"
    }
    if ($row.source_truth -notlike "*symbolic UI only no character body*") {
        Add-Failure "Asset table row $($row.semantic_name) does not preserve no-character-body boundary"
    }
    if ($row.risk -ne "safe_symbolic_non_character_roster_status_ui") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected risk: $($row.risk)"
    }
    if ($row.target_use -ne "candidate_only_character_roster_status_chips") {
        Add-Failure "Asset table row $($row.semantic_name) has unexpected target_use: $($row.target_use)"
    }
}

$manifestIdsByName = @{}
foreach ($row in $manifestRows) {
    $manifestIdsByName[$row.semantic_name] = $row.asset_id
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in manifest: $($row.semantic_name)"
    }
    if (-not $assetNames.ContainsKey($row.semantic_name)) {
        Add-Failure "Manifest semantic_name missing from asset table: $($row.semantic_name)"
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

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_batch_116_character_roster_status_chips_2026-06-26_final_review.csv"))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Final review should contain 9 rows, found $($reviewRows.Count)"
}
$keepCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_keep" }).Count
$conditionalCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_conditional" }).Count
$pendingCount = @($reviewRows | Where-Object { $_.review_decision -eq "pending_review" }).Count
if ($keepCount -ne 5 -or $conditionalCount -ne 4 -or $pendingCount -ne 0) {
    Add-Failure "Expected final review counts keep=5 conditional=4 pending=0, got keep=$keepCount conditional=$conditionalCount pending=$pendingCount"
}

$expectedConditional = @("character_roster_source_locked_chip", "character_roster_selected_ribbon", "character_roster_scene_affinity_chip", "character_roster_team_slot_chip")
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
    if ($row.unity_gate -notlike "*character_roster_ui_screenshots*" -or $row.unity_gate -notlike "*clean_console*") {
        Add-Failure "Final review row $($row.semantic_name) missing character-roster Unity gate"
    }
    if ($row.ppu -ne "100") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected ppu: $($row.ppu)"
    }
    if ($row.pivot -ne "center") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected pivot: $($row.pivot)"
    }
    if ($row.sorting_layer -ne "UI character roster status chip") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected sorting_layer: $($row.sorting_layer)"
    }
    if ($row.collider_policy -ne "none_ui_parent_hit_area") {
        Add-Failure "Final review row $($row.semantic_name) has unexpected collider_policy: $($row.collider_policy)"
    }
    if ([string]::IsNullOrWhiteSpace($row.agent_notes)) {
        Add-Failure "Final review row $($row.semantic_name) is missing agent_notes"
    }
}

Assert-Contains "$batchDirRelative/thecat_batch_116_character_roster_status_chips_2026-06-26_process_note.md" @(
    "built-in Codex imagegen",
    "imagegen skill",
    "does not expose a model selector",
    "#f803e2",
    "1054347/1572516",
    "279379/1572516",
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
    "character_roster_source_locked_chip",
    "character_roster_selected_ribbon",
    "character_roster_scene_affinity_chip",
    "character_roster_team_slot_chip",
    "Batch113/114",
    "No runtime import was performed",
    "clean Console"
)

$rootReview = Resolve-ProjectPath "$batchDirRelative/BATCH116_CHARACTER_ROSTER_STATUS_CHIPS_AGENT_REVIEW_2026-06-26.md"
$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH116_CHARACTER_ROSTER_STATUS_CHIPS_AGENT_REVIEW_2026-06-26.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $rootReview) -and (Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $rootHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $rootReview).Hash
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($rootHash -ne $localHash -or $rootHash -ne $mirrorHash) {
        Add-Failure "Batch 116 review-note mirrors differ"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 116 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*batch116*" -or
    $_.Name -like "*character_roster_status_chips*" -or
    $_.Name -like "*tc_ui_roster*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 116 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 116 character roster status chips validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 116 character roster status chips validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/tc_ui_roster_batch116_semantic_manifest.csv"
