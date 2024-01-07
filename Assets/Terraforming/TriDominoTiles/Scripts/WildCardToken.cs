using Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Terraforming;
using Terraforming.Dominoes;
using UnityEngine;

public class WildCardToken : DominoToken
{
    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener(ENUM_GameState.poolAnimals, SetWildCardPole);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RemoveListener(ENUM_GameState.poolAnimals, SetWildCardPole);
    }

    private void SetWildCardPole()
    {
       foreach(DominoPole pole in poles)        
            pole.gameObject.SetActive(false);

        Destroy(GetComponent<DragView>());
        dominoCollider.enabled = true;
        DominoPole newPole = gameObject.AddComponent<DominoPole>();
        newPole.biome = (ENUM_Biome)(-1); // -1 Set to everything, it means, receives condor
        poles = new DominoPole[] { newPole };
    }
}