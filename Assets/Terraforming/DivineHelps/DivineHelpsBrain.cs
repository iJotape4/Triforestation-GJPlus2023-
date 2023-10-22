using Events;
using UnityEngine;

public class DivineHelpsBrain : MonoBehaviour
{
    [SerializeField] GameObject divineHelpsPanel;
    private void Awake()
    {
        EventManager.AddListener(ENUM_DominoeEvent.selectDoneEvent, EnableDivineHelpsPanel);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_DominoeEvent.selectDoneEvent, EnableDivineHelpsPanel);      
    }

    private void EnableDivineHelpsPanel()
    {
        divineHelpsPanel.SetActive(true);
    }
}