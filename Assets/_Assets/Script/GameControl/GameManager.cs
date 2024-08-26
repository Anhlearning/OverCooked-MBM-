using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public event EventHandler OnPauseMenu;
    public event EventHandler OffPauseMenu;
    public static GameManager Instance {get;set;}
    public event EventHandler OnStateChange;
    public event EventHandler OnLocalPlayerReady;
    public enum State {
        WattingToStart,
        CountdownToStart,
        GammePlaying,
        GameOver
    }
    private NetworkVariable<State> state=new NetworkVariable<State>(State.WattingToStart);
    private NetworkVariable<float> countdowToStart=new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlaying=new NetworkVariable<float>(300f);
    private float gamePlayingMax=300f;

    private bool isLocalPlayerReady;
    private bool isPause=false;

    private Dictionary<ulong,bool>playerReadyDictionary;
    private void Awake() {
        Instance=this;
        playerReadyDictionary=new Dictionary<ulong, bool>();
    }
    
    private void Start() {
        PlayerInput.Instance.OnPauseGame += PlayerInput_Pause; 
        PlayerInput.Instance.OnInteraction += GameManager_OnInteract;

        // state=State.CountdownToStart;
        // OnStateChange?.Invoke(this,EventArgs.Empty);
    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += GameManager_OnStateChange;
    }

    private void GameManager_OnStateChange(State previousValue, State newValue)
    {
        OnStateChange?.Invoke(this,EventArgs.Empty);
    }

    private void GameManager_OnInteract(object sender, EventArgs e)
    {
        if(state.Value==State.WattingToStart){
            isLocalPlayerReady=true;
            OnLocalPlayerReady?.Invoke(this,EventArgs.Empty);
            SetPlayerReadyServerRpc();
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default){
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId]=true;
        bool allClientReady=true;
        foreach(ulong IdClient in NetworkManager.Singleton.ConnectedClientsIds){
            if(!playerReadyDictionary.ContainsKey(IdClient) || !playerReadyDictionary[IdClient]){
                allClientReady=false;
                break;
            }
        }
        UnityEngine.Debug.Log(allClientReady);
        if(allClientReady==true){
            state.Value=State.CountdownToStart;
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
        if(!IsServer){
            return ;
        }
        switch (state.Value)
        {
            case State.WattingToStart:
            break;
            case State.CountdownToStart:
            countdowToStart.Value-=Time.deltaTime;
            if(countdowToStart.Value <=0){
                state.Value=State.GammePlaying;
                gamePlaying.Value=gamePlayingMax;
            }
            break;
            case State.GammePlaying:
            gamePlaying.Value-=Time.deltaTime;
            if(gamePlaying.Value<=0){
                state.Value=State.GameOver; 
            }
            break;
            case State.GameOver:
            break;
        }
    }
    public bool IsGamePlaying(){
        return state.Value==State.GammePlaying;
    }

    public bool IsCountDownToStart(){
        return state.Value==State.CountdownToStart;
    }
    public float GetCountDownTime(){
        return countdowToStart.Value;
    }
    public bool IsGameOver(){
        return state.Value==State.GameOver;
    }
    public float GetGamePlayingTimerNomolized(){
        return 1-(gamePlaying.Value/gamePlayingMax);
    }
    public State GetState(){
        return state.Value;
    }
    public bool IsLocalPlayerReady(){
        return isLocalPlayerReady;
    }
}
