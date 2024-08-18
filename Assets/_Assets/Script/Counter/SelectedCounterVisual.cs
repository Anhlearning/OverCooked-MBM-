using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter selectedCounter;
    [SerializeField] private GameObject[] visualGameObject;
    private void Start()
    {
        //Player.Instance.OnSelectedCounterChange+=Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangeEventArgs e)
    {
        if(e.selectedCounter == selectedCounter){
            Show();
        }
        else {
            Hide();
        }
    }

    private void Hide()
    {
        foreach(GameObject gameObjectVisual in visualGameObject){
            gameObjectVisual.SetActive(false);
        }
    }

    private void Show()
    {
         foreach(GameObject gameObjectVisual in visualGameObject){
            gameObjectVisual.SetActive(true);
        }
    }
}
