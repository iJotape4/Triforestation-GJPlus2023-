#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine.EventSystems;
#endif

using UnityEngine;

/// <summary>
/// Debugs lots of information about the pointer event data
/// Is not recommended to used out of the editor
/// </summary>
public class PointerEventDataDebugger : MonoBehaviour
{
    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(this);
#endif
    }

#if UNITY_EDITOR
    private void Update()
    {
        // Check if there's a pointer event data
        if (EventSystem.current != null)
        {
            // Get the current pointer event data
            PointerEventData eventData = new PointerEventData(EventSystem.current);

            // Set the position of the pointer (you can customize this)
            eventData.position = Input.mousePosition;

            // Optionally, you may need to raycast to get additional information
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            // Now you can inspect the eventData
            //Debug.Log("Pointer Position: " + eventData.position);
            //Debug.Log("Pointer Delta: " + eventData.delta);
            Debug.Log("Pointer Hovered Objects: " + GetHoveredObjectsNames(results));
            // Add more information as needed

            // Call your method
            OnPointerExit(eventData);
        }
    }

    private void OnPointerExit(PointerEventData eventData)
    {
        // Your method implementation
    }

    private string GetHoveredObjectsNames(List<RaycastResult> results)
    {
        // Helper method to get names of objects currently being hovered by the pointer
        string names = "";
        foreach (var result in results)
        {
            names += result.gameObject.name + ", ";
        }
        return names.TrimEnd(',', ' ');
    }
#endif
}
