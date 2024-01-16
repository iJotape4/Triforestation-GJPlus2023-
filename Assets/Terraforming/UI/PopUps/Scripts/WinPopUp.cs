using Events;
using TMPro;
using UnityEngine;

public class WinPopUp : PopUp
{
    [SerializeField] TextMeshProUGUI scoreText;
    private void Awake()
    {
        EventManager.AddListener(ENUM_GameState.secondPhaseFinished, OpenPopUp);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.secondPhaseFinished, OpenPopUp);
    }

    private void OpenPopUp()
    {
        //TODO: Improve this
        scoreText.text = FindObjectOfType<ScoreManager>().GetScore().ToString();
        OpenPopUp(true);
    }
}