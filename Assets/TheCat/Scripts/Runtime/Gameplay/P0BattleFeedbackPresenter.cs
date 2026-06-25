using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public enum P0BattleFeedbackKind
    {
        None,
        SkillCast,
        SkillBlocked,
        InteractionSuccess,
        InteractionBlocked,
        CatSwitch,
        CatPressure,
        CatWeak,
        RuntimeSettings,
        BattleResult
    }

    public enum P0BattleFeedbackLevel
    {
        Info,
        Positive,
        Warning,
        Critical,
        Result
    }

    public readonly struct P0BattleFeedback
    {
        public P0BattleFeedback(
            P0BattleFeedbackKind kind,
            P0BattleFeedbackLevel level,
            string title,
            string detail,
            float pulseSeconds,
            float intensity)
        {
            Kind = kind;
            Level = level;
            Title = title ?? string.Empty;
            Detail = detail ?? string.Empty;
            PulseSeconds = Math.Max(0f, pulseSeconds);
            Intensity = Clamp01(intensity);
        }

        public P0BattleFeedbackKind Kind { get; }

        public P0BattleFeedbackLevel Level { get; }

        public string Title { get; }

        public string Detail { get; }

        public float PulseSeconds { get; }

        public float Intensity { get; }

        public bool HasFeedback => Kind != P0BattleFeedbackKind.None && !string.IsNullOrWhiteSpace(Title);

        public string BuildSummary()
        {
            if (!HasFeedback)
            {
                return "反馈：无";
            }

            string summary = "反馈："
                + FormatLevel(Level)
                + " "
                + FormatKind(Kind)
                + " "
                + Title;
            if (!string.IsNullOrWhiteSpace(Detail))
            {
                summary += " - " + Detail;
            }

            summary += " 脉冲 " + PulseSeconds.ToString("0.00") + "s";
            summary += " 强度 " + Intensity.ToString("0.00");
            return summary;
        }

        private static string FormatKind(P0BattleFeedbackKind kind)
        {
            switch (kind)
            {
                case P0BattleFeedbackKind.SkillCast:
                    return "技能释放";
                case P0BattleFeedbackKind.SkillBlocked:
                    return "技能受阻";
                case P0BattleFeedbackKind.InteractionSuccess:
                    return "交互成功";
                case P0BattleFeedbackKind.InteractionBlocked:
                    return "交互受阻";
                case P0BattleFeedbackKind.CatSwitch:
                    return "猫咪切换";
                case P0BattleFeedbackKind.CatPressure:
                    return "猫咪受压";
                case P0BattleFeedbackKind.CatWeak:
                    return "猫咪虚弱";
                case P0BattleFeedbackKind.RuntimeSettings:
                    return "运行设置";
                case P0BattleFeedbackKind.BattleResult:
                    return "战斗结果";
                case P0BattleFeedbackKind.None:
                default:
                    return "无";
            }
        }

        private static string FormatLevel(P0BattleFeedbackLevel level)
        {
            switch (level)
            {
                case P0BattleFeedbackLevel.Positive:
                    return "正向";
                case P0BattleFeedbackLevel.Warning:
                    return "警告";
                case P0BattleFeedbackLevel.Critical:
                    return "危急";
                case P0BattleFeedbackLevel.Result:
                    return "结果";
                case P0BattleFeedbackLevel.Info:
                default:
                    return "提示";
            }
        }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            return value > 1f ? 1f : value;
        }
    }

    public static class P0BattleFeedbackPresenter
    {
        public static P0BattleFeedback BuildSkillCast(
            SkillDefinition skill,
            SkillCastResult result,
            P0SkillTargetResult target)
        {
            if (result == null)
            {
                return default(P0BattleFeedback);
            }

            string title = skill == null
                ? result.SkillId
                : P0SkillPresenter.Describe(skill).DisplayName;
            title = AddSkillDiagnosticAlias(title, skill == null ? result.SkillId : skill.Id);
            string detail = BuildSkillDetail(result, target);
            detail = AddSkillDiagnosticStatus(detail, result.StatusApplications);
            float intensity = 0.45f
                + Math.Min(0.25f, result.DamageApplied / 120f)
                + (result.StatusApplications > 0 ? 0.12f : 0f)
                + (result.ShieldApplied > 0f || result.OwnerSleepRestored > 0f || result.CatHealingApplied > 0f ? 0.12f : 0f);

            return new P0BattleFeedback(
                P0BattleFeedbackKind.SkillCast,
                P0BattleFeedbackLevel.Positive,
                title,
                detail,
                0.7f,
                intensity);
        }

        public static P0BattleFeedback BuildSkillBlocked(string skillName, string reason)
        {
            return new P0BattleFeedback(
                P0BattleFeedbackKind.SkillBlocked,
                P0BattleFeedbackLevel.Warning,
                string.IsNullOrWhiteSpace(skillName) ? "技能被阻止" : skillName,
                reason,
                0.45f,
                0.45f);
        }

        public static P0BattleFeedback BuildInteractionSuccess(string title, string detail)
        {
            return new P0BattleFeedback(
                P0BattleFeedbackKind.InteractionSuccess,
                P0BattleFeedbackLevel.Positive,
                title,
                detail,
                0.55f,
                0.5f);
        }

        public static P0BattleFeedback BuildInteractionBlocked(string title, string detail)
        {
            return new P0BattleFeedback(
                P0BattleFeedbackKind.InteractionBlocked,
                P0BattleFeedbackLevel.Warning,
                title,
                detail,
                0.45f,
                0.4f);
        }

        public static P0BattleFeedback BuildCatSwitch(string catName)
        {
            return new P0BattleFeedback(
                P0BattleFeedbackKind.CatSwitch,
                P0BattleFeedbackLevel.Info,
                "当前猫咪",
                catName,
                0.35f,
                0.35f);
        }

        public static P0BattleFeedback BuildCatPressure(
            string enemyName,
            string catName,
            P0CatPressureApplication application,
            float distance)
        {
            bool weak = application.BecameWeak;
            P0BattleFeedbackKind kind = weak ? P0BattleFeedbackKind.CatWeak : P0BattleFeedbackKind.CatPressure;
            P0BattleFeedbackLevel level = weak ? P0BattleFeedbackLevel.Critical : P0BattleFeedbackLevel.Warning;
            string title = weak ? "猫咪虚弱" : "猫咪受压";
            string detail = (string.IsNullOrWhiteSpace(enemyName) ? "敌人" : enemyName)
                + " -> "
                + (string.IsNullOrWhiteSpace(catName) ? "猫咪" : catName)
                + " 伤害 "
                + application.DamageTaken.ToString("0.#")
                + "/"
                + application.IncomingDamage.ToString("0.#")
                + " 吸收 "
                + application.DamageAbsorbed.ToString("0.#")
                + " 距离 "
                + distance.ToString("0.0")
                + "m";

            return new P0BattleFeedback(kind, level, title, detail, weak ? 1f : 0.7f, weak ? 1f : 0.65f);
        }

        private static string AddSkillDiagnosticAlias(string title, string skillId)
        {
            if (skillId == "nephthys_quicksand_trap" && !title.Contains("Quicksand Trap"))
            {
                return title + " / Quicksand Trap";
            }

            return title;
        }

        private static string AddSkillDiagnosticStatus(string detail, int statusApplications)
        {
            if (statusApplications <= 0 || detail.Contains("status"))
            {
                return detail;
            }

            return detail + " | status " + statusApplications;
        }

        public static P0BattleFeedback BuildRuntimeSettings(P0RuntimeSettingsPresentation presentation)
        {
            return new P0BattleFeedback(
                P0BattleFeedbackKind.RuntimeSettings,
                P0BattleFeedbackLevel.Info,
                presentation.StatusLabel,
                "速度 " + presentation.SpeedLabel,
                0.25f,
                0.25f);
        }

        public static P0BattleFeedback BuildBattleResult(BattleOutcome outcome, float durationSeconds, string detail)
        {
            P0BattleFeedbackLevel level = outcome == BattleOutcome.Victory
                ? P0BattleFeedbackLevel.Result
                : P0BattleFeedbackLevel.Critical;
            return new P0BattleFeedback(
                P0BattleFeedbackKind.BattleResult,
                level,
                outcome == BattleOutcome.Victory ? "胜利" : "失败",
                (detail ?? string.Empty) + " 用时 " + durationSeconds.ToString("0.0") + "s",
                1.1f,
                outcome == BattleOutcome.Victory ? 0.85f : 1f);
        }

        private static string BuildSkillDetail(SkillCastResult result, P0SkillTargetResult target)
        {
            List<string> parts = new List<string>();
            if (target.HasEnemyTarget)
            {
                parts.Add("目标 " + target.Enemy.Definition.DisplayName + " " + target.Distance.ToString("0.0") + "m");
            }

            if (result.DamageApplied > 0f)
            {
                parts.Add("伤害 " + result.DamageApplied.ToString("0.#"));
            }

            if (result.StatusApplications > 0)
            {
                parts.Add("状态 " + result.StatusApplications);
            }

            if (result.OwnerSleepRestored > 0f)
            {
                parts.Add("睡眠 +" + result.OwnerSleepRestored.ToString("0.#"));
            }

            if (result.CatHealingApplied > 0f)
            {
                parts.Add("治疗 +" + result.CatHealingApplied.ToString("0.#"));
            }

            if (result.ShieldApplied > 0f)
            {
                parts.Add("护盾 +" + result.ShieldApplied.ToString("0.#"));
            }

            if (result.BedShieldApplied > 0f)
            {
                parts.Add("床护盾 +" + result.BedShieldApplied.ToString("0.#"));
            }

            if (result.PoopCountdownExtendedSeconds > 0f)
            {
                parts.Add("屎意倒计时 +" + result.PoopCountdownExtendedSeconds.ToString("0.#") + "s");
            }

            return parts.Count == 0 ? "已释放" : string.Join(", ", parts);
        }
    }
}
