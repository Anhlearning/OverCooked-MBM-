using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport.Error;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance{get;set;} 
    [SerializeField] private KitchenObjectList kitchenObjectList;
    private void Awake() {
        Instance=this;
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
