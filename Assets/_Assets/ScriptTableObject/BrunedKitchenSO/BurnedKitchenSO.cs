using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnedKitchenSO", menuName = "BurnedKitchenSO",order=2)]
public class BurnedKitchenSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fyringTimerMax;
}
