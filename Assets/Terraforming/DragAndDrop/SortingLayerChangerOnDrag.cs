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
        }

        private void HandleDragEnded(PointerEventData pointerEventData) =>
            sortingLayerModifiable.GetType().GetProperty("sortingLayerName").SetValue(sortingLayerModifiable, initialSortingLayerName, null);
    }
}
