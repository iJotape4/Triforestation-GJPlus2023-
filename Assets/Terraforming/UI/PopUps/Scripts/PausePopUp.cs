using Events;
using UnityEngine;
using UnityEngine.UI;

public class PausePopUp : MonoBehaviour
{
    [SerializeField] GameObject pausePopUp;
    [SerializeField] GameObject RUSurePopUp;
    [SerializeField] Button resumeButton;
    [SerializeField] Button exitButton;

    private void Awake()
    {
        EventManager.AddListener(ENUM_InputEvent.PauseMenu, OnPause);
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(OnPause);
        exitButton.onClick.AddListener(OnExit);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_InputEvent.PauseMenu, OnPause);
    }

    private void OnPause()
    {
        if (RUSurePopUp.activeSelf)
            return;
        Debug.Log("Pause");
        Debug.Log(pausePopUp.activeSelf);
        pausePopUp.SetActive(!pausePopUp.activeSelf);
    }

    private void OnExit()
    {
        RUSurePopUp.SetActive(!RUSurePopUp.activeSelf);
    }  
}