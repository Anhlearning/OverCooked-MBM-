using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
using Unity.Netcode;
using System.Globalization;
using Unity.VisualScripting;
public class Player : NetworkBehaviour,IKitchenObjectParent
{
    // Start is called before the first frame update
     public static void resetEventStatic(){
        OnAnySpawnPlayer=null;
    }
    public static event EventHandler OnAnySpawnPlayer;
    public static event EventHandler OnAnyPlayerPickup;
    public event EventHandler PickUpObject;
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChange;

    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    public static Player InstanceLocal {get;private set;}
    [SerializeField] private float moveSpeed=7f;
    [SerializeField] private LayerMask CounterLayerMask; 
    [SerializeField] private Transform PlayerTopPoint; 
    [SerializeField] private List<Vector3> SpawnPlayerPos;
    [SerializeField]private PlayerVisual playerVisual;
    private CharacterController characterController;

    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private bool isWalking;
    private KitChenObject kitchenObject;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>(); 

    }
    public override void OnNetworkSpawn(){
        if(IsOwner){
            InstanceLocal=this;
        }
        OnAnySpawnPlayer?.Invoke(this,EventArgs.Empty);
        transform.position=SpawnPlayerPos[(int)KitchenGameMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];
        if(IsServer){
            NetworkManager.Singleton.OnClientDisconnectCallback += Player_OnDisconnect;
        }
    }

    private void Player_OnDisconnect(ulong IdClient)
    {
        if(IdClient == OwnerClientId && HasIsKitchenObject()){
            KitChenObject.DestroyKitchenObject(GetKitchenObject());
        }
    }

    private void Start()
    {
        // Debug.Log(OwnerClientId + " "+ NetworkManager.ServerClientId);
        PlayerInput.Instance.OnInteraction+= GameInput_Onteraction;
        PlayerInput.Instance.OnInteractAlternal+=GameInput_OnInteracAlternal;
        PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
    }
    private void GameInput_OnInteracAlternal(object sender, EventArgs e)
    {   
        if(!GameManager.Instance.IsGamePlaying()) return;
        if(selectedCounter !=null){
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_Onteraction(object sender, EventArgs e)
    {
        if(!GameManager.Instance.IsGamePlaying()) return;
        if(selectedCounter!=null){
            selectedCounter.Interact(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleMoveInput();
        HandleInteract();
    }
   

   
    private void HandleInteract(){
        Vector2 inputVector=PlayerInput.Instance.GetMovementVectorNormolized();
        Vector3 moveDir=new Vector3(inputVector.x,0,inputVector.y);

        if(moveDir!=Vector3.zero){
            lastInteractDir=moveDir;
        }

        float distanceInteract = 2f;

        if(Physics.Raycast(transform.position,lastInteractDir,out RaycastHit raycastHit,distanceInteract,CounterLayerMask)){
            if(raycastHit.transform.TryGetComponent(out BaseCounter clearCounter)){
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
        UnityEngine.Vector2 inputVector = PlayerInput.Instance.GetMovementVectorNormolized();
        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(inputVector.x,0f,inputVector.y);
        float moveDistance = moveSpeed*Time.deltaTime;
        float playerHeight=2f;
        float playerRadius=.5f;

        bool canMove= !Physics.CapsuleCast(transform.position,transform.position + UnityEngine.Vector3.up *playerHeight,playerRadius,moveDir,moveDistance);
        
        if(!canMove){
            Vector3 moveDirX=new Vector3(moveDir.x,0,0);
            canMove=moveDir.x!=0 && !Physics.CapsuleCast(transform.position,transform.position + UnityEngine.Vector3.up *playerHeight,playerRadius,moveDirX,moveDistance);
            if(canMove){
                moveDir=moveDirX;
            }
            else {
                Vector3 moveDirZ=new Vector3(0,0,moveDir.z);
                canMove=moveDir.z!=0! && Physics.CapsuleCast(transform.position,transform.position + UnityEngine.Vector3.up *playerHeight,playerRadius,moveDirZ,moveDistance);
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

    private void SetSelectedCounter(BaseCounter SelectedCounter){
        this.selectedCounter=SelectedCounter;
        OnSelectedCounterChange?.Invoke(this,new OnSelectedCounterChangeEventArgs {
            selectedCounter = SelectedCounter
        });
    }


    public void SetKitchenObject(KitChenObject kitChenObject){
        this.kitchenObject=kitChenObject;
        if(kitchenObject !=null){
            PickUpObject?.Invoke(this,EventArgs.Empty);
            OnAnyPlayerPickup?.Invoke(this,EventArgs.Empty);
        }
    }
    public KitChenObject GetKitchenObject(){
        return kitchenObject;
    }
    public bool HasIsKitchenObject(){
        return kitchenObject !=null;
    }
    public void ClearKitchenObject(){
        this.kitchenObject=null;
    }
    public Transform GetKitchenObjectTransform(){
        return PlayerTopPoint;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
    public ulong GetClientId(){
        return OwnerClientId;
    }
}
