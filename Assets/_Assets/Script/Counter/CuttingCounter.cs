using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
   [SerializeField] private KitchenObjectSO KitchenObjectCutting;
    public override void Interact(Player player){
        if(!HasIsKitchenObject()){
            if(player.HasIsKitchenObject()){
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
         
    }
}
