using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button MenuGame;
    [SerializeField] private Button Resume;

    [SerializeField]private Button OptionMenu;

    private void Awake() {
        MenuGame.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.GameMenu);
        });
        Resume.onClick.AddListener(()=>{
            GameManager.Instance.TogglePause();
        });
        OptionMenu.onClick.AddListener(()=>{
            OptionUI.Instance.Show(Show);
            Hide();
        });
    }
    private void Start() {
        GameManager.Instance.OnPauseMenu+=DeliveryManager_OnPauseMenu;
        GameManager.Instance.OffPauseMenu+=DeliveryManager_OffPauseMenu;
        Hide();
    }

    private void DeliveryManager_OffPauseMenu(object sender, EventArgs e)
    {
        Hide();
    }

    private void DeliveryManager_OnPauseMenu(object sender, EventArgs e)
    {
        Show();
    }

    private void Show(){
        gameObject.SetActive(true);
        Resume.Select();
    }
    private void Hide(){
        gameObject.SetActive(false);
    }
}
