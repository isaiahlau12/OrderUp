using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Deck Settings")]
    public List<CardData> startingDeck;  // assign your cards as a list 
    public GameObject cardPrefab;        // card prefab with CardDisplay & DragHandler
    public Transform handContainer;      // handConainer 
    public int maxHandSize = 5;          // default hand size

    [HideInInspector] public List<CardData> currentDeck;
    [HideInInspector] public List<CardData> discardPile;
    [HideInInspector] public int currentHandSize; 

    private void Start()
    {
        InitializeDeck();
        currentHandSize = maxHandSize;
        DrawInitialHand();
    }

    // Initialize deck and shuffle
    public void InitializeDeck()
    {
        currentDeck = new List<CardData>(startingDeck);
        ShuffleDeck();
        discardPile = new List<CardData>();
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < currentDeck.Count; i++)
        {
            int rand = Random.Range(i, currentDeck.Count);
            CardData temp = currentDeck[i];
            currentDeck[i] = currentDeck[rand];
            currentDeck[rand] = temp;
        }
    }

    // Draw initial hand
    public void DrawInitialHand()
    {
        for (int i = 0; i < currentHandSize; i++)
            DrawCard();
    }

    // Draw a single card into hand by
    // Taking the  top card (index 0), instantiate a card prefab under handContaine  and load the card data .
    public void DrawCard()
    {
        //if deck empty reshuffle discard pile 
        if (currentDeck.Count == 0)
        {
            if (discardPile.Count == 0) return; // nothing to draw
            currentDeck.AddRange(discardPile);
            discardPile.Clear();
            ShuffleDeck();
        }

        if (currentDeck.Count > 0)
        {
            //remove top card from the deck 
            CardData drawnCard = currentDeck[0];
            currentDeck.RemoveAt(0);
            // Instantiate UI prefab and assign the CardData so the card updates with respective visuals 
            if (cardPrefab != null && handContainer != null)
            {
                GameObject cardObj = Instantiate(cardPrefab, handContainer);
                CardDisplay display = cardObj.GetComponent<CardDisplay>();
                if (display != null)
                    display.SetCardData(drawnCard);
            }
        }
    }
    //add cards to the discard pile when played or discarded 
    public void DiscardCard(CardData card)
    {
        if (card != null)
            discardPile.Add(card);
    }
    //drawing multiple cards by calling DrawCard count times
    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DrawCard();
        }
    }
    //discard function so you can discard a list of card gameobjects selected in the handContainer
    //before retunring the number of cards discarded 
    public int DiscardAndDrawFromHand(List<GameObject> handCardsToDiscard)
    {
        if (handCardsToDiscard == null || handCardsToDiscard.Count == 0) return 0;

        int count = 0;
        foreach (var cardObj in handCardsToDiscard)
        {
            if (cardObj == null) continue;

            CardDisplay disp = cardObj.GetComponent<CardDisplay>();
            if (disp != null && disp.cardData != null)
            {
                DiscardCard(disp.cardData); 
                count++;
            }

            Destroy(cardObj);
        }

        // after removing cards you draw cards times the count 
        DrawCards(count);
        return count;
    }
}


