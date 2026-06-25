Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$vfxDir = Join-Path $projectRoot "Assets/TheCat/Art/VFX"
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

function Draw-StarSpark {
    param([System.Drawing.Graphics]$Graphics)

    $goldPen = New-Pen (New-Color 238 255 205 104) 8
    $whitePen = New-Pen (New-Color 245 255 255 242) 5
    $redPen = New-Pen (New-Color 190 255 94 92) 5
    $cyanPen = New-Pen (New-Color 170 152 228 245) 4
    try {
        Draw-Ring $Graphics (New-Color 70 152 228 245) 38 38 180 9
        Draw-Ring $Graphics (New-Color 115 255 205 104) 66 66 124 5
        $Graphics.DrawLine($goldPen, 128, 30, 128, 226)
        $Graphics.DrawLine($goldPen, 30, 128, 226, 128)
        $Graphics.DrawLine($whitePen, 60, 60, 196, 196)
        $Graphics.DrawLine($whitePen, 196, 60, 60, 196)
        $Graphics.DrawLine($redPen, 56, 154, 108, 122)
        $Graphics.DrawLine($redPen, 148, 116, 204, 82)
        $Graphics.DrawArc($cyanPen, 52, 44, 156, 170, 205, 96)
    } finally {
        $goldPen.Dispose()
        $whitePen.Dispose()
        $redPen.Dispose()
        $cyanPen.Dispose()
    }
}

function Draw-BedShieldPulse {
    param([System.Drawing.Graphics]$Graphics)

    $shadowPen = New-Pen (New-Color 145 26 38 78) 13
    $shieldPen = New-Pen (New-Color 250 206 233 255) 9
    $bluePen = New-Pen (New-Color 235 91 205 245) 6
    $goldPen = New-Pen (New-Color 220 255 205 104) 5
    $bedBrush = New-Brush (New-Color 205 69 86 135)
    try {
        Draw-Ring $Graphics (New-Color 120 42 58 112) 28 28 200 15
        Draw-Ring $Graphics (New-Color 150 141 226 245) 30 30 196 12
        Draw-Ring $Graphics (New-Color 180 238 246 255) 54 54 148 8
        $Graphics.DrawArc($shadowPen, 70, 46, 116, 150, 205, 130)
        $Graphics.DrawArc($shieldPen, 70, 46, 116, 150, 205, 130)
        $Graphics.DrawLine($shieldPen, 75, 104, 128, 210)
        $Graphics.DrawLine($shieldPen, 181, 104, 128, 210)
        $Graphics.DrawArc($bluePen, 72, 54, 112, 122, 200, 140)
        $Graphics.FillRectangle($bedBrush, 74, 126, 108, 38)
        $Graphics.DrawLine($goldPen, 82, 126, 174, 126)
        $Graphics.DrawArc($goldPen, 70, 90, 116, 116, 25, 130)
    } finally {
        $shadowPen.Dispose()
        $shieldPen.Dispose()
        $bluePen.Dispose()
        $goldPen.Dispose()
        $bedBrush.Dispose()
    }
}

function Draw-SleepStableWave {
    param([System.Drawing.Graphics]$Graphics)

    $shadowPen = New-Pen (New-Color 140 27 39 82) 12
    $wavePen = New-Pen (New-Color 245 94 210 255) 8
    $thinPen = New-Pen (New-Color 220 190 239 255) 5
    $moonBrush = New-Brush (New-Color 210 239 244 255)
    $shadowBrush = New-Brush (New-Color 155 54 68 122)
    $goldBrush = New-Brush (New-Color 210 255 205 104)
    try {
        $Graphics.FillEllipse($moonBrush, 94, 38, 84, 84)
        $Graphics.FillEllipse($shadowBrush, 126, 26, 72, 72)
        $Graphics.DrawArc($shadowPen, 42, 96, 172, 58, 190, 160)
        $Graphics.DrawArc($shadowPen, 30, 124, 196, 66, 190, 160)
        $Graphics.DrawArc($wavePen, 42, 96, 172, 58, 190, 160)
        $Graphics.DrawArc($wavePen, 30, 124, 196, 66, 190, 160)
        $Graphics.DrawArc($thinPen, 52, 154, 152, 54, 190, 160)
        $Graphics.FillEllipse($goldBrush, 78, 84, 9, 9)
        $Graphics.FillEllipse($goldBrush, 178, 132, 7, 7)
        $Graphics.FillEllipse($goldBrush, 96, 184, 6, 6)
    } finally {
        $shadowPen.Dispose()
        $wavePen.Dispose()
        $thinPen.Dispose()
        $moonBrush.Dispose()
        $shadowBrush.Dispose()
        $goldBrush.Dispose()
    }
}

