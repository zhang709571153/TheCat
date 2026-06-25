using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Inputs;
using TheCat.Roguelite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.Gameplay
{
    public enum P0RouteMapCommandAction
    {
        None,
        SelectRouteOption,
        ResolveRewardChoice,
        ResolveCatUpgradeChoice,
        StartBattle,
        StartNewRun,
        RouteEnded
    }

    public readonly struct P0RouteMapCommandResult
    {
        public P0RouteMapCommandResult(
            bool isHandled,
            P0RouteMapCommandAction action,
            string message,
            string selectedNodeId = "",
            string selectedChoiceId = "")
        {
            IsHandled = isHandled;
            Action = action;
            Message = message ?? string.Empty;
            SelectedNodeId = selectedNodeId ?? string.Empty;
            SelectedChoiceId = selectedChoiceId ?? string.Empty;
        }

        public bool IsHandled { get; }

        public P0RouteMapCommandAction Action { get; }

        public string Message { get; }

        public string SelectedNodeId { get; }

        public string SelectedChoiceId { get; }

        public bool ShouldLoadBattle => Action == P0RouteMapCommandAction.StartBattle;

        public static P0RouteMapCommandResult NotHandled()
        {
            return new P0RouteMapCommandResult(false, P0RouteMapCommandAction.None, string.Empty);
        }
    }

    public static class P0RouteMapCommandRouter
    {
        public static P0RouteMapCommandResult Execute(RunProgressionState run, P0InputCommand command)
        {
            if (run == null)
            {
                throw new ArgumentNullException(nameof(run));
            }

            switch (command)
            {
                case P0InputCommand.ContinueRoute:
                    return ConfirmCurrentNode(run);
                case P0InputCommand.SelectCat1:
                    return SelectRouteSlot(run, 0);
                case P0InputCommand.SelectCat2:
                    return SelectRouteSlot(run, 1);
                case P0InputCommand.SelectCat3:
                    return SelectRouteSlot(run, 2);
                case P0InputCommand.RestartRun:
                return new P0RouteMapCommandResult(
                    true,
                    P0RouteMapCommandAction.StartNewRun,
                    "新路线已开始。");
                default:
                    return P0RouteMapCommandResult.NotHandled();
            }
        }

        public static P0RouteMapCommandResult ConfirmCurrentNode(RunProgressionState run)
        {
            if (run == null)
            {
                throw new ArgumentNullException(nameof(run));
            }

            RunRouteState route = run.Route;
            RouteNodeDefinition node = route.CurrentNode;
            if (node == null)
            {
                return BuildRouteEndedResult(route);
            }

            if (run.CatUpgrades.HasPendingUpgrade)
            {
                return new P0RouteMapCommandResult(
                    true,
                    P0RouteMapCommandAction.None,
                    "请选择猫咪升级候选后继续。",
                    selectedNodeId: node.Id);
            }

            if (RouteNodeResolver.RequiresBattle(node.NodeType))
            {
                RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(run);
                return new P0RouteMapCommandResult(
                    true,
                    P0RouteMapCommandAction.StartBattle,
                    resolution.Message,
                    selectedNodeId: node.Id);
            }

            RouteRewardChoice choice = P0RouteRewardResolver.GetDefaultPlaceholderChoice(node, run);
            if (choice == null)
            {
                return new P0RouteMapCommandResult(
                    true,
                    P0RouteMapCommandAction.None,
                    "请选择第 " + node.Layer + " 层奖励。",
                    selectedNodeId: node.Id);
            }

            return ResolveChoice(run, node, choice);
        }

        public static P0RouteMapCommandResult SelectRouteSlot(RunProgressionState run, int slotIndex)
        {
            if (run == null)
            {
                throw new ArgumentNullException(nameof(run));
            }

            if (slotIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(slotIndex), slotIndex, "Route slot index must not be negative.");
            }

            RunRouteState route = run.Route;
            RouteNodeDefinition node = route.CurrentNode;
            if (node == null)
            {
                return BuildRouteEndedResult(route);
            }

            if (run.CatUpgrades.HasPendingUpgrade)
            {
                return ResolveCatUpgradeChoice(run, slotIndex);
            }

            if (route.CurrentLayerOptions.Count > 1 && !route.IsCurrentNodeExplicitlySelected)
            {
                return SelectCurrentLayerOption(run, slotIndex);
            }

            if (!RouteNodeResolver.RequiresBattle(node.NodeType))
            {
                IReadOnlyList<RouteRewardChoice> choices = P0RouteRewardResolver.CreatePlaceholderChoices(node, run);
                if (slotIndex < choices.Count)
                {
                    return ResolveChoice(run, node, choices[slotIndex]);
                }

                return new P0RouteMapCommandResult(
                    true,
                    P0RouteMapCommandAction.None,
                    "第 " + (slotIndex + 1) + " 个奖励栏位为空。",
                    selectedNodeId: node.Id);
            }

            return new P0RouteMapCommandResult(
                true,
                P0RouteMapCommandAction.None,
                "第 " + (slotIndex + 1) + " 个路线栏位为空。",
                selectedNodeId: node.Id);
        }

        private static P0RouteMapCommandResult SelectCurrentLayerOption(RunProgressionState run, int slotIndex)
        {
            RunRouteState route = run.Route;
            IReadOnlyList<RouteNodeDefinition> options = route.CurrentLayerOptions;
            if (slotIndex >= options.Count)
            {
                return new P0RouteMapCommandResult(
                    true,
                    P0RouteMapCommandAction.None,
                    "第 " + (slotIndex + 1) + " 个路线栏位为空。");
            }

            RouteNodeDefinition option = options[slotIndex];
            route.SelectCurrentNode(option.Id);
            return new P0RouteMapCommandResult(
                true,
                P0RouteMapCommandAction.SelectRouteOption,
                "已选择 " + P0RouteNodePresenter.Describe(option, run).Title + "。",
                selectedNodeId: option.Id);
        }

        private static P0RouteMapCommandResult ResolveCatUpgradeChoice(RunProgressionState run, int slotIndex)
        {
            IReadOnlyList<CatUpgradeCandidate> choices = run.CatUpgrades.CreateCurrentOffer(run.Roster);
            RouteNodeDefinition node = run.Route.CurrentNode;
            string nodeId = node == null ? string.Empty : node.Id;
            if (slotIndex >= choices.Count)
            {
                return new P0RouteMapCommandResult(
                    true,
                    P0RouteMapCommandAction.None,
                    "第 " + (slotIndex + 1) + " 个猫咪升级栏位为空。",
                    selectedNodeId: nodeId);
            }

            CatUpgradeCandidate choice = choices[slotIndex];
            if (!run.CatUpgrades.TrySelect(choice.Id, run.Roster, out CatUpgradeCandidate selected))
            {
                return new P0RouteMapCommandResult(
                    true,
                    P0RouteMapCommandAction.None,
                    "该猫咪升级候选已不可用。",
                    selectedNodeId: nodeId,
                    selectedChoiceId: choice.Id);
            }

            CatPresentation cat = P0CatPresenter.Describe(selected.CatId);
            return new P0RouteMapCommandResult(
                true,
                P0RouteMapCommandAction.ResolveCatUpgradeChoice,
                BuildCatUpgradeResolvedMessage(cat, selected),
                selectedNodeId: nodeId,
                selectedChoiceId: selected.Id);
        }

        private static P0RouteMapCommandResult ResolveChoice(
            RunProgressionState run,
            RouteNodeDefinition node,
            RouteRewardChoice choice)
        {
            RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(run, choice);
            return new P0RouteMapCommandResult(
                true,
                resolution.ResolutionType == RouteNodeResolutionType.PlaceholderResolved
                    ? P0RouteMapCommandAction.ResolveRewardChoice
                    : P0RouteMapCommandAction.None,
                resolution.Message,
                selectedNodeId: node.Id,
                selectedChoiceId: choice.Id);
        }

        private static P0RouteMapCommandResult BuildRouteEndedResult(RunRouteState route)
        {
            return new P0RouteMapCommandResult(
                true,
                P0RouteMapCommandAction.RouteEnded,
                route.IsFailed ? "路线失败。" : "路线已通关。");
        }

        private static string BuildCatUpgradeResolvedMessage(CatPresentation cat, CatUpgradeCandidate selected)
        {
            string intent = string.IsNullOrWhiteSpace(selected.PlayerIntent)
                ? string.Empty
                : "（" + selected.PlayerIntent + "）";
            return "已升级 " + cat.ShortLabel + "：" + selected.DisplayName + intent + "。";
        }
    }

    public sealed class RouteMapController : MonoBehaviour
    {
        public const string RouteMapSceneName = P0SceneFlow.RouteMapSceneName;

        private string message = "就绪";
        private Vector2 routeScrollPosition;
        private float routePanelInnerWidth = P0ImGuiLayout.ReferenceWidth;
        private GUIStyle panelContentStyle;
        private GUIStyle wrappedRouteLabel;
        private GUIStyle routeSectionLabel;

        private void Awake()
        {
            P0RunSession.EnsureAnyProgression();
        }

        private void Update()
        {
            HandleKeyboardInput();
        }

        private void OnGUI()
        {
            P0RouteMapSurface surface = BuildRouteMapSurface();
            Rect panelRect = surface.IsRouteComplete
                ? P0ImGuiLayout.BuildLeftPanelRect(520f, 860f, 0.56f)
                : P0ImGuiLayout.BuildLeftPanelRect(360f, 600f, 0.44f);
            routePanelInnerWidth = P0ImGuiLayout.ScrollContentWidth(panelRect);
            P0ImGuiVisualAssetDrawer.DrawTexture(surface.UiShell.DreamGlassPanel, panelRect, ScaleMode.StretchToFill);

            GUILayout.BeginArea(panelRect, PanelContentStyle);
            routeScrollPosition = GUILayout.BeginScrollView(routeScrollPosition, false, true);
            GUILayout.Label(surface.Title, RouteSectionLabel);
            DrawRoute(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);

            if (!surface.IsRouteComplete)
            {
                P0RouteMapAction enterAction = GetAction(surface, P0RouteMapActionIds.EnterCurrentNode);
                GUI.enabled = enterAction.IsEnabled;
                if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(surface.UiShell.PrimaryButton, enterAction.Label, P0ImGuiLayout.PrimaryButtonHeight))
                {
                    EnterCurrentNode();
                }
            }

            P0RouteMapAction newRunAction = GetAction(surface, P0RouteMapActionIds.NewRun);
            GUI.enabled = true;
            if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(surface.UiShell.PrimaryButton, newRunAction.Label, P0ImGuiLayout.ButtonHeight))
            {
                P0RunSession.StartNewRun();
                message = "新路线已开始。";
            }

            P0RouteMapAction mainMenuAction = GetAction(surface, P0RouteMapActionIds.MainMenu);
            if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(surface.UiShell.PrimaryButton, mainMenuAction.Label, P0ImGuiLayout.ButtonHeight))
            {
                SceneManager.LoadScene(MainMenuController.MainMenuSceneName);
            }

            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            if (surface.IsRouteComplete)
            {
                DrawSettlementDetails(surface);
                GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            }

            GUILayout.Label(message);
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        public void ExecuteInputCommand(P0InputCommand command)
        {
            RunProgressionState run = P0RunSession.EnsureAnyProgression();
            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, command);
            if (!result.IsHandled)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(result.Message))
            {
                message = result.Message;
            }

            if (result.Action == P0RouteMapCommandAction.StartNewRun)
            {
                P0RunSession.StartNewRun();
                return;
            }

            if (result.ShouldLoadBattle)
            {
                SceneManager.LoadScene(P0SceneFlow.GrayboxBattleSceneName);
            }
        }

        public P0RouteMapSurface BuildRouteMapSurfaceForSmoke()
        {
            return BuildRouteMapSurface();
        }

        public void EnterCurrentNode()
        {
            RunProgressionState run = P0RunSession.EnsureAnyProgression();
            RunRouteState route = run.Route;
            RouteNodeDefinition node = route.CurrentNode;
            if (node == null)
            {
                message = route.IsCleared ? "路线已通关。" : "路线已结束。";
                return;
            }

            if (run.CatUpgrades.HasPendingUpgrade)
            {
                message = "请选择猫咪升级候选后继续。";
                return;
            }

            if (RouteNodeResolver.RequiresBattle(node.NodeType))
            {
                RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(run);
                message = resolution.Message;
                SceneManager.LoadScene(P0SceneFlow.GrayboxBattleSceneName);
                return;
            }

            message = "请选择 " + P0RouteNodePresenter.Describe(node, run).Title + " 的奖励。";
        }

        private void HandleKeyboardInput()
        {
            if (TryExecuteKeyboardCommand(P0InputCommand.ContinueRoute))
            {
                return;
            }

            if (TryExecuteKeyboardCommand(P0InputCommand.SelectCat1))
            {
                return;
            }

            if (TryExecuteKeyboardCommand(P0InputCommand.SelectCat2))
            {
                return;
            }

            if (TryExecuteKeyboardCommand(P0InputCommand.SelectCat3))
            {
                return;
            }

            TryExecuteKeyboardCommand(P0InputCommand.RestartRun);
        }

        private bool TryExecuteKeyboardCommand(P0InputCommand command)
        {
            if (!P0KeyboardInputMap.WasPressedThisFrame(command))
            {
                return false;
            }

            ExecuteInputCommand(command);
            return true;
        }

        private P0RouteMapSurface BuildRouteMapSurface()
        {
            RunProgressionState run = P0RunSession.EnsureAnyProgression();
            return P0RouteMapPresenter.BuildSurface(run, message);
        }

        private void DrawRoute(P0RouteMapSurface surface)
        {
            if (surface.IsRouteComplete)
            {
                DrawSettlementFocus(surface);
                return;
            }

            GUILayout.Label(surface.ProgressLabel);
            GUILayout.Label("状态：" + surface.StatusLabel);
            if (surface.HasCurrentNode)
            {
                if (surface.CurrentNode.VisualAsset.HasAsset)
                {
                    P0ImGuiVisualAssetDrawer.DrawIcon(surface.CurrentNode.VisualAsset, 56f);
                }

                if (surface.CurrentNode.SummaryBannerAsset.HasAsset)
                {
                    float bannerHeight = P0ImGuiLayout.Scaled(86f);
                    Rect bannerRect = GUILayoutUtility.GetRect(
                        1f,
                        bannerHeight,
                        GUILayout.ExpandWidth(true),
                        GUILayout.Height(bannerHeight));
                    P0ImGuiVisualAssetDrawer.DrawTexture(
                        surface.CurrentNode.SummaryBannerAsset,
                        bannerRect,
                        ScaleMode.ScaleToFit);
                }

                GUILayout.Label("当前：第 " + surface.CurrentNode.Layer + " 层 " + surface.CurrentNode.Title);
                GUILayout.Label("风险：" + surface.CurrentNode.RiskHint + "  奖励：" + surface.CurrentNode.RewardHint);
                GUILayout.Label(surface.CurrentNode.Detail);
            }

            DrawCatUpgradeChoices(surface);
            DrawCurrentNodeOptions(surface);
            GUILayout.Space(6f);
            for (int i = 0; i < surface.LayerRows.Count; i++)
            {
                GUILayout.Label(surface.LayerRows[i].BuildLabel());
            }

            GUILayout.Space(6f);
            DrawWalletStrip(surface);
            for (int i = 0; i < surface.SummaryRows.Count; i++)
            {
                GUILayout.Label(surface.SummaryRows[i]);
            }

            if (surface.SettlementOutcomeBannerAsset.HasAsset)
            {
                float bannerWidth = Mathf.Min(P0ImGuiLayout.Scaled(260f), routePanelInnerWidth);
                P0ImGuiVisualAssetDrawer.DrawGUILayoutTexture(surface.SettlementOutcomeBannerAsset, bannerWidth, P0ImGuiLayout.Scaled(82f));
            }

            for (int i = 0; i < surface.SettlementRows.Count; i++)
            {
                GUILayout.Label(surface.SettlementRows[i]);
            }

            DrawCurrentChoices(surface);
        }

        private void DrawSettlementFocus(P0RouteMapSurface surface)
        {
            GUILayout.Label(surface.ProgressLabel + "  状态：" + surface.StatusLabel, WrappedRouteLabel);
            if (surface.SettlementOutcomeBannerAsset.HasAsset)
            {
                float bannerWidth = Mathf.Min(P0ImGuiLayout.Scaled(420f), routePanelInnerWidth);
                P0ImGuiVisualAssetDrawer.DrawGUILayoutTexture(
                    surface.SettlementOutcomeBannerAsset,
                    bannerWidth,
                    P0ImGuiLayout.Scaled(118f));
            }

            GUILayout.Space(6f);
            GUILayout.Label("结算结果", RouteSectionLabel);
            for (int i = 0; i < surface.SettlementRows.Count; i++)
            {
                if (ShouldShowSettlementFocusRow(i))
                {
                    GUILayout.Label(surface.SettlementRows[i], WrappedRouteLabel);
                }
            }

        }

        private void DrawSettlementDetails(P0RouteMapSurface surface)
        {
            GUILayout.Label("结算明细", RouteSectionLabel);
            for (int i = 0; i < surface.SettlementRows.Count; i++)
            {
                if (!ShouldShowSettlementFocusRow(i))
                {
                    GUILayout.Label(surface.SettlementRows[i], WrappedRouteLabel);
                }
            }

            GUILayout.Space(8f);
            GUILayout.Label("路线回顾", RouteSectionLabel);
            for (int i = 0; i < surface.LayerRows.Count; i++)
            {
                GUILayout.Label(surface.LayerRows[i].BuildLabel(), WrappedRouteLabel);
            }

            GUILayout.Space(8f);
            GUILayout.Label("资源与队伍", RouteSectionLabel);
            DrawWalletStrip(surface);
            for (int i = 0; i < surface.SummaryRows.Count; i++)
            {
                GUILayout.Label(surface.SummaryRows[i], WrappedRouteLabel);
            }
        }

        private static bool ShouldShowSettlementFocusRow(int index)
        {
            return index == 0
                || index == 1
                || index == 2
                || index == 8
                || index == 9
                || index == 10;
        }

        private void DrawCatUpgradeChoices(P0RouteMapSurface surface)
        {
            if (surface.CatUpgradeChoices.Count == 0)
            {
                return;
            }

            GUILayout.Space(6f);
            GUILayout.Label("猫咪升级候选");
            GUILayout.Label("共享经验已满：先选择一名已加入猫的升级，再继续路线。");
            for (int i = 0; i < surface.CatUpgradeChoices.Count; i++)
            {
                P0RouteMapCatUpgradeChoiceCard choice = surface.CatUpgradeChoices[i];
                if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(
                    surface.UiShell.PrimaryButton,
                    choice.BuildButtonLabel(),
                    32f))
                {
                    ChooseCatUpgrade(choice.ChoiceId);
                }

                GUILayout.Label(choice.Summary);
            }

            if (surface.CanRerollCatUpgrade
                && P0ImGuiVisualAssetDrawer.DrawTexturedButton(
                    surface.UiShell.PrimaryButton,
                    "消耗 1 枚猫爪印章，刷新这批候选",
                    30f))
            {
                RerollCatUpgrade();
            }
        }

        private void DrawCurrentNodeOptions(P0RouteMapSurface surface)
        {
            if (surface.RouteOptions.Count == 0)
            {
                return;
            }

            RunProgressionState run = P0RunSession.EnsureAnyProgression();
            GUILayout.Space(6f);
            GUILayout.Label("路线选择");
            for (int i = 0; i < surface.RouteOptions.Count; i++)
            {
                P0RouteMapOptionCard option = surface.RouteOptions[i];
                if (IsRoutePanelNarrow)
                {
                    if (option.VisualAsset.HasAsset)
                    {
                        P0ImGuiVisualAssetDrawer.DrawInlineIcon(option.VisualAsset, P0ImGuiLayout.Scaled(28f));
                    }

                    GUI.enabled = !option.IsSelected;
                    if (GUILayout.Button(option.BuildButtonLabel(), GUILayout.Height(P0ImGuiLayout.ButtonHeight), GUILayout.ExpandWidth(true)))
                    {
                        run.Route.SelectCurrentNode(option.NodeId);
                        message = "已选择 " + option.Title + "。";
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    if (option.VisualAsset.HasAsset)
                    {
                        P0ImGuiVisualAssetDrawer.DrawInlineIcon(option.VisualAsset, P0ImGuiLayout.Scaled(28f));
                    }

                    GUI.enabled = !option.IsSelected;
                    if (GUILayout.Button(option.BuildButtonLabel(), GUILayout.Height(P0ImGuiLayout.ButtonHeight), GUILayout.ExpandWidth(true)))
                    {
                        run.Route.SelectCurrentNode(option.NodeId);
                        message = "已选择 " + option.Title + "。";
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUI.enabled = true;
        }

        private string BuildLayerText(RunRouteState route, int layerIndex)
        {
            IReadOnlyList<RouteNodeDefinition> options = route.Route.GetLayerOptions(layerIndex + 1);
            RouteNodeDefinition selectedNode = null;
            if (layerIndex < route.CompletedCount)
            {
                selectedNode = route.Completions[layerIndex].Node;
            }
            else if (layerIndex == route.CurrentNodeIndex && !route.IsComplete)
            {
                selectedNode = route.CurrentNode;
            }

            string prefix = layerIndex < route.CompletedCount ? "[x] " : layerIndex == route.CurrentNodeIndex ? "> " : "[ ] ";
            string text = prefix + (layerIndex + 1) + ". ";
            for (int i = 0; i < options.Count; i++)
            {
                if (i > 0)
                {
                    text += " | ";
                }

                if (selectedNode != null && selectedNode.Id == options[i].Id)
                {
                    text += "*";
                }

                text += P0RouteNodePresenter.Describe(options[i], P0RunSession.CurrentRun).BuildCompactLabel();
            }

            return text;
        }

        private string BuildCatVitalSummary(RunProgressionState run)
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

        private string BuildRosterSummary(RunProgressionState run)
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

        private void DrawCurrentChoices(P0RouteMapSurface surface)
        {
            if (surface.RewardChoices.Count == 0)
            {
                return;
            }

            GUILayout.Space(6f);
            GUILayout.Label(surface.CurrentNode.Title + " 的奖励选择");
            for (int i = 0; i < surface.RewardChoices.Count; i++)
            {
                P0RouteMapRewardChoiceCard choice = surface.RewardChoices[i];
                bool clicked;
                if (choice.ItemCardAsset.HasAsset)
                {
                    float cardWidth = Mathf.Min(P0ImGuiLayout.Scaled(216f), routePanelInnerWidth);
                    P0ImGuiVisualAssetDrawer.DrawGUILayoutTexture(choice.ItemCardAsset, cardWidth, P0ImGuiLayout.Scaled(90f));
                }

                if (choice.FrameAsset.HasAsset)
                {
                    clicked = P0ImGuiVisualAssetDrawer.DrawRouteChoiceCardButton(
                        choice.FrameAsset,
                        choice.VisualAsset,
                        choice.DetailBadgeAsset,
                        choice.BuildButtonLabel(),
                        58f);
                }
                else
                {
                    if (choice.VisualAsset.HasAsset)
                    {
                        P0ImGuiVisualAssetDrawer.DrawInlineIcon(choice.VisualAsset, P0ImGuiLayout.Scaled(28f));
                    }

                    if (choice.DetailBadgeAsset.HasAsset)
                    {
                        P0ImGuiVisualAssetDrawer.DrawInlineIcon(choice.DetailBadgeAsset, P0ImGuiLayout.Scaled(28f));
                    }

                    clicked = P0ImGuiVisualAssetDrawer.DrawTexturedButton(
                        surface.UiShell.PrimaryButton,
                        choice.BuildButtonLabel(),
                        32f);
                }

                if (clicked)
                {
                    ChooseCurrentReward(choice.ChoiceId);
                }
            }
        }

        private void ChooseCatUpgrade(string choiceId)
        {
            RunProgressionState run = P0RunSession.EnsureAnyProgression();
            if (!run.CatUpgrades.TrySelect(choiceId, run.Roster, out CatUpgradeCandidate selected))
            {
                message = "该猫咪升级候选已不可用。";
                return;
            }

            CatPresentation cat = P0CatPresenter.Describe(selected.CatId);
            message = BuildCatUpgradeResolvedMessage(cat, selected);
        }

        private static string BuildCatUpgradeResolvedMessage(CatPresentation cat, CatUpgradeCandidate selected)
        {
            string intent = string.IsNullOrWhiteSpace(selected.PlayerIntent)
                ? string.Empty
                : "（" + selected.PlayerIntent + "）";
            return "已升级 " + cat.ShortLabel + "：" + selected.DisplayName + intent + "。";
        }

        private void RerollCatUpgrade()
        {
            RunProgressionState run = P0RunSession.EnsureAnyProgression();
            if (!run.CatUpgrades.TryRerollWithPawStamp(run.EventItems))
            {
                message = "没有可用于本次升级刷新的猫爪印章。";
                return;
            }

            message = "已消耗 1 枚猫爪印章，刷新这批猫咪升级候选。";
        }

        private void ChooseCurrentReward(string choiceId)
        {
            RunProgressionState run = P0RunSession.EnsureAnyProgression();
            if (run.CatUpgrades.HasPendingUpgrade)
            {
                message = "请选择猫咪升级候选后继续。";
                return;
            }

            RouteNodeDefinition node = run.Route.CurrentNode;
            if (node == null)
            {
                return;
            }

            RouteRewardChoice choice = null;
            IReadOnlyList<RouteRewardChoice> choices = P0RouteRewardResolver.CreatePlaceholderChoices(node, run);
            for (int i = 0; i < choices.Count; i++)
            {
                if (choices[i].Id == choiceId)
                {
                    choice = choices[i];
                    break;
                }
            }

            if (choice == null)
            {
                message = "该奖励选项已不可用。";
                return;
            }

            RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(run, choice);
            message = resolution.Message;
        }

        private static P0RouteMapAction GetAction(P0RouteMapSurface surface, string actionId)
        {
            if (surface != null && surface.TryGetAction(actionId, out P0RouteMapAction action))
            {
                return action;
            }

            return new P0RouteMapAction(
                actionId,
                actionId,
                false,
                P0InputCommand.ContinueRoute,
                string.Empty,
                "缺失操作");
        }

        private void DrawWalletStrip(P0RouteMapSurface surface)
        {
            RunProgressionState run = P0RunSession.CurrentRun;
            if (run == null)
            {
                return;
            }

            if (IsRoutePanelNarrow)
            {
                GUILayout.BeginHorizontal();
                P0ImGuiVisualAssetDrawer.DrawInlineIcon(surface.UiShell.DreamShardRewardIcon, P0ImGuiLayout.Scaled(20f));
                GUILayout.Label("梦屑 " + run.Wallet.DreamShards, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                P0ImGuiVisualAssetDrawer.DrawInlineIcon(surface.UiShell.FishTreatRewardIcon, P0ImGuiLayout.Scaled(20f));
                GUILayout.Label("小鱼干 " + run.Wallet.FishTreats, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                P0ImGuiVisualAssetDrawer.DrawInlineIcon(surface.UiShell.DreamShardRewardIcon, P0ImGuiLayout.Scaled(20f));
                GUILayout.Label("梦屑 " + run.Wallet.DreamShards, GUILayout.Width(P0ImGuiLayout.Scaled(96f)));
                P0ImGuiVisualAssetDrawer.DrawInlineIcon(surface.UiShell.FishTreatRewardIcon, P0ImGuiLayout.Scaled(20f));
                GUILayout.Label("小鱼干 " + run.Wallet.FishTreats, GUILayout.Width(P0ImGuiLayout.Scaled(100f)));
                GUILayout.EndHorizontal();
            }
        }

        private bool IsRoutePanelNarrow => P0ImGuiLayout.ShouldStackControls(routePanelInnerWidth);

        private GUIStyle WrappedRouteLabel
        {
            get
            {
                if (wrappedRouteLabel == null)
                {
                    wrappedRouteLabel = new GUIStyle(GUI.skin.label)
                    {
                        wordWrap = true
                    };
                }

                return wrappedRouteLabel;
            }
        }

        private GUIStyle RouteSectionLabel
        {
            get
            {
                if (routeSectionLabel == null)
                {
                    routeSectionLabel = new GUIStyle(GUI.skin.label)
                    {
                        fontStyle = FontStyle.Bold,
                        wordWrap = true
                    };
                }

                return routeSectionLabel;
            }
        }

        private GUIStyle PanelContentStyle
        {
            get
            {
                if (panelContentStyle == null)
                {
                    panelContentStyle = new GUIStyle(GUIStyle.none);
                }

                panelContentStyle.padding = P0ImGuiLayout.Padding();
                return panelContentStyle;
            }
        }
    }
}
