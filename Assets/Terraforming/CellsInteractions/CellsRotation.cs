using DG.Tweening;
using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CellsRotation : MonoBehaviour
{
    public InputAction CellsControls;

    [SerializeField] private float tweenTime = 0.5f;
    [SerializeField] private Ease vibrationEase;
    [SerializeField] PolygonCollider2D polygonCollider;
    private float rotationValue = 60f;

    public bool isOver = true;

    private void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void OnEnable()
    {
        CellsControls.Enable();
    }

    private void OnDisable()
    {
        CellsControls.Disable();
    }

    public void Rotate(float rotationSense)
    {     
        float targetRotation = transform.eulerAngles.z + rotationValue * rotationSense;

        // Aplicar rotación suavemente usando DOTween
        transform.DORotate(new Vector3(0, 0, targetRotation), tweenTime).SetEase(vibrationEase).OnComplete(OverTween);
        EventManager.Dispatch(ENUM_SFXEvent.RotateSound);
    }

    public void OverTween()
    {
        isOver = true;
    }

    private void Update()
    {
        if (!polygonCollider.enabled)
            return;

        float rotationSense = CellsControls.ReadValue<float>();

        if(rotationSense != 0 && isOver == true)
        {
            isOver = false;
            Rotate(rotationSense);
        }
    }

}
