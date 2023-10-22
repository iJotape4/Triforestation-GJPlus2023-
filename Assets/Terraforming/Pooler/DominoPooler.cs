using DG.Tweening;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;

public class DominoPooler : MonoBehaviour
{
    public GameObject dominoPrefab; // Reference to the domino prefab.
    public int totalDominoes = 52; // Total number of dominoes to be created.
    public float dominoSpacing = 0.1f;
    private List<DominoToken> dominoes = new List<DominoToken>();
    private int currentIndex = 0;

    public Transform currentDomino; // Current domino position
    public float moveDuration = 0.5f; // Duration of the animation

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
        //UpdateOrderInLayer();
    }
    [ContextMenu("Get next domino")]
    public DominoToken GetNextDomino()
    {
        if (currentIndex < dominoes.Count)
        {
            DominoToken domino = dominoes[currentIndex];
            currentIndex++;
            domino.UncoverDomino();

            domino.transform.DOLocalMove(currentDomino.localPosition, moveDuration).OnComplete(() => domino.ActiveDrag());

            return domino;
        }

        return null; // All dominoes have been used.
    }

    void UpdateOrderInLayer()
    {
        for (int i = 0; i < currentIndex; i++)
        {
            dominoes[i].GetComponent<SpriteRenderer>().sortingOrder = 60 - i;
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