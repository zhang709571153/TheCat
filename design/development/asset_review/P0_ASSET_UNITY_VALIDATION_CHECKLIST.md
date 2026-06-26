# P0 Asset Unity Validation Checklist

P0 asset Unity validation checklist is ready for 41 queue item(s).

## Counts

- Queue items: 41
- Candidate review items: 36
- Unity-blocked items: 5
- Active screenshot validation items: 2
- Installed asset validation items: 2
- Formal install decision items: 1
- Candidate no-meta policy items: 35
- Console-gated items: 40

## Validation Order

1. Refresh the Unity AssetDatabase.
2. Inspect Sprite import settings for any already-installed assets.
3. Confirm candidate-only folders have no Unity `.meta` files.
4. Capture active-cat, active-enemy, cat-room, cat-room locator/socket marker, cat-room overlay control, character-select, battle HUD, skill-selection, skill VFX, skill-targeting telegraph, character roster status chip, character team slot marker, scene-transition status token, runtime-control, secondary-warning, loading/start, result/settlement, settings/pause, main-menu entry token, scene-selection token, scene-preview backplate, scene-preview accent badge, dream-route/route-map, dream-route map-token, dream-route choice-badge, dream-theme scene-token, reward/event token, combat-feedback token, bedroom battle marker, bedroom map decal, bedroom map decal rework, bedroom obstacle prop, bedroom entry/path prop, bedroom map overlay controls, bedroom event pickup, and bedroom interaction screenshots.
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
- Required evidence: `thecat_ui_cat_room_batch90_manifest.csv`; `thecat_ui_cat_room_batch90_review_sheet_v001.png`; `thecat_ui_cat_room_batch90_candidate_review.md`; `validate_ui_cat_room_preflight_candidates.ps1 passes`; `6 transparent cat-room sprites and 4 cat-room mockups stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; candidate-only Unity preflight report `BATCH90_CAT_ROOM_UNITY_PREFLIGHT_REPORT_2026-06-26.md`; 6/6 imported Sprite settings validated under `Assets/TheCat/Art/UI/CatRoom`; formal runtime catalog leak check passed; focused EditMode `Logs/p0_batch90_cat_room_preflight_editmode_20260626_l4.xml` passed 15/15; current Play Mode `02-cat-room.png` verifies the existing runtime cat-room surface only; Batch90 runtime evidence report `batch_90_cat_room_unity_preflight/runtime_evidence_report.md`; four candidate-backed Unity screenshots under `design/development/screenshots/batch_90_cat_room_unity_preflight`; interaction/text-density review; click-target/prop-scale review; `Runtime evidence: 6/8`; `scene/presenter binding, clean Console, and human approval remain pending`
- Next action: Batch 90 cat-room preflight is ready at 6/8 runtime evidence; keep it review-only until a scene/presenter binding plus clean Console report and explicit human approval approve a formal install. Do not treat `02-cat-room.png` as candidate import acceptance.

### Character Select Preflight Candidate Pack

- Queue id: `p0_asset_queue_character_select_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/CharacterSelect`
- Required evidence: `thecat_ui_character_select_batch88_manifest.csv`; `thecat_ui_character_select_batch88_review_sheet_v001.png`; `thecat_ui_character_select_batch88_candidate_review.md`; `validate_ui_character_select_preflight_candidates.ps1 passes`; `6 transparent character-select sprites and 4 character-select mockups stay outside Assets`; `candidate source PNGs stay outside Assets and have no Unity meta files`; candidate-only Unity preflight report `BATCH88_CHARACTER_SELECT_UNITY_PREFLIGHT_REPORT_2026-06-25.md`; 6/6 imported Sprite settings validated under `Assets/TheCat/Art/UI/CharacterSelect`; formal runtime catalog leak check passed; placeholder/blank screenshot and generic PASS markdown gates reject formal install; runtime evidence report records four candidate-backed Unity screenshots, candidate draw audit 6/6 with fallback=0, source-lock avatar consistency, selected/idle state visibility, Chinese text/density/click-target automatic review, and `Runtime evidence: 6/8`; focused EditMode `Logs/p0_batch88_character_select_preflight_editmode_20260625_runtime_evidence_l3.xml` passed 13/13; clean-report gate now rejects missing runtime logs and dirty logs; focused EditMode `Logs/p0_batch88_character_select_preflight_editmode_20260626_console_gate_hardening_noquit.xml` passed 15/15; `BATCH88_CHARACTER_SELECT_UNITY_PREFLIGHT_CONSOLE_BLOCKERS_2026-06-26.md` records current licensing/Unity AI/network blockers; `scene_binding_console_clean_report.md` and `human_review_approval.md` remain pending
- Next action: Batch 88 character-select is candidate-backed runtime-evidence ready at 6/8 only; keep it blocked from formal install until the scene/presenter/Console report references an existing clean Batch88 runtime evidence log and explicit human review approval lands. Do not formally bind Batch 88 into runtime catalogs before those two gates pass.

### Battle HUD Preflight Candidate Pack

- Queue id: `p0_asset_queue_battle_hud_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/BattleHUD`
- Required evidence: `thecat_ui_battle_hud_batch87_manifest.csv`; `thecat_ui_battle_hud_batch87_review_sheet_v001.png`; `thecat_ui_battle_hud_batch87_candidate_review.md`; `validate_ui_battle_hud_preflight_candidates.ps1 passes`; `6 transparent battle HUD source sprites and 4 battle HUD mockups stay under design candidate review`; source candidate PNGs have no Unity meta files; `BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_REPORT_2026-06-25.md` records 6 candidate-only Unity preflight imports, 6 `.meta` files, 6 dimension matches, 0 formal runtime binding leaks, runtime evidence 6/8, and only `console_clean_report.md` plus `human_review_approval.md` still missing; `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_k1_fix.xml` passed 7/7; `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_l1_reviewfix2_rerun.xml` passed 11/11 and verifies runtime-evidence plan plus screenshot dimension guard; `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_m1_layout_guard.xml` passed 14/14 and verifies visual-content guard coverage; `Logs/p0_batch87_battle_hud_preflight_editmode_20260625_m2_gate_hardening_noquit.xml` passed 17/17 and verifies forged-console, dirty-log, candidate-draw-token, and stale/fallback report blockers; `Logs/batch87_battle_hud_runtime_evidence_20260625_m2_visual_retry.log` captured four real-dimension candidate-backed screenshots with legacy HUD suppression, target-layout non-overlap validation, 6/6 candidate texture resolution, and per-capture fallback=0 draw audit`
- Next action: Keep Batch 87 review-only until the clean Console report and explicit human approval are supplied; do not formally bind Batch 87 into the runtime catalog before those two gates pass.

### Skill Selection Preflight Candidate Pack

