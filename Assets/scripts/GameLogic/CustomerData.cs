using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomer", menuName = "Game/Customer Data")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public Sprite neutralPortrait;
    public Sprite happyPortrait;
    public Sprite sadPortrait;
    public int scoreToBeat;
    //preference 
    [Header("Preferences")]
    public List<CardType> preferredCardTypes;
    public int bonusPerPreferredCard = 5;

}
