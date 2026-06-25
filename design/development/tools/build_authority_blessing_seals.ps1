Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$iconsDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Icons"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_authority_blessing_seals_2026-06-14.md"

New-Item -ItemType Directory -Force -Path $iconsDir | Out-Null
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

function Draw-BaseSeal {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Color]$Fill,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Light
    )

    $shadow = New-Brush (New-Color 88 7 10 24)
    $base = New-Brush $Fill
    $soft = New-Brush (New-Color 46 $Light.R $Light.G $Light.B)
    $rim = New-Pen $Accent 5
    $inner = New-Pen (New-Color 120 $Light.R $Light.G $Light.B) 2
    try {
        $Graphics.FillEllipse($shadow, 14, 18, 100, 100)
        $Graphics.FillEllipse($base, 13, 10, 102, 102)
        $Graphics.FillEllipse($soft, 26, 23, 76, 76)
        $Graphics.DrawEllipse($rim, 13, 10, 102, 102)
        $Graphics.DrawEllipse($inner, 25, 22, 78, 78)
    }
    finally {
        $shadow.Dispose()
        $base.Dispose()
        $soft.Dispose()
        $rim.Dispose()
        $inner.Dispose()
    }
}

function Draw-OathBedline {
    param([System.Drawing.Graphics]$Graphics)

    $shield = New-Brush (New-Color 230 184 218 245)
    $gold = New-Pen (New-Color 235 247 205 101) 5
    $white = New-Pen (New-Color 225 255 255 255) 4
    $blue = New-Pen (New-Color 230 73 111 152) 4
    try {
        $points = @(
            [System.Drawing.PointF]::new(64, 27),
            [System.Drawing.PointF]::new(88, 38),
            [System.Drawing.PointF]::new(84, 75),
            [System.Drawing.PointF]::new(64, 96),
            [System.Drawing.PointF]::new(44, 75),
            [System.Drawing.PointF]::new(40, 38)
        )
        $Graphics.FillPolygon($shield, $points)
        $Graphics.DrawPolygon($white, $points)
        $Graphics.DrawLine($blue, 64, 36, 64, 85)
        $Graphics.DrawArc($gold, 40, 71, 48, 18, 190, 160)
        $Graphics.DrawLine($gold, 37, 88, 91, 88)
        $Graphics.DrawLine($white, 47, 59, 81, 59)
    }
    finally {
        $shield.Dispose()
        $gold.Dispose()
        $white.Dispose()
        $blue.Dispose()
    }
}

function Draw-DominionSandglass {
    param([System.Drawing.Graphics]$Graphics)

    $sand = New-Brush (New-Color 230 238 190 91)
    $moon = New-Pen (New-Color 230 210 236 255) 8
    $white = New-Pen (New-Color 220 255 255 255) 4
    $lapis = New-Pen (New-Color 230 77 104 164) 5
    try {
        $Graphics.DrawArc($moon, 35, 20, 42, 42, 80, 250)
        $Graphics.DrawLine($white, 46, 37, 82, 37)
        $Graphics.DrawLine($white, 46, 91, 82, 91)
        $Graphics.DrawLine($white, 49, 40, 79, 88)
        $Graphics.DrawLine($white, 79, 40, 49, 88)
        $Graphics.FillEllipse($sand, 54, 45, 20, 12)
        $Graphics.FillEllipse($sand, 54, 72, 20, 12)
        $Graphics.DrawArc($lapis, 42, 54, 44, 24, 10, 160)
        $Graphics.DrawArc($lapis, 42, 54, 44, 24, 190, 160)
        $Graphics.FillEllipse($sand, 59, 62, 10, 8)
    }
    finally {
        $sand.Dispose()
        $moon.Dispose()
        $white.Dispose()
        $lapis.Dispose()
    }
}

function Draw-RhythmLullaby {
    param([System.Drawing.Graphics]$Graphics)

    $bell = New-Brush (New-Color 232 247 199 86)
    $spark = New-Brush (New-Color 225 255 232 144)
    $red = New-Pen (New-Color 230 228 91 112) 5
    $white = New-Pen (New-Color 225 255 255 255) 4
    $wave = New-Pen (New-Color 230 139 224 234) 5
    try {
        $Graphics.DrawArc($white, 36, 24, 40, 40, 70, 255)
        $Graphics.FillPie($bell, 52, 50, 30, 34, 180, 180)
        $Graphics.FillRectangle($bell, 52, 63, 30, 17)
        $Graphics.DrawArc($red, 51, 44, 32, 32, 205, 130)
        $Graphics.DrawLine($white, 47, 80, 87, 80)
        $Graphics.FillEllipse($bell, 61, 78, 10, 10)
        $Graphics.DrawArc($wave, 31, 86, 32, 16, 205, 125)
        $Graphics.DrawArc($wave, 66, 86, 32, 16, 205, 125)
        Draw-Star $Graphics 88 38 9 4 $spark $null
    }
    finally {
        $bell.Dispose()
        $spark.Dispose()
        $red.Dispose()
        $white.Dispose()
        $wave.Dispose()
    }
}

