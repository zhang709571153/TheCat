Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$batchSlug = "batch_90_cat_room_preflight_2026-06-25"
$batchDirRelative = "design/development/asset_candidates/ui/cat_room/$batchSlug"
$batchDir = Join-Path $projectRoot ($batchDirRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$manifestRelative = "$batchDirRelative/thecat_ui_cat_room_batch90_manifest.csv"
$manifestPath = Join-Path $projectRoot ($manifestRelative -replace "/", [System.IO.Path]::DirectorySeparatorChar)
$contactSheetRelative = "$batchDirRelative/thecat_ui_cat_room_batch90_contact_sheet_v001.png"
$reviewSheetRelative = "$batchDirRelative/thecat_ui_cat_room_batch90_review_sheet_v001.png"
$reviewNoteRelative = "$batchDirRelative/thecat_ui_cat_room_batch90_candidate_review.md"
$processNoteRelative = "$batchDirRelative/thecat_ui_cat_room_batch90_process_note.md"
$agentPromptRelative = "$batchDirRelative/thecat_ui_cat_room_batch90_agent_review_prompt.md"

$expectedVariants = [ordered]@{
    cat_room_stage_panel_frame = "1180x640"
    cat_room_status_rail_frame = "960x120"
    cat_room_interaction_card_slot = "440x180"
    cat_room_prop_hotspot_frame = "256x256"
    cat_room_dream_entrance_button_frame = "420x112"
    cat_room_hover_disabled_prompt_chip = "360x96"
    cat_room_1920x1080 = "1920x1080"
    cat_room_bed_hover_1365x768 = "1365x768"
    cat_room_interaction_disabled_1280x720 = "1280x720"
    cat_room_compact_1024x768 = "1024x768"
}

$expectedTypes = @{
    cat_room_stage_panel_frame = "sprite"
    cat_room_status_rail_frame = "sprite"
    cat_room_interaction_card_slot = "sprite"
    cat_room_prop_hotspot_frame = "sprite"
    cat_room_dream_entrance_button_frame = "sprite"
    cat_room_hover_disabled_prompt_chip = "sprite"
    cat_room_1920x1080 = "local_mockup"
    cat_room_bed_hover_1365x768 = "local_mockup"
    cat_room_interaction_disabled_1280x720 = "local_mockup"
    cat_room_compact_1024x768 = "local_mockup"
}

$sourcePaths = @(
    "Assets/TheCat/Art/Scenes/BedroomDream/thecat_bg_bedroomdream_battle_1920x1080_v001.png",
    "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_bed_sleepglow_sprite_512_v001.png",
    "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_feeder_sprite_256_v001.png",
    "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_litterbox_sprite_256_v001.png",
    "Assets/TheCat/Art/UI/Backgrounds/thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png",
    "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png",
    "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png",
    "Assets/TheCat/Art/UI/Frames/thecat_ui_title_logo_512x256_v001.png",
    "Assets/TheCat/Art/UI/Icons/thecat_ui_core_sleep_icon_64_v001.png",
    "Assets/TheCat/Art/UI/Icons/thecat_ui_core_hunger_icon_64_v001.png",
    "Assets/TheCat/Art/UI/Icons/thecat_ui_core_poop_icon_64_v001.png",
    "Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png",
    "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_bed_ready_ring_256_candidate_v001.png",
    "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_bed_restore_pulse_256_candidate_v001.png",
    "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_feeder_ready_marker_256_candidate_v001.png",
    "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_litter_urgent_marker_256_candidate_v001.png",
    "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15/thecat_ui_interaction_blocked_marker_256_candidate_v001.png",
    "design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/sprites/thecat_ui_common_list_row_selected_640x80_candidate_v001.png",
    "design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/sprites/thecat_ui_common_list_row_disabled_640x80_candidate_v001.png"
)

$failures = New-Object System.Collections.Generic.List[string]

function Add-Failure {
    param([string]$Message)
    $failures.Add($Message)
}

function Resolve-ProjectPath {
    param([string]$RelativePath)
    if ([string]::IsNullOrWhiteSpace($RelativePath)) { return $null }
    if ([System.IO.Path]::IsPathRooted($RelativePath)) {
        Add-Failure "Path must be project-relative: $RelativePath"
        return $null
    }

    $full = [System.IO.Path]::GetFullPath((Join-Path $projectRoot ($RelativePath -replace "/", [System.IO.Path]::DirectorySeparatorChar)))
    $rootFull = [System.IO.Path]::GetFullPath($projectRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar, [System.IO.Path]::AltDirectorySeparatorChar)
    $rootPrefix = $rootFull + [System.IO.Path]::DirectorySeparatorChar
    if (-not ($full.StartsWith($rootPrefix, [System.StringComparison]::OrdinalIgnoreCase) -or $full -eq $rootFull)) {
        Add-Failure "Path escapes project root: $RelativePath"
        return $null
    }

    return $full
}

function Get-ImageSize {
    param([string]$Path)
    $image = [System.Drawing.Image]::FromFile($Path)
    try { return "$($image.Width)x$($image.Height)" } finally { $image.Dispose() }
}

function Test-Hash {
    param([string]$Path, [string]$ExpectedHash, [string]$Label)
    if (-not (Test-Path -LiteralPath $Path)) {
        Add-Failure "$Label missing: $Path"
        return
    }

    $actual = (Get-FileHash -Algorithm SHA256 -LiteralPath $Path).Hash.ToLowerInvariant()
    if ($actual -ne $ExpectedHash.ToLowerInvariant()) {
        Add-Failure "$Label hash mismatch. Expected $ExpectedHash but found $actual"
    }
}

function Test-TransparentCorners {
    param([string]$Path)
    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $points = @(@(0, 0), @(($bitmap.Width - 1), 0), @(0, ($bitmap.Height - 1)), @(($bitmap.Width - 1), ($bitmap.Height - 1)))
        foreach ($point in $points) {
            $pixel = $bitmap.GetPixel($point[0], $point[1])
            if ($pixel.A -gt 24) {
                Add-Failure "$Path should have transparent sprite corners but found alpha $($pixel.A) at $($point[0]),$($point[1])"
            }
        }
    } finally {
        $bitmap.Dispose()
    }
}

