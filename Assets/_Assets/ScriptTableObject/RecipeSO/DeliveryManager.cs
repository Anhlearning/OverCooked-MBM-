using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{   
    public static DeliveryManager Instance{get;set;}  
    [SerializeField] private RecipeSOList recipeSOList;
    private List<RecipeSO>wattingRecipeSOList;
    private float timeSpawnRecipe=4f;
    private float timeSpawnRecipeMax=4f;
    private int wattingRecipeSpawnMax=4;
    private void Awake() {
        Instance=this;
        wattingRecipeSOList=new List<RecipeSO>();
    }
    private void Update() {
        timeSpawnRecipe-=Time.deltaTime;
        if(timeSpawnRecipe<=0f){
            timeSpawnRecipe=timeSpawnRecipeMax;
            if(wattingRecipeSOList.Count < wattingRecipeSpawnMax){
                RecipeSO recipeSOSpawn = recipeSOList.RecipeListSO[Random.Range(0,recipeSOList.RecipeListSO.Count)];
                Debug.Log(recipeSOSpawn.RecipeName); 
                wattingRecipeSOList.Add(recipeSOSpawn);
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
                    wattingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }
        Debug.Log("Recipe Give DeliveryCounter Not correct");
    }
}
