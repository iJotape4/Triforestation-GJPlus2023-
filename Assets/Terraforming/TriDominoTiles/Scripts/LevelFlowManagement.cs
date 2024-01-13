using Events;
using System;
using System.Collections;
using Terraforming.Dominoes;
using UnityEngine;

public class LevelFlowManagement : MonoBehaviour
{
    private int savableHazardsAmount = 0;
    private int droppedTokens;
    private int generatedDropTiles;
    private int generatedPunishTiles;

    protected void Awake()
    {
        EventManager.AddListener(ENUM_GameState.firstPhaseFinished, CountSavablehazards);
        EventManager.AddListener(ENUM_GameState.recoveredEcosystem, RecoveredEcosystem);
        EventManager.AddListener(ENUM_DominoeEvent.dominoDroppedEvent, CountTotalTokens);
        EventManager.AddListener(ENUM_DominoeEvent.spawnedMoorEvent, CountTotalTokens);
        EventManager.AddListener(ENUM_DominoeEvent.generatedTileEvent, CountGeneratedTiles);
        EventManager.AddListener(ENUM_DominoeEvent.punishEvent, CheckIfAvailableMovements);
    }

    protected void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.firstPhaseFinished, CountSavablehazards);
        EventManager.RemoveListener(ENUM_GameState.recoveredEcosystem, RecoveredEcosystem);
        EventManager.RemoveListener(ENUM_DominoeEvent.dominoDroppedEvent, CountTotalTokens);
        EventManager.RemoveListener(ENUM_DominoeEvent.spawnedMoorEvent, CountTotalTokens);
        EventManager.RemoveListener(ENUM_DominoeEvent.generatedTileEvent, CountGeneratedTiles);
        EventManager.RemoveListener(ENUM_DominoeEvent.punishEvent, CheckIfAvailableMovements);
    }
    private void CountSavablehazards()
    {
        StartCoroutine(CountSavablehazardsCoroutine());
    }

    public void CountTotalTokens() => droppedTokens++;
    public void CountGeneratedTiles() => generatedDropTiles++;
    public void CheckIfAvailableMovements()
    {
        generatedPunishTiles++;

        Debug.Log("Generated drop tiles" + generatedDropTiles);
        Debug.Log("Generated punish tiles" + generatedPunishTiles);
        Debug.Log("Dropped tokens" + droppedTokens);

        if(generatedPunishTiles +  droppedTokens >= generatedDropTiles)
        {
            Debug.Log("No more available movements");
            EventManager.Dispatch(ENUM_GameState.firstPhaseFinished);
        }
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

        if (CalculateGameOverIfTooomuchHazards())
            yield return null;
        else if (savableHazardsAmount == 0)
            EventManager.Dispatch(ENUM_GameState.poolAnimals);
        else
            EventManager.Dispatch(ENUM_GameState.poolDescomposers);
    }

    private bool CalculateGameOverIfTooomuchHazards()
    {
        int totalTokens = droppedTokens + generatedPunishTiles;
        if(generatedPunishTiles - savableHazardsAmount > totalTokens * 0.5f)
        {
            EventManager.Dispatch(ENUM_GameState.loose);
            return true;
        }
        return false;
    }

    private void RecoveredEcosystem()
    {
        savableHazardsAmount--;
        if(savableHazardsAmount<=0)
            EventManager.Dispatch(ENUM_GameState.poolAnimals);
    }
}
