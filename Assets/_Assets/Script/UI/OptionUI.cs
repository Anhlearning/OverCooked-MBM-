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
    [SerializeField] private Button music;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Button closeBtn;
    private void Awake() {
        Instance=this;
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
    }

    public void Show(){
        gameObject.SetActive(true);
    }
    public void Hide(){
        gameObject.SetActive(false);
    }
}