function Test-VisibleAlpha {
    param([string]$Path)
    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $visible = 0
        $semiTransparent = 0
        for ($y = 0; $y -lt $bitmap.Height; $y += [Math]::Max(1, [Math]::Floor($bitmap.Height / 32))) {
            for ($x = 0; $x -lt $bitmap.Width; $x += [Math]::Max(1, [Math]::Floor($bitmap.Width / 64))) {
                $alpha = $bitmap.GetPixel($x, $y).A
                if ($alpha -gt 24) { $visible++ }
                if ($alpha -gt 24 -and $alpha -lt 250) { $semiTransparent++ }
            }
        }

        if ($visible -lt 3) { Add-Failure "$Path has too little visible alpha coverage" }
        if ($semiTransparent -lt 1) { Add-Failure "$Path should preserve antialiased or translucent alpha" }
    } finally {
        $bitmap.Dispose()
    }
}

function Test-OpaqueMockup {
    param([string]$Path)
    $bitmap = [System.Drawing.Bitmap]::new($Path)
    try {
        $opaque = 0
        $sampled = 0
        for ($y = 0; $y -lt $bitmap.Height; $y += [Math]::Max(1, [Math]::Floor($bitmap.Height / 36))) {
            for ($x = 0; $x -lt $bitmap.Width; $x += [Math]::Max(1, [Math]::Floor($bitmap.Width / 64))) {
                $sampled++
                if ($bitmap.GetPixel($x, $y).A -gt 245) { $opaque++ }
            }
        }

        if ($sampled -eq 0 -or ($opaque / $sampled) -lt 0.96) {
            Add-Failure "$Path should be an opaque presentation mockup; opaque sample ratio $opaque/$sampled"
        }
    } finally {
        $bitmap.Dispose()
    }
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Batch 90 manifest is missing: $manifestRelative"
}

$rows = @(Import-Csv -LiteralPath $manifestPath)
if ($rows.Count -ne 10) {
    Add-Failure "Expected 10 Batch 90 rows but found $($rows.Count)"
}

