using Events;
using UnityEngine;

public class ButtonConfirm : UIButton
{
   [SerializeField] public ENUM_DominoeEvent currentConfirmableEvent = ENUM_DominoeEvent.confirmSwapEvent;
    protected override void ClickButtonMethod()
    {
        EventManager.Dispatch(currentConfirmableEvent);
        EventManager.Dispatch(ENUM_DominoeEvent.setActivePlayFieldObjects, true);
        EventManager.Dispatch(ENUM_SFXEvent.checkSound);
    }
}