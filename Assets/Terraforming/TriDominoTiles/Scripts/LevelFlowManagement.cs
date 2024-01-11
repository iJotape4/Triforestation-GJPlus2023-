using Events;
using System;
using System.Collections;
using UnityEngine;

public class LevelFlowManagement : MonoBehaviour
{
    private int savableHazardsAmount = 0;
    protected void Awake()
    {
        EventManager.AddListener(ENUM_GameState.firstPhaseFinished, CountSavablehazards);
        EventManager.AddListener(ENUM_GameState.recoveredEcosystem, RecoveredEcosystem);
    }

    protected void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.firstPhaseFinished, CountSavablehazards);
        EventManager.RemoveListener(ENUM_GameState.recoveredEcosystem, RecoveredEcosystem);
    }
    private void CountSavablehazards()
    {
        StartCoroutine(CountSavablehazardsCoroutine());
    }

    private IEnumerator CountSavablehazardsCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        PunishToken[] hazards = GetComponentsInChildren<PunishToken>();
        foreach (PunishToken hazard in hazards)
        {
            if( hazard.savable)
               savableHazardsAmount++;
        }

        if (savableHazardsAmount == 0)
            EventManager.Dispatch(ENUM_GameState.poolAnimals);
        else
            EventManager.Dispatch(ENUM_GameState.poolDescomposers);
    }

    private void RecoveredEcosystem()
    {
        savableHazardsAmount--;
        if(savableHazardsAmount<=0)
            EventManager.Dispatch(ENUM_GameState.poolAnimals);
    }

}
