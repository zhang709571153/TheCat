Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$expectedRecommendation = "installed_debug_reference_not_runtime_binding_pending_unity_visual_smoke"

$specs = @(
    @{
        CatId = "saiban"
        DisplayName = "Saiban"
        BatchNumber = 71
        BatchSlug = "batch_71_saiban_unity_reference_install_2026-06-15"
        BatchRelative = "design/development/asset_candidates/starter_cats/saiban/batch_71_saiban_unity_reference_install_2026-06-15"
        Manifest = "saiban_batch71_unity_reference_install_manifest.csv"
        ReviewSheet = "thecat_cat_saiban_batch71_unity_reference_install_review_sheet.png"
        ReviewNote = "saiban_batch71_unity_reference_install_review.md"
        ProcessNote = "saiban_batch71_unity_reference_install_process_note.md"
        AgentPrompt = "design/development/agent_prompts/p0_asset_batch_71_saiban_unity_reference_install.md"
        AssetId = "thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001"
        SourceLock = "saiban_turnaround_colored"
    },
    @{
        CatId = "nephthys"
        DisplayName = "Nephthys"
        BatchNumber = 72
        BatchSlug = "batch_72_nephthys_unity_reference_install_2026-06-15"
        BatchRelative = "design/development/asset_candidates/starter_cats/nephthys/batch_72_nephthys_unity_reference_install_2026-06-15"
        Manifest = "nephthys_batch72_unity_reference_install_manifest.csv"
        ReviewSheet = "thecat_cat_nephthys_batch72_unity_reference_install_review_sheet.png"
        ReviewNote = "nephthys_batch72_unity_reference_install_review.md"
        ProcessNote = "nephthys_batch72_unity_reference_install_process_note.md"
        AgentPrompt = "design/development/agent_prompts/p0_asset_batch_72_nephthys_unity_reference_install.md"
        AssetId = "thecat_cat_nephthys_turnaround_reference_atlas_2304x768_v001"
        SourceLock = "nephthys_turnaround_colored"
    },
    @{
        CatId = "suzune"
        DisplayName = "Suzune"
        BatchNumber = 73
        BatchSlug = "batch_73_suzune_unity_reference_install_2026-06-15"
        BatchRelative = "design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15"
        Manifest = "suzune_batch73_unity_reference_install_manifest.csv"
        ReviewSheet = "thecat_cat_suzune_batch73_unity_reference_install_review_sheet.png"
        ReviewNote = "suzune_batch73_unity_reference_install_review.md"
        ProcessNote = "suzune_batch73_unity_reference_install_process_note.md"
        AgentPrompt = "design/development/agent_prompts/p0_asset_batch_73_suzune_unity_reference_install.md"
        AssetId = "thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001"
        SourceLock = "suzune_turnaround_colored"
    }
)

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

