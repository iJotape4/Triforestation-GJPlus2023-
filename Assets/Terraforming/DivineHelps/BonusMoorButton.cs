using Events;
using UnityEngine;

public class BonusMoorButton : UIButton
{
    public GameObject bonusMoorPrefab; 
    protected override void ClickButtonMethod()
    {
        EventManager.Dispatch(ENUM_DominoeEvent.startBonusMoorEvent, bonusMoorPrefab);
        EventManager.Dispatch(ENUM_DominoeEvent.tradeCardsForMoor);
        EventManager.Dispatch(ENUM_DominoeEvent.validSwap);
        DivineHelpsManager.Instance.DisableDivineHelpsPanel();
    }

}
