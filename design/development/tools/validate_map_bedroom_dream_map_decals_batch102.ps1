Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_102_bedroom_dream_map_decals_imagegen_2026-06-25"
$batchDirRelative = "design/development/asset_candidates/map/bedroom_dream_map_decals/$batchSlug"
$batchDir = Join-Path $projectRoot ($batchDirRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)

$sourceTruth = "Qr1 UI/style authority revision 816; Qr1 P0 bedroom dream map requirement; Batch54/67 bedroom interaction context; Batch101 marker context"
$requiredFiles = @(
    "$batchDirRelative/source/thecat_map_bedroom_dream_map_decals_batch102_chromakey_source_v001.png",
    "$batchDirRelative/source/thecat_map_bedroom_dream_map_decals_batch102_alpha_sheet_v001.png",
    "$batchDirRelative/source/thecat_map_beddec_batch102_names.txt",
    "$batchDirRelative/thecat_map_bedroom_dream_map_decals_batch102_asset_table.csv",
    "$batchDirRelative/thecat_map_beddec_batch102_semantic_manifest.csv",
    "$batchDirRelative/thecat_map_beddec_batch102_semantic_contact_sheet_v001.png",
    "$batchDirRelative/thecat_map_beddec_batch102_64px_bedroom_map_readability_board_v001.png",
    "$batchDirRelative/thecat_map_beddec_batch102_final_review.csv",
    "$batchDirRelative/thecat_map_beddec_batch102_process_note.md",
    "$batchDirRelative/reviews/BATCH102_BEDROOM_DREAM_MAP_DECALS_AGENT_REVIEW_2026-06-25.md",
    "design/development/asset_review/BATCH102_BEDROOM_DREAM_MAP_DECALS_AGENT_REVIEW_2026-06-25.md"
)

$expectedSemantics = @(
    "bedroom_floor_dream_crack_small",
    "bedroom_floor_dream_crack_large",
    "bedroom_blanket_boundary_trim",
    "bedroom_pillow_barricade_decal",
    "bedroom_toy_block_soft_obstacle",
    "bedroom_moonlight_window_patch",
    "bedroom_dust_sparkle_cluster",
    "bedroom_nightmare_puddle_decal",
    "bedroom_bed_aura_floor_glow"
)

$conditionalSemantics = @(
    "bedroom_blanket_boundary_trim",
    "bedroom_dust_sparkle_cluster"
)

$rejectSemantics = @(
    "bedroom_pillow_barricade_decal",
    "bedroom_toy_block_soft_obstacle",
    "bedroom_nightmare_puddle_decal",
    "bedroom_bed_aura_floor_glow"
)

$failures = [System.Collections.Generic.List[string]]::new()

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
            Add-Failure "$Path should have transparent outer edges but max edge alpha is $maxEdgeAlpha"
        }
    } finally {
        $bitmap.Dispose()
    }
}

