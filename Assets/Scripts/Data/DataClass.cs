using System.Collections.Generic;

public enum GameState
{
    Playing,
    GameOver,
}

public enum GameType
{
    Easy,
    Medium,
    Hard
}

[System.Serializable]
public class DifficultyConfig
{
    public GameType gameType;
    public int rows;
    public int columns;
}

[System.Serializable]
public class MatchData
{
    public int rows;
    public int columns;
    public int score;
    public int matchedPairs;
    public int moves;

    public List<int> cardIds;
    public List<bool> matchedState;
    public List<bool> flippedState;
}