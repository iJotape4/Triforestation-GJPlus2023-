using Events;
using TMPro;
using UnityEngine;

public class WinPopUp : PopUp
{
    [SerializeField] TextMeshProUGUI scoreText;
    private void Awake()
    {
        EventManager.AddListener<int>(ENUM_GameState.win, OpenPopUp);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<int>(ENUM_GameState.win, OpenPopUp);
    }

    private void OpenPopUp(int eventData)
    {
        scoreText.text = eventData.ToString();
        OpenPopUp(true);
    }
}