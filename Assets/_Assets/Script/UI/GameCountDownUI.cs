using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDown;
    private const string Popup="OnPupop";
    private Animator animator;
    int countDownpre;
    private void Awake() {
        animator=GetComponent<Animator>();
    }
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
        int countDownInt= Mathf.CeilToInt(GameManager.Instance.GetCountDownTime());
        countDown.text=countDownInt.ToString();
        if(countDownpre != countDownInt){
            countDownpre=countDownInt;
            animator.SetTrigger(Popup);
            SoundManager.Instance.PlayCountDownSound();
        }
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
}
