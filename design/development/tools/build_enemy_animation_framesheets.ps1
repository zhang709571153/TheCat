Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$outputDir = Join-Path $projectRoot "Assets/TheCat/Art/Enemies/Frames"
New-Item -ItemType Directory -Force -Path $outputDir | Out-Null

function New-Rect {
    param([int]$X, [int]$Y, [int]$Width, [int]$Height)
    return [System.Drawing.Rectangle]::new($X, $Y, $Width, $Height)
}

function New-CenteredRect {
    param(
        [int]$CenterX,
        [int]$CenterY,
        [int]$Width,
        [int]$Height,
        [int]$ImageWidth,
        [int]$ImageHeight
    )

    $x = [Math]::Max(0, [int][Math]::Round($CenterX - ($Width / 2)))
    $y = [Math]::Max(0, [int][Math]::Round($CenterY - ($Height / 2)))
    if ($x + $Width -gt $ImageWidth) {
        $x = $ImageWidth - $Width
    }

    if ($y + $Height -gt $ImageHeight) {
        $y = $ImageHeight - $Height
    }

    return New-Rect $x $y $Width $Height
}

function New-GridRect {
    param(
        [int]$ImageWidth,
        [int]$ImageHeight,
        [int]$Columns,
        [int]$Rows,
        [int]$Column,
        [int]$Row
    )

    $cellWidth = $ImageWidth / $Columns
    $cellHeight = $ImageHeight / $Rows
    $x = [int][Math]::Round($cellWidth * $Column)
    $y = [int][Math]::Round($cellHeight * $Row)
    $nextX = [int][Math]::Round($cellWidth * ($Column + 1))
    $nextY = [int][Math]::Round($cellHeight * ($Row + 1))
    return New-Rect $x $y ($nextX - $x) ($nextY - $y)
}

function Test-BackgroundPixel {
    param([System.Drawing.Color]$Color)

    $maxDelta = [Math]::Max([Math]::Abs($Color.R - $Color.G), [Math]::Abs($Color.G - $Color.B))
    return $Color.R -ge 176 -and $Color.G -ge 176 -and $Color.B -ge 176 -and $maxDelta -le 24
}

function Convert-BackgroundToTransparent {
    param([System.Drawing.Bitmap]$Bitmap)

    for ($y = 0; $y -lt $Bitmap.Height; $y++) {
        for ($x = 0; $x -lt $Bitmap.Width; $x++) {
            $pixel = $Bitmap.GetPixel($x, $y)
            if (Test-BackgroundPixel $pixel) {
                $Bitmap.SetPixel($x, $y, [System.Drawing.Color]::Transparent)
            }
        }
    }
}

function Clear-FrameEdgeArtifacts {
    param([System.Drawing.Bitmap]$Bitmap)

    $topArtifactBand = [Math]::Min(42, $Bitmap.Height)
    for ($y = 0; $y -lt $topArtifactBand; $y++) {
        for ($x = 0; $x -lt $Bitmap.Width; $x++) {
            $Bitmap.SetPixel($x, $y, [System.Drawing.Color]::Transparent)
        }
    }

    $edgeBand = [Math]::Min(6, [int]($Bitmap.Width / 2))
    for ($y = 0; $y -lt $Bitmap.Height; $y++) {
        for ($x = 0; $x -lt $edgeBand; $x++) {
            $Bitmap.SetPixel($x, $y, [System.Drawing.Color]::Transparent)
            $Bitmap.SetPixel($Bitmap.Width - 1 - $x, $y, [System.Drawing.Color]::Transparent)
        }
    }
}

