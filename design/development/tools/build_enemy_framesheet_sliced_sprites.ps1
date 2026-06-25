Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$sourceDir = Join-Path $projectRoot "Assets/TheCat/Art/Enemies/Frames"
$slicedDir = Join-Path $sourceDir "Sliced"
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$manifestPath = Join-Path $reviewDir "ENEMY_FRAMESHEET_SLICED_SPRITES_2026-06-25.csv"
$policyPath = Join-Path $reviewDir "ENEMY_FRAMESHEET_IMPORT_POLICY_2026-06-25.md"
$contactSheetPath = Join-Path $reviewDir "ENEMY_FRAMESHEET_SLICED_SPRITES_2026-06-25.png"
$packetDate = "2026-06-25"

$framesheets = @(
    @{
        EnemyId = "black_mud_nightmare"
        AnimationId = "black_mud_move"
        SourceFile = "thecat_enemy_blackmud_move_framesheet_4x256_v001.png"
        OutputPrefix = "thecat_enemy_blackmud_move"
        SourceLock = "black_mud_animation"
        ReviewerNote = "Preserve red eyes, sludge body, crawling drag tail, and soft-mud silhouette."
    },
    @{
        EnemyId = "cold_light_shadow"
        AnimationId = "cold_light_cast"
        SourceFile = "thecat_enemy_coldlight_cast_framesheet_4x256_v001.png"
        OutputPrefix = "thecat_enemy_coldlight_cast"
        SourceLock = "cold_light_animation"
        ReviewerNote = "Preserve lamp silhouette, mechanical arm, black mud base, red eye, and cast pressure cue."
    },
    @{
        EnemyId = "call_tyrant"
        AnimationId = "call_tyrant_boss_pattern"
        SourceFile = "thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001.png"
        OutputPrefix = "thecat_enemy_calltyrant_bosspattern"
        SourceLock = "call_tyrant_animation"
        ReviewerNote = "Preserve phone boss body, red call eyes, black mud base, purple tie, and APP throw language."
    }
)

$enemyRowByAnimation = @{
    black_mud_move = 0
    cold_light_cast = 1
    call_tyrant_boss_pattern = 2
}

function ConvertTo-ProjectRelativePath {
    param([string]$Path)

    $fullPath = (Resolve-Path -LiteralPath $Path).Path
    $rootPath = $projectRoot.TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar
    $rootUri = [System.Uri]::new($rootPath)
    $fileUri = [System.Uri]::new($fullPath)
    return [System.Uri]::UnescapeDataString($rootUri.MakeRelativeUri($fileUri).ToString()).Replace("\", "/")
}

function Get-DeterministicGuid {
    param([string]$RelativePath)

    $md5 = [System.Security.Cryptography.MD5]::Create()
    try {
        $bytes = [System.Text.Encoding]::UTF8.GetBytes($RelativePath.ToLowerInvariant())
        $hash = $md5.ComputeHash($bytes)
        return (($hash | ForEach-Object { $_.ToString("x2") }) -join "")
    } finally {
        $md5.Dispose()
    }
}

function Write-SingleSpriteMeta {
    param(
        [string]$ImagePath,
        [int]$MaxTextureSize,
        [int]$FlipbookColumns,
        [string]$UserData
    )

    $relativePath = ConvertTo-ProjectRelativePath $ImagePath
    $guid = Get-DeterministicGuid $relativePath
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
  spritePixelsToUnits: 100
  spriteBorder: {x: 0, y: 0, z: 0, w: 0}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 1
  spriteTessellationDetail: -1
  textureType: 8
  textureShape: 1
  singleChannelComponent: 0
  flipbookRows: 1
  flipbookColumns: $FlipbookColumns
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
  spriteSheet:
    serializedVersion: 2
    sprites: []
    outline: []
    physicsShape: []
    bones: []
    spriteID: 5e97eb03825dee720800000000000000
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
  userData: $UserData
  assetBundleName: 
  assetBundleVariant: 
"@

    Set-Content -LiteralPath ($ImagePath + ".meta") -Value $content -Encoding UTF8
}

function Update-SourceFramesheetMeta {
    param([string]$ImagePath)

    $metaPath = $ImagePath + ".meta"
    if (-not (Test-Path -LiteralPath $metaPath)) {
        Write-SingleSpriteMeta $ImagePath 1024 4 "TheCatP0ImportSettings:v2;SourceFramesheetSingleSprite;UseSlicedSprites"
        return
    }

    $content = Get-Content -LiteralPath $metaPath -Raw
    $content = $content -replace "alphaIsTransparency: \d+", "alphaIsTransparency: 1"
    $content = $content -replace "textureType: \d+", "textureType: 8"
    $content = $content -replace "userData:.*", "userData: TheCatP0ImportSettings:v2;SourceFramesheetSingleSprite;UseSlicedSprites"
    Set-Content -LiteralPath $metaPath -Value $content -Encoding UTF8
}

function Copy-Frame {
    param(
        [System.Drawing.Bitmap]$Source,
        [int]$FrameIndex,
        [string]$OutputPath
    )

    $frame = [System.Drawing.Bitmap]::new(256, 256, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($frame)
    try {
        $graphics.CompositingMode = [System.Drawing.Drawing2D.CompositingMode]::SourceCopy
        $graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
        $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
        $graphics.Clear([System.Drawing.Color]::Transparent)
        $sourceRect = [System.Drawing.Rectangle]::new($FrameIndex * 256, 0, 256, 256)
        $destRect = [System.Drawing.Rectangle]::new(0, 0, 256, 256)
        $graphics.DrawImage($Source, $destRect, $sourceRect, [System.Drawing.GraphicsUnit]::Pixel)
        $frame.Save($OutputPath, [System.Drawing.Imaging.ImageFormat]::Png)
    } finally {
        $graphics.Dispose()
        $frame.Dispose()
    }
}

function Draw-CheckerTile {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Rectangle]$Rect
    )

    $dark = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(34, 33, 37))
    $light = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(55, 53, 60))
    try {
        for ($y = $Rect.Top; $y -lt $Rect.Bottom; $y += 16) {
            for ($x = $Rect.Left; $x -lt $Rect.Right; $x += 16) {
                $brush = if (((($x - $Rect.Left) / 16) + (($y - $Rect.Top) / 16)) % 2 -eq 0) { $dark } else { $light }
                $Graphics.FillRectangle($brush, $x, $y, 16, 16)
            }
        }
    } finally {
        $dark.Dispose()
        $light.Dispose()
    }
}

