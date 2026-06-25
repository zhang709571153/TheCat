Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_67_bedroom_interaction_affordance_candidates_2026-06-15"
$batchDirRelative = "design/development/asset_candidates/ui/bedroom_interaction_affordances/$batchSlug"
$batchDir = Join-Path $projectRoot ($batchDirRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
New-Item -ItemType Directory -Force -Path $batchDir | Out-Null

$manifestRelative = "$batchDirRelative/bedroom_interaction_affordance_batch67_manifest.csv"
$reviewSheetRelative = "$batchDirRelative/thecat_ui_bedroom_interaction_affordance_batch67_review_sheet.png"
$reviewNoteRelative = "$batchDirRelative/bedroom_interaction_affordance_batch67_candidate_review.md"
$processNoteRelative = "$batchDirRelative/bedroom_interaction_affordance_batch67_process_note.md"

function New-Bitmap {
    param([int]$Width, [int]$Height)
    $bitmap = [System.Drawing.Bitmap]::new($Width, $Height, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $graphics.Clear([System.Drawing.Color]::Transparent)
    return @{ Bitmap = $bitmap; Graphics = $graphics }
}

function Save-Png {
    param(
        [System.Drawing.Bitmap]$Bitmap,
        [string]$RelativePath
    )

    $path = Join-Path $projectRoot ($RelativePath -replace "/", [System.IO.Path]::DirectorySeparatorChar)
    $Bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
}

function New-Pen {
    param([int]$A, [int]$R, [int]$G, [int]$B, [float]$Width)
    $pen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb($A, $R, $G, $B), $Width)
    $pen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $pen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $pen.LineJoin = [System.Drawing.Drawing2D.LineJoin]::Round
    return $pen
}

function New-Brush {
    param([int]$A, [int]$R, [int]$G, [int]$B)
    return [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb($A, $R, $G, $B))
}

function Draw-BedReadyRing {
    param([string]$RelativePath)

    $surface = New-Bitmap 256 256
    $bitmap = $surface.Bitmap
    $g = $surface.Graphics
    $objects = New-Object System.Collections.Generic.List[System.IDisposable]
    try {
        $glow = New-Brush 34 73 210 255
        $outer = New-Pen 210 115 224 255 8
        $inner = New-Pen 170 243 244 255 4
        $sleep = New-Pen 160 255 244 170 5
        $star = New-Brush 210 255 240 145
        $objects.Add($glow); $objects.Add($outer); $objects.Add($inner); $objects.Add($sleep); $objects.Add($star)

        $g.FillEllipse($glow, 36, 44, 184, 156)
        $g.DrawEllipse($outer, 34, 44, 188, 158)
        $g.DrawEllipse($inner, 56, 66, 144, 114)
        $g.DrawArc($sleep, 74, 112, 48, 26, 8, 168)
        $g.DrawArc($sleep, 118, 104, 58, 30, 8, 168)
        $g.DrawArc($sleep, 92, 135, 76, 34, 8, 168)

        foreach ($p in @(@(78, 70), @(178, 76), @(192, 160))) {
            $x = [float]$p[0]
            $y = [float]$p[1]
            $g.FillPolygon($star, [System.Drawing.PointF[]]@(
                [System.Drawing.PointF]::new($x, $y - 9),
                [System.Drawing.PointF]::new($x + 4, $y - 2),
                [System.Drawing.PointF]::new($x + 12, $y),
                [System.Drawing.PointF]::new($x + 4, $y + 3),
                [System.Drawing.PointF]::new($x, $y + 10),
                [System.Drawing.PointF]::new($x - 4, $y + 3),
                [System.Drawing.PointF]::new($x - 12, $y),
                [System.Drawing.PointF]::new($x - 4, $y - 2)
            ))
        }
    } finally {
        foreach ($object in $objects) { $object.Dispose() }
        $g.Dispose()
    }

    Save-Png $bitmap $RelativePath
    $bitmap.Dispose()
}

function Draw-BedRestorePulse {
    param([string]$RelativePath)

    $surface = New-Bitmap 256 256
    $bitmap = $surface.Bitmap
    $g = $surface.Graphics
    $objects = New-Object System.Collections.Generic.List[System.IDisposable]
    try {
        $glow = New-Brush 34 129 238 192
        $pulse = New-Pen 185 135 238 220 7
        $patch = New-Pen 220 255 226 126 7
        $soft = New-Pen 130 255 255 255 3
        $objects.Add($glow); $objects.Add($pulse); $objects.Add($patch); $objects.Add($soft)

        $g.FillEllipse($glow, 42, 42, 172, 172)
        $g.DrawEllipse($pulse, 36, 36, 184, 184)
        $g.DrawEllipse($soft, 68, 68, 120, 120)
        $g.DrawLine($patch, 128, 76, 128, 180)
        $g.DrawLine($patch, 76, 128, 180, 128)
        $g.DrawArc($soft, 82, 90, 92, 76, 210, 120)
        $g.DrawArc($soft, 82, 90, 92, 76, 30, 120)
    } finally {
        foreach ($object in $objects) { $object.Dispose() }
        $g.Dispose()
    }

    Save-Png $bitmap $RelativePath
    $bitmap.Dispose()
}

function Draw-LitterUrgentMarker {
    param([string]$RelativePath)

    $surface = New-Bitmap 256 256
    $bitmap = $surface.Bitmap
    $g = $surface.Graphics
    $objects = New-Object System.Collections.Generic.List[System.IDisposable]
    try {
        $sand = New-Brush 170 255 193 96
        $rim = New-Pen 230 255 230 154 7
        $urgent = New-Pen 210 255 106 86 7
        $grain = New-Brush 210 255 246 190
        $objects.Add($sand); $objects.Add($rim); $objects.Add($urgent); $objects.Add($grain)

        $diamond = [System.Drawing.PointF[]]@(
            [System.Drawing.PointF]::new(128, 38),
            [System.Drawing.PointF]::new(214, 128),
            [System.Drawing.PointF]::new(128, 218),
            [System.Drawing.PointF]::new(42, 128)
        )
        $g.FillPolygon($sand, $diamond)
        $g.DrawPolygon($rim, $diamond)
        $g.DrawLine($urgent, 128, 72, 128, 142)
        $g.DrawEllipse($urgent, 120, 166, 16, 16)

        foreach ($p in @(@(84, 130), @(101, 104), @(161, 110), @(174, 146), @(113, 176), @(146, 184))) {
            $g.FillEllipse($grain, [int]$p[0] - 5, [int]$p[1] - 5, 10, 10)
        }
    } finally {
        foreach ($object in $objects) { $object.Dispose() }
        $g.Dispose()
    }

    Save-Png $bitmap $RelativePath
    $bitmap.Dispose()
}

function Draw-FeederReadyMarker {
    param([string]$RelativePath)

    $surface = New-Bitmap 256 256
    $bitmap = $surface.Bitmap
    $g = $surface.Graphics
    $objects = New-Object System.Collections.Generic.List[System.IDisposable]
    try {
        $glow = New-Brush 34 255 203 117
        $bowl = New-Pen 225 255 214 145 8
        $food = New-Brush 220 255 174 94
        $cyan = New-Pen 160 117 224 255 4
        $objects.Add($glow); $objects.Add($bowl); $objects.Add($food); $objects.Add($cyan)

        $g.FillEllipse($glow, 42, 52, 172, 148)
        $g.DrawArc($bowl, 68, 100, 120, 88, 0, 180)
        $g.DrawLine($bowl, 76, 144, 180, 144)
        $g.DrawArc($cyan, 78, 58, 100, 50, 18, 144)

        foreach ($p in @(@(102, 126), @(128, 116), @(154, 128), @(116, 146), @(143, 148))) {
            $g.FillEllipse($food, [int]$p[0] - 8, [int]$p[1] - 8, 16, 16)
        }
    } finally {
        foreach ($object in $objects) { $object.Dispose() }
        $g.Dispose()
    }

    Save-Png $bitmap $RelativePath
    $bitmap.Dispose()
}

function Draw-BlockedMarker {
    param([string]$RelativePath)

    $surface = New-Bitmap 256 256
    $bitmap = $surface.Bitmap
    $g = $surface.Graphics
    $objects = New-Object System.Collections.Generic.List[System.IDisposable]
    try {
        $glow = New-Brush 34 255 82 118
        $ring = New-Pen 220 255 93 122 9
        $cross = New-Pen 230 238 196 212 12
        $shadow = New-Pen 150 90 62 104 5
        $objects.Add($glow); $objects.Add($ring); $objects.Add($cross); $objects.Add($shadow)

        $g.FillEllipse($glow, 42, 42, 172, 172)
        $g.DrawEllipse($ring, 48, 48, 160, 160)
        $g.DrawLine($shadow, 82, 82, 174, 174)
        $g.DrawLine($shadow, 174, 82, 82, 174)
        $g.DrawLine($cross, 76, 76, 180, 180)
        $g.DrawLine($cross, 180, 76, 76, 180)
    } finally {
        foreach ($object in $objects) { $object.Dispose() }
        $g.Dispose()
    }

    Save-Png $bitmap $RelativePath
    $bitmap.Dispose()
}

function Draw-RangeRipple {
    param([string]$RelativePath)

    $surface = New-Bitmap 512 512
    $bitmap = $surface.Bitmap
    $g = $surface.Graphics
    $objects = New-Object System.Collections.Generic.List[System.IDisposable]
    try {
        $wide = New-Pen 120 99 221 255 10
        $mid = New-Pen 170 255 224 112 5
        $thin = New-Pen 110 245 252 255 3
        $dot = New-Brush 210 255 240 145
        $objects.Add($wide); $objects.Add($mid); $objects.Add($thin); $objects.Add($dot)

        $g.DrawEllipse($wide, 82, 82, 348, 348)
        $g.DrawEllipse($mid, 134, 134, 244, 244)
        $g.DrawEllipse($thin, 192, 192, 128, 128)

        for ($i = 0; $i -lt 12; $i++) {
            $angle = ($i * 30.0) * [Math]::PI / 180.0
            $x1 = 256 + [Math]::Cos($angle) * 146
            $y1 = 256 + [Math]::Sin($angle) * 146
            $x2 = 256 + [Math]::Cos($angle) * 178
            $y2 = 256 + [Math]::Sin($angle) * 178
            $g.DrawLine($thin, [float]$x1, [float]$y1, [float]$x2, [float]$y2)
        }

        foreach ($p in @(@(256, 98), @(410, 256), @(256, 414), @(102, 256))) {
            $g.FillEllipse($dot, [int]$p[0] - 13, [int]$p[1] - 13, 26, 26)
        }
    } finally {
        foreach ($object in $objects) { $object.Dispose() }
        $g.Dispose()
    }

    Save-Png $bitmap $RelativePath
    $bitmap.Dispose()
}

$candidateSpecs = @(
    @{ AssetId = "thecat_ui_interaction_bed_ready_ring_256_candidate_v001"; SubjectId = "interaction_bed_ready_ring"; Size = "256x256"; Binding = "interaction.bed.ready_ring"; Path = "$batchDirRelative/thecat_ui_interaction_bed_ready_ring_256_candidate_v001.png"; Draw = { Draw-BedReadyRing $args[0] }; Notes = "Moon-blue readiness ring for interactable bed defense and sleep-protect states." },
    @{ AssetId = "thecat_ui_interaction_bed_restore_pulse_256_candidate_v001"; SubjectId = "interaction_bed_restore_pulse"; Size = "256x256"; Binding = "interaction.bed.restore_pulse"; Path = "$batchDirRelative/thecat_ui_interaction_bed_restore_pulse_256_candidate_v001.png"; Draw = { Draw-BedRestorePulse $args[0] }; Notes = "Soft restore pulse for bed repair and owner-sleep recovery feedback." },
    @{ AssetId = "thecat_ui_interaction_litter_urgent_marker_256_candidate_v001"; SubjectId = "interaction_litter_urgent_marker"; Size = "256x256"; Binding = "interaction.litter.urgent_marker"; Path = "$batchDirRelative/thecat_ui_interaction_litter_urgent_marker_256_candidate_v001.png"; Draw = { Draw-LitterUrgentMarker $args[0] }; Notes = "Amber clean-sand urgent marker for poop-pressure interaction prompts." },
    @{ AssetId = "thecat_ui_interaction_feeder_ready_marker_256_candidate_v001"; SubjectId = "interaction_feeder_ready_marker"; Size = "256x256"; Binding = "interaction.feeder.ready_marker"; Path = "$batchDirRelative/thecat_ui_interaction_feeder_ready_marker_256_candidate_v001.png"; Draw = { Draw-FeederReadyMarker $args[0] }; Notes = "Warm feeder-ready marker for hunger recovery prompts." },
    @{ AssetId = "thecat_ui_interaction_blocked_marker_256_candidate_v001"; SubjectId = "interaction_blocked_marker"; Size = "256x256"; Binding = "interaction.blocked_marker"; Path = "$batchDirRelative/thecat_ui_interaction_blocked_marker_256_candidate_v001.png"; Draw = { Draw-BlockedMarker $args[0] }; Notes = "Muted red-violet marker for blocked or invalid interaction states." },
    @{ AssetId = "thecat_ui_interaction_range_ripple_512_candidate_v001"; SubjectId = "interaction_range_ripple"; Size = "512x512"; Binding = "interaction.range_ripple"; Path = "$batchDirRelative/thecat_ui_interaction_range_ripple_512_candidate_v001.png"; Draw = { Draw-RangeRipple $args[0] }; Notes = "Cyan-gold range ripple for valid interaction radius readability." }
)

foreach ($spec in $candidateSpecs) {
    & $spec.Draw $spec.Path
}

$rows = New-Object System.Collections.Generic.List[object]
foreach ($spec in $candidateSpecs) {
    $absolute = Join-Path $projectRoot ($spec.Path -replace "/", [System.IO.Path]::DirectorySeparatorChar)
    $hash = (Get-FileHash -Algorithm SHA256 -LiteralPath $absolute).Hash.ToLowerInvariant()
    $rows.Add([pscustomobject]@{
        asset_id = $spec.AssetId
        subject_id = $spec.SubjectId
        batch_slug = $batchSlug
        asset_type = "bedroom_interaction_affordance_candidate"
        candidate_path = $spec.Path
        candidate_size = $spec.Size
        candidate_sha256 = $hash
        intended_runtime_binding = $spec.Binding
        recommendation = "candidate_review_only_do_not_import"
        notes = $spec.Notes
    })
}

$manifestPath = Join-Path $projectRoot ($manifestRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$rows | Export-Csv -LiteralPath $manifestPath -Encoding UTF8 -NoTypeInformation

$sheet = [System.Drawing.Bitmap]::new(1920, 1080, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$g = [System.Drawing.Graphics]::FromImage($sheet)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$objects = New-Object System.Collections.Generic.List[System.IDisposable]
try {
    $g.Clear([System.Drawing.Color]::FromArgb(255, 19, 22, 35))
    $fontTitle = [System.Drawing.Font]::new("Segoe UI", 30, [System.Drawing.FontStyle]::Bold)
    $fontBody = [System.Drawing.Font]::new("Segoe UI", 15, [System.Drawing.FontStyle]::Regular)
    $fontSmall = [System.Drawing.Font]::new("Segoe UI", 11, [System.Drawing.FontStyle]::Regular)
    $white = New-Brush 255 234 240 255
    $muted = New-Brush 255 159 172 198
    $warm = New-Brush 255 82 58 72
    $dark = New-Brush 255 24 27 43
    $panelPen = New-Pen 210 114 224 255 2
    $objects.Add($fontTitle); $objects.Add($fontBody); $objects.Add($fontSmall)
    $objects.Add($white); $objects.Add($muted); $objects.Add($warm); $objects.Add($dark); $objects.Add($panelPen)

    $g.DrawString("P0 Batch 67 - Bedroom Interaction Affordance Candidates", $fontTitle, $white, 42, 34)
    $g.DrawString("Candidate-only non-cat UI/VFX outside Assets. Bed, litter box, feeder, blocked state, and range readability.", $fontBody, $muted, 46, 84)

    $x = 46
    $y = 145
    foreach ($spec in $candidateSpecs) {
        $candidatePath = Join-Path $projectRoot ($spec.Path -replace "/", [System.IO.Path]::DirectorySeparatorChar)
        $candidate = [System.Drawing.Image]::FromFile($candidatePath)
        try {
            $g.FillRectangle($dark, $x, $y, 280, 300)
            $g.DrawRectangle($panelPen, $x, $y, 280, 300)
            $g.FillRectangle($warm, $x + 15, $y + 24, 115, 115)
            $g.FillRectangle($dark, $x + 150, $y + 24, 115, 115)

            $drawSize = if ($spec.Size -eq "512x512") { 100 } else { 92 }
            $g.DrawImage($candidate, $x + 26, $y + 35, $drawSize, $drawSize)
            $g.DrawImage($candidate, $x + 161, $y + 35, $drawSize, $drawSize)

            $g.DrawString($spec.SubjectId, $fontBody, $white, $x + 16, $y + 168)
            $g.DrawString($spec.Binding, $fontSmall, $muted, $x + 16, $y + 200)
            $g.DrawString($spec.Size, $fontSmall, $muted, $x + 16, $y + 226)
        } finally {
            $candidate.Dispose()
        }

        $x += 304
        if ($x -gt 1600) {
            $x = 46
            $y += 350
        }
    }

    $g.DrawString("Unity gates later: interaction timing screenshots, Console clean, Sprite import settings, scene/prefab binding.", $fontBody, $muted, 46, 1012)
} finally {
    foreach ($object in $objects) { $object.Dispose() }
    $g.Dispose()
}

Save-Png $sheet $reviewSheetRelative
$sheet.Dispose()

$reviewNote = @"
# P0 Batch 67 - Bedroom Interaction Affordance Candidate Review

## Decision

- Candidate pack complete pending Unity review.
- Do not import into ``Assets`` until bedroom interaction screenshots, Console, input timing, Sprite import, and scene/prefab checks pass.
- Non-cat UI/VFX only; no cat body, fur, costume, face, paw, tail, weapon, or colored-turnaround crop is included.

## Review Sheet

- ``$reviewSheetRelative``

## Candidate Rows

"@

foreach ($row in $rows) {
    $reviewNote += @"
### $($row.subject_id)

- Asset: ``$($row.asset_id)``
- Candidate: ``$($row.candidate_path)``
- Size: ``$($row.candidate_size)``
- Binding target: ``$($row.intended_runtime_binding)``
- SHA-256: ``$($row.candidate_sha256)``
- Note: $($row.notes)

"@
}

$reviewNote += @"
## Unity Review Required Later

- Bed ready and restore interaction screenshot.
- Litter box urgent interaction screenshot.
- Feeder ready interaction screenshot.
- Blocked interaction screenshot.
- Range ripple screenshot.
- Console clean.
- Sprite import settings.
- Scene/prefab binding.

## Cat Consistency

This batch is non-cat symbolic UI/VFX only. It does not approve or modify
Saiban, Nephthys, Suzune, future partner cats, starter-cat HUD avatars, or any
colored three-view turnaround source.
"@

Set-Content -LiteralPath (Join-Path $projectRoot ($reviewNoteRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)) -Value $reviewNote -Encoding UTF8

$processNote = @"
# P0 Batch 67 Bedroom Interaction Affordance Process Note

Batch 67 produces candidate-only non-cat UI/VFX for interaction readability.

## Outputs

- Manifest: ``$manifestRelative``
- Review sheet: ``$reviewSheetRelative``
- Review note: ``$reviewNoteRelative``

## Candidate Policy

- All PNGs remain outside ``Assets``.
- No Unity ``.meta`` files are created.
- No manifest/runtime binding baseline changes are made by this generation script.
- No cat body, cat face, cat costume, cat paw, cat tail, weapon silhouette, or colored-turnaround crop is generated.

## Unity Work Later

Formal install remains blocked until Unity interaction screenshots, Console
checks, Sprite import settings, scene/prefab references, and runtime scale
readability pass.
"@

Set-Content -LiteralPath (Join-Path $projectRoot ($processNoteRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)) -Value $processNote -Encoding UTF8

Write-Host "Bedroom interaction affordance Batch 67 candidates generated at $batchDirRelative"
