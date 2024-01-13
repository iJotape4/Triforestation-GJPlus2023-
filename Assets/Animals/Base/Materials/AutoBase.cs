using UnityEngine;

[ExecuteAlways]
public class AutoBase : MonoBehaviour
{
    Material mat;
    [SerializeField] Color groupColor = Color.white; // Default color can be set in the Unity Editor
    [SerializeField] Texture2D baseTexture; // Default texture can be set in the Unity Editor

    private const string p_GroupColor = "_GroupColor";
    private const string p_BaseTexture = "_BaseTexture";

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
}
