Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$cardDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Cards"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_shop_item_cards_2026-06-14.md"

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

    $shadow = New-Brush (New-Color 72 3 5 18)
    $body = New-Brush (New-Color 224 22 27 45)
    $inner = New-Brush (New-Color 82 58 66 94)
    $accentGlow = New-Brush (New-Color 54 $Accent.R $Accent.G $Accent.B)
    $accentGlow2 = New-Brush (New-Color 38 $Accent2.R $Accent2.G $Accent2.B)
    $rim = New-Pen (New-Color 224 $Accent.R $Accent.G $Accent.B) 3
    $hairline = New-Pen (New-Color 126 $Accent2.R $Accent2.G $Accent2.B) 1.5
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(8, 17, 368, 128)) 24 $shadow $null
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(9, 13, 366, 126)) 24 $body $rim
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(24, 29, 336, 94)) 18 $inner $hairline
        $Graphics.FillEllipse($accentGlow, -38, -18, 164, 156)
        $Graphics.FillEllipse($accentGlow2, 242, 2, 170, 150)
        $Graphics.DrawLine($hairline, 142, 44, 336, 44)
        $Graphics.DrawLine($hairline, 142, 110, 336, 110)
    }
    finally {
        $shadow.Dispose()
        $body.Dispose()
        $inner.Dispose()
        $accentGlow.Dispose()
        $accentGlow2.Dispose()
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

function Draw-BedPatchMotif {
    param([System.Drawing.Graphics]$Graphics)

    $goldBrush = New-Brush (New-Color 232 241 201 97)
    $cyanBrush = New-Brush (New-Color 204 116 219 232)
    $softBrush = New-Brush (New-Color 108 241 201 97)
    $goldPen = New-Pen (New-Color 232 241 201 97) 5
    $cyanPen = New-Pen (New-Color 228 116 219 232) 4
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(54, 53, 78, 44)) 16 $softBrush $goldPen
        $Graphics.DrawLine($cyanPen, 67, 75, 120, 75)
        $Graphics.DrawLine($goldPen, 94, 54, 94, 96)
        $Graphics.DrawArc($cyanPen, 155, 53, 58, 36, 18, 270)
        $Graphics.DrawLine($goldPen, 232, 63, 306, 63)
        $Graphics.DrawLine($goldPen, 232, 85, 286, 85)
        Draw-Star $Graphics 322 76 16 7 $goldBrush
        Draw-Star $Graphics 192 104 10 5 $cyanBrush
    }
    finally {
        $goldBrush.Dispose()
        $cyanBrush.Dispose()
        $softBrush.Dispose()
        $goldPen.Dispose()
        $cyanPen.Dispose()
    }
}

function Draw-LitterSachetMotif {
    param([System.Drawing.Graphics]$Graphics)

    $greenBrush = New-Brush (New-Color 220 84 222 154)
    $cyanBrush = New-Brush (New-Color 202 118 223 238)
    $softBrush = New-Brush (New-Color 92 84 222 154)
    $greenPen = New-Pen (New-Color 232 84 222 154) 5
    $cyanPen = New-Pen (New-Color 225 118 223 238) 4
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(54, 47, 58, 64)) 12 $softBrush $greenPen
        $Graphics.DrawLine($greenPen, 64, 60, 101, 60)
        $Graphics.DrawLine($cyanPen, 64, 80, 101, 80)
        $Graphics.DrawArc($cyanPen, 134, 53, 78, 42, 195, 155)
        $Graphics.DrawLine($greenPen, 128, 96, 220, 96)
        $Graphics.DrawLine($cyanPen, 236, 56, 312, 96)
        $Graphics.DrawLine($cyanPen, 312, 96, 300, 75)
        Draw-Star $Graphics 244 92 11 5 $greenBrush
        Draw-Star $Graphics 322 55 13 6 $cyanBrush
    }
    finally {
        $greenBrush.Dispose()
        $cyanBrush.Dispose()
        $softBrush.Dispose()
        $greenPen.Dispose()
        $cyanPen.Dispose()
    }
}

