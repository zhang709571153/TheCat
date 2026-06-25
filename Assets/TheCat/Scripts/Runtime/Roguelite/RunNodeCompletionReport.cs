using System;
using TheCat.Data;

namespace TheCat.Roguelite
{
    public sealed class RunNodeCompletionReport
    {
        public RunNodeCompletionReport(
            RouteNodeDefinition completedNode,
            NodeResult result,
            RouteBattleReward battleReward,
            RouteNodeDefinition nextNode,
            bool isRouteCleared,
            bool isRouteFailed)
        {
            if (result == NodeResult.Unknown)
            {
                throw new ArgumentException("Completion report requires a known result.", nameof(result));
            }

            CompletedNode = completedNode;
            Result = result;
            BattleReward = battleReward;
            NextNode = nextNode;
            IsRouteCleared = isRouteCleared;
            IsRouteFailed = isRouteFailed;
        }

        public RouteNodeDefinition CompletedNode { get; }

        public NodeResult Result { get; }

        public RouteBattleReward BattleReward { get; }

        public RouteNodeDefinition NextNode { get; }

        public bool IsRouteCleared { get; }

        public bool IsRouteFailed { get; }

        public string BuildSummary()
        {
            string nodeTitle = CompletedNode == null
                ? "未知节点"
                : P0RouteNodePresenter.Describe(CompletedNode).Title;
            string summary = FormatResult(Result) + " " + nodeTitle;
            if (BattleReward.HasReward)
            {
                summary += " 奖励 " + BattleReward.BuildSummary();
            }

            if (IsRouteCleared)
            {
                summary += " 路线通关";
            }
            else if (IsRouteFailed)
            {
                summary += " 路线失败";
            }
            else if (NextNode != null)
            {
                summary += " 下一节点 " + P0RouteNodePresenter.Describe(NextNode).Title;
            }

            return summary;
        }

        private static string FormatResult(NodeResult result)
        {
            switch (result)
            {
                case NodeResult.Success:
                    return "成功";
                case NodeResult.Failure:
                    return "失败";
                case NodeResult.Unknown:
                default:
                    return "未知";
            }
        }
    }
}
