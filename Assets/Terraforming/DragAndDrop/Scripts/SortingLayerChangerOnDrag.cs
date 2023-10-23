using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming
{
    [RequireComponent(typeof(DragView))]
    public abstract class SortingLayerChangerOnDrag<T> : MonoBehaviour
    {
        protected T sortingLayerModifiable;
        protected string initialSortingLayerName;
        protected SortingLayerName usedSortingLayerOnDrag = SortingLayerName.Dragged;
        protected int dragSortingOrder = 2;
        private DragView dragView;

        private void Awake()
        {
            sortingLayerModifiable = GetComponent<T>();
            dragView = GetComponent<DragView>();
            dragView.OnDragBegan += HandleDragBegan;
            dragView.OnDragEnded += HandleDragEnded;
        }

        private void OnDestroy()
        {
            dragView.OnDragBegan -= HandleDragBegan;
            dragView.OnDragEnded -= HandleDragEnded;
        }

        private void HandleDragBegan(PointerEventData pointerEventData)
        {
            initialSortingLayerName = (string)sortingLayerModifiable.GetType().GetProperty("sortingLayerName").GetValue(sortingLayerModifiable, null);
            sortingLayerModifiable.GetType().GetProperty("sortingLayerName").SetValue(sortingLayerModifiable, usedSortingLayerOnDrag.ToString(), null);

            // Iterate through child objects and set their sorting order to "dragSortingOrder."
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out Renderer childRenderer))
                {
                    childRenderer.sortingOrder = dragSortingOrder;
                }
            }
        }

        private void HandleDragEnded(PointerEventData pointerEventData)
        {
            sortingLayerModifiable.GetType().GetProperty("sortingLayerName").SetValue(sortingLayerModifiable, initialSortingLayerName, null);

            // Iterate through child objects and set their sorting order back to 1.
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out Renderer childRenderer))
                {
                    childRenderer.sortingOrder = 1;
                }
            }
        }
    }
}
