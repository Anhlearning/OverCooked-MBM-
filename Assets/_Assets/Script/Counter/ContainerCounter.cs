using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter
{
    [SerializeField]private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnplayerGrabbOject;
    public override void Interact(Player player){
        if(!player.HasIsKitchenObject()){
            KitChenObject.SpawnKitchenObject(kitchenObjectSO,player);
            OnplayerGrabbOject?.Invoke(this,EventArgs.Empty);
        }
    }
}
