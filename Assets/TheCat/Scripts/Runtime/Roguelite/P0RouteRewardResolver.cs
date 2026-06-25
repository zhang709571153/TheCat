using System;
using System.Collections.Generic;

namespace TheCat.Roguelite
{
    public static class P0RouteRewardResolver
    {
        public const string PreviewPartnerId = "shadowmaru_preview";
        public const int ShopAuthorityBlessingUpgradeCost = 4;

        public static RouteBattleReward ApplyBattleReward(RouteNodeDefinition node, RunProgressionState run)
        {
            if (node == null || run == null)
            {
                return RouteBattleReward.None;
            }

            RouteBattleReward reward = PreviewBattleReward(node);
            run.Wallet.AddDreamShards(reward.DreamShards);
            run.Wallet.AddFishTreats(reward.FishTreats);
            run.CatUpgrades.GrantExperience(reward.TeamExperience, run.Roster);
            return reward;
        }

        public static RouteBattleReward PreviewBattleReward(RouteNodeDefinition node)
        {
            if (node == null)
            {
                return RouteBattleReward.None;
            }

            switch (node.NodeType)
            {
                case RouteNodeType.Elite:
                    return new RouteBattleReward(
                        dreamShards: 2,
                        fishTreats: 1,
                        teamExperience: RunCatUpgradeState.EliteExperience);
                case RouteNodeType.Boss:
                    return new RouteBattleReward(
                        dreamShards: 5,
                        fishTreats: 3,
                        teamExperience: RunCatUpgradeState.BossExperience);
                case RouteNodeType.Defense:
                    return new RouteBattleReward(
                        dreamShards: 0,
                        fishTreats: 1,
                        teamExperience: RunCatUpgradeState.DefenseExperience);
                default:
                    return RouteBattleReward.None;
            }
        }

        public static void ApplyPlaceholderReward(RouteNodeDefinition node, RunProgressionState run)
        {
            if (node == null || run == null)
            {
                return;
            }

            RouteRewardChoice choice = GetDefaultPlaceholderChoice(node, run);
            if (choice != null)
            {
                ApplyPlaceholderChoice(node, run, choice);
            }
        }

        public static IReadOnlyList<RouteRewardChoice> CreatePlaceholderChoices(RouteNodeDefinition node, RunProgressionState run)
        {
            if (node == null)
            {
                return Array.Empty<RouteRewardChoice>();
            }

            if (node.NodeType == RouteNodeType.DreamEvent)
            {
                return CreateDreamEventChoices(node, run);
            }

            switch (node.NodeType)
            {
                case RouteNodeType.DreamEvent:
                    return new[]
                    {
                        new RouteRewardChoice(
                            "dream_event_clear_notifications",
                            "清除红点",
                            "扫掉未读红点雨，获得少量小鱼干。",
                            RouteRewardChoiceType.GainFishTreats,
                            fishTreatsGained: 2),
                        new RouteRewardChoice(
                            "dream_event_catnip_residue",
                            "吸入猫薄荷残响",
                            "下一场战斗技能更强，但屎意压力上升更快。",
                            RouteRewardChoiceType.DreamEventModifier,
                            nextBattleSkillDamagePercent: 20,
                            nextBattlePoopGrowthPercent: 50),
                        new RouteRewardChoice(
                            "dream_event_mark_all_read",
                            "全部标为已读",
                            "清理梦里的消息噪声，稳定床边睡眠。",
                            RouteRewardChoiceType.GainFishTreats,
                            ownerSleepRestored: 12)
                    };
                case RouteNodeType.Partner:
                    if (run != null && run.Roster.HasCat(PreviewPartnerId))
                    {
                        return new[]
                        {
                            new RouteRewardChoice(
                                "partner_preview_duplicate_supply",
                                "分享夜鱼",
                                "预览伙伴已在队伍中，本节点改为给予资源。",
                                RouteRewardChoiceType.GainFishTreats,
                                fishTreatsGained: 2)
                        };
                    }

                    return new[]
                    {
                        new RouteRewardChoice(
                            "partner_shadowmaru_preview",
                            "邀请影丸",
                            "让预览伙伴加入本轮队伍，并获得一份小鱼干。",
                            RouteRewardChoiceType.RecruitPartner,
                            fishTreatsGained: 1,
                            partnerCatId: PreviewPartnerId,
                            nextBattleShieldPercent: 10,
                            nextBattleStatusDurationPercent: 10,
                            nextBattleOwnerSleepRestorePercent: 10,
                            nextBattleCatHealPercent: 10)
                    };
                case RouteNodeType.Shop:
                    return CreateShopChoices(run);
                case RouteNodeType.BlessingOffering:
                    return CreateBlessingChoices(run);
                case RouteNodeType.RestNest:
                    return new[]
                    {
                        new RouteRewardChoice(
                            "rest_nest_recovery",
                            "修补床线",
                            "借安静猫窝恢复本轮的睡眠、屎意和饱肚压力。",
                            RouteRewardChoiceType.RestSupply,
                            ownerSleepRestored: (int)RunCoreValues.RestNestOwnerSleepRestoreAmount,
                            poopReduced: (int)RunCoreValues.RestNestPoopReductionAmount,
                            hungerSafeLine: (int)RunCoreValues.RestNestHungerSafeLine,
                            catHpSafePercent: (int)(RunCatVitals.RestNestHpSafeRatio * 100f))
                    };
                default:
                    return Array.Empty<RouteRewardChoice>();
            }
        }

