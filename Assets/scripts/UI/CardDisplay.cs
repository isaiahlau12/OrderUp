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
    public TextMeshProUGUI satisfactionText; // to show satisfaction if not already on th card 

    // Assign card data dynamically
    public void SetCardData(CardData data)
    {
        cardData = data;

        if (cardImage != null && cardData.cardSprite != null)
            cardImage.sprite = cardData.cardSprite;

        if (cardNameText != null)
            cardNameText.text = cardData.cardName;

        if (satisfactionText != null)
            satisfactionText.text = cardData.satisfactionValue.ToString();
    }

    public CardType GetCardType()
    {
        return cardData != null ? cardData.cardType : CardType.Meat;
    }

    public CardEffectType GetEffectType()
    {
        return cardData != null ? cardData.effectType : CardEffectType.None;
    }
}
