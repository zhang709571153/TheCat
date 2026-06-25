Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$reviewDir = Join-Path $projectRoot "design/development/asset_review"
$csvPath = Join-Path $reviewDir "P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.csv"
$markdownPath = Join-Path $reviewDir "P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.md"
$packetDate = "2026-06-25"
$batch82Evidence = "design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/thecat_ui_common_states_batch82_manifest.csv;design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/thecat_ui_common_states_batch82_review_sheet_v001.png"

function Resolve-ProjectPath {
    param([string]$RelativePath)

    if ([string]::IsNullOrWhiteSpace($RelativePath) -or $RelativePath -eq "none") {
        return $null
    }

    $trimmed = $RelativePath.Trim()
    if ([System.IO.Path]::IsPathRooted($trimmed)) {
        throw "Evidence paths must be project-relative: $trimmed"
    }

    $normalized = $trimmed -replace "/", [System.IO.Path]::DirectorySeparatorChar
    $fullPath = [System.IO.Path]::GetFullPath((Join-Path $projectRoot $normalized))
    $rootFull = [System.IO.Path]::GetFullPath($projectRoot).TrimEnd(
        [System.IO.Path]::DirectorySeparatorChar,
        [System.IO.Path]::AltDirectorySeparatorChar
    )
    $rootPrefix = $rootFull + [System.IO.Path]::DirectorySeparatorChar

    if (-not ($fullPath.StartsWith($rootPrefix, [System.StringComparison]::OrdinalIgnoreCase) -or $fullPath -eq $rootFull)) {
        throw "Evidence path escapes project root: $trimmed"
    }

    return $fullPath
}

function Test-AllProjectPaths {
    param([string]$SemicolonPaths)

    if ([string]::IsNullOrWhiteSpace($SemicolonPaths) -or $SemicolonPaths -eq "none") {
        return $false
    }

    $paths = @($SemicolonPaths -split ";" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
    if ($paths.Count -eq 0) {
        return $false
    }

    foreach ($relativePath in ($SemicolonPaths -split ";" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })) {
        $path = Resolve-ProjectPath $relativePath
        if ($null -eq $path -or -not (Test-Path -LiteralPath $path)) {
            return $false
        }
    }

    return $true
}

function New-ComponentRow {
    param(
        [string]$ComponentId,
        [string]$Category,
        [string]$RequiredFor,
        [string]$InstalledPaths,
        [string]$CandidateEvidence,
        [string]$CurrentState,
        [string]$NextGate,
        [string]$Notes
    )

    $installedPresent = Test-AllProjectPaths $InstalledPaths
    $candidatePresent = Test-AllProjectPaths $CandidateEvidence
    return [pscustomobject]@{
        component_id = $ComponentId
        category = $Category
        required_for = $RequiredFor
        installed_paths = $InstalledPaths
        installed_present = if ($installedPresent) { "yes" } else { "no" }
        candidate_evidence = $CandidateEvidence
        candidate_present = if ($candidatePresent) { "yes" } else { "no" }
        current_state = $CurrentState
        next_gate = $NextGate
        notes = $Notes
    }
}

New-Item -ItemType Directory -Force -Path $reviewDir | Out-Null

