using UnityEngine;

[CreateAssetMenu(menuName = "Card Match/Card Data")]
public class CardData : ScriptableObject
{
    public int Id;
    public Sprite Sprite;

    public int scoreValue = 10; // Default score value for matching this card
}