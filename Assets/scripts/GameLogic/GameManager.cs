using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlateZoneDrop plateZone;
    public SatisfactionMeter satisfactionMeter;
    public int scoreToBeat = 100;

    public void PlayCards()
    {
        if (plateZone == null || satisfactionMeter == null)
        {
            Debug.LogError("PlateZone or SatisfactionMeter not assigned.");
            return;
        }

        int totalScore = 0;

        foreach (GameObject card in plateZone.cardsInPlate)
        {
            CardDisplay display = card.GetComponent<CardDisplay>();
            if (display != null && display.cardData != null)
            {
                totalScore += display.cardData.satisfactionValue;
            }
        }
        satisfactionMeter.UpdateMeter(totalScore, scoreToBeat);
    }
}
