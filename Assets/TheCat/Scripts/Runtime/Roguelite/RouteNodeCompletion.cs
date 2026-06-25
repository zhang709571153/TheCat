using System;
using TheCat.Data;

namespace TheCat.Roguelite
{
    public readonly struct RouteNodeCompletion
    {
        public RouteNodeCompletion(RouteNodeDefinition node, NodeResult result)
        {
            if (result == NodeResult.Unknown)
            {
                throw new ArgumentException("Completion result must be success or failure.", nameof(result));
            }

            Node = node ?? throw new ArgumentNullException(nameof(node));
            Result = result;
        }

        public RouteNodeDefinition Node { get; }

        public NodeResult Result { get; }
    }
}
