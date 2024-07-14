using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteraction;

    private  void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed+=Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext context)
    {
        OnInteraction?.Invoke(this,EventArgs.Empty);
    }

    public UnityEngine.Vector2 GetMovementVectorNormolized(){
        UnityEngine.Vector2 inputVector = playerInputActions.Player.Move.ReadValue<UnityEngine.Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
