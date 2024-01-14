using System.Collections;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TutorialDominoPooler _pooler;

    private void Start()
    {
        _pooler = FindObjectOfType<TutorialDominoPooler>();
    }

    public bool CurrentTokenMatch(DominoToken token)
    {
        return token == _pooler.actualTutorialToken;
    }

    public void UpdateNextTokenToPlace()
    {
        _pooler.GetNextToken();
    }
}
