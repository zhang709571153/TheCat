Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$backgroundDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Backgrounds"
$framesDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Frames"
$iconsDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Icons"

New-Item -ItemType Directory -Force -Path $backgroundDir, $framesDir, $iconsDir | Out-Null

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
    return [System.Drawing.Pen]::new($Color, $Width)
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

function Draw-CenteredText {
    param(
        [System.Drawing.Graphics]$Graphics,
        [string]$Text,
        [System.Drawing.Font]$Font,
        [System.Drawing.Brush]$Brush,
        [System.Drawing.RectangleF]$Rect
    )

    $format = [System.Drawing.StringFormat]::new()
    try {
        $format.Alignment = [System.Drawing.StringAlignment]::Center
        $format.LineAlignment = [System.Drawing.StringAlignment]::Center
        $Graphics.DrawString($Text, $Font, $Brush, $Rect, $format)
    } finally {
        $format.Dispose()
    }
}

function Save-Png {
    param([System.Drawing.Bitmap]$Bitmap, [string]$Path)
    $Bitmap.Save($Path, [System.Drawing.Imaging.ImageFormat]::Png)
}

function Write-Meta {
    param([string]$Path, [string]$Guid, [int]$MaxTextureSize = 2048)

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
  maxTextureSize: $MaxTextureSize
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
    maxTextureSize: $MaxTextureSize
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

function New-GradientBackground {
    $path = Join-Path $backgroundDir "thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png"
    $bitmap = [System.Drawing.Bitmap]::new(1920, 1080, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $rect = [System.Drawing.Rectangle]::new(0, 0, 1920, 1080)
        $brush = [System.Drawing.Drawing2D.LinearGradientBrush]::new(
            $rect,
            (New-Color 255 25 31 58),
            (New-Color 255 83 54 102),
            90)
        try {
            $graphics.FillRectangle($brush, $rect)
        } finally {
            $brush.Dispose()
        }

        $mistBrush = New-Brush (New-Color 42 155 222 232)
        $moonBrush = New-Brush (New-Color 190 231 238 255)
        $goldBrush = New-Brush (New-Color 190 255 205 104)
        $shadowBrush = New-Brush (New-Color 115 13 18 31)
        $linePen = New-Pen (New-Color 120 194 226 245) 4
        try {
            $graphics.FillEllipse($moonBrush, 1330, 120, 260, 260)
            $graphics.FillEllipse($shadowBrush, 1385, 96, 230, 230)
            for ($i = 0; $i -lt 7; $i++) {
                $x = 120 + $i * 250
                $y = 770 + [int](35 * [Math]::Sin($i))
                $graphics.FillEllipse($mistBrush, $x, $y, 420, 120)
            }

            $bedRect = [System.Drawing.RectangleF]::new(550, 615, 820, 205)
            Draw-RoundedRect $graphics $bedRect 34 (New-Brush (New-Color 220 62 78 112)) (New-Pen (New-Color 210 217 228 255) 5)
            Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(660, 540, 610, 115)) 24 (New-Brush (New-Color 235 210 219 239)) $null
            $graphics.DrawArc($linePen, 390, 390, 1140, 610, 204, 132)
            $graphics.DrawArc((New-Pen (New-Color 110 255 205 104) 3), 440, 450, 1040, 500, 206, 128)
            for ($i = 0; $i -lt 28; $i++) {
                $x = 170 + (($i * 241) % 1580)
                $y = 110 + (($i * 157) % 760)
                $r = 4 + ($i % 6)
                $graphics.FillEllipse($goldBrush, $x, $y, $r, $r)
            }
        } finally {
            $mistBrush.Dispose()
            $moonBrush.Dispose()
            $goldBrush.Dispose()
            $shadowBrush.Dispose()
            $linePen.Dispose()
        }

        Save-Png $bitmap $path
        Write-Meta $path "ef342940a20743a4ac2f8b635498b601" 2048
    } finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }
}

function New-TitleLogo {
    $path = Join-Path $framesDir "thecat_ui_title_logo_512x256_v001.png"
    $bitmap = [System.Drawing.Bitmap]::new(512, 256, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $badgeBrush = New-Brush (New-Color 224 28 34 62)
        $rimPen = New-Pen (New-Color 235 255 205 104) 7
        $bluePen = New-Pen (New-Color 210 164 229 246) 4
        $whiteBrush = New-Brush (New-Color 245 239 244 255)
        $goldBrush = New-Brush (New-Color 245 255 211 118)
        $fontLarge = [System.Drawing.Font]::new("Arial", 54, [System.Drawing.FontStyle]::Bold)
        $fontSmall = [System.Drawing.Font]::new("Arial", 19, [System.Drawing.FontStyle]::Bold)
        try {
            Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(38, 32, 436, 164)) 34 $badgeBrush $rimPen
            $graphics.DrawArc($bluePen, 88, 72, 336, 136, 200, 140)
            $graphics.DrawLine($bluePen, 146, 134, 196, 112)
            $graphics.DrawLine($bluePen, 366, 134, 316, 112)
            $graphics.FillEllipse($goldBrush, 234, 118, 44, 28)
            Draw-CenteredText $graphics "THECAT" $fontLarge $whiteBrush ([System.Drawing.RectangleF]::new(44, 54, 424, 70))
            Draw-CenteredText $graphics "DREAM DOMINATOR" $fontSmall $goldBrush ([System.Drawing.RectangleF]::new(64, 136, 384, 42))
        } finally {
            $badgeBrush.Dispose()
            $rimPen.Dispose()
            $bluePen.Dispose()
            $whiteBrush.Dispose()
            $goldBrush.Dispose()
            $fontLarge.Dispose()
            $fontSmall.Dispose()
        }

        Save-Png $bitmap $path
        Write-Meta $path "41f50c87b8cb4bc9be072cfb3349c8b8" 1024
    } finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }
}

