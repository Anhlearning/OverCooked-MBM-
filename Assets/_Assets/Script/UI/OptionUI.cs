using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance {get;set;}
    [SerializeField] private Button soundEffect;
    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpT;
    [SerializeField] private TextMeshProUGUI moveDownT;
    [SerializeField] private TextMeshProUGUI moveLeftT;
    [SerializeField] private TextMeshProUGUI MoveRightT;
    [SerializeField] private TextMeshProUGUI interactT;
    [SerializeField] private TextMeshProUGUI interactAltT;
    [SerializeField] private TextMeshProUGUI pauseT;
    [SerializeField] private Button moveUpBnt;
    [SerializeField] private Button moveDownBtn;
    [SerializeField] private Button moveLeftBtn;
    [SerializeField] private Button moveRightBTn;
    [SerializeField] private Button interactBTn;
    [SerializeField] private Button interactAltBTN;
    [SerializeField] private Button pauseBTN;
    [SerializeField] private Transform PressKeyUI;

    [SerializeField] private Button music;
    [SerializeField] private Button closeBtn;
    private void Awake() {
        Instance=this;
        HidePressKeyUI();
        soundEffect.onClick.AddListener(()=>{
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        music.onClick.AddListener(()=>{
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeBtn.onClick.AddListener(()=>{
            Hide();
        });
        moveUpBnt.onClick.AddListener(()=>Rebinding(PlayerInput.Binding.MoveUp));
        moveDownBtn.onClick.AddListener(()=>Rebinding(PlayerInput.Binding.MoveDown));
        moveLeftBtn.onClick.AddListener(()=>Rebinding(PlayerInput.Binding.MoveLeft));
        moveRightBTn.onClick.AddListener(()=>Rebinding(PlayerInput.Binding.MoveRight));
        interactBTn.onClick.AddListener(()=>Rebinding(PlayerInput.Binding.Interact));
        interactAltBTN.onClick.AddListener(()=>Rebinding(PlayerInput.Binding.InteractAlt));
        pauseBTN.onClick.AddListener(()=>Rebinding(PlayerInput.Binding.Pause));
    }
    private void Start()
    {   
        Hide();
        GameManager.Instance.OffPauseMenu += Option_Pause;
        UpdateVisual();
    }

    private void Option_Pause(object sender, EventArgs e)
    {
        Hide();
    }

    // Update is called once per frame
    private void UpdateVisual()
    {
        soundEffectText.text="SOUND EFFECT : "+Math.Round(SoundManager.Instance.getVolume()*10).ToString();
        musicText.text="MUCSIC : "+Math.Round(MusicManager.Instance.getVolume()*10f).ToString();

        moveUpT.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.MoveUp);
        moveDownT.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.MoveDown);
        moveLeftT.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.MoveLeft);
        MoveRightT.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.MoveRight);
        interactT.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Interact);
        interactAltT.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.InteractAlt);
        pauseT.text=PlayerInput.Instance.GetBindingText(PlayerInput.Binding.Pause);
    }

    public void Show(){
        gameObject.SetActive(true);
    }
    public void Hide(){
        gameObject.SetActive(false);
    }
    private void HidePressKeyUI(){
        PressKeyUI.gameObject.SetActive(false);
    }
    private void ShowPressKeyUI(){
        PressKeyUI.gameObject.SetActive(true);
    }
    private void Rebinding(PlayerInput.Binding binding){
        ShowPressKeyUI();
        PlayerInput.Instance.ReBindingInput(binding,()=>{
            HidePressKeyUI();
            UpdateVisual();
        });
    } 
}
