param(
    [string]$ProjectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
)

$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing

$outputDir = Join-Path $ProjectRoot "Assets/TheCat/Art/UI/Icons"
New-Item -ItemType Directory -Force -Path $outputDir | Out-Null

$icons = @(
    @{
        Id = "thecat_ui_route_defense_icon_128_v001"
        Guid = "b9a3f41d51c348debc1272b1c28bd3fc"
        Fill = "#244B74"
        Accent = "#AEE4FF"
        Kind = "defense"
    },
    @{
        Id = "thecat_ui_route_elite_icon_128_v001"
        Guid = "c05b81faed0b4d7abfc1eaacae81ecf1"
        Fill = "#512E68"
        Accent = "#FF7A9A"
        Kind = "elite"
    },
    @{
        Id = "thecat_ui_route_partner_icon_128_v001"
        Guid = "df8d9c546bf548bba2b31089c831e131"
        Fill = "#2E5B55"
        Accent = "#B8F0D4"
        Kind = "partner"
    },
    @{
        Id = "thecat_ui_route_shop_icon_128_v001"
        Guid = "0bcfdebd3e44422fa13163508cb22b9a"
        Fill = "#5B4527"
        Accent = "#FFD36B"
        Kind = "shop"
    },
    @{
        Id = "thecat_ui_route_dreamevent_icon_128_v001"
        Guid = "70fa300065834ff9828810fd0a104502"
        Fill = "#35416E"
        Accent = "#C9B8FF"
        Kind = "dream_event"
    },
    @{
        Id = "thecat_ui_route_blessing_icon_128_v001"
        Guid = "5b664ec184244b7886be2c030e44bac3"
        Fill = "#594874"
        Accent = "#F7D889"
        Kind = "blessing"
    },
    @{
        Id = "thecat_ui_route_restnest_icon_128_v001"
        Guid = "16c843b487fe430d98c8a6c0f0b5c3fd"
        Fill = "#2F5572"
        Accent = "#D9F3FF"
        Kind = "rest_nest"
    }
)

function New-Color([string]$hex, [int]$alpha = 255) {
    $value = $hex.TrimStart("#")
    return [System.Drawing.Color]::FromArgb(
        $alpha,
        [Convert]::ToInt32($value.Substring(0, 2), 16),
        [Convert]::ToInt32($value.Substring(2, 2), 16),
        [Convert]::ToInt32($value.Substring(4, 2), 16))
}

function New-Pen([string]$hex, [float]$width, [int]$alpha = 255) {
    $pen = [System.Drawing.Pen]::new((New-Color $hex $alpha), $width)
    $pen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $pen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $pen.LineJoin = [System.Drawing.Drawing2D.LineJoin]::Round
    return $pen
}

function New-Brush([string]$hex, [int]$alpha = 255) {
    return [System.Drawing.SolidBrush]::new((New-Color $hex $alpha))
}

function New-Path([float[]]$points) {
    $path = [System.Drawing.Drawing2D.GraphicsPath]::new()
    for ($i = 0; $i -lt $points.Length; $i += 4) {
        $path.AddLine($points[$i], $points[$i + 1], $points[$i + 2], $points[$i + 3])
    }

    $path.CloseFigure()
    return $path
}

function Draw-Base($g, $fill, $accent) {
    $shadow = New-Brush "#0E1020" 100
    $bg = New-Brush $fill 235
    $rim = New-Pen $accent 5 235
    $inner = New-Pen "#FFFFFF" 2 90

    $g.FillEllipse($shadow, 16, 19, 96, 96)
    $g.FillEllipse($bg, 14, 12, 100, 100)
    $g.DrawEllipse($rim, 14, 12, 100, 100)
    $g.DrawEllipse($inner, 25, 23, 78, 78)

    $shadow.Dispose()
    $bg.Dispose()
    $rim.Dispose()
    $inner.Dispose()
}

function Draw-Defense($g, $accent) {
    $brush = New-Brush $accent 225
    $pen = New-Pen "#FFFFFF" 4 210
    $path = New-Path @(64, 28, 94, 40, 94, 40, 88, 82, 88, 82, 64, 101, 64, 101, 40, 82, 40, 82, 34, 40, 34, 40, 64, 28)
    $g.FillPath($brush, $path)
    $g.DrawPath($pen, $path)
    $bed = New-Pen "#244B74" 5 230
    $g.DrawLine($bed, 45, 63, 83, 63)
    $g.DrawLine($bed, 45, 63, 45, 77)
    $g.DrawLine($bed, 83, 63, 83, 77)
    $g.DrawLine($bed, 45, 76, 83, 76)
    $brush.Dispose(); $pen.Dispose(); $bed.Dispose(); $path.Dispose()
}

function Draw-Elite($g, $accent) {
    $brush = New-Brush $accent 230
    $pen = New-Pen "#FFFFFF" 4 215
    $path = New-Path @(64, 25, 98, 63, 98, 63, 64, 101, 64, 101, 30, 63, 30, 63, 64, 25)
    $g.FillPath($brush, $path)
    $g.DrawPath($pen, $path)
    $dark = New-Pen "#512E68" 6 230
    $g.DrawLine($dark, 64, 44, 64, 71)
    $g.FillEllipse((New-Brush "#512E68" 230), 59, 79, 10, 10)
    $brush.Dispose(); $pen.Dispose(); $dark.Dispose(); $path.Dispose()
}

