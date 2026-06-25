using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public static class P0AssetGenerationBatchCatalog
    {
        public const string StyleAnchorBatchId = "p0_asset_batch_01_style_anchors";
        public const string GameplayPlaceholderBatchId = "p0_asset_batch_02_gameplay_placeholders";
        public const string BossReadinessBatchId = "p0_asset_batch_03_boss_readiness";
        public const string RouteNodeIconBatchId = "p0_asset_batch_06_route_node_icons";
        public const string UiShellBatchId = "p0_asset_batch_08_ui_shell";
        public const string BattleFeedbackVfxBatchId = "p0_asset_batch_09_battle_feedback_vfx";
        public const string EnemyWarningVfxBatchId = "p0_asset_batch_10_enemy_warning_vfx";
        public const string EnemyAnimationFramesheetBatchId = "p0_asset_batch_11_enemy_animation_framesheets";
        public const string RouteChoiceIconBatchId = "p0_asset_batch_12_route_choice_icons";
        public const string RouteRewardCardFrameBatchId = "p0_asset_batch_13_route_reward_card_frames";
        public const string StatusCompactIconBatchId = "p0_asset_batch_14_status_compact_icons";
        public const string RouteRewardDetailBadgeBatchId = "p0_asset_batch_15_route_reward_detail_badges";
        public const string AuthorityBlessingSealBatchId = "p0_asset_batch_16_authority_blessing_seals";
        public const string NonBattleNodeSummaryBannerBatchId = "p0_asset_batch_19_nonbattle_node_summary_banners";
        public const string ShopItemCardBatchId = "p0_asset_batch_20_shop_item_cards";
        public const string DreamEventChoiceCardBatchId = "p0_asset_batch_21_dream_event_choice_cards";
        public const string RestNestRecoveryCardBatchId = "p0_asset_batch_22_rest_nest_recovery_card";
        public const string PartnerChoiceCardBatchId = "p0_asset_batch_23_partner_choice_cards";
        public const string BlessingChoiceCardBatchId = "p0_asset_batch_24_blessing_choice_cards";
        public const string ResultSettlementBannerBatchId = "p0_asset_batch_25_result_settlement_banners";
        public const string CoreGaugeBarBatchId = "p0_asset_batch_27_core_gauge_bars";
        public const string StarterCatHudAvatarBatchId = "p0_asset_batch_58_starter_cat_hud_avatars";
        public const string SkillHudFeedbackBatchId = "p0_asset_batch_60_skill_hud_feedback";
        public const string StarterSkillVfxBatchId = "p0_asset_batch_61_starter_skill_vfx";
        public const string SaibanReferenceAtlasBatchId = "p0_asset_batch_71_saiban_reference_atlas";
        public const string NephthysReferenceAtlasBatchId = "p0_asset_batch_72_nephthys_reference_atlas";
        public const string SuzuneReferenceAtlasBatchId = "p0_asset_batch_73_suzune_reference_atlas";

        public static IReadOnlyList<P0AssetGenerationBatchDefinition> CreateP0Batches()
        {
            return new[]
            {
                new P0AssetGenerationBatchDefinition(
                    StyleAnchorBatchId,
                    "P0 Style Anchors",
                    "design/development/agent_prompts/p0_asset_batch_01_style_anchors.md",
                    new[]
                    {
                        "thecat_style_bedroomdream_anchor_1920x1080_v001",
                        "thecat_style_startercats_lineup_2048_v001",
                        "thecat_style_blackmud_concept_2048_v001",
                        "thecat_style_status_icons_5x64_v001"
                    },
                    Empty()),
                new P0AssetGenerationBatchDefinition(
                    GameplayPlaceholderBatchId,
                    "P0 Gameplay Placeholders",
                    "design/development/agent_prompts/p0_asset_batch_02_gameplay_placeholders.md",
                    new[]
                    {
                        "thecat_bg_bedroomdream_battle_1920x1080_v001",
                        "thecat_cat_saiban_combat_sprite_512_v001",
                        "thecat_cat_nephthys_combat_sprite_512_v001",
                        "thecat_cat_suzune_combat_sprite_512_v001",
                        "thecat_enemy_blackmud_combat_sprite_512_v001",
                        "thecat_enemy_coldlight_combat_sprite_512_v001",
                        "thecat_prop_bed_sleepglow_sprite_512_v001",
                        "thecat_prop_litterbox_sprite_256_v001",
                        "thecat_prop_feeder_sprite_256_v001",
                        "thecat_ui_core_sleep_icon_64_v001",
                        "thecat_ui_core_hp_icon_64_v001",
                        "thecat_ui_core_poop_icon_64_v001",
                        "thecat_ui_core_hunger_icon_64_v001",
                        "thecat_ui_status_sleep_stable_64_v001",
                        "thecat_ui_status_slow_64_v001",
                        "thecat_ui_status_knockback_64_v001",
                        "thecat_ui_status_mark_64_v001",
                        "thecat_ui_status_shield_64_v001"
                    },
                    new[]
                    {
                        "thecat_style_bedroomdream_anchor_1920x1080_v001",
                        "thecat_style_startercats_lineup_2048_v001",
                        "thecat_style_blackmud_concept_2048_v001",
                        "thecat_style_status_icons_5x64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    BossReadinessBatchId,
                    "P0 Boss Readiness",
                    "design/development/agent_prompts/p0_asset_batch_03_boss_readiness.md",
                    new[]
                    {
                        "thecat_enemy_calltyrant_concept_2048_v001",
                        "thecat_vfx_calltyrant_warning_512_v001",
                        "thecat_ui_route_bossnode_icon_128_v001"
                    },
                    Empty()),
                new P0AssetGenerationBatchDefinition(
                    RouteNodeIconBatchId,
                    "P0 Route Node Icons",
                    "design/development/agent_prompts/p0_asset_batch_06_route_node_icons.md",
                    new[]
                    {
                        "thecat_ui_route_defense_icon_128_v001",
                        "thecat_ui_route_elite_icon_128_v001",
                        "thecat_ui_route_partner_icon_128_v001",
                        "thecat_ui_route_shop_icon_128_v001",
                        "thecat_ui_route_dreamevent_icon_128_v001",
                        "thecat_ui_route_blessing_icon_128_v001",
                        "thecat_ui_route_restnest_icon_128_v001"
                    },
                    Empty()),
                new P0AssetGenerationBatchDefinition(
                    UiShellBatchId,
                    "P0 UI Shell",
                    "design/development/agent_prompts/p0_asset_batch_08_ui_shell.md",
                    new[]
                    {
                        "thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001",
                        "thecat_ui_title_logo_512x256_v001",
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_button_primary_384x96_v001",
                        "thecat_ui_reward_fishtreat_icon_128_v001",
                        "thecat_ui_reward_dreamshard_icon_128_v001"
                    },
                    new[]
                    {
                        "thecat_style_bedroomdream_anchor_1920x1080_v001",
                        "thecat_style_status_icons_5x64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    BattleFeedbackVfxBatchId,
                    "P0 Battle Feedback VFX",
                    "design/development/agent_prompts/p0_asset_batch_09_battle_feedback_vfx.md",
                    new[]
                    {
                        "thecat_vfx_hit_spark_256_v001",
                        "thecat_vfx_bed_shield_pulse_256_v001",
                        "thecat_vfx_sleep_stable_wave_256_v001",
                        "thecat_vfx_litter_cleanse_256_v001",
                        "thecat_vfx_feeder_kibble_256_v001",
                        "thecat_vfx_enemy_mark_ring_256_v001"
                    },
                    new[]
                    {
                        "thecat_style_bedroomdream_anchor_1920x1080_v001",
                        "thecat_style_status_icons_5x64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    EnemyWarningVfxBatchId,
                    "P0 Enemy Warning VFX",
                    "design/development/agent_prompts/p0_asset_batch_10_enemy_warning_vfx.md",
                    new[]
                    {
                        "thecat_vfx_blackmud_bed_claw_256_v001",
                        "thecat_vfx_coldlight_beam_warning_256_v001",
                        "thecat_vfx_calltyrant_app_throw_256_v001",
                        "thecat_vfx_calltyrant_summon_portal_256_v001"
                    },
                    new[]
                    {
                        "thecat_enemy_blackmud_combat_sprite_512_v001",
                        "thecat_enemy_coldlight_combat_sprite_512_v001",
                        "thecat_enemy_calltyrant_concept_2048_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    EnemyAnimationFramesheetBatchId,
                    "P0 Enemy Animation Framesheets",
                    "design/development/agent_prompts/p0_asset_batch_11_enemy_animation_framesheets.md",
                    new[]
                    {
                        "thecat_enemy_blackmud_move_framesheet_4x256_v001",
                        "thecat_enemy_coldlight_cast_framesheet_4x256_v001",
                        "thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001"
                    },
                    new[]
                    {
                        "thecat_enemy_blackmud_combat_sprite_512_v001",
                        "thecat_enemy_coldlight_combat_sprite_512_v001",
                        "thecat_enemy_calltyrant_concept_2048_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    RouteChoiceIconBatchId,
                    "P0 Route Choice Icons",
                    "design/development/agent_prompts/p0_asset_batch_12_route_choice_icons.md",
                    new[]
                    {
                        "thecat_ui_choice_partner_recruit_icon_128_v001",
                        "thecat_ui_choice_purchase_supply_icon_128_v001",
                        "thecat_ui_choice_authority_blessing_icon_128_v001",
                        "thecat_ui_choice_authority_upgrade_icon_128_v001",
                        "thecat_ui_choice_rest_supply_icon_128_v001",
                        "thecat_ui_choice_dreamevent_modifier_icon_128_v001"
                    },
                    new[]
                    {
                        "thecat_style_status_icons_5x64_v001",
                        "thecat_ui_reward_fishtreat_icon_128_v001",
                        "thecat_ui_reward_dreamshard_icon_128_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    RouteRewardCardFrameBatchId,
                    "P0 Route Reward Card Frames",
                    "design/development/agent_prompts/p0_asset_batch_13_route_reward_card_frames.md",
                    new[]
                    {
                        "thecat_ui_routecard_partner_frame_512x256_v001",
                        "thecat_ui_routecard_shop_frame_512x256_v001",
                        "thecat_ui_routecard_blessing_frame_512x256_v001",
                        "thecat_ui_routecard_dreamevent_frame_512x256_v001",
                        "thecat_ui_routecard_restnest_frame_512x256_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_choice_partner_recruit_icon_128_v001",
                        "thecat_ui_choice_purchase_supply_icon_128_v001",
                        "thecat_ui_choice_authority_blessing_icon_128_v001",
                        "thecat_ui_choice_dreamevent_modifier_icon_128_v001",
                        "thecat_ui_choice_rest_supply_icon_128_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    StatusCompactIconBatchId,
                    "P0 Status Compact Icons",
                    "design/development/agent_prompts/p0_asset_batch_14_status_compact_icons.md",
                    new[]
                    {
                        "thecat_ui_status_sleep_stable_32_v001",
                        "thecat_ui_status_slow_32_v001",
                        "thecat_ui_status_knockback_32_v001",
                        "thecat_ui_status_mark_32_v001",
                        "thecat_ui_status_shield_32_v001"
                    },
                    new[]
                    {
                        "thecat_ui_status_sleep_stable_64_v001",
                        "thecat_ui_status_slow_64_v001",
                        "thecat_ui_status_knockback_64_v001",
                        "thecat_ui_status_mark_64_v001",
                        "thecat_ui_status_shield_64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    RouteRewardDetailBadgeBatchId,
                    "P0 Route Reward Detail Badges",
                    "design/development/agent_prompts/p0_asset_batch_15_route_reward_detail_badges.md",
                    new[]
                    {
                        "thecat_ui_reward_detail_gain_badge_192x64_v001",
                        "thecat_ui_reward_detail_cost_badge_192x64_v001",
                        "thecat_ui_reward_detail_recovery_badge_192x64_v001",
                        "thecat_ui_reward_detail_risk_badge_192x64_v001",
                        "thecat_ui_reward_detail_upgrade_badge_192x64_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_reward_fishtreat_icon_128_v001",
                        "thecat_ui_reward_dreamshard_icon_128_v001",
                        "thecat_ui_choice_purchase_supply_icon_128_v001",
                        "thecat_ui_choice_rest_supply_icon_128_v001",
                        "thecat_ui_choice_dreamevent_modifier_icon_128_v001",
                        "thecat_ui_choice_authority_upgrade_icon_128_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    AuthorityBlessingSealBatchId,
                    "P0 Authority Blessing Seals",
                    "design/development/agent_prompts/p0_asset_batch_16_authority_blessing_seals.md",
                    new[]
                    {
                        "thecat_ui_blessing_oath_bedline_seal_128_v001",
                        "thecat_ui_blessing_dominion_sandglass_seal_128_v001",
                        "thecat_ui_blessing_rhythm_lullaby_seal_128_v001"
                    },
                    new[]
                    {
                        "thecat_ui_choice_authority_blessing_icon_128_v001",
                        "thecat_ui_routecard_blessing_frame_512x256_v001",
                        "thecat_style_status_icons_5x64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    NonBattleNodeSummaryBannerBatchId,
                    "P0 Non-Battle Node Summary Banners",
                    "design/development/agent_prompts/p0_asset_batch_19_nonbattle_node_summary_banners.md",
                    new[]
                    {
                        "thecat_ui_node_shop_summary_banner_512x160_v001",
                        "thecat_ui_node_dreamevent_summary_banner_512x160_v001",
                        "thecat_ui_node_restnest_summary_banner_512x160_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_routecard_shop_frame_512x256_v001",
                        "thecat_ui_routecard_dreamevent_frame_512x256_v001",
                        "thecat_ui_routecard_restnest_frame_512x256_v001",
                        "thecat_ui_choice_purchase_supply_icon_128_v001",
                        "thecat_ui_choice_dreamevent_modifier_icon_128_v001",
                        "thecat_ui_choice_rest_supply_icon_128_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    ShopItemCardBatchId,
                    "P0 Shop Item Cards",
                    "design/development/agent_prompts/p0_asset_batch_20_shop_item_cards.md",
                    new[]
                    {
                        "thecat_ui_shop_item_bed_patch_card_384x160_v001",
                        "thecat_ui_shop_item_litter_sachet_card_384x160_v001",
                        "thecat_ui_shop_item_late_kibble_card_384x160_v001",
                        "thecat_ui_shop_item_free_sample_card_384x160_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_routecard_shop_frame_512x256_v001",
                        "thecat_ui_node_shop_summary_banner_512x160_v001",
                        "thecat_ui_choice_purchase_supply_icon_128_v001",
                        "thecat_ui_core_sleep_icon_64_v001",
                        "thecat_ui_core_poop_icon_64_v001",
                        "thecat_ui_core_hunger_icon_64_v001",
                        "thecat_ui_reward_fishtreat_icon_128_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    DreamEventChoiceCardBatchId,
                    "P0 Dream Event Choice Cards",
                    "design/development/agent_prompts/p0_asset_batch_21_dream_event_choice_cards.md",
                    new[]
                    {
                        "thecat_ui_dreamevent_clear_notifications_card_384x160_v001",
                        "thecat_ui_dreamevent_catnip_residue_card_384x160_v001",
                        "thecat_ui_dreamevent_mark_all_read_card_384x160_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_routecard_dreamevent_frame_512x256_v001",
                        "thecat_ui_node_dreamevent_summary_banner_512x160_v001",
                        "thecat_ui_choice_dreamevent_modifier_icon_128_v001",
                        "thecat_ui_reward_fishtreat_icon_128_v001",
                        "thecat_ui_reward_detail_risk_badge_192x64_v001",
                        "thecat_ui_core_sleep_icon_64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    RestNestRecoveryCardBatchId,
                    "P0 RestNest Recovery Card",
                    "design/development/agent_prompts/p0_asset_batch_22_rest_nest_recovery_card.md",
                    new[]
                    {
                        "thecat_ui_restnest_recovery_card_384x160_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_routecard_restnest_frame_512x256_v001",
                        "thecat_ui_node_restnest_summary_banner_512x160_v001",
                        "thecat_ui_choice_rest_supply_icon_128_v001",
                        "thecat_ui_core_sleep_icon_64_v001",
                        "thecat_ui_core_poop_icon_64_v001",
                        "thecat_ui_core_hunger_icon_64_v001",
                        "thecat_ui_core_hp_icon_64_v001",
                        "thecat_ui_reward_detail_recovery_badge_192x64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    PartnerChoiceCardBatchId,
                    "P0 Partner Choice Cards",
                    "design/development/agent_prompts/p0_asset_batch_23_partner_choice_cards.md",
                    new[]
                    {
                        "thecat_ui_partner_shadowmaru_preview_card_384x160_v001",
                        "thecat_ui_partner_duplicate_supply_card_384x160_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_routecard_partner_frame_512x256_v001",
                        "thecat_ui_route_partner_icon_128_v001",
                        "thecat_ui_choice_partner_recruit_icon_128_v001",
                        "thecat_ui_reward_fishtreat_icon_128_v001",
                        "thecat_ui_reward_detail_gain_badge_192x64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    BlessingChoiceCardBatchId,
                    "P0 Blessing Choice Cards",
                    "design/development/agent_prompts/p0_asset_batch_24_blessing_choice_cards.md",
                    new[]
                    {
                        "thecat_ui_blessing_oath_bedline_card_384x160_v001",
                        "thecat_ui_blessing_dominion_sandglass_card_384x160_v001",
                        "thecat_ui_blessing_rhythm_lullaby_card_384x160_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_routecard_blessing_frame_512x256_v001",
                        "thecat_ui_route_blessing_icon_128_v001",
                        "thecat_ui_choice_authority_blessing_icon_128_v001",
                        "thecat_ui_choice_authority_upgrade_icon_128_v001",
                        "thecat_ui_blessing_oath_bedline_seal_128_v001",
                        "thecat_ui_blessing_dominion_sandglass_seal_128_v001",
                        "thecat_ui_blessing_rhythm_lullaby_seal_128_v001",
                        "thecat_ui_reward_detail_gain_badge_192x64_v001",
                        "thecat_ui_reward_detail_upgrade_badge_192x64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    ResultSettlementBannerBatchId,
                    "P0 Result Settlement Banners",
                    "design/development/agent_prompts/p0_asset_batch_25_result_settlement_banners.md",
                    new[]
                    {
                        "thecat_ui_battle_result_victory_banner_512x160_v001",
                        "thecat_ui_battle_result_defeat_banner_512x160_v001",
                        "thecat_ui_settlement_run_cleared_banner_512x160_v001",
                        "thecat_ui_settlement_run_failed_banner_512x160_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_ui_core_sleep_icon_64_v001",
                        "thecat_vfx_sleep_stable_wave_256_v001",
                        "thecat_vfx_bed_shield_pulse_256_v001",
                        "thecat_vfx_calltyrant_warning_512_v001",
                        "thecat_ui_route_bossnode_icon_128_v001",
                        "thecat_ui_reward_fishtreat_icon_128_v001",
                        "thecat_ui_reward_dreamshard_icon_128_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    CoreGaugeBarBatchId,
                    "P0 Core Gauge Bars",
                    "design/development/agent_prompts/p0_asset_batch_27_core_gauge_bars.md",
                    new[]
                    {
                        "thecat_ui_core_sleep_gauge_frame_384x48_v001",
                        "thecat_ui_core_sleep_gauge_fill_384x48_v001",
                        "thecat_ui_core_hp_gauge_frame_384x48_v001",
                        "thecat_ui_core_hp_gauge_fill_384x48_v001",
                        "thecat_ui_core_poop_gauge_frame_384x48_v001",
                        "thecat_ui_core_poop_gauge_fill_384x48_v001",
                        "thecat_ui_core_hunger_gauge_frame_384x48_v001",
                        "thecat_ui_core_hunger_gauge_fill_384x48_v001"
                    },
                    new[]
                    {
                        "thecat_ui_core_sleep_icon_64_v001",
                        "thecat_ui_core_hp_icon_64_v001",
                        "thecat_ui_core_poop_icon_64_v001",
                        "thecat_ui_core_hunger_icon_64_v001",
                        "thecat_ui_panel_dreamglass_512x256_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    StarterCatHudAvatarBatchId,
                    "P0 Starter Cat HUD Avatars",
                    "design/development/agent_prompts/p0_asset_batch_58_starter_cat_hud_avatar_install.md",
                    new[]
                    {
                        "thecat_cat_saiban_hud_avatar_256_v001",
                        "thecat_cat_nephthys_hud_avatar_256_v001",
                        "thecat_cat_suzune_hud_avatar_256_v001"
                    },
                    new[]
                    {
                        "thecat_cat_saiban_combat_sprite_512_v001",
                        "thecat_cat_nephthys_combat_sprite_512_v001",
                        "thecat_cat_suzune_combat_sprite_512_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    SkillHudFeedbackBatchId,
                    "P0 Skill HUD Feedback",
                    "design/development/agent_prompts/p0_asset_batch_60_skill_hud_feedback_install.md",
                    new[]
                    {
                        "thecat_ui_skill_ready_frame_512_v001",
                        "thecat_ui_skill_cooldown_overlay_512_v001",
                        "thecat_ui_skill_no_target_marker_512_v001",
                        "thecat_ui_skill_hunger_cost_chip_512_v001",
                        "thecat_ui_auto_target_reticle_512_v001",
                        "thecat_ui_interaction_range_ripple_512_v001"
                    },
                    new[]
                    {
                        "thecat_ui_panel_dreamglass_512x256_v001",
                        "thecat_style_status_icons_5x64_v001",
                        "thecat_ui_core_hunger_icon_64_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    StarterSkillVfxBatchId,
                    "P0 Starter Skill VFX",
                    "design/development/agent_prompts/p0_asset_batch_61_starter_skill_vfx_install.md",
                    new[]
                    {
                        "thecat_vfx_saiban_bedline_skill_512_v001",
                        "thecat_vfx_nephthys_moonsand_skill_512_v001",
                        "thecat_vfx_suzune_lullaby_skill_512_v001"
                    },
                    new[]
                    {
                        "thecat_ui_blessing_oath_bedline_seal_128_v001",
                        "thecat_ui_blessing_dominion_sandglass_seal_128_v001",
                        "thecat_ui_blessing_rhythm_lullaby_seal_128_v001",
                        "thecat_vfx_bed_shield_pulse_256_v001",
                        "thecat_vfx_enemy_mark_ring_256_v001",
                        "thecat_vfx_sleep_stable_wave_256_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    SaibanReferenceAtlasBatchId,
                    "P0 Saiban Reference Atlas",
                    "design/development/agent_prompts/p0_asset_batch_71_saiban_unity_reference_install.md",
                    new[]
                    {
                        "thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001"
                    },
                    new[]
                    {
                        "thecat_cat_saiban_combat_sprite_512_v001",
                        "thecat_cat_saiban_hud_avatar_256_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    NephthysReferenceAtlasBatchId,
                    "P0 Nephthys Reference Atlas",
                    "design/development/agent_prompts/p0_asset_batch_72_nephthys_unity_reference_install.md",
                    new[]
                    {
                        "thecat_cat_nephthys_turnaround_reference_atlas_2304x768_v001"
                    },
                    new[]
                    {
                        "thecat_cat_nephthys_combat_sprite_512_v001",
                        "thecat_cat_nephthys_hud_avatar_256_v001"
                    }),
                new P0AssetGenerationBatchDefinition(
                    SuzuneReferenceAtlasBatchId,
                    "P0 Suzune Reference Atlas",
                    "design/development/agent_prompts/p0_asset_batch_73_suzune_unity_reference_install.md",
                    new[]
                    {
                        "thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001"
                    },
                    new[]
                    {
                        "thecat_cat_suzune_combat_sprite_512_v001",
                        "thecat_cat_suzune_hud_avatar_256_v001"
                    })
            };
        }

        private static IReadOnlyList<string> Empty()
        {
            return new string[0];
        }
    }
}
