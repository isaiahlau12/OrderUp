using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Creating scriptableObjects for cards and card data i.e name , sprite and staisfaction value 
[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardSprite;
    public CardType cardType;
    public int satisfactionValue;
}
//enum for the different card types , more types of cards will be added as devlopment progresses
public enum CardType
{
    Meat,
    Vegetable,
    Sauce,
    Spice,
    Other
}
