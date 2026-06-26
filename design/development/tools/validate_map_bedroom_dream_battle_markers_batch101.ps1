Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_101_bedroom_dream_battle_markers_imagegen_2026-06-25"
$batchDirRelative = "design/development/asset_candidates/map/bedroom_dream_battle_markers/$batchSlug"
$batchDir = Join-Path $projectRoot ($batchDirRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)

$sourceTruth = "Qr1 UI/style authority revision 816; Qr1 P0 bedroom dream map requirement; Batch54/67 bedroom interaction context"
$sourceRelative = "$batchDirRelative/source/thecat_map_bedroom_battle_markers_batch101_chromakey_source_v001.png"
$alphaRelative = "$batchDirRelative/source/thecat_map_bedroom_battle_markers_batch101_alpha_sheet_v001.png"
$namesRelative = "$batchDirRelative/source/thecat_map_bedmark_batch101_names.txt"
$assetTableRelative = "$batchDirRelative/thecat_map_bedroom_battle_markers_batch101_asset_table.csv"
$manifestRelative = "$batchDirRelative/thecat_map_bedmark_batch101_semantic_manifest.csv"
$contactSheetRelative = "$batchDirRelative/thecat_map_bedmark_batch101_semantic_contact_sheet_v001.png"
$readabilityBoardRelative = "$batchDirRelative/thecat_map_bedmark_batch101_64px_bedroom_map_readability_board_v001.png"
$finalReviewRelative = "$batchDirRelative/thecat_map_bedmark_batch101_final_review.csv"
$processNoteRelative = "$batchDirRelative/thecat_map_bedmark_batch101_process_note.md"
$reviewNoteRelative = "$batchDirRelative/reviews/BATCH101_BEDROOM_DREAM_BATTLE_MARKERS_AGENT_REVIEW_2026-06-25.md"
$mirroredReviewNoteRelative = "design/development/asset_review/BATCH101_BEDROOM_DREAM_BATTLE_MARKERS_AGENT_REVIEW_2026-06-25.md"

$expectedSemantics = @(
    "bedroom_bed_defense_anchor",
    "bedroom_enemy_entry_north_gate",
    "bedroom_enemy_entry_east_gate",
    "bedroom_enemy_entry_south_gate",
    "bedroom_enemy_entry_west_gate",
    "bedroom_safe_zone_boundary_ring",
    "bedroom_path_arrow_marker",
    "bedroom_obstacle_blocker_marker",
    "bedroom_spawn_warning_marker"
)

$conditionalSemantics = @(
    "bedroom_safe_zone_boundary_ring",
    "bedroom_path_arrow_marker",
    "bedroom_spawn_warning_marker"
)

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function Resolve-ProjectPath {
    param([string]$RelativePath)

    if ([string]::IsNullOrWhiteSpace($RelativePath)) {
        return $null
    }

    if ([System.IO.Path]::IsPathRooted($RelativePath)) {
        Add-Failure "Path must be project-relative: $RelativePath"
        return $null
    }

    $full = [System.IO.Path]::GetFullPath((Join-Path $projectRoot ($RelativePath -replace "/", [System.IO.Path]::DirectorySeparatorChar)))
    $rootFull = [System.IO.Path]::GetFullPath($projectRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar)
    $rootPrefix = $rootFull + [System.IO.Path]::DirectorySeparatorChar
    if (-not ($full.StartsWith($rootPrefix, [System.StringComparison]::OrdinalIgnoreCase) -or $full -eq $rootFull)) {
        Add-Failure "Path escapes project root: $RelativePath"
        return $null
    }

    return $full
}

function Test-RequiredFile {
    param([string]$RelativePath)

    $path = Resolve-ProjectPath $RelativePath
    if ($null -eq $path) {
        return $null
    }

    if (-not (Test-Path -LiteralPath $path)) {
        Add-Failure "Missing required Batch 101 file: $RelativePath"
        return $null
    }

    return $path
}

function Test-Hash {
    param(
        [string]$Path,
        [string]$ExpectedHash,
        [string]$Label
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        Add-Failure "$Label missing: $Path"
        return
    }

    $actual = (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
    if ($actual -ne $ExpectedHash.ToLowerInvariant()) {
        Add-Failure "$Label hash mismatch. Expected $ExpectedHash but found $actual"
    }
}

function Get-ImageSize {
    param([string]$Path)

    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        return "$($image.Width)x$($image.Height)"
    } finally {
        $image.Dispose()
    }
}

function Get-AlphaExtrema {
    param([string]$Path)

    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $min = 255
        $max = 0
        for ($y = 0; $y -lt $bitmap.Height; $y++) {
            for ($x = 0; $x -lt $bitmap.Width; $x++) {
                $alpha = $bitmap.GetPixel($x, $y).A
                if ($alpha -lt $min) { $min = $alpha }
                if ($alpha -gt $max) { $max = $alpha }
            }
        }
        return "($min, $max)"
    } finally {
        $bitmap.Dispose()
    }
}

