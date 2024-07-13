using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    private  void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

    }

    public UnityEngine.Vector2 GetMovementVectorNormolized(){
        UnityEngine.Vector2 inputVector = playerInputActions.Player.Move.ReadValue<UnityEngine.Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
