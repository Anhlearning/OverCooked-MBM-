using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float moveSpeed=7f;
    private CharacterController characterController;

    private bool isWalking;
    private void Start()
    {
        characterController=GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMoveInput();
    }

    private void HandleMoveInput(){
        UnityEngine.Vector2 inputVector = playerInput.GetMovementVectorNormolized();
        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(inputVector.x,0f,inputVector.y);

        characterController.Move(moveSpeed*moveDir*Time.deltaTime);

        isWalking= moveDir != UnityEngine.Vector3.zero;

        float rotateSpeed = 7f ;

        transform.forward=UnityEngine.Vector3.Slerp(transform.forward,moveDir,Time.deltaTime*rotateSpeed);
    }

    public bool IsWalking(){
        return isWalking;
    }
}
