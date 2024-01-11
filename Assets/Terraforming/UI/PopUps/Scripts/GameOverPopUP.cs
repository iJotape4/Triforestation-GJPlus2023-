using Events;

public class GameOverPopUP : PopUp
{
    private void Awake()
    {
        EventManager.AddListener(ENUM_GameState.loose, OpenPopUp);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.loose, OpenPopUp);
    }

    private void OpenPopUp()
    {
       OpenPopUp(true);
    }
}