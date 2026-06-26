param()

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_103_bedroom_dream_map_decals_rework_imagegen_2026-06-26"
$batchDirRelative = "design/development/asset_candidates/map/bedroom_dream_map_decals/$batchSlug"
$reviewNoteRelative = "design/development/asset_review/BATCH103_BEDROOM_DREAM_MAP_DECALS_REWORK_AGENT_REVIEW_2026-06-26.md"
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
        Add-Failure "Missing required Batch 103 file: $Relative"
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
    "$batchDirRelative/source/thecat_map_bedroom_dream_map_decals_rework_batch103_chromakey_source_v001.png",
    "$batchDirRelative/source/thecat_map_bedroom_dream_map_decals_rework_batch103_alpha_sheet_v001.png",
    "$batchDirRelative/source/thecat_map_beddec_rework_batch103_names.txt",
    "$batchDirRelative/thecat_map_bedroom_dream_map_decals_rework_batch103_asset_table.csv",
    "$batchDirRelative/thecat_map_beddec103_batch103_semantic_manifest.csv",
    "$batchDirRelative/thecat_map_beddec103_batch103_semantic_contact_sheet_v001.png",
    "$batchDirRelative/thecat_map_beddec103_batch103_64px_bedroom_map_readability_board_v001.png",
    "$batchDirRelative/thecat_map_beddec103_batch103_final_review.csv",
    "$batchDirRelative/thecat_map_beddec103_batch103_process_note.md",
    "$batchDirRelative/reviews/BATCH103_BEDROOM_DREAM_MAP_DECALS_REWORK_AGENT_REVIEW_2026-06-26.md",
    $reviewNoteRelative
)

foreach ($file in $requiredFiles) {
    Assert-Exists $file
}

$batchDir = Resolve-ProjectPath $batchDirRelative
if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 103 directory is missing: $batchDirRelative"
}

$expectedNames = @(
    "pillow_barricade_v002",
    "toy_block_obstacle_v002",
    "nightmare_puddle_v002",
    "bed_aura_floor_glow_v002"
)
$namesPath = Resolve-ProjectPath "$batchDirRelative/source/thecat_map_beddec_rework_batch103_names.txt"
if (Test-Path -LiteralPath $namesPath) {
    $actualNames = @(Get-Content -LiteralPath $namesPath | Where-Object { $_.Trim().Length -gt 0 })
    if (($actualNames -join "|") -ne ($expectedNames -join "|")) {
        Add-Failure "Names file does not match expected Batch 103 semantic order"
    }
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_bedroom_dream_map_decals_rework_batch103_asset_table.csv"))
if ($assetRows.Count -ne 4) {
    Add-Failure "Asset table should contain 4 rows, found $($assetRows.Count)"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_beddec103_batch103_semantic_manifest.csv"))
if ($manifestRows.Count -ne 4) {
    Add-Failure "Manifest should contain 4 rows, found $($manifestRows.Count)"
}

foreach ($row in $manifestRows) {
    if ($expectedNames -notcontains $row.semantic_name) {
        Add-Failure "Unexpected semantic_name in manifest: $($row.semantic_name)"
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
    if ($row.path -like "*superseded*") {
        Add-Failure "Manifest must not point at superseded artifacts: $($row.path)"
    }
}

$spriteFiles = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "$batchDirRelative/semantic_sprites") -Filter "*.png" -File)
if ($spriteFiles.Count -ne 4) {
    Add-Failure "semantic_sprites should contain exactly 4 current PNGs, found $($spriteFiles.Count)"
}
foreach ($sprite in $spriteFiles) {
    if ($sprite.Name.Length -gt 96) {
        Add-Failure "Sprite filename remains too long for safe import review: $($sprite.Name)"
    }
}

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_beddec103_batch103_final_review.csv"))
if ($reviewRows.Count -ne 4) {
    Add-Failure "Final review should contain 4 rows, found $($reviewRows.Count)"
}
$keepCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_keep" }).Count
$conditionalCount = @($reviewRows | Where-Object { $_.review_decision -eq "candidate_conditional" }).Count
$rejectCount = @($reviewRows | Where-Object { $_.review_decision -eq "reject_rework" }).Count
if ($keepCount -ne 1 -or $conditionalCount -ne 3 -or $rejectCount -ne 0) {
    Add-Failure "Expected final review counts keep=1 conditional=3 reject=0, got keep=$keepCount conditional=$conditionalCount reject=$rejectCount"
}

Assert-Contains "$batchDirRelative/thecat_map_beddec103_batch103_process_note.md" @(
    "built-in Codex",
    "imagegen",
    "Batch102 agent review reject/rework",
    "No runtime import was performed",
    "candidate-only",
    "path-length risk",
    "does not use or claim IAd character body approval or HDo/FoW9 map archive coverage"
)

Assert-Contains $reviewNoteRelative @(
    "PASS_WITH_P2",
    "Visual/style",
    "Source-lock",
    "Production QA",
    "No character or animation content",
    "candidate_keep",
    "candidate_conditional"
)

$localReview = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH103_BEDROOM_DREAM_MAP_DECALS_REWORK_AGENT_REVIEW_2026-06-26.md"
$mirroredReview = Resolve-ProjectPath $reviewNoteRelative
if ((Test-Path -LiteralPath $localReview) -and (Test-Path -LiteralPath $mirroredReview)) {
    $localHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $localReview).Hash
    $mirrorHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $mirroredReview).Hash
    if ($localHash -ne $mirrorHash) {
        Add-Failure "Mirrored Batch 103 review note differs from candidate review note"
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -Filter "*.meta" -File)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 103 candidate directory must not contain Unity .meta files"
}

$assetLeak = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath "Assets") -Recurse -File -ErrorAction SilentlyContinue | Where-Object {
    $_.Name -like "*beddec103*" -or
    $_.Name -like "*batch103*" -or
    $_.FullName -like "*bedroom_dream_map_decals_rework*"
})
if ($assetLeak.Count -gt 0) {
    Add-Failure "Batch 103 must not write runtime Assets files before approval"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 103 bedroom dream map decal rework validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 103 bedroom dream map decal rework validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/thecat_map_beddec103_batch103_semantic_manifest.csv"
