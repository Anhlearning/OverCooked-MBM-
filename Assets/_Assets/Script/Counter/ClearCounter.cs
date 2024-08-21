using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player){
        if(!HasIsKitchenObject()){
            if(player.HasIsKitchenObject()){
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else {
            if(player.HasIsKitchenObject()){
                //player carrying Plates 
                if(player.GetKitchenObject().TryGetPlate( out PlatesKitchenObject plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        KitChenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
                else {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)){
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())){
                            KitChenObject.DestroyKitchenObject(player.GetKitchenObject());
                        }
                    }
                }
            }
            else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