- Queue id: `p0_asset_queue_skill_selection_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/SkillSelection`
- Required evidence: `thecat_ui_skill_selection_batch89_manifest.csv`; `thecat_ui_skill_selection_batch89_review_sheet_v001.png`; `thecat_ui_skill_selection_batch89_candidate_review.md`; `validate_ui_skill_selection_preflight_candidates.ps1 passes`; `8 transparent skill-selection source sprites and 4 skill-selection mockups stay under design candidate review`; source candidate PNGs have no Unity meta files; `BATCH89_SKILL_SELECTION_UNITY_PREFLIGHT_REPORT_2026-06-26.md` records 8 candidate-only Unity preflight imports, 8 `.meta` files, 8 dimension matches, 0 formal runtime binding leaks, import validation ready, runtime evidence 6/8, and only scene/Console plus human gates still missing; runtime evidence report `batch_89_skill_selection_unity_preflight/runtime_evidence_report.md`; four candidate-backed Unity screenshots under `design/development/screenshots/batch_89_skill_selection_unity_preflight`; state/text-density review; cooldown/low-resource/click-target review; focused EditMode `Logs/batch89_skill_selection_preflight_editmode_20260626_l8.xml` passed 19/19 and verifies runtime-report tokens, missing-log blockers, candidate draw blockers, and token-only approval rejection`
- Next action: Keep Batch 89 review-only until scene/presenter binding, clean Console, explicit human approval, and the formal runtime binding decision approve a formal install.

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
- Required evidence: `thecat_ui_loading_start_batch83_manifest.csv`; `thecat_ui_loading_start_batch83_review_sheet_v001.png`; `thecat_ui_loading_start_batch83_candidate_review.md`; `validate_ui_loading_start_preflight_candidates.ps1 passes`; `4 transparent loading/start sprites and 4 resolution mockups stay under design candidate review`; source candidate PNGs have no Unity meta files; `BATCH83_LOADING_START_UNITY_PREFLIGHT_REPORT_2026-06-26.md` records 4 candidate-only Unity preflight imports, 4 `.meta` files, 4 dimension matches, 0 formal runtime binding leaks, import validation ready, runtime evidence 6/8, and only scene/Console plus human gates still missing; runtime evidence report `batch_83_loading_start_unity_preflight/runtime_evidence_report.md`; four candidate-backed Unity screenshots under `design/development/screenshots/batch_83_loading_start_unity_preflight`; spinner/placeholder text-density review; progress/crowding click-target review; focused EditMode `Logs/batch83_loading_start_preflight_editmode_20260626_03.xml` passed 15/15 and verifies runtime-report tokens, missing-log blockers, dirty-log blockers, blank-screenshot blockers, generic-review blockers, token-only approval blockers, and missing-import blockers`
- Next action: Batch 83 loading/start is candidate-backed runtime-evidence ready at 6/8 only; keep it blocked from formal install until scene/presenter binding plus a new clean Batch83 Console report and explicit human review approval land. Do not formally bind Batch 83 into runtime catalogs before those gates pass.

### Result Settlement Preflight Candidate Pack

- Queue id: `p0_asset_queue_result_settlement_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/ResultSettlement`
- Required evidence: `thecat_ui_result_settlement_batch84_manifest.csv`; `thecat_ui_result_settlement_batch84_review_sheet_v001.png`; `thecat_ui_result_settlement_batch84_candidate_review.md`; `validate_ui_result_settlement_preflight_candidates.ps1 passes`; `7 transparent result/settlement sprites and 4 result/settlement mockups stay under design candidate review`; source candidate PNGs have no Unity meta files; `BATCH84_RESULT_SETTLEMENT_UNITY_PREFLIGHT_REPORT_2026-06-26.md` records 7 candidate-only Unity preflight imports, 7 `.meta` files, 7 dimension matches, 0 formal runtime binding leaks, import validation ready, runtime evidence 6/8, and only scene/Console plus human gates still missing; runtime evidence report `batch_84_result_settlement_unity_preflight/runtime_evidence_report.md`; four candidate-backed Unity screenshots under `design/development/screenshots/batch_84_result_settlement_unity_preflight`; text/reward readability review; outcome/action click-target review; focused EditMode `Logs/batch84_result_settlement_preflight_editmode_20260626_02.xml` passed 15/15 and verifies candidate isolation, runtime-report tokens, blank-screenshot blockers, dirty-log blockers, generic-review blockers, token-only approval blockers, and missing-import blockers`
- Next action: Batch 84 result/settlement is candidate-backed runtime-evidence ready at 6/8 only; keep it blocked from formal install until scene/presenter binding plus clean Console report, explicit human review approval, and formal runtime binding decision land.

### Settings Pause Preflight Candidate Pack

- Queue id: `p0_asset_queue_settings_pause_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/SettingsPause`
- Required evidence: `thecat_ui_settings_pause_batch85_manifest.csv`; `thecat_ui_settings_pause_batch85_review_sheet_v001.png`; `thecat_ui_settings_pause_batch85_candidate_review.md`; `validate_ui_settings_pause_preflight_candidates.ps1 passes`; `6 transparent settings/pause sprites and 4 settings/pause mockups stay under design candidate review`; source candidate PNGs have no Unity meta files; `BATCH85_SETTINGS_PAUSE_UNITY_PREFLIGHT_REPORT_2026-06-26.md` records 6 candidate-only Unity preflight imports, 6 `.meta` files, 6 dimension matches, 0 formal runtime binding leaks, import validation ready, runtime evidence 6/8, and missing scene/Console plus human gates; `batch_85_settings_pause_unity_preflight/runtime_evidence_report.md` confirms 4/4 screenshots, 6/6 candidate draws with fallback=0, settings main/audio state proof, pause overlay proof, key-hint readability, and click-target evidence; focused EditMode `Logs/batch85_settings_pause_preflight_editmode_20260626_04.xml` passed 15/15 and verifies candidate isolation, runtime-report screenshot blockers, dirty-log blockers, generic-review blockers, token-only approval blockers, and missing-import blockers`
- Next action: Keep Batch 85 review-only until scene/presenter binding, clean Console, explicit human approval, and formal runtime binding decision approve a formal install.

### Dream Route Preflight Candidate Pack