$requiredColumns = @(
    "asset_id", "component_id", "variant_id", "target_size", "batch_slug", "asset_type", "candidate_path",
    "candidate_sha256", "candidate_size", "source_assets", "source_sha256", "contact_sheet", "contact_sheet_sha256",
    "review_sheet", "review_sheet_sha256", "review_note", "review_note_sha256", "process_note", "process_note_sha256",
    "agent_prompt", "agent_prompt_sha256", "source_model", "recommendation", "visual_review"
)

if ($rows.Count -gt 0) {
    $actualColumns = @($rows[0].PSObject.Properties.Name)
    foreach ($column in $requiredColumns) {
        if ($column -notin $actualColumns) {
            Add-Failure "Manifest missing required evidence column: $column"
        }
    }
}

$sharedPaths = @($contactSheetRelative, $reviewSheetRelative, $reviewNoteRelative, $processNoteRelative, $agentPromptRelative)
foreach ($variant in $expectedVariants.Keys) {
    $match = @($rows | Where-Object { $_.variant_id -eq $variant })
    if ($match.Count -ne 1) {
        Add-Failure "Expected exactly one row for variant $variant but found $($match.Count)"
        continue
    }

    $row = $match[0]
    if ($row.target_size -ne $expectedVariants[$variant]) { Add-Failure "$variant target_size should be $($expectedVariants[$variant]) but found $($row.target_size)" }
    if ($row.asset_type -ne $expectedTypes[$variant]) { Add-Failure "$variant asset_type should be $($expectedTypes[$variant]) but found $($row.asset_type)" }
    if ($row.batch_slug -ne $batchSlug) { Add-Failure "$variant batch_slug mismatch: $($row.batch_slug)" }
    if ($row.source_model -ne "deterministic_local_derivative_from_existing_bedroom_map_and_interaction_assets_not_image2") { Add-Failure "$variant must not claim image2 provenance. source_model=$($row.source_model)" }
    if ($row.recommendation -ne "candidate_only_pending_unity_cat_room_screenshots") { Add-Failure "$variant recommendation mismatch: $($row.recommendation)" }
    if ($row.candidate_path -notlike "$batchDirRelative/*") { Add-Failure "$variant candidate_path must stay in Batch 90 directory: $($row.candidate_path)" }
    if ($row.candidate_path -like "Assets/*") { Add-Failure "$variant candidate_path must not be under Assets: $($row.candidate_path)" }

    $candidatePath = Resolve-ProjectPath $row.candidate_path
    if ($null -eq $candidatePath) { continue }
    if (-not (Test-Path -LiteralPath $candidatePath)) {
        Add-Failure "$variant candidate file missing: $($row.candidate_path)"
        continue
    }

    Test-Hash $candidatePath $row.candidate_sha256 "$variant candidate"
    $actualSize = Get-ImageSize $candidatePath
    if ($actualSize -ne $row.candidate_size) { Add-Failure "$variant candidate_size mismatch. Manifest $($row.candidate_size), actual $actualSize" }
    if ($actualSize -ne $expectedVariants[$variant]) { Add-Failure "$variant actual image size should be $($expectedVariants[$variant]) but found $actualSize" }
    if ($row.asset_type -eq "sprite") {
        Test-TransparentCorners $candidatePath
        Test-VisibleAlpha $candidatePath
    } elseif ($row.asset_type -eq "local_mockup") {
        Test-OpaqueMockup $candidatePath
    }

    if ($row.contact_sheet -ne $contactSheetRelative) { Add-Failure "$variant contact_sheet should be $contactSheetRelative but found $($row.contact_sheet)" }
    if ($row.review_sheet -ne $reviewSheetRelative) { Add-Failure "$variant review_sheet should be $reviewSheetRelative but found $($row.review_sheet)" }
    if ($row.review_note -ne $reviewNoteRelative) { Add-Failure "$variant review_note should be $reviewNoteRelative but found $($row.review_note)" }
    if ($row.process_note -ne $processNoteRelative) { Add-Failure "$variant process_note should be $processNoteRelative but found $($row.process_note)" }
    if ($row.agent_prompt -ne $agentPromptRelative) { Add-Failure "$variant agent_prompt should be $agentPromptRelative but found $($row.agent_prompt)" }

    foreach ($sharedPath in $sharedPaths) {
        $full = Resolve-ProjectPath $sharedPath
        if ($null -ne $full -and (Test-Path -LiteralPath $full)) {
            $hashColumn = switch ($sharedPath) {
                $contactSheetRelative { "contact_sheet_sha256" }
                $reviewSheetRelative { "review_sheet_sha256" }
                $reviewNoteRelative { "review_note_sha256" }
                $processNoteRelative { "process_note_sha256" }
                $agentPromptRelative { "agent_prompt_sha256" }
            }
            Test-Hash $full $row.$hashColumn "$variant shared evidence $sharedPath"
        }
    }
}