$rows = New-Object System.Collections.Generic.List[object]
$rows.Add((New-ComponentRow "primary_button_default" "button" "entry screen, main menu, route/start confirmations" "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png" "none" "installed_pending_unity" "Runtime click-target and text-fit screenshots" "Only the primary/default frame is installed; no state atlas yet."))
$rows.Add((New-ComponentRow "button_state_atlas" "button" "hover, pressed, selected, disabled, danger/secondary states" "none" $batch82Evidence "candidate_complete_pending_unity_review" "Screen-priority review, then Unity click-target and text-fit screenshots" "Batch 82 derivative textless button states cover the prior missing row; not image2 provenance."))
$rows.Add((New-ComponentRow "dreamglass_panel" "panel" "menu, HUD, pause/settings panels, modal surfaces" "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png" "none" "installed_pending_unity" "Runtime panel scaling and nine-slice/padding proof" "Single base panel exists; runtime should prove scale/padding before more variants."))
$rows.Add((New-ComponentRow "modal_dialog_frame" "dialog" "confirmation, pause prompts, settlement details" "none" $batch82Evidence "candidate_complete_pending_unity_review" "Screen-priority review, then Unity modal scale/padding screenshots" "Batch 82 derivative textless modal frames cover the prior missing row; not image2 provenance."))
$rows.Add((New-ComponentRow "tabs_segmented_controls" "tab" "settings tabs, route/detail switching, collection filters" "none" $batch82Evidence "candidate_complete_pending_unity_review" "Screen-priority review, then Unity selected/unselected tab screenshots" "Batch 82 derivative textless tabs and segmented controls cover the prior missing row; not image2 provenance."))
$rows.Add((New-ComponentRow "route_reward_card_frames" "card_frame" "route choice cards and reward/event/boss cards" "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_partner_frame_512x256_v001.png;Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_shop_frame_512x256_v001.png;Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_blessing_frame_512x256_v001.png;Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_dreamevent_frame_512x256_v001.png;Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_restnest_frame_512x256_v001.png" "none" "installed_pending_unity" "Route/card screenshot readability and selected/current/path states" "Five route card frames are installed."))
$rows.Add((New-ComponentRow "choice_content_cards" "card" "shop, dream event, partner, rest, blessing cards" "Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_bed_patch_card_384x160_v001.png;Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_clear_notifications_card_384x160_v001.png;Assets/TheCat/Art/UI/Cards/thecat_ui_restnest_recovery_card_384x160_v001.png;Assets/TheCat/Art/UI/Cards/thecat_ui_partner_shadowmaru_preview_card_384x160_v001.png;Assets/TheCat/Art/UI/Cards/thecat_ui_blessing_oath_bedline_card_384x160_v001.png" "none" "installed_pending_unity" "Result/route/reward UI screenshots and text overlay proof" "Representative cards are installed for each current card family."))
$rows.Add((New-ComponentRow "list_row_frame" "list_row" "settings rows, reward summaries, collection/list screens" "none" $batch82Evidence "candidate_complete_pending_unity_review" "Screen-priority review, then Unity list density and text-fit screenshots" "Batch 82 derivative textless list rows cover the prior missing row; not image2 provenance."))
$rows.Add((New-ComponentRow "system_icon_set" "icon" "settings, sound, mute, back, close, pause, continue, retry, lock, warning" "none" "design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/system_icons_batch79_manifest.csv;design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_review_sheet_v001.png" "candidate_complete_pending_unity_review" "64px/32px semantics and import decision" "Candidate-only; not installed under Assets."))
$rows.Add((New-ComponentRow "settings_control_set" "settings_control" "sliders, switches, checkboxes, audio/display options" "none" "design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/settings_controls_batch78_manifest.csv;design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/thecat_ui_settings_controls_batch78_review_sheet_v001.png" "candidate_complete_pending_unity_review" "Settings screen screenshot and drag/click behavior" "Candidate-only controls exist."))
$rows.Add((New-ComponentRow "runtime_control_panels" "runtime_control" "pause, speed, restart, keyboard hint panels" "none" "design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/runtime_control_panels_batch63_manifest.csv;design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15/thecat_ui_runtime_control_panels_batch63_review_sheet.png" "candidate_complete_pending_unity_review" "Pause/speed/restart HUD screenshot and click-target checks" "Candidate-only; keep out of Assets until Unity evidence."))
$rows.Add((New-ComponentRow "lock_warning_controls" "icon" "locked route/cards, warning dialogs, unavailable skill states" "none" "design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/system_icons_batch79_manifest.csv" "candidate_complete_pending_unity_review" "32px/64px readability, color semantics, and screen contrast proof" "Covered inside Batch 79 lock/warning system icons."))
$rows.Add((New-ComponentRow "skill_slot_state_frames" "skill_slot" "ready, cooldown, disabled, selected skill states" "none" "design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/thecat_ui_skill_slot_frames_batch81_manifest_v002_light.csv;design/development/asset_candidates/ui/skill_slot_frames/batch_81_skill_slot_frame_candidates_2026-06-25/frames/thecat_ui_skill_slot_frames_batch81_contact_sheet_v002_light.png" "candidate_complete_pending_unity_review" "Battle HUD import-test screenshots, selected-vs-ready, cooldown 99 proof" "Candidate-only; v002_light is the preferred import-test candidate."))
$rows.Add((New-ComponentRow "skill_hud_feedback_overlays" "skill_hud" "ready, cooldown, no target, hunger cost, auto target, range" "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_ready_frame_512_v001.png;Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png;Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png;Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png;Assets/TheCat/Art/UI/Icons/thecat_ui_auto_target_reticle_512_v001.png;Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png" "none" "installed_pending_unity_validation" "Battle HUD timing/readability screenshots and binding proof" "Installed symbolic skill HUD feedback exists."))
$rows.Add((New-ComponentRow "core_gauge_bars" "gauge" "sleep, HP, poop, hunger HUD gauges" "Assets/TheCat/Art/UI/Frames/thecat_ui_core_sleep_gauge_frame_384x48_v001.png;Assets/TheCat/Art/UI/Frames/thecat_ui_core_hp_gauge_frame_384x48_v001.png;Assets/TheCat/Art/UI/Frames/thecat_ui_core_poop_gauge_frame_384x48_v001.png;Assets/TheCat/Art/UI/Frames/thecat_ui_core_hunger_gauge_frame_384x48_v001.png" "none" "installed_pending_unity_validation" "Battle HUD screenshot and number/text overlay proof" "Gauge frames/fills and icons are installed."))
$rows.Add((New-ComponentRow "result_settlement_banners" "banner" "victory, defeat, run clear, run fail" "Assets/TheCat/Art/UI/Banners/thecat_ui_battle_result_victory_banner_512x160_v001.png;Assets/TheCat/Art/UI/Banners/thecat_ui_battle_result_defeat_banner_512x160_v001.png;Assets/TheCat/Art/UI/Banners/thecat_ui_settlement_run_cleared_banner_512x160_v001.png;Assets/TheCat/Art/UI/Banners/thecat_ui_settlement_run_failed_banner_512x160_v001.png" "none" "installed_pending_unity_validation" "Victory/defeat/settlement screenshots" "Installed; still needs screen-level composition proof."))
$rows.Add((New-ComponentRow "reward_detail_badges" "badge" "gain, cost, recovery, risk, upgrade labels" "Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_gain_badge_192x64_v001.png;Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_cost_badge_192x64_v001.png;Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_recovery_badge_192x64_v001.png;Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_risk_badge_192x64_v001.png;Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_upgrade_badge_192x64_v001.png" "none" "installed_pending_unity_validation" "Reward-detail UI screenshot and import scan" "Installed; metas were fixed to Sprite/alpha in this pass."))

