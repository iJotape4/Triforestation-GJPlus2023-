using System.IO;
using System.Linq;
using UnityEditor;

public class FoodChainEditorMethods 
{
    [MenuItem("DevTools/SetProperFoodChainLevels")]
    public static void DebugMenuItem()
    {
        const string folderPath = "Assets/Terraforming/FoodChain/Data/";

        if (Directory.Exists(folderPath))
        {
            Animal[] animSO = AssetDatabase.FindAssets("t:Animal", new[] { folderPath })
            .Select(assetGuid => AssetDatabase.LoadAssetAtPath<Animal>(AssetDatabase.GUIDToAssetPath(assetGuid)))
            .ToArray();

            foreach (Animal anim in animSO)            
                anim.SetAliments();           
        }
    }
}