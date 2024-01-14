using DG.Tweening;
using Events;
using UnityEngine;

public class BackgroundMovementOnSecondPhase : MonoBehaviour
{
    private void Awake()
    {
        EventManager.AddListener(ENUM_GameState.firstPhaseFinished, MoveArregement);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.firstPhaseFinished, MoveArregement);
    }
    private void MoveArregement()
    {
        transform.DOMoveY(-2, 1f);
    }
}