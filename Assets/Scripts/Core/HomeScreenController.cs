using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Screens")]
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject levelScreen;
    [SerializeField] private GameObject resumePopup;
    [SerializeField] private GameObject gameScreen;

    [Header("Difficulty")]
    [SerializeField] private List<DifficultyConfig> difficulties;

    private MatchData cachedSave;
    private void Start()
    {
        cachedSave = SaveSystem.Load();
    }

    public void OnPlayClicked()
    {
        if (cachedSave != null)
            ShowResumePopup();
        else
            ShowLevelSelection();
    }

    public void OnResumeYes()
    {
        ShowGame();
        gameManager.ResumeGame(cachedSave);
    }

    public void OnResumeNo()
    {
        SaveSystem.Clear();
        resumePopup.SetActive(false);
        ShowLevelSelection();

    }

    public void OnDifficultySelected(int difficultyIndex)
    {
        ShowGame();
        GameType difficulty = (GameType)difficultyIndex;
        DifficultyConfig config = difficulties.Find(x => x.gameType == difficulty);
        gameManager.StartNewGame(config.rows, config.columns);
    }

    private void ShowHome()
    {
        homeScreen.SetActive(true);
        levelScreen.SetActive(false);
        resumePopup.SetActive(false);
        gameScreen.SetActive(false);
    }

    private void ShowLevelSelection()
    {
        levelScreen.SetActive(true);
    }

    private void ShowResumePopup()
    {
        resumePopup.SetActive(true);
    }

    private void ShowGame()
    {
        homeScreen.SetActive(false);
    }
}