        public static RouteRewardChoice GetDefaultPlaceholderChoice(RouteNodeDefinition node, RunProgressionState run)
        {
            IReadOnlyList<RouteRewardChoice> choices = CreatePlaceholderChoices(node, run);
            return choices.Count == 0 ? null : choices[0];
        }

        public static bool ApplyPlaceholderChoice(RouteNodeDefinition node, RunProgressionState run, RouteRewardChoice choice)
        {
            if (node == null || run == null || choice == null)
            {
                return false;
            }

            if (!IsChoiceAvailableForNode(node, run, choice))
            {
                return false;
            }

            if (choice.DreamShardsCost > run.Wallet.DreamShards)
            {
                return false;
            }

            if (choice.FishTreatsCost > run.Wallet.FishTreats)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(choice.PartnerCatId) && run.Roster.HasCat(choice.PartnerCatId))
            {
                return false;
            }

            if (choice.AuthorityBlessing != null && run.Blessings.Has(choice.AuthorityBlessing.Id))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(choice.AuthorityBlessingUpgradeId)
                && !run.Blessings.CanUpgrade(choice.AuthorityBlessingUpgradeId))
            {
                return false;
            }

            bool hadBlankWishTagBefore = node.NodeType == RouteNodeType.DreamEvent
                && run.EventItems.Has(RunEventItemInventory.BlankWishTagId);

            if (choice.HasDreamShardCost && !run.Wallet.SpendDreamShards(choice.DreamShardsCost))
            {
                return false;
            }

            if (choice.HasFishTreatCost && !run.Wallet.SpendFishTreats(choice.FishTreatsCost))
            {
                return false;
            }

            run.Wallet.AddDreamShards(choice.DreamShardsGained);
            run.Wallet.AddFishTreats(choice.FishTreatsGained);
            ApplyEventItemEffects(node, run, choice, hadBlankWishTagBefore);

            ApplyCoreValueEffects(run, choice);
            ApplyPendingBattleEffects(run, choice);

            if (choice.CatHpSafePercent > 0)
            {
                run.CatVitals.ApplyRestNestRecovery(choice.CatHpSafePercent / 100f);
            }

            if (!string.IsNullOrWhiteSpace(choice.PartnerCatId))
            {
                run.Roster.AddCat(choice.PartnerCatId);
            }

            if (choice.AuthorityBlessing != null)
            {
                run.Blessings.Add(choice.AuthorityBlessing);
            }

            if (!string.IsNullOrWhiteSpace(choice.AuthorityBlessingUpgradeId))
            {
                run.Blessings.Upgrade(choice.AuthorityBlessingUpgradeId);
            }

