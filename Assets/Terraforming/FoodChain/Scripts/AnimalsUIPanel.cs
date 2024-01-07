using Events;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsUIPanel : MonoBehaviour
{
    AnimalsManager animalsManager;
    List<Animal> animalsList = new List<Animal>();
    [SerializeField] GameObject animalUIPicturePrefab;
    [SerializeField] Animator anim;

    private void Awake()
    {
        EventManager.AddListener(ENUM_GameState.poolAnimals, PoolAnimals);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.poolAnimals, PoolAnimals);
    }
    /// <summary>
    /// Checks for the current biomes in the level, then search the animals that can be found in the biomes. Finally, create the UI pictures for each animal
    /// </summary>
    [ContextMenu("PoolAnimals")]
    private void PoolAnimals()
    {
        foreach (KeyValuePair<ENUM_Biome, int> pair in GameManager.Instance.biomeCounts)
        {
            if (pair.Value > 0)
            {
                Animal[] animalsInBiome = GetAnimalsBiome(pair.Key);
                foreach (Animal animal in animalsInBiome)
                {
                    if(!animalsList.Contains(animal))
                    animalsList.Add(animal);
                }
            }
        }

        //Create UI pictures for each animal
        for (int i = 0; i < animalsList.Count; i++)
        {
            GameObject newAnimal = Instantiate(animalUIPicturePrefab, gameObject.transform);
            newAnimal.GetComponent<AnimalUI>().SetAnimal(animalsList[i]);
        }
    }
    /// <summary>
    /// Check in the animals data and returns every animal that can be found in the biome
    /// </summary>
    /// <param name="biome"></param>
    /// <returns></returns>
    Animal[] GetAnimalsBiome(ENUM_Biome biome)
    {
        List<Animal> animals = new List<Animal>();
        foreach (Animal animal in animalsManager.animals)
        {
            if (animal.biome.HasFlag(biome))
            {
                //TODO: add the verification for bugs when available
                //If no acid rain with biomes around, no bugs should be spawned in the UI
                if (!animal.chainLevel.HasFlag(ENUM_FoodChainLevel.Bug))
                {
                    animals.Add(animal);
                }

            }
        }
        return animals.ToArray();
    }

    void Start()
    {
        animalsManager = AnimalsManager.Instance;
    }
}