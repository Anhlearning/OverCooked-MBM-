using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectedUI : MonoBehaviour
{
    [SerializeField] private Button GameMenuBtn;
    [SerializeField] private Button ReadyBtn;
    private void Start()
    {
        GameMenuBtn.onClick.AddListener(()=>{
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.GameMenu);
        });
        ReadyBtn.onClick.AddListener(()=>{
            SelectedCharacterReady.Instance.SetPlayerReady();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
