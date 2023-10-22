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
        }

        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log($"OnDrop {eventData.position}", gameObject);
            RestoreHoveredObjectScale(eventData);
        }

        public void SetBioma()
        {
            int index = GetBiomeIndex(biome);
            if (index >= 0 && index < biomesManager.biomesSprites.Length)
            {
                spriteRenderer.sprite = biomesManager.biomesSprites[index];
            }
            else
            {
                print("El bioma raro fue: " + biome);
                Debug.LogError("Invalid biome index: " + index);
            }
        }

        private int GetBiomeIndex(ENUM_Biome biome)
        {
            switch (biome)
            {
                case ENUM_Biome.Meadow:
                    return 0;
                case ENUM_Biome.Flowers:
                    return 1;
                case ENUM_Biome.Sweetwater:
                    return 2;
                case ENUM_Biome.Forest:
                    return 3;
                case ENUM_Biome.Jungle:
                    return 4;
                case ENUM_Biome.Mountain:
                    return 5;
                default:
                    return -1; // Handle other cases or error condition.
            }
        }
    }
}