$manifestPaths = @($rows | ForEach-Object { $_.candidate_path })
$actualPngs = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File -Filter *.png | ForEach-Object {
        $_.FullName.Substring($projectRoot.Length + 1).Replace([System.IO.Path]::DirectorySeparatorChar, "/")
    })
foreach ($png in $actualPngs) {
    if ($png -in @($contactSheetRelative, $reviewSheetRelative)) { continue }
    if ($png -notin $manifestPaths) { Add-Failure "Found orphan Batch 90 PNG not listed in manifest: $png" }
}

foreach ($sharedPath in $sharedPaths) {
    $full = Resolve-ProjectPath $sharedPath
    if ($null -ne $full -and -not (Test-Path -LiteralPath $full)) {
        Add-Failure "Missing shared Batch 90 evidence: $sharedPath"
    }
}

$expectedSourceAssetString = ($sourcePaths -join ";")
$expectedSourceHashString = (($sourcePaths | ForEach-Object {
            $full = Resolve-ProjectPath $_
            if ($null -eq $full -or -not (Test-Path -LiteralPath $full)) {
                Add-Failure "Missing source asset: $_"
                ""
            } else {
                (Get-FileHash -Algorithm SHA256 -LiteralPath $full).Hash.ToLowerInvariant()
            }
        }) -join ";")

foreach ($row in $rows) {
    if ($row.source_assets -ne $expectedSourceAssetString) { Add-Failure "$($row.variant_id) source_assets mismatch" }
    if ($row.source_sha256 -ne $expectedSourceHashString) { Add-Failure "$($row.variant_id) source_sha256 mismatch" }
}

$processPath = Resolve-ProjectPath $processNoteRelative
if ($null -ne $processPath -and (Test-Path -LiteralPath $processPath)) {
    $processText = Get-Content -LiteralPath $processPath -Raw
    foreach ($token in @("ui_cat_room", "Qr1", "IAd", "Feishu", "ACL-blocked", "OPENAI_API_KEY", "model selector", "BedroomDream", "Batch 67", "Batch 82", "bed", "feeder", "litter", "dream entrance", "hover", "disabled", "range", "no baked Chinese text", "not image2 provenance", "Candidate-only", "no new cat body art", "Unity-rendered cat-room screenshots")) {
        if ($processText -notlike "*$token*") { Add-Failure "Process note missing control token: $token" }
    }
}

$reviewPath = Resolve-ProjectPath $reviewNoteRelative
if ($null -ne $reviewPath -and (Test-Path -LiteralPath $reviewPath)) {
    $reviewText = Get-Content -LiteralPath $reviewPath -Raw
    foreach ($token in @("cat room", "bed", "feeder", "litter", "dream entrance", "hover", "disabled", "range", "1024x768", "Click-target proof")) {
        if ($reviewText -notlike "*$token*") { Add-Failure "Review note missing expected Batch 90 token: $token" }
    }
}

$metaFiles = @(Get-ChildItem -LiteralPath $batchDir -Recurse -File -Filter *.meta)
if ($metaFiles.Count -gt 0) {
    Add-Failure "Batch 90 candidate directory must not contain Unity .meta files"
}

if ($failures.Count -gt 0) {
    Write-Error ("Batch 90 cat-room preflight validation failed:`n" + ($failures -join "`n"))
    exit 1
}

Write-Output "Batch 90 cat-room preflight validation passed. Rows: $($rows.Count)"
Write-Output $manifestRelative
