using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class DeliveryManager: NetworkBehaviour 
{   
    public event EventHandler DeliveryFalied;
    public event EventHandler DeliverySuccees;
    public event EventHandler OnSpawnRecipe;
    public event EventHandler OnRemoveRecipe;

    public static DeliveryManager Instance{get;set;}  
    [SerializeField] private RecipeSOList recipeSOList;
    private List<RecipeSO>wattingRecipeSOList;
    private float timeSpawnRecipe=4f;
    private float timeSpawnRecipeMax=4f;
    private int wattingRecipeSpawnMax=4;
    private int recipeDelivery;

    private void Awake() {
        Instance=this;
        wattingRecipeSOList=new List<RecipeSO>();
       
    }
    private void Update() {
        if(!IsServer){
            return ;
        }
        timeSpawnRecipe-=Time.deltaTime;
        if(timeSpawnRecipe<=0f){
            timeSpawnRecipe=timeSpawnRecipeMax;
            if(GameManager.Instance.IsGamePlaying()&&wattingRecipeSOList.Count < wattingRecipeSpawnMax){
                int recipeIdx=UnityEngine.Random.Range(0,recipeSOList.RecipeListSO.Count);
                OnSpawnRecipeClientRpc(recipeIdx);
            }
            
        }
    }
    [ClientRpc]
    private void OnSpawnRecipeClientRpc(int indexRecipe){
        RecipeSO recipeSOSpawn = recipeSOList.RecipeListSO[indexRecipe];
        wattingRecipeSOList.Add(recipeSOSpawn);
        OnSpawnRecipe?.Invoke(this,EventArgs.Empty);
    }

    public void DeliveryRecipe(PlatesKitchenObject platesKitchenObject){
        for(int i=0; i < wattingRecipeSOList.Count;i++){
            RecipeSO wattingRecipeSO = wattingRecipeSOList[i];
            if(wattingRecipeSO.kitchenObjectSOList.Count == platesKitchenObject.GetKitchenObjectSOs().Count){
                bool checkKitchenObjectList=true;
                foreach(KitchenObjectSO waKitchenObjectSO in wattingRecipeSO.kitchenObjectSOList){
                    bool checkKitchenObjectSO =false;
                    foreach (KitchenObjectSO platesKitchenSO in platesKitchenObject.GetKitchenObjectSOs())
                    {
                        if(waKitchenObjectSO == platesKitchenSO){
                            checkKitchenObjectSO=true;
                            break;
                        }
                    }
                    if(!checkKitchenObjectSO){
                        checkKitchenObjectList=false;
                        break;
                    }
                }
                if(checkKitchenObjectList){
                    DeliveryCorrectServerRpc(i);
                    return;
                }
            }
        }
        DeliveryFaliedServerRpc();        
    }
    [ServerRpc(RequireOwnership =false)]
    private void DeliveryCorrectServerRpc(int index){
        DeliveryCorrectClientRpc(index);
    }
    [ClientRpc]
    private void DeliveryCorrectClientRpc(int index){
        wattingRecipeSOList.RemoveAt(index);
        recipeDelivery++;
        OnRemoveRecipe?.Invoke(this,EventArgs.Empty);
        DeliverySuccees?.Invoke(this,EventArgs.Empty);
    }
    [ServerRpc(RequireOwnership =false)]
    private void DeliveryFaliedServerRpc(){
        DeliveryFaliedClientRpc();
    }
    [ClientRpc]
    private void DeliveryFaliedClientRpc(){
         DeliveryFalied?.Invoke(this,EventArgs.Empty);
    }
    public List<RecipeSO> GetListRecipeSO(){
        return wattingRecipeSOList;
    }
    public int GetRecipeDelivery(){
        return recipeDelivery;
    }
}
