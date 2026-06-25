Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_82_common_ui_state_candidates_2026-06-25"
$batchDirRelative = "design/development/asset_candidates/ui/common_components/$batchSlug"
$batchDir = Join-Path $projectRoot ($batchDirRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$manifestRelative = "$batchDirRelative/thecat_ui_common_states_batch82_manifest.csv"
$manifestPath = Join-Path $projectRoot ($manifestRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$contactSheetRelative = "$batchDirRelative/thecat_ui_common_states_batch82_contact_sheet_v001.png"
$reviewSheetRelative = "$batchDirRelative/thecat_ui_common_states_batch82_review_sheet_v001.png"
$reviewNoteRelative = "$batchDirRelative/thecat_ui_common_states_batch82_candidate_review.md"
$processNoteRelative = "$batchDirRelative/thecat_ui_common_states_batch82_process_note.md"
$specRelative = "design/development/agent_prompts/p0_asset_batch_82_common_ui_state_derivative_candidates.md"

$expectedVariants = [ordered]@{
    button_default = "384x96"
    button_hover = "384x96"
    button_pressed = "384x96"
    button_selected = "384x96"
    button_disabled = "384x96"
    button_secondary = "384x96"
    button_danger = "384x96"
    button_focus = "384x96"
    modal_large = "768x432"
    modal_medium = "640x360"
    modal_compact = "512x320"
    tab_selected = "192x64"
    tab_unselected = "192x64"
    tab_disabled = "192x64"
    segmented_left_selected = "512x72"
    segmented_middle_selected = "512x72"
    segmented_right_selected = "512x72"
    list_row_default = "640x80"
    list_row_selected = "640x80"
    list_row_disabled = "640x80"
    list_row_warning = "640x80"
    list_row_badge_default = "640x80"
    list_row_badge_selected = "640x80"
    list_row_badge_disabled = "640x80"
    list_row_badge_warning = "640x80"
}

$expectedComponentCounts = @{
    button_state_atlas = 8
    modal_dialog_frame = 3
    tabs_segmented_controls = 6
    list_row_frame = 8
}

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

function Get-ImageSize {
    param([string]$Path)
    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        return "$($image.Width)x$($image.Height)"
    } finally {
        $image.Dispose()
    }
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

function Test-TransparentCorners {
    param([string]$Path)

    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $points = @(
            @(0, 0),
            @(($bitmap.Width - 1), 0),
            @(0, ($bitmap.Height - 1)),
            @(($bitmap.Width - 1), ($bitmap.Height - 1))
        )

        foreach ($point in $points) {
            $pixel = $bitmap.GetPixel($point[0], $point[1])
            if ($pixel.A -gt 20) {
                Add-Failure "$Path corner pixel $($point[0]),$($point[1]) should be transparent but alpha is $($pixel.A)"
            }
        }
    } finally {
        $bitmap.Dispose()
    }
}

function Test-VisibleAlpha {
    param(
        [string]$Path,
        [string]$Label
    )

    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $visible = 0
        for ($y = 0; $y -lt $bitmap.Height; $y++) {
            for ($x = 0; $x -lt $bitmap.Width; $x++) {
                if ($bitmap.GetPixel($x, $y).A -gt 16) {
                    $visible++
                }
            }
        }

        $minimum = [Math]::Max(80, [int](($bitmap.Width * $bitmap.Height) * 0.035))
        if ($visible -lt $minimum) {
            Add-Failure "$Label has too little visible alpha content: $visible pixels, expected at least $minimum"
        }
    } finally {
        $bitmap.Dispose()
    }
}

function Test-AlphaCoverage {
    param(
        [string]$Path,
        [string]$Label,
        [double]$MinWidthRatio,
        [double]$MinHeightRatio
    )

    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $minX = $bitmap.Width
        $minY = $bitmap.Height
        $maxX = -1
        $maxY = -1
        for ($y = 0; $y -lt $bitmap.Height; $y++) {
            for ($x = 0; $x -lt $bitmap.Width; $x++) {
                if ($bitmap.GetPixel($x, $y).A -gt 16) {
                    if ($x -lt $minX) { $minX = $x }
                    if ($y -lt $minY) { $minY = $y }
                    if ($x -gt $maxX) { $maxX = $x }
                    if ($y -gt $maxY) { $maxY = $y }
                }
            }
        }

        if ($maxX -lt 0 -or $maxY -lt 0) {
            Add-Failure "$Label has no alpha bounds"
            return
        }

        $widthRatio = (($maxX - $minX + 1) / [double]$bitmap.Width)
        $heightRatio = (($maxY - $minY + 1) / [double]$bitmap.Height)
        if ($widthRatio -lt $MinWidthRatio -or $heightRatio -lt $MinHeightRatio) {
            Add-Failure "$Label alpha coverage too small: widthRatio=$([Math]::Round($widthRatio, 3)), heightRatio=$([Math]::Round($heightRatio, 3))"
        }
    } finally {
        $bitmap.Dispose()
    }
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 82 manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 25) {
    Add-Failure "Expected 25 Batch 82 rows but found $($rows.Count)"
}

