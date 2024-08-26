using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            AddInGredientServerRpc(KitchenGameMultiplayer.Instance.GetIdxFromKitchenObjectList(kitchenObjectSO));
            return true;
        }
   }
    [ServerRpc(RequireOwnership = false)]
    private void AddInGredientServerRpc(int idxKitchenobject){
        AddInGredientClientRpc(idxKitchenobject);
    }
    [ClientRpc]
    private void AddInGredientClientRpc(int indexKitchenobject){
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIdx(indexKitchenobject);
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnAddInGredient?.Invoke(this,new OnAddInGredientEventArgs{
            kitchenObjectSO=kitchenObjectSO
            });
    }

   public List<KitchenObjectSO> GetKitchenObjectSOs(){
        return kitchenObjectSOList;
   }
}
