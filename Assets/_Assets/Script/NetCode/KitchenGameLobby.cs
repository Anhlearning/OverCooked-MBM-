using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameLobby : MonoBehaviour
{
    private const string KEY_RELAY_JOIN_CODE="RELAYJOINCODE";
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
        if(joinedLobby ==null && AuthenticationService.Instance.IsSignedIn && SceneManager.GetActiveScene().name==Loader.Scene.Lobby.ToString()){
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
    private async Task<Allocation> AllocateRelay(){
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(KitchenGameMultiplayer.MAXPLAYERJOINGAME-1);
            return allocation;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);            
            throw;
        }
    } 
    private async Task<string> GetRelayJoinCode(Allocation allocation){
        
        try
        {
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }
    private async Task<JoinAllocation> JoinRelay(string joinCode){
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return allocation;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }
    public async void CreateLobby(String lobbyName,bool isPrivate){
        OnCreateLobby?.Invoke(this,EventArgs.Empty);
        try
        {
        joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,KitchenGameMultiplayer.MAXPLAYERJOINGAME,new CreateLobbyOptions{
            IsPrivate=isPrivate
        }); 
        Allocation allocation = await AllocateRelay();
        string relayJoinCode= await GetRelayJoinCode(allocation);
        await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions{
            Data= new Dictionary<string, DataObject>{
                {KEY_RELAY_JOIN_CODE,new DataObject(DataObject.VisibilityOptions.Member,relayJoinCode)}
            }
        });
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData
        );
        

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
            string joinCode=joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(joinCode);

           NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
             joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
           );

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
            string joinCode=joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(joinCode);

           NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
             joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
           );
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
            string joinCode=joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(joinCode);

           NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
             joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
           );
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
