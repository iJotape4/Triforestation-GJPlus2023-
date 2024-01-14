using Events;
using FMOD;
using FMOD.Studio;
using System;
using System.Collections;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    
    Coroutine currentCoroutine;
    [SerializeField] [Range(0, 10)] private int sectionMusic;
    [SerializeField] private string music = "event:/MainMusic";
    private FMOD.Studio.EventInstance fmodEventInstance;
    private int sectionPlaying;

    private void Awake()
    {
        
        fmodEventInstance = FMODUnity.RuntimeManager.CreateInstance(music);
        EventManager.AddListener(ENUM_GameState.firstPhaseFinished, OnFirstPhaseFinished);
        EventManager.AddListener(ENUM_GameState.secondPhaseFinished, OnSecondPhaseFinished);
    }
    void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.firstPhaseFinished, OnFirstPhaseFinished);
        EventManager.RemoveListener(ENUM_GameState.secondPhaseFinished, OnSecondPhaseFinished);
        fmodEventInstance.release();
    }
    private void Start()
    {
        fmodEventInstance.start();
        sectionPlaying = sectionMusic;

        currentCoroutine = StartCoroutine(ChangeMusicCoroutine());
    }

    private void OnSecondPhaseFinished()
    {
        StopCoroutine(currentCoroutine);
        sectionMusic = 9;
        
    }

    private void OnFirstPhaseFinished()
    {
        StopCoroutine(currentCoroutine);
        sectionMusic = 5;
        currentCoroutine = StartCoroutine(ChangeMusicSeconPhaseCoroutine());
    }

    private void FixedUpdate()
    {
        
        if (sectionMusic != sectionPlaying)
        {
                print(sectionMusic);
                print(sectionPlaying);
            
                sectionPlaying = sectionMusic;
                ChangeSectionMusic(sectionMusic);
        }
    }

    public void ChangeSectionMusic(int value) {

        sectionMusic = value;
        fmodEventInstance.setParameterByName("MainMusic", value);
        print(fmodEventInstance.setParameterByName("MainMusic", value)); 
    }

    private IEnumerator ChangeMusicCoroutine()
    {
        for (int i = 0; i <= 4; i++)
        {
            yield return new WaitForSeconds(16f);
            ChangeSectionMusic(i + 1);
        }
    }
      private IEnumerator ChangeMusicSeconPhaseCoroutine()
      {
            for (int i = 5; i <=8; i++)
            {
                yield return new WaitForSeconds(32f);
                ChangeSectionMusic(i + 1);
            }       
      }
    //OLD implementation
    /*private IEnumerator ChangeMusicCoroutine()
    {
        yield return new WaitForSeconds(30f);
        ChangeSectionMusic(2);

        yield return new WaitForSeconds(60f);
        ChangeSectionMusic(3);

        yield return new WaitForSeconds(90f);
        ChangeSectionMusic(4);
    }*/
}