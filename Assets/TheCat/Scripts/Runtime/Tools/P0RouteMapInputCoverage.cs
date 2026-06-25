using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Gameplay;
using TheCat.Inputs;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public enum P0RouteMapInputCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0RouteMapInputCoverageIssue
    {
        public P0RouteMapInputCoverageIssue(P0RouteMapInputCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0RouteMapInputCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0RouteMapInputCoverageReport
    {
        private readonly List<P0RouteMapInputCoverageIssue> issues = new List<P0RouteMapInputCoverageIssue>();
        private readonly List<string> coveredActions = new List<string>();

        public IReadOnlyList<P0RouteMapInputCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredActions => coveredActions.AsReadOnly();

        public int FailureCount => Count(P0RouteMapInputCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredActions.Count >= P0RouteMapInputCoverage.ExpectedCoveredActionCount;

        public void AddIssue(P0RouteMapInputCoverageSeverity severity, string message)
        {
            issues.Add(new P0RouteMapInputCoverageIssue(severity, message));
        }

        public void AddCoveredAction(string action)
        {
            if (!string.IsNullOrWhiteSpace(action))
            {
                coveredActions.Add(action);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 route map input coverage complete for " + coveredActions.Count + " action(s)."
                : "P0 route map input coverage has " + FailureCount + " failure(s) across " + coveredActions.Count + " covered action(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredActions.Count; i++)
            {
                lines.Add("- " + coveredActions[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0RouteMapInputCoverageSeverity severity)
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

    public static class P0RouteMapInputCoverage
    {
        public const int ExpectedCoveredActionCount = 6;

        public static P0RouteMapInputCoverageReport EvaluatePrototypeRouteMap()
        {
            P0RouteMapInputCoverageReport report = new P0RouteMapInputCoverageReport();

            EvaluateBattleEnter(report);
            EvaluateDefaultRewardConfirm(report);
            EvaluateRouteOptionSelection(report);
            EvaluateRewardSlotSelection(report);
            EvaluateRestartRunRequest(report);
            EvaluateUnsupportedBattleCommand(report);

            return report;
        }

        private static void EvaluateBattleEnter(P0RouteMapInputCoverageReport report)
        {
            RunProgressionState run = CreatePreparedRun();
            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);

            Require(
                report,
                result.ShouldLoadBattle
                && result.Action == P0RouteMapCommandAction.StartBattle
                && result.SelectedNodeId == P0RouteCatalog.LayerOneDefenseId
                && run.Route.CompletedCount == 0,
                "Enter confirms battle nodes without advancing the route.",
                "Enter did not request the current battle node correctly.");
        }

        private static void EvaluateDefaultRewardConfirm(P0RouteMapInputCoverageReport report)
        {
            RunProgressionState run = CreatePreparedRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);

            Require(
                report,
                result.Action == P0RouteMapCommandAction.ResolveRewardChoice
                && result.SelectedNodeId == "layer_02_dream_event"
                && result.SelectedChoiceId == "dream_event_clear_notifications"
                && run.Route.CurrentLayer == 3
                && run.DreamEventsResolved == 1,
                "Enter applies the default non-battle reward and advances.",
                "Enter did not apply the default non-battle reward correctly.");
        }

        private static void EvaluateRouteOptionSelection(P0RouteMapInputCoverageReport report)
        {
            RunProgressionState run = CreatePreparedRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.SelectCat2);

            Require(
                report,
                result.Action == P0RouteMapCommandAction.SelectRouteOption
                && result.SelectedNodeId == "layer_02_shop_early"
                && run.Route.CurrentNode.Id == "layer_02_shop_early"
                && run.Route.CompletedCount == 1,
                "Number keys select unresolved route branch options.",
                "Number key route option selection failed.");
        }

        private static void EvaluateRewardSlotSelection(P0RouteMapInputCoverageReport report)
        {
            RunProgressionState run = CreatePreparedRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);
            run.Route.SelectCurrentNode("layer_02_dream_event");
            run.CoreValues.Capture(40f, 100f, 100f, 0f, 100f);

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.SelectCat3);

            Require(
                report,
                result.Action == P0RouteMapCommandAction.ResolveRewardChoice
                && result.SelectedChoiceId == "dream_event_mark_all_read"
                && run.CoreValues.OwnerSleepCurrent >= 52f
                && run.Route.CurrentLayer == 3,
                "Number keys resolve reward choices after a node is selected.",
                "Number key reward choice resolution failed.");
        }

        private static void EvaluateRestartRunRequest(P0RouteMapInputCoverageReport report)
        {
            RunProgressionState run = CreatePreparedRun();
            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.RestartRun);

            Require(
                report,
                result.Action == P0RouteMapCommandAction.StartNewRun
                && run.Route.CurrentLayer == 1
                && run.Route.CompletedCount == 0,
                "N requests a new run without mutating the current run in the router.",
                "Restart command did not return a clean new-run request.");
        }

        private static void EvaluateUnsupportedBattleCommand(P0RouteMapInputCoverageReport report)
        {
            RunProgressionState run = CreatePreparedRun();
            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.Skill1);

            Require(
                report,
                !result.IsHandled
                && result.Action == P0RouteMapCommandAction.None
                && run.Route.CompletedCount == 0,
                "Battle-only skill commands are ignored on the route map.",
                "Route map consumed a battle-only skill command.");
        }

        private static RunProgressionState CreatePreparedRun()
        {
            return new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "nephthys", "suzune" });
        }

        private static void Require(
            P0RouteMapInputCoverageReport report,
            bool condition,
            string coveredAction,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredAction(coveredAction);
                return;
            }

            report.AddIssue(P0RouteMapInputCoverageSeverity.Failure, failureMessage);
        }
    }
}
