using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Inputs;
using TheCat.Roguelite;

namespace TheCat.Gameplay
{
    public static class P0RouteMapActionIds
    {
        public const string EnterCurrentNode = "enter_current_node";
        public const string NewRun = "new_run";
        public const string ReturnCatRoom = "return_cat_room";
        public const string MainMenu = "main_menu";
    }

    public readonly struct P0RouteMapAction
    {
        public P0RouteMapAction(
            string actionId,
            string label,
            bool isEnabled,
            P0InputCommand command,
            string targetSceneName,
            string detail)
        {
            ActionId = actionId ?? string.Empty;
            Label = label ?? string.Empty;
            IsEnabled = isEnabled;
            Command = command;
            TargetSceneName = targetSceneName ?? string.Empty;
            Detail = detail ?? string.Empty;
        }

        public string ActionId { get; }

        public string Label { get; }

        public bool IsEnabled { get; }

        public P0InputCommand Command { get; }

        public string TargetSceneName { get; }

        public string Detail { get; }

        public string BuildSummary()
        {
            return Label
                + " 可用 "
                + IsEnabled
                + " 目标 "
                + TargetSceneName
                + " "
                + Detail;
        }
    }

    public readonly struct P0RouteMapLayerRow
    {
        public P0RouteMapLayerRow(
            int layer,
            string stateToken,
            string optionPreview,
            string selectedNodeId,
            int optionCount,
            bool isCurrent,
            bool isCompleted,
            bool hasBranch,
            bool hasBattle,
            bool hasBoss)
        {
            Layer = Math.Max(0, layer);
            StateToken = stateToken ?? string.Empty;
            OptionPreview = optionPreview ?? string.Empty;
            SelectedNodeId = selectedNodeId ?? string.Empty;
            OptionCount = Math.Max(0, optionCount);
            IsCurrent = isCurrent;
            IsCompleted = isCompleted;
            HasBranch = hasBranch;
            HasBattle = hasBattle;
            HasBoss = hasBoss;
        }

        public int Layer { get; }

        public string StateToken { get; }

        public string OptionPreview { get; }

        public string SelectedNodeId { get; }

        public int OptionCount { get; }

        public bool IsCurrent { get; }

        public bool IsCompleted { get; }

        public bool HasBranch { get; }

        public bool HasBattle { get; }

        public bool HasBoss { get; }

        public string BuildLabel()
        {
            string prefix = IsCompleted ? "[x] " : IsCurrent ? "> " : "[ ] ";
            return prefix + Layer + ". " + OptionPreview;
        }

        public string BuildSummary()
        {
            return "第 "
                + Layer
                + " "
                + StateToken
                + " 选项 "
                + OptionCount
                + " 分支 "
                + HasBranch
                + " 战斗 "
                + HasBattle
                + " 首领 "
                + HasBoss;
        }
    }

    public readonly struct P0RouteMapCurrentNodeCard
    {
        public P0RouteMapCurrentNodeCard(
            string nodeId,
            int layer,
            string title,
            string nodeTypeToken,
            string riskHint,
            string rewardHint,
            string detail,
            bool requiresBattle,
            P0VisualAssetReference visualAsset,
            P0VisualAssetReference summaryBannerAsset)
        {
            NodeId = nodeId ?? string.Empty;
            Layer = Math.Max(0, layer);
            Title = title ?? string.Empty;
            NodeTypeToken = nodeTypeToken ?? string.Empty;
            RiskHint = riskHint ?? string.Empty;
            RewardHint = rewardHint ?? string.Empty;
            Detail = detail ?? string.Empty;
            RequiresBattle = requiresBattle;
            VisualAsset = visualAsset;
            SummaryBannerAsset = summaryBannerAsset;
        }

        public string NodeId { get; }

        public int Layer { get; }

        public string Title { get; }

        public string NodeTypeToken { get; }

        public string RiskHint { get; }

        public string RewardHint { get; }

        public string Detail { get; }

        public bool RequiresBattle { get; }

        public P0VisualAssetReference VisualAsset { get; }

        public P0VisualAssetReference SummaryBannerAsset { get; }

        public string BuildSummary()
        {
            return "当前：第 "
                + Layer
                + " 层 "
                + Title
                + " "
                + NodeTypeToken
                + " 战斗 "
                + RequiresBattle
                + " 奖励 "
                + RewardHint;
        }
    }

