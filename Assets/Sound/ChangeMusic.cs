using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string fmodEventPathGame;


    private EventInstance fmodEventInstance;

    [SerializeField] [Range(0, 4)] private int sectionMusic;
    private int sectionPlaying;

    private void Start()
    {
        fmodEventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventPathGame);
        fmodEventInstance.start();
        sectionPlaying = sectionMusic;

    }
    private void FixedUpdate()
    {
        
        if(sectionMusic != sectionPlaying)
        {
                sectionPlaying = sectionMusic;
                ChangeSectionMusic(sectionMusic);
        }
    }

    public void ChangeSectionMusic(int value) { fmodEventInstance.setParameterByName("ChangeMusic", value); Debug.Log(value); }
    private void OnDestroy()
    {
        fmodEventInstance.release();
    }
}
