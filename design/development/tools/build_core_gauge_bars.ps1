Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$frameDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Frames"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_core_gauge_bars_2026-06-14.md"
$batchId = "p0_asset_batch_27_core_gauge_bars"

New-Item -ItemType Directory -Force -Path $frameDir | Out-Null
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

function New-RoundedRectPath {
    param(
        [System.Drawing.RectangleF]$Rect,
        [float]$Radius
    )

    $path = [System.Drawing.Drawing2D.GraphicsPath]::new()
    $diameter = $Radius * 2
    $path.AddArc($Rect.X, $Rect.Y, $diameter, $diameter, 180, 90)
    $path.AddArc($Rect.Right - $diameter, $Rect.Y, $diameter, $diameter, 270, 90)
    $path.AddArc($Rect.Right - $diameter, $Rect.Bottom - $diameter, $diameter, $diameter, 0, 90)
    $path.AddArc($Rect.X, $Rect.Bottom - $diameter, $diameter, $diameter, 90, 90)
    $path.CloseFigure()
    return $path
}

function Draw-RoundedRect {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.RectangleF]$Rect,
        [float]$Radius,
        [System.Drawing.Brush]$Brush,
        [System.Drawing.Pen]$Pen = $null
    )

    $path = New-RoundedRectPath $Rect $Radius
    try {
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

function Write-SpriteMeta {
    param(
        [string]$PngPath,
        [string]$AssetId
    )

    $metaPath = "$PngPath.meta"
    $guid = Get-HexMd5 "$AssetId-meta"
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
  spriteBorder: {x: 10, y: 10, z: 10, w: 10}
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
    spriteID:
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
  userData: TheCatP0ImportSettings:v1; batch:$batchId; spriteBorder:10; nonCatSymbolicOnly:true
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -Path $metaPath -Value $content -Encoding UTF8
}

function New-GaugeFrame {
    param(
        [string]$AssetId,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2
    )

    $path = Join-Path $frameDir ($AssetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(384, 48, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.Clear([System.Drawing.Color]::Transparent)

        $shadow = New-Brush (New-Color 95 18 18 30)
        $base = New-Brush (New-Color 150 43 37 60)
        $inner = New-Brush (New-Color 84 10 12 22)
        $rim = New-Pen $Accent 2.8
        $rim2 = New-Pen $Accent2 1.4
        try {
            Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(5, 7, 374, 34)) 13 $shadow $null
            Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(8, 5, 368, 34)) 12 $base $rim
            Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(17, 14, 350, 16)) 8 $inner $rim2
            $graphics.DrawLine($rim2, 31, 9, 131, 9)
            $graphics.DrawLine($rim2, 253, 37, 356, 37)
        }
        finally {
            $shadow.Dispose()
            $base.Dispose()
            $inner.Dispose()
            $rim.Dispose()
            $rim2.Dispose()
        }

        $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    Write-SpriteMeta $path $AssetId
    return $path
}

function New-GaugeFill {
    param(
        [string]$AssetId,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2
    )

    $path = Join-Path $frameDir ($AssetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(384, 48, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.Clear([System.Drawing.Color]::Transparent)

        $back = New-Brush (New-Color 75 $Accent.R $Accent.G $Accent.B)
        $front = New-Brush (New-Color 218 $Accent.R $Accent.G $Accent.B)
        $spark = New-Pen $Accent2 2
        try {
            Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(8, 12, 368, 24)) 10 $back $null
            Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(13, 16, 358, 15)) 7 $front $null
            for ($x = 28; $x -lt 360; $x += 56) {
                $graphics.DrawBezier($spark, $x, 23, $x + 10, 12, $x + 28, 34, $x + 42, 21)
            }
        }
        finally {
            $back.Dispose()
            $front.Dispose()
            $spark.Dispose()
        }

        $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    Write-SpriteMeta $path $AssetId
    return $path
}

$specs = @(
    @{ Subject = "owner_sleep"; Label = "Owner Sleep"; Frame = "thecat_ui_core_sleep_gauge_frame_384x48_v001"; Fill = "thecat_ui_core_sleep_gauge_fill_384x48_v001"; Accent = (New-Color 255 105 211 239); Accent2 = (New-Color 245 205 227 255) },
    @{ Subject = "cat_hp"; Label = "Cat HP"; Frame = "thecat_ui_core_hp_gauge_frame_384x48_v001"; Fill = "thecat_ui_core_hp_gauge_fill_384x48_v001"; Accent = (New-Color 255 96 221 141); Accent2 = (New-Color 245 231 245 170) },
    @{ Subject = "team_poop"; Label = "Team Poop"; Frame = "thecat_ui_core_poop_gauge_frame_384x48_v001"; Fill = "thecat_ui_core_poop_gauge_fill_384x48_v001"; Accent = (New-Color 255 225 151 70); Accent2 = (New-Color 245 255 220 112) },
    @{ Subject = "team_hunger"; Label = "Team Hunger"; Frame = "thecat_ui_core_hunger_gauge_frame_384x48_v001"; Fill = "thecat_ui_core_hunger_gauge_fill_384x48_v001"; Accent = (New-Color 255 245 194 89); Accent2 = (New-Color 245 255 239 160) }
)

$outputs = @()
foreach ($spec in $specs) {
    $outputs += New-GaugeFrame $spec.Frame $spec.Accent $spec.Accent2
    $outputs += New-GaugeFill $spec.Fill $spec.Accent $spec.Accent2
}

$reviewLines = @()
$reviewLines += "# P0 Core Gauge Bars Review"
$reviewLines += ""
$reviewLines += "Generated: 2026-06-14"
$reviewLines += ""
$reviewLines += "| Subject | Frame | Fill | Notes |"
$reviewLines += "| --- | --- | --- | --- |"
foreach ($spec in $specs) {
    $reviewLines += "| $($spec.Subject) | Assets/TheCat/Art/UI/Frames/$($spec.Frame).png | Assets/TheCat/Art/UI/Frames/$($spec.Fill).png | Non-cat symbolic UI gauge pair using the existing four-core HUD color identity. |"
}
$reviewLines += ""
$reviewLines += "## Consistency"
$reviewLines += ""
$reviewLines += "- Uses dreamglass frame language and accepted four-core HUD icon palettes."
$reviewLines += "- Contains no cat body, face, ears, paws, tails, fur markings, collars, costumes, or character portraits."
$reviewLines += "- Does not derive from Saiban, Nephthys, Suzune, or their colored three-view turnarounds."
$reviewLines += "- Contains no UI text; labels remain runtime-controlled."
$reviewLines += ""
$reviewLines += "## Import"
$reviewLines += ""
$reviewLines += "- TextureImporter.textureType: 8"
$reviewLines += "- spriteMode: 1"
$reviewLines += "- spriteBorder: 10"
$reviewLines += "- userData includes batch:p0_asset_batch_27_core_gauge_bars and nonCatSymbolicOnly:true"

Set-Content -Path $reviewNotePath -Value $reviewLines -Encoding UTF8

Write-Host "Generated core gauge bar assets:"
$outputs | ForEach-Object { Write-Host $_ }
Write-Host $reviewNotePath
