using UnityEngine;
using UnityEngine.UI;

public abstract class UIButton : MonoBehaviour
{
    protected Button button;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickButtonMethod);
    }
    protected abstract void ClickButtonMethod();
}