Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$badgeDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Badges"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_route_reward_detail_badges_2026-06-14.md"

New-Item -ItemType Directory -Force -Path $badgeDir | Out-Null
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

function Write-BadgeMeta {
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
  maxTextureSize: 256
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
  spriteBorder: {x: 8, y: 8, z: 8, w: 8}
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
    maxTextureSize: 256
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
    weights: []
    secondaryTextures: []
    nameFileIdTable: {}
  spritePackingTag:
  pSDRemoveMatte: 0
  pSDShowRemoveMatteOption: 0
  userData: TheCatP0ImportSettings:v1; batch:p0_asset_batch_15_route_reward_detail_badges; spriteBorder:8
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -LiteralPath ($Path + ".meta") -Value $content -NoNewline -Encoding ascii
}

function Draw-BadgeMotif {
    param(
        [System.Drawing.Graphics]$Graphics,
        [string]$Kind,
        [System.Drawing.Brush]$AccentBrush,
        [System.Drawing.Pen]$AccentPen,
        [System.Drawing.Pen]$LightPen
    )

    if ($Kind -eq "gain") {
        $Graphics.DrawLine($LightPen, 34, 22, 34, 42)
        $Graphics.DrawLine($LightPen, 24, 32, 44, 32)
        Draw-Star $Graphics 55 32 11 5 $AccentBrush $null
        return
    }

    if ($Kind -eq "cost") {
        $Graphics.FillEllipse($AccentBrush, 22, 20, 28, 28)
        $Graphics.DrawLine($LightPen, 21, 49, 55, 15)
        $Graphics.DrawArc($AccentPen, 52, 23, 18, 18, 150, 240)
        $Graphics.DrawLine($AccentPen, 67, 32, 76, 25)
        $Graphics.DrawLine($AccentPen, 67, 32, 76, 39)
        return
    }

    if ($Kind -eq "recovery") {
        $Graphics.DrawArc($LightPen, 23, 17, 28, 30, 60, 235)
        $Graphics.FillEllipse($AccentBrush, 46, 27, 20, 16)
        $Graphics.DrawLine($AccentPen, 26, 45, 42, 45)
        $Graphics.DrawLine($AccentPen, 45, 45, 56, 38)
        return
    }

    if ($Kind -eq "risk") {
        $points = @(
            [System.Drawing.PointF]::new(37, 15),
            [System.Drawing.PointF]::new(18, 48),
            [System.Drawing.PointF]::new(56, 48)
        )
        $Graphics.FillPolygon($AccentBrush, $points)
        $Graphics.DrawPolygon($LightPen, $points)
        $Graphics.DrawLine($LightPen, 37, 25, 37, 37)
        $Graphics.FillEllipse($AccentBrush, 34, 41, 6, 6)
        return
    }

    $Graphics.DrawLine($LightPen, 36, 47, 36, 20)
    $Graphics.DrawLine($LightPen, 36, 20, 24, 32)
    $Graphics.DrawLine($LightPen, 36, 20, 48, 32)
    Draw-Star $Graphics 58 28 11 5 $AccentBrush $AccentPen
}

function Draw-Badge {
    param(
        [string]$AssetId,
        [string]$Kind,
        [string]$Label,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2
    )

    $outputPath = Join-Path $badgeDir ($AssetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(192, 64, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $shadow = New-Brush (New-Color 88 6 8 20)
    $body = New-Brush (New-Color 220 26 30 50)
    $inner = New-Brush (New-Color 120 54 60 84)
    $accentBrush = New-Brush (New-Color 225 $Accent.R $Accent.G $Accent.B)
    $accentSoftBrush = New-Brush (New-Color 78 $Accent2.R $Accent2.G $Accent2.B)
    $rimPen = New-Pen (New-Color 220 $Accent.R $Accent.G $Accent.B) 2.2
    $accentPen = New-Pen (New-Color 230 $Accent.R $Accent.G $Accent.B) 3
    $lightPen = New-Pen (New-Color 235 $Accent2.R $Accent2.G $Accent2.B) 3
    $labelBrush = New-Brush (New-Color 240 250 242 222)
    $labelFont = [System.Drawing.Font]::new("Arial", 16, [System.Drawing.FontStyle]::Bold)
    $smallFont = [System.Drawing.Font]::new("Arial", 8, [System.Drawing.FontStyle]::Regular)

    try {
        $graphics.Clear([System.Drawing.Color]::Transparent)
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality

        Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(3, 6, 186, 54)) 14 $shadow $null
        Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(5, 4, 182, 54)) 14 $body $rimPen
        Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(13, 12, 166, 38)) 10 $inner $null
        Draw-RoundedRect $graphics ([System.Drawing.RectangleF]::new(15, 14, 56, 34)) 9 $accentSoftBrush $null

        Draw-BadgeMotif $graphics $Kind $accentBrush $accentPen $lightPen

        $graphics.DrawString($Label, $labelFont, $labelBrush, 82, 17)
        $graphics.DrawString("DETAIL", $smallFont, $labelBrush, 84, 38)
        $graphics.DrawLine($accentPen, 78, 15, 78, 49)
        $graphics.DrawLine($lightPen, 154, 15, 174, 15)
        $graphics.DrawLine($lightPen, 154, 48, 174, 48)

        $bitmap.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
        $shadow.Dispose()
        $body.Dispose()
        $inner.Dispose()
        $accentBrush.Dispose()
        $accentSoftBrush.Dispose()
        $rimPen.Dispose()
        $accentPen.Dispose()
        $lightPen.Dispose()
        $labelBrush.Dispose()
        $labelFont.Dispose()
        $smallFont.Dispose()
    }

    Write-BadgeMeta -Path $outputPath -AssetId $AssetId
    return $outputPath
}

