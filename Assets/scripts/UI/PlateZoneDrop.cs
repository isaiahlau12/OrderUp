using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateZoneDrop : MonoBehaviour, IDropHandler
{
    public List<GameObject> cardsInPlate = new List<GameObject>();
    private Transform canvas;
    private void Start()
    {

        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag;
        if (droppedCard == null) return;

        if (!cardsInPlate.Contains(droppedCard))
        {
            cardsInPlate.Add(droppedCard);
        }

        droppedCard.transform.SetParent(transform, false);
        droppedCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void RemoveCard(GameObject card)
    {
        if (cardsInPlate.Contains(card))
        {
            cardsInPlate.Remove(card);
            card.transform.SetParent(canvas, false);
        }
    }
}
