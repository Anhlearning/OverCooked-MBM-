using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSeletedPlayer : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private Button kickButton;
    [SerializeField] private TextMeshPro playerNameText;

    private void Awake() {
        kickButton.onClick.AddListener(()=>{
            PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromIndex(playerIndex);
            KitchenGameLobby.Instance.KickPlayer(playerData.playerId.ToString());
            KitchenGameMultiplayer.Instance.KickPlayer(playerData.ClientId);
        });
    }
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkChange += KitchenGameMultiplayer_OnPlayerDataNetworkChange;
        SelectedCharacterReady.Instance.OnPlayerReadyChange += SelectedCharacterReady_OnPlayerReadyChange;
        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        UpdateVisualPlayer();
    }
    private void SelectedCharacterReady_OnPlayerReadyChange(object sender,EventArgs e){
        UpdateVisualPlayer();
    }
    private void KitchenGameMultiplayer_OnPlayerDataNetworkChange(object sender, EventArgs e)
    {
        UpdateVisualPlayer();
    }

    private void UpdateVisualPlayer(){
        if(KitchenGameMultiplayer.Instance.IsPlayerIndexConnected(playerIndex)){
            Show();
            PlayerData playerData=KitchenGameMultiplayer.Instance.GetPlayerDataFromIndex(playerIndex);
            readyGameObject.SetActive(SelectedCharacterReady.Instance.IsPlayerReady(playerData.ClientId));
            playerNameText.text=playerData.playerName.ToString();
            playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
        }
        else{
            Hide();
        }
    }
    private void OnDestroy() {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkChange -= KitchenGameMultiplayer_OnPlayerDataNetworkChange;
        SelectedCharacterReady.Instance.OnPlayerReadyChange -= SelectedCharacterReady_OnPlayerReadyChange;
    }
    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
}
