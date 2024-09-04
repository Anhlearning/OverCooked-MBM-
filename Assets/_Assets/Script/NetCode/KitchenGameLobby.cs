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
    private bool IsLobbyHost(){
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
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
            
            Debug.Log(e);
        }
    }
    public async void QuickJoin(){
        try
        {
            joinedLobby= await LobbyService.Instance.QuickJoinLobbyAsync();
            KitchenGameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public Lobby GetLobby(){
        return joinedLobby;
    }
    public async void  JoinWithCode(string lobbyCode){
        try
        {
            joinedLobby= await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }
}
