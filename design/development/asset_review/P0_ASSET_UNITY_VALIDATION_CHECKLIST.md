# P0 Asset Unity Validation Checklist

P0 asset Unity validation checklist is ready for 19 queue item(s).

## Counts

- Queue items: 19
- Candidate review items: 14
- Unity-blocked items: 5
- Active screenshot validation items: 2
- Installed asset validation items: 2
- Formal install decision items: 1
- Candidate no-meta policy items: 14
- Console-gated items: 19

## Validation Order

1. Refresh the Unity AssetDatabase.
2. Inspect Sprite import settings for any already-installed assets.
3. Confirm candidate-only folders have no Unity `.meta` files.
4. Capture active-cat, active-enemy, cat-room, character-select, battle HUD, skill-selection, skill VFX, runtime-control, secondary-warning, loading/start, result/settlement, settings/pause, dream-route/route-map, and bedroom interaction screenshots.
5. Compare starter-cat screenshots against locked colored three-view turnarounds.
6. Check scene and prefab references for every approved install target.
7. Check Console logs after each validation pass.
8. Write formal install decisions only for rows that pass the matching Unity evidence gate.

## Queue Items

### Starter Cat Active Screenshot Validation

- Queue id: `p0_asset_queue_starter_cat_active_validation`
- Phase: `UnityValidation`
- State: `BlockedByUnityValidation`
- Candidate directory: `design/development/asset_candidates/starter_cats`
- Unity import root: `Assets/TheCat/Art/Characters/Sprites`
- Required evidence: `05-active-cat-saiban.png`; `06-active-cat-nephthys.png`; `07-active-cat-suzune.png`; `Unity Console has no new errors`; `active-cat screenshots match colored three-view turnarounds`; `candidate review notes explicitly approve or keep blocked`
- Next action: Capture active-cat screenshots and compare Batch 49/50/51 against locked colored turnarounds before any starter-cat install.

### Core Enemy Active Screenshot Validation

- Queue id: `p0_asset_queue_core_enemy_active_validation`
- Phase: `UnityValidation`
- State: `BlockedByUnityValidation`
- Candidate directory: `design/development/asset_candidates/enemies`
- Unity import root: `Assets/TheCat/Art/Enemies/Sprites`
- Required evidence: `active enemy screenshot set`; `Unity Console has no new errors`; `bed-pressure scale and warning readability pass`; `enemy review notes explicitly approve or keep blocked`
- Next action: Validate the current enemy cutout candidates in Unity before deciding whether to install them.

### Bedroom Interactable Candidate Pack

- Queue id: `p0_asset_queue_bedroom_interactable_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15`
- Unity import root: `Assets/TheCat/Art/Scenes/BedroomDream`
- Required evidence: `bedroom_interactables_batch54_manifest.csv`; `thecat_props_bedroom_interactables_batch54_review_sheet.png`; `bedroom_interactables_batch54_candidate_review.md`; `validate_bedroom_interactable_candidates.ps1 passes`; `bed, litter box, and feeder candidates stay non-cat and outside Assets`; `transparent PNGs include manifest, review sheet, review note, process note, and agent prompt`; `no Unity .meta files are created in candidate folders`
- Next action: Batch 54 candidate pack is complete; keep it review-only until Unity runtime scale, Console, and scene/prefab checks approve a formal install.

### Starter Skill VFX Installed Unity Validation

- Queue id: `p0_asset_queue_starter_skill_vfx_candidates`
- Phase: `UnityValidation`
- State: `BlockedByUnityValidation`
- Candidate directory: `design/development/asset_candidates/vfx/starter_skills/batch_61_starter_skill_vfx_install_2026-06-15`
- Unity import root: `Assets/TheCat/Art/VFX`
- Required evidence: `starter_skill_vfx_batch61_install_manifest.csv`; `thecat_vfx_starter_skills_batch61_install_review_sheet.png`; `starter_skill_vfx_batch61_install_review.md`; `validate_starter_skill_vfx_install.ps1 passes`; `installed symbolic VFX avoids cat-body redraws and starter-cat body replacements`; `installed PNGs include Unity .png.meta files with TheCatP0ImportSettings:v1`; `Unity skill timing, feedback scale, screenshot, and Console validation remain pending`
- Next action: Batch 61 is installed from Batch 55 candidates; validate Unity skill timing, feedback scale, Console, screenshot, and scene/prefab references before marking complete.

### Skill HUD Feedback Installed Unity Validation

