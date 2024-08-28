using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionReasonMessageUI : MonoBehaviour
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private TextMeshProUGUI message;
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailToJoinGame += KitchenGameMultiplayer_OnFailToJoinGame;
        Hide();
        closeBtn.onClick.AddListener(Hide);
    }

    private void KitchenGameMultiplayer_OnFailToJoinGame(object sender, EventArgs e)
    {
        Show();
        message.text=NetworkManager.Singleton.DisconnectReason;

        if(message.text==""){
            message.text="FAIL TO CONNECT";
        }
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
    // Update is called once per frame
    private void OnDestroy() {
         KitchenGameMultiplayer.Instance.OnFailToJoinGame -= KitchenGameMultiplayer_OnFailToJoinGame;
    }
}
