using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Error;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance{get;set;} 

    public static bool playMultiplayer;
    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailToJoinGame;
    public event EventHandler OnPlayerDataNetworkChange;
    public const int MAXPLAYERJOINGAME=4;
    private const string PLAYER_NAME_MULTIPLAYER="PLAYERNAME";
    private NetworkList<PlayerData>PlayerDataNetworkList;
    private string playerName;
    [SerializeField] private KitchenObjectList kitchenObjectList;
    [SerializeField] private List<Color> playerColorList;

    private void Awake() {
        Instance=this;
        PlayerDataNetworkList=new NetworkList<PlayerData>();
        playerName=PlayerPrefs.GetString(PLAYER_NAME_MULTIPLAYER,"PLAYER "+UnityEngine.Random.Range(10,100));
        if(playMultiplayer){
            PlayerDataNetworkList.OnListChanged += PlayerDataList_ChangeValue;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start() {
        if(!playMultiplayer){
           NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", 7777);
            StartHost();
            Loader.NetworkLoad(Loader.Scene.GamePlay);
        }
    }

    private void PlayerDataList_ChangeValue(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkChange?.Invoke(this,EventArgs.Empty);
    }
    public string GetPlayerName(){
        return playerName;
    }
    public void SetPlayerName(string playerName){
        this.playerName=playerName;
        PlayerPrefs.SetString(PLAYER_NAME_MULTIPLAYER,playerName);
    }

    public void StartClient(){
        OnTryingToJoinGame?.Invoke(this,EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnclientDissconnect;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnected;
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_Client_OnClientConnected(ulong obj)
    {
        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIDServerRpc(AuthenticationService.Instance.PlayerId);
    }
    [ServerRpc(RequireOwnership =false)]
    private void SetPlayerNameServerRpc(string playerName,ServerRpcParams serverRpcParams=default){
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = PlayerDataNetworkList[playerDataIndex];
        playerData.playerName=playerName;
        PlayerDataNetworkList[playerDataIndex]=playerData;
    }
     [ServerRpc(RequireOwnership =false)]
    private void SetPlayerIDServerRpc(string playerId,ServerRpcParams serverRpcParams=default){
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = PlayerDataNetworkList[playerDataIndex];
        playerData.playerId=playerId;
        PlayerDataNetworkList[playerDataIndex]=playerData;
    }

    private void NetworkManager_OnclientDissconnect(ulong obj)
    {
        OnFailToJoinGame?.Invoke(this,EventArgs.Empty);
    }

    public void StartHost(){
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManagerHost_OnclientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Host_OnClientConnectedCallBack;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManagerHost_OnclientDisconnectCallback(ulong ClientId)
    {
        for(int i=0 ; i < PlayerDataNetworkList.Count;i++){
            PlayerData playerData = PlayerDataNetworkList[i];
            if(playerData.ClientId==ClientId){
                PlayerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void NetworkManager_Host_OnClientConnectedCallBack(ulong ClientId)
    {   
        PlayerDataNetworkList.Add(new PlayerData {
            ClientId=ClientId,
            colorId=GetFirstUnusedColorId()
        });
        SetPlayerNameServerRpc(GetPlayerName()); 
        SetPlayerIDServerRpc(AuthenticationService.Instance.PlayerId);

    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {   
        if(SceneManager.GetActiveScene().name != Loader.Scene.SelectedCharacter.ToString())
        {
            connectionApprovalResponse.Approved=false;
            connectionApprovalResponse.Reason="Game is Played";
            return;
        }
        if(NetworkManager.Singleton.ConnectedClientsIds.Count >= MAXPLAYERJOINGAME){
            connectionApprovalResponse.Approved=false;
            connectionApprovalResponse.Reason="Maxium Player";
            return;
        }
        connectionApprovalResponse.Approved=true;
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent kitchenObjectParent){
        SpawnKitchenObjectServerRpc(GetIdxFromKitchenObjectList(kitchenObjectSO),kitchenObjectParent.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership =false)]
    public void SpawnKitchenObjectServerRpc(int idx,NetworkObjectReference networkObjectReference){
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectSOFromIdx(idx);

        networkObjectReference.TryGet(out NetworkObject kitchenParentNetworkObject);
        IKitchenObjectParent IkitchenNetworkParent = kitchenParentNetworkObject.GetComponent<IKitchenObjectParent>();
        if(IkitchenNetworkParent.HasIsKitchenObject()){
            return;
        }
        Transform kitchenObjectTrans =Instantiate(kitchenObjectSO.prefab);
        NetworkObject networkObjectKitchen = kitchenObjectTrans.GetComponent<NetworkObject>();
        networkObjectKitchen.Spawn(true);

        KitChenObject kitchenObject = kitchenObjectTrans.GetComponent<KitChenObject>();
        kitchenObject.SetKitchenObjectParent(IkitchenNetworkParent);
    }
    public int GetIdxFromKitchenObjectList(KitchenObjectSO kitchenObjectSO){
        return kitchenObjectList.kitchenObjectList.IndexOf(kitchenObjectSO);
    }
    public KitchenObjectSO GetKitchenObjectSOFromIdx(int idx){
        return kitchenObjectList.kitchenObjectList[idx];
    }
    public void DestroyKitchenObject(KitChenObject kitChenObject){
        DestroyKitchenObjectServerRpc(kitChenObject.NetworkObject);
    }
    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference networkObjectReference){
        networkObjectReference.TryGet(out NetworkObject kitchenNetworkObject);
        if(kitchenNetworkObject ==null){
            return;
        }
        KitChenObject kitChenObject = kitchenNetworkObject.GetComponent<KitChenObject>();
        ClearkitchenObjectClientRpc(networkObjectReference);
        kitChenObject.DestroySelf();
    }
    [ClientRpc]
    private void ClearkitchenObjectClientRpc(NetworkObjectReference networkObjectReference){
        networkObjectReference.TryGet(out NetworkObject kitchenNetworkObject);
        KitChenObject kitChenObject = kitchenNetworkObject.GetComponent<KitChenObject>();
    
        kitChenObject.ClearkitchenObject();
    }
    
    public bool IsPlayerIndexConnected(int playerIndex){
        return PlayerDataNetworkList.Count > playerIndex;
    }
    public PlayerData GetPlayerDataFromIndex(int playerIndex){
        return PlayerDataNetworkList[playerIndex];
    }
    public Color GetPlayerColor(int ColorId){
        return playerColorList[ColorId];
    }
    public PlayerData GetPlayerDataFromClientId(ulong ClientId){
        foreach(PlayerData playerData in PlayerDataNetworkList){
            if(playerData.ClientId==ClientId){
                return playerData;
            }
        }
        return default;
    }
    public PlayerData GetPlayerData(){
       return  GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }
    public void ChangePlayerColor(int colorId){
        ChangePlayerColorServerRpc(colorId);
    }
    public int GetPlayerDataIndexFromClientId(ulong ClientId){
        for(int i=0 ; i < PlayerDataNetworkList.Count;i++){
            if(PlayerDataNetworkList[i].ClientId == ClientId){
                return i;
            }
        }
        return -1;
    }
    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerColorServerRpc(int colorid, ServerRpcParams serverRpcParams=default){
        if(!IsColorAvailable(colorid)){
            return ; 
        }
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = PlayerDataNetworkList[playerDataIndex];
        playerData.colorId=colorid;
        PlayerDataNetworkList[playerDataIndex]=playerData;

    }
    private bool IsColorAvailable(int colorId){
        foreach(PlayerData playerData in PlayerDataNetworkList){
            if(playerData.colorId == colorId){
                return false;
            }
        }
        return true;
    }
    private int GetFirstUnusedColorId(){
        for(int i=0;i< playerColorList.Count;i++){
            if(IsColorAvailable(i)){
                return i;
            }
        }
        return -1;
    }
    public void KickPlayer(ulong ClientId){
        NetworkManager.Singleton.DisconnectClient(ClientId);
        NetworkManagerHost_OnclientDisconnectCallback(ClientId);
    }
}
