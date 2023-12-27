using UnityEngine;
using UnityEngine.EventSystems;

public class DropTile : DropView
{
    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DropedOnTile");
    }
}
