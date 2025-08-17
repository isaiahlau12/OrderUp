using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector3 originalLocalScale;
    private CanvasGroup canvasGroup;
    private Canvas rootCanvas;

    private PlateZoneDrop previousPlate; // if the card started on a plate

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rootCanvas = GetComponentInParent<Canvas>();
        if (!rootCanvas) Debug.LogWarning("CardDragHandler: No Canvas found in parents.");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        originalLocalScale = transform.localScale;

        previousPlate = GetComponentInParent<PlateZoneDrop>();

        // Lift to top layer for dragging
        if (rootCanvas) transform.SetParent(rootCanvas.transform, true);

        transform.localScale = originalLocalScale * 1.05f;
        canvasGroup.blocksRaycasts = false; // allow drop zones to receive OnDrop
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // If no drop zone took the card,  still under the root canvas.
        bool dropAccepted = (transform.parent != null && transform.parent != rootCanvas.transform);

        if (!dropAccepted)
        {
            // If  pulled the card off a plate and didn't drop anywhere valid,
            // send it to the HandContainer.
            var deck = FindObjectOfType<DeckManager>();
            if (previousPlate != null && deck != null && deck.handContainer != null)
            {
                previousPlate.RemoveCard(gameObject);
                transform.SetParent(deck.handContainer, false);
                transform.localScale = originalLocalScale;
                return;
            }

            // Otherwise, revert to original parent (e.g., back into the hand layout)
            transform.SetParent(originalParent, false);
            transform.SetSiblingIndex(originalSiblingIndex);
        }

        // Normalize visuals
        transform.localScale = originalLocalScale;
    }
}
