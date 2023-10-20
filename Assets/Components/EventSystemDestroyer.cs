using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemDestroyer : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<EventSystem>().Length > 1)
            Destroy(gameObject);
    }
}