    public readonly struct P0RouteMapOptionCard
    {
        public P0RouteMapOptionCard(
            string slotLabel,
            string nodeId,
            string title,
            string compactLabel,
            bool isSelected,
            bool requiresBattle,
            P0VisualAssetReference visualAsset)
        {
            SlotLabel = slotLabel ?? string.Empty;
            NodeId = nodeId ?? string.Empty;
            Title = title ?? string.Empty;
            CompactLabel = compactLabel ?? string.Empty;
            IsSelected = isSelected;
            RequiresBattle = requiresBattle;
            VisualAsset = visualAsset;
        }

        public string SlotLabel { get; }

        public string NodeId { get; }

        public string Title { get; }

        public string CompactLabel { get; }

        public bool IsSelected { get; }

        public bool RequiresBattle { get; }

        public P0VisualAssetReference VisualAsset { get; }

        public string BuildButtonLabel()
        {
            return (IsSelected ? "> " : string.Empty) + SlotLabel + ". " + CompactLabel;
        }
    }

    public readonly struct P0RouteMapRewardChoiceCard
    {
        public P0RouteMapRewardChoiceCard(
            string slotLabel,
            string choiceId,
            string displayName,
            string summary,
            P0VisualAssetReference visualAsset,
            P0VisualAssetReference frameAsset,
            P0VisualAssetReference detailBadgeAsset,
            P0VisualAssetReference itemCardAsset)
        {
            SlotLabel = slotLabel ?? string.Empty;
            ChoiceId = choiceId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Summary = summary ?? string.Empty;
            VisualAsset = visualAsset;
            FrameAsset = frameAsset;
            DetailBadgeAsset = detailBadgeAsset;
            ItemCardAsset = itemCardAsset;
        }

        public string SlotLabel { get; }

        public string ChoiceId { get; }

        public string DisplayName { get; }

        public string Summary { get; }

        public P0VisualAssetReference VisualAsset { get; }

        public P0VisualAssetReference FrameAsset { get; }

        public P0VisualAssetReference DetailBadgeAsset { get; }

        public P0VisualAssetReference ItemCardAsset { get; }

        public string BuildButtonLabel()
        {
            return SlotLabel + ". " + Summary;
        }
    }

    public readonly struct P0RouteMapCatUpgradeChoiceCard
    {
        public P0RouteMapCatUpgradeChoiceCard(
            string slotLabel,
            string choiceId,
            string catId,
            string catLabel,
            string stageLabel,
            string displayName,
            string intentLabel,
            string summary)
        {
            SlotLabel = slotLabel ?? string.Empty;
            ChoiceId = choiceId ?? string.Empty;
            CatId = catId ?? string.Empty;
            CatLabel = catLabel ?? string.Empty;
            StageLabel = stageLabel ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            IntentLabel = intentLabel ?? string.Empty;
            Summary = summary ?? string.Empty;
        }

        public string SlotLabel { get; }

        public string ChoiceId { get; }

        public string CatId { get; }

        public string CatLabel { get; }

        public string StageLabel { get; }

        public string DisplayName { get; }

        public string IntentLabel { get; }

        public string Summary { get; }

        public string BuildButtonLabel()
        {
            string intent = string.IsNullOrWhiteSpace(IntentLabel)
                ? string.Empty
                : "（" + IntentLabel + "）";
            return SlotLabel + ". " + CatLabel + " · " + StageLabel + " · " + DisplayName + intent;
        }
    }

