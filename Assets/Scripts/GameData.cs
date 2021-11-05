
public class GameData
{
    public static GameManager gameManager = null;
    public static BotController botController = null;
    
    public static AI AI = null;
    public static AI GetAI()
    {
        if (AI == null)
            AI = new AI();
        return AI;
    }
    
    public static GameState gameState = null;
    public static GameState GetGameState()
    {
        if (gameState == null)
            gameState = new GameState();
        return gameState;
    }
}