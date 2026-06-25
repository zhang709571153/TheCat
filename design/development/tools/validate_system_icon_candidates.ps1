Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_79_system_icon_candidates_2026-06-24"
$batchDirRelative = "design/development/asset_candidates/ui/system_icons/$batchSlug"
$manifestRelative = "$batchDirRelative/system_icons_batch79_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$sourceRelative = "$batchDirRelative/thecat_ui_system_icons_batch79_chromakey_source_v001.png"
$alphaRelative = "$batchDirRelative/thecat_ui_system_icons_batch79_alpha_sheet_v001.png"
$contactSheetRelative = "$batchDirRelative/thecat_ui_system_icons_batch79_contact_sheet_v001.png"
$reviewSheetRelative = "$batchDirRelative/thecat_ui_system_icons_batch79_review_sheet_v001.png"
$reviewNoteRelative = "$batchDirRelative/system_icons_batch79_candidate_review.md"
$processNoteRelative = "$batchDirRelative/system_icons_batch79_process_note.md"
$promptRelative = "design/development/agent_prompts/p0_asset_batch_79_system_icon_candidates.md"

$expectedIcons = @("settings", "sound", "mute", "back", "close", "pause", "continue", "retry", "lock", "warning")
$expectedSizes = @("128", "64", "32")
$requiredReviewTokens = @(
    "candidate review only",
    "do not import into Unity",
    "ten system icons",
    "no cat body",
    "Unity Gate"
)
$requiredPromptTokens = @(
    "Batch 79",
    "system icons",
    "settings",
    "sound",
    "mute",
    "back",
    "close",
    "pause",
    "continue",
    "retry",
    "lock",
    "warning",
    "Do not install anything into Unity"
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

function Get-NormalizedProjectRelativePath {
    param([string]$RelativePath)

    $candidate = Resolve-Path -LiteralPath (Resolve-ProjectPath $RelativePath) -ErrorAction SilentlyContinue
    if ($null -eq $candidate) {
        return $null
    }

    $root = [System.IO.Path]::GetFullPath($projectRoot.Path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
    $full = [System.IO.Path]::GetFullPath($candidate.Path)
    if (-not $full.StartsWith($root + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
        return $null
    }

    return $full.Substring($root.Length + 1).Replace([System.IO.Path]::DirectorySeparatorChar, "/")
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
                $pixel = $bitmap.GetPixel($x, $y)
                if ($pixel.A -gt 16) {
                    $visible++
                }
            }
        }

        if ($visible -lt [Math]::Max(20, [int](($bitmap.Width * $bitmap.Height) * 0.04))) {
            Add-Failure "$Label has too little visible alpha content: $visible pixels"
        }
    } finally {
        $bitmap.Dispose()
    }
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 79 system icon manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 30) {
    Add-Failure "Expected 30 Batch 79 rows but found $($rows.Count)"
}

$assetIds = New-Object System.Collections.Generic.HashSet[string]
$candidatePaths = New-Object System.Collections.Generic.HashSet[string]

foreach ($icon in $expectedIcons) {
    $iconRows = @($rows | Where-Object { $_.icon_id -eq $icon })
    if ($iconRows.Count -ne 3) {
        Add-Failure "Expected 3 size rows for $icon but found $($iconRows.Count)"
    }

    foreach ($size in $expectedSizes) {
        $matches = @($iconRows | Where-Object { $_.size_variant -eq $size })
        if ($matches.Count -ne 1) {
            Add-Failure "Expected one $size px row for $icon but found $($matches.Count)"
        }
    }
}

