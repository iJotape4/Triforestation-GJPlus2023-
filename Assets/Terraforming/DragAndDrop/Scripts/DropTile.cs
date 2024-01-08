using Events;
using Terraforming;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropTile : TriangularGrid
{
    public bool isUpwards = true;
    MeshRenderer meshRenderer;
    public Vector3Int intCenter;
    public MeshCollider meshCollider { get; private set; }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        ChangeAlphaMaterial(0);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        DominoToken token = eventData.pointerDrag.gameObject.GetComponent<DominoToken>();
        //Avoids this method when the dropen thing is not a token ( ex : an animal)
        if (token == null)
            return;

        token.transform.position = transform.position;
        if(token.IsUpwards() == isUpwards && token.IsValidBiome())
        {
            eventData.pointerDrag.GetComponent<DragView>().ValidateDrop();
            eventData.pointerDrag.transform.position = transform.position;
            token.TurnOnColliders();
            GenerateNeighBors();
            //Set the token as child of the grid-cell
            //In this way, the grid cell could be free again if the token is removed by a swipe
            token.SetParent(this.transform);
            meshCollider.enabled = false;
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

    public void GenerateNeighBors()=> GenerateNeighBors(intCenter);
    
    public void OccupyCell() => OccupyCell(intCenter);

    //Method to determine if this cell is free
    public bool IsCellFree() => IsCellFree(intCenter);
}