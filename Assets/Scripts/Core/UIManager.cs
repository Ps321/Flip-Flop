using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI movesText;

    private void OnEnable()
    {
        scoreManager.OnScoreChanged += UpdateScore;
        scoreManager.OnMovesChanged += UpdateMoves;
    }

    private void OnDisable()
    {
        scoreManager.OnScoreChanged -= UpdateScore;
        scoreManager.OnMovesChanged -= UpdateMoves;
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