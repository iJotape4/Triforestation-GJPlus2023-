using Events;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;

public class DummyDominoToken : MonoBehaviour
{
    DummyPole[] poles;
    List<DummyPole> selectedPoles = new List<DummyPole> ();

    DominoToken realToken;
    private void Awake()
    {
        EventManager.AddListener<DominoToken>(ENUM_DominoeEvent.selectCardToSwipeEvent, SetPoles);
        EventManager.AddListener(ENUM_DominoeEvent.confirmSwapEvent, ConfirmSwap);
        EventManager.AddListener(ENUM_DominoeEvent.cancelEvent, CancelSelection);
    }


    private void OnDestroy()
    {
        EventManager.RemoveListener<DominoToken>(ENUM_DominoeEvent.selectCardToSwipeEvent, SetPoles);
        EventManager.RemoveListener(ENUM_DominoeEvent.confirmSwapEvent, ConfirmSwap);
        EventManager.RemoveListener(ENUM_DominoeEvent.cancelEvent, CancelSelection);
    }


    private void Start()
    {
        poles = GetComponentsInChildren<DummyPole>();
    }
    private void CancelSelection()
    {
        SetSpritesActive(false);
    }

    private void SetPoles(DominoToken eventData)
    {
        SetSpritesActive(true);
        realToken = eventData;
        UsefulMethods.PositionInTheCenterOfTheScreen();
        for (int i = 0; poles.Length > i; i++)
        {
            poles[i].AssignBiome(realToken.poles[i]);
        }
    }

    private void SetSpritesActive(bool active)
    {
        foreach (DummyPole pole in poles)
        {
            pole.meshRenderer.enabled = active;

            if (!active)
                pole.UnselectOnCancelOrConfirm();
        }
    }

    public int GetSelectedPolesCount()
    {
        int count =0;
        selectedPoles.Clear();
        foreach (var p in poles)
        {
            if (p.selected)
            {
                selectedPoles.Add(p);
                count++;
            }
        }

        return count;
    }

    private void ConfirmSwap()
    {
        if(GetSelectedPolesCount() != 2)       
            return;        

        ENUM_Biome biome1 = selectedPoles[0].biome;
        ENUM_Biome biome2 = selectedPoles[1].biome;

        selectedPoles[0].AssignBiome(biome2);
        selectedPoles[1].AssignBiome(biome1);

        for(int i=0; i<poles.Length; i++)
        {
            realToken.poles[i].AssignBiome(poles[i].biome);         
        }
        SetSpritesActive(false);
        EventManager.Dispatch(ENUM_DominoeEvent.validSwap);
    }
}