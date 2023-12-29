using Events;
public class SwapBiomeButton : UIButton
{
    protected override void ClickButtonMethod()
    {
        EventManager.Dispatch(ENUM_DominoeEvent.startOrRestartSwapEvent);
        EventManager.Dispatch(ENUM_DominoeEvent.setActivePlayFieldObjects, false);
        DivineHelpsManager.Instance.DisableDivineHelpsPanel();
    }
}