function Remove-DetachedArtifacts {
    param([System.Drawing.Bitmap]$Bitmap)

    $width = $Bitmap.Width
    $height = $Bitmap.Height
    $visited = New-Object bool[] ($width * $height)
    $components = New-Object System.Collections.Generic.List[object]

    for ($startY = 0; $startY -lt $height; $startY++) {
        for ($startX = 0; $startX -lt $width; $startX++) {
            $startIndex = ($startY * $width) + $startX
            if ($visited[$startIndex] -or $Bitmap.GetPixel($startX, $startY).A -le 16) {
                continue
            }

            $queue = New-Object System.Collections.Generic.Queue[int]
            $pixels = New-Object System.Collections.Generic.List[int]
            $queue.Enqueue($startIndex)
            $visited[$startIndex] = $true
            $minX = $startX
            $maxX = $startX
            $minY = $startY
            $maxY = $startY

            while ($queue.Count -gt 0) {
                $index = $queue.Dequeue()
                $pixels.Add($index)
                $x = $index % $width
                $y = [int][Math]::Floor($index / $width)

                if ($x -lt $minX) { $minX = $x }
                if ($x -gt $maxX) { $maxX = $x }
                if ($y -lt $minY) { $minY = $y }
                if ($y -gt $maxY) { $maxY = $y }

                for ($ny = $y - 1; $ny -le $y + 1; $ny++) {
                    for ($nx = $x - 1; $nx -le $x + 1; $nx++) {
                        if ($nx -lt 0 -or $ny -lt 0 -or $nx -ge $width -or $ny -ge $height) {
                            continue
                        }

                        $neighborIndex = ($ny * $width) + $nx
                        if ($visited[$neighborIndex] -or $Bitmap.GetPixel($nx, $ny).A -le 16) {
                            continue
                        }

                        $visited[$neighborIndex] = $true
                        $queue.Enqueue($neighborIndex)
                    }
                }
            }

            $components.Add([pscustomobject]@{
                Pixels = $pixels
                Area = $pixels.Count
                MinX = $minX
                MaxX = $maxX
                MinY = $minY
                MaxY = $maxY
            })
        }
    }

    if ($components.Count -le 1) {
        return
    }

    $largest = $components | Sort-Object Area -Descending | Select-Object -First 1
    $padding = 42
    $keepMinX = [Math]::Max(0, $largest.MinX - $padding)
    $keepMaxX = [Math]::Min($width - 1, $largest.MaxX + $padding)
    $keepMinY = [Math]::Max(0, $largest.MinY - $padding)
    $keepMaxY = [Math]::Min($height - 1, $largest.MaxY + $padding)

    foreach ($component in $components) {
        if ([object]::ReferenceEquals($component, $largest)) {
            continue
        }

        $intersectsMainArea = $component.MaxX -ge $keepMinX -and
            $component.MinX -le $keepMaxX -and
            $component.MaxY -ge $keepMinY -and
            $component.MinY -le $keepMaxY

        if ($intersectsMainArea) {
            continue
        }

        foreach ($index in $component.Pixels) {
            $x = $index % $width
            $y = [int][Math]::Floor($index / $width)
            $Bitmap.SetPixel($x, $y, [System.Drawing.Color]::Transparent)
        }
    }
}

function Get-VisibleBounds {
    param([System.Drawing.Bitmap]$Bitmap)

    $minX = $Bitmap.Width
    $minY = $Bitmap.Height
    $maxX = -1
    $maxY = -1

    for ($y = 0; $y -lt $Bitmap.Height; $y++) {
        for ($x = 0; $x -lt $Bitmap.Width; $x++) {
            if ($Bitmap.GetPixel($x, $y).A -gt 16) {
                if ($x -lt $minX) { $minX = $x }
                if ($x -gt $maxX) { $maxX = $x }
                if ($y -lt $minY) { $minY = $y }
                if ($y -gt $maxY) { $maxY = $y }
            }
        }
    }

    if ($maxX -lt $minX -or $maxY -lt $minY) {
        return New-Rect 0 0 $Bitmap.Width $Bitmap.Height
    }

    $padding = 4
    $minX = [Math]::Max(0, $minX - $padding)
    $minY = [Math]::Max(0, $minY - $padding)
    $maxX = [Math]::Min($Bitmap.Width - 1, $maxX + $padding)
    $maxY = [Math]::Min($Bitmap.Height - 1, $maxY + $padding)
    return New-Rect $minX $minY ($maxX - $minX + 1) ($maxY - $minY + 1)
}

