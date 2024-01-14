using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene3DButton : MainMenuButton
{
    [SerializeField, Scene] private string sceneName;
    [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    protected override void ExecuteButtonMethod()
    {
        ChangeScene();
    }
    protected virtual void ChangeScene() => SceneManager.LoadScene(sceneName, loadSceneMode);
}