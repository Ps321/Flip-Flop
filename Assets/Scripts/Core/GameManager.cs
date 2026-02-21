using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 4;

    [Header("References")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private List<CardData> cardDataList;

    [Header("Grid Handler")]
    [SerializeField] private GridHandler gridHandler;

    [Header("Score Manager")]
    [SerializeField] private ScoreManager scoreManager;


    private List<Card> selectedCards = new List<Card>();
    private List<Card> allCards = new List<Card>();
    private int totalMatches;
    private int currentMatches;
    private int score;
    private Dictionary<int, CardData> cardLookup;

    private void Start()
    {
        cardLookup = new Dictionary<int, CardData>();
        foreach (var data in cardDataList)
            cardLookup[data.Id] = data;
    }
    public void StartNewGame(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;

        ClearBoard();

        gridHandler.ConfigureGrid(rows, columns);
        GenerateGrid();
    }

    public void ResumeGame(MatchData data)
    {
        ClearBoard();
        LoadGame(data);
    }

    private void ClearBoard()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        allCards.Clear();
    }

    private void GenerateGrid()
    {
        int totalCards = rows * columns;

        if (totalCards % 2 != 0)
        {
            Debug.LogError("Grid must contain even number of cards.");
            return;
        }

        List<CardData> pairs = CreatePairs(totalCards / 2);
        Shuffle(pairs);

        totalMatches = totalCards / 2;
        currentMatches = 0;

        for (int i = 0; i < pairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridParent);
            card.Initialize(pairs[i]);
            card.OnCardClicked += HandleCardClicked;
            allCards.Add(card);
        }
    }
    private void Shuffle(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private List<CardData> CreatePairs(int pairCount)
    {
        List<CardData> result = new List<CardData>();

        for (int i = 0; i < pairCount; i++)
        {
            CardData data = cardDataList[i % cardDataList.Count];

            result.Add(data);
            result.Add(data);
        }

        return result;
    }

    private void HandleCardClicked(Card card)
    {
        if (card.isFlipped || card.isMatched)
            return;

        card.Flip(true);
        selectedCards.Add(card);

        if (selectedCards.Count == 2)
        {
            Card first = selectedCards[0];
            Card second = selectedCards[1];
            selectedCards.Clear();
            StartCoroutine(CheckMatch(first, second));
        }
    }
    private IEnumerator CheckMatch(Card first, Card second)
    {
        yield return new WaitForSeconds(0.2f);
        scoreManager.AddMove();
        if (first.Id == second.Id)
        {
            first.SetMatched();
            second.SetMatched();
            AudioManager.Instance.Play(AudioType.Match);
            scoreManager.AddMatch(first.Data.scoreValue);
            currentMatches++;
            SaveGame();
            if (currentMatches >= totalMatches)
            {
                GameOver();
            }
        }
        else
        {
            scoreManager.AddMismatch();
            AudioManager.Instance.Play(AudioType.Mismatch);
            StartCoroutine(FlipBackAfterDelay(first, second));
        }
    }

    private IEnumerator FlipBackAfterDelay(Card first, Card second)
    {
        yield return new WaitForSeconds(0.6f);

        first.Flip(false);
        second.Flip(false);
        SaveGame();
    }

    private void GameOver()
    {
        scoreManager.GameOver();
        SaveSystem.Clear();
    }

    public void ResetGame()
    {
        SaveSystem.Clear();
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveGame();
    }


    /***** Save System**********/
    private void SaveGame()
    {
        MatchData data = new MatchData();
        data.rows = rows;
        data.columns = columns;
        data.score = scoreManager.Score;
        data.moves = scoreManager.Moves;
        data.matchedPairs = currentMatches;
        data.cardIds = new List<int>();
        data.matchedState = new List<bool>();

        foreach (var card in allCards)
        {
            data.cardIds.Add(card.Id);
            data.matchedState.Add(card.isMatched);
        }

        SaveSystem.Save(data);
    }

    private void LoadGame(MatchData data)
    {
        rows = data.rows;
        columns = data.columns;
        scoreManager.SetScoreAndMoves(data.score, data.moves);
        currentMatches = data.matchedPairs;
        gridHandler.ConfigureGrid(rows, columns);

        totalMatches = (rows * columns) / 2;

        for (int i = 0; i < data.cardIds.Count; i++)
        {
            CardData cardData = cardDataList.Find(x => x.Id == data.cardIds[i]);

            Card card = Instantiate(cardPrefab, gridParent);
            card.Initialize(cardData);
            card.OnCardClicked += HandleCardClicked;

            if (data.matchedState[i])
            {
                card.SetMatched(false);
                card.Flip(true);
            }

            allCards.Add(card);
        }
        if (currentMatches >= totalMatches)
        {
            GameOver();
        }
    }
}