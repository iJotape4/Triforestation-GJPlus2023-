using UnityEngine;

public class DisableObjectButton : UIButton
{
    [SerializeField] GameObject objectToDisable;

    protected override void ClickButtonMethod()
    {
       objectToDisable.SetActive(false);
    }
}