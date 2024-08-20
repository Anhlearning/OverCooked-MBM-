using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitChenObject : NetworkBehaviour
{
    [SerializeField]private KitchenObjectSO kitchenObjectSO;
    private FollowTransform followTransform;

    protected virtual void Awake() {
        followTransform=GetComponent<FollowTransform>();
    }
    private IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO(){
        return kitchenObjectSO;
    }
    
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenParent){
        SetKitchenObjectServerRpc(kitchenParent.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership =false)]
    private void SetKitchenObjectServerRpc(NetworkObjectReference networkObjectReference){
        SetKitchenObjectClientRpc(networkObjectReference);
    }
    [ClientRpc]
    private void SetKitchenObjectClientRpc(NetworkObjectReference networkObjectReference){
        networkObjectReference.TryGet(out NetworkObject networkObject);
        IKitchenObjectParent kitchenObjectParent = networkObject.GetComponent<IKitchenObjectParent>();
        if(this.kitchenObjectParent != null){
            this.kitchenObjectParent.ClearKitchenObject();
        }
         this.kitchenObjectParent=kitchenObjectParent;
         if(this.kitchenObjectParent.HasIsKitchenObject()){
            Debug.LogError("has been KitChenObject");
         }
        kitchenObjectParent.SetKitchenObject(this);
        followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectTransform());
    }
    public IKitchenObjectParent KitchenObjectParent(){
        return kitchenObjectParent;
    }
    public bool TryGetPlate(out PlatesKitchenObject platesKitchenObject){
        if(this is PlatesKitchenObject){
            platesKitchenObject= this as PlatesKitchenObject;
            return true;
        }
        else {
            platesKitchenObject=null;
            return false;
        }
    }
    public void DestroySelf(){
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }
    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent kitchenObjectParent){
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO,kitchenObjectParent);
    }
}
