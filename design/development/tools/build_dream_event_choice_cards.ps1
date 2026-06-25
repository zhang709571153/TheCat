Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$cardDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Cards"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_dream_event_choice_cards_2026-06-14.md"

New-Item -ItemType Directory -Force -Path $cardDir | Out-Null
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
        [System.Drawing.Brush]$Brush
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
}

function Draw-CardBase {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2
    )

    $shadow = New-Brush (New-Color 72 3 5 18)
    $body = New-Brush (New-Color 224 24 25 49)
    $inner = New-Brush (New-Color 90 62 52 95)
    $accentGlow = New-Brush (New-Color 58 $Accent.R $Accent.G $Accent.B)
    $accentGlow2 = New-Brush (New-Color 42 $Accent2.R $Accent2.G $Accent2.B)
    $rim = New-Pen (New-Color 226 $Accent.R $Accent.G $Accent.B) 3
    $hairline = New-Pen (New-Color 132 $Accent2.R $Accent2.G $Accent2.B) 1.5
    try {
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(8, 17, 368, 128)) 24 $shadow $null
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(9, 13, 366, 126)) 24 $body $rim
        Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new(24, 29, 336, 94)) 18 $inner $hairline
        $Graphics.FillEllipse($accentGlow, -42, -22, 170, 160)
        $Graphics.FillEllipse($accentGlow2, 238, 4, 176, 148)
        $Graphics.DrawBezier($hairline, 142, 48, 186, 33, 252, 63, 336, 43)
        $Graphics.DrawBezier($hairline, 142, 111, 190, 126, 268, 91, 336, 110)
    }
    finally {
        $shadow.Dispose()
        $body.Dispose()
        $inner.Dispose()
        $accentGlow.Dispose()
        $accentGlow2.Dispose()
        $rim.Dispose()
        $hairline.Dispose()
    }
}

function Draw-FishTreat {
    param(
        [System.Drawing.Graphics]$Graphics,
        [float]$X,
        [float]$Y,
        [float]$Scale,
        [System.Drawing.Brush]$Brush,
        [System.Drawing.Pen]$Pen
    )

    $Graphics.FillEllipse($Brush, $X, $Y + (12 * $Scale), 48 * $Scale, 22 * $Scale)
    $tail = @(
        [System.Drawing.PointF]::new($X + (48 * $Scale), $Y + (23 * $Scale)),
        [System.Drawing.PointF]::new($X + (70 * $Scale), $Y + (8 * $Scale)),
        [System.Drawing.PointF]::new($X + (70 * $Scale), $Y + (38 * $Scale))
    )
    $Graphics.FillPolygon($Brush, $tail)
    $Graphics.DrawEllipse($Pen, $X, $Y + (12 * $Scale), 48 * $Scale, 22 * $Scale)
    $Graphics.DrawPolygon($Pen, $tail)
}

function Draw-NotificationDot {
    param(
        [System.Drawing.Graphics]$Graphics,
        [float]$X,
        [float]$Y,
        [float]$Size,
        [System.Drawing.Brush]$Brush,
        [System.Drawing.Pen]$Pen
    )

    $Graphics.FillEllipse($Brush, $X, $Y, $Size, $Size)
    $Graphics.DrawEllipse($Pen, $X, $Y, $Size, $Size)
}

function Draw-MessageCard {
    param(
        [System.Drawing.Graphics]$Graphics,
        [float]$X,
        [float]$Y,
        [float]$Width,
        [float]$Height,
        [System.Drawing.Brush]$Brush,
        [System.Drawing.Pen]$Pen
    )

    Draw-RoundedRect $Graphics ([System.Drawing.RectangleF]::new($X, $Y, $Width, $Height)) 9 $Brush $Pen
    $Graphics.DrawLine($Pen, $X + 12, $Y + 17, $X + $Width - 12, $Y + 17)
    $Graphics.DrawLine($Pen, $X + 12, $Y + 32, $X + $Width - 22, $Y + 32)
}

function Draw-ClearNotificationsMotif {
    param([System.Drawing.Graphics]$Graphics)

    $redBrush = New-Brush (New-Color 224 238 82 99)
    $cyanBrush = New-Brush (New-Color 214 117 222 236)
    $goldBrush = New-Brush (New-Color 232 241 199 86)
    $softBrush = New-Brush (New-Color 78 117 222 236)
    $redPen = New-Pen (New-Color 230 238 82 99) 4
    $cyanPen = New-Pen (New-Color 226 117 222 236) 5
    $goldPen = New-Pen (New-Color 226 241 199 86) 4
    try {
        Draw-MessageCard $Graphics 48 48 76 54 $softBrush $cyanPen
        Draw-NotificationDot $Graphics 112 37 20 $redBrush $redPen
        Draw-NotificationDot $Graphics 137 58 14 $redBrush $redPen
        Draw-NotificationDot $Graphics 157 84 12 $redBrush $redPen
        $Graphics.DrawBezier($cyanPen, 68, 111, 124, 118, 182, 104, 238, 76)
        $Graphics.DrawLine($cyanPen, 238, 76, 225, 64)
        $Graphics.DrawLine($cyanPen, 238, 76, 221, 87)
        Draw-FishTreat $Graphics 262 56 0.72 $goldBrush $goldPen
        Draw-Star $Graphics 244 102 10 5 $cyanBrush
        Draw-Star $Graphics 328 48 12 6 $goldBrush
    }
    finally {
        $redBrush.Dispose()
        $cyanBrush.Dispose()
        $goldBrush.Dispose()
        $softBrush.Dispose()
        $redPen.Dispose()
        $cyanPen.Dispose()
        $goldPen.Dispose()
    }
}

