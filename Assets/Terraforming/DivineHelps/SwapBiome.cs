using Events;

public class SwapBiome : UIButton
{
    protected override void ClickButtonMethod()
    {
        EventManager.Dispatch(ENUM_DominoeEvent.startSwapEvent, true);
        DivineHelpsManager.Instance.DisableDivineHelpsPanel();
    }
}