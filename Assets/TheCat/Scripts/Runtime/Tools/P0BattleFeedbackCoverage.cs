using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;

namespace TheCat.Tools
{
    public enum P0BattleFeedbackCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0BattleFeedbackCoverageIssue
    {
        public P0BattleFeedbackCoverageIssue(P0BattleFeedbackCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0BattleFeedbackCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0BattleFeedbackCoverageReport
    {
        private readonly List<P0BattleFeedbackCoverageIssue> issues = new List<P0BattleFeedbackCoverageIssue>();
        private readonly List<string> coveredFeedback = new List<string>();

        public IReadOnlyList<P0BattleFeedbackCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredFeedback => coveredFeedback.AsReadOnly();

        public int FailureCount => Count(P0BattleFeedbackCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredFeedback.Count >= P0BattleFeedbackCoverage.ExpectedCoveredFeedbackCount;

        public void AddIssue(P0BattleFeedbackCoverageSeverity severity, string message)
        {
            issues.Add(new P0BattleFeedbackCoverageIssue(severity, message));
        }

        public void AddCoveredFeedback(string feedback)
        {
            if (!string.IsNullOrWhiteSpace(feedback))
            {
                coveredFeedback.Add(feedback);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 battle feedback coverage complete for " + coveredFeedback.Count + " feedback check(s)."
                : "P0 battle feedback coverage has " + FailureCount + " failure(s) across " + coveredFeedback.Count + " feedback check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredFeedback.Count; i++)
            {
                lines.Add("- " + coveredFeedback[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0BattleFeedbackCoverageSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0BattleFeedbackCoverage
    {
        public const int ExpectedCoveredFeedbackCount = 6;

        public static P0BattleFeedbackCoverageReport EvaluatePrototypeFeedback()
        {
            P0BattleFeedbackCoverageReport report = new P0BattleFeedbackCoverageReport();

            EvaluateSkillCast(report);
            EvaluateSkillBlocked(report);
            EvaluateInteractionFeedback(report);
            EvaluateCatPressure(report);
            EvaluateShieldedCatPressure(report);
            EvaluateBattleResult(report);

            return report;
        }

        private static void EvaluateSkillCast(P0BattleFeedbackCoverageReport report)
        {
            BattleSimulation battle = CreateBattle();
            battle.Tick(0.1f);
            SkillDefinition skill = GetSkill("nephthys_quicksand_trap");
            BattleEnemyState enemy = battle.ActiveEnemies[0];
            P0SkillTargetResult target = new P0SkillTargetResult(true, enemy, 1.5f, 5.25f);

            SkillCastResult result = battle.CastSkill(skill, new CatBattleState(GetCat(P0PrototypeCatalog.NephthysId)), enemy);
            P0BattleFeedback feedback = P0BattleFeedbackPresenter.BuildSkillCast(skill, result, target);

            Require(
                report,
                feedback.Kind == P0BattleFeedbackKind.SkillCast
                && feedback.Level == P0BattleFeedbackLevel.Positive
                && feedback.Intensity > 0.45f
                && feedback.PulseSeconds > 0f
                && feedback.BuildSummary().Contains("Quicksand Trap")
                && feedback.BuildSummary().Contains("status"),
                "Skill cast feedback summarizes skill result, target, status, pulse, and intensity.",
                "Skill cast feedback is missing result details.");
        }

        private static void EvaluateSkillBlocked(P0BattleFeedbackCoverageReport report)
        {
            P0BattleFeedback feedback = P0BattleFeedbackPresenter.BuildSkillBlocked("安眠铃", "冷却 2.0s");

            Require(
                report,
                feedback.Kind == P0BattleFeedbackKind.SkillBlocked
                && feedback.Level == P0BattleFeedbackLevel.Warning
                && feedback.BuildSummary().Contains("冷却 2.0s"),
                "Blocked skill feedback exposes the skill name and blocking reason.",
                "Blocked skill feedback is missing name or reason.");
        }

        private static void EvaluateInteractionFeedback(P0BattleFeedbackCoverageReport report)
        {
            P0BattleFeedback success = P0BattleFeedbackPresenter.BuildInteractionSuccess("守床照看", "睡眠 +12");
            P0BattleFeedback blocked = P0BattleFeedbackPresenter.BuildInteractionBlocked("猫砂盆", "请靠近一点");

            Require(
                report,
                success.Kind == P0BattleFeedbackKind.InteractionSuccess
                && success.Level == P0BattleFeedbackLevel.Positive
                && blocked.Kind == P0BattleFeedbackKind.InteractionBlocked
                && blocked.Level == P0BattleFeedbackLevel.Warning
                && success.BuildSummary().Contains("睡眠 +12")
                && blocked.BuildSummary().Contains("请靠近一点"),
                "Interaction feedback distinguishes success and range-blocked states.",
                "Interaction feedback did not distinguish success and blocked states.");
        }

        private static void EvaluateCatPressure(P0BattleFeedbackCoverageReport report)
        {
            NodeMetrics metrics = new NodeMetrics(1, "feedback_cat_pressure", 100f);
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.SuzuneId), currentHp: 5f);
            BattleEnemyState enemy = new BattleEnemyState(1, GetEnemy(P0PrototypeCatalog.RedEyeAlarmId), 4f);
            P0CatPressureApplication application = P0CatPressureApplier.Apply(metrics, cat, enemy, 10f, 1f);

            P0BattleFeedback feedback = P0BattleFeedbackPresenter.BuildCatPressure(
                enemy.Definition.DisplayName,
                cat.Definition.DisplayName,
                application,
                2.4f);

            Require(
                report,
                feedback.Kind == P0BattleFeedbackKind.CatWeak
                && feedback.Level == P0BattleFeedbackLevel.Critical
                && feedback.Intensity >= 1f
                && feedback.BuildSummary().Contains("吸收")
                && metrics.WeakIncidents == 1,
                "Cat pressure feedback escalates to critical when a cat becomes weak.",
                "Cat pressure feedback did not escalate weak-cat pressure.");
        }

        private static void EvaluateShieldedCatPressure(P0BattleFeedbackCoverageReport report)
        {
            NodeMetrics metrics = new NodeMetrics(1, "feedback_cat_shield_pressure", 100f);
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));
            cat.ApplyStatus(GetStatus(StatusTagIds.Shield), 30f);
            BattleEnemyState enemy = new BattleEnemyState(1, GetEnemy(P0PrototypeCatalog.BlackMudNightmareId), 5f);
            P0CatPressureApplication application = P0CatPressureApplier.Apply(metrics, cat, enemy, 1f, 1f);

            P0BattleFeedback feedback = P0BattleFeedbackPresenter.BuildCatPressure(
                enemy.Definition.DisplayName,
                cat.Definition.DisplayName,
                application,
                1.8f);

            string summary = feedback.BuildSummary();
            Require(
                report,
                feedback.Kind == P0BattleFeedbackKind.CatPressure
                && feedback.Level == P0BattleFeedbackLevel.Warning
                && application.DamageTaken <= 0f
                && application.DamageAbsorbed > 0f
                && metrics.CatDamageTaken <= 0f
                && metrics.CatDamageAbsorbed > 0f
                && summary.Contains("伤害 0/")
                && summary.Contains("吸收"),
                "Shielded cat pressure still reports absorbed damage without weak escalation.",
                "Shielded cat pressure feedback did not report absorbed-only pressure.");
        }

        private static void EvaluateBattleResult(P0BattleFeedbackCoverageReport report)
        {
            P0BattleFeedback victory = P0BattleFeedbackPresenter.BuildBattleResult(BattleOutcome.Victory, 33.5f, "路线推进");
            P0BattleFeedback defeat = P0BattleFeedbackPresenter.BuildBattleResult(BattleOutcome.Defeat, 41f, "主人睡眠崩溃");

            Require(
                report,
                victory.Kind == P0BattleFeedbackKind.BattleResult
                && victory.Level == P0BattleFeedbackLevel.Result
                && defeat.Kind == P0BattleFeedbackKind.BattleResult
                && defeat.Level == P0BattleFeedbackLevel.Critical
                && victory.BuildSummary().Contains("33.5s")
                && defeat.BuildSummary().Contains("主人睡眠崩溃"),
                "Battle result feedback distinguishes victory and defeat outcomes.",
                "Battle result feedback did not distinguish outcome levels.");
        }

        private static BattleSimulation CreateBattle()
        {
            return new BattleSimulation(
                new BattleSimulationConfig(
                    P0PrototypeCatalog.CreateLayerOneWave(),
                    P0PrototypeCatalog.CreateCoreEnemies(),
                    P0Tuning.Default,
                    statusTags: P0PrototypeCatalog.CreateStatusTags()),
                new RunMetrics());
        }

        private static SkillDefinition GetSkill(string skillId)
        {
            IReadOnlyList<SkillDefinition> skills = P0PrototypeCatalog.CreateStarterSkills();
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].Id == skillId)
                {
                    return skills[i];
                }
            }

            throw new InvalidOperationException("Missing skill: " + skillId);
        }

        private static CatDefinition GetCat(string catId)
        {
            IReadOnlyList<CatDefinition> cats = P0PrototypeCatalog.CreateStarterCats();
            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i].Id == catId)
                {
                    return cats[i];
                }
            }

            throw new InvalidOperationException("Missing cat: " + catId);
        }

        private static EnemyDefinition GetEnemy(string enemyId)
        {
            IReadOnlyList<EnemyDefinition> enemies = P0PrototypeCatalog.CreateCoreEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Id == enemyId)
                {
                    return enemies[i];
                }
            }

            throw new InvalidOperationException("Missing enemy: " + enemyId);
        }

        private static StatusTagDefinition GetStatus(string statusId)
        {
            IReadOnlyList<StatusTagDefinition> statuses = P0PrototypeCatalog.CreateStatusTags();
            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i].Id == statusId)
                {
                    return statuses[i];
                }
            }

            throw new InvalidOperationException("Missing status: " + statusId);
        }

        private static void Require(
            P0BattleFeedbackCoverageReport report,
            bool condition,
            string coveredFeedback,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredFeedback(coveredFeedback);
                return;
            }

            report.AddIssue(P0BattleFeedbackCoverageSeverity.Failure, failureMessage);
        }
    }
}
