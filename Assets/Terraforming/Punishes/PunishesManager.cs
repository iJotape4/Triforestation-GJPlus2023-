using Events;
using UnityEngine;

public class PunishesManager : MonoBehaviour
{
    private void Awake()
    {
        EventManager.AddListener(ENUM_DominoeEvent.punishEvent, TriggerRandomPunish);
    }


    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_DominoeEvent.punishEvent, TriggerRandomPunish);
    }
    private void TriggerRandomPunish()
    {
        //TODO : Trigger random Punish ( maybe acid rain for the jam) After punish animation, trigger the selectDone event
        EventManager.Dispatch(ENUM_DominoeEvent.selectDoneEvent);
    }
}