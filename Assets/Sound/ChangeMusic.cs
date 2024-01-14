using Events;
using FMOD.Studio;
using System;
using System.Collections;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string fmodEventPathGame;

    private EventInstance fmodEventInstance;
    Coroutine currentCoroutine;
    [SerializeField] [Range(0, 10)] private int sectionMusic;
    private int sectionPlaying;

    private void Awake()
    {
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
        fmodEventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventPathGame);
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
        
        if(sectionMusic != sectionPlaying)
        {
                sectionPlaying = sectionMusic;
                ChangeSectionMusic(sectionMusic);
        }
    }

    public void ChangeSectionMusic(int value) { fmodEventInstance.setParameterByName("MainMusic", value); Debug.Log(value); }
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
                yield return new WaitForSeconds(16f);
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