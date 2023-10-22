using Events;
using UnityEngine;

public class EnableOrDisableOnGetEvent : MonoBehaviour
{
    private void Awake()
    {
        EventManager.AddListener<bool>(ENUM_DominoeEvent.setActivePlayFieldObjects, SetActive);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<bool>(ENUM_DominoeEvent.setActivePlayFieldObjects, SetActive);
    }

    private void SetActive(bool eventData)
    {
        gameObject.SetActive(eventData);
    }
}