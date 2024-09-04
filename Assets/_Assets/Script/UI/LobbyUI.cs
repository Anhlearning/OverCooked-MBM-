using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