- Queue id: `p0_asset_queue_dream_route_preflight_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/DreamRoute`
- Required evidence: `thecat_ui_dream_route_batch86_manifest.csv`; `thecat_ui_dream_route_batch86_review_sheet_v001.png`; `thecat_ui_dream_route_batch86_candidate_review.md`; `validate_ui_dream_route_preflight_candidates.ps1 passes`; source candidate PNGs and mockups remain under design candidate review with no Unity metas in the candidate folder; `BATCH86_DREAM_ROUTE_UNITY_PREFLIGHT_REPORT_2026-06-26.md` records 6 candidate-only Unity preflight imports under `Assets/TheCat/Art/UI/DreamRoute`, 6 `.meta` files, 6 dimension matches, 0 formal runtime binding leaks, import validation ready, and `Runtime evidence: 6/8`; runtime evidence report `batch_86_dream_route_unity_preflight/runtime_evidence_report.md`; four candidate-backed Unity screenshots under `design/development/screenshots/batch_86_dream_route_unity_preflight`; text/reward and node/path semantics review; route-choice click-target and density review; focused EditMode `Logs/batch86_dream_route_preflight_editmode_20260626_l7.xml` passed 15/15 and verifies candidate isolation, generic-review blockers, token-only approval blockers, missing-import blockers, nonblank screenshot/runtime-report requirements, and dirty-runtime-log blockers; `scene_binding_console_clean_report.md` and `human_review_approval.md` remain pending`
- Next action: Batch 86 dream-route is candidate-backed runtime-evidence ready at 6/8 only; keep it blocked from formal install until scene/presenter binding plus clean Console report and explicit human review approval land.

### Dream Route Map Token Candidate Pack

- Queue id: `p0_asset_queue_dream_route_map_token_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/dream_route_map_tokens/batch_98_dream_route_map_tokens_imagegen_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/DreamRoute`
- Required evidence: `thecat_ui_dream_route_map_token_batch98_semantic_manifest.csv`; `thecat_ui_dream_route_map_token_batch98_semantic_contact_sheet_v001.png`; `thecat_ui_dream_route_map_token_batch98_64px_readability_board_v001.png`; `thecat_ui_dream_route_map_token_batch98_final_review.csv`; `thecat_ui_dream_route_map_token_batch98_process_note.md`; `BATCH98_DREAM_ROUTE_MAP_TOKENS_AGENT_REVIEW_2026-06-25.md`; `9 transparent route-map token sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity route-map screenshots, boss gate density, current-vs-selectable state distinction, locked node/connector contrast, import settings, binding, and Console validation remain pending`
- Next action: Batch 98 dream-route map token pack is complete; keep it review-only until Unity-rendered route-map screenshots, target-size token readability, state distinction, locked connector contrast, import settings, binding, and Console checks approve a formal install.

### Dream Route Choice Badge Candidate Pack

- Queue id: `p0_asset_queue_dream_route_choice_badge_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/dream_route_choice_badges/batch_99_dream_route_choice_badges_imagegen_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/DreamRoute`
- Required evidence: `thecat_ui_choice_badge_batch99_semantic_manifest.csv`; `thecat_ui_choice_badge_batch99_semantic_contact_sheet_v001.png`; `thecat_ui_choice_badge_batch99_64px_card_readability_board_v001.png`; `thecat_ui_choice_badge_batch99_final_review.csv`; `thecat_ui_choice_badge_batch99_process_note.md`; `BATCH99_DREAM_ROUTE_CHOICE_BADGES_AGENT_REVIEW_2026-06-25.md`; `validate_ui_dream_route_choice_badges_batch99.ps1 passes`; `9 transparent route-choice badge sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity route-choice card screenshots, selected/locked/recommended/danger/reward/rest/event/boss/unknown overlay proof, import settings, binding, and Console validation remain pending`
- Next action: Batch 99 dream-route choice badge pack is complete; keep it review-only until Unity-rendered route-choice card screenshots, locked-chain card-width scale, danger/event/boss readability, selected-corner edge margin, import settings, binding, and Console checks approve a formal install.

### Dream Theme Scene Token Candidate Pack

- Queue id: `p0_asset_queue_dream_theme_scene_token_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/dream_theme_scene_tokens/batch_100_dream_theme_scene_tokens_imagegen_2026-06-25`
- Unity import root: `Assets/TheCat/Art/UI/DreamRoute`
- Required evidence: `thecat_ui_theme_token_batch100_semantic_manifest.csv`; `thecat_ui_theme_token_batch100_semantic_contact_sheet_v001.png`; `thecat_ui_theme_token_batch100_64px_theme_selection_board_v001.png`; `thecat_ui_theme_token_batch100_final_review.csv`; `thecat_ui_theme_token_batch100_process_note.md`; `BATCH100_DREAM_THEME_SCENE_TOKENS_AGENT_REVIEW_2026-06-25.md`; `validate_ui_dream_theme_scene_tokens_batch100.ps1 passes`; `9 transparent dream-theme scene-token sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity theme-selection screenshots, symbolic Egypt-token boundary proof, available/unknown/boss 64px contrast, bedroom-vs-return motif distinction, import settings, binding, and Console validation remain pending`
- Next action: Batch 100 dream-theme scene-token pack is complete; keep it review-only until Unity-rendered theme-selection screenshots, symbolic Egypt-token boundary proof, available glow and unknown veil contrast, boss warning density, bedroom-vs-return distinction, import settings, binding, and Console checks approve a formal install.

### Reward/Event Token Candidate Pack

- Queue id: `p0_asset_queue_reward_event_token_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/reward_tokens/batch_104_reward_tokens_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/Rewards`
- Required evidence: `tc_ui_rwd_batch104_semantic_manifest.csv`; `tc_ui_rwd_batch104_semantic_contact_sheet_v001.png`; `tc_ui_rwd_batch104_64px_reward_event_readability_board_v001.png`; `tc_ui_rwd_batch_104_reward_tokens_2026-06-26_final_review.csv`; `tc_ui_rwd_batch_104_reward_tokens_2026-06-26_process_note.md`; `BATCH104_REWARD_EVENT_TOKENS_AGENT_REVIEW_2026-06-26.md`; `validate_ui_reward_event_tokens_batch104.ps1 passes`; `9 transparent reward/event token sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `event_question_token, curse_risk_token, partner_invite_token, and claim_confirm_token remain candidate_conditional`; `Unity reward/event/shop/blessing screenshots, 64px live contrast, import settings, binding, and Console validation remain pending`
- Next action: Batch 104 reward/event token pack is complete; keep it review-only until Unity-rendered reward/event/shop/blessing UI screenshots, 64px live contrast, no-baked-text symbolic affordance proof, import settings, binding, no recursive candidate import, and Console checks approve a formal install.

### Combat Feedback Token Candidate Pack

