Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_74_runtime_combat_sprite_source_audit_2026-06-15"
$batchRelative = "design/development/asset_candidates/starter_cats/$batchSlug"
$manifestRelative = "$batchRelative/starter_cat_runtime_combat_sprite_source_audit_batch74_manifest.csv"
$reviewSheetRelative = "$batchRelative/thecat_cat_starter_runtime_combat_sprite_source_audit_batch74_review_sheet.png"
$reviewNoteRelative = "$batchRelative/starter_cat_runtime_combat_sprite_source_audit_batch74_review.md"
$processNoteRelative = "$batchRelative/starter_cat_runtime_combat_sprite_source_audit_batch74_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_74_starter_cat_runtime_combat_sprite_source_audit.md"

$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetPath = Join-Path $projectRoot $reviewSheetRelative
$reviewNotePath = Join-Path $projectRoot $reviewNoteRelative
$processNotePath = Join-Path $projectRoot $processNoteRelative
$agentPromptPath = Join-Path $projectRoot $agentPromptRelative

$expected = @{
    saiban = @{
        asset = "thecat_cat_saiban_combat_sprite_512_v001"
        binding = "cat.combat.saiban"
        constant = "SaibanCombatSpriteId"
        lock = "saiban_turnaround_colored"
        sprite = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png"
        plate = "design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_saiban_turnaround_front_reference_plate_768_batch70_v001.png"
    }
    nephthys = @{
        asset = "thecat_cat_nephthys_combat_sprite_512_v001"
        binding = "cat.combat.nephthys"
        constant = "NephthysCombatSpriteId"
        lock = "nephthys_turnaround_colored"
        sprite = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png"
        plate = "design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_nephthys_turnaround_front_reference_plate_768_batch70_v001.png"
    }
    suzune = @{
        asset = "thecat_cat_suzune_combat_sprite_512_v001"
        binding = "cat.combat.suzune"
        constant = "SuzuneCombatSpriteId"
        lock = "suzune_turnaround_colored"
        sprite = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png"
        plate = "design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_front_reference_plate_768_batch70_v001.png"
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
        for ($y = 0; $y -lt $bitmap.Height; $y += 8) {
            for ($x = 0; $x -lt $bitmap.Width; $x += 8) {
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

    if (-not (Test-Path -LiteralPath $Path)) {
        Add-Failure "$Label missing at $Path"
        return
    }

    $actual = (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
    if ($actual -ne $ExpectedHash.ToLowerInvariant()) {
        Add-Failure "$Label hash mismatch. Expected $ExpectedHash but found $actual"
    }
}

function Test-Contains {
    param(
        [string]$Text,
        [string]$Token,
        [string]$Label
    )

    if ($Text.IndexOf($Token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        Add-Failure "$Label missing token: $Token"
    }
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 74 runtime combat sprite source audit manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne 3) {
    Add-Failure "Expected 3 Batch 74 runtime sprite audit rows but found $($rows.Count)"
}

foreach ($catId in $expected.Keys) {
    $matches = @($rows | Where-Object { $_.cat_id -eq $catId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected one Batch 74 row for $catId but found $($matches.Count)"
        continue
    }

    $row = $matches[0]
    $expectedRow = $expected[$catId]

    foreach ($pair in @(
        @("batch_slug", $batchSlug),
        @("asset_id", $expectedRow.asset),
        @("asset_type", "runtime_combat_sprite"),
        @("runtime_binding_id", $expectedRow.binding),
        @("visual_catalog_constant", $expectedRow.constant),
        @("source_lock_id", $expectedRow.lock),
        @("front_reference_plate_path", $expectedRow.plate),
        @("runtime_sprite_path", $expectedRow.sprite),
        @("runtime_sprite_meta_path", "$($expectedRow.sprite).meta"),
        @("runtime_sprite_size", "512x512"),
        @("runtime_sprite_has_alpha", "yes"),
        @("recommendation", "runtime_sprite_source_audit_ready_pending_unity_playmode_screenshot")
    )) {
        if ($row.($pair[0]) -ne $pair[1]) {
            Add-Failure "$catId $($pair[0]) expected $($pair[1]) but found $($row.($pair[0]))"
        }
    }

    $spritePath = Resolve-ProjectPath $row.runtime_sprite_path
    $platePath = Resolve-ProjectPath $row.front_reference_plate_path
    $sourcePath = Resolve-ProjectPath $row.source_turnaround_path
    $metaPath = Resolve-ProjectPath $row.runtime_sprite_meta_path

    if (Test-Path -LiteralPath $spritePath) {
        $spriteSize = Get-ImageSize $spritePath
        if ($spriteSize -ne "512x512") {
            Add-Failure "$catId runtime sprite should be 512x512 but is $spriteSize"
        }

        if (-not (Test-ImageHasAlpha $spritePath)) {
            Add-Failure "$catId runtime sprite should contain alpha transparency."
        }
    }

    if (Test-Path -LiteralPath $platePath) {
        $plateSize = Get-ImageSize $platePath
        if ($plateSize -ne "768x768") {
            Add-Failure "$catId Batch 70 front plate should be 768x768 but is $plateSize"
        }
    }

    Test-Hash $spritePath $row.runtime_sprite_sha256 "$catId runtime sprite"
    Test-Hash $platePath $row.front_reference_plate_sha256 "$catId front reference plate"
    Test-Hash $sourcePath $row.source_turnaround_sha256 "$catId source turnaround"

    if (-not (Test-Path -LiteralPath $metaPath)) {
        Add-Failure "$catId runtime sprite meta missing: $($row.runtime_sprite_meta_path)"
    } else {
        $metaText = Get-Content -LiteralPath $metaPath -Raw
        foreach ($token in @("TextureImporter:", "textureType: 8", "spriteMode: 1", "enableMipMap: 0", "alphaIsTransparency: 1", "userData: TheCatP0ImportSettings:v1")) {
            Test-Contains $metaText $token $row.runtime_sprite_meta_path
        }
    }
}

foreach ($path in @($reviewSheetPath, $reviewNotePath, $processNotePath, $agentPromptPath)) {
    if (-not (Test-Path -LiteralPath $path)) {
        Add-Failure "Missing Batch 74 audit artifact: $path"
    }
}

if (Test-Path -LiteralPath $reviewSheetPath) {
    $sheetSize = Get-ImageSize $reviewSheetPath
    if ($sheetSize -ne "2200x1320") {
        Add-Failure "Batch 74 review sheet should be 2200x1320 but is $sheetSize"
    }
}

if (Test-Path -LiteralPath $reviewNotePath) {
    $reviewText = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in @(
        "runtime sprite source audit ready",
        "does not generate new cat body art",
        "does not replace any Unity sprite",
        "does not approve AI-generated",
        "P0StarterCatFormalImportReadiness",
        "active-cat Play Mode screenshots"
    )) {
        Test-Contains $reviewText $token $reviewNoteRelative
    }
}

if (Test-Path -LiteralPath $processNotePath) {
    $processText = Get-Content -LiteralPath $processNotePath -Raw
    foreach ($token in @("Image generation model: not used", "Unity runtime assets changed: none", "Existing runtime-bound combat sprites audited: 3")) {
        Test-Contains $processText $token $processNoteRelative
    }
}

$visualCatalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs"
$visualCatalogText = Get-Content -LiteralPath $visualCatalogPath -Raw
foreach ($token in @(
    "SaibanCombatSpriteId",
    "NephthysCombatSpriteId",
    "SuzuneCombatSpriteId",
    "cat.combat.saiban",
    "cat.combat.nephthys",
    "cat.combat.suzune"
)) {
    Test-Contains $visualCatalogText $token "P0VisualAssetCatalog"
}

$reviewPacketPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Tools/P0AssetReviewPacket.cs"
if (Test-Path -LiteralPath $reviewPacketPath) {
    $reviewPacketText = Get-Content -LiteralPath $reviewPacketPath -Raw
    foreach ($token in @(
        "Starter Cat Runtime Combat Sprite Source Audit",
        "P0StarterCatRuntimeCombatSpriteAuditEvidence",
        "runtime-bound sprites; no Unity sprite replacement"
    )) {
        Test-Contains $reviewPacketText $token "P0AssetReviewPacket"
    }
}

$evidencePath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Tools/P0StarterCatRuntimeCombatSpriteAuditEvidence.cs"
if (Test-Path -LiteralPath $evidencePath) {
    $evidenceText = Get-Content -LiteralPath $evidencePath -Raw
    foreach ($token in @(
        $batchSlug,
        "ExpectedStarterCatCount = 3",
        "ExpectedRuntimeSpriteCount = 3",
        "ExpectedRuntimeBindingMentionCount = 3",
        "runtime_sprite_source_audit_ready_pending_unity_playmode_screenshot"
    )) {
        Test-Contains $evidenceText $token "P0StarterCatRuntimeCombatSpriteAuditEvidence"
    }
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Starter-cat Batch 74 runtime combat sprite source audit validation failed with $($failures.Count) issue(s)."
}

Write-Host "Starter-cat Batch 74 runtime combat sprite source audit validation passed."
Write-Host "Rows: $($rows.Count)"
Write-Host "Runtime combat sprites audited: 3"
Write-Host "Unity active-cat screenshot comparison remains pending."
