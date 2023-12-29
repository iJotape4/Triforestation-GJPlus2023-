using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum ENUM_PolePosition
{
    Position1 = 1,
    Position2 = 2,
    Position3 = 3
}

namespace Terraforming.Dominoes
{
    public class DominoPole : DropView 
    {
        public Transform pivot;
        public MeshRenderer meshRenderer;
        public ENUM_PolePosition position;
        public ENUM_Biome biome;
        protected BiomesManager biomesManager;



        public Collider poleCollider;
        protected virtual void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            poleCollider = GetComponent<Collider>();
            biomesManager = BiomesManager.Instance;
        }

        private void SetActive(bool eventData)
    {
        if (transform.parent.parent == null)
            return;

        meshRenderer.enabled = eventData;
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
            if (biome == 0)
                return;

            AnimalToken token = eventData.pointerDrag.gameObject.GetComponent<AnimalToken>();

            if (token == null)
                return;

            if ( (token.animal.biome &  biome ) == biome)
            {
                token.GetComponent<DragView>().ValidateDrop();
                poleCollider.enabled= false;
            }
        }

        protected void SetBioma()
        {
            int index = GetBiomeIndex(biome);
            if (index >= 0 && index < biomesManager.biomesMaterials.Length)
            {
                //TODO -> change for mesh renderer
                // spriteRenderer.sprite = biomesManager.biomesSprites[index];

                List<Material> materials = biomesManager.GetBiomeMaterials(index);
                materials.Add(meshRenderer.materials[2]);

                meshRenderer.SetMaterials(materials);
                //Debug.Log("SetBioma: " + biome+ "and material" + biomesManager.biomesMaterials[index]);
            }
            else
            {
                print("El bioma raro fue: " + biome);
                Debug.LogError("Invalid biome index: " + index, gameObject);
            }
        }

        protected int GetBiomeIndex(ENUM_Biome biome)
        {
            int biomeID = biome switch
            {
                ENUM_Biome.Jungle => 0,
                ENUM_Biome.Forest => 1,
                ENUM_Biome.Mountain => 2,
                ENUM_Biome.Savannah => 3,
                ENUM_Biome.Meadow => 4,
                ENUM_Biome.Sweetwater => 5,
                //ENUM_Biome.SaltyWater => 6,
                //ENUM_Biome.Snowy => 7,
                //ENUM_Biome.Volcano => 8,
                //ENUM_Biome.Flowers => 9,
                //ENUM_Biome.Desert => 10,
                //ENUM_Biome.Flat => 11,
                _ => -1, // Handle other cases or error condition.
            };
            return biomeID;
        }
    }
}