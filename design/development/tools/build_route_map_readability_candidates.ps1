Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_65_route_map_readability_candidates_2026-06-15"
$batchDirRelative = "design/development/asset_candidates/ui/route_map/$batchSlug"
$batchDir = Join-Path $projectRoot ($batchDirRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
New-Item -ItemType Directory -Force -Path $batchDir | Out-Null

$manifestRelative = "$batchDirRelative/route_map_readability_batch65_manifest.csv"
$reviewSheetRelative = "$batchDirRelative/thecat_ui_route_map_readability_batch65_review_sheet.png"
$reviewNoteRelative = "$batchDirRelative/route_map_readability_batch65_candidate_review.md"
$processNoteRelative = "$batchDirRelative/route_map_readability_batch65_process_note.md"

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
    return $pen
}

function New-Brush {
    param([int]$A, [int]$R, [int]$G, [int]$B)
    return [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb($A, $R, $G, $B))
}

function Draw-NodeHalo {
    param(
        [string]$RelativePath,
        [bool]$Selected
    )

    $surface = New-Bitmap 256 256
    $bitmap = $surface.Bitmap
    $g = $surface.Graphics
    try {
        $outer = if ($Selected) { New-Pen 220 255 233 136 10 } else { New-Pen 180 116 226 255 9 }
        $mid = if ($Selected) { New-Pen 180 255 123 96 5 } else { New-Pen 140 174 118 255 5 }
        $inner = New-Pen 130 255 255 255 2
        $glow = if ($Selected) { New-Brush 34 255 204 112 } else { New-Brush 28 88 208 255 }
        $g.FillEllipse($glow, 42, 42, 172, 172)
        $g.DrawEllipse($outer, 36, 36, 184, 184)
        $g.DrawEllipse($mid, 58, 58, 140, 140)
        $g.DrawEllipse($inner, 82, 82, 92, 92)

        for ($i = 0; $i -lt 8; $i++) {
            $angle = ($i * 45.0) * [Math]::PI / 180.0
            $x1 = 128 + [Math]::Cos($angle) * 80
            $y1 = 128 + [Math]::Sin($angle) * 80
            $x2 = 128 + [Math]::Cos($angle) * 105
            $y2 = 128 + [Math]::Sin($angle) * 105
            $g.DrawLine($mid, [float]$x1, [float]$y1, [float]$x2, [float]$y2)
        }
    } finally {
        $outer.Dispose()
        $mid.Dispose()
        $inner.Dispose()
        $glow.Dispose()
        $g.Dispose()
    }

    Save-Png $bitmap $RelativePath
    $bitmap.Dispose()
}

function Draw-Connector {
    param(
        [string]$RelativePath,
        [string]$Mode
    )

    $surface = New-Bitmap 512 128
    $bitmap = $surface.Bitmap
    $g = $surface.Graphics
    try {
        if ($Mode -eq "available") {
            $basePen = New-Pen 160 80 220 255 12
            $accentPen = New-Pen 230 255 232 126 5
            $dotBrush = New-Brush 220 255 255 255
        } elseif ($Mode -eq "locked") {
            $basePen = New-Pen 95 92 103 128 10
            $accentPen = New-Pen 155 160 170 196 3
            $dotBrush = New-Brush 130 160 170 196
        } else {
            $basePen = New-Pen 155 232 80 140 13
            $accentPen = New-Pen 225 255 116 86 5
            $dotBrush = New-Brush 210 255 235 168
        }

        $points = [System.Drawing.PointF[]]@(
            [System.Drawing.PointF]::new(40, 66),
            [System.Drawing.PointF]::new(145, 46),
            [System.Drawing.PointF]::new(256, 74),
            [System.Drawing.PointF]::new(370, 48),
            [System.Drawing.PointF]::new(472, 66)
        )
        $g.DrawCurve($basePen, $points, 0.45)
        $g.DrawCurve($accentPen, $points, 0.45)

        foreach ($point in $points) {
            $g.FillEllipse($dotBrush, $point.X - 10, $point.Y - 10, 20, 20)
        }

        if ($Mode -eq "locked") {
            $crossPen = New-Pen 180 220 228 240 5
            try {
                $g.DrawLine($crossPen, 238, 46, 274, 82)
                $g.DrawLine($crossPen, 274, 46, 238, 82)
            } finally {
                $crossPen.Dispose()
            }
        }

        if ($Mode -eq "boss") {
            $pulsePen = New-Pen 150 255 80 80 4
            try {
                $g.DrawEllipse($pulsePen, 218, 22, 76, 76)
                $g.DrawEllipse($pulsePen, 202, 6, 108, 108)
            } finally {
                $pulsePen.Dispose()
            }
        }
    } finally {
        $basePen.Dispose()
        $accentPen.Dispose()
        $dotBrush.Dispose()
        $g.Dispose()
    }

    Save-Png $bitmap $RelativePath
    $bitmap.Dispose()
}

