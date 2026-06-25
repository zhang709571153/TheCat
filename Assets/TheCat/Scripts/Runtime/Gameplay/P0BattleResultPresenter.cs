using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Inputs;
using TheCat.Roguelite;

namespace TheCat.Gameplay
{
    public static class P0BattleResultActionIds
    {
        public const string ContinueRoute = "continue_route";
        public const string ReturnCatRoom = "return_cat_room";
        public const string RestartRun = "restart_run";
    }

    public readonly struct P0BattleResultAction
    {
        public P0BattleResultAction(
            string actionId,
            P0InputCommand command,
            string label,
            string detail,
            string shortcutLabel,
            string targetSceneName,
            bool isEnabled,
            string disabledReasonLabel = "")
        {
            ActionId = actionId ?? string.Empty;
            Command = command;
            Label = label ?? string.Empty;
            Detail = detail ?? string.Empty;
            ShortcutLabel = shortcutLabel ?? string.Empty;
            TargetSceneName = targetSceneName ?? string.Empty;
            IsEnabled = isEnabled;
            DisabledReasonLabel = disabledReasonLabel ?? string.Empty;
        }

        public string ActionId { get; }

        public P0InputCommand Command { get; }

        public string Label { get; }

        public string Detail { get; }

        public string ShortcutLabel { get; }

        public string TargetSceneName { get; }

        public bool IsEnabled { get; }

        public string DisabledReasonLabel { get; }

        public string BuildButtonLabel()
        {
            string label = Label;
            if (!string.IsNullOrWhiteSpace(ShortcutLabel))
            {
                label += " [" + ShortcutLabel + "]";
            }

            if (IsEnabled)
            {
                return label;
            }

            string reason = string.IsNullOrWhiteSpace(DisabledReasonLabel)
                ? "暂不可用"
                : DisabledReasonLabel;
            return label + "（" + reason + "）";
        }
    }

    public sealed class P0BattleResultSurface
    {
        public P0BattleResultSurface(
            BattleOutcome outcome,
            string title,
            string resultLabel,
            string promptText,
            P0VisualAssetReference outcomeBannerAsset,
            IEnumerable<string> metricRows,
            IEnumerable<string> coreRows,
            IEnumerable<string> routeRows,
            IEnumerable<P0BattleResultAction> actions)
        {
            Outcome = outcome;
            Title = title ?? string.Empty;
            ResultLabel = resultLabel ?? string.Empty;
            PromptText = promptText ?? string.Empty;
            OutcomeBannerAsset = outcomeBannerAsset;
            MetricRows = metricRows == null
                ? Array.Empty<string>()
                : new List<string>(metricRows).AsReadOnly();
            CoreRows = coreRows == null
                ? Array.Empty<string>()
                : new List<string>(coreRows).AsReadOnly();
            RouteRows = routeRows == null
                ? Array.Empty<string>()
                : new List<string>(routeRows).AsReadOnly();
            Actions = actions == null
                ? Array.Empty<P0BattleResultAction>()
                : new List<P0BattleResultAction>(actions).AsReadOnly();
        }

        public BattleOutcome Outcome { get; }

        public string Title { get; }

        public string ResultLabel { get; }

        public string PromptText { get; }

        public P0VisualAssetReference OutcomeBannerAsset { get; }

        public IReadOnlyList<string> MetricRows { get; }

        public IReadOnlyList<string> CoreRows { get; }

        public IReadOnlyList<string> RouteRows { get; }

        public IReadOnlyList<P0BattleResultAction> Actions { get; }

        public bool IsResolved => Outcome != BattleOutcome.InProgress;

