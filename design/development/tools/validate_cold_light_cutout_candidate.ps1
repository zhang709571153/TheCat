Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_42_cold_light_cutout_candidate_2026-06-15"
$candidateRoot = "design/development/asset_candidates/enemies/cold_light_shadow/$batchSlug"
$manifestRelative = "design/development/asset_candidates/enemies/$batchSlug/cold_light_batch42_cutout_manifest.csv"
$manifestPath = Join-Path $projectRoot ($manifestRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)

$expectedTypes = @{
    cutout_beam_alpha_1024 = "1024x1024"
    cutout_beam_alpha_512_preview = "512x512"
    cutout_beam_checkerboard_512_review = "512x512"
    cutout_beam_darkfield_512_review = "512x512"
    cutout_beam_warmfield_512_review = "512x512"
    cutout_beam_alpha_mask_512_review = "512x512"
}

$alphaTypes = @(
    "cutout_beam_alpha_1024",
    "cutout_beam_alpha_512_preview"
)

$requiredReviewTokens = @(
    "candidate review only",
    "do not import into Unity yet",
    "Formal Unity import remains blocked",
    "Cold Light Shadow",
    "source concept",
    "source animation",
    "Batch 38 review sheet",
    "Batch 41 input candidate review",
    "cold lamp silhouette",
    "cyan beam/light language",
    "mechanical spring arm",
    "black mud base",
    "single red eye",
    "long shadow-limb",
    "ranged-pressure",
    "semi-transparent alpha",
    "Rejection Rules",
    "08-active-enemy-cold-light.png"
)

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function Resolve-ProjectPath {
    param([string]$RelativePath)
    if ([string]::IsNullOrWhiteSpace($RelativePath)) {
        return ""
    }

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

function Test-AlphaCandidate {
    param(
        [string]$Path,
        [string]$Label
    )

    $bitmap = [System.Drawing.Bitmap]::FromFile($Path)
    try {
        if (-not [System.Drawing.Image]::IsAlphaPixelFormat($bitmap.PixelFormat)) {
            Add-Failure "$Label must use an alpha-capable pixel format but was $($bitmap.PixelFormat)"
        }

        $corners = @(
            $bitmap.GetPixel(0, 0),
            $bitmap.GetPixel($bitmap.Width - 1, 0),
            $bitmap.GetPixel(0, $bitmap.Height - 1),
            $bitmap.GetPixel($bitmap.Width - 1, $bitmap.Height - 1)
        )

        foreach ($corner in $corners) {
            if ($corner.A -gt 8) {
                Add-Failure "$Label corner alpha should be transparent but found $($corner.A)"
            }
        }

        $total = $bitmap.Width * $bitmap.Height
        $transparent = 0
        $semi = 0
        $opaque = 0
        $cyanSemi = 0
        for ($y = 0; $y -lt $bitmap.Height; $y++) {
            for ($x = 0; $x -lt $bitmap.Width; $x++) {
                $pixel = $bitmap.GetPixel($x, $y)
                if ($pixel.A -le 8) {
                    $transparent++
                } elseif ($pixel.A -lt 200) {
                    $semi++
                    if (($pixel.B -ge ($pixel.R + 10)) -and ($pixel.G -ge ($pixel.R + 4))) {
                        $cyanSemi++
                    }
                } else {
                    $opaque++
                }
            }
        }

        if ($transparent -lt [int]($total * 0.25)) {
            Add-Failure "$Label should preserve transparent background. Transparent pixels: $transparent of $total"
        }

        if ($opaque -lt [int]($total * 0.04)) {
            Add-Failure "$Label should keep an opaque lamp/body silhouette. Opaque pixels: $opaque of $total"
        }

        if ($semi -lt [int]($total * 0.01)) {
            Add-Failure "$Label should include semi-transparent beam/glow pixels. Semi pixels: $semi of $total"
        }

        if ($cyanSemi -lt [int]($total * 0.002)) {
            Add-Failure "$Label should preserve cyan semi-transparent beam pixels. Cyan semi pixels: $cyanSemi of $total"
        }
    } finally {
        $bitmap.Dispose()
    }
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Cold Light cutout manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne $expectedTypes.Count) {
    Add-Failure "Expected $($expectedTypes.Count) Cold Light cutout rows but found $($rows.Count)"
}

foreach ($type in $expectedTypes.Keys) {
    $matches = @($rows | Where-Object { $_.asset_type -eq $type })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected exactly one $type row but found $($matches.Count)"
    }
}

