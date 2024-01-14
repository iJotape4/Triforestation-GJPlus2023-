using UnityEngine;

public class AnimalsPanelPanicButton : UIButton
{
    [SerializeField] AnimalsUIPanel animalsPanel;

    protected override void ClickButtonMethod()
    {
        animalsPanel.EnableAnimalsPanel();
    }
}