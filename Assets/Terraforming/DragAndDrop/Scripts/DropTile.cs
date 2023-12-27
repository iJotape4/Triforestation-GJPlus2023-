using Events;
using Terraforming;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropTile : DropView
{
    public bool isUpwards = true;
    MeshRenderer meshRenderer;
    private void Start() => meshRenderer = GetComponent<MeshRenderer>();

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
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (eventData.pointerDrag == null)
            return;

        DominoToken token = eventData.pointerDrag.gameObject.GetComponent<DominoToken>();

        if (token == null)
            return;

        if (token.IsUpwards() == isUpwards)
            ChangeColorMaterial(Color.yellow);
        else
            ChangeColorMaterial(Color.red);
        
            EventManager.Dispatch(ObjectInteractionEvents.Hover);
            ChangeAlphaMaterial(52f / 255f);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ChangeAlphaMaterial(0);
    }

    public void ChangeColorMaterial(Color color) => meshRenderer.material.color = color;
    //Change the material alpha
    public void ChangeAlphaMaterial(float alpha)
    {
        Color color = meshRenderer.material.color;
        color.a = alpha;
        meshRenderer.material.color = color;
    }
}