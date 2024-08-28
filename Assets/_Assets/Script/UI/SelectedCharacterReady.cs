using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SelectedCharacterReady : NetworkBehaviour
{
    private Dictionary<ulong,bool>playerReadyDictionary;
    
    public static SelectedCharacterReady Instance;
    private void Awake() {
        Instance=this;
    }
    private void Start()
    {
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady(){
        SetPlayerReadyServerRpc();
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
            Loader.NetworkLoad(Loader.Scene.GamePlay);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
