using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public enum P0BattleActionAffordanceKind
    {
        Skill,
        Interaction
    }

    public readonly struct P0BattleActionAffordance
    {
        public P0BattleActionAffordance(
            P0BattleActionAffordanceKind kind,
            string id,
            string title,
            string status,
            string detail,
            bool isEnabled)
        {
            Kind = kind;
            Id = id ?? string.Empty;
            Title = title ?? string.Empty;
            Status = status ?? string.Empty;
            Detail = detail ?? string.Empty;
            IsEnabled = isEnabled;
        }

        public P0BattleActionAffordanceKind Kind { get; }

        public string Id { get; }

        public string Title { get; }

        public string Status { get; }

        public string Detail { get; }

        public bool IsEnabled { get; }

        public string BuildButtonLabel()
        {
            string label = Title;
            if (!string.IsNullOrWhiteSpace(Status))
            {
                label += "\n" + Status;
            }

            if (!string.IsNullOrWhiteSpace(Detail))
            {
                label += "\n" + Detail;
            }

            return label;
        }

        public string BuildSummary()
        {
            string summary = FormatKind(Kind) + "：" + Title;
            if (!string.IsNullOrWhiteSpace(Status))
            {
                summary += " - " + Status;
            }

            if (!string.IsNullOrWhiteSpace(Detail))
            {
                summary += " (" + Detail + ")";
            }

            return summary;
        }

        private static string FormatKind(P0BattleActionAffordanceKind kind)
        {
            switch (kind)
            {
                case P0BattleActionAffordanceKind.Skill:
                    return "技能";
                case P0BattleActionAffordanceKind.Interaction:
                    return "交互";
                default:
                    return "行动";
            }
        }
    }

    public static class P0BattleActionAffordancePresenter
    {
        private const string BedCareId = "bed_care";
        private const string LitterBoxId = "litter_box";
        private const string FeederId = "feeder";

        public static P0BattleActionAffordance BuildSkill(
            SkillDefinition skill,
            float cooldownSeconds,
            float currentHunger,
            P0SkillTargetResult target,
            bool battleActive)
        {
            if (skill == null)
            {
                return new P0BattleActionAffordance(
                    P0BattleActionAffordanceKind.Skill,
                    "missing_skill",
                    "缺失技能",
                    "缺少定义",
                    "检查原型目录",
                    false);
            }

            SkillPresentation presentation = P0SkillPresenter.Describe(skill);
            string title = presentation.DisplayName;
            string detail = BuildSkillDetail(skill, presentation, currentHunger, target);
            if (!battleActive)
            {
                return BuildSkillAffordance(skill, title, "未激活", detail, false);
            }

            if (cooldownSeconds > 0f)
            {
                return BuildSkillAffordance(skill, title, "冷却 " + cooldownSeconds.ToString("0.0") + "s", detail, false);
            }

            if (!target.CanCast)
            {
                string status = target.RequiresEnemyTarget
                    ? "需要目标 <= " + target.Range.ToString("0.0") + "m"
                    : "不可用";
                return BuildSkillAffordance(skill, title, status, detail, false);
            }

            string readyStatus = currentHunger < skill.HungerCost
                ? "就绪，饱肚度偏低"
                : "就绪";
            return BuildSkillAffordance(skill, title, readyStatus, detail, true);
        }

        public static P0BattleActionAffordance BuildBedCare(
            bool battleActive,
            bool inRange,
            OwnerSleepState sleep,
            TeamHungerGauge hunger)
        {
            string status = BuildInteractionStatus(battleActive, inRange, "靠近床");
            string detail = "恢复睡眠 +"
                + TheCat.Combat.BattleSimulation.BedCareSleepRestoreAmount.ToString("0")
                + "，饱肚 -"
                + TheCat.Combat.BattleSimulation.BedCareHungerCost.ToString("0")
                + " | "
                + BuildSleepDetail(sleep)
                + " | "
                + BuildHungerDetail(hunger);
            return BuildInteraction(BedCareId, "照看床 [B]", status, detail, battleActive && inRange);
        }

        public static P0BattleActionAffordance BuildLitterBox(
            bool battleActive,
            bool inRange,
            TeamPoopGauge poop)
        {
            string status = BuildInteractionStatus(battleActive, inRange, "靠近猫砂盆");
            string detail = "降低屎意并取消倒计时 | " + BuildPoopDetail(poop);
            return BuildInteraction(LitterBoxId, "猫砂盆 [L]", status, detail, battleActive && inRange);
        }

        public static P0BattleActionAffordance BuildFeeder(
            bool battleActive,
            bool inRange,
            TeamHungerGauge hunger)
        {
            string status = BuildInteractionStatus(battleActive, inRange, "靠近喂食器");
            string detail = "恢复饱肚 +"
                + TeamHungerGauge.FeederRestoreAmount.ToString("0")
                + "，消化 "
                + TeamHungerGauge.DigestionDurationSeconds.ToString("0")
                + "s | "
                + BuildHungerDetail(hunger);
            return BuildInteraction(FeederId, "喂食器 [F]", status, detail, battleActive && inRange);
        }

        public static bool HasP0BattleActionAffordances(IReadOnlyList<P0BattleActionAffordance> affordances)
        {
            if (affordances == null)
            {
                return false;
            }

            int skillCount = 0;
            bool hasBedCare = false;
            bool hasLitterBox = false;
            bool hasFeeder = false;
            for (int i = 0; i < affordances.Count; i++)
            {
                P0BattleActionAffordance affordance = affordances[i];
                if (string.IsNullOrWhiteSpace(affordance.BuildButtonLabel()))
                {
                    return false;
                }

                if (affordance.Kind == P0BattleActionAffordanceKind.Skill)
                {
                    skillCount++;
                }

                hasBedCare |= affordance.Id == BedCareId;
                hasLitterBox |= affordance.Id == LitterBoxId;
                hasFeeder |= affordance.Id == FeederId;
            }

            return skillCount >= 3 && hasBedCare && hasLitterBox && hasFeeder;
        }

        public static string BuildCompactSummary(IReadOnlyList<P0BattleActionAffordance> affordances)
        {
            if (affordances == null || affordances.Count == 0)
            {
                return "战斗行动：空";
            }

            int skillCount = 0;
            int interactionCount = 0;
            int enabledCount = 0;
            for (int i = 0; i < affordances.Count; i++)
            {
                if (affordances[i].Kind == P0BattleActionAffordanceKind.Skill)
                {
                    skillCount++;
                }
                else if (affordances[i].Kind == P0BattleActionAffordanceKind.Interaction)
                {
                    interactionCount++;
                }

                if (affordances[i].IsEnabled)
                {
                    enabledCount++;
                }
            }

            return "战斗行动：技能 "
                + skillCount
                + "，交互 "
                + interactionCount
                + "，可用 "
                + enabledCount;
        }

        private static P0BattleActionAffordance BuildSkillAffordance(
            SkillDefinition skill,
            string title,
            string status,
            string detail,
            bool isEnabled)
        {
            return new P0BattleActionAffordance(
                P0BattleActionAffordanceKind.Skill,
                skill.Id,
                title,
                status,
                detail,
                isEnabled);
        }

        private static P0BattleActionAffordance BuildInteraction(
            string id,
            string title,
            string status,
            string detail,
            bool isEnabled)
        {
            return new P0BattleActionAffordance(
                P0BattleActionAffordanceKind.Interaction,
                id,
                title,
                status,
                detail,
                isEnabled);
        }

        private static string BuildSkillDetail(
            SkillDefinition skill,
            SkillPresentation presentation,
            float currentHunger,
            P0SkillTargetResult target)
        {
            string detail = FormatSkillSlot(skill.Slot)
                + " 饱肚 "
                + currentHunger.ToString("0")
                + "->"
                + System.Math.Max(0f, currentHunger - skill.HungerCost).ToString("0");
            if (!string.IsNullOrWhiteSpace(presentation.EffectHint))
            {
                detail += " | " + presentation.EffectHint;
            }

            detail += " | " + BuildTargetDetail(target);
            return detail;
        }

        private static string BuildTargetDetail(P0SkillTargetResult target)
        {
            if (!target.RequiresEnemyTarget)
            {
                return "无需目标";
            }

            if (target.HasEnemyTarget)
            {
                return "目标 "
                    + target.Enemy.Definition.DisplayName
                    + " "
                    + target.Distance.ToString("0.0")
                    + "/"
                    + target.Range.ToString("0.0")
                    + "m";
            }

            return "无目标 <= " + target.Range.ToString("0.0") + "m";
        }

        private static string FormatSkillSlot(SkillSlot slot)
        {
            switch (slot)
            {
                case SkillSlot.SmallSkill1:
                    return "小技能1";
                case SkillSlot.SmallSkill2:
                    return "小技能2";
                case SkillSlot.SmallSkill3:
                    return "小技能3";
                case SkillSlot.Ultimate1:
                    return "大招";
                default:
                    return "技能";
            }
        }

        private static string BuildInteractionStatus(bool battleActive, bool inRange, string moveCloserText)
        {
            if (!battleActive)
            {
                return "未激活";
            }

            return inRange ? "就绪" : moveCloserText;
        }

        private static string BuildSleepDetail(OwnerSleepState sleep)
        {
            return sleep == null
                ? "睡眠缺失"
                : "睡眠 " + sleep.Current.ToString("0") + "/" + sleep.Max.ToString("0");
        }

        private static string BuildPoopDetail(TeamPoopGauge poop)
        {
            if (poop == null)
            {
                return "屎意缺失";
            }

            string detail = "屎意 " + poop.Current.ToString("0") + "/100";
            if (poop.IsCountdownActive)
            {
                detail += " 倒计时 " + poop.CountdownRemainingSeconds.ToString("0") + "s";
            }

            return detail;
        }

        private static string BuildHungerDetail(TeamHungerGauge hunger)
        {
            if (hunger == null)
            {
                return "饱肚缺失";
            }

            string detail = "饱肚 " + hunger.Current.ToString("0") + "/100";
            if (hunger.IsDigesting)
            {
                detail += " 消化中 " + hunger.DigestionRemainingSeconds.ToString("0") + "s";
            }

            return detail;
        }
    }
}
