using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Hide();
        GameManager.Instance.OnMultiplayerPause += GameManager_OnPauseGameMultiplayer;
        GameManager.Instance.UnMultiplayerPause += GameManager_UnPauseGameMultiplayer;
    }

    private void GameManager_UnPauseGameMultiplayer(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnPauseGameMultiplayer(object sender, EventArgs e)
    {
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
}
