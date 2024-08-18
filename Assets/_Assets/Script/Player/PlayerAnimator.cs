using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class PlayerAnimator : NetworkBehaviour 
{
    [SerializeField] private Player player;
    private Animator animator;
    private const String IS_WALKING="IsWalking";

    private void Awake()
    {
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!IsOwner) return ;
        animator.SetBool(IS_WALKING,player.IsWalking());
    }
}
