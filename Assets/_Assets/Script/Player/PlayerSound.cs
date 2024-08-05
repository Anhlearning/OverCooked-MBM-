using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float timeStepmax=1f;
    private float timeStep=1f;
    private void Awake()
    {
        player=GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        timeStep-=Time.deltaTime;
        if(timeStep<=0){
            timeStep=timeStepmax;
            if(player.IsWalking()){
                SoundManager.Instance.PlayPlayerSound(player.transform.position);
            }
        }
    }
}
