using Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnableOrDisableOnGetEvent<T> : MonoBehaviour where T: Enum
{
    [SerializeField] protected  T enableEvent;
    [SerializeField] protected T disableEvent;
    protected void Awake()
    {
        if (!EqualityComparer<T>.Default.Equals(disableEvent, default(T)))
        {
            EventManager.AddListener(enableEvent, EnableObject);
            EventManager.AddListener(disableEvent, DisableObject);
        }
        else
        {
            EventManager.AddListener(enableEvent, SetActive);
            EventManager.AddListener<bool>(enableEvent, SetActive);
        }
    }

    protected void OnDestroy()
    {
        if (!EqualityComparer<T>.Default.Equals(disableEvent, default(T)))
        {
            EventManager.RemoveListener(enableEvent, EnableObject);
            EventManager.RemoveListener(disableEvent, DisableObject);
        }
        else
        {
            EventManager.RemoveListener(enableEvent, SetActive);
            EventManager.RemoveListener<bool>(enableEvent, SetActive);
        }
    }

    protected void SetActive() => gameObject.SetActive(gameObject.activeSelf ? false : true);
    protected void SetActive(bool eventData) => gameObject.SetActive(eventData);
    protected void EnableObject() => gameObject.SetActive(true);
    protected void DisableObject() => gameObject.SetActive(false);
    protected void SetEvent(T _ev) => enableEvent = _ev;
}
