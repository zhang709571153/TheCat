Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$projectRoot = Resolve-Path (Join-Path $PSScriptRoot "..\..\..")
$outputRelative = "design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png"
$noteRelative = "design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.md"
$outputPath = Join-Path $projectRoot $outputRelative
$notePath = Join-Path $projectRoot $noteRelative

$items = @(
    @{ Binding = "background.bedroom_dream"; Surface = "battle_world"; AssetId = "thecat_bg_bedroomdream_battle_1920x1080_v001"; Path = "Assets/TheCat/Art/Scenes/BedroomDream/thecat_bg_bedroomdream_battle_1920x1080_v001.png"; SourceLock = "bedroom_map_concept" },
    @{ Binding = "cat.combat.saiban"; Surface = "cat_hud"; AssetId = "thecat_cat_saiban_combat_sprite_512_v001"; Path = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png"; SourceLock = "saiban_turnaround_colored" },
    @{ Binding = "cat.combat.nephthys"; Surface = "cat_hud"; AssetId = "thecat_cat_nephthys_combat_sprite_512_v001"; Path = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png"; SourceLock = "nephthys_turnaround_colored" },
    @{ Binding = "cat.combat.suzune"; Surface = "cat_hud"; AssetId = "thecat_cat_suzune_combat_sprite_512_v001"; Path = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png"; SourceLock = "suzune_turnaround_colored" },
    @{ Binding = "cat.avatar.saiban"; Surface = "cat_hud"; AssetId = "thecat_cat_saiban_hud_avatar_256_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_cat_saiban_hud_avatar_256_v001.png"; SourceLock = "saiban_turnaround_colored" },
    @{ Binding = "cat.avatar.nephthys"; Surface = "cat_hud"; AssetId = "thecat_cat_nephthys_hud_avatar_256_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_cat_nephthys_hud_avatar_256_v001.png"; SourceLock = "nephthys_turnaround_colored" },
    @{ Binding = "cat.avatar.suzune"; Surface = "cat_hud"; AssetId = "thecat_cat_suzune_hud_avatar_256_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_cat_suzune_hud_avatar_256_v001.png"; SourceLock = "suzune_turnaround_colored" },
    @{ Binding = "enemy.combat.black_mud"; Surface = "battle_world"; AssetId = "thecat_enemy_blackmud_combat_sprite_512_v001"; Path = "Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_blackmud_combat_sprite_512_v001.png"; SourceLock = "black_mud_concept" },
    @{ Binding = "enemy.combat.cold_light"; Surface = "battle_world"; AssetId = "thecat_enemy_coldlight_combat_sprite_512_v001"; Path = "Assets/TheCat/Art/Enemies/Sprites/thecat_enemy_coldlight_combat_sprite_512_v001.png"; SourceLock = "cold_light_concept" },
    @{ Binding = "enemy.combat.call_tyrant"; Surface = "battle_world"; AssetId = "thecat_enemy_calltyrant_concept_2048_v001"; Path = "Assets/TheCat/Art/Enemies/Concepts/thecat_enemy_calltyrant_concept_2048_v001.png"; SourceLock = "call_tyrant_concept" },
    @{ Binding = "enemy.anim.black_mud_move"; Surface = "battle_world"; AssetId = "thecat_enemy_blackmud_move_framesheet_4x256_v001"; Path = "Assets/TheCat/Art/Enemies/Frames/thecat_enemy_blackmud_move_framesheet_4x256_v001.png"; SourceLock = "black_mud_animation" },
    @{ Binding = "enemy.anim.cold_light_cast"; Surface = "battle_world"; AssetId = "thecat_enemy_coldlight_cast_framesheet_4x256_v001"; Path = "Assets/TheCat/Art/Enemies/Frames/thecat_enemy_coldlight_cast_framesheet_4x256_v001.png"; SourceLock = "cold_light_animation" },
    @{ Binding = "enemy.anim.call_tyrant_boss_pattern"; Surface = "battle_world"; AssetId = "thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001"; Path = "Assets/TheCat/Art/Enemies/Frames/thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001.png"; SourceLock = "call_tyrant_animation" },
    @{ Binding = "prop.bed"; Surface = "battle_world"; AssetId = "thecat_prop_bed_sleepglow_sprite_512_v001"; Path = "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_bed_sleepglow_sprite_512_v001.png"; SourceLock = "bedroom_map_concept" },
    @{ Binding = "prop.litter_box"; Surface = "battle_world"; AssetId = "thecat_prop_litterbox_sprite_256_v001"; Path = "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_litterbox_sprite_256_v001.png"; SourceLock = "bedroom_mid_background_sprites" },
    @{ Binding = "prop.feeder"; Surface = "battle_world"; AssetId = "thecat_prop_feeder_sprite_256_v001"; Path = "Assets/TheCat/Art/Scenes/BedroomDream/thecat_prop_feeder_sprite_256_v001.png"; SourceLock = "bedroom_mid_background_sprites" },
    @{ Binding = "core.owner_sleep"; Surface = "battle_hud"; AssetId = "thecat_ui_core_sleep_icon_64_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_core_sleep_icon_64_v001.png"; SourceLock = "status_icon_style_anchor" },
    @{ Binding = "core.cat_hp"; Surface = "cat_hud"; AssetId = "thecat_ui_core_hp_icon_64_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_core_hp_icon_64_v001.png"; SourceLock = "status_icon_style_anchor" },
    @{ Binding = "core.team_poop"; Surface = "battle_hud"; AssetId = "thecat_ui_core_poop_icon_64_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_core_poop_icon_64_v001.png"; SourceLock = "status_icon_style_anchor" },
    @{ Binding = "core.team_hunger"; Surface = "battle_hud"; AssetId = "thecat_ui_core_hunger_icon_64_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_core_hunger_icon_64_v001.png"; SourceLock = "status_icon_style_anchor" },
    @{ Binding = "skill_hud.ready_frame"; Surface = "skill_hud"; AssetId = "thecat_ui_skill_ready_frame_512_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_ready_frame_512_v001.png"; SourceLock = "skill_hud_feedback_batch_60" },
    @{ Binding = "skill_hud.cooldown_overlay"; Surface = "skill_hud"; AssetId = "thecat_ui_skill_cooldown_overlay_512_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png"; SourceLock = "skill_hud_feedback_batch_60" },
    @{ Binding = "skill_hud.no_target_marker"; Surface = "skill_hud"; AssetId = "thecat_ui_skill_no_target_marker_512_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png"; SourceLock = "skill_hud_feedback_batch_60" },
    @{ Binding = "skill_hud.hunger_cost_chip"; Surface = "skill_hud"; AssetId = "thecat_ui_skill_hunger_cost_chip_512_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png"; SourceLock = "skill_hud_feedback_batch_60" },
    @{ Binding = "skill_hud.auto_target_reticle"; Surface = "skill_hud"; AssetId = "thecat_ui_auto_target_reticle_512_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_auto_target_reticle_512_v001.png"; SourceLock = "skill_hud_feedback_batch_60" },
    @{ Binding = "battle_hud.interaction_range_ripple"; Surface = "battle_hud"; AssetId = "thecat_ui_interaction_range_ripple_512_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png"; SourceLock = "skill_hud_feedback_batch_60" },
    @{ Binding = "skill_vfx.saiban_bedline"; Surface = "battle_feedback"; AssetId = "thecat_vfx_saiban_bedline_skill_512_v001"; Path = "Assets/TheCat/Art/VFX/thecat_vfx_saiban_bedline_skill_512_v001.png"; SourceLock = "batch_61_saiban_turnaround_symbolic_vfx" },
    @{ Binding = "skill_vfx.nephthys_moonsand"; Surface = "battle_feedback"; AssetId = "thecat_vfx_nephthys_moonsand_skill_512_v001"; Path = "Assets/TheCat/Art/VFX/thecat_vfx_nephthys_moonsand_skill_512_v001.png"; SourceLock = "batch_61_nephthys_turnaround_symbolic_vfx" },
    @{ Binding = "skill_vfx.suzune_lullaby"; Surface = "battle_feedback"; AssetId = "thecat_vfx_suzune_lullaby_skill_512_v001"; Path = "Assets/TheCat/Art/VFX/thecat_vfx_suzune_lullaby_skill_512_v001.png"; SourceLock = "batch_61_suzune_turnaround_symbolic_vfx" },
    @{ Binding = "core_gauge.owner_sleep.frame"; Surface = "battle_hud"; AssetId = "thecat_ui_core_sleep_gauge_frame_384x48_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_core_sleep_gauge_frame_384x48_v001.png"; SourceLock = "core_gauge_bar_batch_27" },
    @{ Binding = "core_gauge.owner_sleep.fill"; Surface = "battle_hud"; AssetId = "thecat_ui_core_sleep_gauge_fill_384x48_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_core_sleep_gauge_fill_384x48_v001.png"; SourceLock = "core_gauge_bar_batch_27" },
    @{ Binding = "core_gauge.cat_hp.frame"; Surface = "cat_hud"; AssetId = "thecat_ui_core_hp_gauge_frame_384x48_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_core_hp_gauge_frame_384x48_v001.png"; SourceLock = "core_gauge_bar_batch_27" },
    @{ Binding = "core_gauge.cat_hp.fill"; Surface = "cat_hud"; AssetId = "thecat_ui_core_hp_gauge_fill_384x48_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_core_hp_gauge_fill_384x48_v001.png"; SourceLock = "core_gauge_bar_batch_27" },
    @{ Binding = "core_gauge.team_poop.frame"; Surface = "battle_hud"; AssetId = "thecat_ui_core_poop_gauge_frame_384x48_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_core_poop_gauge_frame_384x48_v001.png"; SourceLock = "core_gauge_bar_batch_27" },
    @{ Binding = "core_gauge.team_poop.fill"; Surface = "battle_hud"; AssetId = "thecat_ui_core_poop_gauge_fill_384x48_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_core_poop_gauge_fill_384x48_v001.png"; SourceLock = "core_gauge_bar_batch_27" },
    @{ Binding = "core_gauge.team_hunger.frame"; Surface = "battle_hud"; AssetId = "thecat_ui_core_hunger_gauge_frame_384x48_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_core_hunger_gauge_frame_384x48_v001.png"; SourceLock = "core_gauge_bar_batch_27" },
    @{ Binding = "core_gauge.team_hunger.fill"; Surface = "battle_hud"; AssetId = "thecat_ui_core_hunger_gauge_fill_384x48_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_core_hunger_gauge_fill_384x48_v001.png"; SourceLock = "core_gauge_bar_batch_27" },
    @{ Binding = "status.sleep_stable"; Surface = "status_hud"; AssetId = "thecat_ui_status_sleep_stable_64_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_sleep_stable_64_v001.png"; SourceLock = "status_icon_style_cell_0" },
    @{ Binding = "status.slow"; Surface = "status_hud"; AssetId = "thecat_ui_status_slow_64_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_slow_64_v001.png"; SourceLock = "status_icon_style_cell_1" },
    @{ Binding = "status.knockback"; Surface = "status_hud"; AssetId = "thecat_ui_status_knockback_64_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_knockback_64_v001.png"; SourceLock = "status_icon_style_cell_2" },
    @{ Binding = "status.mark"; Surface = "status_hud"; AssetId = "thecat_ui_status_mark_64_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_mark_64_v001.png"; SourceLock = "status_icon_style_cell_3" },
    @{ Binding = "status.shield"; Surface = "status_hud"; AssetId = "thecat_ui_status_shield_64_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_shield_64_v001.png"; SourceLock = "status_icon_style_cell_4" },
    @{ Binding = "status_compact.sleep_stable"; Surface = "status_hud"; AssetId = "thecat_ui_status_sleep_stable_32_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_sleep_stable_32_v001.png"; SourceLock = "status_compact_batch_14" },
    @{ Binding = "status_compact.slow"; Surface = "status_hud"; AssetId = "thecat_ui_status_slow_32_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_slow_32_v001.png"; SourceLock = "status_compact_batch_14" },
    @{ Binding = "status_compact.knockback"; Surface = "status_hud"; AssetId = "thecat_ui_status_knockback_32_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_knockback_32_v001.png"; SourceLock = "status_compact_batch_14" },
    @{ Binding = "status_compact.mark"; Surface = "status_hud"; AssetId = "thecat_ui_status_mark_32_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_mark_32_v001.png"; SourceLock = "status_compact_batch_14" },
    @{ Binding = "status_compact.shield"; Surface = "status_hud"; AssetId = "thecat_ui_status_shield_32_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_status_shield_32_v001.png"; SourceLock = "status_compact_batch_14" },
    @{ Binding = "route.defense_node"; Surface = "route_map"; AssetId = "thecat_ui_route_defense_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_route_defense_icon_128_v001.png"; SourceLock = "route_node_icon_batch_06" },
    @{ Binding = "route.elite_node"; Surface = "route_map"; AssetId = "thecat_ui_route_elite_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_route_elite_icon_128_v001.png"; SourceLock = "route_node_icon_batch_06" },
    @{ Binding = "route.partner_node"; Surface = "route_map"; AssetId = "thecat_ui_route_partner_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_route_partner_icon_128_v001.png"; SourceLock = "route_node_icon_batch_06" },
    @{ Binding = "route.shop_node"; Surface = "route_map"; AssetId = "thecat_ui_route_shop_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_route_shop_icon_128_v001.png"; SourceLock = "route_node_icon_batch_06" },
    @{ Binding = "route.dream_event_node"; Surface = "route_map"; AssetId = "thecat_ui_route_dreamevent_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_route_dreamevent_icon_128_v001.png"; SourceLock = "route_node_icon_batch_06" },
    @{ Binding = "route.blessing_node"; Surface = "route_map"; AssetId = "thecat_ui_route_blessing_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_route_blessing_icon_128_v001.png"; SourceLock = "route_node_icon_batch_06" },
    @{ Binding = "route.rest_nest_node"; Surface = "route_map"; AssetId = "thecat_ui_route_restnest_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_route_restnest_icon_128_v001.png"; SourceLock = "route_node_icon_batch_06" },
    @{ Binding = "route.boss_node"; Surface = "route_map"; AssetId = "thecat_ui_route_bossnode_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_route_bossnode_icon_128_v001.png"; SourceLock = "call_tyrant_concept" },
    @{ Binding = "route_summary.shop"; Surface = "route_node_summary"; AssetId = "thecat_ui_node_shop_summary_banner_512x160_v001"; Path = "Assets/TheCat/Art/UI/Banners/thecat_ui_node_shop_summary_banner_512x160_v001.png"; SourceLock = "nonbattle_node_summary_banner_batch_19" },
    @{ Binding = "route_summary.dream_event"; Surface = "route_node_summary"; AssetId = "thecat_ui_node_dreamevent_summary_banner_512x160_v001"; Path = "Assets/TheCat/Art/UI/Banners/thecat_ui_node_dreamevent_summary_banner_512x160_v001.png"; SourceLock = "nonbattle_node_summary_banner_batch_19" },
    @{ Binding = "route_summary.rest_nest"; Surface = "route_node_summary"; AssetId = "thecat_ui_node_restnest_summary_banner_512x160_v001"; Path = "Assets/TheCat/Art/UI/Banners/thecat_ui_node_restnest_summary_banner_512x160_v001.png"; SourceLock = "nonbattle_node_summary_banner_batch_19" },
    @{ Binding = "shop_item.bed_patch"; Surface = "shop_item_card"; AssetId = "thecat_ui_shop_item_bed_patch_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_bed_patch_card_384x160_v001.png"; SourceLock = "shop_item_card_batch_20" },
    @{ Binding = "shop_item.litter_sachet"; Surface = "shop_item_card"; AssetId = "thecat_ui_shop_item_litter_sachet_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_litter_sachet_card_384x160_v001.png"; SourceLock = "shop_item_card_batch_20" },
    @{ Binding = "shop_item.late_kibble"; Surface = "shop_item_card"; AssetId = "thecat_ui_shop_item_late_kibble_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_late_kibble_card_384x160_v001.png"; SourceLock = "shop_item_card_batch_20" },
    @{ Binding = "shop_item.free_sample"; Surface = "shop_item_card"; AssetId = "thecat_ui_shop_item_free_sample_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_shop_item_free_sample_card_384x160_v001.png"; SourceLock = "shop_item_card_batch_20" },
    @{ Binding = "dream_event_choice.clear_notifications"; Surface = "dream_event_choice_card"; AssetId = "thecat_ui_dreamevent_clear_notifications_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_clear_notifications_card_384x160_v001.png"; SourceLock = "dream_event_choice_card_batch_21" },
    @{ Binding = "dream_event_choice.catnip_residue"; Surface = "dream_event_choice_card"; AssetId = "thecat_ui_dreamevent_catnip_residue_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_catnip_residue_card_384x160_v001.png"; SourceLock = "dream_event_choice_card_batch_21" },
    @{ Binding = "dream_event_choice.mark_all_read"; Surface = "dream_event_choice_card"; AssetId = "thecat_ui_dreamevent_mark_all_read_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_dreamevent_mark_all_read_card_384x160_v001.png"; SourceLock = "dream_event_choice_card_batch_21" },
    @{ Binding = "rest_nest_choice.recovery"; Surface = "rest_nest_choice_card"; AssetId = "thecat_ui_restnest_recovery_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_restnest_recovery_card_384x160_v001.png"; SourceLock = "rest_nest_recovery_card_batch_22" },
    @{ Binding = "partner_choice.shadowmaru_preview"; Surface = "partner_choice_card"; AssetId = "thecat_ui_partner_shadowmaru_preview_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_partner_shadowmaru_preview_card_384x160_v001.png"; SourceLock = "partner_choice_card_batch_23" },
    @{ Binding = "partner_choice.duplicate_supply"; Surface = "partner_choice_card"; AssetId = "thecat_ui_partner_duplicate_supply_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_partner_duplicate_supply_card_384x160_v001.png"; SourceLock = "partner_choice_card_batch_23" },
    @{ Binding = "blessing_choice.oath_bedline"; Surface = "blessing_choice_card"; AssetId = "thecat_ui_blessing_oath_bedline_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_blessing_oath_bedline_card_384x160_v001.png"; SourceLock = "blessing_choice_card_batch_24" },
    @{ Binding = "blessing_choice.dominion_sandglass"; Surface = "blessing_choice_card"; AssetId = "thecat_ui_blessing_dominion_sandglass_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_blessing_dominion_sandglass_card_384x160_v001.png"; SourceLock = "blessing_choice_card_batch_24" },
    @{ Binding = "blessing_choice.rhythm_lullaby"; Surface = "blessing_choice_card"; AssetId = "thecat_ui_blessing_rhythm_lullaby_card_384x160_v001"; Path = "Assets/TheCat/Art/UI/Cards/thecat_ui_blessing_rhythm_lullaby_card_384x160_v001.png"; SourceLock = "blessing_choice_card_batch_24" },
    @{ Binding = "warning.call_tyrant"; Surface = "battle_warning"; AssetId = "thecat_vfx_calltyrant_warning_512_v001"; Path = "Assets/TheCat/Art/Enemies/VFX/thecat_vfx_calltyrant_warning_512_v001.png"; SourceLock = "call_tyrant_animation" },
    @{ Binding = "warning.black_mud_bed_claw"; Surface = "battle_warning"; AssetId = "thecat_vfx_blackmud_bed_claw_256_v001"; Path = "Assets/TheCat/Art/Enemies/VFX/thecat_vfx_blackmud_bed_claw_256_v001.png"; SourceLock = "black_mud_animation" },
    @{ Binding = "warning.cold_light_beam"; Surface = "battle_warning"; AssetId = "thecat_vfx_coldlight_beam_warning_256_v001"; Path = "Assets/TheCat/Art/Enemies/VFX/thecat_vfx_coldlight_beam_warning_256_v001.png"; SourceLock = "cold_light_animation" },
    @{ Binding = "warning.call_tyrant_app_throw"; Surface = "battle_warning"; AssetId = "thecat_vfx_calltyrant_app_throw_256_v001"; Path = "Assets/TheCat/Art/Enemies/VFX/thecat_vfx_calltyrant_app_throw_256_v001.png"; SourceLock = "call_tyrant_animation" },
    @{ Binding = "warning.call_tyrant_summon_portal"; Surface = "battle_warning"; AssetId = "thecat_vfx_calltyrant_summon_portal_256_v001"; Path = "Assets/TheCat/Art/Enemies/VFX/thecat_vfx_calltyrant_summon_portal_256_v001.png"; SourceLock = "call_tyrant_animation" },
    @{ Binding = "feedback.hit_spark"; Surface = "battle_feedback"; AssetId = "thecat_vfx_hit_spark_256_v001"; Path = "Assets/TheCat/Art/VFX/thecat_vfx_hit_spark_256_v001.png"; SourceLock = "battle_feedback_vfx_batch_09" },
    @{ Binding = "feedback.bed_shield_pulse"; Surface = "battle_feedback"; AssetId = "thecat_vfx_bed_shield_pulse_256_v001"; Path = "Assets/TheCat/Art/VFX/thecat_vfx_bed_shield_pulse_256_v001.png"; SourceLock = "battle_feedback_vfx_batch_09" },
    @{ Binding = "feedback.sleep_stable_wave"; Surface = "battle_feedback"; AssetId = "thecat_vfx_sleep_stable_wave_256_v001"; Path = "Assets/TheCat/Art/VFX/thecat_vfx_sleep_stable_wave_256_v001.png"; SourceLock = "battle_feedback_vfx_batch_09" },
    @{ Binding = "feedback.litter_cleanse"; Surface = "battle_feedback"; AssetId = "thecat_vfx_litter_cleanse_256_v001"; Path = "Assets/TheCat/Art/VFX/thecat_vfx_litter_cleanse_256_v001.png"; SourceLock = "battle_feedback_vfx_batch_09" },
    @{ Binding = "feedback.feeder_kibble"; Surface = "battle_feedback"; AssetId = "thecat_vfx_feeder_kibble_256_v001"; Path = "Assets/TheCat/Art/VFX/thecat_vfx_feeder_kibble_256_v001.png"; SourceLock = "battle_feedback_vfx_batch_09" },
    @{ Binding = "feedback.enemy_mark_ring"; Surface = "battle_feedback"; AssetId = "thecat_vfx_enemy_mark_ring_256_v001"; Path = "Assets/TheCat/Art/VFX/thecat_vfx_enemy_mark_ring_256_v001.png"; SourceLock = "battle_feedback_vfx_batch_09" },
    @{ Binding = "main_menu.background"; Surface = "main_menu"; AssetId = "thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001"; Path = "Assets/TheCat/Art/UI/Backgrounds/thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png"; SourceLock = "ui_shell_batch_08" },
    @{ Binding = "main_menu.title_logo"; Surface = "main_menu"; AssetId = "thecat_ui_title_logo_512x256_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_title_logo_512x256_v001.png"; SourceLock = "ui_shell_batch_08" },
    @{ Binding = "ui.panel.dreamglass"; Surface = "ui_shell"; AssetId = "thecat_ui_panel_dreamglass_512x256_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png"; SourceLock = "ui_shell_batch_08" },
    @{ Binding = "ui.button.primary"; Surface = "ui_shell"; AssetId = "thecat_ui_button_primary_384x96_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png"; SourceLock = "ui_shell_batch_08" },
    @{ Binding = "settlement.reward.fish_treat"; Surface = "settlement"; AssetId = "thecat_ui_reward_fishtreat_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_reward_fishtreat_icon_128_v001.png"; SourceLock = "ui_shell_batch_08" },
    @{ Binding = "settlement.reward.dream_shard"; Surface = "settlement"; AssetId = "thecat_ui_reward_dreamshard_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_reward_dreamshard_icon_128_v001.png"; SourceLock = "ui_shell_batch_08" },
    @{ Binding = "battle_result.victory_banner"; Surface = "battle_result"; AssetId = "thecat_ui_battle_result_victory_banner_512x160_v001"; Path = "Assets/TheCat/Art/UI/Banners/thecat_ui_battle_result_victory_banner_512x160_v001.png"; SourceLock = "result_settlement_banner_batch_25" },
    @{ Binding = "battle_result.defeat_banner"; Surface = "battle_result"; AssetId = "thecat_ui_battle_result_defeat_banner_512x160_v001"; Path = "Assets/TheCat/Art/UI/Banners/thecat_ui_battle_result_defeat_banner_512x160_v001.png"; SourceLock = "result_settlement_banner_batch_25" },
    @{ Binding = "settlement.run_cleared_banner"; Surface = "settlement"; AssetId = "thecat_ui_settlement_run_cleared_banner_512x160_v001"; Path = "Assets/TheCat/Art/UI/Banners/thecat_ui_settlement_run_cleared_banner_512x160_v001.png"; SourceLock = "result_settlement_banner_batch_25" },
    @{ Binding = "settlement.run_failed_banner"; Surface = "settlement"; AssetId = "thecat_ui_settlement_run_failed_banner_512x160_v001"; Path = "Assets/TheCat/Art/UI/Banners/thecat_ui_settlement_run_failed_banner_512x160_v001.png"; SourceLock = "result_settlement_banner_batch_25" },
    @{ Binding = "route_choice.partner_recruit"; Surface = "route_choice"; AssetId = "thecat_ui_choice_partner_recruit_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_choice_partner_recruit_icon_128_v001.png"; SourceLock = "route_choice_icon_batch_12" },
    @{ Binding = "route_choice.purchase_supply"; Surface = "route_choice"; AssetId = "thecat_ui_choice_purchase_supply_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_choice_purchase_supply_icon_128_v001.png"; SourceLock = "route_choice_icon_batch_12" },
    @{ Binding = "route_choice.authority_blessing"; Surface = "route_choice"; AssetId = "thecat_ui_choice_authority_blessing_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_choice_authority_blessing_icon_128_v001.png"; SourceLock = "route_choice_icon_batch_12" },
    @{ Binding = "route_choice.authority_upgrade"; Surface = "route_choice"; AssetId = "thecat_ui_choice_authority_upgrade_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_choice_authority_upgrade_icon_128_v001.png"; SourceLock = "route_choice_icon_batch_12" },
    @{ Binding = "route_choice.rest_supply"; Surface = "route_choice"; AssetId = "thecat_ui_choice_rest_supply_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_choice_rest_supply_icon_128_v001.png"; SourceLock = "route_choice_icon_batch_12" },
    @{ Binding = "route_choice.dream_event_modifier"; Surface = "route_choice"; AssetId = "thecat_ui_choice_dreamevent_modifier_icon_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_choice_dreamevent_modifier_icon_128_v001.png"; SourceLock = "route_choice_icon_batch_12" },
    @{ Binding = "route_reward_card.partner"; Surface = "route_reward_card"; AssetId = "thecat_ui_routecard_partner_frame_512x256_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_partner_frame_512x256_v001.png"; SourceLock = "route_reward_card_frame_batch_13" },
    @{ Binding = "route_reward_card.shop"; Surface = "route_reward_card"; AssetId = "thecat_ui_routecard_shop_frame_512x256_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_shop_frame_512x256_v001.png"; SourceLock = "route_reward_card_frame_batch_13" },
    @{ Binding = "route_reward_card.blessing"; Surface = "route_reward_card"; AssetId = "thecat_ui_routecard_blessing_frame_512x256_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_blessing_frame_512x256_v001.png"; SourceLock = "route_reward_card_frame_batch_13" },
    @{ Binding = "route_reward_card.dream_event"; Surface = "route_reward_card"; AssetId = "thecat_ui_routecard_dreamevent_frame_512x256_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_dreamevent_frame_512x256_v001.png"; SourceLock = "route_reward_card_frame_batch_13" },
    @{ Binding = "route_reward_card.rest_nest"; Surface = "route_reward_card"; AssetId = "thecat_ui_routecard_restnest_frame_512x256_v001"; Path = "Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_restnest_frame_512x256_v001.png"; SourceLock = "route_reward_card_frame_batch_13" },
    @{ Binding = "route_reward_detail.gain"; Surface = "route_reward_detail"; AssetId = "thecat_ui_reward_detail_gain_badge_192x64_v001"; Path = "Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_gain_badge_192x64_v001.png"; SourceLock = "route_reward_detail_badge_batch_15" },
    @{ Binding = "route_reward_detail.cost"; Surface = "route_reward_detail"; AssetId = "thecat_ui_reward_detail_cost_badge_192x64_v001"; Path = "Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_cost_badge_192x64_v001.png"; SourceLock = "route_reward_detail_badge_batch_15" },
    @{ Binding = "route_reward_detail.recovery"; Surface = "route_reward_detail"; AssetId = "thecat_ui_reward_detail_recovery_badge_192x64_v001"; Path = "Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_recovery_badge_192x64_v001.png"; SourceLock = "route_reward_detail_badge_batch_15" },
    @{ Binding = "route_reward_detail.risk"; Surface = "route_reward_detail"; AssetId = "thecat_ui_reward_detail_risk_badge_192x64_v001"; Path = "Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_risk_badge_192x64_v001.png"; SourceLock = "route_reward_detail_badge_batch_15" },
    @{ Binding = "route_reward_detail.upgrade"; Surface = "route_reward_detail"; AssetId = "thecat_ui_reward_detail_upgrade_badge_192x64_v001"; Path = "Assets/TheCat/Art/UI/Badges/thecat_ui_reward_detail_upgrade_badge_192x64_v001.png"; SourceLock = "route_reward_detail_badge_batch_15" },
    @{ Binding = "blessing_detail.oath_bedline"; Surface = "blessing_detail"; AssetId = "thecat_ui_blessing_oath_bedline_seal_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_oath_bedline_seal_128_v001.png"; SourceLock = "authority_blessing_seal_batch_16" },
    @{ Binding = "blessing_detail.dominion_sandglass"; Surface = "blessing_detail"; AssetId = "thecat_ui_blessing_dominion_sandglass_seal_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_dominion_sandglass_seal_128_v001.png"; SourceLock = "authority_blessing_seal_batch_16" },
    @{ Binding = "blessing_detail.rhythm_lullaby"; Surface = "blessing_detail"; AssetId = "thecat_ui_blessing_rhythm_lullaby_seal_128_v001"; Path = "Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_rhythm_lullaby_seal_128_v001.png"; SourceLock = "authority_blessing_seal_batch_16" }
)

function Draw-ContainedImage {
    param(
        [System.Drawing.Graphics]$Graphics,
        [System.Drawing.Image]$Image,
        [int]$X,
        [int]$Y,
        [int]$Width,
        [int]$Height
    )

    $scale = [Math]::Min($Width / $Image.Width, $Height / $Image.Height)
    $drawWidth = [int]($Image.Width * $scale)
    $drawHeight = [int]($Image.Height * $scale)
    $drawX = $X + [int](($Width - $drawWidth) / 2)
    $drawY = $Y + [int](($Height - $drawHeight) / 2)
    $Graphics.DrawImage($Image, $drawX, $drawY, $drawWidth, $drawHeight)
}

function Draw-Checkerboard {
    param(
        [System.Drawing.Graphics]$Graphics,
        [int]$X,
        [int]$Y,
        [int]$Width,
        [int]$Height
    )

    $light = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(244, 241, 232))
    $dark = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(218, 212, 203))
    try {
        $tile = 20
        for ($yy = $Y; $yy -lt $Y + $Height; $yy += $tile) {
            for ($xx = $X; $xx -lt $X + $Width; $xx += $tile) {
                $brush = if (((($xx - $X) / $tile + ($yy - $Y) / $tile) % 2) -eq 0) { $light } else { $dark }
                $Graphics.FillRectangle($brush, $xx, $yy, $tile, $tile)
            }
        }
    } finally {
        $light.Dispose()
        $dark.Dispose()
    }
}

$missing = @()
foreach ($item in $items) {
    $fullPath = Join-Path $projectRoot $item.Path
    if (-not (Test-Path $fullPath)) {
        $missing += $item.Path
    }
}

if ($missing.Count -gt 0) {
    throw "Missing runtime visual asset(s): $($missing -join ', ')"
}

$columns = 3
$cardWidth = 640
$cardHeight = 430
$margin = 38
$gap = 24
$headerHeight = 130
$rows = [int][Math]::Ceiling($items.Count / $columns)
$width = ($columns * $cardWidth) + (($columns - 1) * $gap) + ($margin * 2)
$height = $headerHeight + ($rows * $cardHeight) + (($rows - 1) * $gap) + $margin

$bitmap = [System.Drawing.Bitmap]::new($width, $height)
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality

$background = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(28, 26, 36))
$cardBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(247, 243, 234))
$titleBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(255, 246, 210))
$textBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(42, 37, 45))
$mutedBrush = [System.Drawing.SolidBrush]::new([System.Drawing.Color]::FromArgb(98, 86, 96))
$borderPen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(93, 77, 112), 3)
$imageBorderPen = [System.Drawing.Pen]::new([System.Drawing.Color]::FromArgb(180, 169, 180), 2)
$titleFont = [System.Drawing.Font]::new("Arial", 34, [System.Drawing.FontStyle]::Bold)
$subtitleFont = [System.Drawing.Font]::new("Arial", 15, [System.Drawing.FontStyle]::Regular)
$bindingFont = [System.Drawing.Font]::new("Consolas", 15, [System.Drawing.FontStyle]::Bold)
$metaFont = [System.Drawing.Font]::new("Arial", 12, [System.Drawing.FontStyle]::Regular)

