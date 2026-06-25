Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$iconsDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Icons"

New-Item -ItemType Directory -Force -Path $iconsDir | Out-Null

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
  - serializedVersion: 4
    buildTarget: Standalone
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
    customData: 
    physicsShape: []
    bones: []
    spriteID: ${Guid}00000
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
"@

    Set-Content -LiteralPath ($Path + ".meta") -Value $content -Encoding UTF8
}

function Draw-Base {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Color]$Fill,
        [System.Drawing.Color]$Accent
    )

    $shadow = New-Brush (New-Color 96 11 13 28)
    $base = New-Brush $Fill
    $rim = New-Pen $Accent 5
    $inner = New-Pen (New-Color 92 255 255 255) 2
    try {
        $Graphics.FillEllipse($shadow, 16, 19, 96, 96)
        $Graphics.FillEllipse($base, 14, 12, 100, 100)
        $Graphics.DrawEllipse($rim, 14, 12, 100, 100)
        $Graphics.DrawEllipse($inner, 25, 23, 78, 78)
    } finally {
        $shadow.Dispose()
        $base.Dispose()
        $rim.Dispose()
        $inner.Dispose()
    }
}

function Draw-PartnerRecruit {
    param([System.Drawing.Graphics]$Graphics)

    $a = New-Brush (New-Color 230 184 240 212)
    $b = New-Brush (New-Color 220 110 210 198)
    $white = New-Pen (New-Color 220 250 255 255) 4
    $line = New-Pen (New-Color 230 45 86 84) 5
    try {
        $Graphics.FillEllipse($a, 39, 43, 28, 28)
        $Graphics.FillEllipse($b, 65, 43, 28, 28)
        $Graphics.DrawEllipse($white, 39, 43, 28, 28)
        $Graphics.DrawEllipse($white, 65, 43, 28, 28)
        $Graphics.DrawArc($line, 44, 65, 44, 23, 205, 130)
        $Graphics.DrawLine($white, 64, 25, 64, 39)
        $Graphics.DrawLine($white, 57, 32, 71, 32)
    } finally {
        $a.Dispose()
        $b.Dispose()
        $white.Dispose()
        $line.Dispose()
    }
}

function Draw-PurchaseSupply {
    param([System.Drawing.Graphics]$Graphics)

    $bag = New-Brush (New-Color 230 255 203 111)
    $tag = New-Brush (New-Color 230 158 226 246)
    $ink = New-Pen (New-Color 225 82 57 36) 4
    $white = New-Pen (New-Color 215 255 255 255) 3
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(40, 45, 48, 45)) 11 $bag $white
        $Graphics.DrawArc($ink, 49, 30, 30, 28, 190, 160)
        $Graphics.FillEllipse($tag, 79, 39, 20, 20)
        $Graphics.DrawLine($ink, 51, 67, 77, 67)
        $Graphics.DrawLine($ink, 58, 78, 70, 78)
    } finally {
        $bag.Dispose()
        $tag.Dispose()
        $ink.Dispose()
        $white.Dispose()
    }
}

function Draw-AuthorityBlessing {
    param([System.Drawing.Graphics]$Graphics)

    $glow = New-Brush (New-Color 220 247 216 137)
    $blue = New-Brush (New-Color 210 148 224 255)
    $white = New-Pen (New-Color 220 255 255 255) 4
    $altar = New-Pen (New-Color 235 96 78 124) 5
    try {
        Draw-Star $Graphics 64 47 28 12 $glow $white
        $Graphics.FillEllipse($blue, 55, 49, 18, 18)
        $Graphics.DrawLine($altar, 43, 87, 85, 87)
        $Graphics.DrawLine($altar, 50, 76, 78, 76)
        $Graphics.DrawLine($altar, 57, 67, 71, 67)
    } finally {
        $glow.Dispose()
        $blue.Dispose()
        $white.Dispose()
        $altar.Dispose()
    }
}

function Draw-AuthorityUpgrade {
    param([System.Drawing.Graphics]$Graphics)

    $gold = New-Brush (New-Color 225 255 210 112)
    $pink = New-Brush (New-Color 210 255 126 168)
    $white = New-Pen (New-Color 220 255 255 255) 4
    $arrow = New-Pen (New-Color 235 93 61 113) 7
    try {
        Draw-Star $Graphics 54 59 24 10 $gold $white
        Draw-Star $Graphics 79 43 17 7 $pink $null
        $Graphics.DrawLine($arrow, 45, 91, 82, 91)
        $Graphics.DrawLine($arrow, 82, 91, 70, 79)
        $Graphics.DrawLine($arrow, 82, 91, 70, 103)
    } finally {
        $gold.Dispose()
        $pink.Dispose()
        $white.Dispose()
        $arrow.Dispose()
    }
}