    public sealed class P0RouteMapSurface
    {
        public P0RouteMapSurface(
            string title,
            string message,
            string statusLabel,
            string progressLabel,
            bool isRouteComplete,
            bool isRouteCleared,
            P0RouteMapCurrentNodeCard currentNode,
            P0UiShellSurface uiShell,
            IReadOnlyList<P0RouteMapLayerRow> layerRows,
            IReadOnlyList<P0RouteMapOptionCard> routeOptions,
            IReadOnlyList<P0RouteMapRewardChoiceCard> rewardChoices,
            IReadOnlyList<P0RouteMapCatUpgradeChoiceCard> catUpgradeChoices,
            bool canRerollCatUpgrade,
            IReadOnlyList<string> summaryRows,
            IReadOnlyList<string> settlementRows,
            IReadOnlyList<string> settlementFocusRows,
            P0VisualAssetReference settlementOutcomeBannerAsset,
            IReadOnlyList<P0RouteMapAction> actions)
        {
            Title = title ?? string.Empty;
            Message = message ?? string.Empty;
            StatusLabel = statusLabel ?? string.Empty;
            ProgressLabel = progressLabel ?? string.Empty;
            IsRouteComplete = isRouteComplete;
            IsRouteCleared = isRouteCleared;
            CurrentNode = currentNode;
            UiShell = uiShell ?? P0UiShellPresenter.BuildSurface();
            LayerRows = layerRows ?? Array.Empty<P0RouteMapLayerRow>();
            RouteOptions = routeOptions ?? Array.Empty<P0RouteMapOptionCard>();
            RewardChoices = rewardChoices ?? Array.Empty<P0RouteMapRewardChoiceCard>();
            CatUpgradeChoices = catUpgradeChoices ?? Array.Empty<P0RouteMapCatUpgradeChoiceCard>();
            CanRerollCatUpgrade = canRerollCatUpgrade;
            SummaryRows = summaryRows ?? Array.Empty<string>();
            SettlementRows = settlementRows ?? Array.Empty<string>();
            SettlementFocusRows = settlementFocusRows ?? Array.Empty<string>();
            SettlementOutcomeBannerAsset = settlementOutcomeBannerAsset;
            Actions = actions ?? Array.Empty<P0RouteMapAction>();
        }

        public string Title { get; }

        public string Message { get; }

        public string StatusLabel { get; }

        public string ProgressLabel { get; }

        public bool IsRouteComplete { get; }

        public bool IsRouteCleared { get; }

        public P0RouteMapCurrentNodeCard CurrentNode { get; }

        public P0UiShellSurface UiShell { get; }

        public IReadOnlyList<P0RouteMapLayerRow> LayerRows { get; }

        public IReadOnlyList<P0RouteMapOptionCard> RouteOptions { get; }

        public IReadOnlyList<P0RouteMapRewardChoiceCard> RewardChoices { get; }

        public IReadOnlyList<P0RouteMapCatUpgradeChoiceCard> CatUpgradeChoices { get; }

        public bool CanRerollCatUpgrade { get; }

        public IReadOnlyList<string> SummaryRows { get; }

        public IReadOnlyList<string> SettlementRows { get; }

        public IReadOnlyList<string> SettlementFocusRows { get; }

        public P0VisualAssetReference SettlementOutcomeBannerAsset { get; }

        public IReadOnlyList<P0RouteMapAction> Actions { get; }

        public bool HasCurrentNode => !string.IsNullOrWhiteSpace(CurrentNode.NodeId);

