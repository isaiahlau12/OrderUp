using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlateZoneDrop plateZone;
    public SatisfactionMeter satisfactionMeter;
    public CustomerManager customerManager;

    public void PlayCards()
    {
        if (plateZone == null || satisfactionMeter == null || customerManager == null)
        {
            Debug.LogError("PlateZone, SatisfactionMeter, or CustomerManager not assigned.");
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

        int scoreToBeat = customerManager.GetCurrentScoreToBeat();
        satisfactionMeter.UpdateMeter(totalScore, scoreToBeat);
        customerManager.ReactToSatisfaction(totalScore);
    }
}
