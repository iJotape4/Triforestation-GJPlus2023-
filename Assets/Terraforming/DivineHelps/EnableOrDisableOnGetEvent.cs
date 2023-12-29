using Events;
using UnityEngine;

public class EnableOrDisableOnGetEvent : MonoBehaviour
{
    [SerializeField] ENUM_DominoeEvent ev = ENUM_DominoeEvent.startOrRestartSwapEvent;
    [SerializeField] ENUM_DominoeEvent disableEvent;
    private void Awake()
    {
        if(disableEvent != 0)
        {
            EventManager.AddListener(ev, EnableObject);
            EventManager.AddListener(disableEvent, DisableObject);
        }
        else
        {
            EventManager.AddListener(ev, SetActive);
            EventManager.AddListener<bool>(ev, SetActive);
        }       
    }

    private void OnDestroy()
    {
        if (disableEvent != 0)
        {
            EventManager.RemoveListener(ev, EnableObject);
            EventManager.RemoveListener(disableEvent, DisableObject);
        }
        else
        {
            EventManager.RemoveListener(ev, SetActive);
            EventManager.RemoveListener<bool>(ev, SetActive);
        }
    }

    private void SetActive() => gameObject.SetActive(gameObject.activeSelf ? false : true);
    private void SetActive(bool eventData) => gameObject.SetActive(eventData);
    private void EnableObject() => gameObject.SetActive(true);
    private void DisableObject() => gameObject.SetActive(false);
    public void SetEvent(ENUM_DominoeEvent _ev) => ev = _ev;
}