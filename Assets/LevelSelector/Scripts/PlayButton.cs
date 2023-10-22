using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelSelector
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField, Scene] string sceneName;
        [SerializeField] PregamePopUp pregamePopUp;
        [SerializeField] public Button playButton;

        private void Start()
        {
            pregamePopUp = GetComponentInParent<PregamePopUp>();
            playButton =    GetComponent<Button>();

            playButton.onClick.AddListener( ClickButton);
            pregamePopUp.enablePlayButton += SwitchEnablePlayButton;
    
        }
        
        private void SwitchEnablePlayButton(bool enable)=>
            playButton.enabled= enable;

        private void ClickButton()
        {
            SceneManager.LoadScene(sceneName);
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