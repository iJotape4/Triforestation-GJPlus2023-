using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming
{
    [RequireComponent(typeof(Collider2D))]
    public class DragView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> OnDragBegan;
        public event Action<PointerEventData> OnDragEnded;

        private new Collider2D collider;
        private Camera mainCamera;

        private Vector3 initialDragPosition;
        private Vector3 currentDragPosition;

        bool isDragging, draggingAllowed = true;
        private void Awake()
        {
            mainCamera = Camera.main;
            collider = GetComponent<Collider2D>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!draggingAllowed) return;
            //Debug.Log($"OnBeginDrag {eventData.position}", gameObject);
            initialDragPosition = transform.position;
            collider.enabled = false;
            isDragging = true;
            OnDragBegan?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!draggingAllowed)
            {
                if (isDragging)               
                    OnEndDrag(eventData);
                
                eventData.pointerDrag = null;
                return;
            }
            //Debug.Log($"OnDrag {eventData.position}", gameObject);
            currentDragPosition = mainCamera.ScreenToWorldPoint(eventData.position);
            currentDragPosition.z = transform.position.z;
            transform.position = currentDragPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log($"OnEndDrag {eventData.position}", gameObject);
            if (ValidateDrop())
                Drop();
            else
                ReturnToPosition();


            OnDragEnded?.Invoke(eventData);
        }
        bool ValidateDrop()
        {
            //TODO : Depending if the object was placed in a right way
            return false;
        }

        void ReturnToPosition()
        {
            transform.position = initialDragPosition;
            collider.enabled = true;
            isDragging = false;
        }

        void Drop()
        {
            //TODO : What should I do if drop is valid?
        }

        public void ForceAllowDragging() => draggingAllowed = true;
        public void ForceEndDrag(bool inTutorial) => draggingAllowed = !inTutorial;
    }
}