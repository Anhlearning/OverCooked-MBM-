using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitChenObject : MonoBehaviour
{
    [SerializeField]private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO(){
        return kitchenObjectSO;
    }
    
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenParent){
        if(this.kitchenObjectParent != null){
            this.kitchenObjectParent.ClearKitchenObject();
        }
         this.kitchenObjectParent=kitchenParent;
         if(this.kitchenObjectParent.HasIsKitchenObject()){
            Debug.LogError("has been KitChenObject");
         }
         kitchenObjectParent.SetKitchenObject(this);
         transform.parent=kitchenObjectParent.GetKitchenObjectTransform();
         transform.localPosition=Vector3.zero;
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
    public static KitChenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,IKitchenObjectParent kitchenObjectParent){
        Transform kitchenObjectTransfom = Instantiate(kitchenObjectSO.prefab);
        kitchenObjectTransfom.localPosition=Vector3.zero;
        KitChenObject kitChenObject = kitchenObjectTransfom.GetComponent<KitChenObject>();
        kitChenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitChenObject;
    }
}
