using Events;
using UnityEngine;

public class EnableOrDisableOnGetEvent : MonoBehaviour
{
    private void Awake()
    {
    }

    private void OnDestroy()
    {
    }

    private void SetActive(bool eventData)
    {
        gameObject.SetActive(eventData);
    }
}