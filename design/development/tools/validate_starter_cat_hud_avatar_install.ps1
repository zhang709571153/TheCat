Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_58_starter_cat_hud_avatar_install_2026-06-15"
$manifestRelative = "design/development/asset_candidates/starter_cats/$batchSlug/starter_cat_batch58_hud_avatar_install_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "design/development/asset_candidates/starter_cats/$batchSlug/thecat_starter_cat_batch58_hud_avatar_install_review_sheet.png"
$reviewNoteRelative = "design/development/asset_candidates/starter_cats/$batchSlug/starter_cat_batch58_hud_avatar_install_review.md"
$processNoteRelative = "design/development/asset_candidates/starter_cats/$batchSlug/starter_cat_batch58_hud_avatar_install_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_58_starter_cat_hud_avatar_install.md"

$expected = @{
    saiban = @{
        asset = "thecat_cat_saiban_hud_avatar_256_v001"
        path = "Assets/TheCat/Art/UI/Icons/thecat_cat_saiban_hud_avatar_256_v001.png"
        lock = "saiban_turnaround_colored"
    }
    nephthys = @{
        asset = "thecat_cat_nephthys_hud_avatar_256_v001"
        path = "Assets/TheCat/Art/UI/Icons/thecat_cat_nephthys_hud_avatar_256_v001.png"
        lock = "nephthys_turnaround_colored"
    }
    suzune = @{
        asset = "thecat_cat_suzune_hud_avatar_256_v001"
        path = "Assets/TheCat/Art/UI/Icons/thecat_cat_suzune_hud_avatar_256_v001.png"
        lock = "suzune_turnaround_colored"
    }
}

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function Resolve-ProjectPath {
    param([string]$RelativePath)
    return Join-Path $projectRoot ($RelativePath -replace "/", [System.IO.Path]::DirectorySeparatorChar)
}

function Get-ImageSize {
    param([string]$Path)
    $image = [System.Drawing.Image]::FromFile($Path)
    try {
        return "$($image.Width)x$($image.Height)"
    } finally {
        $image.Dispose()
    }
}

function Test-ImageHasAlpha {
    param([string]$Path)
    $bitmap = [System.Drawing.Bitmap]::FromFile($Path)
    try {
        for ($y = 0; $y -lt $bitmap.Height; $y += 16) {
            for ($x = 0; $x -lt $bitmap.Width; $x += 16) {
                if ($bitmap.GetPixel($x, $y).A -lt 255) {
                    return $true
                }
            }
        }

        return $false
    } finally {
        $bitmap.Dispose()
    }
}

function Test-Hash {
    param(
        [string]$Path,
        [string]$ExpectedHash,
        [string]$Label
    )

    if (-not (Test-Path $Path)) {
        Add-Failure "$Label missing at $Path"
        return
    }

    $actual = (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
    if ($actual -ne $ExpectedHash.ToLowerInvariant()) {
        Add-Failure "$Label hash mismatch. Expected $ExpectedHash but found $actual"
    }
}

function Test-Meta {
    param([string]$Path)
    $metaPath = "$Path.meta"
    if (-not (Test-Path $metaPath)) {
        Add-Failure "Missing Unity meta file: $metaPath"
        return
    }

    $text = Get-Content -LiteralPath $metaPath -Raw
    foreach ($token in @("TextureImporter:", "textureType: 8", "spriteMode: 1", "enableMipMap: 0", "alphaIsTransparency: 1", "userData: TheCatP0ImportSettings:v1")) {
        if ($text.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
            Add-Failure "$metaPath missing import setting token: $token"
        }
    }
}

if (-not (Test-Path $manifestPath)) {
    throw "Batch 58 starter-cat HUD avatar manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne 3) {
    Add-Failure "Expected 3 Batch 58 avatar rows but found $($rows.Count)"
}

foreach ($catId in $expected.Keys) {
    $matches = @($rows | Where-Object { $_.cat_id -eq $catId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected one Batch 58 row for $catId but found $($matches.Count)"
        continue
    }

    $row = $matches[0]
    $expectedRow = $expected[$catId]
    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$catId has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.asset_id -ne $expectedRow.asset) {
        Add-Failure "$catId has wrong asset id: $($row.asset_id)"
    }

    if ($row.asset_type -ne "avatar_icon") {
        Add-Failure "$catId should use avatar_icon asset type but found $($row.asset_type)"
    }

    if ($row.unity_import_path -ne $expectedRow.path) {
        Add-Failure "$catId has wrong Unity import path: $($row.unity_import_path)"
    }

    if ($row.source_lock_id -ne $expectedRow.lock) {
        Add-Failure "$catId has wrong source lock id: $($row.source_lock_id)"
    }

    if ($row.recommendation -ne "installed_source_locked_avatar_pending_unity_visual_smoke") {
        Add-Failure "$catId has unsafe recommendation: $($row.recommendation)"
    }

    $assetPath = Resolve-ProjectPath $row.unity_import_path
    if (-not (Test-Path $assetPath)) {
        Add-Failure "$catId installed avatar is missing: $($row.unity_import_path)"
    } else {
        $size = Get-ImageSize $assetPath
        if ($size -ne "256x256") {
            Add-Failure "$catId installed avatar should be 256x256 but is $size"
        }

        if ($row.installed_size -ne "256x256") {
            Add-Failure "$catId manifest installed_size should be 256x256 but is $($row.installed_size)"
        }

        if (-not (Test-ImageHasAlpha $assetPath)) {
            Add-Failure "$catId installed avatar should contain alpha transparency."
        }

        Test-Hash $assetPath $row.installed_sha256 "$catId installed avatar"
        Test-Meta $assetPath
    }

    Test-Hash (Resolve-ProjectPath $row.source_turnaround_path) $row.source_turnaround_sha256 "$catId source turnaround"
    Test-Hash (Resolve-ProjectPath $row.locked_sprite_path) $row.locked_sprite_sha256 "$catId locked sprite"
}

foreach ($relative in @($reviewSheetRelative, $reviewNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    if (-not (Test-Path (Resolve-ProjectPath $relative))) {
        Add-Failure "Missing Batch 58 packet file: $relative"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (Test-Path $reviewSheetPath) {
    $size = Get-ImageSize $reviewSheetPath
    if ($size -ne "2100x760") {
        Add-Failure "Batch 58 review sheet should be 2100x760 but is $size"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path $reviewNotePath) {
    $text = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in @("installed source-locked HUD avatars", "No AI-generated cat body candidate was imported", "Does not modify or replace starter-cat combat sprites", "Unity visual smoke still pending")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative missing token: $token"
        }
    }
}

$catalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs"
$catalogText = Get-Content -LiteralPath $catalogPath -Raw
foreach ($token in @("SaibanHudAvatarId", "NephthysHudAvatarId", "SuzuneHudAvatarId", "GetStarterCatHudAvatar", "cat.avatar.saiban", "cat.avatar.nephthys", "cat.avatar.suzune")) {
    if ($catalogText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "P0VisualAssetCatalog missing token: $token"
    }
}

$manifestCatalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs"
$manifestCatalogText = Get-Content -LiteralPath $manifestCatalogPath -Raw
foreach ($token in @("avatar_icon", "Batch 58 source-locked HUD avatar", "no AI body-art candidate imported")) {
    if ($manifestCatalogText.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        Add-Failure "P0AssetManifestCatalog missing token: $token"
    }
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Starter-cat Batch 58 HUD avatar install validation failed with $($failures.Count) issue(s)."
}

Write-Host "Starter-cat Batch 58 HUD avatar install validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Installed avatars: 3"
Write-Host "Unity visual smoke remains pending."
