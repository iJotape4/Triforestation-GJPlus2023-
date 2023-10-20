using Events;
using FMODUnity;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
   void Awake()
   {
        // TODO Subscribe to Methods
        Events.EventManager.AddListener(EventsExamples.Example1, PlaySFX1);

    }

    private void OnDestroy()
    {
        //TODO UnSubscribe to Methods
        Events.EventManager.RemoveListener(EventsExamples.Example1, PlaySFX1);
    }

    void PlaySFX(string sFX) => RuntimeManager.PlayOneShot(sFX);

    //TOOD Create Methods
    void PlaySFX1() => PlaySFX(SFXDictionary.sfx1);
}