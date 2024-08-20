using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter 
{   
    public event EventHandler OnPlateSpawn;
    public event EventHandler OnPlateRemove;
    [SerializeField] private KitchenObjectSO PlatesObjectSO;
    private float spawnPlatesTimer=0f;
    private float spawnPlatesTimerMax=4f;
    private int platesCount;
    private int platesCountMax=4;
    private void Update() {
        if(!IsServer){
            return; 
        }
        spawnPlatesTimer += Time.deltaTime;
        if(GameManager.Instance.IsGamePlaying()&&spawnPlatesTimer >= spawnPlatesTimerMax){
            spawnPlatesTimer=0f;
            if(platesCount <= platesCountMax){
                spawnPlatesServerRpc();
            }
        }
    }
    [ServerRpc]
    private void spawnPlatesServerRpc(){
        spawnPlatesClientRpc();
    }
    [ClientRpc]
    private void spawnPlatesClientRpc(){
            platesCount++;
            OnPlateSpawn?.Invoke(this,EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if(!player.HasIsKitchenObject()){
            if(platesCount > 0){
                platesCount --;
                KitChenObject.SpawnKitchenObject(PlatesObjectSO,player);
                InteractServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void InteractServerRpc(){
        InteractClientRpc();
    }
    [ClientRpc]
    private void InteractClientRpc(){
        OnPlateRemove?.Invoke(this,EventArgs.Empty);
    }
}
