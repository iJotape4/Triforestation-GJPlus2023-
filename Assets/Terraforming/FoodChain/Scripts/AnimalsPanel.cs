using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class AnimalsPanel : MonoBehaviour
{
    [SerializeField, ReadOnly] public Dictionary<ENUM_Biome, int> biomeCounts = new Dictionary<ENUM_Biome, int>()
    {
        {ENUM_Biome.Meadow, 10},     
    };

    AnimalsManager animalsManager;
    List<Animal> animalsList = new List<Animal>();
    [SerializeField] GameObject animalTokenPrefab;
    [SerializeField] Transform[] childs;
    void Start()
    {
        animalsManager = AnimalsManager.Instance;
        childs = GetComponentsInChildren<Transform>();
        PoolAnimals();
    }
    
    void PoolAnimals()
    {
        foreach (KeyValuePair< ENUM_Biome,int> pair in biomeCounts)
        {
            Animal[] animalsInBiome = GetAnimalsBiome(pair.Key);
            for(int i=0; i < pair.Value; i++)
            {
              animalsList.Add(GetRandomAnimalFromList(animalsInBiome));
            }
        }

        for (int j = 0; j<animalsList.Count;j++)
        {
            GameObject newAnimal = Instantiate(animalTokenPrefab, childs[j+2]);
            StartCoroutine(AnimalSetToken(newAnimal, j)); 
        }
    }
   
    Animal GetRandomAnimalFromList(Animal[] animals)
    {
        int r = UnityEngine.Random.Range(0, animals.Length);
        return animals[r];
    }

    Animal[] GetAnimalsBiome(ENUM_Biome biome)
    {
       List<Animal> animals = new List<Animal>();
        foreach(Animal animal in animalsManager.animals)
        {
            if(animal.biome.HasFlag(biome))
            {
                if(animal.chainLevel.HasFlag(ENUM_FoodChainLevel.Prey) || animal.chainLevel.HasFlag(ENUM_FoodChainLevel.Predator))
                {
                   animals.Add(animal);
                }
            }
        }
        return animals.ToArray();
    }

    IEnumerator AnimalSetToken(GameObject newAnimal, int index)
    {
        yield return new WaitForSeconds(2f);
        newAnimal.GetComponent<AnimalToken>().SetToken(animalsList[index]);
    }
}