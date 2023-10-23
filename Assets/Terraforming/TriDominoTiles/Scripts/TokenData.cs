using UnityEngine;

[CreateAssetMenu(fileName = "Token", menuName = "ScriptableObjects/TokenData", order = 3)]
public class TokenData : ScriptableObject
{
    [SerializeField] public ENUM_Biome[] biomes;
}