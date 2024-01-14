using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ObjectActivationToggle : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToogleActivateObject);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(ToogleActivateObject);
    }

    private void ToogleActivateObject()
    {
        objectToActivate.SetActive(!objectToActivate.activeSelf);
    }
}
