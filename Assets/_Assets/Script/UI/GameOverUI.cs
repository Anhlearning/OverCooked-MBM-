using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI textRecipeDelivery;
    private void Start()
    {
        GameManager.Instance.OnStateChange+=GameOverUI_OnStateChange;
        Hide();
    }

    private void GameOverUI_OnStateChange(object sender, EventArgs e)
    {
        if(GameManager.Instance.IsGameOver()){
            Show();
        }
        else {
            Hide();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
        textRecipeDelivery.text=DeliveryManager.Instance.GetRecipeDelivery().ToString();
    }
}
