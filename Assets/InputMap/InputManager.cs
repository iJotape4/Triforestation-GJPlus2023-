using Events;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputActionsMap actionMap;
    InputActionsMap.MainActionsActions mainActions;

    private void Awake()
    {
        actionMap = new InputActionsMap();
        mainActions = actionMap.MainActions;
    }

    private void Start()
    {
        mainActions.Rotate.started += ctx => PerformRotation(ctx.ReadValue<float>());
        mainActions.AskDivineGift.performed += ctx => EventManager.Dispatch(ENUM_InputEvent.AskDivineGift);
    }

    private void PerformRotation(float axisValue)
    {
        EventManager.Dispatch(ENUM_InputEvent.Rotate, axisValue);
    }

    private void OnEnable() => actionMap.Enable();
    private void OnDestroy() => actionMap.Disable();
}

public enum ENUM_InputEvent
{
    Rotate,
    AskDivineGift
}