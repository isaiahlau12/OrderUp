using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// GameManager purposes inlcude  resolving  hands, applying different  card effects and recipe/preference bonuses,
// handles hand counters and win/lose flow 
// whne win  loads EndScene. 
// If PersistentGameStats exists it will populate the endsc
public class GameManager : MonoBehaviour
{
    [Header("References")]
    public PlateZoneDrop plateZone;
    public SatisfactionMeter satisfactionMeter;
    public CustomerManager customerManager;
    public DeckManager deckManager;
    public RecipeBookManager recipeBook;
    public HandCounterUI handCounterUI;

    [Header("Level")]
    public int initialHands = 10;           // how many hands allowed for this level
    public string endSceneName = "EndScene"; // win scene name
    public string loseSceneName = "LoseScene";

    [SerializeField] private float postPlayPause = 1.0f;

    // runtime 
    private int handsLeft;
    private int totalHandsPlayed = 0;
    private int customersSatisfied = 0;

    // accumulated score for surrent suctomer
    private int accumulatedScore = 0;

    // Initialize counters and UI
    private void Start()
    {
        handsLeft = Mathf.Max(0, initialHands);
        UpdateHandUI();

        if (satisfactionMeter != null && customerManager != null)
            satisfactionMeter.UpdateMeter(accumulatedScore, customerManager.GetCurrentScoreToBeat());

        Debug.Log($"[GameManager] Start: handsLeft={handsLeft}, initialHands={initialHands}");
    }

    // Called by UI when player presses play 
    //resolves value of cards withj bonuses 
    //updates the counters
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
        int perCardBonuses = 0;

        List<GameObject> cards = plateZone.cardsInPlate;

        // Process each played card
        foreach (GameObject cardObj in cards)
        {
            var display = cardObj ? cardObj.GetComponent<CardDisplay>() : null;
            if (display != null && display.cardData != null)
            {
                // Base satisfaction value
                handScore += display.cardData.satisfactionValue;

                // +X per card played effect
                if (display.cardData.effectType == CardEffectType.BonusForCardsPlayed)
                {
                    perCardBonuses += display.cardData.effectValue * cards.Count;
                }

                // Draw X effect also applis to next hand 
                if (display.cardData.effectType == CardEffectType.DrawCards)
                {
                    tempDrawBoost += Mathf.Max(0, display.cardData.effectValue);
                }

                // Move card data to discard pile
                deckManager.DiscardCard(display.cardData);
            }
        }

        // recipe bonus
        int recipeBonus = 0;
        if (recipeBook != null)
            recipeBonus = recipeBook.GetTotalRecipeBonus(cards);

        // customer prefernces 
        int preferredTypeBonusTotal = 0;
        var currentCustomer = customerManager.GetCurrentCustomer();
        if (currentCustomer != null && currentCustomer.hasPreferredType && currentCustomer.preferredTypeBonus != 0)
        {
            int matches = 0;
            foreach (var cardObj in cards)
            {
                var disp = cardObj ? cardObj.GetComponent<CardDisplay>() : null;
                if (disp != null && disp.cardData != null)
                {
                    if (disp.cardData.cardType == currentCustomer.preferredType)
                        matches++;
                }
            }

            if (matches > 0)
            {
                if (currentCustomer.bonusPerMatchingCard)
                    preferredTypeBonusTotal = currentCustomer.preferredTypeBonus * matches; // per match
                else
                    preferredTypeBonusTotal = currentCustomer.preferredTypeBonus; // one-time
            }
        }

        int totalThisHand = handScore + perCardBonuses + recipeBonus + preferredTypeBonusTotal;

        // Add to accumulated score for this customer
        accumulatedScore += totalThisHand;

        // Update UI and reaction
        satisfactionMeter.UpdateMeter(accumulatedScore, scoreToBeat);
        customerManager.ReactToSatisfaction(accumulatedScore);

        //taking note of hands played 
        totalHandsPlayed++;
        handsLeft = Mathf.Max(0, handsLeft - 1);
        Debug.Log($"[GameManager] PlayCards: totalThisHand={totalThisHand}, accumulatedScore={accumulatedScore}, handsLeft={handsLeft}, totalHandsPlayed={totalHandsPlayed}");
        UpdateHandUI();

