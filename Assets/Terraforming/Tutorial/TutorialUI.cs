using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] public List<GameObject[]> dialogueBlocks = new List<GameObject[]>();
    public GameObject tutorialPanelUI;
    public GameObject[] firstBlock;
    public GameObject[] secondBlock;
    public GameObject[] thirdBlock;
    private int currentDialogueBlockIndex = 0;
    private int currentDialogueIndex = 0;

    private void Awake()
    {
        InitializeDialogues();
        EventManager.AddListener(ENUM_TutorialEvent.OnDialogueOn, OnDialogueOnEvent);
        EventManager.AddListener(ENUM_TutorialEvent.OnDialogueOff, OnDialogueOffEvent);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_TutorialEvent.OnDialogueOn, OnDialogueOnEvent);
        EventManager.RemoveListener(ENUM_TutorialEvent.OnDialogueOff, OnDialogueOffEvent);
    }

    private void InitializeDialogues()
    {
        dialogueBlocks.Add(firstBlock);
        dialogueBlocks.Add(secondBlock);
        dialogueBlocks.Add(thirdBlock);
    }

    private void Start()
    {
        DisplayCurrentDialogueElement();
    }

    private void Update()
    {
        // Check for user input to proceed to the next dialogue element
        if (Input.GetMouseButtonDown(0) && currentDialogueIndex < dialogueBlocks[currentDialogueBlockIndex].Length)
        {
            DisplayNextDialogueElement();
        }
    }

    private void DisplayNextDialogueElement()
    {
        // Deactivate the current dialogue element
        SetDialogueElementActive(currentDialogueBlockIndex, currentDialogueIndex, false);

        // Move to the next dialogue element
        currentDialogueIndex++;

        // If this was the last element in the block, dispatch event to turn off dialogue
        if (currentDialogueIndex == dialogueBlocks[currentDialogueBlockIndex].Length)
        {
            EventManager.Dispatch(ENUM_TutorialEvent.OnDialogueOff);
        }
        else
        {
            // Activate the next dialogue element
            SetDialogueElementActive(currentDialogueBlockIndex, currentDialogueIndex, true);
        }
    }

    private void DisplayCurrentDialogueElement()
    {
        // Activate the current dialogue element (the first one)
        SetDialogueElementActive(currentDialogueBlockIndex, currentDialogueIndex, true);
    }

    private void SetDialogueElementActive(int blockIndex, int elementIndex, bool isActive)
    {
        if (blockIndex < dialogueBlocks.Count && elementIndex < dialogueBlocks[blockIndex].Length)
        {
            GameObject obj = dialogueBlocks[blockIndex][elementIndex];
            obj.SetActive(isActive);
        }
    }

    // Event triggered when dialogue is turned on
    private void OnDialogueOnEvent()
    {
        tutorialPanelUI.SetActive(true);
        // Move to the next dialogue block
        currentDialogueBlockIndex++;

        // If there are more dialogue blocks, display the next one
        if (currentDialogueBlockIndex < dialogueBlocks.Count)
        {
            // Reset the dialogue index for the new block
            currentDialogueIndex = 0;
            // Display the first dialogue element
            SetDialogueElementActive(currentDialogueBlockIndex, currentDialogueIndex, true);
        }
    }

    // Event triggered when dialogue is turned off
    // Event triggered when dialogue is turned off
    private void OnDialogueOffEvent()
    {
        // Deactivate the current dialogue element
        SetDialogueElementActive(currentDialogueBlockIndex, currentDialogueIndex, false);
        tutorialPanelUI.SetActive(false);
    }
}
