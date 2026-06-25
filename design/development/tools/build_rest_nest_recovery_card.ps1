Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$cardDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Cards"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_rest_nest_recovery_card_2026-06-14.md"

New-Item -ItemType Directory -Force -Path $cardDir | Out-Null
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

function Draw-CardBase {
    param([System.Drawing.Graphics]$Graphics)

    $accent = New-Color 255 116 228 201
    $accent2 = New-Color 255 128 205 240
    $shadow = New-Brush (New-Color 76 3 6 18)
    $body = New-Brush (New-Color 224 22 31 50)
    $inner = New-Brush (New-Color 96 46 75 83)
    $glowA = New-Brush (New-Color 56 $accent.R $accent.G $accent.B)
    $glowB = New-Brush (New-Color 46 $accent2.R $accent2.G $accent2.B)
    $rim = New-Pen (New-Color 226 $accent.R $accent.G $accent.B) 3
    $hairline = New-Pen (New-Color 142 $accent2.R $accent2.G $accent2.B) 1.5
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(8, 17, 368, 128)) 24 $shadow $null
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(9, 13, 366, 126)) 24 $body $rim
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(24, 29, 336, 94)) 18 $inner $hairline
        $Graphics.FillEllipse($glowA, -34, -18, 160, 150)
        $Graphics.FillEllipse($glowB, 242, 0, 170, 150)
        $Graphics.DrawBezier($hairline, 138, 47, 190, 32, 256, 62, 340, 42)
        $Graphics.DrawBezier($hairline, 138, 112, 190, 128, 270, 90, 340, 112)
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

function Draw-CrescentNest {
    param([System.Drawing.Graphics]$Graphics)

    $mintBrush = New-Brush (New-Color 224 116 228 201)
    $deepBrush = New-Brush (New-Color 240 22 31 50)
    $cyanPen = New-Pen (New-Color 230 128 205 240) 5
    $mintPen = New-Pen (New-Color 230 116 228 201) 5
    $goldPen = New-Pen (New-Color 226 239 204 104) 4
    try {
        $Graphics.FillEllipse($mintBrush, 54, 43, 74, 74)
        $Graphics.FillEllipse($deepBrush, 79, 32, 66, 82)
        $Graphics.DrawArc($cyanPen, 50, 45, 92, 54, 8, 164)
        $Graphics.DrawArc($mintPen, 60, 57, 76, 38, 12, 156)
        $Graphics.DrawLine($goldPen, 62, 103, 117, 103)
        $Graphics.DrawLine($goldPen, 76, 112, 130, 112)
    }
    finally {
        $mintBrush.Dispose()
        $deepBrush.Dispose()
        $cyanPen.Dispose()
        $mintPen.Dispose()
        $goldPen.Dispose()
    }
}

function Draw-BedlineRepair {
    param([System.Drawing.Graphics]$Graphics)

    $cyanPen = New-Pen (New-Color 232 128 205 240) 5
    $mintPen = New-Pen (New-Color 232 116 228 201) 5
    $goldPen = New-Pen (New-Color 230 239 204 104) 4
    $softBrush = New-Brush (New-Color 72 128 205 240)
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(146, 58, 88, 34)) 12 $softBrush $cyanPen
        $Graphics.DrawLine($mintPen, 157, 75, 178, 75)
        $Graphics.DrawLine($mintPen, 198, 75, 222, 75)
        $Graphics.DrawLine($goldPen, 182, 64, 194, 86)
        $Graphics.DrawLine($goldPen, 197, 64, 185, 86)
        $Graphics.DrawLine($cyanPen, 135, 98, 242, 98)
        $Graphics.DrawLine($cyanPen, 242, 98, 230, 88)
        $Graphics.DrawLine($cyanPen, 242, 98, 230, 108)
    }
    finally {
        $cyanPen.Dispose()
        $mintPen.Dispose()
        $goldPen.Dispose()
        $softBrush.Dispose()
    }
}

