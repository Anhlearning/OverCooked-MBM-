using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]private Button multiplayerGameBtn;
    [SerializeField]private Button singelPlayerGameBtn;
    [SerializeField]private Button quitGame;
    
    private void Awake() {
        multiplayerGameBtn.onClick.AddListener(()=>{
            KitchenGameMultiplayer.playMultiplayer=true;
            Loader.Load(Loader.Scene.Lobby);
        });
        singelPlayerGameBtn.onClick.AddListener(()=>{
            KitchenGameMultiplayer.playMultiplayer=false;
            Loader.Load(Loader.Scene.Lobby);
        });
        quitGame.onClick.AddListener(()=>{
            Application.Quit();
        });
        Time.timeScale=1f;
    }

    
}
