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