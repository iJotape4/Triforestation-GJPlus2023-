using MyBox;
using UnityEngine;

public class BiomesManager : SinglentonParent<BiomesManager> 
{
    [SerializeField, ReadOnly] public Sprite[] biomesSprites;
    const string biomesPath = "Biomes";
    private void Start()
    {
        biomesSprites = Resources.LoadAll<Sprite>(biomesPath);
    }
}
