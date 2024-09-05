using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessage : MonoBehaviour
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private TextMeshProUGUI message;
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailToJoinGame += KitchenGameMultiplayer_OnFailToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobby += KitchenGameLobby_OnCreateLobby;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinLobby += KitchenGameLobby_OnJoinLobby;
        KitchenGameLobby.Instance.OnQuickJoinLobbyFailed += KitchenGameLobby_OnQuickJoinFailed;
        KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_OnJoinFailed;
        Hide();
        closeBtn.onClick.AddListener(Hide);
    }

    private void KitchenGameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed Join Lobby!!");
    }

    private void KitchenGameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Could Not Find QuickJoin!!");
    }

    private void KitchenGameLobby_OnJoinLobby(object sender, EventArgs e)
    {
        ShowMessage("Joinning Lobby... ");
    }

    private void KitchenGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Create Lobby Failed...!!");
    }

    private void KitchenGameLobby_OnCreateLobby(object sender, EventArgs e)
    {
        ShowMessage("Create Lobby....");
    }

    private void KitchenGameMultiplayer_OnFailToJoinGame(object sender, EventArgs e)
    {
        message.text=NetworkManager.Singleton.DisconnectReason;
        if(message.text == ""){
            message.text="Client DissConnect";
        }
        else {
            ShowMessage(message.text);
        }
    }
    private void ShowMessage(string context){
        Show();
        message.text=context;
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