function Test-TransparentOuterEdges {
    param([string]$Path)

    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $maxEdgeAlpha = 0
        for ($x = 0; $x -lt $bitmap.Width; $x++) {
            foreach ($y in @(0, ($bitmap.Height - 1))) {
                $alpha = $bitmap.GetPixel($x, $y).A
                if ($alpha -gt $maxEdgeAlpha) { $maxEdgeAlpha = $alpha }
            }
        }
        for ($y = 0; $y -lt $bitmap.Height; $y++) {
            foreach ($x in @(0, ($bitmap.Width - 1))) {
                $alpha = $bitmap.GetPixel($x, $y).A
                if ($alpha -gt $maxEdgeAlpha) { $maxEdgeAlpha = $alpha }
            }
        }

        if ($maxEdgeAlpha -gt 0) {
            Add-Failure "$Path should have fully transparent outer edges but max edge alpha is $maxEdgeAlpha"
        }
    } finally {
        $bitmap.Dispose()
    }
}

foreach ($required in @(
        $sourceRelative,
        $alphaRelative,
        $namesRelative,
        $assetTableRelative,
        $manifestRelative,
        $contactSheetRelative,
        $readabilityBoardRelative,
        $finalReviewRelative,
        $processNoteRelative,
        $reviewNoteRelative,
        $mirroredReviewNoteRelative
    )) {
    [void](Test-RequiredFile $required)
}

if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 101 directory is missing: $batchDirRelative"
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath $assetTableRelative))
if ($assetRows.Count -ne 9) {
    Add-Failure "Expected 9 asset-table rows but found $($assetRows.Count)"
}

foreach ($semantic in $expectedSemantics) {
    $assetMatch = @($assetRows | Where-Object { $_.semantic_name -eq $semantic })
    if ($assetMatch.Count -ne 1) {
        Add-Failure "Expected exactly one asset-table row for $semantic but found $($assetMatch.Count)"
        continue
    }

    $row = $assetMatch[0]
    if ($row.source_truth -ne $sourceTruth) {
        Add-Failure "$semantic source_truth mismatch. Found: $($row.source_truth)"
    }
    if ($row.source_truth -match "IAd|MDr|IZp|HDo|FoW9|character|enemy body|map archive") {
        Add-Failure "$semantic source_truth includes a blocked or unsupported source: $($row.source_truth)"
    }
    if ($row.risk -ne "safe") {
        Add-Failure "$semantic risk should be safe"
    }
    if ($row.target_use -ne "candidate_only_bedroom_battle_map_marker_import_test") {
        Add-Failure "$semantic target_use mismatch: $($row.target_use)"
    }
}

$nameLines = @(Get-Content -LiteralPath (Resolve-ProjectPath $namesRelative) | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
if (($nameLines -join "|") -ne ($expectedSemantics -join "|")) {
    Add-Failure "Names file does not match expected semantic order"
}

$manifestRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath $manifestRelative))
if ($manifestRows.Count -ne 9) {
    Add-Failure "Expected 9 manifest rows but found $($manifestRows.Count)"
}

foreach ($semantic in $expectedSemantics) {
    $manifestMatch = @($manifestRows | Where-Object { $_.semantic_name -eq $semantic })
    if ($manifestMatch.Count -ne 1) {
        Add-Failure "Expected exactly one manifest row for $semantic but found $($manifestMatch.Count)"
        continue
    }

    $row = $manifestMatch[0]
    $assetTableId = (@($assetRows | Where-Object { $_.semantic_name -eq $semantic })[0]).asset_id
    if ($assetTableId -ne $row.asset_id) {
        Add-Failure "$semantic asset table id must match manifest asset_id. Asset table $assetTableId, manifest $($row.asset_id)"
    }
    if ($row.batch -ne "batch101") {
        Add-Failure "$semantic manifest batch should be batch101"
    }
    if ($row.source -ne "alpha_sheet_projection_split") {
        Add-Failure "$semantic source should be alpha_sheet_projection_split"
    }
    if ($row.status -ne "ok") {
        Add-Failure "$semantic status should be ok"
    }
    if ($row.path -notlike "$batchDirRelative/semantic_sprites/*") {
        Add-Failure "$semantic path should stay under semantic_sprites: $($row.path)"
    }
    if ($row.path -like "Assets/*") {
        Add-Failure "$semantic path must not be under Assets: $($row.path)"
    }

    $candidatePath = Resolve-ProjectPath $row.path
    if ($null -eq $candidatePath -or -not (Test-Path -LiteralPath $candidatePath)) {
        Add-Failure "$semantic candidate PNG missing: $($row.path)"
        continue
    }

    Test-Hash $candidatePath $row.sha256 "$semantic candidate"

    $actualSize = Get-ImageSize $candidatePath
    $manifestSize = "$($row.width)x$($row.height)"
    if ($actualSize -ne $manifestSize) {
        Add-Failure "$semantic image size mismatch. Manifest $manifestSize, actual $actualSize"
    }

    $actualAlpha = Get-AlphaExtrema $candidatePath
    if ($actualAlpha -ne $row.alpha_extrema) {
        Add-Failure "$semantic alpha extrema mismatch. Manifest $($row.alpha_extrema), actual $actualAlpha"
    }
    if ($actualAlpha -ne "(0, 255)") {
        Add-Failure "$semantic should preserve full transparent/opaque alpha range"
    }

    Test-TransparentOuterEdges $candidatePath
}

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath $finalReviewRelative))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Expected 9 final-review rows but found $($reviewRows.Count)"
}

