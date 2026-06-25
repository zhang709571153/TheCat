using System.Collections.Generic;
using TheCat.Data.Catalogs;

namespace TheCat.Roguelite
{
    public static class P0RouteNodePresenter
    {
        public static RouteNodePresentation Describe(RouteNodeDefinition node, RunProgressionState run = null)
        {
            if (node == null)
            {
                return new RouteNodePresentation(
                    "路线完成",
                    string.Empty,
                    string.Empty,
                    "当前没有可用节点。",
                    requiresBattle: false);
            }

            switch (node.NodeType)
            {
                case RouteNodeType.Defense:
                    return DescribeDefense(node);
                case RouteNodeType.Elite:
                    return DescribeElite(node);
                case RouteNodeType.Boss:
                    return new RouteNodePresentation(
                        "来电暴君首领",
                        "召唤与投掷",
                        P0RouteRewardResolver.PreviewBattleReward(node).BuildSummary(),
                        "最终来电梦魇：在召唤、投掷和直接压床中守住床。",
                        requiresBattle: true,
                        P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Boss));
                case RouteNodeType.DreamEvent:
                    return DescribeDreamEvent(node);
                case RouteNodeType.Partner:
                    return new RouteNodePresentation(
                        "伙伴：影丸",
                        "安全招募",
                        run != null && run.Roster.HasCat(P0RouteRewardResolver.PreviewPartnerId)
                            ? "重复伙伴转资源"
                            : "预览伙伴 + 小鱼干",
                        "邀请第一只预览伙伴加入队伍，重复时转化为资源。",
                        requiresBattle: false,
                        P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Partner));
                case RouteNodeType.Shop:
                    return new RouteNodePresentation(
                        "午夜猫粮商店",
                        "花费小鱼干",
                        BuildChoiceCountRewardHint(node, run, "资源与祝福"),
                        "购买床修补、猫砂缓解、猫粮、权柄祝福或祝福升级。",
                        requiresBattle: false,
                        P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Shop));
                case RouteNodeType.BlessingOffering:
                    return new RouteNodePresentation(
                        "权柄供奉",
                        "构筑选择",
                        BuildChoiceCountRewardHint(node, run, "权柄祝福"),
                        "选择或加深与初始猫绑定的 P0 权柄祝福。",
                        requiresBattle: false,
                        P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.BlessingOffering));
                case RouteNodeType.RestNest:
                    return new RouteNodePresentation(
                        "休整猫窝",
                        "安全回复",
                        "睡眠/屎意/饱肚/猫咪生命",
                        "下一波梦境压力前的安静回复节点。",
                        requiresBattle: false,
                        P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.RestNest));
                default:
                    return new RouteNodePresentation(
                        node.NodeType.ToString(),
                        "未知",
                        node.ContentId,
                        "该节点尚未配置 P0 展示规则。",
                        RouteNodeResolver.RequiresBattle(node.NodeType));
            }
        }

        public static string BuildRouteChoiceLabel(RouteNodeDefinition node, RunProgressionState run, bool isSelected)
        {
            RouteNodePresentation presentation = Describe(node, run);
            string prefix = isSelected ? "> " : string.Empty;
            return prefix + node.Layer + ". " + presentation.BuildCompactLabel();
        }

        private static RouteNodePresentation DescribeDefense(RouteNodeDefinition node)
        {
            if (node.ContentId == P0RouteCatalog.LayerOneDefenseId)
            {
                return new RouteNodePresentation(
                    "卧室门槛",
                    "低压床压力",
                    P0RouteRewardResolver.PreviewBattleReward(node).BuildSummary(),
                    "对抗黑泥梦魇的教程守床战。",
                    requiresBattle: true,
                    P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Defense));
            }

            return new RouteNodePresentation(
                "梦轨守床",
                "火车路线压力",
                P0RouteRewardResolver.PreviewBattleReward(node).BuildSummary(),
                "中段守床波次，包含梦轨小火车冲锋与远程压力。",
                requiresBattle: true,
                P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Defense));
        }

        private static RouteNodePresentation DescribeElite(RouteNodeDefinition node)
        {
            switch (node.ContentId)
            {
                case "elite_cold_light_shadow":
                    return new RouteNodePresentation(
                        "冷光精英",
                        "远程压力",
                        P0RouteRewardResolver.PreviewBattleReward(node).BuildSummary(),
                        "考验控场、护盾时机与猫咪切换的远程灯影。",
                        requiresBattle: true,
                        P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Elite));
                case "elite_red_eye_alarm":
                    return new RouteNodePresentation(
                        "红眼闹铃精英",
                        "闹铃加飞虫",
                        P0RouteRewardResolver.PreviewBattleReward(node).BuildSummary(),
                        "后段精英，使用远程骚扰和未读红点小飞虫。",
                        requiresBattle: true,
                        P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Elite));
                case "elite_falling_dream_teddy":
                    return new RouteNodePresentation(
                        "坠梦玩具熊",
                        "跳砸精英",
                        P0RouteRewardResolver.PreviewBattleReward(node).BuildSummary(),
                        "沉重玩具熊跳砸，并由火车和飞虫支援。",
                        requiresBattle: true,
                        P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Elite));
                default:
                    return new RouteNodePresentation(
                        "精英梦境入侵者",
                        "精英压力",
                        P0RouteRewardResolver.PreviewBattleReward(node).BuildSummary(),
                        "尚未细分的精英路线内容。",
                        requiresBattle: true,
                        P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Elite));
            }
        }

        private static RouteNodePresentation DescribeDreamEvent(RouteNodeDefinition node)
        {
            if (node.ContentId == "event_unread_red_dot_rain")
            {
                return new RouteNodePresentation(
                    "未读红点雨",
                    "事件选择",
                    "小鱼干/睡眠/风险增益",
                    "在资源、睡眠稳定或高风险下一战修正之间选择。",
                    requiresBattle: false,
                    P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.DreamEvent));
            }

            return new RouteNodePresentation(
                "柔雨窗台",
                "事件选择",
                "小鱼干/睡眠/风险增益",
                "压力抬升前，一个让玩家调整资源的平缓梦境事件。",
                requiresBattle: false,
                P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.DreamEvent));
        }

        private static string BuildChoiceCountRewardHint(RouteNodeDefinition node, RunProgressionState run, string fallback)
        {
            if (node == null || run == null)
            {
                return fallback;
            }

            IReadOnlyList<RouteRewardChoice> choices = P0RouteRewardResolver.CreatePlaceholderChoices(node, run);
            if (choices.Count == 0)
            {
                return fallback;
            }

            return choices.Count == 1 ? choices[0].DisplayName : choices.Count + " 个选择";
        }
    }
}
