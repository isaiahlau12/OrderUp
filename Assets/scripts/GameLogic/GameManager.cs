using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public PlateZoneDrop plateZone;
    public SatisfactionMeter satisfactionMeter;
    public CustomerManager customerManager;
    public DeckManager deckManager;

    [SerializeField] private float postPlayPause = 1.0f;

    // NEW: persists for the current customer until satisfied
    private int accumulatedScore = 0;

    public void PlayCards()
    {
        if (!plateZone || !satisfactionMeter || !customerManager || !deckManager)
        {
            Debug.LogError("GameManager: Missing references.");
            return;
        }

        int scoreToBeat = customerManager.GetCurrentScoreToBeat();
        int handScore = 0;
        int tempDrawBoost = 0;

        // Sum the hand, collect draw effects, discard played cards
        foreach (GameObject cardObj in plateZone.cardsInPlate)
        {
            var display = cardObj ? cardObj.GetComponent<CardDisplay>() : null;
            if (display != null && display.cardData != null)
            {
                handScore += display.cardData.satisfactionValue;

                if (display.cardData.effectType == CardEffectType.DrawCards)
                    tempDrawBoost += Mathf.Max(0, display.cardData.effectValue);

                deckManager.DiscardCard(display.cardData);
            }
        }

        // Add this hand's score to the running total
        accumulatedScore += handScore;

        // Update UI & reaction based on the accumulated score
        satisfactionMeter.UpdateMeter(accumulatedScore, scoreToBeat);
        bool satisfied = accumulatedScore >= scoreToBeat;
        customerManager.ReactToSatisfaction(accumulatedScore);

        // Hand-size boost applies to the *next* hand only
        deckManager.currentHandSize = deckManager.maxHandSize + tempDrawBoost;

        StartCoroutine(EndHandFlow(satisfied));
    }

    private IEnumerator EndHandFlow(bool satisfied)
    {
        yield return new WaitForSeconds(postPlayPause);

        // Clear played cards from the plate
        if (plateZone != null) plateZone.ClearPlate();

        if (satisfied)
        {
            // Move on, and reset the accumulator for the next customer
            customerManager.NextCustomer();
            accumulatedScore = 0;

            int newTarget = customerManager.GetCurrentScoreToBeat();
            satisfactionMeter.UpdateMeter(0, newTarget);
        }
        else
        {
            // Stay on the same customer; keep the accumulated score showing
            int sameTarget = customerManager.GetCurrentScoreToBeat();
            satisfactionMeter.UpdateMeter(accumulatedScore, sameTarget);
        }

        // Refill a fresh hand up to the (possibly boosted) currentHandSize
        RefillHandToCurrentSize();

        // Temporary boost lasts only for this refill; then revert
        deckManager.currentHandSize = deckManager.maxHandSize;
    }

    private void RefillHandToCurrentSize()
    {
        if (deckManager == null || deckManager.handContainer == null) return;

        int have = deckManager.handContainer.childCount;
        int need = Mathf.Max(0, deckManager.currentHandSize - have);

        for (int i = 0; i < need; i++)
            deckManager.DrawCard();
    }

    // Optional: if you have a way to manually reset/skip customer, also reset accumulatedScore there.
    public void ResetForNewCustomer()
    {
        accumulatedScore = 0;
        int target = customerManager.GetCurrentScoreToBeat();
        satisfactionMeter.UpdateMeter(0, target);
    }
}
