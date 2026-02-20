using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }
    public int Moves { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<int> OnMovesChanged;
    public event Action<int, int> OnGameOver;

    public void AddMatch(int value)
    {
        Score += value;
        OnScoreChanged?.Invoke(Score);
    }

    public void AddMove(int moves = 1)
    {
        Moves += moves;
        OnMovesChanged?.Invoke(Moves);
    }

    public void GameOver()
    {
        OnGameOver?.Invoke(Score, Moves);
    }
}