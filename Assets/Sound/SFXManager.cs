using Events;
using FMODUnity;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
   void Awake()
   {
        // TODO Subscribe to Methods
        EventManager.AddListener(ENUM_SFXEvent.deckStart, PlaySFX1);
        EventManager.AddListener(ENUM_SFXEvent.dragSound, PlayDragSound);
        EventManager.AddListener(ENUM_DominoeEvent.dominoDroppedEvent, PlayDropSound);
        EventManager.AddListener(ENUM_SFXEvent.checkSound, PlayCheckSound);
        EventManager.AddListener(ENUM_SFXEvent.GoodSound, PlayGoodSound);
        EventManager.AddListener(ENUM_SFXEvent.ErrorSound, PlayErrorSound);
        EventManager.AddListener(ENUM_SFXEvent.RotateSound, PlayRotateSound);
        EventManager.AddListener(ENUM_SFXEvent.SelectSound, PlaySelect);
        EventManager.AddListener(ENUM_SFXEvent.UnselectSound, PlayUnselect);
        EventManager.AddListener(ENUM_SFXEvent.PlaySound, PlayUnselect);
    }

    private void OnDestroy()
    {
        //TODO UnSubscribe to Methods
        EventManager.RemoveListener(ENUM_SFXEvent.deckStart, PlaySFX1);
        EventManager.RemoveListener(ENUM_SFXEvent.dragSound, PlayDragSound);
        EventManager.RemoveListener(ENUM_DominoeEvent.dominoDroppedEvent, PlayDropSound);
        EventManager.RemoveListener(ENUM_SFXEvent.checkSound, PlayCheckSound);
        EventManager.RemoveListener(ENUM_SFXEvent.GoodSound, PlayGoodSound);
        EventManager.RemoveListener(ENUM_SFXEvent.ErrorSound, PlayErrorSound);
        EventManager.RemoveListener(ENUM_SFXEvent.RotateSound, PlayRotateSound);
        EventManager.RemoveListener(ENUM_SFXEvent.SelectSound, PlaySelect);
        EventManager.RemoveListener(ENUM_SFXEvent.UnselectSound, PlayPLay);
    }

    void PlaySFX(string sFX) => RuntimeManager.PlayOneShot(sFX);

    //TOOD Create Methods
    void PlaySFX1() => PlaySFX(SFXDictionary.spellCard);
    void PlayDragSound() => PlaySFX(SFXDictionary.drag);
    void PlayDropSound() => PlaySFX(SFXDictionary.drop);
    void PlayCheckSound() => PlaySFX(SFXDictionary.check);
    void PlayErrorSound() => PlaySFX(SFXDictionary.Error);
    void PlayGoodSound() => PlaySFX(SFXDictionary.Good);
    void PlayRotateSound() => PlaySFX(SFXDictionary.Rotate);
    void PlaySelect() => PlaySFX(SFXDictionary.Select);
    void PlayUnselect() => PlaySFX(SFXDictionary.Unselect);
    void PlayPLay() => PlaySFX(SFXDictionary.Play);
}