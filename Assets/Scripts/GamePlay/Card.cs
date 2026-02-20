using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
    public int Id { get; private set; }
    public bool isMatched { get; private set; }
    public bool isFlipped { get; private set; }

    [Header("Card References")]
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;

    private bool isAnimating;
    public event Action<Card> OnCardClicked;
    public void Initialize(int id, Sprite sprite)
    {
        Id = id;
        frontImage.sprite = sprite;
        ShowFront(false);
        isMatched = false;
        isFlipped = false;
    }
    public void Select()
    {
        if (isMatched || isFlipped || isAnimating)
            return;
        OnCardClicked?.Invoke(this);

    }
    public void ShowFront(bool value)
    {
        isFlipped = value;
        frontImage.gameObject.SetActive(value);
    }
    public void SetMatched()
    {
        isMatched = true;
        StartCoroutine(HideCard());
    }
    IEnumerator HideCard()
    {
        yield return new WaitForSeconds(0.5f);
        frontImage.enabled = false;
        backImage.enabled = false;
    }

    public void Flip(bool showFront)
    {
        if (isAnimating || isMatched) return;
        StartCoroutine(FlipAnimation(showFront));
    }
    IEnumerator FlipAnimation(bool showFront)
    {
        isAnimating = true;
        float duration = 0.2f;
        float time = 0;

        while (time < duration)
        {
            float scale = Mathf.Lerp(1, 0, time / duration);
            transform.localScale = new Vector3(scale, 1, 1);
            time += Time.deltaTime;
            yield return null;
        }


        ShowFront(showFront); //Mid way

        time = 0f;

        while (time < duration)
        {
            float scale = Mathf.Lerp(0, 1, time / duration);
            transform.localScale = new Vector3(scale, 1, 1);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one;
        isFlipped = showFront;
        isAnimating = false;
    }


}
