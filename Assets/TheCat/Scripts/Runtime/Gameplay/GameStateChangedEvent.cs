namespace TheCat.Gameplay
{
    public readonly struct GameStateChangedEvent
    {
        public GameStateChangedEvent(GameState previousState, GameState newState)
        {
            PreviousState = previousState;
            NewState = newState;
        }

        public GameState PreviousState { get; }

        public GameState NewState { get; }
    }
}
