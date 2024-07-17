using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Image Bar;
    [SerializeField] private CuttingCounter cuttingCounter; 
    private void Awake() {
        cuttingCounter.ProgressBar+= ProgressBar_OnChange;
        gameObject.SetActive(false);
    }

    private void ProgressBar_OnChange(object sender, CuttingCounter.ProgressBarEvent e)
    {
        Bar.fillAmount=e.progressNomalize;

        if(e.progressNomalize == 0 || e.progressNomalize ==1){
            gameObject.SetActive(false);
        }
        else {
            gameObject.SetActive(true);
        }
    }
}
