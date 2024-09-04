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
    void Start()
    {
        mainMenuBtn.onClick.AddListener(()=>{
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
