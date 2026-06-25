using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Gameplay
{
    public static class P0MainMenuActionIds
    {
        public const string EnterCatRoom = "enter_cat_room";
        public const string StartSelectedRoute = "start_selected_route";
        public const string StartDefaultRoute = "start_default_route";
        public const string QuickBattle = "quick_battle";
        public const string ClearSession = "clear_session";
    }

    public enum P0MainMenuActionCategory
    {
        PlayerPrimary,
        DevRouteHelper,
        GrayboxBattleHelper,
        Utility
    }

    public readonly struct P0MainMenuStarterCard
    {
        public P0MainMenuStarterCard(
            string catId,
            string displayName,
            string title,
            string roleToken,
            string roleLabel,
            string roleHint,
            string authorityLabel,
            string attributeLabel,
            string signatureLine,
            string visualToken,
            string visualIdentity,
            bool isSelected,
            P0VisualAssetReference hudAvatar,
            string selectionStateLabel,
            string readyBadgeLabel,
            IReadOnlyList<string> skillLabels)
        {
            CatId = catId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Title = title ?? string.Empty;
            RoleToken = roleToken ?? string.Empty;
            RoleLabel = roleLabel ?? string.Empty;
            RoleHint = roleHint ?? string.Empty;
            AuthorityLabel = authorityLabel ?? string.Empty;
            AttributeLabel = attributeLabel ?? string.Empty;
            SignatureLine = signatureLine ?? string.Empty;
            VisualToken = visualToken ?? string.Empty;
            VisualIdentity = visualIdentity ?? string.Empty;
            IsSelected = isSelected;
            HudAvatar = hudAvatar;
            SelectionStateLabel = selectionStateLabel ?? string.Empty;
            ReadyBadgeLabel = readyBadgeLabel ?? string.Empty;
            SkillLabels = skillLabels ?? Array.Empty<string>();
        }

        public string CatId { get; }

        public string DisplayName { get; }

        public string Title { get; }

        public string RoleToken { get; }

        public string RoleLabel { get; }

        public string RoleHint { get; }

        public string AuthorityLabel { get; }

        public string AttributeLabel { get; }

        public string SignatureLine { get; }

        public string VisualToken { get; }

        public string VisualIdentity { get; }

        public bool IsSelected { get; }

        public P0VisualAssetReference HudAvatar { get; }

        public string SelectionStateLabel { get; }

        public string ReadyBadgeLabel { get; }

        public IReadOnlyList<string> SkillLabels { get; }

        public int SkillCount => SkillLabels.Count;

        public string BuildSelectionLabel()
        {
            return DisplayName
                + " | "
                + RoleLabel
                + " | "
                + RoleHint
                + " | "
                + AuthorityLabel
                + " / "
                + AttributeLabel;
        }

        public string BuildSkillPreview()
        {
            return SkillLabels.Count == 0 ? "无" : string.Join(" | ", SkillLabels);
        }

        public string BuildDesignPreview()
        {
            string line = string.IsNullOrWhiteSpace(SignatureLine) ? "暂无台词" : SignatureLine;
            string token = string.IsNullOrWhiteSpace(VisualToken) ? "暂无视觉符号" : VisualToken;
            string identity = string.IsNullOrWhiteSpace(VisualIdentity) ? "暂无视觉识别" : VisualIdentity;
            return token + " - " + identity + " - " + line;
        }

        public string BuildCharacterSelectSummary()
        {
            return ReadyBadgeLabel
                + " "
                + DisplayName
                + " "
                + RoleLabel
                + " "
                + RoleHint
                + " 技能 "
                + BuildSkillPreview();
        }

        public string BuildSummary()
        {
            return (IsSelected ? "已选择 " : "候补 ")
                + DisplayName
                + " "
                + RoleLabel
                + " 技能 "
                + SkillCount
                + " "
                + AuthorityLabel
                + "/"
                + AttributeLabel
                + " 视觉 "
                + VisualToken;
        }
    }

    public readonly struct P0MainMenuRouteRow
    {
        public P0MainMenuRouteRow(
            int layer,
            int optionCount,
            string previewLabel,
            bool hasBattle,
            bool hasBoss,
            string nodeTypeSummary)
        {
            Layer = Math.Max(0, layer);
            OptionCount = Math.Max(0, optionCount);
            PreviewLabel = previewLabel ?? string.Empty;
            HasBattle = hasBattle;
            HasBoss = hasBoss;
            NodeTypeSummary = nodeTypeSummary ?? string.Empty;
        }

        public int Layer { get; }

        public int OptionCount { get; }

        public string PreviewLabel { get; }

        public bool HasBattle { get; }

        public bool HasBoss { get; }

        public string NodeTypeSummary { get; }

        public string BuildPreviewLabel()
        {
            return Layer + ". " + PreviewLabel;
        }

        public string BuildSummary()
        {
            return "第 "
                + Layer
                + " 层 选项 "
                + OptionCount
                + " 战斗 "
                + HasBattle
                + " 首领 "
                + HasBoss
                + " "
                + NodeTypeSummary;
        }
    }

    public readonly struct P0MainMenuAction
    {
        public P0MainMenuAction(
            string actionId,
            string label,
            bool isEnabled,
            P0RunStartMode startMode,
            string targetSceneName,
            string detail,
            P0MainMenuActionCategory actionCategory)
        {
            ActionId = actionId ?? string.Empty;
            Label = label ?? string.Empty;
            IsEnabled = isEnabled;
            StartMode = startMode;
            TargetSceneName = targetSceneName ?? string.Empty;
            Detail = detail ?? string.Empty;
            ActionCategory = actionCategory;
        }

        public string ActionId { get; }

        public string Label { get; }

        public bool IsEnabled { get; }

        public P0RunStartMode StartMode { get; }

        public string TargetSceneName { get; }

        public string Detail { get; }

        public P0MainMenuActionCategory ActionCategory { get; }

        public string BuildSummary()
        {
            return Label
                + " 可用 "
                + IsEnabled
                + " 目标 "
                + TargetSceneName
                + " 分类 "
                + ActionCategory
                + " "
                + Detail;
        }
    }

    public sealed class P0MainMenuSurface
    {
        public P0MainMenuSurface(
            string title,
            string subtitle,
            string message,
            string playerPathLabel,
            string characterSelectLabel,
            string grayboxHelperLabel,
            P0UiShellSurface uiShell,
            IReadOnlyList<P0MainMenuStarterCard> starterCards,
            IReadOnlyList<P0MainMenuRouteRow> routeRows,
            IReadOnlyList<P0MainMenuAction> actions)
        {
            Title = title ?? string.Empty;
            Subtitle = subtitle ?? string.Empty;
            Message = message ?? string.Empty;
            PlayerPathLabel = playerPathLabel ?? string.Empty;
            CharacterSelectLabel = characterSelectLabel ?? string.Empty;
            GrayboxHelperLabel = grayboxHelperLabel ?? string.Empty;
            UiShell = uiShell ?? P0UiShellPresenter.BuildSurface();
            StarterCards = starterCards ?? Array.Empty<P0MainMenuStarterCard>();
            RouteRows = routeRows ?? Array.Empty<P0MainMenuRouteRow>();
            Actions = actions ?? Array.Empty<P0MainMenuAction>();
        }

        public string Title { get; }

        public string Subtitle { get; }

        public string Message { get; }

        public string PlayerPathLabel { get; }

        public string CharacterSelectLabel { get; }

        public string GrayboxHelperLabel { get; }

        public P0UiShellSurface UiShell { get; }

        public IReadOnlyList<P0MainMenuStarterCard> StarterCards { get; }

        public IReadOnlyList<P0MainMenuRouteRow> RouteRows { get; }

        public IReadOnlyList<P0MainMenuAction> Actions { get; }

        public int SelectedStarterCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < StarterCards.Count; i++)
                {
                    count += StarterCards[i].IsSelected ? 1 : 0;
                }

                return count;
            }
        }

        public bool HasAnyStarterSelected => SelectedStarterCount > 0;

        public bool TryGetAction(string actionId, out P0MainMenuAction action)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionId == actionId)
                {
                    action = Actions[i];
                    return true;
                }
            }

            action = default(P0MainMenuAction);
            return false;
        }
    }

    public static class P0MainMenuPresenter
    {
        public static P0MainMenuSurface BuildSurface(
            IReadOnlyList<CatDefinition> starterCats,
            IReadOnlyList<string> selectedStarterIds,
            RouteDefinition route,
            string message)
        {
            IReadOnlyList<P0MainMenuStarterCard> starterCards = BuildStarterCards(starterCats, selectedStarterIds);
            IReadOnlyList<P0MainMenuRouteRow> routeRows = BuildRouteRows(route);
            bool hasSelection = HasAnySelected(starterCards);

            return new P0MainMenuSurface(
                "猫眠所 / 梦境支配者",
                "选择今晚守梦的猫队",
                message,
                "主路径：猫房 -> 卧室梦境 -> 守护中心床",
                "初始猫队",
                "灰盒验证入口",
                P0UiShellPresenter.BuildSurface(),
                starterCards,
                routeRows,
                BuildActions(hasSelection));
        }

        public static bool HasP0MainMenuSurface(P0MainMenuSurface surface)
        {
            if (surface == null
                || string.IsNullOrWhiteSpace(surface.Title)
                || string.IsNullOrWhiteSpace(surface.Subtitle)
                || string.IsNullOrWhiteSpace(surface.PlayerPathLabel)
                || string.IsNullOrWhiteSpace(surface.CharacterSelectLabel)
                || string.IsNullOrWhiteSpace(surface.GrayboxHelperLabel)
                || !P0UiShellPresenter.HasP0UiShellSurface(surface.UiShell)
                || surface.StarterCards.Count < 3
                || surface.RouteRows.Count != P0RouteCatalog.CreateTenLayerRoute().LayerCount)
            {
                return false;
            }

            bool hasDefender = false;
            bool hasController = false;
            bool hasHealer = false;
            for (int i = 0; i < surface.StarterCards.Count; i++)
            {
                hasDefender |= surface.StarterCards[i].RoleToken == "DEF";
                hasController |= surface.StarterCards[i].RoleToken == "CTRL";
                hasHealer |= surface.StarterCards[i].RoleToken == "HEAL";
                if (surface.StarterCards[i].SkillCount <= 0
                    || string.IsNullOrWhiteSpace(surface.StarterCards[i].BuildSelectionLabel())
                    || string.IsNullOrWhiteSpace(surface.StarterCards[i].BuildCharacterSelectSummary())
                    || string.IsNullOrWhiteSpace(surface.StarterCards[i].SelectionStateLabel)
                    || string.IsNullOrWhiteSpace(surface.StarterCards[i].ReadyBadgeLabel)
                    || !surface.StarterCards[i].HudAvatar.HasAsset
                    || string.IsNullOrWhiteSpace(surface.StarterCards[i].SignatureLine)
                    || string.IsNullOrWhiteSpace(surface.StarterCards[i].VisualToken)
                    || string.IsNullOrWhiteSpace(surface.StarterCards[i].VisualIdentity))
                {
                    return false;
                }
            }

            bool hasBoss = false;
            for (int i = 0; i < surface.RouteRows.Count; i++)
            {
                hasBoss |= surface.RouteRows[i].HasBoss;
                if (string.IsNullOrWhiteSpace(surface.RouteRows[i].PreviewLabel))
                {
                    return false;
                }
            }

            return hasDefender
                && hasController
                && hasHealer
                && hasBoss
                && HasRequiredStartActions(surface);
        }

        public static string BuildCompactSummary(P0MainMenuSurface surface)
        {
            if (surface == null)
            {
                return "主菜单界面：缺失";
            }

            int bossRows = 0;
            int enabledActions = 0;
            for (int i = 0; i < surface.RouteRows.Count; i++)
            {
                bossRows += surface.RouteRows[i].HasBoss ? 1 : 0;
            }

            for (int i = 0; i < surface.Actions.Count; i++)
            {
                enabledActions += surface.Actions[i].IsEnabled ? 1 : 0;
            }

            return "主菜单界面：初始猫 "
                + surface.StarterCards.Count
                + " 已选 "
                + surface.SelectedStarterCount
                + " 路线 "
                + surface.RouteRows.Count
                + " 首领层 "
                + bossRows
                + " 操作 "
                + surface.Actions.Count
                + " 可用 "
                + enabledActions
                + " uiShell "
                + surface.UiShell.AssetCount
                + " | starters "
                + surface.StarterCards.Count
                + " selected "
                + surface.SelectedStarterCount
                + " route "
                + surface.RouteRows.Count
                + " actions "
                + surface.Actions.Count
                + " uiShell "
                + surface.UiShell.AssetCount;
        }

        private static IReadOnlyList<P0MainMenuStarterCard> BuildStarterCards(
            IReadOnlyList<CatDefinition> starterCats,
            IReadOnlyList<string> selectedStarterIds)
        {
            if (starterCats == null || starterCats.Count == 0)
            {
                return Array.Empty<P0MainMenuStarterCard>();
            }

            HashSet<string> selected = new HashSet<string>();
            if (selectedStarterIds != null)
            {
                for (int i = 0; i < selectedStarterIds.Count; i++)
                {
                    selected.Add(selectedStarterIds[i]);
                }
            }

            List<P0MainMenuStarterCard> cards = new List<P0MainMenuStarterCard>();
            for (int i = 0; i < starterCats.Count; i++)
            {
                CatDefinition cat = starterCats[i];
                CatPresentation presentation = P0CatPresenter.Describe(cat);
                cards.Add(new P0MainMenuStarterCard(
                    cat.Id,
                    presentation.DisplayName,
                    presentation.Title,
                    BuildRoleToken(cat.Role),
                    BuildRoleLabel(cat.Role),
                    presentation.RoleHint,
                    presentation.AuthorityLabel,
                    presentation.AttributeLabel,
                    presentation.SignatureLine,
                    presentation.VisualToken,
                    presentation.VisualIdentity,
                    selected.Contains(cat.Id),
                    P0VisualAssetCatalog.GetStarterCatHudAvatar(cat.Id),
                    selected.Contains(cat.Id) ? "已加入猫队" : "待选择",
                    selected.Contains(cat.Id) ? "已准备" : "未选择",
                    BuildSkillLabels(cat)));
            }

            return cards.AsReadOnly();
        }

        private static IReadOnlyList<string> BuildSkillLabels(CatDefinition cat)
        {
            if (cat == null || cat.SkillIds.Count == 0)
            {
                return Array.Empty<string>();
            }

            List<string> labels = new List<string>();
            for (int i = 0; i < cat.SkillIds.Count; i++)
            {
                labels.Add(P0SkillPresenter.Describe(cat.SkillIds[i]).DisplayName);
            }

            return labels.AsReadOnly();
        }

        private static IReadOnlyList<P0MainMenuRouteRow> BuildRouteRows(RouteDefinition route)
        {
            if (route == null)
            {
                return Array.Empty<P0MainMenuRouteRow>();
            }

            List<P0MainMenuRouteRow> rows = new List<P0MainMenuRouteRow>();
            for (int layer = 1; layer <= route.LayerCount; layer++)
            {
                IReadOnlyList<RouteNodeDefinition> options = route.GetLayerOptions(layer);
                rows.Add(BuildRouteRow(layer, options));
            }

            return rows.AsReadOnly();
        }

        private static P0MainMenuRouteRow BuildRouteRow(int layer, IReadOnlyList<RouteNodeDefinition> options)
        {
            if (options == null || options.Count == 0)
            {
                return new P0MainMenuRouteRow(layer, 0, "空层", false, false, "无");
            }

            bool hasBattle = false;
            bool hasBoss = false;
            List<string> labels = new List<string>();
            List<string> nodeTypes = new List<string>();
            for (int i = 0; i < options.Count; i++)
            {
                RouteNodeDefinition option = options[i];
                RouteNodePresentation presentation = P0RouteNodePresenter.Describe(option);
                labels.Add(presentation.BuildCompactLabel());
                hasBattle |= presentation.RequiresBattle;
                hasBoss |= option.NodeType == RouteNodeType.Boss;
                nodeTypes.Add(BuildNodeTypeLabel(option.NodeType));
            }

            return new P0MainMenuRouteRow(
                layer,
                options.Count,
                string.Join(" | ", labels),
                hasBattle,
                hasBoss,
                string.Join("/", nodeTypes));
        }

        private static IReadOnlyList<P0MainMenuAction> BuildActions(bool hasSelection)
        {
            return new[]
            {
                new P0MainMenuAction(
                    P0MainMenuActionIds.EnterCatRoom,
                    "进入猫房准备",
                    hasSelection,
                    P0RunStartMode.CatRoom,
                    P0SceneFlow.GetStartSceneName(P0RunStartMode.CatRoom),
                    "主路径：猫房 -> 卧室梦境 -> 守护中心床",
                    P0MainMenuActionCategory.PlayerPrimary),
                new P0MainMenuAction(
                    P0MainMenuActionIds.StartSelectedRoute,
                    "灰盒：所选猫队查看路线",
                    hasSelection,
                    P0RunStartMode.RouteMap,
                    P0SceneFlow.GetStartSceneName(P0RunStartMode.RouteMap),
                    "开发辅助：跳过猫房进入路线图",
                    P0MainMenuActionCategory.DevRouteHelper),
                new P0MainMenuAction(
                    P0MainMenuActionIds.StartDefaultRoute,
                    "灰盒：默认三猫查看路线",
                    true,
                    P0RunStartMode.RouteMap,
                    P0SceneFlow.GetStartSceneName(P0RunStartMode.RouteMap),
                    "开发辅助：恢复默认猫队并查看路线图",
                    P0MainMenuActionCategory.DevRouteHelper),
                new P0MainMenuAction(
                    P0MainMenuActionIds.QuickBattle,
                    "调试：快速卧室战斗",
                    hasSelection,
                    P0RunStartMode.QuickBattle,
                    P0SceneFlow.GetStartSceneName(P0RunStartMode.QuickBattle),
                    "跳过猫房和路线图进入卧室战斗",
                    P0MainMenuActionCategory.GrayboxBattleHelper),
                new P0MainMenuAction(
                    P0MainMenuActionIds.ClearSession,
                    "清除当前进度",
                    true,
                    P0RunStartMode.RouteMap,
                    string.Empty,
                    "清除当前路线会话",
                    P0MainMenuActionCategory.Utility)
            };
        }

        private static bool HasRequiredStartActions(P0MainMenuSurface surface)
        {
            return HasAction(surface, P0MainMenuActionIds.StartSelectedRoute, P0SceneFlow.RouteMapSceneName)
                && HasAction(surface, P0MainMenuActionIds.StartDefaultRoute, P0SceneFlow.RouteMapSceneName)
                && HasAction(surface, P0MainMenuActionIds.EnterCatRoom, P0SceneFlow.CatRoomSceneName)
                && HasAction(surface, P0MainMenuActionIds.QuickBattle, P0SceneFlow.GrayboxBattleSceneName)
                && HasAction(surface, P0MainMenuActionIds.ClearSession, string.Empty);
        }

        private static bool HasAction(P0MainMenuSurface surface, string actionId, string targetSceneName)
        {
            if (!surface.TryGetAction(actionId, out P0MainMenuAction action))
            {
                return false;
            }

            return action.Label.Length > 0 && action.TargetSceneName == targetSceneName;
        }

        private static bool HasAnySelected(IReadOnlyList<P0MainMenuStarterCard> starterCards)
        {
            if (starterCards == null)
            {
                return false;
            }

            for (int i = 0; i < starterCards.Count; i++)
            {
                if (starterCards[i].IsSelected)
                {
                    return true;
                }
            }

            return false;
        }

        private static string BuildRoleToken(CatRole role)
        {
            switch (role)
            {
                case CatRole.Defender:
                    return "DEF";
                case CatRole.Controller:
                    return "CTRL";
                case CatRole.Healer:
                    return "HEAL";
                case CatRole.Assassin:
                    return "ASN";
                case CatRole.Commander:
                    return "CMD";
                case CatRole.Counter:
                    return "CTR";
                default:
                    return "CAT";
            }
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

        private static string BuildRoleLabel(CatRole role)
        {
            switch (role)
            {
                case CatRole.Defender:
                    return "防守";
                case CatRole.Controller:
                    return "控场";
                case CatRole.Healer:
                    return "治疗";
                case CatRole.Assassin:
                    return "刺杀";
                case CatRole.Commander:
                    return "指挥";
                case CatRole.Counter:
                    return "反制";
                default:
                    return "专精";
            }
        }
    }
}
