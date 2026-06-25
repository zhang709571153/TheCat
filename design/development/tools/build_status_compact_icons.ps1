Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$iconDir = Join-Path $projectRoot "Assets/TheCat/Art/UI/Icons"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$reviewNotePath = Join-Path $reviewDir "p0_status_compact_icons_2026-06-14.md"

New-Item -ItemType Directory -Force -Path $iconDir | Out-Null
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

function Write-CompactMeta {
    param(
        [string]$Path,
        [string]$AssetId,
        [string]$SourceAssetId
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
  maxTextureSize: 64
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
    maxTextureSize: 64
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
  userData: TheCatP0ImportSettings:v1; batch:p0_asset_batch_14_status_compact_icons; derivedFrom:$SourceAssetId
  assetBundleName:
  assetBundleVariant:
"@

    Set-Content -LiteralPath ($Path + ".meta") -Value $content -NoNewline -Encoding ascii
}

$icons = @(
    @{ Source = "thecat_ui_status_sleep_stable_64_v001"; Asset = "thecat_ui_status_sleep_stable_32_v001"; Subject = "sleep_stable_compact"; Display = "Sleep Stable" },
    @{ Source = "thecat_ui_status_slow_64_v001"; Asset = "thecat_ui_status_slow_32_v001"; Subject = "slow_compact"; Display = "Slow" },
    @{ Source = "thecat_ui_status_knockback_64_v001"; Asset = "thecat_ui_status_knockback_32_v001"; Subject = "knockback_compact"; Display = "Knockback" },
    @{ Source = "thecat_ui_status_mark_64_v001"; Asset = "thecat_ui_status_mark_32_v001"; Subject = "mark_compact"; Display = "Mark" },
    @{ Source = "thecat_ui_status_shield_64_v001"; Asset = "thecat_ui_status_shield_32_v001"; Subject = "shield_compact"; Display = "Shield" }
)

$reviewLines = New-Object System.Collections.Generic.List[string]
$reviewLines.Add("# P0 Status Compact Icons")
$reviewLines.Add("")
$reviewLines.Add('- Batch: `p0_asset_batch_14_status_compact_icons`')
$reviewLines.Add("- Source rule: derive only from accepted 64px status icons.")
$reviewLines.Add('- Output directory: `Assets/TheCat/Art/UI/Icons`')
$reviewLines.Add("- Cat asset impact: none; no starter cat source or sprite file is read or written.")
$reviewLines.Add("")
$reviewLines.Add("| subject | display | source 64px asset | output 32px asset | md5 |")
$reviewLines.Add("| --- | --- | --- | --- | --- |")

foreach ($icon in $icons) {
    $sourceAssetId = [string]$icon.Source
    $assetId = [string]$icon.Asset
    $sourcePath = Join-Path $iconDir ($sourceAssetId + ".png")
    $outputPath = Join-Path $iconDir ($assetId + ".png")

    if (!(Test-Path -LiteralPath $sourcePath)) {
        throw "Missing source status icon: $sourcePath"
    }

    $source = [System.Drawing.Image]::FromFile($sourcePath)
    try {
        if ($source.Width -ne 64 -or $source.Height -ne 64) {
            throw "Expected 64x64 source icon, found $($source.Width)x$($source.Height): $sourcePath"
        }

        $compact = [System.Drawing.Bitmap]::new(32, 32, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
        $graphics = [System.Drawing.Graphics]::FromImage($compact)
        try {
            $graphics.Clear([System.Drawing.Color]::Transparent)
            $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
            $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
            $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
            $graphics.DrawImage($source, 0, 0, 32, 32)
            $compact.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
        }
        finally {
            $graphics.Dispose()
            $compact.Dispose()
        }
    }
    finally {
        $source.Dispose()
    }

    Write-CompactMeta -Path $outputPath -AssetId $assetId -SourceAssetId $sourceAssetId
    $hash = (Get-FileHash -Algorithm MD5 $outputPath).Hash.ToLowerInvariant()
    $relativeOutput = "Assets/TheCat/Art/UI/Icons/" + $assetId + ".png"
    $reviewLines.Add("| $($icon.Subject) | $($icon.Display) | ``$sourceAssetId`` | ``$relativeOutput`` | ``$hash`` |")
}

Set-Content -LiteralPath $reviewNotePath -Value $reviewLines -Encoding utf8
Write-Output "Generated $($icons.Count) compact status icons."
Write-Output "Review note: $reviewNotePath"
