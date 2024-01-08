using UnityEngine;
using Events;
using DG.Tweening;

public class DominoPoolerPhaseChange : MonoBehaviour
{
    public GameObject[] dominoSpots;
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
        foreach (GameObject go in dominoSpots) 
        {
            go.SetActive(false);
        }
    }

}
