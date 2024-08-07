using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDown;
    
    private void Start() {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
        Hide();
    }

    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if(GameManager.Instance.IsCountDownToStart()){
            Show();
        }
        else {
            Hide();
        }
    }
    private void Update() {
        countDown.text= math.ceil(GameManager.Instance.GetCountDownTime()).ToString();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
}
