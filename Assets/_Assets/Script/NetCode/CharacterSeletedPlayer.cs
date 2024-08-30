using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSeletedPlayer : MonoBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private PlayerVisual playerVisual;
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkChange += KitchenGameMultiplayer_OnPlayerDataNetworkChange;
        SelectedCharacterReady.Instance.OnPlayerReadyChange += SelectedCharacterReady_OnPlayerReadyChange;
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
            playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerIndex));
        }
        else{
            Hide();
        }
    }
    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
}
