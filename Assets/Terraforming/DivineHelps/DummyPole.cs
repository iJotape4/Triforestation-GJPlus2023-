using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;

public class DummyPole : DominoPole, IPointerDownHandler
{
    public bool selected;
    DummyDominoToken token;
    protected override void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        poleCollider = GetComponent<Collider2D>();
        biomesManager = BiomesManager.Instance;
        token = GetComponentInParent<DummyDominoToken>();
    }

    public void AssignBiome(DominoPole pole)
    {
        biome = pole.biome;
        SetBioma();
        TurnColliderOn();
    }

    public override void AssignBiome(ENUM_Biome _biome)
    {
        biome = _biome;
        SetBioma();
        TurnColliderOff();
        Select();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!CheckCanSelect() && !selected)
            return;

        Select();
    }

    public bool CheckCanSelect()
    {
        if (token.GetSelectedPolesCount() >=2f)
            return false;
        return true;
    }

    public void Select()
    {
        selected = selected ? false : true;
        spriteRenderer.color = selected ?  Color.grey :Color.white;
    }
}