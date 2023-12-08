using DG.Tweening;
using Events;
using System;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming
{
    [RequireComponent(typeof(Collider))]
    public class DragView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public event Action<PointerEventData> OnDragBegan;
        public event Action<PointerEventData> OnDragEnded;

        private new Collider collider;
        private Camera mainCamera;

        private Vector3 initialDragPosition;
        private Vector3 currentDragPosition;

        private bool validDrop = false;

        bool isDragging, draggingAllowed = true;

        [Header("In Divine Dones Properties")]
        private bool validClickInDivineDone = false;

        float distanceToCamera;
        private void Awake()
        {
            mainCamera = Camera.main;
            collider = GetComponent<Collider>();
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
            Debug.Log($"OnBeginDrag {eventData.position}", gameObject);
            if (!draggingAllowed) return;
            initialDragPosition = transform.position;
            collider.enabled = false;
            isDragging = true;
            OnDragBegan?.Invoke(eventData);
            distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);
            EventManager.Dispatch(ENUM_SFXEvent.dragSound);
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
            Debug.Log($"OnDrag {eventData.position}", gameObject);

             

            // Update the current drag position in 3D space
            currentDragPosition = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x,  eventData.position.y, distanceToCamera));
            transform.position = currentDragPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log($"OnEndDrag {eventData.position}", gameObject);
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