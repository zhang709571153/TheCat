Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_76_owner_sleep_state_framesheet_candidate_2026-06-24"
$batchDirRelative = "design/development/asset_candidates/owner_sleep_states/$batchSlug"
$manifestRelative = "$batchDirRelative/owner_sleep_states_batch76_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$frameDirRelative = "$batchDirRelative/frames"
$reviewSheetRelative = "$batchDirRelative/thecat_owner_sleep_states_batch76_review_sheet_1920x1320_v001.png"
$contactSheetRelative = "$batchDirRelative/thecat_owner_sleep_states_batch76_contact_sheet_1920x1320_v001.png"
$reviewNoteRelative = "$batchDirRelative/owner_sleep_states_batch76_candidate_review.md"
$processNoteRelative = "$batchDirRelative/owner_sleep_states_batch76_process_note.md"
$promptRelative = "design/development/agent_prompts/p0_asset_batch_76_owner_sleep_state_framesheet_candidate.md"
$alphaSheetRelative = "$batchDirRelative/thecat_owner_sleep_states_batch76_alpha_sheet_1536x1024_candidate_v002.png"
$sourceRelative = "$batchDirRelative/thecat_owner_sleep_states_batch76_chromakey_source_1536x1024_v002.png"

$expectedStates = @("deep_sleep", "half_awake", "near_awake", "wake_failure")
$requiredReviewTokens = @(
    "candidate review only",
    "do not import into Unity",
    "no cat body",
    "4 states x 6 frames",
    "Unity Gate",
    "v002",
    "padded"
)
$requiredPromptTokens = @(
    "Batch 76",
    "owner sleep state",
    "4-row by 6-column",
    "No cats",
    "Do not install anything into Unity",
    "v002"
)

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function Resolve-ProjectPath {
    param([string]$RelativePath)
    return Join-Path $projectRoot ($RelativePath -replace "/", [System.IO.Path]::DirectorySeparatorChar)
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
        Add-Failure "$Label missing at $Path"
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
            if ($pixel.A -gt 12) {
                Add-Failure "$Path corner pixel $($point[0]),$($point[1]) should be transparent but alpha is $($pixel.A)"
            }
        }
    } finally {
        $bitmap.Dispose()
    }
}

function Test-AlphaMargins {
    param(
        [string]$Path,
        [string]$Label
    )

    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $minX = $bitmap.Width
        $minY = $bitmap.Height
        $maxX = -1
        $maxY = -1

        for ($y = 0; $y -lt $bitmap.Height; $y++) {
            for ($x = 0; $x -lt $bitmap.Width; $x++) {
                $pixel = $bitmap.GetPixel($x, $y)
                if ($pixel.A -gt 16) {
                    if ($x -lt $minX) { $minX = $x }
                    if ($y -lt $minY) { $minY = $y }
                    if ($x -gt $maxX) { $maxX = $x }
                    if ($y -gt $maxY) { $maxY = $y }
                }
            }
        }

        if ($maxX -lt 0) {
            Add-Failure "$Label has no visible alpha content"
            return
        }

        $left = $minX
        $top = $minY
        $right = ($bitmap.Width - 1) - $maxX
        $bottom = ($bitmap.Height - 1) - $maxY
        $minimumMargin = 12
        if ($left -lt $minimumMargin -or $top -lt $minimumMargin -or $right -lt $minimumMargin -or $bottom -lt $minimumMargin) {
            Add-Failure "$Label alpha content is too close to edge. Margins left/top/right/bottom: $left/$top/$right/$bottom"
        }
    } finally {
        $bitmap.Dispose()
    }
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 76 owner sleep state manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 24) {
    Add-Failure "Expected 24 Batch 76 frame rows but found $($rows.Count)"
}

foreach ($state in $expectedStates) {
    $stateRows = @($rows | Where-Object { $_.state_id -eq $state })
    if ($stateRows.Count -ne 6) {
        Add-Failure "Expected 6 frame rows for $state but found $($stateRows.Count)"
    }

    $frameIndexes = @($stateRows | Sort-Object {[int]$_.frame_index} | ForEach-Object { [int]$_.frame_index })
    for ($i = 1; $i -le 6; $i++) {
        if ($frameIndexes -notcontains $i) {
            Add-Failure "$state is missing frame index $i"
        }
    }
}

$assetIds = New-Object System.Collections.Generic.HashSet[string]
$candidatePaths = New-Object System.Collections.Generic.HashSet[string]

