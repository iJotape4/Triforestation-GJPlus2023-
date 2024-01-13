using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    Vector3 initialScale;
    private void Start()
    {
        initialScale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        ExecuteButtonMethod();
        //TODO: Add sound
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      transform.DOScale(transform.localScale + new Vector3(0.1f, 0.1f, 0.1f), 0.5f).SetEase(Ease.Linear).SetLoops(0, LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      transform.DOScale(initialScale, 0.5f).SetEase(Ease.Linear).SetLoops(0, LoopType.Yoyo);
    }

    protected abstract void ExecuteButtonMethod();
}