- Queue id: `p0_asset_queue_combat_feedback_token_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/vfx/combat_feedback/batch_105_combat_feedback_2026-06-26`
- Unity import root: `Assets/TheCat/Art/VFX/CombatFeedback`
- Required evidence: `tc_vfx_cfb_batch105_semantic_manifest.csv`; `tc_vfx_cfb_batch105_semantic_contact_sheet_v001.png`; `tc_vfx_cfb_batch105_64px_combat_feedback_readability_board_v001.png`; `tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_final_review.csv`; `tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_process_note.md`; `BATCH105_COMBAT_FEEDBACK_TOKENS_AGENT_REVIEW_2026-06-26.md`; `validate_vfx_combat_feedback_tokens_batch105.ps1 passes`; `9 transparent combat-feedback token sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `damage_number_chip_token, bed_hit_crack_token, monster_death_puff_token, and debuff_burst_token remain candidate_conditional`; `Unity combat screenshots, runtime number overlay proof, bed-hit context proof, dark-background live contrast, import settings, binding, and Console validation remain pending`
- Next action: Batch 105 combat-feedback token pack is complete; keep it review-only until Unity-rendered combat feedback screenshots, combat-event binding proof, runtime number overlay proof, bed-hit context proof, dark-background live contrast, import settings, no recursive candidate import, and Console checks approve a formal install.

### Skill Targeting Telegraph Candidate Pack

- Queue id: `p0_asset_queue_skill_targeting_telegraph_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/vfx/skill_targeting_telegraphs/batch_115_skill_targeting_telegraphs_2026-06-26`
- Unity import root: `Assets/TheCat/Art/VFX/SkillTargetingTelegraphs`
- Required evidence: `tgt_batch115_semantic_manifest.csv`; `tgt_batch115_semantic_contact_sheet_v001.png`; `tgt_batch115_128px_64px_skill_targeting_readability_board_v001.png`; `tc_vfx_tgt_batch_115_skill_targeting_telegraphs_2026-06-26_final_review.csv`; `tc_vfx_tgt_batch_115_skill_targeting_telegraphs_2026-06-26_process_note.md`; `BATCH115_SKILL_TARGETING_TELEGRAPHS_AGENT_REVIEW_2026-06-26.md`; `validate_vfx_skill_targeting_telegraphs_batch115.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent skill-targeting telegraph sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `skill_aoe_circle_field, skill_chain_link_path, skill_shield_zone_floor, and skill_summon_slot_rune remain candidate_conditional`; `Unity skill-cast battle screenshots, 128px/64px live contrast, route/HUD/combat-token conflict proof, opacity/occlusion proof, import settings, binding, and Console validation remain pending`
- Next action: Batch 115 skill-targeting telegraph pack is complete; keep it review-only until Unity-rendered skill-cast battle screenshots, target-state binding proof, route/HUD/combat-token conflict proof, opacity/occlusion proof, import settings, no recursive candidate import, and Console checks approve a formal install.

### Character Roster Status Chip Candidate Pack

- Queue id: `p0_asset_queue_character_roster_status_chip_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/character_roster_status_chips/batch_116_character_roster_status_chips_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/CharacterRosterStatusChips`
- Required evidence: `tc_ui_roster_batch116_semantic_manifest.csv`; `tc_ui_roster_batch116_semantic_contact_sheet_v001.png`; `tc_ui_roster_batch116_96px_64px_character_roster_readability_board_v001.png`; `thecat_batch_116_character_roster_status_chips_2026-06-26_final_review.csv`; `thecat_batch_116_character_roster_status_chips_2026-06-26_process_note.md`; `BATCH116_CHARACTER_ROSTER_STATUS_CHIPS_AGENT_REVIEW_2026-06-26.md`; `validate_ui_character_roster_status_chips_batch116.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent character-roster status-chip sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `character_roster_source_locked_chip, character_roster_selected_ribbon, character_roster_scene_affinity_chip, and character_roster_team_slot_chip remain candidate_conditional`; `Unity character roster/card UI screenshots, 96px/64px dark/warm live contrast, ready/selected/team distinction, locked/source-locked distinction, new/scene-affinity distinction, Batch88/113/114 collision proof, RectTransform binding, and Console validation remain pending`
- Next action: Batch 116 character-roster status-chip pack is complete; keep it review-only until Unity-rendered character roster/card UI screenshots, target-state binding proof, 96px/64px live contrast, ready/selected/team and locked/source-locked collision proof, import settings, no recursive candidate import, RectTransform binding, and Console checks approve a formal install.

### Character Team Slot Marker Candidate Pack

- Queue id: `p0_asset_queue_character_team_slot_marker_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/character_team_slot_markers/batch_118_character_team_slot_markers_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/CharacterTeamSlots`
- Required evidence: `thecat_ui_team_slot_batch118_semantic_manifest.csv`; `thecat_ui_team_slot_batch118_semantic_contact_sheet_v002.png`; `thecat_ui_team_slot_batch118_96px_64px_dark_warm_readability_board_v002.png`; `thecat_batch_118_character_team_slot_markers_2026-06-26_final_review.csv`; `thecat_batch_118_character_team_slot_markers_2026-06-26_process_note.md`; `BATCH118_CHARACTER_TEAM_SLOT_MARKERS_AGENT_REVIEW_2026-06-26.md`; `validate_ui_character_team_slot_markers_batch118.ps1 passes`; `59-lane non-cat validation matrix passes`; `9 transparent character-team slot marker sprites stay outside Assets`; `front_line_slot_marker uses candidate_v002`; `candidate PNGs stay outside Assets and have no Unity meta files`; `3 semantic sprites are candidate_keep and 6 are candidate_conditional`; `active review variants use short prefixes`; `Unity character-team UI screenshots, 96px/64px dark/warm live contrast, Batch95/113/116 semantic collision proof, front/rear/reserve/lock formation-context proof, RectTransform binding, no recursive candidate import, human approval, and Console validation remain pending`
- Next action: Batch 118 character-team slot marker pack is complete; keep it review-only until Unity-rendered character-team UI screenshots prove role/slot context, front-vs-shield separation, rear/support clarity, reserve/lock semantics, Batch95/113/116 non-duplication, import settings, RectTransform binding, no recursive candidate import, clean Console, and explicit human approval.

### Main Menu Entry Token Candidate Pack

- Queue id: `p0_asset_queue_main_menu_entry_token_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/main_menu_entry_tokens/batch_106_main_menu_entry_tokens_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/MainMenu`
- Required evidence: `tc_ui_menu_batch106_semantic_manifest.csv`; `tc_ui_menu_batch106_semantic_contact_sheet_v001.png`; `tc_ui_menu_batch106_64px_main_menu_readability_board_v001.png`; `tc_ui_menu_batch_106_main_menu_entry_tokens_2026-06-26_final_review.csv`; `tc_ui_menu_batch_106_main_menu_entry_tokens_2026-06-26_process_note.md`; `BATCH106_MAIN_MENU_ENTRY_TOKENS_AGENT_REVIEW_2026-06-26.md`; `validate_ui_main_menu_entry_tokens_batch106.ps1 passes`; `9 transparent main-menu entry token sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `cat_room_home_token, settings_cog_token, exit_door_token, and disabled_lock_token remain candidate_conditional`; `Unity main-menu screenshots, start/exit semantic disambiguation, settings/lock semantic disambiguation, cat-room/home meaning proof, target-background readability, import settings, binding, and Console validation remain pending`
- Next action: Batch 106 main-menu entry token pack is complete; keep it review-only until Unity-rendered main-menu screenshots, target-size token readability, start/exit and settings/lock disambiguation, cat-room/home meaning proof, import settings, no recursive candidate import, and Console checks approve a formal install.