try {
    $graphics.FillRectangle($background, 0, 0, $width, $height)
    $graphics.DrawString("P0 Runtime Visual Contact Sheet", $titleFont, $titleBrush, $margin, 24)
    $graphics.DrawString("$($items.Count) manifest-backed runtime bindings. Starter-cat body replacements remain locked behind colored-turnaround review.", $subtitleFont, $titleBrush, $margin, 75)

    for ($i = 0; $i -lt $items.Count; $i++) {
        $item = $items[$i]
        $column = $i % $columns
        $row = [int][Math]::Floor($i / $columns)
        $x = $margin + ($column * ($cardWidth + $gap))
        $y = $headerHeight + ($row * ($cardHeight + $gap))

        $graphics.FillRectangle($cardBrush, $x, $y, $cardWidth, $cardHeight)
        $graphics.DrawRectangle($borderPen, $x, $y, $cardWidth, $cardHeight)

        $imageX = $x + 24
        $imageY = $y + 24
        $imageW = $cardWidth - 48
        $imageH = 260
        Draw-Checkerboard $graphics $imageX $imageY $imageW $imageH

        $imagePath = Join-Path $projectRoot $item.Path
        $image = [System.Drawing.Image]::FromFile($imagePath)
        try {
            Draw-ContainedImage $graphics $image $imageX $imageY $imageW $imageH
        } finally {
            $image.Dispose()
        }

        $graphics.DrawRectangle($imageBorderPen, $imageX, $imageY, $imageW, $imageH)
        $textY = $imageY + $imageH + 18
        $graphics.DrawString($item.Binding, $bindingFont, $textBrush, $imageX, $textY)
        $graphics.DrawString("surface: $($item.Surface)", $metaFont, $mutedBrush, $imageX, $textY + 32)
        $graphics.DrawString("source: $($item['SourceLock'])", $metaFont, $mutedBrush, $imageX, $textY + 56)
        $graphics.DrawString("asset: $($item.AssetId)", $metaFont, $mutedBrush, $imageX, $textY + 80)
    }

    $bitmap.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
} finally {
    $graphics.Dispose()
    $bitmap.Dispose()
    $background.Dispose()
    $cardBrush.Dispose()
    $titleBrush.Dispose()
    $textBrush.Dispose()
    $mutedBrush.Dispose()
    $borderPen.Dispose()
    $imageBorderPen.Dispose()
    $titleFont.Dispose()
    $subtitleFont.Dispose()
    $bindingFont.Dispose()
    $metaFont.Dispose()
}

$outputHash = (Get-FileHash -Algorithm SHA256 $outputPath).Hash.ToLowerInvariant()
$lines = @(
    "# P0 Runtime Visual Contact Sheet",
    "",
    "- Output image: ``$outputRelative``",
    "- Output SHA-256: ``$outputHash``",
    "- Runtime bindings covered: ``$($items.Count)``",
    "- Generated by: ``design/development/tools/build_runtime_visual_contact_sheet.ps1``",
    "",
    "## Production Decision",
    "",
    "- Accepted as offline review evidence for the current P0 runtime visual baseline.",
    "- This sheet does not approve starter cat formal imports.",
    "- Starter-cat body replacements remain blocked behind active-cat Play Mode screenshots and colored three-view turnaround comparison.",
    "",
    "## Binding Order",
    ""
)

foreach ($item in $items) {
    $lines += "- ``$($item.Binding)`` -> ``$($item.AssetId)``"
}

Set-Content -Path $notePath -Value $lines -Encoding UTF8
Write-Output "Wrote $outputRelative"
Write-Output "Wrote $noteRelative"
