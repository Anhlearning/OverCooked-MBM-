using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter selectedCounter;
    [SerializeField] private GameObject visualGameObject;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChange+=Player_OnSelectedCounterChange;
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
        visualGameObject.SetActive(false);
    }

    private void Show()
    {
        visualGameObject.SetActive(true);
    }
}
