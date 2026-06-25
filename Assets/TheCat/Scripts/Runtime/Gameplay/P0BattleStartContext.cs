using System;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Gameplay
{
    public readonly struct P0BattleStartContext
    {
        public P0BattleStartContext(
            RouteNodeDefinition node,
            DreamMapDefinition dreamMap,
            WaveDefinition wave,
            bool isRouteBattle,
            string title,
            string startMessage)
        {
            Node = node;
            DreamMap = dreamMap ?? P0DreamMapCatalog.GetBedroomDreamMap();
            Wave = wave ?? throw new ArgumentNullException(nameof(wave));
            IsRouteBattle = isRouteBattle;
            Title = title ?? string.Empty;
            StartMessage = startMessage ?? string.Empty;
        }

        public RouteNodeDefinition Node { get; }

        public DreamMapDefinition DreamMap { get; }

        public WaveDefinition Wave { get; }

        public bool HasNode => Node != null;

        public bool IsRouteBattle { get; }

        public bool ShouldCompleteRouteNode => IsRouteBattle && Node != null;

        public bool ShouldPersistRunState => ShouldCompleteRouteNode;

        public string Title { get; }

        public string StartMessage { get; }

        public static P0BattleStartContext Create(RunProgressionState run)
        {
            if (run == null)
            {
                throw new ArgumentNullException(nameof(run));
            }

            RouteNodeDefinition node = run.Route.CurrentNode;
            DreamMapDefinition dreamMap = run.DreamMap ?? P0DreamMapCatalog.GetBedroomDreamMap();
            if (node == null)
            {
                return CreateStandalone(
                    null,
                    dreamMap,
                    "独立灰盒战斗",
                    "独立战斗已开始。");
            }

            if (!RouteNodeResolver.RequiresBattle(node.NodeType))
            {
                RouteNodePresentation presentation = P0RouteNodePresenter.Describe(node, run);
                return CreateStandalone(
                    node,
                    dreamMap,
                    "独立灰盒战斗",
                    "当前路线节点 " + presentation.Title + " 不是战斗节点，已开始独立灰盒战斗。");
            }

            WaveDefinition wave = P0PrototypeCatalog.CreateWaveForContentId(node.ContentId);
            string title = P0RouteNodePresenter.Describe(node, run).Title;
            return new P0BattleStartContext(
                node,
                dreamMap,
                wave,
                true,
                title,
                dreamMap.DisplayName + "，" + title + " 已开始。");
        }

        public string BuildSummary()
        {
            return (IsRouteBattle ? "Route battle" : "Standalone battle")
                + " title " + Title
                + " map " + DreamMap.Id
                + " target " + DreamMap.DefenseTargetLabel
                + " wave " + Wave.Id
                + " completeRoute " + ShouldCompleteRouteNode
                + " persistRun " + ShouldPersistRunState;
        }

        private static P0BattleStartContext CreateStandalone(
            RouteNodeDefinition node,
            DreamMapDefinition dreamMap,
            string title,
            string startMessage)
        {
            return new P0BattleStartContext(
                node,
                dreamMap,
                P0PrototypeCatalog.CreateLayerOneWave(),
                false,
                title,
                startMessage);
        }
    }
}
