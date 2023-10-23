using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelSelector
{
    public class PlayButton : MonoBehaviour
    {
        

        public void ClickButton()
        {
            SceneManager.LoadScene("SwapPoles");
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    GameObject[] gameObjects = scene.GetRootGameObjects();
        //    foreach (GameObject gameObject in gameObjects)
        //    {
        //        if (gameObject.GetComponent<LevelInstantiator>() != null)
        //        {
        //            gameObject.GetComponent<LevelInstantiator>().SetLevelData(pregamePopUp.level);
        //            break;
        //        }
        //    }
        //}
    }
}