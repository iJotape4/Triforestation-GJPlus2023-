using UnityEngine;

[ExecuteAlways]
public class AutoBase : MonoBehaviour
{
    Material mat;
    [SerializeField] Animal animal;

    [Header("Shader Graph Parameters")]
    [SerializeField] Color groupColor = Color.white; // Default color can be set in the Unity Editor
    [SerializeField] Texture2D baseTexture; // Default texture can be set in the Unity Editor


    private const string p_GroupColor = "_GroupColor";
    private const string p_BaseTexture = "_BaseTexture";

    [Header("Hexadecimals Colors")]
    // Define the first color with hexadecimal code #6481FF
    public static Color blue = new Color(0x5D / 255f, 0x94 / 255f, 0xFF / 255f);
    // Define the second color with hexadecimal code #FF6D6D
    public static Color red = new Color(0xFF / 255f, 0x55 / 255f, 0x55 / 255f);
    // Define the third color with hexadecimal code #62FF7F
    public static Color green = new Color(0x62 / 255f, 0xFF / 255f, 0x7F / 255f);
    // Define the fourth color with hexadecimal code #797979
    public static Color grey = new Color(0x79 / 255f, 0x79 / 255f, 0x79 / 255f);

    void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;

        // Set the GroupColor during Awake if it's not already set in the Unity Editor
        if (!mat.HasProperty(p_GroupColor))
        {
            Debug.LogError("Shader Graph does not have the specified parameter: " + p_GroupColor);
        }
        else
        {
            mat.SetColor(p_GroupColor, groupColor);
        }

        // Set the BaseTexture during Awake if it's not already set in the Unity Editor
        if (!mat.HasProperty(p_BaseTexture))
        {
            Debug.LogError("Shader Graph does not have the specified parameter: " + p_BaseTexture);
        }
        else
        {
            mat.SetTexture(p_BaseTexture, baseTexture);
        }
    }

    void OnValidate()
    {
        // Update the GroupColor in the Unity Editor
        if (mat.HasProperty(p_GroupColor))
        {
            mat.SetColor(p_GroupColor, groupColor);
        }
        else
        {
            Debug.LogError("Shader Graph does not have the specified parameter: " + p_GroupColor);
        }

        // Update the BaseTexture in the Unity Editor
        if (mat.HasProperty(p_BaseTexture))
        {
            mat.SetTexture(p_BaseTexture, baseTexture);
        }
        else
        {
            Debug.LogError("Shader Graph does not have the specified parameter: " + p_BaseTexture);
        }
        UpdateMaterials();
    }

    void Start()
    {
        // Set the GroupColor during runtime if it's not already set
        if (mat.HasProperty(p_GroupColor))
        {
            mat.SetColor(p_GroupColor, groupColor);
        }
        else
        {
            Debug.LogError("Shader Graph does not have the specified parameter: " + p_GroupColor);
        }

        // Set the BaseTexture during runtime if it's not already set
        if (mat.HasProperty(p_BaseTexture))
        {
            mat.SetTexture(p_BaseTexture, baseTexture);
        }
        else
        {
            Debug.LogError("Shader Graph does not have the specified parameter: " + p_BaseTexture);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("UpdateMaterials")]
    void UpdateMaterials()
    {
        UpdateBorderMaterial();
        UpdateCenterMaterial();
    }
    void UpdateCenterMaterial()
    {
        baseTexture = (Texture2D)GetBiomeSprite(animal.biome);
    }

    void UpdateBorderMaterial()
    {
        groupColor = GetChainLevelColor(animal.chainLevel);
    }

    Color GetChainLevelColor(ENUM_FoodChainLevel foodChainLevel)
    {
        Color color = foodChainLevel switch
        {
            ENUM_FoodChainLevel.AnimalKing => red,
            ENUM_FoodChainLevel.Predator => blue,
            ENUM_FoodChainLevel.Prey => green,
            ENUM_FoodChainLevel.Bug => grey,
            _ => green, // Handle other cases or error condition.
        };
        return color;
    }


    Texture GetBiomeSprite(ENUM_Biome biome)
    {
        int index = GetBiomeIndex(biome);
        const string biomesPath = "Biomes";

        Material[] biomesSprites = Resources.LoadAll<Material>(biomesPath);

        if (index >= 0 && index < biomesSprites.Length)
        {
            return biomesSprites[index].mainTexture;
        }
        else
            return null;
    }

    protected int GetBiomeIndex(ENUM_Biome biome)
    {
        int biomeID = biome switch
        {
            ENUM_Biome.Jungle => 0,
            ENUM_Biome.Forest => 1,
            ENUM_Biome.Mountain => 2,
            ENUM_Biome.Savannah => 3,
            ENUM_Biome.Meadow => 4,
            ENUM_Biome.Sweetwater => 5,
            //ENUM_Biome.SaltyWater => 6,
            //ENUM_Biome.Snowy => 7,
            //ENUM_Biome.Volcano => 8,
            //ENUM_Biome.Flowers => 9,
            //ENUM_Biome.Desert => 10,
            //ENUM_Biome.Flat => 11,
            _ => -1, // Handle other cases or error condition.
        };
        return biomeID;
    }
#endif
}