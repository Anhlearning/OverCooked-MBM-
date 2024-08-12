using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    
    private void Start()
    {
        stoveCounter.ProgressBar += StoveCounter_ProgressBar;
        Hide();
    }

    private void StoveCounter_ProgressBar(object sender, IProgressBar.ProgressBarEvent e)
    {   
        float burningWarningPercen=.5f;
        bool isShow = stoveCounter.isFried() && burningWarningPercen <= e.progressNomalize;
        if(isShow){
            Show();
        }
        else {
            Hide();
        }
    }
    private void Show(){
        gameObject.SetActive(true);
    }
    private void Hide(){
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
