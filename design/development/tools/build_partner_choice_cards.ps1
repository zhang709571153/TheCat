Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$cardDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Cards"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_partner_choice_cards_2026-06-14.md"

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

    $shadow = New-Brush (New-Color 76 3 6 18)
    $body = New-Brush (New-Color 224 23 25 49)
    $inner = New-Brush (New-Color 94 54 58 96)
    $glowA = New-Brush (New-Color 54 $Accent.R $Accent.G $Accent.B)
    $glowB = New-Brush (New-Color 44 $Accent2.R $Accent2.G $Accent2.B)
    $rim = New-Pen (New-Color 226 $Accent.R $Accent.G $Accent.B) 3
    $hairline = New-Pen (New-Color 136 $Accent2.R $Accent2.G $Accent2.B) 1.5
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(8, 17, 368, 128)) 24 $shadow $null
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(9, 13, 366, 126)) 24 $body $rim
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(24, 29, 336, 94)) 18 $inner $hairline
        $Graphics.FillEllipse($glowA, -38, -20, 166, 152)
        $Graphics.FillEllipse($glowB, 238, 2, 176, 150)
        $Graphics.DrawBezier($hairline, 140, 48, 190, 34, 252, 62, 338, 44)
        $Graphics.DrawBezier($hairline, 140, 112, 192, 126, 270, 92, 338, 111)
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

function Draw-FishTreat {
    param(
        [System.Drawing.Graphics]$Graphics,
        [float]$X,
        [float]$Y,
        [float]$Scale,
        [System.Drawing.Brush]$Brush,
        [System.Drawing.Pen]$Pen
    )

    $Graphics.FillEllipse($Brush, $X, $Y + (12 * $Scale), 48 * $Scale, 22 * $Scale)
    $tail = @(
        [System.Drawing.PointF]::new($X + (48 * $Scale), $Y + (23 * $Scale)),
        [System.Drawing.PointF]::new($X + (70 * $Scale), $Y + (8 * $Scale)),
        [System.Drawing.PointF]::new($X + (70 * $Scale), $Y + (38 * $Scale))
    )
    $Graphics.FillPolygon($Brush, $tail)
    $Graphics.DrawEllipse($Pen, $X, $Y + (12 * $Scale), 48 * $Scale, 22 * $Scale)
    $Graphics.DrawPolygon($Pen, $tail)
}

function Draw-InviteMotif {
    param([System.Drawing.Graphics]$Graphics)

    $cyan = New-Brush (New-Color 210 118 222 236)
    $violet = New-Brush (New-Color 190 168 120 240)
    $shadowBrush = New-Brush (New-Color 185 14 18 43)
    $gold = New-Brush (New-Color 224 239 203 92)
    $cyanPen = New-Pen (New-Color 230 118 222 236) 5
    $violetPen = New-Pen (New-Color 225 168 120 240) 5
    $goldPen = New-Pen (New-Color 225 239 203 92) 4
    try {
        $Graphics.FillEllipse($shadowBrush, 56, 48, 66, 50)
        $Graphics.DrawArc($violetPen, 48, 39, 86, 70, 208, 225)
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(142, 53, 92, 42)) 12 (New-Brush (New-Color 74 118 222 236)) $cyanPen
        $Graphics.DrawLine($goldPen, 154, 74, 190, 74)
        $Graphics.DrawLine($goldPen, 190, 74, 178, 62)
        $Graphics.DrawLine($goldPen, 190, 74, 178, 86)
        $Graphics.DrawArc($cyanPen, 237, 47, 56, 50, 110, 250)
        $Graphics.DrawLine($cyanPen, 280, 71, 326, 71)
        $Graphics.DrawLine($cyanPen, 326, 71, 312, 58)
        $Graphics.DrawLine($cyanPen, 326, 71, 312, 84)
        Draw-Star $Graphics 112 41 10 5 $gold
        Draw-Star $Graphics 256 102 9 4 $violet
        Draw-Star $Graphics 330 44 9 4 $cyan
    }
    finally {
        $cyan.Dispose()
        $violet.Dispose()
        $shadowBrush.Dispose()
        $gold.Dispose()
        $cyanPen.Dispose()
        $violetPen.Dispose()
        $goldPen.Dispose()
    }
}

