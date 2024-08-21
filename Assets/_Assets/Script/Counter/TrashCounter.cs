using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void resetEventStatic(){
        OnAnyObjectTrashed=null;
    }
    public override void Interact(Player player)
    {
        if(player.HasIsKitchenObject()){
            KitChenObject.DestroyKitchenObject(player.GetKitchenObject());
            InteractServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractServerRpc(){
        InteractClientRpc();
    }
    [ClientRpc]
    private void InteractClientRpc(){
        OnAnyObjectTrashed?.Invoke(this,EventArgs.Empty);
    }
}