function Draw-LitterCleanse {
    param([System.Drawing.Graphics]$Graphics)

    $boxBrush = New-Brush (New-Color 220 95 75 110)
    $sandBrush = New-Brush (New-Color 240 238 217 174)
    $shadowPen = New-Pen (New-Color 130 25 50 70) 12
    $cleanPen = New-Pen (New-Color 245 91 236 220) 8
    $sparkBrush = New-Brush (New-Color 235 255 255 238)
    try {
        Draw-Ring $Graphics (New-Color 118 151 239 225) 38 38 180 12
        $Graphics.FillRectangle($boxBrush, 58, 130, 140, 52)
        $Graphics.FillEllipse($sandBrush, 70, 120, 116, 34)
        $Graphics.DrawArc($shadowPen, 52, 56, 154, 136, 210, 255)
        $Graphics.DrawArc($cleanPen, 52, 56, 154, 136, 210, 255)
        $Graphics.DrawLine($cleanPen, 178, 76, 198, 72)
        $Graphics.FillEllipse($sparkBrush, 74, 78, 11, 11)
        $Graphics.FillEllipse($sparkBrush, 166, 108, 9, 9)
        $Graphics.FillEllipse($sparkBrush, 128, 60, 7, 7)
    } finally {
        $boxBrush.Dispose()
        $sandBrush.Dispose()
        $shadowPen.Dispose()
        $cleanPen.Dispose()
        $sparkBrush.Dispose()
    }
}

function Draw-FeederKibble {
    param([System.Drawing.Graphics]$Graphics)

    $bowlBrush = New-Brush (New-Color 230 83 93 142)
    $rimPen = New-Pen (New-Color 245 143 220 245) 7
    $kibbleBrush = New-Brush (New-Color 240 255 205 104)
    $orangeBrush = New-Brush (New-Color 230 255 146 92)
    try {
        Draw-Ring $Graphics (New-Color 118 255 205 104) 40 40 176 12
        $Graphics.FillEllipse($bowlBrush, 58, 132, 140, 58)
        $Graphics.DrawEllipse($rimPen, 58, 118, 140, 46)
        $Graphics.FillEllipse($kibbleBrush, 88, 70, 22, 18)
        $Graphics.FillEllipse($kibbleBrush, 132, 52, 18, 16)
        $Graphics.FillEllipse($orangeBrush, 160, 82, 20, 17)
        $Graphics.FillEllipse($orangeBrush, 112, 108, 17, 15)
        $Graphics.FillEllipse($kibbleBrush, 150, 126, 16, 14)
    } finally {
        $bowlBrush.Dispose()
        $rimPen.Dispose()
        $kibbleBrush.Dispose()
        $orangeBrush.Dispose()
    }
}

function Draw-EnemyMarkRing {
    param([System.Drawing.Graphics]$Graphics)

    $violetPen = New-Pen (New-Color 220 190 121 255) 7
    $redPen = New-Pen (New-Color 215 255 86 91) 5
    $bluePen = New-Pen (New-Color 165 141 226 245) 3
    $pawBrush = New-Brush (New-Color 220 255 205 104)
    try {
        Draw-Ring $Graphics (New-Color 80 190 121 255) 34 34 188 11
        $Graphics.DrawEllipse($violetPen, 62, 62, 132, 132)
        $Graphics.DrawLine($redPen, 128, 28, 128, 72)
        $Graphics.DrawLine($redPen, 128, 184, 128, 228)
        $Graphics.DrawLine($redPen, 28, 128, 72, 128)
        $Graphics.DrawLine($redPen, 184, 128, 228, 128)
        $Graphics.DrawArc($bluePen, 76, 76, 104, 104, 220, 100)
        $Graphics.FillEllipse($pawBrush, 112, 126, 34, 28)
        $Graphics.FillEllipse($pawBrush, 94, 102, 16, 16)
        $Graphics.FillEllipse($pawBrush, 120, 92, 16, 16)
        $Graphics.FillEllipse($pawBrush, 148, 102, 16, 16)
    } finally {
        $violetPen.Dispose()
        $redPen.Dispose()
        $bluePen.Dispose()
        $pawBrush.Dispose()
    }
}

function New-VfxAsset {
    param(
        [string]$FileName,
        [string]$Guid,
        [scriptblock]$Draw
    )

    $canvas = New-Canvas
    try {
        & $Draw $canvas.Graphics
        $path = Join-Path $vfxDir $FileName
        Save-Png $canvas.Bitmap $path
        Write-Meta $path $Guid
    } finally {
        $canvas.Graphics.Dispose()
        $canvas.Bitmap.Dispose()
    }
}

New-VfxAsset "thecat_vfx_hit_spark_256_v001.png" "4b4e32c4885e4bd7ab76c60c3f5dcff5" { param($g) Draw-StarSpark $g }
New-VfxAsset "thecat_vfx_bed_shield_pulse_256_v001.png" "cb2e81219c6d4b74ad66c57e6e7557d1" { param($g) Draw-BedShieldPulse $g }
New-VfxAsset "thecat_vfx_sleep_stable_wave_256_v001.png" "f63e733f92f64c22af8f19c20b3a7c51" { param($g) Draw-SleepStableWave $g }
New-VfxAsset "thecat_vfx_litter_cleanse_256_v001.png" "a75e33bbd54e4b31b73f316df607ed32" { param($g) Draw-LitterCleanse $g }
New-VfxAsset "thecat_vfx_feeder_kibble_256_v001.png" "1db948c6aa2841e49aa7ec79c885ec8d" { param($g) Draw-FeederKibble $g }
New-VfxAsset "thecat_vfx_enemy_mark_ring_256_v001.png" "8ec6fd63d8f1436e99677866774a2dd8" { param($g) Draw-EnemyMarkRing $g }

Write-Host "Wrote P0 battle feedback VFX assets."
