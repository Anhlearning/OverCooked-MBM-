using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter,IProgressBar
{
    
   public static event EventHandler AnyOnCut; 

   new public static void resetEventStatic(){
        AnyOnCut=null;
   }
   [SerializeField] private CuttingKitchenSO[] listCuttingKitchen ;
   public event EventHandler <IProgressBar.ProgressBarEvent> ProgressBar;
   public event EventHandler OnCut;
   private int CuttingProgess=0;
   private CuttingKitchenSO cuttingKitchenSO;
    public override void Interact(Player player){
        if(!HasIsKitchenObject()){
            if(player.HasIsKitchenObject() && HasKitchenCuttingSO(player.GetKitchenObject().GetKitchenObjectSO())){
                KitChenObject kitChenObject = player.GetKitchenObject();
                //trong single player thì mình setkitchenobjectparent thì kitchenobject sẽ được set ngay lập tức và kitchenobject từ null -> not null ngay lập tức  
                kitChenObject.SetKitchenObjectParent(this);
                //=>>> getkitchenobject trong single nó sẽ not null ngay lập tức khi setkitchenobjectparent 
                cuttingKitchenSO = GettingCuttingKichen(kitChenObject.GetKitchenObjectSO());
                InteractLogicPlaceObjectOnCounterServerRpc();
            }
        }
        else {
            if(player.HasIsKitchenObject()){
                  if(player.GetKitchenObject().TryGetPlate( out PlatesKitchenObject plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        KitChenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
            }
            else {
                ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                    progressNomalize=0f
                });
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc(){
        InteractLogicPlaceObjectOnCounterClientRpc();
    }
    [ClientRpc]
    private void InteractLogicPlaceObjectOnCounterClientRpc(){
        CuttingProgess=0;    
        ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
            progressNomalize=0f
        });
    }
    public override void InteractAlternate(Player player)
    {
        if(HasIsKitchenObject() && HasKitchenCuttingSO(GetKitchenObject().GetKitchenObjectSO())){
                CutObjectServerRpc();
                TestCuttingDoneProgressServerRpc();
         }
    }
    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc(){  
        CutobjectClientRpc();
    }
    [ClientRpc]
    private void CutobjectClientRpc(){
        CuttingProgess++;
        OnCut?.Invoke(this,EventArgs.Empty);
        AnyOnCut?.Invoke(this,EventArgs.Empty);
        cuttingKitchenSO = GettingCuttingKichen(GetKitchenObject().GetKitchenObjectSO());
        ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
            progressNomalize=(float)CuttingProgess/cuttingKitchenSO.CuttingProgess
        });
    }
    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingDoneProgressServerRpc(){
        cuttingKitchenSO = GettingCuttingKichen(GetKitchenObject().GetKitchenObjectSO());
        if(CuttingProgess >= cuttingKitchenSO.CuttingProgess){
            KitChenObject.DestroyKitchenObject(GetKitchenObject());
            CuttingProgess=0;
            KitChenObject.SpawnKitchenObject(cuttingKitchenSO.outputKitchen,this);
        }
    }

    private bool HasKitchenCuttingSO(KitchenObjectSO  kitchenObjectSO){
        return GettingCuttingKichen(kitchenObjectSO) !=null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO){
        CuttingKitchenSO cuttingKitchenSO = GettingCuttingKichen(kitchenObjectSO);
        if(cuttingKitchenSO !=null){
            return cuttingKitchenSO.outputKitchen;
        }
        else {
            return null;
        }
    }

    private CuttingKitchenSO GettingCuttingKichen(KitchenObjectSO kitchenObjectSO){
        foreach(CuttingKitchenSO cuttingKitchenSO in listCuttingKitchen){
            if(cuttingKitchenSO.inputKitchen == kitchenObjectSO ){
                return cuttingKitchenSO;
            }
        }
        return null;
    }

}
