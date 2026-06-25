using TheCat.Combat;
using TheCat.Roguelite;

namespace TheCat.Gameplay
{
    public static class P0CatRoomSession
    {
        public static P0CatRoomState CurrentState { get; private set; } = P0CatRoomState.CreateFreshStart();

        public static void RecordFreshStart(RunProgressionState run)
        {
            CurrentState = BuildState(P0CatRoomReturnReason.FreshStart, "今晚从猫房出发。", run);
        }

        public static void RecordBattleReturn(
            BattleOutcome outcome,
            RunProgressionState run,
            RunNodeCompletionReport completionReport)
        {
            P0CatRoomReturnReason reason = BuildBattleReturnReason(outcome, run);
            string detail = completionReport == null ? string.Empty : completionReport.BuildSummary();
            CurrentState = BuildState(reason, detail, run);
        }

        public static void RecordRouteReturn(RunProgressionState run)
        {
            P0CatRoomReturnReason reason = BuildRouteReturnReason(run);
            CurrentState = BuildState(reason, "路线结算已回收到猫房。", run);
        }

        public static P0CatRoomState BuildState(
            P0CatRoomReturnReason returnReason,
            string detail,
            RunProgressionState run)
        {
            bool hasActiveRun = run != null && !run.Route.IsComplete;
            int fishTreats = run == null ? 0 : run.Wallet.FishTreats;
            int dreamShards = run == null ? 0 : run.Wallet.DreamShards;
            switch (returnReason)
            {
                case P0CatRoomReturnReason.BattleVictory:
                case P0CatRoomReturnReason.RouteCleared:
                    return new P0CatRoomState(0.78f, 0.86f, 0.65f, 0.25f, fishTreats, dreamShards, hasActiveRun, true, returnReason, detail);
                case P0CatRoomReturnReason.BattleDefeat:
                case P0CatRoomReturnReason.RouteFailed:
                    return new P0CatRoomState(0.28f, 0.55f, 0.32f, 0.58f, fishTreats, dreamShards, hasActiveRun, true, returnReason, detail);
                default:
                    return new P0CatRoomState(1f, 1f, 0.85f, 0.1f, fishTreats, dreamShards, hasActiveRun, true, P0CatRoomReturnReason.FreshStart, detail);
            }
        }

        private static P0CatRoomReturnReason BuildBattleReturnReason(BattleOutcome outcome, RunProgressionState run)
        {
            if (run != null)
            {
                if (run.Route.IsCleared)
                {
                    return P0CatRoomReturnReason.RouteCleared;
                }

                if (run.Route.IsFailed)
                {
                    return P0CatRoomReturnReason.RouteFailed;
                }
            }

            return outcome == BattleOutcome.Victory
                ? P0CatRoomReturnReason.BattleVictory
                : P0CatRoomReturnReason.BattleDefeat;
        }

        private static P0CatRoomReturnReason BuildRouteReturnReason(RunProgressionState run)
        {
            if (run != null && run.Route.IsCleared)
            {
                return P0CatRoomReturnReason.RouteCleared;
            }

            if (run != null && run.Route.IsFailed)
            {
                return P0CatRoomReturnReason.RouteFailed;
            }

            return P0CatRoomReturnReason.FreshStart;
        }
    }
}
