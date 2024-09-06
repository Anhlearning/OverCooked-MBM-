using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button createLobbyBtn;
    [SerializeField] private Button joinLobbyBtn;
    [SerializeField] private Button joinCodeBtn;
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private Transform lobbyListContainer;
    [SerializeField] private Transform template;
    private void Awake()
    {
        mainMenuBtn.onClick.AddListener(()=>{
            KitchenGameLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.GameMenu);
        });
        createLobbyBtn.onClick.AddListener(()=>{
            lobbyCreateUI.Show();
        });
        joinLobbyBtn.onClick.AddListener(()=>{
            KitchenGameLobby.Instance.QuickJoin();
        });
        joinCodeBtn.onClick.AddListener(()=>{
            KitchenGameLobby.Instance.JoinWithCode(joinCodeInput.text);
        });
    }
    private void Start() {
        playerNameInputField.text=KitchenGameMultiplayer.Instance.GetPlayerName();
        playerNameInputField.onValueChanged.AddListener((string newText)=>{
            KitchenGameMultiplayer.Instance.SetPlayerName(newText);
        });
        UpdateLobby(new List<Lobby>());
        KitchenGameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_OnLobbyListChange;
    }

    private void KitchenGameLobby_OnLobbyListChange(object sender, KitchenGameLobby.OnLobbyListChangeEventArgs e)
    {
        UpdateLobby(e.lobbyList);
    }

    private void UpdateLobby(List<Lobby> lobbies){
        foreach (Transform child in lobbyListContainer)
        {
            if(child == template){
                continue;
            }
            Destroy(child.gameObject);
        }
        foreach (Lobby lobby in lobbies)
        {
            Transform lobbyContainerSingel= Instantiate(template,lobbyListContainer);
            lobbyContainerSingel.gameObject.SetActive(true);
            lobbyContainerSingel.GetComponent<LobbyContainerSingelUI>().SetLobby(lobby);
        }

    }
    private void OnDestroy() {
        KitchenGameLobby.Instance.OnLobbyListChanged -= KitchenGameLobby_OnLobbyListChange;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
