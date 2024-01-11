using UnityEngine;

public class ClosePopUPButton : UIButton
{
    [SerializeField] Animator popUp;
    const string animationParamTrigger = "Enable";

    protected override void ClickButtonMethod()
    {
        popUp.SetBool(animationParamTrigger,false);
    }
}