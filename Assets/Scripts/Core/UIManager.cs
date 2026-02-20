using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [Header("Game HUD")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text movesText;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalMovesText;
    private void OnEnable()
    {
        scoreManager.OnScoreChanged += UpdateScore;
        scoreManager.OnMovesChanged += UpdateMoves;
        scoreManager.OnGameOver += ShowGameOver;
    }

    private void ShowGameOver(int score, int moves)
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Final Score: {score}";
        finalMovesText.text = $"Total Moves: {moves}";
        movesText.text = "";
        scoreText.text = "";
    }

    private void OnDisable()
    {
        scoreManager.OnScoreChanged -= UpdateScore;
        scoreManager.OnMovesChanged -= UpdateMoves;
        scoreManager.OnGameOver -= ShowGameOver;
    }

    private void UpdateScore(int value)
    {
        scoreText.text = value.ToString();
    }

    private void UpdateMoves(int value)
    {
        movesText.text = value.ToString();
    }
}