$badges = @(
    @{ Asset = "thecat_ui_reward_detail_gain_badge_192x64_v001"; Subject = "route_reward_gain_detail"; Kind = "gain"; Label = "GAIN"; Accent = (New-Color 255 88 204 224); Accent2 = (New-Color 255 239 194 94) },
    @{ Asset = "thecat_ui_reward_detail_cost_badge_192x64_v001"; Subject = "route_reward_cost_detail"; Kind = "cost"; Label = "COST"; Accent = (New-Color 255 233 166 72); Accent2 = (New-Color 255 255 221 142) },
    @{ Asset = "thecat_ui_reward_detail_recovery_badge_192x64_v001"; Subject = "route_reward_recovery_detail"; Kind = "recovery"; Label = "REST"; Accent = (New-Color 255 88 218 158); Accent2 = (New-Color 255 140 216 236) },
    @{ Asset = "thecat_ui_reward_detail_risk_badge_192x64_v001"; Subject = "route_reward_risk_detail"; Kind = "risk"; Label = "RISK"; Accent = (New-Color 255 232 91 126); Accent2 = (New-Color 255 184 130 226) },
    @{ Asset = "thecat_ui_reward_detail_upgrade_badge_192x64_v001"; Subject = "route_reward_upgrade_detail"; Kind = "upgrade"; Label = "UP"; Accent = (New-Color 255 127 176 244); Accent2 = (New-Color 255 245 206 104) }
)

$reviewLines = New-Object System.Collections.Generic.List[string]
$reviewLines.Add("# P0 Route Reward Detail Badges")
$reviewLines.Add("")
$reviewLines.Add('- Batch: `p0_asset_batch_15_route_reward_detail_badges`')
$reviewLines.Add('- Output directory: `Assets/TheCat/Art/UI/Badges`')
$reviewLines.Add('- Size: `192x64`')
$reviewLines.Add("- Cat asset impact: none; no starter cat source, turnaround, or sprite file is read or written.")
$reviewLines.Add("- Consistency rule: non-cat symbolic UI only, matching dreamglass route-card language.")
$reviewLines.Add("")
$reviewLines.Add("| subject | kind | output asset | md5 |")
$reviewLines.Add("| --- | --- | --- | --- |")

foreach ($badge in $badges) {
    $assetId = [string]$badge.Asset
    $outputPath = Draw-Badge `
        -AssetId $assetId `
        -Kind ([string]$badge.Kind) `
        -Label ([string]$badge.Label) `
        -Accent ([System.Drawing.Color]$badge.Accent) `
        -Accent2 ([System.Drawing.Color]$badge.Accent2)

    $hash = (Get-FileHash -Algorithm MD5 $outputPath).Hash.ToLowerInvariant()
    $relativeOutput = "Assets/TheCat/Art/UI/Badges/" + $assetId + ".png"
    $reviewLines.Add("| $($badge.Subject) | $($badge.Kind) | ``$relativeOutput`` | ``$hash`` |")
}

Set-Content -LiteralPath $reviewNotePath -Value $reviewLines -Encoding utf8
Write-Output "Generated $($badges.Count) route reward detail badges."
Write-Output "Review note: $reviewNotePath"
