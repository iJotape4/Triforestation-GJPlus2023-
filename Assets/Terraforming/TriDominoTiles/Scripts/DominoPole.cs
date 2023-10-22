using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming.Dominoes
{
    public class DominoPole : DropView 
    { 
        SpriteRenderer spriteRenderer;
        ENUM_Biome biome;
        BiomesManager biomesManager;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            biomesManager = BiomesManager.Instance;
        }

        private void Start()
        {
            biome = UsefulMethods.GetRandomFromEnum<ENUM_Biome>();
            SetBioma();
        }

        public void SetBioma()
        {
            //TODO bioma 
            spriteRenderer.sprite = biomesManager.biomesSprites[(int)biome];              
        }


        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log($"OnDrop {eventData.position}", gameObject);
            RestoreHoveredObjectScale(eventData);
        }
    }
}