            RecordPlaceholderNode(node, run);
            return true;
        }

        private static bool IsChoiceAvailableForNode(RouteNodeDefinition node, RunProgressionState run, RouteRewardChoice choice)
        {
            IReadOnlyList<RouteRewardChoice> availableChoices = CreatePlaceholderChoices(node, run);
            for (int i = 0; i < availableChoices.Count; i++)
            {
                if (availableChoices[i].Id == choice.Id)
                {
                    return true;
                }
            }

            return false;
        }

        private static IReadOnlyList<RouteRewardChoice> CreateDreamEventChoices(RouteNodeDefinition node, RunProgressionState run)
        {
            List<RouteRewardChoice> choices = node != null && IsUnreadRedDotRain(node.ContentId)
                ? new List<RouteRewardChoice>(CreateUnreadRedDotRainChoices(run))
                : new List<RouteRewardChoice>(CreateSoftRainWindowChoices(run));

            if (run != null && run.EventItems.Has(RunEventItemInventory.BlankWishTagId))
            {
                choices.Add(new RouteRewardChoice(
                    "dream_event_blank_wish_extra",
                    "兑现空白许愿签",
                    "许愿签让这次梦境事件多出一个额外奖励选项。",
                    RouteRewardChoiceType.GainEventItem,
                    eventItemId: RunEventItemInventory.FadedFishBagId,
                    eventItemCount: 1));
            }

            return choices.AsReadOnly();
        }

        private static IReadOnlyList<RouteRewardChoice> CreateSoftRainWindowChoices(RunProgressionState run)
        {
            const int baseFishTreats = 2;
            return new[]
            {
                new RouteRewardChoice(
                    "dream_event_clear_notifications",
                    "清除红点",
                    "扫掉未读红点雨，获得少量小鱼干。",
                    RouteRewardChoiceType.GainFishTreats,
                    fishTreatsGained: PreviewEventFishTreatGain(run, baseFishTreats),
                    baseFishTreatsGained: baseFishTreats),
                new RouteRewardChoice(
                    "dream_event_catnip_residue",
                    "吸入猫薄荷残响",
                    "下一场战斗技能更强，但屎意压力上升更快。",
                    RouteRewardChoiceType.DreamEventModifier,
                    nextBattleSkillDamagePercent: 20,
                    nextBattlePoopGrowthPercent: 50),
                new RouteRewardChoice(
                    "dream_event_mark_all_read",
                    "全部标为已读",
                    "清理梦里的消息噪声，稳定床边睡眠。",
                    RouteRewardChoiceType.GainFishTreats,
                    ownerSleepRestored: 12)
            };
        }

        private static IReadOnlyList<RouteRewardChoice> CreateUnreadRedDotRainChoices(RunProgressionState run)
        {
            const int baseFishTreats = 3;
            return new[]
            {
                new RouteRewardChoice(
                    "dream_event_red_dot_cleanup",
                    "清扫未读红点雨",
                    "趁红点雨还没堆满屏幕，换取更多小鱼干。",
                    RouteRewardChoiceType.GainFishTreats,
                    fishTreatsGained: PreviewEventFishTreatGain(run, baseFishTreats),
                    baseFishTreatsGained: baseFishTreats),
                new RouteRewardChoice(
                    "dream_event_red_dot_ignore",
                    "假装没看见",
                    "获得空白许愿签，但下一场战斗开局压力更高。",
                    RouteRewardChoiceType.GainEventItem,
                    nextBattlePoopGrowthPercent: 75,
                    eventItemId: RunEventItemInventory.BlankWishTagId,
                    eventItemCount: 1),
                new RouteRewardChoice(
                    "dream_event_red_dot_mute_thread",
                    "整夜静音线程",
                    "暂时按下所有提醒，让床边睡眠回稳。",
                    RouteRewardChoiceType.GainFishTreats,
                    ownerSleepRestored: 16)
            };
        }

        private static bool IsUnreadRedDotRain(string contentId)
        {
            return contentId == P0RouteCatalog.UnreadRedDotRainEventContentId
                || contentId == P0RouteCatalog.LegacyUnreadRedDotRainEventContentId;
        }

        private static int PreviewEventFishTreatGain(RunProgressionState run, int baseAmount)
        {
            return run == null ? baseAmount : run.EventItems.PreviewFishTreatGain(baseAmount);
        }

        private static int PreviewShopFishTreatCost(RunProgressionState run, int baseCost)
        {
            return run == null ? baseCost : run.EventItems.PreviewFishTreatCost(baseCost);
        }

        private static IReadOnlyList<RouteRewardChoice> CreateShopChoices(RunProgressionState run)
        {
            List<RouteRewardChoice> choices = new List<RouteRewardChoice>();
            const int bedPatchBaseCost = 3;
            int bedPatchCost = PreviewShopFishTreatCost(run, bedPatchBaseCost);
            if (run == null || run.Wallet.FishTreats >= bedPatchCost)
            {
                choices.Add(new RouteRewardChoice(
                    "shop_bed_patch",
                    "购买床补丁",
                    "花费小鱼干，在下一场战斗前修补床。",
                    RouteRewardChoiceType.PurchaseSupply,
                    fishTreatsCost: bedPatchCost,
                    baseFishTreatsCost: bedPatchBaseCost,
                    ownerSleepRestored: 20));
            }

            const int supplyBaseCost = 2;
            int supplyCost = PreviewShopFishTreatCost(run, supplyBaseCost);
            if (run == null || run.Wallet.FishTreats >= supplyCost)
            {
                choices.Add(new RouteRewardChoice(
                    "shop_litter_sachet",
                    "购买猫砂香袋",
                    "花费小鱼干，降低队伍屎意压力。",
                    RouteRewardChoiceType.PurchaseSupply,
                    fishTreatsCost: supplyCost,
                    baseFishTreatsCost: supplyBaseCost,
                    poopReduced: 30));

                choices.Add(new RouteRewardChoice(
                    "shop_late_kibble",
                    "购买夜宵猫粮",
                    "花费小鱼干，将队伍饱肚度恢复到安全线。",
                    RouteRewardChoiceType.PurchaseSupply,
                    fishTreatsCost: supplyCost,
                    baseFishTreatsCost: supplyBaseCost,
                    hungerSafeLine: 85));
            }

            AuthorityBlessingDefinition blessing = GetFirstAvailableBlessing(run);
            const int authorityBlessingBaseCost = 5;
            int authorityBlessingCost = PreviewShopFishTreatCost(run, authorityBlessingBaseCost);
            if (blessing != null && (run == null || run.Wallet.FishTreats >= authorityBlessingCost))
            {
                choices.Add(new RouteRewardChoice(
                    "shop_authority_" + blessing.Id,
                    "购买 " + blessing.DisplayName,
                    "花费小鱼干获得一个 P0 权柄祝福。",
                    RouteRewardChoiceType.GainAuthorityBlessing,
                    fishTreatsCost: authorityBlessingCost,
                    baseFishTreatsCost: authorityBlessingBaseCost,
                    authorityBlessing: blessing));
            }

            AddAuthorityBlessingUpgradeChoices(
                choices,
                run,
                ShopAuthorityBlessingUpgradeCost,
                "shop_upgrade_",
                "升级 ",
                "花费小鱼干加深已有的 P0 权柄祝福。");

            choices.Add(new RouteRewardChoice(
                "shop_free_sample",
                "领取试吃",
                "不消耗梦屑，获得一份小鱼干。",
                RouteRewardChoiceType.GainFishTreats,
                fishTreatsGained: 1));

            return choices.AsReadOnly();
        }

        private static void ApplyEventItemEffects(
            RouteNodeDefinition node,
            RunProgressionState run,
            RouteRewardChoice choice,
            bool hadBlankWishTagBefore)
        {
            if (choice.HasEventItemReward)
            {
                run.EventItems.Add(choice.EventItemId, choice.EventItemCount);
            }

            if (node.NodeType == RouteNodeType.DreamEvent)
            {
                run.EventItems.ConsumeFishGainBonusIfNeeded(choice.BaseFishTreatsGained);
                if (hadBlankWishTagBefore)
                {
                    run.EventItems.Consume(RunEventItemInventory.BlankWishTagId);
                }
            }

            if (node.NodeType == RouteNodeType.Shop)
            {
                run.EventItems.ConsumeShopDiscountIfNeeded(choice.BaseFishTreatsCost);
            }
        }

        private static void ApplyCoreValueEffects(RunProgressionState run, RouteRewardChoice choice)
        {
            if (choice.OwnerSleepRestored > 0)
            {
                run.CoreValues.RestoreOwnerSleep(choice.OwnerSleepRestored);
            }

            if (choice.OwnerSleepDamaged > 0)
            {
                run.CoreValues.DamageOwnerSleep(choice.OwnerSleepDamaged);
            }

            if (choice.PoopReduced > 0)
            {
                run.CoreValues.ReducePoop(choice.PoopReduced);
            }

            if (choice.PoopIncreased > 0)
            {
                run.CoreValues.IncreasePoop(choice.PoopIncreased);
            }

            if (choice.HungerSafeLine > 0)
            {
                run.CoreValues.RestoreHungerToSafeLine(choice.HungerSafeLine);
            }
        }

        private static void ApplyPendingBattleEffects(RunProgressionState run, RouteRewardChoice choice)
        {
            if (!choice.HasNextBattleEffect)
            {
                return;
            }

            run.PendingBattleModifiers.Add(
                skillDamageMultiplier: 1f + choice.NextBattleSkillDamagePercent / 100f,
                poopGrowthMultiplier: 1f + choice.NextBattlePoopGrowthPercent / 100f,
                shieldMultiplier: 1f + choice.NextBattleShieldPercent / 100f,
                enemyStatusDurationMultiplier: 1f + choice.NextBattleStatusDurationPercent / 100f,
                ownerSleepRestoreMultiplier: 1f + choice.NextBattleOwnerSleepRestorePercent / 100f,
                catHealMultiplier: 1f + choice.NextBattleCatHealPercent / 100f);
        }

        private static IReadOnlyList<RouteRewardChoice> CreateBlessingChoices(RunProgressionState run)
        {
            List<RouteRewardChoice> choices = new List<RouteRewardChoice>();
            IReadOnlyList<AuthorityBlessingDefinition> blessings = P0BlessingCatalog.CreateAuthorityBlessings();
            for (int i = 0; i < blessings.Count; i++)
            {
                AuthorityBlessingDefinition blessing = blessings[i];
                if (run != null && run.Blessings.Has(blessing.Id))
                {
                    continue;
                }

                choices.Add(new RouteRewardChoice(
                    "blessing_" + blessing.Id,
                    blessing.DisplayName,
                    blessing.Description,
                    RouteRewardChoiceType.GainAuthorityBlessing,
                    authorityBlessing: blessing));
            }

            if (choices.Count == 0)
            {
                AddAuthorityBlessingUpgradeChoices(
                    choices,
                    run,
                    0,
                    "blessing_upgrade_",
                    "加深 ",
                    "祭坛会加深一个已拥有的 P0 权柄祝福。");
            }

            if (choices.Count == 0)
            {
                choices.Add(new RouteRewardChoice(
                    "blessing_all_claimed_shards",
                    "收集残余梦尘",
                    "所有 P0 权柄祝福已达到上限，祭坛改为给予梦屑。",
                    RouteRewardChoiceType.GainDreamShards,
                    dreamShardsGained: 2));
            }

            return choices.AsReadOnly();
        }

        private static void AddAuthorityBlessingUpgradeChoices(
            List<RouteRewardChoice> choices,
            RunProgressionState run,
            int fishTreatsCost,
            string idPrefix,
            string displayPrefix,
            string description)
        {
            if (run == null)
            {
                return;
            }

            IReadOnlyList<AuthorityBlessingDefinition> blessings = P0BlessingCatalog.CreateAuthorityBlessings();
            for (int i = 0; i < blessings.Count; i++)
            {
                AuthorityBlessingDefinition blessing = blessings[i];
                if (!run.Blessings.CanUpgrade(blessing.Id))
                {
                    continue;
                }

                int effectiveFishTreatsCost = PreviewShopFishTreatCost(run, fishTreatsCost);
                if (effectiveFishTreatsCost > run.Wallet.FishTreats)
                {
                    continue;
                }

                int nextLevel = run.Blessings.GetLevel(blessing.Id) + 1;
                choices.Add(new RouteRewardChoice(
                    idPrefix + blessing.Id,
                    displayPrefix + blessing.DisplayName + " 等级" + nextLevel,
                    description,
                    RouteRewardChoiceType.UpgradeAuthorityBlessing,
                    fishTreatsCost: effectiveFishTreatsCost,
                    baseFishTreatsCost: fishTreatsCost,
                    authorityBlessingUpgradeId: blessing.Id));
            }
        }

        private static AuthorityBlessingDefinition GetFirstAvailableBlessing(RunProgressionState run)
        {
            IReadOnlyList<AuthorityBlessingDefinition> blessings = P0BlessingCatalog.CreateAuthorityBlessings();
            for (int i = 0; i < blessings.Count; i++)
            {
                if (run != null && run.Blessings.Has(blessings[i].Id))
                {
                    continue;
                }

                return blessings[i];
            }

            return null;
        }

        private static void RecordPlaceholderNode(RouteNodeDefinition node, RunProgressionState run)
        {
            switch (node.NodeType)
            {
                case RouteNodeType.DreamEvent:
                    run.RecordDreamEvent();
                    break;
                case RouteNodeType.Shop:
                    run.RecordShopPurchase();
                    break;
                case RouteNodeType.RestNest:
                    run.RecordRestNestUse();
                    break;
                default:
                    run.RecordPlaceholderReward();
                    break;
            }
        }
    }
}
