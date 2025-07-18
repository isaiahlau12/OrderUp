using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;
    public Image cardImage;
    public TextMeshProUGUI cardNameText;

    void Start()
    {
        if (cardData != null)
        {
            cardImage.sprite = cardData.cardSprite;
            cardNameText.text = cardData.cardName;
        }
    }
}
