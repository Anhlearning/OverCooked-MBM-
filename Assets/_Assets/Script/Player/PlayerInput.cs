using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance {get;private set;}
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteraction;
    public event EventHandler OnInteractAlternal;

    public event EventHandler OnPauseGame;
    private  void Awake() 
    {
        Instance=this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed+=Interact_performed;
        playerInputActions.Player.InteractAlternal.performed+=InteracAlternal_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;

    }
    private void OnDestroy() {
        playerInputActions.Player.Interact.performed-=Interact_performed;
        playerInputActions.Player.InteractAlternal.performed-=InteracAlternal_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }
    private void Pause_performed(InputAction.CallbackContext context){
        OnPauseGame?.Invoke(this,EventArgs.Empty);
    }
    private void InteracAlternal_performed(InputAction.CallbackContext context)
    {
        OnInteractAlternal?.Invoke(this,EventArgs.Empty);
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
