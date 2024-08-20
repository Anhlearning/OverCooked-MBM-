using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : NetworkBehaviour,IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaceHere;
    public static void resetEventStatic(){
        OnAnyObjectPlaceHere=null;
    }
    [SerializeField] protected Transform counterTopPoint;

    private KitChenObject kitChenObject;
    public virtual void InteractAlternate(Player player){
        
    }
    public virtual void Interact(Player player){

    }
    public void SetKitchenObject(KitChenObject kitChenObject){
        this.kitChenObject=kitChenObject;
        if(kitChenObject != null){
            OnAnyObjectPlaceHere?.Invoke(this,EventArgs.Empty);
        }
    }
    public KitChenObject GetKitchenObject(){
        return kitChenObject;
    }
    public bool HasIsKitchenObject(){
        return kitChenObject !=null;
    }
    public void ClearKitchenObject(){
        this.kitChenObject=null;
    }
    public Transform GetKitchenObjectTransform(){
        return counterTopPoint;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}

