using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StoveCounter : BaseCounter,IProgressBar
{   
    
   public event EventHandler <IProgressBar.ProgressBarEvent> ProgressBar;
   public event EventHandler<OnStateChangeEvents>OnFryring;

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
    private State state;
    private FyringObjectSO fyringObjectSO;
    private BurnedKitchenSO burnedKitchenSO;
    private float fyringTimer;
    private float BurningTimer;
    private void Start() {
        state=State.Idle;
    }
    private void Update() {
        if(HasIsKitchenObject()){
            switch (state)
            {
                case State.Idle:
                    break; 
                case State.Fyring:
                    fyringTimer+=Time.deltaTime;
                    ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                    progressNomalize=(float)fyringTimer/fyringObjectSO.fyringTimerMax
                    });
                    if(fyringTimer>= fyringObjectSO.fyringTimerMax){
                        GetKitchenObject().DestroySelf();
                        KitChenObject.SpawnKitchenObject(fyringObjectSO.output,this);
                        burnedKitchenSO=GettingBurnedKitchen(fyringObjectSO.output);
                        state=State.Fired;
                        fyringTimer=0f;
                    }
                    OnFryring?.Invoke(this,new OnStateChangeEvents{
                    state=state
                });
                    break;
                case State.Fired:
                    BurningTimer+=Time.deltaTime;
                    ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                        progressNomalize=(float)BurningTimer/burnedKitchenSO.fyringTimerMax
                    });
                    if(BurningTimer >= burnedKitchenSO.fyringTimerMax){
                        GetKitchenObject().DestroySelf();
                        KitChenObject.SpawnKitchenObject(burnedKitchenSO.output,this);
                        state=State.Burned;
                        BurningTimer=0f;
                        ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                         progressNomalize=0f
                        });
                    }
                    OnFryring?.Invoke(this,new OnStateChangeEvents{
                    state=state
                    });
                    break;
                case State.Burned:
                    break;

            }
            }
        }
    public override void Interact(Player player){
        if(!HasIsKitchenObject()){
            if(player.HasIsKitchenObject() && HasKitchenFyringSO(player.GetKitchenObject().GetKitchenObjectSO())){
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fyringObjectSO=GettingFyringKitchen(GetKitchenObject().GetKitchenObjectSO());
                state=State.Fyring;
                ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                    progressNomalize=(float)fyringTimer/fyringObjectSO.fyringTimerMax
                });
                OnFryring?.Invoke(this,new OnStateChangeEvents{
                    state=state
                });
            }
        }
        else {
            if(player.HasIsKitchenObject()){
                  if(player.GetKitchenObject().TryGetPlate( out PlatesKitchenObject plateKitchenObject)){
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                        state=State.Idle;
                        ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                            progressNomalize=0f
                        });
                        OnFryring?.Invoke(this,new OnStateChangeEvents{
                            state=state
                        });
                    }
                }
            }
            else {
                state=State.Idle;
                ProgressBar?.Invoke(this,new IProgressBar.ProgressBarEvent{
                    progressNomalize=0f
                });
                 OnFryring?.Invoke(this,new OnStateChangeEvents{
                    state=state
                });
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
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
}
