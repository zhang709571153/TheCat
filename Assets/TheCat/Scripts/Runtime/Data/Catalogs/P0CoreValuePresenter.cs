using System.Collections.Generic;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Data.Catalogs
{
    public static class P0CoreValuePresenter
    {
        public static CoreValuePresentation DescribeOwnerSleep(OwnerSleepState sleep)
        {
            if (sleep == null)
            {
                return new CoreValuePresentation("主人睡眠度", "0/0", "缺失", "请启动战斗状态");
            }

            return new CoreValuePresentation(
                "主人睡眠度",
                FormatCurrentMax(sleep.Current, sleep.Max),
                FormatOwnerSleepStage(sleep.Stage),
                GetOwnerSleepHint(sleep.Stage),
                FormatMaxLoss(sleep.Max, sleep.BaseMax),
                P0VisualAssetCatalog.GetOwnerSleepIcon(),
                P0VisualAssetCatalog.GetCoreGaugeFrame("owner_sleep"),
                P0VisualAssetCatalog.GetCoreGaugeFill("owner_sleep"),
                Ratio(sleep.Current, sleep.Max));
        }

        public static CoreValuePresentation DescribeTeamPoop(TeamPoopGauge poop)
        {
            if (poop == null)
            {
                return new CoreValuePresentation("屎意值", "0/100", "缺失", "请启动战斗状态");
            }

            string detail = poop.IsCountdownActive
                ? "倒计时 " + poop.CountdownRemainingSeconds.ToString("0") + "s"
                : string.Empty;
            return new CoreValuePresentation(
                "屎意值",
                FormatCurrentMax(poop.Current, TeamPoopGauge.MaxValue),
                FormatPoopStage(poop.Stage),
                GetPoopHint(poop.Stage, poop.IsCountdownActive),
                detail,
                P0VisualAssetCatalog.GetTeamPoopIcon(),
                P0VisualAssetCatalog.GetCoreGaugeFrame("team_poop"),
                P0VisualAssetCatalog.GetCoreGaugeFill("team_poop"),
                Ratio(poop.Current, TeamPoopGauge.MaxValue));
        }

        public static CoreValuePresentation DescribeTeamHunger(TeamHungerGauge hunger)
        {
            if (hunger == null)
            {
                return new CoreValuePresentation("饱肚度", "0/100", "缺失", "请启动战斗状态");
            }

            string detail = "伤害 " + hunger.DamageMultiplier.ToString("0.##") + " 倍";
            if (hunger.IsDigesting)
            {
                detail += " 消化中 " + hunger.DigestionRemainingSeconds.ToString("0") + "s";
            }

            return new CoreValuePresentation(
                "饱肚度",
                FormatCurrentMax(hunger.Current, TeamHungerGauge.MaxValue),
                FormatHungerStage(hunger.Stage),
                GetHungerHint(hunger.Stage),
                detail,
                P0VisualAssetCatalog.GetTeamHungerIcon(),
                P0VisualAssetCatalog.GetCoreGaugeFrame("team_hunger"),
                P0VisualAssetCatalog.GetCoreGaugeFill("team_hunger"),
                Ratio(hunger.Current, TeamHungerGauge.MaxValue));
        }

        public static string BuildBattleCoreSummary(
            OwnerSleepState sleep,
            TeamPoopGauge poop,
            TeamHungerGauge hunger)
        {
            List<string> summaries = new List<string>
            {
                DescribeOwnerSleep(sleep).BuildSummary(),
                DescribeTeamPoop(poop).BuildSummary(),
                DescribeTeamHunger(hunger).BuildSummary()
            };

            return string.Join("; ", summaries);
        }

        public static string BuildRunCoreSummary(RunCoreValues values)
        {
            if (values == null)
            {
                return "核心：未初始化";
            }

            OwnerSleepState sleep = new OwnerSleepState(
                values.OwnerSleepCurrent,
                values.OwnerSleepMax,
                values.OwnerSleepBaseMax);
            TeamPoopGauge poop = new TeamPoopGauge(values.TeamPoop);
            TeamHungerGauge hunger = new TeamHungerGauge(values.TeamHunger);

            return BuildBattleCoreSummary(sleep, poop, hunger);
        }

        public static string BuildSettlementCoreSummary(P0RunSettlementSummary summary)
        {
            if (summary.OwnerSleepMax <= 0f)
            {
                return "核心：未初始化";
            }

            OwnerSleepState sleep = new OwnerSleepState(
                summary.OwnerSleepCurrent,
                summary.OwnerSleepMax,
                OwnerSleepState.DefaultBaseMax);
            TeamPoopGauge poop = new TeamPoopGauge(summary.TeamPoop);
            TeamHungerGauge hunger = new TeamHungerGauge(summary.TeamHunger);

            return BuildBattleCoreSummary(sleep, poop, hunger);
        }

        private static string FormatCurrentMax(float current, float max)
        {
            return current.ToString("0") + "/" + max.ToString("0");
        }

        private static float Ratio(float current, float max)
        {
            return max <= 0f ? 0f : current / max;
        }

        private static string FormatMaxLoss(float max, float baseMax)
        {
            float lost = baseMax - max;
            return lost <= 0f ? string.Empty : "上限损失 " + lost.ToString("0");
        }

        private static string FormatOwnerSleepStage(OwnerSleepStage stage)
        {
            switch (stage)
            {
                case OwnerSleepStage.Stable:
                    return "稳定";
                case OwnerSleepStage.Uneasy:
                    return "不安";
                case OwnerSleepStage.Danger:
                    return "危险";
                case OwnerSleepStage.Critical:
                    return "危急";
                case OwnerSleepStage.Failed:
                    return "失败";
                default:
                    return "未知";
            }
        }

        private static string FormatPoopStage(PoopStage stage)
        {
            switch (stage)
            {
                case PoopStage.Normal:
                    return "正常";
                case PoopStage.Medium:
                    return "偏高";
                case PoopStage.High:
                    return "高";
                case PoopStage.Critical:
                    return "危急";
                default:
                    return "未知";
            }
        }

        private static string FormatHungerStage(HungerStage stage)
        {
            switch (stage)
            {
                case HungerStage.Full:
                    return "充足";
                case HungerStage.Hungry:
                    return "饥饿";
                case HungerStage.Starving:
                    return "很饿";
                case HungerStage.Empty:
                    return "空腹";
                default:
                    return "未知";
            }
        }

        private static string GetOwnerSleepHint(OwnerSleepStage stage)
        {
            switch (stage)
            {
                case OwnerSleepStage.Uneasy:
                    return "关注床边压力";
                case OwnerSleepStage.Danger:
                    return "使用床交互或安眠技能";
                case OwnerSleepStage.Critical:
                    return "立刻守住床";
                case OwnerSleepStage.Failed:
                    return "路线失败";
                default:
                    return string.Empty;
            }
        }

        private static string GetPoopHint(PoopStage stage, bool isCountdownActive)
        {
            if (isCountdownActive)
            {
                return "立刻使用猫砂盆";
            }

            switch (stage)
            {
                case PoopStage.High:
                    return "准备去猫砂盆";
                case PoopStage.Critical:
                    return "尽快使用猫砂盆";
                default:
                    return string.Empty;
            }
        }

        private static string GetHungerHint(HungerStage stage)
        {
            switch (stage)
            {
                case HungerStage.Hungry:
                    return "伤害降低";
                case HungerStage.Starving:
                    return "尽快使用喂食器";
                case HungerStage.Empty:
                    return "立刻使用喂食器";
                default:
                    return string.Empty;
            }
        }
    }
}