- Queue id: `p0_asset_queue_skill_hud_feedback_candidates`
- Phase: `UnityValidation`
- State: `BlockedByUnityValidation`
- Candidate directory: `design/development/asset_candidates/ui/skill_hud/batch_60_skill_hud_feedback_install_2026-06-15`
- Unity import root: `Assets/TheCat/Art/UI`
- Required evidence: `skill_hud_feedback_batch60_install_manifest.csv`; `thecat_ui_skill_hud_feedback_batch60_install_review_sheet.png`; `skill_hud_feedback_batch60_install_review.md`; `validate_skill_hud_feedback_install.ps1 passes`; `installed assets are non-cat UI/VFX and avoid starter-cat body redraws and colored-turnaround crops`; `installed PNGs include Unity .png.meta files with TheCatP0ImportSettings:v1`; `Unity HUD screenshot and Console validation remain pending`
- Next action: Batch 60 is installed from Batch 57 candidates; validate Unity HUD scale, timing, Console, screenshot, and scene/prefab references before marking complete.

### Cat Room Preflight Candidate Pack

- Queue id: `p0_asset_queue_cat_room_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/CatRoom`
- Required evidence: `thecat_ui_cat_room_batch90_manifest.csv`; `thecat_ui_cat_room_batch90_review_sheet_v001.png`; `thecat_ui_cat_room_batch90_candidate_review.md`; `validate_ui_cat_room_preflight_candidates.ps1 passes`; `6 transparent cat-room sprites and 4 cat-room mockups stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; current Play Mode `02-cat-room.png` verifies the existing runtime cat-room surface only; `Batch90 candidate-backed screenshots, bed/feeder/litter/dream entrance interaction proof, hover/disabled/range state readability, click targets, import settings, binding, and Console validation remain pending`
- Next action: Batch 90 cat-room preflight is complete; keep it review-only until Batch90 candidate-backed Unity-rendered cat-room screenshots, bed/feeder/litter/dream entrance interactions, hover/disabled/range states, click targets, import settings, binding, and Console checks approve a formal install. Do not treat `02-cat-room.png` as candidate import acceptance.

### Character Select Preflight Candidate Pack

- Queue id: `p0_asset_queue_character_select_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/CharacterSelect`
- Required evidence: `thecat_ui_character_select_batch88_manifest.csv`; `thecat_ui_character_select_batch88_review_sheet_v001.png`; `thecat_ui_character_select_batch88_candidate_review.md`; `validate_ui_character_select_preflight_candidates.ps1 passes`; `6 transparent character-select sprites and 4 character-select mockups stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity character-select screenshots, source-lock avatar consistency, text replacement, selected/idle state readability, click targets, import settings, binding, and Console validation remain pending`
- Next action: Batch 88 character-select preflight is complete; keep it review-only until Unity-rendered character-select screenshots, source-lock avatar consistency, text and number replacement, selected/idle states, click targets, import settings, binding, and Console checks approve a formal install.

### Battle HUD Preflight Candidate Pack

- Queue id: `p0_asset_queue_battle_hud_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/BattleHUD`
- Required evidence: `thecat_ui_battle_hud_batch87_manifest.csv`; `thecat_ui_battle_hud_batch87_review_sheet_v001.png`; `thecat_ui_battle_hud_batch87_candidate_review.md`; `validate_ui_battle_hud_preflight_candidates.ps1 passes`; `6 transparent battle HUD sprites and 4 battle HUD mockups stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity battle HUD screenshots, gauge/text replacement, skill state readability, 1024x768 four-gauge proof, enemy/telegraph occlusion, import settings, click targets, binding, and Console validation remain pending`
- Next action: Batch 87 battle HUD preflight is complete; keep it review-only until Unity-rendered battle HUD screenshots, four-gauge proof, text and number replacement, skill states, click targets, import settings, binding, and Console checks approve a formal install.

### Skill Selection Preflight Candidate Pack

- Queue id: `p0_asset_queue_skill_selection_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/SkillSelection`
- Required evidence: `thecat_ui_skill_selection_batch89_manifest.csv`; `thecat_ui_skill_selection_batch89_review_sheet_v001.png`; `thecat_ui_skill_selection_batch89_candidate_review.md`; `validate_ui_skill_selection_preflight_candidates.ps1 passes`; `8 transparent skill-selection sprites and 4 skill-selection mockups stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity skill-selection screenshots, selected/ready/disabled/locked states, cooldown/low-resource/no-target semantics, text replacement, click targets, import settings, binding, and Console validation remain pending`
- Next action: Batch 89 skill-selection preflight is complete; keep it review-only until Unity-rendered skill-selection screenshots, selected/ready/disabled/locked states, cooldown and low-resource semantics, text and number replacement, click targets, import settings, binding, and Console checks approve a formal install.

### Runtime Control Icon Candidate Pack

- Queue id: `p0_asset_queue_runtime_control_icon_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15`
- Unity import root: `Assets/TheCat/Art/UI/Icons`
- Required evidence: `runtime_control_icons_batch62_manifest.csv`; `thecat_ui_runtime_controls_batch62_review_sheet.png`; `runtime_control_icons_batch62_candidate_review.md`; `validate_runtime_control_icon_candidates.ps1 passes`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity battle HUD readability and Console validation remain pending`
- Next action: Batch 62 candidate pack is complete; keep it review-only until Unity HUD scale, Console, screenshot, and pause/speed/restart affordance checks approve a formal install.