$variantIds = New-Object System.Collections.Generic.HashSet[string]
$candidatePaths = New-Object System.Collections.Generic.HashSet[string]

foreach ($variantId in $expectedVariants.Keys) {
    $matches = @($rows | Where-Object { $_.variant_id -eq $variantId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected one manifest row for $variantId but found $($matches.Count)"
    } elseif ($matches[0].target_size -ne $expectedVariants[$variantId]) {
        Add-Failure "$variantId target_size should be $($expectedVariants[$variantId]) but found $($matches[0].target_size)"
    }
}

foreach ($componentId in $expectedComponentCounts.Keys) {
    $matches = @($rows | Where-Object { $_.component_id -eq $componentId })
    if ($matches.Count -ne $expectedComponentCounts[$componentId]) {
        Add-Failure "$componentId should have $($expectedComponentCounts[$componentId]) rows but found $($matches.Count)"
    }
}

foreach ($row in $rows) {
    if (-not $variantIds.Add($row.variant_id)) {
        Add-Failure "Duplicate variant_id in manifest: $($row.variant_id)"
    }

    if (-not $candidatePaths.Add($row.candidate_path)) {
        Add-Failure "Duplicate candidate_path in manifest: $($row.candidate_path)"
    }

    if (-not $expectedVariants.Contains($row.variant_id)) {
        Add-Failure "$($row.asset_id) has unexpected variant_id=$($row.variant_id)"
    }

    if (-not $expectedComponentCounts.ContainsKey($row.component_id)) {
        Add-Failure "$($row.asset_id) has unexpected component_id=$($row.component_id)"
    }

    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$($row.asset_id) has wrong batch_slug=$($row.batch_slug)"
    }

    if ($row.asset_type -ne "ui_common_state_candidate") {
        Add-Failure "$($row.asset_id) has wrong asset_type=$($row.asset_type)"
    }

    if ($row.recommendation -ne "candidate_review_only_do_not_import") {
        Add-Failure "$($row.asset_id) has unsafe recommendation=$($row.recommendation)"
    }

    if ($row.source_model -ne "derived_from_existing_ui_assets_not_image2") {
        Add-Failure "$($row.asset_id) source_model must explicitly say derived_from_existing_ui_assets_not_image2"
    }

    $sourceAssets = @($row.source_assets -split ";" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
    $sourceHashes = @($row.source_sha256 -split ";" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
    if ($sourceAssets.Count -ne 3 -or $sourceHashes.Count -ne 3 -or $sourceAssets.Count -ne $sourceHashes.Count) {
        Add-Failure "$($row.asset_id) should record three source assets and matching hashes"
    } else {
        for ($index = 0; $index -lt $sourceAssets.Count; $index++) {
            $sourcePath = Resolve-ProjectPath $sourceAssets[$index]
            if ($null -eq $sourcePath -or -not (Test-Path -LiteralPath $sourcePath)) {
                Add-Failure "$($row.asset_id) source asset not found: $($sourceAssets[$index])"
            } else {
                Test-Hash -Path $sourcePath -ExpectedHash $sourceHashes[$index] -Label "$($row.asset_id) source $($sourceAssets[$index])"
            }
        }
    }

    if ($row.candidate_path -like "Assets/*" -or $row.candidate_path -like "Assets\*") {
        Add-Failure "$($row.asset_id) candidate_path must not be inside Assets: $($row.candidate_path)"
    }

    if ($row.candidate_path -notlike "$batchDirRelative/*") {
        Add-Failure "$($row.asset_id) candidate_path must stay in Batch 82 directory: $($row.candidate_path)"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if ($null -ne $candidatePath) {
        if (-not (Test-Path -LiteralPath $candidatePath)) {
            Add-Failure "$($row.asset_id) candidate_path not found: $($row.candidate_path)"
        } else {
            $actualSize = Get-ImageSize $candidatePath
            if ($actualSize -ne $row.candidate_size -or $actualSize -ne $row.target_size) {
                Add-Failure "$($row.asset_id) size mismatch. target=$($row.target_size), manifest=$($row.candidate_size), actual=$actualSize"
            }
            Test-Hash -Path $candidatePath -ExpectedHash $row.candidate_sha256 -Label $row.asset_id
            Test-TransparentCorners -Path $candidatePath
            Test-VisibleAlpha -Path $candidatePath -Label $row.asset_id
            switch ($row.component_id) {
                "button_state_atlas" { Test-AlphaCoverage -Path $candidatePath -Label $row.asset_id -MinWidthRatio 0.70 -MinHeightRatio 0.48 }
                "modal_dialog_frame" { Test-AlphaCoverage -Path $candidatePath -Label $row.asset_id -MinWidthRatio 0.70 -MinHeightRatio 0.58 }
                "tabs_segmented_controls" { Test-AlphaCoverage -Path $candidatePath -Label $row.asset_id -MinWidthRatio 0.66 -MinHeightRatio 0.45 }
                "list_row_frame" { Test-AlphaCoverage -Path $candidatePath -Label $row.asset_id -MinWidthRatio 0.70 -MinHeightRatio 0.42 }
            }
        }
    }

    foreach ($expectedSharedPath in @($contactSheetRelative, $reviewSheetRelative, $reviewNoteRelative, $processNoteRelative)) {
        $fieldName = switch ($expectedSharedPath) {
            $contactSheetRelative { "contact_sheet" }
            $reviewSheetRelative { "review_sheet" }
            $reviewNoteRelative { "review_note" }
            $processNoteRelative { "process_note" }
        }
        if ($row.$fieldName -ne $expectedSharedPath) {
            Add-Failure "$($row.asset_id) $fieldName should be $expectedSharedPath but found $($row.$fieldName)"
        }
    }
}

foreach ($sharedPath in @($contactSheetRelative, $reviewSheetRelative, $reviewNoteRelative, $processNoteRelative, $specRelative)) {
    $path = Resolve-ProjectPath $sharedPath
    if ($null -eq $path -or -not (Test-Path -LiteralPath $path)) {
        Add-Failure "Missing shared Batch 82 evidence: $sharedPath"
    }
}

if (Test-Path -LiteralPath $batchDir) {
    $metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File -Filter "*.meta")
    if ($metaFiles.Count -ne 0) {
        Add-Failure "Candidate batch must not contain Unity .meta files; found $($metaFiles.Count)"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
$processNotePath = Resolve-ProjectPath $processNoteRelative
$specPath = Resolve-ProjectPath $specRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $reviewText = Get-Content -LiteralPath $reviewNotePath -Raw -Encoding UTF8
    foreach ($token in @("candidate review only", "do not import into Unity", "button_state_atlas", "modal_dialog_frame", "tabs_segmented_controls", "list_row_frame", "No starter-cat body")) {
        if ($reviewText -notmatch [regex]::Escape($token)) {
            Add-Failure "Review note missing token: $token"
        }
    }
}

if (Test-Path -LiteralPath $processNotePath) {
    $processText = Get-Content -LiteralPath $processNotePath -Raw -Encoding UTF8
    foreach ($token in @("deterministic derivative", "Why no image2", "OPENAI_API_KEY", "does not expose a model selector", "Unity gate")) {
        if ($processText -notmatch [regex]::Escape($token)) {
            Add-Failure "Process note missing token: $token"
        }
    }
}

if (Test-Path -LiteralPath $specPath) {
    $specText = Get-Content -LiteralPath $specPath -Raw -Encoding UTF8
    foreach ($token in @("Batch 82", "Qr1 UI/style source truth", "no baked Chinese text", "do not claim image2 provenance")) {
        if ($specText -notmatch [regex]::Escape($token)) {
            Add-Failure "Spec note missing token: $token"
        }
    }
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 82 common UI state candidate validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 82 common UI state candidate validation passed. Rows: $($rows.Count)"
Write-Output $manifestRelative
