using UnityEngine.SceneManagement;

public class LevelWinPopUpButton : ClosePopUPButton
{
    const string sceneName = "LevelSelector";
    protected override void ClickButtonMethod()
    {
        base.ClickButtonMethod();
        SceneManager.LoadScene(sceneName);
        //TODO: Add win screen?
    }
}