function Draw-DuplicateSupplyMotif {
    param([System.Drawing.Graphics]$Graphics)

    $cyanBrush = New-Brush (New-Color 196 118 222 236)
    $violetBrush = New-Brush (New-Color 166 168 120 240)
    $goldBrush = New-Brush (New-Color 225 239 203 92)
    $cyanPen = New-Pen (New-Color 226 118 222 236) 5
    $violetPen = New-Pen (New-Color 218 168 120 240) 4
    $goldPen = New-Pen (New-Color 226 239 203 92) 4
    try {
        $Graphics.DrawArc($violetPen, 54, 42, 70, 52, 20, 320)
        $Graphics.DrawLine($violetPen, 70, 94, 113, 94)
        $Graphics.DrawLine($cyanPen, 143, 58, 186, 58)
        $Graphics.DrawLine($cyanPen, 143, 83, 186, 83)
        $Graphics.DrawLine($cyanPen, 164, 44, 164, 98)
        $Graphics.DrawBezier($cyanPen, 196, 95, 225, 111, 262, 98, 287, 71)
        $Graphics.DrawLine($cyanPen, 287, 71, 270, 63)
        $Graphics.DrawLine($cyanPen, 287, 71, 278, 88)
        Draw-FishTreat $Graphics 282 54 0.65 $goldBrush $goldPen
        Draw-FishTreat $Graphics 296 80 0.52 $goldBrush $goldPen
        Draw-Star $Graphics 110 44 9 4 $cyanBrush
        Draw-Star $Graphics 222 54 8 4 $violetBrush
    }
    finally {
        $cyanBrush.Dispose()
        $violetBrush.Dispose()
        $goldBrush.Dispose()
        $cyanPen.Dispose()
        $violetPen.Dispose()
        $goldPen.Dispose()
    }
}

function Write-SpriteMeta {
    param(
        [string]$AssetPath,
        [string]$AssetId
    )

    $guid = Get-HexMd5 ("TheCat:" + $AssetId + ":p0_asset_batch_23_partner_choice_cards")
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
  userData: TheCatP0ImportSettings:v1; batch:p0_asset_batch_23_partner_choice_cards; spriteBorder:12; nonCatSymbolicOnly:true
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -Path $metaPath -Value $content -Encoding UTF8
}

function New-PartnerChoiceCard {
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
$outputs += New-PartnerChoiceCard "thecat_ui_partner_shadowmaru_preview_card_384x160_v001" ${function:Draw-InviteMotif} (New-Color 255 118 222 236) (New-Color 255 168 120 240)
$outputs += New-PartnerChoiceCard "thecat_ui_partner_duplicate_supply_card_384x160_v001" ${function:Draw-DuplicateSupplyMotif} (New-Color 255 168 120 240) (New-Color 255 239 203 92)

$review = @'
# P0 Partner Choice Cards Review

Generated: 2026-06-14

| Asset | Subject | Path | Notes |
| --- | --- | --- | --- |
| thecat_ui_partner_shadowmaru_preview_card_384x160_v001 | partner_shadowmaru_preview | Assets/TheCat/Art/UI/Cards/thecat_ui_partner_shadowmaru_preview_card_384x160_v001.png | Deterministic non-cat partner invitation card using shadow contract, linked dream path, and invite arrow symbols. |
| thecat_ui_partner_duplicate_supply_card_384x160_v001 | partner_preview_duplicate_supply | Assets/TheCat/Art/UI/Cards/thecat_ui_partner_duplicate_supply_card_384x160_v001.png | Deterministic non-cat duplicate partner fallback card using linked circles, conversion arrow, and night fish supply symbols. |

## Consistency

- Uses partner-node cyan/violet route UI language and existing P0 dreamglass card proportions.
- Contains no cat body, ears, paws, tails, fur markings, collars, costumes, or starter-cat civilization motifs.
- Does not define Shadowmaru body art and does not derive from Saiban, Nephthys, Suzune, or colored turnaround references.
- Runtime UI supplies labels; image pixels contain no text.

## Import

- TextureImporter.textureType: 8
- spriteMode: 1
- spriteBorder: 12
- userData includes batch:p0_asset_batch_23_partner_choice_cards and nonCatSymbolicOnly:true
'@

Set-Content -Path $reviewNotePath -Value $review -Encoding UTF8

Write-Host "Generated partner choice cards:"
$outputs | ForEach-Object { Write-Host $_ }
Write-Host $reviewNotePath
