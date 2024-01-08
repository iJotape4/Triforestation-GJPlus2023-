using DG.Tweening;
using Events;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    InputActionsMap actionMap;
    InputActionsMap.MainActionsActions mainActions;

    private Vector2 movementInput;
    private float movementSpeed = 0.1f;

    private void Awake()
    {
        actionMap = new InputActionsMap();
        mainActions = actionMap.MainActions;

        EventManager.AddListener(ENUM_GameState.firstPhaseFinished, TransitionToPuttingAnimalsPosition);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.firstPhaseFinished, TransitionToPuttingAnimalsPosition);
    }

    private void TransitionToPuttingAnimalsPosition()
    {
        transform.DOMove(new Vector3(transform.position.x, 3f, transform.position.z),2f);
        transform.DORotate(new Vector3(20f, 0f, 0f), 2f);
    }

    private void Update()
    {
        MoveCamera();
    }

    private void SetMovementInput(Vector2 input)
    {
        movementInput = input;
    }

    private void MoveCamera()
    {
        // Assuming you want to move only in the X and Z axes (horizontal and vertical)
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y);
        Vector3 newPosition = transform.position + movement*movementSpeed;
        transform.position = newPosition;
    }
    private void OnEnable()
    {
        mainActions.CameraMovement.performed += ctx => SetMovementInput(ctx.ReadValue<Vector2>());
        mainActions.CameraMovement.canceled += ctx => SetMovementInput(Vector2.zero);
        actionMap.Enable();
    }
    private void OnDisable() => actionMap.Disable();
}