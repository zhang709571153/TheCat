Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$cardDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Cards"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_blessing_choice_cards_2026-06-14.md"
$batchId = "p0_asset_batch_24_blessing_choice_cards"

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
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2
    )

    $shadow = New-Brush (New-Color 78 3 6 18)
    $body = New-Brush (New-Color 226 21 24 52)
    $inner = New-Brush (New-Color 98 56 52 98)
    $glowA = New-Brush (New-Color 58 $Accent.R $Accent.G $Accent.B)
    $glowB = New-Brush (New-Color 48 $Accent2.R $Accent2.G $Accent2.B)
    $rim = New-Pen (New-Color 226 $Accent.R $Accent.G $Accent.B) 3
    $hairline = New-Pen (New-Color 136 $Accent2.R $Accent2.G $Accent2.B) 1.5
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(8, 17, 368, 128)) 24 $shadow $null
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(9, 13, 366, 126)) 24 $body $rim
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(24, 29, 336, 94)) 18 $inner $hairline
        $Graphics.FillEllipse($glowA, -42, -22, 170, 154)
        $Graphics.FillEllipse($glowB, 236, 0, 180, 154)
        $Graphics.DrawBezier($hairline, 132, 46, 184, 31, 256, 63, 344, 43)
        $Graphics.DrawBezier($hairline, 128, 113, 193, 130, 269, 89, 346, 111)
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

function Draw-ShieldMotif {
    param([System.Drawing.Graphics]$Graphics)

    $cyan = New-Brush (New-Color 214 123 224 238)
    $blue = New-Brush (New-Color 110 78 150 230)
    $gold = New-Brush (New-Color 230 238 202 92)
    $silverPen = New-Pen (New-Color 232 198 226 238) 5
    $cyanPen = New-Pen (New-Color 230 123 224 238) 5
    $goldPen = New-Pen (New-Color 225 238 202 92) 4
    try {
        $shield = @(
            [System.Drawing.PointF]::new(82, 35),
            [System.Drawing.PointF]::new(124, 48),
            [System.Drawing.PointF]::new(116, 95),
            [System.Drawing.PointF]::new(83, 116),
            [System.Drawing.PointF]::new(50, 95),
            [System.Drawing.PointF]::new(42, 48)
        )
        $Graphics.FillPolygon($blue, $shield)
        $Graphics.DrawPolygon($silverPen, $shield)
        $Graphics.DrawLine($goldPen, 47, 88, 119, 88)
        $Graphics.DrawBezier($cyanPen, 140, 98, 174, 72, 213, 72, 247, 95)
        $Graphics.DrawLine($cyanPen, 249, 95, 236, 78)
        $Graphics.DrawLine($cyanPen, 249, 95, 229, 96)
        $Graphics.DrawArc($silverPen, 230, 50, 76, 58, 205, 230)
        Draw-Star $Graphics 162 51 9 4 $gold
        Draw-Star $Graphics 304 98 8 4 $cyan
    }
    finally {
        $cyan.Dispose()
        $blue.Dispose()
        $gold.Dispose()
        $silverPen.Dispose()
        $cyanPen.Dispose()
        $goldPen.Dispose()
    }
}

function Draw-SandglassMotif {
    param([System.Drawing.Graphics]$Graphics)

    $violet = New-Brush (New-Color 190 168 120 240)
    $gold = New-Brush (New-Color 228 238 202 92)
    $cyan = New-Brush (New-Color 170 118 222 236)
    $violetPen = New-Pen (New-Color 228 168 120 240) 5
    $goldPen = New-Pen (New-Color 232 238 202 92) 4
    $cyanPen = New-Pen (New-Color 225 118 222 236) 4
    try {
        $Graphics.DrawLine($violetPen, 70, 42, 134, 42)
        $Graphics.DrawLine($violetPen, 70, 108, 134, 108)
        $Graphics.DrawLine($violetPen, 82, 45, 122, 108)
        $Graphics.DrawLine($violetPen, 122, 45, 82, 108)
        $Graphics.FillEllipse($gold, 91, 57, 22, 14)
        $Graphics.FillEllipse($gold, 84, 87, 36, 12)
        $Graphics.DrawEllipse($cyanPen, 46, 34, 112, 86)
        $Graphics.DrawArc($violetPen, 186, 51, 72, 50, 15, 330)
        $Graphics.FillEllipse($cyan, 213, 66, 17, 17)
        $Graphics.DrawEllipse($goldPen, 202, 55, 39, 39)
        $Graphics.DrawBezier($cyanPen, 260, 99, 284, 78, 318, 79, 340, 99)
        Draw-Star $Graphics 171 48 8 4 $gold
        Draw-Star $Graphics 327 48 8 4 $violet
    }
    finally {
        $violet.Dispose()
        $gold.Dispose()
        $cyan.Dispose()
        $violetPen.Dispose()
        $goldPen.Dispose()
        $cyanPen.Dispose()
    }
}

