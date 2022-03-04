public enum AdType
{
    Admob,
    Unity
}

public enum GameState
{
    GoToHome,
    GameOver
}

[System.Serializable]
public class AdEvents
{
    public AdType adType;
    public GameState adPlacement;
}
