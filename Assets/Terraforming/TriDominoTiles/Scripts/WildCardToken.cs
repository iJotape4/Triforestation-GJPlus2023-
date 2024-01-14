using Events;
using System;
using System.Collections;
using Terraforming;
using Terraforming.Dominoes;
using UnityEngine;

public class WildCardToken : DominoToken
{
    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener(ENUM_GameState.firstPhaseFinished, SetWildCardPole);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RemoveListener(ENUM_GameState.firstPhaseFinished, SetWildCardPole);
    }
    /// <summary>
    /// Transforms the WildCard token into an whole pole that only receives the condor
    /// </summary>
    private void SetWildCardPole()
    {
        StartCoroutine(SetWildCardPoleCoroutine());
    }

    private IEnumerator SetWildCardPoleCoroutine()
    {
        //This wait time is necessary to allow hazard tokens to check if they are savable when are next to a wildcard
        yield return new WaitForSeconds(1f);
        ConvertWholeTokenIntoAnUniquePole(-1);// -1 Set to everything, it means, receives condor
    }

    protected void ConvertWholeTokenIntoAnUniquePole(int biomeValue)
    {
        foreach (DominoPole pole in poles)
            pole.gameObject.SetActive(false);

        Destroy(GetComponent<DragView>());
        dominoCollider.enabled = true;
        DominoPole newPole = gameObject.AddComponent<DominoPole>();
        newPole.biome = (ENUM_Biome)(biomeValue);
        poles = new DominoPole[] { newPole };
    }
}