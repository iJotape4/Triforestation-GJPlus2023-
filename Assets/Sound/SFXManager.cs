using Events;
using FMODUnity;
using System;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
   void Awake()
   {
        // TODO Subscribe to Methods
        EventManager.AddListener(ENUM_SFXEvent.deckStart, PlaySFX1);
        EventManager.AddListener(ENUM_SFXEvent.dragSound, PlayDragSound);
        EventManager.AddListener(ENUM_DominoeEvent.dominoDroppedEvent, PlayDropSound);

    }
    private void OnDestroy()
    {
        //TODO UnSubscribe to Methods
        EventManager.RemoveListener(ENUM_SFXEvent.deckStart, PlaySFX1);
        EventManager.RemoveListener(ENUM_SFXEvent.dragSound, PlayDragSound);
        EventManager.RemoveListener(ENUM_DominoeEvent.dominoDroppedEvent, PlayDropSound);
    }

    void PlaySFX(string sFX) => RuntimeManager.PlayOneShot(sFX);

    //TOOD Create Methods
    void PlaySFX1() => PlaySFX(SFXDictionary.spellCard);
    void PlayDragSound() => PlaySFX(SFXDictionary.drag);
    void PlayDropSound() => PlaySFX(SFXDictionary.drop);
}