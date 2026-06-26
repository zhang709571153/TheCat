Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_98_dream_route_map_tokens_imagegen_2026-06-25"
$batchDirRelative = "design/development/asset_candidates/ui/dream_route_map_tokens/$batchSlug"
$batchDir = Join-Path $projectRoot ($batchDirRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)

$sourceRelative = "$batchDirRelative/source/thecat_ui_dream_route_map_tokens_batch98_chromakey_source_v001.png"
$alphaRelative = "$batchDirRelative/source/thecat_ui_dream_route_map_tokens_batch98_alpha_sheet_v001.png"
$namesRelative = "$batchDirRelative/source/thecat_ui_dream_route_map_tokens_batch98_names.txt"
$assetTableRelative = "$batchDirRelative/thecat_ui_dream_route_map_tokens_batch98_asset_table.csv"
$manifestRelative = "$batchDirRelative/thecat_ui_dream_route_map_token_batch98_semantic_manifest.csv"
$contactSheetRelative = "$batchDirRelative/thecat_ui_dream_route_map_token_batch98_semantic_contact_sheet_v001.png"
$readabilityBoardRelative = "$batchDirRelative/thecat_ui_dream_route_map_token_batch98_64px_readability_board_v001.png"
$finalReviewRelative = "$batchDirRelative/thecat_ui_dream_route_map_token_batch98_final_review.csv"
$processNoteRelative = "$batchDirRelative/thecat_ui_dream_route_map_token_batch98_process_note.md"
$reviewNoteRelative = "$batchDirRelative/reviews/BATCH98_DREAM_ROUTE_MAP_TOKENS_AGENT_REVIEW_2026-06-25.md"
$mirroredReviewNoteRelative = "design/development/asset_review/BATCH98_DREAM_ROUTE_MAP_TOKENS_AGENT_REVIEW_2026-06-25.md"

$expectedSemantics = @(
    "dream_route_current_node_marker",
    "dream_route_selectable_node_marker",
    "dream_route_locked_node_marker",
    "dream_route_available_path_connector",
    "dream_route_locked_path_connector",
    "dream_route_boss_gate_marker",
    "dream_route_reward_cache_marker",
    "dream_route_rest_nest_marker",
    "dream_route_event_mist_marker"
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

function Test-RequiredFile {
    param([string]$RelativePath)

    $path = Resolve-ProjectPath $RelativePath
    if ($null -eq $path) {
        return $null
    }

    if (-not (Test-Path -LiteralPath $path)) {
        Add-Failure "Missing required Batch 98 file: $RelativePath"
        return $null
    }

    return $path
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
    throw "Batch 98 directory is missing: $batchDirRelative"
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
    if ($row.source_truth -ne "Qr1 UI/style authority revision 816; Batch86 dream-route preflight") {
        Add-Failure "$semantic source_truth must remain narrowed to Qr1 plus Batch86. Found: $($row.source_truth)"
    }
    if ($row.source_truth -match "Batch65|Batch 65") {
        Add-Failure "$semantic source_truth must not cite Batch65 after source-lock P1 fix"
    }
    if ($row.risk -ne "safe") {
        Add-Failure "$semantic risk should be safe"
    }
    if ($row.target_use -ne "candidate_only_route_map_import_test") {
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

$reviewRows = @(Import-Csv -LiteralPath (Resolve-ProjectPath $finalReviewRelative))
if ($reviewRows.Count -ne 9) {
    Add-Failure "Expected 9 final-review rows but found $($reviewRows.Count)"
}

foreach ($semantic in $expectedSemantics) {
    $manifestMatch = @($manifestRows | Where-Object { $_.semantic_name -eq $semantic })
    if ($manifestMatch.Count -ne 1) {
        Add-Failure "Expected exactly one manifest row for $semantic but found $($manifestMatch.Count)"
        continue
    }

    $row = $manifestMatch[0]
    if ($row.batch -ne "batch98") {
        Add-Failure "$semantic manifest batch should be batch98"
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

foreach ($semantic in $expectedSemantics) {
    $reviewMatch = @($reviewRows | Where-Object { $_.semantic_name -eq $semantic })
    if ($reviewMatch.Count -ne 1) {
        Add-Failure "Expected exactly one final-review row for $semantic but found $($reviewMatch.Count)"
        continue
    }

    $row = $reviewMatch[0]
    $expectedDecision = if ($semantic -eq "dream_route_boss_gate_marker") { "candidate_conditional" } else { "candidate_keep" }
    if ($row.review_decision -ne $expectedDecision) {
        Add-Failure "$semantic review_decision should be $expectedDecision"
    }
    if ($row.unity_gate -ne "candidate_only_pending_unity_import_settings_route_map_screenshot_console") {
        Add-Failure "$semantic unity_gate mismatch: $($row.unity_gate)"
    }
    if (-not [string]::IsNullOrWhiteSpace($row.target_runtime_path)) {
        Add-Failure "$semantic target_runtime_path must stay blank before formal Unity approval"
    }
    if ($row.ppu -ne "100" -or $row.pivot -ne "center" -or $row.sorting_layer -ne "UI/RouteMap") {
        Add-Failure "$semantic import metadata mismatch"
    }
    if ($row.collider_policy -ne "none_ui_click_target_defined_by_button_or_node_controller") {
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
    Add-Failure "Batch 98 candidate directory must not contain Unity .meta files"
}

$processText = Get-Content -LiteralPath (Resolve-ProjectPath $processNoteRelative) -Raw
foreach ($token in @('built-in Codex', 'magenta chroma-key', 'Source-lock review: final', 'PASS_WITH_P2', 'candidate_only_pending_unity_import_settings_route_map_screenshot_console', '.meta` files in candidate package: 0')) {
    if (-not $processText.Contains($token)) {
        Add-Failure "Process note missing token: $token"
    }
}

$reviewText = Get-Content -LiteralPath (Resolve-ProjectPath $reviewNoteRelative) -Raw
foreach ($token in @('PASS_WITH_P2', 'Source-lock', 'Production QA', 'boss gate density', 'No sprite content violation')) {
    if (-not $reviewText.Contains($token)) {
        Add-Failure "Review note missing token: $token"
    }
}

$mirroredReviewText = Get-Content -LiteralPath (Resolve-ProjectPath $mirroredReviewNoteRelative) -Raw
if ($mirroredReviewText -ne $reviewText) {
    Add-Failure "Mirrored Batch 98 review note differs from candidate-package review note"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 98 dream-route map token validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 98 dream-route map token validation passed. Rows: $($manifestRows.Count)"
Write-Output $manifestRelative
