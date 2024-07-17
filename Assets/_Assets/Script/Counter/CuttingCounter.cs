using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
   [SerializeField] private CuttingKitchenSO[] listCuttingKitchen ;
    public event EventHandler<ProgressBarEvent>ProgressBar;
    public event EventHandler OnCut;
    public class ProgressBarEvent : EventArgs{
        public float  progressNomalize;
    }
   private int CuttingProgess;
    public override void Interact(Player player){
        if(!HasIsKitchenObject()){
            if(player.HasIsKitchenObject() && HasKitchenCuttingSO(player.GetKitchenObject().GetKitchenObjectSO())){
                player.GetKitchenObject().SetKitchenObjectParent(this);
                CuttingProgess=0;
                CuttingKitchenSO cuttingKitchenSO = GettingCuttingKichen(GetKitchenObject().GetKitchenObjectSO());
                ProgressBar?.Invoke(this,new ProgressBarEvent{
                    progressNomalize=(float)CuttingProgess/cuttingKitchenSO.CuttingProgess
                });
            }
        }
        else {
            if(player.HasIsKitchenObject()){
                //player is carrying object 
            }
            else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }


    public override void InteractAlternate(Player player)
    {
         if(HasIsKitchenObject() && HasKitchenCuttingSO(GetKitchenObject().GetKitchenObjectSO())){
                CuttingProgess++;
                OnCut?.Invoke(this,EventArgs.Empty);
                CuttingKitchenSO cuttingKitchenSO = GettingCuttingKichen(GetKitchenObject().GetKitchenObjectSO());
                 ProgressBar?.Invoke(this,new ProgressBarEvent{
                    progressNomalize=(float)CuttingProgess/cuttingKitchenSO.CuttingProgess
                });
                if(CuttingProgess >= cuttingKitchenSO.CuttingProgess){
                    GetKitchenObject().DestroySelf();
                    KitChenObject.SpawnKitchenObject(cuttingKitchenSO.outputKitchen,this);
               }

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
