using System.Collections.Generic;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Data.Catalogs
{
    public static class P0AssetProductionQueueCatalog
    {
        public const int ExpectedP0QueueCount = 19;
        public const int ExpectedCodexRunnableCount = 0;
        public const int ExpectedCandidatePackCompletePendingUnityReviewCount = 14;
        public const int ExpectedUnityBlockedCount = 5;

        public const string StarterCatActiveValidationQueueId = "p0_asset_queue_starter_cat_active_validation";
        public const string CoreEnemyActiveValidationQueueId = "p0_asset_queue_core_enemy_active_validation";
        public const string BedroomInteractableCandidateQueueId = "p0_asset_queue_bedroom_interactable_candidates";
        public const string StarterSkillVfxCandidateQueueId = "p0_asset_queue_starter_skill_vfx_candidates";
        public const string SkillHudFeedbackCandidateQueueId = "p0_asset_queue_skill_hud_feedback_candidates";
        public const string CatRoomPreflightCandidateQueueId = "p0_asset_queue_cat_room_preflight_candidates";
        public const string CharacterSelectPreflightCandidateQueueId = "p0_asset_queue_character_select_preflight_candidates";
        public const string BattleHudPreflightCandidateQueueId = "p0_asset_queue_battle_hud_preflight_candidates";
        public const string SkillSelectionPreflightCandidateQueueId = "p0_asset_queue_skill_selection_preflight_candidates";
        public const string RuntimeControlIconCandidateQueueId = "p0_asset_queue_runtime_control_icon_candidates";
        public const string RuntimeControlPanelCandidateQueueId = "p0_asset_queue_runtime_control_panel_candidates";
        public const string SecondaryEnemyWarningCandidateQueueId = "p0_asset_queue_secondary_enemy_warning_candidates";
        public const string RouteMapReadabilityCandidateQueueId = "p0_asset_queue_route_map_readability_candidates";
        public const string BedroomInteractionAffordanceCandidateQueueId = "p0_asset_queue_bedroom_interaction_affordance_candidates";
        public const string LoadingStartPreflightCandidateQueueId = "p0_asset_queue_loading_start_preflight_candidates";
        public const string ResultSettlementPreflightCandidateQueueId = "p0_asset_queue_result_settlement_preflight_candidates";
        public const string SettingsPausePreflightCandidateQueueId = "p0_asset_queue_settings_pause_preflight_candidates";
        public const string DreamRoutePreflightCandidateQueueId = "p0_asset_queue_dream_route_preflight_candidates";
        public const string FormalInstallDecisionQueueId = "p0_asset_queue_formal_install_decision";

        public static IReadOnlyList<P0AssetProductionQueueEntry> CreateP0Queue()
        {
            return new[]
            {
                new P0AssetProductionQueueEntry(
                    StarterCatActiveValidationQueueId,
                    10,
                    "Starter Cat Active Screenshot Validation",
                    "starter_cats",
                    P0AssetProductionQueuePhase.UnityValidation,
                    P0AssetProductionQueueState.BlockedByUnityValidation,
                    "design/development/agent_prompts/p0_asset_batch_52_starter_cat_active_validation.md",
                    "design/development/asset_candidates/starter_cats",
                    "Assets/TheCat/Art/Characters/Sprites",
                    new[]
                    {
                        "batch_49_saiban_low_drift_refinement_2026-06-15",
                        "batch_50_nephthys_strict_ai_generation_candidate_2026-06-15",
                        "batch_51_suzune_strict_ai_generation_candidate_2026-06-15"
                    },
                    new[]
                    {
                        "saiban_turnaround_colored",
                        "nephthys_turnaround_colored",
                        "suzune_turnaround_colored"
                    },
                    new[]
                    {
                        P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName,
                        P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName,
                        P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName,
                        "Unity Console has no new errors",
                        "active-cat screenshots match colored three-view turnarounds",
                        "candidate review notes explicitly approve or keep blocked"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/Characters/Sprites",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Capture active-cat screenshots and compare Batch 49/50/51 against locked colored turnarounds before any starter-cat install."),
                new P0AssetProductionQueueEntry(
                    CoreEnemyActiveValidationQueueId,
                    20,
                    "Core Enemy Active Screenshot Validation",
                    "core_enemies",
                    P0AssetProductionQueuePhase.UnityValidation,
                    P0AssetProductionQueueState.BlockedByUnityValidation,
                    "design/development/agent_prompts/p0_asset_batch_53_core_enemy_active_validation.md",
                    "design/development/asset_candidates/enemies",
                    "Assets/TheCat/Art/Enemies/Sprites",
                    new[]
                    {
                        "batch_40_black_mud_cutout_candidate_2026-06-15",
                        "batch_42_cold_light_cutout_candidate_2026-06-15",
                        "batch_44_call_tyrant_cutout_candidate_2026-06-15"
                    },
                    new[]
                    {
                        "black_mud_source_reference",
                        "cold_light_source_reference",
                        "call_tyrant_source_reference"
                    },
                    new[]
                    {
                        "active enemy screenshot set",
                        "Unity Console has no new errors",
                        "bed-pressure scale and warning readability pass",
                        "enemy review notes explicitly approve or keep blocked"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/Enemies/Sprites",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Validate the current enemy cutout candidates in Unity before deciding whether to install them."),
                new P0AssetProductionQueueEntry(
                    BedroomInteractableCandidateQueueId,
                    30,
                    "Bedroom Interactable Candidate Pack",
                    "bedroom_interactables",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_54_bedroom_interactable_candidates.md",
                    "design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15",
                    "Assets/TheCat/Art/Scenes/BedroomDream",
                    new[]
                    {
                        "batch_54_bedroom_interactable_candidates_2026-06-15",
                        "p0_asset_batch_02_gameplay_placeholders"
                    },
                    new[]
                    {
                        "bed_sleepglow_placeholder",
                        "litterbox_placeholder",
                        "feeder_placeholder"
                    },
                    new[]
                    {
                        "bedroom_interactables_batch54_manifest.csv",
                        "thecat_props_bedroom_interactables_batch54_review_sheet.png",
                        "bedroom_interactables_batch54_candidate_review.md",
                        "validate_bedroom_interactable_candidates.ps1 passes",
                        "bed, litter box, and feeder candidates stay non-cat and outside Assets",
                        "transparent PNGs include manifest, review sheet, review note, process note, and agent prompt",
                        "no Unity .meta files are created in candidate folders"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/Scenes/BedroomDream",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 54 candidate pack is complete; keep it review-only until Unity runtime scale, Console, and scene/prefab checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    StarterSkillVfxCandidateQueueId,
                    40,
                    "Starter Skill VFX Installed Unity Validation",
                    "starter_skill_vfx",
                    P0AssetProductionQueuePhase.UnityValidation,
                    P0AssetProductionQueueState.BlockedByUnityValidation,
                    "design/development/agent_prompts/p0_asset_batch_61_starter_skill_vfx_install.md",
                    "design/development/asset_candidates/vfx/starter_skills/batch_61_starter_skill_vfx_install_2026-06-15",
                    "Assets/TheCat/Art/VFX",
                    new[]
                    {
                        "batch_55_starter_skill_vfx_candidates_2026-06-15",
                        "batch_61_starter_skill_vfx_install_2026-06-15",
                        "p0_asset_batch_09_battle_feedback_vfx",
                        "p0_asset_batch_16_authority_blessing_seals"
                    },
                    new[]
                    {
                        "saiban_turnaround_colored",
                        "nephthys_turnaround_colored",
                        "suzune_turnaround_colored"
                    },
                    new[]
                    {
                        "starter_skill_vfx_batch61_install_manifest.csv",
                        "thecat_vfx_starter_skills_batch61_install_review_sheet.png",
                        "starter_skill_vfx_batch61_install_review.md",
                        "validate_starter_skill_vfx_install.ps1 passes",
                        "installed symbolic VFX avoids cat-body redraws and starter-cat body replacements",
                        "installed PNGs include Unity .png.meta files with TheCatP0ImportSettings:v1",
                        "Unity skill timing, feedback scale, screenshot, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/VFX",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 61 is installed from Batch 55 candidates; validate Unity skill timing, feedback scale, Console, screenshot, and scene/prefab references before marking complete."),
                new P0AssetProductionQueueEntry(
                    SkillHudFeedbackCandidateQueueId,
                    50,
                    "Skill HUD Feedback Installed Unity Validation",
                    "skill_hud_feedback",
                    P0AssetProductionQueuePhase.UnityValidation,
                    P0AssetProductionQueueState.BlockedByUnityValidation,
                    "design/development/agent_prompts/p0_asset_batch_60_skill_hud_feedback_install.md",
                    "design/development/asset_candidates/ui/skill_hud/batch_60_skill_hud_feedback_install_2026-06-15",
                    "Assets/TheCat/Art/UI",
                    new[]
                    {
                        "batch_57_skill_hud_feedback_candidates_2026-06-15",
                        "batch_60_skill_hud_feedback_install_2026-06-15",
                        "p0_asset_batch_08_ui_shell",
                        "p0_asset_batch_09_battle_feedback_vfx",
                        "p0_asset_batch_27_core_gauge_bars"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "non_cat_status_and_feedback_references"
                    },
                    new[]
                    {
                        "skill_hud_feedback_batch60_install_manifest.csv",
                        "thecat_ui_skill_hud_feedback_batch60_install_review_sheet.png",
                        "skill_hud_feedback_batch60_install_review.md",
                        "validate_skill_hud_feedback_install.ps1 passes",
                        "installed assets are non-cat UI/VFX and avoid starter-cat body redraws and colored-turnaround crops",
                        "installed PNGs include Unity .png.meta files with TheCatP0ImportSettings:v1",
                        "Unity HUD screenshot and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI",
                        "Assets/TheCat/Art/VFX",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 60 is installed from Batch 57 candidates; validate Unity HUD scale, timing, Console, screenshot, and scene/prefab references before marking complete."),
                new P0AssetProductionQueueEntry(
                    CatRoomPreflightCandidateQueueId,
                    51,
                    "Cat Room Preflight Candidate Pack",
                    "ui_cat_room",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_90_cat_room_preflight.md",
                    "design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25",
                    "Assets/TheCat/Art/UI/CatRoom",
                    new[]
                    {
                        "batch_90_cat_room_preflight_2026-06-25",
                        "batch_54_bedroom_interactable_candidates_2026-06-15",
                        "batch_67_bedroom_interaction_affordance_candidates_2026-06-15",
                        "p0_cat_room_surface",
                        "p0_asset_batch_08_ui_shell"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "bedroom_interactable_source_references",
                        "interaction_affordance_candidates",
                        "cat_room_preflight_candidates"
                    },
                    new[]
                    {
                        "thecat_ui_cat_room_batch90_manifest.csv",
                        "thecat_ui_cat_room_batch90_review_sheet_v001.png",
                        "thecat_ui_cat_room_batch90_candidate_review.md",
                        "validate_ui_cat_room_preflight_candidates.ps1 passes",
                        "6 transparent cat-room sprites and 4 cat-room mockups stay outside Assets",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity cat-room screenshots, bed/feeder/litter/dream entrance interaction proof, hover/disabled/range state readability, click targets, import settings, binding, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI/CatRoom",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 90 cat-room preflight is complete; keep it review-only until Unity-rendered cat-room screenshots, bed/feeder/litter/dream entrance interactions, hover/disabled/range states, click targets, import settings, binding, and Console checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    CharacterSelectPreflightCandidateQueueId,
                    52,
                    "Character Select Preflight Candidate Pack",
                    "ui_character_select",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_88_character_select_preflight.md",
                    "design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25",
                    "Assets/TheCat/Art/UI/CharacterSelect",
                    new[]
                    {
                        "batch_88_character_select_preflight_2026-06-25",
                        "batch_58_starter_cat_hud_avatar_install_2026-06-15",
                        "batch_80_starter_skill_icon_motifs_2026-06-25",
                        "p0_main_menu_character_select_surface",
                        "p0_asset_batch_08_ui_shell"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "hud_avatar_source_locks",
                        "starter_skill_icon_motif_candidates",
                        "character_select_preflight_candidates"
                    },
                    new[]
                    {
                        "thecat_ui_character_select_batch88_manifest.csv",
                        "thecat_ui_character_select_batch88_review_sheet_v001.png",
                        "thecat_ui_character_select_batch88_candidate_review.md",
                        "validate_ui_character_select_preflight_candidates.ps1 passes",
                        "6 transparent character-select sprites and 4 character-select mockups stay outside Assets",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity character-select screenshots, source-lock avatar consistency, text replacement, selected/idle state readability, click targets, import settings, binding, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI/CharacterSelect",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 88 character-select preflight is complete; keep it review-only until Unity-rendered character-select screenshots, source-lock avatar consistency, text and number replacement, selected/idle states, click targets, import settings, binding, and Console checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    BattleHudPreflightCandidateQueueId,
                    55,
                    "Battle HUD Preflight Candidate Pack",
                    "ui_battle_hud",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_87_battle_hud_preflight.md",
                    "design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25",
                    "Assets/TheCat/Art/UI/BattleHUD",
                    new[]
                    {
                        "batch_87_battle_hud_preflight_2026-06-25",
                        "batch_60_skill_hud_feedback_install_2026-06-15",
                        "batch_62_runtime_control_icon_candidates_2026-06-15",
                        "batch_63_runtime_control_panel_candidates_2026-06-15",
                        "batch_80_starter_skill_icon_motifs_2026-06-25",
                        "batch_81_skill_slot_frame_candidates_2026-06-25",
                        "p0_battle_hud_surface",
                        "p0_asset_batch_27_core_gauge_bars",
                        "p0_asset_batch_08_ui_shell"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "non_cat_status_and_feedback_references",
                        "core_gauge_bars",
                        "skill_hud_feedback_assets",
                        "runtime_control_icon_candidates",
                        "starter_skill_icon_motif_candidates",
                        "skill_slot_frame_candidates",
                        "battle_hud_preflight_candidates"
                    },
                    new[]
                    {
                        "thecat_ui_battle_hud_batch87_manifest.csv",
                        "thecat_ui_battle_hud_batch87_review_sheet_v001.png",
                        "thecat_ui_battle_hud_batch87_candidate_review.md",
                        "validate_ui_battle_hud_preflight_candidates.ps1 passes",
                        "6 transparent battle HUD sprites and 4 battle HUD mockups stay outside Assets",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity battle HUD screenshots, gauge/text replacement, skill state readability, 1024x768 four-gauge proof, enemy/telegraph occlusion, import settings, click targets, binding, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI/BattleHUD",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 87 battle HUD preflight is complete; keep it review-only until Unity-rendered battle HUD screenshots, four-gauge proof, text and number replacement, skill states, click targets, import settings, binding, and Console checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    SkillSelectionPreflightCandidateQueueId,
                    56,
                    "Skill Selection Preflight Candidate Pack",
                    "ui_skill_selection",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_89_skill_selection_preflight.md",
                    "design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25",
                    "Assets/TheCat/Art/UI/SkillSelection",
                    new[]
                    {
                        "batch_89_skill_selection_preflight_2026-06-25",
                        "batch_80_starter_skill_icon_motifs_2026-06-25",
                        "batch_81_skill_slot_frame_candidates_2026-06-25",
                        "batch_82_common_ui_state_candidates_2026-06-25",
                        "p0_skill_selection_surface",
                        "p0_asset_batch_08_ui_shell"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "starter_skill_icon_motif_candidates",
                        "skill_slot_frame_candidates",
                        "common_ui_state_candidates",
                        "skill_selection_preflight_candidates"
                    },
                    new[]
                    {
                        "thecat_ui_skill_selection_batch89_manifest.csv",
                        "thecat_ui_skill_selection_batch89_review_sheet_v001.png",
                        "thecat_ui_skill_selection_batch89_candidate_review.md",
                        "validate_ui_skill_selection_preflight_candidates.ps1 passes",
                        "8 transparent skill-selection sprites and 4 skill-selection mockups stay outside Assets",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity skill-selection screenshots, selected/ready/disabled/locked states, cooldown/low-resource/no-target semantics, text replacement, click targets, import settings, binding, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI/SkillSelection",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 89 skill-selection preflight is complete; keep it review-only until Unity-rendered skill-selection screenshots, selected/ready/disabled/locked states, cooldown and low-resource semantics, text and number replacement, click targets, import settings, binding, and Console checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    RuntimeControlIconCandidateQueueId,
                    60,
                    "Runtime Control Icon Candidate Pack",
                    "runtime_controls",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_62_runtime_control_icon_candidates.md",
                    "design/development/asset_candidates/ui/runtime_controls/batch_62_runtime_control_icon_candidates_2026-06-15",
                    "Assets/TheCat/Art/UI/Icons",
                    new[]
                    {
                        "batch_62_runtime_control_icon_candidates_2026-06-15",
                        "p0_runtime_settings_surface",
                        "p0_keyboard_input_map",
                        "p0_asset_batch_08_ui_shell"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "runtime_settings_controls"
                    },
                    new[]
                    {
                        "runtime_control_icons_batch62_manifest.csv",
                        "thecat_ui_runtime_controls_batch62_review_sheet.png",
                        "runtime_control_icons_batch62_candidate_review.md",
                        "validate_runtime_control_icon_candidates.ps1 passes",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity battle HUD readability and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI/Icons",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 62 candidate pack is complete; keep it review-only until Unity HUD scale, Console, screenshot, and pause/speed/restart affordance checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    RuntimeControlPanelCandidateQueueId,
                    70,
                    "Runtime Control Panel Candidate Pack",
                    "runtime_controls",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_63_runtime_control_panel_candidates.md",
                    "design/development/asset_candidates/ui/runtime_controls/batch_63_runtime_control_panel_candidates_2026-06-15",
                    "Assets/TheCat/Art/UI",
                    new[]
                    {
                        "batch_63_runtime_control_panel_candidates_2026-06-15",
                        "batch_62_runtime_control_icon_candidates_2026-06-15",
                        "p0_runtime_settings_surface",
                        "p0_keyboard_input_map",
                        "p0_asset_batch_08_ui_shell"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "runtime_settings_controls",
                        "runtime_control_icon_candidates"
                    },
                    new[]
                    {
                        "runtime_control_panels_batch63_manifest.csv",
                        "thecat_ui_runtime_control_panels_batch63_review_sheet.png",
                        "runtime_control_panels_batch63_candidate_review.md",
                        "validate_runtime_control_panel_candidates.ps1 passes",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity pause overlay, speed selector, restart plate, keyboard hint, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 63 candidate pack is complete; keep it review-only until Unity HUD scale, Console, screenshot, and pause/speed/restart panel checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    SecondaryEnemyWarningCandidateQueueId,
                    80,
                    "Secondary Enemy Warning Candidate Pack",
                    "secondary_enemy_warning_vfx",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_64_secondary_enemy_warning_candidates.md",
                    "design/development/asset_candidates/enemies/secondary_warning_vfx/batch_64_secondary_enemy_warning_candidates_2026-06-15",
                    "Assets/TheCat/Art/Enemies/VFX",
                    new[]
                    {
                        "batch_64_secondary_enemy_warning_candidates_2026-06-15",
                        "p0_asset_batch_10_enemy_warning_vfx",
                        "dream_rail_train_warning_design",
                        "red_eye_alarm_warning_design",
                        "unread_red_dot_warning_design",
                        "falling_dream_teddy_warning_design"
                    },
                    new[]
                    {
                        "dream_rail_train_concept",
                        "red_eye_alarm_concept",
                        "falling_dream_teddy_concept",
                        "batch_10_enemy_warning_vfx_language"
                    },
                    new[]
                    {
                        "secondary_enemy_warning_batch64_manifest.csv",
                        "thecat_vfx_secondary_enemy_warnings_batch64_review_sheet.png",
                        "secondary_enemy_warning_batch64_candidate_review.md",
                        "validate_secondary_enemy_warning_candidates.ps1 passes",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity enemy warning readability, Console, screenshot, and future prefab binding validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/Enemies/VFX",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 64 candidate pack is complete; keep it review-only until Unity warning readability, Console, screenshot, and future secondary-enemy prefab checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    RouteMapReadabilityCandidateQueueId,
                    85,
                    "Route Map Readability Candidate Pack",
                    "route_map_readability",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_65_route_map_readability_candidates.md",
                    "design/development/asset_candidates/ui/route_map/batch_65_route_map_readability_candidates_2026-06-15",
                    "Assets/TheCat/Art/UI",
                    new[]
                    {
                        "batch_65_route_map_readability_candidates_2026-06-15",
                        "p0_route_map_surface",
                        "p0_route_node_icons",
                        "p0_asset_batch_08_ui_shell"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "route_map_readability_candidates",
                        "route_node_icon_baseline"
                    },
                    new[]
                    {
                        "route_map_readability_batch65_manifest.csv",
                        "thecat_ui_route_map_readability_batch65_review_sheet.png",
                        "route_map_readability_batch65_candidate_review.md",
                        "validate_route_map_readability_candidates.ps1 passes",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity route-map scale, selected/current contrast, path readability, Boss pressure readability, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 65 candidate pack is complete; keep it review-only until Unity route-map readability, Console, screenshot, and scene/prefab checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    BedroomInteractionAffordanceCandidateQueueId,
                    87,
                    "Bedroom Interaction Affordance Candidate Pack",
                    "bedroom_interaction_affordances",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_67_bedroom_interaction_affordance_candidates.md",
                    "design/development/asset_candidates/ui/bedroom_interaction_affordances/batch_67_bedroom_interaction_affordance_candidates_2026-06-15",
                    "Assets/TheCat/Art/UI",
                    new[]
                    {
                        "batch_67_bedroom_interaction_affordance_candidates_2026-06-15",
                        "batch_54_bedroom_interactable_candidates_2026-06-15",
                        "p0_interactable_presenter",
                        "p0_core_values"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "bedroom_interactable_source_references",
                        "interaction_affordance_candidates"
                    },
                    new[]
                    {
                        "bedroom_interaction_affordance_batch67_manifest.csv",
                        "thecat_ui_bedroom_interaction_affordance_batch67_review_sheet.png",
                        "bedroom_interaction_affordance_batch67_candidate_review.md",
                        "validate_bedroom_interaction_affordance_candidates.ps1 passes",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity bed, litter box, feeder, blocked interaction, range readability, input timing, scene/prefab, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 67 candidate pack is complete; keep it review-only until Unity interaction timing, Console, screenshot, and scene/prefab checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    LoadingStartPreflightCandidateQueueId,
                    88,
                    "Loading Start Preflight Candidate Pack",
                    "ui_loading_start",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_83_loading_start_preflight.md",
                    "design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25",
                    "Assets/TheCat/Art/UI/LoadingStart",
                    new[]
                    {
                        "batch_83_loading_start_preflight_2026-06-25",
                        "p0_asset_batch_08_ui_shell",
                        "p0_loading_start_surface"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "loading_start_preflight_candidates"
                    },
                    new[]
                    {
                        "thecat_ui_loading_start_batch83_manifest.csv",
                        "thecat_ui_loading_start_batch83_review_sheet_v001.png",
                        "thecat_ui_loading_start_batch83_candidate_review.md",
                        "validate_ui_loading_start_preflight_candidates.ps1 passes",
                        "4 transparent loading/start sprites and 4 resolution mockups stay outside Assets",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity loading/start screenshots, spinner interpretation, placeholder state replacement, 1024x768 crowding, import settings, binding, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI/LoadingStart",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 83 loading/start preflight is complete; keep it review-only until Unity-rendered loading/start screenshots, import settings, binding, placeholder-state replacement, and Console checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    ResultSettlementPreflightCandidateQueueId,
                    89,
                    "Result Settlement Preflight Candidate Pack",
                    "ui_result_settlement",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_84_result_settlement_preflight.md",
                    "design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25",
                    "Assets/TheCat/Art/UI/ResultSettlement",
                    new[]
                    {
                        "batch_84_result_settlement_preflight_2026-06-25",
                        "p0_asset_batch_08_ui_shell",
                        "p0_result_settlement_surface"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "result_settlement_preflight_candidates"
                    },
                    new[]
                    {
                        "thecat_ui_result_settlement_batch84_manifest.csv",
                        "thecat_ui_result_settlement_batch84_review_sheet_v001.png",
                        "thecat_ui_result_settlement_batch84_candidate_review.md",
                        "validate_ui_result_settlement_preflight_candidates.ps1 passes",
                        "7 transparent result/settlement sprites and 4 result/settlement mockups stay outside Assets",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity victory/defeat/settlement screenshots, text replacement, 1024x768 crowding, import settings, binding, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI/ResultSettlement",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 84 result/settlement preflight is complete; keep it review-only until Unity-rendered victory, defeat, run-cleared, and run-failed screenshots, text replacement, 1024x768 crowding, import settings, binding, and Console checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    SettingsPausePreflightCandidateQueueId,
                    90,
                    "Settings Pause Preflight Candidate Pack",
                    "ui_settings_pause",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_85_settings_pause_preflight.md",
                    "design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25",
                    "Assets/TheCat/Art/UI/SettingsPause",
                    new[]
                    {
                        "batch_85_settings_pause_preflight_2026-06-25",
                        "batch_78_settings_control_candidates_2026-06-24",
                        "batch_79_system_icon_candidates_2026-06-24",
                        "p0_settings_pause_surface",
                        "p0_asset_batch_08_ui_shell"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "settings_control_candidates",
                        "system_icon_candidates",
                        "settings_pause_preflight_candidates"
                    },
                    new[]
                    {
                        "thecat_ui_settings_pause_batch85_manifest.csv",
                        "thecat_ui_settings_pause_batch85_review_sheet_v001.png",
                        "thecat_ui_settings_pause_batch85_candidate_review.md",
                        "validate_ui_settings_pause_preflight_candidates.ps1 passes",
                        "6 transparent settings/pause sprites and 4 settings/pause mockups stay outside Assets",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity settings/pause screenshots, text replacement, 1024x768 key-hint semantics, import settings, click targets, binding, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI/SettingsPause",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 85 settings/pause preflight is complete; keep it review-only until Unity-rendered settings and pause screenshots, text replacement, 1024x768 key-hint semantics, import settings, click targets, binding, and Console checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    DreamRoutePreflightCandidateQueueId,
                    91,
                    "Dream Route Preflight Candidate Pack",
                    "ui_dream_route",
                    P0AssetProductionQueuePhase.CodexCandidateProduction,
                    P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview,
                    "design/development/agent_prompts/p0_asset_batch_86_dream_route_preflight.md",
                    "design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25",
                    "Assets/TheCat/Art/UI/DreamRoute",
                    new[]
                    {
                        "batch_86_dream_route_preflight_2026-06-25",
                        "batch_65_route_map_readability_candidates_2026-06-15",
                        "p0_route_map_surface",
                        "p0_dream_entry_surface",
                        "p0_asset_batch_08_ui_shell"
                    },
                    new[]
                    {
                        "non_cat_ui_shell_references",
                        "route_map_readability_candidates",
                        "route_node_icon_baseline",
                        "dream_route_preflight_candidates"
                    },
                    new[]
                    {
                        "thecat_ui_dream_route_batch86_manifest.csv",
                        "thecat_ui_dream_route_batch86_review_sheet_v001.png",
                        "thecat_ui_dream_route_batch86_candidate_review.md",
                        "validate_ui_dream_route_preflight_candidates.ps1 passes",
                        "6 transparent dream-route sprites and 4 dream-route mockups stay outside Assets",
                        "candidate PNGs stay outside Assets and have no Unity meta files",
                        "Unity dream-entry/route-map screenshots, text replacement, node/path semantics, 1024x768 route-card crowding, boss gate dominance, import settings, click targets, binding, and Console validation remain pending"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art/UI/DreamRoute",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Batch 86 dream-route preflight is complete; keep it review-only until Unity-rendered dream-entry and route-map screenshots, text replacement, node/path semantics, route-card click targets, import settings, binding, and Console checks approve a formal install."),
                new P0AssetProductionQueueEntry(
                    FormalInstallDecisionQueueId,
                    100,
                    "Formal Install Decision Packet",
                    "approved_candidates",
                    P0AssetProductionQueuePhase.FormalUnityInstall,
                    P0AssetProductionQueueState.BlockedByUnityValidation,
                    "design/development/agent_prompts/p0_asset_batch_56_formal_install_decision_packet.md",
                    "design/development/asset_candidates/formal_install_decisions",
                    "Assets/TheCat/Art",
                    new[]
                    {
                        "batch_49_saiban_low_drift_refinement_2026-06-15",
                        "batch_50_nephthys_strict_ai_generation_candidate_2026-06-15",
                        "batch_51_suzune_strict_ai_generation_candidate_2026-06-15",
                        "batch_40_black_mud_cutout_candidate_2026-06-15",
                        "batch_42_cold_light_cutout_candidate_2026-06-15",
                        "batch_44_call_tyrant_cutout_candidate_2026-06-15",
                        "batch_54_bedroom_interactable_candidates_2026-06-15",
                        "batch_55_starter_skill_vfx_candidates_2026-06-15",
                        "batch_61_starter_skill_vfx_install_2026-06-15",
                        "batch_57_skill_hud_feedback_candidates_2026-06-15",
                        "batch_60_skill_hud_feedback_install_2026-06-15",
                        "batch_62_runtime_control_icon_candidates_2026-06-15",
                        "batch_63_runtime_control_panel_candidates_2026-06-15",
                        "batch_64_secondary_enemy_warning_candidates_2026-06-15",
                        "batch_65_route_map_readability_candidates_2026-06-15",
                        "batch_67_bedroom_interaction_affordance_candidates_2026-06-15",
                        "batch_83_loading_start_preflight_2026-06-25",
                        "batch_84_result_settlement_preflight_2026-06-25",
                        "batch_85_settings_pause_preflight_2026-06-25",
                        "batch_86_dream_route_preflight_2026-06-25",
                        "batch_88_character_select_preflight_2026-06-25",
                        "batch_87_battle_hud_preflight_2026-06-25",
                        "batch_89_skill_selection_preflight_2026-06-25",
                        "batch_90_cat_room_preflight_2026-06-25"
                    },
                    new[]
                    {
                        "starter_cat_source_locks",
                        "core_enemy_source_references",
                        "bedroom_interactable_source_references",
                        "starter_skill_vfx_source_locks",
                        "runtime_control_icon_candidates",
                        "runtime_control_panel_candidates",
                        "secondary_enemy_warning_candidates",
                        "route_map_readability_candidates",
                        "interaction_affordance_candidates",
                        "loading_start_preflight_candidates",
                        "result_settlement_preflight_candidates",
                        "settings_pause_preflight_candidates",
                        "dream_route_preflight_candidates",
                        "character_select_preflight_candidates",
                        "battle_hud_preflight_candidates",
                        "skill_selection_preflight_candidates",
                        "cat_room_preflight_candidates"
                    },
                    new[]
                    {
                        "explicit human review approval",
                        "Unity Console has no new errors",
                        "runtime screenshot comparison passed",
                        "Batch 54 prop runtime scale and pathing readability passed",
                        "Batch 61 starter skill VFX runtime scale, skill timing, and readability passed",
                        "Batch 60 skill HUD feedback timing, HUD scale, and readability passed",
                        "Batch 62 runtime control icon readability and pause/speed/restart affordance passed",
                        "Batch 63 runtime control panel readability and pause/speed/restart affordance passed",
                        "Batch 64 secondary enemy warning readability and future enemy attack affordance passed",
                        "Batch 65 route map current/selected/path/Boss readability passed",
                        "Batch 67 bedroom interaction affordance timing, blocked-state, range, and interactable readability passed",
                        "Batch 83 loading/start spinner, placeholder-state replacement, 1024x768 crowding, import settings, and binding passed",
                        "Batch 84 result/settlement text replacement, 1024x768 crowding, import settings, and binding passed",
                        "Batch 85 settings/pause text replacement, 1024x768 key-hint semantics, click-target scale, import settings, and binding passed",
                        "Batch 86 dream-route text replacement, route node/path semantics, 1024x768 route-card crowding, boss gate scale, click-target scale, import settings, and binding passed",
                        "Batch 88 character-select source-lock avatar consistency, text replacement, selected/idle state readability, click-target scale, import settings, and binding passed",
                        "Batch 87 battle HUD four-gauge layout, text replacement, skill states, enemy/telegraph occlusion, click-target scale, import settings, and binding passed",
                        "Batch 89 skill-selection selected/ready/disabled/locked state readability, cooldown/low-resource/no-target semantics, text replacement, click-target scale, import settings, and binding passed",
                        "Batch 90 cat-room bed/feeder/litter/dream entrance interactions, hover/disabled/range states, text replacement, click-target scale, import settings, and binding passed",
                        "manifest and runtime binding diffs are scoped"
                    },
                    new[]
                    {
                        "Assets/TheCat/Art",
                        "Assets/TheCat/Prefabs",
                        "Assets/TheCat/Scenes"
                    },
                    "Create the formal install packet only after active screenshot review approves specific candidates.")
            };
        }
    }
}
