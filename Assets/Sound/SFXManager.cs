using Events;
using FMODUnity;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
   void Awake()
   {
        // TODO Subscribe to Methods
        EventManager.AddListener(ENUM_SFXEvent.deckStart, PlaySFX1);

    }

    private void OnDestroy()
    {
        //TODO UnSubscribe to Methods
        EventManager.RemoveListener(ENUM_SFXEvent.deckStart, PlaySFX1);
    }

    void PlaySFX(string sFX) => RuntimeManager.PlayOneShot(sFX);

    //TOOD Create Methods
    void PlaySFX1() => PlaySFX(SFXDictionary.spellCard);
}