function Draw-RecoveryMetrics {
    param([System.Drawing.Graphics]$Graphics)

    $sleepPen = New-Pen (New-Color 230 128 205 240) 4
    $poopPen = New-Pen (New-Color 230 180 135 92) 4
    $hungerPen = New-Pen (New-Color 230 239 204 104) 4
    $hpPen = New-Pen (New-Color 230 116 228 201) 4
    $redPen = New-Pen (New-Color 220 238 82 99) 3
    $sleepBrush = New-Brush (New-Color 130 128 205 240)
    $goldBrush = New-Brush (New-Color 180 239 204 104)
    $mintBrush = New-Brush (New-Color 180 116 228 201)
    try {
        $Graphics.DrawArc($sleepPen, 260, 39, 32, 18, 190, 250)
        $Graphics.DrawArc($sleepPen, 287, 39, 32, 18, 190, 250)
        $Graphics.DrawLine($poopPen, 266, 83, 290, 106)
        $Graphics.DrawLine($poopPen, 290, 106, 274, 103)
        $Graphics.DrawLine($poopPen, 290, 106, 286, 90)
        $Graphics.DrawEllipse($redPen, 257, 86, 28, 19)
        $Graphics.FillEllipse($goldBrush, 311, 82, 34, 18)
        $Graphics.DrawEllipse($hungerPen, 311, 82, 34, 18)
        $Graphics.DrawLine($hungerPen, 310, 72, 348, 72)
        $Graphics.DrawLine($hungerPen, 326, 64, 326, 80)
        $Graphics.FillEllipse($mintBrush, 302, 36, 37, 37)
        $Graphics.DrawLine($hpPen, 320, 44, 320, 64)
        $Graphics.DrawLine($hpPen, 310, 54, 330, 54)
        Draw-Star $Graphics 251 58 8 4 $sleepBrush
    }
    finally {
        $sleepPen.Dispose()
        $poopPen.Dispose()
        $hungerPen.Dispose()
        $hpPen.Dispose()
        $redPen.Dispose()
        $sleepBrush.Dispose()
        $goldBrush.Dispose()
        $mintBrush.Dispose()
    }
}

function Write-SpriteMeta {
    param(
        [string]$AssetPath,
        [string]$AssetId
    )

    $guid = Get-HexMd5 ("TheCat:" + $AssetId + ":p0_asset_batch_22_rest_nest_recovery_card")
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
  spriteBorder: {x: 12, y: 12, z: 12, w: 12}
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
  userData: TheCatP0ImportSettings:v1; batch:p0_asset_batch_22_rest_nest_recovery_card; spriteBorder:12; nonCatSymbolicOnly:true
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -Path $metaPath -Value $content -Encoding UTF8
}

function New-RestNestRecoveryCard {
    $assetId = "thecat_ui_restnest_recovery_card_384x160_v001"
    $path = Join-Path $cardDir ($assetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(384, 160, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.Clear([System.Drawing.Color]::Transparent)
        Draw-CardBase $graphics
        Draw-CrescentNest $graphics
        Draw-BedlineRepair $graphics
        Draw-RecoveryMetrics $graphics
        $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    Write-SpriteMeta $path $assetId
    return $path
}

$output = New-RestNestRecoveryCard

$review = @'
# P0 RestNest Recovery Card Review

Generated: 2026-06-14

| Asset | Subject | Path | Notes |
| --- | --- | --- | --- |
| thecat_ui_restnest_recovery_card_384x160_v001 | rest_nest_recovery | Assets/TheCat/Art/UI/Cards/thecat_ui_restnest_recovery_card_384x160_v001.png | Deterministic non-cat RestNest recovery card using crescent nest, bedline repair, sleep-wave, poop-down, hunger safe-line, and HP plus symbols. |

## Consistency

- Uses RestNest mint/cyan recovery accent and existing P0 dreamglass card proportions.
- Contains no cat body, ears, paws, tails, fur markings, collars, costumes, or starter-cat civilization motifs.
- Does not derive from Saiban, Nephthys, Suzune, or colored turnaround references.
- Runtime UI supplies labels; image pixels contain no text.

## Import

- TextureImporter.textureType: 8
- spriteMode: 1
- spriteBorder: 12
- userData includes batch:p0_asset_batch_22_rest_nest_recovery_card and nonCatSymbolicOnly:true
'@

Set-Content -Path $reviewNotePath -Value $review -Encoding UTF8

Write-Host "Generated RestNest recovery card:"
Write-Host $output
Write-Host $reviewNotePath