foreach ($semantic in $expectedSemantics) {
    $reviewMatch = @($reviewRows | Where-Object { $_.semantic_name -eq $semantic })
    if ($reviewMatch.Count -ne 1) {
        Add-Failure "Expected exactly one final-review row for $semantic but found $($reviewMatch.Count)"
        continue
    }

    $row = $reviewMatch[0]
    $expectedDecision = if ($conditionalSemantics -contains $semantic) { "candidate_conditional" } else { "candidate_keep" }
    if ($row.review_decision -ne $expectedDecision) {
        Add-Failure "$semantic review_decision should be $expectedDecision"
    }
    if ($row.unity_gate -ne "bedroom_battle_screenshot; import_settings; binding_proof; clean_console") {
        Add-Failure "$semantic unity_gate mismatch: $($row.unity_gate)"
    }
    if (-not [string]::IsNullOrWhiteSpace($row.target_runtime_path)) {
        Add-Failure "$semantic target_runtime_path must stay blank before formal Unity approval"
    }
    if ($row.sorting_layer -ne "MapOverlay") {
        Add-Failure "$semantic sorting_layer should be MapOverlay"
    }
    if ($row.collider_policy -ne "none_visual_marker_runtime_trigger_owned_by_scene") {
        Add-Failure "$semantic collider policy mismatch: $($row.collider_policy)"
    }
}

$semanticSpriteDir = Join-Path $batchDir "semantic_sprites"
$semanticPngs = @(Get-ChildItem -LiteralPath $semanticSpriteDir -File -Filter *.png)
if ($semanticPngs.Count -ne 9) {
    Add-Failure "Expected 9 semantic sprite PNGs but found $($semanticPngs.Count)"
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File -Filter *.meta)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 101 candidate directory must not contain Unity .meta files"
}

$assetFiles = @(
    Get-ChildItem -LiteralPath (Join-Path $projectRoot "Assets") -Recurse -File -ErrorAction SilentlyContinue |
        Where-Object {
            $_.Name -like "*bedmark*batch101*" -or
            $_.Name -like "*bedroom_battle*batch101*" -or
            $_.FullName -like "*bedroom_dream_battle_markers*batch_101*"
        }
)
if ($assetFiles.Count -gt 0) {
    Add-Failure "Batch 101 must not write runtime Assets files before approval"
}

$processPath = Resolve-ProjectPath $processNoteRelative
if ($null -ne $processPath -and (Test-Path -LiteralPath $processPath)) {
    $processText = Get-Content -LiteralPath $processPath -Raw
    foreach ($token in @("built-in Codex imagegen", "magenta chroma-key", "candidate_only_pending_unity_bedroom_battle_marker_review", "No Unity import was performed", "No character bodies")) {
        if (-not $processText.Contains($token)) {
            Add-Failure "Process note missing token: $token"
        }
    }
}

$reviewPath = Resolve-ProjectPath $reviewNoteRelative
$mirroredPath = Resolve-ProjectPath $mirroredReviewNoteRelative
if ($null -ne $reviewPath -and $null -ne $mirroredPath -and (Test-Path -LiteralPath $reviewPath) -and (Test-Path -LiteralPath $mirroredPath)) {
    $reviewText = Get-Content -LiteralPath $reviewPath -Raw
    foreach ($token in @("PASS_WITH_P2", "Visual/style", "Source-lock", "Production QA", "No character or animation content")) {
        if (-not $reviewText.Contains($token)) {
            Add-Failure "Review note missing token: $token"
        }
    }
    $mirroredReviewText = Get-Content -LiteralPath $mirroredPath -Raw
    if ($mirroredReviewText -ne $reviewText) {
        Add-Failure "Mirrored Batch 101 review note differs from candidate-package review note"
    }
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 101 bedroom dream battle marker validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 101 bedroom dream battle marker validation passed. Rows: $($manifestRows.Count); Manifest: $manifestRelative"
