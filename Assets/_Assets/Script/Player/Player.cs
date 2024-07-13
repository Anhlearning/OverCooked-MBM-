using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float moveSpeed=7f;
    private CharacterController characterController;

    private bool isWalking;
    private void Awake()
    {
        characterController=GetComponent<CharacterController>();
        isWalking=false;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMoveInput();
    }

    private void HandleMoveInput(){
        UnityEngine.Vector2 inputVector = playerInput.GetMovementVectorNormolized();
        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(inputVector.x,0f,inputVector.y);

        float moveDistance = moveSpeed*Time.deltaTime;
        float playerHeight=2f;
        float playerRadius=.7f;
        bool canMove=!Physics.CapsuleCast(transform.position,transform.position + UnityEngine.Vector3.up *playerHeight,playerRadius,moveDir,moveDistance);
        
        if(!canMove){
            Vector3 moveDirX=new Vector3(moveDir.x,0,0);
            canMove=!Physics.CapsuleCast(transform.position,transform.position + UnityEngine.Vector3.up *playerHeight,playerRadius,moveDirX,moveDistance);
            if(canMove){
                moveDir=moveDirX;
            }
            else {
                Vector3 moveDirZ=new Vector3(0,0,moveDir.z);
                canMove=!Physics.CapsuleCast(transform.position,transform.position + UnityEngine.Vector3.up *playerHeight,playerRadius,moveDirZ,moveDistance);
                if(canMove){
                    moveDir=moveDirZ;
                }
            }
        }
        if(canMove){
            characterController.Move(moveSpeed*moveDir*Time.deltaTime);
        }

        isWalking = moveDir != UnityEngine.Vector3.zero;

        float rotateSpeed = 7f ;

        transform.forward=UnityEngine.Vector3.Slerp(transform.forward,moveDir,Time.deltaTime*rotateSpeed);
    }

    public bool IsWalking(){
        return isWalking;
    }
}
