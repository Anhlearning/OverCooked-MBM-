using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoveUp;
    [SerializeField] private TextMeshProUGUI MoveDown;
    [SerializeField] private TextMeshProUGUI MoveLeft;
    [SerializeField] private TextMeshProUGUI MoveRight;
    [SerializeField] private TextMeshProUGUI Interact;
    [SerializeField] private TextMeshProUGUI InteractAlt;
    [SerializeField] private TextMeshProUGUI Pause;
    
    private void Start() {
        UpdateVisual();
    }
    private void Awake() {
        PlayerInput.Instance.OnChangeBinding += KeyUI_ChangeBinding;
        GameManager.Instance.OnStateChange += KeyUI_OnStateChange;        
    }

    private void KeyUI_OnStateChange(object sender, EventArgs e)
    {
        // Debug.Log(GameManager.Instance.GetState());
        if(GameManager.Instance.IsCountDownToStart()){
            Hide();
        }
    }

    private void KeyUI_ChangeBinding(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual(){
        MoveUp.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.MoveUp);
        MoveDown.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.MoveDown);
        MoveRight.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.MoveRight);
        MoveLeft.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.MoveLeft);
        Interact.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Interact);
        InteractAlt.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.InteractAlt);
        Pause.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Pause);
    }
    private void Hide(){
        gameObject.SetActive(false);
    }
    
    private void Show(){
        gameObject.SetActive(true);
    }
}