### Runtime Control Panel Candidate Pack

- Queue id: `p0_asset_queue_runtime_control_panel_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15`
- Unity import root: `Assets/TheCat/Art/UI`
- Required evidence: `runtime_control_panels_batch63_manifest.csv`; `thecat_ui_runtime_control_panels_batch63_review_sheet.png`; `runtime_control_panels_batch63_candidate_review.md`; `validate_runtime_control_panel_candidates.ps1 passes`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity pause overlay, speed selector, restart plate, keyboard hint, and Console validation remain pending`
- Next action: Batch 63 candidate pack is complete; keep it review-only until Unity HUD scale, Console, screenshot, and pause/speed/restart panel checks approve a formal install.

### Secondary Enemy Warning Candidate Pack

- Queue id: `p0_asset_queue_secondary_enemy_warning_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/enemies/secondary_warning_vfx/batch_64_secondary_enemy_warning_candidates_2026-06-15`
- Unity import root: `Assets/TheCat/Art/Enemies/VFX`
- Required evidence: `secondary_enemy_warning_batch64_manifest.csv`; `thecat_vfx_secondary_enemy_warnings_batch64_review_sheet.png`; `secondary_enemy_warning_batch64_candidate_review.md`; `validate_secondary_enemy_warning_candidates.ps1 passes`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity enemy warning readability, Console, screenshot, and future prefab binding validation remain pending`
- Next action: Batch 64 candidate pack is complete; keep it review-only until Unity warning readability, Console, screenshot, and future secondary-enemy prefab checks approve a formal install.

### Route Map Readability Candidate Pack

- Queue id: `p0_asset_queue_route_map_readability_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15`
- Unity import root: `Assets/TheCat/Art/UI`
- Required evidence: `route_map_readability_batch65_manifest.csv`; `thecat_ui_route_map_readability_batch65_review_sheet.png`; `route_map_readability_batch65_candidate_review.md`; `validate_route_map_readability_candidates.ps1 passes`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity route-map scale, selected/current contrast, path readability, Boss pressure readability, and Console validation remain pending`
- Next action: Batch 65 candidate pack is complete; keep it review-only until Unity route-map readability, Console, screenshot, and scene/prefab checks approve a formal install.

### Bedroom Interaction Affordance Candidate Pack

- Queue id: `p0_asset_queue_bedroom_interaction_affordance_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15`
- Unity import root: `Assets/TheCat/Art/UI`
- Required evidence: `bedroom_interaction_affordance_batch67_manifest.csv`; `thecat_ui_bedroom_interaction_affordance_batch67_review_sheet.png`; `bedroom_interaction_affordance_batch67_candidate_review.md`; `validate_bedroom_interaction_affordance_candidates.ps1 passes`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity bed, litter box, feeder, blocked interaction, range readability, input timing, scene/prefab, and Console validation remain pending`
- Next action: Batch 67 candidate pack is complete; keep it review-only until Unity interaction timing, Console, screenshot, and scene/prefab checks approve a formal install.

### Loading Start Preflight Candidate Pack

- Queue id: `p0_asset_queue_loading_start_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/LoadingStart`
- Required evidence: `thecat_ui_loading_start_batch83_manifest.csv`; `thecat_ui_loading_start_batch83_review_sheet_v001.png`; `thecat_ui_loading_start_batch83_candidate_review.md`; `validate_ui_loading_start_preflight_candidates.ps1 passes`; `4 transparent loading/start sprites and 4 resolution mockups stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity loading/start screenshots, spinner interpretation, placeholder state replacement, 1024x768 crowding, import settings, binding, and Console validation remain pending`
- Next action: Batch 83 loading/start preflight is complete; keep it review-only until Unity-rendered loading/start screenshots, import settings, binding, placeholder-state replacement, and Console checks approve a formal install.

### Result Settlement Preflight Candidate Pack

- Queue id: `p0_asset_queue_result_settlement_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/ResultSettlement`
- Required evidence: `thecat_ui_result_settlement_batch84_manifest.csv`; `thecat_ui_result_settlement_batch84_review_sheet_v001.png`; `thecat_ui_result_settlement_batch84_candidate_review.md`; `validate_ui_result_settlement_preflight_candidates.ps1 passes`; `7 transparent result/settlement sprites and 4 result/settlement mockups stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity victory/defeat/settlement screenshots, text replacement, 1024x768 crowding, import settings, binding, and Console validation remain pending`
- Next action: Batch 84 result/settlement preflight is complete; keep it review-only until Unity-rendered victory, defeat, run-cleared, and run-failed screenshots, text replacement, 1024x768 crowding, import settings, binding, and Console checks approve a formal install.

