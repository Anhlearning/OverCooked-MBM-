using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "KitchenObjectSO")]

public class KitchenObjectSO : ScriptableObject {
    public Transform prefab;
    public Sprite Sprite;
    public string Object;
}

