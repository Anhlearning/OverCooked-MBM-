using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingCharacterUI : MonoBehaviour
{
    [SerializeField]
    private Button readyBtn;
    private void Start()
    {
        readyBtn.onClick.AddListener(()=>{
            SelectedCharacterReady.Instance.SetPlayerReady();
        });
    }
    
}
