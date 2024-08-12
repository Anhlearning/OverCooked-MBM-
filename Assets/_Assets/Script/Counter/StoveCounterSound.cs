using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    bool playingWarningSound;
    float warningSoundTime=.2f;
    private void Awake()
    {
        audioSource=GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnFryring+=stoveCounter_OnFryingSound;
        stoveCounter.ProgressBar += StoveCounter_OnSoundWarning;
    }

    private void StoveCounter_OnSoundWarning(object sender, IProgressBar.ProgressBarEvent e)
    {
        float burningWarningPercen=.5f;
        playingWarningSound = stoveCounter.isFried() && burningWarningPercen <= e.progressNomalize;
    }

    private void stoveCounter_OnFryingSound(object sender, StoveCounter.OnStateChangeEvents e)
    {
       bool playSound=e.state== StoveCounter.State.Fyring || e.state == StoveCounter.State.Fired;
       if(playSound){
            audioSource.Play();
       }
       else {
            audioSource.Pause();
       }
    }
    private void Update() {
        if(playingWarningSound){
            warningSoundTime-=Time.deltaTime;
            if(warningSoundTime <=0){
                float warningSoundTimeMax=.2f;
                warningSoundTime=warningSoundTimeMax;
                
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
