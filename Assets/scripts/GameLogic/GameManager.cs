using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public Transform plateZone; // platezone
    public SatisfactionMeter satisfactionMeter; // satisfactionmeter

    public void PlayCards()
    {
        // Debug to chekc if fucntion is called
        Debug.Log("PlayCards() triggered");

        // Null checks
        if (plateZone == null || satisfactionMeter == null)
        {
            Debug.LogError("Missing references in GameManager!");
            return;
        }

        // Calculate total satisfaction score
        int totalScore = 0;
        foreach (Transform card in plateZone)
        {
            if (card == null) continue;

            CardDisplay display = card.GetComponent<CardDisplay>();
            if (display == null || display.cardData == null) continue;

            totalScore += display.cardData.satisfactionValue;
            Debug.Log($"Added {display.cardData.cardName}: {display.cardData.satisfactionValue}");
        }

        Debug.Log($"Total Score: {totalScore}");
        satisfactionMeter.UpdateMeter(totalScore); // update the meter
    }
}