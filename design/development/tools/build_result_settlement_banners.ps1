Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$bannerDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Banners"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_result_settlement_banners_2026-06-14.md"
$batchId = "p0_asset_batch_25_result_settlement_banners"

New-Item -ItemType Directory -Force -Path $bannerDir | Out-Null
New-Item -ItemType Directory -Force -Path $reviewDir | Out-Null

function Get-HexMd5 {
    param([string]$Value)

    $md5 = [System.Security.Cryptography.MD5]::Create()
    try {
        $bytes = [System.Text.Encoding]::UTF8.GetBytes($Value)
        $hash = $md5.ComputeHash($bytes)
        return -join ($hash | ForEach-Object { $_.ToString("x2") })
    }
    finally {
        $md5.Dispose()
    }
}

function New-Color {
    param([int]$A, [int]$R, [int]$G, [int]$B)
    return [System.Drawing.Color]::FromArgb($A, $R, $G, $B)
}

function New-Brush {
    param([System.Drawing.Color]$Color)
    return [System.Drawing.SolidBrush]::new($Color)
}

function New-Pen {
    param([System.Drawing.Color]$Color, [float]$Width = 1)

    $pen = [System.Drawing.Pen]::new($Color, $Width)
    $pen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $pen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $pen.LineJoin = [System.Drawing.Drawing2D.LineJoin]::Round
    return $pen
}

function Draw-RoundedRect {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.RectangleF]$Rect,
        [float]$Radius,
        [System.Drawing.Brush]$Brush,
        [System.Drawing.Pen]$Pen = $null
    )

    $path = [System.Drawing.Drawing2D.GraphicsPath]::new()
    try {
        $diameter = $Radius * 2
        $path.AddArc($Rect.X, $Rect.Y, $diameter, $diameter, 180, 90)
        $path.AddArc($Rect.Right - $diameter, $Rect.Y, $diameter, $diameter, 270, 90)
        $path.AddArc($Rect.Right - $diameter, $Rect.Bottom - $diameter, $diameter, $diameter, 0, 90)
        $path.AddArc($Rect.X, $Rect.Bottom - $diameter, $diameter, $diameter, 90, 90)
        $path.CloseFigure()

        if ($Brush -ne $null) {
            $Graphics.FillPath($Brush, $path)
        }

        if ($Pen -ne $null) {
            $Graphics.DrawPath($Pen, $path)
        }
    }
    finally {
        $path.Dispose()
    }
}

function Draw-Star {
    param(
        [System.Drawing.Graphics]$Graphics,
        [float]$CenterX,
        [float]$CenterY,
        [float]$OuterRadius,
        [float]$InnerRadius,
        [System.Drawing.Brush]$Brush
    )

    $points = New-Object System.Collections.Generic.List[System.Drawing.PointF]
    for ($i = 0; $i -lt 10; $i++) {
        $angle = (-90 + $i * 36) * [Math]::PI / 180
        $radius = if (($i % 2) -eq 0) { $OuterRadius } else { $InnerRadius }
        $points.Add([System.Drawing.PointF]::new(
            $CenterX + [float]([Math]::Cos($angle) * $radius),
            $CenterY + [float]([Math]::Sin($angle) * $radius)))
    }

    $Graphics.FillPolygon($Brush, $points.ToArray())
}

function Draw-BannerBase {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2
    )

    $shadow = New-Brush (New-Color 74 3 5 18)
    $body = New-Brush (New-Color 226 19 23 54)
    $inner = New-Brush (New-Color 92 58 54 104)
    $glowA = New-Brush (New-Color 62 $Accent.R $Accent.G $Accent.B)
    $glowB = New-Brush (New-Color 46 $Accent2.R $Accent2.G $Accent2.B)
    $rim = New-Pen (New-Color 226 $Accent.R $Accent.G $Accent.B) 3
    $hairline = New-Pen (New-Color 128 $Accent2.R $Accent2.G $Accent2.B) 1.5
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(10, 18, 492, 124)) 28 $shadow $null
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(11, 13, 490, 124)) 28 $body $rim
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(30, 31, 452, 88)) 20 $inner $hairline
        $Graphics.FillEllipse($glowA, -35, -35, 188, 160)
        $Graphics.FillEllipse($glowB, 330, -18, 210, 160)
        $Graphics.DrawBezier($hairline, 132, 48, 212, 24, 308, 67, 458, 43)
        $Graphics.DrawBezier($hairline, 118, 114, 220, 132, 325, 82, 468, 110)
    }
    finally {
        $shadow.Dispose()
        $body.Dispose()
        $inner.Dispose()
        $glowA.Dispose()
        $glowB.Dispose()
        $rim.Dispose()
        $hairline.Dispose()
    }
}

