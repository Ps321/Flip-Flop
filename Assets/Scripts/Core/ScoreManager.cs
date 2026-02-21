using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }
    public int Moves { get; private set; }
    public int Combo { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<int> OnMovesChanged;
    public event Action<int, int> OnGameOver;
    public event Action<int> OnComboChanged;

    public void AddMatch(int value)
    {
        Combo++;
        int finalValue = value * Combo;
        Score += finalValue;
        OnComboChanged?.Invoke(Combo);
        OnScoreChanged?.Invoke(Score);
    }
    public void AddMismatch()
    {
        Combo = 0;
        OnComboChanged?.Invoke(Combo);
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

    public void SetScoreAndMoves(int score, int moves)
    {
        Score = score;
        Moves = moves;

        OnScoreChanged?.Invoke(Score);
        OnMovesChanged?.Invoke(Moves);
    }
}