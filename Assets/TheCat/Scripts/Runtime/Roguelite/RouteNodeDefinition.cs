using System;

namespace TheCat.Roguelite
{
    public sealed class RouteNodeDefinition
    {
        public RouteNodeDefinition(int layer, string id, RouteNodeType nodeType, string contentId)
        {
            if (layer <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(layer), layer, "Layer must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Node id is required.", nameof(id));
            }

            Layer = layer;
            Id = id;
            NodeType = nodeType;
            ContentId = contentId ?? string.Empty;
        }

        public int Layer { get; }

        public string Id { get; }

        public RouteNodeType NodeType { get; }

        public string ContentId { get; }
    }
}
