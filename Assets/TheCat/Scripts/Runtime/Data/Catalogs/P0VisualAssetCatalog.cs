using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Data.Catalogs
{
    public static class P0VisualAssetCatalog
    {
        public const int P0RuntimeVisualBindingCount = 111;

        public const string SaibanCombatSpriteId = "thecat_cat_saiban_combat_sprite_512_v001";
        public const string NephthysCombatSpriteId = "thecat_cat_nephthys_combat_sprite_512_v001";
        public const string SuzuneCombatSpriteId = "thecat_cat_suzune_combat_sprite_512_v001";
        public const string SaibanHudAvatarId = "thecat_cat_saiban_hud_avatar_256_v001";
        public const string NephthysHudAvatarId = "thecat_cat_nephthys_hud_avatar_256_v001";
        public const string SuzuneHudAvatarId = "thecat_cat_suzune_hud_avatar_256_v001";
        public const string BlackMudCombatSpriteId = "thecat_enemy_blackmud_combat_sprite_512_v001";
        public const string ColdLightCombatSpriteId = "thecat_enemy_coldlight_combat_sprite_512_v001";
        public const string OwnerSleepIconId = "thecat_ui_core_sleep_icon_64_v001";
        public const string CatHpIconId = "thecat_ui_core_hp_icon_64_v001";
        public const string TeamPoopIconId = "thecat_ui_core_poop_icon_64_v001";
        public const string TeamHungerIconId = "thecat_ui_core_hunger_icon_64_v001";
        public const string SkillReadyFrameId = "thecat_ui_skill_ready_frame_512_v001";
        public const string SkillCooldownOverlayId = "thecat_ui_skill_cooldown_overlay_512_v001";
        public const string SkillNoTargetMarkerId = "thecat_ui_skill_no_target_marker_512_v001";
        public const string SkillHungerCostChipId = "thecat_ui_skill_hunger_cost_chip_512_v001";
        public const string AutoTargetReticleId = "thecat_ui_auto_target_reticle_512_v001";
        public const string InteractionRangeRippleId = "thecat_ui_interaction_range_ripple_512_v001";
        public const string OwnerSleepGaugeFrameId = "thecat_ui_core_sleep_gauge_frame_384x48_v001";
        public const string OwnerSleepGaugeFillId = "thecat_ui_core_sleep_gauge_fill_384x48_v001";
        public const string CatHpGaugeFrameId = "thecat_ui_core_hp_gauge_frame_384x48_v001";
        public const string CatHpGaugeFillId = "thecat_ui_core_hp_gauge_fill_384x48_v001";
        public const string TeamPoopGaugeFrameId = "thecat_ui_core_poop_gauge_frame_384x48_v001";
        public const string TeamPoopGaugeFillId = "thecat_ui_core_poop_gauge_fill_384x48_v001";
        public const string TeamHungerGaugeFrameId = "thecat_ui_core_hunger_gauge_frame_384x48_v001";
        public const string TeamHungerGaugeFillId = "thecat_ui_core_hunger_gauge_fill_384x48_v001";
        public const string SleepStableStatusIconId = "thecat_ui_status_sleep_stable_64_v001";
        public const string SlowStatusIconId = "thecat_ui_status_slow_64_v001";
        public const string KnockbackStatusIconId = "thecat_ui_status_knockback_64_v001";
        public const string MarkStatusIconId = "thecat_ui_status_mark_64_v001";
        public const string ShieldStatusIconId = "thecat_ui_status_shield_64_v001";
        public const string SleepStableStatusCompactIconId = "thecat_ui_status_sleep_stable_32_v001";
        public const string SlowStatusCompactIconId = "thecat_ui_status_slow_32_v001";
        public const string KnockbackStatusCompactIconId = "thecat_ui_status_knockback_32_v001";
        public const string MarkStatusCompactIconId = "thecat_ui_status_mark_32_v001";
        public const string ShieldStatusCompactIconId = "thecat_ui_status_shield_32_v001";
        public const string BedroomDreamBattleBackgroundId = "thecat_bg_bedroomdream_battle_1920x1080_v001";
        public const string CallTyrantConceptId = "thecat_enemy_calltyrant_concept_2048_v001";
        public const string CallTyrantWarningVfxId = "thecat_vfx_calltyrant_warning_512_v001";
        public const string BlackMudBedClawWarningVfxId = "thecat_vfx_blackmud_bed_claw_256_v001";
        public const string ColdLightBeamWarningVfxId = "thecat_vfx_coldlight_beam_warning_256_v001";
        public const string CallTyrantAppThrowVfxId = "thecat_vfx_calltyrant_app_throw_256_v001";
        public const string CallTyrantSummonPortalVfxId = "thecat_vfx_calltyrant_summon_portal_256_v001";
        public const string BlackMudMoveFramesheetId = "thecat_enemy_blackmud_move_framesheet_4x256_v001";
        public const string ColdLightCastFramesheetId = "thecat_enemy_coldlight_cast_framesheet_4x256_v001";
        public const string CallTyrantBossPatternFramesheetId = "thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001";
        public const string BossRouteNodeIconId = "thecat_ui_route_bossnode_icon_128_v001";
        public const string DefenseRouteNodeIconId = "thecat_ui_route_defense_icon_128_v001";
        public const string EliteRouteNodeIconId = "thecat_ui_route_elite_icon_128_v001";
        public const string PartnerRouteNodeIconId = "thecat_ui_route_partner_icon_128_v001";
        public const string ShopRouteNodeIconId = "thecat_ui_route_shop_icon_128_v001";
        public const string DreamEventRouteNodeIconId = "thecat_ui_route_dreamevent_icon_128_v001";
        public const string BlessingRouteNodeIconId = "thecat_ui_route_blessing_icon_128_v001";
        public const string RestNestRouteNodeIconId = "thecat_ui_route_restnest_icon_128_v001";
        public const string ShopRouteNodeSummaryBannerId = "thecat_ui_node_shop_summary_banner_512x160_v001";
        public const string DreamEventRouteNodeSummaryBannerId = "thecat_ui_node_dreamevent_summary_banner_512x160_v001";
        public const string RestNestRouteNodeSummaryBannerId = "thecat_ui_node_restnest_summary_banner_512x160_v001";
        public const string ShopBedPatchCardId = "thecat_ui_shop_item_bed_patch_card_384x160_v001";
        public const string ShopLitterSachetCardId = "thecat_ui_shop_item_litter_sachet_card_384x160_v001";
        public const string ShopLateKibbleCardId = "thecat_ui_shop_item_late_kibble_card_384x160_v001";
        public const string ShopFreeSampleCardId = "thecat_ui_shop_item_free_sample_card_384x160_v001";
        public const string DreamEventClearNotificationsCardId = "thecat_ui_dreamevent_clear_notifications_card_384x160_v001";
        public const string DreamEventCatnipResidueCardId = "thecat_ui_dreamevent_catnip_residue_card_384x160_v001";
        public const string DreamEventMarkAllReadCardId = "thecat_ui_dreamevent_mark_all_read_card_384x160_v001";
        public const string RestNestRecoveryCardId = "thecat_ui_restnest_recovery_card_384x160_v001";
        public const string PartnerShadowmaruPreviewCardId = "thecat_ui_partner_shadowmaru_preview_card_384x160_v001";
        public const string PartnerDuplicateSupplyCardId = "thecat_ui_partner_duplicate_supply_card_384x160_v001";
        public const string AuthorityOathBedlineCardId = "thecat_ui_blessing_oath_bedline_card_384x160_v001";
        public const string AuthorityDominionSandglassCardId = "thecat_ui_blessing_dominion_sandglass_card_384x160_v001";
        public const string AuthorityRhythmLullabyCardId = "thecat_ui_blessing_rhythm_lullaby_card_384x160_v001";
        public const string BattleResultVictoryBannerId = "thecat_ui_battle_result_victory_banner_512x160_v001";
        public const string BattleResultDefeatBannerId = "thecat_ui_battle_result_defeat_banner_512x160_v001";
        public const string SettlementRunClearedBannerId = "thecat_ui_settlement_run_cleared_banner_512x160_v001";
        public const string SettlementRunFailedBannerId = "thecat_ui_settlement_run_failed_banner_512x160_v001";
        public const string MainMenuDreamEntryBackgroundId = "thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001";
        public const string TitleLogoId = "thecat_ui_title_logo_512x256_v001";
        public const string DreamGlassPanelId = "thecat_ui_panel_dreamglass_512x256_v001";
        public const string PrimaryButtonId = "thecat_ui_button_primary_384x96_v001";
        public const string FishTreatRewardIconId = "thecat_ui_reward_fishtreat_icon_128_v001";
        public const string DreamShardRewardIconId = "thecat_ui_reward_dreamshard_icon_128_v001";
        public const string PartnerRecruitChoiceIconId = "thecat_ui_choice_partner_recruit_icon_128_v001";
        public const string PurchaseSupplyChoiceIconId = "thecat_ui_choice_purchase_supply_icon_128_v001";
        public const string AuthorityBlessingChoiceIconId = "thecat_ui_choice_authority_blessing_icon_128_v001";
        public const string AuthorityUpgradeChoiceIconId = "thecat_ui_choice_authority_upgrade_icon_128_v001";
        public const string RestSupplyChoiceIconId = "thecat_ui_choice_rest_supply_icon_128_v001";
        public const string DreamEventModifierChoiceIconId = "thecat_ui_choice_dreamevent_modifier_icon_128_v001";
        public const string AuthorityOathBedlineSealId = "thecat_ui_blessing_oath_bedline_seal_128_v001";
        public const string AuthorityDominionSandglassSealId = "thecat_ui_blessing_dominion_sandglass_seal_128_v001";
        public const string AuthorityRhythmLullabySealId = "thecat_ui_blessing_rhythm_lullaby_seal_128_v001";
        public const string PartnerRouteCardFrameId = "thecat_ui_routecard_partner_frame_512x256_v001";
        public const string ShopRouteCardFrameId = "thecat_ui_routecard_shop_frame_512x256_v001";
        public const string BlessingRouteCardFrameId = "thecat_ui_routecard_blessing_frame_512x256_v001";
        public const string DreamEventRouteCardFrameId = "thecat_ui_routecard_dreamevent_frame_512x256_v001";
        public const string RestNestRouteCardFrameId = "thecat_ui_routecard_restnest_frame_512x256_v001";
        public const string RouteRewardGainBadgeId = "thecat_ui_reward_detail_gain_badge_192x64_v001";
        public const string RouteRewardCostBadgeId = "thecat_ui_reward_detail_cost_badge_192x64_v001";
        public const string RouteRewardRecoveryBadgeId = "thecat_ui_reward_detail_recovery_badge_192x64_v001";
        public const string RouteRewardRiskBadgeId = "thecat_ui_reward_detail_risk_badge_192x64_v001";
        public const string RouteRewardUpgradeBadgeId = "thecat_ui_reward_detail_upgrade_badge_192x64_v001";
        public const string HitSparkVfxId = "thecat_vfx_hit_spark_256_v001";
        public const string BedShieldPulseVfxId = "thecat_vfx_bed_shield_pulse_256_v001";
        public const string SleepStableWaveVfxId = "thecat_vfx_sleep_stable_wave_256_v001";
        public const string LitterCleanseVfxId = "thecat_vfx_litter_cleanse_256_v001";
        public const string FeederKibbleVfxId = "thecat_vfx_feeder_kibble_256_v001";
        public const string EnemyMarkRingVfxId = "thecat_vfx_enemy_mark_ring_256_v001";
        public const string SaibanBedlineSkillVfxId = "thecat_vfx_saiban_bedline_skill_512_v001";
        public const string NephthysMoonsandSkillVfxId = "thecat_vfx_nephthys_moonsand_skill_512_v001";
        public const string SuzuneLullabySkillVfxId = "thecat_vfx_suzune_lullaby_skill_512_v001";
        public const string BedSpriteId = "thecat_prop_bed_sleepglow_sprite_512_v001";
        public const string LitterBoxSpriteId = "thecat_prop_litterbox_sprite_256_v001";
        public const string FeederSpriteId = "thecat_prop_feeder_sprite_256_v001";

        public static P0VisualAssetReference GetStarterCatCombatSprite(string catId)
        {
            switch (catId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return Find(SaibanCombatSpriteId);
                case P0PrototypeCatalog.NephthysId:
                    return Find(NephthysCombatSpriteId);
                case P0PrototypeCatalog.SuzuneId:
                    return Find(SuzuneCombatSpriteId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetStarterCatHudAvatar(string catId)
        {
            switch (catId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return Find(SaibanHudAvatarId);
                case P0PrototypeCatalog.NephthysId:
                    return Find(NephthysHudAvatarId);
                case P0PrototypeCatalog.SuzuneId:
                    return Find(SuzuneHudAvatarId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetOwnerSleepIcon()
        {
            return Find(OwnerSleepIconId);
        }

        public static P0VisualAssetReference GetEnemyCombatSprite(string enemyId)
        {
            switch (enemyId)
            {
                case P0PrototypeCatalog.BlackMudNightmareId:
                    return Find(BlackMudCombatSpriteId);
                case P0PrototypeCatalog.ColdLightShadowId:
                    return Find(ColdLightCombatSpriteId);
                case P0PrototypeCatalog.CallTyrantId:
                    return Find(CallTyrantConceptId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetCatHpIcon()
        {
            return Find(CatHpIconId);
        }

        public static P0VisualAssetReference GetTeamPoopIcon()
        {
            return Find(TeamPoopIconId);
        }

        public static P0VisualAssetReference GetTeamHungerIcon()
        {
            return Find(TeamHungerIconId);
        }

        public static P0VisualAssetReference GetSkillHudStatusFeedback(string statusToken)
        {
            switch (statusToken)
            {
                case "ready":
                    return Find(SkillReadyFrameId);
                case "cooldown":
                    return Find(SkillCooldownOverlayId);
                case "no_target":
                    return Find(SkillNoTargetMarkerId);
                case "low_hunger":
                    return Find(SkillHungerCostChipId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetSkillHudHungerCostChip()
        {
            return Find(SkillHungerCostChipId);
        }

        public static P0VisualAssetReference GetAutoTargetReticle()
        {
            return Find(AutoTargetReticleId);
        }

        public static P0VisualAssetReference GetInteractionRangeRipple()
        {
            return Find(InteractionRangeRippleId);
        }

        public static P0VisualAssetReference GetCoreGaugeFrame(string subjectId)
        {
            switch (subjectId)
            {
                case "owner_sleep":
                    return Find(OwnerSleepGaugeFrameId);
                case "cat_hp":
                    return Find(CatHpGaugeFrameId);
                case "team_poop":
                    return Find(TeamPoopGaugeFrameId);
                case "team_hunger":
                    return Find(TeamHungerGaugeFrameId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetCoreGaugeFill(string subjectId)
        {
            switch (subjectId)
            {
                case "owner_sleep":
                    return Find(OwnerSleepGaugeFillId);
                case "cat_hp":
                    return Find(CatHpGaugeFillId);
                case "team_poop":
                    return Find(TeamPoopGaugeFillId);
                case "team_hunger":
                    return Find(TeamHungerGaugeFillId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetStatusIcon(string statusTagId)
        {
            switch (statusTagId)
            {
                case StatusTagIds.SleepStable:
                    return Find(SleepStableStatusIconId);
                case StatusTagIds.Slow:
                    return Find(SlowStatusIconId);
                case StatusTagIds.Knockback:
                    return Find(KnockbackStatusIconId);
                case StatusTagIds.Mark:
                    return Find(MarkStatusIconId);
                case StatusTagIds.Shield:
                    return Find(ShieldStatusIconId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetCompactStatusIcon(string statusTagId)
        {
            switch (statusTagId)
            {
                case StatusTagIds.SleepStable:
                    return Find(SleepStableStatusCompactIconId);
                case StatusTagIds.Slow:
                    return Find(SlowStatusCompactIconId);
                case StatusTagIds.Knockback:
                    return Find(KnockbackStatusCompactIconId);
                case StatusTagIds.Mark:
                    return Find(MarkStatusCompactIconId);
                case StatusTagIds.Shield:
                    return Find(ShieldStatusCompactIconId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetCallTyrantConcept()
        {
            return Find(CallTyrantConceptId);
        }

        public static P0VisualAssetReference GetBedroomDreamBattleBackground()
        {
            return Find(BedroomDreamBattleBackgroundId);
        }

        public static P0VisualAssetReference GetMainMenuBackground()
        {
            return Find(MainMenuDreamEntryBackgroundId);
        }

        public static P0VisualAssetReference GetTitleLogo()
        {
            return Find(TitleLogoId);
        }

        public static P0VisualAssetReference GetDreamGlassPanel()
        {
            return Find(DreamGlassPanelId);
        }

        public static P0VisualAssetReference GetPrimaryButton()
        {
            return Find(PrimaryButtonId);
        }

        public static P0VisualAssetReference GetRewardIcon(string rewardId)
        {
            switch (rewardId)
            {
                case "fish_treats":
                    return Find(FishTreatRewardIconId);
                case "dream_shards":
                    return Find(DreamShardRewardIconId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetBattleResultOutcomeBanner(BattleOutcome outcome)
        {
            switch (outcome)
            {
                case BattleOutcome.Victory:
                    return Find(BattleResultVictoryBannerId);
                case BattleOutcome.Defeat:
                    return Find(BattleResultDefeatBannerId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetSettlementOutcomeBanner(bool isCleared)
        {
            return isCleared
                ? Find(SettlementRunClearedBannerId)
                : Find(SettlementRunFailedBannerId);
        }

        public static P0VisualAssetReference GetRouteChoiceIcon(RouteRewardChoiceType choiceType)
        {
            switch (choiceType)
            {
                case RouteRewardChoiceType.GainDreamShards:
                    return Find(DreamShardRewardIconId);
                case RouteRewardChoiceType.GainFishTreats:
                case RouteRewardChoiceType.PurchaseFishTreats:
                    return Find(FishTreatRewardIconId);
                case RouteRewardChoiceType.RecruitPartner:
                    return Find(PartnerRecruitChoiceIconId);
                case RouteRewardChoiceType.PurchaseSupply:
                    return Find(PurchaseSupplyChoiceIconId);
                case RouteRewardChoiceType.GainAuthorityBlessing:
                    return Find(AuthorityBlessingChoiceIconId);
                case RouteRewardChoiceType.UpgradeAuthorityBlessing:
                    return Find(AuthorityUpgradeChoiceIconId);
                case RouteRewardChoiceType.RestSupply:
                    return Find(RestSupplyChoiceIconId);
                case RouteRewardChoiceType.DreamEventModifier:
                case RouteRewardChoiceType.GainEventItem:
                    return Find(DreamEventModifierChoiceIconId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetRouteChoiceIcon(RouteRewardChoice choice)
        {
            if (choice == null)
            {
                return default(P0VisualAssetReference);
            }

            if (choice.AuthorityBlessing != null)
            {
                P0VisualAssetReference seal = GetAuthorityBlessingSeal(choice.AuthorityBlessing.Id);
                if (seal.HasAsset)
                {
                    return seal;
                }
            }

            if (!string.IsNullOrWhiteSpace(choice.AuthorityBlessingUpgradeId))
            {
                P0VisualAssetReference seal = GetAuthorityBlessingSeal(choice.AuthorityBlessingUpgradeId);
                if (seal.HasAsset)
                {
                    return seal;
                }
            }

            return GetRouteChoiceIcon(choice.ChoiceType);
        }

        public static P0VisualAssetReference GetAuthorityBlessingSeal(string blessingId)
        {
            switch (blessingId)
            {
                case P0BlessingCatalog.SaibanBedlineId:
                    return Find(AuthorityOathBedlineSealId);
                case P0BlessingCatalog.NephthysSandglassId:
                    return Find(AuthorityDominionSandglassSealId);
                case P0BlessingCatalog.SuzuneLullabyId:
                    return Find(AuthorityRhythmLullabySealId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetAuthorityBlessingChoiceCard(RouteRewardChoice choice)
        {
            if (choice == null)
            {
                return default(P0VisualAssetReference);
            }

            string blessingId = choice.AuthorityBlessing != null
                ? choice.AuthorityBlessing.Id
                : choice.AuthorityBlessingUpgradeId;

            switch (blessingId)
            {
                case P0BlessingCatalog.SaibanBedlineId:
                    return Find(AuthorityOathBedlineCardId);
                case P0BlessingCatalog.NephthysSandglassId:
                    return Find(AuthorityDominionSandglassCardId);
                case P0BlessingCatalog.SuzuneLullabyId:
                    return Find(AuthorityRhythmLullabyCardId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetRouteRewardCardFrame(RouteNodeType nodeType)
        {
            switch (nodeType)
            {
                case RouteNodeType.Partner:
                    return Find(PartnerRouteCardFrameId);
                case RouteNodeType.Shop:
                    return Find(ShopRouteCardFrameId);
                case RouteNodeType.BlessingOffering:
                    return Find(BlessingRouteCardFrameId);
                case RouteNodeType.DreamEvent:
                    return Find(DreamEventRouteCardFrameId);
                case RouteNodeType.RestNest:
                    return Find(RestNestRouteCardFrameId);
                default:
                    return GetDreamGlassPanel();
            }
        }

        public static P0VisualAssetReference GetRouteRewardDetailBadge(RouteRewardChoice choice)
        {
            if (choice == null)
            {
                return default(P0VisualAssetReference);
            }

            if (!string.IsNullOrWhiteSpace(choice.AuthorityBlessingUpgradeId))
            {
                return Find(RouteRewardUpgradeBadgeId);
            }

            if (choice.NextBattleSkillDamagePercent > 0
                || choice.NextBattlePoopGrowthPercent > 0
                || choice.OwnerSleepDamaged > 0
                || choice.PoopIncreased > 0)
            {
                return Find(RouteRewardRiskBadgeId);
            }

            if (choice.HasDreamShardCost || choice.HasFishTreatCost)
            {
                return Find(RouteRewardCostBadgeId);
            }

            if (choice.OwnerSleepRestored > 0
                || choice.PoopReduced > 0
                || choice.HungerSafeLine > 0
                || choice.CatHpSafePercent > 0)
            {
                return Find(RouteRewardRecoveryBadgeId);
            }

            return Find(RouteRewardGainBadgeId);
        }

        public static P0VisualAssetReference GetHitSparkVfx()
        {
            return Find(HitSparkVfxId);
        }

        public static P0VisualAssetReference GetBedShieldPulseVfx()
        {
            return Find(BedShieldPulseVfxId);
        }

        public static P0VisualAssetReference GetSleepStableWaveVfx()
        {
            return Find(SleepStableWaveVfxId);
        }

        public static P0VisualAssetReference GetLitterCleanseVfx()
        {
            return Find(LitterCleanseVfxId);
        }

        public static P0VisualAssetReference GetFeederKibbleVfx()
        {
            return Find(FeederKibbleVfxId);
        }

        public static P0VisualAssetReference GetEnemyMarkRingVfx()
        {
            return Find(EnemyMarkRingVfxId);
        }

        public static P0VisualAssetReference GetStarterSkillVfx(string skillId)
        {
            switch (skillId)
            {
                case "saiban_oath_shield":
                case "saiban_sword_sweep":
                case "saiban_sun_charge":
                    return Find(SaibanBedlineSkillVfxId);
                case "nephthys_moon_sand_obelisk":
                case "nephthys_quicksand_trap":
                case "nephthys_royal_mark":
                    return Find(NephthysMoonsandSkillVfxId);
                case "suzune_sleep_bell":
                case "suzune_healing_bell":
                case "suzune_moon_torii":
                    return Find(SuzuneLullabySkillVfxId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetCallTyrantWarningVfx()
        {
            return Find(CallTyrantWarningVfxId);
        }

        public static P0VisualAssetReference GetBlackMudBedClawWarningVfx()
        {
            return Find(BlackMudBedClawWarningVfxId);
        }

        public static P0VisualAssetReference GetColdLightBeamWarningVfx()
        {
            return Find(ColdLightBeamWarningVfxId);
        }

        public static P0VisualAssetReference GetCallTyrantAppThrowVfx()
        {
            return Find(CallTyrantAppThrowVfxId);
        }

        public static P0VisualAssetReference GetCallTyrantSummonPortalVfx()
        {
            return Find(CallTyrantSummonPortalVfxId);
        }

        public static P0VisualAssetReference GetEnemyAnimationFramesheet(string enemyId)
        {
            switch (enemyId)
            {
                case P0PrototypeCatalog.BlackMudNightmareId:
                    return Find(BlackMudMoveFramesheetId);
                case P0PrototypeCatalog.ColdLightShadowId:
                    return Find(ColdLightCastFramesheetId);
                case P0PrototypeCatalog.CallTyrantId:
                    return Find(CallTyrantBossPatternFramesheetId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetBossRouteNodeIcon()
        {
            return Find(BossRouteNodeIconId);
        }

        public static P0VisualAssetReference GetRouteNodeIcon(RouteNodeType nodeType)
        {
            switch (nodeType)
            {
                case RouteNodeType.Defense:
                    return Find(DefenseRouteNodeIconId);
                case RouteNodeType.Elite:
                    return Find(EliteRouteNodeIconId);
                case RouteNodeType.Partner:
                    return Find(PartnerRouteNodeIconId);
                case RouteNodeType.Shop:
                    return Find(ShopRouteNodeIconId);
                case RouteNodeType.DreamEvent:
                    return Find(DreamEventRouteNodeIconId);
                case RouteNodeType.BlessingOffering:
                    return Find(BlessingRouteNodeIconId);
                case RouteNodeType.RestNest:
                    return Find(RestNestRouteNodeIconId);
                case RouteNodeType.Boss:
                    return GetBossRouteNodeIcon();
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetRouteNodeSummaryBanner(RouteNodeType nodeType)
        {
            switch (nodeType)
            {
                case RouteNodeType.Shop:
                    return Find(ShopRouteNodeSummaryBannerId);
                case RouteNodeType.DreamEvent:
                    return Find(DreamEventRouteNodeSummaryBannerId);
                case RouteNodeType.RestNest:
                    return Find(RestNestRouteNodeSummaryBannerId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetShopItemCard(RouteRewardChoice choice)
        {
            if (choice == null)
            {
                return default(P0VisualAssetReference);
            }

            switch (choice.Id)
            {
                case "shop_bed_patch":
                    return Find(ShopBedPatchCardId);
                case "shop_litter_sachet":
                    return Find(ShopLitterSachetCardId);
                case "shop_late_kibble":
                    return Find(ShopLateKibbleCardId);
                case "shop_free_sample":
                    return Find(ShopFreeSampleCardId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetDreamEventChoiceCard(RouteRewardChoice choice)
        {
            if (choice == null)
            {
                return default(P0VisualAssetReference);
            }

            switch (choice.Id)
            {
                case "dream_event_clear_notifications":
                case "dream_event_red_dot_cleanup":
                    return Find(DreamEventClearNotificationsCardId);
                case "dream_event_catnip_residue":
                case "dream_event_red_dot_ignore":
                    return Find(DreamEventCatnipResidueCardId);
                case "dream_event_mark_all_read":
                case "dream_event_blank_wish_extra":
                case "dream_event_red_dot_mute_thread":
                    return Find(DreamEventMarkAllReadCardId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetRestNestChoiceCard(RouteRewardChoice choice)
        {
            if (choice == null)
            {
                return default(P0VisualAssetReference);
            }

            switch (choice.Id)
            {
                case "rest_nest_recovery":
                    return Find(RestNestRecoveryCardId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetPartnerChoiceCard(RouteRewardChoice choice)
        {
            if (choice == null)
            {
                return default(P0VisualAssetReference);
            }

            switch (choice.Id)
            {
                case "partner_shadowmaru_preview":
                    return Find(PartnerShadowmaruPreviewCardId);
                case "partner_preview_duplicate_supply":
                    return Find(PartnerDuplicateSupplyCardId);
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetRouteChoiceCard(RouteRewardChoice choice)
        {
            P0VisualAssetReference shopCard = GetShopItemCard(choice);
            if (shopCard.HasAsset)
            {
                return shopCard;
            }

            P0VisualAssetReference dreamEventCard = GetDreamEventChoiceCard(choice);
            if (dreamEventCard.HasAsset)
            {
                return dreamEventCard;
            }

            P0VisualAssetReference partnerCard = GetPartnerChoiceCard(choice);
            if (partnerCard.HasAsset)
            {
                return partnerCard;
            }

            P0VisualAssetReference restNestCard = GetRestNestChoiceCard(choice);
            if (restNestCard.HasAsset)
            {
                return restNestCard;
            }

            return GetAuthorityBlessingChoiceCard(choice);
        }

        public static P0VisualAssetReference GetBedSprite()
        {
            return Find(BedSpriteId);
        }

        public static P0VisualAssetReference GetLitterBoxSprite()
        {
            return Find(LitterBoxSpriteId);
        }

        public static P0VisualAssetReference GetFeederSprite()
        {
            return Find(FeederSpriteId);
        }

        public static P0VisualAssetReference GetInteractableSprite(string subjectId)
        {
            switch (subjectId)
            {
                case "bed":
                    return GetBedSprite();
                case "litter_box":
                    return GetLitterBoxSprite();
                case "feeder":
                    return GetFeederSprite();
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetReference GetCoreValueIcon(string subjectId)
        {
            switch (subjectId)
            {
                case "owner_sleep":
                    return GetOwnerSleepIcon();
                case "cat_hp":
                    return GetCatHpIcon();
                case "team_poop":
                    return GetTeamPoopIcon();
                case "team_hunger":
                    return GetTeamHungerIcon();
                default:
                    return default(P0VisualAssetReference);
            }
        }

        public static P0VisualAssetBinding[] CreateP0RuntimeBindings()
        {
            return new[]
            {
                Binding("background.bedroom_dream", "battle_world", "bedroom_dream_battle", "background", GetBedroomDreamBattleBackground(), "bedroom_map_concept source lock"),
                Binding("cat.combat.saiban", "cat_hud", P0PrototypeCatalog.SaibanId, "combat_sprite", GetStarterCatCombatSprite(P0PrototypeCatalog.SaibanId), "saiban colored turnaround source lock"),
                Binding("cat.combat.nephthys", "cat_hud", P0PrototypeCatalog.NephthysId, "combat_sprite", GetStarterCatCombatSprite(P0PrototypeCatalog.NephthysId), "nephthys colored turnaround source lock"),
                Binding("cat.combat.suzune", "cat_hud", P0PrototypeCatalog.SuzuneId, "combat_sprite", GetStarterCatCombatSprite(P0PrototypeCatalog.SuzuneId), "suzune colored turnaround source lock"),
                Binding("cat.avatar.saiban", "cat_hud", P0PrototypeCatalog.SaibanId, "avatar_icon", GetStarterCatHudAvatar(P0PrototypeCatalog.SaibanId), "saiban colored turnaround source-locked Batch 58 HUD avatar"),
                Binding("cat.avatar.nephthys", "cat_hud", P0PrototypeCatalog.NephthysId, "avatar_icon", GetStarterCatHudAvatar(P0PrototypeCatalog.NephthysId), "nephthys colored turnaround source-locked Batch 58 HUD avatar"),
                Binding("cat.avatar.suzune", "cat_hud", P0PrototypeCatalog.SuzuneId, "avatar_icon", GetStarterCatHudAvatar(P0PrototypeCatalog.SuzuneId), "suzune colored turnaround source-locked Batch 58 HUD avatar"),
                Binding("enemy.combat.black_mud", "battle_world", P0PrototypeCatalog.BlackMudNightmareId, "combat_sprite", GetEnemyCombatSprite(P0PrototypeCatalog.BlackMudNightmareId), "black_mud_concept source lock"),
                Binding("enemy.combat.cold_light", "battle_world", P0PrototypeCatalog.ColdLightShadowId, "combat_sprite", GetEnemyCombatSprite(P0PrototypeCatalog.ColdLightShadowId), "cold_light_concept source lock"),
                Binding("enemy.combat.call_tyrant", "battle_world", P0PrototypeCatalog.CallTyrantId, "combat_visual", GetEnemyCombatSprite(P0PrototypeCatalog.CallTyrantId), "call_tyrant_concept source lock"),
                Binding("enemy.anim.black_mud_move", "battle_world", P0PrototypeCatalog.BlackMudNightmareId, "animation_framesheet", GetEnemyAnimationFramesheet(P0PrototypeCatalog.BlackMudNightmareId), "black_mud_animation source lock"),
                Binding("enemy.anim.cold_light_cast", "battle_world", P0PrototypeCatalog.ColdLightShadowId, "animation_framesheet", GetEnemyAnimationFramesheet(P0PrototypeCatalog.ColdLightShadowId), "cold_light_animation source lock"),
                Binding("enemy.anim.call_tyrant_boss_pattern", "battle_world", P0PrototypeCatalog.CallTyrantId, "animation_framesheet", GetEnemyAnimationFramesheet(P0PrototypeCatalog.CallTyrantId), "call_tyrant_animation source lock"),
                Binding("prop.bed", "battle_world", "bed", "interactable_sprite", GetBedSprite(), "bedroom_map_concept source lock"),
                Binding("prop.litter_box", "battle_world", "litter_box", "interactable_sprite", GetLitterBoxSprite(), "bedroom_mid_background_sprites source lock"),
                Binding("prop.feeder", "battle_world", "feeder", "interactable_sprite", GetFeederSprite(), "bedroom_mid_background_sprites source lock"),
                Binding("core.owner_sleep", "battle_hud", "owner_sleep", "core_icon", GetOwnerSleepIcon(), "status icon style anchor"),
                Binding("core.cat_hp", "cat_hud", "cat_hp", "core_icon", GetCatHpIcon(), "status icon style anchor"),
                Binding("core.team_poop", "battle_hud", "team_poop", "core_icon", GetTeamPoopIcon(), "status icon style anchor"),
                Binding("core.team_hunger", "battle_hud", "team_hunger", "core_icon", GetTeamHungerIcon(), "status icon style anchor"),
                Binding("skill_hud.ready_frame", "skill_hud", "ready", "ready_frame", GetSkillHudStatusFeedback("ready"), "Batch 60 non-cat Skill HUD feedback install"),
                Binding("skill_hud.cooldown_overlay", "skill_hud", "cooldown", "cooldown_overlay", GetSkillHudStatusFeedback("cooldown"), "Batch 60 non-cat Skill HUD feedback install"),
                Binding("skill_hud.no_target_marker", "skill_hud", "no_target", "no_target_marker", GetSkillHudStatusFeedback("no_target"), "Batch 60 non-cat Skill HUD feedback install"),
                Binding("skill_hud.hunger_cost_chip", "skill_hud", "low_hunger", "hunger_cost_chip", GetSkillHudHungerCostChip(), "Batch 60 non-cat Skill HUD feedback install"),
                Binding("skill_hud.auto_target_reticle", "skill_hud", "auto_target", "auto_target_reticle", GetAutoTargetReticle(), "Batch 60 non-cat Skill HUD feedback install"),
                Binding("battle_hud.interaction_range_ripple", "battle_hud", "interaction_range", "interaction_range_ripple", GetInteractionRangeRipple(), "Batch 60 non-cat Skill HUD feedback install"),
                Binding("skill_vfx.saiban_bedline", "battle_feedback", P0PrototypeCatalog.SaibanId, "starter_skill_vfx", GetStarterSkillVfx("saiban_oath_shield"), "Batch 61 symbolic starter skill VFX; Saiban colored-turnaround source lock is authority-symbol only"),
                Binding("skill_vfx.nephthys_moonsand", "battle_feedback", P0PrototypeCatalog.NephthysId, "starter_skill_vfx", GetStarterSkillVfx("nephthys_moon_sand_obelisk"), "Batch 61 symbolic starter skill VFX; Nephthys colored-turnaround source lock is authority-symbol only"),
                Binding("skill_vfx.suzune_lullaby", "battle_feedback", P0PrototypeCatalog.SuzuneId, "starter_skill_vfx", GetStarterSkillVfx("suzune_sleep_bell"), "Batch 61 symbolic starter skill VFX; Suzune colored-turnaround source lock is authority-symbol only"),
                Binding("core_gauge.owner_sleep.frame", "battle_hud", "owner_sleep", "gauge_frame", GetCoreGaugeFrame("owner_sleep"), "core gauge bar batch 27"),
                Binding("core_gauge.owner_sleep.fill", "battle_hud", "owner_sleep", "gauge_fill", GetCoreGaugeFill("owner_sleep"), "core gauge bar batch 27"),
                Binding("core_gauge.cat_hp.frame", "cat_hud", "cat_hp", "gauge_frame", GetCoreGaugeFrame("cat_hp"), "core gauge bar batch 27"),
                Binding("core_gauge.cat_hp.fill", "cat_hud", "cat_hp", "gauge_fill", GetCoreGaugeFill("cat_hp"), "core gauge bar batch 27"),
                Binding("core_gauge.team_poop.frame", "battle_hud", "team_poop", "gauge_frame", GetCoreGaugeFrame("team_poop"), "core gauge bar batch 27"),
                Binding("core_gauge.team_poop.fill", "battle_hud", "team_poop", "gauge_fill", GetCoreGaugeFill("team_poop"), "core gauge bar batch 27"),
                Binding("core_gauge.team_hunger.frame", "battle_hud", "team_hunger", "gauge_frame", GetCoreGaugeFrame("team_hunger"), "core gauge bar batch 27"),
                Binding("core_gauge.team_hunger.fill", "battle_hud", "team_hunger", "gauge_fill", GetCoreGaugeFill("team_hunger"), "core gauge bar batch 27"),
                Binding("status.sleep_stable", "status_hud", StatusTagIds.SleepStable, "status_icon", GetStatusIcon(StatusTagIds.SleepStable), "status icon style anchor cell 0"),
                Binding("status.slow", "status_hud", StatusTagIds.Slow, "status_icon", GetStatusIcon(StatusTagIds.Slow), "status icon style anchor cell 1"),
                Binding("status.knockback", "status_hud", StatusTagIds.Knockback, "status_icon", GetStatusIcon(StatusTagIds.Knockback), "status icon style anchor cell 2"),
                Binding("status.mark", "status_hud", StatusTagIds.Mark, "status_icon", GetStatusIcon(StatusTagIds.Mark), "status icon style anchor cell 3"),
                Binding("status.shield", "status_hud", StatusTagIds.Shield, "status_icon", GetStatusIcon(StatusTagIds.Shield), "status icon style anchor cell 4"),
                Binding("status_compact.sleep_stable", "status_hud", StatusTagIds.SleepStable, "compact_status_icon", GetCompactStatusIcon(StatusTagIds.SleepStable), "status compact icon batch 14"),
                Binding("status_compact.slow", "status_hud", StatusTagIds.Slow, "compact_status_icon", GetCompactStatusIcon(StatusTagIds.Slow), "status compact icon batch 14"),
                Binding("status_compact.knockback", "status_hud", StatusTagIds.Knockback, "compact_status_icon", GetCompactStatusIcon(StatusTagIds.Knockback), "status compact icon batch 14"),
                Binding("status_compact.mark", "status_hud", StatusTagIds.Mark, "compact_status_icon", GetCompactStatusIcon(StatusTagIds.Mark), "status compact icon batch 14"),
                Binding("status_compact.shield", "status_hud", StatusTagIds.Shield, "compact_status_icon", GetCompactStatusIcon(StatusTagIds.Shield), "status compact icon batch 14"),
                Binding("route.defense_node", "route_map", "defense_route_node", "node_icon", GetRouteNodeIcon(RouteNodeType.Defense), "route node icon batch 06 defense"),
                Binding("route.elite_node", "route_map", "elite_route_node", "node_icon", GetRouteNodeIcon(RouteNodeType.Elite), "route node icon batch 06 elite"),
                Binding("route.partner_node", "route_map", "partner_route_node", "node_icon", GetRouteNodeIcon(RouteNodeType.Partner), "route node icon batch 06 partner"),
                Binding("route.shop_node", "route_map", "shop_route_node", "node_icon", GetRouteNodeIcon(RouteNodeType.Shop), "route node icon batch 06 shop"),
                Binding("route.dream_event_node", "route_map", "dream_event_route_node", "node_icon", GetRouteNodeIcon(RouteNodeType.DreamEvent), "route node icon batch 06 dream event"),
                Binding("route.blessing_node", "route_map", "blessing_route_node", "node_icon", GetRouteNodeIcon(RouteNodeType.BlessingOffering), "route node icon batch 06 blessing offering"),
                Binding("route.rest_nest_node", "route_map", "rest_nest_route_node", "node_icon", GetRouteNodeIcon(RouteNodeType.RestNest), "route node icon batch 06 rest nest"),
                Binding("route.boss_node", "route_map", "boss_route_node", "node_icon", GetBossRouteNodeIcon(), "call_tyrant_concept source lock"),
                Binding("route_summary.shop", "route_node_summary", "shop_route_node", "summary_banner", GetRouteNodeSummaryBanner(RouteNodeType.Shop), "non-battle node summary banner batch 19"),
                Binding("route_summary.dream_event", "route_node_summary", "dream_event_route_node", "summary_banner", GetRouteNodeSummaryBanner(RouteNodeType.DreamEvent), "non-battle node summary banner batch 19"),
                Binding("route_summary.rest_nest", "route_node_summary", "rest_nest_route_node", "summary_banner", GetRouteNodeSummaryBanner(RouteNodeType.RestNest), "non-battle node summary banner batch 19"),
                Binding("shop_item.bed_patch", "shop_item_card", "shop_bed_patch", "item_card", Find(ShopBedPatchCardId), "shop item card batch 20"),
                Binding("shop_item.litter_sachet", "shop_item_card", "shop_litter_sachet", "item_card", Find(ShopLitterSachetCardId), "shop item card batch 20"),
                Binding("shop_item.late_kibble", "shop_item_card", "shop_late_kibble", "item_card", Find(ShopLateKibbleCardId), "shop item card batch 20"),
                Binding("shop_item.free_sample", "shop_item_card", "shop_free_sample", "item_card", Find(ShopFreeSampleCardId), "shop item card batch 20"),
                Binding("dream_event_choice.clear_notifications", "dream_event_choice_card", "dream_event_clear_notifications", "choice_card", Find(DreamEventClearNotificationsCardId), "dream event choice card batch 21"),
                Binding("dream_event_choice.catnip_residue", "dream_event_choice_card", "dream_event_catnip_residue", "choice_card", Find(DreamEventCatnipResidueCardId), "dream event choice card batch 21"),
                Binding("dream_event_choice.mark_all_read", "dream_event_choice_card", "dream_event_mark_all_read", "choice_card", Find(DreamEventMarkAllReadCardId), "dream event choice card batch 21"),
                Binding("rest_nest_choice.recovery", "rest_nest_choice_card", "rest_nest_recovery", "choice_card", Find(RestNestRecoveryCardId), "rest nest recovery card batch 22"),
                Binding("partner_choice.shadowmaru_preview", "partner_choice_card", "partner_shadowmaru_preview", "choice_card", Find(PartnerShadowmaruPreviewCardId), "partner choice card batch 23"),
                Binding("partner_choice.duplicate_supply", "partner_choice_card", "partner_preview_duplicate_supply", "choice_card", Find(PartnerDuplicateSupplyCardId), "partner choice card batch 23"),
                Binding("blessing_choice.oath_bedline", "blessing_choice_card", P0BlessingCatalog.SaibanBedlineId, "choice_card", Find(AuthorityOathBedlineCardId), "blessing choice card batch 24"),
                Binding("blessing_choice.dominion_sandglass", "blessing_choice_card", P0BlessingCatalog.NephthysSandglassId, "choice_card", Find(AuthorityDominionSandglassCardId), "blessing choice card batch 24"),
                Binding("blessing_choice.rhythm_lullaby", "blessing_choice_card", P0BlessingCatalog.SuzuneLullabyId, "choice_card", Find(AuthorityRhythmLullabyCardId), "blessing choice card batch 24"),
                Binding("warning.call_tyrant", "battle_warning", "call_tyrant_warning", "warning_vfx", GetCallTyrantWarningVfx(), "call_tyrant_animation source lock"),
                Binding("warning.black_mud_bed_claw", "battle_warning", "black_mud_bed_claw", "warning_vfx", GetBlackMudBedClawWarningVfx(), "black_mud_animation source lock"),
                Binding("warning.cold_light_beam", "battle_warning", "cold_light_beam_warning", "warning_vfx", GetColdLightBeamWarningVfx(), "cold_light_animation source lock"),
                Binding("warning.call_tyrant_app_throw", "battle_warning", "call_tyrant_app_throw", "warning_vfx", GetCallTyrantAppThrowVfx(), "call_tyrant_animation source lock"),
                Binding("warning.call_tyrant_summon_portal", "battle_warning", "call_tyrant_summon_portal", "warning_vfx", GetCallTyrantSummonPortalVfx(), "call_tyrant_animation source lock"),
                Binding("feedback.hit_spark", "battle_feedback", "hit_spark", "feedback_vfx", GetHitSparkVfx(), "battle feedback vfx batch 09"),
                Binding("feedback.bed_shield_pulse", "battle_feedback", "bed_shield_pulse", "feedback_vfx", GetBedShieldPulseVfx(), "battle feedback vfx batch 09"),
                Binding("feedback.sleep_stable_wave", "battle_feedback", "sleep_stable_wave", "feedback_vfx", GetSleepStableWaveVfx(), "battle feedback vfx batch 09"),
                Binding("feedback.litter_cleanse", "battle_feedback", "litter_cleanse", "feedback_vfx", GetLitterCleanseVfx(), "battle feedback vfx batch 09"),
                Binding("feedback.feeder_kibble", "battle_feedback", "feeder_kibble", "feedback_vfx", GetFeederKibbleVfx(), "battle feedback vfx batch 09"),
                Binding("feedback.enemy_mark_ring", "battle_feedback", "enemy_mark_ring", "feedback_vfx", GetEnemyMarkRingVfx(), "battle feedback vfx batch 09"),
                Binding("main_menu.background", "main_menu", "main_menu", "background", GetMainMenuBackground(), "ui shell batch 08"),
                Binding("main_menu.title_logo", "main_menu", "game_title", "logo", GetTitleLogo(), "ui shell batch 08"),
                Binding("ui.panel.dreamglass", "ui_shell", "dreamglass_panel", "panel_frame", GetDreamGlassPanel(), "ui shell batch 08"),
                Binding("ui.button.primary", "ui_shell", "primary_button", "button_frame", GetPrimaryButton(), "ui shell batch 08"),
                Binding("settlement.reward.fish_treat", "settlement", "fish_treats", "reward_icon", GetRewardIcon("fish_treats"), "ui shell batch 08"),
                Binding("settlement.reward.dream_shard", "settlement", "dream_shards", "reward_icon", GetRewardIcon("dream_shards"), "ui shell batch 08"),
                Binding("battle_result.victory_banner", "battle_result", "victory", "outcome_banner", GetBattleResultOutcomeBanner(BattleOutcome.Victory), "result settlement banner batch 25"),
                Binding("battle_result.defeat_banner", "battle_result", "defeat", "outcome_banner", GetBattleResultOutcomeBanner(BattleOutcome.Defeat), "result settlement banner batch 25"),
                Binding("settlement.run_cleared_banner", "settlement", "run_cleared", "outcome_banner", GetSettlementOutcomeBanner(true), "result settlement banner batch 25"),
                Binding("settlement.run_failed_banner", "settlement", "run_failed", "outcome_banner", GetSettlementOutcomeBanner(false), "result settlement banner batch 25"),
                Binding("route_choice.partner_recruit", "route_choice", "partner_recruit_choice", "choice_icon", GetRouteChoiceIcon(RouteRewardChoiceType.RecruitPartner), "route choice icon batch 12"),
                Binding("route_choice.purchase_supply", "route_choice", "purchase_supply_choice", "choice_icon", GetRouteChoiceIcon(RouteRewardChoiceType.PurchaseSupply), "route choice icon batch 12"),
                Binding("route_choice.authority_blessing", "route_choice", "authority_blessing_choice", "choice_icon", GetRouteChoiceIcon(RouteRewardChoiceType.GainAuthorityBlessing), "route choice icon batch 12"),
                Binding("route_choice.authority_upgrade", "route_choice", "authority_upgrade_choice", "choice_icon", GetRouteChoiceIcon(RouteRewardChoiceType.UpgradeAuthorityBlessing), "route choice icon batch 12"),
                Binding("route_choice.rest_supply", "route_choice", "rest_supply_choice", "choice_icon", GetRouteChoiceIcon(RouteRewardChoiceType.RestSupply), "route choice icon batch 12"),
                Binding("route_choice.dream_event_modifier", "route_choice", "dream_event_modifier_choice", "choice_icon", GetRouteChoiceIcon(RouteRewardChoiceType.DreamEventModifier), "route choice icon batch 12"),
                Binding("route_reward_card.partner", "route_reward_card", "partner_route_card", "card_frame", GetRouteRewardCardFrame(RouteNodeType.Partner), "route reward card frame batch 13"),
                Binding("route_reward_card.shop", "route_reward_card", "shop_route_card", "card_frame", GetRouteRewardCardFrame(RouteNodeType.Shop), "route reward card frame batch 13"),
                Binding("route_reward_card.blessing", "route_reward_card", "blessing_route_card", "card_frame", GetRouteRewardCardFrame(RouteNodeType.BlessingOffering), "route reward card frame batch 13"),
                Binding("route_reward_card.dream_event", "route_reward_card", "dream_event_route_card", "card_frame", GetRouteRewardCardFrame(RouteNodeType.DreamEvent), "route reward card frame batch 13"),
                Binding("route_reward_card.rest_nest", "route_reward_card", "rest_nest_route_card", "card_frame", GetRouteRewardCardFrame(RouteNodeType.RestNest), "route reward card frame batch 13"),
                Binding("route_reward_detail.gain", "route_reward_detail", "gain_detail", "detail_badge", Find(RouteRewardGainBadgeId), "route reward detail badge batch 15"),
                Binding("route_reward_detail.cost", "route_reward_detail", "cost_detail", "detail_badge", Find(RouteRewardCostBadgeId), "route reward detail badge batch 15"),
                Binding("route_reward_detail.recovery", "route_reward_detail", "recovery_detail", "detail_badge", Find(RouteRewardRecoveryBadgeId), "route reward detail badge batch 15"),
                Binding("route_reward_detail.risk", "route_reward_detail", "risk_detail", "detail_badge", Find(RouteRewardRiskBadgeId), "route reward detail badge batch 15"),
                Binding("route_reward_detail.upgrade", "route_reward_detail", "upgrade_detail", "detail_badge", Find(RouteRewardUpgradeBadgeId), "route reward detail badge batch 15"),
                Binding("blessing_detail.oath_bedline", "blessing_detail", P0BlessingCatalog.SaibanBedlineId, "blessing_seal", GetAuthorityBlessingSeal(P0BlessingCatalog.SaibanBedlineId), "authority blessing seal batch 16"),
                Binding("blessing_detail.dominion_sandglass", "blessing_detail", P0BlessingCatalog.NephthysSandglassId, "blessing_seal", GetAuthorityBlessingSeal(P0BlessingCatalog.NephthysSandglassId), "authority blessing seal batch 16"),
                Binding("blessing_detail.rhythm_lullaby", "blessing_detail", P0BlessingCatalog.SuzuneLullabyId, "blessing_seal", GetAuthorityBlessingSeal(P0BlessingCatalog.SuzuneLullabyId), "authority blessing seal batch 16")
            };
        }

        public static P0VisualAssetReference Find(string assetId)
        {
            if (string.IsNullOrWhiteSpace(assetId))
            {
                return default(P0VisualAssetReference);
            }

            foreach (P0AssetManifestEntry entry in P0AssetManifestCatalog.CreateP0PlannedManifest())
            {
                if (entry.AssetId == assetId)
                {
                    return new P0VisualAssetReference(
                        entry.AssetId,
                        entry.UnityImportPath,
                        entry.AssetType,
                        entry.Status,
                        entry.ReferenceAssetIds,
                        entry.SourceLockIds);
                }
            }

            return default(P0VisualAssetReference);
        }

        private static P0VisualAssetBinding Binding(
            string bindingId,
            string surfaceId,
            string subjectId,
            string slotId,
            P0VisualAssetReference asset,
            string sourceAuthority)
        {
            return new P0VisualAssetBinding(bindingId, surfaceId, subjectId, slotId, asset, sourceAuthority);
        }
    }
}
