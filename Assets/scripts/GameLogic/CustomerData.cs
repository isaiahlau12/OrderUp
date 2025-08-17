using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomer", menuName = "Game/Customer Data")]
//set scriptable objects in unity 
public class CustomerData : ScriptableObject
{
    public string customerName;
    public Sprite neutralPortrait;
    public Sprite happyPortrait;
    public Sprite sadPortrait;
    public int scoreToBeat;
    //preference 
    [Header("Preferences")]
    public bool hasPreferredType = false;
    public CardType preferredType;           
    public int preferredTypeBonus = 0;       
    public bool bonusPerMatchingCard = true;

}