function Draw-RestSupply {
    param([System.Drawing.Graphics]$Graphics)

    $moon = New-Pen (New-Color 235 219 246 255) 11
    $nest = New-Pen (New-Color 225 164 225 205) 5
    $spark = New-Pen (New-Color 225 255 210 112) 4
    try {
        $Graphics.DrawArc($moon, 38, 27, 48, 48, 55, 260)
        $Graphics.DrawArc($nest, 34, 76, 60, 22, 190, 160)
        $Graphics.DrawArc($nest, 41, 86, 46, 16, 190, 160)
        $Graphics.DrawLine($spark, 92, 34, 92, 48)
        $Graphics.DrawLine($spark, 85, 41, 99, 41)
    } finally {
        $moon.Dispose()
        $nest.Dispose()
        $spark.Dispose()
    }
}

function Draw-DreamEventModifier {
    param([System.Drawing.Graphics]$Graphics)

    $cloud = New-Brush (New-Color 220 205 185 255)
    $green = New-Pen (New-Color 230 138 232 154) 5
    $red = New-Pen (New-Color 215 255 101 126) 5
    $white = New-Pen (New-Color 210 255 255 255) 3
    try {
        $Graphics.FillEllipse($cloud, 37, 47, 30, 28)
        $Graphics.FillEllipse($cloud, 55, 34, 38, 34)
        $Graphics.FillEllipse($cloud, 74, 50, 25, 25)
        $Graphics.FillRectangle($cloud, 47, 57, 43, 21)
        $Graphics.DrawArc($white, 37, 47, 30, 28, 180, 180)
        $Graphics.DrawArc($white, 55, 34, 38, 34, 185, 180)
        $Graphics.DrawLine($green, 48, 94, 64, 81)
        $Graphics.DrawLine($green, 64, 81, 81, 95)
        $Graphics.DrawLine($red, 84, 31, 99, 31)
        $Graphics.DrawLine($red, 92, 23, 92, 39)
    } finally {
        $cloud.Dispose()
        $green.Dispose()
        $red.Dispose()
        $white.Dispose()
    }
}

$icons = @(
    @{
        Id = "thecat_ui_choice_partner_recruit_icon_128_v001"
        Guid = "1d0ed8893dad4cefb0aa15735f7959db"
        Fill = (New-Color 235 46 87 83)
        Accent = (New-Color 235 184 240 212)
        Draw = ${function:Draw-PartnerRecruit}
    },
    @{
        Id = "thecat_ui_choice_purchase_supply_icon_128_v001"
        Guid = "6dfd1a5fe8a347dca30fce5125c9c9f1"
        Fill = (New-Color 235 92 68 40)
        Accent = (New-Color 235 255 210 112)
        Draw = ${function:Draw-PurchaseSupply}
    },
    @{
        Id = "thecat_ui_choice_authority_blessing_icon_128_v001"
        Guid = "8de3068bdca64034b1e4e1efd6592875"
        Fill = (New-Color 235 83 68 117)
        Accent = (New-Color 235 247 216 137)
        Draw = ${function:Draw-AuthorityBlessing}
    },
    @{
        Id = "thecat_ui_choice_authority_upgrade_icon_128_v001"
        Guid = "a620755c2c374e8186d9dd191b8f9ab4"
        Fill = (New-Color 235 73 57 101)
        Accent = (New-Color 235 255 126 168)
        Draw = ${function:Draw-AuthorityUpgrade}
    },
    @{
        Id = "thecat_ui_choice_rest_supply_icon_128_v001"
        Guid = "3fa0bc15ff97401d83c96dfdb67d5339"
        Fill = (New-Color 235 45 77 105)
        Accent = (New-Color 235 219 246 255)
        Draw = ${function:Draw-RestSupply}
    },
    @{
        Id = "thecat_ui_choice_dreamevent_modifier_icon_128_v001"
        Guid = "cf8277f834004f6189617837d7933ff2"
        Fill = (New-Color 235 55 63 109)
        Accent = (New-Color 235 205 185 255)
        Draw = ${function:Draw-DreamEventModifier}
    }
)

foreach ($icon in $icons) {
    $path = Join-Path $iconsDir ($icon.Id + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(128, 128, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
        $graphics.Clear([System.Drawing.Color]::Transparent)

        Draw-Base $graphics $icon.Fill $icon.Accent
        & $icon.Draw $graphics

        $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
        Write-Meta $path $icon.Guid
    } finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }
}

Write-Host "Generated $($icons.Count) route choice icons in $iconsDir"
