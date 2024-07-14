using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChange;

    public class OnSelectedCounterChangeEventArgs : EventArgs{
        public ClearCounter selectedCounter;

    }

    public static Player Instance {get;private set;}
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float moveSpeed=7f;
    [SerializeField] private LayerMask CounterLayerMask; 
    private CharacterController characterController;

    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;
    private bool isWalking;

    
    private void Awake()
    {   
        Instance = this;
        characterController=GetComponent<CharacterController>();
        playerInput.OnInteraction+= GameInput_Onteraction;
    }

    private void GameInput_Onteraction(object sender, EventArgs e)
    {
        if(selectedCounter!=null){
            selectedCounter.Interact();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMoveInput();
        HandleInteract();
    }

    private void HandleInteract(){
        Vector2 inputVector=playerInput.GetMovementVectorNormolized();
        Vector3 moveDir=new Vector3(inputVector.x,0,inputVector.y);

        if(moveDir!=Vector3.zero){
            lastInteractDir=moveDir;
        }

        float distanceInteract = 2f;

        if(Physics.Raycast(transform.position,lastInteractDir,out RaycastHit raycastHit,distanceInteract,CounterLayerMask)){
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)){
                if(clearCounter != selectedCounter){
                    SetSelectedCounter(clearCounter);
                }
            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }
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

    private void SetSelectedCounter(ClearCounter SelectedCounter){
        this.selectedCounter=SelectedCounter;
        OnSelectedCounterChange?.Invoke(this,new OnSelectedCounterChangeEventArgs {
            selectedCounter = SelectedCounter
        });
    }
}
