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
    }
}
