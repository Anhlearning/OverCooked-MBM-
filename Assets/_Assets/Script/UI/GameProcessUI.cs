using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameProcessUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDone;
    [SerializeField] private Transform gameSuccesTrans;
    [SerializeField] private Button menuBtn;
    [SerializeField]private Button quitBtn;
    private void Awake() {
        menuBtn.onClick.AddListener(()=>{
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.GameMenu);
        });
        quitBtn.onClick.AddListener(()=>{
            Application.Quit();
        });
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeUpdateChange += DeliveryManager_OnUpdateRecipeSucces;
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
        Hide();
        UpdateVisual();
    }

    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if(GameManager.Instance.IsGameSucces()){
            Show();
            Time.timeScale = 0f;
        }
    }

    private void DeliveryManager_OnUpdateRecipeSucces(object sender, EventArgs e)
    {
        UpdateVisual();
    }
    private void Hide(){
        gameSuccesTrans.gameObject.SetActive(false);
    }
    private void Show(){
        gameSuccesTrans.gameObject.SetActive(true);
    }

    private void UpdateVisual(){
        recipeDone.text ="RECIPE SUCCES: "+DeliveryManager.Instance.GetRecipeDelivery().ToString() + "/" + DeliveryManager.Instance.GetRecipeDeliveryMax();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