foreach ($row in $rows) {
    if (-not $assetIds.Add($row.asset_id)) {
        Add-Failure "Duplicate asset_id in manifest: $($row.asset_id)"
    }

    if (-not $candidatePaths.Add($row.candidate_path)) {
        Add-Failure "Duplicate candidate_path in manifest: $($row.candidate_path)"
    }

    if ($expectedIcons -notcontains $row.icon_id) {
        Add-Failure "$($row.asset_id) has unexpected icon id: $($row.icon_id)"
    }

    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$($row.asset_id) has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.asset_type -ne "system_icon_candidate") {
        Add-Failure "$($row.asset_id) has wrong asset type: $($row.asset_type)"
    }

    if ($row.recommendation -ne "candidate_review_only_do_not_import") {
        Add-Failure "$($row.asset_id) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.candidate_path -like "Assets/*") {
        Add-Failure "$($row.asset_id) candidate path must not be inside Assets"
    }

    $normalizedCandidatePath = Get-NormalizedProjectRelativePath $row.candidate_path
    if ($null -eq $normalizedCandidatePath) {
        Add-Failure "$($row.asset_id) candidate path does not resolve inside the project: $($row.candidate_path)"
    } elseif ($normalizedCandidatePath -like "Assets/*") {
        Add-Failure "$($row.asset_id) candidate path must not resolve inside Assets: $($row.candidate_path)"
    } elseif ($normalizedCandidatePath -ne ($row.candidate_path -replace "\\", "/")) {
        Add-Failure "$($row.asset_id) candidate path should be normalized without traversal: $($row.candidate_path)"
    }

    if ($row.source_prompt_path -ne $promptRelative) {
        Add-Failure "$($row.asset_id) prompt path should be $promptRelative but found $($row.source_prompt_path)"
    }

    if ($row.source_image_path -ne $sourceRelative) {
        Add-Failure "$($row.asset_id) source image path should be $sourceRelative but found $($row.source_image_path)"
    }

    if ($row.alpha_sheet_path -ne $alphaRelative) {
        Add-Failure "$($row.asset_id) alpha sheet path should be $alphaRelative but found $($row.alpha_sheet_path)"
    }

    if ($row.contact_sheet -ne $contactSheetRelative) {
        Add-Failure "$($row.asset_id) contact sheet should be $contactSheetRelative but found $($row.contact_sheet)"
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

    $candidateStem = [System.IO.Path]::GetFileNameWithoutExtension($row.candidate_path)
    if ($row.asset_id -ne $candidateStem) {
        Add-Failure "$($row.asset_id) asset_id must match candidate file stem $candidateStem"
    }

    $expectedSize = "$($row.size_variant)x$($row.size_variant)"
    $candidatePath = Resolve-ProjectPath $row.candidate_path
    Test-Hash $candidatePath $row.candidate_sha256 "$($row.asset_id) candidate"
    if (Test-Path -LiteralPath $candidatePath) {
        $size = Get-ImageSize $candidatePath
        if ($size -ne $expectedSize) {
            Add-Failure "$($row.asset_id) should be $expectedSize but is $size"
        }

        if ($size -ne $row.candidate_size) {
            Add-Failure "$($row.asset_id) manifest size $($row.candidate_size) does not match actual $size"
        }

        Test-TransparentCorners $candidatePath
        Test-VisibleAlpha $candidatePath "$($row.asset_id) candidate"

        if (Test-Path -LiteralPath "$candidatePath.meta") {
            Add-Failure "$($row.asset_id) candidate must not have a Unity .meta file"
        }
    }

    Test-Hash (Resolve-ProjectPath $row.source_image_path) $row.source_image_sha256 "$($row.asset_id) source image"
    Test-Hash (Resolve-ProjectPath $row.alpha_sheet_path) $row.alpha_sheet_sha256 "$($row.asset_id) alpha sheet"
}

foreach ($imageRelative in @($sourceRelative, $alphaRelative, $contactSheetRelative, $reviewSheetRelative)) {
    $path = Resolve-ProjectPath $imageRelative
    if (-not (Test-Path -LiteralPath $path)) {
        Add-Failure "Required Batch 79 image missing: $imageRelative"
    } else {
        if (Test-Path -LiteralPath "$path.meta") {
            Add-Failure "$imageRelative must not have a Unity .meta file"
        }
    }
}

foreach ($textRelative in @($reviewNoteRelative, $processNoteRelative, $promptRelative)) {
    $textPath = Resolve-ProjectPath $textRelative
    if (-not (Test-Path -LiteralPath $textPath)) {
        Add-Failure "Required Batch 79 text file is missing: $textRelative"
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
    Add-Failure "Batch 79 must not create Unity .meta files: $($metaFiles.FullName -join ', ')"
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "System icon Batch 79 validation failed with $($failures.Count) issue(s)."
}

Write-Host "System icon Batch 79 validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Icons: $($expectedIcons.Count)"
Write-Host "Size variants per icon: $($expectedSizes.Count)"
Write-Host "Formal Unity import remains blocked pending UI screenshot review."
