/*using Kitchen;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LevelSelector
{
    public class LevelsList : MonoBehaviour
    {
       [SerializeField] List< LevelData> levels = new List<LevelData>();
        string levelsPath = Path.Combine("Assets", "Kitchen", "Elements", "LevelManager", "Data");

        [SerializeField] LevelButton[] levelButtons;

        private void Awake()
        {
            levels = GetAssetsList<LevelData>(levelsPath);

            levelButtons = new LevelButton[transform.childCount];

            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i] = transform.GetChild(i).GetComponent<LevelButton>();
                levelButtons[i].text.text = (i + 1).ToString();
                levelButtons[i].level = levels[i];
            }
        }

        List<T> GetAssetsList<T>(string folder) where T : ScriptableObject
        {
            string[] dataFiles = Directory.GetFiles(folder, "*.asset");
            List<T> list = new List<T>();

            foreach (string dataFile in dataFiles)
            {
                string assetPath = dataFile.Replace(Application.dataPath, "").Replace('\\', '/');
                T sourceAsset = (T)AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));

                list.Add(sourceAsset);
            }

            return list;
        }

    }
}*/
