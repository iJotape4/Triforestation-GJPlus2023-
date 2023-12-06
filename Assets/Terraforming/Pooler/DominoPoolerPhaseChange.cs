using UnityEngine;
using Events;
using DG.Tweening;

public class DominoPoolerPhaseChange : MonoBehaviour
{
    public GameObject[] dominoSpots;
    private void Awake()
    {
        EventManager.AddListener(ENUM_GameState.poolAnimals, MoveArregement);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.poolAnimals, MoveArregement);
    }

    private void MoveArregement()
    {
        foreach (GameObject go in dominoSpots) 
        {
            go.SetActive(false);
        }
    }

}
