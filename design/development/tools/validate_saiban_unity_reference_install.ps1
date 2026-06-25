Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_71_saiban_unity_reference_install_2026-06-15"
$batchRelative = "design/development/asset_candidates/starter_cats/saiban/$batchSlug"
$manifestRelative = "$batchRelative/saiban_batch71_unity_reference_install_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchRelative/thecat_cat_saiban_batch71_unity_reference_install_review_sheet.png"
$reviewNoteRelative = "$batchRelative/saiban_batch71_unity_reference_install_review.md"
$processNoteRelative = "$batchRelative/saiban_batch71_unity_reference_install_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_71_saiban_unity_reference_install.md"
$expectedAssetId = "thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001"
$expectedUnityPath = "Assets/TheCat/Art/Characters/References/$expectedAssetId.png"
$expectedRecommendation = "installed_debug_reference_not_runtime_binding_pending_unity_visual_smoke"

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

function Test-Hash {
    param(
        [string]$Path,
        [string]$ExpectedHash,
        [string]$Label
    )

    if (-not (Test-Path -LiteralPath $Path)) {
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
    if (-not (Test-Path -LiteralPath $metaPath)) {
        Add-Failure "Missing Unity meta file: $metaPath"
        return
    }

    $text = Get-Content -LiteralPath $metaPath -Raw
    foreach ($token in @("TextureImporter:", "textureType: 8", "spriteMode: 1", "enableMipMap: 0", "alphaIsTransparency: 1", "maxTextureSize: 4096", "userData: TheCatP0ImportSettings:v1")) {
        if ($text.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
            Add-Failure "$metaPath missing import setting token: $token"
        }
    }
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 71 manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne 1) {
    Add-Failure "Expected 1 Batch 71 row but found $($rows.Count)"
}

if ($rows.Count -gt 0) {
    $row = $rows[0]
    if ($row.cat_id -ne "saiban") {
        Add-Failure "Expected cat_id saiban but found $($row.cat_id)"
    }

    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "Expected batch slug $batchSlug but found $($row.batch_slug)"
    }

    if ($row.asset_id -ne $expectedAssetId) {
        Add-Failure "Expected asset id $expectedAssetId but found $($row.asset_id)"
    }

    if ($row.asset_type -ne "reference_atlas") {
        Add-Failure "Expected reference_atlas asset type but found $($row.asset_type)"
    }

    if ($row.unity_import_path -ne $expectedUnityPath) {
        Add-Failure "Expected Unity path $expectedUnityPath but found $($row.unity_import_path)"
    }

    if ($row.unity_meta_path -ne "$expectedUnityPath.meta") {
        Add-Failure "Expected Unity meta path $expectedUnityPath.meta but found $($row.unity_meta_path)"
    }

    if ($row.source_lock_id -ne "saiban_turnaround_colored") {
        Add-Failure "Expected Saiban source lock but found $($row.source_lock_id)"
    }

    if ($row.recommendation -ne $expectedRecommendation) {
        Add-Failure "Unsafe recommendation: $($row.recommendation)"
    }

    $assetPath = Resolve-ProjectPath $row.unity_import_path
    if (-not (Test-Path -LiteralPath $assetPath)) {
        Add-Failure "Installed atlas is missing: $($row.unity_import_path)"
    } else {
        $size = Get-ImageSize $assetPath
        if ($size -ne "2304x768") {
            Add-Failure "Installed atlas should be 2304x768 but is $size"
        }

        if ($row.installed_size -ne "2304x768") {
            Add-Failure "Manifest installed_size should be 2304x768 but is $($row.installed_size)"
        }

        Test-Hash $assetPath $row.installed_sha256 "installed atlas"
        Test-Meta $assetPath
    }

    Test-Hash (Resolve-ProjectPath $row.source_turnaround_path) $row.source_turnaround_sha256 "Saiban source turnaround"
    Test-Hash (Resolve-ProjectPath $row.locked_combat_sprite_path) $row.locked_combat_sprite_sha256 "Saiban locked combat sprite"
    Test-Hash (Resolve-ProjectPath $row.front_reference_plate_path) $row.front_reference_plate_sha256 "front reference plate"
    Test-Hash (Resolve-ProjectPath $row.side_reference_plate_path) $row.side_reference_plate_sha256 "side reference plate"
    Test-Hash (Resolve-ProjectPath $row.back_reference_plate_path) $row.back_reference_plate_sha256 "back reference plate"
}

foreach ($relative in @($reviewSheetRelative, $reviewNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    if (-not (Test-Path -LiteralPath (Resolve-ProjectPath $relative))) {
        Add-Failure "Missing Batch 71 packet file: $relative"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (Test-Path -LiteralPath $reviewSheetPath) {
    $size = Get-ImageSize $reviewSheetPath
    if ($size -ne "2200x1180") {
        Add-Failure "Batch 71 review sheet should be 2200x1180 but is $size"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $text = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in @("source-derived", "does not replace the existing Saiban combat sprite", "not a runtime-bound combat sprite", "Formal starter-cat body-art import remains blocked", "No AI-generated cat body candidate was imported", "Unity visual smoke remains pending")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative missing token: $token"
        }
    }
}

$processNotePath = Resolve-ProjectPath $processNoteRelative
if (Test-Path -LiteralPath $processNotePath) {
    $text = Get-Content -LiteralPath $processNotePath -Raw
    foreach ($token in @("No image generation was performed", "deterministically from Batch 70", "not runtime-bound")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$processNoteRelative missing token: $token"
        }
    }
}

$manifestCatalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs"
$manifestCatalogText = Get-Content -LiteralPath $manifestCatalogPath -Raw
foreach ($token in @($expectedAssetId, "reference_atlas", "Batch 71 Unity-installed source-derived Saiban front/side/back turnaround reference atlas")) {
    if ($manifestCatalogText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "P0AssetManifestCatalog missing token: $token"
    }
}

$reviewPacketPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs"
$reviewPacketText = Get-Content -LiteralPath $reviewPacketPath -Raw
foreach ($token in @("StarterCatUnityReferenceInstallReady", "AppendStarterCatUnityReferenceInstall", "Batch 71-73 Starter Cat Unity Reference Installs")) {
    if ($reviewPacketText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "P0AssetReviewPacket missing token: $token"
    }
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Saiban Batch 71 Unity reference install validation failed with $($failures.Count) issue(s)."
}

Write-Host "Saiban Batch 71 Unity reference install validation passed."
Write-Host "Installed atlas: $expectedUnityPath"
Write-Host "Unity visual smoke remains pending."
