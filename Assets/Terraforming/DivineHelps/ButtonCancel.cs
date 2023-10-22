using Events;

public class ButtonCancel : UIButton
{
    protected override void ClickButtonMethod()
    {
        EventManager.Dispatch(ENUM_DominoeEvent.cancelEvent);
    }
}