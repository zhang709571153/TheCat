using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Gameplay;

namespace TheCat.Tools
{
    public enum P0StatusHudCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StatusHudCoverageIssue
    {
        public P0StatusHudCoverageIssue(P0StatusHudCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StatusHudCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StatusHudCoverageReport
    {
        private readonly List<P0StatusHudCoverageIssue> issues = new List<P0StatusHudCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StatusHudCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0StatusHudCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0StatusHudCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0StatusHudCoverageSeverity severity, string message)
        {
            issues.Add(new P0StatusHudCoverageIssue(severity, message));
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
                ? "P0 status HUD coverage complete for " + coveredChecks.Count + " check(s)."
                : "P0 status HUD coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " check(s).";
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

        private int Count(P0StatusHudCoverageSeverity severity)
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

    public static class P0StatusHudCoverage
    {
        public const int ExpectedCoveredCheckCount = 8;

        public static P0StatusHudCoverageReport EvaluatePrototypeHud()
        {
            P0StatusHudCoverageReport report = new P0StatusHudCoverageReport();
            IReadOnlyList<P0StatusHudEntry> entries = P0StatusHudPresenter.BuildPrototypeEntries();

            EvaluateP0TagSet(entries, report);
            EvaluateSleepStableBedEntry(entries, report);
            EvaluateEnemySlowAndKnockbackEntry(entries, report);
            EvaluateEnemyMarkEntry(entries, report);
            EvaluateShieldScopes(entries, report);
            EvaluateStatusIcons(entries, report);
            EvaluateCompactStatusIcons(entries, report);
            EvaluateCompactSummary(entries, report);

            return report;
        }

        private static void EvaluateP0TagSet(
            IReadOnlyList<P0StatusHudEntry> entries,
            P0StatusHudCoverageReport report)
        {
            Require(
                report,
                P0StatusHudPresenter.HasP0StatusHudEntries(entries),
                "Status HUD exposes bed, enemy, cat scopes and all five P0 status tags.",
                "Status HUD did not expose all required P0 tag scopes.");
        }

        private static void EvaluateSleepStableBedEntry(
            IReadOnlyList<P0StatusHudEntry> entries,
            P0StatusHudCoverageReport report)
        {
            bool found = TryFind(entries, P0StatusHudTargetKind.Bed, StatusTagIds.SleepStable, out P0StatusHudEntry entry);
            Require(
                report,
                found
                && entry.Indicator.Text.Contains("安眠")
                && entry.ResponseSummary.Contains("主人睡眠稳定"),
                "Status HUD shows bed Sleep Stable with Chinese label and owner sleep response.",
                "Status HUD did not show bed sleep-stable response.");
        }

        private static void EvaluateEnemySlowAndKnockbackEntry(
            IReadOnlyList<P0StatusHudEntry> entries,
            P0StatusHudCoverageReport report)
        {
            bool found = TryFind(entries, P0StatusHudTargetKind.Enemy, StatusTagIds.Slow, out P0StatusHudEntry entry);
            Require(
                report,
                found
                && entry.HasTag(StatusTagIds.Knockback)
                && entry.MovementRateMultiplier < 1f
                && entry.TimeToBedSeconds > 3f
                && entry.ResponseSummary.Contains("移动 ")
                && entry.ResponseSummary.Contains(" 倍")
                && entry.ResponseSummary.Contains("击退生效"),
                "Status HUD shows enemy Slow and Knockback response with movement and time-to-bed feedback.",
                "Status HUD did not show enemy slow/knockback response.");
        }

        private static void EvaluateEnemyMarkEntry(
            IReadOnlyList<P0StatusHudEntry> entries,
            P0StatusHudCoverageReport report)
        {
            bool found = TryFind(entries, P0StatusHudTargetKind.Enemy, StatusTagIds.Mark, out P0StatusHudEntry entry);
            Require(
                report,
                found
                && entry.DamageTakenMultiplier > 1f
                && entry.Indicator.Text.Contains("标记")
                && entry.ResponseSummary.Contains("承伤 ")
                && entry.ResponseSummary.Contains(" 倍"),
                "Status HUD shows enemy Mark as a visible focus and damage-taken response.",
                "Status HUD did not show mark focus response.");
        }

        private static void EvaluateShieldScopes(
            IReadOnlyList<P0StatusHudEntry> entries,
            P0StatusHudCoverageReport report)
        {
            bool bedShield = TryFind(entries, P0StatusHudTargetKind.Bed, StatusTagIds.Shield, out P0StatusHudEntry bed);
            bool catShield = TryFind(entries, P0StatusHudTargetKind.Cat, StatusTagIds.Shield, out P0StatusHudEntry cat);
            Require(
                report,
                bedShield
                && catShield
                && bed.ShieldAmount > 0f
                && cat.ShieldAmount > 0f
                && bed.ResponseSummary.Contains("床护盾")
                && cat.ResponseSummary.Contains("猫护盾"),
                "Status HUD distinguishes bed shield from cat shield with remaining protection amounts.",
                "Status HUD did not distinguish bed and cat shield scopes.");
        }

        private static void EvaluateStatusIcons(
            IReadOnlyList<P0StatusHudEntry> entries,
            P0StatusHudCoverageReport report)
        {
            Require(
                report,
                HasIcon(entries, StatusTagIds.SleepStable)
                && HasIcon(entries, StatusTagIds.Slow)
                && HasIcon(entries, StatusTagIds.Knockback)
                && HasIcon(entries, StatusTagIds.Mark)
                && HasIcon(entries, StatusTagIds.Shield),
                "Status HUD maps all five P0 status tags to generated icon assets.",
                "Status HUD is missing one or more P0 status icon asset mappings.");
        }

        private static void EvaluateCompactStatusIcons(
            IReadOnlyList<P0StatusHudEntry> entries,
            P0StatusHudCoverageReport report)
        {
            Require(
                report,
                HasCompactIcon(entries, StatusTagIds.SleepStable)
                && HasCompactIcon(entries, StatusTagIds.Slow)
                && HasCompactIcon(entries, StatusTagIds.Knockback)
                && HasCompactIcon(entries, StatusTagIds.Mark)
                && HasCompactIcon(entries, StatusTagIds.Shield),
                "Status HUD maps all five P0 status tags to compact 32px icon assets.",
                "Status HUD is missing one or more P0 compact status icon asset mappings.");
        }

        private static void EvaluateCompactSummary(
            IReadOnlyList<P0StatusHudEntry> entries,
            P0StatusHudCoverageReport report)
        {
            string summary = P0StatusHudPresenter.BuildCompactSummary(entries);
            Require(
                report,
                summary.Contains("床 1")
                && summary.Contains("敌人 1")
                && summary.Contains("猫 1")
                && summary.Contains("标签 5")
                && summary.Contains("图标 3")
                && summary.Contains("响应 3"),
                "Status HUD compact summary reports scopes, unique tags, shield, and response counts.",
                "Status HUD compact summary did not report required totals.");
        }

        private static bool TryFind(
            IReadOnlyList<P0StatusHudEntry> entries,
            P0StatusHudTargetKind kind,
            string statusTagId,
            out P0StatusHudEntry entry)
        {
            if (entries != null)
            {
                for (int i = 0; i < entries.Count; i++)
                {
                    if (entries[i].TargetKind == kind && entries[i].HasTag(statusTagId))
                    {
                        entry = entries[i];
                        return true;
                    }
                }
            }

            entry = default(P0StatusHudEntry);
            return false;
        }

        private static bool HasIcon(IReadOnlyList<P0StatusHudEntry> entries, string statusTagId)
        {
            if (entries == null)
            {
                return false;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].HasIconFor(statusTagId))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasCompactIcon(IReadOnlyList<P0StatusHudEntry> entries, string statusTagId)
        {
            if (entries == null)
            {
                return false;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].HasCompactIconFor(statusTagId))
                {
                    return true;
                }
            }

            return false;
        }

        private static void Require(
            P0StatusHudCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StatusHudCoverageSeverity.Failure, failureMessage);
        }
    }
}