### Scene Selection Token Candidate Pack

- Queue id: `p0_asset_queue_scene_selection_token_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/SceneSelection`
- Required evidence: `tc_ui_scene_batch107_semantic_manifest.csv`; `tc_ui_scene_batch107_semantic_contact_sheet_v001.png`; `tc_ui_scene_batch107_64px_scene_selection_readability_board_v001.png`; `tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_final_review.csv`; `tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_process_note.md`; `BATCH107_SCENE_SELECTION_TOKENS_AGENT_REVIEW_2026-06-26.md`; `validate_ui_scene_selection_tokens_batch107.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent scene-selection token sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `cat_room_scene_card_token, dream_route_scene_card_token, shop_event_scene_card_token, and unknown_scene_card_token remain candidate_conditional`; `Unity scene-selection screenshots, unknown-state readability, cat-room/shop/reward distinction, target-background readability, import settings, binding, and Console validation remain pending`
- Next action: Batch 107 scene-selection token pack is complete; keep it review-only until Unity-rendered scene-selection screenshots, target-size token readability, unknown-state interpretation, cat-room/shop/reward distinction, import settings, no recursive candidate import, and Console checks approve a formal install.

### Bedroom Battle Marker Candidate Pack

- Queue id: `p0_asset_queue_bedroom_battle_marker_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/map/bedroom_dream_battle_markers/batch_101_bedroom_dream_battle_markers_imagegen_2026-06-25`
- Unity import root: `Assets/TheCat/Art/Scenes/BedroomDream`
- Required evidence: `thecat_map_bedmark_batch101_semantic_manifest.csv`; `thecat_map_bedmark_batch101_semantic_contact_sheet_v001.png`; `thecat_map_bedmark_batch101_64px_bedroom_map_readability_board_v001.png`; `thecat_map_bedmark_batch101_final_review.csv`; `thecat_map_bedmark_batch101_process_note.md`; `BATCH101_BEDROOM_DREAM_BATTLE_MARKERS_AGENT_REVIEW_2026-06-25.md`; `validate_map_bedroom_dream_battle_markers_batch101.ps1 passes`; `9 transparent bedroom battle marker sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `Unity bedroom battle screenshots, bed-anchor scale, four-entry direction readability, safe-zone/path-arrow/spawn-warning contrast, scene-owned collider policy, import settings, binding, and Console validation remain pending`
- Next action: Batch 101 bedroom battle marker pack is complete; keep it review-only until Unity-rendered bedroom battle screenshots, bed-anchor scale, four-entry direction readability, safe-zone/path-arrow/spawn-warning contrast, scene-owned collider policy, import settings, binding, and Console checks approve a formal install.

### Bedroom Dream Map Decal Candidate Pack

- Queue id: `p0_asset_queue_bedroom_dream_map_decal_candidates`
- Phase: `CodexCandidateProduction`
- State: `PartialCandidatePackCompletePendingUnityReviewAndRework`
- Candidate directory: `design/development/asset_candidates/map/bedroom_dream_map_decals/batch_102_bedroom_dream_map_decals_imagegen_2026-06-25`
- Unity import root: `Assets/TheCat/Art/Scenes/BedroomDream`
- Required evidence: `thecat_map_beddec_batch102_semantic_manifest.csv`; `thecat_map_beddec_batch102_semantic_contact_sheet_v001.png`; `thecat_map_beddec_batch102_64px_bedroom_map_readability_board_v001.png`; `thecat_map_beddec_batch102_final_review.csv`; `thecat_map_beddec_batch102_process_note.md`; `BATCH102_BEDROOM_DREAM_MAP_DECALS_AGENT_REVIEW_2026-06-25.md`; `validate_map_bedroom_dream_map_decals_batch102.ps1 passes`; `9 transparent tracked bedroom dream map decal sprites stay outside Assets with only accepted/conditional subset eligible for review`; `candidate PNGs stay outside Assets and have no Unity meta files`; `reject_rework items 04/05/08/09 are excluded from import`; `Unity bedroom map screenshots, accepted decal contrast, blanket decorative-only proof, dust live-floor contrast, import settings, binding, and Console validation remain pending`
- Next action: Batch 102 is partial-candidate complete; review only the accepted/conditional subset until Unity-rendered bedroom map screenshots, accepted decal contrast, blanket decorative-only proof, dust live-floor contrast, rejected-item exclusion, import settings, binding, and Console checks approve a formal install.

### Bedroom Dream Map Decal Rework Candidate Pack

- Queue id: `p0_asset_queue_bedroom_dream_map_decal_rework_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/map/bedroom_dream_map_decals/batch_103_bedroom_dream_map_decals_rework_imagegen_2026-06-26`
- Unity import root: `Assets/TheCat/Art/Scenes/BedroomDream`
- Required evidence: `thecat_map_beddec103_batch103_semantic_manifest.csv`; `thecat_map_beddec103_batch103_semantic_contact_sheet_v001.png`; `thecat_map_beddec103_batch103_64px_bedroom_map_readability_board_v001.png`; `thecat_map_beddec103_batch103_final_review.csv`; `thecat_map_beddec103_batch103_process_note.md`; `BATCH103_BEDROOM_DREAM_MAP_DECALS_REWORK_AGENT_REVIEW_2026-06-26.md`; `validate_map_bedroom_dream_map_decals_rework_batch103.ps1 passes`; `4 transparent reworked bedroom dream map decal sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `nightmare_puddle_v002 is candidate_keep`; `pillow_barricade_v002, toy_block_obstacle_v002, and bed_aura_floor_glow_v002 are candidate_conditional`; `Unity bedroom map screenshots, nightmare puddle contrast, aura warm-floor contrast, soft-obstacle scale/collider proof, import settings, binding, and Console validation remain pending`
- Next action: Batch 103 reworks the Batch102 rejected concepts; keep it review-only until Unity-rendered bedroom map screenshots, nightmare puddle contrast, aura warm-floor contrast, pillow/toy floor-hugging scale, scene-owned collider policy, import settings, binding, and Console checks approve a formal install.

### Bedroom Obstacle Prop Candidate Pack

- Queue id: `p0_asset_queue_bedroom_obstacle_prop_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/map/bedroom_obstacle_props/batch_108_bedroom_obstacle_props_2026-06-26`
- Unity import root: `Assets/TheCat/Art/Scenes/BedroomDream`
- Required evidence: `thecat_map_obst_batch108_semantic_manifest.csv`; `thecat_map_obst_batch108_semantic_contact_sheet_v001.png`; `thecat_map_obst_batch108_64px_bedroom_obstacle_readability_board_v001.png`; `thecat_map_obst_batch_108_bedroom_obstacle_props_2026-06-26_final_review.csv`; `thecat_map_obst_batch_108_bedroom_obstacle_props_2026-06-26_process_note.md`; `BATCH108_BEDROOM_OBSTACLE_PROPS_AGENT_REVIEW_2026-06-26.md`; `validate_map_bedroom_obstacle_props_batch108.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent bedroom obstacle/prop sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `nightstand_corner_prop and moon_dust_patch_prop remain candidate_conditional`; `moon_dust_patch_prop stays decal-only with no collider`; `Unity bedroom map screenshots, obstacle scale proof, nightstand furniture-vs-obstacle proof, moon-dust decal contrast/no-collider proof, dream-crystal prop-vs-VFX layer proof, import settings, binding, and Console validation remain pending`
- Next action: Batch 108 bedroom obstacle prop pack is complete; keep it review-only until Unity-rendered bedroom map screenshots, obstacle scale and collider policy, nightstand role, moon-dust decal/no-collider proof, dream-crystal layer decision, import settings, no recursive candidate import, and Console checks approve a formal install.

