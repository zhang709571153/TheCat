Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$bannerDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Banners"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_nonbattle_node_summary_banners_2026-06-14.md"

New-Item -ItemType Directory -Force -Path $bannerDir | Out-Null
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

function Draw-BaseBanner {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2
    )

    $shadow = New-Brush (New-Color 76 4 8 24)
    $body = New-Brush (New-Color 220 24 30 50)
    $inner = New-Brush (New-Color 88 62 68 96)
    $leftGlow = New-Brush (New-Color 58 $Accent.R $Accent.G $Accent.B)
    $rightGlow = New-Brush (New-Color 42 $Accent2.R $Accent2.G $Accent2.B)
    $rim = New-Pen (New-Color 218 $Accent.R $Accent.G $Accent.B) 3
    $hairline = New-Pen (New-Color 110 $Accent2.R $Accent2.G $Accent2.B) 1.5
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(8, 18, 496, 128)) 28 $shadow $null
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(10, 14, 492, 126)) 28 $body $rim
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(28, 32, 456, 90)) 20 $inner $hairline
        $Graphics.FillEllipse($leftGlow, -36, 6, 196, 148)
        $Graphics.FillEllipse($rightGlow, 364, 12, 182, 140)
        $Graphics.DrawLine($hairline, 164, 42, 460, 42)
        $Graphics.DrawLine($hairline, 164, 116, 460, 116)
    }
    finally {
        $shadow.Dispose()
        $body.Dispose()
        $inner.Dispose()
        $leftGlow.Dispose()
        $rightGlow.Dispose()
        $rim.Dispose()
        $hairline.Dispose()
    }
}

function Draw-ShopMotif {
    param([System.Drawing.Graphics]$Graphics)

    $goldBrush = New-Brush (New-Color 235 239 194 94)
    $cyanBrush = New-Brush (New-Color 190 89 205 224)
    $softBrush = New-Brush (New-Color 90 239 194 94)
    $goldPen = New-Pen (New-Color 235 239 194 94) 6
    $cyanPen = New-Pen (New-Color 220 115 224 240) 4
    $darkPen = New-Pen (New-Color 180 72 50 78) 5
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(64, 45, 76, 58)) 14 $softBrush $goldPen
        $Graphics.DrawLine($goldPen, 84, 47, 99, 26)
        $Graphics.DrawLine($goldPen, 120, 47, 105, 26)
        $Graphics.DrawArc($cyanPen, 84, 24, 40, 34, 192, 156)
        $Graphics.FillEllipse($goldBrush, 98, 62, 16, 16)
        $Graphics.DrawLine($darkPen, 82, 93, 126, 93)

        for ($i = 0; $i -lt 4; $i++) {
            $x = 188 + ($i * 42)
            $Graphics.FillEllipse($goldBrush, $x, 60, 26, 26)
            $Graphics.DrawEllipse($cyanPen, $x + 2, 62, 22, 22)
        }

        $Graphics.DrawLine($cyanPen, 180, 103, 416, 103)
        $Graphics.DrawLine($goldPen, 184, 108, 408, 108)
        Draw-Star $Graphics 414 68 16 7 $goldBrush $null
        Draw-Star $Graphics 448 90 10 5 $cyanBrush $null
    }
    finally {
        $goldBrush.Dispose()
        $cyanBrush.Dispose()
        $softBrush.Dispose()
        $goldPen.Dispose()
        $cyanPen.Dispose()
        $darkPen.Dispose()
    }
}

function Draw-DreamEventMotif {
    param([System.Drawing.Graphics]$Graphics)

    $violetBrush = New-Brush (New-Color 205 184 130 226)
    $cyanBrush = New-Brush (New-Color 205 91 213 230)
    $moonPen = New-Pen (New-Color 232 215 230 255) 9
    $violetPen = New-Pen (New-Color 230 184 130 226) 5
    $cyanPen = New-Pen (New-Color 225 91 213 230) 5
    $goldPen = New-Pen (New-Color 226 238 190 91) 4
    try {
        $Graphics.DrawArc($moonPen, 65, 34, 58, 58, 78, 252)
        $Graphics.FillEllipse($violetBrush, 105, 62, 74, 38)
        $Graphics.FillEllipse($cyanBrush, 74, 72, 82, 34)
        $Graphics.DrawArc($violetPen, 98, 56, 82, 48, 200, 145)

        $Graphics.DrawLine($cyanPen, 228, 78, 318, 52)
        $Graphics.DrawLine($cyanPen, 318, 52, 302, 42)
        $Graphics.DrawLine($cyanPen, 318, 52, 309, 69)
        $Graphics.DrawLine($violetPen, 228, 86, 318, 112)
        $Graphics.DrawLine($violetPen, 318, 112, 302, 122)
        $Graphics.DrawLine($violetPen, 318, 112, 309, 95)

        Draw-Star $Graphics 372 58 15 7 $cyanBrush $null
        Draw-Star $Graphics 408 92 11 5 $violetBrush $null
        $Graphics.DrawArc($goldPen, 378, 74, 54, 30, 198, 144)
    }
    finally {
        $violetBrush.Dispose()
        $cyanBrush.Dispose()
        $moonPen.Dispose()
        $violetPen.Dispose()
        $cyanPen.Dispose()
        $goldPen.Dispose()
    }
}

