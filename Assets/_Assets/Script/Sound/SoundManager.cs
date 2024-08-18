using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME="soundEffectVolume";
    public static SoundManager Instance{get;set;}
    [SerializeField] private AudioClipRefSO audioClipRefSO;
    
    private float volume =1f;
    private void Awake() {
        Instance=this;
        volume=PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME,1f);
    }
    private void Start() {
        //Player.Instance.PickUpObject+=Player_PickUpObject;
        DeliveryManager.Instance.DeliveryFalied+=DeliveryManager_DeliveryFalied;
        DeliveryManager.Instance.DeliverySuccees+=DeliveryManager_DeliverySuccees;
        CuttingCounter.AnyOnCut+=CuttingCounter_AnyOnCut;
        BaseCounter.OnAnyObjectPlaceHere+=BaseCounter_OnAnyPlaceST;
        TrashCounter.OnAnyObjectTrashed+=TrashCounter_OnTrashST;
    }

    private void TrashCounter_OnTrashST(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter=sender as TrashCounter;
        PlaySound(audioClipRefSO.trash,trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyPlaceST(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter=sender as BaseCounter;
        PlaySound(audioClipRefSO.objectDrop,baseCounter.transform.position);
    }

    private void Player_PickUpObject(object sender, System.EventArgs e)
    {
       // PlaySound(audioClipRefSO.objectPickup,Player.Instance.transform.position);
    }

    private void CuttingCounter_AnyOnCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefSO.Chop,cuttingCounter.transform.position);
    }

    private void DeliveryManager_DeliverySuccees(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefSO.deliverySuccees,DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_DeliveryFalied(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefSO.deliveryFalied,DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audioClips,Vector3 position){
        PlaySound(audioClips[UnityEngine.Random.Range(0,audioClips.Length)],position);
    }
   private void PlaySound(AudioClip audioClip,Vector3 position,float volumeMultiple=1){
        AudioSource.PlayClipAtPoint(audioClip,position,volume*volumeMultiple);
   }
   public void PlayPlayerSound(Vector3 position){
        PlaySound(audioClipRefSO.footStep,position);
   }
   public void PlayCountDownSound(){
        PlaySound(audioClipRefSO.warning,Vector3.zero);
   }
   public void ChangeVolume(){
        volume+=.1f;
        if(volume >=1f ){
            volume=0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME,volume);
        PlayerPrefs.Save();
   }
   public void PlayWarningSound(Vector3 position){
        PlaySound(audioClipRefSO.warning,position);
   }
   public float  getVolume(){
     return volume;
   }
}
