using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{
    
    [SerializeField] protected Transform counterTopPoint;

    private KitChenObject kitChenObject;
    public virtual void InteractAlternate(Player player){
        
    }
    public virtual void Interact(Player player){

    }
    public void SetKitchenObject(KitChenObject kitChenObject){
        this.kitChenObject=kitChenObject;
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
}