function Test-Hash {
    param(
        [string]$Path,
        [string]$ExpectedHash,
        [string]$Label
    )
    $actual = (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
    if ($actual -ne $ExpectedHash.ToLowerInvariant()) {
        Add-Failure "$Label hash mismatch. Expected $ExpectedHash but found $actual"
    }
}

foreach ($relative in $requiredFiles) {
    $path = Resolve-ProjectPath $relative
    if ($null -eq $path -or -not (Test-Path -LiteralPath $path)) {
        Add-Failure "Missing required Batch 102 file: $relative"
    }
}

if (-not (Test-Path -LiteralPath $batchDir)) {
    throw "Batch 102 directory is missing: $batchDirRelative"
}

$namesPath = Resolve-ProjectPath "$batchDirRelative/source/thecat_map_beddec_batch102_names.txt"
$nameLines = @(Get-Content -LiteralPath $namesPath | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
if (($nameLines -join "|") -ne ($expectedSemantics -join "|")) {
    Add-Failure "Names file does not match expected Batch 102 semantic order"
}

$assetRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_bedroom_dream_map_decals_batch102_asset_table.csv"))
if ($assetRows.Count -ne 9) {
    Add-Failure "Expected 9 asset table rows but found $($assetRows.Count)"
}

foreach ($semantic in $expectedSemantics) {
    $match = @($assetRows | Where-Object { $_.semantic_name -eq $semantic })
    if ($match.Count -ne 1) {
        Add-Failure "Expected one asset table row for $semantic but found $($match.Count)"
        continue
    }
    $row = $match[0]
    if ($row.source_truth -ne $sourceTruth) {
        Add-Failure "$semantic source_truth mismatch: $($row.source_truth)"
    }
    if ($row.source_truth -match "IAd|MDr|IZp|HDo|FoW9|character|enemy body|map archive") {
        Add-Failure "$semantic source_truth includes blocked or unsupported source: $($row.source_truth)"
    }
    if ($row.family -ne "map_decal") {
        Add-Failure "$semantic family should be map_decal"
    }
    if ($row.risk -ne "safe") {
        Add-Failure "$semantic risk should be safe"
    }
    if ($row.target_use -ne "candidate_only_bedroom_dream_map_decal_import_test") {
        Add-Failure "$semantic target_use mismatch: $($row.target_use)"
    }
}

$manifestPath = Resolve-ProjectPath "$batchDirRelative/thecat_map_beddec_batch102_semantic_manifest.csv"
$manifestRows = @(Import-Csv -LiteralPath $manifestPath)
if ($manifestRows.Count -ne 9) {
    Add-Failure "Expected 9 manifest rows but found $($manifestRows.Count)"
}

foreach ($semantic in $expectedSemantics) {
    $match = @($manifestRows | Where-Object { $_.semantic_name -eq $semantic })
    if ($match.Count -ne 1) {
        Add-Failure "Expected one manifest row for $semantic but found $($match.Count)"
        continue
    }
    $row = $match[0]
    if ($row.batch -ne "batch102") {
        Add-Failure "$semantic manifest batch should be batch102"
    }
    if ($row.source -ne "alpha_sheet_projection_split") {
        Add-Failure "$semantic manifest source should be alpha_sheet_projection_split"
    }
    if ($semantic -eq "bedroom_blanket_boundary_trim") {
        if ($row.status -ne "ok_v002_hardkey_selected") {
            Add-Failure "$semantic manifest status should be ok_v002_hardkey_selected"
        }
        if ($row.path -notlike "*candidate_v002.png") {
            Add-Failure "$semantic should point to candidate_v002 after hard-key salvage"
        }
    } elseif ($row.status -ne "ok") {
        Add-Failure "$semantic manifest status should be ok"
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
        Add-Failure "$semantic alpha mismatch. Manifest $($row.alpha_extrema), actual $actualAlpha"
    }
    if ($actualAlpha -ne "(0, 255)") {
        Add-Failure "$semantic should preserve full alpha range"
    }
    Test-TransparentOuterEdges $candidatePath
}

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_beddec_batch102_final_review.csv"))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Expected 9 final review rows but found $($reviewRows.Count)"
}

foreach ($semantic in $expectedSemantics) {
    $match = @($reviewRows | Where-Object { $_.semantic_name -eq $semantic })
    if ($match.Count -ne 1) {
        Add-Failure "Expected one final review row for $semantic but found $($match.Count)"
        continue
    }
    $row = $match[0]
    $expectedDecision = if ($rejectSemantics -contains $semantic) {
        "reject_rework"
    } elseif ($conditionalSemantics -contains $semantic) {
        "candidate_conditional"
    } else {
        "candidate_keep"
    }
    if ($row.review_decision -ne $expectedDecision) {
        Add-Failure "$semantic review_decision should be $expectedDecision"
    }
    if ($row.unity_gate -ne "bedroom_map_screenshot; import_settings; binding_proof; clean_console") {
        Add-Failure "$semantic unity_gate mismatch: $($row.unity_gate)"
    }
    if (-not [string]::IsNullOrWhiteSpace($row.target_runtime_path)) {
        Add-Failure "$semantic target_runtime_path must stay blank before formal Unity approval"
    }
    if ($row.collider_policy -ne "none_visual_decal_runtime_collision_owned_by_scene") {
        Add-Failure "$semantic collider policy mismatch: $($row.collider_policy)"
    }
}

$semanticPngs = @(Get-ChildItem -LiteralPath (Join-Path $batchDir "semantic_sprites") -File -Filter *.png)
if ($semanticPngs.Count -ne 9) {
    Add-Failure "Expected 9 semantic sprite PNGs but found $($semanticPngs.Count)"
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File -Filter *.meta)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 102 candidate directory must not contain Unity .meta files"
}

$assetFiles = @(
    Get-ChildItem -LiteralPath (Join-Path $projectRoot "Assets") -Recurse -File -ErrorAction SilentlyContinue |
        Where-Object {
            $_.Name -like "*beddec*batch102*" -or
            $_.Name -like "*bedroom_dream_map_decals*batch102*" -or
            $_.FullName -like "*bedroom_dream_map_decals*batch_102*"
        }
)
if ($assetFiles.Count -gt 0) {
    Add-Failure "Batch 102 must not write runtime Assets files before approval"
}

$processText = Get-Content -LiteralPath (Resolve-ProjectPath "$batchDirRelative/thecat_map_beddec_batch102_process_note.md") -Raw
foreach ($token in @('built-in Codex `imagegen`', 'magenta chroma-key', 'row-8 drift cleanup', 'v003 hard-key salvage', 'partial_candidate_only_pending_unity_bedroom_map_decal_review_with_rework_required', 'No Unity import was performed', 'No files were copied into `Assets/`')) {
    if (-not $processText.Contains($token)) {
        Add-Failure "Process note missing token: $token"
    }
}

$reviewPath = Resolve-ProjectPath "$batchDirRelative/reviews/BATCH102_BEDROOM_DREAM_MAP_DECALS_AGENT_REVIEW_2026-06-25.md"
$mirrorPath = Resolve-ProjectPath "design/development/asset_review/BATCH102_BEDROOM_DREAM_MAP_DECALS_AGENT_REVIEW_2026-06-25.md"
if ($null -ne $reviewPath -and $null -ne $mirrorPath -and (Test-Path -LiteralPath $reviewPath) -and (Test-Path -LiteralPath $mirrorPath)) {
    $reviewText = Get-Content -LiteralPath $reviewPath -Raw
    foreach ($token in @("PASS_WITH_P2", "Visual/style", "Source-lock", "Production QA", "No character or animation content")) {
        if (-not $reviewText.Contains($token)) {
            Add-Failure "Review note missing token: $token"
        }
    }
    $mirrorText = Get-Content -LiteralPath $mirrorPath -Raw
    if ($mirrorText -ne $reviewText) {
        Add-Failure "Mirrored Batch 102 review note differs from candidate review note"
    }
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 102 bedroom dream map decal validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 102 bedroom dream map decal validation passed. Rows: $($manifestRows.Count); Manifest: $batchDirRelative/thecat_map_beddec_batch102_semantic_manifest.csv"
