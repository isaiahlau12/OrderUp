using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Creating scriptableObjects for cards and card data i.e name , sprite and staisfaction value 
[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardID; 
    public string cardName;
    public Sprite cardSprite;
    public CardType cardType;
    public int satisfactionValue;
    public List<CardType> tags = new List<CardType>();

    [Header("Special Effects")]
    public CardEffectType effectType = CardEffectType.None;
    public int effectValue; // Bonus or number of cards to draw
}
//enum for the different card types , more types of cards will be added as devlopment progresses
public enum CardType
{
    Meat,
    Vegetable,
    Sauce,
    Spice,
    sweets,
    processes,
    BBQ
}
//special effect for cards that dont exclusively add satisfaction
public enum CardEffectType
{
    None,
    BonusForCardsPlayed, // +staisfaction for x of cards played
    DrawCards // Draw X cards from the deck
}
