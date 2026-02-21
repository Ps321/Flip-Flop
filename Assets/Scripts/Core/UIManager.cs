using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [Header("Game HUD")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text movesText;
    [SerializeField] private TMP_Text comboText;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalMovesText;
    private void OnEnable()
    {
        scoreManager.OnScoreChanged += UpdateScore;
        scoreManager.OnMovesChanged += UpdateMoves;
        scoreManager.OnComboChanged += UpdateCombo;
        scoreManager.OnGameOver += ShowGameOver;
    }

    private void ShowGameOver(int score, int moves)
    {
        StartCoroutine(ShowGameOverCoroutine(score, moves));

    }
    private IEnumerator ShowGameOverCoroutine(int score, int moves)
    {
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.Play(AudioType.GameOver);
        gameOverPanel.SetActive(true);
        finalScoreText.text = score.ToString();
        finalMovesText.text = moves.ToString();
        movesText.text = "";
        scoreText.text = "";
    }
    private void OnDisable()
    {
        scoreManager.OnScoreChanged -= UpdateScore;
        scoreManager.OnMovesChanged -= UpdateMoves;
        scoreManager.OnComboChanged -= UpdateCombo;
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
    private void UpdateCombo(int value)
    {
        if (value > 1)
            comboText.text = "Combo x" + value;
        else
            comboText.text = "";
    }
}