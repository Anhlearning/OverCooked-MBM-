using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUp : MonoBehaviour
{
    // Start is called before the first frame updateaw

    private void Awake() {
        if(NetworkManager.Singleton != null){
            Destroy(NetworkManager.Singleton.gameObject);
        }
        if(KitchenGameMultiplayer.Instance !=null){
            Destroy(KitchenGameMultiplayer.Instance.gameObject);
        }
    }
}