$candidateSpecs = @(
    @{ AssetId = "thecat_ui_route_current_node_halo_256_candidate_v001"; SubjectId = "route_current_node_halo"; Size = "256x256"; Binding = "route_map.current_node_halo"; Path = "$batchDirRelative/thecat_ui_route_current_node_halo_256_candidate_v001.png"; Draw = { Draw-NodeHalo $args[0] $false }; Notes = "Current node halo for the active route layer." },
    @{ AssetId = "thecat_ui_route_selected_node_ring_256_candidate_v001"; SubjectId = "route_selected_node_ring"; Size = "256x256"; Binding = "route_map.selected_node_ring"; Path = "$batchDirRelative/thecat_ui_route_selected_node_ring_256_candidate_v001.png"; Draw = { Draw-NodeHalo $args[0] $true }; Notes = "Selected branch ring for route-choice confirmation." },
    @{ AssetId = "thecat_ui_route_available_path_connector_512x128_candidate_v001"; SubjectId = "route_available_path_connector"; Size = "512x128"; Binding = "route_map.available_path_connector"; Path = "$batchDirRelative/thecat_ui_route_available_path_connector_512x128_candidate_v001.png"; Draw = { Draw-Connector $args[0] "available" }; Notes = "Readable connector for available next route options." },
    @{ AssetId = "thecat_ui_route_locked_path_connector_512x128_candidate_v001"; SubjectId = "route_locked_path_connector"; Size = "512x128"; Binding = "route_map.locked_path_connector"; Path = "$batchDirRelative/thecat_ui_route_locked_path_connector_512x128_candidate_v001.png"; Draw = { Draw-Connector $args[0] "locked" }; Notes = "Muted connector for unavailable or future route path preview." },
    @{ AssetId = "thecat_ui_route_boss_path_pressure_512x128_candidate_v001"; SubjectId = "route_boss_path_pressure"; Size = "512x128"; Binding = "route_map.boss_path_pressure"; Path = "$batchDirRelative/thecat_ui_route_boss_path_pressure_512x128_candidate_v001.png"; Draw = { Draw-Connector $args[0] "boss" }; Notes = "Boss path pressure accent for the layer-ten Call Tyrant route." }
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
        asset_type = "route_map_readability_candidate"
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

$sheet = [System.Drawing.Bitmap]::new(1860, 860, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$g = [System.Drawing.Graphics]::FromImage($sheet)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
try {
    $g.Clear([System.Drawing.Color]::FromArgb(255, 18, 21, 34))
    $fontTitle = [System.Drawing.Font]::new("Segoe UI", 30, [System.Drawing.FontStyle]::Bold)
    $fontBody = [System.Drawing.Font]::new("Segoe UI", 16, [System.Drawing.FontStyle]::Regular)
    $fontSmall = [System.Drawing.Font]::new("Segoe UI", 12, [System.Drawing.FontStyle]::Regular)
    $white = New-Brush 255 232 238 255
    $muted = New-Brush 255 160 171 196
    $panelBrush = New-Brush 255 32 38 56
    $panelPen = New-Pen 220 116 226 255 2

    $g.DrawString("P0 Batch 65 - Route Map Readability Candidates", $fontTitle, $white, 42, 34)
    $g.DrawString("Candidate-only UI packet outside Assets. No cat body, fur, costume, or turnaround crop.", $fontBody, $muted, 46, 82)

    $x = 46
    $y = 142
    foreach ($spec in $candidateSpecs) {
        $candidatePath = Join-Path $projectRoot ($spec.Path -replace "/", [System.IO.Path]::DirectorySeparatorChar)
        $candidate = [System.Drawing.Image]::FromFile($candidatePath)
        try {
            $g.FillRectangle($panelBrush, $x, $y, 330, 270)
            $g.DrawRectangle($panelPen, $x, $y, 330, 270)
            $targetWidth = if ($spec.Size -eq "512x128") { 260 } else { 154 }
            $targetHeight = if ($spec.Size -eq "512x128") { 65 } else { 154 }
            $targetX = $x + [int]((330 - $targetWidth) / 2)
            $targetY = $y + 34
            $g.DrawImage($candidate, $targetX, $targetY, $targetWidth, $targetHeight)
            $g.DrawString($spec.SubjectId, $fontBody, $white, $x + 18, $y + 204)
            $g.DrawString($spec.Binding, $fontSmall, $muted, $x + 18, $y + 236)
        } finally {
            $candidate.Dispose()
        }

        $x += 358
        if ($x -gt 1500) {
            $x = 46
            $y += 320
        }
    }

    $g.DrawString("Pending Unity checks: route-map scale, selected/current contrast, path readability, Boss pressure readability, Console clean.", $fontBody, $muted, 46, 792)
} finally {
    $fontTitle.Dispose()
    $fontBody.Dispose()
    $fontSmall.Dispose()
    $white.Dispose()
    $muted.Dispose()
    $panelBrush.Dispose()
    $panelPen.Dispose()
    $g.Dispose()
}

Save-Png $sheet $reviewSheetRelative
$sheet.Dispose()

$reviewNote = @"
# P0 Batch 65 - Route Map Readability Candidate Review

## Decision

- Candidate pack complete pending Unity review.
- Do not import into ``Assets`` until route-map scale, Console, screenshot, and route selection readability checks pass.
- Non-cat UI only; no starter-cat body, fur, costume, prop, or colored-turnaround crop is included.

## Review Sheet

- ``$reviewSheetRelative``

## Candidate Rows

$(
    ($rows | ForEach-Object { "- ``$($_.asset_id)`` -> ``$($_.candidate_path)`` binding hint ``$($_.intended_runtime_binding)``" }) -join "`n"
)

## Pending Unity Checks

- Current layer halo remains readable over route node icons.
- Selected branch ring is distinct from the current-node halo.
- Available, locked, and Boss path connectors remain legible at route-map scale.
- Boss path pressure accent does not obscure the Call Tyrant node icon.
- Console has no missing texture, import, or IMGUI layout errors.
"@

$reviewNotePath = Join-Path $projectRoot ($reviewNoteRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
Set-Content -LiteralPath $reviewNotePath -Value $reviewNote -Encoding UTF8

$processNote = @"
# P0 Batch 65 Route Map Readability Process Note

- Batch slug: ``$batchSlug``
- Output directory: ``$batchDirRelative``
- Production method: deterministic System.Drawing generation from project UI color language.
- Candidate-only status: true.
- Unity import status: blocked until human/Unity screenshot review approves specific assets.
- Cat consistency impact: none. This batch does not read, generate, crop, recolor, or route starter-cat body art.
- No Unity ``.meta`` files should exist in this candidate folder.
"@

$processNotePath = Join-Path $projectRoot ($processNoteRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
Set-Content -LiteralPath $processNotePath -Value $processNote -Encoding UTF8

Write-Host "Route map readability Batch 65 candidates generated at $batchDirRelative"
