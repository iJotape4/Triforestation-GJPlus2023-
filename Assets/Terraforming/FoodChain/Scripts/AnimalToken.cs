using System;
using UnityEngine;

public class AnimalToken : MonoBehaviour
{
    [SerializeField] SpriteRenderer tokenSpr, animalSpr;
    [SerializeField] SpriteRenderer[] biomeSpr;
    [SerializeField] Animal animal;
    [SerializeField] Transform biomesParent;
    AnimalsManager animalsManager;
    private void Awake()
    {
        
    }
    private void Start()
    {
        animalsManager = AnimalsManager.Instance;
        SetToken(animal);
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
        if (index >= 0 && index < animalsManager.biomesSprites.Length)
        {
            return animalsManager.biomesSprites[index];
        }
        else
            return null;
    }

    protected int GetBiomeIndex(ENUM_Biome biome)
    {
        switch (biome)
        {
            case ENUM_Biome.Meadow:
                return 0;
            case ENUM_Biome.Flowers:
                return 0;
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

    void ClearBiomesArray()
    {
        foreach (var spr in biomeSpr)
            spr.sprite = null;
    }
}