using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField]private Image BgImage;
    [SerializeField]private Image iconImage;
    [SerializeField]private TextMeshProUGUI textMess;
    [SerializeField] private Color colorSucces;
    [SerializeField] private Color colorfalied;
    [SerializeField] private Sprite spriteSucces;
    [SerializeField] private Sprite spriteFalied;
    private Animator animator;
    private void Awake() {
        animator=GetComponent<Animator>();

    }
    void Start()
    {
        DeliveryManager.Instance.DeliverySuccees+= Delivery_Success;
        DeliveryManager.Instance.DeliveryFalied+= Delivery_Falied;
        gameObject.SetActive(false);
    }

    private void Delivery_Falied(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Popup");
        BgImage.color=colorfalied;
        iconImage.sprite=spriteFalied;
        textMess.text="DELIVERY\nFAILED";
    }

    private void Delivery_Success(object sender, EventArgs e)
    {
         gameObject.SetActive(true);
        animator.SetTrigger("Popup");
        BgImage.color=colorSucces;
        iconImage.sprite=spriteSucces;
        textMess.text="DELIVERY\nSUCCESS";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
