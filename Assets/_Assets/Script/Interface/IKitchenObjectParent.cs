using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent 
{
  
    public void SetKitchenObject(KitChenObject kitChenObject);
    public KitChenObject GetKitchenObject();
    public bool HasIsKitchenObject();
    public void ClearKitchenObject();
    public Transform GetKitchenObjectTransform();
}
