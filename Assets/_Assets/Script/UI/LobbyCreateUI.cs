using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private TMP_InputField lobbyNameInput;
    
    private void Awake() {
        closeButton.onClick.AddListener(()=>{
            Hide();
        });
        createPublicButton.onClick.AddListener(()=>{
            KitchenGameLobby.Instance.CreateLobby(lobbyNameInput.text,false);
        });
        createPrivateButton.onClick.AddListener(()=>{
            KitchenGameLobby.Instance.CreateLobby(lobbyNameInput.text,true);
        });
    }
    private void Start()
    {
     Hide();   
    }
    public void Show(){
        gameObject.SetActive(true);
    }
    private void Hide(){
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}