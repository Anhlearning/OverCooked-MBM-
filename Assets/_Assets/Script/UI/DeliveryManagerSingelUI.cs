using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform icon;
    private void Awake() {
        icon.gameObject.SetActive(false);
    }
    public void SetRecipeSO(RecipeSO recipeSO){
        recipeText.text=recipeSO.RecipeName;

        foreach(Transform child in iconContainer){
            if(child == icon) {
                continue;
            }
            Destroy(child.gameObject);
        }
        foreach(KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList){
            Transform kitchenObjectSpawn=Instantiate(icon,iconContainer);
            kitchenObjectSpawn.gameObject.SetActive(true);
            kitchenObjectSpawn.GetComponent<Image>().sprite=kitchenObjectSO.Sprite;
        }

    }
}
