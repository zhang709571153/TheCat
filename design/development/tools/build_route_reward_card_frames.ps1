Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$framesDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Frames"

New-Item -ItemType Directory -Force -Path $framesDir | Out-Null

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
    } finally {
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
        [System.Drawing.Brush]$Brush,
        [System.Drawing.Pen]$Pen = $null
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
    if ($Pen -ne $null) {
        $Graphics.DrawPolygon($Pen, $points.ToArray())
    }
}

function Write-Meta {
    param([string]$Path, [string]$Guid)

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
  spriteBorder: {x: 18, y: 18, z: 18, w: 18}
  spritePixelsToUnits: 100
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
    textureCompression: 1
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
    spriteID:
    internalID: 0
    vertices: []
    indices:
    edges: []
    weights: []
    secondaryTextures: []
    nameFileIdTable: {}
  spritePackingTag:
  pSDRemoveMatte: 0
  pSDShowRemoveMatteOption: 0
  userData: TheCatP0ImportSettings:v1; batch:p0_asset_batch_13_route_reward_card_frames; spriteBorder:18
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -Path "$Path.meta" -Value $content -Encoding UTF8
}

function Draw-BaseFrame {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2,
        [string]$Mode
    )

    $shadow = New-Brush (New-Color 90 8 12 26)
    $body = New-Brush (New-Color 216 24 28 48)
    $inner = New-Brush (New-Color 190 42 48 72)
    $rimPen = New-Pen (New-Color 235 $Accent.R $Accent.G $Accent.B) 4
    $softPen = New-Pen (New-Color 150 $Accent2.R $Accent2.G $Accent2.B) 2

    Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(10, 18, 492, 220)) 28 $shadow $null
    Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(14, 12, 484, 216)) 26 $body $rimPen
    Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(32, 34, 448, 172)) 20 $inner $softPen

    $accentBrush = New-Brush (New-Color 215 $Accent.R $Accent.G $Accent.B)
    $accentSoft = New-Brush (New-Color 90 $Accent2.R $Accent2.G $Accent2.B)
    Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(34, 32, 92, 176)) 18 $accentSoft $null
    Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(46, 48, 68, 144)) 16 $accentBrush $null

    for ($i = 0; $i -lt 5; $i++) {
        $x = 148 + ($i * 58)
        $Graphics.DrawLine($softPen, $x, 56, $x + 34, 56)
        $Graphics.DrawLine($softPen, $x, 184, $x + 34, 184)
    }

    $dot = New-Brush (New-Color 180 $Accent.R $Accent.G $Accent.B)
    for ($i = 0; $i -lt 4; $i++) {
        $Graphics.FillEllipse($dot, 426 + ($i * 14), 31 + (($i % 2) * 8), 7, 7)
    }
}

function Draw-BlessingMotif {
    param([System.Drawing.Graphics]$Graphics)
    $gold = New-Brush (New-Color 240 236 188 86)
    $blue = New-Pen (New-Color 220 136 218 234) 4
    $purple = New-Pen (New-Color 210 164 123 218) 3
    Draw-Star $Graphics 80 96 34 14 $gold $blue
    $Graphics.DrawLine($purple, 58, 152, 102, 152)
    $Graphics.DrawLine($purple, 66, 166, 94, 166)
    $Graphics.DrawArc($blue, 54, 62, 52, 92, 205, 130)
}

function Draw-PartnerMotif {
    param([System.Drawing.Graphics]$Graphics)
    $ring = New-Pen (New-Color 230 156 215 235) 5
    $gold = New-Pen (New-Color 220 238 190 92) 4
    $plus = New-Brush (New-Color 230 238 190 92)
    $Graphics.DrawEllipse($ring, 52, 82, 42, 42)
    $Graphics.DrawEllipse($ring, 72, 104, 42, 42)
    $Graphics.DrawLine($gold, 72, 103, 94, 125)
    $Graphics.FillRectangle($plus, 78, 56, 10, 34)
    $Graphics.FillRectangle($plus, 66, 68, 34, 10)
}

function Draw-ShopMotif {
    param([System.Drawing.Graphics]$Graphics)
    $bag = New-Brush (New-Color 238 223 176 94)
    $rim = New-Pen (New-Color 230 255 229 151) 4
    $fish = New-Brush (New-Color 235 86 195 214)
    Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(54, 78, 56, 70)) 12 $bag $rim
    $Graphics.DrawArc($rim, 66, 62, 32, 34, 190, 160)
    $Graphics.FillEllipse($fish, 64, 108, 32, 17)
    $Graphics.FillPolygon($fish, @(
        [System.Drawing.PointF]::new(95, 116),
        [System.Drawing.PointF]::new(111, 105),
        [System.Drawing.PointF]::new(111, 127)))
}

