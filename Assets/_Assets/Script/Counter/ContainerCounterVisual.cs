using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private Animator animator; 
    [SerializeField] private ContainerCounter containerCounter; 
    private const string OPEN_CLOSE="OpenClose";
    private void Awake(){
        animator=GetComponent<Animator>();
        containerCounter.OnplayerGrabbOject+=onPlayerGrabbOject;
    }

    private void onPlayerGrabbOject(object sender, EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }

   
}
