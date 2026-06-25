Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$vfxDir = Join-Path $projectRoot "Assets/TheCat/Art/Enemies/VFX"
New-Item -ItemType Directory -Force -Path $vfxDir | Out-Null

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
    return $pen
}

function Save-Png {
    param([System.Drawing.Bitmap]$Bitmap, [string]$Path)
    $Bitmap.Save($Path, [System.Drawing.Imaging.ImageFormat]::Png)
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
  userData: TheCatP0ImportSettings:v1
  assetBundleName: 
  assetBundleVariant: 
"@
    Set-Content -LiteralPath ($Path + ".meta") -Value $content -Encoding UTF8
}

function New-Canvas {
    $bitmap = [System.Drawing.Bitmap]::new(256, 256, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.Clear([System.Drawing.Color]::Transparent)
    return @{ Bitmap = $bitmap; Graphics = $graphics }
}

function Draw-Ring {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Color]$Color,
        [float]$X,
        [float]$Y,
        [float]$Size,
        [float]$Width
    )

    $pen = New-Pen $Color $Width
    try {
        $Graphics.DrawEllipse($pen, $X, $Y, $Size, $Size)
    } finally {
        $pen.Dispose()
    }
}

function Draw-BlackMudBedClaw {
    param([System.Drawing.Graphics]$Graphics)

    $mudBrush = New-Brush (New-Color 235 34 28 45)
    $glowBrush = New-Brush (New-Color 180 255 71 62)
    $redPen = New-Pen (New-Color 238 255 72 62) 9
    $shadowPen = New-Pen (New-Color 210 28 24 38) 15
    $goldPen = New-Pen (New-Color 210 255 198 87) 5
    try {
        Draw-Ring $Graphics (New-Color 95 255 72 62) 24 24 208 16
        Draw-Ring $Graphics (New-Color 160 54 32 58) 48 48 160 9
        $Graphics.FillEllipse($mudBrush, 76, 80, 104, 116)
        $Graphics.FillEllipse($glowBrush, 108, 112, 40, 40)
        $Graphics.DrawLine($shadowPen, 64, 84, 118, 170)
        $Graphics.DrawLine($shadowPen, 128, 62, 128, 184)
        $Graphics.DrawLine($shadowPen, 192, 84, 138, 170)
        $Graphics.DrawLine($redPen, 64, 84, 118, 170)
        $Graphics.DrawLine($redPen, 128, 62, 128, 184)
        $Graphics.DrawLine($redPen, 192, 84, 138, 170)
        $Graphics.DrawArc($goldPen, 72, 174, 112, 38, 188, 164)
    } finally {
        $mudBrush.Dispose()
        $glowBrush.Dispose()
        $redPen.Dispose()
        $shadowPen.Dispose()
        $goldPen.Dispose()
    }
}

function Draw-ColdLightBeamWarning {
    param([System.Drawing.Graphics]$Graphics)

    $beamPen = New-Pen (New-Color 245 121 236 255) 12
    $corePen = New-Pen (New-Color 248 242 255 255) 5
    $cyanPen = New-Pen (New-Color 205 79 195 235) 5
    $blueBrush = New-Brush (New-Color 215 49 67 110)
    $eyeBrush = New-Brush (New-Color 235 204 248 255)
    try {
        $Graphics.DrawLine($beamPen, 38, 132, 218, 82)
        $Graphics.DrawLine($corePen, 44, 132, 212, 86)
        $Graphics.DrawLine($cyanPen, 56, 160, 214, 160)
        $Graphics.FillEllipse($blueBrush, 54, 64, 80, 80)
        $Graphics.FillEllipse($eyeBrush, 78, 92, 34, 18)
        Draw-Ring $Graphics (New-Color 120 121 236 255) 34 44 184 132 9
        $Graphics.DrawArc($cyanPen, 44, 46, 168, 166, 220, 145)
        $Graphics.DrawArc($cyanPen, 52, 52, 154, 154, 35, 105)
    } finally {
        $beamPen.Dispose()
        $corePen.Dispose()
        $cyanPen.Dispose()
        $blueBrush.Dispose()
        $eyeBrush.Dispose()
    }
}