$rows | Export-Csv -LiteralPath $csvPath -NoTypeInformation -Encoding UTF8

$installed = @($rows | Where-Object { $_.current_state -like "installed*" }).Count
$candidate = @($rows | Where-Object { $_.current_state -like "*candidate*" }).Count
$missing = @($rows | Where-Object { $_.current_state -eq "missing_design_needed" }).Count

$markdown = New-Object System.Collections.Generic.List[string]
$markdown.Add("# P0 UI Common Component Inventory")
$markdown.Add("")
$markdown.Add("Packet date: $packetDate")
$markdown.Add("")
$markdown.Add("Authority: `Qr1XdXd6KosnjMxjgW7cS89kn9c` live revision `816`, especially section 9 P0 minimum art assets, plus current local asset manifests.")
$markdown.Add("")
$markdown.Add("This packet inventories common UI component coverage before opening another UI image-generation batch. It deliberately separates installed assets, candidate-only packets, and screen-level runtime gates.")
$markdown.Add("")
$markdown.Add("- Rows: $($rows.Count)")
$markdown.Add("- Installed pending Unity evidence: $installed")
$markdown.Add("- Candidate-only or import-test rows: $candidate")
$markdown.Add("- Missing design-needed rows: $missing")
$markdown.Add("")
$markdown.Add("| Component | Category | Current state | Installed | Candidate | Next gate | Notes |")
$markdown.Add("| --- | --- | --- | --- | --- | --- | --- |")
foreach ($row in $rows) {
    $safeNotes = $row.notes.Replace("|", "\|")
    $safeGate = $row.next_gate.Replace("|", "\|")
    $markdown.Add("| $($row.component_id) | $($row.category) | $($row.current_state) | $($row.installed_present) | $($row.candidate_present) | $safeGate | $safeNotes |")
}
$markdown.Add("")
$markdown.Add("## Production Policy")
$markdown.Add("")
$markdown.Add("- Do not promote or expand common UI component art until Batch 82 is reviewed against real screens.")
$markdown.Add("- Batch 82 derivative candidates now cover the prior missing button state, modal/dialog, tab/segmented-control, and list-row rows.")
$markdown.Add("- No missing design-needed rows remain in the common-component inventory, but screen-level entry/loading, cat-room prompt, and skill-selection detail coverage remains separate.")
$markdown.Add('- Candidate-only system/settings/runtime/skill-slot rows must stay outside `Assets/` until Unity screenshots, click-target proof, import settings, and Console checks pass.')
$markdown.Add("- Avoid baked Chinese text in generated UI sprites; keep text rendered by Unity.")
$markdown.Add("- No starter-cat body, starter-cat frame, or character replacement asset is required or allowed by this packet.")
$markdown.Add("")
$markdown.Add("## Validator")
$markdown.Add("")
$markdown.Add('`design/development/tools/validate_ui_common_component_inventory.ps1`')

$markdown | Set-Content -LiteralPath $markdownPath -Encoding UTF8

Write-Output "P0 UI common component inventory generated. Rows: $($rows.Count) Installed: $installed Candidate: $candidate Missing: $missing"
Write-Output "design/development/asset_review/P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.csv"
Write-Output "design/development/asset_review/P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.md"
