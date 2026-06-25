using System;
using System.Collections.Generic;
using TheCat.Data;

namespace TheCat.Roguelite
{
    public sealed class RunRouteState
    {
        private readonly List<RouteNodeCompletion> completions = new List<RouteNodeCompletion>();
        private readonly List<RouteNodeDefinition> selectedNodes = new List<RouteNodeDefinition>();

        public RunRouteState(RouteDefinition route)
        {
            Route = route ?? throw new ArgumentNullException(nameof(route));
            for (int i = 0; i < Route.LayerCount; i++)
            {
                selectedNodes.Add(null);
            }
        }

        public RouteDefinition Route { get; }

        public int CurrentNodeIndex { get; private set; }

        public IReadOnlyList<RouteNodeCompletion> Completions => completions.AsReadOnly();

        public bool IsFailed { get; private set; }

        public bool IsCleared => !IsFailed && CurrentNodeIndex >= Route.Nodes.Count;

        public bool IsComplete => IsFailed || IsCleared;

        public int CurrentLayer => IsComplete ? 0 : CurrentNodeIndex + 1;

        public IReadOnlyList<RouteNodeDefinition> CurrentLayerOptions => IsComplete
            ? Array.Empty<RouteNodeDefinition>()
            : Route.GetLayerOptions(CurrentLayer);

        public RouteNodeDefinition CurrentNode
        {
            get
            {
                if (IsComplete)
                {
                    return null;
                }

                RouteNodeDefinition selected = selectedNodes[CurrentNodeIndex];
                return selected ?? CurrentLayerOptions[0];
            }
        }

        public bool IsCurrentNodeExplicitlySelected => !IsComplete && selectedNodes[CurrentNodeIndex] != null;

        public int CompletedCount => completions.Count;

        public bool SelectCurrentNode(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                throw new ArgumentException("Node id is required.", nameof(nodeId));
            }

            if (IsComplete)
            {
                return false;
            }

            IReadOnlyList<RouteNodeDefinition> options = CurrentLayerOptions;
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].Id == nodeId)
                {
                    selectedNodes[CurrentNodeIndex] = options[i];
                    return true;
                }
            }

            return false;
        }

        public void CompleteCurrentNode(NodeResult result)
        {
            if (result == NodeResult.Unknown)
            {
                throw new ArgumentException("Route node completion requires success or failure.", nameof(result));
            }

            if (IsComplete)
            {
                throw new InvalidOperationException("Cannot complete a node after the route has ended.");
            }

            RouteNodeDefinition node = CurrentNode;
            completions.Add(new RouteNodeCompletion(node, result));
            if (result == NodeResult.Failure)
            {
                IsFailed = true;
                return;
            }

            CurrentNodeIndex++;
        }
    }
}
