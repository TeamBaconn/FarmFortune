public enum GameState
{
    NEW_GAME, LOAD_GAME, GAME_WIN_END
}

public class OnGameStateChange : EventParam
{
    public GameplayManager manager;
    public GameState state;

    public OnGameStateChange(GameplayManager manager, GameState state)
    {
        this.manager = manager;
        this.state = state;
    }
}