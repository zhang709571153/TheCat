using System.Collections.Generic;
using TheCat.Data;
using TheCat.Data.Catalogs;

namespace TheCat.Roguelite
{
    public static class P0RunSession
    {
        private static readonly string[] DefaultStarterCatIds =
        {
            P0PrototypeCatalog.SaibanId,
            P0PrototypeCatalog.NephthysId,
            P0PrototypeCatalog.SuzuneId
        };

        public static RunProgressionState CurrentRun { get; private set; }

        public static RunRouteState CurrentRoute => CurrentRun == null ? null : CurrentRun.Route;

        public static RunNodeCompletionReport LastCompletionReport { get; private set; }

        public static bool HasActiveRun => CurrentRun != null && !CurrentRun.Route.IsComplete;

        public static RunRouteState StartNewRun()
        {
            return StartNewRun(DefaultStarterCatIds);
        }

        public static RunRouteState StartNewRun(DreamMapDefinition dreamMap)
        {
            return StartNewRun(DefaultStarterCatIds, dreamMap);
        }

        public static RunRouteState StartNewRun(IEnumerable<string> starterCatIds)
        {
            return StartNewRun(starterCatIds, P0DreamMapCatalog.GetBedroomDreamMap());
        }

        public static RunRouteState StartNewRun(IEnumerable<string> starterCatIds, DreamMapDefinition dreamMap)
        {
            CurrentRun = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(dreamMap ?? P0DreamMapCatalog.GetBedroomDreamMap()),
                NormalizeStarterCatIds(starterCatIds));
            LastCompletionReport = null;

            return CurrentRun.Route;
        }

        public static RunRouteState EnsureRun()
        {
            return HasActiveRun ? CurrentRoute : StartNewRun();
        }

        public static RunRouteState EnsureAnyRun()
        {
            return CurrentRun == null ? StartNewRun() : CurrentRun.Route;
        }

        public static RunProgressionState EnsureProgression()
        {
            if (HasActiveRun)
            {
                return CurrentRun;
            }

            StartNewRun();
            return CurrentRun;
        }

        public static RunProgressionState EnsureAnyProgression()
        {
            if (CurrentRun == null)
            {
                StartNewRun();
            }

            return CurrentRun;
        }

        public static RunRouteState EnsureBedroomDreamRun()
        {
            return EnsureDreamRun(P0DreamMapCatalog.GetBedroomDreamMap());
        }

        public static RunRouteState EnsureEgyptDreamRun()
        {
            return EnsureDreamRun(P0DreamMapCatalog.GetEgyptDreamMap());
        }

        public static RunRouteState EnsureDreamRun(DreamMapDefinition dreamMap)
        {
            DreamMapDefinition targetMap = dreamMap ?? P0DreamMapCatalog.GetBedroomDreamMap();
            if (HasActiveRun && CurrentRun.DreamMap.Id == targetMap.Id)
            {
                return CurrentRoute;
            }

            IReadOnlyList<string> starterCatIds = CurrentRun == null
                ? CreateDefaultStarterCatIds()
                : CurrentRun.Roster.CatIds;
            return StartNewRun(starterCatIds, targetMap);
        }

        public static RunNodeCompletionReport CompleteCurrentNode(NodeResult result)
        {
            RunProgressionState run = EnsureProgression();
            RouteNodeDefinition node = run.Route.CurrentNode;
            RouteBattleReward reward = RouteBattleReward.None;
            run.Route.CompleteCurrentNode(result);
            if (result == NodeResult.Success && node != null && RouteNodeResolver.RequiresBattle(node.NodeType))
            {
                reward = P0RouteRewardResolver.ApplyBattleReward(node, run);
            }

            LastCompletionReport = new RunNodeCompletionReport(
                node,
                result,
                reward,
                run.Route.CurrentNode,
                run.Route.IsCleared,
                run.Route.IsFailed);

            return LastCompletionReport;
        }

        public static void Clear()
        {
            CurrentRun = null;
            LastCompletionReport = null;
        }

        public static IReadOnlyList<string> CreateDefaultStarterCatIds()
        {
            return new List<string>(DefaultStarterCatIds).AsReadOnly();
        }

        public static IReadOnlyList<string> NormalizeStarterCatIds(IEnumerable<string> starterCatIds)
        {
            List<string> normalized = new List<string>();
            HashSet<string> seen = new HashSet<string>();
            if (starterCatIds != null)
            {
                foreach (string catId in starterCatIds)
                {
                    if (!IsP0StarterCat(catId) || !seen.Add(catId))
                    {
                        continue;
                    }

                    normalized.Add(catId);
                }
            }

            if (normalized.Count == 0)
            {
                normalized.AddRange(DefaultStarterCatIds);
            }

            return normalized.AsReadOnly();
        }

        private static bool IsP0StarterCat(string catId)
        {
            return catId == P0PrototypeCatalog.SaibanId
                || catId == P0PrototypeCatalog.NephthysId
                || catId == P0PrototypeCatalog.SuzuneId;
        }
    }
}
