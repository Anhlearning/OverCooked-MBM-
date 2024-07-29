using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesIcon : MonoBehaviour
{
    [SerializeField]private PlatesKitchenObject platesKitchenObject;
    [SerializeField]private Transform iconTemplate;
    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start() {
        platesKitchenObject.OnAddInGredient+= PlatesKitchenObject_OnAddIngredient;
    }

    private void PlatesKitchenObject_OnAddIngredient(object sender, PlatesKitchenObject.OnAddInGredientEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach(Transform child in transform){
            if(child == iconTemplate) continue;
            else {
                Destroy(child.gameObject);
            }
            
        }
        foreach(KitchenObjectSO kitchenObjectSO in platesKitchenObject.GetKitchenObjectSOs()){
            Transform iconTrans= Instantiate(iconTemplate,transform);
            iconTrans.gameObject.SetActive(true);
            iconTrans.GetComponent<PlatesIconSingel>().SetIconImageKitchenObject(kitchenObjectSO);
        }
    }
}