$reviewNotesSeen = New-Object System.Collections.Generic.HashSet[string]
$reviewSheetsSeen = New-Object System.Collections.Generic.HashSet[string]
$processNotesSeen = New-Object System.Collections.Generic.HashSet[string]
$agentPromptsSeen = New-Object System.Collections.Generic.HashSet[string]

foreach ($row in $rows) {
    if ($row.enemy_id -ne "cold_light_shadow") {
        Add-Failure "Unexpected enemy id: $($row.enemy_id)"
    }

    if ($row.source_lock_ids -ne "cold_light_concept;cold_light_animation") {
        Add-Failure "$($row.asset_type) source locks should be cold_light_concept;cold_light_animation but found $($row.source_lock_ids)"
    }

    if ($row.active_screenshot -ne "08-active-enemy-cold-light.png") {
        Add-Failure "$($row.asset_type) active screenshot should be 08-active-enemy-cold-light.png but found $($row.active_screenshot)"
    }

    if ($row.recommendation -ne "candidate_review_only_pending_playmode_screenshot") {
        Add-Failure "$($row.candidate_path) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.candidate_path -notlike "design/development/asset_candidates/enemies/cold_light_shadow/$batchSlug/*") {
        Add-Failure "$($row.candidate_path) must stay in the Cold Light Batch 42 candidate directory"
    }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if (-not (Test-Path -LiteralPath $candidatePath)) {
        Add-Failure "Candidate PNG missing: $($row.candidate_path)"
    } else {
        $actualSize = Get-ImageSize $candidatePath
        $expectedSize = $expectedTypes[$row.asset_type]
        if ($actualSize -ne $expectedSize) {
            Add-Failure "$($row.candidate_path) should be $expectedSize but is $actualSize"
        }

        if ($actualSize -ne $row.candidate_size) {
            Add-Failure "$($row.candidate_path) manifest candidate_size is $($row.candidate_size) but actual is $actualSize"
        }

        Test-Hash $candidatePath $row.candidate_sha256 "candidate $($row.candidate_path)"

        if (Test-Path -LiteralPath "$candidatePath.meta") {
            Add-Failure "Candidate PNG must not have a Unity .meta file: $($row.candidate_path).meta"
        }

        if ($alphaTypes -contains $row.asset_type) {
            Test-AlphaCandidate $candidatePath $row.asset_type
        }
    }

    if ($row.concept_source_path -notlike "design/*/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png") {
        Add-Failure "$($row.asset_type) must pin the Cold Light concept source"
    }

    if ($row.animation_source_path -notlike "design/*/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png") {
        Add-Failure "$($row.asset_type) must pin the Cold Light animation source"
    }

    Test-Hash (Resolve-ProjectPath $row.concept_source_path) $row.concept_source_sha256 "source concept"
    Test-Hash (Resolve-ProjectPath $row.animation_source_path) $row.animation_source_sha256 "source animation"

    $inputCandidatePath = Resolve-ProjectPath $row.input_candidate_path
    if (-not (Test-Path -LiteralPath $inputCandidatePath)) {
        Add-Failure "Input Batch 41 candidate is missing: $($row.input_candidate_path)"
    } else {
        Test-Hash $inputCandidatePath $row.input_candidate_sha256 "input Batch 41 candidate"
    }

    foreach ($referenceField in @("batch38_review_sheet", "batch38_combat_reference")) {
        $referencePath = Resolve-ProjectPath $row.$referenceField
        if (-not (Test-Path -LiteralPath $referencePath)) {
            Add-Failure "$referenceField missing: $($row.$referenceField)"
        }
    }

    $reviewSheetPath = Resolve-ProjectPath $row.review_sheet
    if (-not (Test-Path -LiteralPath $reviewSheetPath)) {
        Add-Failure "Review sheet missing: $($row.review_sheet)"
    } else {
        $reviewSheetsSeen.Add($row.review_sheet) | Out-Null
        $sheetSize = Get-ImageSize $reviewSheetPath
        if ($sheetSize -ne "1600x900") {
            Add-Failure "$($row.review_sheet) should be 1600x900 but is $sheetSize"
        }
        if (Test-Path -LiteralPath "$reviewSheetPath.meta") {
            Add-Failure "Review sheet must not have a Unity .meta file: $($row.review_sheet).meta"
        }
    }

    $reviewNotePath = Resolve-ProjectPath $row.review_note
    if (-not (Test-Path -LiteralPath $reviewNotePath)) {
        Add-Failure "Review note missing: $($row.review_note)"
    } else {
        $reviewNotesSeen.Add($row.review_note) | Out-Null
    }

    $processNotePath = Resolve-ProjectPath $row.process_note
    if (-not (Test-Path -LiteralPath $processNotePath)) {
        Add-Failure "Process note missing: $($row.process_note)"
    } else {
        $processNotesSeen.Add($row.process_note) | Out-Null
    }

    $agentPromptPath = Resolve-ProjectPath $row.agent_prompt
    if (-not (Test-Path -LiteralPath $agentPromptPath)) {
        Add-Failure "Agent prompt missing: $($row.agent_prompt)"
    } else {
        $agentPromptsSeen.Add($row.agent_prompt) | Out-Null
    }
}

