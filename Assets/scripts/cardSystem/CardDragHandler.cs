using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
// enables drag and drop 
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup; // control raycast 
    private Transform originalParent; 

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); // moves to the top of the hierarchy 
        canvasGroup.blocksRaycasts = false; //disables raycast 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //Follow mouse 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //  dropped on something invalid, return to hand
        if (transform.parent == transform.root)
        {
            transform.SetParent(originalParent); // Return to hand if not dropped properly
        }

        canvasGroup.blocksRaycasts = true;
    }

}
