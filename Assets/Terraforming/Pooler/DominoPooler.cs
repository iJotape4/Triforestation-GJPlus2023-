using DG.Tweening;
using Events;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;
using MyBox;

public class DominoPooler : MonoBehaviour
{
    public GameObject dominoPrefab; // Reference to the domino prefab.
    public int totalDominoes = 52; // Total number of dominoes to be created.
    public float dominoSpacing = 0.1f;
    private List<DominoToken> dominoes = new List<DominoToken>();
    private int currentIndex = 0;

    public DominoSpot[] dominoesSpots; // Current domino position
    [SerializeField, ReadOnly] private List<DominoToken> currentDominoesList;
    public float moveDuration = 0.5f; // Duration of the animation

    private void Awake()
    {
        EventManager.AddListener<DominoToken>(ENUM_DominoeEvent.dominoDroppedEvent, OnDominoDropped);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<DominoToken>(ENUM_DominoeEvent.dominoDroppedEvent, OnDominoDropped);        
    }

    private void OnDominoDropped(DominoToken domino)
    {
        currentDominoesList.Remove(domino);

    }

    void Start()
    {
        CreateDominoes();
    }

    void CreateDominoes()
    {
        for (int i = 0; i < totalDominoes; i++)
        {
            GameObject dominoObj = Instantiate(dominoPrefab, transform);
            DominoToken domino = dominoObj.GetComponent<DominoToken>();

            // Set the position of the domino in a row from left to right.
            float xPos = i * dominoSpacing; // Adjust the spacing as needed.
            domino.transform.localPosition = new Vector3(xPos, 0, 0);

            dominoes.Add(domino);
            domino.ResetDomino();
        }

        // Update the order in layer to ensure the rightmost domino is on top.
        UpdateOrderInLayer();
        GetNextDomino();
    }
    [ContextMenu("Get next domino")]
    public DominoToken GetNextDomino()
    {
        if (currentIndex < dominoes.Count)
        {
            Transform _nextPosition = GetNextFreePosition();
            if(_nextPosition == null)
            {
                EventManager.Dispatch(ENUM_DominoeEvent.punishEvent);
                return null;
            }

            EventManager.Dispatch(ENUM_DominoeEvent.getCardEvent);

            DominoToken domino = dominoes[currentIndex];
            currentIndex++;
            currentDominoesList.Add(domino);
            // Create a new DOTween sequence
            Sequence uncoverSequence = DOTween.Sequence();

            // Add the local move animation to the sequence
            uncoverSequence.Append(domino.transform.DOLocalMove(_nextPosition.localPosition, moveDuration))
                .OnStart(() =>
                {
                    // Activate the movement
                    domino.ActiveDrag();
                });

            // Add the rotation animation (both parts) to the sequence using Join
            uncoverSequence.Join(domino.transform.DORotate(new Vector3(0, 90, 0), moveDuration, RotateMode.WorldAxisAdd))
                .OnComplete(() =>
                {
                    // Deactivate the dominoCover
                    domino.gameObject.GetComponent<SpriteRenderer>().enabled = false;

                    // Rotate the GameObject back to 0 degrees
                    domino.transform.DORotate(Vector3.zero, moveDuration);
                });

            // Play the sequence
            uncoverSequence.Play();
            return domino;
        }

        return null; // All dominoes have been used.
    }

     Transform GetNextFreePosition()
     {
        if (currentDominoesList.Count >= dominoesSpots.Length)
            return null;

        foreach (DominoSpot spot in dominoesSpots)
            if (spot.IsSpotFree())
            {
                spot.SetCurrentToken(dominoes[currentIndex]); 
                return spot.transform;
            }

        return null;
     }

    void UpdateOrderInLayer()
    {
        for (int i = 0; i < dominoes.Count; i++)
        {
            dominoes[i].gameObject.GetComponent<SpriteRenderer>().sortingOrder = 100 - i;
        }
    }
    [ContextMenu("Reset Cards")]
    public void ResetDominoes()
    {
        for (int i = 0; i < dominoes.Count; i++)
        {
            DominoToken domino = dominoes[i];

            // Reset the position of the domino in the row from left to right.
            float xPos = i * dominoSpacing; // Adjust the spacing as needed.
            domino.transform.localPosition = new Vector3(xPos, 0, 0);

            domino.ResetDomino();
        }

        // Reset the currentIndex to the beginning.
        currentIndex = 0;
    }
}