if ($reviewNotesSeen.Count -ne 1) {
    Add-Failure "Expected one Cold Light cutout review note but found $($reviewNotesSeen.Count)"
}

if ($reviewSheetsSeen.Count -ne 1) {
    Add-Failure "Expected one Cold Light cutout review sheet but found $($reviewSheetsSeen.Count)"
}

if ($processNotesSeen.Count -ne 1) {
    Add-Failure "Expected one Cold Light cutout process note but found $($processNotesSeen.Count)"
}

if ($agentPromptsSeen.Count -ne 1) {
    Add-Failure "Expected one Cold Light cutout agent prompt but found $($agentPromptsSeen.Count)"
}

foreach ($noteRelative in $reviewNotesSeen) {
    $notePath = Resolve-ProjectPath $noteRelative
    $content = Get-Content -LiteralPath $notePath -Raw -Encoding UTF8
    foreach ($token in $requiredReviewTokens) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$noteRelative is missing review token: $token"
        }
    }
}

foreach ($processRelative in $processNotesSeen) {
    $processPath = Resolve-ProjectPath $processRelative
    $content = Get-Content -LiteralPath $processPath -Raw -Encoding UTF8
    foreach ($token in @("deterministic local post-processing", "No new image generation", "semi-transparent alpha", 'outside `Assets`', "formal import remains blocked")) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$processRelative is missing process token: $token"
        }
    }
}

foreach ($promptRelative in $agentPromptsSeen) {
    $promptPath = Resolve-ProjectPath $promptRelative
    $content = Get-Content -LiteralPath $promptPath -Raw -Encoding UTF8
    foreach ($token in @("beam-preserving transparent cutout", 'outside `Assets`', "formal import remains blocked", "one-enemy-at-a-time", "active-enemy", "semi-transparent cyan")) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$promptRelative is missing prompt token: $token"
        }
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath $candidateRoot) -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 42 candidate folder must not contain Unity .meta files."
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Cold Light cutout validation failed with $($failures.Count) issue(s)."
}

Write-Host "Cold Light cutout candidate validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Review notes: $($reviewNotesSeen.Count)"
Write-Host "Review sheets: $($reviewSheetsSeen.Count)"
Write-Host "Process notes: $($processNotesSeen.Count)"
Write-Host "Agent prompts: $($agentPromptsSeen.Count)"
Write-Host "Formal Unity import remains blocked pending active-enemy Play Mode screenshot review."
