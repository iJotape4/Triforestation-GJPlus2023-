using MyBox;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BiomesManager : SinglentonParent<BiomesManager> 
{
    [SerializeField, ReadOnly] public Material[] biomesMaterials;
    [SerializeField] public Material[] colorsMaterials;

    const string biomesPath = "Biomes";
    private void Start()
    {
        biomesMaterials = Resources.LoadAll<Material>(biomesPath);
    }

    public List<Material> GetBiomeMaterials(int index)
    {
        Material[] materials = new Material[2];
        materials[0] = biomesMaterials[index];
        materials[1] = colorsMaterials[index];
        return materials.ToList();
    }
}