foreach ($row in $rows) {
    if (-not $assetIds.Add($row.asset_id)) {
        Add-Failure "Duplicate asset_id in manifest: $($row.asset_id)"
    }

    if (-not $candidatePaths.Add($row.candidate_path)) {
        Add-Failure "Duplicate candidate_path in manifest: $($row.candidate_path)"
    }

    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$($row.asset_id) has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.subject_id -ne "owner_sleep_state") {
        Add-Failure "$($row.asset_id) has wrong subject id: $($row.subject_id)"
    }

    if ($row.asset_type -ne "alpha_frame_256") {
        Add-Failure "$($row.asset_id) has wrong asset type: $($row.asset_type)"
    }

    if ($row.recommendation -ne "candidate_review_only_do_not_import") {
        Add-Failure "$($row.asset_id) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.candidate_path -notlike "$frameDirRelative/*") {
        Add-Failure "$($row.asset_id) candidate path must stay under $frameDirRelative"
    }

    if ($row.candidate_path -like "Assets/*") {
        Add-Failure "$($row.asset_id) candidate path must not be inside Assets"
    }

    $candidateStem = [System.IO.Path]::GetFileNameWithoutExtension($row.candidate_path)
    if ($row.asset_id -ne $candidateStem) {
        Add-Failure "$($row.asset_id) asset_id must match candidate file stem $candidateStem"
    }

    if ($row.source_prompt_path -ne $promptRelative) {
        Add-Failure "$($row.asset_id) prompt path should be $promptRelative but found $($row.source_prompt_path)"
    }

    if ($row.source_image_path -ne $sourceRelative) {
        Add-Failure "$($row.asset_id) source image path should be $sourceRelative but found $($row.source_image_path)"
    }

    if ($row.alpha_sheet_path -ne $alphaSheetRelative) {
        Add-Failure "$($row.asset_id) alpha sheet path should be $alphaSheetRelative but found $($row.alpha_sheet_path)"
    }

    if ($row.review_sheet -ne $reviewSheetRelative) {
        Add-Failure "$($row.asset_id) review sheet should be $reviewSheetRelative but found $($row.review_sheet)"
    }

    if ($row.review_note -ne $reviewNoteRelative) {
        Add-Failure "$($row.asset_id) review note should be $reviewNoteRelative but found $($row.review_note)"
    }

    if ($row.process_note -ne $processNoteRelative) {
        Add-Failure "$($row.asset_id) process note should be $processNoteRelative but found $($row.process_note)"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    Test-Hash $candidatePath $row.candidate_sha256 "$($row.asset_id) candidate"
    if (Test-Path -LiteralPath $candidatePath) {
        $size = Get-ImageSize $candidatePath
        if ($size -ne "256x256") {
            Add-Failure "$($row.asset_id) should be 256x256 but is $size"
        }

        if ($size -ne $row.candidate_size) {
            Add-Failure "$($row.asset_id) manifest size $($row.candidate_size) does not match actual $size"
        }

        Test-TransparentCorners $candidatePath
        Test-AlphaMargins $candidatePath "$($row.asset_id) candidate"

        if (Test-Path -LiteralPath "$candidatePath.meta") {
            Add-Failure "$($row.asset_id) candidate must not have a Unity .meta file"
        }
    }

    Test-Hash (Resolve-ProjectPath $row.source_image_path) $row.source_image_sha256 "$($row.asset_id) source image"
    Test-Hash (Resolve-ProjectPath $row.alpha_sheet_path) $row.alpha_sheet_sha256 "$($row.asset_id) alpha sheet"
}

foreach ($imageRelative in @($sourceRelative, $alphaSheetRelative)) {
    $path = Resolve-ProjectPath $imageRelative
    if (-not (Test-Path -LiteralPath $path)) {
        Add-Failure "Required Batch 76 image missing: $imageRelative"
    } else {
        $size = Get-ImageSize $path
        if ($size -ne "1536x1024") {
            Add-Failure "$imageRelative should be 1536x1024 but is $size"
        }
    }
}

foreach ($imageRelative in @($reviewSheetRelative, $contactSheetRelative)) {
    $path = Resolve-ProjectPath $imageRelative
    if (-not (Test-Path -LiteralPath $path)) {
        Add-Failure "Required Batch 76 review image missing: $imageRelative"
    } else {
        $size = Get-ImageSize $path
        if ($size -ne "1920x1320") {
            Add-Failure "$imageRelative should be 1920x1320 but is $size"
        }

        if (Test-Path -LiteralPath "$path.meta") {
            Add-Failure "$imageRelative must not have a Unity .meta file"
        }
    }
}

foreach ($textRelative in @($reviewNoteRelative, $processNoteRelative, $promptRelative)) {
    $textPath = Resolve-ProjectPath $textRelative
    if (-not (Test-Path -LiteralPath $textPath)) {
        Add-Failure "Required Batch 76 text file is missing: $textRelative"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $content = Get-Content -LiteralPath $reviewNotePath -Raw -Encoding UTF8
    foreach ($token in $requiredReviewTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative is missing review token: $token"
        }
    }
}

$promptPath = Resolve-ProjectPath $promptRelative
if (Test-Path -LiteralPath $promptPath) {
    $content = Get-Content -LiteralPath $promptPath -Raw -Encoding UTF8
    foreach ($token in $requiredPromptTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$promptRelative is missing prompt token: $token"
        }
    }
}

$metaFiles = @(Get-ChildItem -Path (Resolve-ProjectPath $batchDirRelative) -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 76 must not create Unity .meta files: $($metaFiles.FullName -join ', ')"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Owner sleep state Batch 76 validation failed with $($failures.Count) issue(s)."
}

Write-Host "Owner sleep state Batch 76 validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "States: $($expectedStates.Count)"
Write-Host "Frames per state: 6"
Write-Host "Formal Unity import remains blocked pending runtime screenshot review."
