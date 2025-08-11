using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomer", menuName = "Game/Customer Data")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public Sprite neutralPortrait;
    public Sprite happyPortrait;
    public Sprite sadPortrait;
    public int scoreToBeat;
}