function Draw-CatnipResidueMotif {
    param([System.Drawing.Graphics]$Graphics)

    $purpleBrush = New-Brush (New-Color 182 170 117 239)
    $greenBrush = New-Brush (New-Color 204 87 221 150)
    $redBrush = New-Brush (New-Color 202 238 82 99)
    $cyanBrush = New-Brush (New-Color 190 115 221 236)
    $purplePen = New-Pen (New-Color 226 170 117 239) 5
    $greenPen = New-Pen (New-Color 226 87 221 150) 4
    $redPen = New-Pen (New-Color 222 238 82 99) 4
    $cyanPen = New-Pen (New-Color 220 115 221 236) 4
    try {
        $Graphics.FillEllipse($purpleBrush, 49, 49, 54, 36)
        $Graphics.FillEllipse($purpleBrush, 81, 38, 58, 50)
        $Graphics.FillEllipse($purpleBrush, 107, 62, 44, 34)
        $Graphics.DrawBezier($purplePen, 70, 104, 120, 73, 172, 90, 216, 58)
        $Graphics.DrawLine($greenPen, 225, 93, 264, 54)
        $Graphics.DrawLine($greenPen, 264, 54, 264, 78)
        $Graphics.DrawLine($greenPen, 264, 54, 240, 54)
        $Graphics.DrawLine($redPen, 283, 58, 319, 95)
        $Graphics.DrawLine($redPen, 319, 95, 297, 91)
        $Graphics.DrawLine($redPen, 319, 95, 314, 73)
        $Graphics.DrawArc($cyanPen, 168, 48, 48, 34, 190, 260)
        Draw-Star $Graphics 111 101 10 5 $greenBrush
        Draw-Star $Graphics 196 102 9 4 $cyanBrush
        Draw-Star $Graphics 331 53 11 5 $redBrush
    }
    finally {
        $purpleBrush.Dispose()
        $greenBrush.Dispose()
        $redBrush.Dispose()
        $cyanBrush.Dispose()
        $purplePen.Dispose()
        $greenPen.Dispose()
        $redPen.Dispose()
        $cyanPen.Dispose()
    }
}

function Draw-MarkAllReadMotif {
    param([System.Drawing.Graphics]$Graphics)

    $cyanBrush = New-Brush (New-Color 198 112 222 236)
    $blueBrush = New-Brush (New-Color 112 91 128 204)
    $goldBrush = New-Brush (New-Color 226 242 198 85)
    $greenBrush = New-Brush (New-Color 206 87 221 150)
    $cyanPen = New-Pen (New-Color 224 112 222 236) 4
    $goldPen = New-Pen (New-Color 226 242 198 85) 5
    $greenPen = New-Pen (New-Color 226 87 221 150) 5
    try {
        Draw-MessageCard $Graphics 52 48 66 50 $blueBrush $cyanPen
        Draw-MessageCard $Graphics 96 42 72 56 $blueBrush $cyanPen
        Draw-MessageCard $Graphics 142 54 70 50 $blueBrush $cyanPen
        $Graphics.DrawLine($greenPen, 226, 78, 247, 99)
        $Graphics.DrawLine($greenPen, 247, 99, 312, 55)
        $Graphics.DrawArc($goldPen, 238, 72, 64, 34, 15, 320)
        $Graphics.DrawLine($goldPen, 291, 89, 323, 89)
        Draw-Star $Graphics 229 54 10 5 $goldBrush
        Draw-Star $Graphics 324 102 13 6 $cyanBrush
        Draw-Star $Graphics 75 110 8 4 $greenBrush
    }
    finally {
        $cyanBrush.Dispose()
        $blueBrush.Dispose()
        $goldBrush.Dispose()
        $greenBrush.Dispose()
        $cyanPen.Dispose()
        $goldPen.Dispose()
        $greenPen.Dispose()
    }
}

function Write-CardMeta {
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
  userData: TheCatP0ImportSettings:v1; batch:p0_asset_batch_21_dream_event_choice_cards; spriteBorder:12; nonCatSymbolicOnly:true
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -LiteralPath ($Path + ".meta") -Value $content -NoNewline -Encoding ascii
}

