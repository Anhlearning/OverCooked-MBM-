using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FyringObjectSO", menuName = "FyringObjectSO",order=1)]
public class FyringObjectSO : ScriptableObject {
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fyringTimerMax;
}

