using Events;
using MyBox;
using System;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent (typeof(TriangularGrid))]
    public class DropView : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<PointerEventData> OnPointerEntered;
        public event Action<PointerEventData> OnPointerExited;

        public Func<PointerEventData, bool> IsDraggedObjectInteractableWithMe;

        private Vector3 initialHoveredObjectScale;
        private static GameObject currentPointerDrag;

        private TriangularGrid grid;

        private void Start()
        {
            grid = GetComponent<TriangularGrid>();
        }
        virtual public void OnDrop(PointerEventData eventData)
        {
            DominoToken token = eventData.pointerDrag.gameObject.GetComponent<DominoToken>();

            Vector3 currentPos = token.transform.position;

            Vector3Int triangle = grid.PickTri(currentPos.x , currentPos.z);

            Vector2 triangleCenter = grid.TriCenter(triangle.x, triangle.y, triangle.z);

            if ((token.IsUpwards() && grid.PointsUp(triangle.x, triangle.y, triangle.z)))
            {
                eventData.pointerDrag.GetComponent<DragView>().ValidateDrop();
                eventData.pointerDrag.transform.position = new Vector3(triangleCenter.x, transform.position.y, triangleCenter.y);
                token.TurnOnColliders();
                EventManager.Dispatch(ENUM_DominoeEvent.dominoDroppedEvent, token);
                RestoreHoveredObjectScale(eventData);
            }

            // CODE USED FOR THE 2D VERSION OF TREEFORESTATION
            /*
            //Debug.Log($"OnDrop {eventData.position}", gameObject);
            DominoToken token = eventData.pointerDrag.gameObject.GetComponent<DominoToken>();
            token.transform.position = transform.position;
            if (token.IsValidRotation(transform.localEulerAngles.z) && token.IsValidBiome())
            {
                eventData.pointerDrag.GetComponent<DragView>().ValidateDrop();
                eventData.pointerDrag.transform.position = transform.position;
                token.TurnOnColliders();
                gameObject.SetActive(false);
                EventManager.Dispatch(ENUM_DominoeEvent.dominoDroppedEvent, token);
                RestoreHoveredObjectScale(eventData);
            }
            else
            {
                EventManager.Dispatch(ENUM_SFXEvent.ErrorSound);
            }
            */
        }

        virtual public void OnPointerEnter(PointerEventData eventData)
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

        virtual public void OnPointerExit(PointerEventData eventData)
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
}