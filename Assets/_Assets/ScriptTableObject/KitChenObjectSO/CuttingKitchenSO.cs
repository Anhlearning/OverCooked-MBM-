using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CuttingKitchenSO", menuName = "CuttingKitchenSO", order = 0)]
public class CuttingKitchenSO : ScriptableObject {
     [SerializeField] public KitchenObjectSO inputKitchen;
     [SerializeField] public KitchenObjectSO outputKitchen; 

     public int CuttingProgess;
}

