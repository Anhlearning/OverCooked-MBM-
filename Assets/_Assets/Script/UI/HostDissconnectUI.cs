using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDissconnectUI : MonoBehaviour
{
    [SerializeField] private Button playAgainBtn;
    // NetworkVariable<bool>isHostDissconnect = new NetworkVariable<bool>(false);
    private void Start()
    {
        playAgainBtn.onClick.AddListener(()=>{
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.GameMenu);
        });
        //NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnDissconnectCallBack;
        NetworkManager.Singleton.OnClientStopped += NetworkManager_OnClientDissconnect;
        Hide();
    }

    private void NetworkManager_OnClientDissconnect(bool obj)
    {
        Show();
    }
    private void OnDestroy() {
        if(NetworkManager.Singleton !=null){
            NetworkManager.Singleton.OnClientStopped -= NetworkManager_OnClientDissconnect;
        }
    }
    // private void NetworkManager_OnDissconnectCallBack(ulong ClientId)
    // {   Debug.Log("Client ID dissconnect is "+ ClientId);
    //     if(ClientId == NetworkManager.ServerClientId){
    //         Show();
    //     }
    // }
    // private void LateUpdate() {
    //     if(isHostDissconnect){
    //         isHostDissconnect=false;
    //         Show();
    //     }
    // }
    private void Hide(){
        gameObject.SetActive(false);
    }
    private void Show(){
        gameObject.SetActive(true);
    }
}
