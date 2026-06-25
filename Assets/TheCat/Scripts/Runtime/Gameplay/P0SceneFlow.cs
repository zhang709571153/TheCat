using System;
using TheCat.Combat;

namespace TheCat.Gameplay
{
    public static class P0SceneFlow
    {
        public const string MainMenuSceneName = "P0MainMenu";
        public const string CatRoomSceneName = "P0CatRoom";
        public const string RouteMapSceneName = "P0RouteMap";
        public const string GrayboxBattleSceneName = "P0GrayboxBattle";

        public static string GetStartSceneName(P0RunStartMode startMode)
        {
            switch (startMode)
            {
                case P0RunStartMode.RouteMap:
                    return RouteMapSceneName;
                case P0RunStartMode.QuickBattle:
                    return GrayboxBattleSceneName;
                case P0RunStartMode.CatRoom:
                    return CatRoomSceneName;
                default:
                    throw new ArgumentOutOfRangeException(nameof(startMode), startMode, "Unknown P0 start mode.");
            }
        }

        public static string GetCatRoomReturnSceneName(P0CatRoomReturnReason returnReason)
        {
            switch (returnReason)
            {
                case P0CatRoomReturnReason.FreshStart:
                case P0CatRoomReturnReason.BattleVictory:
                case P0CatRoomReturnReason.BattleDefeat:
                case P0CatRoomReturnReason.RouteCleared:
                case P0CatRoomReturnReason.RouteFailed:
                    return CatRoomSceneName;
                default:
                    throw new ArgumentOutOfRangeException(nameof(returnReason), returnReason, "Unknown cat-room return reason.");
            }
        }

        public static string GetPostBattleSceneName(BattleOutcome outcome)
        {
            switch (outcome)
            {
                case BattleOutcome.Victory:
                case BattleOutcome.Defeat:
                    return RouteMapSceneName;
                case BattleOutcome.InProgress:
                    throw new InvalidOperationException("Cannot continue route while battle is still in progress.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(outcome), outcome, "Unknown battle outcome.");
            }
        }
    }
}