### Settings Pause Preflight Candidate Pack

- Queue id: `p0_asset_queue_settings_pause_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/SettingsPause`
- Required evidence: `thecat_ui_settings_pause_batch85_manifest.csv`; `thecat_ui_settings_pause_batch85_review_sheet_v001.png`; `thecat_ui_settings_pause_batch85_candidate_review.md`; `validate_ui_settings_pause_preflight_candidates.ps1 passes`; `6 transparent settings/pause sprites and 4 settings/pause mockups stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity settings/pause screenshots, text replacement, 1024x768 key-hint semantics, import settings, click targets, binding, and Console validation remain pending`
- Next action: Batch 85 settings/pause preflight is complete; keep it review-only until Unity-rendered settings and pause screenshots, text replacement, 1024x768 key-hint semantics, import settings, click targets, binding, and Console checks approve a formal install.

### Dream Route Preflight Candidate Pack

- Queue id: `p0_asset_queue_dream_route_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/DreamRoute`
- Required evidence: `thecat_ui_dream_route_batch86_manifest.csv`; `thecat_ui_dream_route_batch86_review_sheet_v001.png`; `thecat_ui_dream_route_batch86_candidate_review.md`; `validate_ui_dream_route_preflight_candidates.ps1 passes`; `6 transparent dream-route sprites and 4 dream-route mockups stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity dream-entry/route-map screenshots, text replacement, node/path semantics, 1024x768 route-card crowding, boss gate dominance, import settings, click targets, binding, and Console validation remain pending`
- Next action: Batch 86 dream-route preflight is complete; keep it review-only until Unity-rendered dream-entry and route-map screenshots, text replacement, node/path semantics, route-card click targets, import settings, binding, and Console checks approve a formal install.

### Formal Install Decision Packet

- Queue id: `p0_asset_queue_formal_install_decision`
- Phase: `FormalUnityInstall`
- State: `BlockedByUnityValidation`
- Candidate directory: `design/development/asset_candidates/formal_install_decisions`
- Unity import root: `Assets/TheCat/Art`
- Required evidence: `explicit human review approval`; `Unity Console has no new errors`; `runtime screenshot comparison passed`; `Batch 54 prop runtime scale and pathing readability passed`; `Batch 61 starter skill VFX runtime scale, skill timing, and readability passed`; `Batch 60 skill HUD feedback timing, HUD scale, and readability passed`; `Batch 62 runtime control icon readability and pause/speed/restart affordance passed`; `Batch 63 runtime control panel readability and pause/speed/restart affordance passed`; `Batch 64 secondary enemy warning readability and future enemy attack affordance passed`; `Batch 65 route map current/selected/path/Boss readability passed`; `Batch 67 bedroom interaction affordance timing, blocked-state, range, and interactable readability passed`; `Batch 83 loading/start spinner, placeholder-state replacement, 1024x768 crowding, import settings, and binding passed`; `Batch 84 result/settlement text replacement, 1024x768 crowding, import settings, and binding passed`; `Batch 85 settings/pause text replacement, 1024x768 key-hint semantics, click-target scale, import settings, and binding passed`; `Batch 86 dream-route text replacement, route node/path semantics, 1024x768 route-card crowding, boss gate scale, click-target scale, import settings, and binding passed`; `Batch 88 character-select source-lock avatar consistency, text replacement, selected/idle state readability, click-target scale, import settings, and binding passed`; `Batch 87 battle HUD four-gauge layout, text replacement, skill states, enemy/telegraph occlusion, click-target scale, import settings, and binding passed`; `Batch 89 skill-selection selected/ready/disabled/locked state readability, cooldown/low-resource/no-target semantics, text replacement, click-target scale, import settings, and binding passed`; `Batch 90 cat-room bed/feeder/litter/dream entrance interactions, hover/disabled/range states, text replacement, click-target scale, import settings, and binding passed`; `manifest and runtime binding diffs are scoped`
- Next action: Create the formal install packet only after active screenshot review approves specific candidates.

## Current MCP Status

- Local Unity MCP setup check found Unity 6000.4.10f1, `com.unity.ai.assistant` 2.12.0-pre.1, relay configuration, and approved connection records.
- Current Codex session does not expose Unity run, Console, scene, or screenshot MCP tools, so this file is an offline handoff checklist rather than completed Unity validation.
- When Unity MCP tools are exposed, run the editor menu `TheCat/P0/Write P0 Asset Unity Validation Checklist`, compare the regenerated file against this artifact, then run the screenshot and Console gates above.