### Bedroom Entry/Path Prop Candidate Pack

- Queue id: `p0_asset_queue_bedroom_entry_path_prop_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26`
- Unity import root: `Assets/TheCat/Art/Scenes/BedroomDream`
- Required evidence: `thecat_map_path_batch109_semantic_manifest.csv`; `thecat_map_path_batch109_semantic_contact_sheet_v001.png`; `thecat_map_path_batch109_64px_bedroom_entry_path_readability_board_v001.png`; `thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_final_review.csv`; `thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_process_note.md`; `BATCH109_BEDROOM_ENTRY_PATH_PROPS_AGENT_REVIEW_2026-06-26.md`; `validate_map_bedroom_entry_path_props_batch109.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent bedroom entry/path sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `blanket_curtain_entry_prop, alarm_clock_hazard_prop, nightlight_safe_glow_marker, and wall_crack_entry_marker remain candidate_conditional`; `Unity bedroom map screenshots, entry/path semantic proof, floor/decal contrast, scale proof against Batch101/102/103/108, sorting/collider policy, import settings, binding, and Console validation remain pending`
- Next action: Batch 109 bedroom entry/path prop pack is complete; keep it review-only until Unity-rendered bedroom map screenshots, doorway/path/safe/hazard/entry-crack semantic proof, warm-floor and dark-shadow contrast, scale and sorting against prior bedroom map batches, import settings, no recursive candidate import, and Console checks approve a formal install.

### Bedroom Map Overlay Controls Candidate Pack

- Queue id: `p0_asset_queue_bedroom_map_overlay_controls_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/bedroom_map_overlay_controls/batch_110_bedroom_map_overlay_controls_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/BedroomMapOverlay`
- Required evidence: `thecat_ui_mapovr_batch110_semantic_manifest.csv`; `thecat_ui_mapovr_batch110_semantic_contact_sheet_v001.png`; `thecat_ui_mapovr_batch110_64px_bedroom_map_overlay_readability_board_v001.png`; `thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_final_review.csv`; `thecat_ui_mapovr_batch_110_bedroom_map_overlay_controls_2026-06-26_process_note.md`; `BATCH110_BEDROOM_MAP_OVERLAY_CONTROLS_AGENT_REVIEW_2026-06-26.md`; `validate_ui_bedroom_map_overlay_controls_batch110.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent bedroom map overlay sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `map_legend_entry_token and map_overlay_pin_available remain candidate_conditional`; `Unity bedroom map overlay screenshots, legend/pin/foldout semantic proof, entry-vs-available and selected-vs-available distinction, info-frame content padding, import settings, binding, and Console validation remain pending`
- Next action: Batch 110 bedroom map overlay controls pack is complete; keep it review-only until Unity-rendered bedroom map overlay screenshots prove legend readability, pin-state separation, foldout content padding, no recursive candidate import, and Console checks approve a formal install.

### Bedroom Event Pickup Candidate Pack

- Queue id: `p0_asset_queue_bedroom_event_pickup_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/map/bedroom_event_pickups/batch_117_bedroom_event_pickups_2026-06-26`
- Unity import root: `Assets/TheCat/Art/Scenes/BedroomDream/Pickups`
- Required evidence: `tc_map_pickup_batch117_semantic_manifest.csv`; `tc_map_pickup_batch117_semantic_contact_sheet_v002.png`; `tc_map_pickup_batch117_96px_64px_bedroom_event_pickup_readability_board_v002.png`; `tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_final_review.csv`; `tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_process_note.md`; `BATCH117_BEDROOM_EVENT_PICKUPS_AGENT_REVIEW_2026-06-26.md`; `validate_map_bedroom_event_pickups_batch117.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent bedroom event pickup sprites stay outside Assets`; `story_page_clue_pickup uses candidate_v002`; `candidate PNGs stay outside Assets and have no Unity meta files`; `5 semantic sprites are candidate_keep and 4 are candidate_conditional`; `Unity bedroom map pickup screenshots, 96px/64px dark/warm live contrast, ticket/page/key/dice ambiguity strip, Batch101/108/109/104 semantic conflict proof, scene-owned trigger-or-none collider policy, import settings, binding, and Console validation remain pending`
- Next action: Keep Batch117 review-only until Unity-rendered bedroom map screenshots prove pickup scale/readability, ticket/page/key/dice distinction, semantic separation from earlier bedroom and reward/event batches, import settings, no recursive candidate import, scene-owned trigger-or-none collider policy, and Console checks approve promotion.

### Bedroom Route Socket Marker Candidate Pack

