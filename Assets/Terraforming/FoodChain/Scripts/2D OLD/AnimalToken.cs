using System;
using UnityEngine;
namespace FoodChain.Old
{ 
    public class AnimalToken : MonoBehaviour
{
    [SerializeField] SpriteRenderer tokenSpr, animalSpr;
    [SerializeField] SpriteRenderer[] biomeSpr;
    [SerializeField] public Animal animal;
    [SerializeField] Transform biomesParent;
    AnimalsManager animalsManager;

    private void Start()
    {
        animalsManager = AnimalsManager.Instance;
        //SetToken(animal);
    }

    public void SetToken(Animal an)
    {
        animal = an;
        SetToken();
    }

    [ContextMenu("SetToken")]
    public void SetToken()
    {
        animalSpr.sprite = animal.sprite;
        tokenSpr.color = GetTokenColor();
        SetBiomesList();
    }

    Color GetTokenColor()
    {
        Color color;
        color = animal.chainLevel switch
        {
            ENUM_FoodChainLevel.AnimalKing => Color.red,
            ENUM_FoodChainLevel.Predator => Color.yellow,
            ENUM_FoodChainLevel.Prey => Color.white,
            ENUM_FoodChainLevel.Bug => Color.magenta,
            _ => Color.white
        }; 
        return color;
    }

    void SetBiomesList()
    {
        ClearBiomesArray();
        if((int)animal.biome == -1)
        {
            biomesParent.GetChild(0).GetComponent<SpriteRenderer>().sprite = animalsManager.moorSprite;
            return;
        }

        int index = 0;
        foreach (ENUM_Biome flag in Enum.GetValues(typeof(ENUM_Biome)))
        {
            if (animal.biome.HasFlag(flag))
            {
                AddBiome(flag, index);
                index++;
            }
        }
    }

    void AddBiome(ENUM_Biome biome, int index)
    {
        biomesParent.GetChild(index).GetComponent<SpriteRenderer>().sprite = GetBiomeSprite(biome);
    }

    Sprite GetBiomeSprite(ENUM_Biome biome)
    {
        int index = GetBiomeIndex(biome);
        if (index >= 0 && index < AnimalsManager.Instance.biomesSprites.Length)
        {
            return animalsManager.biomesSprites[index];
        }
        else
            return null;
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

    void ClearBiomesArray()
    {
        foreach (var spr in biomeSpr)
            spr.sprite = null;
    }
}
}