function Draw-LateKibbleMotif {
    param([System.Drawing.Graphics]$Graphics)

    $goldBrush = New-Brush (New-Color 228 236 187 86)
    $orangeBrush = New-Brush (New-Color 212 226 118 78)
    $cyanBrush = New-Brush (New-Color 202 117 218 230)
    $goldPen = New-Pen (New-Color 232 236 187 86) 5
    $cyanPen = New-Pen (New-Color 224 117 218 230) 4
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(52, 44, 62, 70)) 14 $orangeBrush $goldPen
        $Graphics.DrawLine($cyanPen, 62, 63, 104, 63)
        $Graphics.DrawArc($goldPen, 136, 74, 82, 32, 190, 160)
        $Graphics.FillEllipse($goldBrush, 156, 74, 12, 12)
        $Graphics.FillEllipse($goldBrush, 176, 68, 12, 12)
        $Graphics.FillEllipse($goldBrush, 193, 78, 12, 12)
        $Graphics.DrawLine($cyanPen, 238, 88, 322, 88)
        $Graphics.DrawLine($cyanPen, 322, 88, 309, 76)
        $Graphics.DrawLine($cyanPen, 322, 88, 309, 101)
        Draw-Star $Graphics 246 56 9 5 $cyanBrush
    }
    finally {
        $goldBrush.Dispose()
        $orangeBrush.Dispose()
        $cyanBrush.Dispose()
        $goldPen.Dispose()
        $cyanPen.Dispose()
    }
}

function Draw-FreeSampleMotif {
    param([System.Drawing.Graphics]$Graphics)

    $goldBrush = New-Brush (New-Color 236 242 196 83)
    $cyanBrush = New-Brush (New-Color 208 108 221 234)
    $softBrush = New-Brush (New-Color 88 242 196 83)
    $goldPen = New-Pen (New-Color 235 242 196 83) 5
    $cyanPen = New-Pen (New-Color 225 108 221 234) 4
    try {
        Draw-FishTreat $Graphics 54 51 0.86 $goldBrush $goldPen
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(154, 54, 82, 44)) 10 $softBrush $cyanPen
        $Graphics.DrawLine($goldPen, 170, 76, 220, 76)
        $Graphics.DrawLine($cyanPen, 254, 76, 320, 76)
        $Graphics.DrawLine($cyanPen, 320, 76, 307, 64)
        $Graphics.DrawLine($cyanPen, 320, 76, 307, 89)
        Draw-Star $Graphics 146 102 10 5 $goldBrush
        Draw-Star $Graphics 334 54 14 6 $cyanBrush
    }
    finally {
        $goldBrush.Dispose()
        $cyanBrush.Dispose()
        $softBrush.Dispose()
        $goldPen.Dispose()
        $cyanPen.Dispose()
    }
}

function Write-CardMeta {
    param(
        [string]$Path,
        [string]$AssetId
    )

    $guid = Get-HexMd5 ("TheCatUnityGuid:" + $AssetId)
    $spriteId = Get-HexMd5 ("TheCatSpriteId:" + $AssetId)
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
  userData: TheCatP0ImportSettings:v1; batch:p0_asset_batch_20_shop_item_cards; spriteBorder:12; nonCatSymbolicOnly:true
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -LiteralPath ($Path + ".meta") -Value $content -NoNewline -Encoding ascii
}