- Queue id: `p0_asset_queue_bedroom_route_socket_marker_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/map/bedroom_route_socket_markers/batch_119_bedroom_route_socket_markers_2026-06-26`
- Unity import root: `Assets/TheCat/Art/Scenes/BedroomDream/Sockets`
- Required evidence: `thecat_map_sock_batch119_semantic_manifest.csv`; `thecat_map_sock_batch119_semantic_contact_sheet_v001.png`; `thecat_map_sock_batch119_96px_64px_bedroom_socket_readability_board_v001.png`; `thecat_batch_119_bedroom_route_socket_markers_2026-06-26_final_review.csv`; `thecat_batch_119_bedroom_route_socket_markers_2026-06-26_process_note.md`; `BATCH119_BEDROOM_ROUTE_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md`; `validate_map_bedroom_route_socket_markers_batch119.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent bedroom route/socket marker sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `5 semantic sprites are candidate_keep and 4 are candidate_conditional`; `Unity bedroom map socket screenshots, 96px/64px dark/warm live contrast, directional-entry distinction, bed/safe/reward/locked context proof, Batch101/109/117 and Batch107/116/118 semantic collision proof, scene-owned visual-overlay collider policy, import settings, binding, no recursive candidate import, human approval, and Console validation remain pending`
- Next action: Keep Batch119 review-only until Unity-rendered bedroom map socket screenshots prove direction, context, live contrast, and semantic separation from prior bedroom/pickup/lock batches; then validate import settings, no recursive candidate import, human approval, and Console checks before promotion.

### Scene Transition Status Token Candidate Pack

- Queue id: `p0_asset_queue_scene_transition_status_token_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/scene_transition_status_tokens/batch_120_scene_transition_status_tokens_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/SceneTransitions`
- Required evidence: `thecat_ui_scene_xfer_batch120_semantic_manifest.csv`; `thecat_ui_scene_xfer_batch120_semantic_contact_sheet_v001.png`; `thecat_ui_scene_xfer_batch120_96px_64px_scene_transition_readability_board_v001.png`; `tc120_final_review.csv`; `tc120_process_note.md`; `BATCH120_SCENE_TRANSITION_STATUS_TOKENS_AGENT_REVIEW_2026-06-26.md`; `validate_ui_scene_transition_status_tokens_batch120.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent scene-transition status token sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `3 semantic sprites are candidate_keep and 6 are candidate_conditional`; `Unity scene-transition screenshots at 96px/64px on dark/warm backgrounds, bedroom/cat-room distinction, dream/unknown distinction, reward/shop-event distinction, locked/unknown distinction, battle/generic-combat distinction, settings-safe/generic-gear distinction, import settings, binding, no recursive candidate import, human approval for unknown symbol, and Console validation remain pending`
- Next action: Keep Batch120 review-only until Unity-rendered scene-transition UI screenshots prove state distinction and live contrast, then validate import settings, no recursive candidate import, human approval for the unknown symbol, and Console checks before promotion.

### Cat-Room Locator Socket Marker Candidate Pack

- Queue id: `p0_asset_queue_cat_room_locator_socket_marker_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/map/cat_room_locator_socket_markers/batch_121_cat_room_locator_socket_markers_2026-06-26`
- Unity import root: `Assets/TheCat/Art/Scenes/CatRoom/Sockets`
- Required evidence: `thecat_map_catroom_socket_batch121_semantic_manifest.csv`; `thecat_map_catroom_socket_batch121_semantic_contact_sheet_v001.png`; `thecat_map_catroom_socket_batch121_96px_64px_cat_room_readability_board_v001.png`; `thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_final_review.csv`; `thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_process_note.md`; `BATCH121_CAT_ROOM_LOCATOR_SOCKET_MARKERS_AGENT_REVIEW_2026-06-26.md`; `validate_map_cat_room_locator_socket_markers_batch121.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent cat-room locator/socket marker sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `3 semantic sprites are candidate_keep and 6 are candidate_conditional`; `Unity cat-room screenshots at 96px/64px, marker-vs-prop proof beside Batch91/92/93 context, bed/rest, feeder/hunger, litter/cleanup, portal/return pairwise proof, import settings, binding, no recursive candidate import, human approval, and Console validation remain pending`
- Next action: Keep Batch121 review-only until Unity-rendered cat-room screenshots prove marker placement, live contrast, and semantic separation from Batch91/92/93 props, interaction states, and decals; then validate import settings, no recursive candidate import, human approval, and Console checks before promotion.

### Cat-Room Overlay Control Candidate Pack

- Queue id: `p0_asset_queue_cat_room_overlay_control_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/cat_room_overlay_controls/batch_122_cat_room_overlay_controls_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/CatRoomOverlayControls`
- Required evidence: `thecat_ui_catroom_overlay_batch122_semantic_manifest.csv`; `thecat_ui_catroom_overlay_batch122_semantic_contact_sheet_v001.png`; `thecat_ui_catroom_overlay_batch122_96px_64px_readability_board_v001.png`; `thecat_batch_122_cat_room_overlay_controls_2026-06-26_final_review.csv`; `thecat_batch_122_cat_room_overlay_controls_2026-06-26_process_note.md`; `BATCH122_CAT_ROOM_OVERLAY_CONTROLS_AGENT_REVIEW_2026-06-26.md`; `validate_ui_cat_room_overlay_controls_batch122.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent cat-room overlay-control sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `4 semantic sprites are candidate_keep and 5 are candidate_conditional`; `Unity cat-room overlay screenshots at 96px/64px, control-vs-marker proof beside Batch90/91/92/93/121 context, resource/filter semantic proof, import settings, binding, no recursive candidate import, human approval, and Console validation remain pending`
- Next action: Keep Batch122 review-only until Unity-rendered cat-room overlay screenshots prove control placement, live contrast, and semantic separation from Batch90/91/92/93/121 props, states, decals, and locator/socket markers; then validate import settings, no recursive candidate import, human approval, and Console checks before promotion.

### Scene Preview Backplates Candidate Pack

- Queue id: `p0_asset_queue_scene_preview_backplates_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/ScenePreviewBackplates`
- Required evidence: `tc_ui_scene_prev_batch111_semantic_manifest.csv`; `tc_ui_scene_prev_batch111_semantic_contact_sheet_v001.png`; `tc_ui_scene_prev_batch111_192x108_scene_preview_readability_board_v001.png`; `tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_final_review.csv`; `tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_process_note.md`; `BATCH111_SCENE_PREVIEW_BACKPLATES_AGENT_REVIEW_2026-06-26.md`; `validate_ui_scene_preview_backplates_batch111.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent scene-preview backplate sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `all 9 semantic sprites are candidate_keep`; `cat_room_preview_backplate and dream_route_preview_backplate remain symbolic preview cards only`; `Unity scene-selection screenshots, scene preview backplate distinction, card-content overlay padding, cat-room/shop/reward distinction, unknown-vs-dream-route distinction, import settings, binding, and Console validation remain pending`
- Next action: Batch 111 scene-preview backplate pack is complete; keep it review-only until Unity-rendered scene-selection screenshots prove backplate distinction, overlay padding, symbolic-card boundaries, no recursive candidate import, and Console checks approve a formal install.

### Scene Preview Accent Badges Candidate Pack

- Queue id: `p0_asset_queue_scene_preview_accent_badges_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/scene_preview_accent_badges/batch_112_scene_preview_accent_badges_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/ScenePreviewAccentBadges`
- Required evidence: `tc_ui_scene_accent_batch112_semantic_manifest.csv`; `tc_ui_scene_accent_batch112_semantic_contact_sheet_v001.png`; `tc_ui_scene_accent_batch112_96px_64px_scene_preview_readability_board_v001.png`; `tc_batch_112_scene_preview_accent_badges_2026-06-26_final_review.csv`; `tc_batch_112_scene_preview_accent_badges_2026-06-26_process_note.md`; `BATCH112_SCENE_PREVIEW_ACCENT_BADGES_AGENT_REVIEW_2026-06-26.md`; `validate_ui_scene_preview_accent_badges_batch112.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent scene-preview accent badge sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `5 semantic sprites are candidate_keep and 4 are candidate_conditional`; `bedroom_accent_badge, cat_room_accent_badge, battle_accent_badge, and shop_event_accent_badge remain conditional until placement/density proof`; `Unity scene-selection screenshots, accent overlay placement over Batch111 backplates, mini-scene-card density proof, cat-room/shop/reward distinction, unknown-vs-dream-route distinction, import settings, binding, and Console validation remain pending`
- Next action: Batch 112 scene-preview accent badge pack is complete; keep it review-only until Unity-rendered scene-selection screenshots prove accent overlay placement, density, symbolic-card boundaries, no recursive candidate import, and Console checks approve a formal install.

