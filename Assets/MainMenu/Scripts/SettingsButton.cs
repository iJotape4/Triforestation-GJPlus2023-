using UnityEngine;

public class SettingsButton : MainMenuButton
{
    [SerializeField] private GameObject objectToActivate;

    protected override void ExecuteButtonMethod()
    {
        ToggleActivateObject();
    }

    private void ToggleActivateObject()
    {
        objectToActivate.SetActive(!objectToActivate.activeSelf);
    }
}