        public bool TryGetAction(string actionId, out P0BattleResultAction action)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionId == actionId)
                {
                    action = Actions[i];
                    return true;
                }
            }

            action = default(P0BattleResultAction);
            return false;
        }
    }

    public static class P0BattleResultPresenter
    {
        public static P0BattleResultSurface Build(
            BattleSimulation battle,
            RunProgressionState run,
            RunNodeCompletionReport completionReport)
        {
            BattleOutcome outcome = battle == null ? BattleOutcome.InProgress : battle.Outcome;
            return new P0BattleResultSurface(
                outcome,
                BuildTitle(outcome),
                BuildResultLabel(outcome),
                BuildPromptText(outcome),
                P0VisualAssetCatalog.GetBattleResultOutcomeBanner(outcome),
                BuildMetricRows(battle),
                BuildCoreRows(battle),
                BuildRouteRows(run, completionReport),
                BuildActions(outcome));
        }

        public static bool HasP0BattleResultSurface(P0BattleResultSurface surface)
        {
            if (surface == null || !surface.IsResolved)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(surface.Title)
                && !string.IsNullOrWhiteSpace(surface.ResultLabel)
                && surface.OutcomeBannerAsset.HasAsset
                && surface.MetricRows.Count >= 5
                && surface.CoreRows.Count >= 3
                && surface.RouteRows.Count >= 3
                && surface.TryGetAction(P0BattleResultActionIds.ContinueRoute, out P0BattleResultAction continueRoute)
                && continueRoute.IsEnabled
                && continueRoute.TargetSceneName == P0SceneFlow.RouteMapSceneName
                && surface.TryGetAction(P0BattleResultActionIds.ReturnCatRoom, out P0BattleResultAction returnCatRoom)
                && returnCatRoom.IsEnabled
                && returnCatRoom.TargetSceneName == P0SceneFlow.CatRoomSceneName
                && surface.TryGetAction(P0BattleResultActionIds.RestartRun, out P0BattleResultAction restart)
                && restart.IsEnabled
                && restart.TargetSceneName == P0SceneFlow.GrayboxBattleSceneName;
        }

        public static string BuildCompactSummary(P0BattleResultSurface surface)
        {
            if (surface == null)
            {
                return "战斗结果：空";
            }

            return "战斗结果："
                + surface.ResultLabel
                + " 指标 "
                + surface.MetricRows.Count
                + " 核心 "
                + surface.CoreRows.Count
                + " 路线 "
                + surface.RouteRows.Count
                + " 操作 "
                + surface.Actions.Count;
        }

        private static string BuildTitle(BattleOutcome outcome)
        {
            switch (outcome)
            {
                case BattleOutcome.Victory:
                    return "胜利";
                case BattleOutcome.Defeat:
                    return "失败";
                default:
                    return "战斗进行中";
            }
        }

        private static string BuildResultLabel(BattleOutcome outcome)
        {
            switch (outcome)
            {
                case BattleOutcome.Victory:
                    return "胜利";
                case BattleOutcome.Defeat:
                    return "失败";
                default:
                    return "进行中";
            }
        }

        private static string BuildPromptText(BattleOutcome outcome)
        {
            switch (outcome)
            {
                case BattleOutcome.Victory:
                    return "继续路线，领取下一个节点。";
                case BattleOutcome.Defeat:
                    return "继续路线查看失败结算，或立即重开。";
                default:
                    return "先完成当前战斗再继续路线。";
            }
        }

        private static IReadOnlyList<string> BuildMetricRows(BattleSimulation battle)
        {
            if (battle == null)
            {
                return Array.Empty<string>();
            }

            return new[]
            {
                "用时：" + battle.NodeMetrics.DurationSeconds.ToString("0.0") + "s",
                "睡眠变化：" + battle.NodeMetrics.SleepDelta.ToString("0.0")
                    + " 结束 " + battle.OwnerSleep.Current.ToString("0.#") + "/" + battle.OwnerSleep.Max.ToString("0.#"),
                "照看：床 " + battle.NodeMetrics.BedCareUses
                    + " 猫砂盆 " + battle.NodeMetrics.LitterBoxUses
                    + " 喂食器 " + battle.NodeMetrics.FeederUses,
                "压力：拉屎 " + battle.NodeMetrics.PoopIncidents
                    + " 睡眠上限损失 " + battle.NodeMetrics.SleepMaxLost.ToString("0.#")
                    + " 虚弱 " + battle.NodeMetrics.WeakIncidents,
                "敌人压力：床 " + battle.NodeMetrics.BedPressureHits
                    + " 首领 " + battle.NodeMetrics.BossThrowPressureHits
                    + " 睡眠 " + battle.NodeMetrics.EnemySleepDamageTaken.ToString("0.#")
                    + "/" + battle.NodeMetrics.EnemySleepDamageIncoming.ToString("0.#"),
                "行动：切换 " + battle.NodeMetrics.CatSwitchesSucceeded
                    + "/" + battle.NodeMetrics.CatSwitchAttempts
                    + " 技能 " + battle.NodeMetrics.SkillCastsSucceeded
                    + "/" + battle.NodeMetrics.SkillCastAttempts
                    + " 交互 " + battle.NodeMetrics.InteractionSuccesses
                    + "/" + battle.NodeMetrics.InteractionAttempts
            };
        }

        private static IReadOnlyList<string> BuildCoreRows(BattleSimulation battle)
        {
            if (battle == null)
            {
                return Array.Empty<string>();
            }

            return new[]
            {
                P0CoreValuePresenter.DescribeOwnerSleep(battle.OwnerSleep).BuildSummary(),
                P0CoreValuePresenter.DescribeTeamPoop(battle.TeamPoop).BuildSummary(),
                P0CoreValuePresenter.DescribeTeamHunger(battle.TeamHunger).BuildSummary()
            };
        }

        private static IReadOnlyList<string> BuildRouteRows(
            RunProgressionState run,
            RunNodeCompletionReport completionReport)
        {
            List<string> rows = new List<string>();
            if (completionReport != null)
            {
                rows.Add("路线结果：" + completionReport.BuildSummary());
                if (completionReport.BattleReward.HasReward)
                {
                    rows.Add("奖励：" + completionReport.BattleReward.BuildSummary());
                }
            }

            if (run == null)
            {
                if (rows.Count == 0)
                {
                    rows.Add("路线：未初始化");
                }

                return rows.AsReadOnly();
            }

            rows.Add("路线进度：" + run.Route.CompletedCount + "/" + run.Route.Route.LayerCount);
            if (run.Route.IsCleared)
            {
                rows.Add("路线状态：通关");
            }
            else if (run.Route.IsFailed)
            {
                rows.Add("路线状态：失败");
            }
            else if (run.Route.CurrentNode != null)
            {
                rows.Add("下一节点：" + P0RouteNodePresenter.Describe(run.Route.CurrentNode).Title);
            }

            rows.Add("资源：梦屑 " + run.Wallet.DreamShards + " 小鱼干 " + run.Wallet.FishTreats);
            rows.Add("祝福：" + run.Blessings.Count + " 总等级 " + run.Blessings.TotalLevel);
            return rows.AsReadOnly();
        }

        private static IReadOnlyList<P0BattleResultAction> BuildActions(BattleOutcome outcome)
        {
            bool isResolved = outcome != BattleOutcome.InProgress;
            return new[]
            {
                new P0BattleResultAction(
                    P0BattleResultActionIds.ContinueRoute,
                    P0InputCommand.ContinueRoute,
                    "继续路线",
                    "返回路线图并应用节点结果。",
                    "Enter",
                    P0SceneFlow.RouteMapSceneName,
                    isResolved,
                    "战斗结束后可用"),
                new P0BattleResultAction(
                    P0BattleResultActionIds.ReturnCatRoom,
                    P0InputCommand.ContinueRoute,
                    "返回猫房",
                    "把本场结果带回猫房，整理状态后再选择梦境。",
                    "C",
                    P0SceneFlow.CatRoomSceneName,
                    isResolved,
                    "战斗结束后可用"),
                new P0BattleResultAction(
                    P0BattleResultActionIds.RestartRun,
                    P0InputCommand.RestartRun,
                    "重开路线",
                    "立即开始新的第一层战斗。",
                    "R",
                    P0SceneFlow.GrayboxBattleSceneName,
                    true)
            };
        }
    }
}
