using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform originalParent;
    [HideInInspector] public bool isOverContainer;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); // To allow free movement across the canvas
        MusicController.Instance.PlaySound(MusicController.Instance.dragClip);

        // Ensure the dragged object's position is in front of other elements
        Vector3 newPosition = transform.position;
        newPosition.z = 0f; // Set to 0 or a suitable value within the camera's clipping range
        transform.position = newPosition;

        transform.SetAsLastSibling();
        image.raycastTarget = false;
        isOverContainer = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        newPosition.z = 0f; // Maintain the Z position
        transform.position = newPosition;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isOverContainer)
        {
            transform.SetParent(originalParent);

            // Return to the original position in screen space
            Vector3 returnPosition = originalParent.position;
            returnPosition.z = 0f; // Or the original Z position if needed
            transform.position = returnPosition;
        }
        image.raycastTarget = true;
    }
}
