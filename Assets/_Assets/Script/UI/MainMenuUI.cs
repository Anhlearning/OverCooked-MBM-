using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]private Button playGame;
    [SerializeField]private Button quitGame;
    
    private void Awake() {
        playGame.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.Lobby);
        });
        quitGame.onClick.AddListener(()=>{
            Application.Quit();
        });
        Time.timeScale=1f;
    }

    
}
