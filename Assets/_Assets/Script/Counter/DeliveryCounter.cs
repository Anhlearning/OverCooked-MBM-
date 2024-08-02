using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if(!HasIsKitchenObject()){
            if(player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject)){
                DeliveryManager.Instance.DeliveryRecipe(platesKitchenObject);

                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