function Draw-Bedline {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Pen]$Pen,
        [bool]$Cracked
    )

    $Graphics.DrawLine($Pen, 80, 89, 278, 89)
    $Graphics.DrawLine($Pen, 96, 72, 190, 72)
    $Graphics.DrawLine($Pen, 96, 72, 80, 89)
    $Graphics.DrawLine($Pen, 190, 72, 278, 89)

    if ($Cracked) {
        $Graphics.DrawLine($Pen, 176, 68, 162, 87)
        $Graphics.DrawLine($Pen, 162, 87, 183, 100)
        $Graphics.DrawLine($Pen, 230, 82, 216, 96)
        $Graphics.DrawLine($Pen, 216, 96, 238, 108)
    }
}

function Draw-VictoryMotif {
    param([System.Drawing.Graphics]$Graphics)

    $cyan = New-Brush (New-Color 220 123 224 238)
    $gold = New-Brush (New-Color 226 238 202 92)
    $violet = New-Brush (New-Color 142 167 122 238)
    $cyanPen = New-Pen (New-Color 232 123 224 238) 5
    $goldPen = New-Pen (New-Color 232 238 202 92) 4
    try {
        Draw-Bedline $Graphics $cyanPen $false
        $Graphics.DrawArc($goldPen, 55, 44, 86, 80, 210, 250)
        $Graphics.DrawBezier($cyanPen, 296, 88, 335, 58, 392, 59, 438, 88)
        $Graphics.DrawBezier($cyanPen, 306, 105, 350, 84, 402, 84, 450, 105)
        Draw-Star $Graphics 318 48 11 5 $gold
        Draw-Star $Graphics 382 103 9 4 $cyan
        Draw-Star $Graphics 448 57 8 4 $violet
        $Graphics.FillEllipse($gold, 392, 39, 13, 13)
        $Graphics.FillEllipse($cyan, 424, 74, 11, 11)
    }
    finally {
        $cyan.Dispose()
        $gold.Dispose()
        $violet.Dispose()
        $cyanPen.Dispose()
        $goldPen.Dispose()
    }
}

function Draw-DefeatMotif {
    param([System.Drawing.Graphics]$Graphics)

    $red = New-Brush (New-Color 180 246 86 104)
    $violet = New-Brush (New-Color 128 168 120 240)
    $redPen = New-Pen (New-Color 226 246 86 104) 5
    $violetPen = New-Pen (New-Color 184 168 120 240) 4
    try {
        Draw-Bedline $Graphics $redPen $true
        $Graphics.DrawArc($violetPen, 62, 41, 92, 88, 60, 250)
        $Graphics.DrawArc($redPen, 303, 41, 74, 74, 205, 260)
        $Graphics.DrawLine($redPen, 365, 48, 410, 105)
        $Graphics.DrawLine($redPen, 410, 48, 365, 105)
        $Graphics.DrawBezier($violetPen, 304, 107, 348, 82, 405, 84, 450, 108)
        Draw-Star $Graphics 284 53 8 4 $red
        Draw-Star $Graphics 448 58 8 4 $violet
        $Graphics.FillEllipse($red, 430, 85, 13, 13)
    }
    finally {
        $red.Dispose()
        $violet.Dispose()
        $redPen.Dispose()
        $violetPen.Dispose()
    }
}

