using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private Animator animator; 
    [SerializeField] private CuttingCounter cuttingCounter; 
    private const string ONCUT="Cut";
    private void Awake(){
        animator=GetComponent<Animator>();
        cuttingCounter.OnCut+=OnCut_preformer;
    }

    private void OnCut_preformer(object sender, EventArgs e)
    {
        animator.SetTrigger(ONCUT);
    }
}
