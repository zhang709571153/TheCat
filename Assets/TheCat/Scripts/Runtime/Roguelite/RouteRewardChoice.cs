using System;
using TheCat.Data.Catalogs;

namespace TheCat.Roguelite
{
    public sealed class RouteRewardChoice
    {
        public RouteRewardChoice(
            string id,
            string displayName,
            string description,
            RouteRewardChoiceType choiceType,
            int dreamShardsGained = 0,
            int fishTreatsGained = 0,
            int dreamShardsCost = 0,
            int fishTreatsCost = 0,
            string partnerCatId = "",
            AuthorityBlessingDefinition authorityBlessing = null,
            int ownerSleepRestored = 0,
            int ownerSleepDamaged = 0,
            int poopReduced = 0,
            int poopIncreased = 0,
            int hungerSafeLine = 0,
            int catHpSafePercent = 0,
            int nextBattleSkillDamagePercent = 0,
            int nextBattlePoopGrowthPercent = 0,
            int nextBattleShieldPercent = 0,
            int nextBattleStatusDurationPercent = 0,
            int nextBattleOwnerSleepRestorePercent = 0,
            int nextBattleCatHealPercent = 0,
            string authorityBlessingUpgradeId = "",
            string eventItemId = "",
            int eventItemCount = 0,
            int baseFishTreatsGained = 0,
            int baseFishTreatsCost = 0)
        {
            RequireText(id, nameof(id));
            RequireText(displayName, nameof(displayName));
            RequireNonNegative(dreamShardsGained, nameof(dreamShardsGained));
            RequireNonNegative(fishTreatsGained, nameof(fishTreatsGained));
            RequireNonNegative(dreamShardsCost, nameof(dreamShardsCost));
            RequireNonNegative(fishTreatsCost, nameof(fishTreatsCost));
            RequireNonNegative(ownerSleepRestored, nameof(ownerSleepRestored));
            RequireNonNegative(ownerSleepDamaged, nameof(ownerSleepDamaged));
            RequireNonNegative(poopReduced, nameof(poopReduced));
            RequireNonNegative(poopIncreased, nameof(poopIncreased));
            RequireNonNegative(hungerSafeLine, nameof(hungerSafeLine));
            RequireNonNegative(catHpSafePercent, nameof(catHpSafePercent));
            RequireNonNegative(nextBattleSkillDamagePercent, nameof(nextBattleSkillDamagePercent));
            RequireNonNegative(nextBattlePoopGrowthPercent, nameof(nextBattlePoopGrowthPercent));
            RequireNonNegative(nextBattleShieldPercent, nameof(nextBattleShieldPercent));
            RequireNonNegative(nextBattleStatusDurationPercent, nameof(nextBattleStatusDurationPercent));
            RequireNonNegative(nextBattleOwnerSleepRestorePercent, nameof(nextBattleOwnerSleepRestorePercent));
            RequireNonNegative(nextBattleCatHealPercent, nameof(nextBattleCatHealPercent));
            RequireNonNegative(eventItemCount, nameof(eventItemCount));
            RequireNonNegative(baseFishTreatsGained, nameof(baseFishTreatsGained));
            RequireNonNegative(baseFishTreatsCost, nameof(baseFishTreatsCost));
            if (catHpSafePercent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(catHpSafePercent), catHpSafePercent, "Cat HP safe percent must not exceed 100.");
            }

            Id = id;
            DisplayName = displayName;
            Description = description ?? string.Empty;
            ChoiceType = choiceType;
            DreamShardsGained = dreamShardsGained;
            FishTreatsGained = fishTreatsGained;
            DreamShardsCost = dreamShardsCost;
            FishTreatsCost = fishTreatsCost;
            PartnerCatId = partnerCatId ?? string.Empty;
            AuthorityBlessing = authorityBlessing;
            OwnerSleepRestored = ownerSleepRestored;
            OwnerSleepDamaged = ownerSleepDamaged;
            PoopReduced = poopReduced;
            PoopIncreased = poopIncreased;
            HungerSafeLine = hungerSafeLine;
            CatHpSafePercent = catHpSafePercent;
            NextBattleSkillDamagePercent = nextBattleSkillDamagePercent;
            NextBattlePoopGrowthPercent = nextBattlePoopGrowthPercent;
            NextBattleShieldPercent = nextBattleShieldPercent;
            NextBattleStatusDurationPercent = nextBattleStatusDurationPercent;
            NextBattleOwnerSleepRestorePercent = nextBattleOwnerSleepRestorePercent;
            NextBattleCatHealPercent = nextBattleCatHealPercent;
            AuthorityBlessingUpgradeId = authorityBlessingUpgradeId ?? string.Empty;
            EventItemId = eventItemId ?? string.Empty;
            EventItemCount = eventItemCount;
            BaseFishTreatsGained = baseFishTreatsGained;
            BaseFishTreatsCost = baseFishTreatsCost;

