using Events;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming
{
    [RequireComponent(typeof(Collider2D))]
    public class DropView : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PointerEventData> OnPointerEntered;
        public event Action<PointerEventData> OnPointerExited;
        public event Action<PointerEventData> OnDropped;

        public Func<PointerEventData, bool> IsDraggedObjectInteractableWithMe;

        private Vector3 initialHoveredObjectScale;
        private static GameObject currentPointerDrag;
        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log($"OnDrop {eventData.position}", gameObject);
            OnDropped?.Invoke(eventData);
            RestoreHoveredObjectScale(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"OnPointerEnter {eventData.position}", gameObject);
            OnPointerEntered?.Invoke(eventData);
            currentPointerDrag = eventData.pointerDrag;
            if (eventData.pointerDrag == null || IsDraggedObjectInteractableWithMe == null || !IsDraggedObjectInteractableWithMe(eventData))
                return;
            
            initialHoveredObjectScale = eventData.pointerDrag.transform.localScale;
            eventData.pointerDrag.transform.localScale = eventData.pointerDrag.transform.localScale * 0.7f;
            EventManager.Dispatch(ObjectInteractionEvents.Hover);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"OnPointerExit {eventData.position}", gameObject);
            OnPointerExited?.Invoke(eventData);
            RestoreHoveredObjectScale(eventData);
            currentPointerDrag = null;
        }

        private void RestoreHoveredObjectScale(PointerEventData eventData)
        {
            if (initialHoveredObjectScale == Vector3.zero)
                return;
            currentPointerDrag.transform.localScale = initialHoveredObjectScale;
            initialHoveredObjectScale = Vector3.zero;
        }
    }
}