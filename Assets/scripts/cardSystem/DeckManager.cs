using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Deck Settings")]
    public List<CardData> startingDeck;  // assign your ScriptableObjects
    public GameObject cardPrefab;        // card prefab with CardDisplay & DragHandler
    public Transform handContainer;      // container for cards in hand
    public int maxHandSize = 5;          // default hand size

    [HideInInspector] public List<CardData> currentDeck;
    [HideInInspector] public List<CardData> discardPile;
    [HideInInspector] public int currentHandSize; // now accessible from GameManager

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

    // Draw a single card into hand
    public void DrawCard()
    {
        if (currentDeck.Count == 0)
        {
            if (discardPile.Count == 0) return; // nothing to draw
            currentDeck.AddRange(discardPile);
            discardPile.Clear();
            ShuffleDeck();
        }

        if (currentDeck.Count > 0)
        {
            CardData drawnCard = currentDeck[0];
            currentDeck.RemoveAt(0);

            GameObject cardObj = Instantiate(cardPrefab, handContainer);
            CardDisplay display = cardObj.GetComponent<CardDisplay>();
            if (display != null)
                display.SetCardData(drawnCard);
        }
    }

    // Add a card to discard pile
    public void DiscardCard(CardData card)
    {
        if (card != null)
            discardPile.Add(card);
    }
}
