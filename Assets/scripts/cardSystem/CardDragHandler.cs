using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // If card is not dropped in a drop zone, return to hand
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            transform.localScale = Vector3.one;

            // If card was removed from plate, tell the plate
            PlateZoneDrop plateZone = originalParent.GetComponentInParent<PlateZoneDrop>();
            if (plateZone != null)
            {
                plateZone.RemoveCard(gameObject);
            }
        }
    }
}
