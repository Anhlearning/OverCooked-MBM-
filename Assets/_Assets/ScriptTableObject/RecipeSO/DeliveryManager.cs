using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
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
        timeSpawnRecipe-=Time.deltaTime;
        if(timeSpawnRecipe<=0f){
            timeSpawnRecipe=timeSpawnRecipeMax;
            if(GameManager.Instance.IsGamePlaying()&&wattingRecipeSOList.Count < wattingRecipeSpawnMax){
                RecipeSO recipeSOSpawn = recipeSOList.RecipeListSO[UnityEngine.Random.Range(0,recipeSOList.RecipeListSO.Count)];
                Debug.Log(recipeSOSpawn.RecipeName); 
                wattingRecipeSOList.Add(recipeSOSpawn);
                OnSpawnRecipe?.Invoke(this,EventArgs.Empty);
            }
            
        }
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
                    Debug.Log("Recipe Correct");
                    OnRemoveRecipe?.Invoke(this,EventArgs.Empty);
                    DeliverySuccees?.Invoke(this,EventArgs.Empty);
                    recipeDelivery++;
                    wattingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }
        DeliveryFalied?.Invoke(this,EventArgs.Empty);
        Debug.Log("Recipe Give DeliveryCounter Not correct");
    }
    public List<RecipeSO> GetListRecipeSO(){
        return wattingRecipeSOList;
    }
    public int GetRecipeDelivery(){
        return recipeDelivery;
    }
}
