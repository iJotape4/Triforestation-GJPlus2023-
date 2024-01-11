using Events;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsUIPanel : MonoBehaviour
{
    AnimalsManager animalsManager;
    List<Animal> animalsList = new List<Animal>();
    [SerializeField] GameObject animalUIPicturePrefab;
    [SerializeField] Animator anim;

    const string animationParam = "Enable";

    private void Awake()
    {
        EventManager.AddListener(ENUM_GameState.poolAnimals, PoolAnimals);
        EventManager.AddListener(ENUM_GameState.poolDescomposers, PoolDescomposers);

        EventManager.AddListener(ENUM_AnimalEvent.animalPrefabCreated, DisableAnimalsPanel);
        EventManager.AddListener(ENUM_GameState.secondPhaseFinished, DisableAnimalsPanel);
        EventManager.AddListener(ENUM_AnimalEvent.animalPrefabDestroyed, EnableAnimalsPanel);
        EventManager.AddListener(ENUM_AnimalEvent.animalDroped, EnableAnimalsPanel);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.poolAnimals, PoolAnimals);
        EventManager.RemoveListener(ENUM_GameState.poolDescomposers, PoolDescomposers);

        EventManager.RemoveListener(ENUM_AnimalEvent.animalPrefabCreated, DisableAnimalsPanel);
        EventManager.RemoveListener(ENUM_GameState.secondPhaseFinished, DisableAnimalsPanel);
        EventManager.RemoveListener(ENUM_AnimalEvent.animalPrefabDestroyed, EnableAnimalsPanel);
        EventManager.RemoveListener(ENUM_AnimalEvent.animalDroped, EnableAnimalsPanel);
    }

    private void EnableAnimalsPanel()
    {
        anim.SetBool(animationParam, true);
    }
    private void DisableAnimalsPanel()
    {
           anim.SetBool(animationParam, false);
    }
    /// <summary>
    /// Checks for the current biomes in the level, then search the animals that can be found in the biomes. Finally, create the UI pictures for each animal
    /// </summary>
    [ContextMenu("PoolAnimals")]
    private void PoolAnimals()
    {
        DestroyChildren();
        foreach (KeyValuePair<ENUM_Biome, int> pair in GameManager.Instance.biomeCounts)
        {
            if (pair.Value > 0)
            {
                Animal[] animalsInBiome = GetAnimalsBiome(pair.Key);
                foreach (Animal animal in animalsInBiome)
                {
                    if(!animalsList.Contains(animal) && animal.chainLevel != ENUM_FoodChainLevel.Bug)
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
        EnableAnimalsPanel();
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
                animals.Add(animal);               
            }
        }
        return animals.ToArray();
    }

    private void PoolDescomposers()
    {
        DestroyChildren();
        foreach (KeyValuePair<ENUM_Biome, int> pair in GameManager.Instance.biomeCounts)
        {
            if (pair.Value > 0)
            {
                Animal[] animalsInBiome = GetAnimalsBiome(pair.Key);
                foreach (Animal animal in animalsInBiome)
                {
                    if (!animalsList.Contains(animal)  && animal.chainLevel == ENUM_FoodChainLevel.Bug)
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

        EnableAnimalsPanel();
    }

    //A method to destroy every child of this  gameobject
    public void DestroyChildren()
    {
        animalsList.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Start()
    {
        animalsManager = AnimalsManager.Instance;
    }
}