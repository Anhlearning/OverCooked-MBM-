using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private Transform kitChenPrefab;
    [SerializeField] private Transform counterTopPoint;
    public void Interact(){
        Instantiate(kitChenPrefab,counterTopPoint);    
    }

}