foreach ($spec in $specs) {
    $manifestRelative = "$($spec.BatchRelative)/$($spec.Manifest)"
    $manifestPath = Resolve-ProjectPath $manifestRelative
    $expectedUnityPath = "Assets/TheCat/Art/Characters/References/$($spec.AssetId).png"

    if (-not (Test-Path -LiteralPath $manifestPath)) {
        Add-Failure "Batch $($spec.BatchNumber) manifest is missing: $manifestRelative"
        continue
    }

    $rows = @(Import-Csv -LiteralPath $manifestPath)
    if ($rows.Count -ne 1) {
        Add-Failure "Expected 1 Batch $($spec.BatchNumber) row but found $($rows.Count)"
    }

    if ($rows.Count -gt 0) {
        $row = $rows[0]
        if ($row.cat_id -ne $spec.CatId) {
            Add-Failure "Expected cat_id $($spec.CatId) but found $($row.cat_id)"
        }

        if ($row.batch_slug -ne $spec.BatchSlug) {
            Add-Failure "Expected batch slug $($spec.BatchSlug) but found $($row.batch_slug)"
        }

        if ($row.asset_id -ne $spec.AssetId) {
            Add-Failure "Expected asset id $($spec.AssetId) but found $($row.asset_id)"
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

        if ($row.source_lock_id -ne $spec.SourceLock) {
            Add-Failure "Expected source lock $($spec.SourceLock) but found $($row.source_lock_id)"
        }

        if ($row.recommendation -ne $expectedRecommendation) {
            Add-Failure "Unsafe recommendation for $($spec.DisplayName): $($row.recommendation)"
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

            Test-Hash $assetPath $row.installed_sha256 "$($spec.DisplayName) installed atlas"
            Test-Meta $assetPath
        }

        Test-Hash (Resolve-ProjectPath $row.source_turnaround_path) $row.source_turnaround_sha256 "$($spec.DisplayName) source turnaround"
        Test-Hash (Resolve-ProjectPath $row.locked_combat_sprite_path) $row.locked_combat_sprite_sha256 "$($spec.DisplayName) locked combat sprite"
        Test-Hash (Resolve-ProjectPath $row.front_reference_plate_path) $row.front_reference_plate_sha256 "$($spec.DisplayName) front reference plate"
        Test-Hash (Resolve-ProjectPath $row.side_reference_plate_path) $row.side_reference_plate_sha256 "$($spec.DisplayName) side reference plate"
        Test-Hash (Resolve-ProjectPath $row.back_reference_plate_path) $row.back_reference_plate_sha256 "$($spec.DisplayName) back reference plate"
    }

    foreach ($relative in @(
        "$($spec.BatchRelative)/$($spec.ReviewSheet)",
        "$($spec.BatchRelative)/$($spec.ReviewNote)",
        "$($spec.BatchRelative)/$($spec.ProcessNote)",
        $spec.AgentPrompt)) {
        if (-not (Test-Path -LiteralPath (Resolve-ProjectPath $relative))) {
            Add-Failure "Missing Batch $($spec.BatchNumber) packet file: $relative"
        }
    }

    $reviewSheetPath = Resolve-ProjectPath "$($spec.BatchRelative)/$($spec.ReviewSheet)"
    if (Test-Path -LiteralPath $reviewSheetPath) {
        $size = Get-ImageSize $reviewSheetPath
        if ($size -ne "2200x1180") {
            Add-Failure "Batch $($spec.BatchNumber) review sheet should be 2200x1180 but is $size"
        }
    }

    $reviewNotePath = Resolve-ProjectPath "$($spec.BatchRelative)/$($spec.ReviewNote)"
    if (Test-Path -LiteralPath $reviewNotePath) {
        $text = Get-Content -LiteralPath $reviewNotePath -Raw
        foreach ($token in @("source-derived", "does not replace the existing $($spec.DisplayName) combat sprite", "not a runtime-bound combat sprite", "Formal starter-cat body-art import remains blocked", "No AI-generated cat body candidate was imported", "Unity visual smoke remains pending")) {
            if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
                Add-Failure "$($spec.ReviewNote) missing token: $token"
            }
        }
    }

    $processNotePath = Resolve-ProjectPath "$($spec.BatchRelative)/$($spec.ProcessNote)"
    if (Test-Path -LiteralPath $processNotePath) {
        $text = Get-Content -LiteralPath $processNotePath -Raw
        foreach ($token in @("No image generation was performed", "deterministically from Batch 70", "not runtime-bound")) {
            if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
                Add-Failure "$($spec.ProcessNote) missing token: $token"
            }
        }
    }
}

$manifestCatalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs"
$manifestCatalogText = Get-Content -LiteralPath $manifestCatalogPath -Raw
foreach ($spec in $specs) {
    foreach ($token in @($spec.AssetId, "reference_atlas", "Batch $($spec.BatchNumber) Unity-installed source-derived $($spec.DisplayName) front/side/back turnaround reference atlas")) {
        if ($manifestCatalogText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
            Add-Failure "P0AssetManifestCatalog missing token: $token"
        }
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

    throw "Starter-cat Unity reference install validation failed with $($failures.Count) issue(s)."
}

Write-Host "Starter-cat Unity reference install validation passed for $($specs.Count) cat(s)."
Write-Host "Installed atlases: 3"
Write-Host "Unity visual smoke remains pending."