### Character Scene Affinity Badges Candidate Pack

- Queue id: `p0_asset_queue_character_scene_affinity_badges_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/character_scene_affinity_badges/batch_113_character_scene_affinity_badges_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/CharacterSceneAffinityBadges`
- Required evidence: `thecat_ui_char_scene_aff_batch113_semantic_manifest.csv`; `thecat_ui_char_scene_aff_batch113_semantic_contact_sheet_v001.png`; `thecat_ui_char_scene_aff_batch113_96px_64px_readability_board_v001.png`; `thecat_batch_113_character_scene_affinity_badges_2026-06-26_final_review.csv`; `thecat_batch_113_character_scene_affinity_badges_2026-06-26_process_note.md`; `BATCH113_CHARACTER_SCENE_AFFINITY_BADGES_AGENT_REVIEW_2026-06-26.md`; `validate_ui_character_scene_affinity_badges_batch113.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent character-scene affinity badge sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `5 semantic sprites are candidate_keep and 4 are candidate_conditional`; `nephthys_dream_route_badge, suzune_cat_room_support_badge, cotton_safe_room_badge, and unknown_role_scene_badge remain conditional until target-background and wording proof`; `Unity character-scene UI screenshots, dark/warm target-background readability, unknown-token wording proof, layout normalization, import settings, binding, and Console validation remain pending`
- Next action: Batch 113 character-scene affinity badge pack is complete; keep it review-only until Unity-rendered character-scene UI screenshots prove target-background readability, unknown-token wording, layout normalization, no recursive candidate import, and Console checks approve a formal install.

### Character Scene Status Corner Tabs Candidate Pack

- Queue id: `p0_asset_queue_character_scene_status_corner_tabs_candidates`
- Phase: `CodexCandidateProduction`
- State: `CandidatePackCompletePendingUnityReview`
- Candidate directory: `design/development/asset_candidates/ui/character_scene_status_corner_tabs/batch_114_character_scene_status_corner_tabs_2026-06-26`
- Unity import root: `Assets/TheCat/Art/UI/CharacterSceneStatusCornerTabs`
- Required evidence: `thecat_ui_char_scene_tab_batch114_semantic_manifest.csv`; `thecat_ui_char_scene_tab_batch114_semantic_contact_sheet_v001.png`; `thecat_ui_char_scene_tab_batch114_96px_64px_readability_board_v001.png`; `thecat_batch_114_character_scene_status_corner_tabs_2026-06-26_final_review.csv`; `thecat_batch_114_character_scene_status_corner_tabs_2026-06-26_process_note.md`; `BATCH114_CHARACTER_SCENE_STATUS_CORNER_TABS_AGENT_REVIEW_2026-06-26.md`; `validate_ui_character_scene_status_corner_tabs_batch114.ps1 passes`; `validate_candidate_pack.py passes`; `9 transparent character-scene status corner-tab sprites stay outside Assets`; `candidate PNGs stay outside Assets and have no Unity meta files`; `6 semantic sprites are candidate_keep and 3 are candidate_conditional`; `unknown_pairing_corner_tab, selected_pairing_corner_tab, and team_slot_ready_corner_tab remain conditional until target UI, selected-state, and team-ready non-duplication proof`; `Unity character-scene UI screenshots, dark/warm target-background readability, unknown-state wording proof, selected-state context proof, team-ready badge non-duplication proof, import settings, binding, and Console validation remain pending`
- Next action: Batch 114 character-scene status corner-tab pack is complete; keep it review-only until Unity-rendered character-scene UI screenshots prove target-background readability, unknown/selected/team-ready semantics, no recursive candidate import, and Console checks approve a formal install.

### Formal Install Decision Packet

- Queue id: `p0_asset_queue_formal_install_decision`
- Phase: `FormalUnityInstall`
- State: `BlockedByUnityValidation`
- Candidate directory: `design/development/asset_candidates/formal_install_decisions`
- Unity import root: `Assets/TheCat/Art`
- Required evidence before formal install: explicit human review approval; Unity Console has no new errors; runtime screenshot comparison passes; manifest and runtime binding diffs are scoped.
- Installed/candidate lanes must each provide their own runtime-scale readability, text/value replacement, click-target, import settings, scene/presenter binding, and clean Console evidence before promotion.
- Current Batch 86 state: dream-entry/route-map screenshots, route-state readability, text/reward replacement, node/path semantics, boss-gate scale, route-card density, click-target scale, and import settings are evidenced at `Runtime evidence: 6/8`; scene/presenter binding, clean Console, explicit human approval, and formal runtime binding decision remain pending.
- Current Batch 84 state: battle victory/defeat and run clear/fail screenshots, text/reward readability, outcome/action semantics, click-target scale, and import settings are evidenced at `Runtime evidence: 6/8`; scene/presenter binding, clean Console, explicit human approval, and formal runtime binding decision remain pending.
- Current Batch 89 state: skill-selection screenshots, selected/ready/disabled/locked state readability, cooldown/low-resource/no-target semantics, text/value replacement, click-target scale, and import settings are evidenced at `Runtime evidence: 6/8`; scene/presenter binding, clean Console, explicit human approval, and formal runtime binding decision remain pending.
- Current Batch 90 state: cat-room bed/feeder/litter/dream entrance interaction states, hover/disabled/range semantics, text/value replacement, click-target/prop scale, and import settings are evidenced at `Runtime evidence: 6/8`; scene/presenter binding, clean Console, explicit human approval, and formal runtime binding decision remain pending.
- Next action: Create the formal install packet only after active screenshot review approves specific candidates.

## Current MCP Status

- Local Unity MCP setup check found Unity 6000.4.10f1, `com.unity.ai.assistant` 2.12.0-pre.1, relay configuration, and approved connection records.
- Current Codex session does not expose Unity run, Console, scene, or screenshot MCP tools, so this file is an offline handoff checklist rather than completed Unity validation.
- When Unity MCP tools are exposed, run the editor menu `TheCat/P0/Write P0 Asset Unity Validation Checklist`, compare the regenerated file against this artifact, then run the screenshot and Console gates above.
