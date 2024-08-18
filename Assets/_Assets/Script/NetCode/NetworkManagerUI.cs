using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button ClientBtn;
    [SerializeField] private Button HostBtn;
    private void Start() {
        ClientBtn.onClick.AddListener(() =>
        {
            Debug.Log("CLIENT");
            NetworkManager.Singleton.StartClient(); 
            hide();
        });
        HostBtn.onClick.AddListener(() => {
            Debug.Log("HOST");
            NetworkManager.Singleton.StartHost();
            hide(); 
        });
    }

    private void hide()
    {
        gameObject.SetActive(false);    
    }
}
