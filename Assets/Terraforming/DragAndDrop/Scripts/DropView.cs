using Events;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DropView : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<PointerEventData> OnPointerEntered;
    public event Action<PointerEventData> OnPointerExited;
    protected static GameObject currentPointerDrag;
    protected Vector3 initialHoveredObjectScale;
    public Func<PointerEventData, bool> IsDraggedObjectInteractableWithMe;

    public abstract void OnDrop(PointerEventData eventData);
    
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"OnPointerEnter {eventData.position}", gameObject);
        OnPointerEntered?.Invoke(eventData);
        currentPointerDrag = eventData.pointerDrag;
        if (eventData.pointerDrag == null || IsDraggedObjectInteractableWithMe == null || !IsDraggedObjectInteractableWithMe(eventData))
            return;

        initialHoveredObjectScale = eventData.pointerDrag.transform.localScale;
        eventData.pointerDrag.transform.localScale = eventData.pointerDrag.transform.localScale * 0.7f;
        EventManager.Dispatch(ObjectInteractionEvents.Hover);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"OnPointerExit {eventData.position}", gameObject);
        OnPointerExited?.Invoke(eventData);
        RestoreHoveredObjectScale(eventData);
        currentPointerDrag = null;
    }

    protected void RestoreHoveredObjectScale(PointerEventData eventData)
    {
        if (initialHoveredObjectScale == Vector3.zero)
            return;
        currentPointerDrag.transform.localScale = initialHoveredObjectScale;
        initialHoveredObjectScale = Vector3.zero;
    }
}