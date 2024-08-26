using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.OnLocalPlayerReady += GameManager_OnPlayerReady;
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
        Hide();
    }

    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if(GameManager.Instance.IsCountDownToStart()){
            Hide();
        }
    }

    private void GameManager_OnPlayerReady(object sender, EventArgs e)
    {  
        if(GameManager.Instance.IsLocalPlayerReady()){
            Show();
        }
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
    
}
