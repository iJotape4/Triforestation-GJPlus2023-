using Events;
using MyBox;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelSelector
{
    public class PlayButton : MonoBehaviour
    {
        public void ClickButton()
        {

            StartCoroutine(PlayCourutine());
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }


        private IEnumerator PlayCourutine()
        {
            EventManager.Dispatch(ENUM_SFXEvent.PlaySound);
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene("SwapPoles");
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