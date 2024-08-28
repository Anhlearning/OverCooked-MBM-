using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport.Error;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance{get;set;} 

    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailToJoinGame;
    private const int MAXPLAYERJOINGAME=4;
    [SerializeField] private KitchenObjectList kitchenObjectList;
    private void Awake() {
        Instance=this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartClient(){
        OnTryingToJoinGame?.Invoke(this,EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnclientDissconnect;
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_OnclientDissconnect(ulong obj)
    {
        OnFailToJoinGame?.Invoke(this,EventArgs.Empty);
    }

    public void StartHost(){
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
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
        Transform kitchenObjectTrans =Instantiate(kitchenObjectSO.prefab);
        NetworkObject networkObjectKitchen = kitchenObjectTrans.GetComponent<NetworkObject>();
        networkObjectKitchen.Spawn(true);

        networkObjectReference.TryGet(out NetworkObject kitchenParentNetworkObject);
        IKitchenObjectParent IkitchenNetworkParent = kitchenParentNetworkObject.GetComponent<IKitchenObjectParent>();

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
    
}
