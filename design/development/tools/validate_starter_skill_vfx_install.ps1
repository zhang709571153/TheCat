Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_61_starter_skill_vfx_install_2026-06-15"
$sourceBatchSlug = "batch_55_starter_skill_vfx_candidates_2026-06-15"
$batchDirRelative = "design/development/asset_candidates/vfx/starter_skills/$batchSlug"
$manifestRelative = "$batchDirRelative/starter_skill_vfx_batch61_install_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchDirRelative/thecat_vfx_starter_skills_batch61_install_review_sheet.png"
$reviewNoteRelative = "$batchDirRelative/starter_skill_vfx_batch61_install_review.md"
$processNoteRelative = "$batchDirRelative/starter_skill_vfx_batch61_install_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_61_starter_skill_vfx_install.md"

$expected = @{
    saiban_bedline_skill_vfx = @{
        asset = "thecat_vfx_saiban_bedline_skill_512_v001"
        path = "Assets/TheCat/Art/VFX/thecat_vfx_saiban_bedline_skill_512_v001.png"
        binding = "skill_vfx.saiban_bedline"
        sourceLock = "saiban_turnaround_colored"
        skill = "saiban_oath_shield"
    }
    nephthys_moonsand_skill_vfx = @{
        asset = "thecat_vfx_nephthys_moonsand_skill_512_v001"
        path = "Assets/TheCat/Art/VFX/thecat_vfx_nephthys_moonsand_skill_512_v001.png"
        binding = "skill_vfx.nephthys_moonsand"
        sourceLock = "nephthys_turnaround_colored"
        skill = "nephthys_moon_sand_obelisk"
    }
    suzune_lullaby_skill_vfx = @{
        asset = "thecat_vfx_suzune_lullaby_skill_512_v001"
        path = "Assets/TheCat/Art/VFX/thecat_vfx_suzune_lullaby_skill_512_v001.png"
        binding = "skill_vfx.suzune_lullaby"
        sourceLock = "suzune_turnaround_colored"
        skill = "suzune_sleep_bell"
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

function Test-TransparentCorners {
    param([string]$Path)
    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $points = @(
            @(0, 0),
            @(($bitmap.Width - 1), 0),
            @(0, ($bitmap.Height - 1)),
            @(($bitmap.Width - 1), ($bitmap.Height - 1))
        )

        foreach ($point in $points) {
            $pixel = $bitmap.GetPixel($point[0], $point[1])
            if ($pixel.A -gt 12) {
                Add-Failure "$Path corner pixel $($point[0]),$($point[1]) should be transparent but alpha is $($pixel.A)"
            }
        }
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

function Test-Meta {
    param([string]$Path)
    $metaPath = "$Path.meta"
    if (-not (Test-Path -LiteralPath $metaPath)) {
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

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 61 starter skill VFX install manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 3) {
    Add-Failure "Expected 3 Batch 61 install rows but found $($rows.Count)"
}

foreach ($subjectId in $expected.Keys) {
    $matches = @($rows | Where-Object { $_.subject_id -eq $subjectId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected one Batch 61 row for $subjectId but found $($matches.Count)"
        continue
    }

    $row = $matches[0]
    $expectedRow = $expected[$subjectId]
    if ($row.batch_slug -ne $batchSlug) {
        Add-Failure "$subjectId has wrong batch slug: $($row.batch_slug)"
    }

    if ($row.asset_id -ne $expectedRow.asset) {
        Add-Failure "$subjectId has wrong asset id: $($row.asset_id)"
    }

    if ($row.asset_type -ne "vfx") {
        Add-Failure "$subjectId should use vfx asset type but found $($row.asset_type)"
    }

    if ($row.unity_import_path -ne $expectedRow.path) {
        Add-Failure "$subjectId has wrong Unity import path: $($row.unity_import_path)"
    }

    if ($row.runtime_binding_id -ne $expectedRow.binding) {
        Add-Failure "$subjectId has wrong runtime binding id: $($row.runtime_binding_id)"
    }

    if ($row.source_lock_id -ne $expectedRow.sourceLock) {
        Add-Failure "$subjectId has wrong source lock: $($row.source_lock_id)"
    }

    if ($row.mapped_skill_ids.IndexOf($expectedRow.skill, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "$subjectId mapped skills should include $($expectedRow.skill)"
    }

    if ($row.source_candidate_batch -ne $sourceBatchSlug) {
        Add-Failure "$subjectId should source from $sourceBatchSlug but found $($row.source_candidate_batch)"
    }

    if ($row.recommendation -ne "installed_symbolic_starter_skill_vfx_pending_unity_timing_smoke") {
        Add-Failure "$subjectId has unsafe recommendation: $($row.recommendation)"
    }

    $assetPath = Resolve-ProjectPath $row.unity_import_path
    if (-not (Test-Path -LiteralPath $assetPath)) {
        Add-Failure "$subjectId installed asset is missing: $($row.unity_import_path)"
    } else {
        $size = Get-ImageSize $assetPath
        if ($size -ne "512x512") {
            Add-Failure "$subjectId installed asset should be 512x512 but is $size"
        }

        if ($row.installed_size -ne "512x512") {
            Add-Failure "$subjectId manifest installed_size should be 512x512 but is $($row.installed_size)"
        }

        Test-TransparentCorners $assetPath
        Test-Hash $assetPath $row.installed_sha256 "$subjectId installed asset"
        Test-Meta $assetPath
    }

    Test-Hash (Resolve-ProjectPath $row.source_candidate_path) $row.source_candidate_sha256 "$subjectId source candidate"
}

foreach ($relative in @($reviewSheetRelative, $reviewNoteRelative, $processNoteRelative, $agentPromptRelative)) {
    if (-not (Test-Path -LiteralPath (Resolve-ProjectPath $relative))) {
        Add-Failure "Missing Batch 61 packet file: $relative"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (Test-Path -LiteralPath $reviewSheetPath) {
    $size = Get-ImageSize $reviewSheetPath
    if ($size -ne "1540x620") {
        Add-Failure "Batch 61 review sheet should be 1540x620 but is $size"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $text = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in @("Installed three symbolic starter skill VFX assets", "no cat bodies", "Runtime Scope", "Pending Unity Checks")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative missing token: $token"
        }
    }
}

$catalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs"
$catalogText = Get-Content -LiteralPath $catalogPath -Raw
foreach ($token in @("P0RuntimeVisualBindingCount = 111", "SaibanBedlineSkillVfxId", "GetStarterSkillVfx", "skill_vfx.saiban_bedline", "skill_vfx.nephthys_moonsand", "skill_vfx.suzune_lullaby")) {
    if ($catalogText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "P0VisualAssetCatalog missing token: $token"
    }
}

$manifestCatalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs"
$manifestCatalogText = Get-Content -LiteralPath $manifestCatalogPath -Raw
foreach ($token in @("P0ManifestAssetCount =", "Batch 61 symbolic starter skill VFX", "saiban_turnaround_colored", "thecat_vfx_suzune_lullaby_skill_512_v001")) {
    if ($manifestCatalogText.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        Add-Failure "P0AssetManifestCatalog missing token: $token"
    }
}

$feedbackPresenterPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Gameplay/P0BattleFeedbackVisualPresenter.cs"
$feedbackPresenterText = Get-Content -LiteralPath $feedbackPresenterPath -Raw
foreach ($token in @("GetStarterSkillVfx", "Silver Oath Shield", "Moon-Sand Obelisk", "Sleep Bell")) {
    if ($feedbackPresenterText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "P0BattleFeedbackVisualPresenter missing token: $token"
    }
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Starter skill VFX Batch 61 install validation failed with $($failures.Count) issue(s)."
}

Write-Host "Starter skill VFX Batch 61 install validation passed for $($rows.Count) asset(s)."
