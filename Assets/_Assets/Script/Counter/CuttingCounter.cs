using System;
using System.Collections;
using System.Collections.Generic;
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
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingKitchenSO = GettingCuttingKichen(GetKitchenObject().GetKitchenObjectSO());
                ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                    progressNomalize=(float)CuttingProgess/cuttingKitchenSO.CuttingProgess
                });
            }
        }
        else {
            if(player.HasIsKitchenObject()){
                  if(player.GetKitchenObject().TryGetPlate( out PlatesKitchenObject plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
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


    public override void InteractAlternate(Player player)
    {
         if(HasIsKitchenObject() && HasKitchenCuttingSO(GetKitchenObject().GetKitchenObjectSO())){
                CuttingProgess++;
                OnCut?.Invoke(this,EventArgs.Empty);
                AnyOnCut?.Invoke(this,EventArgs.Empty);
                // cuttingKitchenSO = GettingCuttingKichen(GetKitchenObject().GetKitchenObjectSO());
                 ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                    progressNomalize=(float)CuttingProgess/cuttingKitchenSO.CuttingProgess
                });
                if(CuttingProgess >= cuttingKitchenSO.CuttingProgess){
                    GetKitchenObject().DestroySelf();
                    CuttingProgess=0;
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
