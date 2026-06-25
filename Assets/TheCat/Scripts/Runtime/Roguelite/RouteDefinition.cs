using System;
using System.Collections.Generic;

namespace TheCat.Roguelite
{
    public sealed class RouteDefinition
    {
        public RouteDefinition(string id, IReadOnlyList<RouteNodeDefinition> nodes)
            : this(id, P0DreamMapCatalog.GetBedroomDreamMap(), BuildSingleNodeOptions(nodes))
        {
        }

        public RouteDefinition(string id, DreamMapDefinition dreamMap, IReadOnlyList<RouteNodeDefinition> nodes)
            : this(id, dreamMap, BuildSingleNodeOptions(nodes))
        {
        }

        public RouteDefinition(string id, IReadOnlyList<IReadOnlyList<RouteNodeDefinition>> layerOptions)
            : this(id, P0DreamMapCatalog.GetBedroomDreamMap(), layerOptions)
        {
        }

        public RouteDefinition(string id, DreamMapDefinition dreamMap, IReadOnlyList<IReadOnlyList<RouteNodeDefinition>> layerOptions)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Route id is required.", nameof(id));
            }

            if (layerOptions == null || layerOptions.Count == 0)
            {
                throw new ArgumentException("Route requires at least one layer.", nameof(layerOptions));
            }

            Id = id;
            DreamMap = dreamMap ?? throw new ArgumentNullException(nameof(dreamMap));
            LayerOptions = CopyLayerOptions(layerOptions);
            Nodes = BuildDefaultNodes(LayerOptions);
            ValidateLayerOptions();
        }

        public string Id { get; }

        public DreamMapDefinition DreamMap { get; }

        public IReadOnlyList<RouteNodeDefinition> Nodes { get; }

        public IReadOnlyList<IReadOnlyList<RouteNodeDefinition>> LayerOptions { get; }

        public int LayerCount => LayerOptions.Count;

        public IReadOnlyList<RouteNodeDefinition> GetLayerOptions(int layer)
        {
            if (layer <= 0 || layer > LayerOptions.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(layer), layer, "Layer is outside this route.");
            }

            return LayerOptions[layer - 1];
        }

        private void ValidateLayerOptions()
        {
            HashSet<string> nodeIds = new HashSet<string>();
            for (int layerIndex = 0; layerIndex < LayerOptions.Count; layerIndex++)
            {
                IReadOnlyList<RouteNodeDefinition> options = LayerOptions[layerIndex];
                if (options.Count == 0)
                {
                    throw new ArgumentException("Every route layer requires at least one node.", nameof(LayerOptions));
                }

                int expectedLayer = layerIndex + 1;
                for (int optionIndex = 0; optionIndex < options.Count; optionIndex++)
                {
                    RouteNodeDefinition node = options[optionIndex];
                    if (node.Layer != expectedLayer)
                    {
                        throw new ArgumentException("Route node layer does not match its layer option group.", nameof(LayerOptions));
                    }

                    if (!nodeIds.Add(node.Id))
                    {
                        throw new ArgumentException("Route node ids must be unique: " + node.Id, nameof(LayerOptions));
                    }
                }
            }
        }

        private static IReadOnlyList<IReadOnlyList<RouteNodeDefinition>> BuildSingleNodeOptions(IReadOnlyList<RouteNodeDefinition> nodes)
        {
            if (nodes == null || nodes.Count == 0)
            {
                throw new ArgumentException("Route requires at least one node.", nameof(nodes));
            }

            List<IReadOnlyList<RouteNodeDefinition>> options = new List<IReadOnlyList<RouteNodeDefinition>>();
            for (int i = 0; i < nodes.Count; i++)
            {
                options.Add(new[] { nodes[i] });
            }

            return options.AsReadOnly();
        }

        private static IReadOnlyList<IReadOnlyList<RouteNodeDefinition>> CopyLayerOptions(IReadOnlyList<IReadOnlyList<RouteNodeDefinition>> layerOptions)
        {
            List<IReadOnlyList<RouteNodeDefinition>> copy = new List<IReadOnlyList<RouteNodeDefinition>>();
            for (int i = 0; i < layerOptions.Count; i++)
            {
                if (layerOptions[i] == null)
                {
                    throw new ArgumentException("Route layer options cannot contain null layers.", nameof(layerOptions));
                }

                copy.Add(new List<RouteNodeDefinition>(layerOptions[i]).AsReadOnly());
            }

            return copy.AsReadOnly();
        }

        private static IReadOnlyList<RouteNodeDefinition> BuildDefaultNodes(IReadOnlyList<IReadOnlyList<RouteNodeDefinition>> layerOptions)
        {
            List<RouteNodeDefinition> defaults = new List<RouteNodeDefinition>();
            for (int i = 0; i < layerOptions.Count; i++)
            {
                if (layerOptions[i].Count == 0)
                {
                    continue;
                }

                defaults.Add(layerOptions[i][0]);
            }

            return defaults.AsReadOnly();
        }
    }
}
