using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<Card> selectedCards = new List<Card>();
    private int totalMatches;
    private int currentMatches;
    private int score;

    private void Start()
    {
        gridHandler.ConfigureGrid(rows, columns);
        GenerateGrid();
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
            card.Initialize(pairs[i].Id, pairs[i].Sprite);
            card.OnCardClicked += HandleCardClicked;
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
            CheckMatch(first, second);
        }
    }
    private void CheckMatch(Card first, Card second)
    {
        if (first.Id == second.Id)
        {
            first.SetMatched();
            second.SetMatched();

            score += GetScoreForMatch(first.Id);

            currentMatches++;

            if (currentMatches >= totalMatches)
            {
                GameOver();
            }

        }
        else
        {
            StartCoroutine(FlipBackAfterDelay(first, second));
        }
    }

    private IEnumerator FlipBackAfterDelay(Card first, Card second)
    {
        yield return new WaitForSeconds(0.6f);

        first.Flip(false);
        second.Flip(false);
    }

    private int GetScoreForMatch(int id)
    {
        CardData data = cardDataList.Find(x => x.Id == id);
        return data != null ? data.scoreValue : 0;
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + score);
    }
}