using System.Collections;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;
using DG.Tweening;
using Events;
using UnityEngine.UIElements;

public class TutorialManager : MonoBehaviour
{
    private TutorialDominoPooler _pooler;
    private TutorialTriangularGrid _triangularGrid;
    public GameObject[] dominoes;
    public GameObject[] dominoesSpots;


    // Second tutorial step
    public GameObject swapedDomino;
    public GameObject acidRainDomino;
    public GameObject divineHelpsUI;
    public GameObject swapConfirmUI;

    private int dialogueOffCount = 0; // Counter to keep track of how many times DialogueOff is dispatched

    private void Start()
    {
        SetFirstDominoes();
        _triangularGrid = FindObjectOfType<TutorialTriangularGrid>();
        EventManager.AddListener(ENUM_TutorialEvent.OnDialogueOff, OnDialogueOffEvent);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_TutorialEvent.OnDialogueOff, OnDialogueOffEvent);
    }

    public bool CurrentTokenMatch(DominoToken token)
    {
        UpdateNextTokenToPlace();
        return token.tokenData == _pooler.actualTutorialToken.tokenData;
    }

    public void UpdateNextTokenToPlace()
    {
        _pooler.GetNextToken();
    }

    public void SetFirstDominoes()
    {
        int length = Mathf.Min(dominoes.Length, dominoesSpots.Length);

        for (int i = 0; i < length; i++)
        {
            // Get the current domino and spot
            GameObject currentDomino = dominoes[i];
            GameObject currentSpot = dominoesSpots[i];

            // Set the initial position of the domino to the spot
            currentDomino.transform.position = currentSpot.transform.position;

            // Use DoTween to move the domino to its initial position with a delay
            currentDomino.transform.DOMoveY(currentSpot.transform.position.y, 1.0f)
                .SetDelay(i * 0.5f)  // Adjust the delay as needed
                .SetEase(Ease.OutBounce);  // Adjust the ease as needed
        }
    }

    public void FirstTutorialStep()
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < 3; i++)
        {
            GameObject currentDomino = dominoes[i];
            Vector3 position = _triangularGrid.GiveNextCenter();

            // Add the DOMove animation to the sequence with appropriate delay
            sequence.Append(currentDomino.transform.DOMove(position, 2f).SetEase(Ease.Linear));

            // If this is the last domino, add the OnComplete callback
            if (i == 2)
            {
                sequence.OnComplete(() =>
                {
                    // Dispatch an event when the third animation finishes
                    EventManager.Dispatch(ENUM_TutorialEvent.OnDialogueOn);
                });
            }
        }
    }

    public void SecondTutorialStep()
    {
        for (int i = 3; i < 6; i++)
        {
            // Get the current domino and spot
            GameObject currentDomino = dominoes[i];
            GameObject currentSpot = dominoesSpots[i - 3];

            // Set the initial position of the domino to the spot
            currentDomino.transform.position = currentSpot.transform.position;

            // Use DoTween to move the domino to its initial position with a delay
            currentDomino.transform.DOMove(currentSpot.transform.position, 1.0f)
                .SetDelay(i * 0.5f)  // Adjust the delay as needed
                .SetEase(Ease.Linear);  // Adjust the ease as needed
        }
        Vector3 acidPosition = _triangularGrid.GiveNextCenter();
        acidRainDomino.transform.position = acidPosition;
        acidRainDomino.SetActive(true);
        StartCoroutine(DivineHelpCoroutine());
        
    }

    private void OnDialogueOffEvent()
    {
        dialogueOffCount++;

        switch (dialogueOffCount)
        {
            case 1:
                FirstTutorialStep();
                break;
            case 2:
                SecondTutorialStep();
                break;
            default:
                break;
        }

    }

    private IEnumerator DivineHelpCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        // Rotate each domino smoothly by -60 degrees in the Y-axis
        for (int i = 3; i < 6; i++)
        {
            GameObject currentDomino = dominoes[i];
            currentDomino.transform.DORotate(new Vector3(0f, -60f, 0f), 1.0f)
            .SetEase(Ease.Linear);  // Adjust the ease as needed
 
        }
        yield return new WaitForSeconds(1.5f);
        divineHelpsUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        divineHelpsUI.SetActive(false);
        swapConfirmUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        swapConfirmUI.SetActive(false);
        swapedDomino.SetActive(true);
        swapedDomino.transform.position = dominoes[3].transform.position;
        dominoes[3].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Vector3 position = _triangularGrid.GiveNextCenter();
        swapedDomino.transform.DOMove(position, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            // Dispatch an event when the third animation finishes
            EventManager.Dispatch(ENUM_TutorialEvent.OnDialogueOn);
        });
        yield return null;
    }
}
