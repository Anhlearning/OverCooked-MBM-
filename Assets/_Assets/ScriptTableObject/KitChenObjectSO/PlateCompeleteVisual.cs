using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompeleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject{
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObjectSO;
    }
    [SerializeField] private PlatesKitchenObject platesKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject>kitchenObjectSO_GameObjects;
    private void Start()
    {
        // kitchenObjectSO_GameObjects=new List<KitchenObjectSO_GameObject>();
        platesKitchenObject.OnAddInGredient+= Plates_OnAddIngredient;
        foreach(KitchenObjectSO_GameObject kitchenObjectSO_Game in kitchenObjectSO_GameObjects){
                kitchenObjectSO_Game.gameObjectSO.SetActive(false);
        }
    }

    private void Plates_OnAddIngredient(object sender, PlatesKitchenObject.OnAddInGredientEventArgs e)
    {
        foreach(KitchenObjectSO_GameObject kitchenObjectSO_Game in kitchenObjectSO_GameObjects){
            if(e.kitchenObjectSO == kitchenObjectSO_Game.kitchenObjectSO){
                kitchenObjectSO_Game.gameObjectSO.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
