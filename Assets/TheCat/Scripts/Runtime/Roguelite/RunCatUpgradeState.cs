using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Roguelite
{
    public enum CatUpgradeStage
    {
        Passive,
        SmallSkill,
        Ultimate,
        Complete
    }

    public readonly struct CatUpgradeCandidate
    {
        public CatUpgradeCandidate(
            string id,
            string catId,
            CatUpgradeStage stage,
            string displayName,
            string description,
            string playerIntent = "")
        {
            Id = id ?? string.Empty;
            CatId = catId ?? string.Empty;
            Stage = stage;
            DisplayName = displayName ?? string.Empty;
            Description = description ?? string.Empty;
            PlayerIntent = playerIntent ?? string.Empty;
        }

        public string Id { get; }

        public string CatId { get; }

        public CatUpgradeStage Stage { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public string PlayerIntent { get; }

        public bool IsValid => !string.IsNullOrWhiteSpace(Id)
            && !string.IsNullOrWhiteSpace(CatId)
            && Stage != CatUpgradeStage.Complete;

        public string StageLabel => P0CatUpgradeCatalog.GetStageLabel(Stage);

        public string BuildSummary()
        {
            string intent = string.IsNullOrWhiteSpace(PlayerIntent)
                ? string.Empty
                : PlayerIntent + "：";
            return intent + Description;
        }
    }

    public static class P0CatUpgradeCatalog
    {
        public const string SaibanPassiveOathReflowId = "cat_upgrade_saiban_passive_oath_reflow";
        public const string SaibanPassiveBedlineGuardId = "cat_upgrade_saiban_passive_bedline_guard";
        public const string SaibanSmallOathShieldFocusId = "cat_upgrade_saiban_small_oath_shield_focus";
        public const string SaibanSmallSwordSweepArcId = "cat_upgrade_saiban_small_sword_sweep_arc";
        public const string SaibanSmallBedlineInterceptId = "cat_upgrade_saiban_small_bedline_intercept";
        public const string SaibanSmallOathCounterId = "cat_upgrade_saiban_small_oath_counter";
        public const string SaibanUltimateSunCrownId = "cat_upgrade_saiban_ultimate_sun_crown";
        public const string SaibanUltimateOathDomainId = "cat_upgrade_saiban_ultimate_oath_domain";

        public const string NephthysPassiveSandEyeId = "cat_upgrade_nephthys_passive_sand_eye";
        public const string NephthysPassiveRoyalOverseerId = "cat_upgrade_nephthys_passive_royal_overseer";
        public const string NephthysSmallMoonSandId = "cat_upgrade_nephthys_small_moon_sand";
        public const string NephthysSmallQuicksandId = "cat_upgrade_nephthys_small_quicksand";
        public const string NephthysSmallRoyalMarkId = "cat_upgrade_nephthys_small_royal_mark";
        public const string NephthysSmallSandSentinelId = "cat_upgrade_nephthys_small_sand_sentinel";
        public const string NephthysUltimateSandThroneId = "cat_upgrade_nephthys_ultimate_sand_throne";
        public const string NephthysUltimateEclipseObeliskId = "cat_upgrade_nephthys_ultimate_eclipse_obelisk";

        public const string SuzunePassiveLingeringBellId = "cat_upgrade_suzune_passive_lingering_bell";
        public const string SuzunePassiveSleepPrayerId = "cat_upgrade_suzune_passive_sleep_prayer";
        public const string SuzuneSmallSleepBellId = "cat_upgrade_suzune_small_sleep_bell";
        public const string SuzuneSmallHealingBellId = "cat_upgrade_suzune_small_healing_bell";
        public const string SuzuneSmallMoonToriiId = "cat_upgrade_suzune_small_moon_torii";
        public const string SuzuneSmallDreamChimeId = "cat_upgrade_suzune_small_dream_chime";
        public const string SuzuneUltimateMoonSleepId = "cat_upgrade_suzune_ultimate_moon_sleep";
        public const string SuzuneUltimateKaguraCleanseId = "cat_upgrade_suzune_ultimate_kagura_cleanse";

        private static readonly Dictionary<string, Dictionary<CatUpgradeStage, CatUpgradeCandidate[]>> CandidateTable =
            BuildCandidateTable();

        public static IReadOnlyList<CatUpgradeCandidate> CreateOffer(
            IReadOnlyList<string> rosterCatIds,
            int offerSeed,
            Func<string, int> upgradeCountProvider)
        {
            if (rosterCatIds == null || upgradeCountProvider == null)
            {
                return Array.Empty<CatUpgradeCandidate>();
            }

            List<CatUpgradeCandidate> offer = new List<CatUpgradeCandidate>();
            for (int i = 0; i < rosterCatIds.Count; i++)
            {
                string catId = rosterCatIds[i];
                CatUpgradeStage stage = GetNextStage(upgradeCountProvider(catId));
                if (!TryGetCandidates(catId, stage, out CatUpgradeCandidate[] candidates))
                {
                    continue;
                }

                AddRotatedCandidates(offer, candidates, offerSeed + i);
            }

            return offer.AsReadOnly();
        }

        public static int GetStageCandidateCount(string catId, CatUpgradeStage stage)
        {
            return TryGetCandidates(catId, stage, out CatUpgradeCandidate[] candidates) ? candidates.Length : 0;
        }

        public static CatUpgradeStage GetNextStage(int upgradeCount)
        {
            if (upgradeCount <= 0)
            {
                return CatUpgradeStage.Passive;
            }

            if (upgradeCount == 1)
            {
                return CatUpgradeStage.SmallSkill;
            }

            if (upgradeCount == 2)
            {
                return CatUpgradeStage.Ultimate;
            }

            return CatUpgradeStage.Complete;
        }

        public static string GetStageLabel(CatUpgradeStage stage)
        {
            switch (stage)
            {
                case CatUpgradeStage.Passive:
                    return "被动";
                case CatUpgradeStage.SmallSkill:
                    return "小技能";
                case CatUpgradeStage.Ultimate:
                    return "大招";
                default:
                    return "完成";
            }
        }

        private static bool TryGetCandidates(
            string catId,
            CatUpgradeStage stage,
            out CatUpgradeCandidate[] candidates)
        {
            candidates = null;
            return !string.IsNullOrWhiteSpace(catId)
                && CandidateTable.TryGetValue(catId, out Dictionary<CatUpgradeStage, CatUpgradeCandidate[]> byStage)
                && byStage.TryGetValue(stage, out candidates)
                && candidates != null
                && candidates.Length > 0;
        }

        private static int PositiveModulo(int value, int divisor)
        {
            if (divisor <= 0)
            {
                return 0;
            }

            int result = value % divisor;
            return result < 0 ? result + divisor : result;
        }

        private static void AddRotatedCandidates(
            List<CatUpgradeCandidate> offer,
            CatUpgradeCandidate[] candidates,
            int offset)
        {
            int start = PositiveModulo(offset, candidates.Length);
            for (int i = 0; i < candidates.Length; i++)
            {
                offer.Add(candidates[PositiveModulo(start + i, candidates.Length)]);
            }
        }

        private static Dictionary<string, Dictionary<CatUpgradeStage, CatUpgradeCandidate[]>> BuildCandidateTable()
        {
            return new Dictionary<string, Dictionary<CatUpgradeStage, CatUpgradeCandidate[]>>
            {
                {
                    P0PrototypeCatalog.SaibanId,
                    new Dictionary<CatUpgradeStage, CatUpgradeCandidate[]>
                    {
                        {
                            CatUpgradeStage.Passive,
                            new[]
                            {
                                new CatUpgradeCandidate(SaibanPassiveOathReflowId, P0PrototypeCatalog.SaibanId, CatUpgradeStage.Passive, "誓约回流", "护盾吸收后提升床线续航。", "守床续航"),
                                new CatUpgradeCandidate(SaibanPassiveBedlineGuardId, P0PrototypeCatalog.SaibanId, CatUpgradeStage.Passive, "床线守誓", "强化赛班的守床身板。", "前排抗压")
                            }
                        },
                        {
                            CatUpgradeStage.SmallSkill,
                            new[]
                            {
                                new CatUpgradeCandidate(SaibanSmallOathShieldFocusId, P0PrototypeCatalog.SaibanId, CatUpgradeStage.SmallSkill, "誓约护盾专注", "解锁更厚的单体护盾小技能。", "护盾救急"),
                                new CatUpgradeCandidate(SaibanSmallSwordSweepArcId, P0PrototypeCatalog.SaibanId, CatUpgradeStage.SmallSkill, "王剑横扫弧光", "解锁更强击退的横扫小技能。", "击退清线"),
                                new CatUpgradeCandidate(SaibanSmallBedlineInterceptId, P0PrototypeCatalog.SaibanId, CatUpgradeStage.SmallSkill, "床线拦截", "解锁快速拦截入侵者的小技能。", "拦截入侵"),
                                new CatUpgradeCandidate(SaibanSmallOathCounterId, P0PrototypeCatalog.SaibanId, CatUpgradeStage.SmallSkill, "誓约反制", "解锁护盾并反击最近敌人的小技能。", "护盾反击")
                            }
                        },
                        {
                            CatUpgradeStage.Ultimate,
                            new[]
                            {
                                new CatUpgradeCandidate(SaibanUltimateSunCrownId, P0PrototypeCatalog.SaibanId, CatUpgradeStage.Ultimate, "日冕圣裁", "解锁爆发击退型大招。", "爆发击退"),
                                new CatUpgradeCandidate(SaibanUltimateOathDomainId, P0PrototypeCatalog.SaibanId, CatUpgradeStage.Ultimate, "誓约领域", "解锁守护领域型大招。", "守护领域")
                            }
                        }
                    }
                },
                {
                    P0PrototypeCatalog.NephthysId,
                    new Dictionary<CatUpgradeStage, CatUpgradeCandidate[]>
                    {
                        {
                            CatUpgradeStage.Passive,
                            new[]
                            {
                                new CatUpgradeCandidate(NephthysPassiveSandEyeId, P0PrototypeCatalog.NephthysId, CatUpgradeStage.Passive, "沙眼回响", "强化奈芙蒂斯的控场生存。", "控场生存"),
                                new CatUpgradeCandidate(NephthysPassiveRoyalOverseerId, P0PrototypeCatalog.NephthysId, CatUpgradeStage.Passive, "王座监工", "提升奈芙蒂斯的控场机动。", "机动控场")
                            }
                        },
                        {
                            CatUpgradeStage.SmallSkill,
                            new[]
                            {
                                new CatUpgradeCandidate(NephthysSmallMoonSandId, P0PrototypeCatalog.NephthysId, CatUpgradeStage.SmallSkill, "月沙方尖碑", "解锁召唤控场小技能。", "召唤减速"),
                                new CatUpgradeCandidate(NephthysSmallQuicksandId, P0PrototypeCatalog.NephthysId, CatUpgradeStage.SmallSkill, "流沙牢笼", "解锁更强减速陷阱小技能。", "强减速"),
                                new CatUpgradeCandidate(NephthysSmallRoyalMarkId, P0PrototypeCatalog.NephthysId, CatUpgradeStage.SmallSkill, "支配沙令", "解锁标记集火小技能。", "标记集火"),
                                new CatUpgradeCandidate(NephthysSmallSandSentinelId, P0PrototypeCatalog.NephthysId, CatUpgradeStage.SmallSkill, "沙卫哨", "解锁召唤并压制的控场小技能。", "召唤压制")
                            }
                        },
                        {
                            CatUpgradeStage.Ultimate,
                            new[]
                            {
                                new CatUpgradeCandidate(NephthysUltimateSandThroneId, P0PrototypeCatalog.NephthysId, CatUpgradeStage.Ultimate, "沙海王座", "解锁大范围控场大招。", "大范围控场"),
                                new CatUpgradeCandidate(NephthysUltimateEclipseObeliskId, P0PrototypeCatalog.NephthysId, CatUpgradeStage.Ultimate, "月蚀方尖碑", "解锁召唤压制型大招。", "召唤压制")
                            }
                        }
                    }
                },
                {
                    P0PrototypeCatalog.SuzuneId,
                    new Dictionary<CatUpgradeStage, CatUpgradeCandidate[]>
                    {
                        {
                            CatUpgradeStage.Passive,
                            new[]
                            {
                                new CatUpgradeCandidate(SuzunePassiveLingeringBellId, P0PrototypeCatalog.SuzuneId, CatUpgradeStage.Passive, "余音不绝", "强化铃音的治疗安全线。", "治疗安全线"),
                                new CatUpgradeCandidate(SuzunePassiveSleepPrayerId, P0PrototypeCatalog.SuzuneId, CatUpgradeStage.Passive, "安眠祈愿", "强化铃音的守梦续航。", "守梦续航")
                            }
                        },
                        {
                            CatUpgradeStage.SmallSkill,
                            new[]
                            {
                                new CatUpgradeCandidate(SuzuneSmallSleepBellId, P0PrototypeCatalog.SuzuneId, CatUpgradeStage.SmallSkill, "安眠铃音", "解锁更强安眠恢复小技能。", "安眠恢复"),
                                new CatUpgradeCandidate(SuzuneSmallHealingBellId, P0PrototypeCatalog.SuzuneId, CatUpgradeStage.SmallSkill, "治愈铃", "解锁更强猫咪治疗小技能。", "猫咪急救"),
                                new CatUpgradeCandidate(SuzuneSmallMoonToriiId, P0PrototypeCatalog.SuzuneId, CatUpgradeStage.SmallSkill, "月眠鸟居", "解锁守床结界小技能。", "守床结界"),
                                new CatUpgradeCandidate(SuzuneSmallDreamChimeId, P0PrototypeCatalog.SuzuneId, CatUpgradeStage.SmallSkill, "梦响小铃", "解锁护佑当前猫咪的小技能。", "护盾治疗")
                            }
                        },
                        {
                            CatUpgradeStage.Ultimate,
                            new[]
                            {
                                new CatUpgradeCandidate(SuzuneUltimateMoonSleepId, P0PrototypeCatalog.SuzuneId, CatUpgradeStage.Ultimate, "月眠结界", "解锁强力守梦大招。", "强力守梦"),
                                new CatUpgradeCandidate(SuzuneUltimateKaguraCleanseId, P0PrototypeCatalog.SuzuneId, CatUpgradeStage.Ultimate, "神乐净梦", "解锁治疗护盾型大招。", "净梦续航")
                            }
                        }
                    }
                }
            };
        }
    }

    public sealed class RunCatUpgradeState
    {
        public const int ExperienceToUpgrade = 3;
        public const int DefenseExperience = 1;
        public const int EliteExperience = 2;
        public const int BossExperience = 3;

        private readonly Dictionary<string, int> upgradeCountsByCat = new Dictionary<string, int>();
        private readonly HashSet<string> selectedUpgradeIds = new HashSet<string>();

        private int teamExperience;
        private bool hasPendingUpgrade;
        private int offerSeed;
        private string lastResolvedSummary = string.Empty;

        public int TeamExperience => teamExperience;

        public bool HasPendingUpgrade => hasPendingUpgrade;

        public int OfferSeed => offerSeed;

        public int LearnedUpgradeCount => selectedUpgradeIds.Count;

        public string LastResolvedSummary => lastResolvedSummary;

        public bool HasSelectedUpgrade(string upgradeId)
        {
            return !string.IsNullOrWhiteSpace(upgradeId) && selectedUpgradeIds.Contains(upgradeId);
        }

        public int GetUpgradeCount(string catId)
        {
            return !string.IsNullOrWhiteSpace(catId) && upgradeCountsByCat.TryGetValue(catId, out int count)
                ? count
                : 0;
        }

        public bool GrantExperience(int amount, RunPartnerRoster roster)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Experience amount must not be negative.");
            }

            if (amount == 0)
            {
                return hasPendingUpgrade;
            }

            teamExperience += amount;
            QueuePendingUpgradeIfReady(roster);
            return hasPendingUpgrade;
        }

        public IReadOnlyList<CatUpgradeCandidate> CreateCurrentOffer(RunPartnerRoster roster)
        {
            if (!hasPendingUpgrade || roster == null)
            {
                return Array.Empty<CatUpgradeCandidate>();
            }

            return P0CatUpgradeCatalog.CreateOffer(
                roster.CatIds,
                offerSeed,
                GetUpgradeCount);
        }

        public bool CanRerollWithPawStamp(RunEventItemInventory eventItems)
        {
            return hasPendingUpgrade
                && eventItems != null
                && eventItems.Has(RunEventItemInventory.PawStampId);
        }

        public bool TryRerollWithPawStamp(RunEventItemInventory eventItems)
        {
            if (!CanRerollWithPawStamp(eventItems))
            {
                return false;
            }

            if (!eventItems.Consume(RunEventItemInventory.PawStampId))
            {
                return false;
            }

            offerSeed++;
            return true;
        }

        public bool TrySelect(
            string candidateId,
            RunPartnerRoster roster,
            out CatUpgradeCandidate selected)
        {
            selected = default(CatUpgradeCandidate);
            if (string.IsNullOrWhiteSpace(candidateId) || !hasPendingUpgrade)
            {
                return false;
            }

            IReadOnlyList<CatUpgradeCandidate> offer = CreateCurrentOffer(roster);
            for (int i = 0; i < offer.Count; i++)
            {
                if (offer[i].Id != candidateId || !offer[i].IsValid)
                {
                    continue;
                }

                selected = offer[i];
                upgradeCountsByCat[selected.CatId] = GetUpgradeCount(selected.CatId) + 1;
                selectedUpgradeIds.Add(selected.Id);
                hasPendingUpgrade = false;
                lastResolvedSummary = BuildSelectionSummary(selected);
                QueuePendingUpgradeIfReady(roster);
                return true;
            }

            return false;
        }

        public string BuildProgressSummary()
        {
            string pending = hasPendingUpgrade ? "，待选择猫咪升级" : string.Empty;
            return "猫咪经验：" + teamExperience + "/" + ExperienceToUpgrade + pending;
        }

        private void QueuePendingUpgradeIfReady(RunPartnerRoster roster)
        {
            if (hasPendingUpgrade || teamExperience < ExperienceToUpgrade || !HasUpgradeableCat(roster))
            {
                return;
            }

            teamExperience -= ExperienceToUpgrade;
            hasPendingUpgrade = true;
            offerSeed++;
        }

        private bool HasUpgradeableCat(RunPartnerRoster roster)
        {
            if (roster == null)
            {
                return false;
            }

            IReadOnlyList<CatUpgradeCandidate> offer = P0CatUpgradeCatalog.CreateOffer(
                roster.CatIds,
                offerSeed,
                GetUpgradeCount);
            return offer.Count > 0;
        }

        private static string BuildSelectionSummary(CatUpgradeCandidate selected)
        {
            CatPresentation presentation = P0CatPresenter.Describe(selected.CatId);
            return presentation.ShortLabel
                + " 获得 "
                + selected.StageLabel
                + "《"
                + selected.DisplayName
                + "》";
        }
    }
}
