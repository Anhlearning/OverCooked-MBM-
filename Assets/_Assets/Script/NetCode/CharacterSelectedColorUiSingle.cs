using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectedColorUiSingle : MonoBehaviour
{
    [SerializeField] private int colorId;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selected;
    

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(()=>{
            KitchenGameMultiplayer.Instance.ChangePlayerColor(colorId);
        });
    }

    private void KitchenGameMultiplayer_OnListPlayerdataChange(object sender, EventArgs e)
    {
        UpdateSelected();
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkChange += KitchenGameMultiplayer_OnListPlayerdataChange;
        image.color=KitchenGameMultiplayer.Instance.GetPlayerColor(colorId);
        UpdateSelected();
    }

    private void UpdateSelected(){
        if(KitchenGameMultiplayer.Instance.GetPlayerData().colorId==colorId ){
           selected.SetActive(true); 
        }
        else {
            selected.SetActive(false);
        }
    }
    private void OnDestroy() {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkChange -= KitchenGameMultiplayer_OnListPlayerdataChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
