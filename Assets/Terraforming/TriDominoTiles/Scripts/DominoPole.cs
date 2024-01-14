using Events;
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
        public Transform centroid;
        public MeshRenderer meshRenderer;
        public ENUM_PolePosition position;
        public ENUM_Biome biome;
        protected BiomesManager biomesManager;

        public Collider poleCollider;
        public bool occupuied { get; private set; }
        [HideInInspector] public Animal animalData;
        [HideInInspector] public Group group { get; set; }

        // Define the first color with hexadecimal code #6481FF
        public static Color blue = new Color(0x64 / 255f, 0x81 / 255f, 0xFF / 255f);
        // Define the second color with hexadecimal code #FF6D6D
        public static Color red = new Color(0xFF / 255f, 0x6D / 255f, 0x6D / 255f);
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
            poleCollider.enabled = false;
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
            AnimalUI token = eventData.pointerDrag.gameObject.GetComponent<AnimalUI>();

            if (token == null)
               return;

            if(occupuied)
            {
                token.InvalidDrop();
                return;
            }

            //Check when the animal is a condor.
            if ((int)token.animal.biome == -1)
            {
                if ((int)biome == -1f)             
                    OccupyPole(token.spawnedPrefab, transform);              
                else
                {
                    token.InvalidDrop();
                    return;
                }
            }
            else if(token.animal.chainLevel == ENUM_FoodChainLevel.Bug && biome == 0)
            {
                PunishToken thisPunishToken = GetComponent<PunishToken>();
                if (thisPunishToken.savable)
                {
                    OccupyPole(token.spawnedPrefab, transform);
                    thisPunishToken.RecoverEcosystem(token.animal.biome, token.spawnedPrefab);
                }
                else
                {
                    token.InvalidDrop();
                    return;
                }              
            }
            else if(biome == 0)
            {
                token.InvalidDrop();
                return;
            }
            //Check when animal is not a condor
            else if ((biome & token.animal.biome) == biome)
            {
                //Check current population in the group
                if (group.AddAnimal(token.animal.chainLevel))
                    OccupyPole(token.spawnedPrefab, centroid);
                else
                    token.InvalidDrop();

                return;
            }
            else
            {
                token.InvalidDrop();
                return;
            }
                
            //TODO: Add scoring
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            RestoreColor();          
        }

        public void RestoreColor() => meshRenderer.material.color = Color.white;

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
       
        public void CheckBiome(ENUM_Biome _biome)
        {
            if (biome == 0)
                Debug.Log("Biome: " + biome);

            if (occupuied || _biome != biome)
            {
                ChangeMeshColor(red);
                if (biome == 0)
                Debug.Log("RED");
            }
            else
            {
                ChangeMeshColor(blue);
            }
        }

        private void ChangeMeshColor(Color color)
        {
            meshRenderer.material.color = color;
        }

        public void OccupyPole(GameObject animal, Transform position)
        {
            occupuied = true;
            animal.transform.position = position.position;

            AnimalBehaviour animalBehaviour = animal.GetComponent<AnimalBehaviour>();
            animalBehaviour.OnAnimalDroped();
            animalData = animalBehaviour.animalData;
            EventManager.Dispatch(ENUM_AnimalEvent.animalDroped);
        }

        /// <summary>
        /// This method should only be called by the PunishToken script
        /// </summary>
        public void MarkPunishTokenPoleAsOccupied()
        {
            occupuied = true;
        }
    }
}