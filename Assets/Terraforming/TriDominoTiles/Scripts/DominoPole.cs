using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming.Dominoes
{
    public class DominoPole : DropView 
    { 
        SpriteRenderer spriteRenderer;
        public ENUM_Biome biome;
        BiomesManager biomesManager;

        public Collider2D poleCollider;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            biomesManager = BiomesManager.Instance;
        }

        public void SetBioma()
        {
            //TODO bioma 
            spriteRenderer.sprite = biomesManager.biomesSprites[(int)biome];              
        }

        public void TurnColliderOn()
        {
            poleCollider.enabled = true;
        }

        public void TurnColliderOff()
        {
            poleCollider.enabled = true;
        }

        public void AssignBiome()
        {
            biome = UsefulMethods.GetRandomFromEnum<ENUM_Biome>();
            SetBioma();

        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log($"OnDrop {eventData.position}", gameObject);
            RestoreHoveredObjectScale(eventData);
        }
    }
}