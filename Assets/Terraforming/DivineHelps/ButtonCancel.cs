using Events;
public class ButtonCancel : UIButton
{
    protected override void ClickButtonMethod()
    {
        EventManager.Dispatch(ENUM_DominoeEvent.startOrRestartSwapEvent);
        EventManager.Dispatch(ENUM_SFXEvent.checkSound);
    }
}