using Events;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Text.RegularExpressions;
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
        EventManager.AddListener(ENUM_DominoeEvent.spawnedAcidRainEvent, PlayAcidRain);
        EventManager.AddListener(ENUM_AnimalEvent.animalPrefabCreated, PlayDragSound);
        EventManager.AddListener(ENUM_AnimalEvent.animalDroped, PlayGoodSound);
        EventManager.AddListener(ENUM_AnimalEvent.animalPrefabDestroyed, PlayErrorSound);
        EventManager.AddListener<ENUM_AnimalEvent>(ENUM_SFXEvent.animalSound, PlayAnimalSound);
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
        EventManager.RemoveListener(ENUM_SFXEvent.PlaySound, PlayUnselect);
        EventManager.RemoveListener(ENUM_DominoeEvent.spawnedAcidRainEvent, PlayAcidRain);
        EventManager.RemoveListener(ENUM_AnimalEvent.animalPrefabCreated, PlayDragSound);
        EventManager.RemoveListener(ENUM_AnimalEvent.animalDroped, PlayGoodSound);
        EventManager.RemoveListener(ENUM_AnimalEvent.animalPrefabDestroyed, PlayErrorSound);
        EventManager.RemoveListener<ENUM_AnimalEvent>(ENUM_SFXEvent.animalSound, PlayAnimalSound);
    }

    void PlaySFX(string sFX) => RuntimeManager.PlayOneShot(sFX);
    void PlaySFX<T>(string sFx, T parameter) where T: Enum
    {
       EventInstance sound =  RuntimeManager.CreateInstance(sFx);
        int parameterID = Convert.ToInt32(parameter);
        string result = Regex.Replace(sFx, ".*?/", "");
        FMOD.RESULT param = sound.setParameterByName(result, parameterID);
        sound.start();
    }

    //TOOD Create Methods
    void PlaySFX1() => PlaySFX(SFXDictionary.spellCard, SFXParameters_Button.SpellCard);
    void PlayDragSound() => PlaySFX(SFXDictionary.drag);
    void PlayDropSound() => PlaySFX(SFXDictionary.drop);
    void PlayCheckSound() => PlaySFX(SFXDictionary.check, SFXParameters_Button.Correcto);
    void PlayErrorSound() => PlaySFX(SFXDictionary.Error, SFXParameters_Button.Error);
    void PlayGoodSound() => PlaySFX(SFXDictionary.Good, SFXParameters_Button.Bien);
    void PlayRotateSound() => PlaySFX(SFXDictionary.Rotate, SFXParameters_Button.Rotar);
    void PlaySelect() => PlaySFX(SFXDictionary.Select, SFXParameters_Button.Seleccion);
    void PlayUnselect() => PlaySFX(SFXDictionary.Unselect, SFXParameters_Button.Deseleccion);
    void PlayPLay() => PlaySFX(SFXDictionary.Play, SFXParameters_Button.Jugar);
    void PlayAcidRain() => PlaySFX(SFXDictionary.ActionSFX,SFXParameters_ActionSfx.LluviaAcida );

    private void PlayAnimalSound(ENUM_AnimalEvent eventData) => PlaySFX(SFXDictionary.AnimalSFX, eventData);
}