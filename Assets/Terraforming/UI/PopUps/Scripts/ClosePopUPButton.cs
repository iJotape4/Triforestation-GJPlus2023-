using UnityEngine;

public class ClosePopUPButton : UIButton
{
    [SerializeField] Animator popUp;
    const string animationParamTrigger = "Enable";

    protected override void ClickButtonMethod()
    {
        if (popUp != null)
            popUp.SetBool(animationParamTrigger,false);
        else
            gameObject.SetActive(false);
    }
}