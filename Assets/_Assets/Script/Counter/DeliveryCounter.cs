using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance {get;set;}
    private void Awake() {
        Instance=this;
    }
    public override void Interact(Player player)
    {
        if(!HasIsKitchenObject()){
            if(player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject)){
                DeliveryManager.Instance.DeliveryRecipe(platesKitchenObject);

                KitChenObject.DestroyKitchenObject(player.GetKitchenObject());
            }
        }
    }
}
