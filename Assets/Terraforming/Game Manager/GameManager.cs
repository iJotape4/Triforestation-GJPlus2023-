using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SinglentonParent<GameManager>
{

    public Dictionary<ENUM_Biome, int> biomeCounts = new Dictionary<ENUM_Biome, int>();

    public void SetDictionary(Dictionary<ENUM_Biome, int> dict)
    {
        biomeCounts = dict;
        EventManager.Dispatch(ENUM_GameState.firstPhaseFinished);
    }
}
