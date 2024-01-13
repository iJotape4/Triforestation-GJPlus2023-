using Events;
using UnityEngine;

public class BonusMoorButton : UIButton
{
    public GameObject bonusMoorPrefab; 
    protected override void ClickButtonMethod()
    {
        EventManager.Dispatch(ENUM_DominoeEvent.startBonusMoorEvent, bonusMoorPrefab);
        EventManager.Dispatch(ENUM_DominoeEvent.tradeCardsForMoor);
        EventManager.Dispatch(ENUM_DominoeEvent.finishPunishEvent);
        DivineHelpsManager.Instance.DisableDivineHelpsPanel();
    }

}
