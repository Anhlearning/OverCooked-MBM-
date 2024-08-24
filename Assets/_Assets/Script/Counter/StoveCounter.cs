using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter,IProgressBar
{   
    
   public event EventHandler <IProgressBar.ProgressBarEvent> ProgressBar;
   public event EventHandler<OnStateChangeEvents>OnStateChange;

   public class OnStateChangeEvents : EventArgs{
        public State state;
   }
    [SerializeField] private FyringObjectSO[] fyringObjectArrays;
    [SerializeField] private BurnedKitchenSO[] burnedObjectArrays;
    public enum State {
        Idle,
        Fyring,
        Fired,
        Burned
    }
    private NetworkVariable<State> state=new NetworkVariable<State>(State.Idle);
    private FyringObjectSO fyringObjectSO;
    private BurnedKitchenSO burnedKitchenSO;
    private NetworkVariable<float> fyringTimer=new NetworkVariable<float>(0f);
    private NetworkVariable<float> burningTimer=new NetworkVariable<float>(0f);

    // private void Start() {
    //     state=State.Idle;
    // }

    public override void OnNetworkSpawn()
    {
        fyringTimer.OnValueChanged += FyringTimer_OnvalueChange;
        burningTimer.OnValueChanged += BurningTimer_OnvalueChange;
        state.OnValueChanged += State_OnvalueChange;
    }

    private void State_OnvalueChange(State previousValue, State newValue)
    {
        OnStateChange?.Invoke(this,new OnStateChangeEvents{
            state=state.Value
        });
        if(newValue == State.Idle || newValue == State.Burned){
            ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                progressNomalize = 0
            });
        }
    }
    
    private void BurningTimer_OnvalueChange(float previousValue, float newValue)
    {
        float BruningTImerMax = burnedKitchenSO !=null ? burnedKitchenSO.fyringTimerMax: 1f;
        ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
            progressNomalize=burningTimer.Value/BruningTImerMax
        });
    }

    private void FyringTimer_OnvalueChange(float previousValue, float newValue)
    {
        float fyringTimerMax = fyringObjectSO !=null ? fyringObjectSO.fyringTimerMax: 1f;
        ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
            progressNomalize=fyringTimer.Value/fyringTimerMax
        });
    }

    private void Update() {
        if(!IsServer){
            return;
        }
        if(HasIsKitchenObject()){
            switch (state.Value)
            {
                case State.Idle:
                    break; 
                case State.Fyring:
                    fyringTimer.Value+=Time.deltaTime;
                    if(fyringTimer.Value > fyringObjectSO.fyringTimerMax){
                        KitChenObject.DestroyKitchenObject(GetKitchenObject());
                        KitChenObject.SpawnKitchenObject(fyringObjectSO.output,this);
                        state.Value=State.Fired;
                        burningTimer.Value=0f;
                        SetRecipeBurningClientRpc(KitchenGameMultiplayer.Instance.GetIdxFromKitchenObjectList(GetKitchenObject().GetKitchenObjectSO()));
                    }
                    break;
                case State.Fired:
                    burningTimer.Value+=Time.deltaTime; 
                    if(burningTimer.Value > burnedKitchenSO.fyringTimerMax){
                        KitChenObject.DestroyKitchenObject(GetKitchenObject());
                        KitChenObject.SpawnKitchenObject(burnedKitchenSO.output,this);
                        state.Value=State.Burned;
                        burningTimer.Value=0f;
                    }
                    break;
                case State.Burned:
                    break;

            }
            }
        }
    public override void Interact(Player player){
        if(!HasIsKitchenObject()){
            if(player.HasIsKitchenObject() && HasKitchenFyringSO(player.GetKitchenObject().GetKitchenObjectSO())){
                KitChenObject kitChenObject=player.GetKitchenObject();
                kitChenObject.SetKitchenObjectParent(this);
                InteractServerRpc(KitchenGameMultiplayer.Instance.GetIdxFromKitchenObjectList(kitChenObject.GetKitchenObjectSO()));
            }
        }
        else {
            if(player.HasIsKitchenObject()){
                  if(player.GetKitchenObject().TryGetPlate( out PlatesKitchenObject plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        ResetIdleStateServerRpc();
                        KitChenObject.DestroyKitchenObject(GetKitchenObject());
                }
            }
            }
            else {
                ResetIdleStateServerRpc();
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void ResetIdleStateServerRpc(){
        state.Value=State.Idle;
        fyringTimer.Value=0f;
        burningTimer.Value=0f;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractServerRpc(int indexKitchenObject){
        fyringTimer.Value=0f;
        state.Value=State.Fyring;
        SetRecipeFryringClientRpc(indexKitchenObject);
    }
    [ClientRpc]
    private void SetRecipeFryringClientRpc(int indexKitchenObject){
        fyringObjectSO=GettingFyringKitchen(KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIdx(indexKitchenObject));
    }
    [ClientRpc]
     private void SetRecipeBurningClientRpc(int indexKitchenObject){
        burnedKitchenSO=GettingBurnedKitchen(KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIdx(indexKitchenObject));
    }


    private bool HasKitchenFyringSO(KitchenObjectSO  kitchenObjectSO){
        return GettingFyringKitchen(kitchenObjectSO) !=null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO){
        FyringObjectSO cuttingKitchenSO = GettingFyringKitchen(kitchenObjectSO);
        if(cuttingKitchenSO !=null){
            return cuttingKitchenSO.output;
        }
        else {
            return null;
        }
    }

    private FyringObjectSO GettingFyringKitchen(KitchenObjectSO kitchenObjectSO){
        foreach(FyringObjectSO fyringObjectSO in fyringObjectArrays){
            if(fyringObjectSO.input == kitchenObjectSO ){
                return fyringObjectSO;
            }
        }
        return null;
    }
    private BurnedKitchenSO GettingBurnedKitchen(KitchenObjectSO kitchenObjectSO){
        foreach (BurnedKitchenSO item in burnedObjectArrays)
        {
            if(item.input == kitchenObjectSO){
                return item;
            }
        }
        return null;
    }
    public bool isFried(){
        return state.Value==State.Fired;
    }
}