function Draw-ClearedMotif {
    param([System.Drawing.Graphics]$Graphics)

    $cyan = New-Brush (New-Color 220 123 224 238)
    $gold = New-Brush (New-Color 230 238 202 92)
    $pink = New-Brush (New-Color 166 241 126 185)
    $cyanPen = New-Pen (New-Color 228 123 224 238) 5
    $goldPen = New-Pen (New-Color 232 238 202 92) 4
    $pinkPen = New-Pen (New-Color 210 241 126 185) 4
    try {
        $lastX = 66
        $lastY = 91
        for ($i = 0; $i -lt 10; $i++) {
            $x = 66 + ($i * 28)
            $y = if (($i % 2) -eq 0) { 91 } else { 65 }
            if ($i -gt 0) {
                $Graphics.DrawLine($cyanPen, $lastX, $lastY, $x, $y)
            }

            $Graphics.FillEllipse($cyan, $x - 6, $y - 6, 12, 12)
            $lastX = $x
            $lastY = $y
        }

        $Graphics.DrawEllipse($goldPen, 354, 42, 80, 80)
        $Graphics.DrawLine($pinkPen, 372, 64, 416, 102)
        $Graphics.DrawLine($pinkPen, 416, 64, 372, 102)
        Draw-Star $Graphics 338 54 11 5 $gold
        Draw-Star $Graphics 447 63 10 5 $cyan
        $Graphics.FillEllipse($gold, 438, 93, 16, 16)
        $Graphics.FillEllipse($pink, 465, 90, 12, 12)
    }
    finally {
        $cyan.Dispose()
        $gold.Dispose()
        $pink.Dispose()
        $cyanPen.Dispose()
        $goldPen.Dispose()
        $pinkPen.Dispose()
    }
}

function Draw-FailedMotif {
    param([System.Drawing.Graphics]$Graphics)

    $red = New-Brush (New-Color 184 246 86 104)
    $violet = New-Brush (New-Color 134 168 120 240)
    $redPen = New-Pen (New-Color 226 246 86 104) 5
    $violetPen = New-Pen (New-Color 190 168 120 240) 4
    try {
        $points = @(
            [System.Drawing.PointF]::new(64, 92),
            [System.Drawing.PointF]::new(112, 64),
            [System.Drawing.PointF]::new(157, 88),
            [System.Drawing.PointF]::new(203, 66),
            [System.Drawing.PointF]::new(247, 96)
        )
        $Graphics.DrawLines($violetPen, $points)
        $Graphics.DrawLine($redPen, 170, 55, 145, 106)
        $Graphics.DrawLine($redPen, 145, 55, 170, 106)
        Draw-Bedline $Graphics $redPen $true
        $Graphics.DrawArc($violetPen, 326, 43, 86, 78, 225, 245)
        $Graphics.DrawBezier($redPen, 323, 103, 364, 80, 413, 83, 454, 104)
        Draw-Star $Graphics 431 55 9 4 $red
        $Graphics.FillEllipse($violet, 454, 87, 13, 13)
    }
    finally {
        $red.Dispose()
        $violet.Dispose()
        $redPen.Dispose()
        $violetPen.Dispose()
    }
}

function Write-SpriteMeta {
    param(
        [string]$AssetPath,
        [string]$AssetId
    )

    $guid = Get-HexMd5 ("TheCat:" + $AssetId + ":" + $batchId)
    $spriteId = (Get-HexMd5 ("sprite:" + $AssetId)).Substring(0, 24)
    $metaPath = $AssetPath + ".meta"
    $content = @"
fileFormatVersion: 2
guid: $guid
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
  maxTextureSize: 512
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
  spriteBorder: {x: 16, y: 16, z: 16, w: 16}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 1
  spriteTessellationDetail: -1
  textureType: 8
  textureShape: 1
  singleChannelComponent: 0
  flipbookRows: 1
  flipbookColumns: 1
  maxTextureSizeSet: 0
  compressionQualitySet: 0
  textureFormatSet: 0
  ignorePngGamma: 0
  applyGammaDecoding: 0
  cookieLightType: 0
  platformSettings:
  - serializedVersion: 4
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 512
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 0
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    physicsShape: []
    bones: []
    spriteID: $spriteId
    internalID: 0
    vertices: []
    indices:
    edges: []
    weights:
    secondaryTextures: []
    nameFileIdTable: {}
  spritePackingTag:
  pSDRemoveMatte: 0
  pSDShowRemoveMatteOption: 0
  userData: TheCatP0ImportSettings:v1; batch:$batchId; spriteBorder:16; nonCatSymbolicOnly:true
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -Path $metaPath -Value $content -Encoding UTF8
}