function Draw-Card {
    param(
        [string]$AssetId,
        [string]$Subject,
        [string]$Motif,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2,
        [scriptblock]$Draw
    )

    $outputPath = Join-Path $cardDir ($AssetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(384, 160, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.Clear([System.Drawing.Color]::Transparent)
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality

        Draw-CardBase $graphics $Accent $Accent2
        & $Draw $graphics
        $bitmap.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    Write-CardMeta -Path $outputPath -AssetId $AssetId
    return @{
        AssetId = $AssetId
        Subject = $Subject
        Motif = $Motif
        Path = $outputPath
        Hash = (Get-FileHash -Algorithm MD5 $outputPath).Hash.ToLowerInvariant()
    }
}

$cards = @(
    @{
        Asset = "thecat_ui_shop_item_bed_patch_card_384x160_v001"
        Subject = "shop_bed_patch"
        Motif = "patched pillow and owner sleep repair"
        Accent = (New-Color 255 241 201 97)
        Accent2 = (New-Color 255 116 219 232)
        Draw = ${function:Draw-BedPatchMotif}
    },
    @{
        Asset = "thecat_ui_shop_item_litter_sachet_card_384x160_v001"
        Subject = "shop_litter_sachet"
        Motif = "litter sachet and clean scoop relief"
        Accent = (New-Color 255 84 222 154)
        Accent2 = (New-Color 255 118 223 238)
        Draw = ${function:Draw-LitterSachetMotif}
    },
    @{
        Asset = "thecat_ui_shop_item_late_kibble_card_384x160_v001"
        Subject = "shop_late_kibble"
        Motif = "kibble pouch and hunger safe line"
        Accent = (New-Color 255 236 187 86)
        Accent2 = (New-Color 255 117 218 230)
        Draw = ${function:Draw-LateKibbleMotif}
    },
    @{
        Asset = "thecat_ui_shop_item_free_sample_card_384x160_v001"
        Subject = "shop_free_sample"
        Motif = "fish treat free sample tag"
        Accent = (New-Color 255 242 196 83)
        Accent2 = (New-Color 255 108 221 234)
        Draw = ${function:Draw-FreeSampleMotif}
    }
)

$results = New-Object System.Collections.Generic.List[object]
foreach ($card in $cards) {
    $results.Add((Draw-Card `
        -AssetId ([string]$card.Asset) `
        -Subject ([string]$card.Subject) `
        -Motif ([string]$card.Motif) `
        -Accent ([System.Drawing.Color]$card.Accent) `
        -Accent2 ([System.Drawing.Color]$card.Accent2) `
        -Draw ([scriptblock]$card.Draw)))
}

$reviewLines = New-Object System.Collections.Generic.List[string]
$reviewLines.Add("# P0 Shop Item Cards")
$reviewLines.Add("")
$reviewLines.Add('- Batch: `p0_asset_batch_20_shop_item_cards`')
$reviewLines.Add('- Output directory: `Assets/TheCat/Art/UI/Cards`')
$reviewLines.Add('- Prompt: `design/development/prompts/p0_shop_item_cards.md`')
$reviewLines.Add('- Scope: deterministic non-cat UI cards for existing shop reward choice ids.')
$reviewLines.Add('- Cat constraint: no cat silhouette, no fur markings, no starter-cat turnaround derivative, no civilization costume motif.')
$reviewLines.Add('- Runtime bindings: `shop_item.bed_patch`, `shop_item.litter_sachet`, `shop_item.late_kibble`, `shop_item.free_sample`.')
$reviewLines.Add("")
$reviewLines.Add("## Generated Assets")
$reviewLines.Add("")
foreach ($result in $results) {
    $relative = $result.Path.Substring($projectRoot.Length + 1).Replace("\", "/")
    $reviewLines.Add('- `' + $result.AssetId + '`')
    $reviewLines.Add('  - subject: `' + $result.Subject + '`')
    $reviewLines.Add("  - motif: $($result.Motif)")
    $reviewLines.Add("  - size: 384x160")
    $reviewLines.Add('  - path: `' + $relative + '`')
    $reviewLines.Add('  - md5: `' + $result.Hash + '`')
}

$reviewLines.Add("")
$reviewLines.Add("## Consistency Check")
$reviewLines.Add("")
$reviewLines.Add("- Uses the accepted dreamglass/shop-card UI palette and Batch 19 shop summary banner visual language.")
$reviewLines.Add("- Keeps all forms symbolic: patch, sachet, kibble, fish treat, arrows, stars, and safe-line marks.")
$reviewLines.Add("- Does not create or modify any starter cat asset; colored turnaround conformance remains untouched.")
$reviewLines.Add('- `.meta` files carry `TheCatP0ImportSettings:v1`, `batch:p0_asset_batch_20_shop_item_cards`, and `nonCatSymbolicOnly:true`.')

Set-Content -LiteralPath $reviewNotePath -Value ($reviewLines -join [Environment]::NewLine) -Encoding utf8

Write-Host "Generated $($results.Count) shop item cards."
foreach ($result in $results) {
    Write-Host "$($result.AssetId) -> $($result.Path)"
}
