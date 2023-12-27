using Events;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent (typeof(TriangularGrid))]
    public class DropGrid : DropView
    {     
        private TriangularGrid grid;

        private void Start()
        {
            grid = GetComponent<TriangularGrid>();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            DominoToken token = eventData.pointerDrag.gameObject.GetComponent<DominoToken>();

            Vector3 currentPos = token.transform.position;

            Vector3Int? triangle = grid.PickTri(currentPos.x , currentPos.z);

            if (triangle == null)
                return;

            Vector2 triangleCenter = grid.TriCenter((Vector3Int)triangle);

            if (token.IsUpwards() && grid.PointsUp((Vector3Int)triangle))
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
    }
}