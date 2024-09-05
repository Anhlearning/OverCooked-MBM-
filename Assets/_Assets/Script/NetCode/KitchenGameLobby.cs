using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class KitchenGameLobby : MonoBehaviour
{
    public static KitchenGameLobby Instance;
    private Lobby joinedLobby;
    private float heartBeatTimer;
    private float listLobbyTimer;

    public event EventHandler OnCreateLobby;
    public event EventHandler OnCreateLobbyFailed;
    public event EventHandler OnJoinLobby;
    public event EventHandler OnQuickJoinLobbyFailed;
    public event EventHandler OnJoinFailed;
    public EventHandler<OnLobbyListChangeEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangeEventArgs: EventArgs{
        public List<Lobby>lobbyList;
    }
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        Instance=this;
        InitializeUnityAuthentication();
    }
    private void Start()
    {
        
    }
    private void Update() {
        HandleHearBeat();
        HandlePriodicListLobbies();
    }
    private void HandleHearBeat(){
        if(IsLobbyHost()){
            heartBeatTimer-=Time.deltaTime;
            if(heartBeatTimer <=0f){
                float heartBeatTimerMax=15f;
                heartBeatTimer=heartBeatTimerMax;
                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }
    private void  HandlePriodicListLobbies(){
        if(joinedLobby ==null && AuthenticationService.Instance.IsSignedIn){
            listLobbyTimer -= Time.deltaTime;
            if(listLobbyTimer <= 0f){
                float listLobbyTimerMax=3f;
                listLobbyTimer=listLobbyTimerMax;
                ListLobbies();
            }
        }
    }
    private bool IsLobbyHost(){
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }
    private async void ListLobbies(){
        try
        {
        QueryLobbiesOptions queryLobbiesOptions =  new QueryLobbiesOptions{
            Filters= new List<QueryFilter>{
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
            }
        };
        QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
        OnLobbyListChanged?.Invoke(this,new OnLobbyListChangeEventArgs{
            lobbyList= queryResponse.Results
        });    
        }
        catch (LobbyServiceException e )
        {
            Debug.Log(e);
            throw;
        } 
    }

    private async void InitializeUnityAuthentication(){
        if(UnityServices.State !=ServicesInitializationState.Initialized){
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(UnityEngine.Random.Range(0,10000).ToString());

            await UnityServices.InitializeAsync(initializationOptions);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }   
    public async void CreateLobby(String lobbyName,bool isPrivate){
        OnCreateLobby?.Invoke(this,EventArgs.Empty);
        try
        {
        joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,KitchenGameMultiplayer.MAXPLAYERJOINGAME,new CreateLobbyOptions{
            IsPrivate=isPrivate
        });            
        KitchenGameMultiplayer.Instance.StartHost();
        Loader.NetworkLoad(Loader.Scene.SelectedCharacter);
        }
        catch (LobbyServiceException e)
        {
            OnCreateLobbyFailed?.Invoke(this,EventArgs.Empty);
            Debug.Log(e);
        }
    }
    public async void QuickJoin(){
        OnJoinLobby?.Invoke(this,EventArgs.Empty);
        try
        {
            joinedLobby= await LobbyService.Instance.QuickJoinLobbyAsync();
            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {   
            OnQuickJoinLobbyFailed?.Invoke(this,EventArgs.Empty);
            Debug.Log(e);
        }
    }
    public Lobby GetLobby(){
        return joinedLobby;
    }
    public async void  JoinWithId(string lobbyCode){
        OnJoinLobby?.Invoke(this,EventArgs.Empty);
        try
        {
            joinedLobby= await LobbyService.Instance.JoinLobbyByIdAsync(lobbyCode);
            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            OnJoinFailed?.Invoke(this,EventArgs.Empty);
            Debug.Log(e);
            throw;
        }
    }

    public async void  JoinWithCode(string lobbyCode){
        OnJoinLobby?.Invoke(this,EventArgs.Empty);
        try
        {
            joinedLobby= await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            OnJoinFailed?.Invoke(this,EventArgs.Empty);
            Debug.Log(e);
            throw;
        }
    }
    public async void LeaveLobby(){
        if(joinedLobby !=null){
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id,AuthenticationService.Instance.PlayerId);
            joinedLobby=null;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }    
    }    
    }
    public async void KickPlayer(string playerId){
        if(IsLobbyHost()){
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id,playerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }    
    }    
    }
    public async void DeleteLobby(){
        if(joinedLobby !=null){
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                joinedLobby=null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}
