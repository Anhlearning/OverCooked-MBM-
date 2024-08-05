using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AudioClipRefSO",menuName ="audioClips")]
public class AudioClipRefSO : ScriptableObject
{
    public AudioClip[] Chop;
    public AudioClip[] deliveryFalied;
    public AudioClip[] deliverySuccees;
    public AudioClip[] footStep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip[] pansizzeLop;
    public AudioClip[] trash;

    public AudioClip[] warning;



}
