using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI textRecipeDelivery;
    [SerializeField] private Button playAgainBtn;
    private void Start()
    {
        GameManager.Instance.OnStateChange+=GameOverUI_OnStateChange;
        playAgainBtn.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.GameMenu);
            NetworkManager.Singleton.Shutdown();
        });
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
