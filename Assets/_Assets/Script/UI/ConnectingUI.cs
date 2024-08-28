using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame += KitchenGameMultiplayer_OnTryingToJoinGame;
        KitchenGameMultiplayer.Instance.OnFailToJoinGame += KitchenGameMultiplayer_OnTryingToFailGame;
        Hide();
    }

    private void KitchenGameMultiplayer_OnTryingToFailGame(object sender, EventArgs e)
    {
        Hide();
    }

    private void KitchenGameMultiplayer_OnTryingToJoinGame(object sender, EventArgs e)
    {
        Show();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
    private void OnDestroy() {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame -= KitchenGameMultiplayer_OnTryingToJoinGame;
        KitchenGameMultiplayer.Instance.OnFailToJoinGame -= KitchenGameMultiplayer_OnTryingToFailGame;
    }
}
