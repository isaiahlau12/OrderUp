using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//defines area where cards can be dropped
public class PlateZoneDrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag;
        if (droppedCard == null) return;

        // Snap card to the plate and center it as parent
        droppedCard.transform.SetParent(transform, false);
        droppedCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        Debug.Log($"Card dropped: {droppedCard.name}");
    }
}