New-Item -ItemType Directory -Force -Path $slicedDir | Out-Null
New-Item -ItemType Directory -Force -Path $reviewDir | Out-Null

$rows = New-Object System.Collections.Generic.List[object]
$generatedSprites = New-Object System.Collections.Generic.List[object]

foreach ($framesheet in $framesheets) {
    $sourcePath = Join-Path $sourceDir $framesheet.SourceFile
    if (-not (Test-Path -LiteralPath $sourcePath)) {
        throw "Missing source framesheet: $sourcePath"
    }

    Update-SourceFramesheetMeta $sourcePath

    $source = [System.Drawing.Bitmap]::FromFile($sourcePath)
    try {
        if ($source.Width -ne 1024 -or $source.Height -ne 256) {
            throw "Expected 1024x256 source framesheet but found $($source.Width)x$($source.Height): $sourcePath"
        }

        for ($i = 0; $i -lt 4; $i++) {
            $frameNumber = $i + 1
            $outputName = "{0}_f{1:00}_256_v001.png" -f $framesheet.OutputPrefix, $frameNumber
            $outputPath = Join-Path $slicedDir $outputName
            Copy-Frame $source $i $outputPath
            Write-SingleSpriteMeta $outputPath 256 1 "TheCatP0ImportSettings:v1;EnemyFramesheetSlice;Source=$($framesheet.AnimationId);Frame=$frameNumber"

            $relativeSource = ConvertTo-ProjectRelativePath $sourcePath
            $relativeOutput = ConvertTo-ProjectRelativePath $outputPath
            $relativeMeta = ConvertTo-ProjectRelativePath ($outputPath + ".meta")
            $hash = (Get-FileHash -Algorithm SHA256 -LiteralPath $outputPath).Hash.ToLowerInvariant()
            $row = [pscustomobject]@{
                enemy_id = $framesheet.EnemyId
                animation_id = $framesheet.AnimationId
                frame_index = $frameNumber
                source_lock = $framesheet.SourceLock
                source_framesheet_path = $relativeSource
                output_sprite_path = $relativeOutput
                output_meta_path = $relativeMeta
                size_px = "256x256"
                sha256 = $hash
                status = "split_sprite_ready_pending_unity_runtime"
                reviewer_note = $framesheet.ReviewerNote
            }
            $rows.Add($row)
            $generatedSprites.Add([pscustomobject]@{
                Row = [int]$enemyRowByAnimation[$framesheet.AnimationId]
                Column = $i
                Label = "$($framesheet.AnimationId) f$frameNumber"
                Path = $outputPath
            })
        }
    } finally {
        $source.Dispose()
    }
}

$rows | Export-Csv -LiteralPath $manifestPath -NoTypeInformation -Encoding UTF8

