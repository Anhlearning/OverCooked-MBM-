using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBarAnimation : MonoBehaviour
{
   [SerializeField] private StoveCounter stoveCounter;
   private Animator animator;
    
    private void Awake() {
        animator=GetComponent<Animator>();
    }
    private void Start()
    {
        stoveCounter.ProgressBar += StoveCounter_ProgressBar;
        animator.SetBool("Flash",false);
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
        animator.SetBool("Flash",true);
    }
    private void Hide(){
        animator.SetBool("Flash",false);
    }
    
}
