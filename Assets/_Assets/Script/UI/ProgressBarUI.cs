using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{   
    [SerializeField] private GameObject hasProgressBarGameObject;
    [SerializeField] private Image Bar;
    private IProgressBar hasProgress; 
    private void Awake() {
        hasProgress=hasProgressBarGameObject.GetComponent<IProgressBar>();
        if(hasProgress ==null){
            Debug.LogError(gameObject.name +"no hasProgress");
        }
        hasProgress.ProgressBar+= ProgressBar_OnChange;
        gameObject.SetActive(false);
    }

    private void ProgressBar_OnChange(object sender, IProgressBar.ProgressBarEvent e)
    {
        Bar.fillAmount=e.progressNomalize;
        Debug.Log(e.progressNomalize);
        if(e.progressNomalize == 0f || e.progressNomalize == 1f){
            gameObject.SetActive(false);
        }
        else {
            gameObject.SetActive(true);
        }
    }
}
