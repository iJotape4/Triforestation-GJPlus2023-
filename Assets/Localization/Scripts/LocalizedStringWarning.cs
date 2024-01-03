using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace com.t3ampo.imagink.localization
{
    /// <summary>
    /// Localized string events must be set in the inspector. This class sends a warning when any of them are not set properly
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI), typeof(LocalizeStringEvent))]
    public class LocalizedStringWarning : MonoBehaviour
    {
        LocalizeStringEvent stringEvent;
        TextMeshProUGUI text;

        [ExecuteAlways]
        void Awake()
        {
            stringEvent = GetComponent<LocalizeStringEvent>();
            text = GetComponent<TextMeshProUGUI>();

            if (stringEvent.StringReference.ToString() == "TableReference(Empty)/TableEntryReference(Empty)")
                Debug.LogWarning("Localized string reference is not set", gameObject);
            else if (stringEvent.OnUpdateString.GetPersistentEventCount() == 0)
                Debug.LogWarning("No listeners are registered on Localized String Event", gameObject);
            else if (stringEvent.OnUpdateString.GetPersistentTarget(0) != text)
                Debug.LogWarning("The textmeshPro component attached to this gameObject is not assigned to the Localized string Event", gameObject);
            else if(stringEvent.OnUpdateString.GetPersistentMethodName(0) != "set_text")
                Debug.LogWarning("The method text is not assigned to the Localized String event", gameObject);
        }
    }
}