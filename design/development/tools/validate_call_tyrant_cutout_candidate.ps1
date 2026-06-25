Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_44_call_tyrant_cutout_candidate_2026-06-15"
$candidateRoot = "design/development/asset_candidates/enemies/call_tyrant/$batchSlug"
$manifestRelative = "design/development/asset_candidates/enemies/$batchSlug/call_tyrant_batch44_cutout_manifest.csv"
$manifestPath = Join-Path $projectRoot ($manifestRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)

$expectedTypes = @{
    cutout_boss_alpha_1024 = "1024x1024"
    cutout_boss_alpha_512_preview = "512x512"
    cutout_boss_checkerboard_512_review = "512x512"
    cutout_boss_darkfield_512_review = "512x512"
    cutout_boss_warmfield_512_review = "512x512"
    cutout_boss_alpha_mask_512_review = "512x512"
}

$alphaTypes = @(
    "cutout_boss_alpha_1024",
    "cutout_boss_alpha_512_preview"
)

$requiredReviewTokens = @(
    "candidate review only",
    "do not import into Unity yet",
    "Formal Unity import remains blocked",
    "Call Tyrant",
    "source concept",
    "source animation",
    "Batch 38 review sheet",
    "Batch 43 input candidate review",
    "giant phone shell",
    "red call-eye signal",
    "purple tie",
    "black mud body",
    "app projectile language",
    "Boss-scale silhouette",
    "semi-transparent",
    "Rejection Rules",
    "09-active-enemy-call-tyrant.png"
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
        $redOpaque = 0
        $purpleOpaque = 0
        $saturatedOpaque = 0
        for ($y = 0; $y -lt $bitmap.Height; $y++) {
            for ($x = 0; $x -lt $bitmap.Width; $x++) {
                $pixel = $bitmap.GetPixel($x, $y)
                if ($pixel.A -le 8) {
                    $transparent++
                    continue
                }

                if ($pixel.A -lt 200) {
                    $semi++
                } else {
                    $opaque++
                }

                $max = [Math]::Max($pixel.R, [Math]::Max($pixel.G, $pixel.B))
                $min = [Math]::Min($pixel.R, [Math]::Min($pixel.G, $pixel.B))
                if ($pixel.A -ge 80 -and $pixel.R -ge 130 -and $pixel.R -ge ($pixel.G + 30) -and $pixel.R -ge ($pixel.B + 20)) {
                    $redOpaque++
                }

                if ($pixel.A -ge 80 -and $pixel.R -ge 58 -and $pixel.B -ge 72 -and $pixel.B -ge ($pixel.G + 16) -and $pixel.R -ge ($pixel.G + 8)) {
                    $purpleOpaque++
                }

                if ($pixel.A -ge 80 -and ($max - $min) -ge 58 -and $max -ge 120) {
                    $saturatedOpaque++
                }
            }
        }

        if ($transparent -lt [int]($total * 0.20)) {
            Add-Failure "$Label should preserve transparent background. Transparent pixels: $transparent of $total"
        }

        if ($opaque -lt [int]($total * 0.08)) {
            Add-Failure "$Label should keep an opaque Boss silhouette. Opaque pixels: $opaque of $total"
        }

        if ($semi -lt [int]($total * 0.002)) {
            Add-Failure "$Label should include semi-transparent throw-edge or matte pixels. Semi pixels: $semi of $total"
        }

        if ($redOpaque -lt [int]($total * 0.001)) {
            Add-Failure "$Label should preserve red call-eye pixels. Red pixels: $redOpaque of $total"
        }

        if ($purpleOpaque -lt [int]($total * 0.001)) {
            Add-Failure "$Label should preserve purple tie pixels. Purple pixels: $purpleOpaque of $total"
        }

        if ($saturatedOpaque -lt [int]($total * 0.002)) {
            Add-Failure "$Label should preserve saturated app projectile/readability pixels. Saturated pixels: $saturatedOpaque of $total"
        }
    } finally {
        $bitmap.Dispose()
    }
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Call Tyrant cutout manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne $expectedTypes.Count) {
    Add-Failure "Expected $($expectedTypes.Count) Call Tyrant cutout rows but found $($rows.Count)"
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
    if ($row.enemy_id -ne "call_tyrant") {
        Add-Failure "Unexpected enemy id: $($row.enemy_id)"
    }

    if ($row.source_lock_ids -ne "call_tyrant_concept;call_tyrant_animation") {
        Add-Failure "$($row.asset_type) source locks should be call_tyrant_concept;call_tyrant_animation but found $($row.source_lock_ids)"
    }

    if ($row.active_screenshot -ne "09-active-enemy-call-tyrant.png") {
        Add-Failure "$($row.asset_type) active screenshot should be 09-active-enemy-call-tyrant.png but found $($row.active_screenshot)"
    }

    if ($row.recommendation -ne "candidate_review_only_pending_playmode_screenshot") {
        Add-Failure "$($row.candidate_path) has unsafe recommendation: $($row.recommendation)"
    }

    if ($row.candidate_path -notlike "design/development/asset_candidates/enemies/call_tyrant/$batchSlug/*") {
        Add-Failure "$($row.candidate_path) must stay in the Call Tyrant Batch 44 candidate directory"
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

    if ($row.concept_source_path -notlike "design/*/assets/enemies/en06_call_tyrant/concept/call_tyrant_concept.png") {
        Add-Failure "$($row.asset_type) must pin the Call Tyrant concept source"
    }

    if ($row.animation_source_path -notlike "design/*/assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png") {
        Add-Failure "$($row.asset_type) must pin the Call Tyrant animation source"
    }

    Test-Hash (Resolve-ProjectPath $row.concept_source_path) $row.concept_source_sha256 "source concept"
    Test-Hash (Resolve-ProjectPath $row.animation_source_path) $row.animation_source_sha256 "source animation"

    $inputCandidatePath = Resolve-ProjectPath $row.input_candidate_path
    if (-not (Test-Path -LiteralPath $inputCandidatePath)) {
        Add-Failure "Input Batch 43 candidate is missing: $($row.input_candidate_path)"
    } else {
        Test-Hash $inputCandidatePath $row.input_candidate_sha256 "input Batch 43 candidate"
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
    Add-Failure "Expected one Call Tyrant cutout review note but found $($reviewNotesSeen.Count)"
}

if ($reviewSheetsSeen.Count -ne 1) {
    Add-Failure "Expected one Call Tyrant cutout review sheet but found $($reviewSheetsSeen.Count)"
}

if ($processNotesSeen.Count -ne 1) {
    Add-Failure "Expected one Call Tyrant cutout process note but found $($processNotesSeen.Count)"
}

if ($agentPromptsSeen.Count -ne 1) {
    Add-Failure "Expected one Call Tyrant cutout agent prompt but found $($agentPromptsSeen.Count)"
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
    foreach ($token in @("Boss transparent cutout", 'outside `Assets`', "formal import remains blocked", "one-enemy-at-a-time", "active-enemy", "red call-eye", "purple tie", "app projectile")) {
        if ($content.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$promptRelative is missing prompt token: $token"
        }
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath (Resolve-ProjectPath $candidateRoot) -Recurse -Filter "*.meta" -File -ErrorAction SilentlyContinue)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 44 candidate folder must not contain Unity .meta files."
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Call Tyrant cutout validation failed with $($failures.Count) issue(s)."
}

Write-Host "Call Tyrant cutout candidate validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Review notes: $($reviewNotesSeen.Count)"
Write-Host "Review sheets: $($reviewSheetsSeen.Count)"
Write-Host "Process notes: $($processNotesSeen.Count)"
Write-Host "Agent prompts: $($agentPromptsSeen.Count)"
Write-Host "Formal Unity import remains blocked pending active-enemy Play Mode screenshot review."
