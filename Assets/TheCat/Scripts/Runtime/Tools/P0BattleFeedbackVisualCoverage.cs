using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tools
{
    public enum P0BattleFeedbackVisualCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0BattleFeedbackVisualCoverageIssue
    {
        public P0BattleFeedbackVisualCoverageIssue(P0BattleFeedbackVisualCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0BattleFeedbackVisualCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0BattleFeedbackVisualCoverageReport
    {
        private readonly List<P0BattleFeedbackVisualCoverageIssue> issues = new List<P0BattleFeedbackVisualCoverageIssue>();
        private readonly List<string> coveredVisuals = new List<string>();

        public IReadOnlyList<P0BattleFeedbackVisualCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredVisuals => coveredVisuals.AsReadOnly();

        public int FailureCount => Count(P0BattleFeedbackVisualCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredVisuals.Count >= P0BattleFeedbackVisualCoverage.ExpectedCoveredVisualCount;

        public void AddIssue(P0BattleFeedbackVisualCoverageSeverity severity, string message)
        {
            issues.Add(new P0BattleFeedbackVisualCoverageIssue(severity, message));
        }

        public void AddCoveredVisual(string visual)
        {
            if (!string.IsNullOrWhiteSpace(visual))
            {
                coveredVisuals.Add(visual);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 battle feedback visual coverage complete for " + coveredVisuals.Count + " visual check(s)."
                : "P0 battle feedback visual coverage has " + FailureCount + " failure(s) across " + coveredVisuals.Count + " visual check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredVisuals.Count; i++)
            {
                lines.Add("- " + coveredVisuals[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0BattleFeedbackVisualCoverageSeverity severity)
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

    public static class P0BattleFeedbackVisualCoverage
    {
        public const int ExpectedCoveredVisualCount = 9;

        public static P0BattleFeedbackVisualCoverageReport EvaluatePrototypeVisuals()
        {
            P0BattleFeedbackVisualCoverageReport report = new P0BattleFeedbackVisualCoverageReport();

            EvaluateInfoPulse(report);
            EvaluatePositiveVisual(report);
            EvaluateStarterSkillVfxAssets(report);
            EvaluateWarningVisual(report);
            EvaluateCriticalVisual(report);
            EvaluateExpiredResultVisual(report);
            EvaluateInteractionVfxAssets(report);
            EvaluateSkillTargetMarkVfxAsset(report);
            EvaluateBossWarningVfxAsset(report);

            return report;
        }

        private static void EvaluateInfoPulse(P0BattleFeedbackVisualCoverageReport report)
        {
            P0BattleFeedback feedback = new P0BattleFeedback(
                P0BattleFeedbackKind.CatSwitch,
                P0BattleFeedbackLevel.Info,
                "Active Cat",
                "Saiban",
                0.4f,
                0.35f);
            P0BattleFeedbackVisualState visual = P0BattleFeedbackVisualPresenter.Build(feedback, 0.1f);

            Require(
                report,
                visual.HasVisual
                && visual.AccentToken == "blue"
                && visual.Progress01 > 0f
                && visual.Progress01 < 1f
                && visual.PulseFill01 > 0f
                && visual.PulseAlpha > 0f,
                "Info feedback visual exposes blue accent, active progress, and pulse fill.",
                "Info feedback visual did not expose an active blue pulse.");
        }

        private static void EvaluatePositiveVisual(P0BattleFeedbackVisualCoverageReport report)
        {
            P0BattleFeedback feedback = new P0BattleFeedback(
                P0BattleFeedbackKind.SkillCast,
                P0BattleFeedbackLevel.Positive,
                "通用护盾",
                "护盾 +35",
                0.7f,
                0.65f);
            P0BattleFeedbackVisualState visual = P0BattleFeedbackVisualPresenter.Build(feedback, 0f);

            Require(
                report,
                visual.HasVisual
                && visual.AccentToken == "green"
                && visual.VisualAsset.AssetId == P0VisualAssetCatalog.BedShieldPulseVfxId
                && visual.BackgroundColor.A > 0.5f
                && visual.PulseFill01 >= 0.99f,
                "Positive feedback visual uses green accent, shield VFX, and starts with full pulse fill.",
                "Positive feedback visual did not start with full green shield pulse.");
        }

        private static void EvaluateStarterSkillVfxAssets(P0BattleFeedbackVisualCoverageReport report)
        {
            P0BattleFeedbackVisualState saiban = P0BattleFeedbackVisualPresenter.Build(
                new P0BattleFeedback(
                    P0BattleFeedbackKind.SkillCast,
                    P0BattleFeedbackLevel.Positive,
                    "圆盾冲锋",
                    "伤害 24，标签 1",
                    0.7f,
                    0.65f),
                0f);
            P0BattleFeedbackVisualState nephthys = P0BattleFeedbackVisualPresenter.Build(
                new P0BattleFeedback(
                    P0BattleFeedbackKind.SkillCast,
                    P0BattleFeedbackLevel.Positive,
                    "月砂方尖碑",
                    "目标 黑泥梦魇 2.0m，标签 1",
                    0.7f,
                    0.65f),
                0f);
            P0BattleFeedbackVisualState suzune = P0BattleFeedbackVisualPresenter.Build(
                new P0BattleFeedback(
                    P0BattleFeedbackKind.SkillCast,
                    P0BattleFeedbackLevel.Positive,
                    "安眠铃",
                    "睡眠 +10，治疗 +20",
                    0.7f,
                    0.65f),
                0f);

            Require(
                report,
                saiban.VisualAsset.AssetId == P0VisualAssetCatalog.SaibanBedlineSkillVfxId
                && nephthys.VisualAsset.AssetId == P0VisualAssetCatalog.NephthysMoonsandSkillVfxId
                && suzune.VisualAsset.AssetId == P0VisualAssetCatalog.SuzuneLullabySkillVfxId
                && Contains(saiban.VisualAsset.SourceLockIds, "saiban_turnaround_colored")
                && Contains(nephthys.VisualAsset.SourceLockIds, "nephthys_turnaround_colored")
                && Contains(suzune.VisualAsset.SourceLockIds, "suzune_turnaround_colored"),
                "Starter skill feedback routes Saiban, Nephthys, and Suzune skill casts to Batch 61 symbolic VFX assets.",
                "Starter skill feedback did not route to the expected Batch 61 symbolic VFX assets.");
        }

        private static void EvaluateWarningVisual(P0BattleFeedbackVisualCoverageReport report)
        {
            P0BattleFeedback feedback = new P0BattleFeedback(
                P0BattleFeedbackKind.CatPressure,
                P0BattleFeedbackLevel.Warning,
                "猫咪受压",
                "吸收 12",
                0.8f,
                0.65f);
            P0BattleFeedbackVisualState visual = P0BattleFeedbackVisualPresenter.Build(feedback, 0.4f);

            Require(
                report,
                visual.HasVisual
                && visual.AccentToken == "amber"
                && visual.VisualAsset.AssetId == P0VisualAssetCatalog.HitSparkVfxId
                && visual.Progress01 > 0.45f
                && visual.Progress01 < 0.55f
                && visual.RemainingSeconds > 0f,
                "Warning feedback visual uses amber accent and reports mid-pulse progress.",
                "Warning feedback visual did not report the expected amber mid-pulse state.");
        }

        private static void EvaluateCriticalVisual(P0BattleFeedbackVisualCoverageReport report)
        {
            P0BattleFeedback feedback = new P0BattleFeedback(
                P0BattleFeedbackKind.CatWeak,
                P0BattleFeedbackLevel.Critical,
                "猫咪虚弱",
                "铃音虚弱",
                1f,
                1f);
            P0BattleFeedbackVisualState visual = P0BattleFeedbackVisualPresenter.Build(feedback, 0f);

            Require(
                report,
                visual.HasVisual
                && visual.AccentToken == "red"
                && visual.VisualAsset.AssetId == P0VisualAssetCatalog.HitSparkVfxId
                && visual.PulseAlpha >= 0.99f
                && visual.BackgroundColor.A >= 0.69f,
                "Critical feedback visual uses red accent and maximum pulse alpha.",
                "Critical feedback visual did not expose a maximum red pulse.");
        }

        private static void EvaluateExpiredResultVisual(P0BattleFeedbackVisualCoverageReport report)
        {
            P0BattleFeedback feedback = new P0BattleFeedback(
                P0BattleFeedbackKind.BattleResult,
                P0BattleFeedbackLevel.Result,
                "胜利",
                "路线推进",
                1.1f,
                0.85f);
            P0BattleFeedbackVisualState visual = P0BattleFeedbackVisualPresenter.Build(feedback, 2f);

            Require(
                report,
                visual.HasVisual
                && visual.AccentToken == "violet"
                && visual.VisualAsset.AssetId == P0VisualAssetCatalog.SleepStableWaveVfxId
                && visual.Progress01 >= 0.99f
                && visual.PulseFill01 <= 0.01f
                && visual.RemainingSeconds <= 0.01f
                && visual.BuildSummary().Contains("结果 战斗结果"),
                "Result feedback visual remains visible after its pulse finishes.",
                "Result feedback visual disappeared or kept pulse fill after expiration.");
        }

        private static void EvaluateInteractionVfxAssets(P0BattleFeedbackVisualCoverageReport report)
        {
            P0BattleFeedbackVisualState litter = P0BattleFeedbackVisualPresenter.Build(
                P0BattleFeedbackPresenter.BuildInteractionSuccess("猫砂盆", "已使用"),
                0f);
            P0BattleFeedbackVisualState feeder = P0BattleFeedbackVisualPresenter.Build(
                P0BattleFeedbackPresenter.BuildInteractionSuccess("喂食器", "已使用"),
                0f);
            P0BattleFeedbackVisualState bed = P0BattleFeedbackVisualPresenter.Build(
                P0BattleFeedbackPresenter.BuildInteractionSuccess("守床照看", "睡眠 +12"),
                0f);

            Require(
                report,
                litter.VisualAsset.AssetId == P0VisualAssetCatalog.LitterCleanseVfxId
                && feeder.VisualAsset.AssetId == P0VisualAssetCatalog.FeederKibbleVfxId
                && bed.VisualAsset.AssetId == P0VisualAssetCatalog.SleepStableWaveVfxId,
                "Interaction success feedback routes litter, feeder, and bed care to distinct VFX assets.",
                "Interaction success feedback did not route to the expected VFX assets.");
        }

        private static void EvaluateSkillTargetMarkVfxAsset(P0BattleFeedbackVisualCoverageReport report)
        {
            P0BattleFeedback feedback = new P0BattleFeedback(
                P0BattleFeedbackKind.SkillCast,
                P0BattleFeedbackLevel.Positive,
                "Moon Thread",
                "target Black Mud 2.0m, status 1",
                0.7f,
                0.65f);
            P0BattleFeedbackVisualState visual = P0BattleFeedbackVisualPresenter.Build(feedback, 0f);

            Require(
                report,
                visual.HasVisual
                && visual.VisualAsset.AssetId == P0VisualAssetCatalog.EnemyMarkRingVfxId
                && visual.VisualAsset.RequiresWorkspaceFile,
                "Targeted status skill feedback routes to the enemy mark ring VFX asset.",
                "Targeted status skill feedback is missing the enemy mark ring VFX asset.");
        }

        private static void EvaluateBossWarningVfxAsset(P0BattleFeedbackVisualCoverageReport report)
        {
            BattleEnemyState boss = new BattleEnemyState(
                1,
                FindEnemy(P0PrototypeCatalog.CallTyrantId),
                12f);
            boss.DebugSetBossTimers(EnemyWarningFormatter.BossSummonWarningThresholdSeconds, 0.5f);

            P0EnemyWarningIndicatorState warning = P0EnemyWarningIndicatorPresenter.Build(
                boss,
                new Vector2(0f, 3f),
                new Vector2(0f, -3.5f));

            Require(
                report,
                warning.Kind == P0EnemyWarningKind.BossThrow
                && warning.VisualAsset.AssetId == P0VisualAssetCatalog.CallTyrantAppThrowVfxId
                && warning.VisualAsset.RequiresWorkspaceFile
                && Contains(warning.VisualAsset.SourceLockIds, "call_tyrant_animation"),
                "Boss throw warning visual exposes the generated Call Tyrant app-throw VFX asset.",
                "Boss throw warning visual is missing the generated Call Tyrant app-throw VFX asset.");
        }

        private static EnemyDefinition FindEnemy(string enemyId)
        {
            foreach (EnemyDefinition enemy in P0PrototypeCatalog.CreateCoreEnemies())
            {
                if (enemy.Id == enemyId)
                {
                    return enemy;
                }
            }

            throw new InvalidOperationException("Missing enemy definition: " + enemyId);
        }

        private static bool Contains(IReadOnlyList<string> values, string expected)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == expected)
                {
                    return true;
                }
            }

            return false;
        }

        private static void Require(
            P0BattleFeedbackVisualCoverageReport report,
            bool condition,
            string coveredVisual,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredVisual(coveredVisual);
                return;
            }

            report.AddIssue(P0BattleFeedbackVisualCoverageSeverity.Failure, failureMessage);
        }
    }
}
