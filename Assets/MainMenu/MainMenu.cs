using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;

public class MainMenu : MonoBehaviour
{
    [SerializeField, Scene] string sceneName;
    public void exit()
    {
        Application.Quit();
    }

    public void play()
    {
        SceneManager.LoadScene(sceneName);
    }
}
