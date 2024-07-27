using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField]private GameObject StoveOnVisual;
    [SerializeField]private GameObject particle;
    [SerializeField]private StoveCounter stoveCounter; 
    private bool onFryring;
    private void Start()
    {
        stoveCounter.OnFryring+=OnFryringChange;
    }

    private void OnFryringChange(object sender, StoveCounter.OnStateChangeEvents e)
    {
        onFryring=e.state==StoveCounter.State.Fyring|| e.state==StoveCounter.State.Fired;
        particle.SetActive(onFryring);
        StoveOnVisual.SetActive(onFryring);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
