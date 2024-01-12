using DG.Tweening;
using Events;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;
using MyBox;
using LevelSelector;

public class TutorialDominoPooler : DominoPooler
{
    protected override void CreateDominoes()
    {
        for (int i = 0; i < levelData.dominoesAmount; i++)
        {
            GameObject dominoObj = Instantiate(dominoPrefab, transform);
            DominoToken domino = dominoObj.GetComponent<DominoToken>();

            // Set the position of the domino in a row from left to right.
            float xPos = i * dominoSpacing; // Adjust the spacing as needed.
            dominoObj.transform.localPosition = new Vector3(xPos, 0, 0);
            domino.tokenData = levelData.tokenDatas[i];
            dominoes.Add(domino);
            domino.ResetDomino();
        }
        // Update the order in layer to ensure the rightmost domino is on top.
        base.UpdateOrderInLayer();
        base.GiveInitialDominoes();
        EventManager.Dispatch(ENUM_SFXEvent.deckStart);
    }
}

