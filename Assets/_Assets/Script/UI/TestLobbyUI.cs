using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLobbyUI : MonoBehaviour
{
    [SerializeField]
    private Button createGameBtn;
    [SerializeField]
    private Button joinGameBtn;
    private void Start()
    {
        createGameBtn.onClick.AddListener(()=>{
            KitchenGameMultiplayer.Instance.StartHost();
            Loader.NetworkLoad(Loader.Scene.SelectedCharacter);
        });
        joinGameBtn.onClick.AddListener(()=>{
            KitchenGameMultiplayer.Instance.StartClient();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
