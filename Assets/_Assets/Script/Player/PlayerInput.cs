using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private const string Player_Input_Rebinding="InputBindings";
    public static PlayerInput Instance {get;private set;}
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteraction;
    public event EventHandler OnInteractAlternal;
    public event EventHandler OnChangeBinding;
    public enum Binding{
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlt,
        Pause
    }
    public event EventHandler OnPauseGame;
    private  void Awake() 
    {
        Instance=this;
        playerInputActions = new PlayerInputActions();
        if(PlayerPrefs.HasKey(Player_Input_Rebinding)){
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(Player_Input_Rebinding));
        }
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
    public string GetBindingText(Binding binding){
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt:
                return playerInputActions.Player.InteractAlternal.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();

        }
    }
    public void ReBindingInput(Binding binding,Action OnRebounding){
        playerInputActions.Disable();
        InputAction playerInputNews;
        int bindingIndx;
        switch (binding)
        {
            default:
            case Binding.MoveUp:
            playerInputNews=playerInputActions.Player.Move;
            bindingIndx=1;
            break;
            case Binding.MoveDown:
            playerInputNews=playerInputActions.Player.Move;
            bindingIndx=2;
            break;
            case Binding.MoveLeft:
            playerInputNews=playerInputActions.Player.Move;
            bindingIndx=3;
            break;
            case Binding.MoveRight:
            playerInputNews=playerInputActions.Player.Move;
            bindingIndx=4;
            break;
            case Binding.Interact:
            playerInputNews=playerInputActions.Player.Interact;
            bindingIndx=0;
            break;
            case Binding.InteractAlt:
            playerInputNews=playerInputActions.Player.InteractAlternal;
            bindingIndx=0;
            break;
            case Binding.Pause:
            playerInputNews=playerInputActions.Player.Pause;
            bindingIndx=0;
            break;
        }
        playerInputNews.PerformInteractiveRebinding(bindingIndx).OnComplete(callback => {
            callback.Dispose();
            playerInputActions.Player.Enable();
            OnChangeBinding?.Invoke(this,EventArgs.Empty);
            PlayerPrefs.SetString(Player_Input_Rebinding,playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            OnRebounding();
        })
        .Start();
    }
    public UnityEngine.Vector2 GetMovementVectorNormolized(){
        UnityEngine.Vector2 inputVector = playerInputActions.Player.Move.ReadValue<UnityEngine.Vector2>();
        inputVector = inputVector.normalized;
        
        return inputVector;
    }
}
