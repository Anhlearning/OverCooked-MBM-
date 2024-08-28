using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public event EventHandler OnPauseMenu;
    public event EventHandler OffPauseMenu;
    public event EventHandler OnMultiplayerPause;
    public event EventHandler UnMultiplayerPause;
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

    private bool isLocalPlayerReady=false;
    private bool isLocalPause=false;
    private bool isClientDisconnet=false;
    private NetworkVariable<bool>isPause=new NetworkVariable<bool>(false);

    private Dictionary<ulong,bool>playerReadyDictionary;
    private Dictionary<ulong,bool>playerPauseDictionary;
    [SerializeField] private Transform Player;
    private void Awake() {
        Instance=this;
        playerReadyDictionary=new Dictionary<ulong, bool>();
        playerPauseDictionary=new Dictionary<ulong, bool>();
    }
    
    private void Start() {
        PlayerInput.Instance.OnPauseGame += PlayerInput_Pause; 
        PlayerInput.Instance.OnInteraction += GameManager_OnInteract;


    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += GameManager_OnStateChange;
        isPause.OnValueChanged += GameManager_OnPauseChange;

        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening) {
            return;  // Đảm bảo rằng NetworkManager đã được khởi tạo và đang lắng nghe trước khi thay đổi NetworkVariable
        }
        if(IsServer){
            NetworkManager.Singleton.OnClientDisconnectCallback += GameManager_OnClientDisconnect;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += GameManager_OnLoadEventComplete; 
        }
    }

    private void GameManager_OnLoadEventComplete(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong ClientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerSpawn = Instantiate(Player);
            playerSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(ClientId,true);
        }
    }

    private void Update() {
        if(!NetworkManager.Singleton.IsServer ){
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
    private void LateUpdate() {
        if(isClientDisconnet){
            isClientDisconnet=false;
            TestGamePauseState();
        }
    }
    private void GameManager_OnClientDisconnect(ulong obj)
    {
        isClientDisconnet=true;
    }

    private void GameManager_OnPauseChange(bool previousValue, bool newValue)
    {
        if(isPause.Value==true){
            Time.timeScale=0;
            OnMultiplayerPause?.Invoke(this,EventArgs.Empty);
        }
        else {
            Time.timeScale=1;
            UnMultiplayerPause?.Invoke(this,EventArgs.Empty);
        }
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
        isLocalPause=!isLocalPause;
        if(isLocalPause){
            OnPauseMenu?.Invoke(this,EventArgs.Empty);
           PauseGameServerRpc();
        }
        else {
            OffPauseMenu?.Invoke(this,EventArgs.Empty);
           UnPauseGameServerRpc();
        }
    }

    [ServerRpc(RequireOwnership =false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default){
        playerPauseDictionary[serverRpcParams.Receive.SenderClientId]=true;
        TestGamePauseState();
    }
    [ServerRpc(RequireOwnership = false)]
    private void UnPauseGameServerRpc(ServerRpcParams serverRpcParams = default){
        playerPauseDictionary[serverRpcParams.Receive.SenderClientId]=false;
        TestGamePauseState();
    }
    private void TestGamePauseState(){
        foreach (ulong IdClient in NetworkManager.Singleton.ConnectedClientsIds){
            if(playerPauseDictionary.ContainsKey(IdClient) && playerPauseDictionary[IdClient]){
                isPause.Value=true;
                return;
            }
        }
        isPause.Value=false;
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
    public bool isWattingToStart(){
        return state.Value == State.WattingToStart;
    }
    public bool IsLocalPlayerReady(){
        return isLocalPlayerReady;
    }
}