function Draw-Partner($g, $accent) {
    $brush = New-Brush $accent 220
    $pen = New-Pen "#FFFFFF" 4 210
    $g.FillEllipse($brush, 33, 47, 34, 34)
    $g.FillEllipse($brush, 61, 47, 34, 34)
    $g.FillEllipse($brush, 47, 32, 34, 34)
    $g.DrawEllipse($pen, 33, 47, 34, 34)
    $g.DrawEllipse($pen, 61, 47, 34, 34)
    $g.DrawEllipse($pen, 47, 32, 34, 34)
    $link = New-Pen "#2E5B55" 5 230
    $g.DrawArc($link, 43, 59, 42, 32, 205, 130)
    $brush.Dispose(); $pen.Dispose(); $link.Dispose()
}

function Draw-Shop($g, $accent) {
    $brush = New-Brush $accent 230
    $pen = New-Pen "#FFFFFF" 4 220
    $g.FillEllipse($brush, 32, 36, 64, 56)
    $g.DrawEllipse($pen, 32, 36, 64, 56)
    $tail = New-Path @(92, 64, 108, 50, 108, 50, 108, 78, 108, 78, 92, 64)
    $g.FillPath($brush, $tail)
    $g.DrawPath($pen, $tail)
    $coin = New-Pen "#5B4527" 5 230
    $g.DrawLine($coin, 50, 55, 72, 55)
    $g.DrawLine($coin, 45, 70, 78, 70)
    $brush.Dispose(); $pen.Dispose(); $coin.Dispose(); $tail.Dispose()
}

function Draw-DreamEvent($g, $accent) {
    $brush = New-Brush $accent 225
    $pen = New-Pen "#FFFFFF" 4 220
    $g.FillEllipse($brush, 33, 50, 32, 28)
    $g.FillEllipse($brush, 52, 36, 38, 36)
    $g.FillEllipse($brush, 72, 51, 26, 27)
    $g.FillRectangle($brush, 43, 58, 46, 25)
    $g.DrawArc($pen, 34, 50, 31, 29, 175, 180)
    $g.DrawArc($pen, 52, 36, 38, 36, 185, 200)
    $g.DrawArc($pen, 72, 51, 26, 27, 270, 170)
    $spark = New-Pen "#35416E" 5 230
    $g.DrawLine($spark, 62, 91, 62, 104)
    $g.DrawLine($spark, 55, 98, 69, 98)
    $g.DrawLine($spark, 85, 87, 90, 99)
    $brush.Dispose(); $pen.Dispose(); $spark.Dispose()
}

function Draw-Blessing($g, $accent) {
    $brush = New-Brush $accent 228
    $pen = New-Pen "#FFFFFF" 4 220
    $path = New-Path @(38, 83, 49, 45, 49, 45, 64, 70, 64, 70, 79, 45, 79, 45, 90, 83, 90, 83, 38, 83)
    $g.FillPath($brush, $path)
    $g.DrawPath($pen, $path)
    $altar = New-Pen "#594874" 5 230
    $g.DrawLine($altar, 43, 92, 85, 92)
    $g.DrawLine($altar, 52, 35, 76, 35)
    $g.FillEllipse((New-Brush "#594874" 230), 59, 57, 10, 10)
    $brush.Dispose(); $pen.Dispose(); $altar.Dispose(); $path.Dispose()
}

function Draw-RestNest($g, $accent) {
    $pen = New-Pen $accent 12 235
    $thin = New-Pen "#FFFFFF" 4 210
    $g.DrawArc($pen, 33, 31, 62, 62, 55, 260)
    $g.DrawArc($thin, 43, 41, 42, 42, 70, 230)
    $nest = New-Pen $accent 5 225
    $g.DrawArc($nest, 34, 73, 60, 22, 190, 160)
    $g.DrawArc($nest, 41, 82, 46, 17, 190, 160)
    $pen.Dispose(); $thin.Dispose(); $nest.Dispose()
}

foreach ($icon in $icons) {
    $bitmap = [System.Drawing.Bitmap]::new(128, 128, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $graphics.Clear([System.Drawing.Color]::Transparent)

    Draw-Base $graphics $icon.Fill $icon.Accent
    switch ($icon.Kind) {
        "defense" { Draw-Defense $graphics $icon.Accent }
        "elite" { Draw-Elite $graphics $icon.Accent }
        "partner" { Draw-Partner $graphics $icon.Accent }
        "shop" { Draw-Shop $graphics $icon.Accent }
        "dream_event" { Draw-DreamEvent $graphics $icon.Accent }
        "blessing" { Draw-Blessing $graphics $icon.Accent }
        "rest_nest" { Draw-RestNest $graphics $icon.Accent }
    }

    $path = Join-Path $outputDir ($icon.Id + ".png")
    $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $graphics.Dispose()
    $bitmap.Dispose()

    $metaPath = $path + ".meta"
    @"
fileFormatVersion: 2
guid: $($icon.Guid)
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
  maxTextureSize: 2048
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
  flipbookColumns: 1
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
    maxTextureSize: 2048
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
  - serializedVersion: 4
    buildTarget: Standalone
    maxTextureSize: 2048
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
    customData: 
    physicsShape: []
    bones: []
    spriteID: $($icon.Guid)00000
    internalID: 0
    vertices: []
    indices: 
    edges: []
    weights: []
    secondaryTextures: []
    spriteCustomMetadata:
      entries: []
    nameFileIdTable: {}
  mipmapLimitGroupName: 
  pSDRemoveMatte: 0
  userData: TheCatP0ImportSettings:v1
  assetBundleName: 
  assetBundleVariant: 
"@ | Set-Content -Path $metaPath -Encoding UTF8
}

Write-Host "Generated $($icons.Count) route node icons in $outputDir"
