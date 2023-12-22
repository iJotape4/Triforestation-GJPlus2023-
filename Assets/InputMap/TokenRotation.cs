using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class TokenRotation : MonoBehaviour
{
    public InputAction CellsControls;

    [SerializeField] private float tweenTime = 0.5f;
    [SerializeField] private Ease vibrationEase;
    [SerializeField] Collider polygonCollider;
    private float rotationValue = 60f;

    public bool isOver = true;

    private void Awake()
    {
        EventManager.AddListener<float>(ENUM_InputEvent.Rotate, PerformRotation);
    }
    
    private void OnDestroy()
    {
        EventManager.RemoveListener<float>(ENUM_InputEvent.Rotate, PerformRotation);
    }

    private void Start()
    {
        polygonCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        CellsControls.Enable();
    }

    private void OnDisable()
    {
        CellsControls.Disable();
    }

    public void RotateToken(float rotationSense)
    {     
        float targetRotation = transform.eulerAngles.y + rotationValue * rotationSense;

        // Applies the rotation using DOTween
        transform.DORotate(new Vector3(0, targetRotation,0 ), tweenTime).SetEase(vibrationEase).OnComplete(OverTween);
        EventManager.Dispatch(ENUM_SFXEvent.RotateSound);
    }

    public void OverTween()
    {
        isOver = true;
    }

    private void PerformRotation(float rotationSense)
    {
        if (!polygonCollider.enabled)
            return;

        if(isOver)
        {
            isOver = false;
            RotateToken(rotationSense);
        }
    }
}