function New-OutcomeBanner {
    param(
        [string]$AssetId,
        [scriptblock]$Motif,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2
    )

    $path = Join-Path $bannerDir ($AssetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(512, 160, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.Clear([System.Drawing.Color]::Transparent)
        Draw-BannerBase $graphics $Accent $Accent2
        & $Motif $graphics
        $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    Write-SpriteMeta $path $AssetId
    return $path
}

$outputs = @()
$outputs += New-OutcomeBanner "thecat_ui_battle_result_victory_banner_512x160_v001" ${function:Draw-VictoryMotif} (New-Color 255 123 224 238) (New-Color 255 238 202 92)
$outputs += New-OutcomeBanner "thecat_ui_battle_result_defeat_banner_512x160_v001" ${function:Draw-DefeatMotif} (New-Color 255 246 86 104) (New-Color 255 168 120 240)
$outputs += New-OutcomeBanner "thecat_ui_settlement_run_cleared_banner_512x160_v001" ${function:Draw-ClearedMotif} (New-Color 255 123 224 238) (New-Color 255 241 126 185)
$outputs += New-OutcomeBanner "thecat_ui_settlement_run_failed_banner_512x160_v001" ${function:Draw-FailedMotif} (New-Color 255 246 86 104) (New-Color 255 168 120 240)

$review = @'
# P0 Result Settlement Banners Review

Generated: 2026-06-14

| Asset | Subject | Path | Notes |
| --- | --- | --- | --- |
| thecat_ui_battle_result_victory_banner_512x160_v001 | battle_result_victory | Assets/TheCat/Art/UI/Banners/thecat_ui_battle_result_victory_banner_512x160_v001.png | Non-cat battle victory banner using protected bedline, sleep-stable wave, and reward glints. |
| thecat_ui_battle_result_defeat_banner_512x160_v001 | battle_result_defeat | Assets/TheCat/Art/UI/Banners/thecat_ui_battle_result_defeat_banner_512x160_v001.png | Non-cat battle defeat banner using cracked bedline, dim sleep crescent, and pressure-warning arcs. |
| thecat_ui_settlement_run_cleared_banner_512x160_v001 | settlement_run_cleared | Assets/TheCat/Art/UI/Banners/thecat_ui_settlement_run_cleared_banner_512x160_v001.png | Non-cat cleared-run settlement banner using ten-layer path, Boss seal break, fish treat, and dream shard symbols. |
| thecat_ui_settlement_run_failed_banner_512x160_v001 | settlement_run_failed | Assets/TheCat/Art/UI/Banners/thecat_ui_settlement_run_failed_banner_512x160_v001.png | Non-cat failed-run settlement banner using broken route path, dim bedline, and unresolved Boss pressure symbols. |

## Consistency

- Uses the existing dreamglass panel language, P0 status glow palette, reward icon palette, and route/Boss symbolic vocabulary.
- Contains no cat body, face, ears, paws, tails, fur markings, collars, costumes, or character portraits.
- Does not derive from Saiban, Nephthys, Suzune, or their colored three-view turnarounds.
- Image pixels contain no UI text; runtime labels remain controlled by presenter rows.

## Import

- TextureImporter.textureType: 8
- spriteMode: 1
- spriteBorder: 16
- userData includes batch:p0_asset_batch_25_result_settlement_banners and nonCatSymbolicOnly:true
'@

Set-Content -Path $reviewNotePath -Value $review -Encoding UTF8

Write-Host "Generated result settlement banners:"
$outputs | ForEach-Object { Write-Host $_ }
Write-Host $reviewNotePath
