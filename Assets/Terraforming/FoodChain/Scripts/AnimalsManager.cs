using MyBox;
using UnityEngine;

public class AnimalsManager : SinglentonParent<AnimalsManager> 
{
    [SerializeField, ReadOnly] public Animal[] animals;
    [SerializeField, ReadOnly] public Sprite[] biomesSprites;
    [SerializeField] public Sprite moorSprite;
    const string animalsPath = "FoodChain";
    const string biomesPath = "Biomes/UI";
    void Start()
    {
        animals = Resources.LoadAll<Animal>(animalsPath); ;
        biomesSprites = Resources.LoadAll<Sprite>(biomesPath);
        moorSprite = biomesSprites[6];
    }
}