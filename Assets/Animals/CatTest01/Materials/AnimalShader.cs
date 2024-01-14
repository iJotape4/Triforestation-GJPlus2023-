using UnityEngine;

[ExecuteInEditMode]
public class AnimalShader : MonoBehaviour
{
    Material animalMat;
    [SerializeField] Texture2D baseColorTexture;
    [Space(10)]
    [SerializeField] bool activeNormal = false;
    [SerializeField] Texture2D normalTexture;
    [Space(10)]
    [SerializeField] bool activeAO = false;
    [SerializeField] Texture2D aoTexture;
    [Space(10)]
    [SerializeField] bool activeAlpha = false;
    [SerializeField] Texture2D alphaTexture;

    private const string p_BaseColor = "_BaseColor";
    private const string p_ActiveNormal = "_ActiveNormal";
    private const string p_Normal = "_Normal";
    private const string p_ActiveAO = "_ActiveAO";
    private const string p_AO = "_AO";
    private const string p_ActiveAlpha = "_ActiveAlpha";
    private const string p_Alpha = "_Alpha";
    


    void Awake()
    {
        animalMat = GetComponent<Renderer>().material;

        SetTextureProperty(p_BaseColor, baseColorTexture);
        SetTextureProperty(p_Normal, normalTexture);
        SetTextureProperty(p_AO, aoTexture);
        SetTextureProperty(p_Alpha, alphaTexture);

        SetBoolProperty(p_ActiveNormal, activeNormal);
        SetBoolProperty(p_ActiveAO, activeAO);
        SetBoolProperty(p_ActiveAlpha, activeAlpha);
    }

    void OnValidate()
    {
        if (Application.isPlaying)
            return;

        SetTextureProperty(p_BaseColor, baseColorTexture);
        SetTextureProperty(p_Normal, normalTexture);
        SetTextureProperty(p_AO, aoTexture);
        SetTextureProperty(p_Alpha, alphaTexture);

        SetBoolProperty(p_ActiveNormal, activeNormal);
        SetBoolProperty(p_ActiveAO, activeAO);
        SetBoolProperty(p_ActiveAlpha, activeAlpha);
    }

    void Start()
    {
        // You can add additional runtime logic here if needed
    }

    // Helper method to set a texture property on the material
    void SetTextureProperty(string propertyName, Texture2D texture)
    {
        if (animalMat.HasProperty(propertyName))
        {
            animalMat.SetTexture(propertyName, texture);
        }
        else
        {
            Debug.LogError("Shader does not have the specified parameter: " + propertyName);
        }
    }

    // Helper method to set a boolean property on the material
    void SetBoolProperty(string propertyName, bool value)
    {
        if (animalMat.HasProperty(propertyName))
        {
            animalMat.SetInt(propertyName, value ? 1 : 0);
        }
        else
        {
            Debug.LogError("Shader does not have the specified parameter: " + propertyName);
        }
    }
}