        // for persistant stats to be used later at end scene 
        if (PersistentGameStats.Instance != null)
        {
            PersistentGameStats.Instance.handsPlayed = totalHandsPlayed;
            PersistentGameStats.Instance.customersSatisfied = customersSatisfied;
            PersistentGameStats.Instance.totalCustomersInLevel = customerManager.GetTotalCustomers();
            Debug.Log($"[PersistentGameStats] Updated mid-game: handsPlayed={totalHandsPlayed}, customersSatisfied={customersSatisfied}, total={customerManager.GetTotalCustomers()}");
        }

        // If customer satisfied makred them as satisfied 
        bool satisfied = accumulatedScore >= scoreToBeat;
        if (satisfied)
        {
            customersSatisfied++;
            customerManager.MarkCurrentCustomerSatisfied();
            Debug.Log($"[GameManager] Customer satisfied! customersSatisfied={customersSatisfied}");
        }

        deckManager.currentHandSize = deckManager.maxHandSize + tempDrawBoost;

        StartCoroutine(EndHandFlow(satisfied));
    }


    // handles the post-play pause, clearing visuals, checking win/lose and refilling the hand.
    private IEnumerator EndHandFlow(bool satisfied)
    {
        yield return new WaitForSeconds(postPlayPause);

        // Clear cards on plate 
        if (plateZone != null) plateZone.ClearPlate();

        if (satisfied)
        {
            // If all customer satisfied you win if not move on and reset current score 
            bool allSatisfied = customerManager.IsAllCustomersSatisfied() || customersSatisfied >= customerManager.GetTotalCustomers();
            if (allSatisfied)
            {
                Debug.Log("[GameManager] All customers satisfied — WIN. Loading EndScene.");

                // If PersistentGameStats exists, set the final stats
                if (PersistentGameStats.Instance != null)
                {
                    PersistentGameStats.Instance.handsPlayed = totalHandsPlayed;
                    PersistentGameStats.Instance.customersSatisfied = customersSatisfied;
                    PersistentGameStats.Instance.totalCustomersInLevel = customerManager.GetTotalCustomers();
                }

                // Load win scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(endSceneName);
                yield break;
            }
            else
            {
                // Advance to next customer and reset accumulated score 
                customerManager.NextCustomer();
                accumulatedScore = 0;
                int newTarget = customerManager.GetCurrentScoreToBeat();
                satisfactionMeter.UpdateMeter(0, newTarget);
            }
        }
        else
        {
            // Not satisfied  saty on same customer 
            satisfactionMeter.UpdateMeter(accumulatedScore, customerManager.GetCurrentScoreToBeat());
        }

        // Lose condition: ran out of hands
        if (handsLeft <= 0)
        {
            Debug.Log("[GameManager] No hands left - loading lose scene");

            if (PersistentGameStats.Instance != null)
            {
                PersistentGameStats.Instance.handsPlayed = totalHandsPlayed;
                PersistentGameStats.Instance.customersSatisfied = customersSatisfied;
                PersistentGameStats.Instance.totalCustomersInLevel = customerManager.GetTotalCustomers();
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(loseSceneName);
            yield break;
        }

        // Refill hand up to currentHandSize (may contain temporary boost)
        RefillHandToCurrentSize();

        // Revert temporary boost
        deckManager.currentHandSize = deckManager.maxHandSize;
    }

    // Ensures the player's hand contains deckManager.currentHandSize cards by drawing as needed.
    private void RefillHandToCurrentSize()
    {
        if (deckManager == null || deckManager.handContainer == null) return;

        int have = deckManager.handContainer.childCount;
        int need = Mathf.Max(0, deckManager.currentHandSize - have);

        for (int i = 0; i < need; i++)
            deckManager.DrawCard();
    }

    public void ResetForNewCustomer()
    {
        accumulatedScore = 0;
        int target = customerManager.GetCurrentScoreToBeat();
        satisfactionMeter.UpdateMeter(0, target);
    }

    // Updates the hand UI 
    private void UpdateHandUI()
    {
        if (handCounterUI != null)
            handCounterUI.SetHandsLeft(handsLeft);
        else
            Debug.LogWarning("[GameManager] handCounterUI is null - cannot update hand UI");
    }
}
