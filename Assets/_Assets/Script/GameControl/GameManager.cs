using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnPauseMenu;
    public event EventHandler OffPauseMenu;
    public static GameManager Instance {get;set;}
    public event EventHandler OnStateChange;
    public enum State {
        WattingToStart,
        CountdownToStart,
        GammePlaying,
        GameOver
    }
    private State state;
    private float countdowToStart=1f;
    private float gamePlaying;
    private float gamePlayingMax=300f;

    private bool isPause=false;
    private void Awake() {
        Instance=this;

        state=State.WattingToStart;
    }
    private void Start() {
        PlayerInput.Instance.OnPauseGame += PlayerInput_Pause; 
        PlayerInput.Instance.OnInteraction += GameManager_OnInteract;

        state=State.CountdownToStart;
        OnStateChange?.Invoke(this,EventArgs.Empty);
    }

    private void GameManager_OnInteract(object sender, EventArgs e)
    {
        if(state==State.WattingToStart){
            state=State.CountdownToStart;
            OnStateChange?.Invoke(this,EventArgs.Empty);
        }
    }

    private void PlayerInput_Pause(object sender, EventArgs e)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        isPause=!isPause;
        if(isPause){
            OnPauseMenu?.Invoke(this,EventArgs.Empty);
            Time.timeScale=0;
        }
        else {
            OffPauseMenu?.Invoke(this,EventArgs.Empty);
            Time.timeScale=1;
        }
    }
    private void Update() {
        switch (state)
        {
            case State.WattingToStart:
            break;
            case State.CountdownToStart:
            countdowToStart-=Time.deltaTime;
            if(countdowToStart <=0){
                state=State.GammePlaying;
                gamePlaying=gamePlayingMax;
                OnStateChange?.Invoke(this,EventArgs.Empty);

            }
            break;
            case State.GammePlaying:
            gamePlaying-=Time.deltaTime;
            if(gamePlaying<=0){
                state=State.GameOver;
                OnStateChange?.Invoke(this,EventArgs.Empty);
            }
            break;
            case State.GameOver:
            break;
        }
    }
    public bool IsGamePlaying(){
        return state==State.GammePlaying;
    }

    public bool IsCountDownToStart(){
        return state==State.CountdownToStart;
    }
    public float GetCountDownTime(){
        return countdowToStart;
    }
    public bool IsGameOver(){
        return state==State.GameOver;
    }
    public float GetGamePlayingTimerNomolized(){
        return 1-(gamePlaying/gamePlayingMax);
    }
    public State GetState(){
        return state;
    }
}
