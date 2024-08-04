using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
    }
    private void Start() {
        DeliveryManager.Instance.OnSpawnRecipe+= DeliveryManager_OnSpawnRecipe;
        DeliveryManager.Instance.OnRemoveRecipe+=DeliveryManager_OnRemoveRecipe;
        UpdateVisual();
    }

    private void DeliveryManager_OnRemoveRecipe(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnSpawnRecipe(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual(){
        foreach(Transform child in container){
            if(child == recipeTemplate){
                continue;
            }
            Destroy(child.gameObject);
        }
        foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetListRecipeSO()){
            Transform recipeTemplateSpawn = Instantiate(recipeTemplate,container);
            recipeTemplateSpawn.gameObject.SetActive(true);
            recipeTemplateSpawn.GetComponent<DeliveryManagerSingelUI>().SetRecipeSO(recipeSO);
        }
    }
}
