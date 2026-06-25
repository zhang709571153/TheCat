using TheCat.Core;

namespace TheCat.Gameplay
{
    public sealed class GameStateMachine
    {
        private readonly EventBus eventBus;

        public GameStateMachine(EventBus eventBus = null)
        {
            this.eventBus = eventBus;
            CurrentState = GameState.Boot;
        }

        public GameState CurrentState { get; private set; }

        public bool TryChangeState(GameState nextState)
        {
            if (!CanTransition(CurrentState, nextState))
            {
                return false;
            }

            ForceChangeState(nextState);
            return true;
        }

        public void ForceChangeState(GameState nextState)
        {
            if (CurrentState == nextState)
            {
                return;
            }

            GameState previousState = CurrentState;
            CurrentState = nextState;
            eventBus?.Publish(new GameStateChangedEvent(previousState, nextState));
        }

        public static bool CanTransition(GameState from, GameState to)
        {
            if (from == to)
            {
                return true;
            }

            switch (from)
            {
                case GameState.Boot:
                    return to == GameState.MainMenu;
                case GameState.MainMenu:
                    return to == GameState.CharacterSelect ||
                           to == GameState.CatRoom ||
                           to == GameState.RouteMap ||
                           to == GameState.BattleLoading;
                case GameState.CharacterSelect:
                    return to == GameState.MainMenu ||
                           to == GameState.CatRoom ||
                           to == GameState.RouteMap ||
                           to == GameState.BattleLoading;
                case GameState.CatRoom:
                    return to == GameState.MainMenu ||
                           to == GameState.CharacterSelect ||
                           to == GameState.RouteMap;
                case GameState.RouteMap:
                    return to == GameState.MainMenu ||
                           to == GameState.BattleLoading ||
                           to == GameState.RestNest ||
                           to == GameState.Shop ||
                           to == GameState.DreamEvent ||
                           to == GameState.BlessingOffering;
                case GameState.BattleLoading:
                    return to == GameState.Battle;
                case GameState.Battle:
                    return to == GameState.Paused ||
                           to == GameState.Result;
                case GameState.Paused:
                    return to == GameState.Battle ||
                           to == GameState.MainMenu;
                case GameState.RestNest:
                case GameState.Shop:
                case GameState.DreamEvent:
                case GameState.BlessingOffering:
                    return to == GameState.RouteMap;
                case GameState.Result:
                    return to == GameState.MainMenu ||
                           to == GameState.CatRoom ||
                           to == GameState.RouteMap ||
                           to == GameState.BattleLoading;
                default:
                    return false;
            }
        }
    }
}
