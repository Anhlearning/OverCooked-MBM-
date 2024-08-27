using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDissconnectUI : MonoBehaviour
{
    [SerializeField] private Button playAgainBtn;

    private void Start()
    {
        // playAgainBtn.onClick.AddListener(()=>{
        //     NetworkManager.Singleton.Shutdown();
        //     Loader.Load(Loader.Scene.GameMenu);
        // });
        Debug.Log(NetworkManager.ServerClientId);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnDissconnectCallBack;
        Hide();
    }

    private void NetworkManager_OnDissconnectCallBack(ulong clientId)
    {
        if(clientId == 1){
            Show();
        }
        
    }
    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
