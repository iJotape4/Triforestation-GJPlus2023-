using Events;
public class ButtonCancel : UIButton
{
    protected override void ClickButtonMethod()
    {
        EventManager.Dispatch(ENUM_DominoeEvent.cancelEvent);
        EventManager.Dispatch(ENUM_SFXEvent.checkSound);
    }
}