function Draw-CallTyrantAppThrow {
    param([System.Drawing.Graphics]$Graphics)

    $redPen = New-Pen (New-Color 242 255 63 61) 10
    $cyanPen = New-Pen (New-Color 218 84 220 244) 6
    $screenBrush = New-Brush (New-Color 230 34 37 62)
    $screenGlow = New-Brush (New-Color 210 255 67 77)
    $tieBrush = New-Brush (New-Color 220 118 70 171)
    try {
        $Graphics.DrawArc($redPen, 40, 54, 176, 142, 205, 124)
        $Graphics.DrawArc($cyanPen, 50, 44, 156, 156, 202, 128)
        $Graphics.FillRectangle($screenBrush, 92, 70, 72, 96)
        $Graphics.FillRectangle($screenGlow, 104, 88, 48, 28)
        $Graphics.FillPolygon($tieBrush, @(
            [System.Drawing.Point]::new(128, 124),
            [System.Drawing.Point]::new(146, 180),
            [System.Drawing.Point]::new(128, 208),
            [System.Drawing.Point]::new(110, 180)
        ))
        $Graphics.DrawLine($redPen, 174, 62, 210, 42)
        $Graphics.DrawLine($cyanPen, 188, 88, 224, 74)
        Draw-Ring $Graphics (New-Color 110 255 63 61) 62 42 132 132 8
    } finally {
        $redPen.Dispose()
        $cyanPen.Dispose()
        $screenBrush.Dispose()
        $screenGlow.Dispose()
        $tieBrush.Dispose()
    }
}

function Draw-CallTyrantSummonPortal {
    param([System.Drawing.Graphics]$Graphics)

    $outerPen = New-Pen (New-Color 235 255 54 63) 12
    $innerPen = New-Pen (New-Color 220 103 225 246) 7
    $darkBrush = New-Brush (New-Color 218 27 25 42)
    $sparkBrush = New-Brush (New-Color 235 255 205 104)
    try {
        $Graphics.FillEllipse($darkBrush, 52, 44, 152, 168)
        Draw-Ring $Graphics (New-Color 115 255 54 63) 32 28 192 192 18
        $Graphics.DrawArc($outerPen, 46, 38, 164, 176, 16, 315)
        $Graphics.DrawArc($innerPen, 68, 60, 120, 132, 206, 295)
        $Graphics.DrawLine($innerPen, 96, 100, 160, 156)
        $Graphics.FillEllipse($sparkBrush, 76, 64, 10, 10)
        $Graphics.FillEllipse($sparkBrush, 178, 92, 8, 8)
        $Graphics.FillEllipse($sparkBrush, 94, 190, 7, 7)
        $Graphics.FillEllipse($sparkBrush, 164, 186, 9, 9)
    } finally {
        $outerPen.Dispose()
        $innerPen.Dispose()
        $darkBrush.Dispose()
        $sparkBrush.Dispose()
    }
}

function New-Asset {
    param(
        [string]$FileName,
        [string]$Guid,
        [scriptblock]$Draw
    )

    $path = Join-Path $vfxDir $FileName
    $canvas = New-Canvas
    try {
        & $Draw $canvas.Graphics
        Save-Png $canvas.Bitmap $path
    } finally {
        $canvas.Graphics.Dispose()
        $canvas.Bitmap.Dispose()
    }

    Write-Meta $path $Guid
    Write-Output "Wrote $path"
}

New-Asset "thecat_vfx_blackmud_bed_claw_256_v001.png" "a6d0f1e9c2b34c5fa8120d6e7b9c3a41" { param($g) Draw-BlackMudBedClaw $g }
New-Asset "thecat_vfx_coldlight_beam_warning_256_v001.png" "b7e19c4a5d624f10a3c89b2e6f7d5012" { param($g) Draw-ColdLightBeamWarning $g }
New-Asset "thecat_vfx_calltyrant_app_throw_256_v001.png" "c8f2a05b6e734102bd91c3e7a4f8d620" { param($g) Draw-CallTyrantAppThrow $g }
New-Asset "thecat_vfx_calltyrant_summon_portal_256_v001.png" "d9a3b16c7f854213ce02d4f8b5a9e731" { param($g) Draw-CallTyrantSummonPortal $g }
