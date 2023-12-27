using Events;
using Terraforming;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropTile : DropView
{
    public bool isUpwards = true;

    public override void OnDrop(PointerEventData eventData)
    {
        DominoToken token = eventData.pointerDrag.gameObject.GetComponent<DominoToken>();

        if(token.IsUpwards() == isUpwards)
        {
            eventData.pointerDrag.GetComponent<DragView>().ValidateDrop();
            eventData.pointerDrag.transform.position = transform.position;
            token.TurnOnColliders();
            gameObject.SetActive(false);
            EventManager.Dispatch(ENUM_DominoeEvent.dominoDroppedEvent, token);
            RestoreHoveredObjectScale(eventData);
        }
    }
}
