using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }
    public int Moves { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<int> OnMovesChanged;

    public void AddMatch(int value)
    {
        Score += value;
        OnScoreChanged?.Invoke(Score);
    }

    public void AddMove()
    {
        Moves++;
        OnMovesChanged?.Invoke(Moves);
    }

    public void Reset()
    {
        Score = 0;
        Moves = 0;
        OnScoreChanged?.Invoke(Score);
        OnMovesChanged?.Invoke(Moves);
    }
}