        public bool TryGetAction(string actionId, out P0RouteMapAction action)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionId == actionId)
                {
                    action = Actions[i];
                    return true;
                }
            }

            action = default(P0RouteMapAction);
            return false;
        }
    }

    public static class P0RouteMapPresenter
    {
        private const int OldDreamMapPreviewOptionLimit = 2;

        public static P0RouteMapSurface BuildSurface(RunProgressionState run, string message)
        {
            if (run == null || run.Route == null)
            {
                return new P0RouteMapSurface(
                    "梦境路线",
                    message,
                    "路线未初始化",
                    "进度：0/0",
                    false,
                    false,
                    default(P0RouteMapCurrentNodeCard),
                    P0UiShellPresenter.BuildSurface(),
                    Array.Empty<P0RouteMapLayerRow>(),
                    Array.Empty<P0RouteMapOptionCard>(),
                    Array.Empty<P0RouteMapRewardChoiceCard>(),
                    Array.Empty<P0RouteMapCatUpgradeChoiceCard>(),
                    false,
                    new[] { "路线：未初始化" },
                    Array.Empty<string>(),
                    Array.Empty<string>(),
                    default(P0VisualAssetReference),
                    BuildActions(null));
            }

            RunRouteState route = run.Route;
            return new P0RouteMapSurface(
                "梦境路线",
                message,
                BuildStatusLabel(route),
                "进度：" + route.CompletedCount + "/" + route.Route.LayerCount,
                route.IsComplete,
                route.IsCleared,
                BuildCurrentNode(route.CurrentNode, run),
                P0UiShellPresenter.BuildSurface(),
                BuildLayerRows(route, run),
                BuildRouteOptions(route, run),
                BuildRewardChoices(route.CurrentNode, run),
                BuildCatUpgradeChoices(run),
                run.CatUpgrades.CanRerollWithPawStamp(run.EventItems),
                BuildSummaryRows(run),
                BuildSettlementRows(run),
                BuildSettlementFocusRows(run),
                BuildSettlementOutcomeBanner(run),
                BuildActions(run));
        }

        public static bool HasP0RouteMapSurface(P0RouteMapSurface surface)
        {
            if (surface == null
                || surface.Title != "梦境路线"
                || string.IsNullOrWhiteSpace(surface.ProgressLabel)
                || !P0UiShellPresenter.HasP0UiShellSurface(surface.UiShell)
                || surface.LayerRows.Count != P0RouteCatalog.CreateTenLayerRoute().LayerCount
                || !HasAction(surface, P0RouteMapActionIds.EnterCurrentNode)
                || !HasAction(surface, P0RouteMapActionIds.NewRun)
                || !HasAction(surface, P0RouteMapActionIds.MainMenu)
                || surface.SummaryRows.Count < 6)
            {
                return false;
            }

            if (surface.IsRouteComplete && !HasAction(surface, P0RouteMapActionIds.ReturnCatRoom))
            {
                return false;
            }

            bool hasBoss = false;
            bool hasBranch = false;
            bool hasCurrent = false;
            for (int i = 0; i < surface.LayerRows.Count; i++)
            {
                hasBoss |= surface.LayerRows[i].HasBoss;
                hasBranch |= surface.LayerRows[i].HasBranch;
                hasCurrent |= surface.LayerRows[i].IsCurrent;
                if (string.IsNullOrWhiteSpace(surface.LayerRows[i].BuildLabel()))
                {
                    return false;
                }
            }

            if (!surface.IsRouteComplete && (!surface.HasCurrentNode || !hasCurrent))
            {
                return false;
            }

            return hasBoss && hasBranch;
        }

        public static string BuildCompactSummary(P0RouteMapSurface surface)
        {
            if (surface == null)
            {
                return "路线图界面：缺失";
            }

            int branchRows = 0;
            int bossRows = 0;
            for (int i = 0; i < surface.LayerRows.Count; i++)
            {
                branchRows += surface.LayerRows[i].HasBranch ? 1 : 0;
                bossRows += surface.LayerRows[i].HasBoss ? 1 : 0;
            }

            return "路线图界面：层数 "
                + surface.LayerRows.Count
                + " 进度 "
                + surface.ProgressLabel
                + " 选项 "
                + surface.RouteOptions.Count
                + " 奖励 "
                + surface.RewardChoices.Count
                + " 分支 "
                + branchRows
                + " 首领层 "
                + bossRows
                + " 操作 "
                + surface.Actions.Count
                + " uiShell "
                + surface.UiShell.AssetCount
                + " 状态 "
                + surface.StatusLabel;
        }

        private static P0RouteMapCurrentNodeCard BuildCurrentNode(
            RouteNodeDefinition node,
            RunProgressionState run)
        {
            if (node == null)
            {
                return default(P0RouteMapCurrentNodeCard);
            }

            RouteNodePresentation presentation = P0RouteNodePresenter.Describe(node, run);
            return new P0RouteMapCurrentNodeCard(
                node.Id,
                node.Layer,
                presentation.Title,
                BuildNodeTypeLabel(node.NodeType),
                presentation.RiskHint,
                presentation.RewardHint,
                presentation.Detail,
                presentation.RequiresBattle,
                presentation.VisualAsset,
                P0VisualAssetCatalog.GetRouteNodeSummaryBanner(node.NodeType));
        }

        private static IReadOnlyList<P0RouteMapLayerRow> BuildLayerRows(
            RunRouteState route,
            RunProgressionState run)
        {
            List<P0RouteMapLayerRow> rows = new List<P0RouteMapLayerRow>();
            for (int layerIndex = 0; layerIndex < route.Route.LayerCount; layerIndex++)
            {
                rows.Add(BuildLayerRow(route, run, layerIndex));
            }

            return rows.AsReadOnly();
        }

        private static P0RouteMapLayerRow BuildLayerRow(
            RunRouteState route,
            RunProgressionState run,
            int layerIndex)
        {
            IReadOnlyList<RouteNodeDefinition> options = route.Route.GetLayerOptions(layerIndex + 1);
            RouteNodeDefinition selectedNode = GetSelectedNodeForLayer(route, layerIndex);
            bool isCompleted = layerIndex < route.CompletedCount;
            bool isCurrent = layerIndex == route.CurrentNodeIndex && !route.IsComplete;
            bool isOldDreamMapPreview = ShouldRevealOldDreamMapPreview(route, run, layerIndex);
            bool shouldRevealOptions = isCompleted || isCurrent || isOldDreamMapPreview;
            bool hasBattle = false;
            bool hasBoss = false;
            List<string> labels = new List<string>();

            for (int i = 0; i < options.Count; i++)
            {
                RouteNodeDefinition option = options[i];
                hasBattle |= RouteNodeResolver.RequiresBattle(option.NodeType);
                hasBoss |= option.NodeType == RouteNodeType.Boss;
                if (shouldRevealOptions && (!isOldDreamMapPreview || labels.Count < OldDreamMapPreviewOptionLimit))
                {
                    string label = P0RouteNodePresenter.Describe(option, run).BuildCompactLabel();
                    labels.Add(selectedNode != null && selectedNode.Id == option.Id ? "*" + label : label);
                }
            }

            if (!shouldRevealOptions)
            {
                labels.Add(options.Count > 1 ? "未知分支 x" + options.Count : "未知路线");
            }
            else if (isOldDreamMapPreview && options.Count > OldDreamMapPreviewOptionLimit)
            {
                labels.Add("...");
            }

            return new P0RouteMapLayerRow(
                layerIndex + 1,
                isCompleted ? "已完成" : isCurrent ? "当前" : isOldDreamMapPreview ? "旧梦地图预览" : "未来",
                string.Join(" | ", labels),
                selectedNode == null ? string.Empty : selectedNode.Id,
                options.Count,
                isCurrent,
                isCompleted,
                options.Count > 1,
                hasBattle,
                hasBoss);
        }

        private static RouteNodeDefinition GetSelectedNodeForLayer(RunRouteState route, int layerIndex)
        {
            if (layerIndex < route.CompletedCount)
            {
                return route.Completions[layerIndex].Node;
            }

            if (layerIndex == route.CurrentNodeIndex && !route.IsComplete)
            {
                return route.CurrentNode;
            }

            return null;
        }

        private static bool ShouldRevealOldDreamMapPreview(
            RunRouteState route,
            RunProgressionState run,
            int layerIndex)
        {
            if (route == null
                || run == null
                || route.IsComplete
                || !run.EventItems.Has(RunEventItemInventory.OldDreamMapId))
            {
                return false;
            }

            return layerIndex == route.CurrentNodeIndex + 1;
        }

        private static IReadOnlyList<P0RouteMapOptionCard> BuildRouteOptions(
            RunRouteState route,
            RunProgressionState run)
        {
            if (route == null || route.IsComplete || route.CurrentLayerOptions.Count <= 1)
            {
                return Array.Empty<P0RouteMapOptionCard>();
            }

            List<P0RouteMapOptionCard> options = new List<P0RouteMapOptionCard>();
            for (int i = 0; i < route.CurrentLayerOptions.Count; i++)
            {
                RouteNodeDefinition option = route.CurrentLayerOptions[i];
                RouteNodePresentation presentation = P0RouteNodePresenter.Describe(option, run);
                options.Add(new P0RouteMapOptionCard(
                    (i + 1).ToString(),
                    option.Id,
                    presentation.Title,
                    presentation.BuildCompactLabel(),
                    route.CurrentNode.Id == option.Id,
                    presentation.RequiresBattle,
                    presentation.VisualAsset));
            }

            return options.AsReadOnly();
        }

        private static IReadOnlyList<P0RouteMapRewardChoiceCard> BuildRewardChoices(
            RouteNodeDefinition node,
            RunProgressionState run)
        {
            if (node == null || run == null || RouteNodeResolver.RequiresBattle(node.NodeType))
            {
                return Array.Empty<P0RouteMapRewardChoiceCard>();
            }

            IReadOnlyList<RouteRewardChoice> choices = P0RouteRewardResolver.CreatePlaceholderChoices(node, run);
            P0VisualAssetReference frameAsset = P0VisualAssetCatalog.GetRouteRewardCardFrame(node.NodeType);
            List<P0RouteMapRewardChoiceCard> cards = new List<P0RouteMapRewardChoiceCard>();
            for (int i = 0; i < choices.Count; i++)
            {
                cards.Add(new P0RouteMapRewardChoiceCard(
                    (i + 1).ToString(),
                    choices[i].Id,
                    choices[i].DisplayName,
                    choices[i].BuildSummary(),
                    P0VisualAssetCatalog.GetRouteChoiceIcon(choices[i]),
                    frameAsset,
                    P0VisualAssetCatalog.GetRouteRewardDetailBadge(choices[i]),
                    P0VisualAssetCatalog.GetRouteChoiceCard(choices[i])));
            }

            return cards.AsReadOnly();
        }

        private static IReadOnlyList<P0RouteMapCatUpgradeChoiceCard> BuildCatUpgradeChoices(RunProgressionState run)
        {
            if (run == null)
            {
                return Array.Empty<P0RouteMapCatUpgradeChoiceCard>();
            }

            IReadOnlyList<CatUpgradeCandidate> choices = run.CatUpgrades.CreateCurrentOffer(run.Roster);
            if (choices.Count == 0)
            {
                return Array.Empty<P0RouteMapCatUpgradeChoiceCard>();
            }

            List<P0RouteMapCatUpgradeChoiceCard> cards = new List<P0RouteMapCatUpgradeChoiceCard>();
            for (int i = 0; i < choices.Count; i++)
            {
                CatUpgradeCandidate choice = choices[i];
                CatPresentation cat = P0CatPresenter.Describe(choice.CatId);
                cards.Add(new P0RouteMapCatUpgradeChoiceCard(
                    (i + 1).ToString(),
                    choice.Id,
                    choice.CatId,
                    cat.ShortLabel,
                    choice.StageLabel,
                    choice.DisplayName,
                    choice.PlayerIntent,
                    choice.BuildSummary()));
            }

            return cards.AsReadOnly();
        }

        private static IReadOnlyList<string> BuildSummaryRows(RunProgressionState run)
        {
            List<string> rows = new List<string>
            {
                "资源：梦屑 " + run.Wallet.DreamShards + " 小鱼干 " + run.Wallet.FishTreats,
                "核心：" + P0CoreValuePresenter.BuildRunCoreSummary(run.CoreValues),
                "猫咪生命：" + BuildCatVitalSummary(run),
                "待触发事件：" + run.PendingBattleModifiers.BuildSummary(),
                "事件道具：" + run.EventItems.BuildSummary(),
                BuildOldDreamMapSummary(run),
                "队伍：" + BuildRosterSummary(run),
                "祝福：" + run.Blessings.Count + " 等级 " + run.Blessings.TotalLevel,
                "祝福详情：" + run.Blessings.BuildSummary()
            };

            rows.Insert(1, BuildDreamMapSummary(run));
            rows.Insert(2, run.CatUpgrades.BuildProgressSummary());
            rows.Insert(6, BuildPawStampSummary(run));
            if (!string.IsNullOrWhiteSpace(run.CatUpgrades.LastResolvedSummary))
            {
                rows.Add("上次猫咪升级：" + run.CatUpgrades.LastResolvedSummary);
            }

            RunNodeCompletionReport report = P0RunSession.LastCompletionReport;
            if (report != null)
            {
                rows.Add("上一节点：" + report.BuildSummary());
            }

            return rows.AsReadOnly();
        }

        private static string BuildDreamMapSummary(RunProgressionState run)
        {
            DreamMapDefinition map = run == null || run.DreamMap == null
                ? P0DreamMapCatalog.GetBedroomDreamMap()
                : run.DreamMap;
            string readiness = map.IsPlayableInP0 ? "可玩" : "占位";
            return "梦境主题：" + map.DisplayName
                + " / " + map.ThemeLabel
                + " / 守护 " + map.DefenseTargetLabel
                + " / " + readiness;
        }

        private static string BuildPawStampSummary(RunProgressionState run)
        {
            if (run == null || !run.EventItems.Has(RunEventItemInventory.PawStampId))
            {
                return "猫爪印章：未持有";
            }

            if (run.CatUpgrades.HasPendingUpgrade)
            {
                return "猫爪印章：可消耗 1 枚，刷新这批猫咪升级候选";
            }

            return "猫爪印章：等待下一次猫咪升级";
        }

        private static string BuildOldDreamMapSummary(RunProgressionState run)
        {
            if (run == null || !run.EventItems.Has(RunEventItemInventory.OldDreamMapId))
            {
                return "旧梦地图：未持有";
            }

            RunRouteState route = run.Route;
            if (route == null || route.IsComplete || route.CurrentNodeIndex + 1 >= route.Route.LayerCount)
            {
                return "旧梦地图：暂无可预览路线";
            }

            int previewLayer = route.CurrentNodeIndex + 2;
            IReadOnlyList<RouteNodeDefinition> options = route.Route.GetLayerOptions(previewLayer);
            List<string> labels = new List<string>();
            for (int i = 0; i < options.Count && i < OldDreamMapPreviewOptionLimit; i++)
            {
                labels.Add(P0RouteNodePresenter.Describe(options[i], run).Title);
            }

            if (labels.Count == 0)
            {
                return "旧梦地图：暂无可预览路线";
            }

            return "旧梦地图：第 " + previewLayer + " 层预览 " + string.Join(" / ", labels);
        }

        private static IReadOnlyList<string> BuildSettlementRows(RunProgressionState run)
        {
            if (run == null || !run.Route.IsComplete)
            {
                return Array.Empty<string>();
            }

            return P0SettlementPresenter.BuildRows(new P0RunSettlementSummary(run));
        }

        private static IReadOnlyList<string> BuildSettlementFocusRows(RunProgressionState run)
        {
            if (run == null || !run.Route.IsComplete)
            {
                return Array.Empty<string>();
            }

            return P0SettlementPresenter.BuildPlayerFocusRows(new P0RunSettlementSummary(run));
        }

        private static P0VisualAssetReference BuildSettlementOutcomeBanner(RunProgressionState run)
        {
            if (run == null || !run.Route.IsComplete)
            {
                return default(P0VisualAssetReference);
            }

            return P0VisualAssetCatalog.GetSettlementOutcomeBanner(run.Route.IsCleared);
        }

        private static IReadOnlyList<P0RouteMapAction> BuildActions(RunProgressionState run)
        {
            RunRouteState route = run == null ? null : run.Route;
            bool canEnter = route != null && !route.IsComplete;
            if (run != null && run.CatUpgrades.HasPendingUpgrade)
            {
                canEnter = false;
            }

            bool startsBattle = canEnter
                && route.CurrentNode != null
                && RouteNodeResolver.RequiresBattle(route.CurrentNode.NodeType);

            List<P0RouteMapAction> actions = new List<P0RouteMapAction>
            {
                new P0RouteMapAction(
                    P0RouteMapActionIds.EnterCurrentNode,
                    "进入当前节点",
                    canEnter,
                    P0InputCommand.ContinueRoute,
                    startsBattle ? P0SceneFlow.GrayboxBattleSceneName : string.Empty,
                    startsBattle ? "开始战斗" : "结算路线节点")
            };

            if (route != null && route.IsComplete)
            {
                actions.Add(new P0RouteMapAction(
                    P0RouteMapActionIds.ReturnCatRoom,
                    "返回猫房",
                    true,
                    P0InputCommand.ContinueRoute,
                    P0SceneFlow.CatRoomSceneName,
                    "把本轮路线结算带回猫房"));
            }

            actions.Add(
                new P0RouteMapAction(
                    P0RouteMapActionIds.NewRun,
                    "新路线",
                    true,
                    P0InputCommand.RestartRun,
                    P0SceneFlow.RouteMapSceneName,
                    "重开路线"));
            actions.Add(
                new P0RouteMapAction(
                    P0RouteMapActionIds.MainMenu,
                    "返回主菜单",
                    true,
                    P0InputCommand.RestartRun,
                    P0SceneFlow.MainMenuSceneName,
                    "返回主菜单"));
            return actions.AsReadOnly();
        }

        private static string BuildStatusLabel(RunRouteState route)
        {
            if (route.IsFailed)
            {
                return "失败";
            }

            if (route.IsCleared)
            {
                return "通关";
            }

            return "进行中";
        }

        private static string BuildNodeTypeLabel(RouteNodeType nodeType)
        {
            switch (nodeType)
            {
                case RouteNodeType.Defense:
                    return "守床";
                case RouteNodeType.Elite:
                    return "精英";
                case RouteNodeType.Partner:
                    return "伙伴";
                case RouteNodeType.Shop:
                    return "商店";
                case RouteNodeType.DreamEvent:
                    return "梦境事件";
                case RouteNodeType.BlessingOffering:
                    return "祝福供奉";
                case RouteNodeType.RestNest:
                    return "休整猫窝";
                case RouteNodeType.Boss:
                    return "首领";
                default:
                    return "未知";
            }
        }

        private static string BuildCatVitalSummary(RunProgressionState run)
        {
            if (run == null || run.CatVitals.Count == 0)
            {
                return "未初始化";
            }

            List<string> summaries = new List<string>();
            for (int i = 0; i < run.Roster.CatIds.Count; i++)
            {
                if (!run.CatVitals.TryGet(run.Roster.CatIds[i], out RunCatVitalSnapshot snapshot))
                {
                    continue;
                }

                CatPresentation presentation = P0CatPresenter.Describe(snapshot.CatId);
                summaries.Add(presentation.BuildVitalLabel(
                    snapshot.CurrentHp,
                    snapshot.MaxHp,
                    snapshot.IsWeak,
                    snapshot.WeakRemainingSeconds));
            }

            return summaries.Count == 0 ? "未初始化" : string.Join("; ", summaries);
        }

        private static string BuildRosterSummary(RunProgressionState run)
        {
            if (run == null || run.Roster.Count == 0)
            {
                return "无";
            }

            List<string> labels = new List<string>();
            for (int i = 0; i < run.Roster.CatIds.Count; i++)
            {
                labels.Add(P0CatPresenter.Describe(run.Roster.CatIds[i]).ShortLabel);
            }

            return string.Join(", ", labels);
        }

        private static bool HasAction(P0RouteMapSurface surface, string actionId)
        {
            return surface.TryGetAction(actionId, out P0RouteMapAction action)
                && !string.IsNullOrWhiteSpace(action.Label);
        }
    }
}