function Write-SealMeta {
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
    weights: []
    secondaryTextures: []
    nameFileIdTable: {}
  spritePackingTag:
  pSDRemoveMatte: 0
  pSDShowRemoveMatteOption: 0
  userData: TheCatP0ImportSettings:v1; batch:p0_asset_batch_16_authority_blessing_seals; nonCatSymbolicOnly:true
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -LiteralPath ($Path + ".meta") -Value $content -NoNewline -Encoding ascii
}

function Draw-Seal {
    param(
        [string]$AssetId,
        [string]$Subject,
        [string]$Authority,
        [System.Drawing.Color]$Fill,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Light,
        [scriptblock]$Draw
    )

    $outputPath = Join-Path $iconsDir ($AssetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(128, 128, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
        $graphics.Clear([System.Drawing.Color]::Transparent)

        Draw-BaseSeal $graphics $Fill $Accent $Light
        & $Draw $graphics

        $bitmap.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    Write-SealMeta -Path $outputPath -AssetId $AssetId
    return @{
        AssetId = $AssetId
        Subject = $Subject
        Authority = $Authority
        Path = $outputPath
        Hash = (Get-FileHash -Algorithm MD5 $outputPath).Hash.ToLowerInvariant()
    }
}

$seals = @(
    @{
        Asset = "thecat_ui_blessing_oath_bedline_seal_128_v001"
        Subject = "authority_oath_bedline_seal"
        Authority = "Oath Bedline"
        Fill = (New-Color 235 45 69 100)
        Accent = (New-Color 235 235 203 111)
        Light = (New-Color 255 201 230 255)
        Draw = ${function:Draw-OathBedline}
    },
    @{
        Asset = "thecat_ui_blessing_dominion_sandglass_seal_128_v001"
        Subject = "authority_dominion_sandglass_seal"
        Authority = "Moon-Sand Dominion"
        Fill = (New-Color 235 42 50 96)
        Accent = (New-Color 235 214 181 96)
        Light = (New-Color 255 199 221 255)
        Draw = ${function:Draw-DominionSandglass}
    },
    @{
        Asset = "thecat_ui_blessing_rhythm_lullaby_seal_128_v001"
        Subject = "authority_rhythm_lullaby_seal"
        Authority = "Lullaby Rhythm"
        Fill = (New-Color 235 80 49 83)
        Accent = (New-Color 235 245 198 84)
        Light = (New-Color 255 161 231 235)
        Draw = ${function:Draw-RhythmLullaby}
    }
)

$results = New-Object System.Collections.Generic.List[object]
foreach ($seal in $seals) {
    $results.Add((Draw-Seal `
        -AssetId ([string]$seal.Asset) `
        -Subject ([string]$seal.Subject) `
        -Authority ([string]$seal.Authority) `
        -Fill ([System.Drawing.Color]$seal.Fill) `
        -Accent ([System.Drawing.Color]$seal.Accent) `
        -Light ([System.Drawing.Color]$seal.Light) `
        -Draw ([scriptblock]$seal.Draw)))
}

$reviewLines = New-Object System.Collections.Generic.List[string]
$reviewLines.Add('# P0 Authority Blessing Seals')
$reviewLines.Add('')
$reviewLines.Add('- Batch: p0_asset_batch_16_authority_blessing_seals')
$reviewLines.Add('- Output directory: Assets/TheCat/Art/UI/Icons')
$reviewLines.Add('- Size: 128x128')
$reviewLines.Add('- Cat asset impact: none; no starter cat source, turnaround, candidate, or sprite file is read or written.')
$reviewLines.Add('- Consistency rule: non-cat symbolic UI only. Cat identity remains locked to colored three-view turnaround review.')
$reviewLines.Add('')
$reviewLines.Add('| subject | authority | output asset | md5 |')
$reviewLines.Add('| --- | --- | --- | --- |')

foreach ($result in $results) {
    $relativeOutput = "Assets/TheCat/Art/UI/Icons/" + $result.AssetId + ".png"
    $reviewLines.Add("| $($result.Subject) | $($result.Authority) | $relativeOutput | $($result.Hash) |")
}

Set-Content -LiteralPath $reviewNotePath -Value $reviewLines -Encoding utf8
Write-Output "Generated $($results.Count) authority blessing seals."
Write-Output "Review note: $reviewNotePath"