function Draw-RestNestMotif {
    param([System.Drawing.Graphics]$Graphics)

    $greenBrush = New-Brush (New-Color 215 88 218 158)
    $cyanBrush = New-Brush (New-Color 198 140 216 236)
    $goldBrush = New-Brush (New-Color 220 245 206 104)
    $greenPen = New-Pen (New-Color 225 88 218 158) 6
    $cyanPen = New-Pen (New-Color 225 140 216 236) 5
    $goldPen = New-Pen (New-Color 230 245 206 104) 5
    try {
        $Graphics.DrawArc($goldPen, 64, 28, 72, 72, 74, 250)
        $Graphics.DrawArc($greenPen, 76, 70, 86, 42, 190, 160)
        $Graphics.DrawLine($greenPen, 68, 99, 168, 99)
        $Graphics.FillEllipse($cyanBrush, 92, 78, 42, 18)

        $Graphics.DrawArc($cyanPen, 214, 58, 54, 28, 205, 125)
        $Graphics.DrawArc($cyanPen, 274, 58, 54, 28, 205, 125)
        $Graphics.DrawArc($cyanPen, 334, 58, 54, 28, 205, 125)
        $Graphics.DrawLine($greenPen, 410, 56, 410, 104)
        $Graphics.DrawLine($greenPen, 386, 80, 434, 80)
        Draw-Star $Graphics 454 56 12 6 $goldBrush $null
        Draw-Star $Graphics 208 103 9 5 $greenBrush $null
    }
    finally {
        $greenBrush.Dispose()
        $cyanBrush.Dispose()
        $goldBrush.Dispose()
        $greenPen.Dispose()
        $cyanPen.Dispose()
        $goldPen.Dispose()
    }
}

function Write-BannerMeta {
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
  userData: TheCatP0ImportSettings:v1; batch:p0_asset_batch_19_nonbattle_node_summary_banners; spriteBorder:12; nonCatSymbolicOnly:true
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -LiteralPath ($Path + ".meta") -Value $content -NoNewline -Encoding ascii
}

function Draw-Banner {
    param(
        [string]$AssetId,
        [string]$Subject,
        [string]$Kind,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2,
        [scriptblock]$Draw
    )

    $outputPath = Join-Path $bannerDir ($AssetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(512, 160, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.Clear([System.Drawing.Color]::Transparent)
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality

        Draw-BaseBanner $graphics $Accent $Accent2
        & $Draw $graphics
        $bitmap.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    Write-BannerMeta -Path $outputPath -AssetId $AssetId
    return @{
        AssetId = $AssetId
        Subject = $Subject
        Kind = $Kind
        Path = $outputPath
        Hash = (Get-FileHash -Algorithm MD5 $outputPath).Hash.ToLowerInvariant()
    }
}

$banners = @(
    @{
        Asset = "thecat_ui_node_shop_summary_banner_512x160_v001"
        Subject = "shop_node_summary_banner"
        Kind = "shop"
        Accent = (New-Color 255 239 194 94)
        Accent2 = (New-Color 255 89 205 224)
        Draw = ${function:Draw-ShopMotif}
    },
    @{
        Asset = "thecat_ui_node_dreamevent_summary_banner_512x160_v001"
        Subject = "dream_event_node_summary_banner"
        Kind = "dream_event"
        Accent = (New-Color 255 184 130 226)
        Accent2 = (New-Color 255 91 213 230)
        Draw = ${function:Draw-DreamEventMotif}
    },
    @{
        Asset = "thecat_ui_node_restnest_summary_banner_512x160_v001"
        Subject = "rest_nest_node_summary_banner"
        Kind = "rest_nest"
        Accent = (New-Color 255 88 218 158)
        Accent2 = (New-Color 255 245 206 104)
        Draw = ${function:Draw-RestNestMotif}
    }
)

$results = New-Object System.Collections.Generic.List[object]
foreach ($banner in $banners) {
    $results.Add((Draw-Banner `
        -AssetId ([string]$banner.Asset) `
        -Subject ([string]$banner.Subject) `
        -Kind ([string]$banner.Kind) `
        -Accent ([System.Drawing.Color]$banner.Accent) `
        -Accent2 ([System.Drawing.Color]$banner.Accent2) `
        -Draw ([scriptblock]$banner.Draw)))
}

$reviewLines = New-Object System.Collections.Generic.List[string]
$reviewLines.Add("# P0 Non-Battle Node Summary Banners")
$reviewLines.Add("")
$reviewLines.Add('- Batch: `p0_asset_batch_19_nonbattle_node_summary_banners`')
$reviewLines.Add('- Output directory: `Assets/TheCat/Art/UI/Banners`')
$reviewLines.Add('- Size: `512x160`')
$reviewLines.Add("- Cat asset impact: none; no starter cat source, turnaround, candidate, or sprite file is read or written.")
$reviewLines.Add("- Consistency rule: non-cat symbolic UI only, matching dreamglass route-card language.")
$reviewLines.Add('- Runtime surface: `route_node_summary` / `summary_banner` for shop, dream event, and rest nest current-node cards.')
$reviewLines.Add("")
$reviewLines.Add("| subject | kind | output asset | md5 |")
$reviewLines.Add("| --- | --- | --- | --- |")

foreach ($result in $results) {
    $relativeOutput = "Assets/TheCat/Art/UI/Banners/" + $result.AssetId + ".png"
    $reviewLines.Add("| $($result.Subject) | $($result.Kind) | ``$relativeOutput`` | ``$($result.Hash)`` |")
}

Set-Content -LiteralPath $reviewNotePath -Value $reviewLines -Encoding utf8
Write-Output "Generated $($results.Count) non-battle node summary banners."
Write-Output "Review note: $reviewNotePath"
