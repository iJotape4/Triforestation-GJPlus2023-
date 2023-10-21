using UnityEngine;

namespace Terraforming.Dominoes
{
    public class DominoPole : MonoBehaviour
    {

        SpriteRenderer spriteRenderer;
        public ENUM_Biome biome;
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
    }
}