using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClockUI : MonoBehaviour
{
    [SerializeField] private Image ClockUI;
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        ClockUI.fillAmount=GameManager.Instance.GetGamePlayingTimerNomolized();
    }
}