function Draw-LullabyMotif {
    param([System.Drawing.Graphics]$Graphics)

    $pink = New-Brush (New-Color 192 241 126 185)
    $gold = New-Brush (New-Color 225 238 202 92)
    $cyan = New-Brush (New-Color 174 118 222 236)
    $pinkPen = New-Pen (New-Color 226 241 126 185) 5
    $goldPen = New-Pen (New-Color 232 238 202 92) 4
    $cyanPen = New-Pen (New-Color 224 118 222 236) 4
    try {
        $Graphics.DrawArc($goldPen, 52, 38, 78, 70, 60, 250)
        $Graphics.DrawEllipse($pinkPen, 129, 48, 42, 42)
        $Graphics.DrawLine($pinkPen, 150, 90, 150, 111)
        $Graphics.DrawArc($cyanPen, 188, 58, 72, 42, 195, 310)
        $Graphics.DrawArc($cyanPen, 210, 44, 88, 68, 204, 298)
        $Graphics.DrawLine($goldPen, 292, 80, 336, 80)
        $Graphics.DrawLine($goldPen, 314, 58, 314, 102)
        Draw-Star $Graphics 93 42 9 4 $cyan
        Draw-Star $Graphics 176 105 8 4 $gold
        Draw-Star $Graphics 273 102 8 4 $pink
        $Graphics.FillEllipse($cyan, 254, 74, 11, 11)
        $Graphics.FillEllipse($gold, 323, 69, 13, 13)
    }
    finally {
        $pink.Dispose()
        $gold.Dispose()
        $cyan.Dispose()
        $pinkPen.Dispose()
        $goldPen.Dispose()
        $cyanPen.Dispose()
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
  userData: TheCatP0ImportSettings:v1; batch:$batchId; spriteBorder:12; nonCatSymbolicOnly:true
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -Path $metaPath -Value $content -Encoding UTF8
}

function New-BlessingChoiceCard {
    param(
        [string]$AssetId,
        [scriptblock]$Motif,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2
    )

    $path = Join-Path $cardDir ($AssetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(384, 160, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.Clear([System.Drawing.Color]::Transparent)
        Draw-CardBase $graphics $Accent $Accent2
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
$outputs += New-BlessingChoiceCard "thecat_ui_blessing_oath_bedline_card_384x160_v001" ${function:Draw-ShieldMotif} (New-Color 255 123 224 238) (New-Color 255 238 202 92)
$outputs += New-BlessingChoiceCard "thecat_ui_blessing_dominion_sandglass_card_384x160_v001" ${function:Draw-SandglassMotif} (New-Color 255 168 120 240) (New-Color 255 238 202 92)
$outputs += New-BlessingChoiceCard "thecat_ui_blessing_rhythm_lullaby_card_384x160_v001" ${function:Draw-LullabyMotif} (New-Color 255 241 126 185) (New-Color 255 118 222 236)

$review = @'
# P0 Blessing Choice Cards Review

Generated: 2026-06-14

| Asset | Subject | Path | Notes |
| --- | --- | --- | --- |
| thecat_ui_blessing_oath_bedline_card_384x160_v001 | blessing_authority_oath_bedline | Assets/TheCat/Art/UI/Cards/thecat_ui_blessing_oath_bedline_card_384x160_v001.png | Deterministic non-cat authority card using bedline shield, knockback wave, and oath sparks. |
| thecat_ui_blessing_dominion_sandglass_card_384x160_v001 | blessing_authority_dominion_sandglass | Assets/TheCat/Art/UI/Cards/thecat_ui_blessing_dominion_sandglass_card_384x160_v001.png | Deterministic non-cat authority card using moon sandglass, slow ring, mark-eye, and sand motes. |
| thecat_ui_blessing_rhythm_lullaby_card_384x160_v001 | blessing_authority_rhythm_lullaby | Assets/TheCat/Art/UI/Cards/thecat_ui_blessing_rhythm_lullaby_card_384x160_v001.png | Deterministic non-cat authority card using crescent lullaby waves, bell rhythm, and recovery pulse. |

## Consistency

- Uses blessing-node cyan, violet, gold, and pink route UI language with existing P0 dreamglass card proportions.
- Contains no cat body, face, ears, paws, tails, fur markings, collars, costumes, or character portraits.
- Does not derive from Saiban, Nephthys, Suzune, or their colored three-view turnarounds.
- Runtime UI supplies labels; image pixels contain no text.

## Import

- TextureImporter.textureType: 8
- spriteMode: 1
- spriteBorder: 12
- userData includes batch:p0_asset_batch_24_blessing_choice_cards and nonCatSymbolicOnly:true
'@

Set-Content -Path $reviewNotePath -Value $review -Encoding UTF8

Write-Host "Generated blessing choice cards:"
$outputs | ForEach-Object { Write-Host $_ }
Write-Host $reviewNotePath