$contactSheet = [System.Drawing.Bitmap]::new(1120, 900, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$graphics = [System.Drawing.Graphics]::FromImage($contactSheet)
$background = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(20, 18, 23))
$white = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(235, 232, 224))
$muted = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(178, 170, 162))
$font = [System.Drawing.Font]::new("Arial", 15, [System.Drawing.FontStyle]::Regular)
$titleFont = [System.Drawing.Font]::new("Arial", 22, [System.Drawing.FontStyle]::Bold)
try {
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
    $graphics.FillRectangle($background, 0, 0, $contactSheet.Width, $contactSheet.Height)
    $graphics.DrawString("P0 enemy framesheet sliced sprite review", $titleFont, $white, 32, 24)
    $graphics.DrawString("Source sheets stay as single-sprite references; these 12 slices are the import-test units.", $font, $muted, 32, 58)

    foreach ($sprite in $generatedSprites) {
        $x = 32 + ($sprite.Column * 270)
        $y = 100 + ($sprite.Row * 260)
        $tile = [System.Drawing.Rectangle]::new($x, $y, 256, 256)
        Draw-CheckerTile $graphics $tile
        $image = [System.Drawing.Bitmap]::FromFile($sprite.Path)
        try {
            $graphics.DrawImage($image, $tile)
        } finally {
            $image.Dispose()
        }

        $graphics.DrawString($sprite.Label, $font, $white, $x, $y + 260)
    }

    $contactSheet.Save($contactSheetPath, [System.Drawing.Imaging.ImageFormat]::Png)
} finally {
    $font.Dispose()
    $titleFont.Dispose()
    $background.Dispose()
    $white.Dispose()
    $muted.Dispose()
    $graphics.Dispose()
    $contactSheet.Dispose()
}

$policy = New-Object System.Collections.Generic.List[string]
$policy.Add("# Enemy Framesheet Import Policy")
$policy.Add("")
$policy.Add("Packet date: $packetDate")
$policy.Add("")
$policy.Add("## Decision")
$policy.Add("")
$policy.Add('Use the three existing 4x256 enemy framesheets as source-locked reference sheets and import them as single Sprite textures with alpha transparency. Their source-sheet `.meta` userData includes `SourceFramesheetSingleSprite`. Do not hand-author Unity Multiple Sprite YAML for these files in this pass, because the project has no reliable sliced Sprite meta template to copy.')
$policy.Add("")
$policy.Add('Runtime/import-test units are the 12 deterministic 256x256 split sprites under `Assets/TheCat/Art/Enemies/Frames/Sliced/`. Each split sprite has its own Unity `.meta` with `textureType: 8`, `spriteMode: 1`, `alphaIsTransparency: 1`, `maxTextureSize: 256`, `EnemyFramesheetSlice` userData, and a deterministic GUID derived from its project-relative path.')
$policy.Add("")
$policy.Add("## Source Sheets")
$policy.Add("")
$policy.Add("| Enemy | Animation | Source sheet | Source lock |")
$policy.Add("| --- | --- | --- | --- |")
foreach ($framesheet in $framesheets) {
    $sourcePath = Join-Path $sourceDir $framesheet.SourceFile
    $relativeSource = ConvertTo-ProjectRelativePath $sourcePath
    $policy.Add("| $($framesheet.EnemyId) | $($framesheet.AnimationId) | ``$relativeSource`` | $($framesheet.SourceLock) |")
}
$policy.Add("")
$policy.Add("## Split Sprites")
$policy.Add("")
$policy.Add("| Enemy | Animation | Frame | Split sprite | Hash |")
$policy.Add("| --- | --- | ---: | --- | --- |")
foreach ($row in $rows) {
    $policy.Add("| $($row.enemy_id) | $($row.animation_id) | $($row.frame_index) | ``$($row.output_sprite_path)`` | ``$($row.sha256)`` |")
}
$policy.Add("")
$policy.Add("## Review Rules")
$policy.Add("")
$policy.Add("- This policy resolves local import/slicing package readiness only; it is not Unity runtime acceptance.")
$policy.Add("- Starter cat bodies, starter cat framesheets, and character replacement sprites remain blocked by the character source-lock screenshot gate.")
$policy.Add("- No split sprite may be sourced from character asset directories or a starter-cat body packet.")
$policy.Add("- Next required evidence: Unity import refresh, active enemy animation screenshot, prefab/catalog binding proof, and Console check.")
$policy.Add('- Validator: `design/development/tools/validate_enemy_framesheet_import_policy.ps1`.')
$policy.Add("")
$policy.Add("## Review Contact Sheet")
$policy.Add("")
$policy.Add('`design/development/asset_review/ENEMY_FRAMESHEET_SLICED_SPRITES_2026-06-25.png`')

$policy | Set-Content -LiteralPath $policyPath -Encoding UTF8

Write-Output "Enemy framesheet sliced sprites generated: $($rows.Count)"
Write-Output (ConvertTo-ProjectRelativePath $manifestPath)
Write-Output (ConvertTo-ProjectRelativePath $policyPath)
Write-Output (ConvertTo-ProjectRelativePath $contactSheetPath)
