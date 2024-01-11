using UnityEngine.SceneManagement;

public class GameOverPopUpButton : ClosePopUPButton
{
    const string sceneName = "LevelSelector";
    protected override void ClickButtonMethod()
    {
        base.ClickButtonMethod();
        SceneManager.LoadScene(sceneName);

        //TODO: Add a Game Over Screen (?)
    }
}