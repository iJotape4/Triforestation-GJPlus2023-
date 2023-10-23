using DG.Tweening;
using Events;
using System;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming
{
    [RequireComponent(typeof(Collider2D))]
    public class DragView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public event Action<PointerEventData> OnDragBegan;
        public event Action<PointerEventData> OnDragEnded;

        private new Collider2D collider;
        private Camera mainCamera;

        private Vector3 initialDragPosition;
        private Vector3 currentDragPosition;

        private bool validDrop = false;

        bool isDragging, draggingAllowed = true;

        [Header("In Divine Dones Properties")]
        private bool validClickInDivineDone = false;

        private void Awake()
        {
            mainCamera = Camera.main;
            collider = GetComponent<Collider2D>();
            EventManager.AddListener(ENUM_DominoeEvent.startSwapEvent, EnableClicking);
            EventManager.AddListener(ENUM_DominoeEvent.validSwap, DisableClicking);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(ENUM_DominoeEvent.startSwapEvent, EnableClicking);
            EventManager.RemoveListener(ENUM_DominoeEvent.validSwap, DisableClicking);
        }

        private void EnableClicking()
        {
            draggingAllowed = false;
            validClickInDivineDone = true;
        }

        private void DisableClicking()
        {
            draggingAllowed = true;
            validClickInDivineDone = false;
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
            if (validDrop)
                Drop();
            else
                ReturnToPosition();


            OnDragEnded?.Invoke(eventData);
        }
        public void ValidateDrop()
        {
            validDrop = true;
        }

        void ReturnToPosition()
        {
            transform.position = initialDragPosition;
            collider.enabled = true;
            isDragging = false;
        }

        void Drop()
        {
            // Check if there is a parent GameObject
            if (gameObject.transform.parent != null)
            {
                // Get the parent GameObject
                GameObject parentObject = gameObject.transform.parent.gameObject;

                // Change the layer of the parent GameObject to the default layer
                parentObject.layer = LayerMask.NameToLayer("Default");
                EventManager.RemoveListener(ENUM_DominoeEvent.startSwapEvent, EnableClicking);
                EventManager.RemoveListener(ENUM_DominoeEvent.validSwap, DisableClicking);
                draggingAllowed = false;

 
            }
        }

        public void ForceAllowDragging() => draggingAllowed = true;
        public void ForceEndDrag(bool inTutorial) => draggingAllowed = !inTutorial;

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!validClickInDivineDone) return;

            EventManager.Dispatch(ENUM_DominoeEvent.selectCardToSwipeEvent,GetComponent<DominoToken>());
        }
    }
}