            ValidateTypedPayload();
        }

        public string Id { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public RouteRewardChoiceType ChoiceType { get; }

        public int DreamShardsGained { get; }

        public int FishTreatsGained { get; }

        public int DreamShardsCost { get; }

        public int FishTreatsCost { get; }

        public string PartnerCatId { get; }

        public AuthorityBlessingDefinition AuthorityBlessing { get; }

        public int OwnerSleepRestored { get; }

        public int OwnerSleepDamaged { get; }

        public int PoopReduced { get; }

        public int PoopIncreased { get; }

        public int HungerSafeLine { get; }

        public int CatHpSafePercent { get; }

        public int NextBattleSkillDamagePercent { get; }

        public int NextBattlePoopGrowthPercent { get; }

        public int NextBattleShieldPercent { get; }

        public int NextBattleStatusDurationPercent { get; }

        public int NextBattleOwnerSleepRestorePercent { get; }

        public int NextBattleCatHealPercent { get; }

        public string AuthorityBlessingUpgradeId { get; }

        public string EventItemId { get; }

        public int EventItemCount { get; }

        public int BaseFishTreatsGained { get; }

        public int BaseFishTreatsCost { get; }

        public bool HasDreamShardCost => DreamShardsCost > 0;

        public bool HasFishTreatCost => FishTreatsCost > 0;

        public bool HasEventItemReward => EventItemCount > 0;

        public bool HasNextBattleEffect => NextBattleSkillDamagePercent > 0
            || NextBattlePoopGrowthPercent > 0
            || NextBattleShieldPercent > 0
            || NextBattleStatusDurationPercent > 0
            || NextBattleOwnerSleepRestorePercent > 0
            || NextBattleCatHealPercent > 0;

        public string BuildSummary()
        {
            string summary = DisplayName;
            if (DreamShardsCost > 0)
            {
                summary += " 消耗 " + DreamShardsCost + " 梦屑";
            }

            if (FishTreatsCost > 0)
            {
                summary += " 消耗 " + FishTreatsCost + " 小鱼干";
            }

            if (DreamShardsGained > 0)
            {
                summary += " +" + DreamShardsGained + " 梦屑";
            }

            if (FishTreatsGained > 0)
            {
                summary += " +" + FishTreatsGained + " 小鱼干";
            }

            if (!string.IsNullOrWhiteSpace(PartnerCatId))
            {
                summary += " 招募 " + P0CatPresenter.Describe(PartnerCatId).DisplayName;
            }

            if (AuthorityBlessing != null)
            {
                summary += " 祝福 " + AuthorityBlessing.DisplayName;
            }

            if (!string.IsNullOrWhiteSpace(AuthorityBlessingUpgradeId))
            {
                summary += " 升级 " + P0BlessingCatalog.GetAuthorityBlessingDisplayName(AuthorityBlessingUpgradeId);
            }

            if (HasEventItemReward)
            {
                summary += " 事件道具 " + RunEventItemInventory.GetDisplayName(EventItemId);
                if (EventItemCount > 1)
                {
                    summary += " x" + EventItemCount;
                }
            }

            if (OwnerSleepRestored > 0)
            {
                summary += " 睡眠 +" + OwnerSleepRestored;
            }

            if (OwnerSleepDamaged > 0)
            {
                summary += " 睡眠 -" + OwnerSleepDamaged;
            }

            if (PoopReduced > 0)
            {
                summary += " 屎意 -" + PoopReduced;
            }

            if (PoopIncreased > 0)
            {
                summary += " 屎意 +" + PoopIncreased;
            }

            if (HungerSafeLine > 0)
            {
                summary += " 饱肚 >=" + HungerSafeLine;
            }

            if (CatHpSafePercent > 0)
            {
                summary += " 猫生命 >=" + CatHpSafePercent + "%";
            }

            if (NextBattleSkillDamagePercent > 0)
            {
                summary += " 下战伤害 +" + NextBattleSkillDamagePercent + "%";
            }

            if (NextBattlePoopGrowthPercent > 0)
            {
                summary += " 下战屎意 +" + NextBattlePoopGrowthPercent + "%";
            }

            return summary;
        }

        public string BuildDiagnosticSummary()
        {
            string summary = BuildSummary();
            AppendReadableTokens(ref summary);
            return summary;
        }

        private void AppendReadableTokens(ref string summary)
        {
            if (DreamShardsCost > 0)
            {
                summary += " cost " + DreamShardsCost + " dream";
            }

            if (FishTreatsCost > 0)
            {
                summary += " cost " + FishTreatsCost + " fish";
                if (BaseFishTreatsCost > 0 && BaseFishTreatsCost != FishTreatsCost)
                {
                    summary += " base fish cost " + BaseFishTreatsCost;
                }
            }

            if (DreamShardsGained > 0)
            {
                summary += " dream +" + DreamShardsGained;
            }

            if (FishTreatsGained > 0)
            {
                summary += " fish +" + FishTreatsGained;
                if (BaseFishTreatsGained > 0 && BaseFishTreatsGained != FishTreatsGained)
                {
                    summary += " base fish +" + BaseFishTreatsGained;
                }
            }

            if (!string.IsNullOrWhiteSpace(PartnerCatId))
            {
                summary += " recruit " + P0CatPresenter.Describe(PartnerCatId).DisplayName;
            }

            if (OwnerSleepRestored > 0)
            {
                summary += " sleep +" + OwnerSleepRestored;
            }

            if (OwnerSleepDamaged > 0)
            {
                summary += " sleep -" + OwnerSleepDamaged;
            }

            if (PoopReduced > 0)
            {
                summary += " poop -" + PoopReduced;
            }

            if (PoopIncreased > 0)
            {
                summary += " poop +" + PoopIncreased;
            }

            if (HungerSafeLine > 0)
            {
                summary += " hunger >=" + HungerSafeLine;
            }

            if (CatHpSafePercent > 0)
            {
                summary += " cat hp >=" + CatHpSafePercent + "%";
            }

            if (HasEventItemReward)
            {
                summary += " item " + EventItemId + " +" + EventItemCount;
            }

            if (NextBattleSkillDamagePercent > 0)
            {
                summary += " next dmg +" + NextBattleSkillDamagePercent + "%";
            }

            if (NextBattlePoopGrowthPercent > 0)
            {
                summary += " next poop +" + NextBattlePoopGrowthPercent + "%";
            }

            if (NextBattleShieldPercent > 0)
            {
                summary += " next shield +" + NextBattleShieldPercent + "%";
            }

            if (NextBattleStatusDurationPercent > 0)
            {
                summary += " next status +" + NextBattleStatusDurationPercent + "%";
            }

            if (NextBattleOwnerSleepRestorePercent > 0)
            {
                summary += " next sleep +" + NextBattleOwnerSleepRestorePercent + "%";
            }

            if (NextBattleCatHealPercent > 0)
            {
                summary += " next heal +" + NextBattleCatHealPercent + "%";
            }
        }

        private void ValidateTypedPayload()
        {
            switch (ChoiceType)
            {
                case RouteRewardChoiceType.RecruitPartner:
                    RequireText(PartnerCatId, nameof(PartnerCatId));
                    break;
                case RouteRewardChoiceType.GainAuthorityBlessing:
                    if (AuthorityBlessing == null)
                    {
                        throw new ArgumentNullException(nameof(AuthorityBlessing));
                    }

                    break;
                case RouteRewardChoiceType.UpgradeAuthorityBlessing:
                    RequireText(AuthorityBlessingUpgradeId, nameof(AuthorityBlessingUpgradeId));
                    break;
                case RouteRewardChoiceType.GainEventItem:
                    RequireText(EventItemId, nameof(EventItemId));
                    if (EventItemCount <= 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(EventItemCount), EventItemCount, "Event item count must be greater than zero.");
                    }

                    break;
            }
        }

        private static void RequireText(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is required.", name);
            }
        }

        private static void RequireNonNegative(int value, string name)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must not be negative.");
            }
        }
    }
}
