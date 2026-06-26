using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;
using TheCat.Inputs;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public enum P0BattleResultCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0BattleResultCoverageIssue
    {
        public P0BattleResultCoverageIssue(P0BattleResultCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0BattleResultCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0BattleResultCoverageReport
    {
        private readonly List<P0BattleResultCoverageIssue> issues = new List<P0BattleResultCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0BattleResultCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0BattleResultCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0BattleResultCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0BattleResultCoverageSeverity severity, string message)
        {
            issues.Add(new P0BattleResultCoverageIssue(severity, message));
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
                ? "P0 battle result coverage complete for " + coveredChecks.Count + " result check(s)."
                : "P0 battle result coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " result check(s).";
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

        private int Count(P0BattleResultCoverageSeverity severity)
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

    public static class P0BattleResultCoverage
    {
        public const int ExpectedCoveredCheckCount = 7;

        public static P0BattleResultCoverageReport EvaluatePrototypeSurface()
        {
            P0BattleResultCoverageReport report = new P0BattleResultCoverageReport();

            EvaluateVictorySurface(report);
            EvaluateDefeatSurface(report);
            EvaluateOutcomeBannerSurface(report);
            EvaluateInProgressSurface(report);
            EvaluateCompactSummary(report);
            EvaluatePlayerFocusRows(report);
            EvaluateActionShortcutLabels(report);

            return report;
        }

        private static void EvaluateVictorySurface(P0BattleResultCoverageReport report)
        {
            BattleSimulation battle = CreateLayerOneBattle();
            ResolveVictory(battle);
            RunProgressionState run = CreateRun();
            RunNodeCompletionReport completion = CompleteCurrentNode(run, NodeResult.Success);
            P0BattleResultSurface surface = P0BattleResultPresenter.Build(battle, run, completion);

            Require(
                report,
                P0BattleResultPresenter.HasP0BattleResultSurface(surface)
                && surface.Outcome == BattleOutcome.Victory
                && surface.Title == "胜利"
                && surface.ResultLabel == "胜利"
                && Contains(surface.RouteRows, "奖励：")
                && Contains(surface.RouteRows, "下一节点：")
                && Contains(surface.RouteRows, "路线进度：1/10")
                && Contains(surface.RouteRows, "总等级")
                && Contains(surface.MetricRows, "用时：")
                && Contains(surface.CoreRows, "主人睡眠度")
                && surface.TryGetAction(P0BattleResultActionIds.ContinueRoute, out P0BattleResultAction action)
                && action.Label.Contains("继续路线")
                && action.Detail.Contains("返回路线图")
                && action.TargetSceneName == P0SceneFlow.RouteMapSceneName
                && HasNoPlayerFacingDeveloperTokens(surface),
                "Victory result surface exposes result, reward, next-node route, core values, metrics, and continue action.",
                "Victory result surface is incomplete.");
        }

        private static void EvaluateDefeatSurface(P0BattleResultCoverageReport report)
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.DebugDamageOwnerSleep(999f);
            RunProgressionState run = CreateRun();
            RunNodeCompletionReport completion = CompleteCurrentNode(run, NodeResult.Failure);
            P0BattleResultSurface surface = P0BattleResultPresenter.Build(battle, run, completion);

            Require(
                report,
                P0BattleResultPresenter.HasP0BattleResultSurface(surface)
                && surface.Outcome == BattleOutcome.Defeat
                && surface.Title == "失败"
                && surface.ResultLabel == "失败"
                && Contains(surface.RouteRows, "路线状态：失败")
                && !Contains(surface.RouteRows, "奖励：")
                && Contains(surface.RouteRows, "总等级")
                && surface.PromptText.Contains("失败结算")
                && surface.TryGetAction(P0BattleResultActionIds.ContinueRoute, out P0BattleResultAction continueRoute)
                && continueRoute.IsEnabled
                && continueRoute.TargetSceneName == P0SceneFlow.RouteMapSceneName
                && HasNoPlayerFacingDeveloperTokens(surface),
                "Defeat result surface keeps route-settlement continuation available with failed-route context.",
                "Defeat result surface does not expose failed-route continuation correctly.");
        }

        private static void EvaluateInProgressSurface(P0BattleResultCoverageReport report)
        {
            P0BattleResultSurface surface = P0BattleResultPresenter.Build(CreateLayerOneBattle(), CreateRun(), null);

            Require(
                report,
                !P0BattleResultPresenter.HasP0BattleResultSurface(surface)
                && !surface.IsResolved
                && surface.TryGetAction(P0BattleResultActionIds.ContinueRoute, out P0BattleResultAction continueRoute)
                && !continueRoute.IsEnabled
                && continueRoute.BuildButtonLabel().Contains("战斗结束后可用")
                && surface.TryGetAction(P0BattleResultActionIds.RestartRun, out P0BattleResultAction restart)
                && restart.IsEnabled
                && HasNoPlayerFacingDeveloperTokens(surface),
                "In-progress result surface locks route continuation while keeping restart available.",
                "In-progress result surface allowed route continuation or hid restart.");
        }

        private static void EvaluateOutcomeBannerSurface(P0BattleResultCoverageReport report)
        {
            BattleSimulation victoryBattle = CreateLayerOneBattle();
            ResolveVictory(victoryBattle);
            RunProgressionState victoryRun = CreateRun();
            P0BattleResultSurface victorySurface = P0BattleResultPresenter.Build(
                victoryBattle,
                victoryRun,
                CompleteCurrentNode(victoryRun, NodeResult.Success));

            BattleSimulation defeatBattle = CreateLayerOneBattle();
            defeatBattle.DebugDamageOwnerSleep(999f);
            RunProgressionState defeatRun = CreateRun();
            P0BattleResultSurface defeatSurface = P0BattleResultPresenter.Build(
                defeatBattle,
                defeatRun,
                CompleteCurrentNode(defeatRun, NodeResult.Failure));

            P0BattleResultSurface inProgressSurface = P0BattleResultPresenter.Build(
                CreateLayerOneBattle(),
                CreateRun(),
                null);

            Require(
                report,
                victorySurface.OutcomeBannerAsset.AssetId == P0VisualAssetCatalog.BattleResultVictoryBannerId
                && defeatSurface.OutcomeBannerAsset.AssetId == P0VisualAssetCatalog.BattleResultDefeatBannerId
                && !inProgressSurface.OutcomeBannerAsset.HasAsset,
                "Battle result surface exposes specific victory and defeat outcome banner assets while in-progress stays unbannered.",
                "Battle result surface is missing one or more outcome banner assets.");
        }

        private static void EvaluateCompactSummary(P0BattleResultCoverageReport report)
        {
            BattleSimulation battle = CreateLayerOneBattle();
            ResolveVictory(battle);
            RunProgressionState run = CreateRun();
            P0BattleResultSurface surface = P0BattleResultPresenter.Build(
                battle,
                run,
                CompleteCurrentNode(run, NodeResult.Success));
            string summary = P0BattleResultPresenter.BuildCompactSummary(surface);

            Require(
                report,
                summary.Contains("胜利")
                && summary.Contains("指标")
                && summary.Contains("核心")
                && summary.Contains("路线")
                && summary.Contains("操作 3"),
                "Battle result compact summary reports outcome, metrics, core rows, route rows, and action count.",
                "Battle result compact summary is missing required totals.");
        }

        private static void EvaluatePlayerFocusRows(P0BattleResultCoverageReport report)
        {
            BattleSimulation victoryBattle = CreateLayerOneBattle();
            ResolveVictory(victoryBattle);
            RunProgressionState victoryRun = CreateRun();
            P0BattleResultSurface victorySurface = P0BattleResultPresenter.Build(
                victoryBattle,
                victoryRun,
                CompleteCurrentNode(victoryRun, NodeResult.Success));
            IReadOnlyList<string> victoryRows = P0BattleResultPresenter.BuildPlayerFocusRows(victorySurface);

            BattleSimulation defeatBattle = CreateLayerOneBattle();
            defeatBattle.DebugDamageOwnerSleep(999f);
            RunProgressionState defeatRun = CreateRun();
            P0BattleResultSurface defeatSurface = P0BattleResultPresenter.Build(
                defeatBattle,
                defeatRun,
                CompleteCurrentNode(defeatRun, NodeResult.Failure));
            IReadOnlyList<string> defeatRows = P0BattleResultPresenter.BuildPlayerFocusRows(defeatSurface);

            Require(
                report,
                victoryRows.Count >= 5
                && Contains(victoryRows, "路线结果：")
                && Contains(victoryRows, "奖励：")
                && Contains(victoryRows, "下一节点：")
                && Contains(victoryRows, "路线进度：1/10")
                && Contains(victoryRows, "主人睡眠度")
                && defeatRows.Count >= 4
                && Contains(defeatRows, "路线结果：")
                && Contains(defeatRows, "路线状态：失败")
                && Contains(defeatRows, "路线进度：1/10")
                && Contains(defeatRows, "主人睡眠度")
                && !Contains(defeatRows, "奖励：")
                && HasNoPlayerFacingDeveloperTokens(victoryRows)
                && HasNoPlayerFacingDeveloperTokens(defeatRows),
                "Player-facing result focus rows expose reward, next node, failed-route state, route progress, and core sleep without developer tokens.",
                "Player-facing result focus rows are missing a primary outcome detail.");
        }

        private static void EvaluateActionShortcutLabels(P0BattleResultCoverageReport report)
        {
            BattleSimulation battle = CreateLayerOneBattle();
            ResolveVictory(battle);
            RunProgressionState run = CreateRun();
            P0BattleResultSurface surface = P0BattleResultPresenter.Build(
                battle,
                run,
                CompleteCurrentNode(run, NodeResult.Success));

            bool hasContinueBinding = P0KeyboardInputMap.TryGetBinding(P0InputCommand.ContinueRoute, out P0InputBinding continueBinding);
            bool hasRestartBinding = P0KeyboardInputMap.TryGetBinding(P0InputCommand.RestartRun, out P0InputBinding restartBinding);

            Require(
                report,
                hasContinueBinding
                && hasRestartBinding
                && surface.TryGetAction(P0BattleResultActionIds.ContinueRoute, out P0BattleResultAction continueRoute)
                && continueRoute.ShortcutLabel == continueBinding.PrimaryKeyLabel
                && continueRoute.BuildButtonLabel().Contains("[" + continueBinding.PrimaryKeyLabel + "]")
                && surface.TryGetAction(P0BattleResultActionIds.ReturnCatRoom, out P0BattleResultAction returnCatRoom)
                && string.IsNullOrWhiteSpace(returnCatRoom.ShortcutLabel)
                && !returnCatRoom.BuildButtonLabel().Contains("[C]")
                && surface.TryGetAction(P0BattleResultActionIds.RestartRun, out P0BattleResultAction restart)
                && restart.ShortcutLabel == restartBinding.PrimaryKeyLabel
                && restart.BuildButtonLabel().Contains("[" + restartBinding.PrimaryKeyLabel + "]")
                && !restart.BuildButtonLabel().Contains("[R]")
                && restart.Command == P0InputCommand.RestartRun
                && restart.TargetSceneName == P0SceneFlow.GrayboxBattleSceneName,
                "Result action shortcut labels match the keyboard input map and avoid fake cat-room shortcuts.",
                "Result action shortcut labels drifted from the keyboard input map.");
        }

        private static BattleSimulation CreateLayerOneBattle()
        {
            return new BattleSimulation(
                new BattleSimulationConfig(
                    P0PrototypeCatalog.CreateLayerOneWave(),
                    P0PrototypeCatalog.CreateCoreEnemies(),
                    P0Tuning.Default,
                    statusTags: P0PrototypeCatalog.CreateStatusTags()),
                new RunMetrics());
        }

        private static RunProgressionState CreateRun()
        {
            return new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "nephthys", "suzune" });
        }

        private static void ResolveVictory(BattleSimulation battle)
        {
            for (int i = 0; i < 32 && battle.Outcome == BattleOutcome.InProgress; i++)
            {
                battle.Tick(5f);
                while (battle.ApplyDamageToNearestEnemy(999f))
                {
                }
            }
        }

        private static RunNodeCompletionReport CompleteCurrentNode(RunProgressionState run, NodeResult result)
        {
            RouteNodeDefinition node = run.Route.CurrentNode;
            RouteBattleReward reward = RouteBattleReward.None;
            run.Route.CompleteCurrentNode(result);
            if (result == NodeResult.Success && node != null && RouteNodeResolver.RequiresBattle(node.NodeType))
            {
                reward = P0RouteRewardResolver.ApplyBattleReward(node, run);
            }

            return new RunNodeCompletionReport(
                node,
                result,
                reward,
                run.Route.CurrentNode,
                run.Route.IsCleared,
                run.Route.IsFailed);
        }

        private static bool Contains(IReadOnlyList<string> rows, string expected)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].Contains(expected))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasNoPlayerFacingDeveloperTokens(P0BattleResultSurface surface)
        {
            if (surface == null)
            {
                return false;
            }

            List<string> text = new List<string>
            {
                surface.Title,
                surface.ResultLabel,
                surface.PromptText
            };

            AddRows(text, surface.MetricRows);
            AddRows(text, surface.CoreRows);
            AddRows(text, surface.RouteRows);

            for (int i = 0; i < surface.Actions.Count; i++)
            {
                P0BattleResultAction action = surface.Actions[i];
                text.Add(action.Label);
                text.Add(action.Detail);
                text.Add(action.DisabledReasonLabel);
                text.Add(action.BuildButtonLabel());
            }

            return HasNoPlayerFacingDeveloperTokens(text);
        }

        private static bool HasNoPlayerFacingDeveloperTokens(IReadOnlyList<string> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                string row = rows[i];
                if (HasPlayerFacingDeveloperToken(row))
                {
                    return false;
                }
            }

            return true;
        }

        private static void AddRows(List<string> target, IReadOnlyList<string> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                target.Add(rows[i]);
            }
        }

        private static bool HasPlayerFacingDeveloperToken(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text.Contains("HP")
                || text.Contains(" Lv")
                || text.Contains("未解锁")
                || text.IndexOf("Continue Route", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("Return to route", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("locked", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(
            P0BattleResultCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0BattleResultCoverageSeverity.Failure, failureMessage);
        }
    }
}
