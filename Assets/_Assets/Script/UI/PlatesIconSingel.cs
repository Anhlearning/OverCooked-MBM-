using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatesIconSingel : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetIconImageKitchenObject(KitchenObjectSO kitchenObjectSO){
        image.sprite=kitchenObjectSO.Sprite;
    }
}
