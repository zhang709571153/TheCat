Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$batchSlug = "batch_60_skill_hud_feedback_install_2026-06-15"
$sourceBatchSlug = "batch_57_skill_hud_feedback_candidates_2026-06-15"
$batchDirRelative = "design/development/asset_candidates/ui/skill_hud/$batchSlug"
$manifestRelative = "$batchDirRelative/skill_hud_feedback_batch60_install_manifest.csv"
$manifestPath = Join-Path $projectRoot $manifestRelative
$reviewSheetRelative = "$batchDirRelative/thecat_ui_skill_hud_feedback_batch60_install_review_sheet.png"
$reviewNoteRelative = "$batchDirRelative/skill_hud_feedback_batch60_install_review.md"
$processNoteRelative = "$batchDirRelative/skill_hud_feedback_batch60_install_process_note.md"
$agentPromptRelative = "design/development/agent_prompts/p0_asset_batch_60_skill_hud_feedback_install.md"

$expected = @{
    skill_ready_frame = @{
        asset = "thecat_ui_skill_ready_frame_512_v001"
        path = "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_ready_frame_512_v001.png"
        binding = "skill_hud.ready_frame"
    }
    skill_cooldown_overlay = @{
        asset = "thecat_ui_skill_cooldown_overlay_512_v001"
        path = "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png"
        binding = "skill_hud.cooldown_overlay"
    }
    skill_no_target_marker = @{
        asset = "thecat_ui_skill_no_target_marker_512_v001"
        path = "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png"
        binding = "skill_hud.no_target_marker"
    }
    skill_hunger_cost_chip = @{
        asset = "thecat_ui_skill_hunger_cost_chip_512_v001"
        path = "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png"
        binding = "skill_hud.hunger_cost_chip"
    }
    auto_target_reticle = @{
        asset = "thecat_ui_auto_target_reticle_512_v001"
        path = "Assets/TheCat/Art/UI/Icons/thecat_ui_auto_target_reticle_512_v001.png"
        binding = "skill_hud.auto_target_reticle"
    }
    interaction_range_ripple = @{
        asset = "thecat_ui_interaction_range_ripple_512_v001"
        path = "Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png"
        binding = "battle_hud.interaction_range_ripple"
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
            if ($pixel.A -gt 8) {
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
    throw "Batch 60 skill HUD feedback install manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath -Encoding UTF8)
if ($rows.Count -ne 6) {
    Add-Failure "Expected 6 Batch 60 install rows but found $($rows.Count)"
}

foreach ($subjectId in $expected.Keys) {
    $matches = @($rows | Where-Object { $_.subject_id -eq $subjectId })
    if ($matches.Count -ne 1) {
        Add-Failure "Expected one Batch 60 row for $subjectId but found $($matches.Count)"
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

    if ($row.asset_type -ne "skill_hud_feedback") {
        Add-Failure "$subjectId should use skill_hud_feedback asset type but found $($row.asset_type)"
    }

    if ($row.unity_import_path -ne $expectedRow.path) {
        Add-Failure "$subjectId has wrong Unity import path: $($row.unity_import_path)"
    }

    if ($row.runtime_binding_id -ne $expectedRow.binding) {
        Add-Failure "$subjectId has wrong runtime binding id: $($row.runtime_binding_id)"
    }

    if ($row.source_candidate_batch -ne $sourceBatchSlug) {
        Add-Failure "$subjectId should source from $sourceBatchSlug but found $($row.source_candidate_batch)"
    }

    if ($row.recommendation -ne "installed_non_cat_skill_hud_feedback_pending_unity_visual_smoke") {
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
        Add-Failure "Missing Batch 60 packet file: $relative"
    }
}

$reviewSheetPath = Resolve-ProjectPath $reviewSheetRelative
if (Test-Path -LiteralPath $reviewSheetPath) {
    $size = Get-ImageSize $reviewSheetPath
    if ($size -ne "1860x980") {
        Add-Failure "Batch 60 review sheet should be 1860x980 but is $size"
    }
}

$reviewNotePath = Resolve-ProjectPath $reviewNoteRelative
if (Test-Path -LiteralPath $reviewNotePath) {
    $text = Get-Content -LiteralPath $reviewNotePath -Raw
    foreach ($token in @("Installed six non-cat Skill HUD feedback assets", "no cat bodies", "Runtime Scope", "Pending Unity Checks")) {
        if ($text.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
            Add-Failure "$reviewNoteRelative missing token: $token"
        }
    }
}

$catalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs"
$catalogText = Get-Content -LiteralPath $catalogPath -Raw
foreach ($token in @("P0RuntimeVisualBindingCount = 111", "SkillReadyFrameId", "GetSkillHudStatusFeedback", "GetAutoTargetReticle", "GetInteractionRangeRipple", "skill_hud.ready_frame", "battle_hud.interaction_range_ripple")) {
    if ($catalogText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "P0VisualAssetCatalog missing token: $token"
    }
}

$manifestCatalogPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs"
$manifestCatalogText = Get-Content -LiteralPath $manifestCatalogPath -Raw
foreach ($token in @("P0ManifestAssetCount =", "skill_hud_feedback", "Batch 60 non-cat Skill HUD feedback")) {
    if ($manifestCatalogText.IndexOf($token, [System.StringComparison]::OrdinalIgnoreCase) -lt 0) {
        Add-Failure "P0AssetManifestCatalog missing token: $token"
    }
}

$skillHudPresenterPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Gameplay/P0SkillHudPresenter.cs"
$skillHudPresenterText = Get-Content -LiteralPath $skillHudPresenterPath -Raw
foreach ($token in @("StatusVisualAsset", "TargetReticleAsset", "HungerCostVisualAsset", "GetSkillHudStatusFeedback")) {
    if ($skillHudPresenterText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "P0SkillHudPresenter missing token: $token"
    }
}

$battleControllerPath = Resolve-ProjectPath "Assets/TheCat/Scripts/Runtime/Gameplay/GrayboxBattleController.cs"
$battleControllerText = Get-Content -LiteralPath $battleControllerPath -Raw
foreach ($token in @("card.StatusVisualAsset", "card.TargetReticleAsset", "GetInteractionRangeRipple")) {
    if ($battleControllerText.IndexOf($token, [System.StringComparison]::Ordinal) -lt 0) {
        Add-Failure "GrayboxBattleController missing token: $token"
    }
}

if ($failures.Count -gt 0) {
    foreach ($failure in $failures) {
        Write-Host "[FAIL] $failure"
    }

    throw "Skill HUD Batch 60 install validation failed with $($failures.Count) issue(s)."
}

Write-Host "Skill HUD Batch 60 install validation passed for $($rows.Count) asset(s)."
