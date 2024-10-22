using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Terraforming.Dominoes
{
    public class DominoPole : DropView 
    { 
        public SpriteRenderer spriteRenderer;
        public ENUM_Biome biome;
        protected BiomesManager biomesManager;

        public Collider2D poleCollider;
        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            poleCollider = GetComponent<Collider2D>();
            biomesManager = BiomesManager.Instance;
        }

    private void SetActive(bool eventData)
    {
        if (transform.parent.parent == null)
            return;

        spriteRenderer.enabled = eventData;
        poleCollider.enabled = eventData;
    }

    public void TurnColliderOn()
        {
            poleCollider.enabled = true;
        }

        public void TurnColliderOff()
        {
            poleCollider.enabled = true;
        }

        public virtual void AssignBiome()
        {
            biome = UsefulMethods.GetRandomFromEnum<ENUM_Biome>();
            SetBioma();
        }

        public virtual void AssignBiome(ENUM_Biome _biome)
        {
            biome = _biome;
            SetBioma();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log($"OnDrop {eventData.position}", gameObject);
            RestoreHoveredObjectScale(eventData);
        }

        protected void SetBioma()
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

        protected int GetBiomeIndex(ENUM_Biome biome)
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