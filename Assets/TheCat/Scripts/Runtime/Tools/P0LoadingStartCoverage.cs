using System;
using System.Collections.Generic;
using TheCat.Gameplay;

namespace TheCat.Tools
{
    public enum P0LoadingStartCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0LoadingStartCoverageIssue
    {
        public P0LoadingStartCoverageIssue(P0LoadingStartCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0LoadingStartCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0LoadingStartCoverageReport
    {
        private readonly List<P0LoadingStartCoverageIssue> issues = new List<P0LoadingStartCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0LoadingStartCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0LoadingStartCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0LoadingStartCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0LoadingStartCoverageSeverity severity, string message)
        {
            issues.Add(new P0LoadingStartCoverageIssue(severity, message));
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
                ? "P0 loading/start coverage complete for " + coveredChecks.Count + " check(s)."
                : "P0 loading/start coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " covered check(s).";
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

        private int Count(P0LoadingStartCoverageSeverity severity)
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

    public static class P0LoadingStartCoverage
    {
        public const int ExpectedCoveredCheckCount = 4;

        public static P0LoadingStartCoverageReport EvaluatePrototypeLoadingStart()
        {
            P0LoadingStartCoverageReport report = new P0LoadingStartCoverageReport();
            EvaluateCatRoomStart(report);
            EvaluateRouteAndBattleTargets(report);
            EvaluateProgressClamping(report);
            EvaluateBatch83Boundary(report);
            return report;
        }

        private static void EvaluateCatRoomStart(P0LoadingStartCoverageReport report)
        {
            P0LoadingStartSurface surface = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.CatRoom, 0.35f);
            Require(
                report,
                P0LoadingStartPresenter.HasP0LoadingStartSurface(surface)
                && surface.TargetSceneName == P0SceneFlow.CatRoomSceneName
                && surface.BuildStatusLine().Contains("35%")
                && surface.HasScreenshotHook,
                "Loading/start surface exposes cat-room target, progress, spinner, and screenshot hook.",
                "Loading/start cat-room surface is missing target, progress, spinner, or screenshot hook.");
        }

        private static void EvaluateRouteAndBattleTargets(P0LoadingStartCoverageReport report)
        {
            P0LoadingStartSurface route = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.RouteMap, 0.5f);
            P0LoadingStartSurface battle = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.QuickBattle, 0.75f);
            Require(
                report,
                route.TargetSceneName == P0SceneFlow.RouteMapSceneName
                && battle.TargetSceneName == P0SceneFlow.GrayboxBattleSceneName
                && P0LoadingStartPresenter.HasP0LoadingStartSurface(route)
                && P0LoadingStartPresenter.HasP0LoadingStartSurface(battle),
                "Loading/start target routing covers route map and quick-battle graybox helper starts.",
                "Loading/start target routing is missing route map or quick battle targets.");
        }

        private static void EvaluateProgressClamping(P0LoadingStartCoverageReport report)
        {
            P0LoadingStartSurface low = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.CatRoom, -1f);
            P0LoadingStartSurface high = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.CatRoom, 2f);
            Require(
                report,
                low.Progress01 == 0f
                && high.Progress01 == 1f
                && low.ProgressLabel == "0%"
                && high.ProgressLabel == "100%",
                "Loading/start progress clamps to 0-100 percent for screenshot states.",
                "Loading/start progress labels do not clamp to 0-100 percent.");
        }

        private static void EvaluateBatch83Boundary(P0LoadingStartCoverageReport report)
        {
            P0LoadingStartSurface surface = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.CatRoom, 1f);
            P0LoadingStartSurface badDetail = new P0LoadingStartSurface(
                surface.TitleLabel,
                surface.TargetSceneName,
                surface.TargetLabel,
                surface.StateLabel,
                surface.Progress01,
                surface.ProgressLabel,
                surface.SpinnerLabel,
                new[]
                {
                    "Assets/TheCat/Art/UI/batch_83_loading_start_preflight_2026-06-25/mock.png",
                    "candidate_v001.png",
                    ".meta"
                },
                surface.HasScreenshotHook);
            string summary = surface.BuildSummary();
            Require(
                report,
                !summary.Contains(".png")
                && !summary.Contains(".meta")
                && !summary.Contains("Sprite")
                && !summary.Contains("batch_83")
                && P0LoadingStartPresenter.HasP0LoadingStartSurface(surface)
                && !P0LoadingStartPresenter.HasP0LoadingStartSurface(badDetail),
                "Batch 83 boundary stays candidate-only: loading/start hook deep-scans public surface text and has no runtime asset binding requirement.",
                "Batch 83 candidate assets leaked into loading/start runtime surface.");
        }

        private static void Require(
            P0LoadingStartCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0LoadingStartCoverageSeverity.Failure, failureMessage);
        }
    }
}
