using System;
using System.Collections;
using System.Collections.Generic;
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
            OnAnyObjectTrashed?.Invoke(this,EventArgs.Empty);
            player.GetKitchenObject().DestroySelf();
            
        }
    }
}
