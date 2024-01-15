using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "ScriptableObjects/Animal", order = 1)]
public class Animal : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] public ENUM_FoodChainLevel chainLevel;
    [SerializeField] public ENUM_FoodChainLevel aliments;
    [SerializeField] public ENUM_Biome biome;
    [SerializeField] public Sprite sprite;
    [SerializeField] private GameObject[] _3dPrefab;
    [SerializeField] public ENUM_SFXAnimals animalSFX;


    [Header("Hexadecimals Colors")]
    // Define the first color with hexadecimal code #6481FF
    public static Color blue = new Color(0x5D / 255f, 0x94 / 255f, 0xFF / 255f);
    // Define the second color with hexadecimal code #FF6D6D
    public static Color red = new Color(0xFF / 255f, 0x55 / 255f, 0x55 / 255f);
    // Define the third color with hexadecimal code #236830
    public static Color green = new Color(0x23 / 255f, 0x68 / 255f, 0x30 / 255f);
    // Define the fourth color with hexadecimal code #797979
    public static Color grey = new Color(0x79 / 255f, 0x79 / 255f, 0x79 / 255f);

    //Create  a method that returns a random prefab from the array
    public GameObject Get3DPrefab()
   {
        int randomIndex = UnityEngine.Random.Range(0, _3dPrefab.Length);
        return _3dPrefab[randomIndex];
   }

    public Color GetChainLevelColor()
    {
        Color color = this.chainLevel switch
        {
            ENUM_FoodChainLevel.AnimalKing => red,
            ENUM_FoodChainLevel.Predator => blue,
            ENUM_FoodChainLevel.Prey => green,
            ENUM_FoodChainLevel.Bug => grey,
            _ => green, // Handle other cases or error condition.
        };
        return color;
    }

    [ContextMenu("SetAliments")]
    public void SetAliments()
    {
        if (!CheckPredatorTypeValid())
            return;

        aliments = 0;
        aliments = chainLevel switch
        {
            ENUM_FoodChainLevel.AnimalKing => aliments |= ENUM_FoodChainLevel.Predator,
            ENUM_FoodChainLevel.Predator => aliments = ENUM_FoodChainLevel.Prey | ENUM_FoodChainLevel.Bug,
            ENUM_FoodChainLevel.Prey => aliments |= ENUM_FoodChainLevel.Bug,
            ENUM_FoodChainLevel.Bug => aliments =0,
            _ => aliments =0
        };;;
    }

    public bool CheckPredatorTypeValid()
    {
        int count = 0;

        foreach (ENUM_FoodChainLevel flag in Enum.GetValues(typeof(ENUM_FoodChainLevel)))
        {
            if (chainLevel.HasFlag(flag))
            {
                count++;
            }
        }

        if (count != 1)
        {
            Debug.LogError("An animal SO with chainLevel invalid detected " + name);
            return false;
        }
        else
            return true;
    }
}