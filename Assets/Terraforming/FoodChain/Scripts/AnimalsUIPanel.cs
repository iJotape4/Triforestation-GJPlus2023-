using Events;
using System.Collections;
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

    [ContextMenu("PoolAnimals")]
    private void PoolAnimals()
    {
        foreach (KeyValuePair<ENUM_Biome, int> pair in GameManager.Instance.biomeCounts)
        {
            Animal[] animalsInBiome = GetAnimalsBiome(pair.Key);
            for (int i = 0; i < pair.Value; i++)
            {
                animalsList.Add(GetRandomAnimalFromList(animalsInBiome));
            }
        }

        for (int j = 0; j < animalsList.Count; j++)
        {
            GameObject newAnimal = Instantiate(animalUIPicturePrefab, gameObject.transform);
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
        foreach (Animal animal in animalsManager.animals)
        {
            if (animal.biome.HasFlag(biome))
            {
                if (animal.chainLevel.HasFlag(ENUM_FoodChainLevel.Prey) || animal.chainLevel.HasFlag(ENUM_FoodChainLevel.Predator))
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
        newAnimal.GetComponent<AnimalUI>().SetAnimal(animalsList[index]);
    }

    void Start()
    {
        animalsManager = AnimalsManager.Instance;
    }
}