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

    private List<Card> selectedCards = new List<Card>();
    private int totalMatches;
    private int currentMatches;
    private int score;

    private void Start()
    {
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
        if (selectedCards.Contains(card))
            return;

        if (selectedCards.Count >= 2)
            return;

        card.Flip(true);
        selectedCards.Add(card);

        if (selectedCards.Count == 2)
        {
            // Check for match
        }
    }

}