function Draw-EventMotif {
    param([System.Drawing.Graphics]$Graphics)
    $cloud = New-Brush (New-Color 224 123 185 226)
    $pink = New-Pen (New-Color 225 230 118 168) 4
    $cyan = New-Pen (New-Color 215 92 210 222) 4
    $Graphics.FillEllipse($cloud, 48, 92, 36, 28)
    $Graphics.FillEllipse($cloud, 70, 76, 42, 42)
    $Graphics.FillEllipse($cloud, 84, 98, 34, 26)
    $Graphics.FillRectangle($cloud, 60, 103, 48, 24)
    $Graphics.DrawLine($pink, 64, 152, 88, 132)
    $Graphics.DrawLine($pink, 88, 132, 108, 152)
    $Graphics.DrawLine($cyan, 64, 166, 108, 166)
}

function Draw-RestMotif {
    param([System.Drawing.Graphics]$Graphics)
    $nest = New-Pen (New-Color 230 230 188 112) 5
    $moon = New-Pen (New-Color 230 164 219 235) 6
    $leaf = New-Brush (New-Color 210 115 184 142)
    $Graphics.DrawArc($nest, 48, 110, 70, 48, 10, 160)
    $Graphics.DrawArc($nest, 54, 124, 58, 34, 10, 160)
    $Graphics.DrawArc($moon, 66, 58, 42, 58, 65, 240)
    $Graphics.FillEllipse($leaf, 58, 151, 18, 10)
    $Graphics.FillEllipse($leaf, 93, 151, 18, 10)
}

function New-FrameAsset {
    param(
        [string]$FileName,
        [string]$Guid,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2,
        [scriptblock]$Motif
    )

    $path = Join-Path $framesDir $FileName
    $bitmap = [System.Drawing.Bitmap]::new(512, 256, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)

    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
        $graphics.Clear([System.Drawing.Color]::Transparent)
        Draw-BaseFrame $graphics $Accent $Accent2 $FileName
        & $Motif $graphics
        $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    } finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    Write-Meta $path $Guid
    Write-Output $path
}

$assets = @(
    @{
        FileName = "thecat_ui_routecard_partner_frame_512x256_v001.png"
        Guid = "0e20d67555c14cd1a8389d5fb70e7a8c"
        Accent = New-Color 255 105 201 226
        Accent2 = New-Color 255 229 184 80
        Motif = { param($g) Draw-PartnerMotif $g }
    },
    @{
        FileName = "thecat_ui_routecard_blessing_frame_512x256_v001.png"
        Guid = "56e13a8c2cc340989b42f8d3d13251a4"
        Accent = New-Color 255 229 184 80
        Accent2 = New-Color 255 125 202 230
        Motif = { param($g) Draw-BlessingMotif $g }
    },
    @{
        FileName = "thecat_ui_routecard_shop_frame_512x256_v001.png"
        Guid = "9c47ecad7b5b43169c4d2295a04f217e"
        Accent = New-Color 255 226 168 74
        Accent2 = New-Color 255 82 195 214
        Motif = { param($g) Draw-ShopMotif $g }
    },
    @{
        FileName = "thecat_ui_routecard_dreamevent_frame_512x256_v001.png"
        Guid = "c6f7afe1a23f45cca0fc66c1384d2e0c"
        Accent = New-Color 255 156 127 220
        Accent2 = New-Color 255 232 118 166
        Motif = { param($g) Draw-EventMotif $g }
    },
    @{
        FileName = "thecat_ui_routecard_restnest_frame_512x256_v001.png"
        Guid = "2fd0bcebb7624d41bcba6679efc26bd6"
        Accent = New-Color 255 133 190 148
        Accent2 = New-Color 255 170 220 232
        Motif = { param($g) Draw-RestMotif $g }
    }
)

foreach ($asset in $assets) {
    New-FrameAsset `
        -FileName $asset.FileName `
        -Guid $asset.Guid `
        -Accent $asset.Accent `
        -Accent2 $asset.Accent2 `
        -Motif $asset.Motif
}