function Draw-Card {
    param(
        [string]$AssetId,
        [string]$Subject,
        [string]$Motif,
        [System.Drawing.Color]$Accent,
        [System.Drawing.Color]$Accent2,
        [scriptblock]$Draw
    )

    $outputPath = Join-Path $cardDir ($AssetId + ".png")
    $bitmap = [System.Drawing.Bitmap]::new(384, 160, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    try {
        $graphics.Clear([System.Drawing.Color]::Transparent)
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality

        Draw-CardBase $graphics $Accent $Accent2
        & $Draw $graphics
        $bitmap.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    finally {
        $graphics.Dispose()
        $bitmap.Dispose()
    }

    Write-CardMeta -Path $outputPath -AssetId $AssetId
    return @{
        AssetId = $AssetId
        Subject = $Subject
        Motif = $Motif
        Path = $outputPath
        Hash = (Get-FileHash -Algorithm MD5 $outputPath).Hash.ToLowerInvariant()
    }
}

$cards = @(
    @{
        Asset = "thecat_ui_dreamevent_clear_notifications_card_384x160_v001"
        Subject = "dream_event_clear_notifications"
        Motif = "red notification rain swept into fish treat reward"
        Accent = (New-Color 255 117 222 236)
        Accent2 = (New-Color 255 238 82 99)
        Draw = ${function:Draw-ClearNotificationsMotif}
    },
    @{
        Asset = "thecat_ui_dreamevent_catnip_residue_card_384x160_v001"
        Subject = "dream_event_catnip_residue"
        Motif = "residue cloud, skill up arrow, and poop-growth warning"
        Accent = (New-Color 255 170 117 239)
        Accent2 = (New-Color 255 87 221 150)
        Draw = ${function:Draw-CatnipResidueMotif}
    },
    @{
        Asset = "thecat_ui_dreamevent_mark_all_read_card_384x160_v001"
        Subject = "dream_event_mark_all_read"
        Motif = "message stack check mark and owner sleep stabilization"
        Accent = (New-Color 255 112 222 236)
        Accent2 = (New-Color 255 242 198 85)
        Draw = ${function:Draw-MarkAllReadMotif}
    }
)

$results = New-Object System.Collections.Generic.List[object]
foreach ($card in $cards) {
    $results.Add((Draw-Card `
        -AssetId ([string]$card.Asset) `
        -Subject ([string]$card.Subject) `
        -Motif ([string]$card.Motif) `
        -Accent ([System.Drawing.Color]$card.Accent) `
        -Accent2 ([System.Drawing.Color]$card.Accent2) `
        -Draw ([scriptblock]$card.Draw)))
}

$reviewLines = New-Object System.Collections.Generic.List[string]
$reviewLines.Add("# P0 Dream Event Choice Cards")
$reviewLines.Add("")
$reviewLines.Add('- Batch: `p0_asset_batch_21_dream_event_choice_cards`')
$reviewLines.Add('- Output directory: `Assets/TheCat/Art/UI/Cards`')
$reviewLines.Add('- Prompt: `design/development/prompts/p0_dream_event_choice_cards.md`')
$reviewLines.Add('- Scope: deterministic non-cat UI cards for existing DreamEvent reward choice ids.')
$reviewLines.Add('- Cat constraint: no cat silhouette, no fur markings, no starter-cat turnaround derivative, no civilization costume motif.')
$reviewLines.Add('- Runtime bindings: `dream_event_choice.clear_notifications`, `dream_event_choice.catnip_residue`, `dream_event_choice.mark_all_read`.')
$reviewLines.Add("")
$reviewLines.Add("## Generated Assets")
$reviewLines.Add("")
foreach ($result in $results) {
    $relative = $result.Path.Substring($projectRoot.Length + 1).Replace("\", "/")
    $reviewLines.Add('- `' + $result.AssetId + '`')
    $reviewLines.Add('  - subject: `' + $result.Subject + '`')
    $reviewLines.Add("  - motif: $($result.Motif)")
    $reviewLines.Add("  - size: 384x160")
    $reviewLines.Add('  - path: `' + $relative + '`')
    $reviewLines.Add('  - md5: `' + $result.Hash + '`')
}

$reviewLines.Add("")
$reviewLines.Add("## Consistency Check")
$reviewLines.Add("")
$reviewLines.Add("- Uses the accepted dreamglass, dream-event card-frame, route choice icon, and Batch 19 dream-event summary banner visual language.")
$reviewLines.Add("- Keeps all forms symbolic: notification dots, message cards, residue cloud, fish treat, arrows, check mark, sleep wave, and stars.")
$reviewLines.Add("- Does not create or modify any starter cat asset; colored turnaround conformance remains untouched.")
$reviewLines.Add('- `.meta` files carry `TheCatP0ImportSettings:v1`, `batch:p0_asset_batch_21_dream_event_choice_cards`, and `nonCatSymbolicOnly:true`.')

Set-Content -LiteralPath $reviewNotePath -Value ($reviewLines -join [Environment]::NewLine) -Encoding utf8

Write-Host "Generated $($results.Count) dream event choice cards."
foreach ($result in $results) {
    Write-Host "$($result.AssetId) -> $($result.Path)"
}
