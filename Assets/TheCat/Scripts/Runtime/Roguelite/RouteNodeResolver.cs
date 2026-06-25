using System;
using TheCat.Data;

namespace TheCat.Roguelite
{
    public static class RouteNodeResolver
    {
        public static RouteNodeResolution ResolveCurrentNode(RunRouteState route)
        {
            if (route == null)
            {
                throw new ArgumentNullException(nameof(route));
            }

            RouteNodeDefinition node = route.CurrentNode;
            if (node == null)
            {
                return route.IsFailed
                    ? new RouteNodeResolution(RouteNodeResolutionType.RouteFailed, null, "路线失败。")
                    : new RouteNodeResolution(RouteNodeResolutionType.RouteCleared, null, "路线已通关。");
            }

            if (RequiresBattle(node.NodeType))
            {
                return new RouteNodeResolution(RouteNodeResolutionType.NeedsBattle, node, "第 " + node.Layer + " 层战斗节点已就绪。");
            }

            route.CompleteCurrentNode(NodeResult.Success);
            return new RouteNodeResolution(RouteNodeResolutionType.PlaceholderResolved, node, "已结算第 " + node.Layer + " 层 " + node.NodeType + "。");
        }

        public static RouteNodeResolution ResolveCurrentNode(RunProgressionState run)
        {
            if (run == null)
            {
                throw new ArgumentNullException(nameof(run));
            }

            RunRouteState route = run.Route;
            RouteNodeDefinition node = route.CurrentNode;
            if (node == null)
            {
                return route.IsFailed
                    ? new RouteNodeResolution(RouteNodeResolutionType.RouteFailed, null, "路线失败。")
                    : new RouteNodeResolution(RouteNodeResolutionType.RouteCleared, null, "路线已通关。");
            }

            if (RequiresBattle(node.NodeType))
            {
                return new RouteNodeResolution(RouteNodeResolutionType.NeedsBattle, node, "第 " + node.Layer + " 层战斗节点已就绪。");
            }

            P0RouteRewardResolver.ApplyPlaceholderReward(node, run);
            route.CompleteCurrentNode(NodeResult.Success);
            return new RouteNodeResolution(RouteNodeResolutionType.PlaceholderResolved, node, "已结算第 " + node.Layer + " 层 " + node.NodeType + "。");
        }

        public static RouteNodeResolution ResolveCurrentNode(RunProgressionState run, RouteRewardChoice choice)
        {
            if (run == null)
            {
                throw new ArgumentNullException(nameof(run));
            }

            RunRouteState route = run.Route;
            RouteNodeDefinition node = route.CurrentNode;
            if (node == null)
            {
                return route.IsFailed
                    ? new RouteNodeResolution(RouteNodeResolutionType.RouteFailed, null, "路线失败。")
                    : new RouteNodeResolution(RouteNodeResolutionType.RouteCleared, null, "路线已通关。");
            }

            if (RequiresBattle(node.NodeType))
            {
                return new RouteNodeResolution(RouteNodeResolutionType.NeedsBattle, node, "第 " + node.Layer + " 层战斗节点已就绪。");
            }

            if (choice == null)
            {
                return new RouteNodeResolution(RouteNodeResolutionType.ChoiceRequired, node, "请选择第 " + node.Layer + " 层奖励。");
            }

            if (!P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice))
            {
                return new RouteNodeResolution(RouteNodeResolutionType.InvalidChoice, node, "无法应用奖励选择：" + choice.DisplayName + "。");
            }

            route.CompleteCurrentNode(NodeResult.Success);
            return new RouteNodeResolution(RouteNodeResolutionType.PlaceholderResolved, node, "第 " + node.Layer + " 层已选择 " + choice.DisplayName + "。");
        }

        public static bool RequiresBattle(RouteNodeType nodeType)
        {
            return nodeType == RouteNodeType.Defense
                || nodeType == RouteNodeType.Elite
                || nodeType == RouteNodeType.Boss;
        }
    }
}
