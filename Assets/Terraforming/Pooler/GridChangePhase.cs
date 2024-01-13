using DG.Tweening;
using Events;
using UnityEngine;

public class GridChangePhase : MonoBehaviour
{
    public Transform finalPosition;
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
        transform.DOMove(finalPosition.position, 1.5f);
    }
}