function New-PanelFrame {
    $path = Join-Path $framesDir "thecat_ui_panel_dreamglass_512x256_v001.png"
    $bitmap = [System.Drawing.Bitmap]::new(512, 256, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(18, 18, 476, 220)) 28 (New-Brush (New-Color 198 26 32 58)) (New-Pen (New-Color 210 170 224 245) 5)
        Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(34, 36, 444, 184)) 20 (New-Brush (New-Color 54 255 255 255)) (New-Pen (New-Color 130 255 205 104) 2)
        $graphics.DrawLine((New-Pen (New-Color 100 255 205 104) 3), 64, 68, 448, 68)
        $graphics.DrawLine((New-Pen (New-Color 90 150 226 245) 3), 64, 190, 448, 190)
        Save-Png $bitmap $path
        Write-Meta $path "8da64e7f7c8d43aaa6ab12ce203013e7" 1024
    } finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }
}

function New-ButtonPrimary {
    $path = Join-Path $framesDir "thecat_ui_button_primary_384x96_v001.png"
    $bitmap = [System.Drawing.Bitmap]::new(384, 96, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(10, 12, 364, 68)) 22 (New-Brush (New-Color 235 59 75 120)) (New-Pen (New-Color 238 255 205 104) 4)
        Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(26, 22, 332, 22)) 10 (New-Brush (New-Color 58 255 255 255)) $null
        $graphics.DrawLine((New-Pen (New-Color 160 171 231 246) 3), 66, 60, 318, 60)
        Save-Png $bitmap $path
        Write-Meta $path "f5290dad83f04673a315780a369a04c2" 512
    } finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }
}

function New-RewardIcons {
    $items = @(
        @{ Path = (Join-Path $iconsDir "thecat_ui_reward_fishtreat_icon_128_v001.png"); Guid = "937fbdf5f0844deea5f3a98507fe0e31"; Kind = "fish" },
        @{ Path = (Join-Path $iconsDir "thecat_ui_reward_dreamshard_icon_128_v001.png"); Guid = "e8bf683d543646289429bb3cf99808e1"; Kind = "shard" }
    )

    foreach ($item in $items) {
        $bitmap = [System.Drawing.Bitmap]::new(128, 128, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
        $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
        try {
            $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
            Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(12, 12, 104, 104)) 28 (New-Brush (New-Color 225 27 35 62)) (New-Pen (New-Color 210 170 224 245) 4)
            if ($item.Kind -eq "fish") {
                $bodyBrush = New-Brush (New-Color 245 255 205 104)
                $finBrush = New-Brush (New-Color 230 255 145 86)
                try {
                    $graphics.FillEllipse($bodyBrush, 34, 47, 58, 34)
                    $points = [System.Drawing.PointF[]]@(
                        [System.Drawing.PointF]::new(92, 64),
                        [System.Drawing.PointF]::new(112, 44),
                        [System.Drawing.PointF]::new(112, 84)
                    )
                    $graphics.FillPolygon($finBrush, $points)
                    $graphics.FillEllipse((New-Brush (New-Color 255 42 34 62)), 48, 58, 7, 7)
                    $graphics.DrawArc((New-Pen (New-Color 180 42 34 62) 2), 34, 47, 58, 34, 290, 110)
                } finally {
                    $bodyBrush.Dispose()
                    $finBrush.Dispose()
                }
            } else {
                $shardBrush = New-Brush (New-Color 240 153 230 255)
                $shardBrush2 = New-Brush (New-Color 220 91 135 225)
                try {
                    $pointsA = [System.Drawing.PointF[]]@(
                        [System.Drawing.PointF]::new(64, 24),
                        [System.Drawing.PointF]::new(92, 58),
                        [System.Drawing.PointF]::new(72, 104),
                        [System.Drawing.PointF]::new(40, 80),
                        [System.Drawing.PointF]::new(46, 42)
                    )
                    $graphics.FillPolygon($shardBrush, $pointsA)
                    $pointsB = [System.Drawing.PointF[]]@(
                        [System.Drawing.PointF]::new(64, 24),
                        [System.Drawing.PointF]::new(72, 104),
                        [System.Drawing.PointF]::new(58, 68)
                    )
                    $graphics.FillPolygon($shardBrush2, $pointsB)
                    $graphics.DrawPolygon((New-Pen (New-Color 230 238 246 255) 3), $pointsA)
                } finally {
                    $shardBrush.Dispose()
                    $shardBrush2.Dispose()
                }
            }

            Save-Png $bitmap $item.Path
            Write-Meta $item.Path $item.Guid 512
        } finally {
            $graphics.Dispose()
            $bitmap.Dispose()
        }
    }
}

New-GradientBackground
New-TitleLogo
New-PanelFrame
New-ButtonPrimary
New-RewardIcons

Write-Host "Wrote P0 UI shell assets."
