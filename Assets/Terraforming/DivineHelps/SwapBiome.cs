using Events;

public class SwapBiome : UIButton
{
    protected override void ClickButtonMethod()
    {
        EventManager.Dispatch(ENUM_DominoeEvent.startSwapEvent);
        EventManager.Dispatch(ENUM_DominoeEvent.setActivePlayFieldObjects, false);
        DivineHelpsManager.Instance.DisableDivineHelpsPanel();
    }
}