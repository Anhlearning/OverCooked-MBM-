using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName ="KitchenObjectList",menuName ="KitchenObjectList")]
public class KitchenObjectList : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectList;
}
