using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;

namespace TheCat.Tools
{
    public enum P0EnemyHudCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0EnemyHudCoverageIssue
    {
        public P0EnemyHudCoverageIssue(P0EnemyHudCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0EnemyHudCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0EnemyHudCoverageReport
    {
        private readonly List<P0EnemyHudCoverageIssue> issues = new List<P0EnemyHudCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0EnemyHudCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0EnemyHudCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0EnemyHudCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0EnemyHudCoverageSeverity severity, string message)
        {
            issues.Add(new P0EnemyHudCoverageIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 enemy HUD coverage complete for " + coveredChecks.Count + " check(s)."
                : "P0 enemy HUD coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0EnemyHudCoverageSeverity severity)
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

    public static class P0EnemyHudCoverage
    {
        public const int ExpectedCoveredCheckCount = 5;

        public static P0EnemyHudCoverageReport EvaluatePrototypeCards()
        {
            P0EnemyHudCoverageReport report = new P0EnemyHudCoverageReport();
            IReadOnlyList<P0EnemyHudCard> cards = P0EnemyHudPresenter.BuildPrototypeCards();

            EvaluateCoreThreatSet(cards, report);
            EvaluateBlackMudPressure(cards, report);
            EvaluateColdLightPressure(cards, report);
            EvaluateCallTyrantPattern(cards, report);
            EvaluateCompactSummary(cards, report);

            return report;
        }

        private static void EvaluateCoreThreatSet(
            IReadOnlyList<P0EnemyHudCard> cards,
            P0EnemyHudCoverageReport report)
        {
            Require(
                report,
                P0EnemyHudPresenter.HasP0EnemyHudCards(cards),
                "Enemy HUD cards expose Black Mud, Cold Light, and Call Tyrant P0 threats.",
                "Enemy HUD did not expose all required P0 core threats.");
        }

        private static void EvaluateBlackMudPressure(
            IReadOnlyList<P0EnemyHudCard> cards,
            P0EnemyHudCoverageReport report)
        {
            bool found = TryFind(cards, P0PrototypeCatalog.BlackMudNightmareId, out P0EnemyHudCard card);
            Require(
                report,
                found
                && card.ThreatToken == "压床"
                && card.TargetToken == "床"
                && card.PriorityToken == "危急"
                && card.HasWarning
                && card.WarningText.Contains("压床")
                && card.CounterHint.Contains("拦截"),
                "Enemy HUD card shows Black Mud as critical bed pressure with a counter hint.",
                "Enemy HUD did not expose Black Mud bed pressure.");
        }

        private static void EvaluateColdLightPressure(
            IReadOnlyList<P0EnemyHudCard> cards,
            P0EnemyHudCoverageReport report)
        {
            bool found = TryFind(cards, P0PrototypeCatalog.ColdLightShadowId, out P0EnemyHudCard card);
            Require(
                report,
                found
                && card.ThreatToken == "远程压制"
                && card.TargetToken == "猫"
                && card.IsPressureSource
                && card.PressureRange > 4f
                && card.CatDamage > card.BedDamage
                && card.WarningText.Contains("远程压制"),
                "Enemy HUD card shows Cold Light as ranged cat pressure with pressure-source marking.",
                "Enemy HUD did not expose Cold Light ranged pressure.");
        }

        private static void EvaluateCallTyrantPattern(
            IReadOnlyList<P0EnemyHudCard> cards,
            P0EnemyHudCoverageReport report)
        {
            bool found = TryFind(cards, P0PrototypeCatalog.CallTyrantId, out P0EnemyHudCard card);
            Require(
                report,
                found
                && card.IsBoss
                && card.ThreatToken == "首领机制"
                && card.TargetToken == "床+猫"
                && card.PriorityToken == "危急"
                && card.WarningText.Contains("首领召唤")
                && card.WarningText.Contains("首领投掷")
                && card.BossSummonRemainingSeconds <= 2f
                && card.BossThrowRemainingSeconds <= 2f,
                "Enemy HUD card shows Call Tyrant chief summon and app-throw timers.",
                "Enemy HUD did not expose Call Tyrant chief pattern.");
        }

        private static void EvaluateCompactSummary(
            IReadOnlyList<P0EnemyHudCard> cards,
            P0EnemyHudCoverageReport report)
        {
            string summary = P0EnemyHudPresenter.BuildCompactSummary(cards);
            Require(
                report,
                summary.Contains("3")
                && summary.Contains("首领 1")
                && summary.Contains("预警 3")
                && summary.Contains("压力源 1")
                && summary.Contains("压床 1")
                && summary.Contains("远程 1"),
                "Enemy HUD compact summary reports boss, warnings, pressure source, bed, and ranged counts.",
                "Enemy HUD compact summary did not report required totals.");
        }

        private static bool TryFind(
            IReadOnlyList<P0EnemyHudCard> cards,
            string enemyId,
            out P0EnemyHudCard card)
        {
            if (cards != null)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (cards[i].EnemyId == enemyId)
                    {
                        card = cards[i];
                        return true;
                    }
                }
            }

            card = default(P0EnemyHudCard);
            return false;
        }

        private static void Require(
            P0EnemyHudCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0EnemyHudCoverageSeverity.Failure, failureMessage);
        }
    }
}
