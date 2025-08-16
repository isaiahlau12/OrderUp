using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateZoneDrop : MonoBehaviour, IDropHandler
{
    public List<GameObject> cardsInPlate = new List<GameObject>();
    private Transform uiCanvas;

    [Header("Grid Settings")]
    public Vector2 cardSize = new Vector2(80f, 120f);   // width x height of each card
    public Vector2 spacing = new Vector2(10f, 10f);     // horizontal and vertical spacing
    public int maxColumns = 5;                           // max cards per row

    private void Start()
    {
        GameObject c = GameObject.FindGameObjectWithTag("Canvas");
        if (c != null) uiCanvas = c.transform;
        else Debug.LogWarning("PlateZoneDrop: No Canvas tagged object found.");
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag;
        if (droppedCard == null) return;

        if (!cardsInPlate.Contains(droppedCard))
            cardsInPlate.Add(droppedCard);

        droppedCard.transform.SetParent(transform, false);
        droppedCard.transform.localScale = Vector3.one; // ensure consistent scale

        RepositionCards();
        DeckManager deck = FindObjectOfType<DeckManager>();
        CardDisplay display = droppedCard.GetComponent<CardDisplay>();
        if (display != null && deck != null)
        {
            deck.DiscardCard(display.cardData);
        }

    }

    public void RemoveCard(GameObject card)
    {
        if (cardsInPlate.Contains(card))
        {
            cardsInPlate.Remove(card);
            if (uiCanvas != null)
                card.transform.SetParent(uiCanvas, false);
            else
                card.transform.SetParent(null);

            RepositionCards();
        }
    }

    public void ClearPlate()
    {
        for (int i = cardsInPlate.Count - 1; i >= 0; i--)
        {
            GameObject card = cardsInPlate[i];
            if (card != null)
                Destroy(card);
        }
        cardsInPlate.Clear();
    }

    private void RepositionCards()
    {
        for (int i = 0; i < cardsInPlate.Count; i++)
        {
            GameObject card = cardsInPlate[i];
            if (card != null)
            {
                RectTransform rt = card.GetComponent<RectTransform>();
                if (rt != null)
                {
                    int row = i / maxColumns;
                    int column = i % maxColumns;

                    float xPos = (cardSize.x + spacing.x) * column;
                    float yPos = -(cardSize.y + spacing.y) * row; // negative y goes down

                    // Center the grid in PlateZone
                    int totalColumns = Mathf.Min(cardsInPlate.Count, maxColumns);
                    float gridWidth = totalColumns * cardSize.x + (totalColumns - 1) * spacing.x;
                    float gridHeight = ((cardsInPlate.Count + maxColumns - 1) / maxColumns) * cardSize.y
                                     + (((cardsInPlate.Count + maxColumns - 1) / maxColumns) - 1) * spacing.y;

                    rt.anchoredPosition = new Vector2(xPos - gridWidth / 2 + cardSize.x / 2,
                                                      yPos + gridHeight / 2 - cardSize.y / 2);

                    rt.sizeDelta = cardSize;
                }
            }
        }
    }
}