function Copy-FrameToSheet {
    param(
        [System.Drawing.Bitmap]$Source,
        [System.Drawing.Rectangle]$SourceRect,
        [System.Drawing.Graphics]$Graphics,
        [int]$FrameIndex
    )

    $raw = [System.Drawing.Bitmap]::new($SourceRect.Width, $SourceRect.Height, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $rawGraphics = [System.Drawing.Graphics]::FromImage($raw)
    try {
        $rawGraphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
        $rawGraphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
        $rawGraphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $rawGraphics.Clear([System.Drawing.Color]::Transparent)
        $rawGraphics.DrawImage($Source, 0, 0, $SourceRect, [System.Drawing.GraphicsUnit]::Pixel)
    } finally {
        $rawGraphics.Dispose()
    }

    try {
        Convert-BackgroundToTransparent $raw
        Clear-FrameEdgeArtifacts $raw
        Remove-DetachedArtifacts $raw
        $bounds = Get-VisibleBounds $raw
        $maxDraw = 224
        $scale = [Math]::Min($maxDraw / $bounds.Width, $maxDraw / $bounds.Height)
        $drawWidth = [int][Math]::Round($bounds.Width * $scale)
        $drawHeight = [int][Math]::Round($bounds.Height * $scale)
        $drawX = ($FrameIndex * 256) + [int][Math]::Round((256 - $drawWidth) / 2)
        $drawY = [int][Math]::Round((256 - $drawHeight) / 2)
        $dest = New-Rect $drawX $drawY $drawWidth $drawHeight
        $Graphics.DrawImage($raw, $dest, $bounds, [System.Drawing.GraphicsUnit]::Pixel)
    } finally {
        $raw.Dispose()
    }
}

function Write-Meta {
    param([string]$Path, [string]$Guid, [int]$MaxTextureSize)

    $content = @"
fileFormatVersion: 2
guid: $Guid
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {}
  serializedVersion: 13
  mipmaps:
    mipMapMode: 0
    enableMipMap: 0
    sRGBTexture: 1
    linearTexture: 0
    fadeOut: 0
    borderMipMap: 0
    mipMapsPreserveCoverage: 0
    alphaTestReferenceValue: 0.5
    mipMapFadeDistanceStart: 1
    mipMapFadeDistanceEnd: 3
  bumpmap:
    convertToNormalMap: 0
    externalNormalMap: 0
    heightScale: 0.25
    normalMapFilter: 0
    flipGreenChannel: 0
  isReadable: 0
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  vTOnly: 0
  ignoreMipmapLimit: 0
  grayScaleToAlpha: 0
  generateCubemap: 6
  cubemapConvolution: 0
  seamlessCubemap: 0
  textureFormat: 1
  maxTextureSize: $MaxTextureSize
  textureSettings:
    serializedVersion: 2
    filterMode: 1
    aniso: 1
    mipBias: 0
    wrapU: 0
    wrapV: 0
    wrapW: 0
  nPOTScale: 0
  lightmap: 0
  compressionQuality: 50
  spriteMode: 1
  spriteExtrude: 1
  spriteMeshType: 1
  alignment: 0
  spritePivot: {x: 0.5, y: 0.5}
  spriteBorder: {x: 0, y: 0, z: 0, w: 0}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 1
  spriteTessellationDetail: -1
  textureType: 8
  textureShape: 1
  singleChannelComponent: 0
  flipbookRows: 1
  flipbookColumns: 4
  maxTextureSizeSet: 0
  compressionQualitySet: 0
  textureFormatSet: 0
  ignorePngGamma: 0
  applyGammaDecoding: 0
  swizzle: 50462976
  cookieLightType: 0
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: $MaxTextureSize
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 0
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
  userData: TheCatP0ImportSettings:v1
  assetBundleName: 
  assetBundleVariant: 
"@
    Set-Content -LiteralPath ($Path + ".meta") -Value $content -Encoding UTF8
}

function New-FrameSheet {
    param(
        [string]$SourcePath,
        [System.Drawing.Rectangle[]]$Frames,
        [string]$OutputFileName,
        [string]$Guid,
        [int]$FinalTopClearPixels = 0
    )

    if (-not (Test-Path -LiteralPath $SourcePath)) {
        throw "Missing source animation: $SourcePath"
    }

    $source = [System.Drawing.Bitmap]::FromFile($SourcePath)
    $sheet = [System.Drawing.Bitmap]::new(1024, 256, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($sheet)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
        $graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.Clear([System.Drawing.Color]::Transparent)

        for ($i = 0; $i -lt $Frames.Count; $i++) {
            Copy-FrameToSheet $source $Frames[$i] $graphics $i
        }

        if ($FinalTopClearPixels -gt 0) {
            for ($y = 0; $y -lt $FinalTopClearPixels; $y++) {
                for ($x = 0; $x -lt $sheet.Width; $x++) {
                    $sheet.SetPixel($x, $y, [System.Drawing.Color]::Transparent)
                }
            }
        }

        $outputPath = Join-Path $outputDir $OutputFileName
        $sheet.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
        Write-Meta $outputPath $Guid 1024
        Write-Host "Generated $OutputFileName"
    } finally {
        $graphics.Dispose()
        $sheet.Dispose()
        $source.Dispose()
    }
}

$designRoot = Get-ChildItem -LiteralPath (Join-Path $projectRoot "design") -Directory |
    Where-Object { Test-Path -LiteralPath (Join-Path $_.FullName "assets/enemies") } |
    Select-Object -First 1

if ($null -eq $designRoot) {
    throw "Could not find design asset root with assets/enemies."
}

$blackMudSource = Join-Path $designRoot.FullName "assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png"
$coldLightSource = Join-Path $designRoot.FullName "assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png"
$callTyrantSource = Join-Path $designRoot.FullName "assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png"

New-FrameSheet $blackMudSource @(
    (New-GridRect 1024 1536 5 6 0 1),
    (New-GridRect 1024 1536 5 6 1 1),
    (New-GridRect 1024 1536 5 6 2 1),
    (New-GridRect 1024 1536 5 6 3 1)
) "thecat_enemy_blackmud_move_framesheet_4x256_v001.png" "dc1806735efa47f899cb4a23ff421101"

New-FrameSheet $coldLightSource @(
    (New-CenteredRect 112 430 200 210 1536 1024),
    (New-CenteredRect 332 430 200 210 1536 1024),
    (New-CenteredRect 552 430 200 210 1536 1024),
    (New-CenteredRect 772 430 200 210 1536 1024)
) "thecat_enemy_coldlight_cast_framesheet_4x256_v001.png" "8b66fdac58924b51a5781053d120fbe2"

New-FrameSheet $callTyrantSource @(
    (New-CenteredRect 128 630 230 210 1536 1024),
    (New-CenteredRect 384 630 230 210 1536 1024),
    (New-CenteredRect 640 630 230 210 1536 1024),
    (New-CenteredRect 896 630 230 210 1536 1024)
) "thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001.png" "f0d183a3c36f461fa74c83d4bc2e1189" 72
