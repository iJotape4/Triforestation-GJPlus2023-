using Events;
using UnityEngine;

public class DivineHelpsManager : SinglentonParent<DivineHelpsManager>
{
    [SerializeField] GameObject divineHelpsPanel;
    protected override void Awake()
    {
        base.Awake();
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
    public void DisableDivineHelpsPanel()
    {
        divineHelpsPanel.SetActive(false);
    }
}