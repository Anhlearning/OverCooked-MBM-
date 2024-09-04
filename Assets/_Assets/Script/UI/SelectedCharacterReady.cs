using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedCharacterReady : NetworkBehaviour
{
    private Dictionary<ulong,bool>playerReadyDictionary;
    
    public static SelectedCharacterReady Instance;
    public event EventHandler OnPlayerReadyChange;
    private void Awake() {
        Instance=this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }
    private void Start()
    {
        
    }

    public void SetPlayerReady(){
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership =false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default){
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId]=true;
        ClientIdIsReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        bool allClientReady=true;
        foreach(ulong IdClient in NetworkManager.Singleton.ConnectedClientsIds){
            if(!playerReadyDictionary.ContainsKey(IdClient) || !playerReadyDictionary[IdClient]){
                allClientReady=false;
                break;
            }
        }
        if(allClientReady==true){
            KitchenGameLobby.Instance.DeleteLobby();
            Loader.NetworkLoad(Loader.Scene.GamePlay);
        }
    }
    [ClientRpc]
    private void ClientIdIsReadyClientRpc(ulong ClientId){
       
        playerReadyDictionary[ClientId]=true;
        OnPlayerReadyChange?.Invoke(this,EventArgs.Empty);
    }
    public bool IsPlayerReady(ulong ClientId){
        if(playerReadyDictionary.ContainsKey(ClientId)){
            return  playerReadyDictionary[ClientId];
        }
        return false;
    }
}
