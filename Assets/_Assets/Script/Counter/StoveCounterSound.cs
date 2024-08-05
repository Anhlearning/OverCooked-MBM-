using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource=GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnFryring+=stoveCounter_OnFryingSound;
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
}
