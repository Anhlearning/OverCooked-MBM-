using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectedUI : MonoBehaviour
{
    [SerializeField] private Button GameMenuBtn;
    [SerializeField] private Button ReadyBtn;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    private void Start()
    {
        GameMenuBtn.onClick.AddListener(()=>{
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.GameMenu);
        });
        ReadyBtn.onClick.AddListener(()=>{
            SelectedCharacterReady.Instance.SetPlayerReady();
        });
        Lobby lobby = KitchenGameLobby.Instance.GetLobby();
        lobbyNameText.text="Lobby Name: "+lobby.Name;
        lobbyCodeText.text="Lobby Code: "+lobby.LobbyCode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
