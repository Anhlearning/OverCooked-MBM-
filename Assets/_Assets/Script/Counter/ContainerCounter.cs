using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter
{
    [SerializeField]private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnplayerGrabbOject;
    public override void Interact(Player player){
        if(!player.HasIsKitchenObject()){
            KitChenObject.SpawnKitchenObject(kitchenObjectSO,player);
            InteractServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractServerRpc(){
        InteractClientRpc();
    }
    [ClientRpc]
    private void InteractClientRpc(){
        OnplayerGrabbOject?.Invoke(this,EventArgs.Empty);
    }
}
