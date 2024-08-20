using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatesKitchenObject : KitChenObject
{  
    public event EventHandler<OnAddInGredientEventArgs> OnAddInGredient;
    public class OnAddInGredientEventArgs : EventArgs{
        public KitchenObjectSO kitchenObjectSO;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
   private List<KitchenObjectSO> kitchenObjectSOList;

    protected override void Awake() {
        base.Awake();
        kitchenObjectSOList=new List<KitchenObjectSO>();
    }
   public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO){
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO)){
            return false;
        }
        if(kitchenObjectSOList.Contains(kitchenObjectSO)){
            return false;
        }
        else {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnAddInGredient?.Invoke(this,new OnAddInGredientEventArgs{
                kitchenObjectSO=kitchenObjectSO
            });
            return true;
        }
   }
   public List<KitchenObjectSO> GetKitchenObjectSOs(){
        return kitchenObjectSOList;
   }
}
