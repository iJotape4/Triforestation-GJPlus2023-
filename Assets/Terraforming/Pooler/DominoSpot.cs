using Events;
using Terraforming.Dominoes;
using UnityEngine;

public class DominoSpot : MonoBehaviour
{
    DominoToken currentToken = null;

    private void Awake()
    {
        EventManager.AddListener<DominoToken>(ENUM_DominoeEvent.dominoDroppedEvent, CheckDominoToken);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<DominoToken>(ENUM_DominoeEvent.dominoDroppedEvent, CheckDominoToken);
    }
    private void CheckDominoToken(DominoToken eventData)
    {
        if (currentToken == eventData)
            currentToken = null;
    }

    public bool IsSpotFree() => currentToken == null ? true : false;
    public void SetCurrentToken(DominoToken token) => currentToken = token;

}