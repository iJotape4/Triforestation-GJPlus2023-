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
    public DominoToken[] tokensInBoard = new DominoToken[12];


    // Second tutorial step
    public GameObject swapedDomino;
    public GameObject acidRainDomino;
    public GameObject divineHelpsUI;
    public GameObject swapConfirmUI;

    // Animals phase
    public GameObject mainCamera;
    public GameObject[] dominoSpots;

    private int dialogueOffCount = 0; // Counter to keep track of how many times DialogueOff is dispatched

    public Dictionary<ENUM_Biome, int> biomeCounts = new Dictionary<ENUM_Biome, int>();

    private void Start()
    {
        SetFirstDominoes();
        _triangularGrid = FindObjectOfType<TutorialTriangularGrid>();
        foreach (ENUM_Biome biome in System.Enum.GetValues(typeof(ENUM_Biome)))
        {
            biomeCounts[biome] = 0;
        }
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

    [ContextMenu("Contar biomas")]
    public void CountBiomes()
    {
        

        // Iterate through the dominoes array
        for (int i = 0; i < dominoes.Length; i++)
        {
            // Get the DominoToken component from the current GameObject
            DominoToken dominoToken = dominoes[i].GetComponent<DominoToken>();

            // Check if the component exists
            if (dominoToken != null)
            {
                // Assign the obtained DominoToken to the tokensInBoard array
                tokensInBoard[i] = dominoToken;
            }
            else
            {
                // Handle the case where DominoToken component is not found
                Debug.LogError($"DominoToken component not found on {dominoes[i].name}");
            }
        }

        foreach (DominoToken token in tokensInBoard)
        {
            DominoPole[] poles = token.gameObject.GetComponentsInChildren<DominoPole>();

            foreach (var pole in poles)
            {
                IncreaseBiomeCount(pole.biome);
            }
        }

        GameManager.Instance.SetDictionary(biomeCounts);
    }

    // Method to increase the count of a specific biome.
    public void IncreaseBiomeCount(ENUM_Biome biome)
    {
        if (biomeCounts.ContainsKey(biome))
        {
            biomeCounts[biome]++;
        }
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
            currentDomino.SetActive(true);

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

            case 3:
                AnimalsPhase();
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
            StartCoroutine(ThirdTutorialStep());
        });
        yield return null;
    }

    private IEnumerator ThirdTutorialStep()
    {
        yield return new WaitForSeconds(1.5f);
        /*
        for (int i = 6; i < 9; i++)
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
        yield return new WaitForSeconds(1.5f);
        */

        Sequence mySequence = DOTween.Sequence();

        // First Tween (Move and Rotate)
        dominoes[4].SetActive(true);
        mySequence.Append(dominoes[4].transform.DOMove(_triangularGrid.GiveNextCenter(), 1f));
        mySequence.Join(dominoes[4].transform.DORotate(new Vector3(0f, -180f, 0f), 0.6f)).SetEase(Ease.Linear).OnComplete(() =>
        {
            dominoes[5].SetActive(true);
        }); 

        
        mySequence.Append(dominoes[5].transform.DOMove(_triangularGrid.GiveNextCenter(), 1f).SetDelay(0.5f).OnComplete(() =>
        {
            dominoes[6].SetActive(true);
        }));
        mySequence.Join(dominoes[5].transform.DORotate(new Vector3(0f, 120f, 0f), 0.6f).SetEase(Ease.Linear));

        mySequence.Append(dominoes[6].transform.DOMove(_triangularGrid.GiveNextCenter(), 1f).SetDelay(0.5f).OnComplete(() =>
        {
            dominoes[7].SetActive(true);
        }));
        mySequence.Join(dominoes[6].transform.DORotate(new Vector3(0f, -180f, 0f), 0.6f).SetEase(Ease.Linear));

        // Start the second Tween after a delay of 1 second
        mySequence.Append(dominoes[7].transform.DOMove(_triangularGrid.GiveNextCenter(), 1f).SetDelay(0.5f).OnComplete(() =>
        {
            dominoes[8].SetActive(true);
        })); 
        mySequence.Join(dominoes[7].transform.DORotate(new Vector3(0f, 0, 0f), 0.6f).SetEase(Ease.Linear));

        dominoes[8].SetActive(true);
        mySequence.Append(dominoes[8].transform.DOMove(_triangularGrid.GiveNextCenter(), 1f).SetDelay(0.5f).OnComplete(() =>
        {
            dominoes[9].SetActive(true);
        }));
        mySequence.Join(dominoes[8].transform.DORotate(new Vector3(0f, -60f, 0f), 0.6f).SetEase(Ease.Linear));

        
        mySequence.Append(dominoes[9].transform.DOMove(_triangularGrid.GiveNextCenter(), 1f).SetDelay(0.5f).OnComplete(() =>
        {
            dominoes[10].SetActive(true);
        }));
        mySequence.Join(dominoes[9].transform.DORotate(new Vector3(0f, 0, 0f), 0.6f).SetEase(Ease.Linear));

        
        mySequence.Append(dominoes[10].transform.DOMove(_triangularGrid.GiveNextCenter(), 1f).SetDelay(0.5f).OnComplete(() =>
        {
            dominoes[11].SetActive(true);
        }));
        mySequence.Join(dominoes[10].transform.DORotate(new Vector3(0f, 240, 0f), 0.6f).SetEase(Ease.Linear));

        
        mySequence.Append(dominoes[11].transform.DOMove(_triangularGrid.GiveNextCenter(), 1f).SetDelay(0.5f));
        mySequence.Join(dominoes[11].transform.DORotate(new Vector3(0f, 0, 0f), 0.6f).SetEase(Ease.Linear)).OnComplete(() =>
        {
            // Dispatch an event when the third animation finishes
            EventManager.Dispatch(ENUM_TutorialEvent.OnDialogueOn);
        }); ;

        yield return null;
    }

    private void AnimalsPhase()
    {
        foreach(GameObject spot in dominoesSpots)
        {
            spot.SetActive(false);
        }
        CountBiomes();
        EventManager.Dispatch(ENUM_GameState.poolAnimals);
    }
}
