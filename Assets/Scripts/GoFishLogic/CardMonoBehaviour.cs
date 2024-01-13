using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardMonoBehaviour : MonoBehaviour
{
    public Card CardData { get; private set; }
    public Sprite CardSprite { get; private set; }
    public Animation CardAnimation { get; private set; }

    public void Initialize(Card cardData, Sprite cardSprite)
    {
        CardData = cardData;
        CardSprite = cardSprite;

        // Set the card's visual representation, e.g., sprite, position, etc.
        GetComponent<SpriteRenderer>().sprite = cardSprite;
    }
}
